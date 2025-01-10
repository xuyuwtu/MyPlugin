using Terraria;
using TerrariaApi.Server;

namespace VBY.PluginLoaderAutoReload;
[ApiVersion(2, 1)]
public class PluginLoaderAutoReload : TerrariaPlugin
{
    public override string Name => nameof(PluginLoaderAutoReload);
    public override string Description => "检测PluginLoader文件夹更改后自动重载";
    public static FileSystemWatcher Watcher = new("PluginLoader")
    {
        Filter = "*.dll",
        NotifyFilter = NotifyFilters.LastWrite,
        EnableRaisingEvents = true
    };
    public static System.Timers.Timer Timer = new(1500) { AutoReset = false };
    static PluginLoaderAutoReload()
    {
        Timer.Elapsed += OnElapsed;
        Watcher.Changed += OnChanged;
    }

    public PluginLoaderAutoReload(Main game) : base(game)
    {
        
    }

    public override void Initialize()
    {
        
    }
    protected override void Dispose(bool disposing)
    {
        if(disposing) 
        { 
            Watcher.Dispose();
            Timer.Dispose();
        }
    }
    public static void OnChanged(object sender, FileSystemEventArgs e) 
    {
        if(Timer.Enabled)
        {
            Timer.Stop();
        }
        Timer.Start();
    }
    public static void OnElapsed(object? sender, System.Timers.ElapsedEventArgs e) => PluginLoader.PluginLoader.Reload(TShockAPI.TSPlayer.Server);
}