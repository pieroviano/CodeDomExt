using CodeDomExt.Generators;
using Xunit;

namespace CodeDomExtTests
{
    public class CSharpProviderTests
    {
        [Fact]
        public void TestNamespaces()
        {
            string[] expected = new string[]
            {
                "namespace Test.Namespace",
                "{",
                "  using System;",
                "  using System.CodeDom;",
                "}",
                "",
                "namespace Another",
                "{",
                "  using System;",
                "}"
            };

            ProviderTestUtils.DoCSharpTest(expected, ProviderTestUtils.TestNamespacesCompileUnit(),
                new GeneratorOptions() {IndentString = "  ", DoConsistencyChecks = false});
        }

        [Fact]
        public void TestEnum()
        {
            string[] expected = new string[]
            {
                "namespace Test.Namespace",
                "{",
                "    public enum TestEnum",
                "    {",
                "        A,",
                "        B,",
                "        C = 5,",
                "        [System.SerializableAttribute]",
                "        D",
                "    }",
                "}"
            };

            ProviderTestUtils.DoCSharpTest(expected, ProviderTestUtils.TestEnumCompileUnit());
        }

        [Fact]
        public void TestTypesAndFieldMemberAttributes()
        {
            string[] expected = new string[]
            {
                "namespace Test.Namespace",
                "{",
                "    public class TestClass",
                "    {",
                "        private int i;",
                "",
                "        public uint ui;",
                "",
                "        protected short s;",
                "",
                "        internal ushort us;",
                "",
                "        protected internal long l;",
                "",
                "        private protected ulong ul;",
                "",
                "        private static bool bo;",
                "",
                "        private const byte b = 123;",
                "",
                "        private sbyte sb;",
                "",
                "        private char c;",
                "",
                "        private new decimal dec;",
                "",
                "        private float f;",
                "",
                "        private double d = 0;",
                "",
                "        private object obj = new object();",
                "",
                "        private string str;",
                "",
                "        private int? nullableInt;",
                "",
                "        private System.Console console;",
                "",
                "        private System.Collections.Generic.IDictionary<int, System.Collections.Generic.IList<double>> dictionary;",
                "",
                "        private double[,] array;",
                "",
                "        private System.DateTime someDate;",
                "    }",
                "}"
            };

            ProviderTestUtils.DoCSharpTest(expected, ProviderTestUtils.TestTypesAndFieldMemberAttributesCompileUnit());
        }

        [Fact]
        public void TestMethodSignature()
        {
            string[] expected = new string[]
            {
                "namespace Test.Namespace",
                "{",
                "    public class TestClass",
                "    {",
                "        static TestClass()",
                "        {",
                "        }",
                "",
                "        public TestClass()",
                "            : this(0)",
                "        {",
                "        }",
                "",
                "        private TestClass(int i)",
                "        {",
                "        }",
                "",
                "        private protected TestClass(int a, double b)",
                "            : base(a, b)",
                "        {",
                "        }",
                "",
                "        public static int Main(string[] args)",
                "        {",
                "        }",
                "",
                "        protected abstract void Method();",
                "",
                "        public virtual void Method()",
                "        {",
                "        }",
                "",
                "        public override void Method()",
                "        {",
                "        }",
                "",
                "        public void Method(int a, int b)",
                "        {",
                "        }",
                "",
                "        public static void Method()",
                "        {",
                "        }",
                "    }",
                "}"
            };
            
            ProviderTestUtils.DoCSharpTest(expected, ProviderTestUtils.TestMethodSignatureCompileUnit());
        }
        
        [Fact]
        public void TestComplexClassAndMethodSignature()
        {
            string[] expected = new string[]
            {
                "namespace Test.Namespace",
                "{",
                "    public abstract class TestClass<T1, T2>",
                "        : BaseClass, IInterface",
                "        where T1 : A, new()",
                "        where T2 : IA, IB",
                "    {",
                "        private void GenericMethod<TA, TB>(TA a, TB b)",
                "            where TA : IEnumerable<TB>",
                "        {",
                "        }",
                "    }",
                "}"
            };
            ProviderTestUtils.DoCSharpTest(expected, ProviderTestUtils.TestComplexClassAndMethodSignatureCompileUnit());
        }
        
        [Fact]
        public void TestInterface()
        {
            string[] expected = new string[]
            {
                "namespace Test.Namespace",
                "{",
                "    public interface IInterface",
                "        : IAnotherInterface",
                "    {",
                "        int GenericMethod<TA, TB>(TA a, TB b)",
                "            where TA : IEnumerable<TB>;",
                "",
                "        void AnotherMethod();",
                "    }",
                "}"
            };
            ProviderTestUtils.DoCSharpTest(expected, ProviderTestUtils.TestInterfaceCompileUnit());
        }
        
        [Fact]
        public void TestDelegateAndEvent()
        {
            string[] expected = new string[]
            {
                "namespace Test.Namespace",
                "{",
                "    public partial class TestClass",
                "    {",
                "        protected delegate int TestDelegate(int a, int b);",
                "",
                "        protected delegate void TestDelegate2();",
                "",
                "        protected static event TestDelegate2 TestEvent;",
                "    }",
                "}"
            };
            ProviderTestUtils.DoCSharpTest(expected, ProviderTestUtils.TestDelegateAndEventCompileUnit());
        }
        
        [Fact]
        public void TestAttributes()
        {
            string[] expected = new string[]
            {
                "namespace Test.Namespace",
                "{",
                "    [Attribute1]",
                "    [Attribute2(1, Value=2)]",
                "    public class TestClass<[Attribute2(1, Value=2)][Attribute1] T>",
                "    {",
                "        [Attribute1]",
                "        private int a;",
                "",
                "        public void Method([Attribute1] int a, [Attribute1][Attribute2(1, Value=2)] int b)",
                "        {",
                "        }",
                "    }",
                "}"
            };
            ProviderTestUtils.DoCSharpTest(expected, ProviderTestUtils.TestAttributesCompileUnit());
        }
        
        [Fact]
        public void TestSimpleStruct()
        {
            string[] expected = new string[]
            {
                "namespace Test.Namespace",
                "{",
                "    internal struct TestClass",
                "    {",
                "        private int count;",
                "",
                "        private int increment;",
                "",
                "        public TestClass(int count, int increment)",
                "        {",
                "            this.count = count;",
                "            this.increment = increment;",
                "        }",
                "",
                "        internal int IncrementAndGet()",
                "        {",
                "            count = count + increment;",
                "            return count;",
                "        }",
                "    }",
                "}"
            };
            ProviderTestUtils.DoCSharpTest(expected, ProviderTestUtils.TestSimpleStructCompileUnit());
        }
        
        [Fact]
        public void TestSimpleClass()
        {
            string[] expected = new string[]
            {
                "//this is a comment",
                "/// this is a doc comment",
                "namespace Test.Namespace",
                "{",
                "    /// this is a doc comment",
                "    /// this is a doc comment",
                "    internal class TestClass",
                "    {",
                "        private int count;",
                "",
                "        private protected int increment;",
                "",
                "        public int Prop",
                "        {",
                "            get",
                "            {",
                "                return increment;",
                "            }",
                "            set",
                "            {",
                "                increment = value;",
                "            }",
                "        }",
                "",
                "        public virtual int Prop2",
                "        {",
                "            set",
                "            {",
                "                throw new Exception();",
                "            }",
                "        }",
                "",
                "        protected TestClass(int count, int increment)",
                "        {",
                "            this.count = count;",
                "            this.increment = increment;",
                "        }",
                "",
                "        //this is a comment",
                "        protected internal int IncrementAndGet()",
                "        {",
                "            //this is a comment",
                "            count = count + increment;",
                "            return count;",
                "        }",
                "    }",
                "}"
            };
            ProviderTestUtils.DoCSharpTest(expected, ProviderTestUtils.TestSimpleClassCompileUnit());
        }
        
        [Fact]
        public void TestStatementsAndExpressions()
        {
            string[] expected = new string[]
            {
                "namespace Test.Namespace",
                "{",
                "    public class TestClass",
                "    {",
                "        public void Method()",
                "        {",
                "            new Class[] {new A(), new A(this.field, arg)};",
                "            new double[arg * avar];",
                "            avar[(int) 5.5f, Math.Abs(-2)];",
                "            default(uint);",
                "            typeof(int);",
                "            for (int i = 0; i < 10; i = i + 1)",
                "            {",
                "                try",
                "                {",
                "                    base.EventsHolder.AnEvent += new Action(this.Method);",
                "                }",
                "                catch (Exception e)",
                "                {",
                "                }",
                "                catch (AnotherException e)",
                "                {",
                "                }",
                "            }",
                "        }",
                "    }",
                "}"
            };
            ProviderTestUtils.DoCSharpTest(expected, ProviderTestUtils.TestStatementsAndExpressionsCompileUnit());
        }
        
        [Fact]
        public void TestSnippet()
        {
            string[] expected = new string[]
            {
                "namespace Test.Namespace",
                "{",
                "    public class TestClass",
                "    {",
                "        public void SnippetMember()",
                "        {",
                "        ",
                "        }",
                "",
                "        public void Method()",
                "        {",
                "            SnippetStatement",
                "                on multiple lines",
                "            i = snippet expression;",
                "            i2 = multiline",
                "                snippet expression;",
                "        }",
                "    }",
                "}"
            };
            ProviderTestUtils.DoCSharpTest(expected, ProviderTestUtils.TestSnippetCompileUnit());
        }

        [Fact]
        public void TestPrimitives()
        {
            string[] expected = new string[]
            {
                "namespace Test.Namespace",
                "{",
                "    public class TestClass",
                "    {",
                "        private short s = (short) 2;",
                "",
                "        private uint ui = 2U;",
                "    }",
                "}"
            };
            ProviderTestUtils.DoCSharpTest(expected, ProviderTestUtils.TestPrimitivesCompileUnit());
        }
        
        [Fact]
        public void TestIdentifiers()
        {
            string[] expected = new string[]
            {
                "namespace Test.Namespace",
                "{",
                "    public class Class",
                "    {",
                "        private int @value;",
                "",
                "        private void @namespace()",
                "        {",
                "            @namespace();",
                "            this.@namespace();",
                "        }",
                "    }",
                "}"
            };
            ProviderTestUtils.DoCSharpTest(expected, ProviderTestUtils.TestIdentifiersCompileUnit());
        }
        
        [Fact]
        public void TestDirective()
        {
            string[] expected =
            {
                "#region Compile unit region",
                "namespace Test.Namespace",
                "{",
                "    #region Class region",
                "    public class TestClass",
                "    {",
                "        #region Fields region",
                "        private int a;",
                "",
                "        private int b;",
                "        #endregion",
                "",
                "        public void Method()",
                "        {",
                "            #region region a",
                "            #region region b",
                "            a = b;",
                "            return a;",
                "            #endregion",
                "            #endregion",
                "        }",
                "    }",
                "    #endregion",
                "}",
                "#endregion"
            };
            ProviderTestUtils.DoCSharpTest(expected, ProviderTestUtils.TestDirectiveCompileUnit());
        }
    }
}