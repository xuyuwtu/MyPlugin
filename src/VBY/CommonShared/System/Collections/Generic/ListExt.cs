namespace System.Collections.Generic;

public static class ListExt
{
    public static void AddRange<T>(this List<T> list, params T[] collection) => list.AddRange(collection);
    public static void RemoveCollection<T>(this List<T> list, IEnumerable<T> collection)
    {
        foreach (T i in collection)
        {
            list.Remove(i);
        }
    }
    public static T ElementAtOrDefault<T>(this IList<T> list, int index, T defaultValue) => list.Count > index ? list[index] : defaultValue;
    public static void RemoveIndexes<T>(this IList<T> list, IList<int> collection)
    {
        for (int count = 0; count < collection.Count; count++)
        {
            list.RemoveAt(collection[count] - count);
        }
    }
}
