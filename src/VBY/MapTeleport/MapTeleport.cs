using Microsoft.Xna.Framework;

using Terraria;
using TerrariaApi.Server;

using TShockAPI;

using VBY.Common;
using VBY.Common.Hook;

namespace VBY.MapTeleport;
[ApiVersion(2, 1)]
public class MapTeleport : CommonPlugin
{
    public override string Name => GetType().Namespace!;
    public override string Author => "yu";
    public const string MapTeleportPermission = "maptp";
    public const string BackPermission = "back";
    public MapTeleport(Main game) : base(game)
    {
        AddCommands.Add(new(BackPermission, BackCmd, "back")
        {
            HelpText = "返回最后一次死亡的位置"
        });
        AttachHooks.Add(GetDataHandlers.ReadNetModule.GetHook(OnReadNetModule));
        AttachHooks.Add(GetDataHandlers.KillMe.GetHook(OnKillMe));
    }
    private static void OnReadNetModule(object? sender, GetDataHandlers.ReadNetModuleEventArgs e)
    {
        if (e.ModuleType == GetDataHandlers.NetModuleType.Ping && e.Player.HasPermission(MapTeleportPermission))
        {
            using BinaryReader reader = new(e.Data);
            var position = reader.ReadVector2();
            if (WorldGen.InWorld((int)position.X, (int)position.Y))
            {
                e.Player.Teleport(position.X * 16f, position.Y * 16f, 1);
            }
        }
    }

    private static void OnKillMe(object? sender, GetDataHandlers.KillMeEventArgs e) => e.Player.SetData("DeadPoint", e.Player.TPlayer.position.ToPoint());

    private static void BackCmd(CommandArgs args)
    {
        if (args.Player.TPlayer.dead)
        {
            args.Player.SendErrorMessage("你尚未复活, 无法传送回死亡地点.");
            return;
        }
        var point = args.Player.GetData<Point>("DeadPoint");
        if (point == default)
        {
            args.Player.SendErrorMessage("你还未死亡过");
        }
        else
        {
            args.Player.Teleport(point.X, point.Y, 1);
            args.Player.SendSuccessMessage($"已传送至死亡地点 [c/8DF9D8:<{point.X / 16} - {point.Y / 16}>].");
        }
    }
}