namespace Porter;

public class ProjectMapping
{
    public string Name { get; set; } = default!;
    public string PrefixToRemove { get; set; } = default!;
    public string FilePath { get; set; } = default!;
    public List<string> JavaPaths { get; set; } = [];
}
