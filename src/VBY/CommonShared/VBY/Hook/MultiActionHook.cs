using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace VBY.Common.Hook;

public class MultiActionHook : HookBase
{
    public Action RegisterMethod { get; }
    public Action UnregisterMethod { get; }
    public MultiActionHook(Action registerAction, bool onPostInit = false, bool manual = false) : base(onPostInit, manual)
    {
        RegisterMethod = registerAction;
        var registerMethod = registerAction.Method;
        var bytes = registerMethod.GetMethodBody()!.GetILAsByteArray()!;
        var instructions = EmitUtils.GetInstructionsFromBytes(registerMethod.Module, bytes);
        if (!instructions.Where(x => x.OpCode == ILOpCode.Nop).All(x => x.IsBranchTarget))
        {
            throw new Exception("nop is branch target");
        }
        instructions.RemoveCollection(instructions.Where(x => x.OpCode == ILOpCode.Nop));
        if (instructions[^1].OpCode != ILOpCode.Ret)
        {
            throw new Exception($"invalid method {registerAction.Method.DeclaringType}.{registerAction.Method.Name}");
        }
        var instanceMethodMatchOpcodes = new ILOpCode[] { default, ILOpCode.Ldftn, ILOpCode.Newobj, ILOpCode.Call };
        var staticMethodMatchOpcodes = new ILOpCode[] { ILOpCode.Ldsfld, ILOpCode.Dup, ILOpCode.Brtrue_s, ILOpCode.Pop, ILOpCode.Ldnull, ILOpCode.Ldftn, ILOpCode.Newobj, ILOpCode.Dup, ILOpCode.Stsfld, ILOpCode.Call };
        var index = 0;
        var count = instructions.Count;
        var method = new DynamicMethod("Remove_" + registerAction.Method.Name, typeof(void), registerAction.Target is null ? Type.EmptyTypes : new Type[] { registerAction.Target.GetType() }, registerMethod.DeclaringType!);
        var il = method.GetILGenerator();
        while (index < count - 1)
        {
            //Console.WriteLine($"index: {index} count: {count}");
            var canCheckStatic = index + staticMethodMatchOpcodes.Length < count;
            var canCheckInstance = canCheckStatic || index + instanceMethodMatchOpcodes.Length < count;
            var checkSuccess = true;
            var successLength = 0;
            if (canCheckStatic)
            {
                //Console.WriteLine("Static Checking");
                for (int i = 0; i < staticMethodMatchOpcodes.Length; i++)
                {
                    //Console.WriteLine($"{instructions[index + i].OpCode} == {staticMethodMatchOpcodes[i]} {instructions[index + i].OpCode == staticMethodMatchOpcodes[i]}");
                    if (instructions[index + i].OpCode != staticMethodMatchOpcodes[i])
                    {
                        checkSuccess = false;
                        break;
                    }
                }
                if(checkSuccess)
                {
                    successLength = staticMethodMatchOpcodes.Length;
                }
            }
            else
            {
                checkSuccess = false;
            }
            //Console.WriteLine($"CheckSuccess: {checkSuccess}");
            if (!checkSuccess)
            {
                if (!canCheckInstance)
                {
                    throw new Exception($"invalid method {registerAction.Method.DeclaringType}.{registerAction.Method.Name}");
                }
                if (instructions[index].OpCode is not (ILOpCode.Ldarg_0 or ILOpCode.Ldnull))
                {
                    throw new Exception($"invalid OpCode is not Ldarg_0 or Ldnull");
                }
                checkSuccess = true;
                //Console.WriteLine("Instance Checking");
                for (int i = 1; i < instanceMethodMatchOpcodes.Length; i++)
                {
                    //Console.WriteLine($"{instructions[index + i].OpCode} == {instanceMethodMatchOpcodes[i]} {instructions[index + i].OpCode == instanceMethodMatchOpcodes[i]}");
                    if (instructions[index + i].OpCode != instanceMethodMatchOpcodes[i])
                    {
                        checkSuccess = false;
                        break;
                    }
                }
                if (checkSuccess)
                {
                    successLength = instanceMethodMatchOpcodes.Length;
                }
            }
            if (!checkSuccess)
            {
                throw new Exception($"invalid method {registerAction.Method.DeclaringType}.{registerAction.Method.Name}");
            }
            if(successLength == 0)
            {
                throw new Exception($"match error");
            }
            if (successLength == instanceMethodMatchOpcodes.Length)
            {
                EmitUtils.Emit(il, instructions[index]);
                EmitUtils.Emit(il, instructions[index + 1]);
                EmitUtils.Emit(il, instructions[index + 2]);
                EmitUtils.Emit(il, ILOpCode.Call, ((MethodInfo)instructions[index + 3].Operand!).DeclaringType!.GetEvent(((MethodInfo)instructions[index + 3].Operand!).Name.Substring(4))!.GetRemoveMethod()!);
                //Console.WriteLine($"{((MethodInfo)instructions[index + 3].Operand!).DeclaringType!.FullName}.{((MemberInfo)instructions[index+3].Operand!).Name}");
            }
            else
            {
                var callLabel = il.DefineLabel();
                var postCallLabel = il.DefineLabel();
                il.Emit(ILOpCode.Ldsfld, instructions[index].Operand);
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Brtrue_S, callLabel);
                il.Emit(OpCodes.Pop);
                il.Emit(OpCodes.Br_S, postCallLabel);
                il.MarkLabel(callLabel);
                il.Emit(ILOpCode.Call, ((MethodInfo)instructions[index + 9].Operand!).DeclaringType!.GetEvent(((MethodInfo)instructions[index + 9].Operand!).Name.Substring(4))!.GetRemoveMethod()!);
                il.MarkLabel(postCallLabel);
                //Console.WriteLine($"{((MethodInfo)instructions[index + 9].Operand!).DeclaringType!.FullName}.{((MemberInfo)instructions[index + 9].Operand!).Name}");
            }
            index += successLength;
        }
        il.Emit(OpCodes.Ret);
        UnregisterMethod = method.CreateDelegate<Action>(registerAction.Target);
    }
    protected override void RegisterAction() => RegisterMethod();
    protected override void UnregisterAction() => UnregisterMethod();
}
