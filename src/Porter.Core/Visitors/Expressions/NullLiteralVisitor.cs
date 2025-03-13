using Microsoft.CodeAnalysis.CSharp;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class NullLiteralVisitor : NodeVisitor<NullLiteral>
{
    private readonly ConversionContext _context;

    public NullLiteralVisitor(ConversionContext context)
    {
        _context = context;
    }

    public override void Visit(NullLiteral node)
    {
        // TODO: return "default" when the expected type is a type variable.

        _context.PushExpression(SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression));
    }
}
