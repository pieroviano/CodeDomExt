using System;
using System.CodeDom;
using CodeDomExt.Utils;

namespace CodeDomExt.Nodes
{
    /// <summary>
    /// Statement for shorthand operation assignment
    /// </summary>
    public class CodeOperationAssignmentStatement : CodeStatement
    {
        private CodeBinaryOperatorTypeMore _operator;
        /// <summary>
        /// Left expression
        /// </summary>
        public CodeExpression LeftExpression { get; set; }
        /// <summary>
        /// Right expression
        /// </summary>
        public CodeExpression RightExpression { get; set; }

        /// <summary>
        /// Operator type. The specified operator must be a valid shorthand operator
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public CodeBinaryOperatorTypeMore Operator
        {
            get => _operator;
            set {
                if (!value.CanBeShorthandOperator())
                {
                    throw new ArgumentException();
                }
                _operator = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="leftExpression"></param>
        /// <param name="op"></param>
        /// <param name="rightExpression"></param>
        public CodeOperationAssignmentStatement(CodeExpression leftExpression, CodeBinaryOperatorTypeMore op, CodeExpression rightExpression)
        {
            LeftExpression = leftExpression;
            RightExpression = rightExpression;
            Operator = op;
        }

        /// <summary>
        /// Returns an equivalent assign statemnt
        /// </summary>
        /// <returns></returns>
        public CodeAssignStatement AsAssignStatement()
        {
            CodeExpression operatorExpression;
            try
            {
                CodeBinaryOperatorType op = Operator.AsCodeBinaryOperatorType();
                operatorExpression = new CodeBinaryOperatorExpression(LeftExpression, op, RightExpression);
            }
            catch (ArgumentException)
            {
                operatorExpression = new CodeBinaryOperatorExpressionMore(LeftExpression, Operator, RightExpression);
            }
            return new CodeAssignStatement(LeftExpression, operatorExpression);
        }
    }
}