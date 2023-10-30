using System.Text.RegularExpressions;

namespace VBY.SpawnRandomDrop;

internal static class Utils
{
    public static void AddTrueIndexExpressionToList(bool[] bools, List<object> list)
    {
        int start = -1;
        bool lastValue = false;
        int i = 0;
        for (; i < bools.Length; i++)
        {
            if (bools[i])
            {
                if (!lastValue)
                {
                    start = i;
                }
            }
            else if (lastValue)
            {
                if (start + 1 == i)
                {
                    list.Add(start);
                }
                else
                {
                    if (i - start < 4)
                    {
                        list.Add($"{start}+{i - start}");
                    }
                    else
                    {
                        list.Add($"{start}-{i - 1}");
                    }
                }
            }
            lastValue = bools[i];
        }
        if (lastValue)
        {
            if (start + 1 == i)
            {
                list.Add(i);
            }
            else
            {
                if (i - start < 4)
                {
                    list.Add($"{start}+{i - start}");
                }
                else
                {
                    list.Add($"{start}-{i - 1}");
                }
            }
        }
    }
    public static void AddTrueIndexToList(bool[] bools, List<int> list)
    {
        for (int i = 0; i < bools.Length; i++)
        {
            if (bools[i])
            {
                list.Add(i);
            }
        }
    }
    public static T SelectRandom<T>(this IList<T> values, Terraria.Utilities.UnifiedRandom random) => values[random.Next(values.Count)];
    public static void SetBools(bool[] bools, IEnumerable<object> values)
    {
        static void FillRange(bool[] bools, int number1, int number2)
        {
            int start = Math.Min(number1, number2);
            int end = Math.Max(number1, number2);
            Array.Fill(bools, true, start, end - start + 1);
        }
        foreach (var value in values)
        {
            if (value is string str)
            {
                if (Regex.IsMatch(str, @"^\d{1,4}$"))
                {
                    bools[int.Parse(str)] = true;
                }
                else if (Regex.IsMatch(str, @"^\d{1,4}-\d{1,4}$"))
                {
                    FillRange(bools, int.Parse(str.AsSpan(0, str.IndexOf('-'))), int.Parse(str.AsSpan(str.IndexOf('-') + 1)));
                }
                else if (Regex.IsMatch(str, @"^\d{1,4}\+\d{1,2}$"))
                {
                    Array.Fill(bools, true, int.Parse(str.AsSpan(0, str.IndexOf('+'))), int.Parse(str.AsSpan(str.IndexOf('+'))));
                }
                else if (Regex.IsMatch(str, @"^\d{1,3}\[\d{1,3}-\d{1,3}\]$"))
                {
                    var groups = Regex.Match(str, @"^(\d{1,3})\[(\d{1,3})-(\d{1,3})\]$").Groups;
                    FillRange(bools, int.Parse(groups[1].Value + groups[2].Value), int.Parse(groups[1].Value + groups[3].Value));
                }
            }
            else if (value is long)
            {
                bools[(int)(long)value] = true;
            }
            else if(value is int)
            {
                bools[(int)value] = true;
            }
            else if(value is short)
            {
                bools[(short)value] = true;
            }
        }
    }
}
