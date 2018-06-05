using System;

namespace CodeDomExt.Generators
{
    /// <summary>
    /// An exception that can be thrown by some CodeObjectsHandler when an object is not handled
    /// </summary>
    public class ObjectUnhandledException : Exception
    {
        private const string DefaultMsg = "Object of type {0} was not handled.";
        private const string DefaultMsgBy = "Object of type {0} was not handled by {1}.";
        
        /// <summary>
        /// Create the exception with a message specifying the unhandled object type
        /// </summary>
        /// <param name="o"></param>
        public ObjectUnhandledException(object o) : base(string.Format(DefaultMsg, o.GetType()))
        {
        }

        /// <summary>
        /// Create the exception with a message specifying the unhandled object type and the handler name
        /// </summary>
        /// <param name="o"></param>
        /// <param name="handlerName"></param>
        public ObjectUnhandledException(object o, string handlerName) : base(string.Format(DefaultMsgBy, o.GetType(), handlerName))
        {
        }

        /// <summary>
        /// Create the exception with a custom message
        /// </summary>
        /// <param name="msg"></param>
        public ObjectUnhandledException(string msg) : base(msg)
        {
        }
    }
}
