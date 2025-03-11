using org.eclipse.jdt.core.dom;

namespace Porter.Visitors;

public interface INodeDispatcher
{
    void VisitNode(ASTNode node);
    void VisitNodes(ASTNode[] nodes);
}
