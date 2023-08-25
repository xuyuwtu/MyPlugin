using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;

namespace VBY.GameContentModify;

internal static class Utils
{
    public static T SelectRandom<T>(Terraria.Utilities.UnifiedRandom random, params T[] choices)
    {
        return choices[random.Next(choices.Length)];
    }
    public static void SetEmpty(this Terraria.Chest chest)
    {
        for (int i = 0; i < 40; i++)
        {
            chest.item[i] = new Terraria.Item();
        }
    }
    public static Detour GetDetour<T>(Delegate method, bool manualApply = true)
    {
        return new Detour(typeof(T).GetMethod(method.Method.Name), method.Method, new DetourConfig() { ManualApply = manualApply });
    }
    internal static void ClearOwner(Delegate hook)
    {
        var dic = (IDictionary)typeof(HookEndpointManager).GetField("OwnedHookLists", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!;
        var owner = HookEndpointManager.GetOwner(hook);
        if (owner is not null)
        {
            dic.Remove(owner);
        }
    }
}
