using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace CodeDomExt.Nodes
{
    /// <summary>
    /// Extension for object create expression
    /// </summary>
    public class CodeObjectCreateExpressionExt : CodeObjectCreateExpression
    {
        /// <summary>
        /// Expression initializing the object properties
        /// </summary>
        public List<CodePropertyInitializerExpression> PropertyInitializers { get; } =
            new List<CodePropertyInitializerExpression>();

        /// <summary>
        /// Constructor
        /// </summary>
        public CodeObjectCreateExpressionExt()
        {
        }
        
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="createType"></param>
        /// <param name="parameters"></param>
        /// <param name="properties"></param>
        public CodeObjectCreateExpressionExt(Type createType, IEnumerable<CodeExpression> parameters, params CodePropertyInitializerExpression[] properties)
            : base(createType, parameters.ToArray())
        {
            PropertyInitializers.AddRange(properties);
        }
                
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="createType"></param>
        /// <param name="parameters"></param>
        /// <param name="properties"></param>
        public CodeObjectCreateExpressionExt(CodeTypeReference createType, IEnumerable<CodeExpression> parameters, params CodePropertyInitializerExpression[] properties)
            : base(createType, parameters.ToArray())
        {
            PropertyInitializers.AddRange(properties);
        }
                
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="createType"></param>
        /// <param name="parameters"></param>
        /// <param name="properties"></param>
        public CodeObjectCreateExpressionExt(string createType, IEnumerable<CodeExpression> parameters, params CodePropertyInitializerExpression[] properties)
            : base(createType, parameters.ToArray())
        {
            PropertyInitializers.AddRange(properties);
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class CodePropertyInitializerExpression : CodeExpression
    {
        /// <summary>
        /// Name of the property to be set
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// Expression for initializing the property
        /// </summary>
        public CodeExpression InitializerExpression { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="initializerExpression"></param>
        /// <exception cref="ArgumentException"></exception>
        public CodePropertyInitializerExpression(string propertyName, CodeExpression initializerExpression)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException($"{nameof(propertyName)} can't be empty");
            }
            PropertyName = propertyName;
            InitializerExpression = initializerExpression ?? throw new ArgumentNullException(nameof(initializerExpression));
        }
    }
}