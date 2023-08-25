namespace VBY.PluginLoader;
internal class Config
{
    public string[] LoadFromDefault = new[]
    {
        "System.Data.Common",
        "TerrariaServer",
        "OTAPI",
        "OTAPI.Runtime",
        "ModFramework",
        "MonoMod.RuntimeDetour",
        "TShockAPI",
        "Microsoft.Data.Sqlite",
        "MySql.Data",
        "VBY.PluginLoader"
    };
    public string[] LoadFiles = new[]
    {
        "PluginLoader/*.dll"
    };
}