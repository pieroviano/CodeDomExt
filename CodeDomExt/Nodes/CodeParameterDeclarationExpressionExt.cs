using System;
using System.CodeDom;

namespace CodeDomExt.Nodes
{
    /// <summary>
    /// Extension for a codeParameterDeclarationExpression. Allows usage of default value
    /// </summary>
    public class CodeParameterDeclarationExpressionExt : CodeParameterDeclarationExpression
    {
        /// <summary>
        /// Default value for the provided expression
        /// </summary>
        public CodeExpression DefaultValue { get; set; }
        
        /// <summary>
        /// Set to true to mark this parameter declaration as vararrgs (params). Note that this will NOT automatically set
        /// the parameter type to an array
        /// </summary>
        public bool IsVarargs { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public CodeParameterDeclarationExpressionExt()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public CodeParameterDeclarationExpressionExt(CodeTypeReference type, string name) : base(type, name)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public CodeParameterDeclarationExpressionExt(string type, string name) : base(type, name)
        {
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public CodeParameterDeclarationExpressionExt(Type type, string name) : base(type, name)
        {
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        public CodeParameterDeclarationExpressionExt(CodeTypeReference type, string name, CodeExpression defaultValue) : base(type, name)
        {
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        public CodeParameterDeclarationExpressionExt(string type, string name, CodeExpression defaultValue) : base(type, name)
        {
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        public CodeParameterDeclarationExpressionExt(Type type, string name, CodeExpression defaultValue) : base(type, name)
        {
            DefaultValue = defaultValue;
        }
    }
}