using System.CodeDom;
using CodeDomExt.Utils;

namespace CodeDomExt.Nodes
{
    /// <summary>
    /// Expression representing a binary operation
    /// </summary>
    public class CodeBinaryOperatorExpressionMore : CodeExpression
    {
        /// <summary>
        /// Left expression of the operation
        /// </summary>
        public CodeExpression LeftExpression { get; set; }
        /// <summary>
        /// Right expression of the operation
        /// </summary>
        public CodeExpression RightExpression { get; set; }
        /// <summary>
        /// The operator
        /// </summary>
        public CodeBinaryOperatorTypeMore OperatorType { get; set; }

        /// <summary>
        /// Constructor setting all values 
        /// </summary>
        /// <param name="leftExpression"></param>
        /// <param name="operatorType"></param>
        /// <param name="rightExpression"></param>
        public CodeBinaryOperatorExpressionMore(CodeExpression leftExpression, CodeBinaryOperatorTypeMore operatorType, CodeExpression rightExpression)
        {
            LeftExpression = leftExpression;
            RightExpression = rightExpression;
            OperatorType = operatorType;
        }
    }
}