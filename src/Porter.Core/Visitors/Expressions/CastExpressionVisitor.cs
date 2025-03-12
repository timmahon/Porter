using Microsoft.CodeAnalysis.CSharp;
using org.eclipse.jdt.core.dom;
using Porter.Mappings;

namespace Porter.Visitors.Expressions;

public class CastExpressionVisitor : NodeVisitor<CastExpression>
{
    private readonly ConversionContext _context;
    private readonly IMappings _mappings;
    private readonly IExpressionMapper _expressionMapper;

    public CastExpressionVisitor(ConversionContext context, IMappings mappings, IExpressionMapper expressionMapper)
    {
        _context = context;
        _mappings = mappings;
        _expressionMapper = expressionMapper;
    }

    public override void Visit(CastExpression node)
    {
        _context.PushExpression(
            SyntaxFactory.CastExpression(
                _mappings.MappedTypeReference(node.getType(), false),
                _expressionMapper.MapExpression(node.getExpression())));

        // TODO: only cast to unchecked if the expression is negative...
        // Make all byte casts unchecked, this stops C# being upset on minus values
        if (node.getType().resolveBinding().getName().Equals("byte"))
        {
            _context.PushExpression(
                SyntaxFactory.CheckedExpression(
                    SyntaxKind.UncheckedExpression,
                    _context.PopExpression()));
        }
    }
}
