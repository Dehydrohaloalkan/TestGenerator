namespace TestGenerator.Core.CodeWriter;

internal class Writer
{
    public TestInfo Generate(string code)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
        var usings = syntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();


        return new TestInfo("", "");
    }

    private List<string> GetUsings() {
        
    }
}