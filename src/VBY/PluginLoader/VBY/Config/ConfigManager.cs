using System.Diagnostics;

using TShockAPI;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VBY.Common.Config;

public class ConfigManager<T>
{
    public string ConfigDirectory;
    public string ConfigPath;
    public Func<T> GetDefaultFunc;
    private T? instance;
    public T Instance
    {
        get => instance is null ? (instance = GetDefaultFunc()) : instance;
        set => instance = value;
    }

    public bool OnPostInit { get; set; } = false;
    public bool Manual { get; set; } = true;
    public bool Loaded { get; set; }

    private DefaultContractResolver _defaultContractResolver = new() { NamingStrategy = new CamelCaseNamingStrategy() };
    public JsonConverter Converter
    {
        set
        {
            if(value is not null)
            {
                SerializerSettings.Converters.Add(value);
            }
        }
    }
    private JsonSerializerSettings? _serializerSettings;
    public JsonSerializerSettings SerializerSettings 
    {
        get
        {
            if(_serializerSettings is null)
            {
                _serializerSettings = new()
                {
                    ContractResolver = _defaultContractResolver
                };
            }
            return _serializerSettings;
        }
        set
        {
            if(value is not null)
            {
                if(value.ContractResolver is null)
                {
                    value.ContractResolver = _defaultContractResolver;
                }
                _serializerSettings = value;
            }
        }
    }
    public Action<T, TSPlayer?>? PostLoadAction;
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
                T? config = JsonConvert.DeserializeObject<T>(File.ReadAllText(ConfigPath), SerializerSettings);
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
        PostLoadAction?.Invoke(Instance, player);
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
}
