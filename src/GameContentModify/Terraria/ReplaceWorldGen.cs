using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.GameContent.Achievements;

using TUtils = Terraria.Utils;
using VBY.GameContentModify.Config;

namespace VBY.GameContentModify;

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
        if (Main.dayTime && !Main.remixWorld)
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
            int num11 = -1;
            if (Main.expertMode && Main.rand.Next(15) == 0)
            {
                int num12 = Player.FindClosest(position, 1, 1);
                if (Main.player[num12].position.Y < Main.worldSurface * 16.0 && Main.player[num12].afkCounter < 3600)
                {
                    int num13 = Main.rand.Next(1, 640);
                    position.X = Main.player[num12].position.X + Main.rand.Next(-num13, num13 + 1);
                    num11 = num12;
                }
            }
            if (!Collision.SolidCollision(position, 16, 16))
            {
                float num14 = Main.rand.Next(-100, 101);
                float num15 = Main.rand.Next(200) + 100;
                float num16 = (float)Math.Sqrt(num14 * num14 + num15 * num15);
                num16 = num8 / num16;
                num14 *= num16;
                num15 *= num16;
                Projectile.NewProjectile(new EntitySource_ByProjectileSourceId(11), position.X, position.Y, num14, num15, 720, 0, 0f, Main.myPlayer, 0f, num11);
            }
        }
    }
    internal static ushort[] CanSpreadCorruptTileIDs =
    {
        23, 25, 32, 112, 163, 400, 398, 636, 661
    };
    internal static ushort[] ConvertTableCorrupt =
    {
        2, 23,
        53, 112,
        396, 400,
        397, 398,
        60, 661,
        69, 32,
        161, 163,
        1, 25
    };
    internal static ushort[] CanSpreadCrimsonTileIDs =
    {
        199, 200, 201, 203, 205, 234, 352, 401, 399, 662
    };
    internal static ushort[] ConvertTableCrimson =
    {
        2, 199,
        53, 234,
        396, 401,
        397, 399,
        60, 662,
        69, 352,
        161, 200,
        1, 203
    }; 
    internal static ushort[] CanSpreadHallowTileIDs =
    {
        109, 110, 113, 115, 116, 117, 164, 402, 403, 492
    };
    internal static ushort[] ConvertTableHallow =
    {
        2, 109,
        477, 492,
        53, 116,
        396, 403,
        397, 402,
        161, 164,
        1, 117
    };
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名样式", Justification = "<挂起>")]
    internal static void hardUpdateWorldConvert(ushort[] convertTable, int i, int j)
    {
        bool continueConvert = true;
        while (continueConvert)
        {
            continueConvert = false;
            int x = i + WorldGen.genRand.Next(-3, 4);
            int y = j + WorldGen.genRand.Next(-3, 4);
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名样式", Justification = "<挂起>")]
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
                    if (Main.netMode == 2)
                    {
                        NetMessage.SendTileSquare(-1, num6, num7);
                    }
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
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendTileSquare(-1, num8, num9);
                        }
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
                            if (Main.netMode == 2)
                            {
                                NetMessage.SendTileSquare(-1, num8, num9);
                            }
                        }
                        flag = true;
                    }
                    else if (Main.tile[num8, num9].type == 0)
                    {
                        if (OTAPI.Hooks.WorldGen.InvokeHardmodeTileUpdate(num8, num9, 59))
                        {
                            Main.tile[num8, num9].type = 59;
                            WorldGen.SquareTileFrame(num8, num9);
                            if (Main.netMode == 2)
                            {
                                NetMessage.SendTileSquare(-1, num8, num9);
                            }
                        }
                        flag = true;
                    }
                    else if (Main.tile[num8, num9].type == 25 || Main.tile[num8, num9].type == 203)
                    {
                        if (OTAPI.Hooks.WorldGen.InvokeHardmodeTileUpdate(num8, num9, 112))
                        {
                            Main.tile[num8, num9].type = 1;
                            WorldGen.SquareTileFrame(num8, num9);
                            if (Main.netMode == 2)
                            {
                                NetMessage.SendTileSquare(-1, num8, num9);
                            }
                        }
                        flag = true;
                    }
                    else if (Main.tile[num8, num9].type == 112 || Main.tile[num8, num9].type == 234)
                    {
                        if (OTAPI.Hooks.WorldGen.InvokeHardmodeTileUpdate(num8, num9, 53))
                        {
                            Main.tile[num8, num9].type = 53;
                            WorldGen.SquareTileFrame(num8, num9);
                            if (Main.netMode == 2)
                            {
                                NetMessage.SendTileSquare(-1, num8, num9);
                            }
                        }
                        flag = true;
                    }
                    else if (Main.tile[num8, num9].type == 398 || Main.tile[num8, num9].type == 399)
                    {
                        if (OTAPI.Hooks.WorldGen.InvokeHardmodeTileUpdate(num8, num9, 397))
                        {
                            Main.tile[num8, num9].type = 397;
                            WorldGen.SquareTileFrame(num8, num9);
                            if (Main.netMode == 2)
                            {
                                NetMessage.SendTileSquare(-1, num8, num9);
                            }
                        }
                        flag = true;
                    }
                    else if (Main.tile[num8, num9].type == 400 || Main.tile[num8, num9].type == 401)
                    {
                        if (OTAPI.Hooks.WorldGen.InvokeHardmodeTileUpdate(num8, num9, 396))
                        {
                            Main.tile[num8, num9].type = 396;
                            WorldGen.SquareTileFrame(num8, num9);
                            if (Main.netMode == 2)
                            {
                                NetMessage.SendTileSquare(-1, num8, num9);
                            }
                        }
                        flag = true;
                    }
                    else if (Main.tile[num8, num9].type == 24 || Main.tile[num8, num9].type == 201 || Main.tile[num8, num9].type == 32 || Main.tile[num8, num9].type == 352 || Main.tile[num8, num9].type == 636 || Main.tile[num8, num9].type == 205)
                    {
                        WorldGen.KillTile(num8, num9);
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendTileSquare(-1, num8, num9);
                        }
                        flag = true;
                    }
                }
            }
        }
        if ((NPC.downedPlantBoss && WorldGen.genRand.Next(2) != 0) || !WorldGen.AllowedToSpreadInfections)
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
                        if (WorldGen.shadowOrbCount == config.SpanwNPCSmashedCount - 1)
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
}
