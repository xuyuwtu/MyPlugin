namespace Terraria.ID;

public class ClientStateID
{
    public const int RequirePassword = -1;

    /// <summary>
    /// hasPassword
    /// -> <see cref="RequirePassword"/>
    /// else 
    /// -> <see cref="SendMessagePlayerInfo"/>
    /// </summary>
    public const int WaitMessageHello = 0;

    public const int SendMessagePlayerInfo = 1;
    public const int WaitMessageWorldDataRequest = 1;

    public const int ReceivedMessageWorldDataRequest = 2;
    public const int SendingWorldData = 2;
    public const int WaitMessageSpawnTileData = 2;

    public const int SendingSpawnTileData = 3;
    public const int SendMessageInitialSpawn = 3;
    public const int WaitMessagePlayerSpawn = 3;

    public const int Normal = 10;
}
