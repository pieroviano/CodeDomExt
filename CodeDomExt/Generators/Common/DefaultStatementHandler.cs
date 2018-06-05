using System.CodeDom;
using CodeDomExt.Nodes;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Common
{
    /// <summary>
    /// A statement handler providing some implementation for the simpler statements, which also sets context property
    /// <see cref="Context.StatementNeedsTermination"/>.
    /// If an implementation of this shouldn't handle some of the statements it should return null or an empty string in
    /// methods/property returning string, or false in methods returning bool.
    /// </summary>
    /// <remarks>
    /// A statement should be handled on a new and indented line.<para />
    /// When a statement is handled, before returning the handler should set context property
    /// <see cref="Context.StatementNeedsTermination"/> to true if it leaves doing the line termination
    /// (; when necessary and newLine in C#) to the upper level, or false if it handled termination by itself.<para />
    /// The statement handler should generate line termination only when handling statements that contains other
    /// statements (like iterations or conditions)
    /// </remarks>
    public abstract class DefaultStatementHandler : DynamicDispatchHandler<CodeStatement>
    {
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected override bool DoDynamicHandle(CodeStatement obj, Context ctx)
        {
            return HandleDynamic(obj as dynamic, ctx);
        }

        private bool HandleDynamic(CodeCommentStatement obj, Context ctx)
        {
            ctx.HandlerProvider.CommentHandler.Handle(obj.Comment, ctx);
            ctx.Writer.NewLine();
            ctx.StatementNeedsTermination = false;
            return true;
        }

        private bool HandleDynamic(CodeExpressionStatement obj, Context ctx)
        {
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Expression, ctx);
            ctx.StatementNeedsTermination = true;
            return true;
        }
        
        /// <summary>
        /// The symbol assignment operation or null
        /// </summary>
        protected abstract string AssignmentSymbol { get; }
        private bool HandleDynamic(CodeAssignStatement obj, Context ctx)
        {
            if (string.IsNullOrEmpty(AssignmentSymbol))
            {
                return false;
            }
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Left, ctx);
            ctx.Writer.Write($" {AssignmentSymbol} ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.Right, ctx);
            ctx.StatementNeedsTermination = true;
            return true;
        }
 
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleAttachEvent(CodeAttachEventStatement obj, Context ctx);
        private bool HandleDynamic(CodeAttachEventStatement obj, Context ctx)
        {
            bool res = HandleAttachEvent(obj, ctx);
            ctx.StatementNeedsTermination = true;
            return res;
        }

        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleCondition(CodeConditionStatement obj, Context ctx);
        private bool HandleDynamic(CodeConditionStatement obj, Context ctx)
        {
            bool res = HandleCondition(obj, ctx);
            ctx.StatementNeedsTermination = false;
            return res;
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
            if (string.IsNullOrEmpty(GotoKeyword))
            {
                return false;
            }
            ctx.Writer.Write($"{GotoKeyword} {AsIdentifier(obj.Label)}");
            ctx.StatementNeedsTermination = true;
            return true;
        }

        /// <summary>
        /// Handle the code iteration statement ignoring its <see cref="CodeIterationStatement.InitStatement"/> and
        /// <see cref="CodeIterationStatement.IncrementStatement"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected abstract bool HandleWhile(CodeIterationStatement obj, Context ctx);
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleFor(CodeIterationStatement obj, Context ctx);
        private bool HandleDynamic(CodeIterationStatement obj, Context ctx)
        {
            bool res;
            if (obj.InitStatement == null && obj.IncrementStatement == null)
            {
                res = HandleWhile(obj, ctx);
            }
            else
            {
                res = HandleFor(obj, ctx);
            }
            ctx.StatementNeedsTermination = false;
            return res;
        }

        /// <summary>
        /// String to be appended at the end of a code label (used by goto)
        /// </summary>
        protected abstract string LabelDefinitionSuffix { get; }
        private bool HandleDynamic(CodeLabeledStatement obj, Context ctx)
        {
            if (string.IsNullOrEmpty(LabelDefinitionSuffix))
            {
                return false;
            }
            ctx.Writer.WriteLine($"{AsIdentifier(obj.Label)}{LabelDefinitionSuffix}");
            ctx.StatementNeedsTermination = false;
            return true;
        }

        /// <summary>
        /// Return keyword or null
        /// </summary>
        protected abstract string ReturnKeyword { get; }
        private bool HandleDynamic(CodeMethodReturnStatement obj, Context ctx)
        {
            if (string.IsNullOrEmpty(ReturnKeyword))
            {
                return false;
            }
            ctx.Writer.Write(ReturnKeyword);
            if (obj.Expression != null)
            {
                ctx.Writer.Write(" ");
                ctx.HandlerProvider.ExpressionHandler.Handle(obj.Expression, ctx);
            }
            ctx.StatementNeedsTermination = true;
            return true;
        }

        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleRemoveEvent(CodeRemoveEventStatement obj, Context ctx);
        private bool HandleDynamic(CodeRemoveEventStatement obj, Context ctx)
        {
            bool res = HandleRemoveEvent(obj, ctx);
            ctx.StatementNeedsTermination = true;
            return res;
        }

        /// <summary>
        /// Throw keyword or null
        /// </summary>
        protected abstract string ThrowKeyword { get; }
        private bool HandleDynamic(CodeThrowExceptionStatement obj, Context ctx)
        {
            if (string.IsNullOrEmpty(ThrowKeyword))
            {
                return false;
            }
            ctx.Writer.Write($"{ThrowKeyword} ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.ToThrow, ctx);
            ctx.StatementNeedsTermination = true;
            return true;
        }

        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleTryCatchFinally(CodeTryCatchFinallyStatement obj, Context ctx);
        private bool HandleDynamic(CodeTryCatchFinallyStatement obj, Context ctx)
        {
            bool res = HandleTryCatchFinally(obj, ctx);
            ctx.StatementNeedsTermination = false;
            return res;
        }

        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleVariableDeclaration(CodeVariableDeclarationStatement obj, Context ctx);
        private bool HandleDynamic(CodeVariableDeclarationStatement obj, Context ctx)
        {
            bool res = HandleVariableDeclaration(obj, ctx);
            ctx.StatementNeedsTermination = true;
            return res;
        }

        private bool HandleDynamic(CodeSnippetStatement obj, Context ctx)
        {
            GeneralUtils.HandleSnippet(obj.Value, ctx);
            ctx.Writer.NewLine();
            ctx.StatementNeedsTermination = false;
            return true;
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
            if (string.IsNullOrEmpty(op) || string.IsNullOrEmpty(ShorthandOperatorAssignmentSymbol))
            {
                return false;
            }
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.LeftExpression, ctx);
            ctx.Writer.Write($" {op}{ShorthandOperatorAssignmentSymbol} ");
            ctx.HandlerProvider.ExpressionHandler.Handle(obj.RightExpression, ctx);
            ctx.StatementNeedsTermination = true;
            return true;
        }

        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleDoWhile(CodePostTestIterationStatement obj, Context ctx);
        private bool HandleDynamic(CodePostTestIterationStatement obj, Context ctx)
        {
            bool res = HandleDoWhile(obj, ctx);
            ctx.StatementNeedsTermination = false;
            return res;
        }

        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleForEach(CodeForEachStatement obj, Context ctx);
        private bool HandleDynamic(CodeForEachStatement obj, Context ctx)
        {
            bool res = HandleForEach(obj, ctx);
            ctx.StatementNeedsTermination = false;
            return res;
        }

        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleUsing(CodeUsingStatement obj, Context ctx);
        private bool HandleDynamic(CodeUsingStatement obj, Context ctx)
        {
            bool res = HandleUsing(obj, ctx);
            ctx.StatementNeedsTermination = false;
            return res;
        }

        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleBreak(CodeBreakStatement obj, Context ctx);
        private bool HandleDynamic(CodeBreakStatement obj, Context ctx)
        {
            bool res = HandleBreak(obj, ctx);
            ctx.StatementNeedsTermination = true;
            return res;
        }
    }
}