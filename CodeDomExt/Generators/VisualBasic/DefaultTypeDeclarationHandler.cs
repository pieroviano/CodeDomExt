using System;
using System.CodeDom;
using System.Linq;
using CodeDomExt.Nodes;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.VisualBasic
{
    /// <inheritdoc />
    public class DefaultTypeDeclarationHandler : Common.DefaultTypeDeclarationHandler
    {
        /// <inheritdoc />
        protected override void HandleTypeDeclaration(CodeTypeDeclaration obj, DeclarationType type, Context ctx)
        {
            if (obj.IsPartial && (obj.IsStruct || obj.IsInterface || obj.IsClass))
            {
                ctx.Writer.Write("Partial ");
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

            ctx.HandlerProvider.TypeAttributesHandler.Handle(obj.TypeAttributes, ctx);

            switch (type)
            {
                case DeclarationType.Class:
                    HandleClass(obj, ctx);
                    break;
                case DeclarationType.Struct:
                    HandleStruct(obj, ctx);
                    break;
                case DeclarationType.Interface:
                    HandleInterface(obj, ctx);
                    break;
                case DeclarationType.Enum:
                    HandleEnum(obj, ctx);
                    break;
                case DeclarationType.Delegate:
                    HandleDelegate((CodeTypeDelegate) obj, ctx);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <inheritdoc />
        protected override bool CanHandle(DeclarationType type)
        {
            return type == DeclarationType.Class || type == DeclarationType.Delegate || type == DeclarationType.Enum ||
                   type == DeclarationType.Interface || type == DeclarationType.Struct;
        }

        private void HandleClassOrStruct(CodeTypeDeclaration obj, Context ctx, bool isStruct)
        {
            ctx.Writer.Write((isStruct ? "Structure " : "Class ") + obj.Name.AsVbId());
            HandleTypeParameters(obj, ctx);
            
            if (obj.BaseTypes.Count > 0)
            {
                ctx.Indent();
                if (!isStruct && obj.BaseTypes[0].BaseType != typeof(object).FullName)
                {
                    ctx.Writer.NewLine();
                    ctx.Writer.IndentAndWrite("Inherits ", ctx);
                    ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.BaseTypes[0], ctx);
                }

                if (isStruct || obj.BaseTypes.Count > 1)
                {
                    ctx.Writer.NewLine();
                    ctx.Writer.IndentAndWrite("Implements ", ctx);
                    GeneralUtils.HandleCollectionCommaSeparated(
                        obj.BaseTypes.Cast<CodeTypeReference>().Skip(isStruct ? 0 : 1),
                        ctx.HandlerProvider.TypeReferenceHandler, ctx);
                }
                ctx.Writer.NewLine();
                ctx.Unindent();
            }
            
            HandleMembers(obj, ctx, isStruct ? BlockType.Structure : BlockType.Class);
        }

        private void HandleClass(CodeTypeDeclaration obj, Context ctx)
        {
            if (obj is CodeTypeDeclarationExt objExt && objExt.IsStatic)
            {
                HandleModule(obj, ctx);
            }
            else
            {
                HandleClassOrStruct(obj, ctx, false);
            }
        }

        private void HandleModule(CodeTypeDeclaration obj, Context ctx)
        {
            ctx.Writer.Write($"Module {obj.Name.AsVbId()}");
            HandleMembers(obj, ctx, BlockType.Module);
        }

        private void HandleStruct(CodeTypeDeclaration obj, Context ctx)
        {
            HandleClassOrStruct(obj, ctx, true);
        }

        private void HandleInterface(CodeTypeDeclaration obj, Context ctx)
        {
            ctx.Writer.Write($"Interface {obj.Name.AsVbId()}");
            HandleTypeParameters(obj, ctx);
            if (obj.BaseTypes.Count > 0)
            {
                ctx.Writer.NewLine();
                ctx.Indent();
                ctx.Writer.IndentAndWrite("Inherits ", ctx);
                GeneralUtils.HandleCollectionCommaSeparated(obj.BaseTypes.Cast<CodeTypeReference>(),
                        ctx.HandlerProvider.TypeReferenceHandler, ctx);
                ctx.Writer.NewLine();
                ctx.Unindent();
            }
            
            HandleMembers(obj, ctx, BlockType.Interface);
        }

        private void HandleEnum(CodeTypeDeclaration obj, Context ctx)
        {
            ConsistencyChecks.EnumConsistencyChecks(obj, ctx);

            ctx.Writer.Write($"Enum {obj.Name.AsVbId()}");
            VisualBasicUtils.BeginBlock(BlockType.Enum, ctx);
            GeneralUtils.HandleCollection(obj.Members.Cast<CodeTypeMember>(), ctx.HandlerProvider.TypeMemberHandler,
                ctx,
                preAction: (c) => c.Writer.Indent(c),
                postAction: (c) => c.Writer.NewLine(), doPostActionOnLast: true);
            VisualBasicUtils.EndBlock(ctx);
        }

        private void HandleDelegate(CodeTypeDelegate obj, Context ctx)
        {
            ctx.Writer.Write("Delegate ");
            bool isSub = GeneralUtils.IsNullOrVoidType(obj.ReturnType);
            ctx.Writer.Write(isSub ? "Sub " : "Function ");
            ctx.Writer.Write($"{obj.Name.AsVbId()}(");
            GeneralUtils.HandleCollectionCommaSeparated(obj.Parameters.Cast<CodeExpression>(),
                ctx.HandlerProvider.ExpressionHandler, ctx);
            ctx.Writer.Write(")");
            if (!isSub) {
                ctx.Writer.Write(" As ");
                ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.ReturnType, ctx);
            }
            ctx.Writer.NewLine();
        }
        
        private void HandleTypeParameters(CodeTypeDeclaration obj, Context ctx)
        {
            if (obj.TypeParameters.Count > 0)
            {
                ctx.Writer.Write("(Of ");
                GeneralUtils.HandleCollectionCommaSeparated(obj.TypeParameters.Cast<CodeTypeParameter>(), 
                    ctx.HandlerProvider.TypeParameterHandler, ctx);
                ctx.Writer.Write(")");
            }
        }

        private void HandleMembers(CodeTypeDeclaration obj, Context ctx, BlockType blockType)
        {
            VisualBasicUtils.BeginBlock(blockType, ctx);
            GeneralUtils.HandleCollection(obj.Members.Cast<CodeTypeMember>(), ctx.HandlerProvider.TypeMemberHandler,
                ctx,
                preAction: (c) => c.Writer.Indent(c),
                postAction: (c) => c.Writer.NewLine(), doPostActionOnLast: false);
            VisualBasicUtils.EndBlock(ctx);
        }
    }
}