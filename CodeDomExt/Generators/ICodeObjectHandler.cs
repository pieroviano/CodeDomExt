namespace CodeDomExt.Generators
{
    /// <summary>
    /// Basic interface providing a method for "handling" an object of type t
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICodeObjectHandler<in T>
    {
        /// <summary>
        /// If capable handles the provided object, by writing code representing the object in the provided context code
        /// writer.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ctx"></param>
        /// <returns>true if the object was handled</returns>
        bool Handle(T obj, Context ctx);
    }
}