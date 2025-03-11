namespace Porter.AstDump;

public interface IAstPrinter
{
    void StartPrint();
    void EndPrint();
    void StartElement(string name, bool isList);
    void EndElement(string name, bool isList);
    void StartType(string name, bool parentIsList);
    void EndType(string name, bool parentIsList);
    void Literal(string name, object value);
}