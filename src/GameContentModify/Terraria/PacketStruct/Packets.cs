using Terraria.ID;

namespace VBY.GameContentModify.PacketStruct;

[AttributeUsage(AttributeTargets.Struct)]
public class PacketInfoAttribute : Attribute
{
    public byte PacketID;
    public PacketType PacketType;
    public PacketInfoAttribute(byte packetID, PacketType packetType = PacketType.Unknown)
    {
        PacketID = packetID;
        PacketType = packetType;
    }
}
[Flags]
public enum PacketType : byte
{
    Unknown = 0,
    ServerSend = 1,
    ClientSend = 2,
    SerberForward = 4,
    ServerGet = ClientSend,
    ClientGet = ServerSend,
    All = ServerSend | ClientSend | SerberForward
}
[PacketInfo(MessageID.PlayerInfo, PacketType.ClientGet)]
public struct PlayerInfo
{
    public byte PlayerWhoAmI;
    public byte Value;
}
[PacketInfo(MessageID.SyncEquipment, PacketType.All)]
public struct SyncEquipment
{
    public byte PlayerWhoAmI;
    public short Slot;
    public short Stack;
    public byte Prefix;
    public short NetID;
}
[PacketInfo(MessageID.SpawnTileData, PacketType.ServerGet)]
public struct SpawnTileData
{
    public int SectionX;
    public int SectionY;
}
[PacketInfo(MessageID.TileFrameSection, PacketType.ClientGet)]
public struct TileFrameSection
{
    public short SectionStartX;
    public short SectionStartY;
    public short SectionEndXInclusive;
    public short SectionEndYInclusive;
}
[PacketInfo(MessageID.PlayerSpawn, PacketType.All)]
public struct PlayerSpawn
{
    public byte PlayerWhoAmI;
    public short SpawnX;
    public short SpawnY;
    public int RespawnTimer;
    public short NumberOfDeathsPVE;
    public short NumberOfDeathsPVP;
    public byte PlayerSpawnContext;
}