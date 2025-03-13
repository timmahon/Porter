using Microsoft.CodeAnalysis.CSharp;
using org.eclipse.jdt.core.dom;
using Porter.Extensions;
using Porter.Mappings;

namespace Porter.Visitors.Expressions;

public class FieldAccessVisitor : NodeVisitor<FieldAccess>
{
    private readonly ConversionContext _context;
    private readonly IExpressionMapper _expressionMapper;
    private readonly IMappings _mappings;

    public FieldAccessVisitor(ConversionContext context,
        IExpressionMapper expressionMapper,
        IMappings mappings)
    {
        _context = context;
        _expressionMapper = expressionMapper;
        _mappings = mappings;
    }


    public override void Visit(FieldAccess node)
    {
        string name = MappedFieldName(node);

        if (node.getExpression() == null)
        {
            _context.PushExpression(
                SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                    default!,
                    SyntaxFactory.IdentifierName(name)
                ));
        }
        else
        {
            _context.PushExpression(
                SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    _expressionMapper.MapExpression(node.getExpression()),
                    SyntaxFactory.IdentifierName(name)
                ));
        }
    }

    private string MappedFieldName(FieldAccess node)
    {
        string? name = MappedFieldName(node.getName());
        return name != null ? name : Identifier(node.getName());
    }

    private string? MappedFieldName(Name node)
    {
        var binding = node.ResolveVariableBinding();
        return binding == null ? null : _mappings.MappedFieldName(binding);
    }

    private string Identifier(SimpleName name)
    {
        // TODO: Add extension point for custom field casing etc...
        return name.ToString();
    }
}
