using org.eclipse.jdt.core.dom;

namespace Porter.Visitors;

public interface ITypeMapper
{
    void MapType(AbstractTypeDeclaration node);
}
