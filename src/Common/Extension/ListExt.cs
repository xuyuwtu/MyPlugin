namespace System.Collections.Generic;

public static class ListExt
{
    public static void AddRange<T>(this List<T> list, params T[] array) => list.AddRange(array);
    public static void RemoveRange<T>(this List<T> list, IEnumerable<T> collection)
    {
        foreach (T i in collection) { list.Remove(i); }
    }
    public static T ElementAtOrDefault<T>(this IList<T> list, int index, T defaultValue) => list.Count > index ? list[index] : defaultValue;
    public static void RemoveAt<T>(this IList<T> list, IList<int> collection)
    {
        for (int index = 0; index < collection.Count; index++)
        {
            list.RemoveAt(collection[index] - index);
        }
    }
}
