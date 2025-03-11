using org.eclipse.aether.util.artifact;

namespace Porter.Maven;

internal class MavenReference
{
    public string GroupId { get; set; } = string.Empty;
    public string ArtifactId { get; set; } = string.Empty;
    public string? Classifier { get; set; }
    public string Version { get; set; } = string.Empty;
    public bool Optional { get; set; } = false;
    public string Scope { get; set; } = JavaScopes.COMPILE;
}
