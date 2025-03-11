using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class MethodInvocationVisitor : NodeVisitor<MethodInvocation>
{
    public override void Visit(MethodInvocation node)
    {
        base.Visit(node);
    }
}
