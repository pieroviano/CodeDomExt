namespace CodeDomExt.Generators.Csharp
{
    /// <summary>
    /// Context containing information used only by C# code generator
    /// </summary>
    public class CSharpContext
    {
        /// <summary>
        /// The operation requested to the <see cref="CodeDomExt.Generators.Csharp.DefaultTypeAttributesHandler"/>
        /// </summary>
        public TypeParameterHandlerOperations TypeParameterHandlerRequestedOperation { get; set; }
        
        /// <summary>
        /// Enum representing the possible operations for <see cref="CodeDomExt.Generators.Csharp.DefaultTypeAttributesHandler"/>
        /// </summary>
        public enum TypeParameterHandlerOperations
        {
#pragma warning disable 1591
            Declaration, Constraint
#pragma warning restore 1591
        }
    }
}