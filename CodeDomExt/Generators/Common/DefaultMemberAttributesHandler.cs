using System;
using System.CodeDom;
using System.Reflection;
using CodeDomExt.Helpers;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Common
{
    /// <summary>
    /// A partial implementation for a member attributes handler, will output the keywords provided by the implementation.
    /// </summary>
    /// <remarks>
    /// A member attributes handler should output the member attributes keywords on the current line, without indenting and
    /// ending with a space character if anything was generated 
    /// </remarks>
    public abstract class DefaultMemberAttributesHandler : ICodeObjectHandler<MemberAttributes>
    {
        /// <inheritdoc />
        public bool Handle(MemberAttributes obj, Context ctx)
        {
            ctx.IsMemberAbstract = false;
            if (ctx.CurrentDeclarationType != DeclarationType.Interface)
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
                        throw new ConsistencyException("Invalid access level for member attributes");
                    }
                }

                if (accessLevel != AccessibilityLevel.Default)
                {
                    ctx.Writer.Write(GetAccessibilityLevelKeyword(accessLevel, ctx));
                    ctx.Writer.Write(" ");
                }
            }

            if (ctx.CurrentTypeMember != MemberTypes.Constructor)
            {
                if ((obj & MemberAttributes.VTableMask) == MemberAttributes.New && !string.IsNullOrEmpty(GetNewKeyword(ctx)))
                {
                    ctx.Writer.Write(GetNewKeyword(ctx));
                    ctx.Writer.Write(" ");
                }
            }

            if ((obj & MemberAttributes.Overloaded) != 0 && !string.IsNullOrEmpty(GetOverloadedKeyword(ctx))) //TODO auto overload
            {
                if (ctx.CurrentTypeMember == MemberTypes.Method)
                {
                    ctx.Writer.Write(GetOverloadedKeyword(ctx));
                    ctx.Writer.Write(" ");
                }
            }

            if (ctx.CurrentDeclarationType != DeclarationType.Interface && ctx.CurrentTypeMember != MemberTypes.Constructor)
            {
                if ((obj & MemberAttributes.ScopeMask) == MemberAttributes.Abstract)
                {
                    if (ctx.CurrentTypeMember != MemberTypes.Field
                        && ctx.CurrentTypeMember != MemberTypes.Event
                        && !string.IsNullOrEmpty(GetAbstractKeyword(ctx)))
                    {
                        ctx.IsMemberAbstract = true;
                        ctx.Writer.Write(GetAbstractKeyword(ctx) + " ");
                    }
                }
                else if ((obj & MemberAttributes.ScopeMask) == MemberAttributes.Override)
                {
                    if (ctx.CurrentTypeMember != MemberTypes.Field
                        && ctx.CurrentTypeMember != MemberTypes.Event
                        && !string.IsNullOrEmpty(GetOverrideKeyword(ctx)))
                    {
                        ctx.Writer.Write(GetOverrideKeyword(ctx) + " ");
                    }
                }
                else if ((obj & MemberAttributes.ScopeMask) == MemberAttributes.Static)
                {
                    if (!string.IsNullOrEmpty(GetStaticKeyword(ctx)))
                    {
                        ctx.Writer.Write(GetStaticKeyword(ctx) + " ");
                    }
                }
                else if ((obj & MemberAttributes.ScopeMask) == MemberAttributes.Const)
                {
                    if (ctx.CurrentTypeMember == MemberTypes.Field
                        && !string.IsNullOrEmpty(GetConstKeyword(ctx)))
                    {
                        ctx.Writer.Write(GetConstKeyword(ctx) + " ");
                    }
                }
                else if ((obj & MemberAttributes.ScopeMask) == MemberAttributes.Final)
                {
                    if (ctx.CurrentTypeMember != MemberTypes.Field
                        && ctx.CurrentTypeMember != MemberTypes.Event
                        && !string.IsNullOrEmpty(GetFinalKeyword(ctx)))
                    {
                        ctx.Writer.Write(GetFinalKeyword(ctx) + " ");
                    }
                }
                else if ((obj & MemberAttributes.ScopeMask) == 0 || !ctx.Options.DoConsistencyChecks)
                {
                    if (ctx.CurrentTypeMember != MemberTypes.Field &&
                        !string.IsNullOrEmpty(GetNotFinalKeyword(ctx)))
                    {
                        ctx.Writer.Write(GetNotFinalKeyword(ctx) + " ");
                    }
                }
                else
                {
                    throw new ConsistencyException("Invalid scope for member attribute");
                }
            }
            //TODO readonly

            return true;
        }

        /// <summary>
        /// Returns the string representing the keyword for the provided accessibility level
        /// </summary>
        /// <param name="accessibilityLevel"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected abstract string GetAccessibilityLevelKeyword(AccessibilityLevel accessibilityLevel, Context ctx);
        /// <summary>
        /// Returns the string representing the keyword for shadowing
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected abstract string GetNewKeyword(Context ctx);
        /// <summary>
        /// Returns the string representing the keyword for abstract members
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected abstract string GetAbstractKeyword(Context ctx);
        /// <summary>
        /// Returns the string representing the keyword for members overriding base class (or implemented interfaces) members
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected abstract string GetOverrideKeyword(Context ctx);
        /// <summary>
        /// Returns the string representing the keyword for static members
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected abstract string GetStaticKeyword(Context ctx);
        /// <summary>
        /// Returns the string representing the keyword for costant members
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected abstract string GetConstKeyword(Context ctx);
        /// <summary>
        /// Returns the string representing the keyword for final members
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected abstract string GetFinalKeyword(Context ctx);
        /// <summary>
        /// Returns the string representing the keyword for not final (virtual) members 
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected abstract string GetNotFinalKeyword(Context ctx);
        /// <summary>
        /// Returns the string representing the keyword for members overloading other members
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected abstract string GetOverloadedKeyword(Context ctx);
    }
}