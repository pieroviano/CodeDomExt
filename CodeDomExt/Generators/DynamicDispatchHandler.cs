using System;

namespace CodeDomExt.Generators
{
    /// <summary>
    /// A partial implementation of an handler; the handle method delegates the handling to the implementation, but if
    /// it causes a <see cref="Exception"/> this will catch it and return false
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DynamicDispatchHandler<T> : ICodeObjectHandler<T>
    {
        /// <inheritdoc/>
        public bool Handle(T obj, Context ctx)
        {
            try
            {
                return DoDynamicHandle(obj, ctx);
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "RuntimeBinderException")
                {
                    return false;
                }

                throw;
            }
        }

        /// <summary>
        /// Actual implementation of the object handling, may throw <see cref="RuntimeBinderException"/>
        /// <para>
        /// <inheritdoc cref="Handle"/>
        /// </para>W
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected abstract bool DoDynamicHandle(T obj, Context ctx);
    }
}