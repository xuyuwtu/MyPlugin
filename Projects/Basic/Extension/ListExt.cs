namespace VBY.Basic.Extension;

public static class ListExt
{
    public static void AddRange<T>(this List<T> list,params T[] array) => list.AddRange(array);
    public static void RemoveRange<T>(this List<T> list, IEnumerable<T> collection)
    {
        foreach (T i in collection) { list.Remove(i); }
    }
    public static T GetIndexOrValue<T>(this List<T> list, int index, T value) => list.Count > index ? list[index] : value;
}
