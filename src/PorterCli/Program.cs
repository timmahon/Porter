using Porter;
using Porter.Builder;

namespace PorterCli;

internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.Error.WriteLine("Error: expected basepath to project as first argument");
            return;
        }

        var basePath = args[0];

        var config = new PorterConfiguration();
        var startupContext = new StartupContext();
        config.StartUp = startupContext;

        startupContext.BasePath = basePath;
        if (args.Length > 1 && args[1] != null)
        {
            startupContext.JavaPath = args[1];
        }
        else
        {
            startupContext.JavaPath = Path.Combine(basePath, "java");
        }

        startupContext.NetPath = Path.Combine(basePath, "net");


        startupContext.MavenDependencies = [
            "junit:junit:4.12",
            "org.osgi:org.osgi.core:4.2.0",
            "org.osgi:org.osgi.compendium:4.2.0",
            "commons-io:commons-io:2.4"];

        startupContext.JavaRT = new string[] { "f:\\rt.jar" };
        startupContext.ProjectMappings = [
            new ProjectMapping
            {
                Name = "OpenNLP.NET",
                FilePath = "src/OpenNLP.NET",
                JavaPaths = ["opennlp/opennlp-tools/src/main/**/*.java"]
            },
            new ProjectMapping
            {
                Name = "OpenNLP.Tests",
                FilePath = "test/OpenNLP.NET.Tests",
                JavaPaths = ["opennlp/opennlp-tools/src/test/**/*.java"]
            }];

        startupContext.SourceFolders = [
            SourcePathEntry.DefaultsForPath("opennlp\\opennlp-tools\\src\\main\\java\\opennlp"),
            SourcePathEntry.DefaultsForPath("opennlp\\opennlp-tools\\src\\test\\java\\opennlp")
        ];

        startupContext.OverlayMappings = [
            new OverlayMapping("overlay\\base", "")
            ];

        startupContext.ProjectName = "OpenNLP.Net";
        startupContext.DumpASTs = true;

        PorterConversion.Convert(config);
    }
}
