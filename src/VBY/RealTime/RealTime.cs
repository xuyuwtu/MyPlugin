using System.ComponentModel;

using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Events;
using Terraria.ID;
using TerrariaApi.Server;

using TShockAPI;

using VBY.Common;
using VBY.Common.Config;
using VBY.Common.Hook;

namespace VBY.RealTime;

[ApiVersion(2, 1)]
[Description("同步真实时间")]
public class RealTime : CommonPlugin
{
    public override string Author => "yu";
    private static DateTime LastTime = DateTime.Now;
    private static readonly ConfigManager<Config> MainConfig = new("Config", "VBY.RealTime.json", static () => new());
    public RealTime(Main game) : base(game)
    {
        AttachOnPostInitializeHook(OnPostInitizlize);
    }
    [AutoHook]
    public static void OnMain_UpdateTime(On.Terraria.Main.orig_UpdateTime orig)
    {
        orig();
        // 18:00->0 17:59->59 0-59<0
        if (Math.Abs(DateTime.Now.Minute - LastTime.Minute) > 0)
        {
            LastTime = DateTime.Now;
            SetTime(LastTime);
            if (MainConfig.Instance.RaiseMethod)
            {
                if (LastTime.Hour == 4 && LastTime.Minute == 30 || LastTime.Minute % MainConfig.Instance.RaiseMinute == 0)
                {
                    StartDay();
                }
                else if (LastTime.Hour == 19 && LastTime.Minute == 30 || LastTime.Minute % MainConfig.Instance.RaiseMinute == 0)
                {
                    StartNight();
                }
            }
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
    private static void StartDay()
    {
        var time = Main.time;
        var stopEvents = Main.ShouldNormalEventsBeAbleToStart();

        Main.eclipse = false;

        Main.UpdateTime_StartDay(ref stopEvents);
        Main.time = time;
    }
    private static void StartNight()
    {
        var time = Main.time;
        var stopEvents = Main.ShouldNormalEventsBeAbleToStart();

        WorldGen.ResetTreeShakes();
        Main.stopMoonEvent();
        Main.bloodMoon = false;
        if (Main.drunkWorld)
        {
            WorldGen.crimson = !WorldGen.crimson;
        }
        Main.moonPhase++;
        if (Main.moonPhase >= 8)
        {
            Main.moonPhase = 0;
        }
        LanternNight.CheckMorning();

        Main.UpdateTime_StartNight(ref stopEvents);
        Main.time = time;
    }
}