using System;
using System.CodeDom;
using System.Reflection;

namespace CodeDomExt.Helpers
{
    /// <summary>
    /// Possible accessibility levels
    /// </summary>
    public enum AccessibilityLevel
    {
#pragma warning disable 1591
        Public,
        Protected,
        Internal,
        ProtectedInternal,
        Private,
        PrivateProtected,
        Default
#pragma warning restore 1591
    }

    /// <summary>
    /// Utility class for accessibility level
    /// </summary>
    public static class AccessibilityLevelUtils
    {
        /// <summary>
        /// Returns the accessibility level
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static AccessibilityLevel GetAccessibilityLevel(this TypeAttributes attr)
        {
            switch (TypeAttributes.VisibilityMask & attr)
            {
                case TypeAttributes.Public:
                case TypeAttributes.NestedPublic:
                    return AccessibilityLevel.Public;
                case TypeAttributes.NotPublic:
                case TypeAttributes.NestedAssembly:
                    return AccessibilityLevel.Internal;
                case TypeAttributes.NestedPrivate:
                    return AccessibilityLevel.Private;
                case TypeAttributes.NestedFamily:
                    return AccessibilityLevel.Protected;
                case TypeAttributes.NestedFamANDAssem:
                    return AccessibilityLevel.PrivateProtected;
                case TypeAttributes.NestedFamORAssem:
                    return AccessibilityLevel.ProtectedInternal;
            }

            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Returns the type attribute visibility flag for the correponding accessibility level
        /// </summary>
        /// <param name="accessibilityLevel"></param>
        /// <returns></returns>
        public static TypeAttributes GetTypeAttribute(this AccessibilityLevel accessibilityLevel)
        {
            switch (accessibilityLevel)
            {
                case AccessibilityLevel.Public:
                    return TypeAttributes.Public;
                case AccessibilityLevel.Protected:
                    return TypeAttributes.NestedFamily;
                case AccessibilityLevel.ProtectedInternal:
                    return TypeAttributes.NestedFamORAssem;
                case AccessibilityLevel.Private:
                    return TypeAttributes.NestedPrivate;
                case AccessibilityLevel.PrivateProtected:
                    return TypeAttributes.NestedFamANDAssem;
                case AccessibilityLevel.Internal:
                case AccessibilityLevel.Default:
                    return TypeAttributes.NotPublic;
                default:
                    throw new ArgumentOutOfRangeException(nameof(accessibilityLevel), accessibilityLevel, null);
            }
        }

        /// <summary>
        /// Returns the accessibility level
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static AccessibilityLevel GetAccessibilityLevel(this MemberAttributes attr)
        {
            switch (MemberAttributes.AccessMask & attr)
            {
                case MemberAttributes.Public:
                    return AccessibilityLevel.Public;
                case MemberAttributes.Assembly:
                    return AccessibilityLevel.Internal;
                case MemberAttributes.Private:
                    return AccessibilityLevel.Private;
                case MemberAttributes.Family:
                    return AccessibilityLevel.Protected;
                case MemberAttributes.FamilyAndAssembly:
                    return AccessibilityLevel.PrivateProtected;
                case MemberAttributes.FamilyOrAssembly:
                    return AccessibilityLevel.ProtectedInternal;
                case 0:
                    return AccessibilityLevel.Default;
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}