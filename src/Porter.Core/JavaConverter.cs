using Microsoft.Extensions.DependencyInjection;
using org.eclipse.jdt.core.dom;
using Porter.AstDump;
using Porter.Builder;
using Porter.Visitors;

namespace Porter;

public class JavaConverter
{
    private readonly IServiceProvider _serviceProvider;
    private readonly OutputContext _outputContext;
    private readonly PorterConfiguration _configuration;

    public JavaConverter(
        IServiceProvider serviceProvider,
        OutputContext outputContext,
        PorterConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _outputContext = outputContext;
        _configuration=configuration;
    }

    public void Convert(List<SourcePathCompilationUnitPair> pairs)
    {
        if (_outputContext.DumpASTs)
        {
            DumpASTs(pairs);
        }
        foreach (var pair in pairs)
        {
            //ConsoleHelper.WriteWithColor(ConsoleColor.DarkCyan, $"Converting {pair.Path}");
            using var scope = _serviceProvider.CreateScope();
            var compilationUnitVisitor = scope.ServiceProvider.GetRequiredService<CompilationUnitVisitor>();
            var csCompilationUnit = compilationUnitVisitor.VisitCompilationUnit(pair);
            if (csCompilationUnit != null)
            {
                // 
            }

        }

        CopyOverlay();
    }

    //private ProjectMapping? FindProjectMapping(string source, CompilationUnitSyntax csModule)
    //{
    //    foreach (var projectMapping in _outputContext.ProjectMappings)
    //    {
    //        foreach (var javaPath in projectMapping.JavaPaths)
    //        {
    //            var matcher = new Matcher();
    //            matcher.AddInclude(javaPath);
    //            var pathToMatch = System.IO.Path.GetRelativePath(_outputContext.JavaPath, source);
    //            if (matcher.Match(pathToMatch).HasMatches)
    //            {
    //                return projectMapping;
    //            }
    //        }
    //    }

    //    return null;
    //}

    internal virtual string TargetFolderForCompilationUnit(ProjectMapping? projectMapping, string cu, string generatedNamespace)
    {
        if (_outputContext.NetPath == null)
        {
            throw new System.ArgumentException("_targetProjectPath");
        }

        string targetFolder = _outputContext.NetPath;


        // compute target folder based on packageName
        if (_outputContext.ProjectName != null)
        {
            targetFolder = System.IO.Path.Combine(targetFolder, _outputContext.ProjectName);
        }

        if (projectMapping != null)
        {
            targetFolder = System.IO.Path.Combine(targetFolder, projectMapping.FilePath);
        }

        string cuParent = Directory.GetParent(cu).FullName.Replace("\\", "/");
        string packageName = generatedNamespace == null ? cuParent.Substring(cuParent.LastIndexOf("/")) : CleanupNamespace(generatedNamespace);
        if (packageName.Length > 0)
        {
            return GetTargetPackageFolder(targetFolder, packageName);
        }
        return targetFolder;
    }

    public static string CleanupNamespace(string generatedNamespace)
    {
        // remove any keyword markers from the namespace
        return generatedNamespace.Replace("@", string.Empty);
    }

    private string GetTargetPackageFolder(string targetFolder, string packageName)
    {
        //if (!this._configuration.FlatNamespaceDirectoryStructure())
        //{
        packageName = packageName.Replace('.', '/').ToLower();
        //}
        return targetFolder + "/" + packageName;
    }

    private void CopyOverlay()
    {
        if (_outputContext.OverlayMappings.Length > 0)
        {
            foreach (var overlayMapping in _outputContext.OverlayMappings)
            {
                var targetPath = System.IO.Path.Combine("net", overlayMapping.To);
                try
                {
                    foreach (string dirPath in Directory.GetDirectories(System.IO.Path.Combine(_outputContext.BasePath, overlayMapping.From), "*", SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(dirPath.Replace(overlayMapping.From, targetPath));
                    }

                    foreach (string newPath in Directory.GetFiles(System.IO.Path.Combine(_outputContext.BasePath, overlayMapping.From), "*.*", SearchOption.AllDirectories))
                    {
                        File.Copy(newPath, newPath.Replace(overlayMapping.From, targetPath), true);
                    }
                }
                catch (DirectoryNotFoundException)
                {

                }
            }
        }
    }

    private void DumpASTs(List<SourcePathCompilationUnitPair> pairs)
    {
        foreach (var item in pairs)
        {
            var packageName = PackageName(item.Path, item.CompilationUnit);
            var jsonFile = packageName.Replace('.', System.IO.Path.DirectorySeparatorChar) + ".json";
            var outputFile = Path.Combine(_outputContext.NetPath, "asts", jsonFile);

            Directory.CreateDirectory(Path.GetDirectoryName(outputFile));

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            var dumper = new AstDumper(new JsonStyleAstPrinter(sb));
            dumper.Dump(item.CompilationUnit);

            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                sw.Write(dumper.ToString());
            }
        }
    }

    private string PackageName(string javaPath, CompilationUnit compilationUnit)
    {
        var packageTypeNameVisitor = new PackageTypeNameVisitor();
        compilationUnit.accept(packageTypeNameVisitor);

        var packageName = packageTypeNameVisitor.PackageName;
        string typeName = packageTypeNameVisitor.TypeName;
        if (string.IsNullOrEmpty(typeName))
        {
            typeName = Path.GetFileNameWithoutExtension(javaPath);
        }

        if (string.IsNullOrEmpty(packageName))
        {
            return typeName;
        }

        return $"{packageName}.{typeName}";
    }

    private class PackageTypeNameVisitor : ASTVisitor
    {
        public string PackageName { get; private set; } = default!;
        public string TypeName { get; private set; } = default!;

        public void Clear()
        {
            PackageName = null;
            TypeName = null;
        }

        public override bool visit(PackageDeclaration node)
        {
            PackageName = node.getName().ToString();
            return false;
        }

        public override bool visit(TypeDeclaration node)
        {
            if (Modifier.isPublic(node.getModifiers()))
            {
                TypeName = node.getName().ToString();
            }
            return false;
        }

        public override bool visit(EnumDeclaration node)
        {
            if (Modifier.isPublic(node.getModifiers()))
            {
                TypeName = node.getName().ToString();
            }
            return false;
        }
    }
}
