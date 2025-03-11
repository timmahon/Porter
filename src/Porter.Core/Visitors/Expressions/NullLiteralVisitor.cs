using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class NullLiteralVisitor : NodeVisitor<NullLiteral>
{
    public override void Visit(NullLiteral node)
    {
        throw new NotImplementedException();
    }
}
