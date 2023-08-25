using TShockAPI;

namespace FlowerSeaRPG;

internal static class Strings
{
    internal static readonly string ConfigPath = Path.Combine(TShock.SavePath, "FlowerSeaRPG.json");
    internal static readonly string ChangePath = Path.Combine(TShock.SavePath, "FlowerSeaRPG.Change.json");
    internal static string ConfigDirectory = Path.Combine(TShock.SavePath, nameof(FlowerSeaRPG));
}
