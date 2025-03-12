using Microsoft.CodeAnalysis.CSharp.Syntax;
using org.eclipse.jdt.core.dom;

namespace Porter;

public class ConversionContext
{
    public CompilationUnit CompilationUnit { get; set; } = default!;
    public List<Initializer> InstanceInitializers { get; set; } = [];
    public BodyDeclaration CurrentBodyDeclaration { get; set; } = default!;
    public CompilationUnitSyntax CSCompilationUnit { get; set; } = default!;
    public NamespaceDeclarationSyntax Namespace { get; set; } = default!;
    public TypeDeclarationSyntax CurrentType { get; set; } = default!;
    public TypeDeclarationSyntax CurrentAuxiliaryType { get; set; } = default!;
    public string Source { get; set; } = default!;
    public ExpressionSyntax CurrentExpression { get; private set; } = default!;
    private ITypeBinding? CurrentExpectedType { get; set; } = default!;

    public void PushExpression(ExpressionSyntax expression)
    {
        if (CurrentExpression is not null)
        {
            throw new InvalidOperationException("Expression already set");
        }
        CurrentExpression = expression;
    }

    public ExpressionSyntax PopExpression()
    {
        if (CurrentExpression is null)
        {
            throw new InvalidOperationException("No expression set");
        }
        var expression = CurrentExpression;
        CurrentExpression = null!;
        return expression;
    }

    public void WithExpectedType(ITypeBinding type, Action action)
    {
        var saved = CurrentExpectedType;
        CurrentExpectedType = type;
        action?.Invoke();
        CurrentExpectedType = saved;
    }

    public bool ExpectingType(string name)
    {
        return (CurrentExpectedType != null && CurrentExpectedType.getName().Equals(name));
    }
}