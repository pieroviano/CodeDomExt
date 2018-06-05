using System.CodeDom;

namespace CodeDomExt.Helpers
{
    /// <summary>
    /// Factory for <see cref="CodeEventReferenceExpression"/>
    /// </summary>
    public static class EventReferenceExpression
    {
        /// <summary>
        /// Returns a new CodeEventReferenceExpression where target object is this
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static CodeEventReferenceExpression This(string eventName)
        {
            return new CodeEventReferenceExpression(new CodeThisReferenceExpression(), eventName);
        }
        /// <summary>
        /// Returns a new CodeEventReferenceExpression where target object is base
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static CodeEventReferenceExpression Base(string eventName)
        {
            return new CodeEventReferenceExpression(new CodeBaseReferenceExpression(), eventName);
        }
        /// <summary>
        /// Returns a new CodeEventReferenceExpression where target object is null
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static CodeEventReferenceExpression Default(string eventName)
        {
            return new CodeEventReferenceExpression(null, eventName);
        }
    }
}