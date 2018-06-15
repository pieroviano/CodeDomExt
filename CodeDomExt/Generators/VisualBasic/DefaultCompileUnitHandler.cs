using System.CodeDom;
using System.Linq;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.VisualBasic
{
    /// <inheritdoc />
    public class DefaultCompileUnitHandler : ICodeObjectHandler<CodeCompileUnit>
    {
        /// <inheritdoc />
        public bool Handle(CodeCompileUnit obj, Context ctx)
        {
            GeneralUtils.HandleCollectionOnMultipleLines(obj.StartDirectives.Cast<CodeDirective>(),
                ctx.HandlerProvider.DirectiveHandler, ctx, false);
            ctx.Writer.WriteLine("Option Strict Off");
            ctx.Writer.WriteLine("Option Explicit On");
            ctx.Writer.WriteLine("Option Infer On");
            ctx.Writer.NewLine();

            ctx.ImportedNamespaces.Clear();
            GeneralUtils.HandleCollection(
                obj.Namespaces.Cast<CodeNamespace>()
                    .SelectMany((codeNamespace) => codeNamespace.Imports.Cast<CodeNamespaceImport>()),
                ctx.HandlerProvider.NamespaceImportHandler, ctx);
            
            if (ctx.ImportedNamespaces.Count > 0)
            {
                ctx.Writer.NewLine();
            }
            
            GeneralUtils.HandleCollection(obj.Namespaces.Cast<CodeNamespace>(), ctx.HandlerProvider.NamespaceHandler, ctx,
                postAction: (c) => c.Writer.NewLine(), doPostActionOnLast: false);
            GeneralUtils.HandleCollectionOnMultipleLines(obj.EndDirectives.Cast<CodeDirective>(),
                ctx.HandlerProvider.DirectiveHandler, ctx, true);
            return true;
        }
    }
}
