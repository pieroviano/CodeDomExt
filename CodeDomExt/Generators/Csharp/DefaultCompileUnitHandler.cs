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
            GeneralUtils.HandleCollection(obj.Namespaces.Cast<CodeNamespace>(), ctx.HandlerProvider.NamespaceHandler, ctx,
                postAction: (c) => c.Writer.NewLine(), doPostActionOnLast: false);
            return true;
        }
    }
}