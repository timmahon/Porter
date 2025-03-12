using JavaType = org.eclipse.jdt.core.dom.Type;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using org.eclipse.jdt.core.dom;
using Porter.Extensions;
using Porter.Mappings;

namespace Porter.Visitors.Expressions;

public class ArrayCreationVisitor : NodeVisitor<ArrayCreation>
{
    private readonly ConversionContext _context;
    private readonly IExpressionMapper _expressionMapper;
    private readonly IMappings _mappings;

    public ArrayCreationVisitor(ConversionContext context, IExpressionMapper expressionMapper, IMappings mappings)
    {
        _context = context;
        _expressionMapper = expressionMapper;
        _mappings = mappings;
    }

    public override void Visit(ArrayCreation node)
    {
        _context.WithExpectedType(node.getType().getElementType().resolveBinding(), () =>
        {
            ArrayType type = node.getType();

            if (node.getParent() is CastExpression castExpression &&
                castExpression.getType() is ArrayType arrayType &&
                arrayType.getElementType().resolveBinding().isTypeVariable() ||
                type.getElementType().resolveBinding().isRawType())
            {
                // convert explicit casts for raw array creation to the generic type.
                type = (ArrayType)((CastExpression)node.getParent()).getType();
            }

            var dimensions = node.dimensions().ToList<Expression>();
            var initializer = node.getInitializer();

            var dimensionsSyntaxes = dimensions.Select(_expressionMapper.MapExpression).ToList();
            var initializerSyntax = initializer is null ? null : (InitializerExpressionSyntax)_expressionMapper.MapExpression(initializer);

            var arrayCreationExpression = SyntaxFactory.ArrayCreationExpression(
                SyntaxFactory.Token(SyntaxKind.NewKeyword),
                MappedArrayReference(type.getElementType(), type.getDimensions() - 1),
                initializerSyntax)
                .AddTypeRankSpecifiers(
                    SyntaxFactory.ArrayRankSpecifier(
                        SyntaxFactory.SeparatedList(
                            dimensionsSyntaxes,
                            Enumerable.Repeat(SyntaxFactory.Token(SyntaxKind.CommaToken),
                            dimensionsSyntaxes.Count -1))));

            _context.PushExpression(arrayCreationExpression);
        });
    }

    private ArrayTypeSyntax MappedArrayReference(JavaType type, int dimension)
    {
        ITypeBinding binding = type.resolveBinding();
        if (dimension > 0)
        {
            binding = binding.createArrayType(dimension);
        }
        return (ArrayTypeSyntax)_mappings.MappedTypeReference(binding);
    }
}
