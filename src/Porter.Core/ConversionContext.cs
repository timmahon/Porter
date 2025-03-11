using Microsoft.CodeAnalysis.CSharp.Syntax;
using org.eclipse.jdt.core.dom;

namespace Porter;

public class ConversionContext
{
    public CompilationUnit CompilationUnit { get; set; } = default!;
    public List<Initializer> InstanceInitializers { get; set; } = [];
    public BodyDeclaration CurrentBodyDeclaration { get; set; } = default!;
    public CompilationUnitSyntax CSCompilationUnit { get; set; } = default!;
    public TypeDeclarationSyntax CurrentType { get; set; } = default!;
    public TypeDeclarationSyntax CurrentAuxiliaryType { get; set; } = default!;
    public string Source { get; internal set; }
}
