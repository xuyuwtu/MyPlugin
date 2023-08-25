using System.Reflection;
using System.Reflection.Emit;

namespace VBY.Common.Emit;

public class EmitHelper
{
    public static void Ldc_I4(ILGenerator il, int i4)
    {
        switch (i4)
        {
            case 0:
                il.Emit(OpCodes.Ldc_I4_0);
                break;
            case 1:
                il.Emit(OpCodes.Ldc_I4_1);
                break;
            case 2:
                il.Emit(OpCodes.Ldc_I4_2);
                break;
            case 3:
                il.Emit(OpCodes.Ldc_I4_3);
                break;
            case 4:
                il.Emit(OpCodes.Ldc_I4_4);
                break;
            case 5:
                il.Emit(OpCodes.Ldc_I4_5);
                break;
            case 6:
                il.Emit(OpCodes.Ldc_I4_6);
                break;
            case 7:
                il.Emit(OpCodes.Ldc_I4_7);
                break;
            case 8:
                il.Emit(OpCodes.Ldc_I4_8);
                break;
            default:
                if (byte.TryParse(i4.ToString(), out var result))
                    il.Emit(OpCodes.Ldc_I4_S, result);
                else
                    il.Emit(OpCodes.Ldc_I4, i4);
                break;
        }
    }
}
public static class Extensions
{
    public static void EmitNewarr(this ILGenerator il, Type type, int count)
    {
        EmitHelper.Ldc_I4(il, count);
        il.Emit(OpCodes.Newarr, type);
    }
}
public class ILGeneratorClass
{
    public ILGenerator il;
    private Type curtype;
    public ILGeneratorClass(ILGenerator il)
    {
        this.il = il;
        curtype = typeof(void);
    }
    public void Br(Label label) => il.Emit(OpCodes.Br, label);
    public void Brtrue(Label label) => il.Emit(OpCodes.Brtrue, label);
    public void Call(MethodInfo meth) => il.Emit(OpCodes.Call, meth);
    public void Callvirt(MethodInfo meth) => il.Emit(OpCodes.Callvirt, meth);
    public void Castclass(Type cls) => il.Emit(OpCodes.Castclass, cls);
    public void Dup() => il.Emit(OpCodes.Dup);
    public ILGeneratorClass Ld(string name)
    {
        if (name.Contains('.'))
        {
            var names = name.Split('.');
            foreach(var n in names)
                Ld(n);
        }
        else
        {
            var member = curtype.GetMember(name).First();
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    var field = (FieldInfo)member;
                    if (field.IsStatic)
                        Ldsfld(field);
                    else
                        Ldfld(field);
                    curtype = field.FieldType;
                    break;
                case MemberTypes.Property:
                    var property = (PropertyInfo)member;
                    Callvirt(property.GetGetMethod()!);
                    curtype = property.PropertyType;
                    break;
            }
        }
        return this;
    }
    public void Ldarg(byte arg, ParameterInfo[]? parameters = null)
    {
        switch (arg)
        {
            case 0:
                il.Emit(OpCodes.Ldarg_0);
                break;
            case 1:
                il.Emit(OpCodes.Ldarg_1);
                break;
            case 2:
                il.Emit(OpCodes.Ldarg_2);
                break;
            case 3:
                il.Emit(OpCodes.Ldarg_3);
                break;
            default:
                il.Emit(OpCodes.Ldarg_S, arg);
                break;
        }
        if(parameters is not null)
            curtype = parameters[arg].ParameterType;
    }
    public void Ldc_I4(int arg)
    {
        switch (arg)
        {
            case 0:
                il.Emit(OpCodes.Ldc_I4_0);
                break;
            case 1:
                il.Emit(OpCodes.Ldc_I4_1);
                break;
            case 2:
                il.Emit(OpCodes.Ldc_I4_2);
                break;
            case 3:
                il.Emit(OpCodes.Ldc_I4_3);
                break;
            case 4:
                il.Emit(OpCodes.Ldc_I4_4);
                break;
            case 5:
                il.Emit(OpCodes.Ldc_I4_5);
                break;
            case 6:
                il.Emit(OpCodes.Ldc_I4_6);
                break;
            case 7:
                il.Emit(OpCodes.Ldc_I4_7);
                break;
            case 8:
                il.Emit(OpCodes.Ldc_I4_8);
                break;
            default:
                if (byte.TryParse(arg.ToString(), out var result))
                    il.Emit(OpCodes.Ldc_I4_S, result);
                else
                    il.Emit(OpCodes.Ldc_I4, arg);
                break;
        }
    }
    public void Ldfld(FieldInfo filed) => il.Emit(OpCodes.Ldfld, filed);
    public void Ldfld(string name)
    {
        var field = curtype.GetField(name);
        il.Emit(OpCodes.Ldfld, field!);
        curtype = field!.FieldType;
    }
    public void Ldloc(short arg) => il.Emit(OpCodes.Ldloc, arg);
    public void Ldloc(LocalBuilder local) => il.Emit(OpCodes.Ldloc, local);
    public void Ldloca(short arg) => il.Emit(OpCodes.Ldloca, arg);
    public void Ldsfld(FieldInfo field) => il.Emit(OpCodes.Ldsfld, field);
    public void Ldstr(string str) => il.Emit(OpCodes.Ldstr, str);
    public void Newarr(Type cls) => il.Emit(OpCodes.Newarr, cls);
    public void Pop() => il.Emit(OpCodes.Pop);
    public void Ret() => il.Emit(OpCodes.Ret);
    public void Stelem_Ref() => il.Emit(OpCodes.Stelem_Ref);
    public void Stloc(LocalBuilder local) => il.Emit(OpCodes.Stloc, local);
    public void Stsfld(FieldInfo filed) => il.Emit(OpCodes.Stsfld, filed);
    public static implicit operator ILGenerator(ILGeneratorClass il) => il.il;
    public static implicit operator ILGeneratorClass(ILGenerator il) => new(il);
}
