using System.CodeDom;

namespace CodeDomExt.Helpers
{
    /// <summary>
    /// Factory for <see cref="CodeFieldReferenceExpression"/>
    /// </summary>
    public static class FieldReferenceExpression
    {
        /// <summary>
        /// Returns a new CodeFieldReferenceExpression where target object is this
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static CodeFieldReferenceExpression This(string fieldName)
        {
            return new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName);
        }
        /// <summary>
        /// Returns a new CodeFieldReferenceExpression where target object is base
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static CodeFieldReferenceExpression Base(string fieldName)
        {
            return new CodeFieldReferenceExpression(new CodeBaseReferenceExpression(), fieldName);
        }
        /// <summary>
        /// Returns a new CodeFieldReferenceExpression where target object is null
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static CodeFieldReferenceExpression Default(string fieldName)
        {
            return new CodeFieldReferenceExpression(null, fieldName);
        }
    }
}