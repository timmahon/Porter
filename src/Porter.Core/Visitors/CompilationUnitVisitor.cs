using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using org.eclipse.jdt.core.dom;
using Porter.Builder;
using Porter.Extensions;

namespace Porter.Visitors;

public class CompilationUnitVisitor
{
    private readonly INodeDispatcher _nodeDispatcher;
    private readonly ConversionContext _context;
    private readonly PorterConfiguration _configuration;

    public CompilationUnitVisitor(INodeDispatcher nodeDispatcher, ConversionContext context, PorterConfiguration configuration)
    {
        _nodeDispatcher = nodeDispatcher;
        _context = context;
        _configuration = configuration;
    }

    public CompilationUnitSyntax VisitCompilationUnit(SourcePathCompilationUnitPair pair)
    {
        _context.CSCompilationUnit = SyntaxFactory.CompilationUnit();
        _context.Source = File.ReadAllText(pair.Path);
        _context.CompilationUnit = pair.CompilationUnit;

        var packageDeclaration = _context.CompilationUnit.getPackage();
        if (packageDeclaration != null)
        {
            _nodeDispatcher.VisitNode(packageDeclaration);
        }

        var types = _context.CompilationUnit.types().ToList<AbstractTypeDeclaration>();
        foreach (var type in types)
        {
            _nodeDispatcher.VisitNode(type);
        }

        return _context.CSCompilationUnit;
    }
}
