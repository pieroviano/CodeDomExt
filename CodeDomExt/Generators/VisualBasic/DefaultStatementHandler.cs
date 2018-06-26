using System.CodeDom;
using System.Linq;
using CodeDomExt.Nodes;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.VisualBasic
{
    /// <inheritdoc />
    public class DefaultStatementHandler : Common.DefaultStatementHandler
    {
        public DefaultStatementHandler() : base(true, true, true)
        {
        }
        
        /// <inheritdoc />
        protected override string AssignmentSymbol { get; } = "=";

        /// <inheritdoc />
        protected override bool CanHandleAttachEvent => true;

        /// <inheritdoc />
        protected override void HandleAttachEvent(CodeAttachEventStatement obj, Context ctx)
        {
            ctx.Writer.Write("AddHandler ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Event, ctx);
            ctx.Writer.Write(", ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Listener, ctx);
        }

        /// <inheritdoc />
        protected override bool CanHandleCondition => true;

        /// <inheritdoc />
        protected override void HandleCondition(CodeConditionStatement obj, Context ctx)
        {
            VisualBasicUtils.BeginBlock(BlockType.If, ctx, false);
            HandleConditionNoBlock(obj, ctx);
            ctx.Writer.Indent(ctx);
            VisualBasicUtils.EndBlock(ctx, false);
            ctx.Writer.NewLine();
        }
        private void HandleConditionNoBlock(CodeConditionStatement obj, Context ctx)
        {
            ctx.Writer.Write("If ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Condition, ctx);
            ctx.Writer.Write(" Then");
            ctx.Indent();
            ctx.Writer.NewLine();
            VisualBasicUtils.HandleStatementCollection(obj.TrueStatements, ctx);
            ctx.Unindent();
            if (obj.FalseStatements.Count == 1 && obj.FalseStatements[0] is CodeConditionStatement elseIf)
            {
                ctx.Writer.IndentAndWrite("Else", ctx);
                HandleConditionNoBlock(elseIf, ctx);
            }
            else if (obj.FalseStatements.Count > 0)
            {
                ctx.Writer.IndentAndWriteLine("Else", ctx);
                ctx.Indent();
                VisualBasicUtils.HandleStatementCollection(obj.FalseStatements, ctx);
                ctx.Unindent();
            }
        }
        /// <inheritdoc />
        protected override string GotoKeyword { get; } = "GoTo";
        /// <inheritdoc />
        protected override string AsIdentifier(string s)
        {
            return s.AsVbId();
        }

        /// <inheritdoc />
        protected override bool CanHandleWhile => true;

        /// <inheritdoc />
        protected override bool CanHandleFor => true;

        /// <inheritdoc />
        protected override void HandleWhile(CodeIterationStatement obj, Context ctx)
        {
            ctx.Writer.Write("While ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.TestExpression, ctx);
            VisualBasicUtils.HandleStatementCollection(obj.Statements, ctx, BlockType.While);
        }
        /// <inheritdoc />
        protected override void HandleFor(CodeIterationStatement obj, Context ctx)
        {
            if (obj.InitStatement != null)
            {
                ctx.HandlerProvider.StatementHandler.Handle(obj.InitStatement, ctx);
            }
            ctx.Writer.Indent(ctx);
            CodeIterationStatement equivalent = new CodeIterationStatement(null, obj.TestExpression, null);
            equivalent.Statements.AddRange(obj.Statements);
            if (obj.IncrementStatement != null) {
                equivalent.Statements.Add(obj.IncrementStatement);
            }
            ctx.HandlerProvider.StatementHandler.Handle(equivalent, ctx);
        }
        /// <inheritdoc />
        protected override string LabelDefinitionSuffix { get; } = ":";
        /// <inheritdoc />
        protected override string ReturnKeyword { get; } = "Return";

        /// <inheritdoc />
        protected override bool CanHandleRemoveEvent => true;

        /// <inheritdoc />
        protected override void HandleRemoveEvent(CodeRemoveEventStatement obj, Context ctx)
        {
            ctx.Writer.Write("RemoveHandler ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Event, ctx);
            ctx.Writer.Write(", AddressOf ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Listener, ctx);
        }
        /// <inheritdoc />
        protected override string ThrowKeyword { get; } = "Throw";

        /// <inheritdoc />
        protected override bool CanHandleTryCatchFinally => true;

        /// <inheritdoc />
        protected override void HandleTryCatchFinally(CodeTryCatchFinallyStatement obj, Context ctx)
        {
            VisualBasicUtils.BeginBlock(BlockType.Try, ctx, false);
            ctx.Writer.Write("Try");
            ctx.Indent();
            ctx.Writer.NewLine();
            VisualBasicUtils.HandleStatementCollection(obj.TryStatements, ctx);
            ctx.Unindent();
            GeneralUtils.HandleCollection(obj.CatchClauses.Cast<CodeCatchClause>(), _catchClauseHandler, ctx,
                preAction: (c) => c.Writer.Indent(c));
            if (obj.FinallyStatements.Count > 0)
            {
                ctx.Writer.Indent(ctx);
                ctx.Writer.WriteLine("Finally");
                ctx.Indent();
                VisualBasicUtils.HandleStatementCollection(obj.FinallyStatements, ctx);
                ctx.Unindent();
            }
            ctx.Writer.Indent(ctx);
            VisualBasicUtils.EndBlock(ctx, false);
            ctx.Writer.NewLine();
        }

        /// <inheritdoc />
        protected override bool CanHandleVariableDeclaration => true;

        /// <inheritdoc />
        protected override void HandleVariableDeclaration(CodeVariableDeclarationStatement obj, Context ctx)
        {
            ctx.Writer.Write($"Dim {obj.Name.AsVbId()}");
            if (!GeneralUtils.IsNullOrVoidType(obj.Type))
            {
                ctx.Writer.Write(" As ");
                ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.Type, ctx);
            }
            if (obj.InitExpression != null) {
                ctx.Writer.Write(" = ");
                ctx.HandlerProvider.ExpressionHandler.Handle(obj.InitExpression, ctx);
            }
        }
        /// <inheritdoc />
        protected override string ShorthandOperatorAssignmentSymbol { get; } = "=";
        /// <inheritdoc />
        protected override string GetShorthandOperatorSymbol(CodeBinaryOperatorTypeMore op)
        {
            return op.CanBeShorthandOperator() ? VisualBasicKeywordsUtils.OperatorSymbol(op) : null;
        }

        /// <inheritdoc />
        protected override bool CanHandleDoWhile => true;

        /// <inheritdoc />
        protected override void HandleDoWhile(CodePostTestIterationStatement obj, Context ctx)
        {
            VisualBasicUtils.BeginBlock(BlockType.Do, ctx, false);
            ctx.Writer.WriteLine("Do");
            ctx.Indent();
            VisualBasicUtils.HandleStatementCollection(obj.Statements, ctx);
            ctx.Unindent();
            ctx.Writer.IndentAndWrite("Loop While ", ctx);
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.TestExpression, ctx);
            ctx.Writer.NewLine();
            VisualBasicUtils.EndBlock(ctx, false, false);
        }

        /// <inheritdoc />
        protected override bool CanHandleForEach => true;

        /// <inheritdoc />
        protected override void HandleForEach(CodeForEachStatement obj, Context ctx)
        {
            VisualBasicUtils.BeginBlock(BlockType.For, ctx, false);
            ctx.Writer.Write($"For Each {obj.ItemName.AsVbId()}");
            if (!GeneralUtils.IsNullOrVoidType(obj.ItemType))
            {
                ctx.Writer.Write(" As ");
                ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.ItemType, ctx);
            }
            ctx.Writer.Write(" In ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.ObjectToIterate, ctx);
            ctx.Writer.NewLine();
            ctx.Indent();
            VisualBasicUtils.HandleStatementCollection(obj.Statements, ctx);
            ctx.Unindent();
            ctx.Writer.IndentAndWriteLine("Next", ctx);
            VisualBasicUtils.EndBlock(ctx, false, false);
        }

        /// <inheritdoc />
        protected override bool CanHandleUsing => true;

        /// <inheritdoc />
        protected override void HandleUsing(CodeUsingStatement obj, Context ctx)
        {
            ctx.Writer.Write($"Using {obj.VariableName} As ");
            if (!GeneralUtils.IsNullOrVoidType(obj.Type))
            {
                ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.Type, ctx);
                ctx.Writer.Write(" = ");
            }

            ctx.HandlerProvider.ExpressionHandler.Handle(obj.InitializerExpression, ctx);
            VisualBasicUtils.HandleStatementCollection(obj.Statements, ctx, BlockType.Using);
        }

        /// <inheritdoc />
        protected override bool CanHandleBreak => true;

        /// <inheritdoc />
        protected override void HandleBreak(CodeBreakStatement obj, Context ctx)
        {
            ctx.Writer.Write(
                $"Exit {ctx.VisualBasic.BlockTypeStack.First((blockType) => blockType.CanExit()).GetKeyword()}");
        }

        /// <inheritdoc />
        protected override void DoTermination(Context ctx)
        {
            ctx.Writer.NewLine();
        }

        private readonly CatchClauseHandler _catchClauseHandler = new CatchClauseHandler();
        private class CatchClauseHandler : ICodeObjectHandler<CodeCatchClause>
        {
            private const string DefaultExceptionName = "__exception";
            public bool Handle(CodeCatchClause obj, Context ctx)
            {
                ctx.Writer.Write($"Catch {obj.LocalName?.AsVbId() ?? DefaultExceptionName} As ");
                ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.CatchExceptionType, ctx);
                ctx.Indent();
                ctx.Writer.NewLine();
                VisualBasicUtils.HandleStatementCollection(obj.Statements, ctx);
                ctx.Unindent();
                return true;
            }
        }
    }
}