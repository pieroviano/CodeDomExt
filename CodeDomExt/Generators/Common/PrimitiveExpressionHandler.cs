using System;
using System.CodeDom;
using System.Globalization;

namespace CodeDomExt.Generators.Common
{
    /// <inheritdoc />
    public abstract class PrimitiveExpressionHandler : ICodeObjectHandler<CodePrimitiveExpression>
    {
        private static readonly NumberFormatInfo DoubleFormatInfo =
            new NumberFormatInfo() {NumberDecimalSeparator = "."};

        private readonly bool _throwExceptionOnInvalidPrimitive;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="throwExceptionOnInvalidPrimitive">true if this should throw an exception when the type
        /// of CodePrimitiveExpressions value is not a recognized primitive type</param>
        protected PrimitiveExpressionHandler(bool throwExceptionOnInvalidPrimitive)
        {
            _throwExceptionOnInvalidPrimitive = throwExceptionOnInvalidPrimitive;
        }

        /// <summary>
        /// <inheritdoc/>
        /// if the primitive type is not recognized returns false or throws an exception
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public bool Handle(CodePrimitiveExpression obj, Context ctx)
        {
            switch (obj.Value)
            {
                case null:
                    HandleNull(ctx);
                    break;
                case char c:
                    HandleChar(c, ctx);
                    break;
                case string str:
                    HandleString(str, ctx);
                    break;
                case bool boolean:
                    HandleBoolean(boolean, ctx);
                    break;

                case double d:
                    HandleFloatingPoint(obj.Value, d.ToString(DoubleFormatInfo), typeof(double), ctx);
                    break;
                case float f:
                    HandleFloatingPoint(obj.Value, f.ToString(DoubleFormatInfo), typeof(float), ctx);
                    break;
                case decimal dec:
                    HandleFloatingPoint(obj.Value, dec.ToString(DoubleFormatInfo), typeof(decimal), ctx);
                    break;

                case int _:
                    HandleInteger(obj.Value, typeof(int), ctx);
                    break;
                case byte _:
                    HandleInteger(obj.Value, typeof(byte), ctx);
                    break;
                case sbyte _:
                    HandleInteger(obj.Value, typeof(sbyte), ctx);
                    break;
                case short _:
                    HandleInteger(obj.Value, typeof(short), ctx);
                    break;
                case ushort _:
                    HandleInteger(obj.Value, typeof(ushort), ctx);
                    break;
                case uint _:
                    HandleInteger(obj.Value, typeof(uint), ctx);
                    break;
                case long l:
                    if (l >= int.MinValue && l <= int.MaxValue)
                    {
                        HandleInteger(obj.Value, typeof(int), ctx);
                    }
                    else
                    {
                        HandleInteger(obj.Value, typeof(long), ctx);    
                    }
                    break;
                case ulong _:
                    HandleInteger(obj.Value, typeof(ulong), ctx);
                    break;
                default:
                    if (_throwExceptionOnInvalidPrimitive)
                    {
                        throw new ArgumentException($"Type {obj.Value.GetType()} is not valid for code primitive expression");
                    }
                    return false;
            }

            return true;
        }

        private void HandleFloatingPoint(object value, string formattedValue, Type type, Context ctx)
        {
            if (type != DefaultFloatingPointType)
            {
                string forcedLiteral = GetForcedLiteral(type);

                if (string.IsNullOrEmpty(forcedLiteral))
                {
                    HandleWithCast(value, type, DefaultFloatingPointType, ctx);
                }
                else
                {
                    ctx.Writer.Write(formattedValue + forcedLiteral);
                }
            }
            else
            {
                ctx.Writer.Write(formattedValue);
            }
        }
        
        private void HandleInteger(object value, Type type, Context ctx)
        {
            if (type != DefaultIntegerType)
            {
                string forcedLiteral = GetForcedLiteral(type);

                if (string.IsNullOrEmpty(forcedLiteral))
                {
                    HandleWithCast(value, type, DefaultIntegerType, ctx);
                }
                else
                {
                    ctx.Writer.Write(value + forcedLiteral);
                }
            }
            else
            {
                ctx.Writer.Write(value.ToString());
            }
        }

        private void HandleWithCast(object value, Type typeInCast, Type typeInPrimitive, Context ctx)
        {
            ctx.HandlerProvider.ExpressionHandler.Handle(new CodeCastExpression(
                typeInCast,
                new CodePrimitiveExpression(Convert.ChangeType(value, typeInPrimitive))),
                ctx);
        }
        
        /// <summary>
        /// The default type for integer numbers
        /// </summary>
        protected abstract Type DefaultIntegerType { get; }
        /// <summary>
        /// The default type for floating point numbers
        /// </summary>
        protected abstract Type DefaultFloatingPointType { get; }

        /// <summary>
        /// The literal used as suffix to force a literal to be a type different than the default one (like 1u to get an unsigned int 1 in c#)
        /// If there is no literal for the provided type it will be handled as a cast.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected abstract string GetForcedLiteral(Type type);
        /// <summary>
        /// Handle a null primitive
        /// </summary>
        /// <param name="ctx"></param>
        protected abstract void HandleNull(Context ctx);
        /// <summary>
        /// Handle a char primitive
        /// </summary>
        /// <param name="c"></param>
        /// <param name="ctx"></param>
        protected abstract void HandleChar(char c, Context ctx);
        /// <summary>
        /// Handle a string primitive
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ctx"></param>
        protected abstract void HandleString(string str, Context ctx);
        /// <summary>
        /// Handle a boolean primitive
        /// </summary>
        /// <param name="b"></param>
        /// <param name="ctx"></param>
        protected abstract void HandleBoolean(bool b, Context ctx);
    }
}