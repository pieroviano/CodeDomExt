using CodeDomExt.Helpers;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Csharp
{
    /// <inheritdoc/>
    public class DefaultTypeAttributesHandler : Common.DefaultTypeAttributesHandler
    {
        /// <inheritdoc/>
        protected override string GetAccessibilityLevelKeyword(AccessibilityLevel accessibilityLevel, Context ctx)
        {
            return CSharpKeywordsUtils.AccessibilityLevelKeyword(accessibilityLevel);
        }
        /// <inheritdoc/>
        protected override string GetSealedKeyword(Context ctx)
        {
            return "sealed";
        }
        /// <inheritdoc/>
        protected override string GetAbstractKeyword(Context ctx)
        {
            return "abstract";
        }
    }
}