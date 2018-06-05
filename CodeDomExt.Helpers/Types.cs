using System.CodeDom;
using System.Diagnostics;
using System.Linq;

namespace CodeDomExt.Helpers
{
    /// <summary>
    /// Factiry for <see cref="CodeTypeReference"/>
    /// </summary>
    public static class Types
    {
        /// <summary>
        /// Returns a new CodeTypeReference of int
        /// </summary>
        public static CodeTypeReference Int => new CodeTypeReference(typeof(int));
        /// <summary>
        /// Returns a new CodeTypeReference of uint
        /// </summary>
        public static CodeTypeReference UInt => new CodeTypeReference(typeof(uint));
        /// <summary>
        /// Returns a new CodeTypeReference of short
        /// </summary>
        public static CodeTypeReference Short => new CodeTypeReference(typeof(short));
        /// <summary>
        /// Returns a new CodeTypeReference of ushort
        /// </summary>
        public static CodeTypeReference UShort => new CodeTypeReference(typeof(ushort));
        /// <summary>
        /// Returns a new CodeTypeReference of byte
        /// </summary>
        public static CodeTypeReference Byte => new CodeTypeReference(typeof(byte));
        /// <summary>
        /// Returns a new CodeTypeReference of sbyte
        /// </summary>
        public static CodeTypeReference SByte => new CodeTypeReference(typeof(sbyte));
        /// <summary>
        /// Returns a new CodeTypeReference of long
        /// </summary>
        public static CodeTypeReference Long => new CodeTypeReference(typeof(long));
        /// <summary>
        /// Returns a new CodeTypeReference of ulong
        /// </summary>
        public static CodeTypeReference ULong => new CodeTypeReference(typeof(ulong));
        /// <summary>
        /// Returns a new CodeTypeReference of void
        /// </summary>
        public static CodeTypeReference Void => new CodeTypeReference(typeof(void));
        /// <summary>
        /// Returns a new CodeTypeReference of double
        /// </summary>
        public static CodeTypeReference Double => new CodeTypeReference(typeof(double));
        /// <summary>
        /// Returns a new CodeTypeReference of decimal
        /// </summary>
        public static CodeTypeReference Decimal => new CodeTypeReference(typeof(decimal));
        /// <summary>
        /// Returns a new CodeTypeReference of float
        /// </summary>
        public static CodeTypeReference Float => new CodeTypeReference(typeof(float));
        /// <summary>
        /// Returns a new CodeTypeReference of bool
        /// </summary>
        public static CodeTypeReference Bool => new CodeTypeReference(typeof(bool));
        /// <summary>
        /// Returns a new CodeTypeReference of string
        /// </summary>
        public static CodeTypeReference String => new CodeTypeReference(typeof(string));
        /// <summary>
        /// Returns a new CodeTypeReference of char
        /// </summary>
        public static CodeTypeReference Char => new CodeTypeReference(typeof(char));
        /// <summary>
        /// Returns a new CodeTypeReference of object
        /// </summary>
        public static CodeTypeReference Object => new CodeTypeReference(typeof(object));

        /// <summary>
        /// Returns a new CodeTypeReference of the provided CodeNamespace and CodeTypeDeclaration names.
        /// </summary>
        /// <param name="codeNamespace">namespace of the type declaration</param>
        /// <param name="typeDeclaration">the type declaration, or if it is nested the containing types from the
        /// outermost to the innermost and then the target type</param>
        /// <returns></returns>
        public static CodeTypeReference From(CodeNamespace codeNamespace, params CodeTypeDeclaration[] typeDeclaration)
        {
            //TODO generics
            return new CodeTypeReference(codeNamespace.Name + "." + 
                                         typeDeclaration.Select(td => td.Name).Aggregate((c, n) => $"{c}.{n}"));
        }
    }
}