using Microsoft.CodeAnalysis.CSharp;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class ExpressionMethodReferenceVisitor : NodeVisitor<ExpressionMethodReference>
{
    private readonly ConversionContext _context;

    public ExpressionMethodReferenceVisitor(ConversionContext context)
    {
        _context = context;
    }

    public override void Visit(ExpressionMethodReference node)
    {
        // TODO: Not entirely sure what these are at the moment
        // seem to be used in streams in place of a lambda
        // come back to this when more of the application is implemented,
        // for now, just push a null expression, so development can continue...
        _context.PushExpression(SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression));
    }
}
