using Microsoft.CodeAnalysis.CSharp;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class NumberLiteralVisitor : NodeVisitor<NumberLiteral>
{
    private readonly ConversionContext _context;

    public NumberLiteralVisitor(ConversionContext context)
    {
        _context = context;
    }

    public override void Visit(NumberLiteral node)
    {
        // TODO:
        // 1) Unchecked when using hex literals with overflow
        // 2) Octal literals

        var token = node.getToken();
        _context.PushExpression(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(token)));
    }
}
