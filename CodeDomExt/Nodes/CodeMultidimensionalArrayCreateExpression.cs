using System;
using System.CodeDom;
using System.Collections.Generic;

namespace CodeDomExt.Nodes
{
    /// <summary>
    /// Expression for creating a multidimensional array (matrix)
    /// </summary>
    public class CodeMultidimensionalArrayCreateExpression : CodeExpression
    {
        /// <summary>
        /// BaseType of the array 
        /// </summary>
        public CodeTypeReference CreateType { get; set; }
        /// <summary>
        /// Expressions for the size of the array
        /// </summary>
        public List<CodeExpression> SizeExpressions { get; } = new List<CodeExpression>(); 
        /// <summary>
        /// Rank of the created array, ignored if SizeExpression has any element
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// Expression initializing hte array
        /// </summary>
        public CodeArrayInitializerExpression InitializerExpression { get; set; }

        /// <summary>
        /// Array with explicit size
        /// </summary>
        /// <param name="createType"></param>
        /// <param name="sizeExpressions"></param>
        public CodeMultidimensionalArrayCreateExpression(CodeTypeReference createType,
            params CodeExpression[] sizeExpressions) : this(createType, 0, sizeExpressions, null)
        {
        }

        /// <summary>
        /// Array with implicit size and initialization
        /// </summary>
        /// <param name="createType"></param>
        /// <param name="rank"></param>
        /// <param name="initializerExpression"></param>
        public CodeMultidimensionalArrayCreateExpression(CodeTypeReference createType, 
            int rank, CodeArrayInitializerExpression initializerExpression) 
            : this(createType, rank, new CodeExpression[] { }, initializerExpression)
        {
        }

        /// <summary>
        /// Array with explicit size and initialization 
        /// </summary>
        /// <param name="createType"></param>
        /// <param name="sizeExpressions"></param>
        /// <param name="initializerExpression"></param>
        public CodeMultidimensionalArrayCreateExpression(CodeTypeReference createType,
            IEnumerable<CodeExpression> sizeExpressions, CodeArrayInitializerExpression initializerExpression)
            : this (createType, 0, sizeExpressions, initializerExpression)
        {
        }

        private CodeMultidimensionalArrayCreateExpression(CodeTypeReference createType, int rank,
            IEnumerable<CodeExpression> sizeExpressions, CodeArrayInitializerExpression initializerExpression)
        {
            CreateType = createType;
            Rank = rank;
            SizeExpressions.AddRange(sizeExpressions);
            InitializerExpression = initializerExpression;
        }
    }

    /// <summary>
    /// Expression for multidimensional array initialization
    /// </summary>
    public class CodeArrayInitializerExpression : CodeExpression
    {
        /// <summary>
        /// Array values
        /// </summary>
        public List<CodeExpression> Expressions { get; } = new List<CodeExpression>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="expressions"></param>
        public CodeArrayInitializerExpression(params CodeExpression[] expressions)
        {
            Expressions.AddRange(expressions);
        }
    }
}