using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Statements;

public class EmptyStatementVisitor : NodeVisitor<EmptyStatement>
{
    public override void Visit(EmptyStatement node)
    {
        throw new NotImplementedException();
    }
}
