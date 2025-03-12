using org.eclipse.jdt.core.dom;

namespace Porter.AstDump;

public class AstDumper
{
    private readonly IAstPrinter _printer;

    public AstDumper(IAstPrinter printer)
    {
        _printer = printer;
    }

    public void Dump(ASTNode rootNode)
    {
        _printer.StartPrint();
        Walk(rootNode);
        _printer.EndPrint();
    }


    public override string ToString()
    {
        return _printer.ToString() ?? string.Empty;
    }

    private void Walk(ASTNode node)
    {
        Walk(node, false);
    }

    private void Walk(java.util.List nodes)
    {
        for (var iterator = nodes.iterator(); iterator.hasNext();)
        {
            ASTNode node = (ASTNode)iterator.next();
            Walk(node, true);
        }
    }

    private void Walk(ASTNode node, bool parentIsList)
    {
        _printer.StartType(node.getClass().getSimpleName(), parentIsList);
        var properties = node.structuralPropertiesForType();

        for (var iterator = properties.iterator(); iterator.hasNext();)
        {
            object descriptor = iterator.next();
            if (descriptor is SimplePropertyDescriptor simple)
            {
                object value = node.getStructuralProperty(simple);
                _printer.Literal(simple.getId(), value);
            }
            else if (descriptor is ChildPropertyDescriptor child)
            {
                ASTNode childNode = (ASTNode)node.getStructuralProperty(child);
                if (childNode != null)
                {
                    _printer.StartElement(child.getId(), false);
                    Walk(childNode);
                    _printer.EndElement(child.getId(), false);
                }
            }
            else if (descriptor is ChildListPropertyDescriptor list)
            {
                var value = (java.util.List)node.getStructuralProperty(list);
                if (value.size() > 0)
                {
                    _printer.StartElement(list.getId(), true);
                    Walk(value);
                    _printer.EndElement(list.getId(), true);
                }
            }
        }

        _printer.EndType(node.getClass().getSimpleName(), parentIsList);
    }
}