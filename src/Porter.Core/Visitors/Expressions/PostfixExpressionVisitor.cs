using Microsoft.CodeAnalysis.CSharp;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class PostfixExpressionVisitor : NodeVisitor<PostfixExpression>
{
    private readonly ConversionContext _context;
    private readonly IExpressionMapper _expressionMapper;

    public PostfixExpressionVisitor(ConversionContext context,
        IExpressionMapper expressionMapper)
    {
        _context = context;
        _expressionMapper = expressionMapper;
    }

    public override void Visit(PostfixExpression node)
    {
        var expressionSyntax = _expressionMapper.MapExpression(node.getOperand());
        var op = node.getOperator();
        var kind = SyntaxKind.None;

        if (op == PostfixExpression.Operator.DECREMENT)
        {
            kind = SyntaxKind.PostDecrementExpression;
        }
        else if (op == PostfixExpression.Operator.INCREMENT)
        {
            kind = SyntaxKind.PostIncrementExpression;
        }

        _context.PushExpression(SyntaxFactory.PostfixUnaryExpression(kind, expressionSyntax));
    }
}
