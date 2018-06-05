using System.CodeDom;

namespace CodeDomExt.Utils
{
    /// <summary>
    /// The possible types of a <see cref="CodeTypeDeclaration"/>
    /// </summary>
    public enum DeclarationType
    {
#pragma warning disable 1591
        Class,
        Struct,
        Interface,
        Enum,
        Delegate
#pragma warning restore 1591
    }
}