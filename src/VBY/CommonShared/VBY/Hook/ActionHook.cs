using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace VBY.Common.Hook;
public class ActionHook : HookBase
{
    public Action RegisterMethod { get; }
    public Action UnregisterMethod { get; }
    internal static bool HasActionHook = false;
    internal ActionHook(Action registerMethod, Action unregisterMethod, bool onPostInit = false, bool manual = false) : base(onPostInit, manual)
    {
        HasActionHook = true;
        RegisterMethod = registerMethod;
        UnregisterMethod = unregisterMethod;
    }
    public ActionHook(Action registerAction, bool onPostInit = false, bool manual = false) : base(onPostInit, manual)
    {
        HasActionHook = true;
        RegisterMethod = registerAction;
        var registerMethod = registerAction.Method;
        var bytes = registerMethod.GetMethodBody()!.GetILAsByteArray()!;
        var instructions = EmitUtils.GetInstructionsFromBytes(registerMethod.Module, bytes);
        Span<ILOpCode> matchOpcodes = stackalloc ILOpCode[] { instructions[0].OpCode, ILOpCode.Ldftn, ILOpCode.Newobj, ILOpCode.Call, ILOpCode.Ret };
        Span<ILOpCode> matchOpcodes2 = stackalloc ILOpCode[] { ILOpCode.Ldsfld, ILOpCode.Dup, ILOpCode.Brtrue_s, ILOpCode.Pop, ILOpCode.Ldnull, ILOpCode.Ldftn, ILOpCode.Newobj, ILOpCode.Dup, ILOpCode.Stsfld, ILOpCode.Call, ILOpCode.Ret };
        if (instructions.Count != matchOpcodes.Length && instructions.Count != matchOpcodes2.Length)
        {
            throw new InvalidOperationException("method il length not match");
        }
        var check = false;
        for (int i = 0; i < matchOpcodes.Length; i++)
        {
            if (instructions[i].OpCode != matchOpcodes[i])
            {
                check = true;
                break;
            }
        }
        if(check)
        {
            for (int i = 0; i < matchOpcodes2.Length; i++)
            {
                if (instructions[i].OpCode != matchOpcodes2[i])
                {
                    throw new InvalidOperationException("method il not match");
                }
            }
        }
        if (instructions.Count == matchOpcodes.Length)
        {
            object? target;
            Type[] parameterTypes;
            if (instructions[0].OpCode == ILOpCode.Ldarg_0)
            {
                target = registerAction.Target;
                parameterTypes = new Type[] { target!.GetType() };
            }
            else if (instructions[0].OpCode == ILOpCode.Ldnull)
            {
                target = null;
                parameterTypes = Type.EmptyTypes;
            }
            else
            {
                throw new ArgumentException(instructions[0].OpCode.ToString());
            }
            var method = new DynamicMethod("Remove" + ((MethodInfo)instructions[1].Operand!).Name, typeof(void), parameterTypes, registerMethod.DeclaringType!);
            HookUtils.EmitEventChange(method.GetILGenerator(), (MethodInfo)instructions[1].Operand!, (ConstructorInfo)instructions[2].Operand!, ((MethodInfo)instructions[3].Operand!).DeclaringType!.GetEvent(((MethodInfo)instructions[3].Operand!).Name.Substring(4))!.GetRemoveMethod()!);
            UnregisterMethod = method.CreateDelegate<Action>(target);
        }
        else
        {
            var method = new DynamicMethod("Remove" + ((MethodInfo)instructions[5].Operand!).Name, typeof(void), Type.EmptyTypes, registerMethod.DeclaringType!);
            var il = method.GetILGenerator();
            var callLabel = il.DefineLabel();
            var retLabel = il.DefineLabel();
            il.Emit(ILOpCode.Ldsfld, instructions[0].Operand);
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Brtrue_S, callLabel);
            il.Emit(OpCodes.Pop);
            il.Emit(OpCodes.Br_S, retLabel);
            il.MarkLabel(callLabel);
            il.Emit(ILOpCode.Call, ((MethodInfo)instructions[9].Operand!).DeclaringType!.GetEvent(((MethodInfo)instructions[9].Operand!).Name.Substring(4))!.GetRemoveMethod()!);
            il.MarkLabel(retLabel);
            il.Emit(OpCodes.Ret);
            UnregisterMethod = method.CreateDelegate<Action>(null);
        }
    }
    protected override void RegisterAction() => RegisterMethod();
    protected override void UnregisterAction() => UnregisterMethod();
}