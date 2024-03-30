using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

using Terraria;

namespace VBY.GameContentModify.PacketStructs;

[AttributeUsage(AttributeTargets.Struct)]
public class PacketInfoAttribute : Attribute
{
    public byte PacketID;
    public PacketInfoAttribute(byte packetID)
    {
        PacketID = packetID;
    }
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ShortPosition
{
    public short X;
    public short Y;
}
public interface IMessageType
{
    byte MessageType { get; }
}
partial struct Hello
{
    public string Version;
}
partial struct PlayerInfo
{
    public byte PlayerSlot;
    public bool Value;
}
partial struct SyncEquipment 
{
    public byte PlayerSlot;
    public short Slot;
    public short Stack;
    public byte Prefix;
    public short NetID;
}
partial struct SpawnTileData 
{
    public int SectionX;
    public int SectionY;
}
partial struct TileFrameSection 
{
    public short SectionStartX;
    public short SectionStartY;
    public short SectionEndXInclusive;
    public short SectionEndYInclusive;
}
partial struct PlayerSpawn 
{
    public byte PlayerSlot;
    public ShortPosition Position;
    public int RespawnTimer;
    public short NumberOfDeathsPVE;
    public short NumberOfDeathsPVP;
    public byte PlayerSpawnContext;
}
partial struct PlayerControls 
{
    public byte PlayerSlot;
    public BitsByte Flag1;
    public BitsByte Flag2;
    public BitsByte Flag3;
    public BitsByte Flag4;
    public byte SelectedItem;
    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 PotionOfReturnOriginalUsePosition;
    public Vector2 PotionOfReturnHomePosition;
}
partial struct PlayerActive 
{
    public byte PlayerSlot;
    public byte Active;
}
partial struct PlayerLifeMana
{
    public byte PlayerSlot;
    public short StatLife;
    public short StatLifeMax;
}
partial struct TileManipulation
{
    public byte Action;
    public ShortPosition Position;
    public short TileType;
    public byte Style;
}
partial struct SetTime
{
    public byte DayTime;
    public int Time;
    public short SunModY;
    public short MoonModY;
}
partial struct ToggleDoorState
{
    public byte ChangeType;
    public ShortPosition Position;
    public byte Direction;
}
partial struct SyncItem
{
    public short ItemSlot;
    public Vector2 Position;
    public Vector2 Velocity;
    public short Stack;
    public byte Prefix;
    public byte Owner;
    public short ItemType;
}
partial struct ItemOwner
{
    public short ItemSlot;
    public byte PlayerIndex;
}