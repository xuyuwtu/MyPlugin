using System.Reflection;
using System.Runtime.CompilerServices;

using VBY.Common;

namespace System;
public static class SystemExt
{
    public static string LastWord(this string str)
    {
        for (int i = str.Length - 1; i >= 0; i--)
        {
            if (char.IsUpper(str[i]))
                return str[i..];
        }
        return str;
    }
    public static string ToString(this bool args, string trueS, string falseS)
    {
        return args ? trueS : falseS;
    }
    public static string FirstUpper(this string str) => char.ToUpper(str[0]) + str[1..];
    public static string Join(this IEnumerable<string> values, string separator) => string.Join(separator, values);
    public static string Join(this IEnumerable<string> values, char separator) => string.Join(separator, values);
    public static string SubstringAfter(this string str, char afterChar) => str.Substring(str.IndexOf(afterChar) + 1);
    public static int IndexOf(this string str, int startIndex, Predicate<char> preficate)
    {
        for (int i = startIndex; i < str.Length; i++)
        {
            if (preficate(str[i]))
                return i;
        }
        return -1;
    }
    public static int IndexOf(this string str, Predicate<char> preficate) => IndexOf(str, 0, preficate);
    public static int IndexOf<T>(this T[] array, int startIndex, Predicate<T> preficate, int reduce = 0)
    {
        for (int i = startIndex; i < array.Length - reduce; i++)
        {
            if (preficate(array[i]))
                return i;
        }
        return -1;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T>(this T[] array, T value) => Array.IndexOf(array, value);
    public static Delegate GetDelegate(this MethodInfo method, object? target, bool toSubCmdD = true)
    {
        Type delegateType;
        ParameterInfo[] paramInfos = method.GetParameters();
        Type[] paramTypes = paramInfos.Select(x => x.ParameterType).ToArray();
        if (method.ReturnType is null || method.ReturnType == TypeOf.Void)
        {
            if (paramTypes.Length == 0)
            {
                delegateType = Type.GetType("System.Action")!;
            }
            else
            {
                if (toSubCmdD && paramTypes.Length == 1 && paramTypes[0] == TypeOf.SubCmdArgs)
                    delegateType = TypeOf.SubCmdD;
                else
                    delegateType = Type.GetType($"System.Action`{paramTypes.Length}")!.MakeGenericType(paramTypes);
            }
        }
        else
        {
            var list = paramTypes.ToList();
            list.Insert(0, method.ReturnType);
            delegateType = Type.GetType($"System.Func`{paramTypes.Length + 1}")!.MakeGenericType(list.ToArray());
        }
        return Delegate.CreateDelegate(delegateType, target, method);
    }
}