namespace Porter;

public class OutputContext
{
    public string BasePath { get; set; } = default!;
    public string NetPath { get; set; } = default!;
    public string ProjectName { get; set; } = default!;
    public string JavaPath { get; set; } = default!;
    public ProjectMapping[] ProjectMappings { get; set; } = default!;
    public OverlayMapping[] OverlayMappings { get; set; } = default!;
    public bool DumpASTs { get; set; }
}