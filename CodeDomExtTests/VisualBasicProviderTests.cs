using Xunit;

namespace CodeDomExtTests
{
    public class VisualBasicProviderTests
    {
        [Fact]
        public void TestNamespaces()
        {
            string[] expected = new string[]
            {
                "Option Strict Off",
                "Option Explicit On",
                "Option Infer On",
                "",
                "Imports System",
                "Imports System.CodeDom",
                "",
                "Namespace Test.Namespace",
                "End Namespace",
                "",
                "Namespace Another",
                "End Namespace"
            };

            ProviderTestUtils.DoVisualBasicTest(expected, ProviderTestUtils.TestNamespacesCompileUnit(), addOptions: false);
        }
        
        [Fact]
        public void TestEnum()
        {
            string[] expected =
            {
                "Namespace Test.Namespace",
                "    Public Enum TestEnum",
                "        A",
                "        B",
                "        C = 5",
                "        <System.SerializableAttribute>",
                "        D",
                "    End Enum",
                "End Namespace"
            };
            
            ProviderTestUtils.DoVisualBasicTest(expected, ProviderTestUtils.TestEnumCompileUnit());
        }

        [Fact]
        public void TestTypesAndFieldMemberAttributes()
        {
            string[] expected =
            {
                "Namespace Test.Namespace",
                "    Public Class TestClass",
                "        Private i As Integer",
                "",
                "        Public ui As UInteger",
                "",
                "        Protected s As Short",
                "",
                "        Friend us As UShort",
                "",
                "        Protected Friend l As Long",
                "",
                "        Private Protected ul As ULong",
                "",
                "        Private Shared bo As Boolean",
                "",
                "        Private Const b As Byte = 123",
                "",
                "        Private sb As SByte",
                "",
                "        Private c As Char",
                "",
                "        Private Shadows dec As Decimal",
                "",
                "        Private f As Single",
                "",
                "        Private d As Double = 0",
                "",
                "        Private obj As Object = New Object()",
                "",
                "        Private str As String",
                "",
                "        Private nullableInt As Integer?",
                "",
                "        Private console As System.Console",
                "",
                "        Private dictionary As System.Collections.Generic.IDictionary(Of Integer, System.Collections.Generic.IList(Of Double))",
                "",
                "        Private array As Double(,)",
                "",
                "        Private someDate As Date",
                "    End Class",
                "End Namespace"
            };

            ProviderTestUtils.DoVisualBasicTest(expected, ProviderTestUtils.TestTypesAndFieldMemberAttributesCompileUnit());
        }

        [Fact]
        public void TestMethodSignature()
        {
            string[] expected =
            {
                "Namespace Test.Namespace",
                "    Public Class TestClass",
                "        Shared Sub New()",
                "        End Sub",
                "",
                "        Public Sub New()",
                "            Me.New(0)",
                "        End Sub",
                "",
                "        Private Sub New(ByVal i As Integer)",
                "        End Sub",
                "",
                "        Private Protected Sub New(ByVal a As Integer, ByVal b As Double)",
                "            MyBase.New(a, b)",
                "        End Sub",
                "",
                "        Public Shared Function Main(ByVal cmdArgs() As String) As Integer",
                "        End Function",
                "",
                "        Protected MustOverride Sub Method()",
                "",
                "        Public Overridable Sub Method()",
                "        End Sub",
                "",
                "        Public Overrides Sub Method()",
                "        End Sub",
                "",
                "        Public Sub Method(ByVal a As Integer, ByVal b As Integer)",
                "        End Sub",
                "",
                "        Public Shared Sub Method()",
                "        End Sub",
                "    End Class",
                "End Namespace"
            };
            
            ProviderTestUtils.DoVisualBasicTest(expected, ProviderTestUtils.TestMethodSignatureCompileUnit());
        }
        
        [Fact]
        public void TestComplexClassAndMethodSignature()
        {
            string[] expected =
            {
                "Namespace Test.Namespace",
                "    Public MustInherit Class TestClass(Of T1 As {A, New}, T2 As {IA, IB})",
                "        Inherits BaseClass",
                "        Implements IInterface",
                "",
                "        Private Sub GenericMethod(Of TA As IEnumerable(Of TB), TB)(ByVal a As TA, ByVal b As TB)",
                "        End Sub",
                "    End Class",
                "End Namespace"
            };
            ProviderTestUtils.DoVisualBasicTest(expected, ProviderTestUtils.TestComplexClassAndMethodSignatureCompileUnit());
        }
        
        [Fact]
        public void TestInterface()
        {
            string[] expected =
            {
                "Namespace Test.Namespace",
                "    Public Interface IInterface",
                "        Inherits IAnotherInterface",
                "",
                "        Function GenericMethod(Of TA As IEnumerable(Of TB), TB)(ByVal a As TA, ByVal b As TB) As Integer",
                "",
                "        Sub AnotherMethod()",
                "    End Interface",
                "End Namespace"
            };
            ProviderTestUtils.DoVisualBasicTest(expected, ProviderTestUtils.TestInterfaceCompileUnit());
        }

        [Fact]
        public void TestDelegateAndEvent()
        {
            string[] expected =
            {
                "Namespace Test.Namespace",
                "    Partial Public Class TestClass",
                "        Protected Delegate Function TestDelegate(ByVal a As Integer, ByVal b As Integer) As Integer",
                "",
                "        Protected Delegate Sub TestDelegate2()",
                "",
                "        Protected Shared Event TestEvent As TestDelegate2",
                "    End Class",
                "End Namespace"
            };
            ProviderTestUtils.DoVisualBasicTest(expected, ProviderTestUtils.TestDelegateAndEventCompileUnit());
        }
        
        [Fact]
        public void TestAttributes()
        {
            string[] expected =
            {
                "Namespace Test.Namespace",
                "    <Attribute1>",
                "    <Attribute2(1, Value:=2)>",
                "    Public Class TestClass(Of T)",
                "        <Attribute1>",
                "        Private a As Integer",
                "",
                "        Public Sub Method(<Attribute1> ByVal a As Integer, <Attribute1><Attribute2(1, Value:=2)> ByVal b As Integer)",
                "        End Sub",
                "    End Class",
                "End Namespace"
            };
            ProviderTestUtils.DoVisualBasicTest(expected, ProviderTestUtils.TestAttributesCompileUnit());
        }
        
        [Fact]
        public void TestSimpleStruct()
        {
            string[] expected =
            {
                "Namespace Test.Namespace",
                "    Friend Structure TestClass",
                "        Private count As Integer",
                "",
                "        Private increment As Integer",
                "",
                "        Public Sub New(ByVal count As Integer, ByVal increment As Integer)",
                "            Me.count = count",
                "            Me.increment = increment",
                "        End Sub",
                "",
                "        Friend Function IncrementAndGet() As Integer",
                "            count = count + increment",
                "            Return count",
                "        End Function",
                "    End Structure",
                "End Namespace"
            };
            ProviderTestUtils.DoVisualBasicTest(expected, ProviderTestUtils.TestSimpleStructCompileUnit());
        }
        [Fact]
        public void TestSimpleClass()
        {
            string[] expected =
            {
                "'this is a comment",
                "''' this is a doc comment",
                "Namespace Test.Namespace",
                "    ''' this is a doc comment",
                "    ''' this is a doc comment",
                "    Friend Class TestClass",
                "        Private count As Integer",
                "",
                "        Private Protected increment As Integer",
                "",
                "        Public Property Prop As Integer",
                "            Get",
                "                Return increment",
                "            End Get",
                "            Set",
                "                increment = Value",
                "            End Set",
                "        End Property",
                "",
                "        Public Overridable WriteOnly Property Prop2 As Integer",
                "            Set",
                "                Throw New Exception()",
                "            End Set",
                "        End Property",
                "",
                "        Protected Sub New(ByVal count As Integer, ByVal increment As Integer)",
                "            Me.count = count",
                "            Me.increment = increment",
                "        End Sub",
                "",
                "        'this is a comment",
                "        Protected Friend Function IncrementAndGet() As Integer",
                "            'this is a comment",
                "            count = count + increment",
                "            Return count",
                "        End Function",
                "    End Class",
                "End Namespace"
            };
            ProviderTestUtils.DoVisualBasicTest(expected, ProviderTestUtils.TestSimpleClassCompileUnit());
        }
        
        [Fact]
        public void TestStatementsAndExpressions()
        {
            string[] expected =
            {
                "Namespace Test.Namespace",
                "    Public Class TestClass",
                "        Public Sub Method()",
                "            New [Class]() {New A(), New A(Me.field, arg)}",
                "            New Double((arg * avar) - 1) {}",
                "            avar(CType(5.5F, Integer), Math.Abs(-2))",
                "            CType(Nothing, UInteger)",
                "            GetType(Integer)",
                "            Dim i As Integer = 0",
                "            While i < 10",
                "                Try",
                "                    AddHandler MyBase.EventsHolder.AnEvent, AddressOf Me.Method",
                "                Catch e As Exception",
                "                Catch e As AnotherException",
                "                End Try",
                "                i = i + 1",
                "            End While",
                "        End Sub",
                "    End Class",
                "End Namespace"
            };
            ProviderTestUtils.DoVisualBasicTest(expected, ProviderTestUtils.TestStatementsAndExpressionsCompileUnit());
        }
        
        [Fact]
        public void TestIdentifiers()
        {
            string[] expected = new string[]
            {
                "Namespace Test.Namespace",
                "    Public Class [Class]",
                "        Private value As Integer",
                "",
                "        Private Sub [namespace]()",
                "            [namespace]()",
                "            Me.namespace()",
                "        End Sub",
                "    End Class",
                "End Namespace"
            };
            ProviderTestUtils.DoVisualBasicTest(expected, ProviderTestUtils.TestIdentifiersCompileUnit());
        }

        [Fact]
        public void TestDirective()
        {
            string[] expected =
            {
                "#Region \"Compile unit region\"",
                "Option Strict Off",
                "Option Explicit On",
                "Option Infer On",
                "",
                "Namespace Test.Namespace",
                "    #Region \"Class region\"",
                "    Public Class TestClass",
                "        #Region \"Fields region\"",
                "        Private a As Integer",
                "",
                "        Private b As Integer",
                "        #End Region",
                "",
                "        Public Sub Method()",
                "            #Region \"region a\"",
                "            #Region \"region b\"",
                "            a = b",
                "            Return a",
                "            #End Region",
                "            #End Region",
                "        End Sub",
                "    End Class",
                "    #End Region",
                "End Namespace",
                "#End Region"
            };
            
            ProviderTestUtils.DoVisualBasicTest(expected, ProviderTestUtils.TestDirectiveCompileUnit(), addOptions: false);
        }
    }
}