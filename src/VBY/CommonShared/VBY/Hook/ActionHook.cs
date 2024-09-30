using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace VBY.Common.Hook;
public class ActionHook : HookBase
{
    public Action RegisterMethod { get; }
    public Action? UnregisterMethod { get; }
    private Delegate m_delegeObject;
    private MethodInfo m_removeMethod;
    internal static bool HasActionHook = false;
    internal ActionHook(Action registerMethod, Action unregisterMethod, bool onPostInit = false, bool manual = false) : base(onPostInit, manual)
    {
        HasActionHook = true;
        RegisterMethod = registerMethod;
        UnregisterMethod = unregisterMethod;
        m_delegeObject = null!;
        m_removeMethod = null!;
    }
    public ActionHook(Action registerAction, bool onPostInit = false, bool manual = false) : base(onPostInit, manual)
    {
        HasActionHook = true;
        RegisterMethod = registerAction;
        //if (!System.Diagnostics.Debugger.IsAttached)
        //{
        //    System.Diagnostics.Debugger.Launch();
        //}
        var registerMethod = registerAction.Method;
        var module = registerAction.Method.Module;
        var bytes = registerMethod.GetMethodBody()!.GetILAsByteArray()!;
        var instructions = EmitUtils.GetInstructionsFromBytes(bytes);
        var ldftnIns = instructions.Single(x => x.OpCode == ILOpCode.Ldftn);
        var newobjIns = instructions.Single(x => x.OpCode == ILOpCode.Newobj);
        if(instructions.IndexOf(ldftnIns) + 1 != instructions.IndexOf(newobjIns))
        {
            throw new Exception("MSIL is not [ldftn, newobj]");
        }
        var targetOpCode = instructions[instructions.IndexOf(ldftnIns) - 1].OpCode;
        if (targetOpCode is not ILOpCode.Ldnull or ILOpCode.Ldarg_0)
        {
            throw new Exception($"methodName: [{registerMethod.DeclaringType!.FullName}.{registerMethod.Name}] first instruction is not Ldnull or Ldarg_0");
        }
        var callIns = instructions.Single(x => x.OpCode == ILOpCode.Call);
        var callMethod = module.ResolveMethod((int)callIns.Operand!)!;
        if (!callMethod.Name.StartsWith("add_"))
        {
            throw new Exception("methodName is not startwith 'add_'");
        }
        var removeMethodName = string.Concat("remove_", callMethod.Name.AsSpan("add_".Length));
        var removeMethod = callMethod.DeclaringType!.GetMethod(removeMethodName);
        if (removeMethod is null)
        {
            throw new Exception($"can't find remove method '{removeMethodName}'");
        }
        var ldftnMethod = module.ResolveMethod((int)ldftnIns.Operand!)!;
        var newobjMethod = module.ResolveMethod((int)newobjIns.Operand!)!;
        if (targetOpCode == ILOpCode.Ldnull)
        {
            m_delegeObject = Delegate.CreateDelegate(newobjMethod.DeclaringType!, (MethodInfo)ldftnMethod);
        }
        else
        {
            m_delegeObject = Delegate.CreateDelegate(newobjMethod.DeclaringType!, registerAction.Target, (MethodInfo)ldftnMethod);
        }
        //m_delegeObject = ((ConstructorInfo)newobjMethod).Invoke(new object?[] { registerAction.Target, ldftnMethod.MethodHandle.Value })!;
        m_removeMethod = removeMethod;
    }
    protected override void RegisterAction() => RegisterMethod();
    protected override void UnregisterAction()
    {
        if (UnregisterMethod is null)
        {
            m_removeMethod.Invoke(null, new object?[] { m_delegeObject });
        }
        else
        {
            UnregisterMethod();
        }
    }
}