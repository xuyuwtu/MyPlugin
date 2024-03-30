using TerrariaApi.Server;

namespace VBY.Common.Hook;

public sealed class ServerHook<ArgsType> : HookBase where ArgsType : EventArgs
{
    public TerrariaPlugin Plugin { get; private set; }
    public HandlerCollection<ArgsType> Collection { get; private set; }
    public HookHandler<ArgsType> Handler { get; private set; }
    public ServerHook(TerrariaPlugin plugin, HandlerCollection<ArgsType> collection, HookHandler<ArgsType> handler, bool onPostInit = false, bool manual = false) : base(onPostInit, manual)
    {
        Plugin = plugin;
        Collection = collection;
        Handler = handler;
    }
    protected override void RegisterAction() => Collection.Register(Plugin, Handler);
    protected override void UnregisterAction() => Collection.Deregister(Plugin, Handler);
}
