using System.CodeDom;

namespace CodeDomExt.Helpers
{
    /// <summary>
    /// Factory for <see cref="CodePropertyReferenceExpression"/>
    /// </summary>
    public static class PropertyReferenceExpression
    {
        /// <summary>
        /// Returns a new CodePropertyReferenceExpression where target object is this
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static CodePropertyReferenceExpression This(string propertyName)
        {
            return new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), propertyName);
        }
        /// <summary>
        /// Returns a new CodePropertyReferenceExpression where target object is base
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static CodePropertyReferenceExpression Base(string propertyName)
        {
            return new CodePropertyReferenceExpression(new CodeBaseReferenceExpression(), propertyName);
        }
        /// <summary>
        /// Returns a new CodePropertyReferenceExpression where target object is null
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static CodePropertyReferenceExpression Default(string propertyName)
        {
            return new CodePropertyReferenceExpression(null, propertyName);
        }
    }
}