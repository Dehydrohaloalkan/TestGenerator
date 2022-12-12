using System.Threading.Tasks.Dataflow;
using TestGenerator.Core.CodeWriter;

namespace TestGenerator.Core;

public class TestsGenerator
{
    public async void Generate(string path, int readMax, int processMax, int writeMax)
    {
        var readBlock = new TransformBlock<string, string>(ReadFile,
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = readMax });
        
        var generateBlock = new TransformBlock<string, TestInfo>(GenerateCode,
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = processMax });
        
        var writeBlock = new ActionBlock<TestInfo>(code => WriteFile(Path.Combine(path, "Tests"), code),
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = writeMax });
        
        readBlock.LinkTo(generateBlock, new DataflowLinkOptions { PropagateCompletion = true });
        generateBlock.LinkTo(writeBlock, new DataflowLinkOptions { PropagateCompletion = true });

        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        var files = directoryInfo.GetFiles().Select(f => f.FullName);
        foreach (var file in files)
        {
            readBlock.Post(file);
        }

        readBlock.Complete();
        writeBlock.Completion.Wait();

    }

    private async Task<string> ReadFile(string fileName)
    {
        return await File.ReadAllTextAsync(fileName);
    }

    private TestInfo GenerateCode(string code)
    {
        var writer = new Writer();
        return writer.Generate(code);
    }

    private void WriteFile(string outputFolder, TestInfo testInfo)
    {
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }
        File.WriteAllTextAsync(Path.Combine(outputFolder, $"{testInfo.Name}Test.cs"), testInfo.Code);
    }
}