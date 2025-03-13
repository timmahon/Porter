using Microsoft.CodeAnalysis.CSharp;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class ParenthesizedExpressionVisitor : NodeVisitor<ParenthesizedExpression>
{
    private readonly ConversionContext _context;
    private readonly IExpressionMapper _expressionMapper;

    public ParenthesizedExpressionVisitor(ConversionContext context, IExpressionMapper expressionMapper)
    {
        _context = context;
        _expressionMapper = expressionMapper;
    }

    public override void Visit(ParenthesizedExpression node)
    {
        _context.PushExpression(
            SyntaxFactory.ParenthesizedExpression(
                _expressionMapper.MapExpression(node.getExpression())));
    }
}
