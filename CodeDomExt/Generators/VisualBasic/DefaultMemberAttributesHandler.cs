using CodeDomExt.Helpers;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.VisualBasic
{
    /// <inheritdoc />
    public class DefaultMemberAttributesHandler : Common.DefaultMemberAttributesHandler
    {
        /// <inheritdoc />
        protected override string GetAccessibilityLevelKeyword(AccessibilityLevel accessibilityLevel, Context ctx)
        {
            return VisualBasicKeywordsUtils.AccessibilityLevelKeyword(accessibilityLevel);
        }
        /// <inheritdoc />
        protected override string GetNewKeyword(Context ctx)
        {
            return "Shadows";
        }
        /// <inheritdoc />
        protected override string GetAbstractKeyword(Context ctx)
        {
            return "MustOverride";
        }
        /// <inheritdoc />
        protected override string GetOverrideKeyword(Context ctx)
        {
            return "Overrides";
        }
        /// <inheritdoc />
        protected override string GetStaticKeyword(Context ctx)
        {
            //in c# static is required for static classes members, but in vb shared on module gives an error
            return ctx.VisualBasic.CurrentBlockType != BlockType.Module ? "Shared" : null;
        }
        /// <inheritdoc />
        protected override string GetConstKeyword(Context ctx)
        {
            return "Const";
        }
        /// <inheritdoc />
        protected override string GetFinalKeyword(Context ctx)
        {
            return null;
        }
        /// <inheritdoc />
        protected override string GetNotFinalKeyword(Context ctx)
        {
            return "Overridable";
        }
        /// <inheritdoc />
        protected override string GetOverloadedKeyword(Context ctx)
        {
            return "Overloads";
        }
    }
}