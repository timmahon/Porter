using Microsoft.CodeAnalysis.CSharp.Syntax;
using org.eclipse.jdt.core.dom;

namespace Porter.Mappings;

public interface IMappings
{
    TypeSyntax MappedTypeReference(ITypeBinding binding);
    TypeSyntax MappedTypeReference(org.eclipse.jdt.core.dom.Type type, bool v);
}
