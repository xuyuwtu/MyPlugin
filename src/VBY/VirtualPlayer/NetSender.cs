using System.Buffers;
using System.IO.Streams.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using TShockAPI;
using TShockAPI.Sockets;

namespace VBY.VirtualPlayer;

public static class NetSender
{
    public static Dictionary<byte, Action<Player>> SendDataActions = new();
    static NetSender()
    {
        foreach(var field in typeof(MessageID).GetFields())
        {
            if (field.IsLiteral && field.FieldType == typeof(byte))
            {
                var methodInfo = typeof(NetSender).GetMethod(field.Name,new Type[] { typeof(Player)});
                if(methodInfo is not null)
                {
                    SendDataActions.Add((byte)field.GetValue(null)!, (Action<Player>)Delegate.CreateDelegate(typeof(Action<Player>), methodInfo));
                }
            }
        }
        foreach (var field in typeof(MessageID2).GetFields())
        {
            if (field.IsLiteral && field.FieldType == typeof(byte))
            {
                var methodInfo = typeof(NetSender).GetMethod(field.Name, new Type[] { typeof(Player) });
                if (methodInfo is not null)
                {
                    SendDataActions.Add((byte)field.GetValue(null)!, (Action<Player>)Delegate.CreateDelegate(typeof(Action<Player>), methodInfo));
                }
            }
        }
    }
    private static byte[] SendToAll(this byte[] data) => SendToAll(data, data.Length);
    private static byte[] SendToAll(this byte[] data, int size)
    {
        for (int i = 0; i < TShock.Players.Length; i++)
        {
            var tsply = TShock.Players[i];
            if(tsply is null)
            {
                continue;
            }
            if (tsply.Active)
            {
                Netplay.Clients[tsply.Index].Socket.AsyncSend(data, 0, size, Netplay.Clients[tsply.Index].ServerWriteCallBack);
            }
        }
        return data;
    }
    /// <summary>
    /// 4
    /// </summary>
    /// <param name="player"></param>
    public static void SyncPlayer(Player player)
    {
        var ms = new MemoryStream();
        var bw = new BinaryWriter(ms);
        bw.BaseStream.Position = 2;
        bw.Write(MessageID.SyncPlayer);

        bw.Write((byte)player.whoAmI); 
        bw.Write((byte)player.skinVariant);
        bw.Write((byte)player.hair);
        bw.Write(player.name);
        bw.Write(player.hairDye);
        NetMessage.WriteAccessoryVisibility(bw, player.hideVisibleAccessory);
        bw.Write(player.hideMisc);
        bw.WriteRGB(player.hairColor);
        bw.WriteRGB( player.skinColor);
        bw.WriteRGB(player.eyeColor);
        bw.WriteRGB(player.shirtColor);
        bw.WriteRGB(player.underShirtColor);
        bw.WriteRGB(player.pantsColor);
        bw.WriteRGB(player.shoeColor);
        BitsByte bitsByte16 = 0;
        if (player.difficulty == 1)
        {
            bitsByte16[0] = true;
        }
        else if (player.difficulty == 2)
        {
            bitsByte16[1] = true;
        }
        else if (player.difficulty == 3)
        {
            bitsByte16[3] = true;
        }
        bitsByte16[2] = player.extraAccessory;
        bw.Write(bitsByte16);
        BitsByte bitsByte17 = 0;
        bitsByte17[0] = player.UsingBiomeTorches;
        bitsByte17[1] = player.happyFunTorchTime;
        bitsByte17[2] = player.unlockedBiomeTorches;
        bitsByte17[3] = player.unlockedSuperCart;
        bitsByte17[4] = player.enabledSuperCart;
        bw.Write(bitsByte17);
        BitsByte bitsByte18 = 0;
        bitsByte18[0] = player.usedAegisCrystal;
        bitsByte18[1] = player.usedAegisFruit;
        bitsByte18[2] = player.usedArcaneCrystal;
        bitsByte18[3] = player.usedGalaxyPearl;
        bitsByte18[4] = player.usedGummyWorm;
        bitsByte18[5] = player.usedAmbrosia;
        bitsByte18[6] = player.ateArtisanBread;
        bw.Write(bitsByte18);
        bw.BaseStream.Position = 0;
        bw.Write((short)ms.ToArray().Length);
        SendToAll(ms.ToArray());
    }
    public static void SyncEquipment(byte index, short slot, short netID, short stack = 1, byte prefix = 0)
    {
        var sendData = GetArray(PacketLength.SyncEquipment, MessageID.SyncEquipment);
        sendData[3] = index;
        sendData.Set(4, slot);
        sendData.Set(6, stack);
        sendData[8] = prefix;
        sendData.Set(9, netID);
        sendData.SendToAll(PacketLength.SyncEquipment);
        sendData.ReturnPool();
    }
    public static void SyncEquipment(Player player, int slot) => SyncEquipment(player.whoAmI, slot, Utils.GetItemUseSlot(player, slot));
    public static void SyncEquipment(int index, int slot, Item item) => SyncEquipment((byte)index, (short)slot, (short)item.netID, (short)item.stack, item.prefix);
    public static void PlayerSpawn()
    {
        var sendData = GetArray(PacketLength.PlayerSpawn, MessageID.PlayerSpawn);
        sendData[3] = VirtualPlayer.VPlayerIndex;
        sendData.Set<short>(4) = (short)Main.spawnTileX;
        sendData.Set<short>(6) = (short)Main.spawnTileY;
        sendData.Set<int>(8) = 0;
        sendData.Set<short>(12) = 0;
        sendData.Set<short>(14) = 0;
        sendData[16] = 0;
        sendData.SendToAll(PacketLength.PlayerSpawn);
        sendData.ReturnPool();
    }
    public static void PlayerSpawn(Player player)
    {
        var packet = new PlayerSpawnPacket
        {
            SpawnX = (short)((int)player.position.X << 4),
            SpawnY = (short)((int)player.position.Y << 4),
        };
        packet.GetPacketData().SendToAll();
    }
    //13
    public static void PlayerControls(byte index, Vector2 position, Vector2 velocity, bool direction = false)
    {
        var sendData = new byte[25];
        Unsafe.As<byte, ushort>(ref sendData[0]) = (ushort)sendData.Length;
        sendData[2] = MessageID.PlayerControls;
        sendData[3] = index;

        sendData[4] = new BitsByte() {
            //[ControlFlag1.ControlRight] = true,
            [ControlFlag1.ControlUseItem] = VirtualPlayer.VirtualPlayersData[index].Player.controlUseItem,
            //[ControlFlag1.Direction] = VirtualPlayer.VPlayer.direction == 1
            [ControlFlag1.Direction] = direction
        };
        sendData[5] = new BitsByte() { 
            [ControlFlag2.Velocity] = true,
            [ControlFlag2.GravDir] = VirtualPlayer.VirtualPlayersData[index].Player.gravDir == 1 
        };
        sendData[6] = 0;
        sendData[7] = 0;

        sendData[8] = 0; //selectItem
        Unsafe.As<byte, float>(ref sendData[9]) = position.X;
        Unsafe.As<byte, float>(ref sendData[13]) = position.Y;
        Unsafe.As<byte, float>(ref sendData[17]) = velocity.X;
        Unsafe.As<byte, float>(ref sendData[21]) = velocity.Y;
        sendData.SendToAll();
    }
    public static void PlayerControls(Player player)
    {
        var sendData = new byte[25];
        Unsafe.As<byte, ushort>(ref sendData[0]) = (ushort)sendData.Length;
        sendData[2] = MessageID.PlayerControls;
        sendData[3] = (byte)player.whoAmI;

        sendData[4] = new BitsByte()
        {
            //[ControlFlag1.ControlRight] = true,
            [ControlFlag1.ControlUseItem] = player.controlUseItem,
            [ControlFlag1.Direction] = player.direction == 1
        };
        sendData[5] = new BitsByte()
        {
            [ControlFlag2.Velocity] = true,
            [ControlFlag2.GravDir] = player.gravDir == 1
        };
        sendData[6] = 0;
        sendData[7] = 0;

        sendData[8] = 0; //selectItem
        Unsafe.As<byte, float>(ref sendData[9]) = player.position.X;
        Unsafe.As<byte, float>(ref sendData[13]) = player.position.Y;
        Unsafe.As<byte, float>(ref sendData[17]) = player.velocity.X;
        Unsafe.As<byte, float>(ref sendData[21]) = player.velocity.Y;
        sendData.SendToAll();
    }
    public static void PlayerActive(byte index, bool active)
    {
        var sendData = GetArray(PacketLength.PlayerActive, MessageID.PlayerActive);
        sendData[3] = index;
        sendData[4] = active ? (byte)1 : (byte)0;
        sendData.SendToAll(PacketLength.PlayerActive);
        sendData.ReturnPool();
    }
    /// <summary>
    /// 14
    /// </summary>
    /// <param name="player"></param>
    public static void PlayerActive(Player player) => PlayerActive((byte)player.whoAmI, player.active);
    public static void PlayerLifeMana(byte whoAmi, short statLife, short statLifeMax)
    {
        var sendData = GetArray(PacketLength.PlayerLifeMana, MessageID.PlayerLifeMana);
        sendData[3] = whoAmi;
        sendData.Set(4, statLife);
        sendData.Set(6, statLifeMax);
        sendData.SendToAll(PacketLength.PlayerLifeMana);
        sendData.ReturnPool();
    }
    public static void PlayerLifeMana(Player player) => PlayerLifeMana((byte)player.whoAmI, (short)player.statLife, (short)player.statLifeMax);
    public static void TogglePVP(byte whoAmi, bool enable)
    {
        var packet = new TogglePVPPacket
        {
            whoAmi = whoAmi,
            enable = Convert.ToByte(enable)
        };
        packet.GetPacketData().SendToAll();
    }
    public static void TogglePVP(Player player) => TogglePVP((byte)player.whoAmI, player.hostile);
    public static void ReleaseItemOwnership(short value)
    {
        var sendData = new byte[PacketLength.PlayerLifeMana];
        Unsafe.As<byte, ushort>(ref sendData[0]) = (ushort)sendData.Length;
        sendData[2] = MessageID.ReleaseItemOwnership;

        Unsafe.As<byte, short>(ref sendData[3]) = value;
        SendToAll(sendData);
    }
    public static void ShotAnimationAndSound(byte index, float rotation, short animation)
    {
        var sendData = GetArray(PacketLength.ShotAnimationAndSound, MessageID.ShotAnimationAndSound);
        sendData[3] = index;
        sendData.Set(4, rotation);
        sendData.Set(8, animation);
        sendData.SendToAll(PacketLength.ShotAnimationAndSound);
        sendData.ReturnPool();
    }
    public static void PlayerMana(byte whoAmi, short statMana, short statManaMax)
    {
        var sendData = new byte[PacketLength.PlayerMana];
        Unsafe.As<byte, ushort>(ref sendData[0]) = (ushort)sendData.Length;
        sendData[2] = MessageID2.PlayerMana;
        sendData[3] = whoAmi;

        Unsafe.As<byte, short>(ref sendData[4]) = statMana;
        Unsafe.As<byte, short>(ref sendData[6]) = statManaMax;
        SendToAll(sendData);
    }
    public static void PlayerMana(Player player) => PlayerMana((byte)player.whoAmI, (short)player.statMana, (short)player.statManaMax);
    public static void PlayerTeam(byte whoAmi, byte team)
    {
        var sendData = new byte[PacketLength.PlayerTeam];
        Unsafe.As<byte, ushort>(ref sendData[0]) = (ushort)sendData.Length;
        sendData[2] = MessageID2.PlayerTeam;
        sendData[3] = whoAmi;

        sendData[4] = team;
        SendToAll(sendData);
    }
    public static void PlayerTeam(Player player) => PlayerTeam((byte)player.whoAmI, (byte)player.team);
    public static void PlayerBuffs(byte whoAmi, int[] buffTypes)
    {
        var sendData = new byte[3 + 1 + Player.maxBuffs * sizeof(ushort)];
        var position = 0;
        Unsafe.As<byte, ushort>(ref sendData[position]) = (ushort)sendData.Length;
        position += 2;
        sendData[position++] = MessageID.PlayerBuffs;
        sendData[position++] = whoAmi;
        for (int i = 0; i < Player.maxBuffs; i++)
        {
            Unsafe.As<byte, ushort>(ref sendData[position]) = (ushort)buffTypes[i];
            position += 2;
        }
        SendToAll(sendData);
    }
    public static void PlayerBuffs(Player player) => PlayerBuffs((byte)player.whoAmI, player.buffType);
    public static void AnglerQuestFinished(byte whoAmi, int anglerQuestsFinished, int golferScoreAccumulated)
    {
        var sendData = new byte[PacketLength.AnglerQuestFinished];
        Unsafe.As<byte, ushort>(ref sendData[0]) = (ushort)sendData.Length;
        sendData[2] = MessageID.PlayerBuffs;
        sendData[3] = whoAmi;

        Unsafe.As<byte, int>(ref sendData[4]) = anglerQuestsFinished;
        Unsafe.As<byte, int>(ref sendData[8]) = golferScoreAccumulated;
    }
    public static void AnglerQuestFinished(Player player) => AnglerQuestFinished((byte)player.whoAmI, player.anglerQuestsFinished, player.golferScoreAccumulated);

    public static void PlayerText(byte whoAmi, string text)
    {
        Terraria.Net.NetManager.Instance.Broadcast(Terraria.GameContent.NetModules.NetTextModule.SerializeServerMessage(Terraria.Localization.NetworkText.FromLiteral(text), Color.White, whoAmi), -1);
    }
    public static void PlayerText(Player player, string text) => PlayerText((byte)player.whoAmI, text);

    public static void SyncLoadout(byte whoAmi, byte loadout, ushort hideVisibleAccessory)
    {
        var sendData = new byte[PacketLength.SyncLoadout];
        Unsafe.As<byte, ushort>(ref sendData[0]) = (ushort)sendData.Length;
        sendData[2] = MessageID.SyncLoadout;
        sendData[3] = whoAmi;

        sendData[4] = loadout;
        Unsafe.As<byte, ushort>(ref sendData[5]) = hideVisibleAccessory;
        SendToAll(sendData);
    }
    public static void SyncLoadout(Player player)
    {
        ushort hideVisibleAccessory = 0;
        for (int i = 0; i < player.hideVisibleAccessory.Length; i++)
        {
            if (player.hideVisibleAccessory[i])
            {
                hideVisibleAccessory = (ushort)(hideVisibleAccessory | (ushort)(1 << i));
            }
        }
        SyncLoadout((byte)player.whoAmI, (byte)player.CurrentLoadoutIndex, hideVisibleAccessory);
    }
    public static void SendData(byte msgType, Player player, int number = 0)
    {
        if (SendDataActions.TryGetValue(msgType, out var action))
        {
            action(player);
        }
        else
        {
            switch (msgType)
            {
                case MessageID.SyncEquipment:
                    SyncEquipment(player, number);
                    break;
                case MessageID.ReleaseItemOwnership:
                    ReleaseItemOwnership((short)number);
                    break;
            }
        }
    }
    private static byte[] GetArray(ushort length, byte messageID)
    {
        var array = ArrayPool<byte>.Shared.Rent(length);
        Unsafe.As<byte, ushort>(ref array[0]) = length;
        array[2] = messageID;
        return array;
    }

    private static void ReturnPool(this byte[] array) => ArrayPool<byte>.Shared.Return(array);
    private static ref T Set<T>(this byte[] array, int index) => ref Unsafe.As<byte, T>(ref array[index]);
    private static void Set<T>(this byte[] array, int index, T value) => Unsafe.As<byte, T>(ref array[index]) = value;
}
public static class PacketLength
{
    public const ushort SyncEquipment = 3 + 8;
    public const ushort PlayerSpawn = 3 + 14;
    public const ushort PlayerActive = 3 + 2;
    public const ushort PlayerLifeMana = 3 + 5;
    public const ushort ShotAnimationAndSound = 3 + 7;
    public const ushort PlayerMana = 3 + 5;
    public const ushort PlayerTeam = 3 + 2;
    public const ushort AnglerQuestFinished = 3 + 9;
    public const ushort SyncLoadout = 3 + 4;
}
public static class ControlFlag1
{
    public const int ControlRight = 3;
    public const int ControlUseItem = 5;
    public const int Direction = 6;
}
public static class ControlFlag2
{
    public const int Velocity = 2;
    public const int GravDir = 4;
}
public static class MessageID2
{
    public const byte PlayerMana = MessageID.Unknown42;
    public const byte PlayerTeam = MessageID.Unknown45;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TogglePVPPacket
{
    private readonly ushort Length;
    private readonly byte PacketID;

    public byte whoAmi;
    public byte enable;

    internal const ushort DataLength = sizeof(byte) + sizeof(byte);
    public TogglePVPPacket()
    {
        Length = 3 + DataLength;
        PacketID = MessageID.TogglePVP;
    }
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct PlayerManaPacket
{
    private readonly ushort Length;
    private readonly byte PacketID;

    public byte whoAmi;
    public short statMana;
    public short statManaMax;

    internal const ushort DataLength = sizeof(byte) + sizeof(short) + sizeof(short);
    public PlayerManaPacket()
    {
        Length = 3 + DataLength;
        PacketID = MessageID2.PlayerMana;
    }
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct PlayerSpawnPacket
{
    private readonly ushort Length;
    private readonly byte PacketID;

    public byte WhoAmi;
    public short SpawnX;
    public short SpawnY;
    public int RespawnTimer;
    public short NumberOfDeathsPVP;
    public short NumberOfDeathsPVE;
    public byte SpawnContext;

    internal const ushort DataLength = 1 + 2 + 2 + 4 + 2 + 2 + 1;
    public PlayerSpawnPacket()
    {
        Length = 3 + DataLength;
        PacketID = MessageID.TogglePVP;
    }
}

public static class PacketSender
{
    public static unsafe byte[] GetPacketData<T>(this ref T packet) where T : struct => new Span<byte>(Unsafe.AsPointer(ref packet), Unsafe.As<T, ushort>(ref packet)).ToArray();
    public static void Return(this byte[] data) => ArrayPool<byte>.Shared.Return(data);
}