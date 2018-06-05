using System.Collections.Generic;

namespace CodeDomExt.Generators
{
    /// <summary>
    /// An <see cref="ICodeObjectHandler{T}"/> based on the chain of responsibility pattern.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChainOfResponsibilityHandler<T> : ICodeObjectHandler<T>
    {
        private readonly IList<ICodeObjectHandler<T>> _handlers = new List<ICodeObjectHandler<T>>();
        private readonly bool _exceptionOnUnhandledObject;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exceptionOnUnhandledObject">True if this should throw an exception when the Handle method
        /// failse to find a suitable handler</param>
        public ChainOfResponsibilityHandler(bool exceptionOnUnhandledObject = true)
        {
            _exceptionOnUnhandledObject = exceptionOnUnhandledObject;
        }
        
        /// <summary>
        /// <inheritdoc />
        /// Tries to delegate handling of the provided object to the handlers added by the <see cref="AddHandler"/> method,
        /// giving priority to the latest handler added, until the object is handled. If no suitable handler was found
        /// returns false or throws an exception
        /// </summary>
        /// <exception cref="ObjectUnhandledException">
        /// If <see cref="ChainOfResponsibilityHandler{T}(bool)"/>  exceptionOnUnhandledObject parameter was set to true,
        /// and no handler was capable of handling provided object
        /// </exception>
        public bool Handle(T obj, Context ctx)
        {
            for (int i = _handlers.Count - 1; i >= 0; i--)
            {
                if (_handlers[i].Handle(obj, ctx))
                {
                    return true;
                }
            }

            if (_exceptionOnUnhandledObject)
            {
                throw new ObjectUnhandledException(obj);
            }
            return false;
        }

        /// <summary>
        /// Add an handler, which will be used by <see cref="Handle"/> method
        /// </summary>
        /// <param name="handler"></param>
        public void AddHandler(ICodeObjectHandler<T> handler)
        {
            _handlers.Add(handler);
        }
    }
}