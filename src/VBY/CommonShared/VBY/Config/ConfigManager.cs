using System.Diagnostics;

using TShockAPI;

using Newtonsoft.Json;

using VBY.Common.Loader;

namespace VBY.Common.Config;

public class ConfigManager<T> : ILoader
{
    public string ConfigDirectory;
    public string ConfigPath;
    public Func<T> GetDefaultFunc;
    public bool First { get; private set; } = true;
    private T? instance;
    public T Instance
    {
        get => instance is null ? (instance = GetDefaultFunc()) : instance;
        set => instance = value;
    }

    public bool OnPostInit { get; set; } = false;
    public bool Manual { get; set; } = false;
    public bool Loaded { get; set; }

    public JsonConverter? Converter;
    public JsonSerializerSettings? SerializerSettings;
    public Action<T, TSPlayer?, bool>? PostLoadAction;
    public Action<T>? PreSaveAction;
    public ConfigManager(string configDirectory, string configFileName, Func<T> getDefaultFunc)
    {
        ConfigDirectory = configDirectory;
        ConfigPath = Path.Combine(configDirectory, configFileName);
        GetDefaultFunc = getDefaultFunc;
    }
    public ConfigManager(Func<T> getDefaultFunc, string? configFileNameWithoutExtension = null, string configDirectory = "Config")
    {
        ConfigDirectory = configDirectory;
        GetDefaultFunc = getDefaultFunc;
        if (string.IsNullOrEmpty(configFileNameWithoutExtension))
        {
            configFileNameWithoutExtension = new StackTrace().GetFrame(1)!.GetMethod()!.DeclaringType!.Namespace! + ".";
        }
        ConfigPath = Path.Combine(configDirectory, Path.ChangeExtension(configFileNameWithoutExtension, "json"));
    }
    public bool Load(TSPlayer? player)
    {
        if (!Directory.Exists(ConfigDirectory))
        {
            Directory.CreateDirectory(ConfigDirectory);
        }
        if (File.Exists(ConfigPath))
        {
            try
            {
                T? config;
                if (Converter is not null)
                {
                    config = JsonConvert.DeserializeObject<T>(File.ReadAllText(ConfigPath), Converter);
                }
                else
                {
                    config = JsonConvert.DeserializeObject<T>(File.ReadAllText(ConfigPath), SerializerSettings);
                }
                if (config is null)
                {
                    return false;
                }
                instance = config;
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.ToString());
                player?.SendErrorMessage(player.RealPlayer ? ex.ToString() : "转换配置文件 '{0}' 错误", ConfigPath);
                return false;
            }
        }
        else
        {
            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Instance, Formatting.Indented, SerializerSettings));
        }
        PostLoadAction?.Invoke(Instance, player, First);
        First = false;
        return true;
    }
    public void Save(Formatting formatting = Formatting.Indented)
    {
        if (!Directory.Exists(ConfigDirectory))
        {
            Directory.CreateDirectory(ConfigDirectory);
        }
        PreSaveAction?.Invoke(Instance);
        File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Instance, formatting, SerializerSettings));
    }
    void ILoader.Load() => Load(TSPlayer.Server);
    void ILoader.Clear() { }
}
