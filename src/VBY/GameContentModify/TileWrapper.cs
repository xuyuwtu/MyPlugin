using System.Runtime.CompilerServices;

using Terraria;
using Terraria.ID;

namespace VBY.GameContentModify;

#pragma warning disable IDE1006 // 命名样式
public readonly struct TileWrapper
{
    public readonly int x;
    public readonly int y;
    public readonly ITile tile;
    public TileWrapper(int x, int y)
    {
        this.x = x;
        this.y = y;
        tile = Main.tile[x, y];
    }
    public bool active
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => tile.active();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => tile.active(value);
    }
    public bool nactive
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => tile.nactive();
    }
    public ushort type
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => tile.type;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => tile.type = value;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool InWorld(int fluff) => WorldGen.InWorld(x, y, fluff);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SquareTileFrame(bool resetFrame = true) => WorldGen.SquareTileFrame(x, y, resetFrame);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SendTileSquare(int whoAmi = -1, TileChangeType changeType = TileChangeType.None) => NetMessage.SendTileSquare(whoAmi, x, y, changeType);

    public TileWrapper Above => new(x, y - 1);
    public TileWrapper Below => new(x, y + 1);
    public TileWrapper Left => new(x - 1, y);
    public TileWrapper Right => new(x + 1, y);
    public bool IsSolid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Main.tileSolid[type];
    }
    public bool IsBrick
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Main.tileSolid[type];
    }
    public bool IsHousingWalls
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => TileID.Sets.HousingWalls[type];
    }
    public bool WallIsHouse
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Main.wallHouse[tile.wall];
    }
}