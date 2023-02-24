using VBY.Basic.Config;

namespace VBY.ProgressQuery;

public class Config : MainConfig<Root>
{
    public Config(string configDirectory, string fileName = "Progress.json") : base(configDirectory, fileName)
    {
    }
}
public class Root : MainRoot
{
    public 文本 文本 = new();
}
public class 文本
{
    public string[] Boss名称 = Array.Empty<string>();
    public string[] 事件名称 = Array.Empty<string>();
}