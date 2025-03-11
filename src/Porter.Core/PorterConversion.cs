using Runtime = org.eclipse.core.runtime;
using Microsoft.Extensions.DependencyInjection;
using org.eclipse.jdt.core.dom;
using Porter.Builder;
using Porter.Maven;
using Porter.Visitors.Comments;
using Porter.Visitors.Declarations;
using Porter.Visitors.Expressions;
using Porter.Visitors.Statements;
using Porter.Visitors;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

namespace Porter;

public class PorterConversion
{
    public static int Convert(PorterConfiguration pconfig)
    {
        var configurationContext = pconfig.StartUp;

        var environmentOptions = new EnvironmentOptions();
        Array.ForEach(configurationContext.SourceFolders,
            sourceFolder => environmentOptions.SourcePathEntries.Add(
                new SourcePathEntry
                {
                    Path = Path.Combine(configurationContext.JavaPath, sourceFolder.Path),
                    Includes = sourceFolder.Includes,
                    Excludes = sourceFolder.Excludes
                }));

        List<string> jars = [];
        if (configurationContext.MavenDependencies.Length > 0)
        {
            var repos = new List<MavenRepositoryItem>
                {
                    new MavenRepositoryItem("Maven", "https://repo1.maven.org/maven2/")
                };

            jars = FetchMavenDependencies.Execute(repos, new List<string>(configurationContext.MavenDependencies));
        }
        foreach (string javaRT in configurationContext.JavaRT)
        {
            jars.Add(javaRT);
        }
        environmentOptions.ClassPathEntries.AddRange(jars);

        foreach (var sourcePathEntry in environmentOptions.SourcePathEntries)
        {
            var folder = new DirectoryInfo(sourcePathEntry.Path);
            var matcher = new Microsoft.Extensions.FileSystemGlobbing.Matcher();
            foreach (var include in sourcePathEntry.Includes)
            {
                matcher.AddInclude(include);
            }

            foreach (var exclude in sourcePathEntry.Excludes)
            {
                matcher.AddExclude(exclude);
            }


            var result = matcher.Execute(new DirectoryInfoWrapper(folder));
            foreach (var file in result.Files)
            {
                environmentOptions.SourceFiles.Add(Path.Combine(sourcePathEntry.Path, file.Path));
            }
        }

        var outputContext = new OutputContext();
        outputContext.BasePath = configurationContext.BasePath;
        outputContext.NetPath = configurationContext.NetPath;
        outputContext.JavaPath = configurationContext.JavaPath;
        outputContext.ProjectName = configurationContext.ProjectName;
        outputContext.ProjectMappings = configurationContext.ProjectMappings;
        outputContext.OverlayMappings = configurationContext.OverlayMappings;
        outputContext.DumpASTs = configurationContext.DumpASTs;

        var services = new ServiceCollection();
        services.AddSingleton(sp => environmentOptions);
        services.AddSingleton(sp => pconfig);
        services.AddSingleton(sp => outputContext);
        services.AddScoped<IJavaBuilder, JavaBuilder>();
        services.AddScoped<IBuilderOutput, ConsoleBuilderOutput>();
        services.AddScoped<Runtime.IProgressMonitor, Runtime.NullProgressMonitor>();
        services.AddSingleton(sp => environmentOptions);
        services.AddScoped<ConversionContext>();
        services.AddScoped<JavaConverter>();
        services.AddScoped<INodeDispatcher, NodeDispatcher>();
        services.AddScoped<CompilationUnitVisitor>();

        services.TryAddNodeVisitor<BlockComment, BlockCommentVisitor>();
        services.TryAddNodeVisitor<LineComment, LineCommentVisitor>();

        services.TryAddNodeVisitor<AnnotationTypeDeclaration, AnnotationTypeDeclarationVisitor>();
        services.TryAddNodeVisitor<AnnotationTypeMemberDeclaration, AnnotationTypeMemberDeclarationVisitor>();
        services.TryAddNodeVisitor<AnonymousClassDeclaration, AnonymousClassDeclarationVisitor>();
        services.TryAddNodeVisitor<EnumDeclaration, EnumDeclarationVisitor>();
        services.TryAddNodeVisitor<EnumConstantDeclaration, EnumConstantDeclarationVisitor>();
        services.TryAddNodeVisitor<FieldDeclaration, FieldDeclarationVisitor>();
        services.TryAddNodeVisitor<Initializer, InitializerVisitor>();
        services.TryAddNodeVisitor<MethodDeclaration, MethodDeclarationVisitor>();
        services.TryAddNodeVisitor<PackageDeclaration, PackageDeclarationVisitor>();
        services.TryAddNodeVisitor<TypeDeclaration, TypeDeclarationVisitor>();

        services.TryAddNodeVisitor<AssertStatement, AssertStatementVisitor>();
        services.TryAddNodeVisitor<Block, BlockVisitor>();
        services.TryAddNodeVisitor<BreakStatement, BreakStatementVisitor>();
        services.TryAddNodeVisitor<ConstructorInvocation, ConstructorInvocationVisitor>();
        services.TryAddNodeVisitor<ContinueStatement, ContinueStatementVisitor>();
        services.TryAddNodeVisitor<DoStatement, DoStatementVisitor>();
        services.TryAddNodeVisitor<EmptyStatement, EmptyStatementVisitor>();
        services.TryAddNodeVisitor<EnhancedForStatement, EnhancedForStatementVisitor>();
        services.TryAddNodeVisitor<ExpressionStatement, ExpressionStatementVisitor>();
        services.TryAddNodeVisitor<ForStatement, ForStatementVisitor>();
        services.TryAddNodeVisitor<IfStatement, IfStatementVisitor>();
        services.TryAddNodeVisitor<LabeledStatement, LabeledStatementVisitor>();
        services.TryAddNodeVisitor<ReturnStatement, ReturnStatementVisitor>();
        services.TryAddNodeVisitor<SuperConstructorInvocation, SuperConstructorInvocationVisitor>();
        services.TryAddNodeVisitor<SuperMethodInvocation, SuperMethodInvocationVisitor>();
        services.TryAddNodeVisitor<SuperMethodReference, SuperMethodReferenceVisitor>();
        services.TryAddNodeVisitor<SwitchStatement, SwitchStatementVisitor>();
        services.TryAddNodeVisitor<SynchronizedStatement, SynchronizedStatementVisitor>();
        services.TryAddNodeVisitor<ThrowStatement, ThrowStatementVisitor>();
        services.TryAddNodeVisitor<TryStatement, TryStatementVisitor>();
        services.TryAddNodeVisitor<TypeDeclarationStatement, TypeDeclarationStatementVisitor>();
        services.TryAddNodeVisitor<VariableDeclarationStatement, VariableDeclarationStatementVisitor>();
        services.TryAddNodeVisitor<WhileStatement, WhileStatementVisitor>();

        services.TryAddNodeVisitor<ArrayAccess, ArrayAccessVisitor>();
        services.TryAddNodeVisitor<ArrayCreation, ArrayCreationVisitor>();
        services.TryAddNodeVisitor<ArrayInitializer, ArrayInitializerVisitor>();
        services.TryAddNodeVisitor<Assignment, AssignmentVisitor>();
        services.TryAddNodeVisitor<BooleanLiteral, BooleanLiteralVisitor>();
        services.TryAddNodeVisitor<CastExpression, CastExpressionVisitor>();
        services.TryAddNodeVisitor<CharacterLiteral, CharacterLiteralVisitor>();
        services.TryAddNodeVisitor<ClassInstanceCreation, ClassInstanceCreationVisitor>();
        services.TryAddNodeVisitor<ConditionalExpression, ConditionalExpressionVisitor>();
        services.TryAddNodeVisitor<CreationReference, CreationReferenceVisitor>();
        services.TryAddNodeVisitor<ExpressionMethodReference, ExpressionMethodReferenceVisitor>();
        services.TryAddNodeVisitor<FieldAccess, FieldAccessVisitor>();
        services.TryAddNodeVisitor<InfixExpression, InfixExpressionVisitor>();
        services.TryAddNodeVisitor<InstanceofExpression, InstanceofExpressionVisitor>();
        services.TryAddNodeVisitor<LambdaExpression, LambdaExpressionVisitor>();
        services.TryAddNodeVisitor<MethodInvocation, MethodInvocationVisitor>();
        services.TryAddNodeVisitor<NullLiteral, NullLiteralVisitor>();
        services.TryAddNodeVisitor<ParenthesizedExpression, ParenthesizedExpressionVisitor>();
        services.TryAddNodeVisitor<PostfixExpression, PostfixExpressionVisitor>();
        services.TryAddNodeVisitor<PrefixExpression, PrefixExpressionVisitor>();
        services.TryAddNodeVisitor<QualifiedName, QualifiedNameVisitor>();
        services.TryAddNodeVisitor<NumberLiteral, NumberLiteralVisitor>();
        services.TryAddNodeVisitor<SimpleName, SimpleNameVisitor>();
        services.TryAddNodeVisitor<StringLiteral, StringLiteralVisitor>();
        services.TryAddNodeVisitor<SuperFieldAccess, SuperFieldAccessVisitor>();
        services.TryAddNodeVisitor<ThisExpression, ThisExpressionVisitor>();
        services.TryAddNodeVisitor<TypeLiteral, TypeLiteralVisitor>();
        services.TryAddNodeVisitor<VariableDeclarationExpression, VariableDeclarationExpressionVisitor>();

        services.AddScoped<IExpressionMapper, ExpressionMapper>();
        
        var serviceProvider = services.BuildServiceProvider();

        var builder = serviceProvider.GetRequiredService<IJavaBuilder>();
        builder.Build();


        var converter = serviceProvider.GetRequiredService<JavaConverter>();
        converter.Convert(builder.CompilationUnitHolder.Pairs);
        return 0;
    }
}
