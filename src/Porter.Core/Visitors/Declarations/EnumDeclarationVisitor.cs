using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Declarations;

public class EnumDeclarationVisitor : NodeVisitor<EnumDeclaration>
{
    private readonly ITypeMapper _typeMapper;

    public EnumDeclarationVisitor(ITypeMapper typeMapper)
    {
        _typeMapper=typeMapper;
    }

    public override void Visit(EnumDeclaration node)
    {
        _typeMapper.MapType(node);
    }
}
