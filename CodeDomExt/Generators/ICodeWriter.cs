using System.Collections.Immutable;
using System.IO;
using System.Text;

namespace CodeDomExt.Generators
{
    /// <summary>
    /// Provides basic method to output code
    /// </summary>
    public interface ICodeWriter
    {
        /// <summary>
        /// Writes provided string
        /// </summary>
        /// <param name="str"></param>
        void Write(string str);
        /// <summary>
        /// Writes a new line character
        /// </summary>
        void NewLine();
        /// <summary>
        /// Indents
        /// </summary>
        /// <param name="ctx"></param>
        void Indent(Context ctx);
    }

    /// <summary>
    /// Utility class for code writer
    /// </summary>
    public static class CodeWriterExt
    {
        /// <summary>
        /// Writes provided string followed by a new line
        /// </summary>
        /// <param name="self"></param>
        /// <param name="str"></param>
        public static void WriteLine(this ICodeWriter self, string str)
        {
            self.Write(str);
            self.NewLine();
        }
        
        /// <summary>
        /// Indents then write
        /// </summary>
        /// <param name="self"></param>
        /// <param name="str"></param>
        /// <param name="ctx"></param>
        public static void IndentAndWrite(this ICodeWriter self, string str, Context ctx)
        {
            self.Indent(ctx);
            self.Write(str);
        }
        
        /// <summary>
        /// Indents then write provided string followed by newline
        /// </summary>
        /// <param name="self"></param>
        /// <param name="str"></param>
        /// <param name="ctx"></param>
        public static void IndentAndWriteLine(this ICodeWriter self, string str, Context ctx)
        {
            self.IndentAndWrite(str, ctx);
            self.NewLine();
        }
    }
    
    /// <summary>
    /// A class that adapts a <see cref="StreamWriter"/> to be used as <see cref="ICodeWriter"/>
    /// </summary>
    public class StreamWriterAdapter : ICodeWriter
    {
        private readonly StreamWriter _sw;

        /// <summary>
        /// </summary>
        /// <param name="sw">The <see cref="StreamWriter"/> that will be used to output code</param>
        public StreamWriterAdapter(StreamWriter sw)
        {
            _sw = sw;
        }

        /// <inheritdoc/>
        public void Write(string str)
        {
            _sw.Write(str);
        }

        /// <inheritdoc/>
        public void NewLine()
        {
            _sw.WriteLine();
        }

        /// <inheritdoc/>
        public void Indent(Context ctx)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < ctx.Indentation; i++)
            {
                sb.Append(ctx.Options.IndentString);
            }
            _sw.Write(sb.ToString());
        }
    }
}