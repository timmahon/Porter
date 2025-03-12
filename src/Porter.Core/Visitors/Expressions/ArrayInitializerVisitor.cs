using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using org.eclipse.jdt.core.dom;
using Porter.Extensions;
using Porter.Mappings;

namespace Porter.Visitors.Expressions;

public class ArrayInitializerVisitor : NodeVisitor<ArrayInitializer>
{
    private readonly ConversionContext _context;
    private readonly IExpressionMapper _expressionMapper;
    private readonly IMappings _mappings;

    public ArrayInitializerVisitor(ConversionContext context, IExpressionMapper expressionMapper, IMappings mappings)
    {
        _context = context;
        _expressionMapper = expressionMapper;
        _mappings = mappings;
    }

    public override void Visit(ArrayInitializer node)
    {
        if (node.getParent() is not ArrayCreation)
        {
            // process implicity typed array creation
            _context.WithExpectedType(node.resolveTypeBinding().getElementType(), () =>
            {
                var arrayCreationExpression = SyntaxFactory.ArrayCreationExpression(
                    (ArrayTypeSyntax)_mappings.MappedTypeReference(node.resolveTypeBinding().getElementType()),
                    MapArrayInitializer(node));
                _context.PushExpression(arrayCreationExpression);
            });
        }
        else
        {
            _context.PushExpression(MapArrayInitializer(node));
        }
    }

    private InitializerExpressionSyntax MapArrayInitializer(ArrayInitializer node)
    {
        var expressions = node.expressions().ToList<Expression>();
        var initializerExpressions = expressions.Select(_expressionMapper.MapExpression).ToList();
        return SyntaxFactory.InitializerExpression(SyntaxKind.ArrayInitializerExpression,
            SyntaxFactory.SeparatedList(
                initializerExpressions,
                Enumerable.Repeat(SyntaxFactory.Token(SyntaxKind.CommaToken),
                Math.Max(0, initializerExpressions.Count - 1))));
    }
}
