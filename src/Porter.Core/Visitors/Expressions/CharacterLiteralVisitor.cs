using Microsoft.CodeAnalysis.CSharp;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class CharacterLiteralVisitor : NodeVisitor<CharacterLiteral>
{
    private readonly ConversionContext _context;

    public CharacterLiteralVisitor(ConversionContext context)
    {
        _context = context;
    }

    public override void Visit(CharacterLiteral node)
    {
        _context.PushExpression(SyntaxFactory.LiteralExpression(SyntaxKind.CharacterLiteralExpression, SyntaxFactory.Literal(node.getEscapedValue())));

        // if we are expecting a byte, then we need to cast the character to a byte
        if (_context.ExpectingType("byte"))
        {
            _context.PushExpression(SyntaxFactory.CastExpression(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ByteKeyword)), _context.PopExpression()));
        }
    }
}
