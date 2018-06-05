using System;
using System.CodeDom;

namespace CodeDomExt.Utils
{
    /// <summary>
    /// Types of binary operators
    /// </summary>
    public enum CodeBinaryOperatorTypeMore
    {
#pragma warning disable 1591
        Add = CodeBinaryOperatorType.Add,
        Subtract = CodeBinaryOperatorType.Subtract,
        Multiply = CodeBinaryOperatorType.Multiply,
        Divide = CodeBinaryOperatorType.Divide,
        Modulus =  CodeBinaryOperatorType.Modulus,
        Assign =  CodeBinaryOperatorType.Assign,
        IdentityInequality =  CodeBinaryOperatorType.IdentityInequality,
        IdentityEquality =  CodeBinaryOperatorType.IdentityEquality,
        ValueEquality =  CodeBinaryOperatorType.ValueEquality,
        BitwiseOr  =  CodeBinaryOperatorType.BitwiseOr,
        BitwiseAnd =  CodeBinaryOperatorType.BitwiseAnd,
        BooleanOr =  CodeBinaryOperatorType.BooleanOr,
        BooleanAnd =  CodeBinaryOperatorType.BooleanAnd,
        LessThan =  CodeBinaryOperatorType.LessThan,
        LessThanOrEqual =  CodeBinaryOperatorType.LessThanOrEqual,
        GreaterThan =  CodeBinaryOperatorType.GreaterThan,
        GreaterThanOrEqual =  CodeBinaryOperatorType.GreaterThanOrEqual,
        ValueInequality,
        LeftBitShift,
        RightBitShift,
        NullCoalescing,
        BitwiseXOr
#pragma warning restore 1591
    }

    /// <summary>
    /// Utility class for binary operators
    /// </summary>
    public static class BinaryOperatorTypeExtension
    {
        /// <summary>
        /// Returns if the operator can be used as a shorthand operator
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static bool CanBeShorthandOperator(this CodeBinaryOperatorTypeMore op)
        {
            return op == CodeBinaryOperatorTypeMore.Add || 
                   op == CodeBinaryOperatorTypeMore.Subtract ||
                   op == CodeBinaryOperatorTypeMore.Multiply || 
                   op == CodeBinaryOperatorTypeMore.Divide ||
                   op == CodeBinaryOperatorTypeMore.Modulus || 
                   op == CodeBinaryOperatorTypeMore.BitwiseAnd ||
                   op == CodeBinaryOperatorTypeMore.BitwiseOr || 
                   op == CodeBinaryOperatorTypeMore.BitwiseXOr ||
                   op == CodeBinaryOperatorTypeMore.LeftBitShift || 
                   op == CodeBinaryOperatorTypeMore.RightBitShift;
        }
        /// <summary>
        /// Return the provided operator as a <see cref="CodeBinaryOperatorType"/>
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">If there is no <see cref="CodeBinaryOperatorType"/> equivalent</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static CodeBinaryOperatorType AsCodeBinaryOperatorType(this CodeBinaryOperatorTypeMore op)
        {
            switch (op)
            {
                case CodeBinaryOperatorTypeMore.Add:
                case CodeBinaryOperatorTypeMore.Subtract:
                case CodeBinaryOperatorTypeMore.Multiply:
                case CodeBinaryOperatorTypeMore.Divide:
                case CodeBinaryOperatorTypeMore.Modulus:
                case CodeBinaryOperatorTypeMore.Assign:
                case CodeBinaryOperatorTypeMore.IdentityInequality:
                case CodeBinaryOperatorTypeMore.IdentityEquality:
                case CodeBinaryOperatorTypeMore.ValueEquality:
                case CodeBinaryOperatorTypeMore.BitwiseOr:
                case CodeBinaryOperatorTypeMore.BitwiseAnd:
                case CodeBinaryOperatorTypeMore.BooleanOr:
                case CodeBinaryOperatorTypeMore.BooleanAnd:
                case CodeBinaryOperatorTypeMore.LessThan:
                case CodeBinaryOperatorTypeMore.LessThanOrEqual:
                case CodeBinaryOperatorTypeMore.GreaterThan:
                case CodeBinaryOperatorTypeMore.GreaterThanOrEqual:
                    return (CodeBinaryOperatorType)op;
                case CodeBinaryOperatorTypeMore.LeftBitShift:
                case CodeBinaryOperatorTypeMore.RightBitShift:
                case CodeBinaryOperatorTypeMore.NullCoalescing:
                case CodeBinaryOperatorTypeMore.ValueInequality:
                case CodeBinaryOperatorTypeMore.BitwiseXOr:
                    throw new ArgumentException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(op), op, null);
            }
        }

        /// <summary>
        /// Returns the provided operator type as a <see cref="CodeBinaryOperatorTypeMore"/>
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static CodeBinaryOperatorTypeMore AsBinaryOperatorTypeMore(this CodeBinaryOperatorType op)
        {
            return (CodeBinaryOperatorTypeMore)op;
        }
    }
}