using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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

    private static unsafe void OnNetGetData(GetDataEventArgs args)
    {
        if(args.MsgID == (PacketTypes)MessageID.LockAndUnlock)
        {
            if(args.Length != LockAndUnlockPacket.Length)
            {
                Netplay.Clients[args.Msg.whoAmI].PendingTermination = true;
                return;
            }
            LockAndUnlockPacket packet;
            fixed (byte* ptr = &args.Msg.readBuffer[args.Index])
            {
                packet = Unsafe.ReadUnaligned<LockAndUnlockPacket>(ptr);
                if (!BitConverter.IsLittleEndian)
                {
                    packet.X = BinaryPrimitives.ReverseEndianness(packet.X);
                    packet.Y = BinaryPrimitives.ReverseEndianness(packet.Y);
                }
            }
            if(packet.Action == NetMessageID.LockAndUnlock.ActionChestUnlock)
            {
                int unlockX = packet.X;
                int unlockY = packet.Y;
                if (!(Main.tile[unlockX, unlockY] == null || Main.tile[unlockX + 1, unlockY] == null || Main.tile[unlockX, unlockY + 1] == null || Main.tile[unlockX + 1, unlockY + 1] == null))
                {
                    ITile tileSafely = Framing.GetTileSafely(unlockX, unlockY);
                    if(tileSafely.type == TileID.Containers && tileSafely.frameX / TileSize.S2 == TileStyleID.Containers.LockedGoldChest && !NPC.downedBoss3)
                    {
                        args.Handled = true;
                        NetMessage.SendTileSquare(args.Msg.whoAmI, unlockX, unlockY, 2, 2);
                        TShock.Players[args.Msg.whoAmI]?.Kick("骷髅王前禁止解锁箱子");
                    }
                }
            }
        }
    }
}
[StructLayout(LayoutKind.Sequential)]
internal struct LockAndUnlockPacket
{
    public const int Length = sizeof(byte) + sizeof(short) * 2;
    public byte Action;
    public short X;
    public short Y;
}
