using Terraria;
using TerrariaApi.Server;

using TShockAPI;

using VBY.Common.Config;

namespace VBY.TimedUnlockProgress;

[ApiVersion(2, 1)]
public class TimedUnlockProgress : TerrariaPlugin
{
    public static ConfigManager<Config> MainConfig = new(() => new()) { PostLoadAction = PostLoad };
    public static Dictionary<int, TimeSpan> UnlockTimeInfo = new();
    public static DateTime StartedTime = DateTime.Now;
    public TimedUnlockProgress(Main game) : base(game)
    {
        
    }

    public override void Initialize()
    {
        MainConfig.Load(TSPlayer.Server);
        ServerApi.Hooks.NpcSpawn.Register(this, OnNpcSpawn);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ServerApi.Hooks.NpcSpawn.Deregister(this, OnNpcSpawn);
        }
    }
    private void OnNpcSpawn(NpcSpawnEventArgs args)
    {
        if(UnlockTimeInfo.ContainsKey(args.NpcId))
        {
            args.Handled = DateTime.Now - StartedTime < UnlockTimeInfo[args.NpcId];
        }
    }

    private static void PostLoad(Config config, TSPlayer? player)
    {
        if (string.IsNullOrEmpty(config.StartedTime))
        {
            config.StartedTime = DateTime.Now.ToString();
            MainConfig.Save();
        }
        else
        {
            if(DateTime.TryParse(config.StartedTime, out var result))
            {
                StartedTime = result;
            }
            else
            {
                player?.SendErrorMessage("时间格式错误");
            }
        }
        UnlockTimeInfo.Clear();
        foreach(var info in config.UnlockInfos)
        {
            foreach(var id in info.IDs)
            {
                UnlockTimeInfo.Add(id, info.Time);
            }
        }
    }
}