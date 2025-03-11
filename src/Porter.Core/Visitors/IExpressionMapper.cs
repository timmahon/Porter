using Microsoft.CodeAnalysis.CSharp.Syntax;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors;

public interface IExpressionMapper
{
    ExpressionSyntax MapExpression(Expression expression);
    ExpressionSyntax MapExpression(ITypeBinding? expectedType, Expression expression);
}
