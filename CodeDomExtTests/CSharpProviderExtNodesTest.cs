using Xunit;

namespace CodeDomExtTests
{
    public class CSharpProviderExtNodesTest
    {
        [Fact]
        public void TestExpressionsAndStatements()
        {
            string[] expected = new string[]
            {
                "namespace Test.Namespace",
                "{",
                "    public class TestClass",
                "    {",
                "        public void Method()",
                "        {",
                "            (i > 0) ? i : null;",
                "            do",
                "            {",
                "                a <<= 2;",
                "            } while (!true);",
                "            lambda = (int i, int j) =>",
                "            {",
                "                b ?? 0;",
                "                new A()",
                "                {",
                "                    Prop = 0,",
                "                    Prop2 = 2",
                "                };",
                "            };",
                "            while (false)",
                "            {",
                "                i++;",
                "                break;",
                "            }",
                "            using (var stream = new Stream())",
                "            {",
                "                var matrix = new int[,] {{1, 1}, {1, 1}};",
                "            }",
                "            foreach (int i in collection)",
                "            {",
                "                new int[1, 1] {{1}};",
                "                new int[1, 1];",
                "            }",
                "        }",
                "    }",
                "}"
            };
            
            ProviderTestUtils.DoCSharpTest(expected, ProviderExtTestUtils.TestExpressionsAndStatementsCompileUnit());
        }
        
        [Fact]
        public void TestPropertiesAndMethodSignature()
        {
            string[] expected = new string[]
            {
                "namespace Test.Namespace",
                "{",
                "    public class TestClass",
                "    {",
                "        public virtual int Prop",
                "        {",
                "            get;",
                "            private set;",
                "        } = 1;",
                "",
                "        public void Method(int a, int b = 0)",
                "        {",
                "        }",
                "    }",
                "}"
            };
            
            ProviderTestUtils.DoCSharpTest(expected, ProviderExtTestUtils.TestPropertiesAndMethodSignatureCompileUnit());
        }
        
        [Fact]
        public void TestStaticClass()
        {
            string[] expected = new string[]
            {
                "namespace Test.Namespace",
                "{",
                "    public static class TestClass",
                "    {",
                "        public static void Method()",
                "        {",
                "        }",
                "    }",
                "}"
            };
            
            ProviderTestUtils.DoCSharpTest(expected, ProviderExtTestUtils.TestStaticClassCompileUnit());
        }
    }
}