using System.CodeDom;
using System.Linq;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.VisualBasic
{
    /// <inheritdoc />
    public class DefaultNamespaceHandler : Common.DefaultNamespaceHandler
    {
        /// <inheritdoc />
        protected override bool DoHandle(CodeNamespace obj, Context ctx)
        {
            ctx.CurrentNamespace = obj.Name;
            ctx.Writer.Write($"Namespace {VisualBasicUtils.GetValidNamespaceIdentifier(ctx.CurrentNamespace)}");
            VisualBasicUtils.BeginBlock(BlockType.Namespace, ctx);
            GeneralUtils.HandleCollection(obj.Types.Cast<CodeTypeDeclaration>(), ctx.HandlerProvider.TypeDeclarationHandler, ctx,
                preAction: (c) => c.Writer.Indent(c),
                postAction: (c) => c.Writer.NewLine(), doPostActionOnLast: false);
            VisualBasicUtils.EndBlock(ctx);
            return true;
        }
    }
}