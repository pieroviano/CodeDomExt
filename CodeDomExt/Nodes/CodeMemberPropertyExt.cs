using System.CodeDom;
using CodeDomExt.Helpers;
using CodeDomExt.Utils;

namespace CodeDomExt.Nodes
{
    /// <summary>
    /// Extended CodeMemberProperty
    /// </summary>
    public class CodeMemberPropertyExt : CodeMemberProperty
    {
        private AccessibilityLevel? _getAccessibilityLevel;
        private AccessibilityLevel? _setAccessibilityLevel;

        /// <summary>
        /// Accessibilit level of the getter
        /// </summary>
        public AccessibilityLevel GetAccessibilityLevel
        {
            get => _getAccessibilityLevel ?? base.Attributes.GetAccessibilityLevel();
            set => _getAccessibilityLevel = value;
        }
        /// <summary>
        /// Accessibilit level of the setter
        /// </summary>
        public AccessibilityLevel SetAccessibilityLevel
        {
            get => _setAccessibilityLevel ?? base.Attributes.GetAccessibilityLevel(); 
            set => _setAccessibilityLevel = value;
        }
        /// <summary>
        /// Initializer expression for the property
        /// </summary>
        public CodeExpression PropertyInitializer { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CodeMemberPropertyExt()
        {
        }
        
        /// <summary>
        /// Constructor setting basic properties
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hasGet"></param>
        /// <param name="hasSet"></param>
        public CodeMemberPropertyExt(string name, bool hasGet, bool hasSet)
        {
            Name = name;
            HasGet = hasGet;
            HasSet = hasSet;
        }
        
        /// <summary>
        /// Constructor setting basic properties
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hasGet"></param>
        /// <param name="hasSet"></param>
        /// <param name="propertyInitializer"></param>
        public CodeMemberPropertyExt(string name, bool hasGet, bool hasSet, CodeExpression propertyInitializer) : this(name, hasGet, hasSet)
        {
            PropertyInitializer = propertyInitializer;
        }
    }
}