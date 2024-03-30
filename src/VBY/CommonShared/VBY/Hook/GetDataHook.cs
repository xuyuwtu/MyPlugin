using TShockAPI;

namespace VBY.Common.Hook;

public sealed class GetDataHook<T> : HookBase where T : EventArgs
{
    public HandlerList<T> HandlerList { get; private set; }
    public EventHandler<T> EventHandler { get; private set; }
    public GetDataHook(HandlerList<T> handlerList, EventHandler<T> eventHandler, bool onPostInit = false, bool manual = false) : base(onPostInit, manual)
    {
        HandlerList = handlerList;
        EventHandler = eventHandler;
    }
    protected override void RegisterAction() => HandlerList.Register(EventHandler);
    protected override void UnregisterAction() => HandlerList.UnRegister(EventHandler);
}
