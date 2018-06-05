using System.CodeDom;

namespace CodeDomExt.Generators.Common
{
    /// <summary>
    /// A partial namespace handler, only handling namespace comments generation, leaving the actual namespace handling
    /// to subclasses
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
                ctx.Writer.Indent(ctx);
            }

            return DoHandle(obj, ctx);
        }

        /// <inheritdoc cref="Handle"/>
        protected abstract bool DoHandle(CodeNamespace obj, Context ctx);
    }
}