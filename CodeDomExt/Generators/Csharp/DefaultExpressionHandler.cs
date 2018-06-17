using System;
using System.CodeDom;
using System.Linq;
using CodeDomExt.Generators.Common;
using CodeDomExt.Nodes;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Csharp
{
    /// <inheritdoc/>
    public class DefaultExpressionHandler : Common.DefaultExpressionHandler
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DefaultExpressionHandler() : base(true)
        {
        }
        
        /// <inheritdoc/>
        protected override string PropertySetValueReferenceKeyword { get; } = "value";
        /// <inheritdoc/>
        protected override string ThisReferenceKeyword { get; } = "this";
        /// <inheritdoc/>
        protected override string BaseReferenceKeyword { get; } = "base";
        /// <inheritdoc/>
        protected override string MemberAccessOperator { get; } = ".";
        /// <inheritdoc/>
        protected override string AsIdentifier(string s)
        {
            return s.AsCsId();
        }
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeArrayCreateExpression obj, Context ctx)
        {   
            ctx.Writer.Write("new ");
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.CreateType, ctx);
            ctx.Writer.Write("[");
            if (obj.Initializers.Count > 0)
            {
                ctx.Writer.Write("] {");
                GeneralUtils.HandleCollectionCommaSeparated(obj.Initializers.Cast<CodeExpression>(),
                    ctx.HandlerProvider.ExpressionHandler, ctx);
                ctx.Writer.Write("}");
            }
            else
            {
                if (obj.SizeExpression != null)
                {
                    ctx.HandlerProvider.ExpressionHandler.Handle(obj.SizeExpression, ctx);
                }
                else
                {
                    ctx.Writer.Write(obj.Size.ToString());
                }
                ctx.Writer.Write("]");
            }

            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeTypeOfExpression obj, Context ctx)
        {
            ctx.Writer.Write("typeof(");
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.Type,ctx);
            ctx.Writer.Write(")");
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeMethodInvokeExpression obj, Context ctx)
        {
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Method, ctx);
            ctx.Writer.Write("(");
            GeneralUtils.HandleCollectionCommaSeparated(obj.Parameters.Cast<CodeExpression>(), ctx.HandlerProvider.ExpressionHandler, ctx);
            ctx.Writer.Write(")");
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeArrayIndexerExpression obj, Context ctx)
        {
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.TargetObject, ctx);
            ctx.Writer.Write("[");
            GeneralUtils.HandleCollectionCommaSeparated(obj.Indices.Cast<CodeExpression>(), ctx.HandlerProvider.ExpressionHandler, ctx);
            ctx.Writer.Write("]");
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeCastExpression obj, Context ctx)
        {
            ctx.Writer.Write("(");
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.TargetType, ctx);
            ctx.Writer.Write(") ");
            WrapIfNecessaryAndHandle(obj.Expression, ctx);
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeMethodReferenceExpression obj, Context ctx)
        {
            if (obj.TargetObject != null)
            {
                WrapIfNecessaryAndHandle(obj.TargetObject, ctx);
                ctx.Writer.Write(".");
            }
            ctx.Writer.Write(obj.MethodName.AsCsId());
            if (obj.TypeArguments.Count > 0)
            {
                ctx.Writer.Write("<");
                GeneralUtils.HandleCollectionCommaSeparated(obj.TypeArguments.Cast<CodeTypeReference>(),
                    ctx.HandlerProvider.TypeReferenceHandler, ctx);
                ctx.Writer.Write(">");
            }

            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeDefaultValueExpression obj, Context ctx)
        {
            ctx.Writer.Write("default(");
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.Type, ctx);
            ctx.Writer.Write(")");
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeDelegateCreateExpression obj, Context ctx)
        {
            ctx.Writer.Write("new ");
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.DelegateType, ctx);
            ctx.Writer.Write("(");
            if (obj.TargetObject != null)
            {
                WrapIfNecessaryAndHandle(obj.TargetObject, ctx);
                ctx.Writer.Write(".");
            }
            ctx.Writer.Write(obj.MethodName.AsCsId());
            ctx.Writer.Write(")");
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeDelegateInvokeExpression obj, Context ctx)
        {
            WrapIfNecessaryAndHandle(obj.TargetObject, ctx);
            ctx.Writer.Write("(");
            GeneralUtils.HandleCollectionCommaSeparated(obj.Parameters.Cast<CodeExpression>(),
                ctx.HandlerProvider.ExpressionHandler, ctx);
            ctx.Writer.Write(")");
            return true;
        }
        
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeDirectionExpression obj, Context ctx)
        {
            if (obj.Direction != FieldDirection.In) {
                ctx.Writer.Write($"{CSharpKeywordsUtils.DirectionKeyword(obj.Direction)} ");
            }
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Expression, ctx);
            return true;
        }
        
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeObjectCreateExpression obj, Context ctx)
        {
            ctx.Writer.Write("new ");
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.CreateType, ctx);
            ctx.Writer.Write("(");
            GeneralUtils.HandleCollectionCommaSeparated(obj.Parameters.Cast<CodeExpression>(),
                ctx.HandlerProvider.ExpressionHandler, ctx);
            ctx.Writer.Write(")");
            if (obj is CodeObjectCreateExpressionExt objExt && objExt.PropertyInitializers.Any())
            {
                ctx.Writer.NewLine();
                ctx.Writer.IndentAndWriteLine("{", ctx);
                ctx.Indent();
                GeneralUtils.HandleCollection(objExt.PropertyInitializers, ctx.HandlerProvider.ExpressionHandler, ctx,
                    preAction: (c) => c.Writer.Indent(c),
                    postAction:(c) => c.Writer.WriteLine(","), doPostActionOnLast: false);
                ctx.Writer.NewLine();
                ctx.Unindent();
                ctx.Writer.IndentAndWrite("}", ctx);
            }
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodePropertyInitializerExpression obj, Context ctx)
        {
            ctx.Writer.Write($"{obj.PropertyName.AsCsId()} = ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.InitializerExpression, ctx);
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeParameterDeclarationExpression obj, Context ctx)
        {
            if (obj.CustomAttributes.Count > 0)
            {
                GeneralUtils.HandleCollection(obj.CustomAttributes.Cast<CodeAttributeDeclaration>(),
                    ctx.HandlerProvider.AttributeDeclarationHandler, ctx);
                ctx.Writer.Write(" ");
            }

            if (obj is CodeParameterDeclarationExpressionExt objExt && objExt.IsVarargs)
            {
                ctx.Writer.Write("params ");
            }
            
            if (obj.Direction != FieldDirection.In)
            {
                //TODO c# 7 allows "in" with a different meaning from parameters without modifier (readonly reference)
                ctx.Writer.Write($"{CSharpKeywordsUtils.DirectionKeyword(obj.Direction)} ");
            }
            
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.Type, ctx);
            ctx.Writer.Write($" {obj.Name.AsCsId()}");
            if (obj is CodeParameterDeclarationExpressionExt objExt2 && objExt2.DefaultValue != null)
            {
                ctx.Writer.Write(" = ");
                ctx.HandlerProvider.ExpressionHandler.Handle(objExt2.DefaultValue, ctx);
            }
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeUnaryOperatorExpression obj, Context ctx)
        {
            string operatorSymbol = CSharpKeywordsUtils.UnaryOperatorSymbol(obj.Operator, out var isOperatorAfterExpression);
            if (!isOperatorAfterExpression)
            {
                ctx.Writer.Write(operatorSymbol);
            }
            WrapIfNecessaryAndHandle(obj.Expression, ctx);
            if (isOperatorAfterExpression)
            {
                ctx.Writer.Write(operatorSymbol);
            }
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeConditionalOperatorExpression obj, Context ctx)
        {
            WrapIfNecessaryAndHandle(obj.TestExpression, ctx);
            ctx.Writer.Write(" ? ");
            WrapIfNecessaryAndHandle(obj.TrueExpression, ctx);
            ctx.Writer.Write(" : ");
            WrapIfNecessaryAndHandle(obj.FalseExpression, ctx);
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeLambdaDeclarationExpression obj, Context ctx)
        {
            ctx.Writer.Write("(");
            GeneralUtils.HandleCollectionCommaSeparated(obj.Parameters, ctx.HandlerProvider.ExpressionHandler, ctx);
            ctx.Writer.Write(") =>");
            if (obj.Statements.Count == 1 && obj.Statements[0] is CodeExpressionStatement statement)
            {
                ctx.Writer.Write(" ");
                ctx.HandlerProvider.ExpressionHandler.Handle(statement.Expression, ctx);
            }
            else
            {
                ctx.Writer.NewLine();
                ctx.Writer.IndentAndWriteLine("{", ctx);
                ctx.Indent();
                CSharpUtils.HandleStatementCollection(obj.Statements, ctx, false);
                ctx.Unindent();
                ctx.Writer.IndentAndWrite("}", ctx);
            }
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeLambdaParameterDeclarationExpression obj, Context ctx)
        {
            if (obj.Type != null)
            {
                ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.Type, ctx);
                ctx.Writer.Write(" ");
            }
            ctx.Writer.Write(obj.Name.AsCsId());
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeBinaryOperatorExpressionMore obj, Context ctx)
        {
            WrapIfNecessaryAndHandle(obj.LeftExpression, ctx);
            ctx.Writer.Write($" {CSharpKeywordsUtils.OperatorSymbol(obj.OperatorType)} ");
            WrapIfNecessaryAndHandle(obj.RightExpression, ctx);
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeMultidimensionalArrayCreateExpression obj, Context ctx)
        {
            ctx.Writer.Write("new ");
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.CreateType, ctx);
            ctx.Writer.Write("[");
            if (obj.SizeExpressions.Count > 0)
            {
                GeneralUtils.HandleCollectionCommaSeparated(obj.SizeExpressions, ctx.HandlerProvider.ExpressionHandler, ctx);
            }
            else if (obj.Rank > 0)
            {
                for (int i = 1; i < obj.Rank; i++)
                {
                    ctx.Writer.Write(",");
                }
            }
            ctx.Writer.Write("]");
            if (obj.InitializerExpression != null)
            {
                ctx.Writer.Write(" ");
                ctx.HandlerProvider.ExpressionHandler.Handle(obj.InitializerExpression, ctx);
            }
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodeArrayInitializerExpression obj, Context ctx)
        {
            ctx.Writer.Write("{");
            GeneralUtils.HandleCollectionCommaSeparated(obj.Expressions, ctx.HandlerProvider.ExpressionHandler, ctx);
            ctx.Writer.Write("}");
            return true;
        }
        
        /// <inheritdoc/>
        protected override bool HandleDynamic(CodePrimitiveExpression obj, Context ctx)
        {
            return _primitiveHandler.Handle(obj, ctx);
        }
        private readonly PrimitiveHandler _primitiveHandler = new PrimitiveHandler();
        private class PrimitiveHandler : PrimitiveExpressionHandler
        {
            protected override Type DefaultIntegerType { get; } = typeof(int);
            protected override Type DefaultFloatingPointType { get; } = typeof(double);

            public PrimitiveHandler() : base(true)
            {
            }
            
            protected override string GetForcedLiteral(Type type)
            {
                if (type == typeof(float))
                {
                    return "f";
                }
                else if (type == typeof(decimal))
                {
                    return "m";
                }
                else if (type == typeof(uint))
                {
                    return "U";
                }
                else if (type == typeof(long))
                {
                    return "L";
                }
                else if (type == typeof(ulong))
                {
                    return "UL";
                }
                else
                {
                    return null;
                }
            }
            
            protected override void HandleNull(Context ctx)
            {
                ctx.Writer.Write("null");
            }

            protected override void HandleChar(char c, Context ctx)
            {
                ctx.Writer.Write("\'" + c + "\'");
            }

            protected override void HandleString(string str, Context ctx)
            {
                ctx.Writer.Write("\"" + GeneralUtils.EscapeString(str, '\\', '"', '\\') + "\"");
            }

            protected override void HandleBoolean(bool b, Context ctx)
            {
                ctx.Writer.Write(b ? "true" : "false");
            }
        }
        /// <inheritdoc/>
        protected override void WrapAccessorTargetIfNecessaryAndHandle(CodeExpression obj, Context ctx)
        {
            WrapIfNecessaryAndHandle(obj, ctx);
        }

        private void WrapIfNecessaryAndHandle(CodeExpression obj, Context ctx)
        {
            GeneralUtils.WrapIfIsTypeAndHandle(obj, ctx, 
                typeof(CodeBinaryOperatorExpression),
                typeof(CodeBinaryOperatorExpressionMore),
                typeof(CodeConditionalOperatorExpression),
                typeof(CodeCastExpression),
                typeof(CodeLambdaDeclarationExpression));
        }
    }
}