using System.CodeDom;
using System.Linq;
using CodeDomExt.Generators;

namespace CodeDomExt.Utils
{
    /// <summary>
    /// Utility class for consistency checks
    /// </summary>
    public static class ConsistencyChecks
    {
        private static int CountTrue(params bool[] values)
        {
            return values.Count(b => b);
        }
        
        /// <summary>
        /// Does consistency checks on a codeTypeDeclaration type
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ctx"></param>
        /// <exception cref="ConsistencyException"></exception>
        public static void DoTypeCheck(CodeTypeDeclaration obj, Context ctx)
        {
            if (ctx.Options.DoConsistencyChecks)
            {
                int count = CountTrue(obj.IsClass, obj.IsEnum, obj.IsStruct, obj.IsInterface);
                if (obj is CodeTypeDelegate && count != 0)
                {
                    throw new ConsistencyException(
                        $"CodeTypeDeclaration {obj.Name} is a CodeTypeDelegate and shouldn't have any of the flag IsClass, IsEnim, IsStruct and IsInterface enabled");
                }
                else if (count != 1)
                {
                    throw new ConsistencyException(
                        $"CodeTypeDeclaration {obj.Name} must have 1 true value in IsClass, IsEnum, IsStruct or IsInterface, but it has {count}");
                }
            }
        }

        /// <summary>
        /// Does consistency checks on an enum typeDeclaration
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ctx"></param>
        /// <exception cref="ConsistencyException"></exception>
        public static void EnumConsistencyChecks(CodeTypeDeclaration obj, Context ctx)
        {
            if (ctx.Options.DoConsistencyChecks)
            {
                if (!obj.Members.Cast<CodeTypeMember>().All((o) => o is CodeMemberField))
                {
                    throw new ConsistencyException($"Enum {obj.Name} has members that are not fields!");
                }

                if (obj.TypeParameters.Count > 0)
                {
                    throw new ConsistencyException($"Enum {obj.Name} has typeParameters!");
                }

                if (obj.BaseTypes.Count > 0)
                {
                    throw new ConsistencyException($"Enum {obj.Name} has base types!");
                }
            }
        }
    }
}