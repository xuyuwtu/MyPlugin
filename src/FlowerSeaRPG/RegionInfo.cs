using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;

using Newtonsoft.Json;

namespace FlowerSeaRPG;

public class ItemInfo
{
    public int Type, Stack;
    public byte Prefix;

    public ItemInfo(int type, int stack, byte prefix)
    {
        Type = type;
        Stack = stack;
        Prefix = prefix;
    }
}
public class TileInfo : ITile
{
    [JsonIgnore]
    int ITile.collisionType => throw new NotImplementedException();

    public ushort type { get; set; }
    public ushort wall { get; set; }
    public byte liquid { get; set; }
    public ushort sTileHeader { get; set; }
    public byte bTileHeader { get; set; }
    public byte bTileHeader2 { get; set; }
    public byte bTileHeader3 { get; set; }
    public short frameX { get; set; }
    public short frameY { get; set; }
    public TileInfo(ITile copy)
    {
        if (copy == null)
        {
            type = 0;
            wall = 0;
            liquid = 0;
            sTileHeader = 0;
            bTileHeader = 0;
            bTileHeader2 = 0;
            bTileHeader3 = 0;
            frameX = 0;
            frameY = 0;
        }
        else
        {
            type = copy.type;
            wall = copy.wall;
            liquid = copy.liquid;
            sTileHeader = copy.sTileHeader;
            bTileHeader = copy.bTileHeader;
            bTileHeader2 = copy.bTileHeader2;
            bTileHeader3 = copy.bTileHeader3;
            frameX = copy.frameX;
            frameY = copy.frameY;
        }
    }
    Color ITile.actColor(Color oldColor)
    {
        throw new NotImplementedException();
    }

    void ITile.actColor(ref Vector3 oldColor)
    {
        throw new NotImplementedException();
    }

    bool ITile.active()
    {
        throw new NotImplementedException();
    }

    void ITile.active(bool active)
    {
        throw new NotImplementedException();
    }

    bool ITile.actuator()
    {
        throw new NotImplementedException();
    }

    void ITile.actuator(bool actuator)
    {
        throw new NotImplementedException();
    }

    TileColorCache ITile.BlockColorAndCoating()
    {
        throw new NotImplementedException();
    }

    int ITile.blockType()
    {
        throw new NotImplementedException();
    }

    bool ITile.bottomSlope()
    {
        throw new NotImplementedException();
    }

    bool ITile.checkingLiquid()
    {
        throw new NotImplementedException();
    }

    void ITile.checkingLiquid(bool checkingLiquid)
    {
        throw new NotImplementedException();
    }

    void ITile.Clear(TileDataType types)
    {
        throw new NotImplementedException();
    }

    void ITile.ClearBlockPaintAndCoating()
    {
        throw new NotImplementedException();
    }

    void ITile.ClearEverything()
    {
        throw new NotImplementedException();
    }

    void ITile.ClearMetadata()
    {
        throw new NotImplementedException();
    }

    void ITile.ClearTile()
    {
        throw new NotImplementedException();
    }

    void ITile.ClearWallPaintAndCoating()
    {
        throw new NotImplementedException();
    }

    object ITile.Clone()
    {
        throw new NotImplementedException();
    }

    byte ITile.color()
    {
        throw new NotImplementedException();
    }

    void ITile.color(byte color)
    {
        throw new NotImplementedException();
    }

    void ITile.CopyFrom(ITile from)
    {
        throw new NotImplementedException();
    }

    void ITile.CopyPaintAndCoating(ITile other)
    {
        throw new NotImplementedException();
    }

    byte ITile.frameNumber()
    {
        throw new NotImplementedException();
    }

    void ITile.frameNumber(byte frameNumber)
    {
        throw new NotImplementedException();
    }

    bool ITile.fullbrightBlock()
    {
        throw new NotImplementedException();
    }

    void ITile.fullbrightBlock(bool fullbrightBlock)
    {
        throw new NotImplementedException();
    }

    bool ITile.fullbrightWall()
    {
        throw new NotImplementedException();
    }

    void ITile.fullbrightWall(bool fullbrightWall)
    {
        throw new NotImplementedException();
    }

    bool ITile.halfBrick()
    {
        throw new NotImplementedException();
    }

    void ITile.halfBrick(bool halfBrick)
    {
        throw new NotImplementedException();
    }

    bool ITile.HasSameSlope(ITile tile)
    {
        throw new NotImplementedException();
    }

    bool ITile.honey()
    {
        throw new NotImplementedException();
    }

    void ITile.honey(bool honey)
    {
        throw new NotImplementedException();
    }

    bool ITile.inActive()
    {
        throw new NotImplementedException();
    }

    void ITile.inActive(bool inActive)
    {
        throw new NotImplementedException();
    }

    bool ITile.invisibleBlock()
    {
        throw new NotImplementedException();
    }

    void ITile.invisibleBlock(bool invisibleBlock)
    {
        throw new NotImplementedException();
    }

    bool ITile.invisibleWall()
    {
        throw new NotImplementedException();
    }

    void ITile.invisibleWall(bool invisibleWall)
    {
        throw new NotImplementedException();
    }

    bool ITile.isTheSameAs(ITile compTile)
    {
        throw new NotImplementedException();
    }

    bool ITile.lava()
    {
        throw new NotImplementedException();
    }

    void ITile.lava(bool lava)
    {
        throw new NotImplementedException();
    }

    bool ITile.leftSlope()
    {
        throw new NotImplementedException();
    }

    void ITile.liquidType(int liquidType)
    {
        throw new NotImplementedException();
    }

    byte ITile.liquidType()
    {
        throw new NotImplementedException();
    }

    bool ITile.nactive()
    {
        throw new NotImplementedException();
    }

    void ITile.ResetToType(ushort type)
    {
        throw new NotImplementedException();
    }

    bool ITile.rightSlope()
    {
        throw new NotImplementedException();
    }

    bool ITile.shimmer()
    {
        throw new NotImplementedException();
    }

    void ITile.shimmer(bool shimmer)
    {
        throw new NotImplementedException();
    }

    bool ITile.skipLiquid()
    {
        throw new NotImplementedException();
    }

    void ITile.skipLiquid(bool skipLiquid)
    {
        throw new NotImplementedException();
    }

    byte ITile.slope()
    {
        throw new NotImplementedException();
    }

    void ITile.slope(byte slope)
    {
        throw new NotImplementedException();
    }

    bool ITile.topSlope()
    {
        throw new NotImplementedException();
    }

    void ITile.UseBlockColors(TileColorCache cache)
    {
        throw new NotImplementedException();
    }

    void ITile.UseWallColors(TileColorCache cache)
    {
        throw new NotImplementedException();
    }

    byte ITile.wallColor()
    {
        throw new NotImplementedException();
    }

    void ITile.wallColor(byte wallColor)
    {
        throw new NotImplementedException();
    }

    TileColorCache ITile.WallColorAndCoating()
    {
        throw new NotImplementedException();
    }

    byte ITile.wallFrameNumber()
    {
        throw new NotImplementedException();
    }

    void ITile.wallFrameNumber(byte wallFrameNumber)
    {
        throw new NotImplementedException();
    }

    int ITile.wallFrameX()
    {
        throw new NotImplementedException();
    }

    void ITile.wallFrameX(int wallFrameX)
    {
        throw new NotImplementedException();
    }

    int ITile.wallFrameY()
    {
        throw new NotImplementedException();
    }

    void ITile.wallFrameY(int wallFrameY)
    {
        throw new NotImplementedException();
    }

    bool ITile.wire()
    {
        throw new NotImplementedException();
    }

    void ITile.wire(bool wire)
    {
        throw new NotImplementedException();
    }

    bool ITile.wire2()
    {
        throw new NotImplementedException();
    }

    void ITile.wire2(bool wire2)
    {
        throw new NotImplementedException();
    }

    bool ITile.wire3()
    {
        throw new NotImplementedException();
    }

    void ITile.wire3(bool wire3)
    {
        throw new NotImplementedException();
    }

    bool ITile.wire4()
    {
        throw new NotImplementedException();
    }

    void ITile.wire4(bool wire4)
    {
        throw new NotImplementedException();
    }
    public override string ToString()
    {
        return "Tile Type:" + type + " Active:" + " Wall:" + wall + " fX:" + frameX + " fY:" + frameY;
    }
}
public class SignInfo
{
    public int X, Y;
    public string Text;

    public SignInfo(int x, int y, string text)
    {
        X = x;
        Y = y;
        Text = text;
    }
}
public class ChestInfo
{
    public int X, Y;
    public string Name;
    public ItemInfo[] Items = new ItemInfo[40];

    public ChestInfo(int x, int y, string name, ItemInfo[] items)
    {
        X = x;
        Y = y;
        Name = name;
        Items = items;
    }
}
public class RegionInfo
{
    public int Width;
    public int Height;
    public List<SpecialPoint> SpecialPoints = new();
    public TileInfo[] TileInfos;
    public List<SignInfo> SignInfos = new();
    public List<ChestInfo> ChestInfos = new();

    public RegionInfo(int width, int height)
    {
        Width = width;
        Height = height;
        TileInfos = new TileInfo[width * height];
    }
}
public class SpecialPoint
{
    public string Name;
    public Point Point;

    public SpecialPoint(string name, Point point)
    {
        Name = name;
        Point = point;
    }
}