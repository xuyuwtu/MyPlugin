namespace Terraria.ID;

public class ConnectStateID
{
    public const int FindingServer = 0;

    public const int FoundServer = 1;
    public const int SendMessageHello = 1;
    public const int WaitMessagePlayerInfo = 1;

    public const int ReceivedMessagePlayerInfo = 2;
    public const int SendingPlayerData = 2;
    public const int SendMessageRequestWorldData = 2;

    //public const int SentPlayerData = 3;
    public const int RecievingWorldInformation = 3;

    public const int ReceivedWorldInformation = 4;

    public const int ClearingWorld = 5;
    public const int LoadMap = 5;

    public const int ClearedWorld = 6;
    public const int SendMessageSpawnTileData = 6;
    public const int RecievingTileData = 6;
    public const int WaitMessageInitialSpawn = 6;
    public const int SendMessagePlayerSpawn = 6;

    public const int Normal = 10;
}
