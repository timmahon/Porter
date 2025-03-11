using Porter.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porter;

public class StartupContext
{
    public string BasePath { get; set; } = default!;
    public string JavaPath { get; set; } = default!;
    public string NetPath { get; set; } = default!;
    public string[] MavenDependencies { get; set; } = [];
    public SourcePathEntry[] SourceFolders { get; set; } = [];
    public string[] JavaRT { get; set; } = default!;
    public string[] ClassPathEntries { get; set; } = [];
    public ProjectMapping[] ProjectMappings { get; set; } = [];
    public OverlayMapping[] OverlayMappings { get; set; } = [];
    public string ProjectName { get; set; } = default!;
    public bool DumpASTs { get; set; }
}