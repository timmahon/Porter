using org.eclipse.jdt.core.dom;

namespace Porter.Visitors;

public abstract class NodeVisitor
{
    protected internal abstract void Visit(ASTNode node);
    protected internal abstract void EndVisit(ASTNode node);
}

public abstract class NodeVisitor<T> : NodeVisitor where T : ASTNode
{
    public virtual void Visit(T node) { }
    public virtual void EndVisit(T node) { }

    protected internal sealed override void Visit(ASTNode node)
    {
        Visit((T)node);
    }

    protected internal sealed override void EndVisit(ASTNode node)
    {
        EndVisit((T)node);
    }
}
