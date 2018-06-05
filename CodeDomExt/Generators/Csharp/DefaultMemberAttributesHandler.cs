using CodeDomExt.Helpers;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Csharp
{    /// <inheritdoc/>
    public class DefaultMemberAttributesHandler : Common.DefaultMemberAttributesHandler
    {
        /// <inheritdoc/>
        protected override string GetAccessibilityLevelKeyword(AccessibilityLevel accessibilityLevel, Context ctx)
        {
            return CSharpKeywordsUtils.AccessibilityLevelKeyword(accessibilityLevel);
        }
        /// <inheritdoc/>
        protected override string GetNewKeyword(Context ctx)
        {
            return "new";
        }
        /// <inheritdoc/>
        protected override string GetAbstractKeyword(Context ctx)
        {
            return "abstract";
        }
        /// <inheritdoc/>
        protected override string GetOverrideKeyword(Context ctx)
        {
            return "override";
        }
        /// <inheritdoc/>
        protected override string GetStaticKeyword(Context ctx)
        {
            return "static";
        }
        /// <inheritdoc/>
        protected override string GetConstKeyword(Context ctx)
        {
            return "const";
        }
        /// <inheritdoc/>
        protected override string GetFinalKeyword(Context ctx)
        {
            return null;
        }
        /// <inheritdoc/>
        protected override string GetNotFinalKeyword(Context ctx)
        {
            return "virtual";
        }
        /// <inheritdoc/>
        protected override string GetOverloadedKeyword(Context ctx)
        {
            return null;
        }
    }
}