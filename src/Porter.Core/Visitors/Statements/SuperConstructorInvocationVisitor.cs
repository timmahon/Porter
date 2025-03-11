using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Statements;

public class SuperConstructorInvocationVisitor : NodeVisitor<SuperConstructorInvocation>
{
    public override void Visit(SuperConstructorInvocation node)
    {
        throw new NotImplementedException();
    }
}
