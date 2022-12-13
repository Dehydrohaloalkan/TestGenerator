using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TestGenerator.Core;

namespace TestGenerator.Tests
{
    public class Tests
    {
        private List<TestInfo> _list;
        private string _startCode;

        [SetUp]
        public void Setup()
        {
            _startCode = File.ReadAllText(@"../../../Classes/MyClass.cs");
            var writer = new Core.CodeWriter.Writer(_startCode);
            _list = writer.Generate().ToList();
        }

        [Test]
        public void Test_ClassesCount()
        {
            Assert.That(1, Is.EqualTo(_list.Count));
        }

        [Test]
        public void Test_MultipleClasses()
        {
            var writer = new Core.CodeWriter.Writer(File.ReadAllText(@"../../../Classes/MyClass2.cs"));
            var list = writer.Generate().ToList();
            Assert.That(2, Is.EqualTo(list.Count));
        }

        [Test]
        public void Test_MethodsCount()
        {
            var startSyntaxTree = CSharpSyntaxTree.ParseText(_startCode);
            var startMethodCount = 
                startSyntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().Count();

            var generatedSyntaxTree = CSharpSyntaxTree.ParseText(_list.First().Code);
            var generatedMethodCount = 
                generatedSyntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().Count();

            Assert.That(startMethodCount, Is.EqualTo(generatedMethodCount));
        }

        [Test]
        public void Test_UniqueMethodNames()
        {
            var startSyntaxTree = CSharpSyntaxTree.ParseText(_startCode);
            var startMethodCount =
                startSyntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().Count();

            var generatedSyntaxTree = CSharpSyntaxTree.ParseText(_list.First().Code);
            var generatedMethods = 
                generatedSyntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>();
            var UniqueMethodCount = generatedMethods.GroupBy(method => method.Identifier.Text).Count();

            Assert.That(startMethodCount, Is.EqualTo(UniqueMethodCount));
        }

        [Test]
        public void Test_AllUsingsGenerated()
        {

            var expectedUsings = new List<string>() { "NUnit.Framework", "TestGenerator.Tests.Classes" };
            var actualUsings = CSharpSyntaxTree.ParseText(_list.First().Code)
                .GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>()
                .Select(u => u.Name.ToString()).ToList();

            foreach (var actualUsing in actualUsings)
            {
                Assert.That(expectedUsings, Does.Contain(actualUsing));
            }
        }
    }
}