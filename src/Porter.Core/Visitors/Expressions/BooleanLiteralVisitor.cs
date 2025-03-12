using Microsoft.CodeAnalysis.CSharp;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class BooleanLiteralVisitor : NodeVisitor<BooleanLiteral>
{
    private readonly ConversionContext _context;

    public BooleanLiteralVisitor(ConversionContext context)
    {
        _context = context;
    }

    public override void Visit(BooleanLiteral node)
    {
        if (node.booleanValue())
        {
            _context.PushExpression(SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression));
        }
        else
        {
            _context.PushExpression(SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression));
        }
    }
}
