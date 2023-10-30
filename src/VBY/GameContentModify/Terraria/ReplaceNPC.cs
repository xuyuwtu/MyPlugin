using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Events;
using Terraria.GameContent.Drawing;

using VBY.GameContentModify.Config;
using static VBY.GameContentModify.GameContentModify;

namespace VBY.GameContentModify;

public static class ReplaceNPC
{
    public static bool BigMimicSummonCheck(int x, int y, Player user)
    {
        if (!Main.hardMode)
        {
            return false;
        }
        int chestIndex = Chest.FindChest(x, y);
        if (chestIndex < 0)
        {
            return false;
        }
        ushort chestTileType = Main.tile[Main.chest[chestIndex].x, Main.chest[chestIndex].y].type;
        int chestStyle = Main.tile[Main.chest[chestIndex].x, Main.chest[chestIndex].y].frameX / 36;
        Chest chest = Main.chest[chestIndex];
        bool isBasicChest = TileID.Sets.BasicChest[chestTileType];

        bool canExecute = false;
        ChestSpawnInfo? spawnInfo = null;

        if (isBasicChest && (chestTileType != 21 || chestStyle < 5 || chestStyle > 6))
        {
            foreach (var info in ChestSpawnConfig.Instance)
            {
                if (info.CanExecute(chest, user))
                {
                    canExecute = true;
                    spawnInfo = info;
                    break;
                }
            }
        }
        if (canExecute && spawnInfo is not null)
        {
            if (Main.tile[x, y].frameX % 36 != 0)
            {
                x--;
            }
            if (Main.tile[x, y].frameY % 36 != 0)
            {
                y--;
            }
            int chestIndex2 = Chest.FindChest(x, y);
            chest.SetEmpty();
            Chest.DestroyChest(x, y);
            for (int k = x; k <= x + 1; k++)
            {
                for (int l = y; l <= y + 1; l++)
                {
                    if (TileID.Sets.BasicChest[Main.tile[k, l].type])
                    {
                        Main.tile[k, l].ClearTile();
                    }
                }
            }
            int action = 1; //Kill Chest
            if (Main.tile[x, y].type == 467)
            {
                action = 5; //Kill Containers2
            }
            NetMessage.SendData(34, -1, -1, null, action, x, y, 0f, chestIndex2);
            NetMessage.SendTileSquare(-1, x, y, 3);
            spawnInfo.Execute(x, y, user);
        }
        return false;
    }
    public static void SpawnNPC()
    {
        if (NPC.noSpawnCycle)
        {
            NPC.noSpawnCycle = false;
            return;
        }
        bool tooWindyForButterflies = NPC.TooWindyForButterflies;
        bool flag = (double)Main.windSpeedTarget < -0.4 || (double)Main.windSpeedTarget > 0.4;
        NPC.RevengeManager.CheckRespawns();
        int num = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        for (int i = 0; i < 255; i++)
        {
            if (Main.player[i].active)
            {
                num4++;
            }
        }
        float num5 = 0f;
        for (int j = 0; j < 200; j++)
        {
            if (Main.npc[j].active)
            {
                switch (Main.npc[j].type)
                {
                    case 315:
                    case 325:
                    case 327:
                    case 328:
                    case 344:
                    case 345:
                    case 346:
                        num5 += Main.npc[j].npcSlots;
                        break;
                }
            }
        }
        float num6 = (int)((float)NPC.defaultMaxSpawns * (2f + 0.3f * (float)num4));
        for (int k = 0; k < 255; k++)
        {
            if (!Main.player[k].active || Main.player[k].dead)
            {
                continue;
            }

            bool flag3 = false;
            if (Main.player[k].isNearNPC(398, NPC.MoonLordFightingDistance))
            {
                continue;
            }
            if (Main.slimeRain)
            {
                NPC.SlimeRainSpawns(k);
            }
            bool flag4 = false;
            bool flag5 = false;
            bool flag6 = false;
            bool flag7 = false;
            bool flag8 = false;
            bool flag9 = false;
            bool flag10 = false;
            bool flag11 = false;
            bool flag12 = false;
            bool flag13 = false;
            bool flag14 = NPC.downedPlantBoss && Main.hardMode;
            bool isItAHappyWindyDay = Main.IsItAHappyWindyDay;
            if (Main.player[k].active && Main.invasionType > 0 && Main.invasionDelay == 0 && Main.invasionSize > 0 && ((double)Main.player[k].position.Y < Main.worldSurface * 16.0 + (double)NPC.sHeight || Main.remixWorld))
            {
                int num7 = 3000;
                if ((double)Main.player[k].position.X > Main.invasionX * 16.0 - (double)num7 && (double)Main.player[k].position.X < Main.invasionX * 16.0 + (double)num7)
                {
                    flag6 = true;
                }
                else if (Main.invasionX >= (double)(Main.maxTilesX / 2 - 5) && Main.invasionX <= (double)(Main.maxTilesX / 2 + 5))
                {
                    for (int l = 0; l < 200; l++)
                    {
                        if (Main.npc[l].townNPC && Math.Abs(Main.player[k].position.X - Main.npc[l].Center.X) < (float)num7)
                        {
                            if (Main.rand.Next(3) != 0)
                            {
                                flag6 = true;
                            }
                            break;
                        }
                    }
                }
            }
            if (Main.player[k].ZoneTowerSolar || Main.player[k].ZoneTowerNebula || Main.player[k].ZoneTowerVortex || Main.player[k].ZoneTowerStardust)
            {
                flag6 = true;
            }
            int num8 = (int)(Main.player[k].position.X + (float)(Main.player[k].width / 2)) / 16;
            int num9 = (int)(Main.player[k].position.Y + (float)(Main.player[k].height / 2)) / 16;
            if (Main.wallHouse[Main.tile[num8, num9].wall])
            {
                flag5 = true;
            }
            if (Main.tile[num8, num9].wall == 87)
            {
                flag4 = true;
            }
            bool flag2 = false;
            NPC.spawnRate = NPC.defaultSpawnRate;
            NPC.maxSpawns = NPC.defaultMaxSpawns;
            if (Main.hardMode)
            {
                NPC.spawnRate = (int)((double)NPC.defaultSpawnRate * 0.9);
                NPC.maxSpawns = NPC.defaultMaxSpawns + 1;
            }
            if (Main.player[k].position.Y > (float)(Main.UnderworldLayer * 16))
            {
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 2f);
            }
            else if ((double)Main.player[k].position.Y > Main.rockLayer * 16.0 + (double)NPC.sHeight)
            {
                if (Main.remixWorld)
                {
                    if (Main.hardMode)
                    {
                        NPC.spawnRate = (int)((double)NPC.spawnRate * 0.45);
                        NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.8f);
                    }
                    else
                    {
                        NPC.spawnRate = (int)((double)NPC.spawnRate * 0.5);
                        NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.7f);
                    }
                }
                else
                {
                    NPC.spawnRate = (int)((double)NPC.spawnRate * 0.4);
                    NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.9f);
                }
            }
            else if ((double)Main.player[k].position.Y > Main.worldSurface * 16.0 + (double)NPC.sHeight)
            {
                if (Main.remixWorld)
                {
                    NPC.spawnRate = (int)((double)NPC.spawnRate * 0.4);
                    NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.9f);
                }
                else if (Main.hardMode)
                {
                    NPC.spawnRate = (int)((double)NPC.spawnRate * 0.45);
                    NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.8f);
                }
                else
                {
                    NPC.spawnRate = (int)((double)NPC.spawnRate * 0.5);
                    NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.7f);
                }
            }
            else if (Main.remixWorld)
            {
                if (!Main.dayTime)
                {
                    NPC.spawnRate = (int)((double)NPC.spawnRate * 0.6);
                    NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.3f);
                }
            }
            else if (!Main.dayTime)
            {
                NPC.spawnRate = (int)((double)NPC.spawnRate * 0.6);
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.3f);
                if (Main.bloodMoon)
                {
                    NPC.spawnRate = (int)((double)NPC.spawnRate * 0.3);
                    NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.8f);
                }
                if ((Main.pumpkinMoon || Main.snowMoon) && (double)Main.player[k].position.Y < Main.worldSurface * 16.0)
                {
                    NPC.spawnRate = (int)((double)NPC.spawnRate * 0.2);
                    NPC.maxSpawns *= 2;
                }
            }
            else if (Main.dayTime && Main.eclipse)
            {
                NPC.spawnRate = (int)((double)NPC.spawnRate * 0.2);
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.9f);
            }
            if (Main.remixWorld)
            {
                if (!Main.dayTime)
                {
                    if (Main.bloodMoon)
                    {
                        NPC.spawnRate = (int)((double)NPC.spawnRate * 0.3);
                        NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.8f);
                        if ((double)Main.player[k].position.Y > Main.rockLayer * 16.0 + (double)NPC.sHeight)
                        {
                            NPC.spawnRate = (int)((double)NPC.spawnRate * 0.6);
                        }
                    }
                    if (Main.pumpkinMoon || Main.snowMoon)
                    {
                        NPC.spawnRate = (int)((double)NPC.spawnRate * 0.2);
                        NPC.maxSpawns *= 2;
                        if ((double)Main.player[k].position.Y > Main.rockLayer * 16.0 + (double)NPC.sHeight)
                        {
                            NPC.spawnRate = (int)((double)NPC.spawnRate * 0.6);
                        }
                    }
                }
                else if (Main.dayTime && Main.eclipse)
                {
                    NPC.spawnRate = (int)((double)NPC.spawnRate * 0.2);
                    NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.9f);
                }
            }
            if (Main.player[k].ZoneSnow && (double)(Main.player[k].position.Y / 16f) < Main.worldSurface)
            {
                NPC.maxSpawns = (int)((float)NPC.maxSpawns + (float)NPC.maxSpawns * Main.cloudAlpha);
                NPC.spawnRate = (int)((float)NPC.spawnRate * (1f - Main.cloudAlpha + 1f) / 2f);
            }
            if (Main.drunkWorld && Main.tile[num8, num9].wall == 86)
            {
                NPC.spawnRate = (int)((double)NPC.spawnRate * 0.3);
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.8f);
            }
            if (Main.player[k].ZoneDungeon)
            {
                NPC.spawnRate = (int)((double)NPC.spawnRate * 0.3);
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.8f);
            }
            else if (Main.player[k].ZoneSandstorm)
            {
                NPC.spawnRate = (int)((float)NPC.spawnRate * (Main.hardMode ? 0.4f : 0.9f));
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * (Main.hardMode ? 1.5f : 1.2f));
            }
            else if (Main.player[k].ZoneUndergroundDesert)
            {
                NPC.spawnRate = (int)((float)NPC.spawnRate * 0.2f);
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 3f);
            }
            else if (Main.player[k].ZoneJungle)
            {
                if (Main.player[k].townNPCs == 0f)
                {
                    NPC.spawnRate = (int)((double)NPC.spawnRate * 0.4);
                    NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.5f);
                }
                else if (Main.player[k].townNPCs == 1f)
                {
                    NPC.spawnRate = (int)((double)NPC.spawnRate * 0.55);
                    NPC.maxSpawns = (int)((double)NPC.maxSpawns * 1.4);
                }
                else if (Main.player[k].townNPCs == 2f)
                {
                    NPC.spawnRate = (int)((double)NPC.spawnRate * 0.7);
                    NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.3f);
                }
                else
                {
                    NPC.spawnRate = (int)((double)NPC.spawnRate * 0.85);
                    NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.2f);
                }
            }
            else if (Main.player[k].ZoneCorrupt || Main.player[k].ZoneCrimson)
            {
                NPC.spawnRate = (int)((double)NPC.spawnRate * 0.65);
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.3f);
            }
            else if (Main.player[k].ZoneMeteor)
            {
                NPC.spawnRate = (int)((double)NPC.spawnRate * 0.4);
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.1f);
            }
            if (flag4)
            {
                NPC.spawnRate = (int)((float)NPC.spawnRate * 0.8f);
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.2f);
                if (Main.remixWorld)
                {
                    NPC.spawnRate = (int)((double)NPC.spawnRate * 0.4);
                    NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.5f);
                }
            }
            if (Main.remixWorld && (Main.player[k].ZoneCorrupt || Main.player[k].ZoneCrimson) && (double)(Main.player[k].position.Y / 16f) < Main.worldSurface)
            {
                NPC.spawnRate = (int)((double)NPC.spawnRate * 0.5);
                NPC.maxSpawns *= 2;
            }
            if (Main.player[k].ZoneHallow && (double)Main.player[k].position.Y > Main.rockLayer * 16.0 + (double)NPC.sHeight)
            {
                NPC.spawnRate = (int)((double)NPC.spawnRate * 0.65);
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.3f);
            }
            if (Main.wofNPCIndex >= 0 && Main.player[k].position.Y > (float)(Main.UnderworldLayer * 16))
            {
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 0.3f);
                NPC.spawnRate *= 3;
            }
            if ((double)Main.player[k].nearbyActiveNPCs < (double)NPC.maxSpawns * 0.2)
            {
                NPC.spawnRate = (int)((float)NPC.spawnRate * 0.6f);
            }
            else if ((double)Main.player[k].nearbyActiveNPCs < (double)NPC.maxSpawns * 0.4)
            {
                NPC.spawnRate = (int)((float)NPC.spawnRate * 0.7f);
            }
            else if ((double)Main.player[k].nearbyActiveNPCs < (double)NPC.maxSpawns * 0.6)
            {
                NPC.spawnRate = (int)((float)NPC.spawnRate * 0.8f);
            }
            else if ((double)Main.player[k].nearbyActiveNPCs < (double)NPC.maxSpawns * 0.8)
            {
                NPC.spawnRate = (int)((float)NPC.spawnRate * 0.9f);
            }
            if ((double)(Main.player[k].position.Y / 16f) > (Main.worldSurface + Main.rockLayer) / 2.0 || Main.player[k].ZoneCorrupt || Main.player[k].ZoneCrimson)
            {
                if ((double)Main.player[k].nearbyActiveNPCs < (double)NPC.maxSpawns * 0.2)
                {
                    NPC.spawnRate = (int)((float)NPC.spawnRate * 0.7f);
                }
                else if ((double)Main.player[k].nearbyActiveNPCs < (double)NPC.maxSpawns * 0.4)
                {
                    NPC.spawnRate = (int)((float)NPC.spawnRate * 0.9f);
                }
            }
            int maxValue = 65;
            if (Main.remixWorld && (double)(Main.player[k].position.Y / 16f) < Main.worldSurface && (Main.player[k].ZoneCorrupt || Main.player[k].ZoneCrimson))
            {
                maxValue = 25;
                NPC.spawnRate = (int)((double)NPC.spawnRate * 0.8);
                NPC.maxSpawns *= 2;
            }
            if (Main.player[k].invis)
            {
                NPC.spawnRate = (int)((float)NPC.spawnRate * 1.2f);
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 0.8f);
            }
            if (Main.player[k].calmed)
            {
                NPC.spawnRate = (int)((float)NPC.spawnRate * 1.65f);
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 0.6f);
            }
            if (Main.player[k].sunflower)
            {
                NPC.spawnRate = (int)((float)NPC.spawnRate * 1.2f);
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 0.8f);
            }
            if (Main.player[k].anglerSetSpawnReduction)
            {
                NPC.spawnRate = (int)((float)NPC.spawnRate * 1.3f);
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 0.7f);
            }
            if (Main.player[k].enemySpawns)
            {
                NPC.spawnRate = (int)((double)NPC.spawnRate * 0.5);
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 2f);
            }
            if (Main.player[k].ZoneWaterCandle || Main.player[k].inventory[Main.player[k].selectedItem].type == 148)
            {
                if (!Main.player[k].ZonePeaceCandle && Main.player[k].inventory[Main.player[k].selectedItem].type != 3117)
                {
                    NPC.spawnRate = (int)((double)NPC.spawnRate * 0.75);
                    NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.5f);
                }
            }
            else if (Main.player[k].ZonePeaceCandle || Main.player[k].inventory[Main.player[k].selectedItem].type == 3117)
            {
                NPC.spawnRate = (int)((double)NPC.spawnRate * 1.3);
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 0.7f);
            }
            if (Main.player[k].ZoneShadowCandle || Main.player[k].inventory[Main.player[k].selectedItem].type == 5322)
            {
                Main.player[k].townNPCs = 0f;
            }
            if (Main.player[k].ZoneWaterCandle && (double)(Main.player[k].position.Y / 16f) < Main.worldSurface * 0.3499999940395355)
            {
                NPC.spawnRate = (int)((double)NPC.spawnRate * 0.5);
            }
            if (Main.player[k].isNearFairy())
            {
                NPC.spawnRate = (int)((float)NPC.spawnRate * 1.2f);
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 0.8f);
            }
            if ((double)NPC.spawnRate < (double)NPC.defaultSpawnRate * 0.1)
            {
                NPC.spawnRate = (int)((double)NPC.defaultSpawnRate * 0.1);
            }
            if (NPC.maxSpawns > NPC.defaultMaxSpawns * 3)
            {
                NPC.maxSpawns = NPC.defaultMaxSpawns * 3;
            }
            if (Main.getGoodWorld)
            {
                NPC.spawnRate = (int)((float)NPC.spawnRate * 0.8f);
                NPC.maxSpawns = (int)((float)NPC.maxSpawns * 1.2f);
            }
            if (Main.GameModeInfo.IsJourneyMode)
            {
                CreativePowers.SpawnRateSliderPerPlayerPower power = CreativePowerManager.Instance.GetPower<CreativePowers.SpawnRateSliderPerPlayerPower>();
                if (power != null && power.GetIsUnlocked())
                {
                    if (power.GetShouldDisableSpawnsFor(k))
                    {
                        continue;
                    }
                    if (power.GetRemappedSliderValueFor(k, out var num10))
                    {
                        NPC.spawnRate = (int)((float)NPC.spawnRate / num10);
                        NPC.maxSpawns = (int)((float)NPC.maxSpawns * num10);
                    }
                }
            }
            if ((Main.pumpkinMoon || Main.snowMoon) && (Main.remixWorld || (double)Main.player[k].position.Y < Main.worldSurface * 16.0))
            {
                NPC.maxSpawns = (int)((double)NPC.defaultMaxSpawns * (2.0 + 0.3 * (double)num4));
                NPC.spawnRate = 20;
            }
            if (DD2Event.Ongoing && Main.player[k].ZoneOldOneArmy)
            {
                NPC.maxSpawns = NPC.defaultMaxSpawns;
                NPC.spawnRate = NPC.defaultSpawnRate;
            }
            if (flag6)
            {
                NPC.maxSpawns = (int)((double)NPC.defaultMaxSpawns * (2.0 + 0.3 * (double)num4));
                NPC.spawnRate = 20;
            }
            if (Main.player[k].ZoneDungeon && !NPC.downedBoss3)
            {
                NPC.spawnRate = 10;
            }
            if (!flag6 && ((!Main.bloodMoon && !Main.pumpkinMoon && !Main.snowMoon) || Main.dayTime) && (!Main.eclipse || !Main.dayTime) && !Main.player[k].ZoneDungeon && !Main.player[k].ZoneCorrupt && !Main.player[k].ZoneCrimson && !Main.player[k].ZoneMeteor && !Main.player[k].ZoneOldOneArmy)
            {
                if (Main.player[k].Center.Y / 16f > (float)Main.UnderworldLayer && (!Main.remixWorld || !((double)(Main.player[k].Center.X / 16f) > (double)Main.maxTilesX * 0.39 + 50.0) || !((double)(Main.player[k].Center.X / 16f) < (double)Main.maxTilesX * 0.61)))
                {
                    if (Main.player[k].townNPCs == 1f)
                    {
                        if (Main.rand.Next(2) == 0)
                        {
                            flag5 = true;
                        }
                        if (Main.rand.Next(10) == 0)
                        {
                            flag12 = true;
                            NPC.maxSpawns = (int)((double)(float)NPC.maxSpawns * 0.5);
                        }
                        else
                        {
                            NPC.spawnRate = (int)((double)(float)NPC.spawnRate * 1.25);
                        }
                    }
                    else if (Main.player[k].townNPCs == 2f)
                    {
                        if (Main.rand.Next(4) != 0)
                        {
                            flag5 = true;
                        }
                        if (Main.rand.Next(5) == 0)
                        {
                            flag12 = true;
                            NPC.maxSpawns = (int)((double)(float)NPC.maxSpawns * 0.5);
                        }
                        else
                        {
                            NPC.spawnRate = (int)((double)(float)NPC.spawnRate * 1.5);
                        }
                    }
                    else if (Main.player[k].townNPCs >= 3f)
                    {
                        if (Main.rand.Next(10) != 0)
                        {
                            flag5 = true;
                        }
                        if (Main.rand.Next(3) == 0)
                        {
                            flag12 = true;
                            NPC.maxSpawns = (int)((double)(float)NPC.maxSpawns * 0.5);
                        }
                        else
                        {
                            NPC.spawnRate = (int)((float)NPC.spawnRate * 2f);
                        }
                    }
                }
                else if (Main.player[k].townNPCs == 1f)
                {
                    flag5 = true;
                    if (Main.player[k].ZoneGraveyard)
                    {
                        NPC.spawnRate = (int)((double)(float)NPC.spawnRate * 1.66);
                        if (Main.rand.Next(9) == 1)
                        {
                            flag12 = true;
                            NPC.maxSpawns = (int)((double)(float)NPC.maxSpawns * 0.6);
                        }
                    }
                    else if (Main.rand.Next(3) == 1)
                    {
                        flag12 = true;
                        NPC.maxSpawns = (int)((double)(float)NPC.maxSpawns * 0.6);
                    }
                    else
                    {
                        NPC.spawnRate = (int)((float)NPC.spawnRate * 2f);
                    }
                }
                else if (Main.player[k].townNPCs == 2f)
                {
                    flag5 = true;
                    if (Main.player[k].ZoneGraveyard)
                    {
                        NPC.spawnRate = (int)((double)(float)NPC.spawnRate * 2.33);
                        if (Main.rand.Next(6) == 1)
                        {
                            flag12 = true;
                            NPC.maxSpawns = (int)((double)(float)NPC.maxSpawns * 0.6);
                        }
                    }
                    else if (Main.rand.Next(3) != 0)
                    {
                        flag12 = true;
                        NPC.maxSpawns = (int)((double)(float)NPC.maxSpawns * 0.6);
                    }
                    else
                    {
                        NPC.spawnRate = (int)((float)NPC.spawnRate * 3f);
                    }
                }
                else if (Main.player[k].townNPCs >= 3f)
                {
                    flag5 = true;
                    if (Main.player[k].ZoneGraveyard)
                    {
                        NPC.spawnRate = (int)((float)NPC.spawnRate * 3f);
                        if (Main.rand.Next(3) == 1)
                        {
                            flag12 = true;
                            NPC.maxSpawns = (int)((double)(float)NPC.maxSpawns * 0.6);
                        }
                    }
                    else
                    {
                        if (!Main.expertMode || Main.rand.Next(30) != 0)
                        {
                            flag12 = true;
                        }
                        NPC.maxSpawns = (int)((double)(float)NPC.maxSpawns * 0.6);
                    }
                }
            }
            bool flag15 = false;
            if (Main.player[k].active && !Main.player[k].dead && Main.player[k].nearbyActiveNPCs < (float)NPC.maxSpawns && Main.rand.Next(NPC.spawnRate) == 0)
            {
                bool flag16 = Main.player[k].ZoneTowerNebula || Main.player[k].ZoneTowerSolar || Main.player[k].ZoneTowerStardust || Main.player[k].ZoneTowerVortex;
                NPC.spawnRangeX = (int)((double)(NPC.sWidth / 16) * 0.7);
                NPC.spawnRangeY = (int)((double)(NPC.sHeight / 16) * 0.7);
                NPC.safeRangeX = (int)((double)(NPC.sWidth / 16) * 0.52);
                NPC.safeRangeY = (int)((double)(NPC.sHeight / 16) * 0.52);
                if (Main.player[k].inventory[Main.player[k].selectedItem].type == 1254 || Main.player[k].inventory[Main.player[k].selectedItem].type == 1299 || Main.player[k].scope)
                {
                    float num11 = 1.5f;
                    if (Main.player[k].inventory[Main.player[k].selectedItem].type == 1254 && Main.player[k].scope)
                    {
                        num11 = 1.25f;
                    }
                    else if (Main.player[k].inventory[Main.player[k].selectedItem].type == 1254)
                    {
                        num11 = 1.5f;
                    }
                    else if (Main.player[k].inventory[Main.player[k].selectedItem].type == 1299)
                    {
                        num11 = 1.5f;
                    }
                    else if (Main.player[k].scope)
                    {
                        num11 = 2f;
                    }
                    NPC.spawnRangeX += (int)((double)(NPC.sWidth / 16) * 0.5 / (double)num11);
                    NPC.spawnRangeY += (int)((double)(NPC.sHeight / 16) * 0.5 / (double)num11);
                    NPC.safeRangeX += (int)((double)(NPC.sWidth / 16) * 0.5 / (double)num11);
                    NPC.safeRangeY += (int)((double)(NPC.sHeight / 16) * 0.5 / (double)num11);
                }
                int num12 = (int)(Main.player[k].position.X / 16f) - NPC.spawnRangeX;
                int num13 = (int)(Main.player[k].position.X / 16f) + NPC.spawnRangeX;
                int num14 = (int)(Main.player[k].position.Y / 16f) - NPC.spawnRangeY;
                int num15 = (int)(Main.player[k].position.Y / 16f) + NPC.spawnRangeY;
                int num16 = (int)(Main.player[k].position.X / 16f) - NPC.safeRangeX;
                int num17 = (int)(Main.player[k].position.X / 16f) + NPC.safeRangeX;
                int num18 = (int)(Main.player[k].position.Y / 16f) - NPC.safeRangeY;
                int num19 = (int)(Main.player[k].position.Y / 16f) + NPC.safeRangeY;
                if (num12 < 0)
                {
                    num12 = 0;
                }
                if (num13 > Main.maxTilesX)
                {
                    num13 = Main.maxTilesX;
                }
                if (num14 < 0)
                {
                    num14 = 0;
                }
                if (num15 > Main.maxTilesY)
                {
                    num15 = Main.maxTilesY;
                }
                for (int m = 0; m < 50; m++)
                {
                    int num20 = Main.rand.Next(num12, num13);
                    int num21 = Main.rand.Next(num14, num15);
                    if (!Main.tile[num20, num21].nactive() || !Main.tileSolid[Main.tile[num20, num21].type])
                    {
                        if (!flag16 && Main.wallHouse[Main.tile[num20, num21].wall])
                        {
                            continue;
                        }
                        if (!flag6 && (double)num21 < Main.worldSurface * 0.3499999940395355 && !flag12 && ((double)num20 < (double)Main.maxTilesX * 0.45 || (double)num20 > (double)Main.maxTilesX * 0.55 || Main.hardMode))
                        {
                            num3 = Main.tile[num20, num21].type;
                            num = num20;
                            num2 = num21;
                            flag2 = true;
                            flag3 = true;
                        }
                        else if (!flag6 && (double)num21 < Main.worldSurface * 0.44999998807907104 && !flag12 && Main.hardMode && Main.rand.Next(10) == 0)
                        {
                            num3 = Main.tile[num20, num21].type;
                            num = num20;
                            num2 = num21;
                            flag2 = true;
                            flag3 = true;
                        }
                        else
                        {
                            for (int n = num21; n < Main.maxTilesY && n < num15; n++)
                            {
                                if (Main.tile[num20, n].nactive() && Main.tileSolid[Main.tile[num20, n].type])
                                {
                                    if (num20 < num16 || num20 > num17 || n < num18 || n > num19)
                                    {
                                        num3 = Main.tile[num20, n].type;
                                        num = num20;
                                        num2 = n;
                                        flag2 = true;
                                    }
                                    break;
                                }
                            }
                        }
                        if (Main.player[k].ZoneShadowCandle)
                        {
                            flag5 = false;
                        }
                        else if (!flag3 && Main.player[k].afkCounter >= NPC.AFKTimeNeededForNoWorms)
                        {
                            flag5 = true;
                        }
                        if (flag2)
                        {
                            int num22 = num - NPC.spawnSpaceX / 2;
                            int num23 = num + NPC.spawnSpaceX / 2;
                            int num24 = num2 - NPC.spawnSpaceY;
                            int num25 = num2;
                            if (num22 < 0)
                            {
                                flag2 = false;
                            }
                            if (num23 > Main.maxTilesX)
                            {
                                flag2 = false;
                            }
                            if (num24 < 0)
                            {
                                flag2 = false;
                            }
                            if (num25 > Main.maxTilesY)
                            {
                                flag2 = false;
                            }
                            if (flag2)
                            {
                                for (int num26 = num22; num26 < num23; num26++)
                                {
                                    for (int num27 = num24; num27 < num25; num27++)
                                    {
                                        if (Main.tile[num26, num27].nactive() && Main.tileSolid[Main.tile[num26, num27].type])
                                        {
                                            flag2 = false;
                                            break;
                                        }
                                        if (Main.tile[num26, num27].lava())
                                        {
                                            flag2 = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (num >= num16 && num <= num17)
                            {
                                flag15 = true;
                            }
                        }
                    }
                    if (flag2 || flag2)
                    {
                        break;
                    }
                }
            }
            if (flag2)
            {
                Rectangle rectangle = new(num * 16, num2 * 16, 16, 16);
                for (int num28 = 0; num28 < 255; num28++)
                {
                    if (Main.player[num28].active)
                    {
                        Rectangle rectangle2 = new((int)(Main.player[num28].position.X + (float)(Main.player[num28].width / 2) - (float)(NPC.sWidth / 2) - (float)NPC.safeRangeX), (int)(Main.player[num28].position.Y + (float)(Main.player[num28].height / 2) - (float)(NPC.sHeight / 2) - (float)NPC.safeRangeY), NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                        if (rectangle.Intersects(rectangle2))
                        {
                            flag2 = false;
                        }
                    }
                }
            }
            if (flag2)
            {
                if (Main.player[k].ZoneDungeon && (!Main.tileDungeon[Main.tile[num, num2].type] || Main.tile[num, num2 - 1].wall == 0))
                {
                    flag2 = false;
                }
                if (Main.tile[num, num2 - 1].liquid > 0 && Main.tile[num, num2 - 2].liquid > 0 && !Main.tile[num, num2 - 1].lava())
                {
                    if (Main.tile[num, num2 - 1].shimmer())
                    {
                        flag2 = false;
                    }
                    if (Main.tile[num, num2 - 1].honey())
                    {
                        flag8 = true;
                    }
                    else
                    {
                        flag7 = true;
                    }
                }
                int num29 = (int)Main.player[k].Center.X / 16;
                int num30 = (int)(Main.player[k].Bottom.Y + 8f) / 16;
                if (Main.tile[num, num2].type == 367)
                {
                    flag10 = true;
                }
                else if (Main.tile[num, num2].type == 368)
                {
                    flag9 = true;
                }
                else if (Main.tile[num29, num30].type == 367)
                {
                    flag10 = true;
                }
                else if (Main.tile[num29, num30].type == 368)
                {
                    flag9 = true;
                }
                else
                {
                    int num31 = Main.rand.Next(20, 31);
                    int num32 = Main.rand.Next(1, 4);
                    if (num - num31 < 0)
                    {
                        num31 = num;
                    }
                    if (num2 - num31 < 0)
                    {
                        num31 = num2;
                    }
                    if (num + num31 >= Main.maxTilesX)
                    {
                        num31 = Main.maxTilesX - num - 1;
                    }
                    if (num2 + num31 >= Main.maxTilesY)
                    {
                        num31 = Main.maxTilesY - num2 - 1;
                    }
                    for (int num33 = num - num31; num33 <= num + num31; num33 += num32)
                    {
                        int num34 = Main.rand.Next(1, 4);
                        for (int num35 = num2 - num31; num35 <= num2 + num31; num35 += num34)
                        {
                            if (Main.tile[num33, num35].type == 367)
                            {
                                flag10 = true;
                            }
                            if (Main.tile[num33, num35].type == 368)
                            {
                                flag9 = true;
                            }
                        }
                    }
                    num31 = Main.rand.Next(30, 61);
                    num32 = Main.rand.Next(3, 7);
                    if (num29 - num31 < 0)
                    {
                        num31 = num29;
                    }
                    if (num30 - num31 < 0)
                    {
                        num31 = num30;
                    }
                    if (num29 + num31 >= Main.maxTilesX)
                    {
                        num31 = Main.maxTilesX - num29 - 2;
                    }
                    if (num30 + num31 >= Main.maxTilesY)
                    {
                        num31 = Main.maxTilesY - num30 - 2;
                    }
                    for (int num36 = num29 - num31; num36 <= num29 + num31; num36 += num32)
                    {
                        int num37 = Main.rand.Next(3, 7);
                        for (int num38 = num30 - num31; num38 <= num30 + num31; num38 += num37)
                        {
                            if (Main.tile[num36, num38].type == 367)
                            {
                                flag10 = true;
                            }
                            if (Main.tile[num36, num38].type == 368)
                            {
                                flag9 = true;
                            }
                        }
                    }
                }
                if (flag8)
                {
                    flag2 = false;
                }
                if ((num3 == 477 || num3 == 492) && !Main.bloodMoon && !Main.eclipse && Main.invasionType <= 0 && !Main.pumpkinMoon && !Main.snowMoon && !Main.slimeRain && Main.rand.Next(100) < 10)
                {
                    flag2 = false;
                }
            }
            if (!flag2)
            {
                continue;
            }
            if (Main.remixWorld)
            {
                NPC.ResetRemixHax();
            }
            bool flag17 = (double)num2 <= Main.rockLayer;
            if (Main.remixWorld)
            {
                flag17 = (double)num2 > Main.rockLayer && num2 <= Main.maxTilesY - 190;
            }
            bool flag18 = (double)num2 > Main.rockLayer && num2 < Main.UnderworldLayer;
            if (Main.dontStarveWorld)
            {
                flag18 = num2 < Main.UnderworldLayer;
            }
            if (flag18 && !Main.player[k].ZoneDungeon && !flag6)
            {
                if (Main.rand.Next(3) == 0)
                {
                    int num39 = Main.rand.Next(5, 15);
                    if (num - num39 >= 0 && num + num39 < Main.maxTilesX)
                    {
                        for (int num40 = num - num39; num40 < num + num39; num40++)
                        {
                            for (int num41 = num2 - num39; num41 < num2 + num39; num41++)
                            {
                                if (Main.tile[num40, num41].wall == 62)
                                {
                                    flag11 = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    int x = (int)Main.player[k].position.X / 16;
                    int y = (int)Main.player[k].position.Y / 16;
                    if (Main.tile[x, y].wall == 62)
                    {
                        flag11 = true;
                    }
                }
            }
            if ((double)num2 < Main.rockLayer && num2 > 200 && !Main.player[k].ZoneDungeon && !flag6)
            {
                if (Main.rand.Next(3) == 0)
                {
                    int num42 = Main.rand.Next(5, 15);
                    if (num - num42 >= 0 && num + num42 < Main.maxTilesX)
                    {
                        for (int num43 = num - num42; num43 < num + num42; num43++)
                        {
                            for (int num44 = num2 - num42; num44 < num2 + num42; num44++)
                            {
                                if (WallID.Sets.AllowsUndergroundDesertEnemiesToSpawn[Main.tile[num43, num44].wall])
                                {
                                    flag13 = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    int x2 = (int)Main.player[k].position.X / 16;
                    int y2 = (int)Main.player[k].position.Y / 16;
                    if (WallID.Sets.AllowsUndergroundDesertEnemiesToSpawn[Main.tile[x2, y2].wall])
                    {
                        flag13 = true;
                    }
                }
            }

            int num45 = Main.tile[num, num2].type;
            int num46 = Main.tile[num, num2 - 1].wall;
            if (Main.tile[num, num2 - 2].wall == 244 || Main.tile[num, num2].wall == 244)
            {
                num46 = 244;
            }
            bool flag19 = (float)new Point(num8 - num, num9 - num2).X * Main.windSpeedTarget > 0f;
            bool flag20 = (double)num2 <= Main.worldSurface;
            bool flag21 = (double)num2 >= Main.rockLayer;
            bool flag22 = ((num < WorldGen.oceanDistance || num > Main.maxTilesX - WorldGen.oceanDistance) && Main.tileSand[num45] && (double)num2 < Main.rockLayer) || (num3 == 53 && WorldGen.oceanDepths(num, num2));
            bool flag23 = (double)num2 <= Main.worldSurface && (num < WorldGen.beachDistance || num > Main.maxTilesX - WorldGen.beachDistance);
            bool flag24 = Main.cloudAlpha > 0f;
            int range = 10;
            if (Main.remixWorld)
            {
                flag24 = Main.raining;
                flag21 =(double)num2 > Main.worldSurface && (double)num2 < Main.rockLayer;
                if ((double)num2 < Main.worldSurface + 5.0)
                {
                    Main.raining = false;
                    Main.cloudAlpha = 0f;
                    Main.dayTime = false;
                }
                range = 5;
                if (Main.player[k].ZoneCorrupt || Main.player[k].ZoneCrimson)
                {
                    flag22 = false;
                    flag23 = false;
                }
                if ((double)num < (double)Main.maxTilesX * 0.43 || (double)num > (double)Main.maxTilesX * 0.57)
                {
                    if ((double)num2 > Main.rockLayer - 200.0 && num2 < Main.maxTilesY - 200 && Main.rand.Next(2) == 0)
                    {
                        flag22 = true;
                    }
                    if ((double)num2 > Main.rockLayer - 200.0 && num2 < Main.maxTilesY - 200 && Main.rand.Next(2) == 0)
                    {
                        flag23 = true;
                    }
                }
                if ((double)num2 > Main.rockLayer - 20.0)
                {
                    if (num2 <= Main.maxTilesY - 190 && Main.rand.Next(3) != 0)
                    {
                        flag20 = true;
                        Main.dayTime = false;
                        if (Main.rand.Next(2) == 0)
                        {
                            Main.dayTime = true;
                        }
                    }
                    else if ((Main.bloodMoon || (Main.eclipse && Main.dayTime)) && (double)num > (double)Main.maxTilesX * 0.38 + 50.0 && (double)num < (double)Main.maxTilesX * 0.62)
                    {
                        flag20 = true;
                    }
                }
            }
            num45 = NPC.SpawnNPC_TryFindingProperGroundTileType(num45, num, num2);
            int newNPC = 200;
            int cattailX;
            int cattailY;
            if (Main.player[k].ZoneTowerNebula)
            {
                bool flag25 = true;
                int num47 = 0;
                while (flag25)
                {
                    num47 = Utils.SelectRandom<int>(Main.rand, 424, 424, 424, 423, 423, 423, 421, 421, 421, 420, 420);
                    flag25 = false;
                    if (num47 == 424 && NPC.CountNPCS(num47) >= 3)
                    {
                        flag25 = true;
                    }
                    if (num47 == 423 && NPC.CountNPCS(num47) >= 3)
                    {
                        flag25 = true;
                    }
                    if (num47 == 420 && NPC.CountNPCS(num47) >= 3)
                    {
                        flag25 = true;
                    }
                }
                if (num47 != 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, num47, 1);
                }
            }
            else if (Main.player[k].ZoneTowerVortex)
            {
                bool flag26 = true;
                int num48 = 0;
                while (flag26)
                {
                    num48 = Utils.SelectRandom<int>(Main.rand, 429, 429, 429, 429, 427, 427, 425, 425, 426);
                    flag26 = false;
                    if (num48 == 425 && NPC.CountNPCS(num48) >= 3)
                    {
                        flag26 = true;
                    }
                    if (num48 == 426 && NPC.CountNPCS(num48) >= 3)
                    {
                        flag26 = true;
                    }
                    if (num48 == 429 && NPC.CountNPCS(num48) >= 4)
                    {
                        flag26 = true;
                    }
                }
                if (num48 != 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, num48, 1);
                }
            }
            else if (Main.player[k].ZoneTowerStardust)
            {
                int num49 = Utils.SelectRandom<int>(Main.rand, 411, 411, 411, 409, 409, 407, 402, 405);
                if (num49 != 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, num49, 1);
                }
            }
            else if (Main.player[k].ZoneTowerSolar)
            {
                bool flag27 = true;
                int num50 = 0;
                while (flag27)
                {
                    num50 = Utils.SelectRandom<int>(Main.rand, 518, 419, 418, 412, 417, 416, 415);
                    flag27 = false;
                    if (num50 == 418 && Main.rand.Next(2) == 0)
                    {
                        num50 = Utils.SelectRandom<int>(Main.rand, 415, 416, 419, 417);
                    }
                    if (num50 == 518 && NPC.CountNPCS(num50) >= 2)
                    {
                        flag27 = true;
                    }
                    if (num50 == 412 && NPC.CountNPCS(num50) >= 1)
                    {
                        flag27 = true;
                    }
                }
                if (num50 != 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, num50, 1);
                }
            }
            else if (flag3)
            {
                int maxValue2 = 8;
                int maxValue3 = 30;
                bool flag28 = (float)Math.Abs(num - Main.maxTilesX / 2) / (float)(Main.maxTilesX / 2) > 0.33f && (Main.wallLight[Main.tile[num8, num9].wall] || Main.tile[num8, num9].wall == 73);
                if (flag28 && NPC.AnyDanger())
                {
                    flag28 = false;
                }
                if (Main.player[k].ZoneWaterCandle)
                {
                    maxValue2 = 3;
                    maxValue3 = 10;
                }
                if (flag6 && Main.invasionType == 4)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 388);
                }
                else if (flag28 && Main.hardMode && NPC.downedGolemBoss && ((!NPC.downedMartians && Main.rand.Next(maxValue2) == 0) || Main.rand.Next(maxValue3) == 0) && !NPC.AnyNPCs(399))
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 399);
                }
                else if (flag28 && Main.hardMode && NPC.downedGolemBoss && ((!NPC.downedMartians && Main.rand.Next(maxValue2) == 0) || Main.rand.Next(maxValue3) == 0) && !NPC.AnyNPCs(399) && (Main.player[k].inventory[Main.player[k].selectedItem].type == 148 || Main.player[k].ZoneWaterCandle))
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 399);
                }
                else if (Main.hardMode && !NPC.AnyNPCs(87) && !flag5 && Main.rand.Next(10) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 87, 1);
                }
                else if (Main.hardMode && !NPC.AnyNPCs(87) && !flag5 && Main.rand.Next(10) == 0 && (Main.player[k].inventory[Main.player[k].selectedItem].type == 148 || Main.player[k].ZoneWaterCandle))
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 87, 1);
                }
                else if (!NPC.unlockedSlimePurpleSpawn && Main.player[k].RollLuck(25) == 0 && !NPC.AnyNPCs(686))
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 686);
                }
                else
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 48);
                }
            }
            else if (flag6)
            {
                if (Main.invasionType == 1)
                {
                    if (Main.hardMode && !NPC.AnyNPCs(471) && Main.rand.Next(30) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 471);
                    }
                    else if (Main.rand.Next(9) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 29);
                    }
                    else if (Main.rand.Next(5) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 26);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 111);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 27);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 28);
                    }
                }
                else if (Main.invasionType == 2)
                {
                    if (Main.rand.Next(7) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 145);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 143);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 144);
                    }
                }
                else if (Main.invasionType == 3)
                {
                    if (Main.invasionSize < Main.invasionSizeStart / 2 && Main.rand.Next(20) == 0 && !NPC.AnyNPCs(491) && !Collision.SolidTiles(num - 20, num + 20, num2 - 40, num2 - 10))
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, (num2 - 10) * 16, 491);
                    }
                    else if (Main.rand.Next(30) == 0 && !NPC.AnyNPCs(216))
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 216);
                    }
                    else if (Main.rand.Next(11) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 215);
                    }
                    else if (Main.rand.Next(9) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 252);
                    }
                    else if (Main.rand.Next(7) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 214);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 213);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 212);
                    }
                }
                else if (Main.invasionType == 4)
                {
                    int num51 = 0;
                    int num52 = Main.rand.Next(7);
                    bool flag29 = (float)(Main.invasionSizeStart - Main.invasionSize) / (float)Main.invasionSizeStart >= 0.3f && !NPC.AnyNPCs(395);
                    if (Main.rand.Next(45) == 0 && flag29)
                    {
                        num51 = 395;
                    }
                    else if (num52 >= 6)
                    {
                        if (Main.rand.Next(20) == 0 && flag29)
                        {
                            num51 = 395;
                        }
                        else
                        {
                            int num53 = Main.rand.Next(2);
                            if (num53 == 0)
                            {
                                num51 = 390;
                            }
                            if (num53 == 1)
                            {
                                num51 = 386;
                            }
                        }
                    }
                    else if (num52 >= 4)
                    {
                        int num54 = Main.rand.Next(5);
                        num51 = ((num54 < 2) ? 382 : ((num54 >= 4) ? 388 : 381));
                    }
                    else
                    {
                        int num55 = Main.rand.Next(4);
                        if (num55 == 3)
                        {
                            if (!NPC.AnyNPCs(520))
                            {
                                num51 = 520;
                            }
                            else
                            {
                                num55 = Main.rand.Next(3);
                            }
                        }
                        if (num55 == 0)
                        {
                            num51 = 385;
                        }
                        if (num55 == 1)
                        {
                            num51 = 389;
                        }
                        if (num55 == 2)
                        {
                            num51 = 383;
                        }
                    }
                    if (num51 != 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, num51, 1);
                    }
                }
            }
            else if (num46 == 244 && !Main.remixWorld)
            {
                if (flag7)
                {
                    if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 592);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 55);
                    }
                }
                else if ((double)num2 > Main.worldSurface)
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 447);
                        }
                        else
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 300);
                        }
                    }
                    else if (Main.rand.Next(2) == 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 359);
                    }
                    else if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 448);
                    }
                    else if (Main.rand.Next(3) != 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 357);
                    }
                }
                else if (Main.player[k].RollLuck(2) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 624);
                    Main.npc[newNPC].timeLeft *= 10;
                }
                else if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 443);
                }
                else if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 539);
                }
                else if (Main.halloween && Main.rand.Next(3) != 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 303);
                }
                else if (Main.xMas && Main.rand.Next(3) != 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 337);
                }
                else if (BirthdayParty.PartyIsUp && Main.rand.Next(3) != 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 540);
                }
                else if (Main.rand.Next(3) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Utils.SelectRandom(Main.rand, new short[2] { 299, 538 }));
                }
                else
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 46);
                }
            }
            else if (!NPC.savedBartender && DD2Event.ReadyToFindBartender && !NPC.AnyNPCs(579) && Main.rand.Next(80) == 0 && !flag7)
            {
                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 579);
            }
            else if (Main.tile[num, num2].wall == 62 || flag11)
            {
                bool flag30 = flag21 && num2 < Main.maxTilesY - 210;
                if (Main.dontStarveWorld)
                {
                    flag30 = num2 < Main.maxTilesY - 210;
                }
                if (Main.tile[num, num2].wall == 62 && Main.rand.Next(8) == 0 && !flag7 && flag30 && !NPC.savedStylist && !NPC.AnyNPCs(354))
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 354);
                }
                else if (Main.hardMode && Main.rand.Next(10) != 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 163);
                }
                else
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 164);
                }
            }
            else if ((NPC.SpawnTileOrAboveHasAnyWallInSet(num, num2, WallID.Sets.AllowsUndergroundDesertEnemiesToSpawn) || flag13) && WorldGen.checkUnderground(num, num2))
            {
                float num56 = 1.15f;
                if ((double)num2 > (Main.rockLayer * 2.0 + (double)Main.maxTilesY) / 3.0)
                {
                    num56 *= 0.5f;
                }
                else if ((double)num2 > Main.rockLayer)
                {
                    num56 *= 0.85f;
                }
                if (Main.rand.Next(20) == 0 && !flag7 && !NPC.savedGolfer && !NPC.AnyNPCs(589))
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 589);
                }
                else if (Main.hardMode && Main.rand.Next((int)(45f * num56)) == 0 && !flag5 && (double)num2 > Main.worldSurface + 100.0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 510);
                }
                else if (Main.rand.Next((int)(45f * num56)) == 0 && !flag5 && (double)num2 > Main.worldSurface + 100.0 && NPC.CountNPCS(513) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 513);
                }
                else if (Main.hardMode && Main.rand.Next(5) != 0)
                {
                    List<int> list = new();
                    if (Main.player[k].ZoneCorrupt)
                    {
                        list.Add(525);
                        list.Add(525);
                    }
                    if (Main.player[k].ZoneCrimson)
                    {
                        list.Add(526);
                        list.Add(526);
                    }
                    if (Main.player[k].ZoneHallow)
                    {
                        list.Add(527);
                        list.Add(527);
                    }
                    if (list.Count == 0)
                    {
                        list.Add(524);
                        list.Add(524);
                    }
                    if (Main.player[k].ZoneCorrupt || Main.player[k].ZoneCrimson)
                    {
                        list.Add(533);
                        list.Add(529);
                    }
                    else
                    {
                        list.Add(530);
                        list.Add(528);
                    }
                    list.Add(532);
                    int num57 = Utils.SelectRandom(Main.rand, list.ToArray());
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, num57);
                    list.Clear();
                }
                else
                {
                    int num58 = Utils.SelectRandom<int>(Main.rand, 69, 580, 580, 580, 581);
                    if (Main.rand.Next(15) == 0)
                    {
                        num58 = 537;
                    }
                    else if (Main.rand.Next(10) == 0)
                    {
                        switch (num58)
                        {
                            case 580:
                                num58 = 508;
                                break;
                            case 581:
                                num58 = 509;
                                break;
                        }
                    }
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, num58);
                }
            }
            else if (Main.hardMode && flag7 && Main.player[k].ZoneJungle && Main.rand.Next(3) != 0)
            {
                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 157);
            }
            else if (Main.hardMode && flag7 && Main.player[k].ZoneCrimson && Main.rand.Next(3) != 0)
            {
                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 242);
            }
            else if (Main.hardMode && flag7 && Main.player[k].ZoneCrimson && Main.rand.Next(3) != 0)
            {
                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 241);
            }
            else if ((!flag12 || (!NPC.savedAngler && !NPC.AnyNPCs(376))) && flag7 && flag22)
            {
                bool flag31 = false;
                if (!NPC.savedAngler && !NPC.AnyNPCs(376) && ((double)num2 < Main.worldSurface - 10.0 || Main.remixWorld))
                {
                    int num59 = -1;
                    for (int num60 = num2 - 1; num60 > num2 - 50; num60--)
                    {
                        if (Main.tile[num, num60].liquid == 0 && !WorldGen.SolidTile(num, num60) && !WorldGen.SolidTile(num, num60 + 1) && !WorldGen.SolidTile(num, num60 + 2))
                        {
                            num59 = num60 + 2;
                            break;
                        }
                    }
                    if (num59 > num2)
                    {
                        num59 = num2;
                    }
                    if (num59 > 0 && !flag15)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num59 * 16, 376);
                        flag31 = true;
                    }
                }
                if (!flag31 && !flag15)
                {
                    int num61 = -1;
                    int num62 = -1;
                    if (((double)num2 < Main.worldSurface || Main.remixWorld) && num2 > 50)
                    {
                        for (int num63 = num2 - 1; num63 > num2 - 50; num63--)
                        {
                            if (Main.tile[num, num63].liquid == 0 && !WorldGen.SolidTile(num, num63) && !WorldGen.SolidTile(num, num63 + 1) && !WorldGen.SolidTile(num, num63 + 2))
                            {
                                num61 = num63 + 2;
                                if (!WorldGen.SolidTile(num, num61 + 1) && !WorldGen.SolidTile(num, num61 + 2) && !Main.wallHouse[Main.tile[num, num61 + 2].wall])
                                {
                                    num62 = num61 + 2;
                                }
                                if (Main.wallHouse[Main.tile[num, num61].wall])
                                {
                                    num61 = -1;
                                }
                                break;
                            }
                        }
                        if (num61 > num2)
                        {
                            num61 = num2;
                        }
                        if (num62 > num2)
                        {
                            num62 = num2;
                        }
                    }
                    if (num61 > 0 && !flag15 && Main.rand.Next(10) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num61 * 16, 602);
                    }
                    else if (Main.rand.Next(10) == 0)
                    {
                        int num64 = Main.rand.Next(3);
                        if (num64 == 0 && num61 > 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num61 * 16, 625);
                        }
                        else if (num64 == 1 && num62 > 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num62 * 16, 615);
                        }
                        else if (num64 == 2)
                        {
                            int num65 = num2;
                            if (num62 > 0)
                            {
                                num65 = num62;
                            }
                            if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num65 * 16, 627);
                            }
                            else
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num65 * 16, 626);
                            }
                        }
                    }
                    else if (Main.rand.Next(40) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 220);
                    }
                    else if (Main.rand.Next(18) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 221);
                    }
                    else if (Main.rand.Next(8) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 65);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 67);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 64);
                    }
                }
            }
            else if (!flag7 && !NPC.savedAngler && !NPC.AnyNPCs(376) && (num < WorldGen.beachDistance || num > Main.maxTilesX - WorldGen.beachDistance) && Main.tileSand[num45] && ((double)num2 < Main.worldSurface || Main.remixWorld))
            {
                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 376);
            }
            else if (!flag12 && flag7 && ((flag21 && Main.rand.Next(2) == 0) || num45 == 60))
            {
                bool flag32 = false;
                if (num45 == 60 && flag20 && num2 > 50 && Main.rand.Next(3) == 0 && Main.dayTime)
                {
                    int num66 = -1;
                    for (int num67 = num2 - 1; num67 > num2 - 50; num67--)
                    {
                        if (Main.tile[num, num67].liquid == 0 && !WorldGen.SolidTile(num, num67) && !WorldGen.SolidTile(num, num67 + 1) && !WorldGen.SolidTile(num, num67 + 2))
                        {
                            num66 = num67 + 2;
                            break;
                        }
                    }
                    if (num66 > num2)
                    {
                        num66 = num2;
                    }
                    if (num66 > 0 && !flag15)
                    {
                        flag32 = true;
                        if (Main.rand.Next(4) == 0)
                        {
                            flag32 = true;
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num66 * 16, 617);
                        }
                        else if (!flag && Main.cloudAlpha == 0f)
                        {
                            flag32 = true;
                            int num68 = Main.rand.Next(1, 4);
                            for (int num69 = 0; num69 < num68; num69++)
                            {
                                if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 + Main.rand.Next(-16, 17), num66 * 16 - 16, 613);
                                }
                                else
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 + Main.rand.Next(-16, 17), num66 * 16 - 16, 612);
                                }
                            }
                        }
                    }
                }
                if (!flag32)
                {
                    if (Main.hardMode && Main.rand.Next(3) > 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 102);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 58);
                    }
                }
            }
            else if (!flag12 && flag7 && (double)num2 > Main.worldSurface && Main.rand.Next(3) == 0)
            {
                if (Main.hardMode && Main.rand.Next(3) > 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 103);
                }
                else
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 63);
                }
            }
            else if (flag7 && Main.rand.Next(4) == 0 && ((num > WorldGen.oceanDistance && num < Main.maxTilesX - WorldGen.oceanDistance) || (double)num2 > Main.worldSurface + 50.0))
            {
                if (Main.player[k].ZoneCorrupt)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 57);
                }
                else if (Main.player[k].ZoneCrimson)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 465);
                }
                else if ((double)num2 < Main.worldSurface && num2 > 50 && Main.rand.Next(3) != 0 && Main.dayTime)
                {
                    int num70 = -1;
                    for (int num71 = num2 - 1; num71 > num2 - 50; num71--)
                    {
                        if (Main.tile[num, num71].liquid == 0 && !WorldGen.SolidTile(num, num71) && !WorldGen.SolidTile(num, num71 + 1) && !WorldGen.SolidTile(num, num71 + 2))
                        {
                            num70 = num71 + 2;
                            break;
                        }
                    }
                    if (num70 > num2)
                    {
                        num70 = num2;
                    }
                    if (num70 > 0 && !flag15)
                    {
                        if (Main.rand.Next(5) == 0 && (num3 == 2 || num3 == 477))
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num70 * 16, 616);
                        }
                        else if (num3 == 53)
                        {
                            if (Main.rand.Next(2) == 0 && !flag && Main.cloudAlpha == 0f)
                            {
                                int num72 = Main.rand.Next(1, 4);
                                for (int num73 = 0; num73 < num72; num73++)
                                {
                                    if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 + Main.rand.Next(-16, 17), num70 * 16 - 16, 613);
                                    }
                                    else
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 + Main.rand.Next(-16, 17), num70 * 16 - 16, 612);
                                    }
                                }
                            }
                            else
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num70 * 16, 608);
                            }
                        }
                        else if (Main.rand.Next(2) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num70 * 16, 362);
                        }
                        else
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num70 * 16, 364);
                        }
                    }
                    else if (num3 == 53 && num > WorldGen.beachDistance && num < Main.maxTilesX - WorldGen.beachDistance)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num70 * 16, 607);
                    }
                    else if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 592);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 55);
                    }
                }
                else if (num3 == 53 && num > WorldGen.beachDistance && num < Main.maxTilesX - WorldGen.beachDistance)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 607);
                }
                else if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 592);
                }
                else
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 55);
                }
            }
            else if (NPC.downedGoblins && Main.player[k].RollLuck(20) == 0 && !flag7 && flag21 && num2 < Main.maxTilesY - 210 && !NPC.savedGoblin && !NPC.AnyNPCs(105))
            {
                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 105);
            }
            else if (Main.hardMode && Main.player[k].RollLuck(20) == 0 && !flag7 && flag21 && num2 < Main.maxTilesY - 210 && !NPC.savedWizard && !NPC.AnyNPCs(106))
            {
                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 106);
            }
            //else if (NPC.downedBoss3 && Main.player[k].RollLuck(20) == 0 && !flag7 && flag21 && num2 < Main.maxTilesY - 210 && !NPC.unlockedSlimeOldSpawn && !NPC.AnyNPCs(685))
            else if (NPC.downedBoss3 && Main.player[k].RollLuck(20) == 0 && !flag7 && flag21 && num2 < Main.maxTilesY - 210 && !NPC.AnyNPCs(685))
            {
                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 685);
            }
            else if (flag12)
            {
                if (Main.player[k].ZoneGraveyard)
                {
                    if (!flag7)
                    {
                        if (Main.rand.Next(2) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 606);
                        }
                        else
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 610);
                        }
                    }
                }
                else if (!flag15 && flag23)
                {
                    if (flag7)
                    {
                        int num74 = -1;
                        int num75 = -1;
                        if (((double)num2 < Main.worldSurface || Main.remixWorld) && num2 > 50)
                        {
                            for (int num76 = num2 - 1; num76 > num2 - 50; num76--)
                            {
                                if (Main.tile[num, num76].liquid == 0 && !WorldGen.SolidTile(num, num76) && !WorldGen.SolidTile(num, num76 + 1) && !WorldGen.SolidTile(num, num76 + 2))
                                {
                                    num74 = num76 + 2;
                                    if (!WorldGen.SolidTile(num, num74 + 1) && !WorldGen.SolidTile(num, num74 + 2))
                                    {
                                        num75 = num74 + 2;
                                    }
                                    break;
                                }
                            }
                            if (num74 > num2)
                            {
                                num74 = num2;
                            }
                            if (num75 > num2)
                            {
                                num75 = num2;
                            }
                        }
                        if (Main.rand.Next(2) == 0)
                        {
                            int num77 = Main.rand.Next(3);
                            if (num77 == 0 && num74 > 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num74 * 16, 625);
                            }
                            else if (num77 == 1 && num75 > 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num75 * 16, 615);
                            }
                            else if (num77 == 2)
                            {
                                int num78 = num2;
                                if (num75 > 0)
                                {
                                    num78 = num75;
                                }
                                if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num78 * 16, 627);
                                }
                                else
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num78 * 16, 626);
                                }
                            }
                        }
                        else if (num74 > 0 && !flag15)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num74 * 16, 602);
                        }
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 602);
                    }
                }
                else if ((num45 == 2 || num45 == 477 || num45 == 53) && !tooWindyForButterflies && Main.raining && Main.dayTime && Main.rand.Next(2) == 0 && ((double)num2 <= Main.worldSurface || Main.remixWorld) && NPC.FindCattailTop(num, num2, out cattailX, out cattailY))
                {
                    if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), cattailX * 16 + 8, cattailY * 16, 601);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), cattailX * 16 + 8, cattailY * 16, NPC.RollDragonflyType(num45));
                    }
                    if (Main.rand.Next(3) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), cattailX * 16 + 8 - 16, cattailY * 16, NPC.RollDragonflyType(num45));
                    }
                    if (Main.rand.Next(3) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), cattailX * 16 + 8 + 16, cattailY * 16, NPC.RollDragonflyType(num45));
                    }
                }
                else if (flag7)
                {
                    if (flag20 && num2 > 50 && Main.rand.Next(3) != 0 && Main.dayTime)
                    {
                        int num79 = -1;
                        for (int num80 = num2 - 1; num80 > num2 - 50; num80--)
                        {
                            if (Main.tile[num, num80].liquid == 0 && !WorldGen.SolidTile(num, num80) && !WorldGen.SolidTile(num, num80 + 1) && !WorldGen.SolidTile(num, num80 + 2))
                            {
                                num79 = num80 + 2;
                                break;
                            }
                        }
                        if (num79 > num2)
                        {
                            num79 = num2;
                        }
                        if (num79 > 0 && !flag15)
                        {
                            switch (num3)
                            {
                                case 60:
                                    if (Main.rand.Next(3) != 0 && !flag && Main.cloudAlpha == 0f)
                                    {
                                        int num83 = Main.rand.Next(1, 4);
                                        for (int num84 = 0; num84 < num83; num84++)
                                        {
                                            if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                                            {
                                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 + Main.rand.Next(-16, 17), num79 * 16 - 16, 613);
                                            }
                                            else
                                            {
                                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 + Main.rand.Next(-16, 17), num79 * 16 - 16, 612);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num79 * 16, 617);
                                    }
                                    break;
                                case 53:
                                    if (Main.rand.Next(3) != 0 && !flag && Main.cloudAlpha == 0f)
                                    {
                                        int num81 = Main.rand.Next(1, 4);
                                        for (int num82 = 0; num82 < num81; num82++)
                                        {
                                            if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                                            {
                                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 + Main.rand.Next(-16, 17), num79 * 16 - 16, 613);
                                            }
                                            else
                                            {
                                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 + Main.rand.Next(-16, 17), num79 * 16 - 16, 612);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num79 * 16, 608);
                                    }
                                    break;
                                default:
                                    if (Main.rand.Next(5) == 0 && (num3 == 2 || num3 == 477))
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num79 * 16, 616);
                                    }
                                    else if (Main.rand.Next(2) == 0)
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num79 * 16, 362);
                                    }
                                    else
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num79 * 16, 364);
                                    }
                                    break;
                            }
                        }
                        else if (num3 == 53 && num > WorldGen.beachDistance && num < Main.maxTilesX - WorldGen.beachDistance)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 607);
                        }
                        else if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 592);
                        }
                        else
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 55);
                        }
                    }
                    else if (num3 == 53 && num > WorldGen.beachDistance && num < Main.maxTilesX - WorldGen.beachDistance)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 607);
                    }
                    else if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 592);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 55);
                    }
                }
                else if (num45 == 147 || num45 == 161)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 148);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 149);
                    }
                }
                else if (num45 == 60)
                {
                    if (Main.dayTime && Main.rand.Next(3) != 0)
                    {
                        switch (Main.rand.Next(5))
                        {
                            case 0:
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 671);
                                break;
                            case 1:
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 672);
                                break;
                            case 2:
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 673);
                                break;
                            case 3:
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 674);
                                break;
                            default:
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 675);
                                break;
                        }
                    }
                    else
                    {
                        NPC.SpawnNPC_SpawnFrog(num, num2, k);
                    }
                }
                else if (num45 == 53)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(366, 368));
                }
                else
                {
                    if (num45 != 2 && num45 != 477 && num45 != 109 && num45 != 492 && !((double)num2 > Main.worldSurface))
                    {
                        break;
                    }
                    bool flag33 = flag20;
                    if (Main.raining && num2 <= Main.UnderworldLayer)
                    {
                        if (flag21 && Main.rand.Next(5) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, NPC.SpawnNPC_GetGemSquirrelToSpawn());
                        }
                        else if (flag21 && Main.rand.Next(5) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, NPC.SpawnNPC_GetGemBunnyToSpawn());
                        }
                        else if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 448);
                        }
                        else if (Main.rand.Next(3) != 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 357);
                        }
                        else if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 593);
                        }
                        else
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 230);
                        }
                    }
                    else if (!Main.dayTime && Main.numClouds <= 55 && Main.cloudBGActive == 0f && Star.starfallBoost > 3f && flag33 && Main.player[k].RollLuck(2) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 484);
                    }
                    else if (!tooWindyForButterflies && !Main.dayTime && Main.rand.Next(NPC.fireFlyFriendly) == 0 && flag33)
                    {
                        int num85 = 355;
                        if (num45 == 109)
                        {
                            num85 = 358;
                        }
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, num85);
                        if (Main.rand.Next(NPC.fireFlyMultiple) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 - 16, num2 * 16, num85);
                        }
                        if (Main.rand.Next(NPC.fireFlyMultiple) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 + 16, num2 * 16, num85);
                        }
                        if (Main.rand.Next(NPC.fireFlyMultiple) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16 - 16, num85);
                        }
                        if (Main.rand.Next(NPC.fireFlyMultiple) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16 + 16, num85);
                        }
                    }
                    else if (Main.cloudAlpha == 0f && !Main.dayTime && Main.rand.Next(5) == 0 && flag33)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 611);
                    }
                    else if (Main.dayTime && Main.time < 18000.0 && Main.rand.Next(3) != 0 && flag33)
                    {
                        int num86 = Main.rand.Next(4);
                        if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 442);
                        }
                        else
                        {
                            switch (num86)
                            {
                                case 0:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 297);
                                    break;
                                case 1:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 298);
                                    break;
                                default:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 74);
                                    break;
                            }
                        }
                    }
                    else if (!tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(NPC.stinkBugChance) == 0 && flag33)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 669);
                        if (Main.rand.Next(4) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 - 16, num2 * 16, 669);
                        }
                        if (Main.rand.Next(4) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 + 16, num2 * 16, 669);
                        }
                    }
                    else if (!tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(NPC.butterflyChance) == 0 && flag33)
                    {
                        if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 444);
                        }
                        else
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 356);
                        }
                        if (Main.rand.Next(4) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 - 16, num2 * 16, 356);
                        }
                        if (Main.rand.Next(4) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 + 16, num2 * 16, 356);
                        }
                    }
                    else if (tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(NPC.butterflyChance / 2) == 0 && flag33)
                    {
                        if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 605);
                        }
                        else
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 604);
                        }
                        if (Main.rand.Next(3) != 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 604);
                        }
                        if (Main.rand.Next(2) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 604);
                        }
                        if (Main.rand.Next(3) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 604);
                        }
                        if (Main.rand.Next(4) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 604);
                        }
                    }
                    else if (Main.rand.Next(2) == 0 && flag33)
                    {
                        int num87 = Main.rand.Next(4);
                        if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 442);
                        }
                        else
                        {
                            switch (num87)
                            {
                                case 0:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 297);
                                    break;
                                case 1:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 298);
                                    break;
                                default:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 74);
                                    break;
                            }
                        }
                    }
                    else if (num2 > Main.UnderworldLayer)
                    {
                        if (Main.remixWorld && (double)(Main.player[k].Center.X / 16f) > (double)Main.maxTilesX * 0.39 + 50.0 && (double)(Main.player[k].Center.X / 16f) < (double)Main.maxTilesX * 0.61 && Main.rand.Next(2) == 0)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, NPC.SpawnNPC_GetGemSquirrelToSpawn());
                            }
                            else
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, NPC.SpawnNPC_GetGemBunnyToSpawn());
                            }
                        }
                        else
                        {
                            newNPC = NPC.SpawnNPC_SpawnLavaBaitCritters(num, num2);
                        }
                    }
                    else if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 443);
                    }
                    else if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0 && flag33)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 539);
                    }
                    else if (Main.halloween && Main.rand.Next(3) != 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 303);
                    }
                    else if (Main.xMas && Main.rand.Next(3) != 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 337);
                    }
                    else if (BirthdayParty.PartyIsUp && Main.rand.Next(3) != 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 540);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        if (Main.remixWorld)
                        {
                            if ((double)num2 < Main.rockLayer && (double)num2 > Main.worldSurface)
                            {
                                if (Main.rand.Next(5) == 0)
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, NPC.SpawnNPC_GetGemSquirrelToSpawn());
                                }
                            }
                            else if (flag33)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Utils.SelectRandom(Main.rand, new short[2] { 299, 538 }));
                            }
                        }
                        else if ((double)num2 >= Main.rockLayer && num2 <= Main.UnderworldLayer)
                        {
                            if (Main.rand.Next(5) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, NPC.SpawnNPC_GetGemSquirrelToSpawn());
                            }
                        }
                        else if (flag33)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Utils.SelectRandom(Main.rand, new short[2] { 299, 538 }));
                        }
                    }
                    else if (Main.remixWorld)
                    {
                        if ((double)num2 < Main.rockLayer && (double)num2 > Main.worldSurface)
                        {
                            if ((double)num2 >= Main.rockLayer && num2 <= Main.UnderworldLayer)
                            {
                                if (Main.rand.Next(5) == 0)
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, NPC.SpawnNPC_GetGemBunnyToSpawn());
                                }
                            }
                            else
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 46);
                            }
                        }
                    }
                    else if ((double)num2 >= Main.rockLayer && num2 <= Main.UnderworldLayer)
                    {
                        if (Main.rand.Next(5) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, NPC.SpawnNPC_GetGemBunnyToSpawn());
                        }
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 46);
                    }
                }
            }
            else if (Main.player[k].ZoneDungeon)
            {
                int num88 = 0;
                ushort wall = Main.tile[num, num2].wall;
                ushort wall2 = Main.tile[num, num2 - 1].wall;
                if (wall == 94 || wall == 96 || wall == 98 || wall2 == 94 || wall2 == 96 || wall2 == 98)
                {
                    num88 = 1;
                }
                if (wall == 95 || wall == 97 || wall == 99 || wall2 == 95 || wall2 == 97 || wall2 == 99)
                {
                    num88 = 2;
                }
                if (Main.player[k].RollLuck(7) == 0)
                {
                    num88 = Main.rand.Next(3);
                }
                bool flag34 = !NPC.downedBoss3;
                if (Main.drunkWorld && Main.player[k].position.Y / 16f < (float)(Main.dungeonY + 40))
                {
                    flag34 = false;
                }
                if (flag34)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 68);
                }
                else if (NPC.downedBoss3 && !NPC.savedMech && Main.rand.Next(5) == 0 && !flag7 && !NPC.AnyNPCs(123) && (double)num2 > (Main.worldSurface * 4.0 + Main.rockLayer) / 5.0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 123);
                }
                else if (flag14 && Main.rand.Next(30) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 287);
                }
                else if (flag14 && num88 == 0 && Main.rand.Next(15) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 293);
                }
                else if (flag14 && num88 == 1 && Main.rand.Next(15) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 291);
                }
                else if (flag14 && num88 == 2 && Main.rand.Next(15) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 292);
                }
                else if (flag14 && !NPC.AnyNPCs(290) && num88 == 0 && Main.rand.Next(35) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 290);
                }
                else if (flag14 && (num88 == 1 || num88 == 2) && Main.rand.Next(30) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 289);
                }
                else if (flag14 && Main.rand.Next(20) == 0)
                {
                    int num89 = 281;
                    if (num88 == 0)
                    {
                        num89 += 2;
                    }
                    if (num88 == 2)
                    {
                        num89 += 4;
                    }
                    num89 += Main.rand.Next(2);
                    if (!NPC.AnyNPCs(num89))
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, num89);
                    }
                }
                else if (flag14 && Main.rand.Next(3) != 0)
                {
                    int num90 = 269;
                    if (num88 == 0)
                    {
                        num90 += 4;
                    }
                    if (num88 == 2)
                    {
                        num90 += 8;
                    }
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, num90 + Main.rand.Next(4));
                }
                else if (Main.player[k].RollLuck(35) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 71);
                }
                else if (num88 == 1 && Main.rand.Next(3) == 0 && !NPC.NearSpikeBall(num, num2))
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 70);
                }
                else if (num88 == 2 && Main.rand.Next(5) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 72);
                }
                else if (num88 == 0 && Main.rand.Next(7) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 34);
                }
                else if (Main.rand.Next(7) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 32);
                }
                else
                {
                    switch (Main.rand.Next(5))
                    {
                        case 0:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 294);
                            break;
                        case 1:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 295);
                            break;
                        case 2:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 296);
                            break;
                        default:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 31);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-14);
                            }
                            else if (Main.rand.Next(5) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-13);
                            }
                            break;
                    }
                }
            }
            else if (Main.player[k].ZoneMeteor)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 23);
            }
            else if (DD2Event.Ongoing && Main.player[k].ZoneOldOneArmy)
            {
                DD2Event.SpawnNPC(ref newNPC);
            }
            else if ((Main.remixWorld || (double)num2 <= Main.worldSurface) && !Main.dayTime && Main.snowMoon)
            {
                int num91 = NPC.waveNumber;
                if (Main.rand.Next(30) == 0 && NPC.CountNPCS(341) < 4)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 341);
                }
                else if (num91 >= 20)
                {
                    int num92 = Main.rand.Next(3);
                    if (!(num5 >= (float)num4 * num6))
                    {
                        newNPC = num92 switch
                        {
                            0 => NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 345),
                            1 => NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 346),
                            _ => NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 344),
                        };
                    }
                }
                else if (num91 >= 19)
                {
                    newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(345) < 4) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 345) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(346) < 5) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 346) : ((Main.rand.Next(10) != 0 || NPC.CountNPCS(344) >= 7) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 343) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 344))));
                }
                else if (num91 >= 18)
                {
                    newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(345) < 3) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 345) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(346) < 4) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 346) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(344) < 6) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 344) : ((Main.rand.Next(3) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 348) : ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 343) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 351))))));
                }
                else if (num91 >= 17)
                {
                    newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(345) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 345) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(346) < 3) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 346) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(344) < 5) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 344) : ((Main.rand.Next(4) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 347) : ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 343) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 351))))));
                }
                else if (num91 >= 16)
                {
                    newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(345) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 345) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(346) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 346) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(344) < 4) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 344) : ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 343) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 352)))));
                }
                else if (num91 >= 15)
                {
                    newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(345)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 345) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(346) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 346) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(344) < 3) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 344) : ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 343) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 347)))));
                }
                else
                {
                    switch (num91)
                    {
                        case 14:
                            if (Main.rand.Next(10) == 0 && !NPC.AnyNPCs(345))
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 345);
                            }
                            else if (Main.rand.Next(10) == 0 && !NPC.AnyNPCs(346))
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 346);
                            }
                            else if (Main.rand.Next(10) == 0 && !NPC.AnyNPCs(344))
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 344);
                            }
                            else if (Main.rand.Next(3) == 0)
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 343);
                            }
                            break;
                        case 13:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(345)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 345) : ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(346)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 346) : ((Main.rand.Next(3) != 0) ? ((Main.rand.Next(6) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 347) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 342)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 343)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 352))));
                            break;
                        case 12:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(345)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 345) : ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(344)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 344) : ((Main.rand.Next(8) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(338, 341)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 342)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 343))));
                            break;
                        case 11:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(345)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 345) : ((Main.rand.Next(6) != 0) ? ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(338, 341)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 342)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 352)));
                            break;
                        case 10:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(346)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 346) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(344) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 344) : ((Main.rand.Next(6) != 0) ? ((Main.rand.Next(3) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(338, 341)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 347)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 348)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 351))));
                            break;
                        case 9:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(346)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 346) : ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(344)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 344) : ((Main.rand.Next(2) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 342) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 347)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 348))));
                            break;
                        case 8:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(346)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 346) : ((Main.rand.Next(8) != 0) ? ((Main.rand.Next(3) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 350) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 347)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 348)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 351)));
                            break;
                        case 7:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(346)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 346) : ((Main.rand.Next(3) != 0) ? ((Main.rand.Next(4) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(338, 341)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 350)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 342)));
                            break;
                        case 6:
                            newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(344) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 344) : ((Main.rand.Next(4) != 0) ? ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 350) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 348)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 347)));
                            break;
                        case 5:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(344)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 344) : ((Main.rand.Next(4) != 0) ? ((Main.rand.Next(8) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(338, 341)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 348)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 350)));
                            break;
                        case 4:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(344)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 344) : ((Main.rand.Next(4) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(338, 341)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 342)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 350)));
                            break;
                        case 3:
                            newNPC = ((Main.rand.Next(8) != 0) ? ((Main.rand.Next(4) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(338, 341)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 342)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 350)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 348));
                            break;
                        case 2:
                            newNPC = ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(338, 341)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 350));
                            break;
                        default:
                            newNPC = ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(338, 341)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 342));
                            break;
                    }
                }
            }
            else if ((Main.remixWorld || (double)num2 <= Main.worldSurface) && !Main.dayTime && Main.pumpkinMoon)
            {
                int num93 = NPC.waveNumber;
                if (NPC.waveNumber >= 20)
                {
                    if (!(num5 >= (float)num4 * num6))
                    {
                        if (Main.rand.Next(2) == 0 && NPC.CountNPCS(327) < 2)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 327);
                        }
                        else if (Main.rand.Next(3) != 0 && NPC.CountNPCS(325) < 2)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 325);
                        }
                        else if (NPC.CountNPCS(315) < 3)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 315);
                        }
                    }
                }
                else
                {
                    switch (num93)
                    {
                        case 19:
                            if (Main.rand.Next(5) == 0 && NPC.CountNPCS(327) < 2)
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 327);
                            }
                            else if (Main.rand.Next(5) == 0 && NPC.CountNPCS(325) < 2)
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 325);
                            }
                            else if (!(num5 >= (float)num4 * num6) && NPC.CountNPCS(315) < 5)
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 315);
                            }
                            break;
                        case 18:
                            if (Main.rand.Next(7) == 0 && NPC.CountNPCS(327) < 2)
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 327);
                            }
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(325) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 325) : ((Main.rand.Next(7) != 0 || NPC.CountNPCS(315) >= 3) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 330) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 315)));
                            break;
                        case 17:
                            if (Main.rand.Next(7) == 0 && NPC.CountNPCS(327) < 2)
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 327);
                            }
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(325) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 325) : ((Main.rand.Next(7) == 0 && NPC.CountNPCS(315) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 315) : ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 329) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 330))));
                            break;
                        case 16:
                            newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(327) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 327) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(315) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 315) : ((Main.rand.Next(6) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 326) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 329)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 330))));
                            break;
                        case 15:
                            if (Main.rand.Next(10) == 0 && !NPC.AnyNPCs(327))
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 327);
                            }
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(325) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 325) : ((Main.rand.Next(5) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(305, 315)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 326)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 330)));
                            break;
                        case 14:
                            if (Main.rand.Next(10) == 0 && !NPC.AnyNPCs(327))
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 327);
                            }
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(325) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 325) : ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(315)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 315) : ((Main.rand.Next(10) != 0) ? ((Main.rand.Next(7) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(305, 315)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 326)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 329)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 330))));
                            break;
                        case 13:
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(325) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 325) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(315) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 315) : ((Main.rand.Next(6) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 326) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 329)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 330))));
                            break;
                        case 12:
                            newNPC = ((Main.rand.Next(5) != 0 || NPC.AnyNPCs(327)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 330) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 327));
                            break;
                        case 11:
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(325) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 325) : ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 326) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 330)));
                            break;
                        case 10:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(327)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 327) : ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(305, 315)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 329)));
                            break;
                        case 9:
                            newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(325) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 325) : ((Main.rand.Next(8) != 0) ? ((Main.rand.Next(5) != 0) ? ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(305, 315)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 326)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 329)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 330)));
                            break;
                        case 8:
                            newNPC = ((Main.rand.Next(8) == 0 && NPC.CountNPCS(315) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 315) : ((Main.rand.Next(4) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 329) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 330)));
                            break;
                        case 7:
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(325) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 325) : ((Main.rand.Next(4) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 329) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 330)));
                            break;
                        case 6:
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(325) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 325) : ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(305, 315)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 326)));
                            break;
                        case 5:
                            newNPC = ((Main.rand.Next(10) != 0 || NPC.AnyNPCs(315)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 329) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 315));
                            break;
                        case 4:
                            newNPC = ((Main.rand.Next(8) == 0 && !NPC.AnyNPCs(325)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 330) : ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(305, 315)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 326)));
                            break;
                        case 3:
                            newNPC = ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 326) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 329));
                            break;
                        case 2:
                            newNPC = ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(305, 315)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 326));
                            break;
                        default:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(305, 315));
                            break;
                    }
                }
            }
            else if (((double)num2 <= Main.worldSurface || (Main.remixWorld && (double)num2 > Main.rockLayer)) && Main.dayTime && Main.eclipse)
            {
                bool flag35 = false;
                if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                {
                    flag35 = true;
                }
                newNPC = ((NPC.downedPlantBoss && Main.rand.Next(80) == 0 && !NPC.AnyNPCs(477)) 
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 477) 
                    : ((Main.rand.Next(50) == 0 && !NPC.AnyNPCs(251)) 
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 251) 
                    : ((NPC.downedPlantBoss && Main.rand.Next(5) == 0 && !NPC.AnyNPCs(NPCID.Psycho)) 
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, NPCID.Psycho) 
                    : ((NPC.downedPlantBoss && Main.rand.Next(20) == 0 && !NPC.AnyNPCs(463)) 
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 463) 
                    : ((NPC.downedPlantBoss && Main.rand.Next(20) == 0 && NPC.CountNPCS(467) < 2) 
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 467) 
                    : ((Main.rand.Next(15) == 0) 
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 159) 
                    : ((flag35 && Main.rand.Next(13) == 0) 
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 253) 
                    : ((Main.rand.Next(8) == 0) 
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 469) 
                    : ((NPC.downedPlantBoss && Main.rand.Next(7) == 0) 
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 468) 
                    : ((NPC.downedPlantBoss && Main.rand.Next(5) == 0) 
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 460) 
                    : ((Main.rand.Next(4) == 0) 
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 162) 
                    : ((Main.rand.Next(3) == 0) 
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 461) 
                    : ((Main.rand.Next(2) != 0) 
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 166) 
                    : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 462))))))))))))));
            }
            else if (NPC.SpawnNPC_CheckToSpawnUndergroundFairy(num, num2, k))
            {
                int num94 = Main.rand.Next(583, 586);
                if (Main.tenthAnniversaryWorld && !Main.getGoodWorld && Main.rand.Next(4) != 0)
                {
                    num94 = 583;
                }
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, num94);
                Main.npc[newNPC].ai[2] = 2f;
                Main.npc[newNPC].TargetClosest();
                Main.npc[newNPC].ai[3] = 0f;
            }
            else if (!Main.remixWorld && !flag7 && (!Main.dayTime || Main.tile[num, num2].wall > 0) && Main.tile[num8, num9].wall == 244 && !Main.eclipse && !Main.bloodMoon && Main.player[k].RollLuck(30) == 0 && NPC.CountNPCS(624) <= Main.rand.Next(3))
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 624);
            }
            else if (!Main.player[k].ZoneCorrupt && !Main.player[k].ZoneCrimson && !flag7 && !Main.eclipse && !Main.bloodMoon && Main.player[k].RollLuck(range) == 0 && ((!Main.remixWorld && (double)num2 >= Main.worldSurface * 0.800000011920929 && (double)num2 < Main.worldSurface * 1.100000023841858) || (Main.remixWorld && (double)num2 > Main.rockLayer && num2 < Main.maxTilesY - 350)) && NPC.CountNPCS(624) <= Main.rand.Next(3) && (!Main.dayTime || Main.tile[num, num2].wall > 0) && (Main.tile[num, num2].wall == 63 || Main.tile[num, num2].wall == 2 || Main.tile[num, num2].wall == 196 || Main.tile[num, num2].wall == 197 || Main.tile[num, num2].wall == 198 || Main.tile[num, num2].wall == 199))
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 624);
            }
            else if (Main.hardMode && num3 == 70 && flag7)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 256);
            }
            else if (num3 == 70 && (double)num2 <= Main.worldSurface && Main.rand.Next(3) != 0)
            {
                if ((!Main.hardMode && Main.rand.Next(6) == 0) || Main.rand.Next(12) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 360);
                }
                else if (Main.rand.Next(3) != 0)
                {
                    newNPC = ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 255) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 254));
                }
                else if (Main.rand.Next(4) != 0)
                {
                    newNPC = ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 258) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 257));
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, (Main.hardMode && Main.rand.Next(3) != 0) ? 260 :259);
                    Main.npc[newNPC].ai[0] = num;
                    Main.npc[newNPC].ai[1] = num2;
                    Main.npc[newNPC].netUpdate = true;
                }
            }
            else if (num3 == 70 && Main.hardMode && (double)num2 >= Main.worldSurface && Main.rand.Next(3) != 0 && (!Main.remixWorld || Main.getGoodWorld || num2 < Main.maxTilesY - 360))
            {
                if (Main.hardMode && Main.player[k].RollLuck(5) == 0)
                //if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && Main.player[k].RollLuck(5) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, NPCID.TruffleWorm);
                }
                else if ((!Main.hardMode && Main.rand.Next(4) == 0) || Main.rand.Next(8) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, NPCID.GlowingSnail);
                }
                else if (Main.rand.Next(4) != 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, (Main.rand.Next(2) != 0) ? NPCID.MushiLadybug : NPCID.AnomuraFungus);
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, (Main.hardMode && Main.rand.Next(3) != 0) ? NPCID.GiantFungiBulb : NPCID.FungiBulb);
                    Main.npc[newNPC].ai[0] = num;
                    Main.npc[newNPC].ai[1] = num2;
                    Main.npc[newNPC].netUpdate = true;
                }
            }
            else if (Main.player[k].ZoneCorrupt && Main.rand.Next(maxValue) == 0 && !flag5)
            {
                newNPC = ((!Main.hardMode || Main.rand.Next(4) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 7, 1) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 98, 1));
            }
            else if (Main.remixWorld && !Main.hardMode && (double)num2 > Main.worldSurface && Main.player[k].RollLuck(100) == 0)
            {
                newNPC = ((!Main.player[k].ZoneSnow) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 85) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 629));
            }
            else if (Main.hardMode && (double)num2 > Main.worldSurface && Main.player[k].RollLuck(Main.tenthAnniversaryWorld ? 25 : 75) == 0)
            {
                newNPC = ((Main.rand.Next(2) == 0 && Main.player[k].ZoneCorrupt && !NPC.AnyNPCs(473)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 473) : ((Main.rand.Next(2) == 0 && Main.player[k].ZoneCrimson && !NPC.AnyNPCs(474)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 474) : ((Main.rand.Next(2) == 0 && Main.player[k].ZoneHallow && !NPC.AnyNPCs(475)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 475) : ((Main.tenthAnniversaryWorld && Main.rand.Next(2) == 0 && Main.player[k].ZoneJungle && !NPC.AnyNPCs(476)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 476) : ((!Main.player[k].ZoneSnow) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 85) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 629))))));
            }
            else if (Main.hardMode && Main.tile[num, num2].wall == 2 && Main.rand.Next(20) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 85);
            }
            else if (Main.hardMode && (double)num2 <= Main.worldSurface && !Main.dayTime && (Main.rand.Next(20) == 0 || (Main.rand.Next(5) == 0 && Main.moonPhase == 4)))
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 82);
            }
            else if (Main.hardMode && Main.halloween && (double)num2 <= Main.worldSurface && !Main.dayTime && Main.rand.Next(10) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 304);
            }
            else if (num45 == 60 && Main.player[k].RollLuck(500) == 0 && !Main.dayTime)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 52);
            }
            else if (num45 == 60 && (double)num2 > Main.worldSurface && Main.rand.Next(60) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 219);
            }
            else if ((double)num2 > Main.worldSurface && num2 < Main.maxTilesY - 210 && !Main.player[k].ZoneSnow && !Main.player[k].ZoneCrimson && !Main.player[k].ZoneCorrupt && !Main.player[k].ZoneJungle && !Main.player[k].ZoneHallow && Main.rand.Next(8) == 0)
            {
                if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 448);
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 357);
                }
            }
            else if ((double)num2 > Main.worldSurface && num2 < Main.maxTilesY - 210 && !Main.player[k].ZoneSnow && !Main.player[k].ZoneCrimson && !Main.player[k].ZoneCorrupt && !Main.player[k].ZoneJungle && !Main.player[k].ZoneHallow && Main.rand.Next(13) == 0)
            {
                if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 447);
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 300);
                }
            }
            else if ((double)num2 > Main.worldSurface && (double)num2 < (Main.rockLayer + (double)Main.maxTilesY) / 2.0 && !Main.player[k].ZoneSnow && !Main.player[k].ZoneCrimson && !Main.player[k].ZoneCorrupt && !Main.player[k].ZoneHallow && Main.rand.Next(13) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 359);
            }
            else if (flag20 && Main.player[k].ZoneJungle && !Main.player[k].ZoneCrimson && !Main.player[k].ZoneCorrupt && Main.rand.Next(7) == 0)
            {
                if (Main.dayTime && Main.time < 43200.00064373016 && Main.rand.Next(3) != 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 671 + Main.rand.Next(5));
                }
                else
                {
                    NPC.SpawnNPC_SpawnFrog(num, num2, k);
                }
            }
            else if (num45 == 225 && Main.rand.Next(2) == 0)
            {
                if (Main.hardMode && Main.rand.Next(4) != 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 176);
                    if (Main.rand.Next(10) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-18);
                    }
                    if (Main.rand.Next(10) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-19);
                    }
                    if (Main.rand.Next(10) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-20);
                    }
                    if (Main.rand.Next(10) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-21);
                    }
                }
                else
                {
                    switch (Main.rand.Next(8))
                    {
                        case 0:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 231);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-56);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-57);
                            }
                            break;
                        case 1:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 232);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-58);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-59);
                            }
                            break;
                        case 2:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 233);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-60);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-61);
                            }
                            break;
                        case 3:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 234);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-62);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-63);
                            }
                            break;
                        case 4:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 235);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-64);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-65);
                            }
                            break;
                        default:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 42);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-16);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-17);
                            }
                            break;
                    }
                }
            }
            else if (num45 == 60 && Main.hardMode && Main.rand.Next(3) != 0)
            {
                if (flag20 && !Main.dayTime && Main.rand.Next(3) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 152);
                }
                else if (flag20 && Main.dayTime && Main.rand.Next(4) != 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 177);
                }
                else if ((double)num2 > Main.worldSurface && Main.rand.Next(100) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 205);
                }
                else if ((double)num2 > Main.worldSurface && Main.rand.Next(5) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 236);
                }
                else if ((double)num2 > Main.worldSurface && Main.rand.Next(4) != 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 176);
                    if (Main.rand.Next(10) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-18);
                    }
                    if (Main.rand.Next(10) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-19);
                    }
                    if (Main.rand.Next(10) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-20);
                    }
                    if (Main.rand.Next(10) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-21);
                    }
                }
                else if (Main.rand.Next(3) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 175);
                    Main.npc[newNPC].ai[0] = num;
                    Main.npc[newNPC].ai[1] = num2;
                    Main.npc[newNPC].netUpdate = true;
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 153);
                }
            }
            else if (((num45 == 226 || num45 == 232) && flag4) || (Main.remixWorld && flag4))
            {
                newNPC = ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 198) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 226));
            }
            else if (num46 == 86 && Main.rand.Next(8) != 0)
            {
                switch (Main.rand.Next(8))
                {
                    case 0:
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 231);
                        if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(-56);
                        }
                        else if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(-57);
                        }
                        break;
                    case 1:
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 232);
                        if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(-58);
                        }
                        else if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(-59);
                        }
                        break;
                    case 2:
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 233);
                        if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(-60);
                        }
                        else if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(-61);
                        }
                        break;
                    case 3:
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 234);
                        if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(-62);
                        }
                        else if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(-63);
                        }
                        break;
                    case 4:
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 235);
                        if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(-64);
                        }
                        else if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(-65);
                        }
                        break;
                    default:
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 42);
                        if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(-16);
                        }
                        else if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(-17);
                        }
                        break;
                }
            }
            else if (num45 == 60 && ((!Main.remixWorld && (double)num2 > (Main.worldSurface + Main.rockLayer) / 2.0) || (Main.remixWorld && ((double)num2 < Main.rockLayer || Main.rand.Next(2) == 0))))
            {
                if (Main.rand.Next(4) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 204);
                }
                else if (Main.rand.Next(4) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 43);
                    Main.npc[newNPC].ai[0] = num;
                    Main.npc[newNPC].ai[1] = num2;
                    Main.npc[newNPC].netUpdate = true;
                }
                else
                {
                    switch (Main.rand.Next(8))
                    {
                        case 0:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 231);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-56);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-57);
                            }
                            break;
                        case 1:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 232);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-58);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-59);
                            }
                            break;
                        case 2:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 233);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-60);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-61);
                            }
                            break;
                        case 3:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 234);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-62);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-63);
                            }
                            break;
                        case 4:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 235);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-64);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-65);
                            }
                            break;
                        default:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 42);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-16);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-17);
                            }
                            break;
                    }
                }
            }
            else if (num45 == 60 && Main.rand.Next(4) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 51);
            }
            else if (num45 == 60 && Main.rand.Next(8) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 56);
                Main.npc[newNPC].ai[0] = num;
                Main.npc[newNPC].ai[1] = num2;
                Main.npc[newNPC].netUpdate = true;
            }
            else if (Sandstorm.Happening && Main.player[k].ZoneSandstorm && TileID.Sets.Conversion.Sand[num45] && NPC.Spawning_SandstoneCheck(num, num2))
            {
                if (!NPC.downedBoss1 && !Main.hardMode)
                {
                    newNPC = ((Main.rand.Next(2) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 546) : ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 69) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 61)));
                }
                else if (Main.hardMode && Main.rand.Next(20) == 0 && !NPC.AnyNPCs(541))
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 541);
                }
                else if (Main.hardMode && !flag5 && Main.rand.Next(3) == 0 && NPC.CountNPCS(510) < 4)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, (num2 + 10) * 16, 510);
                }
                else if (!Main.hardMode || flag5 || Main.rand.Next(2) != 0)
                {
                    newNPC = ((Main.hardMode && num45 == 53 && Main.rand.Next(3) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 78) : ((Main.hardMode && num45 == 112 && Main.rand.Next(3) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 79) : ((Main.hardMode && num45 == 234 && Main.rand.Next(3) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 630) : ((Main.hardMode && num45 == 116 && Main.rand.Next(3) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 80) : ((Main.rand.Next(2) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 546) : ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 581) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 580)))))));
                }
                else
                {
                    int num95 = 542;
                    if (TileID.Sets.Corrupt[num45])
                    {
                        num95 = 543;
                    }
                    if (TileID.Sets.Crimson[num45])
                    {
                        num95 = 544;
                    }
                    if (TileID.Sets.Hallow[num45])
                    {
                        num95 = 545;
                    }
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, num95);
                }
            }
            else if (Main.hardMode && num45 == 53 && Main.rand.Next(3) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 78);
            }
            else if (Main.hardMode && num45 == 112 && Main.rand.Next(2) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 79);
            }
            else if (Main.hardMode && num45 == 234 && Main.rand.Next(2) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 630);
            }
            else if (Main.hardMode && num45 == 116 && Main.rand.Next(2) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 80);
            }
            else if (Main.hardMode && !flag7 && flag17 && (num45 == 116 || num45 == 117 || num45 == 109 || num45 == 164))
            {
                if (NPC.downedPlantBoss && (Main.remixWorld || (!Main.dayTime && Main.time < 16200.0)) && flag20 && Main.player[k].RollLuck(10) == 0 && !NPC.AnyNPCs(661))
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 661);
                }
                else if (!flag24 || NPC.AnyNPCs(244) || Main.rand.Next(12) != 0)
                {
                    newNPC = ((!Main.dayTime && Main.rand.Next(2) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 122) : ((Main.rand.Next(10) != 0 && (!Main.player[k].ZoneWaterCandle || Main.rand.Next(10) != 0)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 75) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 86)));
                }
                else
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 244);
                }
            }
            else if (!flag5 && Main.hardMode && Main.rand.Next(50) == 0 && !flag7 && flag21 && (num45 == 116 || num45 == 117 || num45 == 109 || num45 == 164))
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 84);
            }
            else if ((num45 == 204 && Main.player[k].ZoneCrimson) || num45 == 199 || num45 == 200 || num45 == 203 || num45 == 234 || num45 == 662)
            {
                bool flag36 = (double)num2 >= Main.rockLayer;
                if (Main.remixWorld)
                {
                    flag36 = (double)num2 <= Main.rockLayer;
                }
                if (Main.hardMode && flag36 && Main.rand.Next(40) == 0 && !flag5)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 179);
                }
                else if (Main.hardMode && flag36 && Main.rand.Next(5) == 0 && !flag5)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 182);
                }
                else if (Main.hardMode && flag36 && Main.rand.Next(2) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 268);
                }
                else if (Main.hardMode && Main.rand.Next(3) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 183);
                    if (Main.rand.Next(3) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-24);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-25);
                    }
                }
                else if (Main.hardMode && (Main.rand.Next(2) == 0 || ((double)num2 > Main.worldSurface && !Main.remixWorld)))
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 174);
                }
                else if ((Main.tile[num, num2].wall > 0 && Main.rand.Next(4) != 0) || Main.rand.Next(8) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 239);
                }
                else if (Main.rand.Next(2) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 181);
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 173);
                    if (Main.rand.Next(3) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-22);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-23);
                    }
                }
            }
            else if ((num45 == 22 && Main.player[k].ZoneCorrupt) || num45 == 23 || num45 == 25 || num45 == 112 || num45 == 163 || num45 == 661)
            {
                bool flag37 = (double)num2 >= Main.rockLayer;
                if (Main.remixWorld)
                {
                    flag37 = (double)num2 <= Main.rockLayer;
                }
                if (Main.hardMode && flag37 && Main.rand.Next(40) == 0 && !flag5)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 83);
                }
                else if (Main.hardMode && flag37 && Main.rand.Next(3) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 101);
                    Main.npc[newNPC].ai[0] = num;
                    Main.npc[newNPC].ai[1] = num2;
                    Main.npc[newNPC].netUpdate = true;
                }
                else if (Main.hardMode && Main.rand.Next(3) == 0)
                {
                    newNPC = ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 81) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 121));
                }
                else if (Main.hardMode && (Main.rand.Next(2) == 0 || flag37))
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 94);
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 6);
                    if (Main.rand.Next(3) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-11);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-12);
                    }
                }
            }
            else if (flag20)
            {
                bool flag38 = (float)Math.Abs(num - Main.maxTilesX / 2) / (float)(Main.maxTilesX / 2) > 0.33f;
                if (flag38 && NPC.AnyDanger())
                {
                    flag38 = false;
                }
                if (Main.player[k].ZoneGraveyard && !flag7 && (num3 == 2 || num3 == 477) && Main.rand.Next(10) == 0)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 606);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 610);
                    }
                }
                else if (Main.player[k].ZoneSnow && Main.hardMode && flag24 && !NPC.AnyNPCs(243) && Main.player[k].RollLuck(20) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 243);
                }
                else if (!Main.player[k].ZoneSnow && Main.hardMode && flag24 && NPC.CountNPCS(250) < 2 && Main.rand.Next(10) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 250);
                }
                else if (flag38 && Main.hardMode && NPC.downedGolemBoss && ((!NPC.downedMartians && Main.rand.Next(100) == 0) || Main.rand.Next(400) == 0) && !NPC.AnyNPCs(399))
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 399);
                }
                else if (!Main.player[k].ZoneGraveyard && Main.dayTime)
                {
                    int num96 = Math.Abs(num - Main.spawnTileX);
                    if (!flag7 && num96 < Main.maxTilesX / 2 && Main.rand.Next(15) == 0 && (num45 == 2 || num45 == 477 || num45 == 109 || num45 == 492 || num45 == 147 || num45 == 161))
                    {
                        if (num45 == 147 || num45 == 161)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 148);
                            }
                            else
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 149);
                            }
                        }
                        else if (!tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(NPC.stinkBugChance) == 0 && flag20)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 669);
                            if (Main.rand.Next(4) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 - 16, num2 * 16, 669);
                            }
                            if (Main.rand.Next(4) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 + 16, num2 * 16, 669);
                            }
                        }
                        else if (!tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(NPC.butterflyChance) == 0 && flag20)
                        {
                            if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 444);
                            }
                            else
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 356);
                            }
                            if (Main.rand.Next(4) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 - 16, num2 * 16, 356);
                            }
                            if (Main.rand.Next(4) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 + 16, num2 * 16, 356);
                            }
                        }
                        else if (tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(NPC.butterflyChance / 2) == 0 && flag20)
                        {
                            if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 605);
                            }
                            else
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 604);
                            }
                            if (Main.rand.Next(3) != 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 604);
                            }
                            if (Main.rand.Next(2) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 604);
                            }
                            if (Main.rand.Next(3) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 604);
                            }
                            if (Main.rand.Next(4) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 604);
                            }
                        }
                        else if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 443);
                        }
                        else if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0 && (double)num2 <= Main.worldSurface)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 539);
                        }
                        else if (Main.halloween && Main.rand.Next(3) != 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 303);
                        }
                        else if (Main.xMas && Main.rand.Next(3) != 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 337);
                        }
                        else if (BirthdayParty.PartyIsUp && Main.rand.Next(3) != 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 540);
                        }
                        else if (Main.rand.Next(3) == 0 && (double)num2 <= Main.worldSurface)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Utils.SelectRandom(Main.rand, new short[2] { 299, 538 }));
                        }
                        else
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 46);
                        }
                    }
                    else if (!flag7 && num > WorldGen.beachDistance && num < Main.maxTilesX - WorldGen.beachDistance && Main.rand.Next(12) == 0 && num45 == 53)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(366, 368));
                    }
                    else if ((num45 == 2 || num45 == 477 || num45 == 53) && !tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(3) != 0 && ((double)num2 <= Main.worldSurface || Main.remixWorld) && NPC.FindCattailTop(num, num2, out cattailX, out cattailY))
                    {
                        if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), cattailX * 16 + 8, cattailY * 16, 601);
                        }
                        else
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), cattailX * 16 + 8, cattailY * 16, NPC.RollDragonflyType(num45));
                        }
                        if (Main.rand.Next(3) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), cattailX * 16 + 8 - 16, cattailY * 16, NPC.RollDragonflyType(num45));
                        }
                        if (Main.rand.Next(3) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), cattailX * 16 + 8 + 16, cattailY * 16, NPC.RollDragonflyType(num45));
                        }
                    }
                    else if (!flag7 && num96 < Main.maxTilesX / 3 && Main.dayTime && Main.time < 18000.0 && (num45 == 2 || num45 == 477 || num45 == 109 || num45 == 492) && Main.rand.Next(4) == 0 && (double)num2 <= Main.worldSurface && NPC.CountNPCS(74) + NPC.CountNPCS(297) + NPC.CountNPCS(298) < 6)
                    {
                        int num97 = Main.rand.Next(4);
                        if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 442);
                        }
                        else
                        {
                            switch (num97)
                            {
                                case 0:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 297);
                                    break;
                                case 1:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 298);
                                    break;
                                default:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 74);
                                    break;
                            }
                        }
                    }
                    else if (!flag7 && num96 < Main.maxTilesX / 3 && Main.rand.Next(15) == 0 && (num45 == 2 || num45 == 477 || num45 == 109 || num45 == 492 || num45 == 147))
                    {
                        int num98 = Main.rand.Next(4);
                        if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 442);
                        }
                        else
                        {
                            switch (num98)
                            {
                                case 0:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 297);
                                    break;
                                case 1:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 298);
                                    break;
                                default:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 74);
                                    break;
                            }
                        }
                    }
                    else if (!flag7 && num96 > Main.maxTilesX / 3 && num45 == 2 && Main.rand.Next(300) == 0 && !NPC.AnyNPCs(50))
                    {
                        NPC.SpawnOnPlayer(k, 50);
                    }
                    else if (!flag15 && num45 == 53 && (num < WorldGen.beachDistance || num > Main.maxTilesX - WorldGen.beachDistance))
                    {
                        if (!flag7 && Main.rand.Next(10) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 602);
                        }
                        else if (flag7)
                        {
                            int num99 = -1;
                            int num100 = -1;
                            if ((double)num2 < Main.worldSurface && num2 > 50)
                            {
                                for (int num101 = num2 - 1; num101 > num2 - 50; num101--)
                                {
                                    if (Main.tile[num, num101].liquid == 0 && !WorldGen.SolidTile(num, num101) && !WorldGen.SolidTile(num, num101 + 1) && !WorldGen.SolidTile(num, num101 + 2))
                                    {
                                        num99 = num101 + 2;
                                        if (!WorldGen.SolidTile(num, num99 + 1) && !WorldGen.SolidTile(num, num99 + 2))
                                        {
                                            num100 = num99 + 2;
                                        }
                                        break;
                                    }
                                }
                                if (num99 > num2)
                                {
                                    num99 = num2;
                                }
                                if (num100 > num2)
                                {
                                    num100 = num2;
                                }
                            }
                            if (Main.rand.Next(10) == 0)
                            {
                                int num102 = Main.rand.Next(3);
                                if (num102 == 0 && num99 > 0)
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num99 * 16, 625);
                                }
                                else if (num102 == 1 && num100 > 0)
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num100 * 16, 615);
                                }
                                else if (num102 == 2)
                                {
                                    int num103 = num2;
                                    if (num100 > 0)
                                    {
                                        num103 = num100;
                                    }
                                    if (Main.player[k].RollLuck(NPC.goldCritterChance) == 0)
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num103 * 16, 627);
                                    }
                                    else
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num103 * 16, 626);
                                    }
                                }
                            }
                        }
                    }
                    else if (!flag7 && num45 == 53 && Main.rand.Next(5) == 0 && NPC.Spawning_SandstoneCheck(num, num2) && !flag7)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 69);
                    }
                    else if (num45 == 53 && !flag7)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 61);
                    }
                    else if (!flag7 && (num96 > Main.maxTilesX / 3 || Main.remixWorld) && (Main.rand.Next(15) == 0 || (!NPC.downedGoblins && WorldGen.shadowOrbSmashed && Main.rand.Next(7) == 0)))
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 73);
                    }
                    else if (Main.raining && Main.rand.Next(4) == 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 224);
                    }
                    else if (!flag7 && Main.raining && Main.rand.Next(2) == 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 225);
                    }
                    else if (!flag7 && num46 == 0 && isItAHappyWindyDay && flag19 && Main.rand.Next(3) != 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 594);
                    }
                    else if (!flag7 && num46 == 0 && (num3 == 2 || num3 == 477) && isItAHappyWindyDay && flag19 && Main.rand.Next(10) != 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 628);
                    }
                    else if (!flag7)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 1);
                        switch (num45)
                        {
                            case 60:
                                Main.npc[newNPC].SetDefaults(-10);
                                break;
                            case 147:
                            case 161:
                                Main.npc[newNPC].SetDefaults(147);
                                break;
                            default:
                                if (Main.halloween && Main.rand.Next(3) != 0)
                                {
                                    Main.npc[newNPC].SetDefaults(302);
                                }
                                else if (Main.xMas && Main.rand.Next(3) != 0)
                                {
                                    Main.npc[newNPC].SetDefaults(Main.rand.Next(333, 337));
                                }
                                else if (Main.rand.Next(3) == 0 || (num96 < 200 && !Main.expertMode))
                                {
                                    Main.npc[newNPC].SetDefaults(-3);
                                }
                                else if (Main.rand.Next(10) == 0 && (num96 > 400 || Main.expertMode))
                                {
                                    Main.npc[newNPC].SetDefaults(-7);
                                }
                                break;
                        }
                    }
                }
                else
                {
                    if (!Main.player[k].ZoneGraveyard && !tooWindyForButterflies && (num45 == 2 || num45 == 477 || num45 == 109 || num45 == 492) && !Main.raining && Main.rand.Next(NPC.fireFlyChance) == 0 && (double)num2 <= Main.worldSurface)
                    {
                        int num104 = 355;
                        if (num45 == 109)
                        {
                            num104 = 358;
                        }
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, num104);
                        if (Main.rand.Next(NPC.fireFlyMultiple) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 - 16, num2 * 16, num104);
                        }
                        if (Main.rand.Next(NPC.fireFlyMultiple) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8 + 16, num2 * 16, num104);
                        }
                        if (Main.rand.Next(NPC.fireFlyMultiple) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16 - 16, num104);
                        }
                        if (Main.rand.Next(NPC.fireFlyMultiple) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16 + 16, num104);
                        }
                    }
                    else if ((Main.halloween || Main.player[k].ZoneGraveyard) && Main.rand.Next(12) == 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 301);
                    }
                    else if (Main.player[k].ZoneGraveyard && Main.rand.Next(30) == 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 316);
                    }
                    else if (Main.player[k].ZoneGraveyard && Main.hardMode && (double)num2 <= Main.worldSurface && Main.rand.Next(10) == 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 304);
                    }
                    else if (Main.rand.Next(6) == 0 || (Main.moonPhase == 4 && Main.rand.Next(2) == 0))
                    {
                        if (Main.hardMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 133);
                        }
                        else if (Main.halloween && Main.rand.Next(2) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(317, 319));
                        }
                        else if (Main.rand.Next(2) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 2);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-43);
                            }
                        }
                        else
                        {
                            switch (Main.rand.Next(5))
                            {
                                case 0:
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 190);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Main.npc[newNPC].SetDefaults(-38);
                                    }
                                    break;
                                case 1:
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 191);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Main.npc[newNPC].SetDefaults(-39);
                                    }
                                    break;
                                case 2:
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 192);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Main.npc[newNPC].SetDefaults(-40);
                                    }
                                    break;
                                case 3:
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 193);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Main.npc[newNPC].SetDefaults(-41);
                                    }
                                    break;
                                case 4:
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 194);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Main.npc[newNPC].SetDefaults(-42);
                                    }
                                    break;
                            }
                        }
                    }
                    else if (Main.hardMode && Main.rand.Next(50) == 0 && Main.bloodMoon && !NPC.AnyNPCs(109))
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 109);
                    }
                    else if (Main.rand.Next(250) == 0 && (Main.bloodMoon || Main.player[k].ZoneGraveyard))
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 53);
                    }
                    else if (Main.rand.Next(250) == 0 && (Main.bloodMoon || Main.player[k].ZoneGraveyard))
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 536);
                    }
                    else if (!Main.dayTime && Main.moonPhase == 0 && Main.hardMode && Main.rand.Next(3) != 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 104);
                    }
                    else if (!Main.dayTime && Main.hardMode && Main.rand.Next(3) == 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 140);
                    }
                    else if (Main.bloodMoon && Main.rand.Next(5) < 2)
                    {
                        newNPC = ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 490) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 489));
                    }
                    else if (num3 == 147 || num3 == 161 || num3 == 163 || num3 == 164 || num3 == 162)
                    {
                        newNPC = ((!Main.player[k].ZoneGraveyard && Main.hardMode && Main.rand.Next(4) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 169) : ((!Main.player[k].ZoneGraveyard && Main.hardMode && Main.rand.Next(3) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 155) : ((!Main.expertMode || Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 161) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 431))));
                    }
                    else if (Main.raining && Main.rand.Next(2) == 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 223);
                        if (Main.rand.Next(3) == 0)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-54);
                            }
                            else
                            {
                                Main.npc[newNPC].SetDefaults(-55);
                            }
                        }
                    }
                    else
                    {
                        int num105 = Main.rand.Next(7);
                        int num106 = 12;
                        int maxValue4 = 20;
                        if (Main.player[k].statLifeMax <= 100)
                        {
                            num106 = 5;
                            num106 -= Main.CurrentFrameFlags.ActivePlayersCount / 2;
                            if (num106 < 2)
                            {
                                num106 = 2;
                            }
                        }
                        if (Main.player[k].ZoneGraveyard && Main.rand.Next(maxValue4) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 632);
                        }
                        else if (Main.rand.Next(num106) == 0)
                        {
                            newNPC = ((!Main.expertMode || Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 590) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 591));
                        }
                        else if (Main.halloween && Main.rand.Next(2) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(319, 322));
                        }
                        else if (Main.xMas && Main.rand.Next(2) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(331, 333));
                        }
                        else if (num105 == 0 && Main.expertMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 430);
                        }
                        else if (num105 == 2 && Main.expertMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 432);
                        }
                        else if (num105 == 3 && Main.expertMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 433);
                        }
                        else if (num105 == 4 && Main.expertMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 434);
                        }
                        else if (num105 == 5 && Main.expertMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 435);
                        }
                        else if (num105 == 6 && Main.expertMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 436);
                        }
                        else
                        {
                            switch (num105)
                            {
                                case 0:
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 3);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            Main.npc[newNPC].SetDefaults(-26);
                                        }
                                        else
                                        {
                                            Main.npc[newNPC].SetDefaults(-27);
                                        }
                                    }
                                    break;
                                case 1:
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 132);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            Main.npc[newNPC].SetDefaults(-28);
                                        }
                                        else
                                        {
                                            Main.npc[newNPC].SetDefaults(-29);
                                        }
                                    }
                                    break;
                                case 2:
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 186);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            Main.npc[newNPC].SetDefaults(-30);
                                        }
                                        else
                                        {
                                            Main.npc[newNPC].SetDefaults(-31);
                                        }
                                    }
                                    break;
                                case 3:
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 187);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            Main.npc[newNPC].SetDefaults(-32);
                                        }
                                        else
                                        {
                                            Main.npc[newNPC].SetDefaults(-33);
                                        }
                                    }
                                    break;
                                case 4:
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 188);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            Main.npc[newNPC].SetDefaults(-34);
                                        }
                                        else
                                        {
                                            Main.npc[newNPC].SetDefaults(-35);
                                        }
                                    }
                                    break;
                                case 5:
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 189);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            Main.npc[newNPC].SetDefaults(-36);
                                        }
                                        else
                                        {
                                            Main.npc[newNPC].SetDefaults(-37);
                                        }
                                    }
                                    break;
                                case 6:
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 200);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            Main.npc[newNPC].SetDefaults(-44);
                                        }
                                        else
                                        {
                                            Main.npc[newNPC].SetDefaults(-45);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    if (Main.player[k].ZoneGraveyard)
                    {
                        Main.npc[newNPC].target = k;
                    }
                }
            }
            else if (flag17)
            {
                if (!flag5 && Main.rand.Next(50) == 0 && !Main.player[k].ZoneSnow)
                {
                    newNPC = ((!Main.hardMode) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 10, 1) : ((Main.rand.Next(3) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 10, 1) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 95, 1)));
                }
                else if (Main.hardMode && Main.rand.Next(3) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 140);
                }
                else if (Main.hardMode && Main.rand.Next(4) != 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 141);
                }
                else if (Main.remixWorld)
                {
                    if (num3 == 147 || num3 == 161 || num3 == 163 || num3 == 164 || num3 == 162 || Main.player[k].ZoneSnow)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 147);
                    }
                    else
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 1);
                        if (Main.rand.Next(3) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(-9);
                        }
                        else
                        {
                            Main.npc[newNPC].SetDefaults(-8);
                        }
                    }
                }
                else if (num45 == 147 || num45 == 161 || Main.player[k].ZoneSnow)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 147);
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 1);
                    if (Main.rand.Next(5) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-9);
                    }
                    else if (Main.rand.Next(2) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(1);
                    }
                    else
                    {
                        Main.npc[newNPC].SetDefaults(-8);
                    }
                }
            }
            else if (num2 > Main.maxTilesY - 190)
            {
                newNPC = ((Main.remixWorld && (double)num > (double)Main.maxTilesX * 0.38 + 50.0 && (double)num < (double)Main.maxTilesX * 0.62) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 59) : ((Main.hardMode && !NPC.savedTaxCollector && Main.rand.Next(20) == 0 && !NPC.AnyNPCs(534)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 534) : ((Main.rand.Next(8) == 0) ? NPC.SpawnNPC_SpawnLavaBaitCritters(num, num2) : ((Main.rand.Next(40) == 0 && !NPC.AnyNPCs(39)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 39, 1) : ((Main.rand.Next(14) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 24) : ((Main.rand.Next(7) == 0) ? ((Main.rand.Next(10) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 66) : ((!Main.hardMode || !NPC.downedMechBossAny || Main.rand.Next(5) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 62) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 156))) : ((Main.rand.Next(3) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 59) : ((!Main.hardMode || !NPC.downedMechBossAny || Main.rand.Next(5) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 60) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 151)))))))));
            }
            else if (NPC.SpawnNPC_CheckToSpawnRockGolem(num, num2, k, num45))
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 631);
            }
            else if (Main.rand.Next(60) == 0)
            {
                newNPC = ((!Main.player[k].ZoneSnow) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 217) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 218));
            }
            else if ((num45 == 116 || num45 == 117 || num45 == 164) && Main.hardMode && !flag5 && Main.rand.Next(8) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 120);
            }
            else if ((num3 == 147 || num3 == 161 || num3 == 162 || num3 == 163 || num3 == 164 || num3 == 200) && !flag5 && Main.hardMode && Main.player[k].ZoneCorrupt && Main.rand.Next(30) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 170);
            }
            else if ((num3 == 147 || num3 == 161 || num3 == 162 || num3 == 163 || num3 == 164 || num3 == 200) && !flag5 && Main.hardMode && Main.player[k].ZoneHallow && Main.rand.Next(30) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 171);
            }
            else if ((num3 == 147 || num3 == 161 || num3 == 162 || num3 == 163 || num3 == 164 || num3 == 200) && !flag5 && Main.hardMode && Main.player[k].ZoneCrimson && Main.rand.Next(30) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 180);
            }
            else if (Main.hardMode && Main.player[k].ZoneSnow && Main.rand.Next(10) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 154);
            }
            else if (!flag5 && Main.rand.Next(100) == 0 && !Main.player[k].ZoneHallow)
            {
                newNPC = (Main.hardMode ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 95, 1) : ((!Main.player[k].ZoneSnow) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 10, 1) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 185)));
            }
            else if (Main.player[k].ZoneSnow && Main.rand.Next(20) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 185);
            }
            else if ((!Main.hardMode && Main.rand.Next(10) == 0) || (Main.hardMode && Main.rand.Next(20) == 0))
            {
                if (Main.player[k].ZoneSnow)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 184);
                }
                else if (Main.rand.Next(3) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 1);
                    Main.npc[newNPC].SetDefaults(-6);
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 16);
                }
            }
            else if (!Main.hardMode && Main.rand.Next(4) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 1);
                if (Main.player[k].ZoneJungle)
                {
                    Main.npc[newNPC].SetDefaults(-10);
                }
                else if (Main.player[k].ZoneSnow)
                {
                    Main.npc[newNPC].SetDefaults(184);
                }
                else
                {
                    Main.npc[newNPC].SetDefaults(-6);
                }
            }
            else if (Main.rand.Next(2) != 0)
            {
                newNPC = ((Main.hardMode && (Main.player[k].ZoneHallow & (Main.rand.Next(2) == 0))) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 138) : (Main.player[k].ZoneJungle ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 51) : ((Main.player[k].ZoneGlowshroom && (num3 == 70 || num3 == 190)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 634) : ((Main.hardMode && Main.player[k].ZoneHallow) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 137) : ((Main.hardMode && Main.rand.Next(6) > 0) ? ((Main.rand.Next(3) != 0 || (num3 != 147 && num3 != 161 && num3 != 162)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 93) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 150)) : ((num3 != 147 && num3 != 161 && num3 != 162) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 49) : ((!Main.hardMode) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 150) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 169))))))));
            }
            else if (Main.rand.Next(35) == 0 && NPC.CountNPCS(453) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 453);
            }
            else if (Main.rand.Next(80) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 195);
            }
            else if (Main.hardMode && (Main.remixWorld || (double)num2 > (Main.rockLayer + (double)Main.maxTilesY) / 2.0) && Main.rand.Next(200) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 172);
            }
            else if ((Main.remixWorld || (double)num2 > (Main.rockLayer + (double)Main.maxTilesY) / 2.0) && (Main.rand.Next(200) == 0 || (Main.rand.Next(50) == 0 && (Main.player[k].armor[1].type == 4256 || (Main.player[k].armor[1].type >= 1282 && Main.player[k].armor[1].type <= 1287)) && Main.player[k].armor[0].type != 238)))
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 45);
            }
            else if (flag10 && Main.rand.Next(4) != 0)
            {
                newNPC = ((Main.rand.Next(6) == 0 || NPC.AnyNPCs(480) || !Main.hardMode) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 481) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 480));
            }
            else if (flag9 && Main.rand.Next(5) != 0)
            {
                newNPC = ((Main.rand.Next(6) == 0 || NPC.AnyNPCs(483)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 482) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 483));
            }
            else if (Main.hardMode && Main.rand.Next(10) != 0)
            {
                if (Main.rand.Next(2) != 0)
                {
                    newNPC = ((!Main.player[k].ZoneSnow) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 110) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 206));
                }
                else if (Main.player[k].ZoneSnow)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 197);
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 77);
                    if ((Main.remixWorld || (double)num2 > (Main.rockLayer + (double)Main.maxTilesY) / 2.0) && Main.rand.Next(5) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-15);
                    }
                }
            }
            else if (!flag5 && (Main.halloween || Main.player[k].ZoneGraveyard) && Main.rand.Next(30) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 316);
            }
            else if (Main.rand.Next(20) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 44);
            }
            else if (num3 == 147 || num3 == 161 || num3 == 162)
            {
                newNPC = ((Main.rand.Next(15) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 167) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 185));
            }
            else if (Main.player[k].ZoneSnow)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 185);
            }
            else if (Main.rand.Next(3) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, NPC.cavernMonsterType[Main.rand.Next(2), Main.rand.Next(3)]);
            }
            else if (Main.player[k].ZoneGlowshroom && (num3 == 70 || num3 == 190))
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 635);
            }
            else if (Main.halloween && Main.rand.Next(2) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, Main.rand.Next(322, 325));
            }
            else if (Main.expertMode && Main.rand.Next(3) == 0)
            {
                int num107 = Main.rand.Next(4);
                newNPC = ((num107 == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 449) : ((num107 == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 450) : ((num107 != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 452) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 451))));
            }
            else
            {
                switch (Main.rand.Next(4))
                {
                    case 0:
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 21);
                        if (Main.rand.Next(3) == 0)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-47);
                            }
                            else
                            {
                                Main.npc[newNPC].SetDefaults(-46);
                            }
                        }
                        break;
                    case 1:
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 201);
                        if (Main.rand.Next(3) == 0)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-49);
                            }
                            else
                            {
                                Main.npc[newNPC].SetDefaults(-48);
                            }
                        }
                        break;
                    case 2:
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 202);
                        if (Main.rand.Next(3) == 0)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-51);
                            }
                            else
                            {
                                Main.npc[newNPC].SetDefaults(-50);
                            }
                        }
                        break;
                    case 3:
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num * 16 + 8, num2 * 16, 203);
                        if (Main.rand.Next(3) == 0)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(-53);
                            }
                            else
                            {
                                Main.npc[newNPC].SetDefaults(-52);
                            }
                        }
                        break;
                }
            }
            if (Main.npc[newNPC].type == 1 && Main.player[k].RollLuck(180) == 0)
            {
                Main.npc[newNPC].SetDefaults(-4);
            }
            if (Main.tenthAnniversaryWorld && Main.npc[newNPC].type == 1 && Main.player[k].RollLuck(180) == 0)
            {
                Main.npc[newNPC].SetDefaults(667);
            }
            if (Main.netMode == 2 && newNPC < 200)
            {
                NetMessage.SendData(23, -1, -1, null, newNPC);
            }
            break;
        }
    }
    public static void TransformElderSlime(int npcIndex)
    {
        if (Main.npc.IndexInRange(npcIndex))
        {
            var npc = Main.npc[npcIndex];
            if (npc.type == 685)
            {
                if (NPC.unlockedSlimeOldSpawn)
                {
                    if (MainConfig.Instance.BoundTownSlimeOldSpawnItemIDs.Length > 0)
                    {
                        NetMessage.SendData(21, -1, -1, null, Item.NewItem(null, npc.Center, Vector2.Zero, Utils.SelectRandom(Main.rand, MainConfig.Instance.BoundTownSlimeOldSpawnItemIDs)));
                        npc.active = false;
                        NetMessage.SendData(23, -1, -1, null, npcIndex);
                    }
                }
                else
                {
                    NPC.unlockedSlimeOldSpawn = true;
                    NetMessage.SendData(7);
                    Vector2 vector = npc.velocity;
                    npc.Transform(679);
                    npc.netUpdate = true;
                    npc.velocity = vector;
                }
                ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.TownSlimeTransform, new ParticleOrchestraSettings
                {
                    PositionInWorld = npc.Center,
                    MovementVector = Vector2.Zero,
                    UniqueInfoPiece = 2
                });
            }
        }
    }
    public static void UpdateNPC(NPC npc, int i)
    {
        npc.whoAmI = i;
        if (!npc.active)
        {
            return;
        }
        if (NPC.offSetDelayTime > 0)
        {
            npc.netOffset *= 0f;
        }
        else if (Main.netMode == 2)
        {
            npc.netOffset *= 0f;
        }
        else if (Main.multiplayerNPCSmoothingRange <= 0)
        {
            npc.netOffset *= 0f;
        }
        else if (npc.netOffset != new Vector2(0f, 0f))
        {
            if (NPCID.Sets.NoMultiplayerSmoothingByType[npc.type])
            {
                npc.netOffset *= 0f;
            }
            else if (NPCID.Sets.NoMultiplayerSmoothingByAI[npc.aiStyle])
            {
                npc.netOffset *= 0f;
            }
            else
            {
                float num = 2f;
                float num2 = Main.multiplayerNPCSmoothingRange;
                float num3 = npc.netOffset.Length();
                if (num3 > num2)
                {
                    npc.netOffset.Normalize();
                    npc.netOffset *= num2;
                    num3 = npc.netOffset.Length();
                }
                num += num3 / num2 * num;
                Vector2 vector = npc.netOffset;
                vector.Normalize();
                vector *= num;
                npc.netOffset -= vector;
                if (npc.netOffset.Length() < num)
                {
                    npc.netOffset *= 0f;
                }
                if (npc.townNPC)
                {
                    if (Vector2.Distance(npc.position, new Vector2(npc.homeTileX * 16 + 8 - npc.width / 2, (float)(npc.homeTileY * 16 - npc.height) - 0.1f)) < 1f)
                    {
                        npc.netOffset *= 0f;
                    }
                    if (npc.ai[0] == 25f)
                    {
                        npc.netOffset *= 0f;
                    }
                }
            }
        }
        npc.UpdateAltTexture();
        if (npc.type == 368)
        {
            NPC.travelNPC = true;
        }
        if (Main.netMode != 2)
        {
            npc.UpdateNPC_CastLights();
        }
        npc.UpdateNPC_TeleportVisuals();
        npc.UpdateNPC_CritterSounds();
        npc.TrySyncingUniqueTownNPCData(i);
        if (npc.aiStyle == 7 && npc.position.Y > Main.bottomWorld - 640f + (float)npc.height && Main.netMode != 1 && !Main.xMas)
        {
            npc.StrikeNPCNoInteraction(9999, 0f, 0);
            if (Main.netMode == 2)
            {
                NetMessage.SendData(28, -1, -1, null, npc.whoAmI, 9999f);
            }
        }
        if (Main.netMode == 1)
        {
            bool flag = false;
            int num4 = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
            int num5 = (int)(npc.position.Y + (float)(npc.height / 2)) / 16;
            try
            {
                if (num4 >= 4 && num4 <= Main.maxTilesX - 4 && num5 >= 4 && num5 <= Main.maxTilesY - 4)
                {
                    if (Main.tile[num4, num5] == null)
                    {
                        flag = true;
                    }
                    else if (Main.tile[num4 - 3, num5] == null)
                    {
                        flag = true;
                    }
                    else if (Main.tile[num4 + 3, num5] == null)
                    {
                        flag = true;
                    }
                    else if (Main.tile[num4, num5 - 3] == null)
                    {
                        flag = true;
                    }
                    else if (Main.tile[num4, num5 + 3] == null)
                    {
                        flag = true;
                    }
                }
            }
            catch
            {
                flag = true;
            }
            if (flag)
            {
                return;
            }
        }
        npc.UpdateNPC_BuffFlagsReset();
        npc.UpdateNPC_BuffSetFlags();
        npc.UpdateNPC_SoulDrainDebuff();
        npc.UpdateNPC_BuffClearExpiredBuffs();
        npc.UpdateNPC_BuffApplyDOTs();
        npc.UpdateNPC_BuffApplyVFX();
        npc.UpdateNPC_BloodMoonTransformations();
        npc.UpdateNPC_UpdateGravity(out var maxFallSpeed);
        if (npc.soundDelay > 0)
        {
            npc.soundDelay--;
        }
        if (npc.life <= 0)
        {
            npc.active = false;
            npc.UpdateNetworkCode(i);
            npc.netUpdate = false;
            npc.justHit = false;
            return;
        }
        npc.oldTarget = npc.target;
        npc.oldDirection = npc.direction;
        npc.oldDirectionY = npc.directionY;
        float num6 = 1f + Math.Abs(npc.velocity.X) / 3f;
        if (npc.gfxOffY > 0f)
        {
            npc.gfxOffY -= num6 * npc.stepSpeed;
            if (npc.gfxOffY < 0f)
            {
                npc.gfxOffY = 0f;
            }
        }
        else if (npc.gfxOffY < 0f)
        {
            npc.gfxOffY += num6 * npc.stepSpeed;
            if (npc.gfxOffY > 0f)
            {
                npc.gfxOffY = 0f;
            }
        }
        if (npc.gfxOffY > 16f)
        {
            npc.gfxOffY = 16f;
        }
        if (npc.gfxOffY < -16f)
        {
            npc.gfxOffY = -16f;
        }
        npc.TryPortalJumping();
        npc.IdleSounds();
        npc.AI();
        npc.SubAI_HandleTemporaryCatchableNPCPlayerInvulnerability();
        if (Main.netMode != 2 && npc.extraValue > 0)
        {
            int num7 = 244;
            float num8 = 30f;
            if (npc.extraValue >= 1000000)
            {
                num7 = 247;
                num8 *= 0.25f;
            }
            else if (npc.extraValue >= 10000)
            {
                num7 = 246;
                num8 *= 0.5f;
            }
            else if (npc.extraValue >= 100)
            {
                num7 = 245;
                num8 *= 0.75f;
            }
            if (Main.rand.Next((int)num8) == 0)
            {
                npc.position += npc.netOffset;
                int num9 = Dust.NewDust(npc.position, npc.width, npc.height, num7, 0f, 0f, 254, default, 0.25f);
                Main.dust[num9].velocity *= 0.1f;
                npc.position -= npc.netOffset;
            }
        }
        for (int j = 0; j < 256; j++)
        {
            if (npc.immune[j] > 0)
            {
                npc.immune[j]--;
            }
        }
        if (!npc.noGravity && !npc.noTileCollide)
        {
            int x = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
            int y = (int)(npc.position.Y + (float)(npc.height / 2)) / 16;
            if (WorldGen.InWorld(x, y) && Main.tile[x, y] == null)
            {
                NPC.gravity = 0f;
                npc.velocity.X = 0f;
                npc.velocity.Y = 0f;
            }
        }
        if (!npc.noGravity)
        {
            npc.velocity.Y += NPC.gravity;
            if (npc.velocity.Y > maxFallSpeed)
            {
                npc.velocity.Y = maxFallSpeed;
            }
        }
        if ((double)npc.velocity.X < 0.005 && (double)npc.velocity.X > -0.005)
        {
            npc.velocity.X = 0f;
        }
        if (Main.netMode != 1 && npc.type != 37 && (npc.friendly || NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[npc.type]))
        {
            if (npc.townNPC)
            {
                npc.CheckDrowning();
            }
            npc.CheckLifeRegen();
            npc.GetHurtByOtherNPCs(NPCID.Sets.AllNPCs);
        }
        //if (Main.netMode != 1 && (NPC.npcsFoundForCheckActive[210] || NPC.npcsFoundForCheckActive[211]) && !NPCID.Sets.HurtingBees[npc.type])
        //{
        //    npc.GetHurtByOtherNPCs(NPCID.Sets.HurtingBees);
        //}
        if (!npc.noTileCollide)
        {
            npc.UpdateCollision();
        }
        else
        {
            npc.oldPosition = npc.position;
            npc.oldDirection = npc.direction;
            npc.position += npc.velocity;
            if (npc.onFire && npc.boss && Main.netMode != 1 && Collision.WetCollision(npc.position, npc.width, npc.height))
            {
                for (int k = 0; k < NPC.maxBuffs; k++)
                {
                    if (npc.buffType[k] == 24)
                    {
                        npc.DelBuff(k);
                    }
                }
            }
        }
        if (Main.netMode != 1 && !npc.noTileCollide && npc.lifeMax > 1 && Collision.SwitchTiles(npc.position, npc.width, npc.height, npc.oldPosition, 2, npc) && (npc.type == 46 || npc.type == 148 || npc.type == 149 || npc.type == 303 || npc.type == 361 || npc.type == 362 || npc.type == 364 || npc.type == 366 || npc.type == 367 || (npc.type >= 442 && npc.type <= 448) || npc.type == 602 || npc.type == 608 || npc.type == 614 || npc.type == 687))
        {
            npc.ai[0] = 1f;
            npc.ai[1] = 400f;
            npc.ai[2] = 0f;
        }
        npc.FindFrame();
        npc.UpdateNPC_UpdateTrails();
        npc.UpdateNetworkCode(i);
        npc.CheckActive();
        npc.netUpdate = false;
        npc.justHit = false;
    }


}
