using System.CodeDom;

namespace CodeDomExt.Helpers
{
    /// <summary>
    /// Factory for <see cref="CodeDelegateCreateExpression"/>
    /// </summary>
    public static class DelegateCreateExpression
    {
        /// <summary>
        /// Returns a new CodeTypeReference where target object is this
        /// </summary>
        /// <param name="type"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static CodeDelegateCreateExpression This(CodeTypeReference type, string eventName)
        {
            return new CodeDelegateCreateExpression(type, new CodeThisReferenceExpression(), eventName);
        }
        /// <summary>
        /// Returns a new CodeTypeReference where target object is base
        /// </summary>
        /// <param name="type"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static CodeDelegateCreateExpression Base(CodeTypeReference type, string eventName)
        {
            return new CodeDelegateCreateExpression(type, new CodeBaseReferenceExpression(), eventName);
        }
        /// <summary>
        /// Returns a new CodeTypeReference where target object is null
        /// </summary>
        /// <param name="type"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static CodeDelegateCreateExpression Default(CodeTypeReference type, string eventName)
        {
            return new CodeDelegateCreateExpression(type, null, eventName);
        }
    }
}