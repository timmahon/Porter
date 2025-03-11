namespace Porter.Builder;

public class CompilationUnitHolder
{
    public int Errors { get; }
    public List<SourcePathCompilationUnitPair> Pairs { get; }

    public CompilationUnitHolder(int errors, List<SourcePathCompilationUnitPair> pairs)
    {
        Errors=errors;
        Pairs=pairs;
    }
}
