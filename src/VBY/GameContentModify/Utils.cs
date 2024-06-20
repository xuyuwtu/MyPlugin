using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

using TShockAPI;

using MonoMod.RuntimeDetour;
using System.ComponentModel;
using System.Diagnostics;

namespace VBY.GameContentModify;

internal static class Utils
{
    public static bool NextIsZero(this Terraria.Utilities.UnifiedRandom random, int maxValue)
    {
        if(maxValue < 1)
        {
            return false;
        }
        return random.Next(maxValue) == 0;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTrueRet(this bool value, bool retValue) => !value || retValue;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFalseRet(this bool value, bool retValue) => !value && retValue;
    public static T SelectRandom<T>(Terraria.Utilities.UnifiedRandom random, params T[] choices)
    {
        return choices[random.Next(choices.Length)];
    }
    public static void SetEmpty(this Terraria.Chest chest)
    {
        for (int i = 0; i < 40; i++)
        {
            chest.item[i] = new Terraria.Item();
        }
    }
    public static Detour GetNameDetour(Type methodType, Delegate method, bool manualApply = true) => new Detour(methodType.GetMethod(method.Method.Name), method.Method, new DetourConfig() { ManualApply = manualApply });
    public static Detour GetDetour(Delegate method, bool manualApply = true)
    {
        return GetNameDetour(method.Method.DeclaringType!.GetCustomAttribute<ReplaceTypeAttribute>()!.Type, method, manualApply);
    }
    public static Detour GetParamDetour(Delegate method, bool manualApply = true)
    {
        var methodType = method.Method.DeclaringType!.GetCustomAttribute<ReplaceTypeAttribute>()!.Type;
        return new Detour(methodType.GetMethod(method.Method.Name, method.Method.GetParameters().Select(x => x.ParameterType).ToArray()), method.Method, new DetourConfig() { ManualApply = manualApply });
    }
    public static void Deconstruct(this Terraria.Chest chest, out int x, out int y)
    {
        x = chest.x;
        y = chest.y;
    }
    public static bool SetMemberValue(TSPlayer player, object target, string propertyName, string realPropertyName, string value)
    {
        if (propertyName.Contains('.'))
        {
            var checkType = target.GetType();
            var propertyNames = propertyName.Split('.');
            for (int i = 0; i < propertyNames.Length - 1; i++)
            {
                if (!TryGetFieldOrPropertyMember(checkType, propertyNames[i], target, out checkType, out object? newTarget))
                {
                    player.SendInfoMessage($"没有找到属性 {string.Join('.', propertyNames, 0, i + 1)}");
                    return false;
                }
                target = newTarget;
            }
            return SetMemberValue(player, target, propertyNames[^1], propertyName, value);
        }
        var type = target.GetType();
        var members = type.GetMember(propertyName, MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public);
        if (members.Length == 0)
        {
            player.SendInfoMessage($"没找到属性 {propertyName}");
        }
        else
        {
            var member = members[0];
            Func<object?, object?> getFunc;
            Action<object?, object> setFunc;
            if (member.MemberType == MemberTypes.Field)
            {
                var field = (FieldInfo)member;
                getFunc = field.GetValue;
                setFunc = field.SetValue;
            }
            else
            {
                var property = (PropertyInfo)member;
                getFunc = property.GetValue;
                setFunc = property.SetValue;
            }
            var propertyValue = getFunc(target);
            switch (propertyValue)
            {
                case int:
                    if (int.TryParse(value, out var intValue))
                    {
                        setFunc(target, intValue);
                        player.SendInfoMessage($"{realPropertyName} = {intValue}");
                        return true;
                    }
                    player.SendInfoMessage($"{value} 转换为 int 失败");
                    break;
                case bool:
                    if (bool.TryParse(value, out var boolValue))
                    {
                        setFunc(target, boolValue);
                        player.SendInfoMessage($"{realPropertyName} = {boolValue}");
                        return true;
                    }
                    player.SendInfoMessage($"{value} 转换为 bool 失败");
                    break;
                case int[]:
                case string[]:
                    player.SendInfoMessage("数组不受支持");
                    break;
                default:
                    player.SendInfoMessage($"类型 {type.FullName} 不受支持");
                    break;
            }
        }
        return false;
    }
    public static bool TryGetFieldOrPropertyMember(Type type, string memberName, object target,[MaybeNullWhen(false)]out Type memberType, [MaybeNullWhen(false)] out object newTarget)
    {
        var members = type.GetMember(memberName, MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public);
        if (members.Length == 0)
        {
            memberType = null;
            newTarget = null;
            return false;
        }
        var findMember = members[0];
        if(findMember.MemberType == MemberTypes.Field)
        {
            var field = (FieldInfo)findMember;
            memberType = field.FieldType;
            newTarget = field.GetValue(target)!;
        }
        else
        {
            var property = (PropertyInfo)findMember;
            memberType = property.PropertyType;
            newTarget = property.GetValue(target)!;
        }
        return true;
    }
    public static Type GetFieldOrPropertyType(MemberInfo memberInfo)
    {
        if(memberInfo.MemberType == MemberTypes.Field)
        {
            return ((FieldInfo)memberInfo).FieldType;
        }
        return ((PropertyInfo)memberInfo).PropertyType;
    }
    public static object? GetFieldOrPropertyValue(MemberInfo memberInfo, object? target = null)
    {
        if (memberInfo.MemberType == MemberTypes.Field)
        {
            return ((FieldInfo)memberInfo).GetValue(target);
        }
        return ((PropertyInfo)memberInfo).GetValue(target);
    }
    public static void SetFieldOrPropertyValue(MemberInfo memberInfo, object? target, object? value)
    {
        if (memberInfo.MemberType == MemberTypes.Field)
        {
            ((FieldInfo)memberInfo).SetValue(target, value);
            return;
        }
        ((PropertyInfo)memberInfo).SetValue(target, value);
    }
    public static Type GetFieldOrPropertyType(MemberInfo memberInfo, out Func<object?, object?> getFunc, out Action<object?, object?> setFunc)
    {
        if(memberInfo.MemberType == MemberTypes.Field)
        {
            var field = (FieldInfo)memberInfo;
            getFunc = field.GetValue;
            setFunc = field.SetValue;
            return field.FieldType;
        }
        var property = ((PropertyInfo)memberInfo);
        getFunc = property.GetValue;
        setFunc = property.SetValue;
        return property.PropertyType;
    }
    public static void HandleNamedDetour(ref bool field, bool value, params string[] detourNames)
    {
        if (field != value)
        {
            if (value)
            {
                foreach (string detourName in detourNames)
                {
                    GameContentModify.NamedDetours[detourName].Apply();
                }
            }
            else
            {
                foreach (string detourName in detourNames)
                {
                    GameContentModify.NamedDetours[detourName].Undo();
                }
            }
            field = value;
        }
    }
    public static void HandleNamedDetour(bool value, params string[] detourNames)
    {
        foreach (var detourName in detourNames)
        {
            if (value)
            {
                GameContentModify.NamedDetours[detourName].Apply();
            }
            else
            {
                GameContentModify.NamedDetours[detourName].Undo();
            }
        }
    }
    public static void HandleNamedActionHook(bool value, string actionHookName)
    {
        if (value)
        {
            GameContentModify.NamedActionHooks[actionHookName].Register();
        }
        else
        {
            GameContentModify.NamedActionHooks[actionHookName].Unregister();
        }
    }
    public static bool NamedActionHookIsRegistered(string actionHookName) => GameContentModify.NamedActionHooks[actionHookName].Registered;
    public static T[] MakeArray<T>(this T item) where T : class => new T[1] { item };
    public static bool MembersValueAllEqualDefault(object target, params string[] names)
    {
        var type = target.GetType();
        foreach (string name in names)
        {
            var members = type.GetMember(name);
            if (!members.Any())
            {
                return false;
            }
            if(members.Length > 1)
            {
                return false;
            }
            var member = members[0];
            if(member.MemberType != MemberTypes.Field &&  member.MemberType != MemberTypes.Property)
            {
                return false;
            }
            var defaultAttr = member.GetCustomAttribute<DefaultValueAttribute>();
            if (defaultAttr is null)
            {
                return false;
            }
            if(!GetFieldOrPropertyValue(member, target)!.Equals(defaultAttr.Value))
            {
                return false;
            }
        }
        return true;
    }
    //public static bool MembersValueAllEqualDefault(object target, params object[] refMembers)
    //{
    //    var type = target.GetType();
    //    foreach (var refMember in refMembers)
    //    {
    //        MemberInfo? member;
    //        if(refMember is RuntimeFieldHandle)
    //        {
    //            member = FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)refMember);
    //        }
    //        else
    //        {
    //            member = type.GetProperty((string)refMember);
    //        }
    //        if(member is null)
    //        {
    //            return false;
    //        }
    //        var defaultAttr = member.GetCustomAttribute<DefaultValueAttribute>();
    //        if (defaultAttr is null)
    //        {
    //            return false;
    //        }
    //        if (!GetFieldOrPropertyValue(member, target)!.Equals(defaultAttr.Value))
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}
    [Conditional("DEBUG")]
    public static void ArgumentWriteLine(bool value, [CallerArgumentExpression(nameof(value))] string expression = "")
    {
        Console.Write(expression);
        Console.Write(": ");
        Console.WriteLine(value);
    }
    [Conditional("DEBUG")]
    public static void ArgumentWriteLine(int value, [CallerArgumentExpression(nameof(value))] string expression = "")
    {
        Console.Write(expression);
        Console.Write(": ");
        Console.WriteLine(value);
    }
}