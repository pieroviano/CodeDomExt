using System;
using System.CodeDom;
using System.Linq;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Common
{
    /// <summary>
    /// A partial implementation for a type reference handler. Implementation should handle type arguments and array
    /// indexer wrapping, the namespace separator string and the keywords for built-in types.
    /// </summary>
    /// <remarks>
    /// A type reference handler should output the type on the current line, without indenting nor finishing with a new
    /// line or whitespace. 
    /// </remarks>
    public abstract class DefaultTypeReferenceHandler : ICodeObjectHandler<CodeTypeReference>
    {
        private const string NamespaceSeparatorInBaseType = ".";
        private readonly bool _handleNullable;
        
        /// <param name="handleNullable">true if nullable types should be handled as type? instead of Nullable&lt;type&gt;</param>
        protected DefaultTypeReferenceHandler(bool handleNullable)
        {
            _handleNullable = handleNullable;
        }

        /// <inheritdoc />
        public bool Handle(CodeTypeReference obj, Context ctx)
        {
            if (_handleNullable && obj.BaseType.Equals("System.Nullable`1") && obj.TypeArguments.Count == 1) 
                //necessary in order to use ? in nullable
            {
                ctx.HandlerProvider.TypeReferenceHandler.Handle(obj.TypeArguments[0], ctx);
                ctx.Writer.Write("?");
            }
            else
            {
                //TODO reference options are ignored
                ctx.Writer.Write(GetBaseTypeString(obj, ctx));
                if (obj.TypeArguments.Count > 0)
                {
                    WrapTypeArguments((c) =>
                    {
                        GeneralUtils.HandleCollectionCommaSeparated(obj.TypeArguments.Cast<CodeTypeReference>(),
                            c.HandlerProvider.TypeReferenceHandler, c);
                    }, ctx);
                }
            }
            
            //if the type is an array obj.BaseType and obj.ArrayElementType are the same
            if (obj.ArrayRank > 0)
            {
                WrapArrayIndexer((c) =>
                {
                    for (int i = 1; i < obj.ArrayRank; i++)
                    {
                        c.Writer.Write(",");
                    }
                }, ctx);
            }

            return true;
        }

        private string GetBaseTypeString(CodeTypeReference obj, Context ctx)
        {
            //TODO visual basic identifiers are not case sensitive
            Type baseType = Type.GetType(obj.BaseType);
            if (baseType != null)
            {
                string keywordType = GetTypeKeywordString(baseType);
                if (keywordType != null)
                {
                    return keywordType;
                }
            }
            
            string res = obj.BaseType;

            res = res.StripGenericTypeArgumentsNumber();

            if (res.Contains(NamespaceSeparatorInBaseType))
            {
                string typeName = res.Split(new[] {NamespaceSeparatorInBaseType}, StringSplitOptions.None).Last();
                string typeNamespace = res.Substring(0, res.Length - typeName.Length - NamespaceSeparatorInBaseType.Length);

                if (!ctx.Options.AlwaysUseFullyQualifiedName)
                {
                    //namespace imported
                    foreach (string importedNamespace in ctx.ImportedNamespaces)
                    {
                        if (importedNamespace == typeNamespace)
                        {
                            return AsId(typeName);
                        }
                    }

                    //partial namespace import
                    if (res.StartsWith(ctx.CurrentNamespace + NamespaceSeparatorInBaseType))
                    {
                        int startIndex = ctx.CurrentNamespace.Length + NamespaceSeparatorInBaseType.Length;
                        int lenght = res.Length - typeName.Length - NamespaceSeparatorInBaseType.Length - startIndex;
                        if (lenght > 0) {
                            string reducedNamespace = res.Substring(startIndex, lenght);
                            return AsValidNamespace($"{reducedNamespace}{NamespaceSeparatorInBaseType}{typeName}");
                        }
                        return AsId(typeName);
                    }
                }

                return AsValidNamespace(res);
            }
            
            return AsId(res);
        }

        /// <summary>
        /// Wrap type arguments
        /// </summary>
        /// <param name="typeArgumentWriteAction">The action handling type arguments</param>
        /// <param name="ctx"></param>
        protected abstract void WrapTypeArguments(Action<Context> typeArgumentWriteAction, Context ctx);
        /// <summary>
        /// Wrap array rank 
        /// </summary>
        /// <param name="arrayIndexerArgumentsWriteAction">The action handling array rank</param>
        /// <param name="ctx"></param>
        protected abstract void WrapArrayIndexer(Action<Context> arrayIndexerArgumentsWriteAction, Context ctx);
        /// <param name="baseType"></param>
        /// <returns>The keyword for the provided type or null if the type is not associated to a keyword</returns>
        protected abstract string GetTypeKeywordString(Type baseType);
        /// <summary>
        /// </summary>
        /// <param name="s"></param>
        /// <returns>The provided string as a valid identifier for the target language</returns>
        protected abstract string AsId(string s);
        /// <summary>
        /// </summary>
        /// <param name="s">A string in the form of a '.' separated namespace or '.' separated namespace + '.' + identifier</param>
        /// <returns>The provided string as a valid identifier for the target language</returns>
        protected abstract string AsValidNamespace(string s);
    }
}