using System;
using System.CodeDom;
using System.Diagnostics;
using System.Linq;
using CodeDomExt.Generators.Common;
using CodeDomExt.Helpers;
using CodeDomExt.Nodes;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.VisualBasic
{
    /// <inheritdoc />
    public class DefaultExpressionHandler : Common.DefaultExpressionHandler
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DefaultExpressionHandler() : base(true)
        {
        }

        /// <inheritdoc />
        protected override bool HandleDynamic(CodeObjectCreateExpression obj, Context ctx)
        {
            ctx.Writer.Write("New ");
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.CreateType, ctx);
            ctx.Writer.Write("(");
            GeneralUtils.HandleCollectionCommaSeparated(obj.Parameters.Cast<CodeExpression>(),
                ctx.HandlerProvider.ExpressionHandler, ctx);
            ctx.Writer.Write(")");
            if (obj is CodeObjectCreateExpressionExt objExt && objExt.PropertyInitializers.Any())
            {
                ctx.Writer.WriteLine(" With {");
                ctx.Indent();
                GeneralUtils.HandleCollection(objExt.PropertyInitializers, ctx.HandlerProvider.ExpressionHandler, ctx,
                    preAction: (c) => c.Writer.Indent(c), postAction: (c) => c.Writer.WriteLine(","), doPostActionOnLast: false);
                ctx.Unindent();
                ctx.Writer.NewLine();
                ctx.Writer.Indent(ctx);
                ctx.Writer.Write("}");
            }

            return true;
        }
        /// <inheritdoc />
        protected override bool HandleDynamic(CodeParameterDeclarationExpression obj, Context ctx)
        {
            if (obj.CustomAttributes.Count > 0)
            {
                GeneralUtils.HandleCollection(obj.CustomAttributes.Cast<CodeAttributeDeclaration>(),
                    ctx.HandlerProvider.AttributeDeclarationHandler, ctx);
                ctx.Writer.Write(" ");
            }

            CodeExpression defaultValue = (obj as CodeParameterDeclarationExpressionExt)?.DefaultValue;

            if (defaultValue != null)
            {
                ctx.Writer.Write("Optional ");
            }
            
            ctx.Writer.Write(VisualBasicKeywordsUtils.DirectionKeyword(obj.Direction) + " ");
            
            if (obj is CodeParameterDeclarationExpressionExt objExt && objExt.IsVarargs)
            {
                ctx.Writer.Write("ParamArray  ");
            }
            
            ctx.Writer.Write($"{obj.Name.AsVbId()} As ");
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.Type, ctx);
            
            if (defaultValue != null)
            {
                ctx.Writer.Write(" = ");
                ctx.HandlerProvider.ExpressionHandler.Handle(defaultValue, ctx);
            }

            return true;
        }
        /// <inheritdoc />
        protected override bool HandleDynamic(CodeArrayCreateExpression obj, Context ctx)
        {   
            ctx.Writer.Write("New ");
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.CreateType, ctx);
            ctx.Writer.Write("(");
            if (obj.Initializers.Count > 0)
            {
                ctx.Writer.Write(") {");
                GeneralUtils.HandleCollectionCommaSeparated(obj.Initializers.Cast<CodeExpression>(),
                    ctx.HandlerProvider.ExpressionHandler, ctx);
                ctx.Writer.Write("}");
            }
            else
            {
                if (obj.SizeExpression != null)
                {
                    ctx.HandlerProvider.ExpressionHandler.Handle(
                        new CodeBinaryOperatorExpressionMore(
                            obj.SizeExpression, 
                            CodeBinaryOperatorTypeMore.Subtract, 
                            new CodePrimitiveExpression(1)), ctx);
                }
                else
                {
                    ctx.Writer.Write((obj.Size - 1).ToString());
                }
                ctx.Writer.Write(") {}");
            }

            return true;
        }
        /// <inheritdoc />
        protected override string AsIdentifier(string s)
        {
            return s.AsVbId();
        }
        /// <inheritdoc />
        protected override bool HandleDynamic(CodeArrayIndexerExpression obj, Context ctx)
        {
            WrapIfNecessaryAndHandle(obj.TargetObject, ctx);
            ctx.Writer.Write("(");
            GeneralUtils.HandleCollectionCommaSeparated(obj.Indices.Cast<CodeExpression>(), ctx.HandlerProvider.ExpressionHandler, ctx);
            ctx.Writer.Write(")");
            return true;
        }
        /// <inheritdoc />
        protected override string BaseReferenceKeyword { get; } = "MyBase";
        /// <inheritdoc />
        protected override bool HandleDynamic(CodeCastExpression obj, Context ctx)
        {
            ctx.Writer.Write("CType(");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Expression, ctx);
            ctx.Writer.Write(", ");
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.TargetType, ctx);
            ctx.Writer.Write(")");
            return true;
        }
        /// <inheritdoc />
        protected override bool HandleDynamic(CodeDefaultValueExpression obj, Context ctx)
        {
            ctx.HandlerProvider.ExpressionHandler.Handle(
                new CodeCastExpression(obj.Type, new CodePrimitiveExpression(null)), ctx);
            return true;
        }

        /// <inheritdoc />
        protected override bool HandleDynamic(CodeDirectionExpression obj, Context ctx)
        {
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Expression, ctx);
            return true;
        }

        /// <inheritdoc />
        protected override bool HandleDynamic(CodeDelegateCreateExpression obj, Context ctx)
        {
            ctx.Writer.Write("AddressOf ");
            if (obj.TargetObject != null)
            {
                WrapIfNecessaryAndHandle(obj.TargetObject, ctx);
                ctx.Writer.Write(".");
            }
            ctx.Writer.Write(obj.MethodName.AsVbId());
            return true;
        }
        /// <inheritdoc />
        protected override bool HandleDynamic(CodeDelegateInvokeExpression obj, Context ctx)
        {
            if (obj.TargetObject is CodeEventReferenceExpression)
            {
                ctx.Writer.Write("RaiseEvent ");
            }
            WrapIfNecessaryAndHandle(obj.TargetObject, ctx);
            ctx.Writer.Write("(");
            GeneralUtils.HandleCollectionCommaSeparated(obj.Parameters.Cast<CodeExpression>(),
                ctx.HandlerProvider.ExpressionHandler, ctx);
            ctx.Writer.Write(")");
            return true;
        }
        /// <inheritdoc />
        protected override string MemberAccessOperator { get; } = ".";
        /// <inheritdoc />
        protected override bool HandleDynamic(CodeMethodInvokeExpression obj, Context ctx)
        {
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Method, ctx);
            ctx.Writer.Write("(");
            GeneralUtils.HandleCollectionCommaSeparated(obj.Parameters.Cast<CodeExpression>(),
                ctx.HandlerProvider.ExpressionHandler, ctx);
            ctx.Writer.Write(")");
            return true;
        }
        /// <inheritdoc />
        protected override bool HandleDynamic(CodeMethodReferenceExpression obj, Context ctx)
        {
            if (obj.TargetObject != null)
            {
                WrapIfNecessaryAndHandle(obj.TargetObject, ctx);
                ctx.Writer.Write(".");
            }
            ctx.Writer.Write(obj.MethodName.AsVbId());
            if (obj.TypeArguments.Count > 0)
            {
                ctx.Writer.Write("(Of ");
                GeneralUtils.HandleCollectionCommaSeparated(obj.TypeArguments.Cast<CodeTypeReference>(),
                    ctx.HandlerProvider.TypeReferenceHandler, ctx);
                ctx.Writer.Write(")");
            }
            return true;
        }
        
        /// <inheritdoc />
        protected override bool HandleDynamic(CodePropertyInitializerExpression obj, Context ctx)
        {
            ctx.Writer.Write($".{obj.PropertyName.AsVbId()} = ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.InitializerExpression, ctx);
            return true;
        }
        /// <inheritdoc />
        protected override string PropertySetValueReferenceKeyword { get; } = "Value";
        /// <inheritdoc />
        protected override string ThisReferenceKeyword { get; } = "Me";
        /// <inheritdoc />
        protected override bool HandleDynamic(CodeTypeOfExpression obj, Context ctx)
        {
            ctx.Writer.Write("GetType(");
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.Type, ctx);
            ctx.Writer.Write(")");
            return true;
        }
        /// <inheritdoc />
        protected override bool HandleDynamic(CodeUnaryOperatorExpression obj, Context ctx)
        {
            string operatorSymbol = VisualBasicKeywordsUtils.UnaryOperatorSymbol(obj.Operator);
            if (operatorSymbol != null)
            {
                ctx.Writer.Write(operatorSymbol);
                WrapIfNecessaryAndHandle(obj.Expression, ctx);
                return true;
            }

            if (obj.Operator == CodeUnaryOperatorType.PostDecrement ||
                obj.Operator == CodeUnaryOperatorType.PostIncrement ||
                obj.Operator == CodeUnaryOperatorType.PreDecrement ||
                obj.Operator == CodeUnaryOperatorType.PreIncrement)
            {
                //TODO this is ok when used as an expression, but not when used as a statement. Should be replaced with a method invocation, and add a library
                bool isIncrement = obj.Operator == CodeUnaryOperatorType.PostIncrement ||
                                   obj.Operator == CodeUnaryOperatorType.PreIncrement;
                bool isPre = obj.Operator == CodeUnaryOperatorType.PreDecrement ||
                             obj.Operator == CodeUnaryOperatorType.PreIncrement;

                CodeExpression toReturn;

                if (isPre)
                {
                    toReturn = obj.Expression;
                }
                else
                {
                    toReturn = new CodeBinaryOperatorExpressionMore(
                        obj.Expression,
                        isIncrement ? CodeBinaryOperatorTypeMore.Subtract : CodeBinaryOperatorTypeMore.Add,
                        Primitives.Int(1));
                }
                
                var lambda = new CodeLambdaDeclarationExpression(
                    new CodeCommentStatement(obj.Operator.ToString("G")),
                    new CodeOperationAssignmentStatement(
                        obj.Expression,
                        isIncrement ? CodeBinaryOperatorTypeMore.Add : CodeBinaryOperatorTypeMore.Subtract,
                        Primitives.Int(1)),
                    new CodeMethodReturnStatement(toReturn));
                ctx.HandlerProvider.ExpressionHandler.Handle(new CodeMethodInvokeExpression(lambda, "Invoke"), ctx);
                return true;
            }
            return false;
        }
        /// <inheritdoc />
        protected override bool HandleDynamic(CodeConditionalOperatorExpression obj, Context ctx)
        {
            ctx.Writer.Write("If(");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.TestExpression, ctx);
            ctx.Writer.Write(", ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.TrueExpression, ctx);
            ctx.Writer.Write(", ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.FalseExpression, ctx);
            ctx.Writer.Write(")");
            return true;
        }
        /// <inheritdoc />
        protected override bool HandleDynamic(CodeLambdaDeclarationExpression obj, Context ctx)
        {
            ctx.Writer.Write("Function"); //it is safe to declare it as a function, but will give a warning
            ctx.Writer.Write("(");
            GeneralUtils.HandleCollectionCommaSeparated(obj.Parameters, ctx.HandlerProvider.ExpressionHandler, ctx);
            ctx.Writer.Write(")");
            if (obj.Statements.Count == 1 && obj.Statements[0] is CodeExpressionStatement statement)
            {
                ctx.Writer.Write(" ");
                VisualBasicUtils.BeginBlock(BlockType.Function, ctx, false);
                ctx.HandlerProvider.ExpressionHandler.Handle(statement.Expression, ctx);
                VisualBasicUtils.EndBlock(ctx, false, false);
            }
            else
            {
                ctx.Writer.NewLine();
                ctx.Indent();
                VisualBasicUtils.BeginBlock(BlockType.Function, ctx, false);
                VisualBasicUtils.HandleStatementCollection(obj.Statements, ctx);
                ctx.Unindent();
                ctx.Writer.Indent(ctx);
                VisualBasicUtils.EndBlock(ctx, false);
            }
            
            return true;
        }
        /// <inheritdoc />
        protected override bool HandleDynamic(CodeLambdaParameterDeclarationExpression obj, Context ctx)
        {
            ctx.Writer.Write(obj.Name.AsVbId());
            if (obj.Type != null)
            {
                ctx.Writer.Write(" As ");
                ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.Type, ctx);
            }

            return true;
        }
        /// <inheritdoc />
        protected override bool HandleDynamic(CodeBinaryOperatorExpressionMore obj, Context ctx)
        {
            if (obj.OperatorType == CodeBinaryOperatorTypeMore.NullCoalescing)
            {
                ctx.Writer.Write("If(");
                ctx.HandlerProvider.ExpressionHandler.Handle(obj.LeftExpression, ctx);
                ctx.Writer.Write(", ");
                ctx.HandlerProvider.ExpressionHandler.Handle(obj.RightExpression, ctx);
                ctx.Writer.Write(")");
            }
            else
            {
                WrapIfNecessaryAndHandle(obj.LeftExpression, ctx);
                ctx.Writer.Write($" {VisualBasicKeywordsUtils.OperatorSymbol(obj.OperatorType)} ");
                WrapIfNecessaryAndHandle(obj.RightExpression, ctx);
            }

            return true;
        }
        /// <inheritdoc />
        protected override bool HandleDynamic(CodeMultidimensionalArrayCreateExpression obj, Context ctx)
        {
            ctx.Writer.Write("New ");
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.CreateType, ctx);
            ctx.Writer.Write("(");
            if (obj.SizeExpressions.Count > 0)
            {
                GeneralUtils.HandleCollectionCommaSeparated(
                    obj.SizeExpressions
                        .Select(exp => new CodeBinaryOperatorExpressionMore(
                            exp, 
                            CodeBinaryOperatorTypeMore.Subtract, 
                            new CodePrimitiveExpression(1))), 
                    ctx.HandlerProvider.ExpressionHandler, 
                    ctx);
            }
            else if (obj.Rank > 0)
            {
                for (int i = 1; i < obj.Rank; i++)
                {
                    ctx.Writer.Write(",");
                }
            }
            ctx.Writer.Write(") ");
            if (obj.InitializerExpression != null)
            {
                ctx.HandlerProvider.ExpressionHandler.Handle(obj.InitializerExpression, ctx);
            }
            else
            {
                ctx.Writer.Write("{}");
            }

            return true;
        }
        /// <inheritdoc />
        protected override bool HandleDynamic(CodeArrayInitializerExpression obj, Context ctx)
        {
            ctx.Writer.Write("{");
            GeneralUtils.HandleCollectionCommaSeparated(obj.Expressions, ctx.HandlerProvider.ExpressionHandler, ctx);
            ctx.Writer.Write("}");
            return true;
        }
        /// <inheritdoc />
        protected override bool HandleDynamic(CodePrimitiveExpression obj, Context ctx)
        {
            return _primitiveHandler.Handle(obj, ctx);
        }

        private readonly PrimitiveHandler _primitiveHandler = new PrimitiveHandler();
        private class PrimitiveHandler : PrimitiveExpressionHandler
        {
            protected override Type DefaultIntegerType { get; } = typeof(int);
            protected override Type DefaultFloatingPointType { get; } = typeof(double);
            
            public PrimitiveHandler() : base(false)
            {
            }
            
            protected override string GetForcedLiteral(Type type)
            {
                if (type == typeof(short))
                {
                    return "S";
                }
                else if (type == typeof(long))
                {
                    return "L";
                }
                else if (type == typeof(decimal))
                {
                    return "D";
                }
                else if (type == typeof(float))
                {
                    return "F";
                }
                else if (type == typeof(ushort))
                {
                    return "US";
                }
                else if (type == typeof(uint))
                {
                    return "UI";
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
                ctx.Writer.Write("Nothing");
            }

            protected override void HandleChar(char c, Context ctx)
            {
                ctx.Writer.Write("\"" + c + "\"C");
            }

            protected override void HandleString(string str, Context ctx)
            {
                ctx.Writer.Write("\"" + GeneralUtils.EscapeString(str, '"', '"') + "\"");
            }

            protected override void HandleBoolean(bool b, Context ctx)
            {
                ctx.Writer.Write(b ? "True" : "False");
            }
        }
        /// <inheritdoc />
        protected override void WrapAccessorTargetIfNecessaryAndHandle(CodeExpression obj, Context ctx)
        {
            WrapIfNecessaryAndHandle(obj, ctx);
        }

        private void WrapIfNecessaryAndHandle(CodeExpression obj, Context ctx)
        {
            GeneralUtils.WrapIfIsTypeAndHandle(obj, ctx, 
                typeof(CodeBinaryOperatorExpression),
                typeof(CodeBinaryOperatorExpressionMore),
                typeof(CodeLambdaDeclarationExpression));
        }
    }
}