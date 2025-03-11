using org.eclipse.jdt.core.compiler;

namespace Porter.Builder;

public interface IBuilderOutput
{
    void OutputProblem(IProblem problem);
    void OutputBuiltFile(string path);
}
