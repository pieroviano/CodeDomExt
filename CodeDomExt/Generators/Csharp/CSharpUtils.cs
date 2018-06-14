using System.CodeDom;
using System.Linq;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Csharp
{
    /// <summary>
    /// Utility class for c# code generator
    /// </summary>
    public static class CSharpUtils
    {
        /// <summary>
        /// Handles a collection of statements
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="ctx"></param>
        /// <param name="doBlockAndIndentation">If true will also output indentation and {}</param>
        public static void HandleStatementCollection(CodeStatementCollection coll, Context ctx, bool doBlockAndIndentation = true)
        { 
            if (doBlockAndIndentation)
            {
                ctx.Writer.NewLine();
                ctx.Writer.IndentAndWriteLine("{", ctx);
                ctx.Indent();
            }
            GeneralUtils.HandleCollection(coll.Cast<CodeStatement>(),
                ctx.HandlerProvider.StatementHandler, ctx,
                preAction: (c) => c.Writer.Indent(c));
            if (doBlockAndIndentation)
            {
                ctx.Unindent();
                ctx.Writer.IndentAndWriteLine("}", ctx);
            }
        }

        /// <summary>
        /// Returns the provided string as a valid namespace identifier
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public static string GetValidNamespaceIdentifier(string nameSpace)
        {
            return string.Join(".", nameSpace.Split('.').Select(CSharpKeywordsUtils.AsCsId));
        }
    }
}