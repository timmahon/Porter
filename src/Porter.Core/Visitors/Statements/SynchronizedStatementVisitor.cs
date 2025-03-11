using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Statements;

public class SynchronizedStatementVisitor : NodeVisitor<SynchronizedStatement>
{
    public override void Visit(SynchronizedStatement node)
    {
        throw new NotImplementedException();
    }
}
