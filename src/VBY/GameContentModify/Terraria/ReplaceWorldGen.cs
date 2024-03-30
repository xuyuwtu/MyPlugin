using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Chat;
using Terraria.Enums;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.GameContent.Achievements;

using TUtils = Terraria.Utils;
using VBY.GameContentModify.Config;

namespace VBY.GameContentModify;

[ReplaceType(typeof(WorldGen))]
public static class ReplaceWorldGen
{
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

        Liquid.skipCount++;
        if (Liquid.skipCount > 1)
        {
            Liquid.UpdateLiquid();
            Liquid.skipCount = 0;
        }
        int worldUpdateRate = WorldGen.GetWorldUpdateRate();
        if (worldUpdateRate == 0)
        {
            return;
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
                    if (Main.npc[i].active && Main.npc[i].homeless && Main.npc[i].townNPC && Main.npc[i].type != 368)
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
                    Projectile.NewProjectile(new EntitySource_ByProjectileSourceId(11), position.X, position.Y, speedX, speedY, 720, 0, 0f, Main.myPlayer, 0f, ai1);
                }
                else
                {
                    Projectile.NewProjectile(new EntitySource_ByProjectileSourceId(11), position.X, position.Y, speedX, speedY, 12, 999, 0f, Main.myPlayer, 0f, ai1);
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
    public static void hardUpdateWorld(int i, int j)
    {
        if ((WorldInfo.StatichardUpdateWorldCheck && !Main.hardMode) || Main.tile[i, j].inActive())
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
                        if (Main.tile[k, l].active() && Main.tile[k, l].type == 129)
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
            if (type == 60 && WorldGen.genRand.Next(300) == 0)
            {
                int num6 = i + WorldGen.genRand.Next(-10, 11);
                int num7 = j + WorldGen.genRand.Next(-10, 11);
                if (WorldGen.InWorld(num6, num7, 2) && Main.tile[num6, num7].active() && Main.tile[num6, num7].type == 59 && (!Main.tile[num6, num7 - 1].active() || (Main.tile[num6, num7 - 1].type != 5 && Main.tile[num6, num7 - 1].type != 236 && Main.tile[num6, num7 - 1].type != 238)) && WorldGen.Chlorophyte(num6, num7) && OTAPI.Hooks.WorldGen.InvokeHardmodeTileUpdate(num6, num7, 211))
                {
                    Main.tile[num6, num7].type = 211;
                    WorldGen.SquareTileFrame(num6, num7);
                    NetMessage.SendTileSquare(-1, num6, num7);
                }
            }
            if (type == 211 || type == 346)
            {
                int num8 = i;
                int num9 = j;
                if (WorldGen.genRand.Next(3) != 0)
                {
                    int num10 = WorldGen.genRand.Next(4);
                    if (num10 == 0)
                    {
                        num8++;
                    }
                    if (num10 == 1)
                    {
                        num8--;
                    }
                    if (num10 == 2)
                    {
                        num9++;
                    }
                    if (num10 == 3)
                    {
                        num9--;
                    }
                    if (WorldGen.InWorld(num8, num9, 2) && Main.tile[num8, num9].active() && (Main.tile[num8, num9].type == 59 || Main.tile[num8, num9].type == 60) && WorldGen.Chlorophyte(num8, num9) && OTAPI.Hooks.WorldGen.InvokeHardmodeTileUpdate(num8, num9, 211))
                    {
                        Main.tile[num8, num9].type = 211;
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
                    if (Main.tile[num8, num9].type == 661 || Main.tile[num8, num9].type == 662 || Main.tile[num8, num9].type == 23 || Main.tile[num8, num9].type == 199 || Main.tile[num8, num9].type == 2 || Main.tile[num8, num9].type == 477 || Main.tile[num8, num9].type == 492 || Main.tile[num8, num9].type == 109)
                    {
                        if (OTAPI.Hooks.WorldGen.InvokeHardmodeTileUpdate(num8, num9, 60))
                        {
                            Main.tile[num8, num9].type = 60;
                            WorldGen.SquareTileFrame(num8, num9);
                            NetMessage.SendTileSquare(-1, num8, num9);
                        }
                        flag = true;
                    }
                    else if (Main.tile[num8, num9].type == 0)
                    {
                        if (OTAPI.Hooks.WorldGen.InvokeHardmodeTileUpdate(num8, num9, 59))
                        {
                            Main.tile[num8, num9].type = 59;
                            WorldGen.SquareTileFrame(num8, num9);
                            NetMessage.SendTileSquare(-1, num8, num9);
                        }
                        flag = true;
                    }
                    else if (Main.tile[num8, num9].type == 25 || Main.tile[num8, num9].type == 203)
                    {
                        if (OTAPI.Hooks.WorldGen.InvokeHardmodeTileUpdate(num8, num9, 112))
                        {
                            Main.tile[num8, num9].type = 1;
                            WorldGen.SquareTileFrame(num8, num9);
                            NetMessage.SendTileSquare(-1, num8, num9);
                        }
                        flag = true;
                    }
                    else if (Main.tile[num8, num9].type == 112 || Main.tile[num8, num9].type == 234)
                    {
                        if (OTAPI.Hooks.WorldGen.InvokeHardmodeTileUpdate(num8, num9, 53))
                        {
                            Main.tile[num8, num9].type = 53;
                            WorldGen.SquareTileFrame(num8, num9);
                            NetMessage.SendTileSquare(-1, num8, num9);
                        }
                        flag = true;
                    }
                    else if (Main.tile[num8, num9].type == 398 || Main.tile[num8, num9].type == 399)
                    {
                        if (OTAPI.Hooks.WorldGen.InvokeHardmodeTileUpdate(num8, num9, 397))
                        {
                            Main.tile[num8, num9].type = 397;
                            WorldGen.SquareTileFrame(num8, num9);
                            NetMessage.SendTileSquare(-1, num8, num9);
                        }
                        flag = true;
                    }
                    else if (Main.tile[num8, num9].type == 400 || Main.tile[num8, num9].type == 401)
                    {
                        if (OTAPI.Hooks.WorldGen.InvokeHardmodeTileUpdate(num8, num9, 396))
                        {
                            Main.tile[num8, num9].type = 396;
                            WorldGen.SquareTileFrame(num8, num9);
                            NetMessage.SendTileSquare(-1, num8, num9);
                        }
                        flag = true;
                    }
                    else if (Main.tile[num8, num9].type == 24 || Main.tile[num8, num9].type == 201 || Main.tile[num8, num9].type == 32 || Main.tile[num8, num9].type == 352 || Main.tile[num8, num9].type == 636 || Main.tile[num8, num9].type == 205)
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
        if(WorldInfo.StaticInfectionTransmissionDistance < 1)
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
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(num, num2), num * 16, num2 * 16, 32, 32, 29);
                    break;
                case 639:
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(num, num2), num * 16, num2 * 16, 32, 32, 109);
                    break;
                case 31:
                    var config = GameContentModify.MainConfig.Instance.Orb;
                    var array = crimson ? config.CrimsonShadowOrbDropItems : config.CorruptionShadowOrbDropItems;
                    int index = Main.rand.Next(array.Length);
                    var items = array[index];
                    foreach (var item in items)
                    {
                        item.NewItem(WorldGen.GetItemSource_FromTileBreak(num, num2), num * 16, num2 * 16, 32, 32);
                    }
                    WorldGen.shadowOrbSmashed = true;
                    WorldGen.shadowOrbCount++;
                    if (WorldGen.shadowOrbCount >= config.SpanwNPCSmashedCount)
                    {
                        if (!(NPC.AnyNPCs(266) && crimson) && (!NPC.AnyNPCs(13) || crimson))
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
                                NPC.SpawnOnPlayer(plr, 266);
                            }
                            else
                            {
                                NPC.SpawnOnPlayer(plr, 13);
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
                    AchievementsHelper.NotifyProgressionEvent(7);
                    break;
            }
        }
        WorldGen.destroyObject = false;
    }
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
        //if (!MainConfigInfo.StaticDisableShakeTreeDropBombProj && Main.getGoodWorld && rand.Next(17) == 0)
        //{
        //    Projectile.NewProjectile(WorldGen.GetProjectileSource_ShakeTree(x, y), x * 16, y * 16, Main.rand.Next(-100, 101) * 0.002f, 0f, 28, 0, 0f, Main.myPlayer, 16f, 16f);
        //}
        //else if (rand.Next(300) == 0 && treeType == TreeTypes.Forest)
        if (rand.Next(300) == 0 && treeType == TreeTypes.Forest)
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
            Projectile.NewProjectile(WorldGen.GetProjectileSource_ShakeTree(x, y), x * 16 + 8, (y - 1) * 16, 0f, 0f, 655, 0, 0f, Main.myPlayer);
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
            NetMessage.SendData(112, -1, -1, null, 1, x, y, 1f, passStyle);
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
                Projectile.NewProjectile(WorldGen.GetProjectileSource_TileBreak(i, j), i * 16 + 16, j * 16 + 16, 0f, -12f, 518, 0, 0f, Main.myPlayer);
            }
            return;
        }
        if (WorldGen.genRand.Next(35) == 0 && Main.wallDungeon[Main.tile[i, j].wall] && j > Main.worldSurface)
        {
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 327);
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
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 75);
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
                int num3 = ((Main.rand.Next(9) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, -7) : ((Main.rand.Next(7) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, -8) : ((Main.rand.Next(6) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, -9) : ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, 1) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, -3)))));
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
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 75);
            }
            return;
        }
        if (Main.remixWorld && i > Main.maxTilesX * 0.37 && i < Main.maxTilesX * 0.63 && j > Main.maxTilesY - 220)
        {
            int stack = Main.rand.Next(20, 41);
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 965, stack);
            return;
        }
        if (WorldGen.genRand.Next(45) == 0 || (Main.rand.Next(45) == 0 && Main.expertMode))
        {
            if (j < Main.worldSurface)
            {
                int randomValue = WorldGen.genRand.Next(10);
                if (randomValue >= 7)
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2350, WorldGen.genRand.Next(1, 3));
                }
                else
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, stackalloc int[]{ 292, 298, 299, 290, 2322, 2324, 2325 }[randomValue]);
                }
            }
            else if (flag)
            {
                int randomValue = WorldGen.genRand.Next(11);
                if (randomValue >= 9)
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2350, WorldGen.genRand.Next(1, 3));
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
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2350, WorldGen.genRand.Next(1, 3));
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
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 4870);
                }
            }
            return;
        }
        if (Main.netMode == 2 && Main.rand.Next(30) == 0)
        {
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2997);
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
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 58);
            if (Main.rand.Next(2) == 0)
            {
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 58);
            }
            if (Main.expertMode)
            {
                if (Main.rand.Next(2) == 0)
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 58);
                }
                if (Main.rand.Next(2) == 0)
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 58);
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
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 965, stack3);
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
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 74, num16);
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
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 73, num17);
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
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 72, num18);
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
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 71, num19);
        }
    }
    //public static void dropMeteor()
    //{
    //    int meteoriteCount = 0;
    //    double num2 = Main.maxTilesX / 4200.0;
    //    int maxMeteoriteCount = (int)(400.0 * num2);
    //    for (int j = 5; j < Main.maxTilesX - 5; j++)
    //    {
    //        for (int k = 5; k < Main.worldSurface; k++)
    //        {
    //            if (Main.tile[j, k].active() && Main.tile[j, k].type == TileID.Meteorite)
    //            {
    //                meteoriteCount++;
    //                if (meteoriteCount > maxMeteoriteCount)
    //                {
    //                    return;
    //                }
    //            }
    //        }
    //    }
    //    bool notNeedCheck = !Main.player.Any(x => x.active);
    //    double spawnCount = 600.0;
    //    int checkCount = 0;
    //    while (!notNeedCheck)
    //    {
    //        double safeWidth = Main.maxTilesX * 0.08;
    //        int dropX = Main.rand.Next(150, Main.maxTilesX - 150);
    //        while (dropX > Main.spawnTileX - safeWidth && dropX < Main.spawnTileX + safeWidth)
    //        {
    //            dropX = Main.rand.Next(150, Main.maxTilesX - 150);
    //        }
    //        for (int dropY = (int)(Main.worldSurface * 0.3); dropY < Main.maxTilesY; dropY++)
    //        {
    //            ITile tile = Main.tile[dropX, dropY];
    //            if (!tile.active() || !Main.tileSolid[tile.type] || TileID.Sets.Platforms[tile.type])
    //            {
    //                continue;
    //            }
    //            int solidCount = 0;
    //            int radius = 15;
    //            for (int m = dropX - radius; m < dropX + radius; m++)
    //            {
    //                for (int n = dropY - radius; n < dropY + radius; n++)
    //                {
    //                    if (WorldGen.SolidTile(m, n))
    //                    {
    //                        solidCount++;
    //                        if (Main.tile[m, n].type == TileID.Cloud || Main.tile[m, n].type == TileID.Sunplate)
    //                        {
    //                            solidCount -= 100;
    //                        }
    //                    }
    //                    else if (Main.tile[m, n].liquid > 0)
    //                    {
    //                        solidCount--;
    //                    }
    //                }
    //            }
    //            if (solidCount >= spawnCount)
    //            {
    //                notNeedCheck = WorldGen.meteor(dropX, dropY);
    //            }
    //            else
    //            {
    //                spawnCount -= 0.5;
    //            }
    //            break;
    //        }
    //        checkCount++;
    //        if (spawnCount < 100.0 || checkCount >= Main.maxTilesX * 5)
    //        {
    //            break;
    //        }
    //    }
    //}
    //public static bool meteor(int x, int y, bool ignorePlayers = false)
    //{
    //    if (x < 50 || x > Main.maxTilesX - 50)
    //    {
    //        return false;
    //    }
    //    if (y < 50 || y > Main.maxTilesY - 50)
    //    {
    //        return false;
    //    }
    //    int radius = 35;
    //    Rectangle rectangle = new Rectangle((x - radius) * 16, (y - radius) * 16, radius * 2 * 16, radius * 2 * 16);
    //    if (!ignorePlayers)
    //    {
    //        for (int k = 0; k < 255; k++)
    //        {
    //            if (Main.player[k].active)
    //            {
    //                Rectangle value = new Rectangle((int)(Main.player[k].position.X + Main.player[k].width / 2 - NPC.sWidth / 2 - NPC.safeRangeX), (int)(Main.player[k].position.Y + Main.player[k].height / 2 - NPC.sHeight / 2 - NPC.safeRangeY), NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
    //                if (rectangle.Intersects(value))
    //                {
    //                    return false;
    //                }
    //            }
    //        }
    //    }
    //    for (int l = 0; l < 200; l++)
    //    {
    //        if (Main.npc[l].active)
    //        {
    //            Rectangle value2 = new Rectangle((int)Main.npc[l].position.X, (int)Main.npc[l].position.Y, Main.npc[l].width, Main.npc[l].height);
    //            if (rectangle.Intersects(value2))
    //            {
    //                return false;
    //            }
    //        }
    //    }
    //    for (int m = x - radius; m < x + radius; m++)
    //    {
    //        for (int n = y - radius; n < y + radius; n++)
    //        {
    //            if (Main.tile[m, n].active())
    //            {
    //                if (TileID.Sets.BasicChest[Main.tile[m, n].type] || Main.tileDungeon[Main.tile[m, n].type])
    //                {
    //                    return false;
    //                }
    //                switch (Main.tile[m, n].type)
    //                {
    //                    case 226:
    //                    case 470:
    //                    case 475:
    //                    case 488:
    //                    case 597:
    //                        return false;
    //                }
    //            }
    //        }
    //    }
    //    if (!OTAPI.Hooks.WorldGen.InvokeMeteor(ref x, ref y))
    //    {
    //        return false;
    //    }
    //    WorldGen.stopDrops = true;
    //    radius = WorldGen.genRand.Next(17, 23);
    //    for (int i = x - radius; i < x + radius; i++)
    //    {
    //        for (int j = y - radius; j < y + radius; j++)
    //        {
    //            if (j <= y + Main.rand.Next(-2, 3) - 5)
    //            {
    //                continue;
    //            }
    //            double width = Math.Abs(x - i);
    //            double height = Math.Abs(y - j);
    //            if (Math.Sqrt(width * width + height * height) < radius * 0.9 + Main.rand.Next(-4, 5))
    //            {
    //                if (!Main.tileSolid[Main.tile[i, j].type])
    //                {
    //                    Main.tile[i, j].active(active: false);
    //                }
    //                Main.tile[i, j].type = TileID.Meteorite;
    //            }
    //        }
    //    }
    //    radius = WorldGen.genRand.Next(8, 14);
    //    for (int i = x - radius; i < x + radius; i++)
    //    {
    //        for (int j = y - radius; j < y + radius; j++)
    //        {
    //            if (j > y + Main.rand.Next(-2, 3) - 4)
    //            {
    //                double width = Math.Abs(x - i);
    //                double height = Math.Abs(y - j);
    //                if (Math.Sqrt(width * width + height * height) < radius * 0.8 + Main.rand.Next(-3, 4))
    //                {
    //                    Main.tile[i, j].active(active: false);
    //                }
    //            }
    //        }
    //    }
    //    radius = WorldGen.genRand.Next(25, 35);
    //    for (int i = x - radius; i < x + radius; i++)
    //    {
    //        for (int j = y - radius; j < y + radius; j++)
    //        {
    //            double width = Math.Abs(x - i);
    //            double height = Math.Abs(y - j);
    //            if (Math.Sqrt(width * width + height * height) < radius * 0.7)
    //            {
    //                if (TileID.Sets.GetsDestroyedForMeteors[Main.tile[i, j].type])
    //                {
    //                    WorldGen.KillTile(i, j);
    //                }
    //                Main.tile[i, j].liquid = 0;
    //            }
    //            if (Main.tile[i, j].type == TileID.Meteorite)
    //            {
    //                if (!WorldGen.SolidTile(i - 1, j) && !WorldGen.SolidTile(i + 1, j) && !WorldGen.SolidTile(i, j - 1) && !WorldGen.SolidTile(i, j + 1))
    //                {
    //                    Main.tile[i, j].active(active: false);
    //                }
    //                else if ((Main.tile[i, j].halfBrick() || Main.tile[i - 1, j].topSlope()) && !WorldGen.SolidTile(i, j + 1))
    //                {
    //                    Main.tile[i, j].active(active: false);
    //                }
    //            }
    //            WorldGen.SquareTileFrame(i, j);
    //            WorldGen.SquareWallFrame(i, j);
    //        }
    //    }
    //    radius = WorldGen.genRand.Next(23, 32);
    //    for (int i = x - radius; i < x + radius; i++)
    //    {
    //        for (int j = y - radius; j < y + radius; j++)
    //        {
    //            if (j <= y + WorldGen.genRand.Next(-3, 4) - 3 || !Main.tile[i, j].active() || Main.rand.Next(10) != 0)
    //            {
    //                continue;
    //            }
    //            double width = Math.Abs(x - i);
    //            double height = Math.Abs(y - j);
    //            if (Math.Sqrt(width * width + height * height) < radius * 0.8)
    //            {
    //                if (TileID.Sets.GetsDestroyedForMeteors[Main.tile[i, j].type])
    //                {
    //                    WorldGen.KillTile(i, j);
    //                }
    //                Main.tile[i, j].type = TileID.Meteorite;
    //                WorldGen.SquareTileFrame(i, j);
    //            }
    //        }
    //    }
    //    radius = WorldGen.genRand.Next(30, 38);
    //    for (int i = x - radius; i < x + radius; i++)
    //    {
    //        for (int j = y - radius; j < y + radius; j++)
    //        {
    //            if (j <= y + WorldGen.genRand.Next(-2, 3) || !Main.tile[i, j].active() || Main.rand.Next(20) != 0)
    //            {
    //                continue;
    //            }
    //            double width = Math.Abs(x - i);
    //            double height = Math.Abs(y - j);
    //            if (Math.Sqrt(width * width + height * height) < radius * 0.85)
    //            {
    //                if (TileID.Sets.GetsDestroyedForMeteors[Main.tile[i, j].type])
    //                {
    //                    WorldGen.KillTile(i, j);
    //                }
    //                Main.tile[i, j].type = TileID.Meteorite;
    //                WorldGen.SquareTileFrame(i, j);
    //            }
    //        }
    //    }
    //    WorldGen.stopDrops = false;
    //    ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.gen[59].Key), new Color(50, 255, 130));
    //    NetMessage.SendTileSquare(-1, x, y, 40);
    //    return true;
    //}
    public static void GrowAlch(int x, int y)
    {
        if (!Main.tile[x, y].active())
        {
            return;
        }
        if (Main.tile[x, y].liquid > 0)
        {
            int style = Main.tile[x, y].frameX / 18;
            if ((!Main.tile[x, y].lava() || style != TileSubID.Herbs.Fireblossom) && (Main.tile[x, y].liquidType() != LiquidID.Water || (style != TileSubID.Herbs.Moonglow && style != TileSubID.Herbs.Waterleaf)))
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

}
