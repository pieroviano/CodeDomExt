using System;

namespace CodeDomExt.Generators
{
    /// <summary>
    /// Options for CodeGenerator generation
    /// </summary>
    public class GeneratorOptions
    {
        private string _indentString = "    ";

        /// <summary>
        /// String used for indentation
        /// </summary>
        public string IndentString
        {
            get => _indentString;
            set => _indentString = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Partially implemented; if true, during code generation, an exception will be thrown when encountering ambiguities,
        /// for example private interface members, methods in enums, ...
        /// </summary>
        public bool DoConsistencyChecks { get; set; } = false;
        /// <summary>
        /// Not currently implemented (always false); will warn the user when identifiers do not respect the output language
        /// </summary>
        public bool DoNamingConventionChecks { get; set; } = false;
        /// <summary>
        /// Not currently implemented (always true); if true will always use blocks (in ifs, whiles, ...) even if there is
        /// a single statement  
        /// </summary>
        public bool UseBlockWhenSingleStatement { get; set; } = true;
        /// <summary>
        /// Always use fully qualified names for types regardless of imports or current namespace (does not apply to
        /// built-in/keyword types)
        /// </summary>
        public bool AlwaysUseFullyQualifiedName { get; set; } = false;
    }
}