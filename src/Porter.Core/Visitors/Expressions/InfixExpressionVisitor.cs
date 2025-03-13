using Microsoft.CodeAnalysis.CSharp;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class InfixExpressionVisitor : NodeVisitor<InfixExpression>
{
    private readonly ConversionContext _context;
    private readonly IExpressionMapper _expressionMapper;

    public InfixExpressionVisitor(ConversionContext context,
        IExpressionMapper expressionMapper)
    {
        _context = context;
        _expressionMapper = expressionMapper;
    }

    public override void Visit(InfixExpression node)
    {
        var leftSyntax = _expressionMapper.MapExpression(node.getLeftOperand());
        var rightSyntax = _expressionMapper.MapExpression(node.getRightOperand());

        var op = node.getOperator();
        var kind = SyntaxKind.None;

        if (op == InfixExpression.Operator.AND)
        {
            kind = SyntaxKind.BitwiseAndExpression;
        }
        else if (op == InfixExpression.Operator.CONDITIONAL_AND)
        {
            kind = SyntaxKind.LogicalAndExpression;
        }
        else if (op == InfixExpression.Operator.CONDITIONAL_OR)
        {
            kind = SyntaxKind.LogicalOrExpression;
        }
        else if (op == InfixExpression.Operator.DIVIDE)
        {
            kind = SyntaxKind.DivideExpression;
        }
        else if (op == InfixExpression.Operator.EQUALS)
        {
            kind = SyntaxKind.EqualsExpression;
        }
        else if (op == InfixExpression.Operator.GREATER)
        {
            kind = SyntaxKind.GreaterThanExpression;
        }
        else if (op == InfixExpression.Operator.GREATER_EQUALS)
        {
            kind = SyntaxKind.GreaterThanOrEqualExpression;
        }
        else if (op == InfixExpression.Operator.LEFT_SHIFT)
        {
            kind = SyntaxKind.LeftShiftExpression;
        }
        else if (op == InfixExpression.Operator.LESS)
        {
            kind = SyntaxKind.LessThanExpression;
        }
        else if (op == InfixExpression.Operator.LESS_EQUALS)
        {
            kind = SyntaxKind.LessThanOrEqualExpression;
        }
        else if (op == InfixExpression.Operator.MINUS)
        {
            kind = SyntaxKind.SubtractExpression;
        }
        else if (op == InfixExpression.Operator.NOT_EQUALS)
        {
            kind = SyntaxKind.NotEqualsExpression;
        }
        else if (op == InfixExpression.Operator.OR)
        {
            kind = SyntaxKind.BitwiseOrExpression;
        }
        else if (op == InfixExpression.Operator.PLUS)
        {
            kind = SyntaxKind.AddExpression;
        }
        else if (op == InfixExpression.Operator.REMAINDER)
        {
            kind = SyntaxKind.ModuloExpression;
        }
        else if (op == InfixExpression.Operator.RIGHT_SHIFT_SIGNED)
        {
            kind = SyntaxKind.RightShiftExpression;
        }
        else if (op == InfixExpression.Operator.RIGHT_SHIFT_UNSIGNED)
        {
            kind = SyntaxKind.UnsignedRightShiftExpression;
        }
        else if (op == InfixExpression.Operator.TIMES)
        {
            kind = SyntaxKind.MultiplyExpression;
        }
        else if (op == InfixExpression.Operator.XOR)
        {
            kind = SyntaxKind.ExclusiveOrExpression;
        }

        if (node.hasExtendedOperands())
        {
            // TODO: Currently, don't know what these are,
            // so if encountered, throw an exception, so the
            // AST and source code can be analyzed and
            // the conversion can be updated to handle them..
            throw new NotImplementedException("Extended operands in InfixExpression not implemented");
        }

        var binaryExpression = SyntaxFactory.BinaryExpression(kind, leftSyntax, rightSyntax);
        _context.PushExpression(binaryExpression);
    }
}
