using System;

namespace CodeDomExt.Generators
{
    /// <summary>
    /// Exception thrown by generators when encountering ambiguities 
    /// </summary>
    public class ConsistencyException : Exception
    {
        /// <summary>
        /// Create the exception with a custom message
        /// </summary>
        /// <param name="message"></param>
        public ConsistencyException(string message) : base(message)
        {
        }
    }
}