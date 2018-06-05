using System.CodeDom;

namespace CodeDomExt.Helpers
{
    /// <summary>
    /// Factory for <see cref="CodeMethodReferenceExpression"/>
    /// </summary>
    public static class MethodReferenceExpression
    {
        /// <summary>
        /// Returns a new CodeMethodReferenceExpression where target object is this
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="typeParameters"></param>
        /// <returns></returns>
        public static CodeMethodReferenceExpression This(string methodName, params CodeTypeReference[] typeParameters)
        {
            return new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), methodName, typeParameters);
        }
        /// <summary>
        /// Returns a new CodeMethodReferenceExpression where target object is base
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="typeParameters"></param>
        /// <returns></returns>
        public static CodeMethodReferenceExpression Base(string methodName, params CodeTypeReference[] typeParameters)
        {
            return new CodeMethodReferenceExpression(new CodeBaseReferenceExpression(), methodName, typeParameters);
        }
        /// <summary>
        /// Returns a new CodeMethodReferenceExpression where target object is null
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="typeParameters"></param>
        /// <returns></returns>
        public static CodeMethodReferenceExpression Default(string methodName, params CodeTypeReference[] typeParameters)
        {
            return new CodeMethodReferenceExpression(null, methodName, typeParameters);
        }
    }
}