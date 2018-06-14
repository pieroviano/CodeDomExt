using System;
using System.CodeDom;

namespace CodeDomExt.Generators.VisualBasic
{
    /// <inheritdoc />
    public class DefaultDirectiveHandler : Common.DefaultDirectiveHandler
    {
        /// <inheritdoc />
        protected override string GetRegionStartString(string regionText)
        {
            return $"#Region \"{regionText}\"";
        }

        /// <inheritdoc />
        protected override string GetRegionEndString()
        {
            return "#End Region";
        }
    }
}