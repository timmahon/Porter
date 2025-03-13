using Microsoft.CodeAnalysis.CSharp.Syntax;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors;

public interface IBlockProcessor
{
    void VisitBodyDeclarationBlock(BodyDeclaration node, Block block, MethodDeclarationSyntax method);
    void ProcessBlock(BodyDeclaration node, Block block, BlockSyntax targetBlock);
    void VisitBlock<T>(BlockSyntax block, T node) where T : ASTNode;
}
