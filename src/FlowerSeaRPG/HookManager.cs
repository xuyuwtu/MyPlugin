using TerrariaApi.Server;
using TShockAPI;

namespace FlowerSeaRPG;

public interface IHookManager
{
    public bool Init { get; protected set; }
    public bool Registered { get; protected set; }
    public abstract void Register();
    public abstract void Unregister();
    public abstract void Initialize();
}
public abstract class HookRegisterManager<TList, THandler> : IHookManager, IDisposable
{
    private bool disposedValue;

    public bool Init { get; set; }
    public bool Registered { get; set; } = false;
    public TList List { get; protected set; }
    public THandler Handler { get; protected set; }

    protected HookRegisterManager(TList list, THandler handler, bool init = true)
    {
        List = list;
        Handler = handler;
        Init = init;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: 释放托管状态(托管对象)
                Unregister();
#pragma warning disable CS8601 // 引用类型赋值可能为 null。
                List = default;
                Handler = default;
#pragma warning restore CS8601 // 引用类型赋值可能为 null。
            }

            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    public abstract void Register();
    public abstract void Unregister();
    public virtual void Initialize() => Register();
}
public class GetDataHandlerManager<T> : HookRegisterManager<HandlerList<T>, EventHandler<T>> where T : EventArgs
{
    public GetDataHandlerManager(HandlerList<T> list, EventHandler<T> handler, bool init = true) : base(list, handler, init)
    {
    }
    public override void Register()
    {
        if (!Registered)
        {
            Registered = true;
            List.Register(Handler);
        }
    }
    public override void Unregister()
    {
        if (Registered)
        {
            Registered = false;
            List.UnRegister(Handler);
        }
    }
}
public class ServerHookHandlerManager<T> : HookRegisterManager<HandlerCollection<T>, HookHandler<T>> where T : EventArgs
{
    public ServerHookHandlerManager(HandlerCollection<T> list, TerrariaPlugin plugin, HookHandler<T> handler, bool init = true) : base(list, handler, init)
    {
        Plugin = plugin;
    }
    public TerrariaPlugin Plugin { get; private set; }
    public override void Register()
    {
        if (!Registered)
        {
            Registered = true;
            List.Register(Plugin, Handler);
        }
    }
    public override void Unregister()
    {
        if (Registered)
        {
            Registered = false;
            List.Deregister(Plugin, Handler);
        }
    }
}