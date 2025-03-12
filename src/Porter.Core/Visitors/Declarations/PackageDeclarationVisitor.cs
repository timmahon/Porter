using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Declarations;

public class PackageDeclarationVisitor : NodeVisitor<PackageDeclaration>
{
    private readonly ConversionContext _context;

    public PackageDeclarationVisitor(ConversionContext context)
    {
        _context = context;
    }

    public override void Visit(PackageDeclaration node)
    {
        // Get the package name from the Java package declaration
        var packageName = node.getName().getFullyQualifiedName();
        
        // Create a C# namespace declaration from the package name
        var nameParts = packageName.Split('.');
        NameSyntax namespaceName = SyntaxFactory.IdentifierName(nameParts[0]);
        
        // Build a qualified name if there are multiple parts
        for (int i = 1; i < nameParts.Length; i++)
        {
            namespaceName = SyntaxFactory.QualifiedName(namespaceName, SyntaxFactory.IdentifierName(nameParts[i]));
        }
        
        // Create the namespace declaration
        var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(namespaceName)
            .WithOpenBraceToken(SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
            .WithCloseBraceToken(SyntaxFactory.Token(SyntaxKind.CloseBraceToken));
        
        // Store the namespace in the conversion context
        _context.Namespace = namespaceDeclaration;
        
        // Update the compilation unit with the namespace
        _context.CSCompilationUnit = _context.CSCompilationUnit.AddMembers(namespaceDeclaration);
    }
}