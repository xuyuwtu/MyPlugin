using Terraria;
using TerrariaApi.Server;

namespace VBY.PluginLoaderAutoReload;
[ApiVersion(2, 1)]
public class PluginLoaderAutoReload : TerrariaPlugin
{
    public static FileSystemWatcher Watcher;
    //public static Dictionary<string, int> ChangeCount = new();
    static PluginLoaderAutoReload()
    {
        Watcher = new("PluginLoader")
        {
            Filter = "*.dll",
            NotifyFilter = NotifyFilters.LastWrite,
            EnableRaisingEvents = true
        };
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
        }
        base.Dispose(disposing);
    }
    public static void OnChanged(object sender, FileSystemEventArgs e) 
    {
        PluginLoader.PluginLoader.Reload(TShockAPI.TSPlayer.Server);
        //var name = e.Name;
        //if (!string.IsNullOrEmpty(name))
        //{
        //    if (ChangeCount.TryGetValue(name, out var count))
        //    {
        //        ChangeCount[name]++;
        //        if (count == 3)
        //        {
        //            PluginLoader.PluginLoader.Reload(TShockAPI.TSPlayer.Server); 
        //            ChangeCount[name] = 0;
        //        }
        //    }
        //    else
        //    {
        //        ChangeCount[name] = 1;
        //    }
        //}
    }
}