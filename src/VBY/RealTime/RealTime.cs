using Terraria;
using Terraria.GameContent.Creative;
using TerrariaApi.Server;

using TShockAPI;

namespace VBY.RealTime;
[ApiVersion(2, 1)]
public class RealTime : TerrariaPlugin
{
    public override string Name => "VBY.RealTime";
    public override string Author => "yu";
    public override string Description => "同步真实时间";
    private static DateTime LastTime = DateTime.Now;
    public RealTime(Main game) : base(game)
    {
    }

    public override void Initialize()
    {
        CreativePowerManager.Initialize();
        CreativePowerManager.Instance.GetPower<CreativePowers.FreezeTime>().Enabled = true;
        On.Terraria.Main.UpdateTime += OnMain_UpdateTime;
    }
    protected override void Dispose(bool disposing)
    {
        if(disposing) 
        {
            On.Terraria.Main.UpdateTime += OnMain_UpdateTime;
        }
        base.Dispose(disposing);
    }
    private static void OnMain_UpdateTime(On.Terraria.Main.orig_UpdateTime orig)
    {
        orig();
        if (Math.Abs(DateTime.Now.Minute - LastTime.Minute) > 0)
        {
            LastTime = DateTime.Now;
            SetTime(LastTime);
        }
    }
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