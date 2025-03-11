using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Statements;

public class ReturnStatementVisitor : NodeVisitor<ReturnStatement>
{
    public override void Visit(ReturnStatement node)
    {
        throw new NotImplementedException();
    }
}
