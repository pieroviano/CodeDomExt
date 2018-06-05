using System.CodeDom;
using System.Reflection;
using CodeDomExt.Helpers;
using CodeDomExt.Nodes;
using CodeDomExt.Utils;

namespace CodeDomExtTests
{
    public static class ProviderExtTestUtils
    {
        public static CodeCompileUnit TestExpressionsAndStatementsCompileUnit(bool generateIncrement = true)
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

            method.Statements.Add(new CodeConditionalOperatorExpression(
                new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("i"), CodeBinaryOperatorType.GreaterThan, Primitives.Int(0)),
                new CodeVariableReferenceExpression("i"),
                Primitives.Null));
            method.Statements.Add(new CodePostTestIterationStatement(
                new CodeUnaryOperatorExpression(Primitives.Bool(true), CodeUnaryOperatorType.BooleanNot), 
                new CodeOperationAssignmentStatement(
                    new CodeVariableReferenceExpression("a"), 
                    CodeBinaryOperatorTypeMore.LeftBitShift, 
                    Primitives.Int(2))));
            method.Statements.Add(new CodeAssignStatement(
                new CodeVariableReferenceExpression("lambda"),
                new CodeLambdaDeclarationExpression(
                    new CodeLambdaParameterDeclarationExpression[]
                    {
                        new CodeLambdaParameterDeclarationExpression(Types.Int, "i"),
                        new CodeLambdaParameterDeclarationExpression(Types.Int, "j")
                    },
                    new CodeExpressionStatement(new CodeBinaryOperatorExpressionMore(
                        new CodeVariableReferenceExpression("b"),
                        CodeBinaryOperatorTypeMore.NullCoalescing,
                        Primitives.Int(0))),
                    new CodeExpressionStatement(new CodeObjectCreateExpressionExt(
                        new CodeTypeReference("A"),
                        new CodeExpression[0],
                        new CodePropertyInitializerExpression("Prop", Primitives.Int(0)),
                        new CodePropertyInitializerExpression("Prop2", Primitives.Int(2)))))));
            var iteration = new CodeIterationStatement(null, Primitives.Bool(false), null);
            if (generateIncrement)
            {
                iteration.Statements.Add(new CodeExpressionStatement(new CodeUnaryOperatorExpression(
                    new CodeVariableReferenceExpression("i"),
                    CodeUnaryOperatorType.PostIncrement)));
            }
            else
            {
                iteration.Statements.Add(new CodeOperationAssignmentStatement(
                    new CodeVariableReferenceExpression("i"), CodeBinaryOperatorTypeMore.Add, Primitives.Int(1)));
            }

            iteration.Statements.Add(new CodeBreakStatement());
            method.Statements.Add(iteration);
            method.Statements.Add(new CodeUsingStatement("stream", new CodeObjectCreateExpression("Stream"),
                new CodeVariableDeclarationStatement("", "matrix",
                    new CodeMultidimensionalArrayCreateExpression(
                        Types.Int, 
                        2, 
                        new CodeArrayInitializerExpression(
                            new CodeArrayInitializerExpression(Primitives.Int(1), Primitives.Int(1)),
                            new CodeArrayInitializerExpression(Primitives.Int(1), Primitives.Int(1)))))));
            method.Statements.Add(new CodeForEachStatement(
                Types.Int, 
                "i", 
                new CodeVariableReferenceExpression("collection"),
                new CodeExpressionStatement(new CodeMultidimensionalArrayCreateExpression(
                    Types.Int,
                    new CodeExpression[] {Primitives.Int(1), Primitives.Int(1)},
                    new CodeArrayInitializerExpression(new CodeArrayInitializerExpression(Primitives.Int(1))))),
                new CodeExpressionStatement(new CodeMultidimensionalArrayCreateExpression(
                    Types.Int,
                    Primitives.Int(1), Primitives.Int(1)))));

            compileUnit.Namespaces.Add(codeNamespace);
            codeNamespace.Types.Add(classDeclaration);
            classDeclaration.Members.Add(method);

            return compileUnit;
        }
        
        public static CodeCompileUnit TestPropertiesAndMethodSignatureCompileUnit()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("Test.Namespace");
            CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration("TestClass")
            {
                IsClass = true
            };
            classDeclaration.TypeAttributes =
                (classDeclaration.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.Public;

            var property = new CodeMemberPropertyExt()
            {
                Name = "Prop",
                Attributes = MemberAttributes.Public,
                Type = Types.Int,
                HasGet = true,
                HasSet = true,
                SetAccessibilityLevel = AccessibilityLevel.Private,
                PropertyInitializer = Primitives.Int(1)
            };
            var method = new CodeMemberMethod()
            {
                Name = "Method",
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            method.Parameters.Add(new CodeParameterDeclarationExpressionExt(
                Types.Int, "a"));
            method.Parameters.Add(new CodeParameterDeclarationExpressionExt(
                Types.Int, "b", Primitives.Int(0)));
            
            compileUnit.Namespaces.Add(codeNamespace);
            codeNamespace.Types.Add(classDeclaration);
            classDeclaration.Members.Add(property);
            classDeclaration.Members.Add(method);
            return compileUnit;
        }

        public static CodeCompileUnit TestStaticClassCompileUnit()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("Test.Namespace");
            CodeTypeDeclaration classDeclaration = new CodeTypeDeclarationExt("TestClass")
            {
                IsClass = true,
                IsStatic = true
            };
            classDeclaration.TypeAttributes =
                (classDeclaration.TypeAttributes & ~TypeAttributes.VisibilityMask) | TypeAttributes.Public;

            var method = new CodeMemberMethod()
            {
                Name = "Method",
                Attributes = MemberAttributes.Public | MemberAttributes.Static
            };
            
            compileUnit.Namespaces.Add(codeNamespace);
            codeNamespace.Types.Add(classDeclaration);
            classDeclaration.Members.Add(method);
            return compileUnit;
        }
    }
}