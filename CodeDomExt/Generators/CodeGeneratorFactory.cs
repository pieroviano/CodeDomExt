using System;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators
{
    /// <summary>
    /// Factory providing instances of a <see cref="CodeGenerator"/> for a specific language
    /// </summary>
    public static class CodeGeneratorFactory
    {
        /// <summary>
        /// Returns the code provider containing all the default handlers for the specified language, using the specified GeneratorOptions
        /// </summary>
        /// <param name="language"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static ChainOfResponsibilityHandlerCodeGenerator GetCodeGenerator(Language language, GeneratorOptions options)
        {
            var res = new ChainOfResponsibilityHandlerCodeGenerator(options);
            switch (language)
            {
                case Language.VisualBasic:
                    res.AddNamespaceHandler(new VisualBasic.DefaultNamespaceHandler());
                    res.AddNamespaceImportHandler(new VisualBasic.DefaultNamespaceImportHandler());
                    res.AddTypeAttributesHandler(new VisualBasic.DefaultTypeAttributesHandler());
                    res.AddTypeDeclarationHandler(new VisualBasic.DefaultTypeDeclarationHandler());
                    res.AddTypeMemberHandler(new VisualBasic.DefaultTypeMemberHandler());
                    res.AddTypeReferenceHandler(new VisualBasic.DefaultTypeReferenceHandler());
                    res.AddMemberAttributesHandler(new VisualBasic.DefaultMemberAttributesHandler());
                    res.AddExpressionHandler(new VisualBasic.DefaultExpressionHandler());
                    res.AddTypeParameterHandler(new VisualBasic.DefaultTypeParameterHandler());
                    res.AddAttributeDeclarationHandler(new VisualBasic.DefaultAttributeDeclarationHandler());
                    res.AddStatementHandler(new VisualBasic.DefaultStatementHandler());
                    res.AddCompileUnitHandler(new VisualBasic.DefaultCompileUnitHandler());
                    res.AddCommentHandler(new VisualBasic.DefaultCommentHandler());
                    res.AddDirectiveHandler(new VisualBasic.DefaultDirectiveHandler());
                    break;
                case Language.CSharp:
                    res.AddNamespaceHandler(new Csharp.DefaultNamespaceHandler());
                    res.AddNamespaceImportHandler(new Csharp.DefaultNamespaceImportHandler());
                    res.AddTypeAttributesHandler(new Csharp.DefaultTypeAttributesHandler());
                    res.AddTypeDeclarationHandler(new Csharp.DefaultTypeDeclarationHandler());
                    res.AddTypeMemberHandler(new Csharp.DefaultTypeMemberHandler());
                    res.AddTypeReferenceHandler(new Csharp.DefaultTypeReferenceHandler());
                    res.AddMemberAttributesHandler(new Csharp.DefaultMemberAttributesHandler());
                    res.AddExpressionHandler(new Csharp.DefaultExpressionHandler());
                    res.AddTypeParameterHandler(new Csharp.DefaultTypeParameterHandler());
                    res.AddAttributeDeclarationHandler(new Csharp.DefaultAttributeDeclarationHandler());
                    res.AddStatementHandler(new Csharp.DefaultStatementHandler());
                    res.AddCompileUnitHandler(new Csharp.DefaultCompileUnitHandler());
                    res.AddCommentHandler(new Csharp.DefaultCommentHandler());
                    res.AddDirectiveHandler(new Csharp.DefaultDirectiveHandler());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
            return res;
        }

        /// <summary>
        /// Returns the code provider containing all the default handlers for the specified language, using default GeneratorOptions
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static ChainOfResponsibilityHandlerCodeGenerator GetCodeGenerator(Language language)
        {
            return GetCodeGenerator(language, new GeneratorOptions());
        }
    }
}