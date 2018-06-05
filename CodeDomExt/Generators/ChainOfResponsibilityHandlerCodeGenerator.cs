using System.CodeDom;
using System.Reflection;

namespace CodeDomExt.Generators
{
    /// <summary>
    /// A code provider using <see cref="ChainOfResponsibilityHandler{T}"/>s
    /// </summary>
    public class ChainOfResponsibilityHandlerCodeGenerator : CodeGenerator, ICodeHandlerProvider
    {
        private readonly GeneratorOptions _options;

        private readonly ChainOfResponsibilityHandler<CodeStatement> _statementHandler = new ChainOfResponsibilityHandler<CodeStatement>();
        private readonly ChainOfResponsibilityHandler<CodeExpression> _expressionHandler = new ChainOfResponsibilityHandler<CodeExpression>();
        private readonly ChainOfResponsibilityHandler<CodeTypeMember> _typeMemberHandler = new ChainOfResponsibilityHandler<CodeTypeMember>();
        private readonly ChainOfResponsibilityHandler<CodeTypeDeclaration> _typeDeclarationHandler = new ChainOfResponsibilityHandler<CodeTypeDeclaration>();
        private readonly ChainOfResponsibilityHandler<CodeTypeReference> _typeReferenceHandler = new ChainOfResponsibilityHandler<CodeTypeReference>();
        private readonly ChainOfResponsibilityHandler<CodeTypeParameter> _typeParameterHandler = new ChainOfResponsibilityHandler<CodeTypeParameter>();
        private readonly ChainOfResponsibilityHandler<CodeNamespace> _namespaceHandler = new ChainOfResponsibilityHandler<CodeNamespace>();
        private readonly ChainOfResponsibilityHandler<CodeNamespaceImport> _namespaceImportHandler = new ChainOfResponsibilityHandler<CodeNamespaceImport>();
        private readonly ChainOfResponsibilityHandler<CodeComment> _commentHandler = new ChainOfResponsibilityHandler<CodeComment>();
        private readonly ChainOfResponsibilityHandler<CodeAttributeDeclaration> _attributeDeclarationHandler = new ChainOfResponsibilityHandler<CodeAttributeDeclaration>();
        private readonly ChainOfResponsibilityHandler<MemberAttributes> _memberAttributesHandler = new ChainOfResponsibilityHandler<MemberAttributes>();
        private readonly ChainOfResponsibilityHandler<TypeAttributes> _typeAttributesHandler = new ChainOfResponsibilityHandler<TypeAttributes>();
        private readonly ChainOfResponsibilityHandler<CodeDirective> _directiveHandler = new ChainOfResponsibilityHandler<CodeDirective>();
        private readonly ChainOfResponsibilityHandler<object> _otherObjectHandler = new ChainOfResponsibilityHandler<object>();
        private readonly ChainOfResponsibilityHandler<CodeCompileUnit> _compileUnitHandler = new ChainOfResponsibilityHandler<CodeCompileUnit>();
        
        /// <inheritdoc />
        public ICodeObjectHandler<CodeStatement> StatementHandler => _statementHandler;
        /// <inheritdoc />
        public ICodeObjectHandler<CodeExpression> ExpressionHandler => _expressionHandler;
        /// <inheritdoc />
        public ICodeObjectHandler<CodeTypeMember> TypeMemberHandler => _typeMemberHandler;
        /// <inheritdoc />
        public ICodeObjectHandler<CodeTypeDeclaration> TypeDeclarationHandler => _typeDeclarationHandler;
        /// <inheritdoc />
        public ICodeObjectHandler<CodeTypeReference> TypeReferenceHandler => _typeReferenceHandler;
        /// <inheritdoc />
        public ICodeObjectHandler<CodeTypeParameter> TypeParameterHandler => _typeParameterHandler;
        /// <inheritdoc />
        public ICodeObjectHandler<CodeNamespace> NamespaceHandler => _namespaceHandler;
        /// <inheritdoc />
        public ICodeObjectHandler<CodeNamespaceImport> NamespaceImportHandler => _namespaceImportHandler;
        /// <inheritdoc />
        public ICodeObjectHandler<CodeComment> CommentHandler => _commentHandler;
        /// <inheritdoc />
        public ICodeObjectHandler<CodeAttributeDeclaration> AttributeDeclarationHandler => _attributeDeclarationHandler;
        /// <inheritdoc />
        public ICodeObjectHandler<MemberAttributes> MemberAttributesHandler => _memberAttributesHandler;
        /// <inheritdoc />
        public ICodeObjectHandler<TypeAttributes> TypeAttributesHandler => _typeAttributesHandler;
        /// <inheritdoc />
        public ICodeObjectHandler<CodeDirective> DirectiveHandler => _directiveHandler;
        /// <inheritdoc />
        public ICodeObjectHandler<object> OtherObjectHandler => _otherObjectHandler;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public ChainOfResponsibilityHandlerCodeGenerator(GeneratorOptions options)
        {
            _options = options;
        }

        /// <inheritdoc />
        public override void Generate(CodeCompileUnit compileUnit, ICodeWriter codeWriter)
        {
            _compileUnitHandler.Handle(compileUnit, new Context(codeWriter, _options, this));
        }

        /// <summary>
        /// Add the provided handler to this <see cref="MemberAttributesHandler"/>
        /// </summary>
        /// <param name="handler"></param>
        public void AddMemberAttributesHandler(ICodeObjectHandler<MemberAttributes> handler)
        {
            _memberAttributesHandler.AddHandler(handler);
        }
        /// <summary>
        /// Add the provided handler to this <see cref="TypeAttributesHandler"/>
        /// </summary>
        /// <param name="handler"></param>
        public void AddTypeAttributesHandler(ICodeObjectHandler<TypeAttributes> handler)
        {
            _typeAttributesHandler.AddHandler(handler);
        }
        /// <summary>
        /// Add the provided handler to this <see cref="TypeMemberHandler"/>
        /// </summary>
        /// <param name="handler"></param>
        public void AddTypeMemberHandler(ICodeObjectHandler<CodeTypeMember> handler)
        {
            _typeMemberHandler.AddHandler(handler);
        }
        /// <summary>
        /// Add the provided handler to this <see cref="TypeDeclarationHandler"/>
        /// </summary>
        /// <param name="handler"></param>
        public void AddTypeDeclarationHandler(ICodeObjectHandler<CodeTypeDeclaration> handler)
        {
            _typeDeclarationHandler.AddHandler(handler);
        }
        /// <summary>
        /// Add the provided handler to this <see cref="TypeReferenceHandler"/>
        /// </summary>
        /// <param name="handler"></param>
        public void AddTypeReferenceHandler(ICodeObjectHandler<CodeTypeReference> handler)
        {
            _typeReferenceHandler.AddHandler(handler);
        }
        /// <summary>
        /// Add the provided handler to this <see cref="TypeParameterHandler"/>
        /// </summary>
        /// <param name="handler"></param>
        public void AddTypeParameterHandler(ICodeObjectHandler<CodeTypeParameter> handler)
        {
            _typeParameterHandler.AddHandler(handler);
        }
        /// <summary>
        /// Add the provided handler to this <see cref="NamespaceImportHandler"/>
        /// </summary>
        /// <param name="handler"></param>
        public void AddNamespaceImportHandler(ICodeObjectHandler<CodeNamespaceImport> handler)
        {
            _namespaceImportHandler.AddHandler(handler);
        }
        /// <summary>
        /// Add the provided handler to this <see cref="StatementHandler"/>
        /// </summary>
        /// <param name="handler"></param>
        public void AddStatementHandler(ICodeObjectHandler<CodeStatement> handler)
        {
            _statementHandler.AddHandler(handler);
        }
        /// <summary>
        /// Add the provided handler to this <see cref="NamespaceHandler"/>
        /// </summary>
        /// <param name="handler"></param>
        public void AddNamespaceHandler(ICodeObjectHandler<CodeNamespace> handler)
        {
            _namespaceHandler.AddHandler(handler);
        }
        /// <summary>
        /// Add the provided handler to this <see cref="OtherObjectHandler"/>
        /// </summary>
        /// <param name="handler"></param>
        public void AddOtherObjectHandler(ICodeObjectHandler<object> handler)
        {
            _otherObjectHandler.AddHandler(handler);
        }
        /// <summary>
        /// Add the provided handler to this <see cref="AttributeDeclarationHandler"/>
        /// </summary>
        /// <param name="handler"></param>
        public void AddAttributeDeclarationHandler(ICodeObjectHandler<CodeAttributeDeclaration> handler)
        {
            _attributeDeclarationHandler.AddHandler(handler);
        }
        /// <summary>
        /// Add the provided handler to this <see cref="CommentHandler"/>
        /// </summary>
        /// <param name="handler"></param>
        public void AddCommentHandler(ICodeObjectHandler<CodeComment> handler)
        {
            _commentHandler.AddHandler(handler);
        }
        /// <summary>
        /// Add the provided handler to this <see cref="DirectiveHandler"/>
        /// </summary>
        /// <param name="handler"></param>
        public void AddDirectiveHandler(ICodeObjectHandler<CodeDirective> handler)
        {
            _directiveHandler.AddHandler(handler);
        }
        /// <summary>
        /// Add the provided handler to this <see cref="ExpressionHandler"/>
        /// </summary>
        /// <param name="handler"></param>
        public void AddExpressionHandler(ICodeObjectHandler<CodeExpression> handler)
        {
            _expressionHandler.AddHandler(handler);
        }
        /// <summary>
        /// Add the provided handler to this compileUnitHandler
        /// </summary>
        /// <param name="handler"></param>
        public void AddCompileUnitHandler(ICodeObjectHandler<CodeCompileUnit> handler)
        {
            _compileUnitHandler.AddHandler(handler);
        }
    }
}