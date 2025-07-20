using TerrariaApi.Server;

using MonoMod.RuntimeDetour;

namespace VBY.ProjectileAI;

[ApiVersion(2, 1)]
public class Plugin : TerrariaPlugin
{
    public override string Name => "VBY.ProjectileAIPlugin Base";
    public override string Author => "yu";
    private static readonly Detour AIDetour = new(typeof(Projectile).GetMethod("AI"), typeof(AIs).GetMethod("AI"), new() { ManualApply = true });
    public static NewProjectileInfo[] NewProjectileInfos = new NewProjectileInfo[Main.maxProjectiles + 1];
    public Plugin(Main game) : base(game)
    {
    }
    public override void Initialize()
    {
        AIDetour.Apply();
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            AIDetour.Dispose();
        }
        base.Dispose(disposing);
    }
}