using Terraria.DataStructures;
namespace VBY.OtherCommand;

public interface ITileClearInfo
{
    public bool TileIDContains(int tileID);
    public bool WallIDContains(int wallID);
    public bool ClearWall { get; }
    public TileDataType ClearType { get; }
}

public class OneClearInfo : ITileClearInfo
{
    public int TileID, WallID;
    public TileDataType ClearType { get; }
    public OneClearInfo(int tileID, int wallID, bool clearWall = true, TileDataType clearType = 0)
    {
        TileID = tileID;
        WallID = wallID;
        ClearWall = clearWall;
        ClearType = clearType;
    }

    public bool ClearWall { get; }

    public bool TileIDContains(int tileID) => TileID == tileID;

    public bool WallIDContains(int tileID) => WallID == tileID;
}
public class MoreClearInfo : ITileClearInfo
{
    public List<int> TileIDs, WallIDs;
    public TileDataType ClearType { get; }
    public MoreClearInfo(List<int> tileIDs, List<int> wallIDs, bool clearWall = true, TileDataType clearType = 0)
    {
        TileIDs = tileIDs;
        WallIDs = wallIDs;
        ClearWall = clearWall;
        ClearType = clearType;
    }

    public bool ClearWall { get; }

    public bool TileIDContains(int tileID) => TileIDs.Contains(tileID);

    public bool WallIDContains(int wallID) => WallIDs.Contains(wallID);
}