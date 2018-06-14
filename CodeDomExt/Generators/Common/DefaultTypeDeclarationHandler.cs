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
            DeclarationType type = GeneralUtils.CheckAndGetDeclarationType(obj, ctx);

            if (!CanHandle(type)) //since i'm going to do part of the generation i must make sure i can handle the whole object
            {
                return false;
            }
            
            foreach (CodeCommentStatement comment in obj.Comments)
            {
                ctx.HandlerProvider.StatementHandler.Handle(comment, ctx);
                ctx.Writer.Indent(ctx);
            }
            
            ctx.TypeDeclarationStack.Push(new Tuple<DeclarationType, CodeTypeDeclaration>(type, obj));

            HandleTypeDeclaration(obj, type, ctx);

            ctx.TypeDeclarationStack.Pop();
            return true;
        }

        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract void HandleTypeDeclaration(CodeTypeDeclaration obj, DeclarationType type, Context ctx);
        /// <summary>
        /// Returns true if the provided declaration type can be handled
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected abstract bool CanHandle(DeclarationType type);
    }
}