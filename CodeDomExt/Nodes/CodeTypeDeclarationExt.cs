using System.CodeDom;

namespace CodeDomExt.Nodes
{
    /// <summary>
    /// Extension for CodeTypeDeclaration
    /// </summary>
    public class CodeTypeDeclarationExt : CodeTypeDeclaration
    {
        /// <summary>
        /// If the code type declaration is a static class or VB module (IsClass attributes must be set to true)
        /// </summary>
        public bool IsStatic { get; set; } = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public CodeTypeDeclarationExt()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public CodeTypeDeclarationExt(string name) : base(name)
        {
        }
    }
}