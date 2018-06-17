using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CodeDomExt.Generators;

namespace CodeDomExt.Utils
{
    /// <summary>
    /// Utility class used by different code generator types
    /// </summary>
    public static class GeneralUtils
    {
        /// <summary>
        /// Returns the DeclarationType of the provided CodeTypeDeclaration.
        /// </summary>
        /// <param name="obj">The CodeTypeDeclaration of which you want to know the DeclarationType</param>
        /// <param name="ctx">Current context</param>
        /// <returns>The DeclarationType of the provided CodeTypeDeclaration.</returns>
        /// <exception cref="ArgumentException">If consistency checks are enabled and the provided object DeclarationType is ambiguous</exception>
        public static DeclarationType CheckAndGetDeclarationType(CodeTypeDeclaration obj, Context ctx)
        {
            if (obj is CodeTypeDelegate)
            {
                return DeclarationType.Delegate;
            }
            else if (obj.IsEnum)
            {
                return DeclarationType.Enum;
            }
            else if (obj.IsInterface)
            {
                return DeclarationType.Interface;
            }
            else if (obj.IsStruct)
            {
                return DeclarationType.Struct;
            }
            else
            {
                return DeclarationType.Class;
            }
        }

        /// <summary>
        /// Handle a collection with the provided handler.
        /// <para/>
        /// Will execute <paramref name="preAction"/> before every handling
        /// <para/>
        /// Will execute <paramref name="postAction"/> after every handling apart from the last one unless
        /// <paramref name="doPostActionOnLast"/> is true
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="handler"></param>
        /// <param name="ctx"></param>
        /// <param name="preAction"></param>
        /// <param name="postAction"></param>
        /// <param name="doPostActionOnLast"></param>
        /// <typeparam name="T"></typeparam>
        public static void HandleCollection<T>(IEnumerable<T> coll, ICodeObjectHandler<T> handler, Context ctx,
            Action<Context> preAction = null, Action<Context> postAction = null, bool doPostActionOnLast = true)
        {
            using (var enumerator = coll.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return;

                preAction?.Invoke(ctx);
                handler.Handle(enumerator.Current, ctx);
                while (enumerator.MoveNext())
                {
                    postAction?.Invoke(ctx);
                    preAction?.Invoke(ctx);
                    handler.Handle(enumerator.Current, ctx);
                }

                if (doPostActionOnLast)
                {
                    postAction?.Invoke(ctx);
                }
            }
        }

        /// <summary>
        /// Handles a collection with the provided handler, separating each handling with a ", "
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="handler"></param>
        /// <param name="ctx"></param>
        /// <typeparam name="T"></typeparam>
        public static void HandleCollectionCommaSeparated<T>(IEnumerable<T> coll, ICodeObjectHandler<T> handler,
            Context ctx)
        {
            HandleCollection(coll, handler, ctx, postAction:(c) => c.Writer.Write(", "), doPostActionOnLast: false);
        }

        /// <summary>
        /// Handles a collection with the provided handler, handling each object on a single line mantaining the current
        /// level of indentation, ending with a new line.
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="handler"></param>
        /// <param name="ctx"></param>
        /// <param name="doIndentAsPreAction">If it is set to true line indentation is handled as pre-action,
        /// otherwise it is done as postaction, after the newline</param>
        /// <typeparam name="T"></typeparam>
        public static void HandleCollectionOnMultipleLines<T>(IEnumerable<T> coll, ICodeObjectHandler<T> handler,
            Context ctx, bool doIndentAsPreAction)
        {
            HandleCollection(coll, handler, ctx,
                preAction: doIndentAsPreAction ? (c) => { c.Writer.Indent(c); } : (Action<Context>) null,
                postAction: doIndentAsPreAction ? (c) => { c.Writer.NewLine(); }
                    : (Action<Context>) ((c) =>
                    {
                        c.Writer.NewLine();
                        c.Writer.Indent(c);
                    }), doPostActionOnLast: true);
        }

        /// <summary>
        /// Handles a code snippet; al snippet lines will begin at the current level of Indentation
        /// </summary>
        /// <param name="snippet"></param>
        /// <param name="ctx"></param>
        public static void HandleSnippet(string snippet, Context ctx)
        {
            string[] result = Regex.Split(snippet, "\r\n|\r|\n");
            if (result.Length < 1)
            {
                return;
            }
            
            int i = 0;
            ctx.Writer.Write(result[i]);
            
            for (i = 1; i < result.Length; i++)
            {
                ctx.Writer.NewLine();
                ctx.Writer.IndentAndWrite(result[i], ctx);
            }
        }

        /// <summary>
        /// Return if the provided type reference is null or referce to System.Void type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNullOrVoidType(CodeTypeReference obj)
        {
            return obj == null || obj.BaseType == typeof(void).FullName;
        }

        /// <summary>
        /// Apply a mask on the provided member attributes
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="keepAccessibilityLevel">if true will keep the original accessibility level information (else it will be set to 0)</param>
        /// <param name="keepVTable">if true will keep the original vtable information (else it will be set to 0)</param>
        /// <param name="keepScope">if true will keep the original scope information (else it will be set to 0)</param>
        /// <param name="forceFinal">if true the scope information will be set to final</param>
        /// <exception cref="ArgumentException">if keepScope and force final are both true</exception>
        /// <returns></returns>
        public static MemberAttributes GetMaskedMemberAttributes(MemberAttributes obj,
            bool keepAccessibilityLevel, bool keepVTable, bool keepScope, bool forceFinal = false)
        {
            if (forceFinal && keepScope)
            {
                throw new ArgumentException($"{nameof(keepScope)} and {nameof(forceFinal)} can't be both true");
            }
            MemberAttributes mask = 0;
            MemberAttributes forceFinalMask = forceFinal ? MemberAttributes.Final : 0;
            if (keepAccessibilityLevel)
            {
                mask |= MemberAttributes.AccessMask;
            }
            if (keepVTable)
            {
                mask |= MemberAttributes.VTableMask;
            }
            if (keepScope)
            {
                mask |= MemberAttributes.ScopeMask;
            }

            return (mask & obj) | forceFinalMask;
        }
        
        /// <summary>
        /// If the provided code expression is one of the provided types or if generator option
        /// <see cref="GeneratorOptions.RemoveRedundantParenthesis"/> is false it will be wrapped in parentheses
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ctx"></param>
        /// <param name="types"></param>
        public static void WrapIfIsTypeAndHandle(CodeExpression obj, Context ctx, params Type[] types)
        {
            WrapIfIsTypeAndHandle(obj, ctx, types, "(", ")");
        }

        /// <summary>
        /// If the provided code expression is one of the provided types or if generator option
        /// <see cref="GeneratorOptions.RemoveRedundantParenthesis"/> is false it will be wrapped with the provided strings
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ctx"></param>
        /// <param name="types"></param>
        /// <param name="preHandleWrapString"></param>
        /// <param name="postHandleWrapString"></param>
        public static void WrapIfIsTypeAndHandle(CodeExpression obj, Context ctx, IEnumerable<Type> types, 
            string preHandleWrapString = "(", string postHandleWrapString = ")")
        {
            bool needsWrapping = true;
            
            if (ctx.Options.RemoveRedundantParenthesis) {
                Type objType = obj.GetType();
                needsWrapping = types.Any((type) => type.IsAssignableFrom(objType));
            }
            
            if (needsWrapping)
            {
                ctx.Writer.Write(preHandleWrapString);
            }

            ctx.HandlerProvider.ExpressionHandler.Handle(obj, ctx);
            
            if (needsWrapping)
            {
                ctx.Writer.Write(postHandleWrapString);
            }
        }

        /// <summary>
        /// If type string has generic types number (indicated by a ` followed by the number of generic types), returns
        /// the string without that, otherwise returns the string. 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string StripGenericTypeArgumentsNumber(this string s)
        {
            int backtickpos = s.IndexOf("`", StringComparison.Ordinal);
            return backtickpos >= 0 ? s.Substring(0, backtickpos) : s;
        }

        /// <summary>
        /// Escapes the provided string by replacing occurencies of the provided characters to be escaped with the same character preceded by the provided escape character
        /// </summary>
        /// <param name="s"></param>
        /// <param name="escapeCharacter"></param>
        /// <param name="toBeEscaped"></param>
        /// <returns></returns>
        public static string EscapeString(string s, char escapeCharacter, params char[] toBeEscaped)
        {
            StringBuilder res = new StringBuilder();
            var toEscapeAsSet = new HashSet<char>(toBeEscaped);
            foreach (var c in s)
            {
                if (toEscapeAsSet.Contains(c))
                {
                    res.Append(escapeCharacter);
                }

                res.Append(c);
            }

            return res.ToString();
        }
    }
}