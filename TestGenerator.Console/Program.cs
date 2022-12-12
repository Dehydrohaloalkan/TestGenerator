namespace TestGenerator.Console;

public static class Application
{
    private const int MaxDegreeOfParallelism = 10;

    public static async Task Main(string[] args)
    {
        if (args.Length is 0 or >= 5)
        {
            System.Console.WriteLine("Invalid number of parameters");
            System.Console.WriteLine("Print -help for help");
            return;
        }

        if (!Directory.Exists(args[0]))
        {
            if (args[0] == "-help")
            {
                PrintHelp();
                return;
            }
            System.Console.WriteLine("Invalid directory path");
            return;
        }
        var path = args[0];
        var readMax = MaxDegreeOfParallelism;
        var processMax = MaxDegreeOfParallelism;
        var writeMax = MaxDegreeOfParallelism;

        if (args.Length >= 2)
        {
            if (!int.TryParse(args[1], out var value))
            {
                System.Console.WriteLine("Invalid second parameter value");
                return;
            }
            readMax = value;
        }

        if (args.Length >= 3)
        {
            if (!int.TryParse(args[2], out var value))
            {
                System.Console.WriteLine("Invalid third parameter value");
                return;
            }
            processMax = value;
        }

        if (args.Length == 4)
        {
            if (!int.TryParse(args[3], out var value))
            {
                System.Console.WriteLine("Invalid fourth parameter value");
                return;
            }
            writeMax = value;
        }

        var generator = new Core.TestsGenerator();
        await generator.Generate(path, readMax, processMax, writeMax);
    }

    private static void PrintHelp()
    {
        System.Console.WriteLine(
    @"USAGE:
    TestGenerator.exe [Path] <Read> <Process> <Write>
    Path ----- Path to files with C# code
    Read ----- Max degree of Parallelism for Read
    Process -- Max degree of Parallelism for Process
    White ---- Max degree of Parallelism for Write
    "
            );

    }

}