using Newtonsoft.Json;

using VBY.Common.Config;

namespace VBY.ProgressQuery;

public class Config : ConfigBase<Root>
{
    public Config(string configDirectory, string fileName = "Progress.json") : base(configDirectory, fileName)
    {
    }
}
public class Root : RootBase
{
    [JsonProperty("文本")]
    public Texts Texts = new();
}
public class Texts
{
    [JsonProperty("Boss名称")]
    public string[] BossNames = Array.Empty<string>();
    [JsonProperty("事件名称")]
    public string[] EventNames = Array.Empty<string>();
}