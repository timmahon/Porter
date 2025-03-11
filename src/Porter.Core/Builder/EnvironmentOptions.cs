using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porter.Builder;

public class EnvironmentOptions
{
    public List<string> SourceFiles { get; } = [];
    public List<string> ClassPathEntries { get; } = [];
    public List<SourcePathEntry> SourcePathEntries { get; } = [];
    public bool IncludeRunningVMBootClassPath { get; set; }
}
