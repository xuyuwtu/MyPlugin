using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using Terraria;
using Terraria.ID;
using TShockAPI;

namespace VBY.OtherCommand;

internal static partial class Utils
{
    public static List<TSPlayer> FindByNameOrID(string search)
    {
        var result = new List<TSPlayer>();
        search = search.Trim();
        var isTsi = search.StartsWith("tsi:");
        var isTsn = search.StartsWith("tsn:");
        if (isTsn || isTsi)
        {
            search = search.Remove(0, 4);
        }
        if (string.IsNullOrEmpty(search))
        {
            return result;
        }
        if (byte.TryParse(search, out var index) && index < byte.MaxValue)
        {
            var tsPlayer = TShock.Players[index];
            if (tsPlayer is { Active: true} or { ConnectionAlive: true })
            {
                if (isTsi)
                {
                    result.Add(tsPlayer);
                    return result;
                }
                result.Add(tsPlayer);
            }
        }
        var players = TShock.Players;
        if(isTsn)
        {
            var query = players.Where(x => x?.Name == search);
            if (query.Any())
            {
                if (result.Any())
                {
                    return new() { query.First() };
                }
                result.Add(query.First());
            }
        }
        else
        {
            var name = search.ToLower();
            foreach (var tsPlayer in players)
            {
                if (tsPlayer is not null)
                {
                    if (tsPlayer.Name.ToLower().StartsWith(name))
                    {
                        result.Add(tsPlayer);
                    }
                }
            }
        }
        return result;
    }
    public static int GetActiveConnectionCount() => Netplay.Clients.Where(x => x.IsActive).Count();
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
    public static IEnumerable<TResult> Select<TSource, TResult>(this Span<TSource> source, Func<TSource, TResult> selector)
    {
        var array = new TResult[source.Length];
        for(int i = 0; i < source.Length; i++)
        {
            array[i] = selector(source[i]);
        }
        return array;
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
        Console.Write(':');
        Console.WriteLine(obj);
    }
    public static T GetNext<T>(this ref List<T>.Enumerator enumerator)
    {
        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException("MoveNext() return false");
        }
        return enumerator.Current;
    }
    internal static Dictionary<string, Func<Expression, Expression, BinaryExpression>> OperatorFuncs = new()
    {
        { "=", Expression.Equal },
        { "!=", Expression.NotEqual },
        { "<", Expression.LessThan },
        { "<=", Expression.LessThanOrEqual },
        { ">", Expression.GreaterThan },
        { ">=", Expression.GreaterThanOrEqual }
    };
    internal static List<string> OperatorStrs = OperatorFuncs.Keys.ToList();
    internal static Dictionary<string, Func<Expression, Expression, BinaryExpression>> CombineFuncs = new()
    {
        { "and", Expression.AndAlso },
        { "or", Expression.OrElse }
    };
    internal static List<string> CombineStrs = CombineFuncs.Keys.ToList();
    public static bool GetMemeber(Type type, string memberName, [MaybeNullWhen(false)] out MemberInfo member, [MaybeNullWhen(true)] out string message)
    {
        member = type.GetMember(memberName, MemberTypes.Field | MemberTypes.Property, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance).FirstOrDefault();
        if (member is null)
        {
            message = $"{memberName} 未找到";
            return false;
        }
        var memberType = member.MemberType == MemberTypes.Field ? ((FieldInfo)member).FieldType : ((PropertyInfo)member).PropertyType;
        if (memberType != typeof(int))
        {
            message = $"{memberName} 的类型非int32,不受支持";
            return false;
        }
        message = null;
        return true;
    }
    public static Expression GetExpression(this MemberInfo memberInfo, ParameterExpression parameterExpression)
    {
        switch (memberInfo.MemberType)
        {
            case MemberTypes.Field:
                return Expression.Field(parameterExpression, (FieldInfo)memberInfo);
            case MemberTypes.Property:
                return Expression.Property(parameterExpression, (PropertyInfo)memberInfo);
            default:
                throw new InvalidOperationException("Invalid MemberTypes is Field and Property");
        }
    }
    public static bool GetExpression(string memberName, string operatorStr, string expression2, ParameterExpression parameterExpression, [MaybeNullWhen(false)] out Expression result, [MaybeNullWhen(true)] out string message)
    {
        var type = typeof(Item);
        if (!GetMemeber(type, memberName, out var member, out message))
        {
            result = null;
            return false;
        }
        if (!OperatorStrs.Contains(operatorStr))
        {
            message = $"'{operatorStr}' 无效操作符";
            result = null;
            return false;
        }
        var success = Regex.IsMatch(expression2, @"-{0,1}\d{1,4}");
        var isConstant = true;
        if (!success)
        {
            isConstant = false;
        }
        MemberInfo? member2 = null;
        if (!success && !GetMemeber(type, expression2, out member2, out message))
        {
            result = null;
            return false;
        }
        result = OperatorFuncs[operatorStr](member.GetExpression(parameterExpression), isConstant ? Expression.Constant(int.Parse(expression2), typeof(int)) : member2!.GetExpression(parameterExpression));
        return true;
    }
    public static Expression<Func<Item, bool>> CreateLambdaExpression(Expression body, params ParameterExpression[] parameters) => Expression.Lambda<Func<Item, bool>>(body, parameters);
    public static bool GetLambdaExpression(TSPlayer player, List<string> parameters, ref List<string>.Enumerator enumerator, [MaybeNullWhen(false)] out Expression<Func<Item, bool>> lambdaExpression)
    {
        var parameterExpression = Expression.Parameter(typeof(Item));
        lambdaExpression = null;
        if (!GetExpression(enumerator.GetNext(), enumerator.GetNext(), enumerator.GetNext(), parameterExpression, out var expression, out var message))
        {
            player.SendErrorMessage(message);
            return false;
        }
        if (parameters.Count >= 8)
        {
            var combineStr = enumerator.GetNext();
            if (!CombineStrs.Contains(combineStr))
            {
                player.SendErrorMessage($"'{combineStr}' 无效连接符");
                return false;
            }
            if (!GetExpression(enumerator.GetNext(), enumerator.GetNext(), enumerator.GetNext(), parameterExpression, out var expression2, out var message2))
            {
                player.SendErrorMessage(message2);
                return false;
            }
            lambdaExpression = CreateLambdaExpression(CombineFuncs[combineStr](expression, expression2), parameterExpression);
        }
        lambdaExpression ??= CreateLambdaExpression(expression, parameterExpression);
        return true;
    }
}
