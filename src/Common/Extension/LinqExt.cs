namespace System.Linq;

public static class LinqExt
{
    public static void Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Action<TSource> action)
    {
        foreach (var item in source)
        {
            if (predicate(item))
            {
                action(item);
            }
        }
    }
}
