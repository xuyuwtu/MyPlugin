using System.Runtime.CompilerServices;

using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace VBY.GameContentModify
{
    public static class TileUtils
    {
        public static int GetStyleID(ITile tile)
        {
            if(tile.type == TileID.MetalBars)
            {
                return tile.frameX / 18;
            }
            if (tile.type is TileID.Torches or TileID.Candles or TileID.BeachPiles)
            {
                return tile.frameY / 22;
            }
            throw new NotSupportedException($"not support type:{tile.type}");
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Ensure(int x, int y)
        {
            if(Main.tile[x, y] is null)
            {
                Main.tile[x, y] = OTAPI.Hooks.Tile.InvokeCreate();
            }
        }
        public static void RandomlyMoveAdjacent(UnifiedRandom random, ref int x, ref int y)
        {
            switch (random.Next(4))
            {
                case 0:
                    x--;
                    break;
                case 1:
                    x++;
                    break;
                case 2:
                    y--;
                    break;
                case 3:
                    y++;
                    break;
            }
        }
    }
}
