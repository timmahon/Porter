using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using org.eclipse.jdt.core.dom;
using Porter.Extensions;
using Porter.Mappings;

namespace Porter.Visitors.Expressions;

public class ClassInstanceCreationVisitor : NodeVisitor<ClassInstanceCreation>
{
    private readonly INodeDispatcher _dispatcher;
    private readonly IExpressionMapper _expressionMapper;
    private readonly ConversionContext _context;
    private readonly IMappings _mappings;
    private readonly PorterConfiguration _configuration;
    private readonly IArgumentsMapper _argumentsMapper;

    public ClassInstanceCreationVisitor(INodeDispatcher dispatcher,
        IExpressionMapper expressionMapper,
        ConversionContext context,
        IMappings mappings,
        PorterConfiguration configuration,
        IArgumentsMapper argumentsMapper)
    {
        _dispatcher = dispatcher;
        _expressionMapper = expressionMapper;
        _context = context;
        _mappings = mappings;
        _configuration = configuration;
        _argumentsMapper = argumentsMapper;
    }

    public override void Visit(ClassInstanceCreation node)
    {
        if (node.getAnonymousClassDeclaration() != null)
        {
            _dispatcher.VisitNode(node.getAnonymousClassDeclaration());
            return;
        }

        //TODO: Macros, static methods, etc.  Currently assumes all java constructors
        //will map to c# constructors...
        var argumentSyntaxes = new List<ArgumentSyntax>();
        if (node.resolveTypeBinding().IsNonStaticNestedType())
        {
            argumentSyntaxes.Add(SyntaxFactory.Argument(SyntaxFactory.ThisExpression()));
        }
        _argumentsMapper.MapArguments(argumentSyntaxes, node.arguments().ToList<Expression>());

        var csType = _mappings.MappedTypeReference(node.resolveTypeBinding());

        var objectCreationExpression = SyntaxFactory.ObjectCreationExpression(
            csType,
            SyntaxFactory.ArgumentList(
                SyntaxFactory.SeparatedList(argumentSyntaxes,
                Enumerable.Repeat(SyntaxFactory.Token(SyntaxKind.CommaToken), Math.Max(0, argumentSyntaxes.Count - 1)))), 
            null);
        
        _context.PushExpression(objectCreationExpression);
    }
}
