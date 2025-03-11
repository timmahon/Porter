using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class NumberLiteralVisitor : NodeVisitor<NumberLiteral>
{
    public override void Visit(NumberLiteral node)
    {
        throw new NotImplementedException();
    }
}
