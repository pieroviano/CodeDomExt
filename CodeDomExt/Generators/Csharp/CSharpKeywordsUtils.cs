using System;
using System.CodeDom;
using System.Collections.Generic;
using CodeDomExt.Helpers;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Csharp
{
    /// <summary>
    /// Utility class for c# code generator
    /// </summary>
    public static class CSharpKeywordsUtils
    {
        private static readonly ISet<string> Keywords = new HashSet<string>(
            new string[] {
            //always keyword
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const",
            "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern",
            "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface",
            "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override",
            "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short",
            "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof",
            "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "using", "static", "virtual", "void", "volatile",
            "while",
            //contextual keywords
            "add", "alias", "ascending", "async", "await", "descending", "dynamic", "from", "get", "global", "group",
            "into", "join", "let", "nameof", "orderby", "partial", "partial", "remove", "select",
            "set", "value", "var", "when", "where", "where", "yield"
            }
        );

        /// <summary>
        /// Returns if the provided string is a C# keyword
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsCsKeyword(this string s)
        {
            return Keywords.Contains(s);
        }
        
        /// <summary>
        /// Returns the keyword for the provided acessibility level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string AccessibilityLevelKeyword(AccessibilityLevel level)
        {
            switch (level)
            {
                case AccessibilityLevel.Public:
                    return "public";
                case AccessibilityLevel.Protected:
                    return "protected";
                case AccessibilityLevel.Internal:
                    return "internal";
                case AccessibilityLevel.ProtectedInternal:
                    return "protected internal";
                case AccessibilityLevel.Private:
                    return "private";
                case AccessibilityLevel.PrivateProtected:
                    return "private protected";
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        /// <summary>
        /// Returns the keyword for the provided type or null if it is not a built-in type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetKeywordFromType(Type type)
        {
            if (type == typeof(bool))
                return "bool";
            else if (type == typeof(byte))
            {
                return "byte";
            }
            else if (type == typeof(sbyte))
            {
                return "sbyte";
            }
            else if (type == typeof(char))
            {
                return "char";
            }
            else if (type == typeof(decimal))
            {
                return "decimal";
            }
            else if (type == typeof(double))
            {
                return "double";
            }
            else if (type == typeof(float))
            {
                return "float";
            }
            else if (type == typeof(int))
            {
                return "int";
            }
            else if (type == typeof(uint))
            {
                return "uint";
            }
            else if (type == typeof(long))
            {
                return "long";
            }
            else if (type == typeof(ulong))
            {
                return "ulong";
            }
            else if (type == typeof(object))
            {
                return "object";
            }
            else if (type == typeof(short))
            {
                return "short";
            }
            else if (type == typeof(ushort))
            {
                return "ushort";
            }
            else if (type == typeof(string))
            {
                return "string";
            }
            else if (type == typeof(void))
            {
                return "void";
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the symbol for the provided operator
        /// </summary>
        /// <param name="operatorType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string OperatorSymbol(CodeBinaryOperatorTypeMore operatorType)
        {
            switch (operatorType)
            {
                case CodeBinaryOperatorTypeMore.Add:
                    return "+";
                case CodeBinaryOperatorTypeMore.Subtract:
                    return "-";
                case CodeBinaryOperatorTypeMore.Multiply:
                    return "*";
                case CodeBinaryOperatorTypeMore.Divide:
                    return "/";
                case CodeBinaryOperatorTypeMore.Modulus:
                    return "%";
                case CodeBinaryOperatorTypeMore.Assign:
                    return "=";
                case CodeBinaryOperatorTypeMore.ValueInequality:
                case CodeBinaryOperatorTypeMore.IdentityInequality:
                    return "!=";
                case CodeBinaryOperatorTypeMore.IdentityEquality:
                case CodeBinaryOperatorTypeMore.ValueEquality:
                    return "==";
                case CodeBinaryOperatorTypeMore.BitwiseOr:
                    return "|";
                case CodeBinaryOperatorTypeMore.BitwiseAnd:
                    return "&";
                case CodeBinaryOperatorTypeMore.BooleanOr:
                    return "||";
                case CodeBinaryOperatorTypeMore.BooleanAnd:
                    return "&&";
                case CodeBinaryOperatorTypeMore.LessThan:
                    return "<";
                case CodeBinaryOperatorTypeMore.LessThanOrEqual:
                    return "<=";
                case CodeBinaryOperatorTypeMore.GreaterThan:
                    return ">";
                case CodeBinaryOperatorTypeMore.GreaterThanOrEqual:
                    return ">=";
                case CodeBinaryOperatorTypeMore.LeftBitShift:
                    return "<<";
                case CodeBinaryOperatorTypeMore.RightBitShift:
                    return ">>";
                case CodeBinaryOperatorTypeMore.NullCoalescing:
                    return "??";
                case CodeBinaryOperatorTypeMore.BitwiseXOr:
                    return "^";
                default:
                    throw new ArgumentOutOfRangeException(nameof(operatorType), operatorType, null);
            }
        }
        
        /// <inheritdoc cref="OperatorSymbol(CodeDomExt.Utils.CodeBinaryOperatorTypeMore)"/>
        public static string OperatorSymbol(CodeBinaryOperatorType operatorType)
        {
            return OperatorSymbol(operatorType.AsBinaryOperatorTypeMore());
        }

        /// <summary>
        /// Returns the symbol for the provided operator, and sets <paramref name="isOperatorAfterExpression"/>
        /// </summary>
        /// <param name="operatorType"></param>
        /// <param name="isOperatorAfterExpression">will be set to true if the provided operator symbol should be written
        /// after the expression</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string UnaryOperatorSymbol(CodeUnaryOperatorType operatorType, out bool isOperatorAfterExpression)
        {
            isOperatorAfterExpression = false;
            switch (operatorType)
            {
                case CodeUnaryOperatorType.BitwiseNot:
                    return "~";
                case CodeUnaryOperatorType.BooleanNot:
                    return "!";
                case CodeUnaryOperatorType.Plus:
                    return "+";
                case CodeUnaryOperatorType.Negation:
                    return "-";
                case CodeUnaryOperatorType.PostIncrement:
                    isOperatorAfterExpression = true;
                    goto case CodeUnaryOperatorType.PreIncrement;
                case CodeUnaryOperatorType.PreIncrement:
                    return "++";
                case CodeUnaryOperatorType.PostDecrement:
                    isOperatorAfterExpression = true;
                    goto case CodeUnaryOperatorType.PreDecrement;
                case CodeUnaryOperatorType.PreDecrement:
                    return "--";
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(operatorType), operatorType, null);
            }
        }
        
        /// <summary>
        /// Returns the keyword for the provided field direction
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string DirectionKeyword(FieldDirection dir)
        {
            switch (dir)
            {
                case FieldDirection.In:
                    return "in";
                case FieldDirection.Out:
                    return "out";
                case FieldDirection.Ref:
                    return "ref";
                default:
                    throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
            }
        }

        /// <summary>
        /// Returns the provided string as a valid c# identifier.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string AsCsId(this string self)
        {
            if (self.Length == 0)
            {
                return null;
            }
            if (self.Contains("."))
            {
                return CSharpUtils.GetValidNamespaceIdentifier(self);
            }
            if (self.IsCsKeyword())
            {
                return "@" + self;
            }
            return self;
        }
    }
}