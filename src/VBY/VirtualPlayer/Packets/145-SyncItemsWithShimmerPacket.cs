namespace VBY.VirtualPlayer.Packets;


[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal sealed class SyncItemsWithShimmerPacket : SyncItemPacket
{
    public bool Shimmer;
    public float ShimmerTime;
    public SyncItemsWithShimmerPacket() : base(MessageID.SyncItemsWithShimmer) { }
}