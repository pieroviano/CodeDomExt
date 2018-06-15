using System.CodeDom;
using System.Linq;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Csharp
{
    /// <inheritdoc/>
    public class DefaultCompileUnitHandler : ICodeObjectHandler<CodeCompileUnit>
    {
        /// <inheritdoc/>
        public bool Handle(CodeCompileUnit obj, Context ctx)
        {
            GeneralUtils.HandleCollectionOnMultipleLines(obj.StartDirectives.Cast<CodeDirective>(),
                ctx.HandlerProvider.DirectiveHandler, ctx, false);
            GeneralUtils.HandleCollection(obj.Namespaces.Cast<CodeNamespace>(), ctx.HandlerProvider.NamespaceHandler, ctx,
                postAction: (c) => c.Writer.NewLine(), doPostActionOnLast: false);
            GeneralUtils.HandleCollectionOnMultipleLines(obj.EndDirectives.Cast<CodeDirective>(),
                ctx.HandlerProvider.DirectiveHandler, ctx, true);
            return true;
        }
    }
}