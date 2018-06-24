using System.CodeDom;
using System.Collections.Generic;
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
        /// <param name="contextExtras">Items that will be added to context's user data; must be all of different types</param>
        public abstract void Generate(CodeCompileUnit compileUnit, ICodeWriter codeWriter, params object[] contextExtras);

        /// <summary>
        /// Generate code from provided compile unit, outputting on provided stream writer
        /// </summary>
        /// <param name="compileUnit"></param>
        /// <param name="textWriter"></param>
        /// <param name="contextExtras">Items that will be added to context's user data; must be all of different types</param>
        public void Generate(CodeCompileUnit compileUnit, TextWriter textWriter, params object[] contextExtras)
        {
            Generate(compileUnit, new TextWriterAdapter(textWriter), contextExtras);
        }

        /// <summary>
        /// Generate code from provided compile unit and returns it as a string
        /// </summary>
        /// <param name="compileUnit"></param>
        /// <param name="contextExtras">Items that will be added to context's user data; must be all of different types</param>
        /// <returns></returns>
        public string GenerateString(CodeCompileUnit compileUnit, params object[] contextExtras)
        {
            StringCodeWriter codeWriter = new StringCodeWriter();
            Generate(compileUnit, codeWriter, contextExtras);
            return codeWriter.GeneratedCode;
        }
    }
}