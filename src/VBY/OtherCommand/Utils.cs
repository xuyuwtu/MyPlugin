using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using TShockAPI;

namespace VBY.OtherCommand;

internal static class Utils
{
    public static void Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Action<TSource> action)
    {
        foreach (var item in source)
        {
            if(predicate(item))
            {
                action(item);
            }
        }
    }
    public static void SetActive(this NPC npc, bool active)
    {
        npc.active = active;
        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI);
    }
    public static string Join(this IEnumerable<string> values, char separator) => string.Join(separator, values);
    public static bool GetNext<T>(this IList<T> list, ref int index, [MaybeNullWhen(false)] out T result)
    {
        index++;
        if(index >= list.Count)
        {
            result = default;
            return false;
        }
        else
        {
            result = list[index];
            return true;
        }
    }
    public static void Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Action<TSource> action)
    {
        foreach(var item in source)
        {
            if(predicate(item))
            {
                action(item);
            }
        }
    }
    public static string SubstringAfter(this string str, char afterChar, int count = 1)
    {
        if(count < 1)
        {
            throw new ArgumentException("count can't less than 1");
        }
        int index = -1;
        while(count != 0)
        {
            index = str.IndexOf(afterChar, index + 1);
            if(index == -1)
            {
                throw new ArgumentException($"can't find enought char '{afterChar}' in str");
            }
            count--;
        }
        return str.Substring(index + 1);
    }
    public static int IndexOfSkipRecpat(this string findString, char findChar, int count)
    {
        var findIndex = -1;
        for (int i = 0; i < findString.Length; i++)
        {
            if (findString[i] == findChar)
            {
                if (i - 1 != findIndex)
                {
                    count--;
                }
                findIndex = i;
                if (count == 0)
                {
                    break;
                }
            }
        }
        return findIndex;
    }
    public static string RemoveRecpat(this string str, char removeChar)
    {
        var index = str.IndexOf(removeChar);
        if (index == -1 || index == str.Length - 1)
        {
            return str;
        }
        index++;
        var chars = str.ToCharArray();
        int newIndex = index;
        bool lastIsRemoveChar = true;
        for (int i = index; i < str.Length; i++)
        {
            if (str[i] == removeChar)
            {
                if (!lastIsRemoveChar)
                {
                    chars[newIndex++] = str[i];
                    lastIsRemoveChar = true;
                }
            }
            else
            {
                chars[newIndex++] = str[i];
                lastIsRemoveChar = false;
            }
        }
        return new string(chars, 0, newIndex);
    }
    public static void ExpressionWriteLine(object obj, [CallerArgumentExpression(nameof(obj))] string expression = "")
    {
        Console.Write(expression);
        Console.Write(' ');
        Console.WriteLine(obj);
    }
    public static bool GetMemeber(Type type, string memberName, TSPlayer player, [MaybeNullWhen(false)] out MemberInfo member)
    {
        member = type.GetMember(memberName, MemberTypes.Field | MemberTypes.Property, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance).FirstOrDefault();
        if (member is null)
        {
            player.SendInfoMessage($"{memberName} 未找到");
            return false;
        }
        var memberType = member.MemberType == MemberTypes.Field ? ((FieldInfo)member).FieldType : ((PropertyInfo)member).PropertyType;
        if (memberType != typeof(int))
        {
            player.SendErrorMessage($"{memberName} 的类型非int32,不受支持");
            return false;
        }
        return true;
    }
}
