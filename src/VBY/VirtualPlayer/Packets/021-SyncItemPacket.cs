namespace VBY.VirtualPlayer.Packets;


[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal class SyncItemPacket : Packet
{
    public short ItemIndex;
    public Vector2 Position;
    public Vector2 Vecloty;
    public short Stack;
    public byte Prefix;
    public byte CanDrop;
    public short ItemID;
    public SyncItemPacket() : base(MessageID.SyncItem) { }
    protected SyncItemPacket(byte messageID) : base(messageID) { }
}