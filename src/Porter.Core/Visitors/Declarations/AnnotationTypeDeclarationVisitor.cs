using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Declarations;

public class AnnotationTypeDeclarationVisitor : NodeVisitor<AnnotationTypeDeclaration>
{
    private readonly ITypeMapper _typeMapper; 

    public AnnotationTypeDeclarationVisitor(ITypeMapper typeMapper)
    {
        _typeMapper = typeMapper;
    }

    public override void Visit(AnnotationTypeDeclaration node)
    {
        _typeMapper.MapType(node);
    }
}
