using System;
using System.CodeDom;
using System.Collections.Immutable;
using System.Linq;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.VisualBasic
{
    /// <summary>
    /// Utility class for VB code generator
    /// </summary>
    public static class VisualBasicUtils
    {
        /// <summary>
        /// Returns the provided string as a valid namespace identifier
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public static string GetValidNamespaceIdentifier(string nameSpace)
        {
            bool flag = false;
            return string.Join(".", nameSpace.Split(new[] {'.'}, 2).Select((s) =>
            {
                if (flag) return s;
                flag = true;
                return s.AsVbId();

            }));
        }
        
        /// <summary>
        /// Begins a vb block, also updating <see cref="VisualBasicContext"/>
        /// </summary>
        /// <param name="blockType"></param>
        /// <param name="ctx"></param>
        /// <param name="doFormatting">If true write a newLine and increase indentation level</param>
        public static void BeginBlock(BlockType blockType, Context ctx, bool doFormatting = true)
        {
            if (doFormatting)
            {
                ctx.Writer.NewLine();
                ctx.Indent();
            }
            
            ctx.VisualBasic.BlockTypeStack.Push(blockType);
        }

        /// <summary>
        /// Ends a vb block, also updating <see cref="VisualBasicContext"/>
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="doFormatting">if true will reduce level of indentation and indent and write "End (currentBlock)"
        /// followed by a newLine</param>
        /// <param name="writeEndBlock">if true when doFormatting is false will output "End (currentBlock)" on the current line
        /// without doing any indentation nor terminating with a newline</param>
        /// <exception cref="ArgumentException"></exception>
        public static void EndBlock(Context ctx, bool doFormatting = true, bool writeEndBlock = true)
        {
            if (doFormatting && !writeEndBlock)
            {
                throw new ArgumentException("If doFormatting is true writeEndBlock must be true as well");
            }
            string toWrite = $"End {ctx.VisualBasic.BlockTypeStack.Pop().GetKeyword()}";
            if (doFormatting)
            {
                ctx.Unindent();
                ctx.Writer.IndentAndWriteLine(toWrite, ctx);
            }
            else if (writeEndBlock)
            {
                ctx.Writer.Write(toWrite);
            }
        }

        /// <summary>
        /// Handle a statement collection without creating a new block
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="ctx"></param>
        public static void HandleStatementCollection(CodeStatementCollection coll, Context ctx)
        {
            HandleStatementCollection(coll, ctx, null);
        }
        
        /// <summary>
        /// Creates a new block and handles a statement collection
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="ctx"></param>
        /// <param name="blockType"></param>
        public static void HandleStatementCollection(CodeStatementCollection coll, Context ctx, BlockType blockType)
        {
            HandleStatementCollection(coll, ctx, (BlockType?) blockType);
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private static void HandleStatementCollection(CodeStatementCollection coll, Context ctx, BlockType? blockType)
        {
            if (blockType != null)
            {
                BeginBlock((BlockType) blockType, ctx);
            }
            GeneralUtils.HandleCollection(coll.Cast<CodeStatement>(), ctx.HandlerProvider.StatementHandler, ctx,
                preAction: (c) => c.Writer.Indent(c));
            if (blockType != null)
            {
                EndBlock(ctx);
            }
        } 
    }

    /// <summary>
    /// An enum representing the possible block types
    /// </summary>
    public enum BlockType
    {
#pragma warning disable 1591
        Class,
        Enum,
        Function,
        Get,
        If,
        Interface,
        Module,
        Namespace,
        Property,
        Select,
        Set,
        Structure,
        Sub,
        Try,
        While,
        Do,
        For,
        Using
#pragma warning restore 1591
    }

    /// <summary>
    /// Utility class for block types
    /// </summary>
    public static class BlockTypeExtension
    {
        private static readonly ImmutableHashSet<BlockType> CanExitBlockTypes = ImmutableHashSet.Create(
            BlockType.Do, BlockType.For, BlockType.Function, BlockType.Property, BlockType.Select, BlockType.Sub,
            BlockType.Try, BlockType.While 
        ); 
        
        /// <summary>
        /// Returns the keyword for the blockType
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string GetKeyword(this BlockType self)
        {
            return self.ToString("G");
        }

        /// <summary>
        /// Returns if the block type can be used in conjunction with an exit statement
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool CanExit(this BlockType self)
        {
            return CanExitBlockTypes.Contains(self);
        }
    }
}