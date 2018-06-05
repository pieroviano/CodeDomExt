namespace CodeDomExt.Generators.VisualBasic
{
    /// <inheritdoc />
    public class DefaultCommentHandler : Common.DefaultCommentHandler
    {
        /// <inheritdoc />
        protected override string SingleLineCommentPrefix { get; } = "'";
        /// <inheritdoc />
        protected override string SingleLineDocCommentPrefix { get; } = "''' ";
    }
}