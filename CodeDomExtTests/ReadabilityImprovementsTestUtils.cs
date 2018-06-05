using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using CodeDomExt.Helpers;

namespace CodeDomExtTests
{
    public static class ReadabilityImprovementsTestUtils
    {
        public static CodeCompileUnit TestElseIfCodeCompileUnit()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("Test.Namespace");
            CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration("TestClass")
            {
                IsClass = true
            };
            classDeclaration.TypeAttributes =
                (classDeclaration.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.Public;

            var method = new CodeMemberMethod()
            {
                Name = "Method",
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            method.Statements.Add(
                ElseIf.New(
                    new Tuple<CodeExpression, IEnumerable<CodeStatement>>(
                        Primitives.Bool(true), new CodeStatement[] {new CodeMethodReturnStatement()})));
            method.Statements.Add(
                ElseIf.New(
                    new CodeStatement[] {new CodeMethodReturnStatement()},
                    new Tuple<CodeExpression, IEnumerable<CodeStatement>>(
                        Primitives.Int(1), new CodeStatement[] {new CodeMethodReturnStatement()}),
                    new Tuple<CodeExpression, IEnumerable<CodeStatement>>(
                        Primitives.Int(2), new CodeStatement[] {new CodeMethodReturnStatement()}),
                    new Tuple<CodeExpression, IEnumerable<CodeStatement>>(
                        Primitives.Int(3), new CodeStatement[] {new CodeMethodReturnStatement()})));

            compileUnit.Namespaces.Add(codeNamespace);
            codeNamespace.Types.Add(classDeclaration);
            classDeclaration.Members.Add(method);
            return compileUnit;
        }

        public static CodeCompileUnit TestTypeWithImport()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("N1");
            CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration("TestClass")
            {
                IsClass = true
            };
            classDeclaration.TypeAttributes =
                (classDeclaration.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.Public;

            var method = new CodeMemberMethod()
            {
                Name = "Method",
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            method.Statements.Add(new CodeObjectCreateExpression(new CodeTypeReference("N1.N2.A")));
            method.Statements.Add(new CodeObjectCreateExpression(new CodeTypeReference("N1.N2.N3.B")));
            method.Statements.Add(new CodeObjectCreateExpression(new CodeTypeReference("N1.N2.N3.N4.C")));
            method.Statements.Add(new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(
                new CodeTypeReferenceExpression(typeof(Console)), "WriteLine")));
            method.Statements.Add(new CodeObjectCreateExpression(new CodeTypeReference(typeof(TestClasses.BaseClass))));
            method.Statements.Add(new CodeObjectCreateExpression(new CodeTypeReference("N1.namespace.object.class")));

            codeNamespace.Imports.Add(new CodeNamespaceImport("N1.N2"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("N1.N2.N3.N4"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("System"));

            compileUnit.Namespaces.Add(codeNamespace);
            codeNamespace.Types.Add(classDeclaration);
            classDeclaration.Members.Add(method);
            return compileUnit;
        }
    }
}