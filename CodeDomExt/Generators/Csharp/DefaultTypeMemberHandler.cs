using System.CodeDom;
using System.Linq;
using CodeDomExt.Helpers;
using CodeDomExt.Nodes;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Csharp
{
    /// <inheritdoc/>
    public class DefaultTypeMemberHandler : Common.DefaultTypeMemberHandler
    {
        protected override bool CanHandleEvent => true;

        /// <inheritdoc/>
        protected override void HandleEvent(CodeMemberEvent obj, Context ctx)
        {
            if (obj.PrivateImplementationType == null)
            {
                ctx.HandlerProvider.MemberAttributesHandler.Handle(obj.Attributes, ctx);
            }
            ctx.Writer.Write("event ");
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.Type, ctx);
            ctx.Writer.Write(" ");
            if (obj.PrivateImplementationType == null)
            {
                ctx.Writer.Write(obj.Name.AsCsId());
            }
            else
            {
                HandlePrivateImplementationTypeMemberName(obj.Name, obj.PrivateImplementationType, ctx);
            }
            ctx.Writer.WriteLine(";");
        }

        protected override bool CanHandleProperty => true;

        /// <inheritdoc/>
        protected override void HandleProperty(CodeMemberProperty obj, Context ctx, 
            bool isExt, CodeMemberPropertyExt objExt, bool doDefaultImplementation)
        {
            if (obj.PrivateImplementationType == null)
            {
                ctx.HandlerProvider.MemberAttributesHandler.Handle(obj.Attributes, ctx);
            }
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.Type, ctx);
            ctx.Writer.Write(" ");
            if (obj.PrivateImplementationType == null)
            {
                ctx.Writer.Write(obj.Name.AsCsId());
            }
            else
            {
                HandlePrivateImplementationTypeMemberName(obj.Name, obj.PrivateImplementationType, ctx);
            }
            ctx.Writer.NewLine();
            ctx.Writer.IndentAndWriteLine("{", ctx);
            ctx.Indent();
            if (obj.HasGet)
            {
                ctx.Writer.Indent(ctx);
                if (isExt && objExt.GetAccessibilityLevel != obj.Attributes.GetAccessibilityLevel() &&
                    obj.PrivateImplementationType == null)
                {
                    ctx.Writer.Write($"{CSharpKeywordsUtils.AccessibilityLevelKeyword(objExt.GetAccessibilityLevel)} ");
                }
                ctx.Writer.Write("get");
                if (doDefaultImplementation)
                {
                    ctx.Writer.WriteLine(";");
                }
                else
                {
                    CSharpUtils.HandleStatementCollection(obj.GetStatements, ctx);
                }
            }
            if (obj.HasSet)
            {
                ctx.Writer.Indent(ctx);
                if (isExt && objExt.SetAccessibilityLevel != obj.Attributes.GetAccessibilityLevel() &&
                    obj.PrivateImplementationType == null)
                {
                    ctx.Writer.Write($"{CSharpKeywordsUtils.AccessibilityLevelKeyword(objExt.SetAccessibilityLevel)} ");
                }
                ctx.Writer.Write("set");
                if (doDefaultImplementation)
                {
                    ctx.Writer.WriteLine(";");
                }
                else
                {
                    CSharpUtils.HandleStatementCollection(obj.SetStatements, ctx);
                }
            }
            ctx.Unindent();
            ctx.Writer.IndentAndWrite("}", ctx);
            if (isExt && objExt.PropertyInitializer != null)
            {
                ctx.Writer.Write(" = ");
                ctx.HandlerProvider.ExpressionHandler.Handle(objExt.PropertyInitializer, ctx);
                ctx.Writer.Write(";");
            }
            ctx.Writer.NewLine();
        }

        protected override bool CanHandleField => true;

        /// <inheritdoc/>
        protected override void HandleField(CodeMemberField obj, Context ctx)
        {
            if (ctx.CurrentDeclarationType == DeclarationType.Enum)
            {
                ctx.Writer.Write(obj.Name.AsCsId());
            }
            else
            {
                ctx.HandlerProvider.MemberAttributesHandler.Handle(obj.Attributes, ctx);
                ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.Type, ctx);
                ctx.Writer.Write($" {obj.Name.AsCsId()}");
            }

            if (obj.InitExpression != null)
            {
                ctx.Writer.Write(" = ");
                ctx.HandlerProvider.ExpressionHandler.Handle(obj.InitExpression, ctx);
            }

            if (ctx.CurrentDeclarationType != DeclarationType.Enum)
            {
                ctx.Writer.WriteLine(";");
            }
        }

        protected override bool CanHandleMethod => true;

        private void HandleMethodParameters(CodeMemberMethod obj, Context ctx)
        {
            ctx.Writer.Write("(");
            GeneralUtils.HandleCollectionCommaSeparated(obj.Parameters.Cast<CodeParameterDeclarationExpression>(),
                ctx.HandlerProvider.ExpressionHandler, ctx);
            ctx.Writer.Write(")");
        }

        private void HandleMethodStatements(CodeMemberMethod obj, Context ctx)
        {
            CSharpUtils.HandleStatementCollection(obj.Statements, ctx);
        }
        /// <inheritdoc />
        protected override void HandleMethod(CodeMemberMethod obj, Context ctx)
        {
            if (GeneralUtils.IsNullOrVoidType(obj.PrivateImplementationType))
            {
                ctx.HandlerProvider.MemberAttributesHandler.Handle(obj.Attributes, ctx);
            }

            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.ReturnType, ctx);

            ctx.Writer.Write($" ");
            if (GeneralUtils.IsNullOrVoidType(obj.PrivateImplementationType))
            {
                ctx.Writer.Write(obj.Name.AsCsId());
            }
            else
            {
                HandlePrivateImplementationTypeMemberName(obj.Name, obj.PrivateImplementationType, ctx);
            }

            if (obj.TypeParameters.Count > 0)
            {
                ctx.CSharp.TypeParameterHandlerRequestedOperation = CSharpContext.TypeParameterHandlerOperations.Declaration;
                ctx.Writer.Write("<");
                GeneralUtils.HandleCollectionCommaSeparated(obj.TypeParameters.Cast<CodeTypeParameter>(),
                    ctx.HandlerProvider.TypeParameterHandler, ctx);
                ctx.Writer.Write(">");
            }

            HandleMethodParameters(obj, ctx);
            
            if (obj.TypeParameters.Count > 0)
            {
                ctx.CSharp.TypeParameterHandlerRequestedOperation = CSharpContext.TypeParameterHandlerOperations.Constraint;
                ctx.Indent();
                GeneralUtils.HandleCollection(obj.TypeParameters.Cast<CodeTypeParameter>(), ctx.HandlerProvider.TypeParameterHandler, ctx);
                ctx.Unindent();
            }

            if (ctx.CurrentDeclarationType == DeclarationType.Interface || ctx.IsMemberAbstract)
            {
                ctx.Writer.WriteLine(";");
            }
            else
            {
                HandleMethodStatements(obj, ctx);
            }
        }

        protected override bool CanHandleConstructor => true;

        /// <inheritdoc/>
        protected override void HandleConstructor(CodeConstructor obj, Context ctx)
        {
            //no return type, only access modifier
            ctx.HandlerProvider.MemberAttributesHandler.Handle(obj.Attributes, ctx);
            ctx.Writer.Write(ctx.CurrentCodeTypeDeclaration.Name.AsCsId());
            //no type parameters
            HandleMethodParameters(obj, ctx);
            if (obj.BaseConstructorArgs.Count > 0 || obj.ChainedConstructorArgs.Count > 0)
            {
                //TODO : this()
                ctx.Writer.NewLine();
                ctx.Indent();

                if (obj.BaseConstructorArgs.Count > 0)
                {
                    ctx.Writer.IndentAndWrite(": base(", ctx);
                    GeneralUtils.HandleCollectionCommaSeparated(obj.BaseConstructorArgs.Cast<CodeExpression>(),
                        ctx.HandlerProvider.ExpressionHandler, ctx);
                }
                else
                {
                    ctx.Writer.IndentAndWrite(": this(", ctx);
                    GeneralUtils.HandleCollectionCommaSeparated(obj.ChainedConstructorArgs.Cast<CodeExpression>(),
                        ctx.HandlerProvider.ExpressionHandler, ctx);
                }

                ctx.Writer.Write(")");

                ctx.Unindent();
            }

            HandleMethodStatements(obj, ctx);
        }

        protected override bool CanHandleTypeConstructor => true;

        /// <inheritdoc/>
        protected override void HandleTypeConstructor(CodeTypeConstructor obj, Context ctx)
        {
            ctx.Writer.Write($"static {ctx.CurrentCodeTypeDeclaration.Name.AsCsId()}()");
            HandleMethodStatements(obj, ctx);
        }

        protected override bool CanHandleEntryPoint => true;

        /// <inheritdoc/>
        protected override void HandleMain(CodeEntryPointMethod obj, Context ctx)
        {
            ctx.HandlerProvider.MemberAttributesHandler.Handle(
                GeneralUtils.GetMaskedMemberAttributes(obj.Attributes, true, false, false, true), ctx);
            ctx.Writer.Write("static ");
            //TODO async main
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.ReturnType, ctx);
            ctx.Writer.Write(" Main(string[] args)");
            HandleMethodStatements(obj, ctx);
        }
        
        private void HandlePrivateImplementationTypeMemberName(string memberName, CodeTypeReference privateImplementationType,
            Context ctx)
        {
            ctx.HandlerProvider.TypeReferenceHandler.Handle(privateImplementationType, ctx);
            ctx.Writer.Write("." + memberName);
        }
    }
}