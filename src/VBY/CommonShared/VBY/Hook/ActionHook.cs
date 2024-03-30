using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace VBY.Common.Hook;
public class ActionHook : HookBase
{
    public Action RegisterMethod{ get; }
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
        var opcodeList = new List<ILOpCode>();
        var operandList = new List<object>();
        var module = registerMethod.Module;
        for (int i = 0; i < bytes.Length; i++)
        {
            var value = (ILOpCode)bytes[i];
            if (value == ILOpCode.Nop)
            {
                continue;
            }
            switch (value)
            {
                case ILOpCode.Ldnull:
                case ILOpCode.Ldarg_0:
                case ILOpCode.Ret:
                    operandList.Add(null!);
                    opcodeList.Add(value);
                    break;
                case ILOpCode.Newobj:
                    operandList.Add(module.ResolveMethod(System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan().Slice(i + 1)))!);
                    i += 4;
                    opcodeList.Add(value);
                    break;
                case ILOpCode.Call:
                    operandList.Add(module.ResolveMethod(System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan().Slice(i + 1)))!);
                    i += 4;
                    opcodeList.Add(value);
                    break;
                case (ILOpCode)0xFE:
                    var extvalue = (ILOpCode)(0xfe00 | bytes[++i]);
                    switch (extvalue)
                    {
                        case ILOpCode.Ldftn:
                            operandList.Add(module.ResolveMethod(System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan().Slice(i + 1)))!);
                            i += 4;
                            opcodeList.Add(extvalue);
                            break;
                        default:
                            throw new InvalidOperationException(extvalue.ToString());
                    }
                    break;
                default:
                    throw new InvalidOperationException(value.ToString());
            }
        }
        ILOpCode firstCodeValue;
        Type[] parameterTypes;
        if (registerAction.Method.IsStatic)
        {
            firstCodeValue = ILOpCode.Ldnull;
            parameterTypes = Type.EmptyTypes;
        }
        else
        {
            firstCodeValue = ILOpCode.Ldarg_0;
            parameterTypes = new Type[] { registerAction.Target!.GetType() };
        }
        ILOpCode[] matchOpcodes = new ILOpCode[] { firstCodeValue, ILOpCode.Ldftn, ILOpCode.Newobj, ILOpCode.Call, ILOpCode.Ret };
        if (opcodeList.Count > matchOpcodes.Length)
        {
            throw new InvalidOperationException("method il length not match");
        }
        for (int i = 0; i < matchOpcodes.Length; i++)
        {
            if (opcodeList[i] != matchOpcodes[i])
            {
                throw new InvalidOperationException("method il not match");
            }
        }
        var method = new DynamicMethod("Remove" + ((MethodInfo)operandList[1]!).Name, registerMethod.ReturnType, parameterTypes, registerMethod.DeclaringType!);
        HookUtils.EmitEventChange(method.GetILGenerator(), (MethodInfo)operandList[1]!, (ConstructorInfo)operandList[2]!, ((MethodInfo)operandList[3]!).DeclaringType!.GetEvent(((MethodInfo)operandList[3]!).Name.Substring(4))!.GetRemoveMethod()!);
        UnregisterMethod = method.CreateDelegate<Action>(registerAction.Target);
    }
    protected override void RegisterAction() => RegisterMethod();
    protected override void UnregisterAction() => UnregisterMethod();
}