namespace Porter.Extensions;

public static class JavaUtilListExtensions
{
    public static List<T> ToList<T>(this java.util.List list)
    {
        var result = new List<T>();
        for (var i = 0; i < list.size(); i++)
        {
            result.Add((T)list.get(i));
        }
        return result;
    }
}
