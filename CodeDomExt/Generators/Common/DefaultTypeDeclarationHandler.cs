using System;
using System.CodeDom;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Common
{
    /// <summary>
    /// A partial implementation of a type declaration handler, which will handle comments, and will manage the
    /// <see cref="Context.TypeDeclarationStack"/>
    /// </summary>
    /// <remarks>
    /// A type declaration handler should not handle indentation.
    /// The type declaration handling should end with a newLine.
    /// </remarks>
    public abstract class DefaultTypeDeclarationHandler : ICodeObjectHandler<CodeTypeDeclaration>
    {
        /// <inheritdoc />
        public bool Handle(CodeTypeDeclaration obj, Context ctx)
        {
            foreach (CodeCommentStatement comment in obj.Comments)
            {
                ctx.HandlerProvider.StatementHandler.Handle(comment, ctx);
                ctx.Writer.Indent(ctx);
            }
            
            DeclarationType type = GeneralUtils.CheckAndGetDeclarationType(obj, ctx);
            ctx.TypeDeclarationStack.Push(new Tuple<DeclarationType, CodeTypeDeclaration>(type, obj));

            bool res = HandleTypeDeclaration(obj, type, ctx);

            ctx.TypeDeclarationStack.Pop();
            return res;
        }

        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleTypeDeclaration(CodeTypeDeclaration obj, DeclarationType type, Context ctx);
    }
}