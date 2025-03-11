using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class StringLiteralVisitor : NodeVisitor<StringLiteral>
{
    public override void Visit(StringLiteral node)
    {
        throw new NotImplementedException();
    }
}
