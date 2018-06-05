using System.CodeDom;

namespace CodeDomExt.Nodes
{
    /// <summary>
    /// Statement for a do-while loop
    /// </summary>
    public class CodePostTestIterationStatement : CodeStatement
    {
        /// <summary>
        /// Statements of the loop
        /// </summary>
        public CodeStatementCollection Statements { get; }
        /// <summary>
        /// Expression to be tested
        /// </summary>
        public CodeExpression TestExpression { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CodePostTestIterationStatement()
        {
            Statements = new CodeStatementCollection();
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public CodePostTestIterationStatement(CodeExpression testExpression, params CodeStatement[] statements)
        {
            TestExpression = testExpression;
            Statements = new CodeStatementCollection(statements);
        }
    }
}