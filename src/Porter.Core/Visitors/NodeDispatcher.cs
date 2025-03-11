using Microsoft.Extensions.DependencyInjection;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors;

public class NodeDispatcher : INodeDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    
    public NodeDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void VisitNode(ASTNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        var visitor = _serviceProvider.GetKeyedService<NodeVisitor>(node.GetType());
        if (visitor != null)
        {
            visitor.Visit(node);
            visitor.EndVisit(node);
            return;
        }

        throw new InvalidOperationException($"No visitor found for node type {node.GetType().Name}");
    }

    public void VisitNodes(ASTNode[] nodes)
    {
        ArgumentNullException.ThrowIfNull(nodes);
        foreach (var node in nodes)
        {
            VisitNode(node);
        }
    }
}
