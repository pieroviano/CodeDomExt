using System.CodeDom;
using System.Linq;
using CodeDomExt.Helpers;
using CodeDomExt.Nodes;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.VisualBasic
{
    /// <inheritdoc />
    public class DefaultTypeMemberHandler : Common.DefaultTypeMemberHandler
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DefaultTypeMemberHandler() : base(true)
        {
        }
        
        protected override bool CanHandleEvent => true;

        /// <inheritdoc />
        protected override void HandleEvent(CodeMemberEvent obj, Context ctx)
        {
            if (GeneralUtils.IsNullOrVoidType(obj.PrivateImplementationType))
            {
                ctx.HandlerProvider.MemberAttributesHandler.Handle(obj.Attributes, ctx);
            }
            ctx.Writer.Write("Event ");
            if (GeneralUtils.IsNullOrVoidType(obj.PrivateImplementationType))
            {
                ctx.Writer.Write(obj.Name.AsVbId());
            }
            else
            {
                HandlePrivateImplementationTypeMemberName(obj.Name, obj.PrivateImplementationType, ctx);
            }
            ctx.Writer.Write(" As ");
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.Type, ctx);
            HandleImplementationTypes(obj.ImplementationTypes, obj.Name, ctx);
            HandlePrivateImplType(obj.PrivateImplementationType, obj.Name, ctx);
            ctx.Writer.NewLine();
        }

        protected override bool CanHandleProperty => true;

        /// <inheritdoc />
        protected override void HandleProperty(CodeMemberProperty obj, Context ctx, bool isExt, 
            CodeMemberPropertyExt objExt, bool doDefaultImplementation)
        {

            if (ctx.Options.DoConsistencyChecks)
            {
                if (!doDefaultImplementation && isExt && objExt.PropertyInitializer != null)
                {
                    throw new ConsistencyException($"Property {obj.Name} can't have initializer if it's not auto-property");
                }
            }

            if (GeneralUtils.IsNullOrVoidType(obj.PrivateImplementationType))
            {
                ctx.HandlerProvider.MemberAttributesHandler.Handle(obj.Attributes, ctx);
            }
  
            if (obj.HasGet && !obj.HasSet)
            {
                ctx.Writer.Write("ReadOnly ");
            }
            else if (obj.HasSet && !obj.HasGet)
            {
                ctx.Writer.Write("WriteOnly ");
            }

            ctx.Writer.Write($"Property ");
            if (GeneralUtils.IsNullOrVoidType(obj.PrivateImplementationType))
            {
                ctx.Writer.Write(obj.Name.AsVbId());
            }
            else
            {
                HandlePrivateImplementationTypeMemberName(obj.Name, obj.PrivateImplementationType, ctx);
            }
            ctx.Writer.Write(" As ");
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.Type, ctx);
            if (doDefaultImplementation)
            {
                if (isExt && objExt.PropertyInitializer != null)
                {
                    ctx.Writer.Write(" = ");
                    ctx.HandlerProvider.ExpressionHandler.Handle(objExt.PropertyInitializer, ctx);
                }
                HandleImplementationTypes(obj.ImplementationTypes, obj.Name, ctx);
                HandlePrivateImplType(obj.PrivateImplementationType, obj.Name, ctx);
                ctx.Writer.NewLine();
            }
            else
            {
                HandleImplementationTypes(obj.ImplementationTypes, obj.Name, ctx);
                HandlePrivateImplType(obj.PrivateImplementationType, obj.Name, ctx);
                VisualBasicUtils.BeginBlock(BlockType.Property, ctx);
                
                if (obj.HasGet)
                {
                    ctx.Writer.Indent(ctx);
                    if (isExt && objExt.GetAccessibilityLevel != obj.Attributes.GetAccessibilityLevel())
                    {
                        ctx.Writer.Write($"{VisualBasicKeywordsUtils.AccessibilityLevelKeyword(objExt.GetAccessibilityLevel)} ");
                    }
                    ctx.Writer.Write("Get");
                    VisualBasicUtils.HandleStatementCollection(obj.GetStatements, ctx, BlockType.Get);
                }

                if (obj.HasSet)
                {
                    ctx.Writer.Indent(ctx);
                    if (isExt && objExt.SetAccessibilityLevel != obj.Attributes.GetAccessibilityLevel())
                    {
                        ctx.Writer.Write($"{VisualBasicKeywordsUtils.AccessibilityLevelKeyword(objExt.SetAccessibilityLevel)} ");
                    }
                    ctx.Writer.Write("Set");
                    VisualBasicUtils.HandleStatementCollection(obj.SetStatements, ctx, BlockType.Set);
                }
                
                VisualBasicUtils.EndBlock(ctx);
            }
        }

        protected override bool CanHandleField => true;

        /// <inheritdoc />
        protected override void HandleField(CodeMemberField obj, Context ctx)
        {
            if (ctx.CurrentDeclarationType == DeclarationType.Enum)
            {
                ctx.Writer.Write(obj.Name.AsVbId());
            }
            else
            {
                ctx.HandlerProvider.MemberAttributesHandler.Handle(obj.Attributes, ctx);
                ctx.Writer.Write($"{obj.Name.AsVbId()} As ");
                ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.Type, ctx);
            }

            if (obj.InitExpression != null)
            {
                ctx.Writer.Write(" = ");
                ctx.HandlerProvider.ExpressionHandler.Handle(obj.InitExpression, ctx);
            }
            
            if (ctx.CurrentDeclarationType != DeclarationType.Enum)
            {
                ctx.Writer.NewLine();
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

        private void HandleMethodStatements(CodeMemberMethod obj, Context ctx, bool isSub)
        {
            VisualBasicUtils.HandleStatementCollection(obj.Statements, ctx, isSub ? BlockType.Sub : BlockType.Function);
        }
        /// <inheritdoc />
        protected override void HandleMethod(CodeMemberMethod obj, Context ctx)
        {
            if (GeneralUtils.IsNullOrVoidType(obj.PrivateImplementationType))
            {
                ctx.HandlerProvider.MemberAttributesHandler.Handle(obj.Attributes, ctx);
            }
            
            bool isSub = GeneralUtils.IsNullOrVoidType(obj.ReturnType);
            ctx.Writer.Write(isSub ? "Sub " : "Function ");

            if (GeneralUtils.IsNullOrVoidType(obj.PrivateImplementationType))
            {
                ctx.Writer.Write(obj.Name.AsVbId());
            }
            else
            {
                HandlePrivateImplementationTypeMemberName(obj.Name, obj.PrivateImplementationType, ctx);
            }

            if (obj.TypeParameters.Count > 0)
            {
                ctx.Writer.Write("(Of ");
                GeneralUtils.HandleCollectionCommaSeparated(obj.TypeParameters.Cast<CodeTypeParameter>(),
                    ctx.HandlerProvider.TypeParameterHandler, ctx);
                ctx.Writer.Write(")");
            }

            HandleMethodParameters(obj, ctx);

            if (!isSub)
            {
                ctx.Writer.Write(" As ");
                ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.ReturnType, ctx);
            }
            
            HandleImplementationTypes(obj.ImplementationTypes, obj.Name, ctx);
            HandlePrivateImplType(obj.PrivateImplementationType, obj.Name, ctx);

            if (ctx.CurrentDeclarationType != DeclarationType.Interface && !ctx.IsMemberAbstract)
            {
                HandleMethodStatements(obj, ctx, isSub);
            }
            else
            {
                ctx.Writer.NewLine();
            }
        }

        protected override bool CanHandleConstructor => true;

        /// <inheritdoc />
        protected override void HandleConstructor(CodeConstructor obj, Context ctx)
        {
            //no return type, only access modifier
            ctx.HandlerProvider.MemberAttributesHandler.Handle(obj.Attributes, ctx);
            ctx.Writer.Write("Sub New");
            //no type parameters
            HandleMethodParameters(obj, ctx);
            VisualBasicUtils.BeginBlock(BlockType.Sub, ctx);
            if (obj.BaseConstructorArgs.Count > 0 || obj.ChainedConstructorArgs.Count > 0)
            {
                //TODO : this()

                if (obj.BaseConstructorArgs.Count > 0)
                {
                    ctx.Writer.IndentAndWrite("MyBase.New(", ctx);
                    GeneralUtils.HandleCollectionCommaSeparated(obj.BaseConstructorArgs.Cast<CodeExpression>(),
                        ctx.HandlerProvider.ExpressionHandler, ctx);
                }
                else
                {
                    ctx.Writer.IndentAndWrite("Me.New(", ctx);
                    GeneralUtils.HandleCollectionCommaSeparated(obj.ChainedConstructorArgs.Cast<CodeExpression>(),
                        ctx.HandlerProvider.ExpressionHandler, ctx);
                }

                ctx.Writer.WriteLine(")");
            }
            VisualBasicUtils.HandleStatementCollection(obj.Statements, ctx);
            VisualBasicUtils.EndBlock(ctx);
        }

        protected override bool CanHandleTypeConstructor => true;

        /// <inheritdoc />
        protected override void HandleTypeConstructor(CodeTypeConstructor obj, Context ctx)
        {
            ctx.Writer.Write("Shared Sub New()");
            HandleMethodStatements(obj, ctx, true);
        }

        protected override bool CanHandleEntryPoint => true;

        /// <inheritdoc />
        protected override void HandleMain(CodeEntryPointMethod obj, Context ctx)
        {
            ctx.Writer.Write("Public ");
            if (ctx.VisualBasic.CurrentBlockType != BlockType.Module)
            {
                ctx.Writer.Write("Shared ");
            }
            bool isSub = GeneralUtils.IsNullOrVoidType(obj.ReturnType);
            
            ctx.Writer.Write((isSub ? "Sub" : "Function") + " Main(ByVal cmdArgs() As String)");

            if (!isSub)
            {
                ctx.Writer.Write(" As ");
                ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.ReturnType, ctx);
            }
            
            HandleMethodStatements(obj, ctx, isSub);
        }
        
        private void HandleImplementationTypes(CodeTypeReferenceCollection implementationTypes, string name, Context ctx)
        {
            if (implementationTypes.Count > 0)
            {
                ctx.Writer.Write(" Implements ");
                for (int i = 0; i < implementationTypes.Count; i++)
                {
                    ctx.HandlerProvider.TypeReferenceHandler.Handle(implementationTypes[i], ctx);
                    ctx.Writer.Write($".{name}");
                    
                }
            }
        }

        private void HandlePrivateImplType(CodeTypeReference privateImplType, string name, Context ctx)
        {
            if (!GeneralUtils.IsNullOrVoidType(privateImplType))
            {
                ctx.Writer.Write(" Implements ");
                ctx.HandlerProvider.TypeReferenceHandler.Handle(privateImplType, ctx);
                ctx.Writer.Write($".{name}");
            }
        }
        
        private void HandlePrivateImplementationTypeMemberName(string memberName, CodeTypeReference privateImplementationType,
            Context ctx)
        {
            ctx.Writer.Write(privateImplementationType.BaseType.StripGenericTypeArgumentsNumber().Replace(".", "_") +
                             "_" + memberName);
        }
    }
}