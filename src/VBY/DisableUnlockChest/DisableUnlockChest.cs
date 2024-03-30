using Terraria;
using Terraria.ID;
using TerrariaApi.Server;
using TShockAPI;

namespace VBY.DisableUnlockChest;

public class DisableUnlockChest : TerrariaPlugin
{
    public override string Name => GetType().Namespace ?? base.Name;
    public override string Description => "骷髅王前禁止开箱";
    public DisableUnlockChest(Main game) : base(game)
    {

    }

    public override void Initialize()
    {
        ServerApi.Hooks.NetGetData.Register(this, OnNetGetData);
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ServerApi.Hooks.NetGetData.Deregister(this, OnNetGetData);
        }
    }

    private static void OnNetGetData(GetDataEventArgs args)
    {
        if(args.MsgID == PacketTypes.ChestUnlock)
        {
            var ms = new MemoryStream(args.Msg.readBuffer, args.Index, args.Length);
            using var br = new BinaryReader(ms);
            if(br.ReadByte() == LockAndUnlockTypeID.ChestUnlock)
            {
                int unlockX = br.ReadInt16();
                int unlockY = br.ReadInt16();
                if (!(Main.tile[unlockX, unlockY] == null || Main.tile[unlockX + 1, unlockY] == null || Main.tile[unlockX, unlockY + 1] == null || Main.tile[unlockX + 1, unlockY + 1] == null))
                {
                    ITile tileSafely = Framing.GetTileSafely(unlockX, unlockY);
                    if(tileSafely.type == TileID.Containers && tileSafely.frameX / 36 == 2 && !NPC.downedBoss3)
                    {
                        args.Handled = true;
                        NetMessage.SendTileSquare(args.Msg.whoAmI, unlockX, unlockY, 2);
                        TShock.Players[args.Msg.whoAmI]?.Kick("骷髅王前禁止解锁箱子");
                    }
                }
            }
        }
    }
}
public static class LockAndUnlockTypeID
{
    public const int ChestUnlock = 1;
    public const int Door = 2;
    public const int ChestLock = 3;
}