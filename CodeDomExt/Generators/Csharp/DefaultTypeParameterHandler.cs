using System;
using System.CodeDom;
using System.Linq;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Csharp
{
    /// <remarks>
    /// A type parameter handler should output the type parameter name or costraint on the current line, without indenting
    /// nor finishing with a new line or whitespace.
    /// <para/>
    /// Type parameter handling is split in two operations that will be called at different times: declaration and constraint.
    /// <para/>
    /// The operation that will be done is specified by <see cref="CSharpContext.TypeParameterHandlerRequestedOperation"/>
    /// </remarks>
    public class DefaultTypeParameterHandler : ICodeObjectHandler<CodeTypeParameter>
    {
        /// <inheritdoc/>
        public bool Handle(CodeTypeParameter obj, Context ctx)
        {
            if (ctx.CSharp.TypeParameterHandlerRequestedOperation == CSharpContext.TypeParameterHandlerOperations.Declaration)
            {
                //TODO struct and class type constraints, out and in
                if (obj.CustomAttributes.Count > 0)
                {
                    GeneralUtils.HandleCollection(obj.CustomAttributes.Cast<CodeAttributeDeclaration>(),
                        ctx.HandlerProvider.AttributeDeclarationHandler, ctx);
                    ctx.Writer.Write(" ");
                }
                ctx.Writer.Write(obj.Name.AsCsId());
            }
            else if (ctx.CSharp.TypeParameterHandlerRequestedOperation == CSharpContext.TypeParameterHandlerOperations.Constraint)
            {
                if (HasConstraints(obj))
                {
                    ctx.Writer.NewLine();
                    ctx.Writer.Indent(ctx);
                    ctx.Writer.Write($"where {obj.Name.AsCsId()} : ");

                    GeneralUtils.HandleCollectionCommaSeparated(obj.Constraints.Cast<CodeTypeReference>(),
                        ctx.HandlerProvider.TypeReferenceHandler, ctx);

                    if (obj.HasConstructorConstraint)
                    {
                        if (obj.Constraints.Count > 0)
                        {
                            ctx.Writer.Write(", ");
                        }

                        ctx.Writer.Write("new()");
                    }
                }
            }
            else
            {
                throw new InvalidOperationException();
            }

            return true;
        }

        private bool HasConstraints(CodeTypeParameter obj)
        {
            return obj.HasConstructorConstraint || obj.Constraints.Count > 0;
        }
    }
}