using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.Localization;

using VBY.GameContentModify.Config;

using TUtils = Terraria.Utils;

namespace VBY.GameContentModify;

[ReplaceType(typeof(WorldGen))]
public static class ReplaceWorldGen
{
    public static short[] SingleTileDropItem;
    public static int[] HasStyleDropIndices;
    public static Dictionary<int, int>[] StyleDrop;
    static ReplaceWorldGen()
    {
        SingleTileDropItem = new short[ItemID.Count];
        Array.Fill<short>(SingleTileDropItem, -1);
        var ids = new List<int>();
        var styles = new List<Dictionary<int, int>>();
        RegisterStyleList(ids, styles, TileID.Torches,
            ItemID.Torch,
            ItemID.BlueTorch,
            ItemID.RedTorch,
            ItemID.GreenTorch,
            ItemID.PurpleTorch,
            ItemID.WhiteTorch,
            ItemID.YellowTorch,
            ItemID.DemonTorch,
            ItemID.CursedTorch,
            ItemID.IceTorch,
            ItemID.OrangeTorch,
            ItemID.IchorTorch,
            ItemID.UltrabrightTorch,
            ItemID.BoneTorch,
            ItemID.RainbowTorch,
            ItemID.PinkTorch,
            ItemID.DesertTorch,
            ItemID.CoralTorch,
            ItemID.CorruptTorch,
            ItemID.CrimsonTorch,
            ItemID.HallowedTorch,
            ItemID.JungleTorch,
            ItemID.MushroomTorch,
            ItemID.ShimmerTorch
        );
        RegisterStyleList(ids, styles, TileID.MetalBars,
            ItemID.CopperBar,
            ItemID.TinBar,
            ItemID.IronBar,
            ItemID.LeadBar,
            ItemID.SilverBar,
            ItemID.TungstenBar,
            ItemID.GoldBar,
            ItemID.PlatinumBar,
            ItemID.DemoniteBar,
            ItemID.MeteoriteBar,
            ItemID.HellstoneBar,
            ItemID.CobaltBar,
            ItemID.PalladiumBar,
            ItemID.MythrilBar,
            ItemID.OrichalcumBar,
            ItemID.AdamantiteBar,
            ItemID.TitaniumBar,
            ItemID.ChlorophyteBar,
            ItemID.HallowedBar,
            ItemID.CrimtaneBar,
            ItemID.ShroomiteBar,
            ItemID.SpectreBar,
            ItemID.LunarBar
        );
        HasStyleDropIndices = ids.ToArray();
        StyleDrop = styles.ToArray();
        TileDropItemInit();

        var canPurifiedTileIDs = new HashSet<ushort>();
        var purifyTable = new List<ushort>();
        RegisterPurifyTable(canPurifiedTileIDs, purifyTable, TileID.JungleGrass, [TileID.CorruptJungleGrass, TileID.CrimsonJungleGrass, TileID.CorruptGrass, TileID.CrimsonGrass, TileID.Grass, TileID.GolfGrass, TileID.GolfGrassHallowed, TileID.HallowedGrass]);
        RegisterPurifyTable(canPurifiedTileIDs, purifyTable, TileID.Mud, [TileID.Dirt]);
        RegisterPurifyTable(canPurifiedTileIDs, purifyTable, TileID.Stone, [TileID.Ebonstone, TileID.Crimstone]);
        RegisterPurifyTable(canPurifiedTileIDs, purifyTable, TileID.Sand, [TileID.Ebonsand, TileID.Crimsand]);
        RegisterPurifyTable(canPurifiedTileIDs, purifyTable, TileID.HardenedSand, [TileID.CorruptHardenedSand, TileID.CrimsonHardenedSand]);
        RegisterPurifyTable(canPurifiedTileIDs, purifyTable, TileID.Sandstone, [TileID.CorruptSandstone, TileID.CrimsonSandstone]);
        CanPurifiedTileIDs = [.. canPurifiedTileIDs];
        Array.Sort(CanPurifiedTileIDs);
        PurifyTable = [.. purifyTable];
        static void RegisterPurifyTable(HashSet<ushort> canPurifiedTileIDs, List<ushort> purifyTable, ushort target, params ushort[] sources)
        {
            foreach (var source in sources) 
            {
                canPurifiedTileIDs.Add(source);
            }
            for (int i = 0; i < sources.Length; i += 2)
            {
                purifyTable.Add(sources[i]);
                purifyTable.Add(target);
            }
        }
    }
    private static void TileDropItemInit()
    {
        RegisterSingleTable(
            668, 5400,
            659, 5349,
            667, 5398,
            633, 172,
            426, 3621,
            430, 3633,
            431, 3634,
            432, 3635,
            433, 3636,
            434, 3637,
            427, 3622,
            435, 3638,
            436, 3639,
            437, 3640,
            438, 3641,
            439, 3642,
            446, 3736,
            447, 3737,
            448, 3738,
            449, 3739,
            450, 3740,
            451, 3741,
            368, 3086,
            369, 3087,
            367, 3081,
            379, 3214,
            353, 2996,
            365, 3077,
            366, 3078,
            357, 3066,
            1, 3,
            442, 3707,
            383, 620,
            315, 2435,
            641, 5306,
            330, 71,
            331, 72,
            332, 73,
            333, 74,
            408, 3460,
            409, 3461,
            669, 5401,
            670, 5402,
            671, 5403,
            672, 5404,
            673, 5405,
            674, 5406,
            675, 5407,
            676, 5408,
            677, 5417,
            678, 5419,
            679, 5421,
            680, 5423,
            681, 5425,
            682, 5427,
            683, 5433,
            684, 5435,
            685, 5429,
            686, 5431,
            687, 5439,
            688, 5440,
            689, 5441,
            690, 5442,
            691, 5443,
            692, 5444,
            666, 5395,
            415, 3573,
            416, 3574,
            417, 3575,
            418, 3576,
            421, 3609,
            422, 3610,
            498, 4139,
            424, 3616,
            445, 3725,
            429, 3629,
            272, 1344,
            273, 2119,
            274, 2120,
            618, 4962,
            460, 3756,
            541, 4392,
            630, 5137,
            631, 5138,
            472, 3951,
            473, 3953,
            474, 3955,
            478, 4050,
            479, 4051,
            496, 4091,
            495, 4090,
            346, 2792,
            347, 2793,
            348, 2794,
            350, 2860,
            336, 2701,
            340, 2751,
            341, 2752,
            342, 2753,
            343, 2754,
            344, 2755,
            351, 2868,
            500, 4229,
            501, 4230,
            502, 4231,
            503, 4232,
            561, 4554,
            574, 4717,
            575, 4718,
            576, 4719,
            577, 4720,
            578, 4721,
            562, 4564,
            563, 4547,
            251, 1725,
            252, 1727,
            253, 1729,
            325, 2692,
            370, 3100,
            396, 3271,
            400, 3276,
            401, 3277,
            403, 3339,
            397, 3272,
            398, 3274,
            399, 3275,
            402, 3338,
            404, 3347,
            407, 3380,
            579, 4761,
            593, 4868,
            624, 5114,
            656, 5333,
            170, 1872,
            284, 2173,
            214, 85,
            213, 965,
            211, 947,
            6, 11,
            7, 12,
            8, 13,
            9, 14,
            202, 824,
            234, 1246,
            226, 1101,
            224, 1103,
            36, 1869,
            311, 2260,
            312, 2261,
            313, 2262,
            229, 1125,
            230, 1127,
            221, 1104,
            222, 1105,
            223, 1106,
            248, 1589,
            249, 1591,
            250, 1593,
            191, 9,
            203, 836,
            204, 880,
            166, 699,
            167, 700,
            168, 701,
            169, 702,
            123, 424,
            124, 480,
            157, 619,
            158, 620,
            159, 621,
            161, 664,
            206, 883,
            232, 1150,
            198, 775,
            189, 751,
            195, 763,
            194, 154,
            193, 762,
            196, 765,
            197, 767,
            22, 56,
            140, 577,
            23, 2,
            25, 61,
            30, 9,
            208, 911,
            372, 3117,
            646, 5322,
            371, 3113,
            174, 713,
            37, 116,
            38, 129,
            39, 131,
            40, 133,
            41, 134,
            43, 137,
            44, 139,
            45, 141,
            46, 143,
            47, 145,
            48, 147,
            49, 148,
            51, 150,
            53, 169,
            151, 607,
            152, 609,
            56, 173,
            57, 172,
            58, 174,
            70, 176,
            75, 192,
            76, 214,
            78, 222,
            81, 275,
            80, 276,
            188, 276,
            107, 364,
            108, 365,
            111, 366,
            150, 604,
            112, 370,
            116, 408,
            117, 409,
            118, 412,
            119, 413,
            120, 414,
            121, 415,
            122, 416,
            136, 538,
            385, 3234,
            141, 580,
            145, 586,
            146, 591,
            147, 593,
            148, 594,
            153, 611,
            154, 612,
            155, 613,
            156, 614,
            160, 662,
            175, 717,
            176, 718,
            177, 719,
            163, 833,
            164, 834,
            200, 835,
            210, 937,
            130, 511,
            131, 512,
            321, 2503,
            322, 2504,
            635, 5215,
            54, 170,
            326, 2693,
            327, 2694,
            458, 3754,
            459, 3755,
            345, 2787,
            328, 2695,
            329, 2697,
            507, 4277,
            508, 4278,
            190, 183,
            566, 999,
            476, 4040,
            494, 4089,
            520, 4326
        );
        RegisterSingleTarget(ItemID.StoneBlock, 179, 180, 181, 182, 183, 381, 534, 536, 539, 625, 627);
        RegisterSingleTarget(ItemID.GrayBrick, 512, 513, 514, 515, 516, 517, 535, 537, 540, 626, 628);
        RegisterSingleTarget(ItemID.DirtBlock, 0, 2, 109, 199, 477, 492);
        RegisterSingleTarget(ItemID.Grate, 546, 557);
        RegisterSingleTarget(ItemID.MudBlock, 59, 60, 661, 662);
        RegisterSingleRange(255, 261, 1970);
        RegisterSingleRange(262, 268, 1970);
        RegisterSingleRange(63, 68, 177);
    }
    private static void RegisterSingleTable(params short[] table)
    {
        if (table.Length % 2 != 0)
        {
            throw new ArgumentException("table.Length % 2 != 0");
        }
        for (int i = 0; i < table.Length; i += 2)
        {
            SingleTileDropItem[table[i]] = table[i + 1];
        }
    }
    private static void RegisterSingleTarget(short dropItemID, params short[] sources)
    {
        for (int i = 0; i < sources.Length; i++)
        {
            SingleTileDropItem[sources[i]] = dropItemID;
        }
    }
    private static void RegisterSingleRange(short start, short end, short sourceStart)
    {
        if (end < start)
        {
            throw new ArgumentException("end < start");
        }
        var count = end - start + 1;
        for (int i = 0; i < count; i++)
        {
            SingleTileDropItem[start + i] = (short)(sourceStart + i);
        }
    }
    private static void RegisterStyleList(List<int> ids, List<Dictionary<int, int>> styles, ushort tileID, params int[] itemIDs)
    {
        ids.Add(tileID);
        var dict = new Dictionary<int, int>();
        for (int i = 0; i < itemIDs.Length; i++)
        {
            dict.Add(i, itemIDs[i]);
        }
        styles.Add(dict);
    }
    [DetourMethod]
    public static void UpdateWorld()
    {
        if (WorldGen.gen)
        {
            return;
        }
        WorldGen.AllowedToSpreadInfections = true;
        CreativePowers.StopBiomeSpreadPower power = CreativePowerManager.Instance.GetPower<CreativePowers.StopBiomeSpreadPower>();
        if (power != null && power.GetIsUnlocked())
        {
            WorldGen.AllowedToSpreadInfections = !power.Enabled;
        }
        int wallDist = 3;
        Wiring.UpdateMech();
        TileEntity.UpdateStart();
        foreach (TileEntity value in TileEntity.ByID.Values)
        {
            value.Update();
        }
        TileEntity.UpdateEnd();
        WorldGen.UpdateLunarApocalypse();

        WorldGen.totalD++;
        if (WorldGen.totalD >= 30)
        {
            WorldGen.totalD = 0;
            WorldGen.CountTiles(WorldGen.totalX);
            WorldGen.totalX++;
            if (WorldGen.totalX >= Main.maxTilesX)
            {
                WorldGen.totalX = 0;
            }
        }

        if (!WorldInfo.StaticDisableLiquidUpdate)
        {
            Liquid.skipCount++;
            if (Liquid.skipCount > 1)
            {
                Liquid.UpdateLiquid();
                Liquid.skipCount = 0;
            }
        }
        int worldUpdateRate = WorldGen.GetWorldUpdateRate();
        if (worldUpdateRate == 0)
        {
            if (!WorldInfo.StaticUpdateStillWhenTimeRateIsZero)
            {
                return;
            }
            worldUpdateRate = WorldInfo.StaticUpdateTimeRateWhenTimeRateIsZero;
        }
        double num = 3E-05f * worldUpdateRate;
        double num2 = 1.5E-05f * worldUpdateRate;
        double num3 = 2.5E-05f * worldUpdateRate;
        bool checkNPCSpawns = false;
        WorldGen.spawnDelay++;
        if ((!SpawnInfo.TownNPCInfo.StaticSpawnAtInvasion && Main.invasionType > 0) || (!SpawnInfo.TownNPCInfo.StaticSpawnAtEclipse && Main.eclipse))
        {
            WorldGen.spawnDelay = 0;
        }
        if (WorldGen.spawnDelay >= 20)
        {
            checkNPCSpawns = true;
            WorldGen.spawnDelay = 0;
            if (WorldGen.prioritizedTownNPCType != 37)
            {
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].homeless && Main.npc[i].townNPC && Main.npc[i].type != NPCID.TravellingMerchant)
                    {
                        WorldGen.prioritizedTownNPCType = Main.npc[i].type;
                        break;
                    }
                }
            }
        }
        double num4 = (Main.maxTilesX * Main.maxTilesY) * num;
        int num5 = 151;
        int num6 = (int)TUtils.Lerp(num5, num5 * 2.8, TUtils.Clamp(Main.maxTilesX / 4200.0 - 1.0, 0.0, 1.0));
        for (int j = 0; j < num4; j++)
        {
            if (Main.rand.Next(num6 * 100) == 0)
            {
                WorldGen.PlantAlch();
            }
            int i2 = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
            int j2 = WorldGen.genRand.Next(10, (int)Main.worldSurface - 1);
            WorldGen.UpdateWorld_OvergroundTile(i2, j2, checkNPCSpawns, wallDist);
        }
        if (Main.remixWorld)
        {
            for (int k = 0; k < (Main.maxTilesX * Main.maxTilesY) * num3; k++)
            {
                int i3 = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
                int j3 = WorldGen.genRand.Next((int)Main.worldSurface - 1, Main.maxTilesY - 20);
                WorldGen.growGrassUnderground = true;
                WorldGen.UpdateWorld_UndergroundTile(i3, j3, checkNPCSpawns, wallDist);
                WorldGen.UpdateWorld_OvergroundTile(i3, j3, checkNPCSpawns, wallDist);
                WorldGen.growGrassUnderground = false;
            }
        }
        else
        {
            for (int l = 0; l < (Main.maxTilesX * Main.maxTilesY) * num2; l++)
            {
                int i4 = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
                int j4 = WorldGen.genRand.Next((int)Main.worldSurface - 1, Main.maxTilesY - 20);
                WorldGen.UpdateWorld_UndergroundTile(i4, j4, checkNPCSpawns, wallDist);
            }
        }
        if (WorldInfo.StaticNoSpawnFallenStar)
        {
            return;
        }
        if (WorldInfo.StaticSpawnFallenStarAtDay.IsFalseRet(Main.dayTime && !Main.remixWorld))
        {
            return;
        }
        for (int m = 0; m < Main.dayRate; m++)
        {
            double num7 = Main.maxTilesX / 4200.0;
            num7 *= Star.starfallBoost;
            if (!(Main.rand.Next(8000) < 10.0 * num7))
            {
                continue;
            }
            int num8 = 12;
            int num9 = Main.rand.Next(Main.maxTilesX - 50) + 100;
            num9 *= 16;
            int num10 = Main.rand.Next((int)(Main.maxTilesY * 0.05));
            num10 *= 16;
            Vector2 position = new(num9, num10);
            int ai1 = -1;
            if (Main.expertMode && Main.rand.Next(15) == 0)
            {
                int num12 = Player.FindClosest(position, 1, 1);
                if (Main.player[num12].position.Y < Main.worldSurface * 16.0 && Main.player[num12].afkCounter < 3600)
                {
                    int num13 = Main.rand.Next(1, 640);
                    position.X = Main.player[num12].position.X + Main.rand.Next(-num13, num13 + 1);
                    ai1 = num12;
                }
            }
            if (!Collision.SolidCollision(position, 16, 16))
            {
                float speedX = Main.rand.Next(-100, 101);
                float speedY = Main.rand.Next(200) + 100;
                float num16 = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                num16 = num8 / num16;
                speedX *= num16;
                speedY *= num16;
                if (!Main.dayTime || Main.remixWorld)
                {
                    Projectile.NewProjectile(new EntitySource_ByProjectileSourceId(11), position.X, position.Y, speedX, speedY, ProjectileID.FallingStarSpawner, 0, 0f, Main.myPlayer, 0f, ai1);
                }
                else
                {
                    Projectile.NewProjectile(new EntitySource_ByProjectileSourceId(11), position.X, position.Y, speedX, speedY, ProjectileID.FallingStar, 999, 0f, Main.myPlayer, 0f, ai1);
                }
            }
        }
    }
    internal static ushort[] CanSpreadCorruptTileIDs =
    {
        TileID.CorruptGrass, TileID.Ebonstone, TileID.CorruptThorns, TileID.Ebonsand, TileID.CorruptIce, TileID.CorruptSandstone, TileID.CorruptHardenedSand, TileID.CorruptVines, TileID.CorruptJungleGrass
    };
    internal static ushort[] ConvertTableCorrupt =
    {
        TileID.Grass, TileID.CorruptGrass,
        TileID.Sand, TileID.Ebonsand,
        TileID.Sandstone, TileID.CorruptSandstone,
        TileID.HardenedSand, TileID.CorruptHardenedSand,
        TileID.JungleGrass, TileID.CorruptJungleGrass,
        TileID.JungleThorns, TileID.CorruptThorns,
        TileID.IceBlock, TileID.CorruptIce,
        TileID.Stone, TileID.Ebonstone
    };
    internal static ushort[] CanSpreadCrimsonTileIDs =
    {
        TileID.CrimsonGrass, TileID.FleshIce, TileID.CrimsonPlants, TileID.Crimstone, TileID.CrimsonVines, TileID.Crimsand, TileID.CrimsonThorns, TileID.CrimsonSandstone, TileID.CrimsonHardenedSand, TileID.CrimsonJungleGrass
    };
    internal static ushort[] ConvertTableCrimson =
    {
        TileID.Grass, TileID.CrimsonGrass,
        TileID.Sand, TileID.Crimsand,
        TileID.Sandstone, TileID.CrimsonSandstone,
        TileID.HardenedSand, TileID.CrimsonHardenedSand,
        TileID.JungleGrass, TileID.CrimsonJungleGrass,
        TileID.JungleThorns, TileID.CrimsonThorns,
        TileID.IceBlock, TileID.FleshIce,
        TileID.Stone, TileID.Crimstone
    };
    internal static ushort[] CanSpreadHallowTileIDs =
    {
        TileID.HallowedGrass, TileID.HallowedPlants, TileID.HallowedPlants2, TileID.HallowedVines, TileID.Pearlsand, TileID.Pearlstone, TileID.HallowedIce, TileID.HallowHardenedSand, TileID.HallowSandstone, TileID.GolfGrassHallowed
    };
    internal static ushort[] ConvertTableHallow =
    {
        TileID.Grass, TileID.HallowedGrass,
        TileID.GolfGrass, TileID.GolfGrassHallowed,
        TileID.Sand, TileID.Pearlsand,
        TileID.Sandstone, TileID.HallowSandstone,
        TileID.HardenedSand, TileID.HallowHardenedSand,
        TileID.IceBlock, TileID.HallowedIce,
        TileID.Stone, TileID.Pearlstone
    };
    internal static ushort[] CanPurifiedTileIDs;
    internal static ushort[] PurifyTable;
    internal static void hardUpdateWorldConvert(ushort[] convertTable, int i, int j)
    {
        bool continueConvert = true;
        while (continueConvert)
        {
            continueConvert = false;
            //int x = i + WorldGen.genRand.Next(-3, 4);
            //int y = j + WorldGen.genRand.Next(-3, 4);
            int x = i + WorldGen.genRand.Next(-WorldInfo.StaticInfectionTransmissionDistance, WorldInfo.StaticInfectionTransmissionDistance + 1);
            int y = j + WorldGen.genRand.Next(-WorldInfo.StaticInfectionTransmissionDistance, WorldInfo.StaticInfectionTransmissionDistance + 1);
            if (!WorldGen.InWorld(x, y, 10))
            {
                continue;
            }
            if (WorldGen.nearbyChlorophyte(x, y))
            {
                WorldGen.ChlorophyteDefense(x, y);
            }
            else
            {
                if (WorldGen.CountNearBlocksTypes(x, y, 2, 1, 27) > 0)
                {
                    continue;
                }
                ushort newType = 0;
                int k;
                for (k = 0; k < convertTable.Length - 2; k += 2)
                {
                    if (Main.tile[x, y].type == convertTable[k])
                    {
                        if (WorldGen.genRand.Next(2) == 0)
                        {
                            continueConvert = true;
                        }
                        newType = convertTable[k + 1];
                        break;
                    }
                }
                if (Main.tile[x, y].type == convertTable[k] || Main.tileMoss[Main.tile[x, y].type])
                {
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        continueConvert = true;
                    }
                    newType = convertTable[k + 1];
                }
                if (newType != 0 && OTAPI.Hooks.WorldGen.InvokeHardmodeTileUpdate(x, y, newType))
                {
                    Main.tile[x, y].type = newType;
                    WorldGen.SquareTileFrame(x, y);
                    NetMessage.SendTileSquare(-1, x, y);
                }
            }
        }
    }
    [DetourMethod]
    public static void hardUpdateWorld(int i, int j) 
    {
        if (!(WorldInfo.StaticEnableHardModeUpdateWhenNotHardMode || Main.hardMode) || Main.tile[i, j].inActive())
        {
            return;
        }
        ushort type = Main.tile[i, j].type;
        if (type > 0 && type < TileID.Count && TileID.Sets.CanGrowCrystalShards[type] && (j > Main.rockLayer || Main.remixWorld) && WorldGen.genRand.Next(5) == 0)
        {
            int num = WorldGen.genRand.Next(4);
            int num2 = 0;
            int num3 = 0;
            switch (num)
            {
                case 0:
                    num2 = -1;
                    break;
                case 1:
                    num2 = 1;
                    break;
                default:
                    num3 = ((num != 0) ? 1 : (-1));
                    break;
            }
            if (!Main.tile[i + num2, j + num3].active())
            {
                int num4 = 0;
                int num5 = 6;
                for (int k = i - num5; k <= i + num5; k++)
                {
                    for (int l = j - num5; l <= j + num5; l++)
                    {
                        if (Main.tile[k, l].active() && Main.tile[k, l].type == TileID.Crystals)
                        {
                            num4++;
                        }
                    }
                }
                if (num4 < 2)
                {
                    short style = (short)WorldGen.genRand.Next(18);
                    if (WorldGen.genRand.Next(50) == 0)
                    {
                        style = (short)(18 + WorldGen.genRand.Next(6));
                    }
                    if (OTAPI.Hooks.WorldGen.InvokeHardmodeTilePlace(i + num2, j + num3, 129, mute: true, forced: false, -1, style))
                    {
                        NetMessage.SendTileSquare(-1, i + num2, j + num3);
                    }
                }
            }
        }
        if (j > (Main.worldSurface + Main.rockLayer) / 2.0 || Main.remixWorld)
        {
            if (type == TileID.JungleGrass && WorldGen.genRand.Next(300) == 0)
            {
                var tileWrapper = new TileWrapper(i + WorldGen.genRand.Next(-10, 11), j + WorldGen.genRand.Next(-10, 11));
                if (tileWrapper.InWorld(2) && tileWrapper is { active: true, type: TileID.Mud } && (!tileWrapper.Above.active || (tileWrapper.Above is { type: not (TileID.Trees or TileID.LifeFruit or TileID.PlanteraBulb) })) && WorldGen.Chlorophyte(tileWrapper.x, tileWrapper.y) && OTAPI.Hooks.WorldGen.InvokeHardmodeTileUpdate(tileWrapper.x, tileWrapper.y, TileID.Chlorophyte))
                {
                    tileWrapper.type = TileID.Chlorophyte;
                    tileWrapper.SquareTileFrame();
                    tileWrapper.SendTileSquare();
                }
            }
            if (type == TileID.Chlorophyte || type == TileID.ChlorophyteBrick)
            {
                int num8 = i;
                int num9 = j;
                if (WorldGen.genRand.Next(3) != 0)
                {
                    TileUtils.RandomlyMoveAdjacent(WorldGen.genRand, ref num8, ref num9);
                    if (WorldGen.InWorld(num8, num9, 2) && Main.tile[num8, num9].active() && (Main.tile[num8, num9].type is TileID.Mud or TileID.JungleGrass) && WorldGen.Chlorophyte(num8, num9) && OTAPI.Hooks.WorldGen.InvokeHardmodeTileUpdate(num8, num9, TileID.Chlorophyte))
                    {
                        Main.tile[num8, num9].type = TileID.Chlorophyte;
                        WorldGen.SquareTileFrame(num8, num9);
                        NetMessage.SendTileSquare(-1, num8, num9);
                    }
                }
                bool flag = true;
                while (flag)
                {
                    flag = false;
                    num8 = i + Main.rand.Next(-6, 7);
                    num9 = j + Main.rand.Next(-6, 7);
                    if (!WorldGen.InWorld(num8, num9, 2) || !Main.tile[num8, num9].active())
                    {
                        continue;
                    }
                    if (CanPurifiedTileIDs.Contains(Main.tile[num8, num9].type))
                    {
                        var curType = Main.tile[num8, num9].type;
                        for (int k = 0; k < PurifyTable.Length; k += 2)
                        {
                            if (PurifyTable[k] == curType)
                            {
                                var newType = PurifyTable[k + 1];
                                if (OTAPI.Hooks.WorldGen.InvokeHardmodeTileUpdate(num8, num9, newType))
                                {
                                    Main.tile[num8, num9].type = newType;
                                    WorldGen.SquareTileFrame(num8, num9);
                                    NetMessage.SendTileSquare(-1, num8, num9);
                                }
                                break;
                            }
                        }
                        flag = true;
                    }
                    else if (Main.tile[num8, num9].type is TileID.CorruptPlants or TileID.CrimsonPlants or TileID.CorruptThorns or TileID.CrimsonThorns or TileID.CorruptVines or TileID.CrimsonVines)
                    {
                        WorldGen.KillTile(num8, num9);
                        NetMessage.SendTileSquare(-1, num8, num9);
                        flag = true;
                    }
                }
            }
        }
        if ((NPC.downedPlantBoss && WorldGen.genRand.Next(2) != 0) || !WorldGen.AllowedToSpreadInfections)
        {
            return;
        }
        if (WorldInfo.StaticInfectionTransmissionDistance < 1)
        {
            return;
        }
        if (CanSpreadCorruptTileIDs.Contains(type))
        {
            hardUpdateWorldConvert(ConvertTableCorrupt, i, j);
        }
        else if (CanSpreadCrimsonTileIDs.Contains(type))
        {
            hardUpdateWorldConvert(ConvertTableCrimson, i, j);
        }
        else if (CanSpreadHallowTileIDs.Contains(type))
        {
            hardUpdateWorldConvert(ConvertTableHallow, i, j);
        }
    }
    public static void UpdateWorld_OvergroundTile(int i, int j, bool checkNPCSpawns, int wallDist)
    {
        int num = i - 1;
        int num2 = i + 2;
        int num3 = j - 1;
        int num4 = j + 2;
        Utils.SetValueInWorld(ref num, ref num2, ref num3, ref num4, 10);
        if (Main.tile[i, j] == null)
        {
            return;
        }
        if (Main.tile[i, j].type == TileID.PlanteraThorns && !NPC.AnyNPCs(NPCID.Plantera))
        {
            WorldGen.KillTile(i, j);
            if (Main.netMode == 2)
            {
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
            }
        }
        if (Main.tile[i, j].type == TileID.Tombstones)
        {
            WorldGen.TryGrowingAbigailsFlower(i, j);
        }
        else if (Main.tileAlch[Main.tile[i, j].type])
        {
            WorldGen.GrowAlch(i, j);
        }
        else if (j < Main.worldSurface + 10.0 && (i < WorldGen.beachDistance || i > Main.maxTilesX - WorldGen.beachDistance) && !Main.tile[i, j].active())
        {
            int num5 = 3000 - (int)(Math.Abs(Main.windSpeedCurrent) * 1250f);
            if (Main.raining)
            {
                num5 -= (int)(1250f * Main.maxRaining);
            }
            if (num5 < 300)
            {
                num5 = 300;
            }
            if (WorldGen.genRand.Next(num5) == 0)
            {
                int k;
                for (k = j; k < Main.worldSurface + 10.0 && !Main.tile[i, k].active() && k - j < 15; k++)
                {
                }
                if (Main.tile[i, k].active() && Main.tile[i, k].type == TileID.Sand && WorldGen.SolidTileAllowBottomSlope(i, k))
                {
                    k--;
                    int num6 = WorldGen.genRand.Next(2, 5);
                    int num7 = WorldGen.genRand.Next(8, 11);
                    int num8 = 0;
                    for (int l = i - num7; l <= i + num7; l++)
                    {
                        for (int m = k - num7; m <= k + num7; m++)
                        {
                            if (Main.tile[l, m].active() && (Main.tile[l, m].type == TileID.BeachPiles || Main.tile[l, m].type == TileID.Coral))
                            {
                                num8++;
                            }
                        }
                    }
                    if (num8 < num6)
                    {
                        if (WorldGen.genRand.Next(2) == 0 && Main.tile[i, k].liquid >= 230)
                        {
                            WorldGen.PlaceTile(i, k, 81, mute: true);
                            if (Main.netMode == 2 && Main.tile[i, k].active())
                            {
                                NetMessage.SendTileSquare(-1, i, k);
                            }
                        }
                        else
                        {
                            WorldGen.PlaceTile(i, k, 324, mute: true, forced: false, -1, WorldGen.RollRandomSeaShellStyle());
                            if (Main.netMode == 2 && Main.tile[i, k].active())
                            {
                                NetMessage.SendTileSquare(-1, i, k);
                            }
                        }
                    }
                }
            }
        }
        if ((Main.tile[i, j].type == TileID.VanityTreeSakura || Main.tile[i, j].type == TileID.VanityTreeYellowWillow || Main.tile[i, j].type == TileID.VanityTreeSakuraSaplings || Main.tile[i, j].type == TileID.VanityTreeWillowSaplings) && (Main.tile[i, j + 1].type == TileID.CrimsonGrass || Main.tile[i, j + 1].type == TileID.CorruptGrass))
        {
            WorldGen.KillTile(i, j);
            if (Main.netMode == 2)
            {
                NetMessage.SendTileSquare(-1, i, j);
            }
        }
        if ((Main.tile[i, j].type == TileID.Bamboo || (Main.tile[i, j].type == TileID.JungleGrass && Main.tile[i, j - 1].liquid > 0)) && WorldGen.genRand.Next(5) == 0 && (!Main.tile[i, j - 1].active() || Main.tile[i, j - 1].type == TileID.JunglePlants || Main.tile[i, j - 1].type == TileID.JunglePlants2 || Main.tile[i, j - 1].type == TileID.LilyPad) && (Main.tile[i, j].type != TileID.JungleGrass || WorldGen.genRand.Next(30) == 0) && WorldGen.PlaceBamboo(i, j - 1))
        {
            NetMessage.SendTileSquare(-1, i, j - 1, 1, 2);
        }
        if (Main.tile[i, j].type == TileID.LilyPad)
        {
            if (Main.tile[i, j].liquid == 0 || (Main.tile[i, j].liquid / 16 >= 9 && WorldGen.SolidTile(i, j - 1)) || (Main.tile[i, j - 1].liquid > 0 && Main.tile[i, j - 1].active()))
            {
                WorldGen.KillTile(i, j);
                if (Main.netMode == 2)
                {
                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
                }
            }
            else
            {
                WorldGen.CheckLilyPad(i, j);
            }
        }
        else if (Main.tile[i, j].type == TileID.Cattail)
        {
            WorldGen.CheckCatTail(i, j);
            if (Main.tile[i, j].active() && WorldGen.genRand.Next(8) == 0)
            {
                WorldGen.GrowCatTail(i, j);
                WorldGen.CheckCatTail(i, j);
            }
        }
        else if (Main.tile[i, j].liquid > 32)
        {
            if (Main.tile[i, j].active())
            {
                if (TileID.Sets.SlowlyDiesInWater[Main.tile[i, j].type])
                {
                    WorldGen.KillTile(i, j);
                    if (Main.netMode == 2)
                    {
                        NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
                    }
                }
                else if (Main.tile[i, j].type == TileID.JungleGrass)
                {
                    WorldGen.UpdateWorld_GrassGrowth(i, j, num, num2, num3, num4, underground: false);
                }
            }
            else if (WorldGen.genRand.Next(600) == 0)
            {
                WorldGen.PlaceTile(i, j, 518, mute: true);
                if (Main.netMode == 2)
                {
                    NetMessage.SendTileSquare(-1, i, j);
                }
            }
            else if (WorldGen.genRand.Next(600) == 0)
            {
                WorldGen.PlaceTile(i, j, 519, mute: true);
                if (Main.netMode == 2)
                {
                    NetMessage.SendTileSquare(-1, i, j);
                }
            }
        }
        else if (Main.tile[i, j].nactive())
        {
            WorldGen.hardUpdateWorld(i, j);
            if (Main.rand.Next(3000) == 0)
            {
                WorldGen.plantDye(i, j);
            }
            else if (Main.hardMode && (i < Main.maxTilesX * 0.4 || i > Main.maxTilesX * 0.6) && Main.rand.Next(15000) == 0)
            {
                WorldGen.plantDye(i, j, exoticPlant: true);
            }
            if (Main.tile[i, j].type == TileID.Cactus)
            {
                if (WorldGen.genRand.Next(15) == 0)
                {
                    WorldGen.GrowCactus(i, j);
                }
            }
            else if (Main.tile[i, j].type == TileID.SeaOats)
            {
                if (WorldGen.CheckSeaOat(i, j) && WorldGen.genRand.Next(20) == 0)
                {
                    WorldGen.GrowSeaOat(i, j);
                }
            }
            else if (TileID.Sets.Conversion.Sand[Main.tile[i, j].type])
            {
                if (!Main.tile[i, num3].active())
                {
                    if (WorldGen.genRand.Next(25) == 0)
                    {
                        WorldGen.PlaceOasisPlant(i, num3, 530);
                        if (Main.tile[i, num3].type == TileID.OasisPlants && Main.netMode == 2)
                        {
                            NetMessage.SendTileSquare(-1, i - 1, num3 - 1, 3, 2);
                        }
                    }
                    if (WorldGen.genRand.Next(20) != 0 || !WorldGen.PlantSeaOat(i, num3))
                    {
                        if (i < WorldGen.oceanDistance || i > Main.maxTilesX - WorldGen.oceanDistance)
                        {
                            if (WorldGen.genRand.Next(500) == 0)
                            {
                                int num9 = 7;
                                int num10 = 6;
                                int num11 = 0;
                                for (int n = i - num9; n <= i + num9; n++)
                                {
                                    for (int num12 = num3 - num9; num12 <= num3 + num9; num12++)
                                    {
                                        if (Main.tile[n, num12].active() && Main.tile[n, num12].type == TileID.Coral)
                                        {
                                            num11++;
                                        }
                                    }
                                }
                                if (num11 < num10 && Main.tile[i, num3].liquid == byte.MaxValue && Main.tile[i, num3 - 1].liquid == byte.MaxValue && Main.tile[i, num3 - 2].liquid == byte.MaxValue && Main.tile[i, num3 - 3].liquid == byte.MaxValue && Main.tile[i, num3 - 4].liquid == byte.MaxValue)
                                {
                                    WorldGen.PlaceTile(i, num3, 81, mute: true);
                                    if (Main.netMode == 2 && Main.tile[i, num3].active())
                                    {
                                        NetMessage.SendTileSquare(-1, i, num3);
                                    }
                                }
                            }
                        }
                        else if (i > WorldGen.beachDistance + 20 && i < Main.maxTilesX - WorldGen.beachDistance - 20 && WorldGen.genRand.Next(300) == 0)
                        {
                            WorldGen.GrowCactus(i, j);
                        }
                    }
                }
            }
            else if (Main.tile[i, j].type == TileID.OasisPlants)
            {
                if (!WorldGen.OasisPlantWaterCheck(i, j, boost: true))
                {
                    WorldGen.KillTile(i, j);
                    if (Main.netMode == 2)
                    {
                        NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
                    }
                }
            }
            else if (Main.tile[i, j].type == TileID.SnowBlock || Main.tile[i, j].type == TileID.IceBlock || Main.tile[i, j].type == TileID.CorruptIce || Main.tile[i, j].type == TileID.HallowedIce || Main.tile[i, j].type == TileID.FleshIce)
            {
                if (Main.rand.Next(10) == 0 && !Main.tile[i, j + 1].active() && !Main.tile[i, j + 2].active())
                {
                    int num13 = i - 3;
                    int num14 = i + 4;
                    int num15 = 0;
                    for (int num16 = num13; num16 < num14; num16++)
                    {
                        if (Main.tile[num16, j].type == TileID.Stalactite && Main.tile[num16, j].active())
                        {
                            num15++;
                        }
                        if (Main.tile[num16, j + 1].type == TileID.Stalactite && Main.tile[num16, j + 1].active())
                        {
                            num15++;
                        }
                        if (Main.tile[num16, j + 2].type == TileID.Stalactite && Main.tile[num16, j + 2].active())
                        {
                            num15++;
                        }
                        if (Main.tile[num16, j + 3].type == TileID.Stalactite && Main.tile[num16, j + 3].active())
                        {
                            num15++;
                        }
                    }
                    if (num15 < 2)
                    {
                        WorldGen.PlaceTight(i, j + 1);
                        WorldGen.SquareTileFrame(i, j + 1);
                        if (Main.netMode == 2 && Main.tile[i, j + 1].active())
                        {
                            NetMessage.SendTileSquare(-1, i, j + 1, 1, 2);
                        }
                    }
                }
            }
            else if (Main.tile[i, j].type == TileID.Pumpkins)
            {
                if (Main.rand.Next((Main.tile[i, j].frameX + 10) / 10) == 0)
                {
                    WorldGen.GrowPumpkin(i, j, 254);
                }
            }
            else if (Main.tile[i, j].type is TileID.ClayPot or TileID.RockGolemHead) //or TileID.PlanterBox)
            {
                if (!Main.tile[i, num3].active() && WorldGen.genRand.Next(2) == 0)
                {
                    WorldGen.PlaceTile(i, num3, TileID.Plants, mute: true);
                    if (Main.netMode == 2 && Main.tile[i, num3].active())
                    {
                        NetMessage.SendTileSquare(-1, i, num3);
                    }
                }
            }
            else if (TileID.Sets.SpreadOverground[Main.tile[i, j].type])
            {
                WorldGen.UpdateWorld_GrassGrowth(i, j, num, num2, num3, num4, underground: false);
                int type = Main.tile[i, j].type;
                if ((type == 32 || type == 352) && WorldGen.genRand.Next(3) == 0)
                {
                    if (type == 32)
                    {
                        WorldGen.GrowSpike(i, j, 32, 23);
                    }
                    else
                    {
                        WorldGen.GrowSpike(i, j, 352, 199);
                    }
                }
            }
            else if (Main.tileMoss[Main.tile[i, j].type] || TileID.Sets.tileMossBrick[Main.tile[i, j].type])
            {
                if (WorldGen.genRand.NextDouble() < 0.5)
                {
                    int type2 = Main.tile[i, j].type;
                    bool flag = false;
                    TileColorCache color = Main.tile[i, j].BlockColorAndCoating();
                    for (int num17 = num; num17 < num2; num17++)
                    {
                        for (int num18 = num3; num18 < num4; num18++)
                        {
                            if ((i != num17 || j != num18) && Main.tile[num17, num18].active() && (Main.tile[num17, num18].type == TileID.Stone || Main.tile[num17, num18].type == TileID.GrayBrick))
                            {
                                int num19 = WorldGen.MossConversion(type2, Main.tile[num17, num18].type);
                                WorldGen.SpreadGrass(num17, num18, Main.tile[num17, num18].type, num19, repeat: false, color);
                                if (Main.tile[num17, num18].type == num19)
                                {
                                    WorldGen.SquareTileFrame(num17, num18);
                                    flag = true;
                                }
                            }
                        }
                    }
                    if (Main.netMode == 2 && flag)
                    {
                        NetMessage.SendTileSquare(-1, i, j, 3);
                    }
                    if (WorldGen.genRand.Next(6) == 0)
                    {
                        int num20 = i;
                        int num21 = j;
                        TileUtils.RandomlyMoveAdjacent(WorldGen.genRand, ref num20, ref num21);
                        if (!Main.tile[num20, num21].active())
                        {
                            if (WorldGen.PlaceTile(num20, num21, 184, mute: true))
                            {
                                Main.tile[num20, num21].CopyPaintAndCoating(Main.tile[i, j]);
                            }
                            if (Main.netMode == 2 && Main.tile[num20, num21].active())
                            {
                                NetMessage.SendTileSquare(-1, num20, num21);
                            }
                        }
                    }
                }
            }
            else if (Main.tile[i, j].type == TileID.Saplings)
            {
                if (WorldGen.genRand.Next(20) == 0)
                {
                    WorldGen.AttemptToGrowTreeFromSapling(i, j, underground: false);
                }
            }
            else if (Main.tile[i, j].type is TileID.VanityTreeSakuraSaplings or TileID.VanityTreeWillowSaplings)
            {
                if (WorldGen.genRand.Next(5) == 0)
                {
                    WorldGen.AttemptToGrowTreeFromSapling(i, j, underground: false);
                }
            }
            else if (Main.tile[i, j].type == TileID.Plants && WorldGen.genRand.Next(20) == 0)
            {
                if (Main.tile[i, j].frameX != 18 * 8)
                {
                    Main.tile[i, j].type = TileID.Plants2;
                    if (Main.netMode == 2)
                    {
                        NetMessage.SendTileSquare(-1, i, j);
                    }
                }
            }
            else if (Main.tile[i, j].type == TileID.HallowedPlants && WorldGen.genRand.Next(20) == 0 && Main.tile[i, j].frameX < 144)
            {
                Main.tile[i, j].type = TileID.HallowedPlants2;
                if (Main.netMode == 2)
                {
                    NetMessage.SendTileSquare(-1, i, j);
                }
            }
        }
        else
        {
            if (Main.tile[i, j].wall == WallID.SpiderUnsafe && Main.tile[i, j].liquid == 0)
            {
                WorldGen.GrowWeb(i, j);
            }
            if (checkNPCSpawns)
            {
                WorldGen.TrySpawningTownNPC(i, j);
            }
        }
        if (WorldGen.AllowedToSpreadInfections)
        {
            if (Main.tile[i, j].wall == WallID.CrimsonGrassUnsafe || Main.tile[i, j].wall == WallID.CrimstoneUnsafe || (Main.tile[i, j].type == TileID.CrimsonGrass && Main.tile[i, j].active()))
            {
                int num22 = i + WorldGen.genRand.Next(-2, 3);
                int num23 = j + WorldGen.genRand.Next(-2, 3);
                if (WorldGen.InWorld(num22, num23, 10) && Main.tile[num22, num23].wall >= 63 && Main.tile[num22, num23].wall <= 68)
                {
                    bool flag2 = false;
                    for (int num24 = i - wallDist; num24 < i + wallDist; num24++)
                    {
                        for (int num25 = j - wallDist; num25 < j + wallDist; num25++)
                        {
                            if (Main.tile[num24, num25].active())
                            {
                                int type3 = Main.tile[num24, num25].type;
                                if (type3 == 199 || type3 == 200 || type3 == 201 || type3 == 203 || type3 == 205 || type3 == 234 || type3 == 352 || type3 == 662)
                                {
                                    flag2 = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (flag2)
                    {
                        Main.tile[num22, num23].wall = WallID.CrimsonGrassUnsafe;
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendTileSquare(-1, num22, num23);
                        }
                    }
                }
            }
            else if (Main.tile[i, j].wall == WallID.CorruptGrassUnsafe || Main.tile[i, j].wall == WallID.EbonstoneUnsafe || (Main.tile[i, j].type == TileID.CorruptGrass && Main.tile[i, j].active()))
            {
                int num26 = i + WorldGen.genRand.Next(-2, 3);
                int num27 = j + WorldGen.genRand.Next(-2, 3);
                if (WorldGen.InWorld(num26, num27, 10) && Main.tile[num26, num27].wall >= 63 && Main.tile[num26, num27].wall <= 68)
                {
                    bool flag3 = false;
                    for (int num28 = i - wallDist; num28 < i + wallDist; num28++)
                    {
                        for (int num29 = j - wallDist; num29 < j + wallDist; num29++)
                        {
                            if (Main.tile[num28, num29].active())
                            {
                                int type4 = Main.tile[num28, num29].type;
                                if (type4 == 22 || type4 == 23 || type4 == 24 || type4 == 25 || type4 == 32 || type4 == 112 || type4 == 163 || type4 == 636 || type4 == 661)
                                {
                                    flag3 = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (flag3)
                    {
                        Main.tile[num26, num27].wall = WallID.CorruptGrassUnsafe;
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendTileSquare(-1, num26, num27);
                        }
                    }
                }
            }
            else if (Main.tile[i, j].wall == WallID.HallowedGrassUnsafe || (Main.tile[i, j].type == TileID.HallowedGrass && Main.tile[i, j].active()))
            {
                int num30 = i + WorldGen.genRand.Next(-2, 3);
                int num31 = j + WorldGen.genRand.Next(-2, 3);
                if ((WorldGen.InWorld(num30, num31, 10) && Main.tile[num30, num31].wall == WallID.GrassUnsafe) || Main.tile[num30, num31].wall == WallID.FlowerUnsafe || Main.tile[num30, num31].wall == WallID.Grass || Main.tile[num30, num31].wall == WallID.Flower)
                {
                    bool flag4 = false;
                    for (int num32 = i - wallDist; num32 < i + wallDist; num32++)
                    {
                        for (int num33 = j - wallDist; num33 < j + wallDist; num33++)
                        {
                            if (Main.tile[num32, num33].active())
                            {
                                int type5 = Main.tile[num32, num33].type;
                                if (type5 == 109 || type5 == 110 || type5 == 113 || type5 == 115 || type5 == 116 || type5 == 117 || type5 == 164)
                                {
                                    flag4 = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (flag4)
                    {
                        Main.tile[num30, num31].wall = WallID.HallowedGrassUnsafe;
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendTileSquare(-1, num30, num31);
                        }
                    }
                }
            }
            WorldGen.SpreadDesertWalls(wallDist, i, j);
        }
        if (Main.tile[i, j].nactive())
        {
            if (Main.tile[i, j].type is TileID.Grass or TileID.Vines or TileID.VineFlowers || (Main.tile[i, j].type == TileID.LeafBlock && WorldGen.genRand.Next(10) == 0))
            {
                //不知名无用东西
                //int num34 = 60;
                //if (Main.tile[i, j].type == TileID.Vines || Main.tile[i, j].type == TileID.VineFlowers)
                //{
                //    num34 = 20;
                //}
                if (WorldGen.genRand.Next(1) == 0 && WorldGen.GrowMoreVines(i, j) && !Main.tile[i, j + 1].active() && !Main.tile[i, j + 1].lava())
                {
                    bool flag5 = false;
                    ushort type6 = 52;
                    if (Main.tile[i, j].type == TileID.VineFlowers)
                    {
                        type6 = 382;
                    }
                    else if (Main.tile[i, j].type != TileID.Vines)
                    {
                        if (Main.tile[i, j].wall == WallID.Flower || Main.tile[i, j].wall == WallID.FlowerUnsafe || Main.tile[i, j].wall == WallID.Grass || Main.tile[i, j].wall == WallID.GrassUnsafe)
                        {
                            type6 = 382;
                        }
                        else if (Main.tile[i, j + 1].wall == WallID.Flower || Main.tile[i, j + 1].wall == WallID.FlowerUnsafe || Main.tile[i, j + 1].wall == WallID.Grass || Main.tile[i, j + 1].wall == WallID.GrassUnsafe)
                        {
                            type6 = 382;
                        }
                        if (Main.remixWorld && WorldGen.genRand.Next(5) == 0)
                        {
                            type6 = 382;
                        }
                    }
                    for (int num35 = j; num35 > j - 10; num35--)
                    {
                        if (Main.tile[i, num35].bottomSlope())
                        {
                            flag5 = false;
                            break;
                        }
                        if (Main.tile[i, num35].active() && Main.tile[i, num35].type == TileID.Grass && !Main.tile[i, num35].bottomSlope())
                        {
                            flag5 = true;
                            break;
                        }
                    }
                    if (flag5)
                    {
                        int num36 = j + 1;
                        Main.tile[i, num36].type = type6;
                        Main.tile[i, num36].active(active: true);
                        Main.tile[i, num36].CopyPaintAndCoating(Main.tile[i, j]);
                        WorldGen.SquareTileFrame(i, num36);
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendTileSquare(-1, i, num36);
                        }
                    }
                }
            }
            else if (Main.tile[i, j].type == TileID.JunglePlants && WorldGen.genRand.Next(3) == 0 && Main.tile[i, j].frameX < 144)
            {
                if (Main.rand.Next(4) == 0)
                {
                    Main.tile[i, j].frameX = (short)(162 + WorldGen.genRand.Next(8) * 18);
                }
                Main.tile[i, j].type = TileID.JunglePlants2;
                if (Main.netMode == 2)
                {
                    NetMessage.SendTileSquare(-1, i, j);
                }
            }
            if ((Main.tile[i, j].type == TileID.JungleGrass || Main.tile[i, j].type == TileID.JungleVines) && WorldGen.GrowMoreVines(i, j))
            {
                int maxValue = 30;
                if (Main.tile[i, j].type == TileID.JungleVines)
                {
                    maxValue = 10;
                }
                if (WorldGen.genRand.Next(maxValue) == 0 && !Main.tile[i, j + 1].active() && !Main.tile[i, j + 1].lava())
                {
                    bool flag6 = false;
                    for (int num37 = j; num37 > j - 10; num37--)
                    {
                        if (Main.tile[i, num37].bottomSlope())
                        {
                            flag6 = false;
                            break;
                        }
                        if (Main.tile[i, num37].active() && Main.tile[i, num37].type == TileID.JungleGrass && !Main.tile[i, num37].bottomSlope())
                        {
                            flag6 = true;
                            break;
                        }
                    }
                    if (flag6)
                    {
                        int num38 = j + 1;
                        Main.tile[i, num38].type = TileID.JungleVines;
                        Main.tile[i, num38].active(active: true);
                        Main.tile[i, num38].CopyPaintAndCoating(Main.tile[i, j]);
                        WorldGen.SquareTileFrame(i, num38);
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendTileSquare(-1, i, num38);
                        }
                    }
                }
            }
            else if ((Main.tile[i, j].type is TileID.MushroomGrass or TileID.MushroomVines) && WorldGen.GrowMoreVines(i, j))
            {
                int maxValue2 = 70;
                if (Main.tile[i, j].type == TileID.MushroomVines)
                {
                    maxValue2 = 7;
                }
                if (WorldGen.genRand.Next(maxValue2) == 0 && !Main.tile[i, j + 1].active() && !Main.tile[i, j + 1].lava())
                {
                    bool flag7 = false;
                    for (int num39 = j; num39 > j - 10; num39--)
                    {
                        if (Main.tile[i, num39].bottomSlope())
                        {
                            flag7 = false;
                            break;
                        }
                        if (Main.tile[i, num39].active() && Main.tile[i, num39].type == TileID.MushroomGrass && !Main.tile[i, num39].bottomSlope())
                        {
                            flag7 = true;
                            break;
                        }
                    }
                    if (flag7)
                    {
                        int num40 = j + 1;
                        Main.tile[i, num40].type = TileID.MushroomVines;
                        Main.tile[i, num40].active(active: true);
                        Main.tile[i, num40].CopyPaintAndCoating(Main.tile[i, j]);
                        WorldGen.SquareTileFrame(i, num40);
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendTileSquare(-1, i, num40);
                        }
                    }
                }
            }
            else if ((Main.tile[i, j].type == TileID.HallowedGrass || Main.tile[i, j].type == TileID.HallowedVines) && WorldGen.GrowMoreVines(i, j))
            {
                int maxValue3 = 60;
                if (Main.tile[i, j].type == TileID.HallowedVines)
                {
                    maxValue3 = 20;
                }
                if (WorldGen.genRand.Next(maxValue3) == 0 && !Main.tile[i, j + 1].active() && !Main.tile[i, j + 1].lava())
                {
                    bool flag8 = false;
                    for (int num41 = j; num41 > j - 10; num41--)
                    {
                        if (Main.tile[i, num41].bottomSlope())
                        {
                            flag8 = false;
                            break;
                        }
                        if (Main.tile[i, num41].active() && Main.tile[i, num41].type == TileID.HallowedGrass && !Main.tile[i, num41].bottomSlope())
                        {
                            flag8 = true;
                            break;
                        }
                    }
                    if (flag8)
                    {
                        int num42 = j + 1;
                        Main.tile[i, num42].type = TileID.HallowedVines;
                        Main.tile[i, num42].active(active: true);
                        Main.tile[i, num42].CopyPaintAndCoating(Main.tile[i, j]);
                        WorldGen.SquareTileFrame(i, num42);
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendTileSquare(-1, i, num42);
                        }
                    }
                }
            }
            else if ((Main.tile[i, j].type == TileID.CorruptGrass || Main.tile[i, j].type == TileID.CorruptJungleGrass || Main.tile[i, j].type == TileID.CorruptVines) && WorldGen.GrowMoreVines(i, j))
            {
                int maxValue4 = 60;
                if (Main.tile[i, j].type == TileID.CorruptVines)
                {
                    maxValue4 = 20;
                }
                if (WorldGen.genRand.Next(maxValue4) == 0 && !Main.tile[i, j + 1].active() && !Main.tile[i, j + 1].lava())
                {
                    bool flag9 = false;
                    for (int num43 = j; num43 > j - 10; num43--)
                    {
                        if (Main.tile[i, num43].bottomSlope())
                        {
                            flag9 = false;
                            break;
                        }
                        if (Main.tile[i, num43].active() && (Main.tile[i, num43].type == TileID.CorruptGrass || Main.tile[i, num43].type == TileID.CorruptJungleGrass) && !Main.tile[i, num43].bottomSlope())
                        {
                            flag9 = true;
                            break;
                        }
                    }
                    if (flag9)
                    {
                        int num44 = j + 1;
                        Main.tile[i, num44].type = TileID.CorruptVines;
                        Main.tile[i, num44].active(active: true);
                        Main.tile[i, num44].CopyPaintAndCoating(Main.tile[i, j]);
                        WorldGen.SquareTileFrame(i, num44);
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendTileSquare(-1, i, num44);
                        }
                    }
                }
            }
            else if ((Main.tile[i, j].type == TileID.CrimsonGrass || Main.tile[i, j].type == TileID.CrimsonJungleGrass || Main.tile[i, j].type == TileID.CrimsonVines) && WorldGen.GrowMoreVines(i, j))
            {
                int maxValue5 = 60;
                if (Main.tile[i, j].type == TileID.CrimsonVines)
                {
                    maxValue5 = 20;
                }
                if (WorldGen.genRand.Next(maxValue5) == 0 && !Main.tile[i, j + 1].active() && !Main.tile[i, j + 1].lava())
                {
                    bool flag10 = false;
                    for (int num45 = j; num45 > j - 10; num45--)
                    {
                        if (Main.tile[i, num45].bottomSlope())
                        {
                            flag10 = false;
                            break;
                        }
                        if (Main.tile[i, num45].active() && (Main.tile[i, num45].type == TileID.CrimsonGrass || Main.tile[i, num45].type == TileID.CrimsonJungleGrass) && !Main.tile[i, num45].bottomSlope())
                        {
                            flag10 = true;
                            break;
                        }
                    }
                    if (flag10)
                    {
                        int num46 = j + 1;
                        Main.tile[i, num46].type = TileID.CrimsonVines;
                        Main.tile[i, num46].active(active: true);
                        Main.tile[i, num46].CopyPaintAndCoating(Main.tile[i, j]);
                        WorldGen.SquareTileFrame(i, num46);
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendTileSquare(-1, i, num46);
                        }
                    }
                }
            }
            else if ((Main.tile[i, j].type == TileID.AshGrass || Main.tile[i, j].type == TileID.AshVines) && WorldGen.GrowMoreVines(i, j))
            {
                int maxValue6 = 70;
                if (Main.tile[i, j].type == TileID.AshVines)
                {
                    maxValue6 = 7;
                }
                if (WorldGen.genRand.Next(maxValue6) == 0 && !Main.tile[i, j + 1].active() && !Main.tile[i, j + 1].lava())
                {
                    bool flag11 = false;
                    for (int num47 = j; num47 > j - 10; num47--)
                    {
                        if (Main.tile[i, num47].bottomSlope())
                        {
                            flag11 = false;
                            break;
                        }
                        if (Main.tile[i, num47].active() && Main.tile[i, num47].type == TileID.AshGrass && !Main.tile[i, num47].bottomSlope())
                        {
                            flag11 = true;
                            break;
                        }
                    }
                    if (flag11)
                    {
                        int num48 = j + 1;
                        Main.tile[i, num48].type = TileID.AshVines;
                        Main.tile[i, num48].active(active: true);
                        Main.tile[i, num48].CopyPaintAndCoating(Main.tile[i, j]);
                        WorldGen.SquareTileFrame(i, num48);
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendTileSquare(-1, i, num48);
                        }
                    }
                }
            }
        }
        if (!Main.remixWorld && Main.dontStarveWorld && Main.rand.Next(200) < 100f * Main.maxRaining && Main.maxRaining >= 0.2f)
        {
            WorldGen.DontStarveTryWateringTile(i, j);
        }
    }
    [DetourMethod]
    public static void UpdateWorld_GrassGrowth(int i, int j, int minI, int maxI, int minJ, int maxJ, bool underground)
    {
        if (!WorldGen.InWorld(i, j, 10))
        {
            return;
        }
        if (underground)
        {
            int type = Main.tile[i, j].type;
            int num = -1;
            int num2 = -1;
            int num3 = -1;
            int maxValue = 1;
            int num4 = type;
            int num5 = -1;
            switch (type)
            {
                case 23:
                    num = 0;
                    num2 = 59;
                    num4 = 23;
                    num5 = 661;
                    num3 = 24;
                    maxValue = 2;
                    if (!WorldGen.AllowedToSpreadInfections)
                    {
                        return;
                    }
                    break;
                case 199:
                    num = 0;
                    num2 = 59;
                    num4 = 199;
                    num5 = 662;
                    num3 = 201;
                    maxValue = 2;
                    if (!WorldGen.AllowedToSpreadInfections)
                    {
                        return;
                    }
                    break;
                case 661:
                    num = 59;
                    num2 = 0;
                    num4 = 661;
                    num5 = 23;
                    num3 = 24;
                    maxValue = 2;
                    if (!WorldGen.AllowedToSpreadInfections)
                    {
                        return;
                    }
                    break;
                case 662:
                    num = 59;
                    num2 = 0;
                    num4 = 662;
                    num5 = 199;
                    num3 = 201;
                    maxValue = 2;
                    if (!WorldGen.AllowedToSpreadInfections)
                    {
                        return;
                    }
                    break;
                case 60:
                    num = 59;
                    num3 = 61;
                    maxValue = 10;
                    break;
                case 70:
                    num = 59;
                    num3 = 71;
                    maxValue = 10;
                    break;
                case 633:
                    num = 57;
                    num3 = 637;
                    maxValue = 2;
                    break;
            }
            bool flag = false;
            if (num3 != -1 && !Main.tile[i, minJ].active() && WorldGen.genRand.Next(maxValue) == 0)
            {
                flag = true;
                if (WorldGen.PlaceTile(i, minJ, num3, mute: true))
                {
                    Main.tile[i, minJ].CopyPaintAndCoating(Main.tile[i, j]);
                }
                if (Main.tile[i, minJ].active())
                {
                    NetMessage.SendTileSquare(-1, i, minJ);
                }
            }
            if (num != -1)
            {
                bool flag2 = false;
                TileColorCache color = Main.tile[i, j].BlockColorAndCoating();
                for (int k = minI; k < maxI; k++)
                {
                    for (int l = minJ; l < maxJ; l++)
                    {
                        if (!WorldGen.InWorld(k, l, 10) || (i == k && j == l) || !Main.tile[k, l].active())
                        {
                            continue;
                        }
                        if (Main.tile[k, l].type == num)
                        {
                            WorldGen.SpreadGrass(k, l, num, num4, repeat: false, color);
                            if (Main.tile[k, l].type == num4)
                            {
                                WorldGen.SquareTileFrame(k, l);
                                flag2 = true;
                            }
                        }
                        else if (num2 > -1 && num5 > -1 && Main.tile[k, l].type == num2)
                        {
                            WorldGen.SpreadGrass(k, l, num2, num5, repeat: false, color);
                            if (Main.tile[k, l].type == num5)
                            {
                                WorldGen.SquareTileFrame(k, l);
                                flag2 = true;
                            }
                        }
                    }
                }
                if (flag2)
                {
                    NetMessage.SendTileSquare(-1, i, j, 3);
                }
            }
            switch (type)
            {
                case 60:
                    {
                        if (flag || WorldGen.genRand.Next(25) != 0 || Main.tile[i, minJ].liquid != 0)
                        {
                            break;
                        }
                        if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && WorldGen.genRand.Next(60) == 0)
                        {
                            bool flag3 = true;
                            int num6 = 150;
                            for (int m = i - num6; m < i + num6; m += 2)
                            {
                                for (int n = j - num6; n < j + num6; n += 2)
                                {
                                    if (m > 1 && m < Main.maxTilesX - 2 && n > 1 && n < Main.maxTilesY - 2 && Main.tile[m, n].active() && Main.tile[m, n].type == TileID.PlanteraBulb)
                                    {
                                        flag3 = false;
                                        break;
                                    }
                                }
                            }
                            if (flag3)
                            {
                                WorldGen.PlaceJunglePlant(i, minJ, 238, 0, 0);
                                WorldGen.SquareTileFrame(i, minJ);
                                WorldGen.SquareTileFrame(i + 2, minJ);
                                WorldGen.SquareTileFrame(i - 1, minJ);
                                if (Main.tile[i, minJ].type == TileID.PlanteraBulb && Main.netMode == 2)
                                {
                                    NetMessage.SendTileSquare(-1, i, minJ, 5);
                                }
                            }
                        }
                        int maxValue2 = (Main.expertMode ? 30 : 40);
                        var canSpawnLifeFruit = true;
                        foreach (var id in WorldInfo.StaticGrowLifeFruitRequireProgressIDs)
                        {
                            if (!ShimmerItemReplaceInfo.DownedFuncs[id]())
                            {
                                canSpawnLifeFruit = false;
                                break;
                            }
                        }
                        //if (Main.hardMode && NPC.downedMechBossAny && WorldGen.genRand.Next(maxValue2) == 0)
                        if (canSpawnLifeFruit && WorldGen.genRand.Next(maxValue2) == 0)
                        {
                            bool flag4 = true;
                            int num7 = Main.expertMode ? 50 : 60;
                            for (int num8 = i - num7; num8 < i + num7; num8 += 2)
                            {
                                for (int num9 = j - num7; num9 < j + num7; num9 += 2)
                                {
                                    if (num8 > 1 && num8 < Main.maxTilesX - 2 && num9 > 1 && num9 < Main.maxTilesY - 2 && Main.tile[num8, num9].active() && Main.tile[num8, num9].type == TileID.LifeFruit)
                                    {
                                        flag4 = false;
                                        break;
                                    }
                                }
                            }
                            if (flag4)
                            {
                                WorldGen.PlaceJunglePlant(i, minJ, TileID.LifeFruit, WorldGen.genRand.Next(3), 0);
                                WorldGen.SquareTileFrame(i, minJ);
                                WorldGen.SquareTileFrame(i + 1, minJ + 1);
                                if (Main.tile[i, minJ].type == TileID.LifeFruit && Main.netMode == 2)
                                {
                                    NetMessage.SendTileSquare(-1, i, minJ, 4);
                                }
                            }
                            break;
                        }
                        WorldGen.PlaceJunglePlant(i, minJ, 233, WorldGen.genRand.Next(8), 0);
                        if (Main.tile[i, minJ].type != TileID.PlantDetritus)
                        {
                            break;
                        }
                        //什么东西，看不懂
                        //if (Main.netMode == 2)
                        //{
                        NetMessage.SendTileSquare(-1, i, minJ, 4);
                        //    break;
                        //}
                        //WorldGen.PlaceJunglePlant(i, minJ, 233, WorldGen.genRand.Next(12), 1);
                        //if (Main.tile[i, minJ].type == 233 && Main.netMode == 2)
                        //{
                        //    NetMessage.SendTileSquare(-1, i, minJ, 3);
                        //}
                        break;
                    }
                case 70:
                    if (Main.tile[i, j - 1].liquid > 0)
                    {
                        WorldGen.PlaceCatTail(i, j - 1);
                    }
                    if (WorldGen.genRand.Next(250) == 0 && WorldGen.GrowTree(i, j) && WorldGen.PlayerLOS(i, j))
                    {
                        WorldGen.TreeGrowFXCheck(i, j - 1);
                    }
                    break;
            }
            return;
        }
        int num10 = Main.tile[i, j].type;
        switch (num10)
        {
            case 2:
            case 23:
            case 32:
            case 109:
            case 199:
            case 352:
            case 477:
            case 492:
            case 661:
            case 662:
                {
                    if (Main.halloween && WorldGen.genRand.Next(75) == 0 && (num10 == 2 || num10 == 109))
                    {
                        int num13 = 100;
                        int num14 = 0;
                        for (int num15 = i - num13; num15 < i + num13; num15 += 2)
                        {
                            for (int num16 = j - num13; num16 < j + num13; num16 += 2)
                            {
                                if (num15 > 1 && num15 < Main.maxTilesX - 2 && num16 > 1 && num16 < Main.maxTilesY - 2 && Main.tile[num15, num16].active() && Main.tile[num15, num16].type == TileID.Pumpkins)
                                {
                                    num14++;
                                }
                            }
                        }
                        if (num14 < 6)
                        {
                            WorldGen.PlacePumpkin(i, minJ);
                            if (Main.tile[i, minJ].type == TileID.Pumpkins)
                            {
                                NetMessage.SendTileSquare(-1, i - 1, minJ - 1, 2, 2);
                            }
                        }
                    }
                    if (!Main.tile[i, minJ].active() && Main.tile[i, minJ].liquid == 0)
                    {
                        int num17 = -1;
                        if (num10 == 2 && WorldGen.genRand.Next(12) == 0)
                        {
                            num17 = 3;
                        }
                        else if (num10 == 23 && WorldGen.genRand.Next(10) == 0)
                        {
                            num17 = 24;
                        }
                        else if (num10 == 199 && WorldGen.genRand.Next(10) == 0)
                        {
                            num17 = 201;
                        }
                        else if (num10 == 661 && WorldGen.genRand.Next(10) == 0)
                        {
                            num17 = 24;
                        }
                        else if (num10 == 662 && WorldGen.genRand.Next(10) == 0)
                        {
                            num17 = 201;
                        }
                        else if (num10 == 109 && WorldGen.genRand.Next(10) == 0)
                        {
                            num17 = 110;
                        }
                        else if (num10 == 633 && WorldGen.genRand.Next(10) == 0)
                        {
                            num17 = 637;
                        }
                        if (num17 != -1 && WorldGen.PlaceTile(i, minJ, num17, mute: true))
                        {
                            Main.tile[i, minJ].CopyPaintAndCoating(Main.tile[i, j]);
                            if (Main.netMode == 2)
                            {
                                NetMessage.SendTileSquare(-1, i, minJ);
                            }
                        }
                    }
                    bool flag6 = false;
                    switch (num10)
                    {
                        case 32:
                            num10 = 23;
                            if (!WorldGen.AllowedToSpreadInfections)
                            {
                                return;
                            }
                            break;
                        case 352:
                            num10 = 199;
                            if (!WorldGen.AllowedToSpreadInfections)
                            {
                                return;
                            }
                            break;
                        case 477:
                            num10 = 2;
                            break;
                        case 492:
                            num10 = 109;
                            break;
                    }
                    int grass = num10;
                    int num18 = -1;
                    if (num10 == 23 || num10 == 661)
                    {
                        grass = 23;
                        num18 = 661;
                    }
                    if (num10 == 199 || num10 == 662)
                    {
                        grass = 199;
                        num18 = 662;
                    }
                    bool flag7 = WorldGen.AllowedToSpreadInfections && (num10 == 23 || num10 == 199 || num10 == 109 || num10 == 492 || num10 == 661 || num10 == 662) && WorldGen.InWorld(i, j, 10);
                    for (int num19 = minI; num19 < maxI; num19++)
                    {
                        for (int num20 = minJ; num20 < maxJ; num20++)
                        {
                            if (!WorldGen.InWorld(num19, num20, 10) || (i == num19 && j == num20) || !Main.tile[num19, num20].active())
                            {
                                continue;
                            }
                            int type2 = Main.tile[num19, num20].type;
                            if (!flag7 && type2 != 0 && (num18 == -1 || type2 != 59))
                            {
                                continue;
                            }
                            TileColorCache color3 = Main.tile[i, j].BlockColorAndCoating();
                            if (type2 == 0 || (num18 > -1 && type2 == 59) || ((num10 == 23 || num10 == 661 || num10 == 199 || num10 == 662) && (type2 == 2 || type2 == 109 || type2 == 477 || type2 == 492)))
                            {
                                WorldGen.SpreadGrass(num19, num20, 0, grass, repeat: false, color3);
                                if (num18 > -1)
                                {
                                    WorldGen.SpreadGrass(num19, num20, 59, num18, repeat: false, color3);
                                }
                                if (WorldGen.AllowedToSpreadInfections && (num10 == 23 || num10 == 199 || num10 == 661 || num10 == 662))
                                {
                                    WorldGen.SpreadGrass(num19, num20, 2, grass, repeat: false, color3);
                                    WorldGen.SpreadGrass(num19, num20, 109, grass, repeat: false, color3);
                                    WorldGen.SpreadGrass(num19, num20, 477, grass, repeat: false, color3);
                                    WorldGen.SpreadGrass(num19, num20, 492, grass, repeat: false, color3);
                                    if (num18 > -1)
                                    {
                                        WorldGen.SpreadGrass(num19, num20, 60, num18, repeat: false, color3);
                                    }
                                }
                                if (Main.tile[num19, num20].type == num10 || (num18 > -1 && Main.tile[num19, num20].type == num18))
                                {
                                    WorldGen.SquareTileFrame(num19, num20);
                                    flag6 = true;
                                }
                            }
                            if (type2 == 0 || ((num10 == 109 || num10 == 492) && (type2 == 2 || type2 == 477 || type2 == 23 || type2 == 199)))
                            {
                                WorldGen.SpreadGrass(num19, num20, 0, grass, repeat: false, color3);
                                if (num10 == 109)
                                {
                                    WorldGen.SpreadGrass(num19, num20, 2, grass, repeat: false, color3);
                                }
                                switch (num10)
                                {
                                    case 492:
                                        WorldGen.SpreadGrass(num19, num20, 477, grass, repeat: false, color3);
                                        break;
                                    case 109:
                                        WorldGen.SpreadGrass(num19, num20, 477, 492, repeat: false, color3);
                                        break;
                                }
                                if ((num10 == 492 || num10 == 109) && WorldGen.AllowedToSpreadInfections)
                                {
                                    WorldGen.SpreadGrass(num19, num20, 23, 109, repeat: false, color3);
                                }
                                if ((num10 == 492 || num10 == 109) && WorldGen.AllowedToSpreadInfections)
                                {
                                    WorldGen.SpreadGrass(num19, num20, 199, 109, repeat: false, color3);
                                }
                                if (Main.tile[num19, num20].type == num10)
                                {
                                    WorldGen.SquareTileFrame(num19, num20);
                                    flag6 = true;
                                }
                            }
                        }
                    }
                    if (flag6)
                    {
                        NetMessage.SendTileSquare(-1, i, j, 3);
                    }
                    break;
                }
            case 70:
                {
                    if (!Main.tile[i, j].inActive())
                    {
                        if (!Main.tile[i, minJ].active() && WorldGen.genRand.Next(10) == 0)
                        {
                            WorldGen.PlaceTile(i, minJ, 71, mute: true);
                            if (Main.tile[i, minJ].active())
                            {
                                Main.tile[i, minJ].CopyPaintAndCoating(Main.tile[i, j]);
                            }
                            if (Main.tile[i, minJ].active())
                            {
                                NetMessage.SendTileSquare(-1, i, minJ);
                            }
                        }
                        if (WorldGen.genRand.Next(300) == 0)
                        {
                            bool flag8 = WorldGen.PlayerLOS(i, j);
                            if (WorldGen.GrowTree(i, j) && flag8)
                            {
                                WorldGen.TreeGrowFXCheck(i, j - 1);
                            }
                        }
                    }
                    bool flag9 = false;
                    TileColorCache color4 = Main.tile[i, j].BlockColorAndCoating();
                    for (int num21 = minI; num21 < maxI; num21++)
                    {
                        for (int num22 = minJ; num22 < maxJ; num22++)
                        {
                            if ((i != num21 || j != num22) && Main.tile[num21, num22].active() && Main.tile[num21, num22].type == TileID.Mud)
                            {
                                WorldGen.SpreadGrass(num21, num22, 59, num10, repeat: false, color4);
                                if (Main.tile[num21, num22].type == num10)
                                {
                                    WorldGen.SquareTileFrame(num21, num22);
                                    flag9 = true;
                                }
                            }
                        }
                    }
                    if (flag9)
                    {
                        NetMessage.SendTileSquare(-1, i, j, 3);
                    }
                    break;
                }
            case 60:
                {
                    if (!Main.tile[i, minJ].active() && WorldGen.genRand.Next(7) == 0)
                    {
                        WorldGen.PlaceTile(i, minJ, 61, mute: true);
                        if (Main.tile[i, minJ].active())
                        {
                            Main.tile[i, minJ].CopyPaintAndCoating(Main.tile[i, j]);
                        }
                        if (Main.tile[i, minJ].active())
                        {
                            NetMessage.SendTileSquare(-1, i, minJ);
                        }
                    }
                    else if (WorldGen.genRand.Next(500) == 0 && (!Main.tile[i, minJ].active() || Main.tile[i, minJ].type == TileID.JunglePlants || Main.tile[i, minJ].type == TileID.JunglePlants2 || Main.tile[i, minJ].type == TileID.JungleThorns))
                    {
                        if (WorldGen.GrowTree(i, j) && WorldGen.PlayerLOS(i, j))
                        {
                            WorldGen.TreeGrowFXCheck(i, j - 1);
                        }
                    }
                    else if (WorldGen.genRand.Next(25) == 0 && Main.tile[i, minJ].liquid == 0)
                    {
                        WorldGen.PlaceJunglePlant(i, minJ, 233, WorldGen.genRand.Next(8), 0);
                        if (Main.tile[i, minJ].type == TileID.PlantDetritus)
                        {
                            if (Main.netMode == 2)
                            {
                                NetMessage.SendTileSquare(-1, i, minJ, 4);
                            }
                            else
                            {
                                WorldGen.PlaceJunglePlant(i, minJ, 233, WorldGen.genRand.Next(12), 1);
                                if (Main.tile[i, minJ].type == TileID.PlantDetritus && Main.netMode == 2)
                                {
                                    NetMessage.SendTileSquare(-1, i, minJ, 3);
                                }
                            }
                        }
                    }
                    bool flag10 = false;
                    TileColorCache color5 = Main.tile[i, j].BlockColorAndCoating();
                    for (int num23 = minI; num23 < maxI; num23++)
                    {
                        for (int num24 = minJ; num24 < maxJ; num24++)
                        {
                            if ((i != num23 || j != num24) && Main.tile[num23, num24].active() && Main.tile[num23, num24].type == TileID.Mud)
                            {
                                WorldGen.SpreadGrass(num23, num24, 59, num10, repeat: false, color5);
                                if (Main.tile[num23, num24].type == num10)
                                {
                                    WorldGen.SquareTileFrame(num23, num24);
                                    flag10 = true;
                                }
                            }
                        }
                    }
                    if (flag10)
                    {
                        NetMessage.SendTileSquare(-1, i, j, 3);
                    }
                    break;
                }
            case 633:
                {
                    if (!Main.tile[i, minJ].active() && WorldGen.genRand.Next(10) == 0)
                    {
                        WorldGen.PlaceTile(i, minJ, 637, mute: true);
                        if (Main.tile[i, minJ].active())
                        {
                            Main.tile[i, minJ].CopyPaintAndCoating(Main.tile[i, j]);
                        }
                        if (Main.tile[i, minJ].active())
                        {
                            NetMessage.SendTileSquare(-1, i, minJ);
                        }
                    }
                    TileColorCache color2 = Main.tile[i, j].BlockColorAndCoating();
                    bool flag5 = false;
                    for (int num11 = minI; num11 < maxI; num11++)
                    {
                        for (int num12 = minJ; num12 < maxJ; num12++)
                        {
                            if ((i != num11 || j != num12) && Main.tile[num11, num12].active() && Main.tile[num11, num12].type == TileID.Ash)
                            {
                                WorldGen.SpreadGrass(num11, num12, 57, num10, repeat: false, color2);
                                if (Main.tile[num11, num12].type == num10)
                                {
                                    WorldGen.SquareTileFrame(num11, num12);
                                    flag5 = true;
                                }
                            }
                        }
                    }
                    if (flag5)
                    {
                        NetMessage.SendTileSquare(-1, i, j, 3);
                    }
                    break;
                }
        }
    }
    [DetourMethod]
    public static void CheckOrb(int i, int j, int type)
    {
        if (Main.tile[i, j] == null)
        {
            return;
        }
        short frameX = Main.tile[i, j].frameX;
        bool crimson = false;
        if (frameX >= 36)
        {
            crimson = true;
        }
        if (WorldGen.destroyObject)
        {
            return;
        }

        int num = Main.tile[i, j].frameX != 0 && Main.tile[i, j].frameX != 36 ? i - 1 : i;
        int num2 = Main.tile[i, j].frameY != 0 ? j - 1 : j;
        for (int k = 0; k < 2; k++)
        {
            for (int l = 0; l < 2; l++)
            {
                ITile tile = Main.tile[num + k, num2 + l];
                if (tile != null && (!tile.nactive() || tile.type != type))
                {
                    WorldGen.destroyObject = true;
                    break;
                }
            }
            if (WorldGen.destroyObject)
            {
                break;
            }
            if (type == 12 || type == 639)
            {
                ITile tile = Main.tile[num + k, num2 + 2];
                if (tile != null && !WorldGen.SolidTileAllowBottomSlope(num + k, num2 + 2))
                {
                    WorldGen.destroyObject = true;
                    break;
                }
            }
        }
        if (!WorldGen.destroyObject)
        {
            return;
        }
        for (int m = num; m < num + 2; m++)
        {
            for (int n = num2; n < num2 + 2; n++)
            {
                if (Main.tile[m, n] != null && Main.tile[m, n].type == type)
                {
                    WorldGen.KillTile(m, n);
                }
            }
        }
        if (!WorldGen.noTileActions)
        {
            switch (type)
            {
                case 12:
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(num, num2), num * 16, num2 * 16, 32, 32, ItemID.LifeCrystal);
                    break;
                case 639:
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(num, num2), num * 16, num2 * 16, 32, 32, ItemID.ManaCrystal);
                    break;
                case TileID.ShadowOrbs:
                    var config = GameContentModify.MainConfig.Instance.Orb;
                    var array = crimson ? config.CrimsonShadowOrbDropItems : config.CorruptionShadowOrbDropItems;
                    int index = Main.rand.Next(array.Length);
                    var items = array[index];
                    foreach (var item in items)
                    {
                        item.NewItem(WorldGen.GetItemSource_FromTileBreak(num, num2), num, num2);
                    }
                    WorldGen.shadowOrbSmashed = true;
                    WorldGen.shadowOrbCount++;
                    if (WorldGen.shadowOrbCount >= config.SpanwNPCSmashedCount)
                    {
                        if (!(NPC.AnyNPCs(NPCID.BrainofCthulhu) && crimson) && (!NPC.AnyNPCs(NPCID.EaterofWorldsHead) || crimson))
                        {
                            WorldGen.shadowOrbCount = 0;
                            float num5 = num * 16;
                            float num6 = num2 * 16;
                            float num7 = -1f;
                            int plr = 0;
                            for (int num8 = 0; num8 < 255; num8++)
                            {
                                float num9 = Math.Abs(Main.player[num8].position.X - num5) + Math.Abs(Main.player[num8].position.Y - num6);
                                if (num9 < num7 || num7 == -1f)
                                {
                                    plr = num8;
                                    num7 = num9;
                                }
                            }
                            if (crimson)
                            {
                                NPC.SpawnOnPlayer(plr, NPCID.BrainofCthulhu);
                            }
                            else
                            {
                                NPC.SpawnOnPlayer(plr, NPCID.EaterofWorldsHead);
                            }
                        }
                    }
                    else
                    {
                        LocalizedText localizedText = Lang.misc[10];
                        if (WorldGen.shadowOrbCount >= config.SpanwNPCSmashedCount - 1)
                        {
                            localizedText = Lang.misc[11];
                        }
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey(localizedText.Key), new Color(50, 255, 130));
                    }
                    AchievementsHelper.NotifyProgressionEvent(AchievementHelperID.Events.SmashShadowOrb);
                    break;
            }
        }
        WorldGen.destroyObject = false;
    }
    [DetourMethod]
    public static void Check3x2(int i, int j, int type)
    {
        if (WorldGen.destroyObject)
        {
            return;
        }
        bool flag = false;
        bool flag2 = false;
        int num = j;
        if (Main.tile[i, j] == null)
        {
            Main.tile[i, j] = OTAPI.Hooks.Tile.InvokeCreate();
        }
        int num2 = 36;
        int num3 = Main.tile[i, j].frameY / num2;
        int num4 = Main.tile[i, j].frameY % num2;
        num -= num4 / 18;
        int num5 = Main.tile[i, j].frameX / 18;
        int num6 = 0;
        while (num5 > 2)
        {
            num5 -= 3;
            num6++;
        }
        num5 = i - num5;
        int num7 = num6 * 54;
        if (type == 14 && num6 == 25)
        {
            flag2 = true;
        }
        int num8 = num + 2;
        if (flag2)
        {
            num8--;
        }
        for (int k = num5; k < num5 + 3; k++)
        {
            for (int l = num; l < num8; l++)
            {
                if (Main.tile[k, l] == null)
                {
                    Main.tile[k, l] = OTAPI.Hooks.Tile.InvokeCreate();
                }
                if (!Main.tile[k, l].active() || Main.tile[k, l].type != type || Main.tile[k, l].frameX != (k - num5) * 18 + num7 || Main.tile[k, l].frameY != (l - num) * 18 + num3 * 36)
                {
                    flag = true;
                }
            }
            if (type == 285 || type == 286 || type == 298 || type == 299 || type == 310 || type == 339 || type == 538 || (type >= 361 && type <= 364) || type == 532 || type == 544 || type == 533 || type == 555 || type == 556 || type == 582 || type == 619 || type == 629)
            {
                if (!WorldGen.SolidTileAllowBottomSlope(k, num8) && (Main.tile[k, num8] == null || !Main.tile[k, num8].nactive() || !Main.tileSolidTop[Main.tile[k, num8].type] || Main.tile[k, num8].frameY != 0) && (Main.tile[k, num8] == null || !Main.tile[k, num8].active() || !TileID.Sets.Platforms[Main.tile[k, num8].type]))
                {
                    flag = true;
                }
            }
            else
            {
                switch (type)
                {
                    case 488:
                        {
                            int num9 = 0;
                            if (Main.tile[k, num8] != null && Main.tile[k, num8].active())
                            {
                                num9 = Main.tile[k, num8].type;
                            }
                            if (num9 != 2 && num9 != 477 && num9 != 109 && num9 != 492)
                            {
                                flag = true;
                            }
                            break;
                        }
                    case 26:
                        {
                            ITile tile2 = Main.tile[k, num8];
                            if (!WorldGen.SolidTileAllowBottomSlope(k, num8) || (tile2 != null && tile2.active() && TileID.Sets.Boulders[tile2.type]))
                            {
                                flag = true;
                            }
                            break;
                        }
                    case 186:
                        {
                            if (!WorldGen.SolidTileAllowBottomSlope(k, num8))
                            {
                                flag = true;
                                break;
                            }
                            ITile tile = Main.tile[k, num8];
                            if (tile == null || !tile.active())
                            {
                                break;
                            }
                            switch (num6)
                            {
                                case 26:
                                case 27:
                                case 28:
                                case 29:
                                case 30:
                                case 31:
                                    if (!TileID.Sets.Snow[tile.type] && !TileID.Sets.Conversion.Ice[tile.type] && tile.type != TileID.BreakableIce && tile.type != TileID.Slush)
                                    {
                                        flag = true;
                                    }
                                    break;
                                case 32:
                                case 33:
                                case 34:
                                    if (!TileID.Sets.Mud[tile.type] && tile.type != TileID.MushroomGrass)
                                    {
                                        flag = true;
                                    }
                                    break;
                            }
                            break;
                        }
                }
            }
            if (type == 187)
            {
                if (!WorldGen.SolidTileAllowBottomSlope(k, num8))
                {
                    flag = true;
                    continue;
                }
                ITile tile3 = Main.tile[k, num8];
                if (tile3 == null || !tile3.active())
                {
                    continue;
                }
                switch (num6)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        if (!TileID.Sets.Mud[tile3.type] && tile3.type != TileID.JungleGrass && tile3.type != TileID.LihzahrdBrick)
                        {
                            flag = true;
                        }
                        break;
                    case 6:
                    case 7:
                    case 8:
                        if (tile3.type != TileID.Ash && tile3.type != TileID.Hellstone && tile3.type != TileID.ObsidianBrick && tile3.type != TileID.HellstoneBrick)
                        {
                            flag = true;
                        }
                        break;
                    case 29:
                    case 30:
                    case 31:
                    case 32:
                    case 33:
                    case 34:
                        if (!TileID.Sets.Conversion.Sand[tile3.type] && !TileID.Sets.Conversion.HardenedSand[tile3.type] && !TileID.Sets.Conversion.Sandstone[tile3.type])
                        {
                            flag = true;
                        }
                        break;
                }
            }
            else if (!WorldGen.SolidTileAllowBottomSlope(k, num8))
            {
                flag = true;
            }
        }
        if (type == 187 && Main.tile[num5, num] != null && Main.tile[num5, num].frameX >= 756 && Main.tile[num5, num].frameX <= 900 && Main.tile[num5, num + 2].type != TileID.Grass && Main.tile[num5 + 1, num + 2].type != TileID.Grass && Main.tile[num5 + 2, num + 2].type != TileID.Grass && Main.tile[num5, num + 2].type != TileID.GolfGrass && Main.tile[num5 + 1, num + 2].type != TileID.GolfGrass && Main.tile[num5 + 2, num + 2].type != TileID.GolfGrass && Main.tile[num5, num + 2].type != TileID.GolfGrassHallowed && Main.tile[num5 + 1, num + 2].type != TileID.GolfGrassHallowed && Main.tile[num5 + 2, num + 2].type != TileID.GolfGrassHallowed)
        {
            Main.tile[num5, num].frameX -= 378;
            Main.tile[num5 + 1, num].frameX -= 378;
            Main.tile[num5 + 2, num].frameX -= 378;
            Main.tile[num5, num + 1].frameX -= 378;
            Main.tile[num5 + 1, num + 1].frameX -= 378;
            Main.tile[num5 + 2, num + 1].frameX -= 378;
            Main.tile[num5, num].type = TileID.LargePiles;
            Main.tile[num5 + 1, num].type = TileID.LargePiles;
            Main.tile[num5 + 2, num].type = TileID.LargePiles;
            Main.tile[num5, num + 1].type = TileID.LargePiles;
            Main.tile[num5 + 1, num + 1].type = TileID.LargePiles;
            Main.tile[num5 + 2, num + 1].type = TileID.LargePiles;
        }
        if (flag && type == 488 && WorldGen.gen)
        {
            for (int m = num5; m < num5 + 3; m++)
            {
                for (int n = num; n < num + 2; n++)
                {
                    Main.tile[m, n].active(active: true);
                    Main.tile[m, n].type = TileID.FallenLog;
                    Main.tile[m, n].frameX = (short)((m - num5) * 18);
                    Main.tile[m, n].frameY = (short)((n - num) * 18);
                }
                Main.tile[m, num + 2].active(active: true);
                Main.tile[m, num + 2].type = TileID.Grass;
                Main.tile[m, num + 2].slope(0);
                Main.tile[m, num + 2].halfBrick(halfBrick: false);
            }
            flag = false;
        }
        if (!flag)
        {
            return;
        }
        int frameX = Main.tile[i, j].frameX;
        WorldGen.destroyObject = true;
        num8 = num + 3;
        if (flag2)
        {
            num8--;
        }
        for (int num10 = num5; num10 < num5 + 3; num10++)
        {
            for (int num11 = num; num11 < num + 3; num11++)
            {
                Main.tile[num10, num11] ??= OTAPI.Hooks.Tile.InvokeCreate();
                if (Main.tile[num10, num11].type == type && Main.tile[num10, num11].active())
                {
                    WorldGen.KillTile(num10, num11);
                }
            }
        }
        if (type == 14)
        {
            int type2 = ((num6 >= 1 && num6 <= 3) ? (637 + num6) : ((num6 >= 15 && num6 <= 20) ? (1698 + num6) : ((num6 >= 4 && num6 <= 7) ? (823 + num6) : (num6 switch
            {
                8 => 917,
                9 => 1144,
                10 => 1397,
                11 => 1400,
                12 => 1403,
                13 => 1460,
                14 => 1510,
                23 => 1926,
                21 => 1794,
                22 => 1816,
                24 => 2248,
                25 => 2259,
                26 => 2532,
                27 => 2550,
                28 => 677,
                29 => 2583,
                30 => 2743,
                31 => 2824,
                32 => 3153,
                33 => 3155,
                34 => 3154,
                _ => 32,
            }))));
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, type2);
        }
        switch (type)
        {
            case 469:
                {
                    int type4 = num6 switch
                    {
                        1 => 3948,
                        2 => 3974,
                        3 => 4162,
                        4 => 4183,
                        5 => 4204,
                        6 => 4225,
                        7 => 4314,
                        8 => 4583,
                        9 => 5165,
                        10 => 5186,
                        11 => 5207,
                        _ => 3920,
                    };
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, type4);
                    break;
                }
            case 114:
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.TinkerersWorkshop);
                break;
            case 26:
                if (!WorldGen.noTileActions && !WorldGen.IsGeneratingHardMode)
                {
                    WorldGen.SmashAltar(i, j);
                }
                break;
            case 298:
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.FrogCage);
                break;
            case 299:
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.MouseCage);
                break;
            case 361:
            case 362:
            case 363:
            case 364:
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, 3073 + type - 361);
                break;
            default:
                if (type >= 391 && type <= 394)
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 48, 32, 3254 + type - 391);
                    break;
                }
                switch (type)
                {
                    case 285:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.SnailCage);
                        break;
                    case 286:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.GlowingSnailCage);
                        break;
                    case 582:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.MagmaSnailCage);
                        break;
                    case 619:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.TruffleWormCage);
                        break;
                    case 310:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.WormCage);
                        break;
                    case 339:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.GrasshopperCage);
                        break;
                    case 538:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.LadybugCage);
                        break;
                    case 544:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.GoldLadybugCage);
                        break;
                    case 532:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.MaggotCage);
                        break;
                    case 533:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.RatCage);
                        break;
                    case 555:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.WaterStriderCage);
                        break;
                    case 556:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.GoldWaterStriderCage);
                        break;
                    case 629:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.StinkbugCage);
                        break;
                    case 217:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.BlendOMatic);
                        break;
                    case 218:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.MeatGrinder);
                        break;
                    case 219:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.Extractinator);
                        break;
                    case 642:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.ChlorophyteExtractinator);
                        break;
                    case 220:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.Solidifier);
                        break;
                    case 377:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.SharpeningStation);
                        break;
                    case 228:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.DyeVat);
                        break;
                    case 405:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.Fireplace);
                        break;
                    case 486:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.DrumSet);
                        break;
                    case 488:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.Wood, WorldGen.genRand.Next(10, 21));
                        break;
                    case 215:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, WorldGen.GetCampfireItemDrop(num6));
                        break;
                    case 244:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.BubbleMachine);
                        break;
                    case 647:
                        {
                            int num13 = 0;
                            if (num6 < 7)
                            {
                                num13 = 154;
                            }
                            else if (num6 < 13)
                            {
                                num13 = 3;
                            }
                            else if (num6 < 16)
                            {
                                num13 = 3;
                            }
                            else if (num6 < 18)
                            {
                                num13 = 71;
                            }
                            else if (num6 < 20)
                            {
                                num13 = 72;
                            }
                            else if (num6 < 22)
                            {
                                num13 = 73;
                            }
                            else if (num6 < 26)
                            {
                                num13 = 9;
                            }
                            else if (num6 < 32)
                            {
                                num13 = 593;
                            }
                            else if (num6 < 35)
                            {
                                num13 = 183;
                            }
                            if (num13 != 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, num13);
                            }
                            break;
                        }
                    case 648:
                        {
                            num6 += num3 * 35;
                            int num12 = 0;
                            if (num6 < 6)
                            {
                                num12 = 195;
                            }
                            else if (num6 < 9)
                            {
                                num12 = 174;
                            }
                            else if (num6 < 14)
                            {
                                num12 = 150;
                            }
                            else if (num6 < 17)
                            {
                                num12 = 3;
                            }
                            else if (num6 < 18)
                            {
                                num12 = 989;
                            }
                            else if (num6 < 21)
                            {
                                num12 = 1101;
                            }
                            else if (num6 < 29)
                            {
                                num12 = 9;
                            }
                            else if (num6 < 35)
                            {
                                num12 = 3271;
                            }
                            else if (num6 < 41)
                            {
                                num12 = 3086;
                            }
                            else if (num6 < 47)
                            {
                                num12 = 3081;
                            }
                            else if (num6 < 52)
                            {
                                num12 = 62;
                            }
                            else if (num6 < 55)
                            {
                                num12 = 154;
                            }
                            if (num12 != 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, num12);
                            }
                            break;
                        }
                    case 651:
                        {
                            int num14 = 0;
                            num14 = ((num6 < 3) ? 195 : ((num6 >= 6) ? 331 : 62));
                            if (num14 != 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, num14);
                            }
                            break;
                        }
                    case 17:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.Furnace);
                        break;
                    case 77:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.Hellforge);
                        break;
                    case 86:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.Loom);
                        break;
                    case 237:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.LihzahrdAltar);
                        break;
                    case 87:
                        {
                            int type3;
                            if (num6 >= 1 && num6 <= 3)
                            {
                                type3 = 640 + num6;
                            }
                            else
                            {
                                switch (num6)
                                {
                                    case 4:
                                        type3 = 919;
                                        break;
                                    case 5:
                                    case 6:
                                    case 7:
                                        type3 = 2245 + num6 - 5;
                                        break;
                                    default:
                                        type3 = ((num6 >= 8 && num6 <= 10) ? (2254 + num6 - 8) : ((num6 >= 11 && num6 <= 20) ? (2376 + num6 - 11) : (num6 switch
                                        {
                                            21 => 2531,
                                            22 => 2548,
                                            23 => 2565,
                                            24 => 2580,
                                            25 => 2671,
                                            26 => 2821,
                                            27 => 3141,
                                            28 => 3143,
                                            29 => 3142,
                                            30 => 3915,
                                            31 => 3916,
                                            32 => 3944,
                                            33 => 3971,
                                            34 => 4158,
                                            35 => 4179,
                                            36 => 4200,
                                            37 => 4221,
                                            38 => 4310,
                                            39 => 4579,
                                            40 => 5161,
                                            41 => 5182,
                                            42 => 5203,
                                            _ => 333,
                                        })));
                                        break;
                                }
                            }
                            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, type3);
                            break;
                        }
                    case 88:
                        {
                            int dresserItemDrop = WorldGen.GetDresserItemDrop(num6);
                            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, dresserItemDrop);
                            break;
                        }
                    case 89:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, WorldGen.GetItemDrop_Benches(num6));
                        break;
                    case 133:
                        if (frameX >= 54)
                        {
                            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.TitaniumForge);
                        }
                        else
                        {
                            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.AdamantiteForge);
                        }
                        break;
                    case TileID.LargePiles:
                        //if (frameX < 864)
                        if (frameX < TileStyleID.LargePiles.LargeCopperCoinStash1 * TileSize.S2)
                        {
                            break;
                        }
                        if (frameX <= 954)
                        {
                            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.CopperCoin, WorldGen.genRand.Next(20, 100));
                            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.CopperCoin, WorldGen.genRand.Next(30, 100));
                            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.CopperCoin, WorldGen.genRand.Next(40, 100));
                            if (WorldGen.genRand.Next(3) != 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.CopperCoin, WorldGen.genRand.Next(20, 100));
                            }
                            if (WorldGen.genRand.Next(3) != 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.CopperCoin, WorldGen.genRand.Next(30, 100));
                            }
                            if (WorldGen.genRand.Next(3) != 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.CopperCoin, WorldGen.genRand.Next(40, 100));
                            }
                            if (WorldGen.genRand.Next(2) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.CopperCoin, WorldGen.genRand.Next(20, 100));
                            }
                            if (WorldGen.genRand.Next(2) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.CopperCoin, WorldGen.genRand.Next(30, 100));
                            }
                            if (WorldGen.genRand.Next(2) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.CopperCoin, WorldGen.genRand.Next(40, 100));
                            }
                            if (WorldGen.genRand.Next(3) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.CopperCoin, WorldGen.genRand.Next(20, 100));
                            }
                            if (WorldGen.genRand.Next(3) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.CopperCoin, WorldGen.genRand.Next(30, 100));
                            }
                            if (WorldGen.genRand.Next(3) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.CopperCoin, WorldGen.genRand.Next(40, 100));
                            }
                            if (WorldGen.genRand.Next(4) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.CopperCoin, WorldGen.genRand.Next(20, 100));
                            }
                            if (WorldGen.genRand.Next(4) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.CopperCoin, WorldGen.genRand.Next(30, 100));
                            }
                            if (WorldGen.genRand.Next(4) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.CopperCoin, WorldGen.genRand.Next(40, 100));
                            }
                            if (WorldGen.genRand.Next(5) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.CopperCoin, WorldGen.genRand.Next(20, 100));
                            }
                            if (WorldGen.genRand.Next(5) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.CopperCoin, WorldGen.genRand.Next(30, 100));
                            }
                            if (WorldGen.genRand.Next(5) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.CopperCoin, WorldGen.genRand.Next(40, 100));
                            }
                        }
                        else if (frameX <= 1062)
                        {
                            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.SilverCoin, WorldGen.genRand.Next(10, 100));
                            if (WorldGen.genRand.Next(2) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.SilverCoin, WorldGen.genRand.Next(20, 100));
                            }
                            if (WorldGen.genRand.Next(3) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.SilverCoin, WorldGen.genRand.Next(30, 100));
                            }
                            if (WorldGen.genRand.Next(4) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.SilverCoin, WorldGen.genRand.Next(40, 100));
                            }
                            if (WorldGen.genRand.Next(5) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.SilverCoin, WorldGen.genRand.Next(50, 100));
                            }
                        }
                        else if (frameX <= 1170)
                        {
                            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.GoldCoin, WorldGen.genRand.Next(1, 7));
                            if (WorldGen.genRand.Next(2) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.GoldCoin, WorldGen.genRand.Next(2, 7));
                            }
                            if (WorldGen.genRand.Next(3) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.GoldCoin, WorldGen.genRand.Next(3, 7));
                            }
                            if (WorldGen.genRand.Next(4) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.GoldCoin, WorldGen.genRand.Next(4, 7));
                            }
                            if (WorldGen.genRand.Next(5) == 0)
                            {
                                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.GoldCoin, WorldGen.genRand.Next(5, 7));
                            }
                        }
                        break;
                    case TileID.LargePiles2:
                        //if (frameX >= 918 && frameX <= 970) //奇怪为什么是970，style18应该是972, 小于等于的话是971
                        if (frameX >= TileStyleID.LargePiles2.EnchantedSwordInStone * TileSize.S3 && frameX < (TileStyleID.LargePiles2.EnchantedSwordInStone + 1) * TileSize.S3)
                        {
                            var values = GameContentModify.MainConfig.Instance.EnchantedSwordInStoneDropItemInfo;
                            for (int k = 0; k < values.Length; k++)
                            {
                                if (values[k].TryDrop(i, j) && !values[k].Continute)
                                {
                                    break;
                                }
                            }
                        }
                        break;
                }
                break;
        }
        WorldGen.destroyObject = false;
        for (int num15 = num5 - 1; num15 < num5 + 4; num15++)
        {
            for (int num16 = num - 1; num16 < num + 4; num16++)
            {
                WorldGen.TileFrame(num15, num16);
            }
        }
        if (type == 488)
        {
            WorldGen.mysticLogsEvent.FallenLogDestroyed();
        }
    }
    public static int[] treeShakeCount = new int[500];
    public static void ShakeTree(int i, int j)
    {
        if (WorldGen.numTreeShakes == WorldGen.maxTreeShakes)
        {
            return;
        }
        WorldGen.GetTreeBottom(i, j, out var x, out var y);
        int y2 = y;
        TreeTypes treeType = WorldGen.GetTreeType(Main.tile[x, y].type);
        if (treeType == TreeTypes.None)
        {
            return;
        }
        for (int k = 0; k < WorldGen.numTreeShakes; k++)
        {
            if (WorldGen.treeShakeX[k] == x && WorldGen.treeShakeY[k] == y)
            {
                return;
            }
        }
        WorldGen.treeShakeX[WorldGen.numTreeShakes] = x;
        WorldGen.treeShakeY[WorldGen.numTreeShakes] = y;
        WorldGen.numTreeShakes++;
        y--;
        while (y > 10 && Main.tile[x, y].active() && TileID.Sets.IsShakeable[Main.tile[x, y].type])
        {
            y--;
        }
        y++;
        if (!WorldGen.IsTileALeafyTreeTop(x, y) || Collision.SolidTiles(x - 2, x + 2, y - 2, y + 2))
        {
            return;
        }
        var rand = WorldGen.genRand;
        if (!MainConfigInfo.StaticDisableShakeTreeDropBombProj && Main.getGoodWorld && rand.Next(17) == 0)
        {
            Projectile.NewProjectile(WorldGen.GetProjectileSource_ShakeTree(x, y), x * 16, y * 16, Main.rand.Next(-100, 101) * 0.002f, 0f, ProjectileID.Bomb, 0, 0f, Main.myPlayer, 16f, 16f);
        }
        else if (rand.Next(300) == 0 && treeType == TreeTypes.Forest)
        {
            newItem(832);
        }
        else if (rand.Next(300) == 0 && treeType == TreeTypes.Forest)
        {
            newItem(933);
        }
        else if (rand.Next(200) == 0 && treeType == TreeTypes.Jungle)
        {
            newItem(3360);
        }
        else if (rand.Next(200) == 0 && treeType == TreeTypes.Jungle)
        {
            newItem(3361);
        }
        else if (rand.Next(1000) == 0 && treeType == TreeTypes.Forest)
        {
            newItem(4366);
        }
        else if (rand.Next(7) == 0 && (treeType == TreeTypes.Forest || treeType == TreeTypes.Snow || treeType == TreeTypes.Hallowed || treeType == TreeTypes.Ash))
        {
            newItem(27, rand.Next(1, 3));
        }
        else if (rand.Next(8) == 0 && treeType == TreeTypes.Mushroom)
        {
            newItem(194, rand.Next(1, 2));
        }
        else if (rand.Next(35) == 0 && Main.halloween)
        {
            newItem(1809, rand.Next(1, 3));
        }
        else if (rand.Next(12) == 0)
        {
            WorldGen.KillTile_GetItemDrops(i, j, Main.tile[i, j], out var dropItem, out var _, out var _, out var _);
            newItem(dropItem, rand.Next(1, 4));
        }
        else if (rand.Next(20) == 0)
        {
            int type = 71;
            int num = rand.Next(50, 100);
            if (rand.Next(30) == 0)
            {
                type = 73;
                num = 1;
                if (rand.Next(5) == 0)
                {
                    num++;
                }
                if (rand.Next(10) == 0)
                {
                    num++;
                }
            }
            else if (rand.Next(10) == 0)
            {
                type = 72;
                num = rand.Next(1, 21);
                if (rand.Next(3) == 0)
                {
                    num += rand.Next(1, 21);
                }
                if (rand.Next(4) == 0)
                {
                    num += rand.Next(1, 21);
                }
            }
            newItem(type, num);
        }
        else if (rand.Next(15) == 0 && (treeType == TreeTypes.Forest || treeType == TreeTypes.Hallowed))
        {
            int type2 = rand.Next(5) switch
            {
                0 => 74,
                1 => 297,
                2 => 298,
                3 => 299,
                _ => 538,
            };
            if (Player.GetClosestRollLuck(x, y, NPC.goldCritterChance) == 0f)
            {
                type2 = ((rand.Next(2) != 0) ? 539 : 442);
            }
            newNPC(type2);
        }
        else if (rand.Next(50) == 0 && treeType == TreeTypes.Hallowed && !Main.dayTime)
        {
            int type3 = Main.rand.NextFromList(new short[3] { 583, 584, 585 });
            if (Main.tenthAnniversaryWorld && Main.rand.Next(4) != 0)
            {
                type3 = 583;
            }
            newNPC(type3);
        }
        else if (rand.Next(50) == 0 && treeType == TreeTypes.Forest && !Main.dayTime)
        {
            NPC obj = Main.npc[newNPC(611)];
            obj.velocity.Y = 1f;
            obj.netUpdate = true;
        }
        else if (rand.Next(50) == 0 && treeType == TreeTypes.Jungle && Main.dayTime)
        {
            NPC obj2 = Main.npc[newNPC(Main.rand.NextFromList(new short[5] { 671, 672, 673, 674, 675 }))];
            obj2.velocity.Y = 1f;
            obj2.netUpdate = true;
        }
        else if (rand.Next(40) == 0 && treeType == TreeTypes.Forest && !Main.dayTime && Main.halloween)
        {
            newNPC(301);
        }
        else if (rand.Next(50) == 0 && (treeType == TreeTypes.Forest || treeType == TreeTypes.Hallowed))
        {
            for (int l = 0; l < 5; l++)
            {
                var point = new Point(x + Main.rand.Next(-2, 2), y - 1 + Main.rand.Next(-2, 2));
                int type4 = ((Player.GetClosestRollLuck(x, y, NPC.goldCritterChance) != 0f) ? Main.rand.NextFromList(new short[3] { 74, 297, 298 }) : 442);
                NPC obj3 = Main.npc[NPC.NewNPC(new EntitySource_ShakeTree(x, y), point.X * 16, point.Y * 16, type4)];
                obj3.velocity = Main.rand.NextVector2CircularEdge(3f, 3f);
                obj3.netUpdate = true;
            }
        }
        else if (rand.Next(40) == 0 && treeType == TreeTypes.Jungle)
        {
            for (int m = 0; m < 5; m++)
            {
                var point2 = new Point(x + Main.rand.Next(-2, 2), y - 1 + Main.rand.Next(-2, 2));
                NPC obj4 = Main.npc[NPC.NewNPC(new EntitySource_ShakeTree(x, y), point2.X * 16, point2.Y * 16, Main.rand.NextFromList(new short[2] { 210, 211 }))];
                obj4.ai[1] = 65f;
                obj4.netUpdate = true;
            }
        }
        else if (rand.Next(20) == 0 && (treeType == TreeTypes.Palm || treeType == TreeTypes.PalmCorrupt || treeType == TreeTypes.PalmCrimson || treeType == TreeTypes.PalmHallowed) && !WorldGen.IsPalmOasisTree(x))
        {
            newNPC(603);
        }
        else if (rand.Next(30) == 0 && (treeType == TreeTypes.Crimson || treeType == TreeTypes.PalmCrimson))
        {
            NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16 + 8, (y - 1) * 16, -22);
        }
        else if (rand.Next(30) == 0 && (treeType == TreeTypes.Corrupt || treeType == TreeTypes.PalmCorrupt))
        {
            NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16 + 8, (y - 1) * 16, -11);
        }
        else if (rand.Next(30) == 0 && treeType == TreeTypes.Jungle && !Main.dayTime)
        {
            newNPC(51);
        }
        else if (rand.Next(40) == 0 && treeType == TreeTypes.Jungle)
        {
            Projectile.NewProjectile(WorldGen.GetProjectileSource_ShakeTree(x, y), x * 16 + 8, (y - 1) * 16, 0f, 0f, ProjectileID.BeeHive, 0, 0f, Main.myPlayer);
        }
        else if (rand.Next(20) == 0 && (treeType == TreeTypes.Forest || treeType == TreeTypes.Hallowed) && !Main.raining && !NPC.TooWindyForButterflies && Main.dayTime)
        {
            int type5 = 356;
            if (Player.GetClosestRollLuck(x, y, NPC.goldCritterChance) == 0f)
            {
                type5 = 444;
            }
            newNPC(type5);
        }
        else if (rand.Next(20) == 0 && treeType == TreeTypes.Ash && y > Main.maxTilesY - 250)
        {
            int num2 = rand.Next(3);
            newNPC(num2 switch
            {
                0 => 654,
                1 => 653,
                _ => 655,
            });
        }
        else if (Main.remixWorld && rand.Next(20) == 0 && treeType == TreeTypes.Ash && y > Main.maxTilesY - 250)
        {
            newItem(965, rand.Next(20, 41));
        }
        else if (rand.Next(12) == 0 && treeType == TreeTypes.Forest)
        {
            int num3 = rand.Next(5);
            newItem(num3 switch
            {
                0 => 4009,
                1 => 4293,
                2 => 4282,
                3 => 4290,
                _ => 4291,
            });
        }
        else if (rand.Next(12) == 0 && treeType == TreeTypes.Snow)
        {
            newItem(Type: (rand.Next(2) != 0) ? 4295 : 4286);
        }
        else if (rand.Next(12) == 0 && treeType == TreeTypes.Jungle)
        {
            newItem(Type: (rand.Next(2) != 0) ? 4292 : 4294);
        }
        else if (rand.Next(12) == 0 && (treeType == TreeTypes.Palm || treeType == TreeTypes.PalmCorrupt || treeType == TreeTypes.PalmCrimson || treeType == TreeTypes.PalmHallowed) && !WorldGen.IsPalmOasisTree(x))
        {
            newItem(Type: (rand.Next(2) != 0) ? 4287 : 4283);
        }
        else if (rand.Next(12) == 0 && (treeType == TreeTypes.Corrupt || treeType == TreeTypes.PalmCorrupt))
        {
            newItem(Type: (rand.Next(2) != 0) ? 4289 : 4284);
        }
        else if (rand.Next(12) == 0 && (treeType == TreeTypes.Hallowed || treeType == TreeTypes.PalmHallowed))
        {
            newItem(Type: (rand.Next(2) != 0) ? 4288 : 4297);
        }
        else if (rand.Next(12) == 0 && (treeType == TreeTypes.Crimson || treeType == TreeTypes.PalmCrimson))
        {
            newItem(Type: (rand.Next(2) != 0) ? 4285 : 4296);
        }
        else if (rand.Next(12) == 0 && treeType == TreeTypes.Ash)
        {
            newItem(Type: (rand.Next(2) != 0) ? 5278 : 5277);
        }
        int treeHeight = 0;
        WorldGen.GetTreeLeaf(x, Main.tile[x, y], Main.tile[x, y2], ref treeHeight, out var _, out var passStyle);
        if (passStyle != -1)
        {
            NetMessage.SendData(MessageID.SpecialFX, -1, -1, null, 1, x, y, 1f, passStyle);
        }
        void newItem(int Type, int Stack = 1) => Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, Type, Stack);
        int newNPC(int Type) => NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, Type);
    }
    public static unsafe void SpawnThingsFromPot(int i, int j, int x2, int y2, int style)
    {
        bool flag = j < Main.rockLayer;
        bool flag2 = j < Main.UnderworldLayer;
        if (Main.remixWorld)
        {
            flag = j > Main.rockLayer && j < Main.UnderworldLayer;
            flag2 = j > Main.worldSurface && j < Main.rockLayer;
        }
        float num = 1f;
        bool flag3 = style >= 34 && style <= 36;
        switch (style)
        {
            case 4:
            case 5:
            case 6:
                num = 1.25f;
                break;
            default:
                if (style >= 7 && style <= 9)
                {
                    num = 1.75f;
                }
                else if (style >= 10 && style <= 12)
                {
                    num = 1.9f;
                }
                else if (style >= 13 && style <= 15)
                {
                    num = 2.1f;
                }
                else if (style >= 16 && style <= 18)
                {
                    num = 1.6f;
                }
                else if (style >= 19 && style <= 21)
                {
                    num = 3.5f;
                }
                else if (style >= 22 && style <= 24)
                {
                    num = 1.6f;
                }
                else if (style >= 25 && style <= 27)
                {
                    num = 10f;
                }
                else if (style >= 28 && style <= 30)
                {
                    if (Main.hardMode)
                    {
                        num = 4f;
                    }
                }
                else if (style >= 31 && style <= 33)
                {
                    num = 2f;
                }
                else if (style >= 34 && style <= 36)
                {
                    num = 1.25f;
                }
                break;
            case 0:
            case 1:
            case 2:
            case 3:
                break;
        }
        num = (num * 2f + 1f) / 3f;
        int range = (int)(500f / ((num + 1f) / 2f));
        if (WorldGen.gen)
        {
            return;
        }
        if (Player.GetClosestRollLuck(i, j, range) == 0f)
        {
            if (Main.netMode != 1)
            {
                Projectile.NewProjectile(WorldGen.GetProjectileSource_TileBreak(i, j), i * 16 + 16, j * 16 + 16, 0f, -12f, ProjectileID.CoinPortal, 0, 0f, Main.myPlayer);
            }
            return;
        }
        if (WorldGen.genRand.Next(35) == 0 && Main.wallDungeon[Main.tile[i, j].wall] && j > Main.worldSurface)
        {
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.GoldenKey);
            return;
        }
        //if (MainConfigInfo.StaticDisablePotDropBombProj.IsFalseRet(Main.getGoodWorld && WorldGen.genRand.Next(6) == 0))
        //{
        //    Projectile.NewProjectile(WorldGen.GetProjectileSource_TileBreak(i, j), i * 16 + 16, j * 16 + 8, Main.rand.Next(-100, 101) * 0.002f, 0f, 28, 0, 0f, Main.myPlayer, 16f, 16f);
        //    return;
        //}
        if (Main.remixWorld && Main.netMode != 1 && WorldGen.genRand.Next(5) == 0)
        {
            Player player = Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)];
            if (Main.rand.Next(2) == 0)
            {
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.FallenStar);
            }
            else if (player.ZoneJungle)
            {
                int num2 = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, -10);
                if (num2 > -1)
                {
                    Main.npc[num2].ai[1] = 75f;
                    Main.npc[num2].netUpdate = true;
                }
            }
            else if (j > Main.rockLayer && j < Main.maxTilesY - 350)
            {
                int num3 = ((Main.rand.Next(9) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, -7) : ((Main.rand.Next(7) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, -8) : ((Main.rand.Next(6) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, -9) : ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, NPCID.BlueSlime) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, -3)))));
                if (num3 > -1)
                {
                    Main.npc[num3].ai[1] = 75f;
                    Main.npc[num3].netUpdate = true;
                }
            }
            else if (j > Main.worldSurface && j <= Main.rockLayer)
            {
                int num4 = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, -6);
                if (num4 > -1)
                {
                    Main.npc[num4].ai[1] = 75f;
                    Main.npc[num4].netUpdate = true;
                }
            }
            else
            {
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.FallenStar);
            }
            return;
        }
        if (Main.remixWorld && i > Main.maxTilesX * 0.37 && i < Main.maxTilesX * 0.63 && j > Main.maxTilesY - 220)
        {
            int stack = Main.rand.Next(20, 41);
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Rope, stack);
            return;
        }
        if (WorldGen.genRand.Next(45) == 0 || (Main.rand.Next(45) == 0 && Main.expertMode))
        {
            if (j < Main.worldSurface)
            {
                int randomValue = WorldGen.genRand.Next(10);
                if (randomValue >= 7)
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.RecallPotion, WorldGen.genRand.Next(1, 3));
                }
                else
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, stackalloc int[] { 292, 298, 299, 290, 2322, 2324, 2325 }[randomValue]);
                }
            }
            else if (flag)
            {
                int randomValue = WorldGen.genRand.Next(11);
                if (randomValue >= 9)
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.RecallPotion, WorldGen.genRand.Next(1, 3));
                }
                else
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, stackalloc int[] { 289, 298, 299, 290, 303, 291, 304, 2322, 2329 }[randomValue]);
                }
            }
            else if (flag2)
            {
                int randomValue = WorldGen.genRand.Next(15);
                if (randomValue >= 14)
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.RecallPotion, WorldGen.genRand.Next(1, 3));
                }
                else
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, stackalloc int[] { 296, 295, 299, 302, 303, 305, 301, 302, 297, 304, 2322, 2323, 2327, 2329 }[randomValue]);
                }
            }
            else
            {
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, WorldGen.genRand.Next(14) switch
                {
                    0 => 296,
                    1 => 295,
                    2 => 293,
                    3 => 288,
                    4 => 294,
                    5 => 297,
                    6 => 304,
                    7 => 305,
                    8 => 301,
                    9 => 302,
                    10 => 288,
                    11 => 300,
                    12 => 2323,
                    _ => 2336
                });
                if (WorldGen.genRand.Next(5) == 0)
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.PotionOfReturn);
                }
            }
            return;
        }
        if (Main.rand.Next(30) == 0)
        {
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.WormholePotion);
            return;
        }
        int num9 = Main.rand.Next(7);
        if (Main.expertMode)
        {
            num9--;
        }
        Player player2 = Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)];
        int num10 = 0;
        int num11 = 20;
        for (int k = 0; k < 50; k++)
        {
            Item item = player2.inventory[k];
            if (!item.IsAir && item.createTile == 4)
            {
                num10 += item.stack;
                if (num10 >= num11)
                {
                    break;
                }
            }
        }
        bool flag4 = num10 < num11;
        if (num9 == 0 && player2.statLife < player2.statLifeMax2)
        {
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Heart);
            if (Main.rand.Next(2) == 0)
            {
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Heart);
            }
            if (Main.expertMode)
            {
                if (Main.rand.Next(2) == 0)
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Heart);
                }
                if (Main.rand.Next(2) == 0)
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Heart);
                }
            }
            return;
        }
        if (num9 == 1 || (num9 == 0 && flag4))
        {
            int num12 = Main.rand.Next(2, 7);
            if (Main.expertMode)
            {
                num12 += Main.rand.Next(1, 7);
            }
            int type = 8;
            int type2 = 282;
            if (player2.ZoneHallow)
            {
                num12 += Main.rand.Next(2, 7);
                type = 4387;
            }
            else if ((style >= 22 && style <= 24) || player2.ZoneCrimson)
            {
                num12 += Main.rand.Next(2, 7);
                type = 4386;
            }
            else if ((style >= 16 && style <= 18) || player2.ZoneCorrupt)
            {
                num12 += Main.rand.Next(2, 7);
                type = 4385;
            }
            else if (style >= 7 && style <= 9)
            {
                num12 += Main.rand.Next(2, 7);
                num12 = (int)(num12 * 1.5f);
                type = 4388;
            }
            else if (style >= 4 && style <= 6)
            {
                type = 974;
                type2 = 286;
            }
            else if (style >= 34 && style <= 36)
            {
                num12 += Main.rand.Next(2, 7);
                type = 4383;
            }
            else if (player2.ZoneGlowshroom)
            {
                num12 += Main.rand.Next(2, 7);
                type = 5293;
            }
            if (Main.tile[i, j].liquid > 0)
            {
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, type2, num12);
            }
            else
            {
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, type, num12);
            }
            return;
        }
        switch (num9)
        {
            case 2:
                {
                    int stack2 = Main.rand.Next(10, 21);
                    int type4 = 40;
                    if (flag && WorldGen.genRand.Next(2) == 0)
                    {
                        type4 = ((!Main.hardMode) ? 42 : 168);
                    }
                    if (j > Main.UnderworldLayer)
                    {
                        type4 = 265;
                    }
                    else if (Main.hardMode)
                    {
                        type4 = ((Main.rand.Next(2) != 0) ? 47 : ((WorldGen.SavedOreTiers.Silver != 168) ? 278 : 4915));
                    }
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, type4, stack2);
                    return;
                }
            case 3:
                {
                    int type5 = 28;
                    if (j > Main.UnderworldLayer || Main.hardMode)
                    {
                        type5 = 188;
                    }
                    int num14 = 1;
                    if (Main.expertMode && Main.rand.Next(3) != 0)
                    {
                        num14++;
                    }
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, type5, num14);
                    return;
                }
            case 4:
                if (flag3 || flag2)
                {
                    int type3 = 166;
                    if (flag3)
                    {
                        type3 = 4423;
                    }
                    int num13 = Main.rand.Next(4) + 1;
                    if (Main.expertMode)
                    {
                        num13 += Main.rand.Next(4);
                    }
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, type3, num13);
                    return;
                }
                break;
        }
        if ((num9 == 4 || num9 == 5) && j < Main.UnderworldLayer && !Main.hardMode)
        {
            int stack3 = Main.rand.Next(20, 41);
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Rope, stack3);
            return;
        }
        float num15 = 200 + WorldGen.genRand.Next(-100, 101);
        if (j < Main.worldSurface)
        {
            num15 *= 0.5f;
        }
        else if (flag)
        {
            num15 *= 0.75f;
        }
        else if (j > Main.maxTilesY - 250)
        {
            num15 *= 1.25f;
        }
        num15 *= 1f + Main.rand.Next(-20, 21) * 0.01f;
        if (Main.rand.Next(4) == 0)
        {
            num15 *= 1f + Main.rand.Next(5, 11) * 0.01f;
        }
        if (Main.rand.Next(8) == 0)
        {
            num15 *= 1f + Main.rand.Next(10, 21) * 0.01f;
        }
        if (Main.rand.Next(12) == 0)
        {
            num15 *= 1f + Main.rand.Next(20, 41) * 0.01f;
        }
        if (Main.rand.Next(16) == 0)
        {
            num15 *= 1f + Main.rand.Next(40, 81) * 0.01f;
        }
        if (Main.rand.Next(20) == 0)
        {
            num15 *= 1f + Main.rand.Next(50, 101) * 0.01f;
        }
        if (Main.expertMode)
        {
            num15 *= 2.5f;
        }
        if (Main.expertMode && Main.rand.Next(2) == 0)
        {
            num15 *= 1.25f;
        }
        if (Main.expertMode && Main.rand.Next(3) == 0)
        {
            num15 *= 1.5f;
        }
        if (Main.expertMode && Main.rand.Next(4) == 0)
        {
            num15 *= 1.75f;
        }
        num15 *= num;
        if (NPC.downedBoss1)
        {
            num15 *= 1.1f;
        }
        if (NPC.downedBoss2)
        {
            num15 *= 1.1f;
        }
        if (NPC.downedBoss3)
        {
            num15 *= 1.1f;
        }
        if (NPC.downedMechBoss1)
        {
            num15 *= 1.1f;
        }
        if (NPC.downedMechBoss2)
        {
            num15 *= 1.1f;
        }
        if (NPC.downedMechBoss3)
        {
            num15 *= 1.1f;
        }
        if (NPC.downedPlantBoss)
        {
            num15 *= 1.1f;
        }
        if (NPC.downedQueenBee)
        {
            num15 *= 1.1f;
        }
        if (NPC.downedGolemBoss)
        {
            num15 *= 1.1f;
        }
        if (NPC.downedPirates)
        {
            num15 *= 1.1f;
        }
        if (NPC.downedGoblins)
        {
            num15 *= 1.1f;
        }
        if (NPC.downedFrost)
        {
            num15 *= 1.1f;
        }
        while ((int)num15 > 0)
        {
            if (num15 > 1000000f)
            {
                int num16 = (int)(num15 / 1000000f);
                if (num16 > 50 && Main.rand.Next(2) == 0)
                {
                    num16 /= Main.rand.Next(3) + 1;
                }
                if (Main.rand.Next(2) == 0)
                {
                    num16 /= Main.rand.Next(3) + 1;
                }
                num15 -= 1000000 * num16;
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.PlatinumCoin, num16);
                continue;
            }
            if (num15 > 10000f)
            {
                int num17 = (int)(num15 / 10000f);
                if (num17 > 50 && Main.rand.Next(2) == 0)
                {
                    num17 /= Main.rand.Next(3) + 1;
                }
                if (Main.rand.Next(2) == 0)
                {
                    num17 /= Main.rand.Next(3) + 1;
                }
                num15 -= 10000 * num17;
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.GoldCoin, num17);
                continue;
            }
            if (num15 > 100f)
            {
                int num18 = (int)(num15 / 100f);
                if (num18 > 50 && Main.rand.Next(2) == 0)
                {
                    num18 /= Main.rand.Next(3) + 1;
                }
                if (Main.rand.Next(2) == 0)
                {
                    num18 /= Main.rand.Next(3) + 1;
                }
                num15 -= 100 * num18;
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.SilverCoin, num18);
                continue;
            }
            int num19 = (int)num15;
            if (num19 > 50 && Main.rand.Next(2) == 0)
            {
                num19 /= Main.rand.Next(3) + 1;
            }
            if (Main.rand.Next(2) == 0)
            {
                num19 /= Main.rand.Next(4) + 1;
            }
            if (num19 < 1)
            {
                num19 = 1;
            }
            num15 -= num19;
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.CopperCoin, num19);
        }
    }
    public static void GrowAlch(int x, int y)
    {
        if (!Main.tile[x, y].active())
        {
            return;
        }
        if (Main.tile[x, y].liquid > 0)
        {
            int style = Main.tile[x, y].frameX / 18;
            if ((!Main.tile[x, y].lava() || style != TileStyleID.Herbs.Fireblossom) && (Main.tile[x, y].liquidType() != LiquidID.Water || (style != TileStyleID.Herbs.Moonglow && style != TileStyleID.Herbs.Waterleaf)))
            {
                WorldGen.KillTile(x, y);
                NetMessage.SendTileSquare(-1, x, y);
                WorldGen.SquareTileFrame(x, y);
            }
        }
        if (Main.tile[x, y].type == TileID.ImmatureHerbs)
        {
            if (WorldGen.genRand.NextIsZero(MainConfigInfo.StaticHerbGrowRandomNumWhenIsRandom))
            {
                Main.tile[x, y].type = TileID.MatureHerbs;
                NetMessage.SendTileSquare(-1, x, y);
                WorldGen.SquareTileFrame(x, y);
            }
        }
        else if (Main.tile[x, y].type == TileID.MatureHerbs)
        {
            if (WorldGen.genRand.NextIsZero(MainConfigInfo.StaticHerbGrowRandomNumWhenIsRandom))
            {
                Main.tile[x, y].type = TileID.BloomingHerbs;
                NetMessage.SendTileSquare(-1, x, y);
                WorldGen.SquareTileFrame(x, y);
            }
        }
        //if (Main.tile[x, y].type == TileID.ImmatureHerbs)
        //{
        //    if (WorldGen.genRand.Next(50) == 0)
        //    {
        //        bool mature = false;
        //        if (Main.tile[x, y].frameX == 18 * TileSubID.Herbs.Shiverthorn)
        //        {
        //            if (WorldGen.genRand.Next(2) == 0)
        //            {
        //                mature = true;
        //            }
        //        }
        //        else
        //        {
        //            mature = true;
        //        }
        //        if (mature)
        //        {
        //            Main.tile[x, y].type = TileID.MatureHerbs;
        //            NetMessage.SendTileSquare(-1, x, y);
        //            WorldGen.SquareTileFrame(x, y);
        //        }
        //    }
        //    else if (Main.tile[x, y].type == TileID.ImmatureHerbs && (
        //        Main.dayTime && Main.tile[x, y].frameX == 18 * TileSubID.Herbs.Daybloom && WorldGen.genRand.Next(50) == 0 ||
        //        !Main.dayTime && Main.tile[x, y].frameX == 18 * TileSubID.Herbs.Moonglow && WorldGen.genRand.Next(50) == 0 ||
        //        Main.raining && Main.tile[x, y].frameX == 18 * TileSubID.Herbs.Waterleaf && WorldGen.genRand.Next(50) == 0 ||
        //        y > Main.worldSurface && Main.tile[x, y].frameX == 18 * TileSubID.Herbs.Blinkroot && WorldGen.genRand.Next(50) == 0 ||
        //        y > Main.maxTilesY - 200 && Main.tile[x, y].frameX == 18 * TileSubID.Herbs.Fireblossom && WorldGen.genRand.Next(50) == 0
        //        ))
        //    {
        //        Main.tile[x, y].type = TileID.MatureHerbs;
        //        NetMessage.SendTileSquare(-1, x, y);
        //        WorldGen.SquareTileFrame(x, y);
        //    }
        //}
        //else if (Main.tile[x, y].frameX == 18 * TileSubID.Herbs.Blinkroot && WorldGen.genRand.Next(3) != 0)
        //{
        //    if (Main.tile[x, y].type == TileID.MatureHerbs)
        //    {
        //        if (WorldGen.genRand.Next(2) == 0)
        //        {
        //            Main.tile[x, y].type = TileID.BloomingHerbs;
        //            NetMessage.SendTileSquare(-1, x, y);
        //        }
        //    }
        //    else if (Main.tile[x, y].type == TileID.BloomingHerbs || WorldGen.genRand.Next(5) == 0)
        //    {
        //        Main.tile[x, y].type = TileID.MatureHerbs;
        //        NetMessage.SendTileSquare(-1, x, y);
        //    }
        //}
        //else if (Main.tile[x, y].frameX == 18 * TileSubID.Herbs.Shiverthorn && Main.tile[x, y].type == TileID.MatureHerbs && WorldGen.genRand.Next(30) == 0)
        //{
        //    Main.tile[x, y].type = TileID.BloomingHerbs;
        //    NetMessage.SendTileSquare(-1, x, y);
        //    WorldGen.SquareTileFrame(x, y);
        //}
    }
    public static bool IsHarvestableHerbWithSeed(int type, int style) => type == TileID.BloomingHerbs;
    [DetourMethod]
    public static bool PlaceTile(int i, int j, int type, bool mute, bool forced, int plr, int style)
    {
        if (WorldGen.gen && Main.tile[i, j].active() && Main.tile[i, j].type == TileID.FallenLog)
        {
            return false;
        }
        if (type >= TileID.Count)
        {
            return false;
        }
        bool result = false;
        if (i >= 0 && j >= 0 && i < Main.maxTilesX && j < Main.maxTilesY)
        {
            ITile tile = Main.tile[i, j];
            if (tile == null)
            {
                tile = OTAPI.Hooks.Tile.InvokeCreate();
                Main.tile[i, j] = tile;
            }
            if (tile.active())
            {
                if (type == 23 && tile.type == TileID.Mud)
                {
                    type = 661;
                }
                if (type == 199 && tile.type == TileID.Mud)
                {
                    type = 662;
                }
            }
            if (forced || Collision.EmptyTile(i, j) || !Main.tileSolid[type] || (type == 23 && tile.type == TileID.Dirt && tile.active()) || (type == 199 && tile.type == TileID.Dirt && tile.active()) || (type == 2 && tile.type == TileID.Dirt && tile.active()) || (type == 109 && tile.type == TileID.Dirt && tile.active()) || (type == 60 && tile.type == TileID.Mud && tile.active()) || (type == 661 && tile.type == TileID.Mud && tile.active()) || (type == 662 && tile.type == TileID.Mud && tile.active()) || (type == 70 && tile.type == TileID.Mud && tile.active()) || (type == 633 && tile.type == TileID.Ash && tile.active()) || (Main.tileMoss[type] && (tile.type == TileID.Stone || tile.type == TileID.GrayBrick) && tile.active()))
            {
                if (!forced && TileID.Sets.BasicChest[tile.type])
                {
                    int num = tile.frameX / 18;
                    int y2 = j - tile.frameY / 18;
                    while (num > 1)
                    {
                        num -= 2;
                    }
                    if (!Chest.DestroyChest(i - num, y2))
                    {
                        return false;
                    }
                }
                if (type == 23 && (tile.type != TileID.Dirt || !tile.active()))
                {
                    return false;
                }
                if (type == 199 && (tile.type != TileID.Dirt || !tile.active()))
                {
                    return false;
                }
                if (type == 2 && (tile.type != TileID.Dirt || !tile.active()))
                {
                    return false;
                }
                if (type == 109 && (tile.type != TileID.Dirt || !tile.active()))
                {
                    return false;
                }
                if (type == 60 && (tile.type != TileID.Mud || !tile.active()))
                {
                    return false;
                }
                if (type == 661 && (tile.type != TileID.Mud || !tile.active()))
                {
                    return false;
                }
                if (type == 662 && (tile.type != TileID.Mud || !tile.active()))
                {
                    return false;
                }
                if (type == 70 && (tile.type != TileID.Mud || !tile.active()))
                {
                    return false;
                }
                if (type == 633 && (tile.type != TileID.Ash || !tile.active()))
                {
                    return false;
                }
                if (Main.tileMoss[type])
                {
                    if ((tile.type != TileID.Stone && tile.type != TileID.GrayBrick) || !tile.active())
                    {
                        return false;
                    }
                    if (tile.type == TileID.GrayBrick)
                    {
                        type = type switch
                        {
                            381 => 517,
                            534 => 535,
                            536 => 537,
                            539 => 540,
                            625 => 626,
                            627 => 628,
                            _ => 512 + type - 179,
                        };
                    }
                }
                if (type == 81)
                {
                    if (Main.tile[i, j - 1] == null)
                    {
                        Main.tile[i, j - 1] = OTAPI.Hooks.Tile.InvokeCreate();
                    }
                    if (Main.tile[i, j + 1] == null)
                    {
                        Main.tile[i, j + 1] = OTAPI.Hooks.Tile.InvokeCreate();
                    }
                    if (Main.tile[i, j - 1].active())
                    {
                        return false;
                    }
                    if (!Main.tile[i, j + 1].active() || !Main.tileSolid[Main.tile[i, j + 1].type] || Main.tile[i, j + 1].halfBrick() || Main.tile[i, j + 1].slope() != 0)
                    {
                        return false;
                    }
                }
                if ((type == 373 || type == 375 || type == 374 || type == 461) && (Main.tile[i, j - 1] == null || Main.tile[i, j - 1].bottomSlope()))
                {
                    return false;
                }
                if (tile.liquid > 0 || tile.checkingLiquid())
                {
                    switch (type)
                    {
                        case 4:
                            if (style != 8 && style != 11 && style != 17)
                            {
                                return false;
                            }
                            break;
                        case 3:
                        case 20:
                        case 24:
                        case 27:
                        case 32:
                        case 51:
                        case 69:
                        case 72:
                        case 201:
                        case 352:
                        case 529:
                        case 624:
                        case 637:
                        case 656:
                            return false;
                    }
                }
                if (TileID.Sets.ResetsHalfBrickPlacementAttempt[type] && (!tile.active() || !Main.tileFrameImportant[tile.type]))
                {
                    tile.halfBrick(halfBrick: false);
                    tile.frameY = 0;
                    tile.frameX = 0;
                }
                if (type == 624)
                {
                    if ((!tile.active() || Main.tileCut[tile.type] || TileID.Sets.BreakableWhenPlacing[tile.type]) && WorldGen.HasValidGroundForAbigailsFlowerBelowSpot(i, j))
                    {
                        tile.active(active: true);
                        tile.type = TileID.AbigailsFlower;
                        tile.halfBrick(halfBrick: false);
                        tile.slope(0);
                        tile.frameX = 0;
                        tile.frameY = 0;
                    }
                }
                else if (type == 656)
                {
                    if ((!tile.active() || Main.tileCut[tile.type] || TileID.Sets.BreakableWhenPlacing[tile.type]) && WorldGen.HasValidGroundForGlowTulipBelowSpot(i, j))
                    {
                        tile.active(active: true);
                        tile.type = TileID.GlowTulip;
                        tile.halfBrick(halfBrick: false);
                        tile.slope(0);
                        tile.frameX = 0;
                        tile.frameY = 0;
                    }
                }
                else if (type == 3 || type == 24 || type == 110 || type == 201 || type == 637)
                {
                    if (WorldGen.IsFitToPlaceFlowerIn(i, j, type))
                    {
                        if (type == 24 && WorldGen.genRand.Next(13) == 0)
                        {
                            tile.active(active: true);
                            tile.type = TileID.CorruptThorns;
                            WorldGen.SquareTileFrame(i, j);
                        }
                        else if (type == 201 && WorldGen.genRand.Next(13) == 0)
                        {
                            tile.active(active: true);
                            tile.type = TileID.CrimsonThorns;
                            WorldGen.SquareTileFrame(i, j);
                        }
                        else if (Main.tile[i, j + 1].type == TileID.ClayPot || Main.tile[i, j + 1].type == TileID.PlanterBox || Main.tile[i, j + 1].type == TileID.RockGolemHead)
                        {
                            tile.active(active: true);
                            tile.type = (ushort)type;
                            int num2 = WorldGen.genRand.NextFromList<int>(6, 7, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 24, 27, 30, 33, 36, 39, 42);
                            switch (num2)
                            {
                                case 21:
                                case 24:
                                case 27:
                                case 30:
                                case 33:
                                case 36:
                                case 39:
                                case 42:
                                    num2 += WorldGen.genRand.Next(3);
                                    break;
                            }
                            tile.frameX = (short)(num2 * 18);
                        }
                        else if (tile.wall >= 0 && tile.wall < WallID.Count && WallID.Sets.AllowsPlantsToGrow[tile.wall] && Main.tile[i, j + 1].wall >= 0 && Main.tile[i, j + 1].wall < WallID.Count && WallID.Sets.AllowsPlantsToGrow[Main.tile[i, j + 1].wall])
                        {
                            if (WorldGen.genRand.Next(50) == 0 || ((type == 24 || type == 201) && WorldGen.genRand.Next(40) == 0))
                            {
                                tile.active(active: true);
                                tile.type = (ushort)type;
                                if (type == 201)
                                {
                                    tile.frameX = 270;
                                }
                                else
                                {
                                    tile.frameX = 144;
                                }
                            }
                            else if (WorldGen.genRand.Next(35) == 0 || (Main.tile[i, j].wall >= 63 && Main.tile[i, j].wall <= 70))
                            {
                                tile.active(active: true);
                                tile.type = (ushort)type;
                                int num3 = WorldGen.genRand.NextFromList<int>(6, 7, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
                                if (type == 201)
                                {
                                    num3 = WorldGen.genRand.NextFromList<int>(6, 7, 8, 9, 10, 11, 12, 13, 14, 16, 17, 18, 19, 20, 21, 22);
                                }
                                if (type == 637)
                                {
                                    num3 = WorldGen.genRand.NextFromList<int>(6, 7, 8, 9, 10);
                                }
                                tile.frameX = (short)(num3 * 18);
                            }
                            else
                            {
                                tile.active(active: true);
                                tile.type = (ushort)type;
                                tile.frameX = (short)(WorldGen.genRand.Next(6) * 18);
                            }
                        }
                    }
                }
                else if (type == 61)
                {
                    if (j + 1 < Main.maxTilesY && Main.tile[i, j + 1].active() && Main.tile[i, j + 1].slope() == 0 && !Main.tile[i, j + 1].halfBrick() && Main.tile[i, j + 1].type == TileID.JungleGrass)
                    {
                        bool flag = j > Main.rockLayer || Main.remixWorld || WorldGen.remixWorldGen;
                        if (WorldGen.genRand.Next(16) == 0 && j > Main.worldSurface)
                        {
                            tile.active(active: true);
                            tile.type = TileID.JungleThorns;
                            WorldGen.SquareTileFrame(i, j);
                        }
                        else if (WorldGen.genRand.Next(60) == 0 && flag)
                        {
                            tile.active(active: true);
                            tile.type = (ushort)type;
                            tile.frameX = 144;
                        }
                        else if (WorldGen.genRand.Next(230) == 0 && flag)
                        {
                            tile.active(active: true);
                            tile.type = (ushort)type;
                            tile.frameX = 162;
                        }
                        else if (WorldGen.genRand.Next(15) == 0)
                        {
                            tile.active(active: true);
                            tile.type = (ushort)type;
                            if (WorldGen.genRand.Next(3) != 0)
                            {
                                tile.frameX = (short)(WorldGen.genRand.Next(2) * 18 + 108);
                            }
                            else
                            {
                                tile.frameX = (short)(WorldGen.genRand.Next(13) * 18 + 180);
                            }
                        }
                        else
                        {
                            tile.active(active: true);
                            tile.type = (ushort)type;
                            tile.frameX = (short)(WorldGen.genRand.Next(6) * 18);
                        }
                    }
                }
                else if (type == 518)
                {
                    WorldGen.PlaceLilyPad(i, j);
                }
                else if (type == 519)
                {
                    WorldGen.PlaceCatTail(i, j);
                }
                else if (type == 529)
                {
                    WorldGen.PlantSeaOat(i, j);
                }
                else if (type == 571)
                {
                    WorldGen.PlaceBamboo(i, j);
                }
                else if (type == 549)
                {
                    WorldGen.PlaceUnderwaterPlant(549, i, j);
                }
                else if (type == 71)
                {
                    if (j + 1 < Main.maxTilesY && Main.tile[i, j + 1].active() && Main.tile[i, j + 1].slope() == 0 && !Main.tile[i, j + 1].halfBrick() && Main.tile[i, j + 1].type == TileID.MushroomGrass)
                    {
                        Point point = new Point(-1, -1);
                        if (j > Main.worldSurface)
                        {
                            point = WorldGen.PlaceCatTail(i, j);
                        }
                        if (WorldGen.InWorld(point.X, point.Y))
                        {
                            if (WorldGen.gen)
                            {
                                int num4 = WorldGen.genRand.Next(14);
                                for (int k = 0; k < num4; k++)
                                {
                                    WorldGen.GrowCatTail(point.X, point.Y);
                                }
                                WorldGen.SquareTileFrame(point.X, point.Y);
                            }
                        }
                        else
                        {
                            tile.active(active: true);
                            tile.type = (ushort)type;
                            tile.frameX = (short)(WorldGen.genRand.Next(5) * 18);
                        }
                    }
                }
                else if (type == 129)
                {
                    if (WorldGen.SolidTile(i - 1, j) || WorldGen.SolidTile(i + 1, j) || WorldGen.SolidTile(i, j - 1) || WorldGen.SolidTile(i, j + 1))
                    {
                        tile.active(active: true);
                        tile.type = (ushort)type;
                        tile.frameX = (short)(style * 18);
                        WorldGen.SquareTileFrame(i, j);
                    }
                }
                else if (type == 178)
                {
                    if (WorldGen.SolidTile(i - 1, j, noDoors: true) || WorldGen.SolidTile(i + 1, j, noDoors: true) || WorldGen.SolidTile(i, j - 1) || WorldGen.SolidTile(i, j + 1))
                    {
                        tile.active(active: true);
                        tile.type = (ushort)type;
                        tile.frameX = (short)(style * 18);
                        tile.frameY = (short)(WorldGen.genRand.Next(3) * 18);
                        WorldGen.SquareTileFrame(i, j);
                    }
                }
                else if (type == 184)
                {
                    if ((Main.tileMoss[Main.tile[i - 1, j].type] && WorldGen.SolidTile(i - 1, j)) || (Main.tileMoss[Main.tile[i + 1, j].type] && WorldGen.SolidTile(i + 1, j)) || (Main.tileMoss[Main.tile[i, j - 1].type] && WorldGen.SolidTile(i, j - 1)) || (Main.tileMoss[Main.tile[i, j + 1].type] && WorldGen.SolidTile(i, j + 1)))
                    {
                        tile.active(active: true);
                        tile.type = (ushort)type;
                        tile.frameX = (short)(style * 18);
                        tile.frameY = (short)(WorldGen.genRand.Next(3) * 18);
                        WorldGen.SquareTileFrame(i, j);
                    }
                    if ((TileID.Sets.tileMossBrick[Main.tile[i - 1, j].type] && WorldGen.SolidTile(i - 1, j)) || (TileID.Sets.tileMossBrick[Main.tile[i + 1, j].type] && WorldGen.SolidTile(i + 1, j)) || (TileID.Sets.tileMossBrick[Main.tile[i, j - 1].type] && WorldGen.SolidTile(i, j - 1)) || (TileID.Sets.tileMossBrick[Main.tile[i, j + 1].type] && WorldGen.SolidTile(i, j + 1)))
                    {
                        tile.active(active: true);
                        tile.type = (ushort)type;
                        tile.frameX = (short)(style * 18);
                        tile.frameY = (short)(WorldGen.genRand.Next(3) * 18);
                        WorldGen.SquareTileFrame(i, j);
                    }
                }
                else if (type == 485)
                {
                    WorldGen.PlaceObject(i, j, type, mute, style);
                }
                else if (type == 171)
                {
                    WorldGen.PlaceXmasTree(i, j, 171);
                }
                else if (type == 254)
                {
                    WorldGen.Place2x2Style(i, j, (ushort)type, style);
                }
                else if (type == 335 || type == 564 || type == 594)
                {
                    WorldGen.Place2x2(i, j, (ushort)type, 0);
                }
                else if (type == 654 || type == 319 || type == 132 || type == 484 || type == 138 || type == 664 || type == 142 || type == 143 || type == 282 || (type >= 288 && type <= 295) || (type >= 316 && type <= 318))
                {
                    WorldGen.Place2x2(i, j, (ushort)type, 0);
                }
                else if (type == 411)
                {
                    WorldGen.Place2x2(i, j, (ushort)type, 0);
                }
                else if (type == 457)
                {
                    WorldGen.Place2x2Horizontal(i, j, 457, style);
                }
                else if (type == 137)
                {
                    tile.active(active: true);
                    tile.type = (ushort)type;
                    tile.frameY = (short)(18 * style);
                }
                else if (type == 136)
                {
                    if (Main.tile[i - 1, j] == null)
                    {
                        Main.tile[i - 1, j] = OTAPI.Hooks.Tile.InvokeCreate();
                    }
                    if (Main.tile[i + 1, j] == null)
                    {
                        Main.tile[i + 1, j] = OTAPI.Hooks.Tile.InvokeCreate();
                    }
                    if (Main.tile[i, j + 1] == null)
                    {
                        Main.tile[i, j + 1] = OTAPI.Hooks.Tile.InvokeCreate();
                    }
                    if ((Main.tile[i - 1, j].nactive() && !Main.tile[i - 1, j].halfBrick() && !TileID.Sets.NotReallySolid[Main.tile[i - 1, j].type] && Main.tile[i - 1, j].slope() == 0 && (WorldGen.SolidTile(i - 1, j) || TileID.Sets.IsBeam[Main.tile[i - 1, j].type] || (Main.tile[i - 1, j].type == TileID.Trees && Main.tile[i - 1, j - 1].type == TileID.Trees && Main.tile[i - 1, j + 1].type == TileID.Trees))) || (Main.tile[i + 1, j].nactive() && !Main.tile[i + 1, j].halfBrick() && !TileID.Sets.NotReallySolid[Main.tile[i + 1, j].type] && Main.tile[i + 1, j].slope() == 0 && (WorldGen.SolidTile(i + 1, j) || TileID.Sets.IsBeam[Main.tile[i + 1, j].type] || (Main.tile[i + 1, j].type == TileID.Trees && Main.tile[i + 1, j - 1].type == TileID.Trees && Main.tile[i + 1, j + 1].type == TileID.Trees))) || (Main.tile[i, j + 1].nactive() && !Main.tile[i, j + 1].halfBrick() && WorldGen.SolidTile(i, j + 1) && Main.tile[i, j + 1].slope() == 0) || tile.wall > 0)
                    {
                        tile.active(active: true);
                        tile.type = (ushort)type;
                        WorldGen.SquareTileFrame(i, j);
                    }
                }
                else if (type == 442)
                {
                    if (Main.tile[i - 1, j] == null)
                    {
                        Main.tile[i - 1, j] = OTAPI.Hooks.Tile.InvokeCreate();
                    }
                    if (Main.tile[i + 1, j] == null)
                    {
                        Main.tile[i + 1, j] = OTAPI.Hooks.Tile.InvokeCreate();
                    }
                    if (Main.tile[i, j + 1] == null)
                    {
                        Main.tile[i, j + 1] = OTAPI.Hooks.Tile.InvokeCreate();
                    }
                    if ((Main.tile[i - 1, j].nactive() && !Main.tile[i - 1, j].halfBrick() && !TileID.Sets.NotReallySolid[Main.tile[i - 1, j].type] && Main.tile[i - 1, j].slope() == 0 && (WorldGen.SolidTile(i - 1, j) || TileID.Sets.IsBeam[Main.tile[i - 1, j].type] || (Main.tile[i - 1, j].type == TileID.Trees && Main.tile[i - 1, j - 1].type == TileID.Trees && Main.tile[i - 1, j + 1].type == TileID.Trees))) || (Main.tile[i + 1, j].nactive() && !Main.tile[i + 1, j].halfBrick() && !TileID.Sets.NotReallySolid[Main.tile[i + 1, j].type] && Main.tile[i + 1, j].slope() == 0 && (WorldGen.SolidTile(i + 1, j) || TileID.Sets.IsBeam[Main.tile[i + 1, j].type] || (Main.tile[i + 1, j].type == TileID.Trees && Main.tile[i + 1, j - 1].type == TileID.Trees && Main.tile[i + 1, j + 1].type == TileID.Trees))) || (Main.tile[i, j + 1].nactive() && !Main.tile[i, j + 1].halfBrick() && WorldGen.SolidTile(i, j + 1) && Main.tile[i, j + 1].slope() == 0))
                    {
                        tile.active(active: true);
                        tile.type = (ushort)type;
                        WorldGen.SquareTileFrame(i, j);
                    }
                }
                else if (type == 4)
                {
                    if (Main.tile[i - 1, j] == null)
                    {
                        Main.tile[i - 1, j] = OTAPI.Hooks.Tile.InvokeCreate();
                    }
                    if (Main.tile[i + 1, j] == null)
                    {
                        Main.tile[i + 1, j] = OTAPI.Hooks.Tile.InvokeCreate();
                    }
                    if (Main.tile[i, j + 1] == null)
                    {
                        Main.tile[i, j + 1] = OTAPI.Hooks.Tile.InvokeCreate();
                    }
                    ITile tile2 = Main.tile[i - 1, j];
                    ITile tile3 = Main.tile[i + 1, j];
                    ITile tile4 = Main.tile[i, j + 1];
                    if (tile.wall > 0 || (tile2.active() && (tile2.slope() == 0 || tile2.slope() % 2 != 1) && ((Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type] && !TileID.Sets.NotReallySolid[tile2.type]) || TileID.Sets.IsBeam[tile2.type] || (WorldGen.IsTreeType(tile2.type) && WorldGen.IsTreeType(Main.tile[i - 1, j - 1].type) && WorldGen.IsTreeType(Main.tile[i - 1, j + 1].type)))) || (tile3.active() && (tile3.slope() == 0 || tile3.slope() % 2 != 0) && ((Main.tileSolid[tile3.type] && !Main.tileSolidTop[tile3.type] && !TileID.Sets.NotReallySolid[tile3.type]) || TileID.Sets.IsBeam[tile3.type] || (WorldGen.IsTreeType(tile3.type) && WorldGen.IsTreeType(Main.tile[i + 1, j - 1].type) && WorldGen.IsTreeType(Main.tile[i + 1, j + 1].type)))) || (tile4.active() && Main.tileSolid[tile4.type] && ((TileID.Sets.Platforms[tile4.type] && WorldGen.TopEdgeCanBeAttachedTo(i, j + 1)) || ((!Main.tileSolidTop[tile4.type] || (tile4.type == TileID.PlanterBox && tile4.slope() == 0)) && !TileID.Sets.NotReallySolid[tile4.type] && !tile4.halfBrick() && tile4.slope() == 0))))
                    {
                        tile.active(active: true);
                        tile.type = (ushort)type;
                        tile.frameY = (short)(22 * style);
                        WorldGen.SquareTileFrame(i, j);
                    }
                }
                else if (type == 10)
                {
                    if (Main.tile[i, j - 1] == null)
                    {
                        Main.tile[i, j - 1] = OTAPI.Hooks.Tile.InvokeCreate();
                    }
                    if (Main.tile[i, j - 2] == null)
                    {
                        Main.tile[i, j - 2] = OTAPI.Hooks.Tile.InvokeCreate();
                    }
                    if (Main.tile[i, j - 3] == null)
                    {
                        Main.tile[i, j - 3] = OTAPI.Hooks.Tile.InvokeCreate();
                    }
                    if (Main.tile[i, j + 1] == null)
                    {
                        Main.tile[i, j + 1] = OTAPI.Hooks.Tile.InvokeCreate();
                    }
                    if (Main.tile[i, j + 2] == null)
                    {
                        Main.tile[i, j + 2] = OTAPI.Hooks.Tile.InvokeCreate();
                    }
                    if (Main.tile[i, j + 3] == null)
                    {
                        Main.tile[i, j + 3] = OTAPI.Hooks.Tile.InvokeCreate();
                    }
                    if (!Main.tile[i, j - 1].active() && !Main.tile[i, j - 2].active() && Main.tile[i, j - 3].active() && Main.tileSolid[Main.tile[i, j - 3].type])
                    {
                        WorldGen.PlaceDoor(i, j - 1, type, style);
                        WorldGen.SquareTileFrame(i, j);
                    }
                    else
                    {
                        if (Main.tile[i, j + 1].active() || Main.tile[i, j + 2].active() || !Main.tile[i, j + 3].active() || !Main.tileSolid[Main.tile[i, j + 3].type])
                        {
                            return false;
                        }
                        WorldGen.PlaceDoor(i, j + 1, type, style);
                        WorldGen.SquareTileFrame(i, j);
                    }
                }
                else if ((type >= 275 && type <= 281) || type == 296 || type == 297 || type == 309 || type == 358 || type == 359 || type == 413 || type == 414 || type == 542)
                {
                    WorldGen.Place6x3(i, j, (ushort)type);
                }
                else if (type == 237 || type == 244 || type == 285 || type == 286 || type == 298 || type == 299 || type == 310 || type == 339 || type == 538 || (type >= 361 && type <= 364) || type == 532 || type == 533 || type == 486 || type == 488 || type == 544 || type == 582 || type == 619 || type == 629)
                {
                    WorldGen.Place3x2(i, j, (ushort)type);
                }
                else if (type == 128)
                {
                    WorldGen.PlaceMan(i, j, style);
                    WorldGen.SquareTileFrame(i, j);
                }
                else if (type == 269)
                {
                    WorldGen.PlaceWoman(i, j, style);
                    WorldGen.SquareTileFrame(i, j);
                }
                else if (type == 334)
                {
                    int style2 = 0;
                    if (style == -1)
                    {
                        style2 = 1;
                    }
                    WorldGen.Place3x3Wall(i, j, 334, style2);
                    WorldGen.SquareTileFrame(i, j);
                }
                else if (type == 149)
                {
                    if (WorldGen.SolidTile(i - 1, j) || WorldGen.SolidTile(i + 1, j) || WorldGen.SolidTile(i, j - 1) || WorldGen.SolidTile(i, j + 1))
                    {
                        tile.frameX = (short)(18 * style);
                        tile.active(active: true);
                        tile.type = (ushort)type;
                        WorldGen.SquareTileFrame(i, j);
                    }
                }
                else if (type == 139 || type == 35)
                {
                    WorldGen.PlaceMB(i, j, (ushort)type, style);
                    WorldGen.SquareTileFrame(i, j);
                }
                else if (type == 165)
                {
                    WorldGen.PlaceTight(i, j);
                    WorldGen.SquareTileFrame(i, j);
                }
                else if (type == 235)
                {
                    WorldGen.Place3x1(i, j, (ushort)type);
                    WorldGen.SquareTileFrame(i, j);
                }
                else if (type == 240)
                {
                    WorldGen.Place3x3Wall(i, j, (ushort)type, style);
                }
                else if (type == 440)
                {
                    WorldGen.Place3x3Wall(i, j, (ushort)type, style);
                }
                else if (type == 245)
                {
                    WorldGen.Place2x3Wall(i, j, (ushort)type, style);
                }
                else if (type == 246)
                {
                    WorldGen.Place3x2Wall(i, j, (ushort)type, style);
                }
                else if (type == 241)
                {
                    WorldGen.Place4x3Wall(i, j, (ushort)type, style);
                }
                else if (type == 242)
                {
                    WorldGen.Place6x4Wall(i, j, (ushort)type, style);
                }
                else if (type == 34)
                {
                    WorldGen.PlaceChand(i, j, (ushort)type, style);
                    WorldGen.SquareTileFrame(i, j);
                }
                else if (type == 106 || type == 212 || type == 219 || type == 220 || type == 228 || type == 231 || type == 243 || type == 247 || type == 283 || (type >= 300 && type <= 308) || type == 354 || type == 355 || type == 491 || type == 642)
                {
                    WorldGen.Place3x3(i, j, (ushort)type, style);
                    WorldGen.SquareTileFrame(i, j);
                }
                else
                {
                    switch (type)
                    {
                        case 13:
                        case 33:
                        case 49:
                        case 50:
                        case 78:
                        case 174:
                        case 372:
                        case 646:
                            WorldGen.PlaceOnTable1x1(i, j, type, style);
                            WorldGen.SquareTileFrame(i, j);
                            break;
                        case 14:
                        case 26:
                        case 86:
                        case 87:
                        case 88:
                        case 89:
                        case 114:
                        case 186:
                        case 187:
                        case 215:
                        case 217:
                        case 218:
                        case 377:
                        case 469:
                            WorldGen.Place3x2(i, j, (ushort)type, style);
                            WorldGen.SquareTileFrame(i, j);
                            break;
                        case 236:
                            WorldGen.PlaceJunglePlant(i, j, (ushort)type, WorldGen.genRand.Next(3), 0);
                            WorldGen.SquareTileFrame(i, j);
                            break;
                        case 238:
                            WorldGen.PlaceJunglePlant(i, j, (ushort)type, 0, 0);
                            WorldGen.SquareTileFrame(i, j);
                            break;
                        case 20:
                            {
                                if (Main.tile[i, j + 1] == null)
                                {
                                    Main.tile[i, j + 1] = OTAPI.Hooks.Tile.InvokeCreate();
                                }
                                int tileType = Main.tile[i, j + 1].type;
                                if (Main.tile[i, j + 1].active() && (tileType == 2 || tileType == 109 || tileType == 147 || tileType == 60 || tileType == 23 || tileType == 199 || tileType == 661 || tileType == 662 || tileType == 53 || tileType == 234 || tileType == 116 || tileType == 112))
                                {
                                    WorldGen.Place1x2(i, j, (ushort)type, style);
                                    WorldGen.SquareTileFrame(i, j);
                                }
                                break;
                            }
                        case 15:
                        case 216:
                        case 338:
                        case 390:
                            if (Main.tile[i, j - 1] == null)
                            {
                                Main.tile[i, j - 1] = OTAPI.Hooks.Tile.InvokeCreate();
                            }
                            if (Main.tile[i, j] == null)
                            {
                                Main.tile[i, j] = OTAPI.Hooks.Tile.InvokeCreate();
                            }
                            WorldGen.Place1x2(i, j, (ushort)type, style);
                            WorldGen.SquareTileFrame(i, j);
                            break;
                        case 227:
                            WorldGen.PlaceDye(i, j, style);
                            WorldGen.SquareTileFrame(i, j);
                            break;
                        case 567:
                            WorldGen.PlaceGnome(i, j, style);
                            WorldGen.SquareTileFrame(i, j);
                            break;
                        case 16:
                        case 18:
                        case 29:
                        case 103:
                        case 134:
                        case 462:
                            WorldGen.Place2x1(i, j, (ushort)type, style);
                            WorldGen.SquareTileFrame(i, j);
                            break;
                        case 92:
                        case 93:
                        case 453:
                            WorldGen.Place1xX(i, j, (ushort)type, style);
                            WorldGen.SquareTileFrame(i, j);
                            break;
                        case 104:
                        case 105:
                        case 320:
                        case 337:
                        case 349:
                        case 356:
                        case 378:
                        case 456:
                        case 506:
                        case 545:
                        case 663:
                            WorldGen.Place2xX(i, j, (ushort)type, style);
                            WorldGen.SquareTileFrame(i, j);
                            break;
                        case 17:
                        case 77:
                        case 133:
                            WorldGen.Place3x2(i, j, (ushort)type, style);
                            WorldGen.SquareTileFrame(i, j);
                            break;
                        case 207:
                            WorldGen.Place2xX(i, j, (ushort)type, style);
                            WorldGen.SquareTileFrame(i, j);
                            break;
                        case 410:
                        case 480:
                        case 509:
                        case 657:
                        case 658:
                            WorldGen.Place2xX(i, j, (ushort)type, style);
                            WorldGen.SquareTileFrame(i, j);
                            break;
                        case 465:
                        case 531:
                        case 591:
                        case 592:
                            WorldGen.Place2xX(i, j, (ushort)type, style);
                            WorldGen.SquareTileFrame(i, j);
                            break;
                        default:
                            if (TileID.Sets.BasicChest[type])
                            {
                                WorldGen.PlaceChest(i, j, (ushort)type, notNearOtherChests: false, style);
                                WorldGen.SquareTileFrame(i, j);
                                break;
                            }
                            switch (type)
                            {
                                case 91:
                                    WorldGen.PlaceBanner(i, j, (ushort)type, style);
                                    WorldGen.SquareTileFrame(i, j);
                                    break;
                                case 419:
                                case 420:
                                case 423:
                                case 424:
                                case 429:
                                case 445:
                                    WorldGen.PlaceLogicTiles(i, j, type, style);
                                    WorldGen.SquareTileFrame(i, j);
                                    break;
                                case 36:
                                case 135:
                                case 141:
                                case 144:
                                case 210:
                                case 239:
                                case 324:
                                case 476:
                                case 494:
                                    WorldGen.Place1x1(i, j, type, style);
                                    WorldGen.SquareTileFrame(i, j);
                                    break;
                                case 101:
                                case 102:
                                case 463:
                                    WorldGen.Place3x4(i, j, (ushort)type, style);
                                    WorldGen.SquareTileFrame(i, j);
                                    break;
                                case 464:
                                case 466:
                                    WorldGen.Place5x4(i, j, (ushort)type, style);
                                    WorldGen.SquareTileFrame(i, j);
                                    break;
                                case 27:
                                    WorldGen.PlaceSunflower(i, j, 27);
                                    WorldGen.SquareTileFrame(i, j);
                                    break;
                                case 28:
                                    WorldGen.PlacePot(i, j, 28, WorldGen.genRand.Next(4));
                                    WorldGen.SquareTileFrame(i, j);
                                    break;
                                case 42:
                                case 270:
                                case 271:
                                    WorldGen.Place1x2Top(i, j, (ushort)type, style);
                                    WorldGen.SquareTileFrame(i, j);
                                    break;
                                case 55:
                                case 425:
                                case 510:
                                case 511:
                                    WorldGen.PlaceSign(i, j, (ushort)type, style);
                                    break;
                                case 85:
                                case 376:
                                    WorldGen.Place2x2Horizontal(i, j, (ushort)type, style);
                                    break;
                                default:
                                    if (Main.tileAlch[type])
                                    {
                                        WorldGen.PlaceAlch(i, j, style);
                                        break;
                                    }
                                    switch (type)
                                    {
                                        case 94:
                                        case 95:
                                        case 97:
                                        case 98:
                                        case 99:
                                        case 100:
                                        case 125:
                                        case 126:
                                        case 172:
                                        case 173:
                                        case 287:
                                            WorldGen.Place2x2(i, j, (ushort)type, style);
                                            break;
                                        case 96:
                                            WorldGen.Place2x2Style(i, j, (ushort)type, style);
                                            break;
                                        case 79:
                                        case 90:
                                            {
                                                int direction = 1;
                                                if (plr > -1)
                                                {
                                                    direction = Main.player[plr].direction;
                                                }
                                                WorldGen.Place4x2(i, j, (ushort)type, direction, style);
                                                break;
                                            }
                                        case 209:
                                            WorldGen.PlaceCannon(i, j, (ushort)type, style);
                                            break;
                                        case 81:
                                            tile.frameX = (short)(26 * WorldGen.genRand.Next(6));
                                            tile.active(active: true);
                                            tile.type = (ushort)type;
                                            break;
                                        case 19:
                                            tile.frameY = (short)(18 * style);
                                            tile.active(active: true);
                                            tile.type = (ushort)type;
                                            break;
                                        case 380:
                                            tile.frameY = (short)(18 * style);
                                            tile.active(active: true);
                                            tile.type = (ushort)type;
                                            break;
                                        case 314:
                                            Minecart.PlaceTrack(tile, style);
                                            break;
                                        default:
                                            tile.active(active: true);
                                            tile.type = (ushort)type;
                                            if (Main.tenthAnniversaryWorld && !Main.remixWorld && (type == 53 || type == 396 || type == 397))
                                            {
                                                tile.color(7);
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                }
                if (tile.active())
                {
                    if (TileID.Sets.BlocksWaterDrawingBehindSelf[tile.type])
                    {
                        WorldGen.SquareWallFrame(i, j);
                    }
                    WorldGen.SquareTileFrame(i, j);
                    result = true;
                }
            }
        }
        return result;
    }
    [DetourMethod]
    public static void Check3x1(int i, int j, int type)
    {
        if (WorldGen.destroyObject)
        {
            return;
        }
        bool flag = false;
        Main.tile[i, j] ??= OTAPI.Hooks.Tile.InvokeCreate();
        int num = Main.tile[i, j].frameX / 18;
        int num2 = 0;
        while (num > 2)
        {
            num -= 3;
            num2++;
        }
        num = i - num;
        int num3 = num2 * 54;
        for (int k = num; k < num + 3; k++)
        {
            Main.tile[k, j] ??= OTAPI.Hooks.Tile.InvokeCreate();
            if (!Main.tile[k, j].active() || Main.tile[k, j].type != type || Main.tile[k, j].frameX != (k - num) * 18 + num3 || Main.tile[k, j].frameY != 0)
            {
                flag = true;
            }
            Main.tile[k, j - 1] ??= OTAPI.Hooks.Tile.InvokeCreate();
            if (Main.tile[k, j - 1].active() && (TileID.Sets.BasicChest[Main.tile[k, j - 1].type] || TileID.Sets.BasicChestFake[Main.tile[k, j - 1].type] || Main.tile[k, j - 1].type == TileID.Dressers || Main.tile[k, j - 1].type == TileID.DisplayDoll || Main.tile[k, j - 1].type == TileID.HatRack || Main.tile[k, j - 1].type == TileID.TeleportationPylon))
            {
                return;
            }
            if (!WorldGen.SolidTileAllowBottomSlope(k, j + 1))
            {
                flag = true;
            }
        }
        if (!flag)
        {
            return;
        }
        WorldGen.destroyObject = true;
        if (type == 235)
        {
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.Teleporter);
        }
        for (int l = num; l < num + 3; l++)
        {
            Main.tile[l, j] ??= OTAPI.Hooks.Tile.InvokeCreate();
            if (Main.tile[l, j].type == type && Main.tile[l, j].active())
            {
                WorldGen.KillTile(l, j);
            }
        }
        WorldGen.destroyObject = false;
        for (int m = num; m < num + 3; m++)
        {
            WorldGen.TileFrame(m, j - 1);
        }
        for (int m = num - 1; m < num + 4; m++)
        {
            WorldGen.TileFrame(m, j);
        }
    }
    [DetourMethod]
    public static bool AttemptToGrowTreeFromSapling(int x, int y, bool underground)
    {
        if (Main.netMode == 1)
        {
            return false;
        }
        if (!WorldGen.InWorld(x, y, 2))
        {
            return false;
        }
        ITile tile = Main.tile[x, y];
        if (tile == null || !tile.active())
        {
            return false;
        }
        bool success = false;
        const int styleWidth = 18 * 3;
        int treeTileStyle = 0;
        int treeTileType = -1;
        switch (tile.type)
        {
            case TileID.Saplings:
                switch (tile.frameX / styleWidth)
                {
                    case 10:
                        success = WorldGen.TryGrowingTreeByType(634, x, y);
                        break;
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        if (underground)
                        {
                            return false;
                        }
                        success = WorldGen.GrowPalmTree(x, y);
                        break;
                    default:
                        if (underground)
                        {
                            return false;
                        }
                        success = WorldGen.GrowTree(x, y);
                        break;
                }
                if (success && WorldGen.PlayerLOS(x, y))
                {
                    WorldGen.TreeGrowFXCheck(x, y);
                }
                return success;
            case TileID.VanityTreeSakuraSaplings:
                treeTileStyle = tile.frameX / styleWidth;
                treeTileType = 596;
                success = WorldGen.TryGrowingTreeByType(treeTileType, x, y);
                if (success && WorldGen.PlayerLOS(x, y))
                {
                    WorldGen.TreeGrowFXCheck(x, y);
                }
                return success;
            case TileID.VanityTreeWillowSaplings:
                treeTileStyle = tile.frameX / styleWidth;
                treeTileType = 616;
                success = WorldGen.TryGrowingTreeByType(treeTileType, x, y);
                if (success && WorldGen.PlayerLOS(x, y))
                {
                    WorldGen.TreeGrowFXCheck(x, y);
                }
                return success;
            case TileID.GemSaplings:
                if (!underground && !WorldInfo.StaticGemTreeCanGrowOverground)
                {
                    return false;
                }
                treeTileStyle = tile.frameX / styleWidth;
                treeTileType = TileID.TreeRuby;
                if (treeTileStyle is >= 0 and <= 6)
                {
                    treeTileType = TileID.TreeTopaz + treeTileStyle;
                }
                success = WorldGen.TryGrowingTreeByType(treeTileType, x, y);
                if (success && WorldGen.PlayerLOS(x, y))
                {
                    WorldGen.TreeGrowFXCheck(x, y);
                }
                return success;
            default:
                return false;
        }
    }
    public static void ScoreRoom(int ignoreNPC, int npcTypeAskingToScoreRoom)
    {
        WorldGen.roomOccupied = false;
        WorldGen.roomEvil = false;
        WorldGen.sharedRoomX = -1;
        if (WorldGen.ScoreRoom_IsThisRoomOccupiedBySomeone(ignoreNPC, npcTypeAskingToScoreRoom))
        {
            WorldGen.roomOccupied = true;
            WorldGen.hiScore = -1;
            return;
        }
        WorldGen.hiScore = 0;
        int num = 0;
        int roomScore = 50;
        WorldGen.Housing_GetTestedRoomBounds(out var startX, out var endX, out var startY, out var endY);
        //int[] tileTypeCounts = new int[TileID.Count];
        //WorldGen.CountTileTypesInArea(tileTypeCounts, startX + 1, endX - 1, startY + 2, endY + 1);
        //int evilScore = -WorldGen.GetTileTypeCountByCategory(tileTypeCounts, TileScanGroup.TotalGoodEvil);
        int evilScore = 0;
        //Console.WriteLine("Call ScoreRoom");
        if (evilScore < 50)
        {
            evilScore = 0;
        }
        roomScore -= evilScore;
        if (evilScore > 0)
        {
            WorldGen.roomEvil = true;
        }
        if (roomScore <= -250)
        {
            WorldGen.hiScore = roomScore;
            return;
        }
        startX = WorldGen.roomX1;
        endX = WorldGen.roomX2;
        startY = WorldGen.roomY1;
        endY = WorldGen.roomY2;
        for (int i = startX + 1; i < endX; i++)
        {
            for (int j = startY + 2; j < endY + 2; j++)
            {
                if (!Main.tile[i, j].nactive() || !WorldGen.ScoreRoom_CanBeHomeSpot(i, j))
                {
                    continue;
                }
                num = roomScore;
                if (!Main.tileSolid[Main.tile[i, j].type] || Main.tileSolidTop[Main.tile[i, j].type] || Collision.SolidTiles(i - 1, i + 1, j - 3, j - 1) || !Main.tile[i - 1, j].nactive() || !Main.tileSolid[Main.tile[i - 1, j].type] || !Main.tile[i + 1, j].nactive() || !Main.tileSolid[Main.tile[i + 1, j].type])
                {
                    continue;
                }
                int num4 = 0;
                int num5 = 0;
                for (int k = i - 2; k < i + 3; k++)
                {
                    for (int l = j - 4; l < j; l++)
                    {
                        ITile tile = Main.tile[k, l];
                        if (tile.nactive() && !TileID.Sets.IgnoredInHouseScore[tile.type] && (tile.type != TileID.OpenDoor || WorldGen.IsOpenDoorAnchorFrame(k, l)))
                        {
                            if (k == i)
                            {
                                num4++;
                            }
                            else if (TileID.Sets.BasicChest[tile.type])
                            {
                                num5++;
                            }
                            else
                            {
                                num = ((tile.type != TileID.ClosedDoor && tile.type != TileID.TallGateClosed) ? ((!WorldGen.IsOpenDoorAnchorFrame(k, l) && tile.type != TileID.TallGateOpen) ? ((!Main.tileSolid[tile.type]) ? (num + 5) : (num - 5)) : (num - 20)) : (num - 20));
                            }
                        }
                    }
                }
                if (WorldGen.sharedRoomX >= 0 && num >= 1 && Math.Abs(WorldGen.sharedRoomX - i) < 3)
                {
                    num = 1;
                }
                if (num > 0 && num5 > 0)
                {
                    num -= 30 * num5;
                    if (num < 1)
                    {
                        num = 1;
                    }
                }
                if (num > 0 && num4 > 0)
                {
                    num -= 15 * num4;
                    if (num <= 0)
                    {
                        num = 0;
                    }
                }
                if (num <= WorldGen.hiScore)
                {
                    continue;
                }
                bool flag = WorldGen.Housing_CheckIfInRoom(i, j);
                bool[] array = new bool[3];
                for (int m = 1; m <= 3; m++)
                {
                    if (!Main.tile[i, j - m].active() || !Main.tileSolid[Main.tile[i, j - m].type])
                    {
                        array[m - 1] = true;
                    }
                    if (!WorldGen.Housing_CheckIfInRoom(i, j - m))
                    {
                        array[m - 1] = false;
                    }
                }
                bool[] array2 = array;
                for (int n = 0; n < array2.Length; n++)
                {
                    if (!array2[n])
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag && !WorldGen.Housing_CheckIfIsCeiling(i, j))
                {
                    WorldGen.hiScore = num;
                    WorldGen.bestX = i;
                    WorldGen.bestY = j;
                }
            }
        }
    }
    private static readonly Stack<Point> CheckRoomStack = new(1024);
    [DetourMethod]
    public static void CheckRoom(int checkX, int checkY)
    {
        var stack = CheckRoomStack;
        stack.Clear();
        stack.Push(new Point(checkX, checkY));
        while (stack.Count > 0)
        {
            var point = stack.Pop();
            if (!WorldGen.canSpawn)
            {
                return;
            }
            var x = point.X;
            var y = point.Y;
            if (!WorldGen.InWorld(x, y, 10))
            {
                WorldGen.roomCheckFailureReason = TownNPCRoomCheckFailureReason.TooCloseToWorldEdge;
                WorldGen.canSpawn = false;
                return;
            }
            for (int i = 0; i < WorldGen.numRoomTiles; i++)
            {
                if (WorldGen.roomX[i] == x && WorldGen.roomY[i] == y)
                {
                    goto whileEnd;
                }
            }
            WorldGen.roomX[WorldGen.numRoomTiles] = x;
            WorldGen.roomY[WorldGen.numRoomTiles] = y;
            bool flag = false;
            for (int j = 0; j < WorldGen.roomCeilingsCount; j++)
            {
                if (WorldGen.roomCeilingX[j] == x)
                {
                    flag = true;
                    if (WorldGen.roomCeilingY[j] > y)
                    {
                        WorldGen.roomCeilingY[j] = y;
                    }
                    break;
                }
            }
            if (!flag)
            {
                WorldGen.roomCeilingX[WorldGen.roomCeilingsCount] = x;
                WorldGen.roomCeilingY[WorldGen.roomCeilingsCount] = y;
                WorldGen.roomCeilingsCount++;
            }
            WorldGen.numRoomTiles++;
            if (WorldGen.numRoomTiles >= WorldGen.maxRoomTiles)
            {
                WorldGen.roomCheckFailureReason = TownNPCRoomCheckFailureReason.RoomIsTooBig;
                WorldGen.canSpawn = false;
                return;
            }
            if (Main.tile[x, y].nactive())
            {
                WorldGen.houseTile[Main.tile[x, y].type] = true;
                if (Main.tileSolid[Main.tile[x, y].type] || (Main.tile[x, y].type == TileID.OpenDoor && (Main.tile[x, y].frameX == 0 || Main.tile[x, y].frameX == 54 || Main.tile[x, y].frameX == 72 || Main.tile[x, y].frameX == 126)) || Main.tile[x, y].type == TileID.TallGateOpen || (Main.tile[x, y].type == TileID.TrapdoorOpen && ((Main.tile[x, y].frameX < 36 && Main.tile[x, y].frameY == 18) || (Main.tile[x, y].frameX >= 36 && Main.tile[x, y].frameY == 0))))
                {
                    continue;
                }
            }
            if (x < WorldGen.roomX1)
            {
                WorldGen.roomX1 = x;
            }
            if (x > WorldGen.roomX2)
            {
                WorldGen.roomX2 = x;
            }
            if (y < WorldGen.roomY1)
            {
                WorldGen.roomY1 = y;
            }
            if (y > WorldGen.roomY2)
            {
                WorldGen.roomY2 = y;
            }
            if (Main.tile[x, y].type == TileID.StinkbugHousingBlocker)
            {
                WorldGen.roomHasStinkbug = true;
            }
            if (Main.tile[x, y].type == TileID.StinkbugHousingBlockerEcho)
            {
                WorldGen.roomHasEchoStinkbug = true;
            }
            bool flag2 = false;
            bool flag3 = false;
            for (int k = -2; k < 3; k++)
            {
                var wrapper = new TileWrapper(x + k, y);
                if (wrapper.WallIsHouse)
                {
                    flag2 = true;
                }
                if (wrapper.nactive && (wrapper.IsSolid || wrapper.IsHousingWalls))
                {
                    flag2 = true;
                }
                wrapper = new TileWrapper(x, y + k);
                if (wrapper.WallIsHouse)
                {
                    flag3 = true;
                }
                if (wrapper.nactive && (wrapper.IsSolid || wrapper.IsHousingWalls))
                {
                    flag3 = true;
                }
            }
            if (!flag2 || !flag3)
            {
                WorldGen.roomCheckFailureReason = TownNPCRoomCheckFailureReason.HoleInWallIsTooBig;
                WorldGen.canSpawn = false;
                return;
            }
            x++;
            stack.Push(new Point(x, y + 1));
            stack.Push(new Point(x, y));
            stack.Push(new Point(x, y - 1));
            x--;
            stack.Push(new Point(x, y + 1));
            stack.Push(new Point(x, y - 1));
            x--;
            stack.Push(new Point(x, y + 1));
            stack.Push(new Point(x, y));
            stack.Push(new Point(x, y - 1));
        whileEnd: { }
        }
    }
    [DetourMethod]
    public static void KillTile_GetItemDrops(int x, int y, ITile tileCache, out int dropItem, out int dropItemStack, out int secondaryItem, out int secondaryItemStack, bool includeLargeObjectDrops)
    {
        dropItem = 0;
        dropItemStack = 1;
        secondaryItem = 0;
        secondaryItemStack = 1;
        if (includeLargeObjectDrops)
        {
            switch (tileCache.type)
            {
                case TileID.Containers:
                case TileID.Containers2:
                    dropItem = WorldGen.GetChestItemDrop(x, y, tileCache.type);
                    break;
                case TileID.Dressers:
                    dropItem = WorldGen.GetDresserItemDrop(tileCache.frameX / 54);
                    break;
                case TileID.Campfire:
                    dropItem = WorldGen.GetCampfireItemDrop(tileCache.frameX / 54);
                    break;
            }
        }
        if (SingleTileDropItem[tileCache.type] != -1)
        {
            dropItem = SingleTileDropItem[tileCache.type];
            return;
        }
        var index = HasStyleDropIndices.IndexOf(tileCache.type);
        if (index != -1)
        {
            if (!StyleDrop[index].TryGetValue(TileUtils.GetStyleID(tileCache), out dropItem))
            {
                dropItem = 0;
            }
            return;
        }
        int num;
        switch (tileCache.type)
        {
            case TileID.Bamboo:
                dropItem = 4564;
                dropItemStack = WorldGen.genRand.Next(1, 3);
                break;
            case TileID.MinecartTrack:
                dropItem = Minecart.GetTrackItem(tileCache);
                break;
            case TileID.AshPlants:
                if (Main.rand.Next(100) == 0)
                {
                    dropItem = 5214;
                }
                break;
            case TileID.Crystals:
                if (tileCache.frameX >= 324)
                {
                    dropItem = 4988;
                }
                else
                {
                    dropItem = 502;
                }
                break;
            case TileID.Plants:
                if (tileCache.frameX == 144)
                {
                    dropItem = 5;
                }
                else if (WorldGen.KillTile_ShouldDropSeeds(x, y))
                {
                    dropItem = 283;
                }
                break;
            case TileID.Cattail:
                if (tileCache.frameY == 90 && WorldGen.genRand.Next(2) == 0)
                {
                    dropItem = 183;
                }
                break;
            case TileID.MushroomVines:
                if (WorldGen.genRand.Next(2) == 0)
                {
                    dropItem = 183;
                }
                break;
            case TileID.HallowedPlants:
                if (tileCache.frameX == 144)
                {
                    dropItem = 5;
                }
                break;
            case TileID.CorruptPlants:
                if (tileCache.frameX == 144)
                {
                    dropItem = 60;
                }
                break;
            case TileID.CrimsonPlants:
                if (tileCache.frameX == 270)
                {
                    dropItem = 2887;
                }
                break;
            case TileID.Plants2:
                if (WorldGen.KillTile_ShouldDropSeeds(x, y))
                {
                    dropItem = 283;
                }
                break;
            case TileID.Vines:
            case TileID.JungleVines:
            case TileID.VineFlowers:
                if (Main.rand.Next(2) == 0 && WorldGen.GetPlayerForTile(x, y).cordage)
                {
                    dropItem = 2996;
                }
                break;
            case TileID.DyePlants:
                num = tileCache.frameX / 34;
                dropItem = 1107 + num;
                if (num >= 8 && num <= 11)
                {
                    dropItem = 3385 + num - 8;
                }
                break;
            case TileID.PlanterBox:
                dropItem = 3215 + tileCache.frameY / 18;
                break;
            case TileID.Trees:
            case TileID.VanityTreeSakura:
            case TileID.VanityTreeYellowWillow:
            case TileID.TreeAsh:
                {
                    bool bonusWood = false;
                    WorldGen.KillTile_GetTreeDrops(x, y, tileCache, ref bonusWood, ref dropItem, ref secondaryItem);
                    if (bonusWood)
                    {
                        dropItemStack++;
                    }
                    break;
                }
            case TileID.PalmTree:
                {
                    dropItem = 2504;
                    if (Main.tenthAnniversaryWorld)
                    {
                        dropItemStack += WorldGen.genRand.Next(2, 5);
                    }
                    if (tileCache.frameX <= 132 && tileCache.frameX >= 88)
                    {
                        secondaryItem = 27;
                    }
                    int j;
                    for (j = y; !Main.tile[x, j].active() || !Main.tileSolid[Main.tile[x, j].type]; j++)
                    {
                    }
                    if (Main.tile[x, j].active())
                    {
                        switch (Main.tile[x, j].type)
                        {
                            case TileID.Crimsand:
                                dropItem = 911;
                                break;
                            case TileID.Pearlsand:
                                dropItem = 621;
                                break;
                            case TileID.Ebonsand:
                                dropItem = 619;
                                break;
                        }
                    }
                    break;
                }
            case TileID.ChristmasTree:
                if (tileCache.frameX >= 10)
                {
                    WorldGen.dropXmasTree(x, y, 0);
                    WorldGen.dropXmasTree(x, y, 1);
                    WorldGen.dropXmasTree(x, y, 2);
                    WorldGen.dropXmasTree(x, y, 3);
                }
                break;
            case TileID.BeachPiles:
                switch (tileCache.frameY / 22)
                {
                    case 0:
                        dropItem = 2625;
                        break;
                    case 1:
                        dropItem = 2626;
                        break;
                    case 2:
                        dropItem = 4072;
                        break;
                    case 3:
                        dropItem = 4073;
                        break;
                    case 4:
                        dropItem = 4071;
                        break;
                }
                break;
            case TileID.LogicGateLamp:
                switch (tileCache.frameX / 18)
                {
                    case 0:
                        dropItem = 3602;
                        break;
                    case 1:
                        dropItem = 3618;
                        break;
                    case 2:
                        dropItem = 3663;
                        break;
                }
                break;
            case TileID.WeightedPressurePlate:
                switch (tileCache.frameY / 18)
                {
                    case 0:
                        dropItem = 3630;
                        break;
                    case 1:
                        dropItem = 3632;
                        break;
                    case 2:
                        dropItem = 3631;
                        break;
                    case 3:
                        dropItem = 3626;
                        break;
                }
                PressurePlateHelper.DestroyPlate(new Point(x, y));
                break;
            case TileID.LogicGate:
                switch (tileCache.frameY / 18)
                {
                    case 0:
                        dropItem = 3603;
                        break;
                    case 1:
                        dropItem = 3604;
                        break;
                    case 2:
                        dropItem = 3605;
                        break;
                    case 3:
                        dropItem = 3606;
                        break;
                    case 4:
                        dropItem = 3607;
                        break;
                    case 5:
                        dropItem = 3608;
                        break;
                }
                break;
            case TileID.SmallPiles1x1Echo:
                num = tileCache.frameX / 18;
                if (num < 6)
                {
                    dropItem = 3;
                }
                else if (num < 12)
                {
                    dropItem = 2;
                }
                else if (num < 20)
                {
                    dropItem = 154;
                }
                else if (num < 28)
                {
                    dropItem = 154;
                }
                else if (num < 36)
                {
                    dropItem = 9;
                }
                else if (num < 42)
                {
                    dropItem = 593;
                }
                else if (num < 48)
                {
                    dropItem = 664;
                }
                else if (num < 54)
                {
                    dropItem = 150;
                }
                else if (num < 60)
                {
                    dropItem = 3271;
                }
                else if (num < 66)
                {
                    dropItem = 3086;
                }
                else if (num < 72)
                {
                    dropItem = 3081;
                }
                else if (num < 73)
                {
                    dropItem = 62;
                }
                else if (num < 77)
                {
                    dropItem = 169;
                }
                break;
            case TileID.LogicSensor:
                TELogicSensor.Kill(x, y);
                switch (tileCache.frameY / 18)
                {
                    case 0:
                        dropItem = 3613;
                        break;
                    case 1:
                        dropItem = 3614;
                        break;
                    case 2:
                        dropItem = 3615;
                        break;
                    case 3:
                        dropItem = 3726;
                        break;
                    case 4:
                        dropItem = 3727;
                        break;
                    case 5:
                        dropItem = 3728;
                        break;
                    case 6:
                        dropItem = 3729;
                        break;
                }
                break;
            case TileID.Hive:
                if (Main.rand.Next(3) == 0)
                {
                    tileCache.honey(honey: true);
                    tileCache.liquid = byte.MaxValue;
                    break;
                }
                dropItem = 1124;
                if (MainConfigInfo.StaticNotSpawnBeeWhenKillHive)
                {
                    break;
                }
                if (Main.netMode != 1 && Main.rand.Next(2) == 0)
                {
                    int num3 = 1;
                    if (Main.rand.Next(3) == 0)
                    {
                        num3 = 2;
                    }
                    for (int i = 0; i < num3; i++)
                    {
                        int num4 = NPC.NewNPC(Type: Main.rand.Next(210, 212), source: WorldGen.GetNPCSource_TileBreak(x, y), X: x * 16 + 8, Y: y * 16 + 15, Start: 1);
                        Main.npc[num4].velocity.X = Main.rand.Next(-200, 201) * 0.002f;
                        Main.npc[num4].velocity.Y = Main.rand.Next(-200, 201) * 0.002f;
                        Main.npc[num4].netUpdate = true;
                    }
                }
                break;
            case TileID.ExposedGems:
                switch (tileCache.frameX / 18)
                {
                    case 0:
                        dropItem = 181;
                        break;
                    case 1:
                        dropItem = 180;
                        break;
                    case 2:
                        dropItem = 177;
                        break;
                    case 3:
                        dropItem = 179;
                        break;
                    case 4:
                        dropItem = 178;
                        break;
                    case 5:
                        dropItem = 182;
                        break;
                    case 6:
                        dropItem = 999;
                        break;
                }
                break;
            case TileID.HolidayLights:
                if (tileCache.frameX == 0 || tileCache.frameX == 54)
                {
                    dropItem = 596;
                }
                else if (tileCache.frameX == 18 || tileCache.frameX == 72)
                {
                    dropItem = 597;
                }
                else if (tileCache.frameX == 36 || tileCache.frameX == 90)
                {
                    dropItem = 598;
                }
                break;
            case TileID.Bottles:
                switch (tileCache.frameX / 18)
                {
                    case 1:
                        dropItem = 28;
                        break;
                    case 2:
                        dropItem = 110;
                        break;
                    case 3:
                        dropItem = 350;
                        break;
                    case 4:
                        dropItem = 351;
                        break;
                    case 5:
                        dropItem = 2234;
                        break;
                    case 6:
                        dropItem = 2244;
                        break;
                    case 7:
                        dropItem = 2257;
                        break;
                    case 8:
                        dropItem = 2258;
                        break;
                    default:
                        dropItem = 31;
                        break;
                }
                break;
            case TileID.Platforms:
                num = tileCache.frameY / 18;
                switch (num)
                {
                    case 0:
                        dropItem = 94;
                        break;
                    case 1:
                        dropItem = 631;
                        break;
                    case 2:
                        dropItem = 632;
                        break;
                    case 3:
                        dropItem = 633;
                        break;
                    case 4:
                        dropItem = 634;
                        break;
                    case 5:
                        dropItem = 913;
                        break;
                    case 6:
                        dropItem = 1384;
                        break;
                    case 7:
                        dropItem = 1385;
                        break;
                    case 8:
                        dropItem = 1386;
                        break;
                    case 9:
                        dropItem = 1387;
                        break;
                    case 10:
                        dropItem = 1388;
                        break;
                    case 11:
                        dropItem = 1389;
                        break;
                    case 12:
                        dropItem = 1418;
                        break;
                    case 13:
                        dropItem = 1457;
                        break;
                    case 14:
                        dropItem = 1702;
                        break;
                    case 15:
                        dropItem = 1796;
                        break;
                    case 16:
                        dropItem = 1818;
                        break;
                    case 17:
                        dropItem = 2518;
                        break;
                    case 18:
                        dropItem = 2549;
                        break;
                    case 19:
                        dropItem = 2566;
                        break;
                    case 20:
                        dropItem = 2581;
                        break;
                    case 21:
                        dropItem = 2627;
                        break;
                    case 22:
                        dropItem = 2628;
                        break;
                    case 23:
                        dropItem = 2629;
                        break;
                    case 24:
                        dropItem = 2630;
                        break;
                    case 25:
                        dropItem = 2744;
                        break;
                    case 26:
                        dropItem = 2822;
                        break;
                    case 27:
                        dropItem = 3144;
                        break;
                    case 28:
                        dropItem = 3146;
                        break;
                    case 29:
                        dropItem = 3145;
                        break;
                    case 30:
                    case 31:
                    case 32:
                    case 33:
                    case 34:
                    case 35:
                        dropItem = 3903 + num - 30;
                        break;
                    default:
                        switch (num)
                        {
                            case 36:
                                dropItem = 3945;
                                break;
                            case 37:
                                dropItem = 3957;
                                break;
                            case 38:
                                dropItem = 4159;
                                break;
                            case 39:
                                dropItem = 4180;
                                break;
                            case 40:
                                dropItem = 4201;
                                break;
                            case 41:
                                dropItem = 4222;
                                break;
                            case 42:
                                dropItem = 4311;
                                break;
                            case 43:
                                dropItem = 4416;
                                break;
                            case 44:
                                dropItem = 4580;
                                break;
                            case 45:
                                dropItem = 5162;
                                break;
                            case 46:
                                dropItem = 5183;
                                break;
                            case 47:
                                dropItem = 5204;
                                break;
                            case 48:
                                dropItem = 5292;
                                break;
                        }
                        break;
                }
                break;
            case TileID.Candles:
                num = tileCache.frameY / 22;
                dropItem = 105;
                switch (num)
                {
                    case 1:
                        dropItem = 1405;
                        break;
                    case 2:
                        dropItem = 1406;
                        break;
                    case 3:
                        dropItem = 1407;
                        break;
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                        dropItem = 2045 + num - 4;
                        break;
                    default:
                        if (num >= 14 && num <= 16)
                        {
                            dropItem = 2153 + num - 14;
                            break;
                        }
                        switch (num)
                        {
                            case 17:
                                dropItem = 2236;
                                break;
                            case 18:
                                dropItem = 2523;
                                break;
                            case 19:
                                dropItem = 2542;
                                break;
                            case 20:
                                dropItem = 2556;
                                break;
                            case 21:
                                dropItem = 2571;
                                break;
                            case 22:
                                dropItem = 2648;
                                break;
                            case 23:
                                dropItem = 2649;
                                break;
                            case 24:
                                dropItem = 2650;
                                break;
                            case 25:
                                dropItem = 2651;
                                break;
                            case 26:
                                dropItem = 2818;
                                break;
                            case 27:
                                dropItem = 3171;
                                break;
                            case 28:
                                dropItem = 3173;
                                break;
                            case 29:
                                dropItem = 3172;
                                break;
                            case 30:
                                dropItem = 3890;
                                break;
                            case 31:
                                dropItem = 3936;
                                break;
                            case 32:
                                dropItem = 3962;
                                break;
                            case 33:
                                dropItem = 4150;
                                break;
                            case 34:
                                dropItem = 4171;
                                break;
                            case 35:
                                dropItem = 4192;
                                break;
                            case 36:
                                dropItem = 4213;
                                break;
                            case 37:
                                dropItem = 4303;
                                break;
                            case 38:
                                dropItem = 4571;
                                break;
                            case 39:
                                dropItem = 5153;
                                break;
                            case 40:
                                dropItem = 5174;
                                break;
                            case 41:
                                dropItem = 5195;
                                break;
                        }
                        break;
                }
                break;
            case TileID.Traps:
                num = tileCache.frameY / 18;
                if (num == 0)
                {
                    dropItem = 539;
                }
                if (num == 1)
                {
                    dropItem = 1146;
                }
                if (num == 2)
                {
                    dropItem = 1147;
                }
                if (num == 3)
                {
                    dropItem = 1148;
                }
                if (num == 4)
                {
                    dropItem = 1149;
                }
                if (num == 5)
                {
                    dropItem = 5135;
                }
                break;
            case TileID.PressurePlates:
                num = tileCache.frameY / 18;
                if (num == 0)
                {
                    dropItem = 529;
                }
                if (num == 1)
                {
                    dropItem = 541;
                }
                if (num == 2)
                {
                    dropItem = 542;
                }
                if (num == 3)
                {
                    dropItem = 543;
                }
                if (num == 4)
                {
                    dropItem = 852;
                }
                if (num == 5)
                {
                    dropItem = 853;
                }
                if (num == 6)
                {
                    dropItem = 1151;
                }
                break;
            case TileID.Timers:
                if (tileCache.frameX == 0)
                {
                    dropItem = 583;
                }
                if (tileCache.frameX == 18)
                {
                    dropItem = 584;
                }
                if (tileCache.frameX == 36)
                {
                    dropItem = 585;
                }
                if (tileCache.frameX == 54)
                {
                    dropItem = 4484;
                }
                if (tileCache.frameX == 72)
                {
                    dropItem = 4485;
                }
                break;
            case TileID.JunglePlants:
            case TileID.JunglePlants2:
                if (tileCache.frameX == 144 && tileCache.type == TileID.JunglePlants)
                {
                    dropItem = 331;
                    dropItemStack = Main.rand.Next(2, 4);
                }
                else if (tileCache.frameX == 162 && tileCache.type == TileID.JunglePlants)
                {
                    dropItem = 223;
                }
                else if (tileCache.frameX >= 108 && tileCache.frameX <= 126 && tileCache.type == TileID.JunglePlants && Main.rand.Next(20) == 0)
                {
                    dropItem = 208;
                }
                else if (Main.rand.Next(100) == 0)
                {
                    dropItem = 195;
                }
                break;
            case TileID.MushroomPlants:
            case TileID.MushroomTrees:
                if (Main.rand.Next(40) == 0)
                {
                    dropItem = 194;
                }
                else if (Main.rand.Next(2) == 0)
                {
                    dropItem = 183;
                }
                break;
            case TileID.Books:
                if (tileCache.frameX == 90)
                {
                    dropItem = 165;
                }
                else
                {
                    dropItem = 149;
                }
                break;
            case TileID.MatureHerbs:
            case TileID.BloomingHerbs:
                {
                    num = tileCache.frameX / 18;
                    dropItem = 313 + num;
                    int num2 = 307 + num;
                    if (num == 6)
                    {
                        dropItem = 2358;
                        num2 = 2357;
                    }
                    bool flag = WorldGen.IsHarvestableHerbWithSeed(tileCache.type, num);
                    Player playerForTile = WorldGen.GetPlayerForTile(x, y);
                    if (playerForTile.HeldItem.type == ItemID.StaffofRegrowth || playerForTile.HeldItem.type == ItemID.AcornAxe)
                    {
                        dropItemStack = Main.rand.Next(1, 3);
                        secondaryItem = num2;
                        secondaryItemStack = Main.rand.Next(1, 6);
                    }
                    else if (flag)
                    {
                        secondaryItem = num2;
                        secondaryItemStack = Main.rand.Next(1, 4);
                    }
                    break;
                }
            case TileID.TreeAmber:
                WorldGen.SetGemTreeDrops(999, 4857, tileCache, ref dropItem, ref secondaryItem);
                if (dropItem == 3)
                {
                    dropItemStack = Main.rand.Next(1, 3);
                }
                break;
            case TileID.TreeAmethyst:
                WorldGen.SetGemTreeDrops(181, 4852, tileCache, ref dropItem, ref secondaryItem);
                if (dropItem == 3)
                {
                    dropItemStack = Main.rand.Next(1, 3);
                }
                break;
            case TileID.TreeTopaz:
                WorldGen.SetGemTreeDrops(180, 4851, tileCache, ref dropItem, ref secondaryItem);
                if (dropItem == 3)
                {
                    dropItemStack = Main.rand.Next(1, 3);
                }
                break;
            case TileID.TreeEmerald:
                WorldGen.SetGemTreeDrops(179, 4854, tileCache, ref dropItem, ref secondaryItem);
                if (dropItem == 3)
                {
                    dropItemStack = Main.rand.Next(1, 3);
                }
                break;
            case TileID.TreeSapphire:
                WorldGen.SetGemTreeDrops(177, 4853, tileCache, ref dropItem, ref secondaryItem);
                if (dropItem == 3)
                {
                    dropItemStack = Main.rand.Next(1, 3);
                }
                break;
            case TileID.TreeRuby:
                WorldGen.SetGemTreeDrops(178, 4855, tileCache, ref dropItem, ref secondaryItem);
                if (dropItem == 3)
                {
                    dropItemStack = Main.rand.Next(1, 3);
                }
                break;
            case TileID.TreeDiamond:
                WorldGen.SetGemTreeDrops(182, 4856, tileCache, ref dropItem, ref secondaryItem);
                if (dropItem == 3)
                {
                    dropItemStack = Main.rand.Next(1, 3);
                }
                break;
        }
    }
    public static void CheckJunglePlant(int i, int j, int type)
    {
        if (WorldGen.destroyObject)
        {
            return;
        }
        if (Main.tile[i, j].frameY >= 36 || Main.tile[i, j].type == TileID.LifeFruit || Main.tile[i, j].type == TileID.PlanteraBulb)
        {
            bool flag = false;
            int num = Main.tile[i, j].frameX / 18;
            int num2 = 0;
            while (num > 1)
            {
                num -= 2;
                num2++;
            }
            num = i - num;
            int num3 = 36;
            if (type == 236 || type == 238)
            {
                num3 = 0;
            }
            int num4;
            for (num4 = Main.tile[i, j].frameY / 18; num4 > 1; num4 -= 2)
            {
            }
            num4 = j - num4;
            int num5 = num2 * 36;
            for (int k = num; k < num + 2; k++)
            {
                for (int l = num4; l < num4 + 2; l++)
                {
                    TileUtils.Ensure(k, l);
                    if (!Main.tile[k, l].active() || Main.tile[k, l].type != type || Main.tile[k, l].frameX != (k - num) * 18 + num5 || Main.tile[k, l].frameY != (l - num4) * 18 + num3)
                    {
                        flag = true;
                    }
                }
                TileUtils.Ensure(k, num4 + 2);
                if (!WorldGen.SolidTile(k, num4 + 2) || Main.tile[k, num4 + 2].type != TileID.JungleGrass)
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                return;
            }
            if (type == 238)
            {
                float num6 = i * 16;
                float num7 = j * 16;
                float num8 = -1f;
                int plr = 0;
                for (int m = 0; m < 255; m++)
                {
                    float num9 = Math.Abs(Main.player[m].position.X - num6) + Math.Abs(Main.player[m].position.Y - num7);
                    if (num9 < num8 || num8 == -1f)
                    {
                        plr = m;
                        num8 = num9;
                    }
                }
                if (num8 / 16f < 50f)
                {
                    //NPC.SpawnOnPlayer(plr, NPCID.Plantera);
                    SpawnPlanteraOnPlayer(plr);
                }
            }
            if (type == 236)
            {
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 32, 32, ItemID.LifeFruit);
            }
            WorldGen.destroyObject = true;
            for (int n = num; n < num + 2; n++)
            {
                for (int num10 = num4; num10 < num4 + 2; num10++)
                {
                    TileUtils.Ensure(n, num10);
                    if (Main.tile[n, num10].type == type && Main.tile[n, num10].active())
                    {
                        WorldGen.KillTile(n, num10);
                    }
                }
            }
            WorldGen.destroyObject = false;
            return;
        }
        bool canKill = false;
        int num11 = j + Main.tile[i, j].frameY / 18 * -1;
        int num12 = Main.tile[i, j].frameX / 18;
        int num13 = 0;
        while (num12 > 2)
        {
            num12 -= 3;
            num13++;
        }
        num12 = i - num12;
        int num14 = num13 * 54;
        for (int num15 = num12; num15 < num12 + 3; num15++)
        {
            for (int num16 = num11; num16 < num11 + 2; num16++)
            {
                TileUtils.Ensure(num15, num16);
                if (!Main.tile[num15, num16].active() || Main.tile[num15, num16].type != type || Main.tile[num15, num16].frameX != (num15 - num12) * 18 + num14 || Main.tile[num15, num16].frameY != (num16 - num11) * 18)
                {
                    canKill = true;
                }
            }
            TileUtils.Ensure(num15, num11 + 2);
            if (!WorldGen.SolidTile(num15, num11 + 2) || Main.tile[num15, num11 + 2].type != TileID.JungleGrass)
            {
                canKill = true;
            }
        }
        if (!canKill)
        {
            return;
        }
        WorldGen.destroyObject = true;
        for (int num17 = num12; num17 < num12 + 3; num17++)
        {
            for (int num18 = num11; num18 < num11 + 3; num18++)
            {
                TileUtils.Ensure(num17, num18);
                if (Main.tile[num17, num18].type == type && Main.tile[num17, num18].active())
                {
                    WorldGen.KillTile(num17, num18);
                }
            }
        }
        WorldGen.destroyObject = false;
    }
    internal static void SpawnPlanteraOnPlayer(int plr)
    {
        const int type = NPCID.Plantera;
        bool findSuccess = false;
        int findTileX = 0;
        int findTileY = 0;
        int spawnMinX = (int)(Main.player[plr].position.X / 16f) - NPC.spawnRangeX * 2;
        int spawnMaxX = (int)(Main.player[plr].position.X / 16f) + NPC.spawnRangeX * 2;
        int spawnMinY = (int)(Main.player[plr].position.Y / 16f) - NPC.spawnRangeY * 2;
        int spawnMaxY = (int)(Main.player[plr].position.Y / 16f) + NPC.spawnRangeY * 2;
        int safeMinX = (int)(Main.player[plr].position.X / 16f) - NPC.safeRangeX;
        int safeMaxX = (int)(Main.player[plr].position.X / 16f) + NPC.safeRangeX;
        int safeMiny = (int)(Main.player[plr].position.Y / 16f) - NPC.safeRangeY;
        int safeMaxY = (int)(Main.player[plr].position.Y / 16f) + NPC.safeRangeY;
        Utils.SetValueInWorld(ref spawnMinX, ref spawnMaxX, ref spawnMinY, ref spawnMaxY);
        for (int m = 0; m < 1000; m++)
        {
            for (int n = 0; n < 100; n++)
            {
                int x = Main.rand.Next(spawnMinX, spawnMaxX);
                int y = Main.rand.Next(spawnMinY, spawnMaxY);
                if (!Main.tile[x, y].nactive() || !Main.tileSolid[Main.tile[x, y].type])
                {
                    if ((Main.wallHouse[Main.tile[x, y].wall] && m < 999))
                    {
                        continue;
                    }
                    for (int y1 = y; y1 < Main.maxTilesY; y1++)
                    {
                        if (Main.tile[x, y1].nactive() && Main.tileSolid[Main.tile[x, y1].type])
                        {
                            if ((x < safeMinX || x > safeMaxX || y1 < safeMiny || y1 > safeMaxY || m == 999) && ((x >= spawnMinX && x <= spawnMaxX && y1 >= spawnMinY && y1 <= spawnMaxY) || m == 999))
                            {
                                findTileX = x;
                                findTileY = y1;
                                findSuccess = true;
                            }
                            break;
                        }
                    }
                    if (findSuccess && m < 999)
                    {
                        int spaceMinX = findTileX - NPC.spawnSpaceX / 2;
                        int spaceMaxX = findTileX + NPC.spawnSpaceX / 2;
                        int spaceMinY = findTileY - NPC.spawnSpaceY;
                        int spaceMaxY = findTileY;
                        if (!Utils.InWorld(spaceMinX, spaceMaxX, spaceMinY, spaceMaxY))
                        {
                            findSuccess = false;
                        }
                        if (findSuccess)
                        {
                            for (int x1 = spaceMinX; x1 < spaceMaxX; x1++)
                            {
                                for (int y1 = spaceMinY; y1 < spaceMaxY; y1++)
                                {
                                    if (Main.tile[x1, y1].nactive() && Main.tileSolid[Main.tile[x1, y1].type])
                                    {
                                        findSuccess = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (findSuccess)
                {
                    break;
                }
            }
            if (findSuccess && m < 999)
            {
                var spawnRectangle = new Rectangle(findTileX * 16, findTileY * 16, 16, 16);
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active)
                    {
                        var safeRectangle = new Rectangle((int)(Main.player[i].position.X + Main.player[i].width / 2 - NPC.sWidth / 2 - NPC.safeRangeX), (int)(Main.player[i].position.Y + Main.player[i].height / 2 - NPC.sHeight / 2 - NPC.safeRangeY), NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                        if (spawnRectangle.Intersects(safeRectangle))
                        {
                            findSuccess = false;
                        }
                    }
                }
            }
            if (findSuccess)
            {
                break;
            }
        }
        if (findSuccess)
        {
            NPC.SpawnBoss(findTileX * 16 + 8, findTileY * 16, type, plr);
        }
    }
}
