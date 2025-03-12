using Microsoft.CodeAnalysis.CSharp;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class AssignmentVisitor : NodeVisitor<Assignment>
{
    private readonly ConversionContext _context;
    private readonly IExpressionMapper _expressionMapper;

    public AssignmentVisitor(ConversionContext context, IExpressionMapper expressionMapper)
    {
        _context = context;
        _expressionMapper = expressionMapper;
    }

    public override void Visit(Assignment node)
    {
        _context.WithExpectedType(node.getLeftHandSide().resolveTypeBinding(), () =>
        {
            var leftSyntax = _expressionMapper.MapExpression(node.getLeftHandSide());
            var rightSyntax = _expressionMapper.MapExpression(node.getRightHandSide());

            var op = node.getOperator();
            var kind = SyntaxKind.None;


            if (op == Assignment.Operator.ASSIGN)
            {
                kind = SyntaxKind.SimpleAssignmentExpression;
            }
            else if (op == Assignment.Operator.BIT_AND_ASSIGN)
            {
                kind = SyntaxKind.AndAssignmentExpression;
            }
            else if (op == Assignment.Operator.BIT_OR_ASSIGN)
            {
                kind = SyntaxKind.OrAssignmentExpression;
            }
            else if (op == Assignment.Operator.BIT_XOR_ASSIGN)
            {
                kind = SyntaxKind.ExclusiveOrAssignmentExpression;
            }
            else if (op == Assignment.Operator.DIVIDE_ASSIGN)
            {
                kind = SyntaxKind.DivideAssignmentExpression;
            }
            else if (op == Assignment.Operator.LEFT_SHIFT_ASSIGN)
            {
                kind = SyntaxKind.LeftShiftAssignmentExpression;
            }
            else if (op == Assignment.Operator.MINUS_ASSIGN)
            {
                kind = SyntaxKind.SubtractAssignmentExpression;
            }
            else if (op == Assignment.Operator.PLUS_ASSIGN)
            {
                kind = SyntaxKind.AddAssignmentExpression;
            }
            else if (op == Assignment.Operator.REMAINDER_ASSIGN)
            {
                kind = SyntaxKind.ModuloAssignmentExpression;
            }
            else if (op == Assignment.Operator.RIGHT_SHIFT_SIGNED_ASSIGN)
            {
                kind = SyntaxKind.RightShiftAssignmentExpression;
            }
            else if (op == Assignment.Operator.RIGHT_SHIFT_UNSIGNED_ASSIGN)
            {
                // TODO: Add support for earlier versions which don't support this.
                kind = SyntaxKind.UnsignedRightShiftAssignmentExpression;
            }
            else if (op == Assignment.Operator.TIMES_ASSIGN)
            {
                kind = SyntaxKind.MultiplyAssignmentExpression;
            }

            _context.PushExpression(SyntaxFactory.AssignmentExpression(kind, leftSyntax, rightSyntax));
        });
    }
}
