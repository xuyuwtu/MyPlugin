using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;

using TShockAPI;

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
    private static void SendRawData(byte[] data)
    {
        for (int i = 0; i < TShock.Players.Length; i++)
        {
            TShock.Players[i]?.SendRawData(data);
        }
    }
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
        Terraria.Utils.WriteRGB(bw, player.hairColor);
        Terraria.Utils.WriteRGB(bw, player.skinColor);
        Terraria.Utils.WriteRGB(bw, player.eyeColor);
        Terraria.Utils.WriteRGB(bw, player.shirtColor);
        Terraria.Utils.WriteRGB(bw, player.underShirtColor);
        Terraria.Utils.WriteRGB(bw, player.pantsColor);
        Terraria.Utils.WriteRGB(bw, player.shoeColor);
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
        SendRawData(ms.ToArray());
    }
    public static void SyncEquipment(byte index, short slot, short netID, short stack = 1, byte prefix = 0)
    {
        var sendData = new byte[PacketLength.SyncEquipment];
        Unsafe.As<byte, ushort>(ref sendData[0]) = (ushort)sendData.Length;
        sendData[2] = MessageID.SyncEquipment;
        sendData[3] = index;

        Unsafe.As<byte, short>(ref sendData[4]) = slot;
        Unsafe.As<byte, short>(ref sendData[6]) = stack;
        sendData[8] = prefix;
        Unsafe.As<byte, short>(ref sendData[9]) = netID;

        SendRawData(sendData);
    }
    public static void SyncEquipment(Player player, int slot) => SyncEquipment(player.whoAmI, slot, Utils.GetItemUseSlot(player, slot));
    public static void SyncEquipment(int index, int slot, Item item) => SyncEquipment((byte)index, (short)slot, (short)item.netID, (short)item.stack, item.prefix);
    public static void PlayerSpawn()
    {
        var sendData = new byte[PacketLength.PlayerSpawn];
        Unsafe.As<byte, ushort>(ref sendData[0]) = (ushort)sendData.Length;
        sendData[2] = MessageID.PlayerSpawn;
        sendData[3] = VirtualPlayer.VPlayerIndex;

        Unsafe.As<byte, short>(ref sendData[4]) = (short)Main.spawnTileX;
        Unsafe.As<byte, short>(ref sendData[6]) = (short)Main.spawnTileY;
        Unsafe.As<byte, int>(ref sendData[8]) = 0;
        Unsafe.As<byte, short>(ref sendData[12]) = 0;
        Unsafe.As<byte, short>(ref sendData[14]) = 0;
        sendData[16] = 0;
        SendRawData(sendData);
    }
    public static void PlayerControls(byte index, Vector2 position, Vector2 velocity, bool direction = false)
    {
        var sendData = new byte[25];
        Unsafe.As<byte, ushort>(ref sendData[0]) = (ushort)sendData.Length;
        sendData[2] = MessageID.PlayerControls;
        sendData[3] = index;

        sendData[4] = new BitsByte() {
            //[ControlFlag1.ControlRight] = true,
            [ControlFlag1.ControlUseItem] = VirtualPlayer.VPlayer.controlUseItem,
            //[ControlFlag1.Direction] = VirtualPlayer.VPlayer.direction == 1
            [ControlFlag1.Direction] = direction
        };
        sendData[5] = new BitsByte() { 
            [ControlFlag2.Velocity] = true,
            [ControlFlag2.GravDir] = VirtualPlayer.VPlayer.gravDir == 1 
        };
        sendData[6] = 0;
        sendData[7] = 0;

        sendData[8] = 0; //selectItem
        Unsafe.As<byte, float>(ref sendData[9]) = position.X;
        Unsafe.As<byte, float>(ref sendData[13]) = position.Y;
        Unsafe.As<byte, float>(ref sendData[17]) = velocity.X;
        Unsafe.As<byte, float>(ref sendData[21]) = velocity.Y;
        SendRawData(sendData);
    }
    public static void PlayerActive(byte index, bool active)
    {
        var sendData = new byte[PacketLength.PlayerActive];
        Unsafe.As<byte, ushort>(ref sendData[0]) = (ushort)sendData.Length;
        sendData[2] = MessageID.PlayerActive;
        sendData[3] = index;

        sendData[4] = active ? (byte)1 : (byte)0;
        SendRawData(sendData);
    }
    public static void PlayerActive(Player player) => PlayerActive((byte)player.whoAmI, player.active);
    public static void PlayerLifeMana(byte index, short statLife, short statLifeMax)
    {
        var sendData = new byte[PacketLength.PlayerLifeMana];
        Unsafe.As<byte, ushort>(ref sendData[0]) = (ushort)sendData.Length;
        sendData[2] = MessageID.PlayerLifeMana;
        sendData[3] = index;

        Unsafe.As<byte, short>(ref sendData[4]) = statLife;
        Unsafe.As<byte, short>(ref sendData[6]) = statLifeMax;
        SendRawData(sendData);
    }
    public static void PlayerLifeMana(Player player) => PlayerLifeMana((byte)player.whoAmI, (short)player.statLife, (short)player.statLifeMax);
    public static void ReleaseItemOwnership(short value)
    {
        var sendData = new byte[PacketLength.PlayerLifeMana];
        Unsafe.As<byte, ushort>(ref sendData[0]) = (ushort)sendData.Length;
        sendData[2] = MessageID.ReleaseItemOwnership;

        Unsafe.As<byte, short>(ref sendData[3]) = value;
        SendRawData(sendData);
    }
    public static void PlayerMana(byte index, short statMana, short statManaMax)
    {
        var sendData = new byte[PacketLength.PlayerMana];
        Unsafe.As<byte, ushort>(ref sendData[0]) = (ushort)sendData.Length;
        sendData[2] = MessageID2.PlayerMana;
        sendData[3] = index;

        Unsafe.As<byte, short>(ref sendData[4]) = statMana;
        Unsafe.As<byte, short>(ref sendData[6]) = statManaMax;
        SendRawData(sendData);
    }
    public static void PlayerMana(Player player) => PlayerMana((byte)player.whoAmI, (short)player.statMana, (short)player.statManaMax);
    public static void PlayerTeam(byte index, byte team)
    {
        var sendData = new byte[PacketLength.PlayerTeam];
        Unsafe.As<byte, ushort>(ref sendData[0]) = (ushort)sendData.Length;
        sendData[2] = MessageID2.PlayerTeam;
        sendData[3] = index;

        sendData[4] = team;
        SendRawData(sendData);
    }
    public static void PlayerTeam(Player player) => PlayerTeam((byte)player.whoAmI, (byte)player.team);
    public static void PlayerBuffs(byte index, int[] buffTypes)
    {
        var sendData = new byte[3 + 1 + Player.maxBuffs * sizeof(ushort)];
        var position = 0;
        Unsafe.As<byte, ushort>(ref sendData[position]) = (ushort)sendData.Length;
        position += 2;
        sendData[position++] = MessageID.PlayerBuffs;
        sendData[position++] = index;
        for (int i = 0; i < Player.maxBuffs; i++)
        {
            Unsafe.As<byte, ushort>(ref sendData[position]) = (ushort)buffTypes[i];
            position += 2;
        }
        SendRawData(sendData);
    }
    public static void PlayerBuffs(Player player) => PlayerBuffs((byte)player.whoAmI, player.buffType);
    public static void AnglerQuestFinished(byte index, int anglerQuestsFinished, int golferScoreAccumulated)
    {
        var sendData = new byte[PacketLength.AnglerQuestFinished];
        Unsafe.As<byte, ushort>(ref sendData[0]) = (ushort)sendData.Length;
        sendData[2] = MessageID.PlayerBuffs;
        sendData[3] = index;

        Unsafe.As<byte, int>(ref sendData[4]) = anglerQuestsFinished;
        Unsafe.As<byte, int>(ref sendData[8]) = golferScoreAccumulated;
    }
    public static void AnglerQuestFinished(Player player) => AnglerQuestFinished((byte)player.whoAmI, player.anglerQuestsFinished, player.golferScoreAccumulated);
    public static void SyncLoadout(byte index, byte loadout, ushort hideVisibleAccessory)
    {
        var sendData = new byte[PacketLength.SyncLoadout];
        Unsafe.As<byte, ushort>(ref sendData[0]) = (ushort)sendData.Length;
        sendData[2] = MessageID.SyncLoadout;
        sendData[3] = index;

        sendData[4] = loadout;
        Unsafe.As<byte, ushort>(ref sendData[5]) = hideVisibleAccessory;
        SendRawData(sendData);
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
}
public static class PacketLength
{
    public const ushort SyncEquipment = 11;
    public const ushort PlayerSpawn = 17;
    public const ushort PlayerActive = 5;
    public const ushort PlayerLifeMana = 8;
    public const ushort PlayerMana = 8;
    public const ushort PlayerTeam = 5;
    public const ushort AnglerQuestFinished = 12;
    public const ushort SyncLoadout = 7;
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