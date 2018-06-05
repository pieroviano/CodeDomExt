using System.CodeDom;
using System.Linq;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Csharp
{
    /// <inheritdoc/>
    public class DefaultNamespaceHandler : Common.DefaultNamespaceHandler
    {
        /// <inheritdoc/>
        protected override bool DoHandle(CodeNamespace obj, Context ctx)
        {
            ctx.CurrentNamespace = obj.Name;
            ctx.Writer.WriteLine($"namespace {CSharpUtils.GetValidNamespaceIdentifier(ctx.CurrentNamespace)}");
            ctx.Writer.WriteLine("{");
            ctx.Indent();
            ctx.ImportedNamespaces.Clear();
            GeneralUtils.HandleCollection(obj.Imports.Cast<CodeNamespaceImport>(), ctx.HandlerProvider.NamespaceImportHandler, ctx,
                preAction: (c) => c.Writer.Indent(c),
                postAction: (c) => c.Writer.WriteLine(";"), doPostActionOnLast: true);
            
            if (obj.Imports.Count > 0 && obj.Types.Count > 0)
            {
                ctx.Writer.NewLine();
            }
            
            GeneralUtils.HandleCollection(obj.Types.Cast<CodeTypeDeclaration>(), ctx.HandlerProvider.TypeDeclarationHandler, ctx,
                preAction: (c) => c.Writer.Indent(c),
                postAction: (c) => c.Writer.NewLine(), doPostActionOnLast: false);
            ctx.Unindent();
            ctx.Writer.WriteLine("}");
            return true;
        }
    }
}