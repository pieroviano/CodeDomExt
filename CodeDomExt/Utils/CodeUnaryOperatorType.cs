namespace CodeDomExt.Utils
{
    /// <summary>
    /// Types of unary operators
    /// </summary>
    public enum CodeUnaryOperatorType
    {
#pragma warning disable 1591
        BitwiseNot,
        BooleanNot,
        Plus,
        Negation,
        PreIncrement,
        PostIncrement,
        PreDecrement,
        PostDecrement
#pragma warning restore 1591
    }
}