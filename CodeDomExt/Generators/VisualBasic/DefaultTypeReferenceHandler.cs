using System;

namespace CodeDomExt.Generators.VisualBasic
{
    /// <inheritdoc />
    public class DefaultTypeReferenceHandler : Common.DefaultTypeReferenceHandler
    {
        /// <inheritdoc />
        protected override void WrapTypeArguments(Action<Context> typeArgumentWriteAction, Context ctx)
        {
            ctx.Writer.Write("(Of ");
            typeArgumentWriteAction(ctx);
            ctx.Writer.Write(")");
        }
        /// <inheritdoc />
        protected override void WrapArrayIndexer(Action<Context> arrayIndexerArgumentsWriteAction, Context ctx)
        {
            ctx.Writer.Write("(");
            arrayIndexerArgumentsWriteAction(ctx);
            ctx.Writer.Write(")");
        }
        /// <inheritdoc />
        protected override string GetTypeKeywordString(Type baseType)
        {
            return VisualBasicKeywordsUtils.GetKeywordFromType(baseType);
        }
        /// <inheritdoc />
        protected override string AsId(string s)
        {
            return s.AsVbId();
        }
        /// <inheritdoc />
        protected override string AsValidNamespace(string s)
        {
            return VisualBasicUtils.GetValidNamespaceIdentifier(s);
        }
        /// <inheritdoc />
        public DefaultTypeReferenceHandler() : base(true)
        {
        }
    }
}