using System.Reflection;

using Terraria;

using Newtonsoft.Json.Linq;

namespace VBY.GameContentModify.Config;

public class MemberSetterInfo
{
    public Dictionary<string, JToken> MemberName = new();
    public Dictionary<string, JToken> MemberValue = new();

    public static Dictionary<string, MemberInfo> MemberNames = new();
    private static Stack<IRestoreable> Restoreables = new();
    public void Resolve()
    {
        MemberNames.Clear();
        RestoreValue();
        foreach (var (customName, jtoken) in MemberName)
        {
            string name;
            if(jtoken is JValue { Type: JTokenType.String } jValue)
            {
                name = (string)jValue.Value!;
            }
            else if(jtoken is JObject jObject)
            {
                try
                {
                    name = (string)Convert.ChangeType(((JValue)jObject["Name"]!).Value, typeof(string))!;
                }
                catch
                {
                    Console.WriteLine($"错误对象值: {customName}");
                    continue;
                }
            }
            else
            {
                Console.WriteLine($"无效名称对象: {customName}");
                continue;
            }
            var index = name.LastIndexOf('.');
            if (index == -1)
            {
                Console.WriteLine($"无效成员: {name}");
                continue;
            }
            var memberType = name.Substring(0, index);
            var type = typeof(Item).Assembly.GetType(name.Substring(0, index));
            if (type is null)
            {
                Console.WriteLine($"无效类: {memberType}");
                continue;
            }
            var memberName = name.Substring(index + 1);
            var members = type.GetMember(memberName);
            if (members is null || members.Length == 0)
            {
                Console.WriteLine($"没找到成员: {memberName}");
                continue;
            }
            var validMembers = members.Where(x => x.MemberType is MemberTypes.Field or MemberTypes.Property).ToArray();
            if (validMembers.Length > 1)
            {
                Console.WriteLine($"找到多个成员: {memberName}");
                continue;
            }
            var member = members.First();
            var supportTypes = new Type[] { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(string), typeof(bool) };
            if (member.MemberType == MemberTypes.Property)
            {
                var property = (PropertyInfo)member;
                if (property.SetMethod is null)
                {
                    Console.WriteLine($"不可设置的属性: {memberName}");
                    continue;
                }
                if(property.GetMethod is null)
                {
                    Console.WriteLine($"不可获取的属性: {memberName}");
                    continue;
                }
                if (!property.SetMethod.IsStatic)
                {
                    Console.WriteLine($"非静态成员: {memberName}");
                    continue;
                }
                if (property.PropertyType.IsArray)
                {
                    if (!supportTypes.Contains(property.PropertyType.GetElementType()))
                    {
                        Console.WriteLine($"不受支持的数组元素类型: {property.PropertyType.GetElementType()}");
                        continue;
                    }
                }
                else if (!supportTypes.Contains(property.PropertyType))
                {
                    Console.WriteLine($"不受支持的元素类型: {property.PropertyType}");
                    continue;
                }
                MemberNames.Add(customName, property);
            }
            else
            {
                var field = (FieldInfo)member;
                if (!field.IsStatic)
                {
                    Console.WriteLine($"非静态成员: {memberName}");
                    continue;
                }
                if (field.FieldType.IsArray)
                {
                    if (!supportTypes.Contains(field.FieldType.GetElementType()))
                    {
                        Console.WriteLine($"不受支持的数组元素类型: {field.FieldType.GetElementType()}");
                        continue;
                    }
                }
                else if (!supportTypes.Contains(field.FieldType))
                {
                    Console.WriteLine($"不受支持的元素类型: {field.FieldType}");
                    continue;
                }
                MemberNames.Add(customName, field);
            }
        }
    }
    public void SetValue()
    {
        foreach (var (customName, value) in MemberValue)
        {
            if (!MemberNames.TryGetValue(customName, out var member))
            {
                Console.WriteLine($"未知命名成员: {customName}");
                continue;
            }
            var type = Utils.GetFieldOrPropertyType(member);
            object? changeValue = null;
            if (type.IsArray)
            {
                if (!value.HasValues || value is not JObject)
                {
                    Console.WriteLine($"无效属性(对于数组成员): {value}");
                }
                var elementType = type.GetElementType() ?? throw new Exception("ElementType还能是null，这么抽象");
                var jObject = (JObject)value;
                foreach (var jMember in jObject)
                {
                    if (!int.TryParse(jMember.Key, out var index))
                    {
                        Console.WriteLine($"无效索引: {jMember.Key}");
                        continue;
                    }
                    if (jMember.Value is JValue jValue)
                    {
                        changeValue = jValue.Value;
                    }
                    else if (jMember.Value is JObject jmObject)
                    {
                        changeValue = ((JValue)jmObject["Value"]!).Value;
                    }
                    if (changeValue is null)
                    {
                        Console.WriteLine("值不可为空");
                        continue;
                    }
                    try
                    {
                        var array = (Array)Utils.GetFieldOrPropertyValue(member, null)!;
                        var originalValue = array.GetValue(index);
                        Restoreables.Push(new ArrayValueRestorer(member, index, originalValue));
                        array.SetValue(Convert.ChangeType(changeValue, elementType), index);
                        Console.WriteLine($"{customName}[{index}]: {originalValue} => {changeValue}");
                    }
                    catch (InvalidCastException)
                    {
                        Console.WriteLine($"无效转换: '{changeValue}' 到 {elementType.FullName}");
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine($"错误格式: '{changeValue}'");
                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine($"值溢出: '{changeValue}' 对于 {elementType.FullName}");
                    }
                }
            }
            else
            {
                if (value is JValue jValue)
                {
                    changeValue = jValue.Value;
                }
                else if (value is JObject jObject)
                {
                    changeValue = ((JValue)jObject["Value"]!).Value;
                }
                if (changeValue is null)
                {
                    Console.WriteLine("值不可为空");
                    continue;
                }
                try
                {
                    var originalValue = Utils.GetFieldOrPropertyValue(member, null);
                    Restoreables.Push(new ValueRestorer(member, originalValue));
                    Utils.SetFieldOrPropertyValue(member, null, Convert.ChangeType(changeValue, type));
                    Console.WriteLine($"{customName}: {originalValue} => {changeValue}");
                }
                catch (InvalidCastException)
                {
                    Console.WriteLine($"无效转换: '{changeValue}' 到 {type.FullName}");
                }
                catch (FormatException)
                {
                    Console.WriteLine($"错误格式: '{changeValue}'");
                }
                catch (OverflowException)
                {
                    Console.WriteLine($"值溢出: '{changeValue}' 对于 {type.FullName}");
                }
            }
        }
    }
    public static void RestoreValue()
    {
        while (Restoreables.Any())
        {
            Restoreables.Pop().Restore();
        }
    }
}
internal sealed class ValueRestorer : IRestoreable
{
    public MemberInfo MemberInfo;
    public object? Value;

    public ValueRestorer(MemberInfo memberInfo, object? value)
    {
        MemberInfo = memberInfo;
        Value = value;
    }
    public void Restore() => Utils.SetFieldOrPropertyValue(MemberInfo, null, Value);
}
internal sealed class ArrayValueRestorer : IRestoreable
{
    public MemberInfo MemberInfo;
    public int Index;
    public object? Value;

    public ArrayValueRestorer(MemberInfo memberInfo, int index, object? value)
    {
        MemberInfo = memberInfo;
        Index = index;
        Value = value;
    }
    public void Restore() => ((Array)Utils.GetFieldOrPropertyValue(MemberInfo, null)!).SetValue(Value, Index);
}