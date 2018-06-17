using System.CodeDom;

using CodeDomExt.Nodes;

namespace CodeDomExt.Generators.Csharp
{
    /// <remarks>
    /// A namespace import handler should output the type on the current line, without indenting nor finishing with a new
    /// line or whitespace.
    /// </remarks> 
    public class DefaultNamespaceImportHandler : ICodeObjectHandler<CodeNamespaceImport>
    {
        /// <inheritdoc/>
        public bool Handle(CodeNamespaceImport obj, Context ctx)
        {
            ctx.Writer.Write("using ");
            if (obj is CodeNamespaceImportExt objExt && objExt.IsStatic)
            {
                ctx.Writer.Write("static ");
            }
            ctx.Writer.Write(CSharpUtils.GetValidNamespaceIdentifier(obj.Namespace));
            ctx.ImportedNamespaces.Add(obj.Namespace);
            return true;
        }
    }
}