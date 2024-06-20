namespace VBY.PluginLoader;
internal class Config
{
    public string[] LoadFromDefault = new[]
    {
        "System.Data.Common",
        "TerrariaServer",
        "TShockAPI",
        "OTAPI",
        "OTAPI.Runtime",
        "MySql.Data",
        "Microsoft.Data.Sqlite",
        "MonoMod.RuntimeDetour",
        "ModFramework",
        "VBY.PluginLoader"
    };
    public string[] LoadFiles = new[]
    {
        "PluginLoader/*.dll"
    };
}