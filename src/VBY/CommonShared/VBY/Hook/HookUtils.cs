using System.Reflection;
using System.Reflection.Emit;

using Terraria;
using TerrariaApi.Server;

using TShockAPI;

namespace VBY.Common.Hook;

public static class HookUtils
{
    public static HookBase GetHook(Delegate hook, bool onPostInit = false, bool manual = false)
    {
        var hookMethdParameters = hook.Method.GetParameters();
        if(hookMethdParameters.Length == 0)
        {
            throw new ArgumentException("hook parameters length is equal zero", nameof(hook));
        }
        var firstParam = hookMethdParameters[0];
        if (firstParam.ParameterType.IsNested && firstParam.ParameterType.Assembly.GetName().Name == "OTAPI.Runtime")
        {
            var paramType = firstParam.ParameterType;
            var typeEvent = paramType.DeclaringType!.GetEvent(paramType.Name[5..])!;
            var constructor = typeEvent.EventHandlerType!.GetConstructors().First();
            var parameterTypes = hook.Method.IsStatic ? Type.EmptyTypes : new Type[] { hook.Target!.GetType() };
            var addMethod = new DynamicMethod($"{hook.Method.DeclaringType!}.AddEvent{hook.Method.Name}", typeof(void), parameterTypes, hook.Method.DeclaringType!);
            EmitEventChange(addMethod.GetILGenerator(), hook.Method, constructor, typeEvent.GetAddMethod()!);
            var removeMethod = new DynamicMethod($"{hook.Method.DeclaringType!}.RemoveEvent{hook.Method.Name}", typeof(void), parameterTypes, hook.Method.DeclaringType!);
            EmitEventChange(removeMethod.GetILGenerator(), hook.Method, constructor, typeEvent.GetRemoveMethod()!);
            return new ActionHook(addMethod.CreateDelegate<Action>(hook.Target), removeMethod.CreateDelegate<Action>(hook.Target), onPostInit, manual);
        }
        throw new NotImplementedException();
    }
    public static HookBase GetHook<ArgsType>(this HandlerCollection<ArgsType> hookCollection, TerrariaPlugin hookPlugin, HookHandler<ArgsType> hookHandler, bool onPostInit = false, bool manual = false) where ArgsType : EventArgs => new ServerHook<ArgsType>(hookPlugin, hookCollection, hookHandler, onPostInit, manual);
    public static HookBase GetHook<ArgsType>(this HandlerList<ArgsType> hookCollection, EventHandler<ArgsType> hookHandler, bool onPostInit = false, bool manual = false) where ArgsType : EventArgs => new GetDataHook<ArgsType>(hookCollection, hookHandler, onPostInit, manual);
    internal static void EmitEventChange(ILGenerator iLGenerator, MethodInfo addEventMethod, ConstructorInfo eventTypeConstructor, MethodInfo eventMethod)
    {
        if (addEventMethod.IsStatic)
        {
            iLGenerator.Emit(OpCodes.Ldnull);
        }
        else
        {
            iLGenerator.Emit(OpCodes.Ldarg_0);
        }
        iLGenerator.Emit(OpCodes.Ldftn, addEventMethod);
        iLGenerator.Emit(OpCodes.Newobj, eventTypeConstructor);
        iLGenerator.Emit(OpCodes.Call, eventMethod);
        iLGenerator.Emit(OpCodes.Ret);
    }
}
