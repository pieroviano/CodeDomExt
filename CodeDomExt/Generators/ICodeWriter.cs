using System;
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
    /// A class that adapts a <see cref="T:System.IO.TextWriter" /> to be used as <see cref="T:CodeDomExt.Generators.ICodeWriter" />
    /// </summary>
    public class TextWriterAdapter : ICodeWriter
    {
        private readonly TextWriter _tw;

        /// <summary>
        /// </summary>
        /// <param name="tw">The <see cref="TextWriter"/> that will be used to output code</param>
        public TextWriterAdapter(TextWriter tw)
        {
            _tw = tw;
        }

        /// <inheritdoc/>
        public void Write(string str)
        {
            _tw.Write(str);
        }

        /// <inheritdoc/>
        public void NewLine()
        {
            _tw.WriteLine();
        }

        /// <inheritdoc/>
        public void Indent(Context ctx)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < ctx.Indentation; i++)
            {
                sb.Append(ctx.Options.IndentString);
            }
            _tw.Write(sb.ToString());
        }
    }

    /// <summary>
    /// A code writer which writes the code to a string
    /// </summary>
    public class StringCodeWriter : ICodeWriter
    {
        private readonly StringBuilder _builder = new StringBuilder();
        private readonly string _newLine;
        private bool _needsUpdate = false;
        private string _generatedCode = "";

        /// <summary>
        /// Create a new StringCodeWriter using the default environment newLine string
        /// </summary>
        /// <param name="newLine"></param>
        public StringCodeWriter()
        {
            _newLine = Environment.NewLine;
        }

        /// <summary>
        /// Create a new StringCodeWriter using the provided newLine string
        /// </summary>
        /// <param name="newLine"></param>
        public StringCodeWriter(string newLine)
        {
            _newLine = newLine;
        }
        
        /// <inheritdoc />
        public void Write(string str)
        {
            _needsUpdate = true;
            _builder.Append(str);
        }

        /// <inheritdoc />
        public void NewLine()
        {
            _needsUpdate = true;
            _builder.Append(_newLine);
        }

        /// <inheritdoc />
        public void Indent(Context ctx)
        {
            _needsUpdate = true;
            for (int i = 0; i < ctx.Indentation; i++)
            {
                _builder.Append(ctx.Options.IndentString);
            }
        }

        /// <summary>
        /// Returns a string representing the generated code
        /// </summary>
        public string GeneratedCode
        {
            get
            {
                if (_needsUpdate)
                {
                    _needsUpdate = false;
                    _generatedCode = _builder.ToString();
                }
                return _generatedCode;
            }
        }
    }
}