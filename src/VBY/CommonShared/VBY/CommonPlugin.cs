using System.ComponentModel;
using System.Reflection;

using Terraria;
using TerrariaApi.Server;
using TShockAPI;

using MonoMod.RuntimeDetour.HookGen;

using VBY.Common.Hook;
using VBY.Common.Loader;

namespace VBY.Common;

public abstract class CommonPlugin : TerrariaPlugin
{
    public override string Name 
    {
        get 
        {
            var type = GetType();
            return type.Namespace ?? type.Name;
        }
    }
    public override string Description => GetType().GetCustomAttribute<DescriptionAttribute>()?.Description ?? base.Description;
    public override Version Version => GetType().Assembly.GetName().Version ?? base.Version;
    internal protected List<TShockAPI.Command> AddCommands = new();
    internal protected List<HookBase> AttachHooks = new();
    internal protected List<ILoader> Loaders = new();
    protected CommonPlugin(Main game) : base(game) { }
    protected virtual void PreInitialize() { }
    public override sealed void Initialize()
    {
        PreInitialize();
        var methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        var attrMethods = new List<(MethodInfo method, AutoHookAttribute attr)>(methods.Length);
        foreach(var method in methods)
        {
            var attr = method.GetCustomAttribute<AutoHookAttribute>();
            if(attr is null)
            {
                continue;
            }
            attrMethods.Add((method, attr));
        }
        foreach(var (method, attr) in attrMethods)
        {
            if(attr.Type is null)
            {
                AttachHooks.Add(HookUtils.GetHook(ReflectionUtils.GetDelegate(method, this), attr.OnPostInit, attr.Manual));
                var type = method.GetParameters()[0].ParameterType;
                Console.WriteLine($"AddHook {string.Join('.', method.DeclaringType?.FullName, method.Name)} to {type.Namespace!}.{type.Name[5..]}");
            }
            else
            {
                if (attr.Type == typeof(HookManager))
                {
                    var propertyName = string.IsNullOrEmpty(attr.HookName) ? method.Name[2..] : attr.HookName;
                    var property = typeof(HookManager).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance) ?? throw new MissingMemberException(nameof(ServerApi), propertyName);
                    var argsType = property.PropertyType.GetGenericArguments()[0];
                    var hookHandlerCollection = property.GetValue(ServerApi.Hooks)!;
                    Func<HandlerCollection<GetDataEventArgs>, TerrariaPlugin, HookHandler<GetDataEventArgs>, bool, bool, HookBase> getHookFunc = HookUtils.GetHook<GetDataEventArgs>;
                    AttachHooks.Add((HookBase)getHookFunc.Method.GetGenericMethodDefinition().MakeGenericMethod(argsType).Invoke(null, new object[] { hookHandlerCollection, this, Delegate.CreateDelegate(typeof(HookHandler<>).MakeGenericType(argsType), method.IsStatic ? null : this, method), attr.OnPostInit, attr.Manual })!);
                    Console.WriteLine($"AddHook {string.Join('.', method.DeclaringType?.FullName, method.Name)} to ServerApi.Hook.{propertyName}");
                }
            }
        }
        //AttachHooks.AddRange(from method in GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
        //                     where method.GetCustomAttribute<AutoHookAttribute>() is not null
        //                     select HookUtils.GetHook(ReflectionUtils.GetDelegate(method, this)));
        AddCommands.GetLoader(x => Commands.ChatCommands.Add(x), x => Commands.ChatCommands.Remove(x)).AddTo(this);
        AttachHooks.GetLoader(x => x.Register(), x => x.Unregister()).AddTo(this);
        AttachOnPostInitializeHook(OnPostInitialize);
        Loaders.Where(x => !(x.OnPostInit || x.Manual)).ForEach(x => x.Load());
    }
    private void OnPostInitialize(EventArgs args) => Loaders.Where(x => x.OnPostInit).ForEach(x => x.Load());
    protected virtual void PreDispose(bool disposing) { }
    protected override sealed void Dispose(bool disposing)
    {
        PreDispose(disposing);
        if (disposing)
        {
            Loaders.ForEach(x => x.Clear());
            if (ActionHook.HasActionHook)
            {
                ((System.Collections.IDictionary)typeof(HookEndpointManager).GetField("OwnedHookLists", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!).Remove(GetType().Assembly);
            }
        }
        //空空如也，不执行
        //base.Dispose(disposing);
    }
    internal protected void AttachOnPostInitializeHook(HookHandler<EventArgs> hookHandler)
    {
        if (Main.netMode == 0)
        {
            AttachHooks.Add(ServerApi.Hooks.GamePostInitialize.GetHook(this, hookHandler));
        }
        else
        {
            hookHandler.Invoke(EventArgs.Empty);
        }
    }
}
