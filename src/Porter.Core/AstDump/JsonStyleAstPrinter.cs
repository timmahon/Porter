using System.Text;

namespace Porter.AstDump;

public class JsonStyleAstPrinter : Indenter, IAstPrinter
{
    private readonly StringBuilder _sb;
    private readonly Stack<bool> _hasItemsStack = new Stack<bool>();

    public JsonStyleAstPrinter(StringBuilder sb)
    {
        _sb = sb;
        // make sure there is one item on the stack which represents
        // the root node
        _hasItemsStack.Push(false);
    }

    public void StartElement(string name, bool isList)
    {
        if (_hasItemsStack.Peek() == true)
        {
            _sb.AppendLine(",");
        }
        else
        {
            _hasItemsStack.Pop();
            _hasItemsStack.Push(true);
        }

        _sb.AppendLine(GetIndentString() + "\"" + name + "\": " + (isList ? "[" : "{"));
        Indent();
        _hasItemsStack.Push(false);
    }

    public void EndElement(string name, bool isList)
    {
        UnIndent();
        _sb.AppendLine();
        _sb.Append(GetIndentString() + (isList ? "]" : "}"));
        _hasItemsStack.Pop();
    }

    public void StartType(string name, bool parentIsList)
    {
        if (_hasItemsStack.Peek() == true)
        {
            _sb.AppendLine(",");
        }
        else
        {
            _hasItemsStack.Pop();
            _hasItemsStack.Push(true);
        }

        if (parentIsList)
        {
            _sb.AppendLine(GetIndentString() + "{");
            Indent();
        }

        _sb.Append(GetIndentString() + "\"node\": \"" + name + "\"");
    }

    public void EndType(string name, bool parentIsList)
    {
        if (parentIsList)
        {
            UnIndent();
            _sb.AppendLine();
            _sb.Append(GetIndentString() + "}");
        }
    }

    private static readonly HashSet<System.Type> JsonAllowedWrapperTypes = new HashSet<System.Type>
        {
            typeof(java.lang.Boolean),
            typeof(java.lang.Byte),
            typeof(java.lang.Short),
            typeof(java.lang.Integer),
            typeof(java.lang.Long),
            typeof(java.lang.Float),
            typeof(java.lang.Double),
            typeof(java.util.ArrayList)
        };

    private static bool IsJsonAllowedType(System.Type type)
    {
        return JsonAllowedWrapperTypes.Contains(type);
    }
    public void Literal(string name, object value)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(value);

        if (_hasItemsStack.Peek() == true)
        {
            _sb.AppendLine(",");
        }
        else
        {
            _hasItemsStack.Pop();
            _hasItemsStack.Push(true);
        }

        _sb.Append(GetIndentString() + "\"" + name + "\": ");
        if (value == null)
        {
            _sb.Append("null");
        }
        else if (IsJsonAllowedType(value.GetType()))
        {
            _sb.Append(value);
        }
        else
        {
            _sb.Append("\"" + JsonEscape(value.ToString()) + "\"");
        }
    }

    public void StartPrint()
    {
        _sb.AppendLine("{");
        Indent();
    }

    public void EndPrint()
    {
        _sb.AppendLine();
        UnIndent();
        _sb.AppendLine("}");
    }

    public override string ToString()
    {
        return _sb.ToString();
    }

    private string JsonEscape(string? s)
    {
        if (s == null || s.Length == 0)
        {
            return "";
        }

        int i;
        int len = s.Length;
        StringBuilder sb = new StringBuilder(len + 4);
        string t;

        for (i = 0; i < len; i += 1)
        {
            char c = s[i];
            switch (c)
            {
                case '\\':
                case '"':
                    sb.Append('\\');
                    sb.Append(c);
                    break;
                case '/':
                    sb.Append('\\');
                    sb.Append(c);
                    break;
                case '\b':
                    sb.Append("\\b");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\f':
                    sb.Append("\\f");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                default:
                    if (c < ' ')
                    {
                        t = "000" + string.Format("X", c);
                        sb.Append("\\u" + t.Substring(t.Length - 4));
                    }
                    else
                    {
                        sb.Append(c);
                    }
                    break;
            }
        }
        return sb.ToString();
    }
}

