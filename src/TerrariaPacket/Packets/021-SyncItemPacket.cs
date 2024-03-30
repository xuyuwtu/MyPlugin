namespace Terraria.Net.Packets;

partial class SyncItemPacket
{
    public short WhoAmi;
    public Vector2 Position;
    public Vector2 Vecloty;
    public short Stack;
    public byte Prefix;
    public byte CanDrop;
    public short ItemID;
    protected SyncItemPacket(byte messageID) : base(messageID) { }
}