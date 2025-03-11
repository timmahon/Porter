using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Expressions;

public class ParenthesizedExpressionVisitor : NodeVisitor<ParenthesizedExpression>
{
    public override void Visit(ParenthesizedExpression node)
    {
        throw new NotImplementedException();
    }
}
