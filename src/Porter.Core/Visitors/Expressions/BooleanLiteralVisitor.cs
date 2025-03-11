using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class BooleanLiteralVisitor : NodeVisitor<BooleanLiteral>
{
    public override void Visit(BooleanLiteral node)
    {
        throw new NotImplementedException();
    }
}
