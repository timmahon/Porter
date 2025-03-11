using org.eclipse.jdt.core.dom;

namespace Porter.Visitors.Comments;

public class LineCommentVisitor : NodeVisitor<LineComment>
{
    public override void Visit(LineComment node)
    {
        throw new NotImplementedException();
    }
}
