using System;

namespace CodeDomExt.Generators.Csharp
{
    /// <inheritdoc/>
    public class DefaultTypeReferenceHandler : Common.DefaultTypeReferenceHandler
    {
        /// <inheritdoc/>
        protected override void WrapTypeArguments(Action<Context> typeArgumentWriteAction, Context ctx)
        {
            ctx.Writer.Write("<");
            typeArgumentWriteAction(ctx);
            ctx.Writer.Write(">");
        }
        /// <inheritdoc/>
        protected override void WrapArrayIndexer(Action<Context> arrayIndexerArgumentsWriteAction, Context ctx)
        {
            ctx.Writer.Write("[");
            arrayIndexerArgumentsWriteAction(ctx);
            ctx.Writer.Write("]");
        }
        /// <inheritdoc/>
        protected override string GetTypeKeywordString(Type baseType)
        {
            return CSharpKeywordsUtils.GetKeywordFromType(baseType);
        }
        /// <inheritdoc/>
        protected override string AsId(string s)
        {
            return s.AsCsId();
        }
        /// <inheritdoc/>
        protected override string AsValidNamespace(string s)
        {
            return CSharpUtils.GetValidNamespaceIdentifier(s);
        }
        /// <inheritdoc/>
        public DefaultTypeReferenceHandler() : base(true)
        {
        }
    }
}