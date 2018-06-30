using System.CodeDom;

namespace CodeDomExt.Generators.Common
{
    /// <summary>
    /// A partial namespace handler, only handling namespace comments generation, leaving the actual namespace handling
    /// to the implementing class
    /// </summary>
    /// <remarks>
    /// A namespace handler should handle the imports only if the target language supports namespace specific imports,
    /// otherwise the imports should be handled by the compileUnit handler.
    /// The namespace handling should end with a newLine.
    /// </remarks>
    public abstract class DefaultNamespaceHandler : ICodeObjectHandler<CodeNamespace>
    {
        /// <inheritdoc />
        public bool Handle(CodeNamespace obj, Context ctx)
        {
            foreach (CodeCommentStatement comment in obj.Comments)
            {
                ctx.HandlerProvider.StatementHandler.Handle(comment, ctx);
                ctx.Writer.NewLine();
                ctx.Writer.Indent(ctx);
            }
            //since i'm already doing some handling DoHandle must handle the rest of the namespace
            DoHandle(obj, ctx);
            return true;
        }

        /// <inheritdoc cref="Handle"/>
        protected abstract void DoHandle(CodeNamespace obj, Context ctx);
    }
}