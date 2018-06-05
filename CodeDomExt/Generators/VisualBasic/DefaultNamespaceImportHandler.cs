using System.CodeDom;

namespace CodeDomExt.Generators.VisualBasic
{
    /// <remarks>
    /// A namespace import handler should output the type on the current line and should finish with a new line,
    /// only if the namespace was not previously imported 
    /// </remarks> 
    public class DefaultNamespaceImportHandler : ICodeObjectHandler<CodeNamespaceImport>
    {
        /// <inheritdoc />
        public bool Handle(CodeNamespaceImport obj, Context ctx)
        {
            if (ctx.ImportedNamespaces.Add(obj.Namespace)) {
                ctx.Writer.WriteLine(
                    $"Imports {VisualBasicUtils.GetValidNamespaceIdentifier(obj.Namespace)}");
            }
            return true;
        }
    }
}