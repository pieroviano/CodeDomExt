using System.CodeDom;
using CodeDomExt.Utils;

namespace CodeDomExt.Nodes
{
    /// <summary>
    /// Expression for a unary operator.
    /// Be careful when using pre/post increment/decrement and generating vb code. Since there is no equivalent to the ++
    /// and -- operator in vb, a workaround was used to generate working code, but it won't work when the expression is
    /// used as a statement. This will be improved in future releases, but until then when possible use <see cref="CodeOperationAssignmentStatement"/>
    /// </summary>
    public class CodeUnaryOperatorExpression : CodeExpression
    {
        /// <summary>
        /// Expression
        /// </summary>
        public CodeExpression Expression { get; set; }
        /// <summary>
        /// Operator type
        /// </summary>
        public CodeUnaryOperatorType Operator { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="op"></param>
        public CodeUnaryOperatorExpression(CodeExpression expression, CodeUnaryOperatorType op)
        {
            Expression = expression;
            Operator = op;
        }
    }
}