using System.Reflection;

using Terraria;
using TerrariaApi.Server;

using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace RestFix;
[ApiVersion(2, 1)]
public class RestFix : TerrariaPlugin
{
    private readonly IDetour detour;
    public RestFix(Main game) : base(game)
    {
        detour = new ILHook(typeof(Rests.Rest).GetMethod("OnRequest", 
            BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic), new ILContext.Manipulator(ILHook_Mitigation_KeepRestAlive));
    }
    public override void Initialize() { }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            detour.Dispose();
        base.Dispose(disposing);
    }
    private void ILHook_Mitigation_KeepRestAlive(ILContext context)
    {
        var cursor = new ILCursor(context);
        cursor.GotoNext(MoveType.Before, (i) => i.MatchCallvirt("HttpServer.Headers.ConnectionHeader", "set_Type"));
        cursor.Emit(OpCodes.Pop);
        cursor.Emit(OpCodes.Ldc_I4_1);
    }
}