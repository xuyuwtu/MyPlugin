using Terraria;
using TerrariaApi.Server;

namespace VBY.PluginLoaderAutoReload;
[ApiVersion(2, 1)]
public class PluginLoaderAutoReload : TerrariaPlugin
{
    public override string Name => nameof(PluginLoaderAutoReload);
    //public static Dictionary<string, int> ChangeCount = new();
    public static FileSystemWatcher Watcher = new("PluginLoader")
    {
        Filter = "*.dll",
        NotifyFilter = NotifyFilters.LastWrite,
        EnableRaisingEvents = true
    };
    public static System.Timers.Timer Timer = new(1000) { AutoReset = false };
    //private const int CheckCount = 3;
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
        //Console.WriteLine(e.FullPath);
        //Console.WriteLine(e.ChangeType);
        //PluginLoader.PluginLoader.Reload(TShockAPI.TSPlayer.Server);
        //var name = e.Name;
        //if (!string.IsNullOrEmpty(name))
        //{
        //    if (!ChangeCount.ContainsKey(name))
        //    {
        //        ChangeCount[name] = 0;
        //    }
        //    ChangeCount[name] = ChangeCount[name] + 1;
        //    if (ChangeCount[name] == CheckCount)
        //    {
        //        PluginLoader.PluginLoader.Reload(TShockAPI.TSPlayer.Server);
        //        ChangeCount[name] = 0;
        //    }
        //}
    }
    public static void OnElapsed(object? sender, System.Timers.ElapsedEventArgs e) => PluginLoader.PluginLoader.Reload(TShockAPI.TSPlayer.Server);
}