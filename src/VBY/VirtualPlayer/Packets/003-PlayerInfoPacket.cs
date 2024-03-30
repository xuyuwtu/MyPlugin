namespace VBY.VirtualPlayer.Packets;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal class PlayerInfoPacket : Packet
{
    public byte WhoAmi;
    public bool Flag;
    public PlayerInfoPacket() : base(MessageID.PlayerInfo) { }
}
