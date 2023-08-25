using TShockAPI;

using Newtonsoft.Json;
using IL.Terraria.GameContent.Bestiary;

namespace VBY.GameContentModify.Config;

public class ConfigBase<T>
{
    public string ConfigDirectory;
    public string ConfigPath;
    public Func<T> GetDefaultFunc;
    public T Instance;
    public JsonConverter? Converter;
    public JsonSerializerSettings? SerializerSettings;
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
            return true;
        }
        else
        {
            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Instance, Formatting.Indented, SerializerSettings));
            return true;
        }
    }
    public void Save(Formatting formatting = Formatting.Indented)
    {
        if (!Directory.Exists(ConfigDirectory))
        {
            Directory.CreateDirectory(ConfigDirectory);
        }
        File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Instance, formatting));
    }
}
