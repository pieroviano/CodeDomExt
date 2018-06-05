using System;

namespace CodeDomExt.Generators.Csharp
{
    /// <inheritdoc/>
    public class DefaultAttributeDeclarationHandler : Common.DefaultAttributeDeclarationHandler
    {
        /// <inheritdoc/>
        protected override void WrapAttributeDeclaration(Action<Context> attributeHandlingAction, Context ctx)
        {
            ctx.Writer.Write("[");
            attributeHandlingAction(ctx);
            ctx.Writer.Write("]");
        }

        /// <inheritdoc/>
        protected override void WrapAttributeParameters(Action<Context> attributeParametersHandlingAction, Context ctx)
        {
            ctx.Writer.Write("(");
            attributeParametersHandlingAction(ctx);
            ctx.Writer.Write(")");
        }

        /// <inheritdoc/>
        protected override string AsId(string s)
        {
            return s.AsCsId();
        }

        /// <inheritdoc/>
        public DefaultAttributeDeclarationHandler() : base("=")
        {
        }
    }
}