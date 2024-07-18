using System;
using System.CodeDom;

namespace CodeDomExt.Generators.Common
{
    /// <summary>
    /// A partial implementation of a directive handler, handles region directives using the instructions provided by the complete implementation 
    /// </summary>
    /// <remarks>
    /// A member attributes handler should output the member attributes keywords on the current line, without indenting and
    /// ending without any spaces nor newlines characters
    /// </remarks>
    public abstract class DefaultDirectiveHandler : DynamicDispatchHandler<CodeDirective>
    {
        /// <inheritdoc />
        protected override bool DoDynamicHandle(CodeDirective obj, Context ctx)
        {
            return HandleDynamic(obj as CodeRegionDirective, ctx);
        }

        private bool HandleDynamic(CodeRegionDirective obj, Context ctx)
        {
            bool res = true;
            switch (obj.RegionMode)
            {
                case CodeRegionMode.None:
                    break;
                case CodeRegionMode.Start:
                    res = WriteIfNotNullOrEmpty(GetRegionStartString(obj.RegionText), ctx);
                    break;
                case CodeRegionMode.End:
                    res = WriteIfNotNullOrEmpty(GetRegionEndString(), ctx);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return res;
        }

        private bool WriteIfNotNullOrEmpty(string s, Context ctx)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            ctx.Writer.Write(s);
            return true;
        }
        
        /// <summary>
        /// Returns the string for the region start directive. If it is null or empty the directive will be considered as not handled.
        /// </summary>
        /// <param name="regionText">the name of the region</param>
        /// <returns></returns>
        protected abstract string GetRegionStartString(string regionText);
        /// <summary>
        /// Returns the string for the region end directive. If it is null or empty the directive will be considered as not handled.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetRegionEndString();
    }
}