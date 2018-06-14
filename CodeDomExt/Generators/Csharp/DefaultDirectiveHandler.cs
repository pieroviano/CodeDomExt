namespace CodeDomExt.Generators.Csharp
{
    /// <inheritdoc />
    public class DefaultDirectiveHandler : Common.DefaultDirectiveHandler
    {
        /// <inheritdoc />
        protected override string GetRegionStartString(string regionText)
        {
            return $"#region {regionText}";
        }

        /// <inheritdoc />
        protected override string GetRegionEndString()
        {
            return "#endregion";
        }
    }
}