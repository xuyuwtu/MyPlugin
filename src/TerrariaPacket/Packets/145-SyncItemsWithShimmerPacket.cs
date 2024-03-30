namespace Terraria.Net.Packets;

sealed partial class SyncItemsWithShimmerPacket : SyncItemPacket
{
    public bool Shimmer;
    public float ShimmerTime;
}