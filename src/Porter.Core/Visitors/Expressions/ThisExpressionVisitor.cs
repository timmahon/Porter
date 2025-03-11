using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class ThisExpressionVisitor : NodeVisitor<ThisExpression>
{
    public override void Visit(ThisExpression node)
    {
        throw new NotImplementedException();
    }
}
