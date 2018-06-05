using System.CodeDom;

namespace CodeDomExt.Nodes
{
    /// <summary>
    /// Using statement, automatically disposing resources
    /// </summary>
    public class CodeUsingStatement : CodeStatement
    {
        /// <summary>
        /// Type of the resource, can be null
        /// </summary>
        public CodeTypeReference Type { get; set; }
        /// <summary>
        /// Name of the resource variable
        /// </summary>
        public string VariableName { get; set; }
        /// <summary>
        /// Expression for initializing the resource variable
        /// </summary>
        public CodeExpression InitializerExpression { get; set; }
        /// <summary>
        /// Statements
        /// </summary>
        public CodeStatementCollection Statements { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="variableName"></param>
        /// <param name="initializerExpression"></param>
        /// <param name="statements"></param>
        public CodeUsingStatement(CodeTypeReference type, string variableName, CodeExpression initializerExpression,
            params CodeStatement[] statements)
        {
            Type = type;
            VariableName = variableName;
            InitializerExpression = initializerExpression;
            Statements = new CodeStatementCollection(statements);
        }

        /// <summary>
        /// Constructor with default type
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="initializerExpression"></param>
        /// <param name="statements"></param>
        public CodeUsingStatement(string variableName, CodeExpression initializerExpression, params CodeStatement[] statements) 
            : this(null, variableName, initializerExpression, statements)
        {
        }
    }
}