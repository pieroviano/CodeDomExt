using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using CodeDomExt.Nodes;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Csharp
{
    /// <inheritdoc/>
    public class DefaultTypeDeclarationHandler : Common.DefaultTypeDeclarationHandler
    {
        /// <inheritdoc/>
        protected override bool HandleTypeDeclaration(CodeTypeDeclaration obj, DeclarationType type, Context ctx)
        {
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

            return true;
        }
        
        private void HandleClassOrStruct(CodeTypeDeclaration obj, Context ctx, bool isStruct)
        {
            if (!isStruct && obj is CodeTypeDeclarationExt objExt && objExt.IsStatic)
            {
                ctx.Writer.Write("static ");
            }
            if (obj.IsPartial)
            {
                ctx.Writer.Write("partial ");
            }
            ctx.Writer.Write((isStruct ? "struct " : "class ") + obj.Name.AsCsId());
            HandleTypeParametersDeclaration(obj, ctx);
            HandleBaseTypes(obj, ctx);
            HandleTypeParametersConstraints(obj, ctx);
            ctx.Writer.NewLine();

            HandleMembers(obj, ctx);
        }

        private void HandleClass(CodeTypeDeclaration obj, Context ctx)
        {
            HandleClassOrStruct(obj, ctx, false);
        }

        private void HandleStruct(CodeTypeDeclaration obj, Context ctx)
        {
            HandleClassOrStruct(obj, ctx, true);
        }

        private void HandleInterface(CodeTypeDeclaration obj, Context ctx)
        {
            if (obj.IsPartial)
            {
                ctx.Writer.Write("partial ");
            }

            ctx.Writer.Write($"interface {obj.Name.AsCsId()}");
            HandleTypeParametersDeclaration(obj, ctx);
            HandleBaseTypes(obj, ctx);
            HandleTypeParametersConstraints(obj, ctx);
            ctx.Writer.NewLine();
            HandleMembers(obj, ctx);
        }

        private void HandleEnum(CodeTypeDeclaration obj, Context ctx)
        {
            ConsistencyChecks.EnumConsistencyChecks(obj, ctx);

            ctx.Writer.WriteLine($"enum {obj.Name.AsCsId()}");
            ctx.Writer.IndentAndWriteLine("{", ctx);
            ctx.Indent();
            GeneralUtils.HandleCollection(obj.Members.Cast<CodeTypeMember>(), ctx.HandlerProvider.TypeMemberHandler,
                ctx,
                preAction: (c) => c.Writer.Indent(c),
                postAction: (c) => c.Writer.WriteLine(","), doPostActionOnLast: false);
            ctx.Writer.NewLine();
            ctx.Unindent();
            ctx.Writer.IndentAndWriteLine("}", ctx);
        }

        private void HandleDelegate(CodeTypeDelegate obj, Context ctx)
        {
            ctx.Writer.Write("delegate ");
            ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.ReturnType, ctx);
            ctx.Writer.Write($" {obj.Name.AsCsId()}(");
            GeneralUtils.HandleCollectionCommaSeparated(obj.Parameters.Cast<CodeExpression>(),
                ctx.HandlerProvider.ExpressionHandler, ctx);
            ctx.Writer.WriteLine(");");
        }
        
        
        private void HandleBaseTypes(CodeTypeDeclaration obj, Context ctx)
        {
            if (obj.BaseTypes.Count > 0)
            {
                LinkedList<CodeTypeReference> baseTypes =
                    new LinkedList<CodeTypeReference>(obj.BaseTypes.Cast<CodeTypeReference>());
                
                if (baseTypes.First.Value.BaseType == typeof(object).FullName) //In order to avoid class Class : object, IInterface
                {
                    baseTypes.RemoveFirst();
                }

                if (baseTypes.Count > 0)
                {
                    ctx.Indent();
                    ctx.Writer.NewLine();
                    ctx.Writer.IndentAndWrite(": ", ctx);
                    GeneralUtils.HandleCollectionCommaSeparated(obj.BaseTypes.Cast<CodeTypeReference>(),
                        ctx.HandlerProvider.TypeReferenceHandler, ctx);
                    ctx.Unindent();
                }
            }
        }

        private void HandleTypeParametersDeclaration(CodeTypeDeclaration obj, Context ctx)
        {
            if (obj.TypeParameters.Count > 0)
            {
                ctx.CSharp.TypeParameterHandlerRequestedOperation = CSharpContext.TypeParameterHandlerOperations.Declaration;
                ctx.Writer.Write("<");
                GeneralUtils.HandleCollection(obj.TypeParameters.Cast<CodeTypeParameter>(),
                    ctx.HandlerProvider.TypeParameterHandler, ctx,
                    postAction: (c) => c.Writer.Write(", "), doPostActionOnLast: false);
                ctx.Writer.Write(">");
            }
        }

        private void HandleTypeParametersConstraints(CodeTypeDeclaration obj, Context ctx)
        {
            if (obj.TypeParameters.Count > 0)
            {
                ctx.Indent();
                ctx.CSharp.TypeParameterHandlerRequestedOperation = CSharpContext.TypeParameterHandlerOperations.Constraint;
                GeneralUtils.HandleCollection(obj.TypeParameters.Cast<CodeTypeParameter>(),
                    ctx.HandlerProvider.TypeParameterHandler, ctx);
                ctx.Unindent();
            }
        }

        private void HandleMembers(CodeTypeDeclaration obj, Context ctx)
        {
            ctx.Writer.IndentAndWriteLine("{", ctx);

            ctx.Indent();
            GeneralUtils.HandleCollection(obj.Members.Cast<CodeTypeMember>(), ctx.HandlerProvider.TypeMemberHandler,
                ctx,
                preAction: (c) => c.Writer.Indent(c),
                postAction: (c) => c.Writer.NewLine(), doPostActionOnLast: false);
            ctx.Unindent();
            ctx.Writer.IndentAndWriteLine("}", ctx);
        }

    }
}