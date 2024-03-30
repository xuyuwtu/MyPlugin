namespace VBY.VirtualPlayer.Packets;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal sealed class SyncEquipmentPacket : Packet
{
    public byte WhoAmi;
    public short Slot;
    public short Stack;
    public byte Prefix;
    public short ItemID;
    public SyncEquipmentPacket() : base(MessageID.SyncEquipment) { }
}
