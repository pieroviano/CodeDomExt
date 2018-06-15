using System;
using System.CodeDom;
using System.Linq;
using CodeDomExt.Nodes;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Common
{
    /// <summary>
    /// A statement handler providing some implementation for the simpler statements, and handling statements termination
    /// when needed and any Start and EndDirectives
    /// If an implementation of this shouldn't handle some of the statements it should return null or an empty string in
    /// methods/property returning string, or false in methods returning bool.
    /// </summary>
    /// <remarks>
    /// A statement should be handled on a new and indented line.<para />
    /// The statement handler should generate its termination (";\n" in c# or just "\n" in vb) only if context property
    /// <see cref="Context.StatementShouldTerminate"/> is set to true; statements containing other statements block may
    /// ignore this property and always handle termination. Said property is present in order to allow correct generation of
    /// the CodeIterationStatement, which has an InitStatement and IncrementStatement<para/>
    /// A statement handler does not handle StartDirectives and EndDirectives
    /// </remarks>
    public abstract class DefaultStatementHandler : DynamicDispatchHandler<CodeStatement>
    {
        private readonly bool _handleSnippet;
        private readonly bool _handleComment;
        private readonly bool _handleExpression;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="handleExpression">true if expression statement should be handled by this</param>
        /// <param name="handleSnippet">true if snippet statement should be handled by this</param>
        /// <param name="handleComment">true if comment statement should be handled by this</param>
        protected DefaultStatementHandler(bool handleExpression, bool handleSnippet, bool handleComment)
        {
            _handleExpression = handleExpression;
            _handleComment = handleComment;
            _handleSnippet = handleSnippet;
        }
        
        
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected override bool DoDynamicHandle(CodeStatement obj, Context ctx)
        {
            return HandleDynamic(obj as dynamic, ctx);
        }

        private bool HandleDynamic(CodeCommentStatement obj, Context ctx)
        {
            return HandleIfTrue(
                () =>
                {
                    ctx.HandlerProvider.CommentHandler.Handle(obj.Comment, ctx);
                    ctx.Writer.NewLine();
                }, obj, ctx, _handleComment, false);
        }

        private bool HandleDynamic(CodeExpressionStatement obj, Context ctx)
        {
            return HandleIfTrue(
                () => { ctx.HandlerProvider.ExpressionHandler.Handle(obj.Expression, ctx); }, obj, ctx,
                _handleExpression);
        }
        
        /// <summary>
        /// The symbol assignment operation or null
        /// </summary>
        protected abstract string AssignmentSymbol { get; }
        private bool HandleDynamic(CodeAssignStatement obj, Context ctx)
        {
            return HandleIfTrue(
                () =>
                {
                    ctx.HandlerProvider.ExpressionHandler.Handle(obj.Left, ctx);
                    ctx.Writer.Write($" {AssignmentSymbol} ");
                    ctx.HandlerProvider.ExpressionHandler.Handle(obj.Right, ctx);
                }, obj, ctx, !string.IsNullOrEmpty(AssignmentSymbol));
        }
 
        /// <summary>
        /// True if the handler should handle attach event statement
        /// </summary>
        protected abstract bool CanHandleAttachEvent { get; }
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract void HandleAttachEvent(CodeAttachEventStatement obj, Context ctx);
        private bool HandleDynamic(CodeAttachEventStatement obj, Context ctx)
        {
            return HandleIfTrue(() => { HandleAttachEvent(obj, ctx); }, obj, ctx, CanHandleAttachEvent);
        }

        /// <summary>
        /// True if the handler should handle condition statement
        /// </summary>
        protected abstract bool CanHandleCondition { get; }
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract void HandleCondition(CodeConditionStatement obj, Context ctx);
        private bool HandleDynamic(CodeConditionStatement obj, Context ctx)
        {
            return HandleIfTrue(() => { HandleCondition(obj, ctx); }, obj, ctx, CanHandleCondition, false);
        }

        /// <summary>
        /// The keyword for a goto statement
        /// </summary>
        protected abstract string GotoKeyword { get; }
        /// <summary>
        /// Returns the provided string as a valid identifier for the current language
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected abstract string AsIdentifier(string s);
        private bool HandleDynamic(CodeGotoStatement obj, Context ctx)
        {
            return HandleIfTrue(() => { ctx.Writer.Write($"{GotoKeyword} {AsIdentifier(obj.Label)}"); }, obj, ctx,
                !string.IsNullOrEmpty(GotoKeyword));
        }

        /// <summary>
        /// True if the handler should handle while statement
        /// </summary>
        protected abstract bool CanHandleWhile { get; }
        /// <summary>
        /// True if the handler should handle for statement
        /// </summary>
        protected abstract bool CanHandleFor { get; }
        /// <summary>
        /// Handle the code iteration statement ignoring its <see cref="CodeIterationStatement.InitStatement"/> and
        /// <see cref="CodeIterationStatement.IncrementStatement"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected abstract void HandleWhile(CodeIterationStatement obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract void HandleFor(CodeIterationStatement obj, Context ctx);
        private bool HandleDynamic(CodeIterationStatement obj, Context ctx)
        {
            if (obj.InitStatement == null && obj.IncrementStatement == null)
            {
                return HandleIfTrue(() => { HandleWhile(obj, ctx); }, obj, ctx, CanHandleWhile, false);

            }
            else
            {
                return HandleIfTrue(() => { HandleFor(obj, ctx); }, obj, ctx, CanHandleFor, false);
            }
        }

        /// <summary>
        /// String to be appended at the end of a code label (used by goto)
        /// </summary>
        protected abstract string LabelDefinitionSuffix { get; }
        private bool HandleDynamic(CodeLabeledStatement obj, Context ctx)
        {
            return HandleIfTrue(() => { ctx.Writer.WriteLine($"{AsIdentifier(obj.Label)}{LabelDefinitionSuffix}"); },
                obj, ctx, !string.IsNullOrEmpty(LabelDefinitionSuffix), false);
        }

        /// <summary>
        /// Return keyword or null
        /// </summary>
        protected abstract string ReturnKeyword { get; }
        private bool HandleDynamic(CodeMethodReturnStatement obj, Context ctx)
        {
            return HandleIfTrue(() =>
            {
                ctx.Writer.Write(ReturnKeyword);
                if (obj.Expression != null)
                {
                    ctx.Writer.Write(" ");
                    ctx.HandlerProvider.ExpressionHandler.Handle(obj.Expression, ctx);
                }
            }, obj, ctx, !string.IsNullOrEmpty(ReturnKeyword));
        }

        /// <summary>
        /// True if the handler should handle remove event statement
        /// </summary>
        protected abstract bool CanHandleRemoveEvent { get; }
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract void HandleRemoveEvent(CodeRemoveEventStatement obj, Context ctx);
        private bool HandleDynamic(CodeRemoveEventStatement obj, Context ctx)
        {
            return HandleIfTrue(() => { HandleRemoveEvent(obj, ctx); }, obj, ctx, CanHandleRemoveEvent);
        }

        /// <summary>
        /// Throw keyword or null
        /// </summary>
        protected abstract string ThrowKeyword { get; }
        private bool HandleDynamic(CodeThrowExceptionStatement obj, Context ctx)
        {
            return HandleIfTrue(() =>
            {
                ctx.Writer.Write($"{ThrowKeyword} ");
                ctx.HandlerProvider.ExpressionHandler.Handle(obj.ToThrow, ctx);
            }, obj, ctx, !string.IsNullOrEmpty(ThrowKeyword));
        }

        /// <summary>
        /// True if the handler should handle try-catch-finally statement
        /// </summary>
        protected abstract bool CanHandleTryCatchFinally { get; }
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract void HandleTryCatchFinally(CodeTryCatchFinallyStatement obj, Context ctx);
        private bool HandleDynamic(CodeTryCatchFinallyStatement obj, Context ctx)
        {
            return HandleIfTrue(() =>
            {
                HandleTryCatchFinally(obj, ctx);
            }, obj, ctx, CanHandleTryCatchFinally, false);
        }

        /// <summary>
        /// True if the handler should handle variable declaration statement
        /// </summary>
        protected abstract bool CanHandleVariableDeclaration { get; }
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract void HandleVariableDeclaration(CodeVariableDeclarationStatement obj, Context ctx);
        private bool HandleDynamic(CodeVariableDeclarationStatement obj, Context ctx)
        {
            return HandleIfTrue(() =>
            {
                HandleVariableDeclaration(obj, ctx);
            }, obj, ctx, CanHandleVariableDeclaration);
        }

        private bool HandleDynamic(CodeSnippetStatement obj, Context ctx)
        {
            return HandleIfTrue(() =>
            {
                GeneralUtils.HandleSnippet(obj.Value, ctx);
                ctx.Writer.NewLine();
            }, obj, ctx, _handleSnippet, false);
        }

        /// <summary>
        /// Returns the string for the provided operator if it can be used as a shorthand operator or null
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        protected abstract string GetShorthandOperatorSymbol(CodeBinaryOperatorTypeMore op);
        /// <summary>
        /// Return the symbol used for the assignment in a shorthand operator
        /// </summary>
        protected abstract string ShorthandOperatorAssignmentSymbol { get; }
        private bool HandleDynamic(CodeOperationAssignmentStatement obj, Context ctx)
        {
            string op = GetShorthandOperatorSymbol(obj.Operator);
            return HandleIfTrue(() =>
                {
                    ctx.HandlerProvider.ExpressionHandler.Handle(obj.LeftExpression, ctx);
                    ctx.Writer.Write($" {op}{ShorthandOperatorAssignmentSymbol} ");
                    ctx.HandlerProvider.ExpressionHandler.Handle(obj.RightExpression, ctx);
                }, obj, ctx, !string.IsNullOrEmpty(op) && !string.IsNullOrEmpty(ShorthandOperatorAssignmentSymbol));
        }

        /// <summary>
        /// True if the handler should handle do while statement
        /// </summary>
        protected abstract bool CanHandleDoWhile { get; }
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract void HandleDoWhile(CodePostTestIterationStatement obj, Context ctx);
        private bool HandleDynamic(CodePostTestIterationStatement obj, Context ctx)
        {
            return HandleIfTrue(() => { HandleDoWhile(obj, ctx); }, obj, ctx, CanHandleDoWhile, false);
        }

        /// <summary>
        /// True if the handler should handle foreach statement
        /// </summary>
        protected abstract bool CanHandleForEach { get; }
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract void HandleForEach(CodeForEachStatement obj, Context ctx);
        private bool HandleDynamic(CodeForEachStatement obj, Context ctx)
        {
            return HandleIfTrue(() => { HandleForEach(obj, ctx); }, obj, ctx, CanHandleForEach, false);
        }

        /// <summary>
        /// True if the handler should handle using statement
        /// </summary>
        protected abstract bool CanHandleUsing { get; }
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract void HandleUsing(CodeUsingStatement obj, Context ctx);
        private bool HandleDynamic(CodeUsingStatement obj, Context ctx)
        {
            return HandleIfTrue(() =>
            {
                HandleUsing(obj, ctx);
            }, obj, ctx, CanHandleUsing, false);
        }

        /// <summary>
        /// True if the handler should handle break statement
        /// </summary>
        protected abstract bool CanHandleBreak { get; }
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract void HandleBreak(CodeBreakStatement obj, Context ctx);
        private bool HandleDynamic(CodeBreakStatement obj, Context ctx)
        {
            return HandleIfTrue(() => { HandleBreak(obj, ctx); }, obj, ctx, CanHandleBreak);
        }

        /// <summary>
        /// Handle the termination of a statement if ctx <see cref="Context.StatementShouldTerminate"/> property is set to true
        /// </summary>
        /// <param name="ctx"></param>
        protected abstract void DoTermination(Context ctx);

        private bool HandleIfTrue(Action handleAction, CodeStatement obj, Context ctx, bool condition, bool doTerminationIfNeeded = true)
        {
            if (condition)
            {
                GeneralUtils.HandleCollectionOnMultipleLines(obj.StartDirectives.Cast<CodeDirective>(),
                    ctx.HandlerProvider.DirectiveHandler, ctx, false);
                handleAction();
                if (doTerminationIfNeeded && ctx.StatementShouldTerminate)
                {
                    DoTermination(ctx);
                }
                GeneralUtils.HandleCollectionOnMultipleLines(obj.EndDirectives.Cast<CodeDirective>(),
                    ctx.HandlerProvider.DirectiveHandler, ctx, true);
            }

            return condition;
        }
    }
}