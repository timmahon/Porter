namespace Porter.Builder;

public interface IJavaBuilder
{
    CompilationUnitHolder CompilationUnitHolder { get; }
    void Build();
}
