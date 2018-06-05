using CodeDomExt.Generators;
using CodeDomExtTests.TestClasses;
using Xunit;

namespace CodeDomExtTests
{
    public class ReadabilityImprovementsTests
    {
        [Fact]
        public void TestElseIfCS()
        {
            string[] expected = new string[]
            {
                "namespace Test.Namespace",
                "{",
                "    public class TestClass",
                "    {",
                "        public void Method()",
                "        {",
                "            if (true)",
                "            {",
                "                return;",
                "            }",
                "            if (1)",
                "            {",
                "                return;",
                "            }",
                "            else if (2)",
                "            {",
                "                return;",
                "            }",
                "            else if (3)",
                "            {",
                "                return;",
                "            }",
                "            else",
                "            {",
                "                return;",
                "            }",
                "        }",
                "    }",
                "}"
            };
            
            ProviderTestUtils.DoCSharpTest(expected, ReadabilityImprovementsTestUtils.TestElseIfCodeCompileUnit());
        } 
        
        [Fact]
        public void TestElseIfVB()
        {
            string[] expected =
            {
                "Namespace Test.Namespace",
                "    Public Class TestClass",
                "        Public Sub Method()",
                "            If True Then",
                "                Return",
                "            End If",
                "            If 1 Then",
                "                Return",
                "            ElseIf 2 Then",
                "                Return",
                "            ElseIf 3 Then",
                "                Return",
                "            Else",
                "                Return",
                "            End If",
                "        End Sub",
                "    End Class",
                "End Namespace"
            };
            
            ProviderTestUtils.DoVisualBasicTest(expected, ReadabilityImprovementsTestUtils.TestElseIfCodeCompileUnit());
        } 
    
        [Fact]
        public void TestNamespaceImportsCS()
        {
            string[] expected =
            {
                "namespace N1",
                "{",
                "    using N1.N2;",
                "    using N1.N2.N3.N4;",
                "    using System;",
                "",
                "    public class TestClass",
                "    {",
                "        public void Method()",
                "        {",
                "            new A();",
                "            new N2.N3.B();",
                "            new C();",
                "            Console.WriteLine();",
                "            new CodeDomExtTests.TestClasses.BaseClass();",
                "            new @namespace.@object.@class();", //N1.namespace.object.class
                "        }",
                "    }",
                "}"
            };
            
            ProviderTestUtils.DoCSharpTest(expected, ReadabilityImprovementsTestUtils.TestTypeWithImport(),
                new GeneratorOptions() {DoConsistencyChecks = false, AlwaysUseFullyQualifiedName = false});
        } 
        
        [Fact]
        public void TestNamespaceImportsVB()
        {
            string[] expected =
            {
                "Imports N1.N2",
                "Imports N1.N2.N3.N4",
                "Imports System",
                "",
                "Namespace N1",
                "    Public Class TestClass",
                "        Public Sub Method()",
                "            New A()",
                "            New N2.N3.B()",
                "            New C()",
                "            Console.WriteLine()",
                "            New CodeDomExtTests.TestClasses.BaseClass()",
                "            New [namespace].object.class()",
                "        End Sub",
                "    End Class",
                "End Namespace"
            };
            
            ProviderTestUtils.DoVisualBasicTest(expected, ReadabilityImprovementsTestUtils.TestTypeWithImport(),
                new GeneratorOptions() {DoConsistencyChecks = false, AlwaysUseFullyQualifiedName = false});
        } 
    }
}