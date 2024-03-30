using System.Text.RegularExpressions;
using System.Reflection;
using Terraria;

namespace VBY.SpawnRandomDrop;

internal static class Utils
{
    public static Dictionary<string, int> ItemNameToID = typeof(Terraria.ID.ItemID).GetFields(BindingFlags.Static | BindingFlags.Public).Where(x => x.IsLiteral).ToDictionary(x => x.Name, x => (int)(short)x.GetValue(null)!);
    public static void AddTrueIndexExpressionToList(bool[] bools, List<object> list)
    {
        list.Clear();
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
                AddExpression();
            }
            lastValue = bools[i];
        }
        if (lastValue)
        {
            AddExpression();
        }

        void AddExpression()
        {
            if (start + 1 == i)
            {
                list.Add(start);
            }
            else
            {
                int count = i - start;
                if (start.ToString()[^1] == '1' && count < 10)
                {
                    list.Add($"{start}+{count}");
                }
                else if (count < 4)
                {
                    list.Add($"{start}+{count}");
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
    public static List<int> GetIndexes(IEnumerable<object> values, Dictionary<string, object[]> idGroup)
    {
        var list = new List<int>();
        static void AddRange(List<int> list, int number1, int number2)
        {
            int start = Math.Min(number1, number2);
            int end = Math.Max(number1, number2);
            list.AddRange(Enumerable.Range(start, end - start + 1));
        }
        foreach (var value in values)
        {
            if (value is string str)
            {
                if (Regex.IsMatch(str, @"^\d{1,4}$"))
                {
                    list.Add(int.Parse(str));
                }
                else if (Regex.IsMatch(str, @"^\d{1,4}-\d{1,4}$"))
                {
                    AddRange(list, int.Parse(str.AsSpan(0, str.IndexOf('-'))), int.Parse(str.AsSpan(str.IndexOf('-') + 1)));
                }
                else if (Regex.IsMatch(str, @"^\d{1,4}\+\d{1,2}$"))
                {
                    AddRange(list, int.Parse(str.AsSpan(0, str.IndexOf('+'))), int.Parse(str.AsSpan(str.IndexOf('+'))));
                }
                else if (Regex.IsMatch(str, @"^\d{1,3}\[\d{1,3}-\d{1,3}\]$"))
                {
                    var groups = Regex.Match(str, @"^(\d{1,3})\[(\d{1,3})-(\d{1,3})\]$").Groups;
                    AddRange(list, int.Parse(groups[1].Value + groups[2].Value), int.Parse(groups[1].Value + groups[3].Value));
                }
                else if (str[0] == '&')
                {
                    list.Add(ItemNameToID[str.Substring(1)]);
                }
                else if (idGroup is not null && idGroup.TryGetValue(str, out var groupValues))
                {
                    list.AddRange(GetIndexes(groupValues, idGroup));
                }
            }
            else if (value is long)
            {
                list.Add((int)(long)value);
            }
            else if (value is int)
            {
                list.Add((int)value);
            }
            else if (value is short)
            {
                list.Add((short)value);
            }
        }
        return list;
    }
    public static HashSet<int> GetNotLockedChestItemIDs()
    {
        var hashSet = new HashSet<int>();
        foreach (var chest in Main.chest)
        {
            if (chest is not null)
            {
                if (!Chest.IsLocked(chest.x, chest.y))
                {
                    foreach (var item in chest.item)
                    {
                        if (!item.IsAir)
                        {
                            hashSet.Add(item.type);
                        }
                    }
                }
            }
        }
        return hashSet;
    }
    public static int ParseSingleNumber(string str)
    {
        if (Regex.IsMatch(str, @"^\d{1,4}$"))
        {
            return int.Parse(str);
        }
        else
        {
            return ItemNameToID[str];
        }
    }
    public static T SelectRandom<T>(this IList<T> values, Terraria.Utilities.UnifiedRandom random) => values[random.Next(values.Count)];
    public static void SetBools(bool[] bools, IEnumerable<object> values, Dictionary<string, object[]> idGroup)
    {
        Array.Fill(bools, false);
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
                else if (str[0] == '&')
                {
                    bools[ItemNameToID[str.Substring(1)]] = true;
                }
                else if (idGroup is not null && idGroup.TryGetValue(str, out var groupValues))
                {
                    SetBools(bools, groupValues, idGroup);
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
