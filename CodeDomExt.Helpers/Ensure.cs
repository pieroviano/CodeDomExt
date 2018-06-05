using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeDomExt.Helpers;

namespace CodeDomExt.Helpers
{
    /// <summary>
    /// Utility class providing methods useful for validating codedome nodes creation
    /// </summary>
    public static class Ensure
    {
        /// <summary>
        /// Checks if the type has a member with the provided name, and returns it
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <exception cref="InvalidOperationException">If no suitable member was found</exception>
        /// <returns></returns>
        public static string EnsureMemberExists(this Type type, string memberName)
        {
            return AllMembers(type).First(it => it.Name.Equals(memberName)).Name;
        }
        
        /// <summary>
        /// Checks if the type has a method with the provided name, and returns it
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <exception cref="InvalidOperationException">If no suitable member was found</exception>
        /// <returns></returns>
        public static string EnsureMethodExists(this Type type, string methodName)
        {
            return AllMembers(type).First(it => it.Name.Equals(methodName) 
                                                && (it.MemberType & MemberTypes.Method) != 0).Name;
        }
        
        /// <summary>
        /// Checks if the type has a field with the provided name, and returns it
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <exception cref="InvalidOperationException">If no suitable member was found</exception>
        /// <returns></returns>
        public static string EnsureFieldExists(this Type type, string fieldName)
        {
            return AllMembers(type).First(it => it.Name.Equals(fieldName)
                                                && (it.MemberType & MemberTypes.Field) != 0).Name;
        }
        
        /// <summary>
        /// Checks if the type has a property with the provided name, and returns it
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <exception cref="InvalidOperationException">If no suitable member was found</exception>
        /// <returns></returns>
        public static string EnsurePropertyExists(this Type type, string propertyName)
        {
            return AllMembers(type).First(it => it.Name.Equals(propertyName) && (it.MemberType & MemberTypes.Property) != 0).Name;
        }

        private static IEnumerable<MemberInfo> AllMembers(Type type)
        {
            return type.GetMembers(BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Instance |
                                   BindingFlags.Public | BindingFlags.NonPublic);
        }
    }
}