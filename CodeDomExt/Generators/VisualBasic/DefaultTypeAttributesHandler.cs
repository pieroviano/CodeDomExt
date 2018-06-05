using CodeDomExt.Helpers;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.VisualBasic
{
    /// <inheritdoc />
    public class DefaultTypeAttributesHandler : Common.DefaultTypeAttributesHandler
    {
        /// <inheritdoc />
        protected override string GetAccessibilityLevelKeyword(AccessibilityLevel accessibilityLevel, Context ctx)
        {
            return VisualBasicKeywordsUtils.AccessibilityLevelKeyword(accessibilityLevel);
        }
        /// <inheritdoc />
        protected override string GetSealedKeyword(Context ctx)
        {
            return "NotInheritable";
        }
        /// <inheritdoc />
        protected override string GetAbstractKeyword(Context ctx)
        {
            return "MustInherit";
        }
    }
}