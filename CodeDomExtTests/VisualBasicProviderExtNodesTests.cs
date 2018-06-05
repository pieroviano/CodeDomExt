using Xunit;

namespace CodeDomExtTests
{
    public class VisualBasicProviderExtNodesTests
    {
        [Fact]
        public void TestExpressionsAndStatements()
        {
            string[] expected =
            {
                "Namespace Test.Namespace",
                "    Public Class TestClass",
                "        Public Sub Method()",
                "            If(i > 0, i, Nothing)",
                "            Do",
                "                a <<= 2",
                "            Loop While Not True",
                "            lambda = Function(i As Integer, j As Integer)",
                "                If(b, 0)",
                "                New A() With {",
                "                    .Prop = 0,",
                "                    .Prop2 = 2",
                "                }",
                "            End Function",
                "            While False",
                "                i += 1",
                "                Exit While",
                "            End While",
                "            Using stream As New Stream()",
                "                Dim matrix = New Integer(,) {{1, 1}, {1, 1}}",
                "            End Using",
                "            For Each i As Integer In collection",
                "                New Integer(1 - 1, 1 - 1) {{1}}",
                "                New Integer(1 - 1, 1 - 1) {}",
                "            Next",
                "        End Sub",
                "    End Class",
                "End Namespace"
            };
            
            ProviderTestUtils.DoVisualBasicTest(expected, ProviderExtTestUtils.TestExpressionsAndStatementsCompileUnit(false));
        }
        
        [Fact]
        public void TestPropertiesAndMethodSignature()
        {
            string[] expected =
            {
                "Namespace Test.Namespace",
                "    Public Class TestClass",
                "        Public Overridable Property Prop As Integer = 1",
                "",
                "        Public Sub Method(ByVal a As Integer, Optional ByVal b As Integer = 0)",
                "        End Sub",
                "    End Class",
                "End Namespace"
            };
            
            ProviderTestUtils.DoVisualBasicTest(expected, ProviderExtTestUtils.TestPropertiesAndMethodSignatureCompileUnit());
        }
        
        [Fact]
        public void TestModule()
        {
            string[] expected =
            {
                "Namespace Test.Namespace",
                "    Public Module TestClass",
                "        Public Sub Method()",
                "        End Sub",
                "    End Module",
                "End Namespace"
            };
            
            ProviderTestUtils.DoVisualBasicTest(expected, ProviderExtTestUtils.TestStaticClassCompileUnit());
        }
    }
}