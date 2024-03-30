namespace VBY.VirtualPlayer.Packets;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal sealed class SyncItemCannotBeTakenByEnemiesPacket : SyncItemPacket
{
    public byte TimeLeftInWhichTheItemCannotBeTakenByEnemies;
    public SyncItemCannotBeTakenByEnemiesPacket() : base(MessageID.SyncItemCannotBeTakenByEnemies) { }
}
