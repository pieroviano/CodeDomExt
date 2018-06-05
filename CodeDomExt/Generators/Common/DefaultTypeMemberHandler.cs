using System.CodeDom;
using System.Linq;
using System.Reflection;
using CodeDomExt.Nodes;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Common
{
    /// <summary>
    /// A partial implementation of a type member handler, which will handle comments, custom attributes and will
    /// manage the <see cref="Context.TypeMemberStack"/>. It will also delegate any type declaration handling to the
    /// type declaration handler
    /// </summary>
    /// <remarks>
    /// A type member handler should not handle indentation.
    /// The type member handling should end with a newLine.
    /// </remarks>
    public abstract class DefaultTypeMemberHandler : DynamicDispatchHandler<CodeTypeMember>
    {
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected override bool DoDynamicHandle(CodeTypeMember obj, Context ctx)
        {
            foreach (CodeCommentStatement comment in obj.Comments)
            {
                ctx.HandlerProvider.StatementHandler.Handle(comment, ctx);
                ctx.Writer.Indent(ctx);
            }
            
            if (obj is CodeTypeDeclaration declaration)
            {
                ctx.HandlerProvider.TypeDeclarationHandler.Handle(declaration, ctx);
                return true;
            }

            if (obj.CustomAttributes.Count > 0)
            {
                GeneralUtils.HandleCollection(obj.CustomAttributes.Cast<CodeAttributeDeclaration>(),
                    ctx.HandlerProvider.AttributeDeclarationHandler, ctx,
                    postAction: (c) =>
                    {
                        c.Writer.NewLine();
                        c.Writer.Indent(c);
                    }, doPostActionOnLast: true);
            }

            return HandleDynamic(obj as dynamic, ctx);
        }

        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleEvent(CodeMemberEvent obj, Context ctx);
        private bool HandleDynamic(CodeMemberEvent obj, Context ctx)
        {
            ctx.TypeMemberStack.Push(MemberTypes.Event);
            var res = HandleEvent(obj, ctx);
            ctx.TypeMemberStack.Pop();
            return res;
        }

        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleProperty(CodeMemberProperty obj, Context ctx, bool isExt, 
            CodeMemberPropertyExt objExt, bool doDefaultImplementation);
        private bool HandleDynamic(CodeMemberProperty obj, Context ctx)
        {
            ctx.TypeMemberStack.Push(MemberTypes.Property);
            CodeMemberPropertyExt objExt = null;
            bool isExt = false;
            if (obj is CodeMemberPropertyExt tmp)
            {
                objExt = tmp;
                isExt = true;
            }

            bool doDefaultImplementation = obj.GetStatements.Count == 0 && obj.SetStatements.Count == 0;
            var res = HandleProperty(obj, ctx, isExt, objExt, doDefaultImplementation);
            ctx.TypeMemberStack.Pop();
            return res;
        }

        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleField(CodeMemberField obj, Context ctx);
        private bool HandleDynamic(CodeMemberField obj, Context ctx)
        {
            ctx.TypeMemberStack.Push(MemberTypes.Field);
            var res = HandleField(obj, ctx);
            ctx.TypeMemberStack.Pop();
            return res;
        }

        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleMethod(CodeMemberMethod obj, Context ctx);
        private bool HandleDynamic(CodeMemberMethod obj, Context ctx)
        {
            ctx.TypeMemberStack.Push(MemberTypes.Method);
            var res = HandleMethod(obj, ctx);
            ctx.TypeMemberStack.Pop();
            return res;
        }
        
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleConstructor(CodeConstructor obj, Context ctx);
        private bool HandleDynamic(CodeConstructor obj, Context ctx)
        {
            ctx.TypeMemberStack.Push(MemberTypes.Constructor);
            var res = HandleConstructor(obj, ctx);
            ctx.TypeMemberStack.Pop();
            return res;
        }
        
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleTypeConstructor(CodeTypeConstructor obj, Context ctx);
        private bool HandleDynamic(CodeTypeConstructor obj, Context ctx)
        {
            ctx.TypeMemberStack.Push(MemberTypes.Method);
            var res = HandleTypeConstructor(obj, ctx);
            ctx.TypeMemberStack.Pop();
            return res;
        }

        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract bool HandleMain(CodeEntryPointMethod obj, Context ctx);
        private bool HandleDynamic(CodeEntryPointMethod obj, Context ctx)
        {
            ctx.TypeMemberStack.Push(MemberTypes.Method);
            var res = HandleMain(obj, ctx);
            ctx.TypeMemberStack.Pop();
            return res;
        }
        
        private bool HandleDynamic(CodeSnippetTypeMember obj, Context ctx)
        {
            GeneralUtils.HandleSnippet(obj.Text, ctx);
            ctx.Writer.NewLine();
            return true;
        }
    }
}