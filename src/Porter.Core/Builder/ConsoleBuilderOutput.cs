using org.eclipse.jdt.core.compiler;

namespace Porter.Builder;

public class ConsoleBuilderOutput : IBuilderOutput
{
    public void OutputProblem(IProblem problem)
    {
        var consoleColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.Write(problem.getOriginatingFileName());
        Console.Error.WriteLine("(" + problem.getSourceLineNumber() + "): " + problem.getMessage());
        Console.ForegroundColor = consoleColor;
    }

    public void OutputBuiltFile(string path)
    {
        var consoleColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Built {path}");
        Console.ForegroundColor = consoleColor;
    }
}
