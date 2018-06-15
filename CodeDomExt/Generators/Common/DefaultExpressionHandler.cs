using System.CodeDom;
using CodeDomExt.Nodes;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Common
{
    /// <summary>
    /// An expression handler providing some implementation for the simpler expressions.
    /// If an implementation of this shouldn't handle some of the statements it should return null or an empty string in
    /// methods/property returning string, or false in methods returning bool.
    /// </summary>
    /// <remarks>
    /// An expression handler should handle the attributes without indenting, nor finishing with a new line or whitespace.
    /// </remarks>
    public abstract class DefaultExpressionHandler : DynamicDispatchHandler<CodeExpression>
    {
        private readonly bool _handleSnippet;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="handleSnippet">true if snippet expression should be handled by this</param>
        protected DefaultExpressionHandler(bool handleSnippet)
        {
            _handleSnippet = handleSnippet;
        }
        
        /// <inheritdoc />
        protected override bool DoDynamicHandle(CodeExpression obj, Context ctx)
        {
           return HandleDynamic(obj as dynamic, ctx);
        }

        /// <summary>
        /// Returns the provided string as a valid identifier for current language
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected abstract string AsIdentifier(string s);
        
        private bool HandleDynamic(CodeArgumentReferenceExpression obj, Context ctx)
        {
            ctx.Writer.Write(AsIdentifier(obj.ParameterName));
            return true;
        }

        private bool HandleDynamic(CodeBinaryOperatorExpression obj, Context ctx)
        {
            return ctx.HandlerProvider.ExpressionHandler.Handle(new CodeBinaryOperatorExpressionMore(
                obj.Left,
                obj.Operator.AsBinaryOperatorTypeMore(),
                obj.Right), ctx);
        }

        /// <summary>
        /// Return the keyword representing the specified field direction in the current language or null
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        protected abstract string GetDirectionKeyword(FieldDirection direction);
        private bool HandleDynamic(CodeDirectionExpression obj, Context ctx)
        {
            if (string.IsNullOrEmpty(GetDirectionKeyword(obj.Direction)))
            {
                return false;
            }
            ctx.Writer.Write($"{GetDirectionKeyword(obj.Direction)} ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Expression, ctx);
            return true;
        }

        /// <summary>
        /// Return the string representing the memberAccessing operator in the current language or null
        /// </summary>
        protected abstract string MemberAccessOperator { get; }
        private bool HandleDynamic(CodeEventReferenceExpression obj, Context ctx)
        {
            if (string.IsNullOrEmpty(MemberAccessOperator))
            {
                return false;
            }
            if (obj.TargetObject != null)
            {
                WrapAccessorTargetIfNecessaryAndHandle(obj.TargetObject, ctx);
                ctx.Writer.Write(MemberAccessOperator);
            }

            ctx.Writer.Write(AsIdentifier(obj.EventName));
            return true;
        }

        private bool HandleDynamic(CodeFieldReferenceExpression obj, Context ctx)
        {
            if (string.IsNullOrEmpty(MemberAccessOperator))
            {
                return false;
            }
            if (obj.TargetObject != null)
            {
                WrapAccessorTargetIfNecessaryAndHandle(obj.TargetObject, ctx);
                ctx.Writer.Write(MemberAccessOperator);
            }

            ctx.Writer.Write(AsIdentifier(obj.FieldName));
            return true;
        }

        private bool HandleDynamic(CodeIndexerExpression obj, Context ctx)
        {
            CodeArrayIndexerExpression equivalent = new CodeArrayIndexerExpression(obj.TargetObject);
            equivalent.Indices.AddRange(obj.Indices);
            return ctx.HandlerProvider.ExpressionHandler.Handle(equivalent, ctx);
        }

        private bool HandleDynamic(CodePropertyReferenceExpression obj, Context ctx)
        {
            if (string.IsNullOrEmpty(MemberAccessOperator))
            {
                return false;
            }
            if (obj.TargetObject != null)
            {
                WrapAccessorTargetIfNecessaryAndHandle(obj.TargetObject, ctx);
                ctx.Writer.Write(MemberAccessOperator);
            }

            ctx.Writer.Write(AsIdentifier(obj.PropertyName));
            return true;
        }

        /// <summary>
        /// Return the string representing the reference to a the set value in a property in the current language or null
        /// </summary>
        protected abstract string PropertySetValueReferenceKeyword { get; }
        private bool HandleDynamic(CodePropertySetValueReferenceExpression obj, Context ctx)
        {
            if (string.IsNullOrEmpty(PropertySetValueReferenceKeyword))
            {
                return false;
            }
            ctx.Writer.Write(PropertySetValueReferenceKeyword);
            return true;
        }

        /// <summary>
        /// Return the string representing the reference to this in the current language or null
        /// </summary>
        protected abstract string ThisReferenceKeyword { get; }
        private bool HandleDynamic(CodeThisReferenceExpression obj, Context ctx)
        {
            if (string.IsNullOrEmpty(ThisReferenceKeyword))
            {
                return false;
            }
            ctx.Writer.Write(ThisReferenceKeyword);
            return true;
        }

        /// <summary>
        /// Return the string representing the reference to the base class in the current language or null
        /// </summary>
        protected abstract string BaseReferenceKeyword { get; }
        private bool HandleDynamic(CodeBaseReferenceExpression obj, Context ctx)
        {
            if (string.IsNullOrEmpty(BaseReferenceKeyword))
            {
                return false;
            }
            ctx.Writer.Write(BaseReferenceKeyword);
            return true;
        }

        private bool HandleDynamic(CodeTypeReferenceExpression obj, Context ctx)
        {
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.Type, ctx);
            return true;
        }

        private bool HandleDynamic(CodeVariableReferenceExpression obj, Context ctx)
        {
            ctx.Writer.Write(AsIdentifier(obj.VariableName));
            return true;
        }

        private bool HandleDynamic(CodeSnippetExpression obj, Context ctx)
        {
            if (!_handleSnippet) return false;
            GeneralUtils.HandleSnippet(obj.Value, ctx);
            return true;
        }

        /// <summary>
        /// Handle an expression that is target of a member access, and if necessary for operation priority wrap it in parentheses
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ctx"></param>
        protected abstract void WrapAccessorTargetIfNecessaryAndHandle(CodeExpression obj, Context ctx);
        
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodeArrayInitializerExpression obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodePrimitiveExpression obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodeUnaryOperatorExpression obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodeConditionalOperatorExpression obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodeLambdaDeclarationExpression obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodeLambdaParameterDeclarationExpression obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodeBinaryOperatorExpressionMore obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodeMultidimensionalArrayCreateExpression obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodePropertyInitializerExpression obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodeDelegateCreateExpression obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodeDelegateInvokeExpression obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodeObjectCreateExpression obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodeParameterDeclarationExpression obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodeArrayCreateExpression obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodeTypeOfExpression obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodeMethodInvokeExpression obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodeArrayIndexerExpression obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodeCastExpression obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodeMethodReferenceExpression obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDynamic(CodeDefaultValueExpression obj, Context ctx);
    }
}