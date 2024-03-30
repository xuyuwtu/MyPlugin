namespace Terraria.Net.Packets;

sealed partial class PlayerSpawnPacket
{
    public byte WhoAmi;
    public short SpawnX;
    public short SpawnY;
    public int RespawnTimer;
    public short NumberOfDeathsPVP;
    public short NumberOfDeathsPVE;
    public byte SpawnContext;
}
