using System.ComponentModel;

using Terraria;
using Terraria.GameContent.Creative;
using TerrariaApi.Server;

using TShockAPI;

using VBY.Common;
using VBY.Common.Hook;

namespace VBY.RealTime;

[ApiVersion(2, 1)]
[Description("同步真实时间")]
public class RealTime : CommonPlugin
{
    public override string Author => "yu";
    private static DateTime LastTime = DateTime.Now;
    public RealTime(Main game) : base(game)
    {
        AttachOnPostInitializeHook(OnPostInitizlize);
    }

    [AutoHook]
    private static void OnMain_UpdateTime(On.Terraria.Main.orig_UpdateTime orig)
    {
        orig();
        if (Math.Abs(DateTime.Now.Minute - LastTime.Minute) > 0)
        {
            LastTime = DateTime.Now;
            SetTime(LastTime);
        }
    }
    private static void OnPostInitizlize(EventArgs args) => CreativePowerManager.Instance.GetPower<CreativePowers.FreezeTime>().Enabled = true;
    private static void SetTime(DateTime setTime)
    {
        decimal time = setTime.Hour + setTime.Minute / 60.0m;
        time -= 4.5m;
        if (time < 0.0m)
        {
            time += 24.0m;
        }
        if (time >= 15.0m)
        {
            TSPlayer.Server.SetTime(dayTime: false, (double)((time - 15.0m) * 3600.0m));
        }
        else
        {
            TSPlayer.Server.SetTime(dayTime: true, (double)(time * 3600.0m));
        }
    }
}