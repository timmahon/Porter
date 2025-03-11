using Microsoft.CodeAnalysis.CSharp.Syntax;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors;

public class ExpressionMapper : IExpressionMapper
{
    private readonly INodeDispatcher _nodeDispatcher;
    private readonly ConversionContext _resultContext;

    public ExpressionMapper(INodeDispatcher nodeDispatcher, ConversionContext resultContext)
    {
        _nodeDispatcher = nodeDispatcher;
        _resultContext = resultContext;
    }

    public ExpressionSyntax MapExpression(Expression expression)
    {
        throw new NotImplementedException();
    }

    public ExpressionSyntax MapExpression(ITypeBinding? expectedType, Expression expression)
    {
        throw new NotImplementedException();
    }
}
