using System.CodeDom;

namespace CodeDomExt.Nodes
{
    /// <summary>
    /// Statement representing a foreach
    /// </summary>
    public class CodeForEachStatement : CodeStatement
    {
        /// <summary>
        /// Type of the item provided by the iterated object, can be null
        /// </summary>
        public CodeTypeReference ItemType { get; set; }
        /// <summary>
        /// Name of the item provided by the iterated object
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// Objects that should be iterated
        /// </summary>
        public CodeExpression ObjectToIterate { get; set; }
        /// <summary>
        /// Statements in the foreach
        /// </summary>
        public CodeStatementCollection Statements { get; }

        /// <summary>
        /// Constructor setting all values
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="itemName"></param>
        /// <param name="objectToIterate"></param>
        /// <param name="statements"></param>
        public CodeForEachStatement(CodeTypeReference itemType, string itemName, CodeExpression objectToIterate,
            params CodeStatement[] statements)
        {
            ItemType = itemType;
            ItemName = itemName;
            ObjectToIterate = objectToIterate;
            Statements = new CodeStatementCollection(statements);
        }

        /// <summary>
        /// Constructor, with default ItemType
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="objectToIterate"></param>
        /// <param name="statements"></param>
        public CodeForEachStatement(string itemName, CodeExpression objectToIterate, params CodeStatement[] statements)
            : this(null, itemName, objectToIterate, statements)
        {
        }
    }
}