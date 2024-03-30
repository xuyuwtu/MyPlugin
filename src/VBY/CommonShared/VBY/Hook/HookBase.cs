using VBY.Common.Loader;

namespace VBY.Common.Hook;

public abstract class HookBase : ILoader
{
    public bool Registered { get; protected set; }
    public bool OnPostInit { get; protected set; }
    public bool Manual { get; protected set; }
    bool ILoader.Loaded => Registered;
    void ILoader.Load() => Register();
    void ILoader.Clear() => Unregister();
    protected HookBase(bool onPostInit = false, bool manual = false)
    {
        OnPostInit = onPostInit;
        Manual = manual;
    }
    public bool Register()
    {
        if (!Registered)
        {
            RegisterAction();
            Registered = true;
            return true;
        }
        return false;
    }
    protected abstract void RegisterAction();
    public bool Unregister()
    {
        if (Registered)
        {
            UnregisterAction();
            Registered = false;
            return true;
        }
        return false;
    }
    protected abstract void UnregisterAction();
}
