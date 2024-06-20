using System.Text.RegularExpressions;

using NuGet.Packaging;

namespace VBY.Common.Config;

public static class ConfigUtlis
{
    private static void AddRange(ICollection<int> collection, (int start, int count) range) => collection.AddRange(Enumerable.Range(range.start, range.count));
    private static void AddCloseRange(ICollection<int> collection, (int start, int end) range) => collection.AddRange(Enumerable.Range(range.start, range.end - range.start + 1));
    private static (int num1, int num2) SimpleSplitCast(this string str, char separator)
    {
        var index = str.IndexOf(separator, 1);
        var span = str.AsSpan();
        return (int.Parse(span.Slice(0, index)), int.Parse(span.Slice(index + 1)));
    }
    private static (int num1, int num2) PositiveSort(int num1, int num2) => num1 < num2 ? (num1, num2) : (num2, num1);
    private static (int num1, int num2) PositiveSort(this (int num1, int num2) nums) => PositiveSort(nums.num1, nums.num2);
    public static void GetIntsAddToCollection(ICollection<int> collection, IEnumerable<object> values, bool clear = false)
    {
        if(clear)
        {
            collection.Clear();
        }
        foreach (var value in values)
        {
            switch(value)
            {
                case string str:
                    if (Regex.IsMatch(str, @"^-?\d{1,4}$"))
                    {
                        collection.Add(int.Parse(str));
                    }
                    else if (Regex.IsMatch(str, @"^-?\d{1,4}--?\d{1,4}$"))
                    {
                        AddCloseRange(collection, str.SimpleSplitCast('-').PositiveSort());
                    }
                    else if (Regex.IsMatch(str, @"^-?\d{1,4}\+\d{1,2}$"))
                    {
                        AddRange(collection, str.SimpleSplitCast('+'));
                    }
                    else if (Regex.IsMatch(str, @"^-?\d{1,3}\[\d{1,3}-\d{1,3}\]$"))
                    {
                        var groups = Regex.Match(str, @"^(-?\d{1,3})\[(\d{1,3})-(\d{1,3})\]$").Groups;
                        AddCloseRange(collection, (int.Parse(groups[1].Value + groups[2].Value), int.Parse(groups[1].Value + groups[3].Value)).PositiveSort());
                    }
                    break;
                case long:
                    collection.Add((int)(long)value);
                    break;
                case int:
                    collection.Add((int)value);
                    break;
                case short:
                    collection.Add((short)value);
                    break;
            }
        }
    }
    public static List<int> GetIntsAsList(IEnumerable<object> values)
    {
        var list = new List<int>();
        GetIntsAddToCollection(list, values);
        return list;
    }
    public static HashSet<int> GetIntsAsHashSet(IEnumerable<object> values)
    {
        var hashSet = new HashSet<int>();
        GetIntsAddToCollection(hashSet, values);
        return hashSet;
    }
}
