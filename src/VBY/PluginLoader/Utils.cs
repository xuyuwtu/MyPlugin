using System.Collections;
using System.Reflection;

using MonoMod.RuntimeDetour.HookGen;

namespace VBY.PluginLoader;

public static class Utils
{
    public static void ClearOwner(Delegate hook)
    {
        var dic = (IDictionary)typeof(HookEndpointManager).GetField("OwnedHookLists", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!;
        var owner = HookEndpointManager.GetOwner(hook);
        if(owner is not null)
        {
            dic.Remove(owner);
        }
    }
}
