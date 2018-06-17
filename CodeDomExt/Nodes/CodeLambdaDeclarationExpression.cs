using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace CodeDomExt.Nodes
{
    /// <summary>
    /// Expression for lambda declaration
    /// </summary>
    public class CodeLambdaDeclarationExpression : CodeExpression
    {
        /// <summary>
        /// Parameters of the lambda expression
        /// </summary>
        public IEnumerable<CodeLambdaParameterDeclarationExpression> Parameters { get; }
        /// <summary>
        /// Statements of the lambda expression
        /// </summary>
        public CodeStatementCollection Statements { get; }

        
        /// <summary>
        /// Constructor for explicitly typed parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="statements"></param>
        public CodeLambdaDeclarationExpression(IEnumerable<CodeLambdaParameterDeclarationExpression> parameters, params CodeStatement[] statements)
        {
            Parameters = new List<CodeLambdaParameterDeclarationExpression>(parameters);
            Statements = new CodeStatementCollection(statements);
        }
        
        /// <summary>
        /// Constructor for implicitly typed parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="statements"></param>
        public CodeLambdaDeclarationExpression(IEnumerable<string> parameters, params CodeStatement[] statements)
            : this(parameters.Select(pName => new CodeLambdaParameterDeclarationExpression(pName)), statements)
        {
        }

        /// <summary>
        /// Constructor for lambda without parameters
        /// </summary>
        /// <param name="statements"></param>
        public CodeLambdaDeclarationExpression(params CodeStatement[] statements)
            : this(new CodeLambdaParameterDeclarationExpression[] { }, statements)
        {
        }
        
        /// <summary>
        /// Constructor initializing an empty lambda expression
        /// </summary>
        public CodeLambdaDeclarationExpression() : this(new CodeLambdaParameterDeclarationExpression[] { }, new CodeStatement[] { })
        {
        }
        /// <summary>
        /// Constructor for explicitly typed parameters, containing only one expression
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="expression"></param>
        public CodeLambdaDeclarationExpression(IEnumerable<CodeLambdaParameterDeclarationExpression> parameters, CodeExpression expression)
            : this(parameters, new CodeExpressionStatement(expression))
        {
        }
        /// <summary>
        /// Constructor for implicitly typed parameters, containing only one expression
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="expression"></param>
        public CodeLambdaDeclarationExpression(IEnumerable<string> parameters, CodeExpression expression)
            : this(parameters.Select(pName => new CodeLambdaParameterDeclarationExpression(pName)), expression)
        {
        }
        /// <summary>
        /// Constructor for lambda without parameters, containing only one expression
        /// </summary>
        /// <param name="expression"></param>
        public CodeLambdaDeclarationExpression(CodeExpression expression)
            : this(new CodeLambdaParameterDeclarationExpression[] { }, expression)
        {
        }
    }

    /// <summary>
    /// Expression for a lambda parameter declaration
    /// </summary>
    public class CodeLambdaParameterDeclarationExpression : CodeExpression
    {
        /// <summary>
        /// Type of the lambda parameter, can be null
        /// </summary>
        public CodeTypeReference Type { get; set; }
        /// <summary>
        /// Name of the lambda parameter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Constructor setting up parameter's name
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="ArgumentException">if name is null or empty</exception>
        public CodeLambdaParameterDeclarationExpression(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(nameof(name));
            }
            Name = name;
        }

        /// <summary>
        /// Constructor setting up both parameter's type and name
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <exception cref="ArgumentException">if name is null or empty</exception>
        public CodeLambdaParameterDeclarationExpression(CodeTypeReference type, string name) : this(name)
        {
            Type = type;
        }
    }
}