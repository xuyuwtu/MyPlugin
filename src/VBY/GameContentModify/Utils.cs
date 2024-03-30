using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

using TShockAPI;

using MonoMod.RuntimeDetour;

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
    public static Detour GetDetour(Type methodType, Delegate method, bool manualApply = true) => new Detour(methodType.GetMethod(method.Method.Name), method.Method, new DetourConfig() { ManualApply = manualApply });
    public static Detour GetDetour(Delegate method, bool manualApply = true)
    {
        return GetDetour(method.Method.DeclaringType!.GetCustomAttribute<ReplaceTypeAttribute>()!.Type, method, manualApply);
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
    public static Type GetMemberType(MemberInfo memberInfo, out Func<object?, object?> getFunc, out Action<object?, object?> setFunc)
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
}