using Microsoft.CodeAnalysis.CSharp;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class ConditionalExpressionVisitor : NodeVisitor<ConditionalExpression>
{
    private readonly ConversionContext _context;
    private readonly IExpressionMapper _expressionMapper;

    public ConditionalExpressionVisitor(ConversionContext context, IExpressionMapper expressionMapper)
    {
        _context = context;
        _expressionMapper = expressionMapper;
    }

    public override void Visit(ConditionalExpression node)
    {
        _context.PushExpression(
            SyntaxFactory.ConditionalExpression(
                _expressionMapper.MapExpression(node.getExpression()),
                _expressionMapper.MapExpression(node.getThenExpression()),
                _expressionMapper.MapExpression(node.getElseExpression())
            )
        );
    }
}
