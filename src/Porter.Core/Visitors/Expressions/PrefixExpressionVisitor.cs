using Microsoft.CodeAnalysis.CSharp;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class PrefixExpressionVisitor : NodeVisitor<PrefixExpression>
{
    private readonly ConversionContext _context;
    private readonly IExpressionMapper _expressionMapper;

    public PrefixExpressionVisitor(ConversionContext context, 
        IExpressionMapper expressionMapper)
    {
        _context = context;
        _expressionMapper = expressionMapper;
    }

    public override void Visit(PrefixExpression node)
    {
        var expressionSyntax = _expressionMapper.MapExpression(node.getOperand());
        var op = node.getOperator();
        var kind = SyntaxKind.None;

        if (op == PrefixExpression.Operator.COMPLEMENT)
        {
            kind = SyntaxKind.BitwiseNotExpression;
        }
        else if (op == PrefixExpression.Operator.DECREMENT)
        {
            kind = SyntaxKind.PreDecrementExpression;
        }
        else if (op == PrefixExpression.Operator.INCREMENT)
        {
            kind = SyntaxKind.PreIncrementExpression;
        }
        else if (op == PrefixExpression.Operator.MINUS)
        {
            kind = SyntaxKind.UnaryMinusExpression;
        }
        else if (op == PrefixExpression.Operator.NOT)
        {
            kind = SyntaxKind.LogicalNotExpression;
        }
        else if (op == PrefixExpression.Operator.PLUS)
        {
            kind = SyntaxKind.UnaryPlusExpression;
        }

        _context.PushExpression(
            SyntaxFactory.PrefixUnaryExpression(kind, expressionSyntax));
    }
}
