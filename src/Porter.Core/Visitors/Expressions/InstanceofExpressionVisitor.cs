using Microsoft.CodeAnalysis.CSharp;
using org.eclipse.jdt.core.dom;
using Porter.Mappings;

namespace Porter.Visitors.Expressions;

public class InstanceofExpressionVisitor : NodeVisitor<InstanceofExpression>
{
    private readonly ConversionContext _context;
    private readonly IExpressionMapper _expressionMapper;
    private readonly IMappings _mappings;
    
    public InstanceofExpressionVisitor(ConversionContext context, IExpressionMapper expressionMapper, IMappings mappings)
    {
        _context = context;
        _expressionMapper = expressionMapper;
        _mappings = mappings;
    }

    public override void Visit(InstanceofExpression node)
    {
        // TODO: Sharpen calls an runtime instance method if the type
        // is generic

        var leftSyntax = _expressionMapper.MapExpression(node.getLeftOperand());
        var rightSyntax = _mappings.MappedTypeReference(node.getRightOperand(), true);

        _context.PushExpression(SyntaxFactory.BinaryExpression(SyntaxKind.IsExpression, leftSyntax, rightSyntax));
    }
}
