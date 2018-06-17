using System.CodeDom;

namespace CodeDomExt.Nodes
{

    /// <summary>
    /// Extended CodeNamespaceImport
    /// </summary>
    public class CodeNamespaceImportExt : CodeNamespaceImport
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsStatic { get; set; } = false;
        /// <summary>
        /// Initializes with an empty string as Name and IsStatic is set to false
        /// </summary>
        public CodeNamespaceImportExt() { }
        /// <summary>
        /// Initializes Name and sets IsStatic to false
        /// </summary>
        /// <param name="nameSpace"></param>
        public CodeNamespaceImportExt(string nameSpace) : base(nameSpace) { }
        /// <summary>
        /// Initializes Name and IsStatic
        /// </summary>
        /// <param name="isStatic"></param>
        /// <param name="nameSpace"></param>
        public CodeNamespaceImportExt(bool isStatic, string nameSpace) : base(nameSpace)
        {
            this.IsStatic = isStatic;
        }
    }
}