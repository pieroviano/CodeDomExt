using System;
using System.Reflection;
using CodeDomExt.Helpers;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Common
{
    /// <summary>
    /// A partial implementation for a type attributes handler, will output the keywords provided by the implementation.
    /// </summary>
    /// <remarks>
    /// A type attributes handler should output the type attributes keywords on the current line, without indenting and
    /// ending with a space character if anything was generated 
    /// </remarks>
    public abstract class DefaultTypeAttributesHandler : ICodeObjectHandler<TypeAttributes>
    {
        /// <inheritdoc />
        public bool Handle(TypeAttributes obj, Context ctx)
        {
            AccessibilityLevel accessLevel = AccessibilityLevel.Default;
            try
            {
                accessLevel = obj.GetAccessibilityLevel();
            }
            catch (ArgumentOutOfRangeException)
            {
                if (ctx.Options.DoConsistencyChecks)
                {
                    throw new ConsistencyException("Invalid type attributes");
                }
            }

            if (accessLevel != AccessibilityLevel.Default)
            {
                ctx.Writer.Write(GetAccessibilityLevelKeyword(accessLevel, ctx));
                ctx.Writer.Write(" ");
            }

            if (ctx.CurrentDeclarationType == DeclarationType.Class)
            {
                if ((obj & TypeAttributes.Sealed) != 0 && (obj & TypeAttributes.Abstract) != 0 
                    && ctx.Options.DoConsistencyChecks)
                {
                    throw new ConsistencyException("A class can't be both sealed and abstract");
                }
                if ((obj & TypeAttributes.Sealed) != 0)
                {
                    if (!string.IsNullOrEmpty(GetSealedKeyword(ctx)))
                    {
                        ctx.Writer.Write(GetSealedKeyword(ctx) + " ");
                    }
                }
                if ((obj & TypeAttributes.Abstract) != 0)
                {
                    if (!string.IsNullOrEmpty(GetAbstractKeyword(ctx)))
                    {
                        ctx.Writer.Write(GetAbstractKeyword(ctx) + " ");
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Returns the accessibility level keyword for the provided accessibility level or null
        /// </summary>
        /// <param name="accessibilityLevel"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected abstract string GetAccessibilityLevelKeyword(AccessibilityLevel accessibilityLevel, Context ctx);
        /// <summary>
        /// Sealed keyword or null
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected abstract string GetSealedKeyword(Context ctx);
        /// <summary>
        /// Abstract keyword or null
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected abstract string GetAbstractKeyword(Context ctx);
    }
}