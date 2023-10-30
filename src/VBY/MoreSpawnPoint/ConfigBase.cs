using TShockAPI;

using Newtonsoft.Json;

namespace VBY.MoreSpawnPoint;

public class ConfigBase<T>
{
    public string ConfigDirectory;
    public string ConfigPath;
    public Func<T> GetDefaultFunc;
    public T Instance;
    public JsonConverter? Converter;
    public Action<T>? PostRead;
    public Action<T>? PreSave;
    public ConfigBase(string configDirectory, string configPath, Func<T> getDefaultFunc)
    {
        ConfigDirectory = configDirectory;
        ConfigPath = configPath;
        GetDefaultFunc = getDefaultFunc;
        Instance = getDefaultFunc();
    }
    public bool Load(TSPlayer player)
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
                if (Converter is null)
                {
                    config = JsonConvert.DeserializeObject<T>(File.ReadAllText(ConfigPath));
                }
                else
                {
                    config = JsonConvert.DeserializeObject<T>(File.ReadAllText(ConfigPath), Converter);
                }
                Instance = config ?? GetDefaultFunc();
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.ToString());
                player.SendErrorMessage(player.RealPlayer ? ex.ToString() : "转换配置文件 '{0}' 错误", ConfigPath);
                return false;
            }
            finally
            {
                Instance ??= GetDefaultFunc();
            }
            PostRead?.Invoke(Instance);
            return true;
        }
        else
        {
            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Instance, Formatting.Indented));
            return true;
        }
    }
    public void Save(Formatting formatting = Formatting.Indented)
    {
        if (!Directory.Exists(ConfigDirectory))
        {
            Directory.CreateDirectory(ConfigDirectory);
        }
        PreSave?.Invoke(Instance);
        File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Instance, formatting));
    }
}
