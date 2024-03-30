namespace Terraria.Net.Packets;
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial class HelloPacket : Packet
{
	public HelloPacket() : base(MessageID.Hello) {}
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial class PlayerInfoPacket : Packet
{
	public PlayerInfoPacket() : base(MessageID.PlayerInfo) {}
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial class SyncPlayerPacket : Packet
{
	public SyncPlayerPacket() : base(MessageID.SyncPlayer) {}
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial class SyncEquipmentPacket : Packet
{
	public SyncEquipmentPacket() : base(MessageID.SyncEquipment) {}
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial class SyncItemPacket : Packet
{
	public SyncItemPacket() : base(MessageID.SyncItem) {}
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial class SyncItemsWithShimmerPacket : SyncItemPacket
{
	public SyncItemsWithShimmerPacket() : base(MessageID.SyncItemsWithShimmer) {}
}[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial class SyncItemCannotBeTakenByEnemiesPacket : SyncItemPacket
{
	public SyncItemCannotBeTakenByEnemiesPacket() : base(MessageID.SyncItemCannotBeTakenByEnemies) {}
}[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial class SpawnTileDataPacket : Packet
{
	public SpawnTileDataPacket() : base(MessageID.SpawnTileData) {}
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial class TileFrameSectionPacket : Packet
{
	public TileFrameSectionPacket() : base(MessageID.TileFrameSection) {}
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial class PlayerSpawnPacket : Packet
{
	public PlayerSpawnPacket() : base(MessageID.PlayerSpawn) {}
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial class PlayerActivePacket : Packet
{
	public PlayerActivePacket() : base(MessageID.PlayerActive) {}
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial class PlayerLifeManaPacket : Packet
{
	public PlayerLifeManaPacket() : base(MessageID.PlayerLifeMana) {}
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial class TileManipulationPacket : Packet
{
	public TileManipulationPacket() : base(MessageID.TileManipulation) {}
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial class SetTimePacket : Packet
{
	public SetTimePacket() : base(MessageID.SetTime) {}
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial class ToggleDoorStatePacket : Packet
{
	public ToggleDoorStatePacket() : base(MessageID.ToggleDoorState) {}
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial class DamageNPCPacket : Packet
{
	public DamageNPCPacket() : base(MessageID.DamageNPC) {}
}
