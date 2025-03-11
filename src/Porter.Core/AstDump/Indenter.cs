using System.Text;

namespace Porter.AstDump;

public class Indenter
{
    private readonly StringBuilder sb = new StringBuilder();
    private int _indentationSize = 4;

    protected string GetIndentString()
    {
        return sb.ToString();
    }

    public void SetIndentationSize(int indentationSize)
    {
        this._indentationSize = indentationSize;
    }

    protected void Indent()
    {
        for (int i = 0; i < _indentationSize; i++)
        {
            sb.Append(' ');
        }
    }

    protected void UnIndent()
    {
        sb.Length -= _indentationSize;
    }
}