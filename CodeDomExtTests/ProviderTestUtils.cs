using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using CodeDomExt.Generators;
using CodeDomExt.Helpers;
using CodeDomExt.Nodes;
using CodeDomExt.Utils;
using CodeDomExtTests.TestClasses;
using Xunit;
// ReSharper disable BitwiseOperatorOnEnumWithoutFlags

namespace CodeDomExtTests
{
    public static class ProviderTestUtils
    {
        public static CodeCompileUnit TestNamespacesCompileUnit()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("Test.Namespace");
            codeNamespace.Imports.Add(new CodeNamespaceImport("System"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("System.CodeDom"));
            compileUnit.Namespaces.Add(codeNamespace);
            CodeNamespace anotherNamespace = new CodeNamespace("Another"); 
            anotherNamespace.Imports.Add(new CodeNamespaceImport("System"));
            compileUnit.Namespaces.Add(anotherNamespace);
            return compileUnit;
        }

        public static CodeCompileUnit TestEnumCompileUnit()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("Test.Namespace");
            CodeTypeDeclaration enumDeclaration = new CodeTypeDeclaration("TestEnum")
            {
                TypeAttributes = TypeAttributes.Public,
                IsEnum = true
            };
            enumDeclaration.Members.Add(new CodeMemberField("TestEnum",
                "A")); //CodeDom implementation ignores the type.
            enumDeclaration.Members.Add(new CodeMemberField()
            {
                Name = "B"
            }); //The type by default is new CodeTypeReference("")
            enumDeclaration.Members.Add(new CodeMemberField()
            {
                Name = "C",
                InitExpression = Primitives.Int(5)
            });
            var dField = new CodeMemberField()
            {
                Name = "D"
            };
            dField.CustomAttributes.Add(
                new CodeAttributeDeclaration(new CodeTypeReference(typeof(SerializableAttribute))));
            enumDeclaration.Members.Add(dField);

            codeNamespace.Types.Add(enumDeclaration);
            compileUnit.Namespaces.Add(codeNamespace);

            return compileUnit;
        }

        
        public static CodeCompileUnit TestTypesAndFieldMemberAttributesCompileUnit()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("Test.Namespace");
            CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration("TestClass")
            {
                IsClass = true
            };
            classDeclaration.TypeAttributes =
                (classDeclaration.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.Public;

            classDeclaration.Members.Add(new CodeMemberField(typeof(int), "i") {Attributes = MemberAttributes.Private});
            classDeclaration.Members.Add(
                new CodeMemberField(typeof(uint), "ui") {Attributes = MemberAttributes.Public});
            classDeclaration.Members.Add(
                new CodeMemberField(typeof(short), "s") {Attributes = MemberAttributes.Family});
            classDeclaration.Members.Add(
                new CodeMemberField(typeof(ushort), "us") {Attributes = MemberAttributes.Assembly});
            classDeclaration.Members.Add(
                new CodeMemberField(typeof(long), "l") {Attributes = MemberAttributes.FamilyOrAssembly});
            classDeclaration.Members.Add(new CodeMemberField(typeof(ulong), "ul")
            {
                Attributes = MemberAttributes.FamilyAndAssembly | MemberAttributes.Abstract
            });
            classDeclaration.Members.Add(new CodeMemberField(typeof(bool), "bo")
            {
                Attributes = MemberAttributes.Private | MemberAttributes.Static
            });
            classDeclaration.Members.Add(new CodeMemberField(typeof(byte), "b")
            {
                Attributes = MemberAttributes.Private | MemberAttributes.Const,
                InitExpression = Primitives.Int(123)
            });
            classDeclaration.Members.Add(new CodeMemberField(typeof(sbyte), "sb")
            {
                Attributes = MemberAttributes.Private | MemberAttributes.Final
            });
            classDeclaration.Members.Add(new CodeMemberField(typeof(char), "c")
            {
                Attributes = MemberAttributes.Private | MemberAttributes.Override
            });
            classDeclaration.Members.Add(new CodeMemberField(typeof(decimal), "dec")
            {
                Attributes = MemberAttributes.Private | MemberAttributes.New
            });
            classDeclaration.Members.Add(new CodeMemberField(typeof(float), "f")
            {
                Attributes = MemberAttributes.Private | MemberAttributes.Override
            });
            classDeclaration.Members.Add(new CodeMemberField(typeof(double), "d")
            {
                Attributes = MemberAttributes.Private | MemberAttributes.Overloaded,
                InitExpression = Primitives.Int(0)
            });
            classDeclaration.Members.Add(new CodeMemberField(typeof(object), "obj")
            {
                Attributes = MemberAttributes.Private,
                InitExpression = new CodeObjectCreateExpression(Types.Object)
            });
            classDeclaration.Members.Add(new CodeMemberField(typeof(string), "str")
            {
                Attributes = MemberAttributes.Private | MemberAttributes.Final
            });
            classDeclaration.Members.Add(new CodeMemberField(typeof(int?), "nullableInt"));
            classDeclaration.Members.Add(new CodeMemberField(typeof(Console), "console"));
            classDeclaration.Members.Add(new CodeMemberField(typeof(IDictionary<int, IList<double>>), "dictionary"));
            classDeclaration.Members.Add(new CodeMemberField(typeof(double[,]), "array"));
            classDeclaration.Members.Add(new CodeMemberField(typeof(DateTime), "someDate"));
            
            codeNamespace.Types.Add(classDeclaration);
            compileUnit.Namespaces.Add(codeNamespace);

            return compileUnit;
        }

        
        public static CodeCompileUnit TestMethodSignatureCompileUnit()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("Test.Namespace");
            CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration("TestClass")
            {
                IsClass = true
            };
            classDeclaration.TypeAttributes =
                (classDeclaration.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.Public;

            classDeclaration.Members.Add(new CodeTypeConstructor() {Attributes = MemberAttributes.Abstract | MemberAttributes.Const | MemberAttributes.Public});//attributes should be ignored
            CodeConstructor constr1 = new CodeConstructor() { Attributes = MemberAttributes.Public | MemberAttributes.Abstract}; //only access attributes should be considered
            constr1.ChainedConstructorArgs.Add(Primitives.Int(0));
            classDeclaration.Members.Add(constr1);
            CodeConstructor constr2 = new CodeConstructor();
            constr2.Parameters.Add(new CodeParameterDeclarationExpression(Types.Int, "i"));
            classDeclaration.Members.Add(constr2);
            CodeConstructor constr3 = new CodeConstructor() {Attributes = MemberAttributes.FamilyAndAssembly};
            constr3.Parameters.Add(new CodeParameterDeclarationExpression(Types.Int, "a"));
            constr3.Parameters.Add(new CodeParameterDeclarationExpression(Types.Double, "b"));
            constr3.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("a"));
            constr3.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("b"));
            classDeclaration.Members.Add(constr3);
            classDeclaration.Members.Add(new CodeEntryPointMethod() {ReturnType = Types.Int, Attributes = MemberAttributes.Public});
            classDeclaration.Members.Add(new CodeMemberMethod() {Name = "Method", Attributes = MemberAttributes.Abstract | MemberAttributes.Family});
            classDeclaration.Members.Add(new CodeMemberMethod() {Name = "Method", Attributes = MemberAttributes.Public});
            classDeclaration.Members.Add(new CodeMemberMethod() {Name = "Method", Attributes = MemberAttributes.Public | MemberAttributes.Override});
            var methodWithPar = new CodeMemberMethod()
            {
                Name = "Method",
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            methodWithPar.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "a"));
            methodWithPar.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "b"));
            classDeclaration.Members.Add(methodWithPar);
            classDeclaration.Members.Add(new CodeMemberMethod() {Name = "Method", Attributes = MemberAttributes.Public | MemberAttributes.Static});

            codeNamespace.Types.Add(classDeclaration);
            compileUnit.Namespaces.Add(codeNamespace);

            return compileUnit;
        }
        
        
        public static CodeCompileUnit TestComplexClassAndMethodSignatureCompileUnit()
        {CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("Test.Namespace");
            CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration("TestClass")
            {
                IsClass = true
            };
            classDeclaration.TypeAttributes =
                (classDeclaration.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.Public;
            classDeclaration.TypeAttributes |= TypeAttributes.Abstract;
            var t1 = new CodeTypeParameter("T1");
            var t2 = new CodeTypeParameter("T2");

            t1.Constraints.Add(new CodeTypeReference("A"));
            t1.HasConstructorConstraint = true;
            t2.Constraints.Add(new CodeTypeReference("IA"));
            t2.Constraints.Add(new CodeTypeReference("IB"));

            classDeclaration.BaseTypes.Add(new CodeTypeReference("BaseClass"));
            classDeclaration.BaseTypes.Add(new CodeTypeReference("IInterface"));
            
            classDeclaration.TypeParameters.Add(t1);
            classDeclaration.TypeParameters.Add(t2);

            CodeMemberMethod method = new CodeMemberMethod() {Name = "GenericMethod"};
            var ta = new CodeTypeParameter("TA");
            var tb = new CodeTypeParameter("TB");
            method.TypeParameters.Add(ta);
            method.TypeParameters.Add(tb);
            method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("TA"), "a"));
            method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("TB"), "b"));
            var ienum = new CodeTypeReference("IEnumerable");
            ienum.TypeArguments.Add(new CodeTypeReference("TB"));
            ta.Constraints.Add(ienum);

            classDeclaration.Members.Add(method);
            codeNamespace.Types.Add(classDeclaration);
            compileUnit.Namespaces.Add(codeNamespace);

            return compileUnit;
        }
        
        
        public static CodeCompileUnit TestInterfaceCompileUnit()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("Test.Namespace");
            CodeTypeDeclaration interfaceDeclaration = new CodeTypeDeclaration("IInterface")
            {
                IsInterface = true
            };
            interfaceDeclaration.TypeAttributes =
                (interfaceDeclaration.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.Public;
            interfaceDeclaration.BaseTypes.Add(new CodeTypeReference("IAnotherInterface"));
            
            CodeMemberMethod method = new CodeMemberMethod()
            {
                Name = "GenericMethod",
                ReturnType = Types.Int,
                Attributes = MemberAttributes.Public
            };
            var ta = new CodeTypeParameter("TA");
            var tb = new CodeTypeParameter("TB");
            method.TypeParameters.Add(ta);
            method.TypeParameters.Add(tb);
            method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("TA"), "a"));
            method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("TB"), "b"));
            var ienum = new CodeTypeReference("IEnumerable");
            ienum.TypeArguments.Add(new CodeTypeReference("TB"));
            ta.Constraints.Add(ienum);

            interfaceDeclaration.Members.Add(method);
            interfaceDeclaration.Members.Add(new CodeMemberMethod()
            {
                Name = "AnotherMethod",
                Attributes = MemberAttributes.Public
            });
            codeNamespace.Types.Add(interfaceDeclaration);
            compileUnit.Namespaces.Add(codeNamespace);

            return compileUnit;
        }

        
        
        public static CodeCompileUnit TestDelegateAndEventCompileUnit()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("Test.Namespace");
            CodeTypeDeclaration declaration = new CodeTypeDeclaration("TestClass")
            {
                IsClass = true,
                IsPartial = true
            };
            declaration.TypeAttributes =
                (declaration.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.Public;
            
            CodeTypeDelegate delegateDeclaration = new CodeTypeDelegate("TestDelegate")
            {
                ReturnType = Types.Int
            };
            delegateDeclaration.TypeAttributes =
                (delegateDeclaration.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.NestedFamily;
            delegateDeclaration.Parameters.Add(
                new CodeParameterDeclarationExpression(Types.Int, "a"));
            delegateDeclaration.Parameters.Add(
                new CodeParameterDeclarationExpression(Types.Int, "b"));

            CodeTypeDelegate delegateDeclaration2 = new CodeTypeDelegate("TestDelegate2");
            delegateDeclaration2.TypeAttributes =
                (delegateDeclaration2.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.NestedFamily;
            
            CodeMemberEvent memberEvent = new CodeMemberEvent()
            {
                Name = "TestEvent",
                Type = new CodeTypeReference("TestDelegate2"),
                Attributes = MemberAttributes.Family | MemberAttributes.Static
            };

            declaration.Members.Add(delegateDeclaration);
            declaration.Members.Add(delegateDeclaration2);
            declaration.Members.Add(memberEvent);
            codeNamespace.Types.Add(declaration);
            compileUnit.Namespaces.Add(codeNamespace);

            return compileUnit;
        }
        
        
        public static CodeCompileUnit TestAttributesCompileUnit()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("Test.Namespace");
            CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration("TestClass")
            {
                IsClass = true
            };
            classDeclaration.TypeAttributes =
                (classDeclaration.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.Public;
            
            CodeAttributeDeclaration attr1 = new CodeAttributeDeclaration("Attribute1");
            CodeAttributeDeclaration attr2 = new CodeAttributeDeclaration(
                "Attribute2",
                new CodeAttributeArgument(Primitives.Int(1)),
                new CodeAttributeArgument("Value", Primitives.Int(2)));

            classDeclaration.CustomAttributes.Add(attr1);
            classDeclaration.CustomAttributes.Add(attr2);
            
            var t = new CodeTypeParameter("T");
            t.CustomAttributes.Add(attr2);
            t.CustomAttributes.Add(attr1);

            classDeclaration.TypeParameters.Add(t);
            
            var field = new CodeMemberField(typeof(int), "a") {Attributes = MemberAttributes.Private};
            field.CustomAttributes.Add(attr1);

            classDeclaration.Members.Add(field);

            var method = new CodeMemberMethod() {Name = "Method", Attributes = MemberAttributes.Public | MemberAttributes.Final};

            var a = new CodeParameterDeclarationExpression(typeof(int), "a");
            var b = new CodeParameterDeclarationExpression(typeof(int), "b");
            a.CustomAttributes.Add(attr1);
            b.CustomAttributes.Add(attr1);
            b.CustomAttributes.Add(attr2);

            method.Parameters.Add(a);
            method.Parameters.Add(b);
            
            classDeclaration.Members.Add(method);

            codeNamespace.Types.Add(classDeclaration);
            compileUnit.Namespaces.Add(codeNamespace);

            return compileUnit;
        }
        
        
        public static CodeCompileUnit TestSimpleStructCompileUnit()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("Test.Namespace");
            CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration("TestClass")
            {
                IsStruct = true
            };
            classDeclaration.TypeAttributes =
                (classDeclaration.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.NotPublic;

            var field1 = new CodeMemberField(typeof(int), "count");
            field1.Attributes = (field1.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Private;
            var field2 = new CodeMemberField(typeof(int), "increment");
            field2.Attributes = (field2.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Private;

            classDeclaration.Members.Add(field1);
            classDeclaration.Members.Add(field2);

            var constructor = new CodeConstructor();
            constructor.Attributes = (constructor.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;

            constructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "count"));
            constructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "increment"));

            constructor.Statements.Add(new CodeAssignStatement(
                FieldReferenceExpression.This("count"),
                new CodeArgumentReferenceExpression("count")));
            constructor.Statements.Add(new CodeAssignStatement(
                FieldReferenceExpression.This("increment"),
                new CodeArgumentReferenceExpression("increment")));

            classDeclaration.Members.Add(constructor);
            classDeclaration.Attributes = (classDeclaration.Attributes & ~MemberAttributes.AccessMask) |
                                          MemberAttributes.Assembly;

            var method = new CodeMemberMethod()
            {
                Name = "IncrementAndGet",
                ReturnType = Types.Int,
                Attributes = MemberAttributes.Assembly | MemberAttributes.Final
            };

            method.Statements.Add(
                new CodeOperationAssignmentStatement(
                    FieldReferenceExpression.Default("count"),
                    CodeBinaryOperatorTypeMore.Add,
                    FieldReferenceExpression.Default("increment")).AsAssignStatement());
            method.Statements.Add(new CodeMethodReturnStatement(FieldReferenceExpression.Default("count")));

            classDeclaration.Members.Add(method);

            codeNamespace.Types.Add(classDeclaration);
            compileUnit.Namespaces.Add(codeNamespace);

            return compileUnit;
        }
        
        
        public static CodeCompileUnit TestSimpleClassCompileUnit()
        {
            CodeCommentStatement docComment = new CodeCommentStatement("this is a doc comment", true);
            CodeCommentStatement comment = new CodeCommentStatement("this is a comment", false);
            
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("Test.Namespace");
            CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration("TestClass")
            {
                IsClass = true
            };
            classDeclaration.TypeAttributes =
                (classDeclaration.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.NotPublic;

            var field1 = new CodeMemberField(typeof(int), "count");
            field1.Attributes = (field1.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Private;
            var field2 = new CodeMemberField(typeof(int), "increment");
            field2.Attributes = (field2.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.FamilyAndAssembly;

            classDeclaration.Members.Add(field1);
            classDeclaration.Members.Add(field2);
            
            var prop1 = new CodeMemberProperty()
            {
                Name = "Prop",
                HasGet = true,
                HasSet = true,
                Type = Types.Int,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            var prop2 = new CodeMemberProperty()
            {
                Name = "Prop2",
                HasSet = true,
                Type = Types.Int,
                Attributes = MemberAttributes.Public
            };
            prop1.GetStatements.Add(new CodeMethodReturnStatement(FieldReferenceExpression.Default("increment")));
            prop1.SetStatements.Add(
                new CodeAssignStatement(FieldReferenceExpression.Default("increment"),
                new CodePropertySetValueReferenceExpression()));
            prop2.SetStatements.Add(new CodeThrowExceptionStatement(
                new CodeObjectCreateExpression(new CodeTypeReference("Exception"))));
            
            classDeclaration.Members.Add(prop1);
            classDeclaration.Members.Add(prop2);

            var constructor = new CodeConstructor();
            constructor.Attributes = (constructor.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Family;

            constructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "count"));
            constructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "increment"));

            constructor.Statements.Add(new CodeAssignStatement(
                FieldReferenceExpression.This( "count"),
                new CodeArgumentReferenceExpression("count")));
            constructor.Statements.Add(new CodeAssignStatement(
                FieldReferenceExpression.This( "increment"),
                new CodeArgumentReferenceExpression("increment")));

            classDeclaration.Members.Add(constructor);
            classDeclaration.Attributes = (classDeclaration.Attributes & ~MemberAttributes.AccessMask) |
                                          MemberAttributes.FamilyOrAssembly;

            var method = new CodeMemberMethod()
            {
                Name = "IncrementAndGet",
                ReturnType = Types.Int,
                Attributes = MemberAttributes.FamilyOrAssembly | MemberAttributes.Final
            };

            method.Statements.Add(comment);
            method.Statements.Add(
                new CodeOperationAssignmentStatement(
                    FieldReferenceExpression.Default("count"),
                    CodeBinaryOperatorTypeMore.Add,
                    FieldReferenceExpression.Default("increment")).AsAssignStatement());
            method.Statements.Add(new CodeMethodReturnStatement(FieldReferenceExpression.Default("count")));

            classDeclaration.Members.Add(method);

            codeNamespace.Types.Add(classDeclaration);
            compileUnit.Namespaces.Add(codeNamespace);

            codeNamespace.Comments.Add(comment);
            codeNamespace.Comments.Add(docComment);

            classDeclaration.Comments.Add(docComment);
            classDeclaration.Comments.Add(docComment);

            method.Comments.Add(comment);
            
            return compileUnit;
        }
        
        
        public static CodeCompileUnit TestStatementsAndExpressionsCompileUnit()
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

            
            CodeFieldReferenceExpression fieldRef = FieldReferenceExpression.This("field");
            CodeArgumentReferenceExpression argRef = new CodeArgumentReferenceExpression("arg");
            CodeVariableReferenceExpression varRef = new CodeVariableReferenceExpression("avar");

            method.Statements.Add(new CodeArrayCreateExpression(new CodeTypeReference("Class"),
                new CodeObjectCreateExpression(new CodeTypeReference("A")),
                new CodeObjectCreateExpression(new CodeTypeReference("A"), fieldRef, argRef)));
            method.Statements.Add(new CodeArrayCreateExpression(Types.Double,
                new CodeBinaryOperatorExpression(argRef, CodeBinaryOperatorType.Multiply, varRef)));
            method.Statements.Add(new CodeArrayIndexerExpression(varRef,
                new CodeCastExpression(typeof(int), Primitives.Float(5.5f)),
                new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("Math"), "Abs", Primitives.Int(-2))));
            method.Statements.Add(new CodeDefaultValueExpression(Types.UInt));
            method.Statements.Add(new CodeTypeOfExpression(Types.Int));
            method.Statements.Add(new CodeIterationStatement(
                new CodeVariableDeclarationStatement(
                    Types.Int, "i", Primitives.Int(0)),
                new CodeBinaryOperatorExpression(
                    new CodeVariableReferenceExpression("i"),
                    CodeBinaryOperatorType.LessThan,
                    Primitives.Int(10)),
                new CodeAssignStatement(
                    new CodeVariableReferenceExpression("i"),
                    new CodeBinaryOperatorExpression(
                        new CodeVariableReferenceExpression("i"),
                        CodeBinaryOperatorType.Add,
                        Primitives.Int(1))),
                new CodeTryCatchFinallyStatement(
                    new CodeStatement[] {new CodeAttachEventStatement(
                        new CodeEventReferenceExpression(PropertyReferenceExpression.Base("EventsHolder"), "AnEvent"),
                        DelegateCreateExpression.This(new CodeTypeReference("Action"), "Method")
                    )},
                    new CodeCatchClause[]
                    {
                        new CodeCatchClause("e", new CodeTypeReference("Exception")),
                        new CodeCatchClause("e", new CodeTypeReference("AnotherException"))
                    })));
            
            classDeclaration.Members.Add(method);
            codeNamespace.Types.Add(classDeclaration);
            compileUnit.Namespaces.Add(codeNamespace);

            return compileUnit;
        }

        public static CodeCompileUnit TestPrimitivesCompileUnit()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("Test.Namespace");
            CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration("TestClass")
            {
                IsClass = true
            };
            classDeclaration.TypeAttributes =
                (classDeclaration.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.Public;

            classDeclaration.Members.Add(
                new CodeMemberField(typeof(short), "s") {InitExpression = Primitives.Short(2)});
            classDeclaration.Members.Add(
                new CodeMemberField(typeof(uint), "ui") {InitExpression = Primitives.UInt(2)});
            
            compileUnit.Namespaces.Add(codeNamespace);
            codeNamespace.Types.Add(classDeclaration);

            return compileUnit;
        }
        
        public static CodeCompileUnit TestIdentifiersCompileUnit()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("Test.Namespace");
            CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration("Class")
            {
                IsClass = true
            };
            classDeclaration.TypeAttributes =
                (classDeclaration.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.Public;

            classDeclaration.Members.Add(new CodeMemberField(typeof(int), "value"));
            
            compileUnit.Namespaces.Add(codeNamespace);
            codeNamespace.Types.Add(classDeclaration);

            return compileUnit;
        }
        
        public static CodeCompileUnit TestSnippetCompileUnit()
        {
            string snippetMemberString =
                "public void SnippetMember()\n" +
                "{\r\n" +
                "\r" +
                "}";
            string snippetStatementString = "SnippetStatement\n" +
                                            "    on multiple lines";
            string snippetExpressionString = "snippet expression";
            string snippetExpressionString2 = "multiline\n" +
                                              "    snippet expression";
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("Test.Namespace");
            CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration("TestClass")
            {
                IsClass = true
            };
            classDeclaration.TypeAttributes =
                (classDeclaration.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.Public;

            classDeclaration.Members.Add(new CodeSnippetTypeMember(snippetMemberString));

            var method = new CodeMemberMethod()
            {
                Name = "Method",
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };

            method.Statements.Add(new CodeSnippetStatement(snippetStatementString));
            method.Statements.Add(new CodeAssignStatement(
                new CodeVariableReferenceExpression("i"), 
                new CodeSnippetExpression(snippetExpressionString)));
            method.Statements.Add(new CodeAssignStatement(
                new CodeVariableReferenceExpression("i2"), 
                new CodeSnippetExpression(snippetExpressionString2)));
            
            classDeclaration.Members.Add(method);
            codeNamespace.Types.Add(classDeclaration);
            compileUnit.Namespaces.Add(codeNamespace);

            return compileUnit;
        }

        public static CodeCompileUnit TestDirectiveCompileUnit()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("Test.Namespace");
            CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration("TestClass")
            {
                IsClass = true
            };
            classDeclaration.SetAccessibilityLevel(AccessibilityLevel.Public);

            var method = new CodeMemberMethod()
            {
                Name = "Method",
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };

            compileUnit.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Compile unit region"));
            compileUnit.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, null));
            
            classDeclaration.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Class region"));
            classDeclaration.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, null));
            
            var field1 = new CodeMemberField(Types.Int, "a");
            var field2 = new CodeMemberField(Types.Int, "b");

            field1.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Fields region"));
            field2.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, null));

            method.Statements.Add(
                new CodeAssignStatement(
                    FieldReferenceExpression.Default("a"),
                    FieldReferenceExpression.Default("b"))
                {
                    StartDirectives =
                    {
                        new CodeRegionDirective(CodeRegionMode.Start, "region a"),
                        new CodeRegionDirective(CodeRegionMode.Start, "region b")
                    }
                });
            method.Statements.Add(
                new CodeMethodReturnStatement(
                    FieldReferenceExpression.Default("a"))
                {
                    EndDirectives =
                    {
                        new CodeRegionDirective(CodeRegionMode.End, null),
                        new CodeRegionDirective(CodeRegionMode.End, null)
                    }
                });
            
            compileUnit.Namespaces.Add(codeNamespace);
            codeNamespace.Types.Add(classDeclaration);
            classDeclaration.Members.Add(field1);
            classDeclaration.Members.Add(field2);
            classDeclaration.Members.Add(method);
            return compileUnit;
        }
        
        public static void DoCSharpTest(string[] expected, CodeCompileUnit compileUnit, GeneratorOptions options = null)
        {
            DoTest(expected, compileUnit, Language.CSharp, options);
        }

        public static void DoVisualBasicTest(string[] expected, CodeCompileUnit compileUnit, GeneratorOptions options = null, bool addOptions = true)
        {
            if (addOptions)
            {
                var tmp = new string[expected.Length + 4];
                tmp[0] = "Option Strict Off";
                tmp[1] = "Option Explicit On";
                tmp[2] = "Option Infer On";
                tmp[3] = "";

                for (int i = 0; i < expected.Length; i++)
                {
                    tmp[i + 4] = expected[i];
                }

                expected = tmp;
            }
            DoTest(expected, compileUnit, Language.VisualBasic, options);
        }

        private static void DoTest(string[] expected, CodeCompileUnit compileUnit, Language language,
            GeneratorOptions options)
        {
            using (StreamWriter sw = StreamUtilities.GetStreamWriter())
            {
                CodeGeneratorFactory.GetCodeGenerator(language,
                        options ?? new GeneratorOptions()
                        {
                            IndentString = "    ", 
                            DoConsistencyChecks = false, 
                            AlwaysUseFullyQualifiedName = true
                        })
                    .Generate(compileUnit, sw);
            }

            var generated = StreamUtilities.ReadStream().ToArray();
            Assert.Equal(expected.Length, generated.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.True(expected[i].Equals(generated[i]), "Generated line doesn't match expected: " + (i + 1));
            }
        }
    }
}