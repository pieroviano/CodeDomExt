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
        /// <param name="textWriter"></param>
        public void Generate(CodeCompileUnit compileUnit, TextWriter textWriter)
        {
            Generate(compileUnit, new TextWriterAdapter(textWriter));
        }

        /// <summary>
        /// Generate code from provided compile unit and returns it as a string
        /// </summary>
        /// <param name="compileUnit"></param>
        /// <returns></returns>
        public string GenerateString(CodeCompileUnit compileUnit)
        {
            StringCodeWriter codeWriter = new StringCodeWriter();
            Generate(compileUnit, codeWriter);
            return codeWriter.GeneratedCode;
        }
    }
}