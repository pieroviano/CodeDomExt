using System;
using System.Collections.Generic;
using System.Linq;
using System.CodeDom;

namespace CodeDomExt.Helpers
{
    /// <summary>
    /// Utility class providing ways of concatenating methods invocations and property/fields access
    /// </summary>
    public static class FluentCalls
    {   
        /// <summary>
        /// Returns a new <see cref="CodeMethodInvokeExpression"/> targeting the provided expression
        /// </summary>
        /// <param name="target"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static CodeMethodInvokeExpression AndThenInvoke(this CodeExpression target, string methodName, params CodeExpression[] parameters)
        {
            return new CodeMethodInvokeExpression(
                targetObject: target,
                methodName: methodName,
                parameters: parameters
            );
        }
        
        /// <summary>
        /// Sets the method reference's target object to this, and returns a new <see cref="CodeMethodInvokeExpression"/>
        /// for the methodReference 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static CodeMethodInvokeExpression AndThenInvoke(this CodeExpression target, CodeMethodReferenceExpression method, params CodeExpression[] parameters)
        {
            method.TargetObject = target;
            return new CodeMethodInvokeExpression(
                method, parameters
            );
        }

        /// <summary>
        /// Returns a new <see cref="CodePropertyReferenceExpression"/> targeting the provided expression
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static CodePropertyReferenceExpression AndThenGet(this CodeExpression target, string propertyName)
        {
            return new CodePropertyReferenceExpression(
                targetObject: target,
                propertyName: propertyName
            );
        }

        /// <summary>
        /// Get the value of the provided expression target object if applicable.
        /// Valid CodeExpression this method can be used on are:
        /// <list type="bullet">
        /// <see cref="CodeMethodInvokeExpression"/>
        /// <see cref="CodePropertyReferenceExpression"/>
        /// <see cref="CodeFieldReferenceExpression"/>
        /// </list>
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">If the expression type is not valid</exception>
        public static CodeExpression GetFluentParent(this CodeExpression expression)
        {
            switch (expression)
            {
                case CodeMethodInvokeExpression methodInvokeExpression:
                    return methodInvokeExpression.Method.TargetObject;
                case CodeMethodReferenceExpression methodReferenceExpression:
                    return methodReferenceExpression.TargetObject;
                case CodePropertyReferenceExpression propertyReferenceExpression:
                    return propertyReferenceExpression.TargetObject;
                case CodeEventReferenceExpression eventReferenceExpression:
                    return eventReferenceExpression.TargetObject;
                case CodeFieldReferenceExpression fieldReferenceExpression:
                    return fieldReferenceExpression.TargetObject;
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Sets the value of the provided expression target object if applicable, and returns the old target object value.
        /// Valid CodeExpression this method can be used on are:
        /// <list type="bullet">
        /// <see cref="CodeMethodInvokeExpression"/>
        /// <see cref="CodePropertyReferenceExpression"/>
        /// <see cref="CodeFieldReferenceExpression"/>
        /// </list>
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">If the expression type is not valid</exception>
        public static CodeExpression SetFluentParent(this CodeExpression expression, CodeExpression target)
        {
            CodeExpression oldTarget;
            switch (expression)
            {
                case CodeMethodInvokeExpression methodInvokeExpression:
                    var method = methodInvokeExpression.Method;
                    oldTarget = method.TargetObject;
                    method.TargetObject = target;
                    break;
                case CodePropertyReferenceExpression propertyReferenceExpression:
                    oldTarget = propertyReferenceExpression.TargetObject;
                    propertyReferenceExpression.TargetObject = target;
                    break;
                case CodeFieldReferenceExpression fieldReferenceExpression:
                    oldTarget = fieldReferenceExpression.TargetObject;
                    fieldReferenceExpression.TargetObject = target;
                    break;
                default:
                    throw new ArgumentException();
            }
            return oldTarget;
        }

        /// <summary>
        /// Set the provided expression's root tergate object to this
        /// </summary>
        /// <param name="target"></param>
        /// <param name="expression"></param>
        /// <exception cref="ArgumentException">If the expression or one expressions on its left can't have a target object</exception>
        /// <returns></returns>
        public static CodeMethodInvokeExpression AndThenInvoke(this CodeExpression target, CodeMethodInvokeExpression expression)
        {
            expression.GetFluentRoot().SetFluentParent(target);
            return expression;
        }

        /// <summary>
        /// Set the provided expression's root tergate object to this
        /// </summary>
        /// <param name="target"></param>
        /// <param name="expression"></param>
        /// <exception cref="ArgumentException">If the expression or one expressions on its left can't have a target object</exception>
        /// <returns></returns>
        public static CodeExpression AndThen(this CodeExpression target, CodeExpression expression)
        {

            expression.GetFluentRoot().SetFluentParent(target);
            return expression;
        }

        /// <summary>
        /// Set the provided expression's root tergate object to this
        /// </summary>
        /// <param name="target"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static CodePropertyReferenceExpression AndThenGet(this CodeExpression target, CodePropertyReferenceExpression expression)
        {
            expression.GetFluentRoot().SetFluentParent(target);
            return expression;
        }

        /// <summary>
        /// Get the root of this expression (what would be the leftmost expression on generated code).
        /// </summary>
        /// <param name="expression"></param>
        /// <exception cref="ArgumentException">If this or one expressions on the left of this can't have a target object</exception>
        /// <returns></returns>
        public static CodeExpression GetFluentRoot(this CodeExpression expression)
        {
            CodeExpression current = expression;
            for (; current.GetFluentParent() != null; current = current.GetFluentParent()) { }
            return current;
        }

        private static IEnumerable<CodeExpression> GetFluentCallsImpl(CodeExpression expression)
        {
            CodeExpression current = expression;
            for (; current.GetFluentParent() != null; current = current.GetFluentParent())
            {
                yield return current;
            }
            yield return current;
        }

        /// <summary>
        /// Get the ordered list of calls (where this is the last one)
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
#if NET35 || NET40
        public static IList<CodeExpression> GetFluentCalls(this CodeExpression expression)
#else
        public static IReadOnlyList<CodeExpression> GetFluentCalls(this CodeExpression expression)
#endif
        {
            return GetFluentCallsImpl(expression).Reverse().ToList();
        }
    }
}