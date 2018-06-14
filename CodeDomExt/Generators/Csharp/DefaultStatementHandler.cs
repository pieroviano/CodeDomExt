using System.CodeDom;
using System.Linq;
using CodeDomExt.Nodes;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Csharp
{
    /// <inheritdoc/>
    public class DefaultStatementHandler : Common.DefaultStatementHandler
    {
        private readonly CatchClauseHandler _catchClauseHandler = new CatchClauseHandler();

        /// <inheritdoc/>
        protected override string AsIdentifier(string s)
        {
            return s.AsCsId();
        }

        /// <inheritdoc/>
        protected override string GotoKeyword { get; } = "goto";
        /// <inheritdoc/>
        protected override string AssignmentSymbol { get; } = "=";
        /// <inheritdoc/>
        protected override string LabelDefinitionSuffix { get; } = ":";
        /// <inheritdoc/>
        protected override string ReturnKeyword { get; } = "return";
        /// <inheritdoc/>
        protected override string ThrowKeyword { get; } = "throw";
        /// <inheritdoc/>
        protected override string ShorthandOperatorAssignmentSymbol { get; } = "=";
        /// <inheritdoc/>
        protected override string GetShorthandOperatorSymbol(CodeBinaryOperatorTypeMore op)
        {
            return op.CanBeShorthandOperator() ? CSharpKeywordsUtils.OperatorSymbol(op) : null;
        }
        /// <inheritdoc/>
        protected override bool HandleAttachEvent(CodeAttachEventStatement obj, Context ctx)
        {
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Event, ctx);
            ctx.Writer.Write(" += ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Listener, ctx);
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleCondition(CodeConditionStatement obj, Context ctx)
        {
            ctx.Writer.Write("if (");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Condition, ctx);
            ctx.Writer.Write(")");
            CSharpUtils.HandleStatementCollection(obj.TrueStatements, ctx);
            if (obj.FalseStatements.Count > 0)
            {
                ctx.Writer.IndentAndWrite("else", ctx);
                if (obj.FalseStatements.Count == 1 && obj.FalseStatements[0] is CodeConditionStatement elseIf)
                {
                    ctx.Writer.Write(" ");
                    HandleCondition(elseIf, ctx);
                }
                else
                {
                    CSharpUtils.HandleStatementCollection(obj.FalseStatements, ctx);
                }
            }
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleWhile(CodeIterationStatement obj, Context ctx)
        {
            ctx.Writer.Write("while (");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.TestExpression, ctx);
            ctx.Writer.Write(")");
            CSharpUtils.HandleStatementCollection(obj.Statements, ctx);
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleFor(CodeIterationStatement obj, Context ctx)
        {
            ctx.StatementShouldTerminate = false;
            ctx.Writer.Write("for (");
            if (obj.InitStatement != null)
            {
                ctx.HandlerProvider.StatementHandler.Handle(obj.InitStatement, ctx);
            }
            ctx.Writer.Write("; ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.TestExpression, ctx);
            ctx.Writer.Write("; ");
            if (obj.IncrementStatement != null)
            {
                ctx.HandlerProvider.StatementHandler.Handle(obj.IncrementStatement, ctx);
            }
            ctx.Writer.Write(")");
            ctx.StatementShouldTerminate = true;
            CSharpUtils.HandleStatementCollection(obj.Statements, ctx);
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleRemoveEvent(CodeRemoveEventStatement obj, Context ctx)
        {
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Event, ctx);
            ctx.Writer.Write(" -= ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Listener, ctx);
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleTryCatchFinally(CodeTryCatchFinallyStatement obj, Context ctx)
        {
            ctx.Writer.Write("try");
            CSharpUtils.HandleStatementCollection(obj.TryStatements, ctx);
            GeneralUtils.HandleCollection(obj.CatchClauses.Cast<CodeCatchClause>(), _catchClauseHandler, ctx,
                preAction: (c) => c.Writer.Indent(c));
            if (obj.FinallyStatements.Count > 0)
            {
                ctx.Writer.Write("finally");
                CSharpUtils.HandleStatementCollection(obj.FinallyStatements, ctx);
            }
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleVariableDeclaration(CodeVariableDeclarationStatement obj, Context ctx)
        {
            WriteTypeOrVar(obj.Type, ctx);
            ctx.Writer.Write($" {obj.Name.AsCsId()}");
            if (obj.InitExpression != null)
            {
                ctx.Writer.Write(" = ");
                ctx.HandlerProvider.ExpressionHandler.Handle(obj.InitExpression, ctx);
            }
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleDoWhile(CodePostTestIterationStatement obj, Context ctx)
        {
            ctx.Writer.WriteLine("do");
            ctx.Writer.IndentAndWriteLine("{", ctx);
            ctx.Indent();
            CSharpUtils.HandleStatementCollection(obj.Statements, ctx, false);
            ctx.Unindent();
            ctx.Writer.IndentAndWrite("} while (", ctx);
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.TestExpression, ctx);
            ctx.Writer.WriteLine(");");
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleForEach(CodeForEachStatement obj, Context ctx)
        {
            ctx.Writer.Write("foreach (");
            WriteTypeOrVar(obj.ItemType, ctx);
            ctx.Writer.Write($" {obj.ItemName.AsCsId()} in ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.ObjectToIterate, ctx);
            ctx.Writer.Write(")");

            CSharpUtils.HandleStatementCollection(obj.Statements, ctx);
            
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleUsing(CodeUsingStatement obj, Context ctx)
        {
            ctx.Writer.Write("using (");
            WriteTypeOrVar(obj.Type, ctx);
            ctx.Writer.Write($" {obj.VariableName.AsCsId()} = ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.InitializerExpression, ctx);
            ctx.Writer.Write(")");
            CSharpUtils.HandleStatementCollection(obj.Statements, ctx);
            
            return true;
        }
        /// <inheritdoc/>
        protected override bool HandleBreak(CodeBreakStatement obj, Context ctx)
        {
            ctx.Writer.Write("break");
            return true;
        }

        /// <inheritdoc />
        protected override void DoTermination(Context ctx)
        {
            ctx.Writer.WriteLine(";");
        }

        private void WriteTypeOrVar(CodeTypeReference type, Context ctx)
        {
            if (GeneralUtils.IsNullOrVoidType(type))
            {
                ctx.Writer.Write("var");
            }
            else
            {
                ctx.HandlerProvider.TypeReferenceHandler.Handle(type, ctx);
            }
        }

        private class CatchClauseHandler : ICodeObjectHandler<CodeCatchClause>
        {
            public bool Handle(CodeCatchClause obj, Context ctx)
            {
                ctx.Writer.Write("catch (");
                ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.CatchExceptionType, ctx);
                if (!string.IsNullOrEmpty(obj.LocalName))
                {
                    ctx.Writer.Write($" {obj.LocalName.AsCsId()}");
                }
                ctx.Writer.Write(")");
                CSharpUtils.HandleStatementCollection(obj.Statements, ctx);
                return true;
            }
        }
    }
}