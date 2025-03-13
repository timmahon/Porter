using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Declarations;

public class TypeDeclarationVisitor : NodeVisitor<TypeDeclaration>
{
    private readonly ITypeMapper _typeMapper;

    public TypeDeclarationVisitor(ITypeMapper typeMapper)
    {
        _typeMapper=typeMapper;
    }

    public override void Visit(TypeDeclaration node)
    {
        _typeMapper.MapType(node);
    }
}
