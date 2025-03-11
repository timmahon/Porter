namespace Porter.Builder;

public class SourcePathEntry
{
    public string Path { get; set; } = default!;
    public List<string> Includes { get; set; } = [];
    public List<string> Excludes { get; set; } = [];

    public static SourcePathEntry DefaultsForPath(string path)
    {
        return new SourcePathEntry
        {
            Path = path,
            Includes = ["**/*.java"],
            Excludes = ["**/package-info.java"]
        };
    }
}
