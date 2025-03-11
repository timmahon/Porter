using org.eclipse.core.runtime;
using org.eclipse.jdt.core.dom;
using org.eclipse.jdt.core;

namespace Porter.Builder;

public class JavaBuilder : IJavaBuilder
{
    private readonly IProgressMonitor _progressMonitor;
    private readonly EnvironmentOptions _environmentOptions;
    private readonly IBuilderOutput _compilerOutput;

    private const int _javaASTLevel = AST.JLS16;
    private const string _javaVersion = JavaCore.VERSION_16;
    private CompilationUnitHolder? _compilationUnitHolder;

    public JavaBuilder(IProgressMonitor progressMonitor,
        EnvironmentOptions environmentOptions,
        IBuilderOutput compilerOutput)
    {
        _progressMonitor=progressMonitor;
        _environmentOptions=environmentOptions;
        _compilerOutput=compilerOutput;
    }

    public CompilationUnitHolder CompilationUnitHolder
    {
        get
        {
            if (_compilationUnitHolder == null)
            {
                throw new InvalidOperationException("The compilation units have not been built, so pairs are not yet available!");
            }

            return _compilationUnitHolder;
        }
    }

    public void Build()
    {
        var pairs = ParseCompilationUnits();
        var errorCount = 0;
        foreach (var item in pairs)
        {
            if (OutputProblems(item.CompilationUnit))
            {
                errorCount++;
            }
        }

        _compilationUnitHolder = new CompilationUnitHolder(errorCount, pairs);
    }

    private List<SourcePathCompilationUnitPair> ParseCompilationUnits()
    {
        var requestor = new JavaBuilderASTRequestor(_compilerOutput.OutputBuiltFile);
        var parser = ASTParser.newParser(_javaASTLevel);
        var options = JavaCore.getOptions();
        options.put(JavaCore.COMPILER_COMPLIANCE, _javaVersion);
        options.put(JavaCore.COMPILER_CODEGEN_TARGET_PLATFORM, _javaVersion);
        options.put(JavaCore.COMPILER_SOURCE, _javaVersion);
        parser.setCompilerOptions(options);
        parser.setKind(ASTParser.K_COMPILATION_UNIT);
        parser.setResolveBindings(true);
        parser.setEnvironment(_environmentOptions.ClassPathEntries.ToArray(),
            SourcePathEntries(_environmentOptions.SourcePathEntries.ToArray()),
            null,
            _environmentOptions.IncludeRunningVMBootClassPath);
        parser.createASTs(_environmentOptions.SourceFiles.ToArray(),
            Encodings(_environmentOptions.SourceFiles.Count),
            Array.Empty<string>(),
            requestor,
            _progressMonitor);
        return requestor.Pairs;
    }

    private static string[] SourcePathEntries(SourcePathEntry[] sourcePathEntries)
    {
        string[] result = new string[sourcePathEntries.Length];
        for (int i = 0; i < sourcePathEntries.Length; i++)
        {
            result[i] = sourcePathEntries[i].Path;
        }

        return result;
    }

    private static string[] Encodings(int count)
    {
        string[] encodings = new string[count];
        Array.Fill(encodings, "UTF-8");
        return encodings;
    }

    private bool OutputProblems(CompilationUnit ast)
    {
        bool hasErrors = false;
        Array.ForEach(ast.getProblems(), (problem) =>
        {
            if (problem.isError())
            {
                _compilerOutput.OutputProblem(problem);
            }
        });

        return hasErrors;
    }

    private class JavaBuilderASTRequestor : FileASTRequestor
    {
        private readonly Action<string>? _callback;

        public JavaBuilderASTRequestor(Action<string>? callback = null)
        {
            _callback = callback;
        }

        public override void acceptAST(string sourceFilePath, CompilationUnit ast)
        {
            Pairs.Add(new SourcePathCompilationUnitPair(sourceFilePath, ast));
            _callback?.Invoke(sourceFilePath);
        }

        public List<SourcePathCompilationUnitPair> Pairs { get; } = [];
    }
}
