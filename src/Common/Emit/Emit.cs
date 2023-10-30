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
                if (i4 >= sbyte.MinValue && i4 <= sbyte.MaxValue)
                    il.Emit(OpCodes.Ldc_I4_S, (sbyte)i4);
                else
                    il.Emit(OpCodes.Ldc_I4, i4);
                break;
        }
    }
}
public static class Extensions
{
    public static DynamicMethod CreateInstanceMethod(this Type type,string name,Type? returnType = null, Type[]? parameterTypes = null)
    {
        Type[] types;
        if(parameterTypes is null)
        {
            types = new Type[] { type };
        }
        else
        {
            types = new Type[parameterTypes.Length + 1];
            types[0] = type;
            Array.Copy(parameterTypes, 0, types, 1, parameterTypes.Length);
        }
        return new DynamicMethod(name, returnType, types);
    }
    public static void EmitNewarr(this ILGenerator il, Type type, int count)
    {
        EmitHelper.Ldc_I4(il, count);
        il.Emit(OpCodes.Newarr, type);
    }
}
public class DynamicMethodClass
{
    public DynamicMethod DynamicMethod { get; private set; }
    public bool IsStatic { get; private set; }
    public ILGeneratorClass ILGeneratorClass { get; private set; }
    public DynamicMethodClass(Type type, string name, Type? returnType = null, Type[]? parameterTypes = null)
    {
        Type[] types;
        if (parameterTypes is null)
        {
            types = new Type[] { type };
        }
        else
        {
            types = new Type[parameterTypes.Length + 1];
            types[0] = type;
            Array.Copy(parameterTypes, 0, types, 1, parameterTypes.Length);
        }
        IsStatic = false;
        DynamicMethod = new DynamicMethod(name, returnType, types);
        ILGeneratorClass = new(DynamicMethod);
    }
    public DynamicMethodClass(string name, Type? returnType = null, Type[]? parameterTypes = null)
    {
        IsStatic = true;
        DynamicMethod = new DynamicMethod(name, returnType, parameterTypes);
        ILGeneratorClass = new(DynamicMethod);
    }
    public T CreateDelegate<T>(object? target) where T : Delegate
    {
        if (IsStatic)
        {
            if (target is not null)
            {
                throw new ArgumentException($"Method is static,'{nameof(target)}' should is null ");
            }
            return (T)DynamicMethod.CreateDelegate(typeof(T), null);
        }
        else
        {
            if (target is null)
            {
                throw new ArgumentException($"Method is not Static,'{nameof(target)}' should is not null");
            }
            if (target.GetType() != DynamicMethod.GetParameters()[0].ParameterType)
            {
                throw new ArgumentException($"target type not equal the method firstmethod");
            }
            return (T)DynamicMethod.CreateDelegate(typeof(T), target);
        }
    }
}
public class ILGeneratorClass
{
    public ILGenerator IlGenerator;
    public Stack<Type> StackTypes = new();
    public List<Type> LocalTypes = new();
    private Type returnType;
    private bool needReturnType;
    private ParameterInfo[] parameterInfos;
    public ILGeneratorClass(DynamicMethod dynamicMethod) : this(dynamicMethod.GetILGenerator(), dynamicMethod.GetParameters(), dynamicMethod.ReturnType)
    {

    }
    public ILGeneratorClass(ILGenerator ilGenerator, ParameterInfo[] parameterInfos, Type returnType)
    {
        IlGenerator = ilGenerator;
        this.parameterInfos = parameterInfos;
        if(returnType is null || returnType == TypeOf.Void)
        {
            this.returnType = TypeOf.Void;
            needReturnType = false;
        }
        else
        {
            this.returnType = returnType;
            needReturnType = true;
        }
    }
    public void Br(Label label)
    {
        StackTypes.Pop();
        IlGenerator.Emit(OpCodes.Br, label);
    }

    public void Brtrue(Label label)
    {
        StackTypes.Pop();
        IlGenerator.Emit(OpCodes.Brtrue, label);
    }

    public void Call(MethodInfo meth)
    {
        for (int i = 0; i < meth.GetParameters().Length; i++)
        {
            StackTypes.Pop();
        }
        if (meth.ReturnType != TypeOf.Void)
        {
            StackTypes.Push(meth.ReturnType);
        }
        IlGenerator.Emit(OpCodes.Call, meth);
    }

    public void Callvirt(MethodInfo meth)
    {
        for (int i = 0; i < meth.GetParameters().Length; i++)
        {
            StackTypes.Pop();
        }  
        if(meth.ReturnType != TypeOf.Void)
        {
            StackTypes.Push(meth.ReturnType);
        }
        IlGenerator.Emit(OpCodes.Callvirt, meth);
    }
    //public void AutoCallvirt(int paramCount, string methodName)
    //{
    //    //pop param types and instance type
    //    for (int i = 0; i < paramCount; i++)
    //    {
    //        StackTypes.Pop();
    //    }
    //    Callvirt(StackTypes.Pop().GetMethod(methodName)!);
    //}
    //public void CallPropertyGet(string name) => AutoCallvirt(1, "get_" + name);
    //public void CallPropertySet(string name) => AutoCallvirt(1, "get_" + name);
    public void CallMethod(MethodInfo meth, bool notVirt = false)
    {
        if (meth.IsStatic || notVirt) 
        {
            Call(meth);
        }
        else
        {
            Callvirt(meth);
        }
    }
    public void Castclass(Type cls)
    {
        StackTypes.Pop();
        StackTypes.Push(cls);
        IlGenerator.Emit(OpCodes.Castclass, cls);
    }

    public void Dup()
    {
        StackTypes.Push(StackTypes.Peek());
        IlGenerator.Emit(OpCodes.Dup);
    }

    public ILGeneratorClass Ld(string name)
    {
        if (name.Contains('.'))
        {
            var names = name.Split('.');
            foreach (var n in names)
            {
                Ld(n);
            }
        }
        else
        {
            var member = StackTypes.Peek().GetMember(name).First();
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    var field = (FieldInfo)member;
                    if (field.IsStatic)
                    {
                        Ldsfld(field);
                    }
                    else
                    {
                        Ldfld(field);
                    }

                    break;
                case MemberTypes.Property:
                    Callvirt(((PropertyInfo)member).GetGetMethod()!);
                    break;
                default:
                    throw new NotSupportedException("");
            }
        }
        return this;
    }
    public ILGeneratorClass Ldarg(byte arg)
    {
        switch (arg)
        {
            case 0:
                IlGenerator.Emit(OpCodes.Ldarg_0);
                break;
            case 1:
                IlGenerator.Emit(OpCodes.Ldarg_1);
                break;
            case 2:
                IlGenerator.Emit(OpCodes.Ldarg_2);
                break;
            case 3:
                IlGenerator.Emit(OpCodes.Ldarg_3);
                break;
            default:
                IlGenerator.Emit(OpCodes.Ldarg_S, arg);
                break;
        }
        StackTypes.Push(parameterInfos[arg].ParameterType);
        return this;
    }
    public void Ldc_I4(int arg)
    {
        switch (arg)
        {
            case 0:
                IlGenerator.Emit(OpCodes.Ldc_I4_0);
                break;
            case 1:
                IlGenerator.Emit(OpCodes.Ldc_I4_1);
                break;
            case 2:
                IlGenerator.Emit(OpCodes.Ldc_I4_2);
                break;
            case 3:
                IlGenerator.Emit(OpCodes.Ldc_I4_3);
                break;
            case 4:
                IlGenerator.Emit(OpCodes.Ldc_I4_4);
                break;
            case 5:
                IlGenerator.Emit(OpCodes.Ldc_I4_5);
                break;
            case 6:
                IlGenerator.Emit(OpCodes.Ldc_I4_6);
                break;
            case 7:
                IlGenerator.Emit(OpCodes.Ldc_I4_7);
                break;
            case 8:
                IlGenerator.Emit(OpCodes.Ldc_I4_8);
                break;
            default:
                if (byte.TryParse(arg.ToString(), out var result))
                    IlGenerator.Emit(OpCodes.Ldc_I4_S, result);
                else
                    IlGenerator.Emit(OpCodes.Ldc_I4, arg);
                break;
        }
        StackTypes.Push(TypeOf.Int32);
    }
    public void Ldloc(short arg)
    {
        StackTypes.Push(LocalTypes[arg]);
        IlGenerator.Emit(OpCodes.Ldloc, arg);
    }

    public void Ldloc(LocalBuilder local)
    {
        StackTypes.Push(local.LocalType);
        IlGenerator.Emit(OpCodes.Ldloc, local);
    }

    public void Ldloca(short arg)
    {
        StackTypes.Push(LocalTypes[arg].MakeByRefType());
        IlGenerator.Emit(OpCodes.Ldloca, arg);
    }

    public ILGeneratorClass Ldfld(FieldInfo field)
    {
        StackTypes.Push(field.FieldType);
        IlGenerator.Emit(OpCodes.Ldfld, field);
        return this;
    }
    public ILGeneratorClass Ldfld(string name) => Ldfld(StackTypes.Pop().GetField(name)!);
    public ILGeneratorClass Ldfld(string name, BindingFlags bindingAttr) => Ldfld(StackTypes.Pop().GetField(name, bindingAttr)!);
    public void Ldsfld(FieldInfo field)
    {
        StackTypes.Push(field.FieldType);
        IlGenerator.Emit(OpCodes.Ldsfld, field);
    }
    public void Ldsfld(string name) => IlGenerator.Emit(OpCodes.Ldfld, StackTypes.Pop().GetField(name)!);
    public void Ldstr(string str)
    {
        StackTypes.Push(TypeOf.String);
        IlGenerator.Emit(OpCodes.Ldstr, str);
    }
    public void Newarr(Type cls)
    {
        //array length
        StackTypes.Pop();
        StackTypes.Push(cls.MakeArrayType());
        IlGenerator.Emit(OpCodes.Newarr, cls);
    }

    public void Pop()
    {
        StackTypes.Pop();
        IlGenerator.Emit(OpCodes.Pop);
    }

    public void Ret()
    {
        if (needReturnType)
        {
            if (StackTypes.Count == 0)
            {
                throw new Exception("not have return value in stack");
            }
            if (StackTypes.Count != 1)
            {
                throw new Exception("stack have more than 1 value");
            }
            if (!(StackTypes.Peek() == returnType) || !StackTypes.Peek().IsSubclassOf(returnType))
            {
                throw new Exception("The return value type does not match the return type");
            }
        }
        else
        {
            if (StackTypes.Count != 0)
            {
                throw new Exception("void method,stack have more than 0 value");
            }
        }
        if (needReturnType)
        {
            StackTypes.Pop();
        }
        IlGenerator.Emit(OpCodes.Ret);
    }

    public void Stelem_Ref()
    {
        StackTypes.Pop();
        StackTypes.Pop();
        IlGenerator.Emit(OpCodes.Stelem_Ref);
    }

    public void Stloc(LocalBuilder local)
    {
        StackTypes.Pop();
        IlGenerator.Emit(OpCodes.Stloc, local);
    }

    public void Stsfld(FieldInfo filed)
    {
        StackTypes.Pop();
        IlGenerator.Emit(OpCodes.Stsfld, filed);
    }

    public LocalBuilder DeclareLocal(Type type)
    {
        LocalTypes.Add(type);
        return IlGenerator.DeclareLocal(type);
    }
    //public static implicit operator ILGenerator(ILGeneratorClass il) => il.IlGenerator;
}
