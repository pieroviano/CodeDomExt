using System.CodeDom;

namespace CodeDomExt.Nodes
{
    /// <summary>
    /// Expression representing a ternary conditional operator
    /// </summary>
    public class CodeConditionalOperatorExpression : CodeExpression
    {
        /// <summary>
        /// Expression to be tested
        /// </summary>
        public CodeExpression TestExpression { get; set; }
        /// <summary>
        /// Value if TestExpression is true
        /// </summary>
        public CodeExpression TrueExpression { get; set; }
        /// <summary>
        /// Value if TetsExpression is false
        /// </summary>
        public CodeExpression FalseExpression { get; set; }

        /// <summary>
        /// Constructor setting all values
        /// </summary>
        /// <param name="testExpression"></param>
        /// <param name="trueExpression"></param>
        /// <param name="falseExpression"></param>
        public CodeConditionalOperatorExpression(CodeExpression testExpression, CodeExpression trueExpression, CodeExpression falseExpression)
        {
            TestExpression = testExpression;
            TrueExpression = trueExpression;
            FalseExpression = falseExpression;
        }
    }
}