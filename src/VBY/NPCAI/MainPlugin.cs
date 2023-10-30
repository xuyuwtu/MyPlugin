using TerrariaApi.Server;

using MonoMod.RuntimeDetour;

namespace VBY.NPCAI;

[ApiVersion(2, 1)]
public class NPCAIPlugin : TerrariaPlugin
{
    public override string Name => "NPCAI Base";
    public override string Author => "yu";
    private static readonly Detour AIDetour = new(typeof(NPC).GetMethod("AI"), typeof(AIs).GetMethod("AI"), new() { ManualApply = true });
    public NPCAIPlugin(Main game) : base(game) 
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