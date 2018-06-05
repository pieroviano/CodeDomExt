using System.CodeDom;

namespace CodeDomExt.Generators.Common
{
    /// <summary>
    /// Partial implementation of a comment handler. Will output the appropiate single-line comment prefix,
    /// followed by the comment text
    /// </summary>
    /// <remarks>
    /// A comment handler should output a single-line comment, without indenting, nor finishing with a new line or whitespace.
    /// </remarks>
    public abstract class DefaultCommentHandler : ICodeObjectHandler<CodeComment>
    {
        /// <inheritdoc />
        public bool Handle(CodeComment obj, Context ctx)
        {
            if (obj.DocComment)
            {
                if (string.IsNullOrEmpty(SingleLineDocCommentPrefix))
                {
                    return false;
                }
                ctx.Writer.Write(SingleLineDocCommentPrefix);
            }
            else
            {
                if (string.IsNullOrEmpty(SingleLineCommentPrefix))
                {
                    return false;
                }
                ctx.Writer.Write(SingleLineCommentPrefix);
            }
            ctx.Writer.Write(obj.Text);
            return true;
        }
        
        /// <summary>
        /// String for single line comment
        /// </summary>
        protected abstract string SingleLineCommentPrefix { get; }
        /// <summary>
        /// String for single line doc comment
        /// </summary>
        protected abstract string SingleLineDocCommentPrefix { get; }
    }
}