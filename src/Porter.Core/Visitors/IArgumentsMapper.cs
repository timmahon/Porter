using Microsoft.CodeAnalysis.CSharp.Syntax;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors;

public interface IArgumentsMapper
{
    void MapArguments(List<ArgumentSyntax> argumentSyntaxes, IList<Expression> arguments);
    void AddArgument(List<ArgumentSyntax> argumentSyntaxes, Expression argument, ITypeBinding? expectedType);
}
