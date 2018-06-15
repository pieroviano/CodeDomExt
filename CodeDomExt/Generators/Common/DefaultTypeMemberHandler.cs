using System;
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
        private readonly bool _handleSnippet;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="handleSnippet"></param>
        protected DefaultTypeMemberHandler(bool handleSnippet)
        {
            _handleSnippet = handleSnippet;
        }
        
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected override bool DoDynamicHandle(CodeTypeMember obj, Context ctx)
        {   
            if (obj is CodeTypeDeclaration declaration)
            {
                ctx.HandlerProvider.TypeDeclarationHandler.Handle(declaration, ctx);
                return true;
            }
            
            //since i'm doing going to do some common handling i must be sure i can handle the whole object; checked in the single handledynamics 

            return HandleDynamic(obj as dynamic, ctx);
        }

        /// <summary>
        /// true if the implemented handler can handle event
        /// </summary>
        protected abstract bool CanHandleEvent { get; }
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract void HandleEvent(CodeMemberEvent obj, Context ctx);
        private bool HandleDynamic(CodeMemberEvent obj, Context ctx)
        {
            return HandleIfTrue(() =>
            {
                ctx.TypeMemberStack.Push(MemberTypes.Event);
                HandleEvent(obj, ctx);
                ctx.TypeMemberStack.Pop();
            }, obj, ctx, CanHandleEvent);
        }

        /// <summary>
        /// true if the implemented handler can handle event
        /// </summary>
        protected abstract bool CanHandleProperty { get; }
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract void HandleProperty(CodeMemberProperty obj, Context ctx, bool isExt, 
            CodeMemberPropertyExt objExt, bool doDefaultImplementation);
        private bool HandleDynamic(CodeMemberProperty obj, Context ctx)
        {
            return HandleIfTrue(() =>
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
                HandleProperty(obj, ctx, isExt, objExt, doDefaultImplementation);
                ctx.TypeMemberStack.Pop();
            }, obj, ctx, CanHandleProperty);
        }

        /// <summary>
        /// true if the implemented handler can handle event
        /// </summary>
        protected abstract bool CanHandleField { get; }
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract void HandleField(CodeMemberField obj, Context ctx);
        private bool HandleDynamic(CodeMemberField obj, Context ctx)
        {
            return HandleIfTrue(() =>
            {
                ctx.TypeMemberStack.Push(MemberTypes.Field);
                HandleField(obj, ctx);
                ctx.TypeMemberStack.Pop();
            }, obj, ctx, CanHandleField);
        }

        /// <summary>
        /// true if the implemented handler can handle event
        /// </summary>
        protected abstract bool CanHandleMethod { get; }
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract void HandleMethod(CodeMemberMethod obj, Context ctx);
        private bool HandleDynamic(CodeMemberMethod obj, Context ctx)
        {
            return HandleIfTrue(() =>
            {
                ctx.TypeMemberStack.Push(MemberTypes.Method);
                HandleMethod(obj, ctx);
                ctx.TypeMemberStack.Pop();
            }, obj, ctx, CanHandleMethod);
        }
        
        /// <summary>
        /// true if the implemented handler can handle event
        /// </summary>
        protected abstract bool CanHandleConstructor { get; }
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract void HandleConstructor(CodeConstructor obj, Context ctx);
        private bool HandleDynamic(CodeConstructor obj, Context ctx)
        {
            return HandleIfTrue(() =>
            {
                ctx.TypeMemberStack.Push(MemberTypes.Constructor);
                HandleConstructor(obj, ctx);
                ctx.TypeMemberStack.Pop();
            }, obj, ctx, CanHandleConstructor);
        }
        
        /// <summary>
        /// true if the implemented handler can handle event
        /// </summary>
        protected abstract bool CanHandleTypeConstructor { get; }
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract void HandleTypeConstructor(CodeTypeConstructor obj, Context ctx);
        private bool HandleDynamic(CodeTypeConstructor obj, Context ctx)
        {
            return HandleIfTrue(() =>
            {
                ctx.TypeMemberStack.Push(MemberTypes.Method);
                HandleTypeConstructor(obj, ctx);
                ctx.TypeMemberStack.Pop();
            }, obj, ctx, CanHandleTypeConstructor);
        }

        /// <summary>
        /// true if the implemented handler can handle event
        /// </summary>
        protected abstract bool CanHandleEntryPoint { get; }
        /// <inheritdoc cref="ICodeObjectHandler{T}.Handle"/>
        protected abstract void HandleMain(CodeEntryPointMethod obj, Context ctx);
        private bool HandleDynamic(CodeEntryPointMethod obj, Context ctx)
        {
            return HandleIfTrue(() =>
            {
                ctx.TypeMemberStack.Push(MemberTypes.Method);
                HandleMain(obj, ctx);
                ctx.TypeMemberStack.Pop();
            }, obj, ctx, CanHandleEntryPoint);
        }
        
        private bool HandleDynamic(CodeSnippetTypeMember obj, Context ctx)
        {
            return HandleIfTrue(() =>
            {
                GeneralUtils.HandleSnippet(obj.Text, ctx);
                ctx.Writer.NewLine();
            }, obj, ctx, _handleSnippet);
        }

        private bool HandleIfTrue(Action handle, CodeTypeMember obj, Context ctx, bool condition)
        {
            if (condition)
            {
                //handle start directives
                GeneralUtils.HandleCollectionOnMultipleLines(obj.StartDirectives.Cast<CodeDirective>(),
                    ctx.HandlerProvider.DirectiveHandler, ctx, false);
                //handle comment
                foreach (CodeCommentStatement comment in obj.Comments)
                {
                    ctx.HandlerProvider.StatementHandler.Handle(comment, ctx);
                    ctx.Writer.Indent(ctx);
                }
                //handle attributes
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
                //handle member
                handle();
                //handle end directives
                GeneralUtils.HandleCollectionOnMultipleLines(obj.EndDirectives.Cast<CodeDirective>(),
                    ctx.HandlerProvider.DirectiveHandler, ctx, true);
            }

            return condition;
        }        
    }
}