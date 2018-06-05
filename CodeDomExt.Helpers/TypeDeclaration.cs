using System.CodeDom;
using System.Reflection;

namespace CodeDomExt.Helpers
{
    public static class TypeDeclaration
    {
        public static void SetAccessibilityLevel(this CodeTypeDeclaration self, AccessibilityLevel accessibilityLevel)
        {
            self.TypeAttributes = (self.TypeAttributes & ~TypeAttributes.VisibilityMask) | (accessibilityLevel.GetTypeAttribute());
        }
    }
}