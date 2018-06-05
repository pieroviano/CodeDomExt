using System.CodeDom;
using System.IO;

namespace CodeDomExt.Generators
{
    /// <summary>
    /// Class responsible for generating code from a <see cref="CodeCompileUnit"/>
    /// </summary>
    public abstract class CodeGenerator
    {
        /// <summary>
        /// Options for code generation
        /// </summary>
        public GeneratorOptions Options { get; set; }

        /// <summary>
        /// Generate code from provided compile unit, outputting on provided code writer
        /// </summary>
        /// <param name="compileUnit"></param>
        /// <param name="codeWriter"></param>
        public abstract void Generate(CodeCompileUnit compileUnit, ICodeWriter codeWriter);

        /// <summary>
        /// Generate code from provided compile unit, outputting on provided stream writer
        /// </summary>
        /// <param name="compileUnit"></param>
        /// <param name="streamWriter"></param>
        public void Generate(CodeCompileUnit compileUnit, StreamWriter streamWriter)
        {
            Generate(compileUnit, new StreamWriterAdapter(streamWriter));
        }
    }
}