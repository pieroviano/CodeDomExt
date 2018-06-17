using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace CodeDomExt.Helpers
{
    /// <summary>
    /// Factory for chained <see cref="CodeConditionStatement"/>
    /// </summary>
    public static class ElseIf
    {
        /// <summary>
        /// Create a new CodeCondition statement. Each tuple in condition is considered as an if or else if statement where the
        /// test expression is the first item of the tuple and the relative statements are the second item of the tuple.
        /// <para/>
        /// The conditions are considered in order, so the first one will be the if, the second one will be the first else if, ...
        /// <para/>
        /// There is no else block
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public static CodeConditionStatement New(params Tuple<CodeExpression, IEnumerable<CodeStatement>>[] conditions)
        {
            return New(new CodeStatement[] { }, conditions);
        }

        /// <summary>
        /// Create a new CodeCondition statement. Each tuple in condition is considered as an if or else if statement where the
        /// test expression is the first item of the tuple and the relative statements are the second item of the tuple.
        /// <para/>
        /// The conditions are considered in order, so the first one will be the if, the second one will be the first else if, ...
        /// </summary>
        /// <param name="elseStatements">statements in the else block</param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public static CodeConditionStatement New(IEnumerable<CodeStatement> elseStatements,
            params Tuple<CodeExpression, IEnumerable<CodeStatement>>[] conditions)
        {
            if (conditions.Length < 1)
            {
                throw new ArgumentException();
            }
            
            CodeConditionStatement res = new CodeConditionStatement();
            CodeConditionStatement prev = res;
            for (int i = 0; i < conditions.Length; i++)
            {
                prev.Condition = conditions[i].Item1;
                prev.TrueStatements.AddRange(conditions[i].Item2.ToArray());
                if (i < conditions.Length - 1)
                {
                    CodeConditionStatement tmp = new CodeConditionStatement();
                    prev.FalseStatements.Add(tmp);
                    prev = tmp;
                }
                else
                {
                    // ReSharper disable once PossibleMultipleEnumeration
                    prev.FalseStatements.AddRange(elseStatements.ToArray());
                }
            }

            return res;
        }
    }
}