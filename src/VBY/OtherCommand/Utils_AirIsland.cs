using Terraria.DataStructures;
using Terraria.ID;

namespace VBY.OtherCommand;

partial class Utils
{
    public static class AirIsland
    {
        private static List<int> DungeonTileIDs = new()
        {
            TileID.BlueDungeonBrick, 
            TileID.GreenDungeonBrick, 
            TileID.PinkDungeonBrick
        };
        private static List<int> DungeonWallIDs = new()
        {
            WallID.BlueDungeonUnsafe,
            WallID.BlueDungeonSlabUnsafe,
            WallID.BlueDungeonTileUnsafe,
            WallID.PinkDungeonUnsafe,
            WallID.PinkDungeonSlabUnsafe,
            WallID.PinkDungeonTileUnsafe,
            WallID.GreenDungeonUnsafe,
            WallID.GreenDungeonSlabUnsafe,
            WallID.GreenDungeonTileUnsafe
        };
        private static int JungleTempleTileID = TileID.LihzahrdBrick;
        private static int JungleTempleWallID = WallID.LihzahrdBrickUnsafe;
        private static List<int> JungleShrineTileIDs = new()
        {
            TileID.TinBrick,
            TileID.IridescentBrick,
            TileID.GoldBrick,
        };
        private static List<int> JungleShrineWallIDs = new()
        {
            WallID.TinBrick,
            WallID.IridescentBrick,
            WallID.GoldBrick,
        };
        private static List<int> LifeTreeTileIDs = new()
        {
            TileID.LivingWood,
            TileID.LeafBlock,
        };
        private static List<int> LifeTreeWallIDs = new()
        {
            WallID.LivingWoodUnsafe
        };
        private static List<int> RedLifeTreeTileIDs = new()
        {
            TileID.LivingMahogany,
            TileID.LivingMahoganyLeaves
        };
        private static List<int> RedLifeTreeWallIDs = new()
        {
            WallID.LivingWood
        };
        private static int PyramidTileID = TileID.SandstoneBrick;
        private static int PyramidWallID = WallID.SandstoneBrick;
        public static List<ITileClearInfo> TileClearInfos = new()
        {
            new MoreClearInfo(DungeonTileIDs, DungeonWallIDs),
            new OneClearInfo(JungleTempleTileID, JungleTempleWallID),
            new MoreClearInfo(JungleShrineTileIDs, JungleShrineWallIDs, true, TileDataType.Tile),
            new MoreClearInfo(LifeTreeTileIDs, LifeTreeWallIDs),
            new MoreClearInfo(RedLifeTreeTileIDs, RedLifeTreeWallIDs),
            new OneClearInfo(PyramidTileID, PyramidWallID)
        };
        public static List<int> SkipWallIDs = new()
        {
            WallID.SpiderUnsafe,
            WallID.Marble,
            WallID.MarbleUnsafe,
            WallID.Granite,
            WallID.GraniteUnsafe,
            WallID.HiveUnsafe
        };
    }
}
