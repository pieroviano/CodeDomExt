using System.CodeDom;

namespace CodeDomExt.Helpers
{
    /// <summary>
    /// Factiory for <see cref="CodePrimitiveExpression"/>
    /// </summary>
    public static class Primitives
    {
        /// <summary>
        /// Returns a new codePrimitiveExpression containing null
        /// </summary>
        public static CodePrimitiveExpression Null => new CodePrimitiveExpression(null);
        /// <summary>
        /// Returns a new codePrimitiveExpression containing a bool
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static CodePrimitiveExpression Bool(bool val)
        {
            return new CodePrimitiveExpression(val);
        }
        /// <summary>
        /// Returns a new codePrimitiveExpression containing a char
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static CodePrimitiveExpression Char(char val)
        {
            return new CodePrimitiveExpression(val);
        }
        /// <summary>
        /// Returns a new codePrimitiveExpression containing a string
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static CodePrimitiveExpression String(string val)
        {
            return new CodePrimitiveExpression(val);
        }
        /// <summary>
        /// Returns a new codePrimitiveExpression containing a double
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static CodePrimitiveExpression Double(double val)
        {
            return new CodePrimitiveExpression(val);
        }
        /// <summary>
        /// Returns a new codePrimitiveExpression containing a float
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static CodePrimitiveExpression Float(float val)
        {
            return new CodePrimitiveExpression(val);
        }
        /// <summary>
        /// Returns a new codePrimitiveExpression containing a decimal
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static CodePrimitiveExpression Decimal(decimal val)
        {
            return new CodePrimitiveExpression(val);
        }
        /// <summary>
        /// Returns a new codePrimitiveExpression containing a int
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static CodePrimitiveExpression Int(int val)
        {
            return new CodePrimitiveExpression(val);
        } 
        /// <summary>
        /// Returns a new codePrimitiveExpression containing a uint
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static CodePrimitiveExpression UInt(uint val)
        {
            return new CodePrimitiveExpression(val);
        }
        /// <summary>
        /// Returns a new codePrimitiveExpression containing a byte
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static CodePrimitiveExpression Byte(byte val)
        {
            return new CodePrimitiveExpression(val);
        }
        /// <summary>
        /// Returns a new codePrimitiveExpression containing a sbyte
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static CodePrimitiveExpression SByte(sbyte val)
        {
            return new CodePrimitiveExpression(val);
        }
        /// <summary>
        /// Returns a new codePrimitiveExpression containing a long
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static CodePrimitiveExpression Long(long val)
        {
            return new CodePrimitiveExpression(val);
        }
        /// <summary>
        /// Returns a new codePrimitiveExpression containing a ulong
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static CodePrimitiveExpression ULong(ulong val)
        {
            return new CodePrimitiveExpression(val);
        }
        /// <summary>
        /// Returns a new codePrimitiveExpression containing a ushort
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static CodePrimitiveExpression UShort(ushort val)
        {
            return new CodePrimitiveExpression(val);
        }
        /// <summary>
        /// Returns a new codePrimitiveExpression containing a short
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static CodePrimitiveExpression Short(short val)
        {
            return new CodePrimitiveExpression(val);
        }
    }
}