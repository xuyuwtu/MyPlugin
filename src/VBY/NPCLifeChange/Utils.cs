using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

using Terraria;

namespace VBY.NpcLifeChange;

internal static class Utils
{
    public static void SetBools(bool[] bools, IEnumerable<object> values)
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
            }
            else if (value is long)
            {
                bools[(int)(long)value] = true;
            }
            else if (value is int)
            {
                bools[(int)value] = true;
            }
            else if (value is short)
            {
                bools[(short)value] = true;
            }
        }
    }
    public static void GetMemberValue(ILGenerator iL, MemberInfo memberInfo)
    {
        if(memberInfo.MemberType == MemberTypes.Field || memberInfo.MemberType == MemberTypes.Property)
        {
            if(memberInfo.MemberType == MemberTypes.Field)
            {
                iL.Emit(OpCodes.Ldfld, (FieldInfo)memberInfo);
            }
            else
            {
                iL.Emit(OpCodes.Callvirt, ((PropertyInfo)memberInfo).GetMethod!);
            }
            return;
        }
        throw new NotImplementedException($"not support type:{memberInfo.MemberType}");
    }
    internal static void SetNotChangeFunc(string[] properties)
    {
        if (properties is null || properties.Length == 0)
        {
            NpcLifeChange.NotChangeFunc = DefaultNotChange;
            return;
        }
        var type = typeof(NPC);
        var error = false;
        var getmembers = new List<MemberInfo>();
        foreach (var propertyName in properties)
        {
            var members = type.GetMember(propertyName, MemberTypes.Property | MemberTypes.Field, BindingFlags.Instance | BindingFlags.Public);
            if (members.Length == 0)
            {
                error = true;
            }
            else if (members.Length > 1)
            {
                error = true;
            }
            if (!error)
            {
                var member = members[0];
                if (member is FieldInfo field)
                {
                    if (field.FieldType != typeof(bool))
                    {
                        error = true;
                    }
                }
                else
                {
                    if (((PropertyInfo)member).PropertyType != typeof(bool))
                    {
                        error = true;
                    }
                }
            }
            if (!error)
            {
                getmembers.Add(members[0]);
            }
            if (error)
            {
                NpcLifeChange.NotChangeFunc = DefaultNotChange;
                return;
            }
        }
        var method = new DynamicMethod("", typeof(bool), new Type[] { typeof(NPC) });
        var il = method.GetILGenerator();
        var retLabel = il.DefineLabel();
        if (getmembers.Count == 1)
        {
            il.Emit(OpCodes.Ldarg_0);
            GetMemberValue(il, getmembers[0]);
        }
        else
        {
            var ldFalseLabel = il.DefineLabel();
            for (int i = 0; i < getmembers.Count - 1; i++)
            {
                il.Emit(OpCodes.Ldarg_0);
                GetMemberValue(il, getmembers[i]);
                il.Emit(OpCodes.Brfalse_S, ldFalseLabel);
            }
            il.Emit(OpCodes.Ldarg_0);
            GetMemberValue(il, getmembers[getmembers.Count-1]);
            il.Emit(OpCodes.Br_S, retLabel);
        }
        il.MarkLabel(retLabel);
        il.Emit(OpCodes.Ret);
        NpcLifeChange.NotChangeFunc = method.CreateDelegate<Func<NPC, bool>>();
    }
    internal static bool DefaultNotChange(NPC npc) => false;
}
