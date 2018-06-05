using System;
using System.CodeDom;
using System.Collections.Immutable;
using System.Linq;
using CodeDomExt.Helpers;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.VisualBasic
{
    /// <summary>
    /// Utility class for VB code generator
    /// </summary>
    public static class VisualBasicKeywordsUtils
    {
        private static readonly ImmutableHashSet<string> Keywords = ImmutableHashSet.Create(
            new[] {
            "AddHandler", "AddressOf", "Alias", "And", "AndAlso", "As", "Boolean", "ByRef", "Byte", "ByVal", "Call",
            "Case", "Catch", "CBool", "CByte", "CChar", "CDate", "CDbl", "CDec", "Char", "CInt", "Class", "CLng",
            "CObj", "Const", "Continue", "CSByte", "CShort", "CSng", "CStr", "CType", "CUInt", "CULng", "CUShort",
            "Date", "Decimal", "Declare", "Default", "Delegate", "Dim", "DirectCast", "Do", "Double", "Each", "Else",
            "ElseIf", "End", "EndIf", "Enum", "Erase", "Error", "Event", "Exit", "False", "Finally", "For", "Each",
            "Next", "Friend", "Function", "Get", "GetType", "GetXMLNamespace", "Global", "GoSub", "GoTo", "Handles",
            "If", "Implements", "Imports", "In", "Inherits", "Integer", "Interface", "Is", "IsNot", "Let", "Lib",
            "Like", "Long", "Loop", "Me", "Mod", "Module", "MustInherit", "MustOverride", "MyBase", "MyClass",
            "Namespace", "Narrowing", "New", "Next", "Not", "Nothing", "NotInheritable", "NotOverridable", "Object",
            "Of", "On", "Operator", "Option", "Optional", "Or", "OrElse", "Out", "Overloads", "Overridable",
            "Overrides", "ParamArray", "Partial", "Private", "Property", "Protected", "Public", "RaiseEvent",
            "ReadOnly", "ReDim", "REM", "RemoveHandler", "Resume", "Return", "SByte", "Select", "Set", "Shadows",
            "Shared", "Short", "Single", "Static", "Step", "Stop", "String", "Structure", "Sub", "SyncLock", "Then",
            "Throw", "To", "True", "Try", "TryCast", "TypeOf", "UInteger", "ULong", "UShort", "Using", "Variant",
            "Wend", "When", "While", "Widening", "With", "WithEvents", "WriteOnly", "Xor"
            }.Select(s => s.ToLowerInvariant()).ToArray()
        );

        /// <summary>
        /// Returns true if the provided string is a VB keyword
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsVbKeyword(this string s)
        {
            if (s.ToLowerInvariant() == "next")
            {
                s.ToLowerInvariant();
            }
            return Keywords.Contains(s.ToLowerInvariant());
        }
        
        /// <summary>
        /// Returns the keyword for the provided accessibility level
        /// </summary>
        /// <param name="accessibilityLevel"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string AccessibilityLevelKeyword(AccessibilityLevel accessibilityLevel)
        {
            switch (accessibilityLevel)
            {
                case AccessibilityLevel.Public:
                    return "Public";
                case AccessibilityLevel.Protected:
                    return "Protected";
                case AccessibilityLevel.Internal:
                    return "Friend";
                case AccessibilityLevel.ProtectedInternal:
                    return "Protected Friend";
                case AccessibilityLevel.Private:
                    return "Private";
                case AccessibilityLevel.PrivateProtected:
                    return "Private Protected";
                default:
                    throw new ArgumentOutOfRangeException(nameof(accessibilityLevel), accessibilityLevel, null);
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
                return "Boolean";
            else if (type == typeof(byte))
            {
                return "Byte";
            }
            else if (type == typeof(sbyte))
            {
                return "SByte";
            }
            else if (type == typeof(char))
            {
                return "Char";
            }
            else if (type == typeof(DateTime))
            {
                return "Date";
            }
            else if (type == typeof(decimal))
            {
                return "Decimal";
            }
            else if (type == typeof(double))
            {
                return "Double";
            }
            else if (type == typeof(float))
            {
                return "Single";
            }
            else if (type == typeof(int))
            {
                return "Integer";
            }
            else if (type == typeof(uint))
            {
                return "UInteger";
            }
            else if (type == typeof(long))
            {
                return "Long";
            }
            else if (type == typeof(ulong))
            {
                return "ULong";
            }
            else if (type == typeof(object))
            {
                return "Object";
            }
            else if (type == typeof(short))
            {
                return "Short";
            }
            else if (type == typeof(ushort))
            {
                return "UShort";
            }
            else if (type == typeof(string))
            {
                return "String";
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the provided string as a valid VB identifier
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string AsVbId(this string self)
        {
            if (self.Length == 0)
            {
                return null;
            }

            if (self.Contains("."))
            {
                return VisualBasicUtils.GetValidNamespaceIdentifier(self);
            }
            if (self.IsVbKeyword())
            {
                return "[" + self + "]";
            }
            return self;
        }

        /// <summary>
        /// Returns the keyword for the provided field direction
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string DirectionKeyword(FieldDirection direction)
        {
            switch (direction)
            {
                case FieldDirection.In:
                    return "ByVal";
                case FieldDirection.Out:
                case FieldDirection.Ref:
                    return "ByRef";
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        
        /// <summary>
        /// Returns the symbol for the provided operator
        /// </summary>
        /// <param name="operator"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string UnaryOperatorSymbol(CodeUnaryOperatorType @operator)
        {
            switch (@operator)
            {
                case CodeUnaryOperatorType.BitwiseNot:
                case CodeUnaryOperatorType.BooleanNot:
                    return "Not ";
                case CodeUnaryOperatorType.Plus:
                    return "+";
                case CodeUnaryOperatorType.Negation:
                    return "-";
                case CodeUnaryOperatorType.PreIncrement:
                case CodeUnaryOperatorType.PostIncrement:
                case CodeUnaryOperatorType.PreDecrement:
                case CodeUnaryOperatorType.PostDecrement:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(@operator), @operator, null);
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
                    return "Mod";
                case CodeBinaryOperatorTypeMore.Assign:
                    return "=";
                case CodeBinaryOperatorTypeMore.IdentityInequality:
                    return "IsNot";
                case CodeBinaryOperatorTypeMore.IdentityEquality:
                    return "Is";
                case CodeBinaryOperatorTypeMore.ValueEquality:
                    return "=";
                case CodeBinaryOperatorTypeMore.BitwiseOr:
                    return "Or";
                case CodeBinaryOperatorTypeMore.BitwiseAnd:
                    return "And";
                case CodeBinaryOperatorTypeMore.BooleanOr:
                    return "OrElse";
                case CodeBinaryOperatorTypeMore.BooleanAnd:
                    return "AndAlso";
                case CodeBinaryOperatorTypeMore.LessThan:
                    return "<";
                case CodeBinaryOperatorTypeMore.LessThanOrEqual:
                    return "<=";
                case CodeBinaryOperatorTypeMore.GreaterThan:
                    return ">";
                case CodeBinaryOperatorTypeMore.GreaterThanOrEqual:
                    return ">=";
                case CodeBinaryOperatorTypeMore.ValueInequality:
                    return "<>";
                case CodeBinaryOperatorTypeMore.LeftBitShift:
                    return "<<";
                case CodeBinaryOperatorTypeMore.RightBitShift:
                    return ">>";
                case CodeBinaryOperatorTypeMore.BitwiseXOr:
                    return "Xor";
                case CodeBinaryOperatorTypeMore.NullCoalescing:
                default:
                    throw new ArgumentOutOfRangeException(nameof(operatorType), operatorType, null);
            }
        }
    }
}