using System.Reflection;
using System.Runtime.Loader;

using Terraria;
using TerrariaApi.Server;

using TShockAPI;

using Newtonsoft.Json;
using SingleFileExtractor.Core;
using VBY.Common.Config;

namespace VBY.PluginLoader;

[ApiVersion(2, 1)]
public class PluginLoader : TerrariaPlugin
{
    public override string Name => GetType().Name;
    public override string Author => "yu";
    public override Version Version => GetType().Assembly.GetName().Version!;
    public override string Description => "也许可卸载的插件加载器";

#pragma warning disable IDE0044 // 添加只读修饰符
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    private Command AddCommand;
    private static List<WeakReference> OldLoaders = new();
    private static MyAssemblyLoadContext Loader;
    private static ConfigManager<Config> MainConfig = new(static () => new()) { PostLoadAction = PostLoad };
    internal static string ConfigPath = Path.Combine("Config", typeof(PluginLoader).Namespace + ".json");
    //internal static string PluginPath = "PluginLoader";
    internal static bool Debug = false;
    public static int LoaderNum = 0;
    public PluginLoader(Main game) : base(game)
    {
        Loader = new MyAssemblyLoadContext("VBY.PluginLoader" + LoaderNum++);
        AddCommand = new(Name.ToLower(), Ctl, "load");
    }
    public override void Initialize()
    {
        MainConfig.Load(TSPlayer.Server);
        Commands.ChatCommands.Add(AddCommand);
        Loader.LoadPlugin(TSPlayer.Server);
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Commands.ChatCommands.Remove(AddCommand);
            Loader.UnloadPlugin(TSPlayer.Server);
        }
    }
    private void Ctl(CommandArgs args)
    {
        if (!args.Parameters.Any())
        {
            args.Player.SendInfoMessage(
                    "/load load load PluginLoader\n" +
                    "/load unload unload PluginLoader\n" +
                    "/load reload unload then load PluginLoader\n" +
                    "/load clear clear old PluginLoader and reset num\n" +
                    "/load plugins show plugins\n" +
                    "/load info show info");
            return;
        }
        bool one = args.Parameters.Count == 1;
        switch (args.Parameters[0])
        {
            case "load":
                if (Loader.Assemblies.Any())
                {
                    args.Player.SendInfoMessage("have assembly, don't load");
                    break;
                }
                Loader.LoadPlugin(args.Player);
                if (LoaderNum > 10)
                {
                    for (int i = 0; OldLoaders.All(x => x.IsAlive) && (i < 10); i++)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                }
                break;
            case "unload":
                if (!Loader.Assemblies.Any())
                {
                    args.Player.SendInfoMessage("don't have assembly, don't unload");
                    break;
                }
                Loader.UnloadPlugin(args.Player);
                OldLoaders.Add(new(Loader));
                Loader = new("VBY.PluginLoader" + LoaderNum++);
                break;
            case "reload":
                Loader.UnloadPlugin(args.Player);
                OldLoaders.Add(new(Loader));
                Loader = new("VBY.PluginLoader" + LoaderNum++);
                Loader.LoadPlugin(args.Player);
                if(LoaderNum > 10)
                {
                    Console.WriteLine("[PluginLoader]AutoClear");
                    for (int i = 0; OldLoaders.Any(x => x.IsAlive) && (i < 10); i++)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                }
                break;
            case "clear":
                for (int i = 0; OldLoaders.Any(x => x.IsAlive) && (i < 10); i++)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                OldLoaders.RemoveAll(x => !x.IsAlive);
                LoaderNum = OldLoaders.Count == 0 ? 1 : OldLoaders.Count - 1;
                args.Player.SendInfoMessage("current active loader count:{0}", OldLoaders.Count);
                break;
            case "info":
                Loader.Assemblies.ToList().ForEach(x => args.Player.SendInfoMessage($"Assembly:{x.GetName().Name} Version:{x.GetName().Version}"));
                args.Player.SendInfoMessage("current active old loader count:{0}", OldLoaders.Count);
                OldLoaders.Where(x => x.IsAlive).ForEach(x =>
                {
                    if (x.Target is not null)
                    {
                        var context = (MyAssemblyLoadContext)x.Target;
                        Console.WriteLine(context.Name);
                        Console.WriteLine(string.Join("\n", context.Assemblies.Select(x => $"{x.GetName().Name} Version:{x.GetName().Version}")));
                    }
                });
                break;
            case "plugins":
                if (Loader.Plugins.Count == 0)
                {
                    args.Player.SendInfoMessage("Plugin Count: 0");
                }
                else
                {
                    Loader.Plugins.ForEach(plugin =>
                    {
                        args.Player.SendInfoMessage($"Plugin {plugin.Name} v{plugin.Version} (by {plugin.Author})");
                    });
                }
                break;
            case "debug":
                {
                    Debug = !Debug;
                    args.Player.SendInfoMessage($"Debug: {Debug}");
                }
                break;
            default:
                args.Player.SendInfoMessage("unknown subcmd {0}", args.Parameters[0]);
                break;
        }
    }
    public static void Reload(TSPlayer player)
    {
        Loader.UnloadPlugin(player);
        OldLoaders.Add(new(Loader));
        Loader = new("VBY.PluginLoader" + LoaderNum++);
        Loader.LoadPlugin(player);
    }
    private static void PostLoad(Config config, TSPlayer? _)
    {
        foreach (string path in config.LoadFiles)
        {
            var d = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(d) && !Directory.Exists(d))
            {
                Directory.CreateDirectory(d);
            }
        }
        MyAssemblyLoadContext.LoadFiles = config.LoadFiles;
        MyAssemblyLoadContext.LoadFromDefault = config.LoadFromDefault;
    }
}

class MyAssemblyLoadContext : AssemblyLoadContext
{
    internal static string[] LoadFromDefault = Array.Empty<string>();
    internal static string[] LoadFiles = Array.Empty<string>();
    internal List<TerrariaPlugin> Plugins = new();
    List<Assembly> LoadAssemblies = new();
    public MyAssemblyLoadContext(string name) : base(name, true)
    {
        Unloading += MyAssemblyLoadContext_Unloading;
    }

    private static void MyAssemblyLoadContext_Unloading(AssemblyLoadContext obj)
    {
        Console.WriteLine("当前正在卸载程序集:{0}", string.Join(", ", obj.Assemblies.Select(x => x.GetName().Name)));
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        if (LoadFromDefault.Contains(assemblyName.Name))
        {
            if(PluginLoader.Debug) Console.WriteLine("LoadFromDefault: {0} Version={1}", assemblyName.Name, assemblyName.Version);
            return AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName);
        }
        if(LoadAssemblies.Select(x => x.GetName()).Contains(assemblyName))
        {
            if (PluginLoader.Debug) Console.WriteLine("LoadFromThis: {0} Version={1}", assemblyName.Name, assemblyName.Version);
            return LoadAssemblies.Find(x => x.GetName() == assemblyName);
        }
        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName);
        if (!string.IsNullOrEmpty(assembly.Location!))
        {
            //Console.WriteLine("LoadFromAssemblyPath: {0} Version={1} Path:{2}", assemblyName.Name, assemblyName.Version, assembly.Location);
            if (PluginLoader.Debug) Console.WriteLine("LoadFromAssemblyPath: {0} Version={1}", assemblyName.Name, assemblyName.Version);
            return LoadFromAssemblyPath(assembly.Location!);
        }
        if (PluginLoader.Debug) Console.WriteLine("LoadFromNull: {0} Version={1}", assemblyName.Name, assemblyName.Version);
        return null;
    }
    public void LoadPlugin(TSPlayer player)
    {
        using var reader = new ExecutableReader(Environment.ProcessPath!);
        if (reader.IsSingleFile)
        {
            using var news = reader.Bundle.Files.First(x => x.Type == FileType.Assembly && x.RelativePath == "Newtonsoft.Json.dll").AsStream();
            LoadFromStream(news);
        }
        else
        {
            try
            {
                using var fileStream = File.OpenRead(typeof(JsonConvert).Assembly.Location);
                LoadFromStream(fileStream);
            }
            catch(NotSupportedException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        foreach (var path in LoadFiles)
        {
            var filePaths = Directory.GetFiles(Path.GetDirectoryName(path) ?? "", Path.GetFileName(path));
            var assemblys = filePaths.Select(filePath =>
            {
                using var stream = File.OpenRead(filePath);
                using var pdbStream = File.Exists(Path.ChangeExtension(filePath, ".pdb")) ? File.OpenRead(Path.ChangeExtension(filePath, ".pdb")) : null;
                return LoadFromStream(stream, pdbStream);
                //return LoadFromStream(stream);
            });
            LoadAssemblies.AddRange(assemblys);
            //GetReferencedAssemblies()
            foreach (var assembly in assemblys)
            {
                foreach (var exportedType in assembly.GetExportedTypes())
                {
                    if (exportedType.IsSubclassOf(typeof(TerrariaPlugin)) && !exportedType.IsAbstract)
                    {
                        var terrariaPlugin = (TerrariaPlugin)Activator.CreateInstance(exportedType, Main.instance)!;
                        terrariaPlugin.Initialize();
                        Plugins.Add(terrariaPlugin);
                        player.SendInfoMessage($"[{Name}]Info: Plugin {terrariaPlugin.Name} v{terrariaPlugin.Version} (by {terrariaPlugin.Author}) initiated");
                    }
                }
            }
        }
    }
    public void UnloadPlugin(TSPlayer? player = null)
    {
        Plugins.ForEach(x =>
        {
            if (player is null)
            {
                Console.WriteLine($"[{Name}]Info]Info: Plugin {x.Name} v{x.Version} (by {x.Author}) disponsed");
            }
            else
            {
                player.SendInfoMessage($"[{Name}]Info: Plugin {x.Name} v{x.Version} (by {x.Author}) disponsed");
            }
            x.Dispose();
        });
        Plugins.Clear();
        LoadAssemblies.Clear();
        Unload();
    }
}