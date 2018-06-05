using System.Collections.Generic;

namespace CodeDomExt.Generators.VisualBasic
{
    /// <summary>
    /// Context containing information used only by VB code generator
    /// </summary>
    public class VisualBasicContext
    {
        /// <summary>
        /// Stack containing the block types being handled
        /// </summary>
        public Stack<BlockType> BlockTypeStack { get; } = new Stack<BlockType>();
        /// <summary>
        /// The block type being currently handled
        /// </summary>
        public BlockType CurrentBlockType => BlockTypeStack.Peek();
    }
}