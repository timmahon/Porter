using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using org.eclipse.jdt.core.dom;
using Porter.Extensions;
using Porter.Mappings;

namespace Porter.Visitors.Expressions;

public class LambdaExpressionVisitor : NodeVisitor<LambdaExpression>
{
    private readonly ConversionContext _context;
    private readonly IExpressionMapper _expressionMapper;
    private readonly IBlockProcessor _blockProcessor;
    private readonly IMappings _mappings;

    public LambdaExpressionVisitor(ConversionContext context, 
        IExpressionMapper expressionMapper, 
        IBlockProcessor blockProcessor, 
        IMappings mappings)
    {
        _context = context;
        _expressionMapper = expressionMapper;
        _blockProcessor = blockProcessor;
        _mappings = mappings;
    }

    public override void Visit(LambdaExpression node)
    {
        var parameters = node.parameters().ToList<VariableDeclaration>();
        var parameterSyntaxes = parameters.Select((vd) =>
        {
            var typeName = _mappings.MappedTypeReference(vd.resolveBinding().getType());
            var identifier = vd.getName().getIdentifier();
            return SyntaxFactory.Parameter(
                [],
                SyntaxFactory.TokenList(),
                typeName,
                SyntaxFactory.ParseToken(identifier),
                null);
        }).ToList();

        CSharpSyntaxNode body;
        if (node.getBody() is Expression bodyExpression)
        {
            body = _expressionMapper.MapExpression(bodyExpression);
        }
        else if (node.getBody() is Block)
        {
            var block = SyntaxFactory.Block();
            _blockProcessor.VisitBlock(SyntaxFactory.Block(), node.getBody());
            body = block;
        }
        else
        {
            throw new NotImplementedException("Unknown lambda body type");
        }

        _context.PushExpression(
            SyntaxFactory.ParenthesizedLambdaExpression(
                SyntaxFactory.ParameterList(
                    SyntaxFactory.SeparatedList(parameterSyntaxes)), body));
    }
}
