using Microsoft.CodeAnalysis.CSharp;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class ArrayAccessVisitor : NodeVisitor<ArrayAccess>
{
    private readonly ConversionContext _context;
    private readonly IExpressionMapper _expressionMapper;

    public ArrayAccessVisitor(ConversionContext context, IExpressionMapper expressionMapper)
    {
        _context = context;
        _expressionMapper = expressionMapper;
    }

    public override void Visit(ArrayAccess node)
    {
        var arraySyntax = _expressionMapper.MapExpression(node.getArray());
        var indexSyntax = _expressionMapper.MapExpression(node.getIndex());

        var elementAccess = SyntaxFactory.ElementAccessExpression(arraySyntax,
            SyntaxFactory.BracketedArgumentList(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Argument(indexSyntax))));

        _context.PushExpression(elementAccess);
    }
}
