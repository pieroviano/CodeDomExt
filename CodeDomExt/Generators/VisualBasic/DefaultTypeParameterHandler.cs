using System.CodeDom;
using System.Linq;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.VisualBasic
{
    /// <remarks>
    /// A type parameter handler should output the type parameter name or costraint on the current line, without indenting
    /// nor finishing with a new line or whitespace.
    /// </remarks>
    public class DefaultTypeParameterHandler : ICodeObjectHandler<CodeTypeParameter>
    {
        /// <inheritdoc />
        public bool Handle(CodeTypeParameter obj, Context ctx)
        {
            if (ctx.Options.DoConsistencyChecks && obj.CustomAttributes.Count > 0)
            {
                throw new ConsistencyException($"Type parameter {obj.Name}: VB does not support custom attributes on type parameters");
            }
            //TODO struct and class type constraints, out and in
            ctx.Writer.Write(obj.Name.AsVbId());
            if (obj.Constraints.Count > 0 || obj.HasConstructorConstraint)
            {
                bool needsBlock = obj.Constraints.Count + (obj.HasConstructorConstraint ? 1 : 0) > 1;
                ctx.Writer.Write(" As ");
                if (needsBlock)
                {
                    ctx.Writer.Write("{");
                }
                GeneralUtils.HandleCollectionCommaSeparated(obj.Constraints.Cast<CodeTypeReference>(), 
                    ctx.HandlerProvider.TypeReferenceHandler, ctx);
                if (obj.HasConstructorConstraint)
                {
                    if (obj.Constraints.Count > 0)
                    {
                        ctx.Writer.Write(", ");
                    }
                    ctx.Writer.Write("New");
                }
                if (needsBlock)
                {
                    ctx.Writer.Write("}");
                }
            }
            return true;
        }
    }
}