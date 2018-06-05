using System.CodeDom;
using System.Reflection;

namespace CodeDomExt.Generators
{
    /// <summary>
    /// An interface providing code-generating handlers.
    /// Information about how an handler is expected to generate the code are found on the default language specific handlers. 
    /// </summary>
    public interface ICodeHandlerProvider
    {
        /// <summary>
        /// Get the handler for CodeStatements
        /// </summary>
        ICodeObjectHandler<CodeStatement> StatementHandler { get; }
        /// <summary>
        /// Get the handler for CodeExpressions
        /// </summary>
        ICodeObjectHandler<CodeExpression> ExpressionHandler { get; }
        /// <summary>
        /// Get the handler for CodeTypeMembers
        /// </summary>
        ICodeObjectHandler<CodeTypeMember> TypeMemberHandler { get; }
        /// <summary>
        /// Get the handler for CodeTypeDeclarations
        /// </summary>
        ICodeObjectHandler<CodeTypeDeclaration> TypeDeclarationHandler { get; }
        /// <summary>
        /// Get the handler for CodeTypeReferences
        /// </summary>
        ICodeObjectHandler<CodeTypeReference> TypeReferenceHandler { get; }
        /// <summary>
        /// Get the handler for CodeTypeParameters
        /// </summary>
        ICodeObjectHandler<CodeTypeParameter> TypeParameterHandler { get; }
        /// <summary>
        /// Get the handler for CodeNamespaces
        /// </summary>
        ICodeObjectHandler<CodeNamespace> NamespaceHandler { get; }
        /// <summary>
        /// Get the handler for CodeNamespaceImports
        /// </summary>
        ICodeObjectHandler<CodeNamespaceImport> NamespaceImportHandler { get; }
        /// <summary>
        /// Get the handler for CodeComments
        /// </summary>
        ICodeObjectHandler<CodeComment> CommentHandler { get; }
        /// <summary>
        /// Get the handler for CodeAttributeDeclarations
        /// </summary>
        ICodeObjectHandler<CodeAttributeDeclaration> AttributeDeclarationHandler { get; }
        /// <summary>
        /// Get the handler for MemberAttributes
        /// </summary>
        ICodeObjectHandler<MemberAttributes> MemberAttributesHandler { get; }
        /// <summary>
        /// Get the handler for TypeAttributes
        /// </summary>
        ICodeObjectHandler<TypeAttributes> TypeAttributesHandler { get; }
        /// <summary>
        /// Get the handler for CodeDirectives
        /// </summary>
        ICodeObjectHandler<CodeDirective> DirectiveHandler { get; } //TODO codeDirectiveHandler
        /// <summary>
        /// Get the handler for other object types. Unused by default implementation.
        /// </summary>
        ICodeObjectHandler<object> OtherObjectHandler { get; }
    }
}