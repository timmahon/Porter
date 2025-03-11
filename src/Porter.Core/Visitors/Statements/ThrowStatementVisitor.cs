using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Statements;

public class ThrowStatementVisitor : NodeVisitor<ThrowStatement>
{
    public override void Visit(ThrowStatement node)
    {
        throw new NotImplementedException();
    }
}
