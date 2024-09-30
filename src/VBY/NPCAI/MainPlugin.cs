using TerrariaApi.Server;

using MonoMod.RuntimeDetour;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace VBY.NPCAI;

[ApiVersion(2, 1)]
public class NPCAIPlugin : TerrariaPlugin
{
    public override string Name => "NPCAI Base";
    public override string Author => "yu";
    private static readonly Detour AIDetour = new(typeof(NPC).GetMethod("AI"), typeof(AIs).GetMethod("AI"), new() { ManualApply = true });
    public static bool HasNewProjectileInfoArray { get; private set; } = false;
    private static Action<int, NewProjectileInfo>? SetInfoAction;
    public NPCAIPlugin(Main game) : base(game) 
    {
    }
    public override void Initialize()
    {
        AIDetour.Apply();
        ServerApi.Hooks.GamePostInitialize.Register(this, OnGamePostInitialize);
    }

    private void OnGamePostInitialize(EventArgs args)
    {
        var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == "VBY.ProjectileAI");
        do
        {
            if (assembly is null)
            {
                break;
            }
            var type = assembly.GetType("VBY.ProjectileAI.ProjectileAIPlugin");
            if (type is null)
            {
                break;
            }
            var field = type.GetField("NewProjectileInfos");
            if(field is null)
            {
                break;
            }
            HasNewProjectileInfoArray = true;
            var method = new DynamicMethod("SetNewProjectileInfo", typeof(void), new Type[] { typeof(int), typeof(NewProjectileInfo) });
            var targetType = field.FieldType.GetElementType()!;
            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldsfld, field);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarga_S, (byte)1);
            il.Emit(OpCodes.Call, typeof(Unsafe).GetMethods().First(x => x.IsGenericMethod && x.GetGenericArguments().Length == 2).MakeGenericMethod(typeof(NewProjectileInfo), targetType));
            il.Emit(OpCodes.Ldobj, targetType);
            il.Emit(OpCodes.Stelem, targetType);
            il.Emit(OpCodes.Ret);
            SetInfoAction = method.CreateDelegate<Action<int, NewProjectileInfo>>();
        } while (false);
        ServerApi.Hooks.GamePostInitialize.Deregister(this, OnGamePostInitialize);
    }
    public static void SetNewProjectileInfo(int index, in NewProjectileInfo info) => SetInfoAction?.Invoke(index, info);
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            AIDetour.Dispose();
        }
        base.Dispose(disposing);
    }
}