using System;
using System.CodeDom;
using System.Linq;
using CodeDomExt.Utils;

namespace CodeDomExt.Generators.Common
{
    /// <summary>
    /// A partial implementation of a custom attributes handler. Full implementations need to specify how to wrap the attribute,
    /// its arguments and how to handle name attribute arguments
    /// </summary>
    /// <remarks>
    /// An attribute declaration handler should handle the attributes without indenting nor finishing with a new line or whitespace.
    /// It should also handle wrapping the attributes declaration with the appropiate characters ([] for c# and
    /// &lt;&gt; for vb)
    /// </remarks>
    public abstract class DefaultAttributeDeclarationHandler : ICodeObjectHandler<CodeAttributeDeclaration>
    {
        private readonly AttributeArgumentHandler _argumentHandler;

        /// <summary></summary>
        /// <param name="assignmentSymbol">
        /// The symbol that will be used to assign a value to a named attribute argument (= in c#, := in vb.net)
        /// </param>
        /// <exception cref="ArgumentException">assignmentSymbol is null or empty</exception>
        protected DefaultAttributeDeclarationHandler(string assignmentSymbol)
        {
            if (string.IsNullOrEmpty(assignmentSymbol))
            {
                throw new ArgumentException(nameof(assignmentSymbol));
            }
            _argumentHandler = new AttributeArgumentHandler(assignmentSymbol, this);
        }

        /// <inheritdoc />
        public bool Handle(CodeAttributeDeclaration obj, Context ctx)
        {
            //TODO specify attribute target [target: attribute] [method: ValidatedContract]
            
            WrapAttributeDeclaration((c) =>
            {
                c.Writer.Write(AsId(obj.Name));
                if (obj.Arguments.Count > 0)
                {
                    WrapAttributeParameters((c2) =>
                    {
                        GeneralUtils.HandleCollectionCommaSeparated(
                            obj.Arguments.Cast<CodeAttributeArgument>(), _argumentHandler, c2);
                    }, c);
                }
            }, ctx);
            return true;
        }

        private class AttributeArgumentHandler : ICodeObjectHandler<CodeAttributeArgument>
        {
            private readonly string _assignmentSymbol;
            private readonly DefaultAttributeDeclarationHandler _parent;
            public AttributeArgumentHandler(string assignmentSymbol, DefaultAttributeDeclarationHandler parent)
            {
                _assignmentSymbol = assignmentSymbol;
                _parent = parent;
            }
            public bool Handle(CodeAttributeArgument obj, Context ctx)
            {
                if (!string.IsNullOrEmpty(obj.Name))
                {
                    ctx.Writer.Write($"{_parent.AsId(obj.Name)}{_assignmentSymbol}");
                }

                ctx.HandlerProvider.ExpressionHandler.Handle(obj.Value, ctx);
                return true;
            }
        }

        /// <summary>
        /// Wraps the attribute declaration
        /// </summary>
        /// <param name="attributeHandlingAction">action handling the attribute declaration</param>
        /// <param name="ctx"></param>
        protected abstract void WrapAttributeDeclaration(Action<Context> attributeHandlingAction, Context ctx);
        /// <summary>
        /// Wraps the attribute declaration parameters
        /// </summary>
        /// <param name="attributeParametersHandlingAction">action handling the attribute declaration parameters</param>
        /// <param name="ctx"></param>
        protected abstract void WrapAttributeParameters(Action<Context> attributeParametersHandlingAction, Context ctx);
        /// <summary>
        /// Returns the provided string as a valid identifier for current language
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected abstract string AsId(string s);
    }
}