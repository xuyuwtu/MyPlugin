using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Events;
using Terraria.GameContent.Drawing;
using Terraria.Localization;

using VBY.GameContentModify.Config;
using static VBY.GameContentModify.GameContentModify;

namespace VBY.GameContentModify;

[ReplaceType(typeof(NPC))]
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
        bool flag = Main.windSpeedTarget < -0.4 || Main.windSpeedTarget > 0.4;
        NPC.RevengeManager.CheckRespawns();
        int tileX = 0;
        int tileY = 0;
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
        float num6 = (int)(NPC.defaultMaxSpawns * (2f + 0.3f * num4));
        for (int k = 0; k < 255; k++)
        {
            ref var player = ref Main.player[k];
            if (!player.active || player.dead)
            {
                continue;
            }

            bool flag3 = false;
            if (player.isNearNPC(398, NPC.MoonLordFightingDistance))
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
            if (player.active && Main.invasionType > 0 && Main.invasionDelay == 0 && Main.invasionSize > 0 && (player.position.Y < Main.worldSurface * 16.0 + NPC.sHeight || Main.remixWorld))
            {
                int num7 = 3000;
                if (player.position.X > Main.invasionX * 16.0 - num7 && player.position.X < Main.invasionX * 16.0 + num7)
                {
                    flag6 = true;
                }
                else if (Main.invasionX >= Main.maxTilesX / 2 - 5 && Main.invasionX <= Main.maxTilesX / 2 + 5)
                {
                    for (int l = 0; l < 200; l++)
                    {
                        if (Main.npc[l].townNPC && Math.Abs(player.position.X - Main.npc[l].Center.X) < num7)
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
            if (player.ZoneTowerSolar || player.ZoneTowerNebula || player.ZoneTowerVortex || player.ZoneTowerStardust)
            {
                flag6 = true;
            }
            int num8 = (int)(player.position.X + player.width / 2) / 16;
            int num9 = (int)(player.position.Y + player.height / 2) / 16;
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
                NPC.spawnRate = (int)(NPC.defaultSpawnRate * 0.9);
                NPC.maxSpawns = NPC.defaultMaxSpawns + 1;
            }
            if (player.position.Y > Main.UnderworldLayer * 16)
            {
                NPC.maxSpawns = (int)(NPC.maxSpawns * 2f);
            }
            else if (player.position.Y > Main.rockLayer * 16.0 + NPC.sHeight)
            {
                if (Main.remixWorld)
                {
                    if (Main.hardMode)
                    {
                        NPC.spawnRate = (int)(NPC.spawnRate * 0.45);
                        NPC.maxSpawns = (int)(NPC.maxSpawns * 1.8f);
                    }
                    else
                    {
                        NPC.spawnRate = (int)(NPC.spawnRate * 0.5);
                        NPC.maxSpawns = (int)(NPC.maxSpawns * 1.7f);
                    }
                }
                else
                {
                    NPC.spawnRate = (int)(NPC.spawnRate * 0.4);
                    NPC.maxSpawns = (int)(NPC.maxSpawns * 1.9f);
                }
            }
            else if (player.position.Y > Main.worldSurface * 16.0 + NPC.sHeight)
            {
                if (Main.remixWorld)
                {
                    NPC.spawnRate = (int)(NPC.spawnRate * 0.4);
                    NPC.maxSpawns = (int)(NPC.maxSpawns * 1.9f);
                }
                else if (Main.hardMode)
                {
                    NPC.spawnRate = (int)(NPC.spawnRate * 0.45);
                    NPC.maxSpawns = (int)(NPC.maxSpawns * 1.8f);
                }
                else
                {
                    NPC.spawnRate = (int)(NPC.spawnRate * 0.5);
                    NPC.maxSpawns = (int)(NPC.maxSpawns * 1.7f);
                }
            }
            else if (Main.remixWorld)
            {
                if (!Main.dayTime)
                {
                    NPC.spawnRate = (int)(NPC.spawnRate * 0.6);
                    NPC.maxSpawns = (int)(NPC.maxSpawns * 1.3f);
                }
            }
            else if (!Main.dayTime)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 0.6);
                NPC.maxSpawns = (int)(NPC.maxSpawns * 1.3f);
                if (Main.bloodMoon)
                {
                    NPC.spawnRate = (int)(NPC.spawnRate * 0.3);
                    NPC.maxSpawns = (int)(NPC.maxSpawns * 1.8f);
                }
                if ((Main.pumpkinMoon || Main.snowMoon) && player.position.Y < Main.worldSurface * 16.0)
                {
                    NPC.spawnRate = (int)(NPC.spawnRate * 0.2);
                    NPC.maxSpawns *= 2;
                }
            }
            else if (Main.dayTime && Main.eclipse)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 0.2);
                NPC.maxSpawns = (int)(NPC.maxSpawns * 1.9f);
            }
            if (Main.remixWorld)
            {
                if (!Main.dayTime)
                {
                    if (Main.bloodMoon)
                    {
                        NPC.spawnRate = (int)(NPC.spawnRate * 0.3);
                        NPC.maxSpawns = (int)(NPC.maxSpawns * 1.8f);
                        if (player.position.Y > Main.rockLayer * 16.0 + NPC.sHeight)
                        {
                            NPC.spawnRate = (int)(NPC.spawnRate * 0.6);
                        }
                    }
                    if (Main.pumpkinMoon || Main.snowMoon)
                    {
                        NPC.spawnRate = (int)(NPC.spawnRate * 0.2);
                        NPC.maxSpawns *= 2;
                        if (player.position.Y > Main.rockLayer * 16.0 + NPC.sHeight)
                        {
                            NPC.spawnRate = (int)(NPC.spawnRate * 0.6);
                        }
                    }
                }
                else if (Main.dayTime && Main.eclipse)
                {
                    NPC.spawnRate = (int)(NPC.spawnRate * 0.2);
                    NPC.maxSpawns = (int)(NPC.maxSpawns * 1.9f);
                }
            }
            if (player.ZoneSnow && (double)(player.position.Y / 16f) < Main.worldSurface)
            {
                NPC.maxSpawns = (int)(NPC.maxSpawns + NPC.maxSpawns * Main.cloudAlpha);
                NPC.spawnRate = (int)(NPC.spawnRate * (1f - Main.cloudAlpha + 1f) / 2f);
            }
            if (Main.drunkWorld && Main.tile[num8, num9].wall == 86)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 0.3);
                NPC.maxSpawns = (int)(NPC.maxSpawns * 1.8f);
            }
            if (player.ZoneDungeon)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 0.3);
                NPC.maxSpawns = (int)(NPC.maxSpawns * 1.8f);
            }
            else if (player.ZoneSandstorm)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * (Main.hardMode ? 0.4f : 0.9f));
                NPC.maxSpawns = (int)(NPC.maxSpawns * (Main.hardMode ? 1.5f : 1.2f));
            }
            else if (player.ZoneUndergroundDesert)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 0.2f);
                NPC.maxSpawns = (int)(NPC.maxSpawns * 3f);
            }
            else if (player.ZoneJungle)
            {
                if (player.townNPCs == 0f)
                {
                    NPC.spawnRate = (int)(NPC.spawnRate * 0.4);
                    NPC.maxSpawns = (int)(NPC.maxSpawns * 1.5f);
                }
                else if (player.townNPCs == 1f)
                {
                    NPC.spawnRate = (int)(NPC.spawnRate * 0.55);
                    NPC.maxSpawns = (int)(NPC.maxSpawns * 1.4);
                }
                else if (player.townNPCs == 2f)
                {
                    NPC.spawnRate = (int)(NPC.spawnRate * 0.7);
                    NPC.maxSpawns = (int)(NPC.maxSpawns * 1.3f);
                }
                else
                {
                    NPC.spawnRate = (int)(NPC.spawnRate * 0.85);
                    NPC.maxSpawns = (int)(NPC.maxSpawns * 1.2f);
                }
            }
            else if (player.ZoneCorrupt || player.ZoneCrimson)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 0.65);
                NPC.maxSpawns = (int)(NPC.maxSpawns * 1.3f);
            }
            else if (player.ZoneMeteor)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 0.4);
                NPC.maxSpawns = (int)(NPC.maxSpawns * 1.1f);
            }
            if (flag4)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 0.8f);
                NPC.maxSpawns = (int)(NPC.maxSpawns * 1.2f);
                if (Main.remixWorld)
                {
                    NPC.spawnRate = (int)(NPC.spawnRate * 0.4);
                    NPC.maxSpawns = (int)(NPC.maxSpawns * 1.5f);
                }
            }
            if (Main.remixWorld && (player.ZoneCorrupt || player.ZoneCrimson) && (double)(player.position.Y / 16f) < Main.worldSurface)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 0.5);
                NPC.maxSpawns *= 2;
            }
            if (player.ZoneHallow && player.position.Y > Main.rockLayer * 16.0 + NPC.sHeight)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 0.65);
                NPC.maxSpawns = (int)(NPC.maxSpawns * 1.3f);
            }
            if (Main.wofNPCIndex >= 0 && player.position.Y > Main.UnderworldLayer * 16)
            {
                NPC.maxSpawns = (int)(NPC.maxSpawns * 0.3f);
                NPC.spawnRate *= 3;
            }
            if (player.nearbyActiveNPCs < NPC.maxSpawns * 0.2)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 0.6f);
            }
            else if (player.nearbyActiveNPCs < NPC.maxSpawns * 0.4)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 0.7f);
            }
            else if (player.nearbyActiveNPCs < NPC.maxSpawns * 0.6)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 0.8f);
            }
            else if (player.nearbyActiveNPCs < NPC.maxSpawns * 0.8)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 0.9f);
            }
            if ((double)(player.position.Y / 16f) > (Main.worldSurface + Main.rockLayer) / 2.0 || player.ZoneCorrupt || player.ZoneCrimson)
            {
                if (player.nearbyActiveNPCs < NPC.maxSpawns * 0.2)
                {
                    NPC.spawnRate = (int)(NPC.spawnRate * 0.7f);
                }
                else if (player.nearbyActiveNPCs < NPC.maxSpawns * 0.4)
                {
                    NPC.spawnRate = (int)(NPC.spawnRate * 0.9f);
                }
            }
            int maxValue = 65;
            if (Main.remixWorld && (double)(player.position.Y / 16f) < Main.worldSurface && (player.ZoneCorrupt || player.ZoneCrimson))
            {
                maxValue = 25;
                NPC.spawnRate = (int)(NPC.spawnRate * 0.8);
                NPC.maxSpawns *= 2;
            }
            if (player.invis)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 1.2f);
                NPC.maxSpawns = (int)(NPC.maxSpawns * 0.8f);
            }
            if (player.calmed)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 1.65f);
                NPC.maxSpawns = (int)(NPC.maxSpawns * 0.6f);
            }
            if (player.sunflower)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 1.2f);
                NPC.maxSpawns = (int)(NPC.maxSpawns * 0.8f);
            }
            if (player.anglerSetSpawnReduction)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 1.3f);
                NPC.maxSpawns = (int)(NPC.maxSpawns * 0.7f);
            }
            if (player.enemySpawns)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 0.5);
                NPC.maxSpawns = (int)(NPC.maxSpawns * 2f);
            }
            if (player.ZoneWaterCandle || player.inventory[player.selectedItem].type == 148)
            {
                if (!player.ZonePeaceCandle && player.inventory[player.selectedItem].type != 3117)
                {
                    NPC.spawnRate = (int)(NPC.spawnRate * 0.75);
                    NPC.maxSpawns = (int)(NPC.maxSpawns * 1.5f);
                }
            }
            else if (player.ZonePeaceCandle || player.inventory[player.selectedItem].type == 3117)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 1.3);
                NPC.maxSpawns = (int)(NPC.maxSpawns * 0.7f);
            }
            if (player.ZoneShadowCandle || player.inventory[player.selectedItem].type == 5322)
            {
                player.townNPCs = 0f;
            }
            if (player.ZoneWaterCandle && (double)(player.position.Y / 16f) < Main.worldSurface * 0.3499999940395355)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 0.5);
            }
            if (player.isNearFairy())
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 1.2f);
                NPC.maxSpawns = (int)(NPC.maxSpawns * 0.8f);
            }
            if (NPC.spawnRate < NPC.defaultSpawnRate * 0.1)
            {
                NPC.spawnRate = (int)(NPC.defaultSpawnRate * 0.1);
            }
            if (NPC.maxSpawns > NPC.defaultMaxSpawns * 3)
            {
                NPC.maxSpawns = NPC.defaultMaxSpawns * 3;
            }
            if (Main.getGoodWorld)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 0.8f);
                NPC.maxSpawns = (int)(NPC.maxSpawns * 1.2f);
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
                        NPC.spawnRate = (int)(NPC.spawnRate / num10);
                        NPC.maxSpawns = (int)(NPC.maxSpawns * num10);
                    }
                }
            }
            if ((Main.pumpkinMoon || Main.snowMoon) && (Main.remixWorld || player.position.Y < Main.worldSurface * 16.0))
            {
                NPC.maxSpawns = (int)(NPC.defaultMaxSpawns * (2.0 + 0.3 * num4));
                NPC.spawnRate = 20;
            }
            if (DD2Event.Ongoing && player.ZoneOldOneArmy)
            {
                NPC.maxSpawns = NPC.defaultMaxSpawns;
                NPC.spawnRate = NPC.defaultSpawnRate;
            }
            if (flag6)
            {
                NPC.maxSpawns = (int)(NPC.defaultMaxSpawns * (2.0 + 0.3 * num4));
                NPC.spawnRate = 20;
            }
            if (player.ZoneDungeon && !NPC.downedBoss3)
            {
                NPC.spawnRate = 10;
            }
            if (!flag6 && ((!Main.bloodMoon && !Main.pumpkinMoon && !Main.snowMoon) || Main.dayTime) && (!Main.eclipse || !Main.dayTime) && !player.ZoneDungeon && !player.ZoneCorrupt && !player.ZoneCrimson && !player.ZoneMeteor && !player.ZoneOldOneArmy)
            {
                if (player.Center.Y / 16f > Main.UnderworldLayer && (!Main.remixWorld || !((double)(player.Center.X / 16f) > Main.maxTilesX * 0.39 + 50.0) || !((double)(player.Center.X / 16f) < Main.maxTilesX * 0.61)))
                {
                    if (player.townNPCs == 1f)
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
                    else if (player.townNPCs == 2f)
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
                    else if (player.townNPCs >= 3f)
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
                            NPC.spawnRate = (int)(NPC.spawnRate * 2f);
                        }
                    }
                }
                else if (player.townNPCs == 1f)
                {
                    flag5 = true;
                    if (player.ZoneGraveyard)
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
                        NPC.spawnRate = (int)(NPC.spawnRate * 2f);
                    }
                }
                else if (player.townNPCs == 2f)
                {
                    flag5 = true;
                    if (player.ZoneGraveyard)
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
                        NPC.spawnRate = (int)(NPC.spawnRate * 3f);
                    }
                }
                else if (player.townNPCs >= 3f)
                {
                    flag5 = true;
                    if (player.ZoneGraveyard)
                    {
                        NPC.spawnRate = (int)(NPC.spawnRate * 3f);
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
            if (player.active && !player.dead && player.nearbyActiveNPCs < NPC.maxSpawns && Main.rand.Next(NPC.spawnRate) == 0)
            {
                bool flag16 = player.ZoneTowerNebula || player.ZoneTowerSolar || player.ZoneTowerStardust || player.ZoneTowerVortex;
                NPC.spawnRangeX = (int)(NPC.sWidth / 16 * 0.7);
                NPC.spawnRangeY = (int)(NPC.sHeight / 16 * 0.7);
                NPC.safeRangeX = (int)(NPC.sWidth / 16 * 0.52);
                NPC.safeRangeY = (int)(NPC.sHeight / 16 * 0.52);
                if (player.inventory[player.selectedItem].type == 1254 || player.inventory[player.selectedItem].type == 1299 || player.scope)
                {
                    float num11 = 1.5f;
                    if (player.inventory[player.selectedItem].type == 1254 && player.scope)
                    {
                        num11 = 1.25f;
                    }
                    else if (player.inventory[player.selectedItem].type == 1254)
                    {
                        num11 = 1.5f;
                    }
                    else if (player.inventory[player.selectedItem].type == 1299)
                    {
                        num11 = 1.5f;
                    }
                    else if (player.scope)
                    {
                        num11 = 2f;
                    }
                    NPC.spawnRangeX += (int)(NPC.sWidth / 16 * 0.5 / (double)num11);
                    NPC.spawnRangeY += (int)(NPC.sHeight / 16 * 0.5 / (double)num11);
                    NPC.safeRangeX += (int)(NPC.sWidth / 16 * 0.5 / (double)num11);
                    NPC.safeRangeY += (int)(NPC.sHeight / 16 * 0.5 / (double)num11);
                }
                int num12 = (int)(player.position.X / 16f) - NPC.spawnRangeX;
                int num13 = (int)(player.position.X / 16f) + NPC.spawnRangeX;
                int num14 = (int)(player.position.Y / 16f) - NPC.spawnRangeY;
                int num15 = (int)(player.position.Y / 16f) + NPC.spawnRangeY;
                int num16 = (int)(player.position.X / 16f) - NPC.safeRangeX;
                int num17 = (int)(player.position.X / 16f) + NPC.safeRangeX;
                int num18 = (int)(player.position.Y / 16f) - NPC.safeRangeY;
                int num19 = (int)(player.position.Y / 16f) + NPC.safeRangeY;
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
                        if (!flag6 && num21 < Main.worldSurface * 0.3499999940395355 && !flag12 && (num20 < Main.maxTilesX * 0.45 || num20 > Main.maxTilesX * 0.55 || Main.hardMode))
                        {
                            num3 = Main.tile[num20, num21].type;
                            tileX = num20;
                            tileY = num21;
                            flag2 = true;
                            flag3 = true;
                        }
                        else if (!flag6 && num21 < Main.worldSurface * 0.44999998807907104 && !flag12 && Main.hardMode && Main.rand.Next(10) == 0)
                        {
                            num3 = Main.tile[num20, num21].type;
                            tileX = num20;
                            tileY = num21;
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
                                        tileX = num20;
                                        tileY = n;
                                        flag2 = true;
                                    }
                                    break;
                                }
                            }
                        }
                        if (player.ZoneShadowCandle)
                        {
                            flag5 = false;
                        }
                        else if (!flag3 && player.afkCounter >= NPC.AFKTimeNeededForNoWorms)
                        {
                            flag5 = true;
                        }
                        if (flag2)
                        {
                            int num22 = tileX - NPC.spawnSpaceX / 2;
                            int num23 = tileX + NPC.spawnSpaceX / 2;
                            int num24 = tileY - NPC.spawnSpaceY;
                            int num25 = tileY;
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
                            if (tileX >= num16 && tileX <= num17)
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
            int spawnX = tileX * 16 + 8;
            int spawnY = tileY * 16;
            if (flag2)
            {
                Rectangle rectangle = new(tileX * 16, spawnY, 16, 16);
                for (int num28 = 0; num28 < 255; num28++)
                {
                    if (Main.player[num28].active)
                    {
                        Rectangle rectangle2 = new((int)(Main.player[num28].position.X + Main.player[num28].width / 2 - NPC.sWidth / 2 - NPC.safeRangeX), (int)(Main.player[num28].position.Y + Main.player[num28].height / 2 - NPC.sHeight / 2 - NPC.safeRangeY), NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                        if (rectangle.Intersects(rectangle2))
                        {
                            flag2 = false;
                        }
                    }
                }
            }
            if (flag2)
            {
                if (player.ZoneDungeon && (!Main.tileDungeon[Main.tile[tileX, tileY].type] || Main.tile[tileX, tileY - 1].wall == 0))
                {
                    flag2 = false;
                }
                if (Main.tile[tileX, tileY - 1].liquid > 0 && Main.tile[tileX, tileY - 2].liquid > 0 && !Main.tile[tileX, tileY - 1].lava())
                {
                    if (Main.tile[tileX, tileY - 1].shimmer())
                    {
                        flag2 = false;
                    }
                    if (Main.tile[tileX, tileY - 1].honey())
                    {
                        flag8 = true;
                    }
                    else
                    {
                        flag7 = true;
                    }
                }
                int num29 = (int)player.Center.X / 16;
                int num30 = (int)(player.Bottom.Y + 8f) / 16;
                if (Main.tile[tileX, tileY].type == 367)
                {
                    flag10 = true;
                }
                else if (Main.tile[tileX, tileY].type == 368)
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
                    if (tileX - num31 < 0)
                    {
                        num31 = tileX;
                    }
                    if (tileY - num31 < 0)
                    {
                        num31 = tileY;
                    }
                    if (tileX + num31 >= Main.maxTilesX)
                    {
                        num31 = Main.maxTilesX - tileX - 1;
                    }
                    if (tileY + num31 >= Main.maxTilesY)
                    {
                        num31 = Main.maxTilesY - tileY - 1;
                    }
                    for (int num33 = tileX - num31; num33 <= tileX + num31; num33 += num32)
                    {
                        int num34 = Main.rand.Next(1, 4);
                        for (int num35 = tileY - num31; num35 <= tileY + num31; num35 += num34)
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
            bool flag17 = tileY <= Main.rockLayer;
            if (Main.remixWorld)
            {
                flag17 = tileY > Main.rockLayer && tileY <= Main.maxTilesY - 190;
            }
            bool flag18 = tileY > Main.rockLayer && tileY < Main.UnderworldLayer;
            if (Main.dontStarveWorld)
            {
                flag18 = tileY < Main.UnderworldLayer;
            }
            if (flag18 && !player.ZoneDungeon && !flag6)
            {
                if (Main.rand.Next(3) == 0)
                {
                    int num39 = Main.rand.Next(5, 15);
                    if (tileX - num39 >= 0 && tileX + num39 < Main.maxTilesX)
                    {
                        for (int num40 = tileX - num39; num40 < tileX + num39; num40++)
                        {
                            for (int num41 = tileY - num39; num41 < tileY + num39; num41++)
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
                    int x = (int)player.position.X / 16;
                    int y = (int)player.position.Y / 16;
                    if (Main.tile[x, y].wall == 62)
                    {
                        flag11 = true;
                    }
                }
            }
            if (tileY < Main.rockLayer && tileY > 200 && !player.ZoneDungeon && !flag6)
            {
                if (Main.rand.Next(3) == 0)
                {
                    int num42 = Main.rand.Next(5, 15);
                    if (tileX - num42 >= 0 && tileX + num42 < Main.maxTilesX)
                    {
                        for (int num43 = tileX - num42; num43 < tileX + num42; num43++)
                        {
                            for (int num44 = tileY - num42; num44 < tileY + num42; num44++)
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
                    int x2 = (int)player.position.X / 16;
                    int y2 = (int)player.position.Y / 16;
                    if (WallID.Sets.AllowsUndergroundDesertEnemiesToSpawn[Main.tile[x2, y2].wall])
                    {
                        flag13 = true;
                    }
                }
            }

            int num45 = Main.tile[tileX, tileY].type;
            int num46 = Main.tile[tileX, tileY - 1].wall;
            if (Main.tile[tileX, tileY - 2].wall == 244 || Main.tile[tileX, tileY].wall == 244)
            {
                num46 = 244;
            }
            bool flag19 = new Point(num8 - tileX, num9 - tileY).X * Main.windSpeedTarget > 0f;
            bool flag20 = tileY <= Main.worldSurface;
            bool flag21 = tileY >= Main.rockLayer;
            bool flag22 = ((tileX < WorldGen.oceanDistance || tileX > Main.maxTilesX - WorldGen.oceanDistance) && Main.tileSand[num45] && tileY < Main.rockLayer) || (num3 == 53 && WorldGen.oceanDepths(tileX, tileY));
            bool flag23 = tileY <= Main.worldSurface && (tileX < WorldGen.beachDistance || tileX > Main.maxTilesX - WorldGen.beachDistance);
            bool flag24 = Main.cloudAlpha > 0f;
            int range = 10;
            if (Main.remixWorld)
            {
                flag24 = Main.raining;
                flag21 = tileY > Main.worldSurface && tileY < Main.rockLayer;
                if (tileY < Main.worldSurface + 5.0)
                {
                    Main.raining = false;
                    Main.cloudAlpha = 0f;
                    Main.dayTime = false;
                }
                range = 5;
                if (player.ZoneCorrupt || player.ZoneCrimson)
                {
                    flag22 = false;
                    flag23 = false;
                }
                if (tileX < Main.maxTilesX * 0.43 || tileX > Main.maxTilesX * 0.57)
                {
                    if (tileY > Main.rockLayer - 200.0 && tileY < Main.maxTilesY - 200 && Main.rand.Next(2) == 0)
                    {
                        flag22 = true;
                    }
                    if (tileY > Main.rockLayer - 200.0 && tileY < Main.maxTilesY - 200 && Main.rand.Next(2) == 0)
                    {
                        flag23 = true;
                    }
                }
                if (tileY > Main.rockLayer - 20.0)
                {
                    if (tileY <= Main.maxTilesY - 190 && Main.rand.Next(3) != 0)
                    {
                        flag20 = true;
                        Main.dayTime = false;
                        if (Main.rand.Next(2) == 0)
                        {
                            Main.dayTime = true;
                        }
                    }
                    else if ((Main.bloodMoon || (Main.eclipse && Main.dayTime)) && tileX > Main.maxTilesX * 0.38 + 50.0 && tileX < Main.maxTilesX * 0.62)
                    {
                        flag20 = true;
                    }
                }
            }
            num45 = NPC.SpawnNPC_TryFindingProperGroundTileType(num45, tileX, tileY);
            int newNPC = 200;
            int cattailX;
            int cattailY;
            if (player.ZoneTowerNebula)
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
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, num47, 1);
                }
            }
            else if (player.ZoneTowerVortex)
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
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, num48, 1);
                }
            }
            else if (player.ZoneTowerStardust)
            {
                int num49 = Utils.SelectRandom<int>(Main.rand, 411, 411, 411, 409, 409, 407, 402, 405);
                if (num49 != 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, num49, 1);
                }
            }
            else if (player.ZoneTowerSolar)
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
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, num50, 1);
                }
            }
            else if (flag3)
            {
                int maxValue2 = 8;
                int maxValue3 = 30;
                bool flag28 = Math.Abs(tileX - Main.maxTilesX / 2) / (float)(Main.maxTilesX / 2) > 0.33f && (Main.wallLight[Main.tile[num8, num9].wall] || Main.tile[num8, num9].wall == 73);
                if (flag28 && NPC.AnyDanger())
                {
                    flag28 = false;
                }
                if (player.ZoneWaterCandle)
                {
                    maxValue2 = 3;
                    maxValue3 = 10;
                }
                if (flag6 && Main.invasionType == 4)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 388);
                }
                else if (flag28 && Main.hardMode && NPC.downedGolemBoss && ((!NPC.downedMartians && Main.rand.Next(maxValue2) == 0) || Main.rand.Next(maxValue3) == 0) && !NPC.AnyNPCs(399))
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 399);
                }
                else if (flag28 && Main.hardMode && NPC.downedGolemBoss && ((!NPC.downedMartians && Main.rand.Next(maxValue2) == 0) || Main.rand.Next(maxValue3) == 0) && !NPC.AnyNPCs(399) && (player.inventory[player.selectedItem].type == 148 || player.ZoneWaterCandle))
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 399);
                }
                else if (Main.hardMode && !NPC.AnyNPCs(87) && !flag5 && Main.rand.Next(10) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 87, 1);
                }
                else if (Main.hardMode && !NPC.AnyNPCs(87) && !flag5 && Main.rand.Next(10) == 0 && (player.inventory[player.selectedItem].type == 148 || player.ZoneWaterCandle))
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 87, 1);
                }
                else if (!NPC.unlockedSlimePurpleSpawn && player.RollLuck(25) == 0 && !NPC.AnyNPCs(686))
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 686);
                }
                else
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 48);
                }
            }
            else if (flag6)
            {
                if (Main.invasionType == 1)
                {
                    if (Main.hardMode && !NPC.AnyNPCs(471) && Main.rand.Next(30) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 471);
                    }
                    else if (Main.rand.Next(9) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 29);
                    }
                    else if (Main.rand.Next(5) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 26);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 111);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 27);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 28);
                    }
                }
                else if (Main.invasionType == 2)
                {
                    if (Main.rand.Next(7) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 145);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 143);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 144);
                    }
                }
                else if (Main.invasionType == 3)
                {
                    if (Main.invasionSize < Main.invasionSizeStart / 2 && Main.rand.Next(20) == 0 && !NPC.AnyNPCs(491) && !Collision.SolidTiles(tileX - 20, tileX + 20, tileY - 40, tileY - 10))
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, (tileY - 10) * 16, 491);
                    }
                    else if (Main.rand.Next(30) == 0 && !NPC.AnyNPCs(216))
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 216);
                    }
                    else if (Main.rand.Next(11) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 215);
                    }
                    else if (Main.rand.Next(9) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 252);
                    }
                    else if (Main.rand.Next(7) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 214);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 213);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 212);
                    }
                }
                else if (Main.invasionType == 4)
                {
                    int num51 = 0;
                    int num52 = Main.rand.Next(7);
                    bool flag29 = (Main.invasionSizeStart - Main.invasionSize) / (float)Main.invasionSizeStart >= 0.3f && !NPC.AnyNPCs(395);
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
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, num51, 1);
                    }
                }
            }
            else if (num46 == 244 && !Main.remixWorld)
            {
                if (flag7)
                {
                    if (player.RollLuck(NPC.goldCritterChance) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 592);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 55);
                    }
                }
                else if (tileY > Main.worldSurface)
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 447);
                        }
                        else
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 300);
                        }
                    }
                    else if (Main.rand.Next(2) == 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 359);
                    }
                    else if (player.RollLuck(NPC.goldCritterChance) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 448);
                    }
                    else if (Main.rand.Next(3) != 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 357);
                    }
                }
                else if (player.RollLuck(2) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 624);
                    Main.npc[newNPC].timeLeft *= 10;
                }
                else if (player.RollLuck(NPC.goldCritterChance) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 443);
                }
                else if (player.RollLuck(NPC.goldCritterChance) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 539);
                }
                else if (Main.halloween && Main.rand.Next(3) != 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 303);
                }
                else if (Main.xMas && Main.rand.Next(3) != 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 337);
                }
                else if (BirthdayParty.PartyIsUp && Main.rand.Next(3) != 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 540);
                }
                else if (Main.rand.Next(3) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Utils.SelectRandom(Main.rand, new short[2] { 299, 538 }));
                }
                else
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 46);
                }
            }
            else if (!NPC.savedBartender && DD2Event.ReadyToFindBartender && !NPC.AnyNPCs(579) && Main.rand.Next(80) == 0 && !flag7)
            {
                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 579);
            }
            else if (Main.tile[tileX, tileY].wall == 62 || flag11)
            {
                bool flag30 = flag21 && tileY < Main.maxTilesY - 210;
                if (Main.dontStarveWorld)
                {
                    flag30 = tileY < Main.maxTilesY - 210;
                }
                if (Main.tile[tileX, tileY].wall == 62 && Main.rand.Next(8) == 0 && !flag7 && flag30 && !NPC.savedStylist && !NPC.AnyNPCs(354))
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 354);
                }
                else if (Main.hardMode && Main.rand.Next(10) != 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 163);
                }
                else
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 164);
                }
            }
            else if ((NPC.SpawnTileOrAboveHasAnyWallInSet(tileX, tileY, WallID.Sets.AllowsUndergroundDesertEnemiesToSpawn) || flag13) && WorldGen.checkUnderground(tileX, tileY))
            {
                float num56 = 1.15f;
                if (tileY > (Main.rockLayer * 2.0 + Main.maxTilesY) / 3.0)
                {
                    num56 *= 0.5f;
                }
                else if (tileY > Main.rockLayer)
                {
                    num56 *= 0.85f;
                }
                if (Main.rand.Next(20) == 0 && !flag7 && !NPC.savedGolfer && !NPC.AnyNPCs(589))
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 589);
                }
                else if (Main.hardMode && Main.rand.Next((int)(45f * num56)) == 0 && !flag5 && tileY > Main.worldSurface + 100.0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 510);
                }
                else if (Main.rand.Next((int)(45f * num56)) == 0 && !flag5 && tileY > Main.worldSurface + 100.0 && NPC.CountNPCS(513) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 513);
                }
                else if (Main.hardMode && Main.rand.Next(5) != 0)
                {
                    List<int> list = new();
                    if (player.ZoneCorrupt)
                    {
                        list.Add(525);
                        list.Add(525);
                    }
                    if (player.ZoneCrimson)
                    {
                        list.Add(526);
                        list.Add(526);
                    }
                    if (player.ZoneHallow)
                    {
                        list.Add(527);
                        list.Add(527);
                    }
                    if (list.Count == 0)
                    {
                        list.Add(524);
                        list.Add(524);
                    }
                    if (player.ZoneCorrupt || player.ZoneCrimson)
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
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, num57);
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
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, num58);
                }
            }
            else if (Main.hardMode && flag7 && player.ZoneJungle && Main.rand.Next(3) != 0)
            {
                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 157);
            }
            else if (Main.hardMode && flag7 && player.ZoneCrimson && Main.rand.Next(3) != 0)
            {
                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 242);
            }
            else if (Main.hardMode && flag7 && player.ZoneCrimson && Main.rand.Next(3) != 0)
            {
                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 241);
            }
            else if ((!flag12 || (!NPC.savedAngler && !NPC.AnyNPCs(376))) && flag7 && flag22)
            {
                bool flag31 = false;
                if (!NPC.savedAngler && !NPC.AnyNPCs(376) && (tileY < Main.worldSurface - 10.0 || Main.remixWorld))
                {
                    int num59 = -1;
                    for (int num60 = tileY - 1; num60 > tileY - 50; num60--)
                    {
                        if (Main.tile[tileX, num60].liquid == 0 && !WorldGen.SolidTile(tileX, num60) && !WorldGen.SolidTile(tileX, num60 + 1) && !WorldGen.SolidTile(tileX, num60 + 2))
                        {
                            num59 = num60 + 2;
                            break;
                        }
                    }
                    if (num59 > tileY)
                    {
                        num59 = tileY;
                    }
                    if (num59 > 0 && !flag15)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num59 * 16, 376);
                        flag31 = true;
                    }
                }
                if (!flag31 && !flag15)
                {
                    int num61 = -1;
                    int num62 = -1;
                    if ((tileY < Main.worldSurface || Main.remixWorld) && tileY > 50)
                    {
                        for (int num63 = tileY - 1; num63 > tileY - 50; num63--)
                        {
                            if (Main.tile[tileX, num63].liquid == 0 && !WorldGen.SolidTile(tileX, num63) && !WorldGen.SolidTile(tileX, num63 + 1) && !WorldGen.SolidTile(tileX, num63 + 2))
                            {
                                num61 = num63 + 2;
                                if (!WorldGen.SolidTile(tileX, num61 + 1) && !WorldGen.SolidTile(tileX, num61 + 2) && !Main.wallHouse[Main.tile[tileX, num61 + 2].wall])
                                {
                                    num62 = num61 + 2;
                                }
                                if (Main.wallHouse[Main.tile[tileX, num61].wall])
                                {
                                    num61 = -1;
                                }
                                break;
                            }
                        }
                        if (num61 > tileY)
                        {
                            num61 = tileY;
                        }
                        if (num62 > tileY)
                        {
                            num62 = tileY;
                        }
                    }
                    if (num61 > 0 && !flag15 && Main.rand.Next(10) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num61 * 16, 602);
                    }
                    else if (Main.rand.Next(10) == 0)
                    {
                        int num64 = Main.rand.Next(3);
                        if (num64 == 0 && num61 > 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num61 * 16, 625);
                        }
                        else if (num64 == 1 && num62 > 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num62 * 16, 615);
                        }
                        else if (num64 == 2)
                        {
                            int num65 = tileY;
                            if (num62 > 0)
                            {
                                num65 = num62;
                            }
                            if (player.RollLuck(NPC.goldCritterChance) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num65 * 16, 627);
                            }
                            else
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num65 * 16, 626);
                            }
                        }
                    }
                    else if (Main.rand.Next(40) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 220);
                    }
                    else if (Main.rand.Next(18) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 221);
                    }
                    else if (Main.rand.Next(8) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 65);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 67);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 64);
                    }
                }
            }
            else if (!flag7 && !NPC.savedAngler && !NPC.AnyNPCs(376) && (tileX < WorldGen.beachDistance || tileX > Main.maxTilesX - WorldGen.beachDistance) && Main.tileSand[num45] && (tileY < Main.worldSurface || Main.remixWorld))
            {
                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 376);
            }
            else if (!flag12 && flag7 && ((flag21 && Main.rand.Next(2) == 0) || num45 == 60))
            {
                bool flag32 = false;
                if (num45 == 60 && flag20 && tileY > 50 && Main.rand.Next(3) == 0 && Main.dayTime)
                {
                    int num66 = -1;
                    for (int num67 = tileY - 1; num67 > tileY - 50; num67--)
                    {
                        if (Main.tile[tileX, num67].liquid == 0 && !WorldGen.SolidTile(tileX, num67) && !WorldGen.SolidTile(tileX, num67 + 1) && !WorldGen.SolidTile(tileX, num67 + 2))
                        {
                            num66 = num67 + 2;
                            break;
                        }
                    }
                    if (num66 > tileY)
                    {
                        num66 = tileY;
                    }
                    if (num66 > 0 && !flag15)
                    {
                        flag32 = true;
                        if (Main.rand.Next(4) == 0)
                        {
                            flag32 = true;
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num66 * 16, 617);
                        }
                        else if (!flag && Main.cloudAlpha == 0f)
                        {
                            flag32 = true;
                            int num68 = Main.rand.Next(1, 4);
                            for (int num69 = 0; num69 < num68; num69++)
                            {
                                if (player.RollLuck(NPC.goldCritterChance) == 0)
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + Main.rand.Next(-16, 17), num66 * 16 - 16, 613);
                                }
                                else
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + Main.rand.Next(-16, 17), num66 * 16 - 16, 612);
                                }
                            }
                        }
                    }
                }
                if (!flag32)
                {
                    if (Main.hardMode && Main.rand.Next(3) > 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 102);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 58);
                    }
                }
            }
            else if (!flag12 && flag7 && tileY > Main.worldSurface && Main.rand.Next(3) == 0)
            {
                if (Main.hardMode && Main.rand.Next(3) > 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 103);
                }
                else
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 63);
                }
            }
            else if (flag7 && Main.rand.Next(4) == 0 && ((tileX > WorldGen.oceanDistance && tileX < Main.maxTilesX - WorldGen.oceanDistance) || tileY > Main.worldSurface + 50.0))
            {
                if (player.ZoneCorrupt)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 57);
                }
                else if (player.ZoneCrimson)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 465);
                }
                else if (tileY < Main.worldSurface && tileY > 50 && Main.rand.Next(3) != 0 && Main.dayTime)
                {
                    int num70 = -1;
                    for (int num71 = tileY - 1; num71 > tileY - 50; num71--)
                    {
                        if (Main.tile[tileX, num71].liquid == 0 && !WorldGen.SolidTile(tileX, num71) && !WorldGen.SolidTile(tileX, num71 + 1) && !WorldGen.SolidTile(tileX, num71 + 2))
                        {
                            num70 = num71 + 2;
                            break;
                        }
                    }
                    if (num70 > tileY)
                    {
                        num70 = tileY;
                    }
                    if (num70 > 0 && !flag15)
                    {
                        if (Main.rand.Next(5) == 0 && (num3 == 2 || num3 == 477))
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num70 * 16, 616);
                        }
                        else if (num3 == 53)
                        {
                            if (Main.rand.Next(2) == 0 && !flag && Main.cloudAlpha == 0f)
                            {
                                int num72 = Main.rand.Next(1, 4);
                                for (int num73 = 0; num73 < num72; num73++)
                                {
                                    if (player.RollLuck(NPC.goldCritterChance) == 0)
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + Main.rand.Next(-16, 17), num70 * 16 - 16, 613);
                                    }
                                    else
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + Main.rand.Next(-16, 17), num70 * 16 - 16, 612);
                                    }
                                }
                            }
                            else
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num70 * 16, 608);
                            }
                        }
                        else
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(2) == 0 ? 362 : 364);
                        }
                    }
                    else if (num3 == 53 && tileX > WorldGen.beachDistance && tileX < Main.maxTilesX - WorldGen.beachDistance)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num70 * 16, 607);
                    }
                    else if (player.RollLuck(NPC.goldCritterChance) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 592);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 55);
                    }
                }
                else if (num3 == 53 && tileX > WorldGen.beachDistance && tileX < Main.maxTilesX - WorldGen.beachDistance)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 607);
                }
                else if (player.RollLuck(NPC.goldCritterChance) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 592);
                }
                else
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 55);
                }
            }
            else if (NPC.downedGoblins && player.RollLuck(20) == 0 && !flag7 && flag21 && tileY < Main.maxTilesY - 210 && !NPC.savedGoblin && !NPC.AnyNPCs(105))
            {
                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 105);
            }
            else if (Main.hardMode && player.RollLuck(20) == 0 && !flag7 && flag21 && tileY < Main.maxTilesY - 210 && !NPC.savedWizard && !NPC.AnyNPCs(106))
            {
                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 106);
            }
            //else if (NPC.downedBoss3 && player.RollLuck(20) == 0 && !flag7 && flag21 && num2 < Main.maxTilesY - 210 && !NPC.unlockedSlimeOldSpawn && !NPC.AnyNPCs(685))
            else if (NPC.downedBoss3 && player.RollLuck(20) == 0 && !flag7 && flag21 && tileY < Main.maxTilesY - 210 && !NPC.AnyNPCs(685))
            {
                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 685);
            }
            else if (flag12)
            {
                if (player.ZoneGraveyard)
                {
                    if (!flag7)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(2) == 0 ? 606 : 610);
                    }
                }
                else if (!flag15 && flag23)
                {
                    if (flag7)
                    {
                        int num74 = -1;
                        int num75 = -1;
                        if ((tileY < Main.worldSurface || Main.remixWorld) && tileY > 50)
                        {
                            for (int num76 = tileY - 1; num76 > tileY - 50; num76--)
                            {
                                if (Main.tile[tileX, num76].liquid == 0 && !WorldGen.SolidTile(tileX, num76) && !WorldGen.SolidTile(tileX, num76 + 1) && !WorldGen.SolidTile(tileX, num76 + 2))
                                {
                                    num74 = num76 + 2;
                                    if (!WorldGen.SolidTile(tileX, num74 + 1) && !WorldGen.SolidTile(tileX, num74 + 2))
                                    {
                                        num75 = num74 + 2;
                                    }
                                    break;
                                }
                            }
                            if (num74 > tileY)
                            {
                                num74 = tileY;
                            }
                            if (num75 > tileY)
                            {
                                num75 = tileY;
                            }
                        }
                        if (Main.rand.Next(2) == 0)
                        {
                            int num77 = Main.rand.Next(3);
                            if (num77 == 0 && num74 > 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num74 * 16, 625);
                            }
                            else if (num77 == 1 && num75 > 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num75 * 16, 615);
                            }
                            else if (num77 == 2)
                            {
                                int num78 = tileY;
                                if (num75 > 0)
                                {
                                    num78 = num75;
                                }
                                if (player.RollLuck(NPC.goldCritterChance) == 0)
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num78 * 16, 627);
                                }
                                else
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num78 * 16, 626);
                                }
                            }
                        }
                        else if (num74 > 0 && !flag15)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num74 * 16, 602);
                        }
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 602);
                    }
                }
                else if ((num45 == 2 || num45 == 477 || num45 == 53) && !tooWindyForButterflies && Main.raining && Main.dayTime && Main.rand.Next(2) == 0 && (tileY <= Main.worldSurface || Main.remixWorld) && NPC.FindCattailTop(tileX, tileY, out cattailX, out cattailY))
                {
                    if (player.RollLuck(NPC.goldCritterChance) == 0)
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
                    if (flag20 && tileY > 50 && Main.rand.Next(3) != 0 && Main.dayTime)
                    {
                        int num79 = -1;
                        for (int num80 = tileY - 1; num80 > tileY - 50; num80--)
                        {
                            if (Main.tile[tileX, num80].liquid == 0 && !WorldGen.SolidTile(tileX, num80) && !WorldGen.SolidTile(tileX, num80 + 1) && !WorldGen.SolidTile(tileX, num80 + 2))
                            {
                                num79 = num80 + 2;
                                break;
                            }
                        }
                        if (num79 > tileY)
                        {
                            num79 = tileY;
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
                                            if (player.RollLuck(NPC.goldCritterChance) == 0)
                                            {
                                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + Main.rand.Next(-16, 17), num79 * 16 - 16, 613);
                                            }
                                            else
                                            {
                                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + Main.rand.Next(-16, 17), num79 * 16 - 16, 612);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num79 * 16, 617);
                                    }
                                    break;
                                case 53:
                                    if (Main.rand.Next(3) != 0 && !flag && Main.cloudAlpha == 0f)
                                    {
                                        int num81 = Main.rand.Next(1, 4);
                                        for (int num82 = 0; num82 < num81; num82++)
                                        {
                                            if (player.RollLuck(NPC.goldCritterChance) == 0)
                                            {
                                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + Main.rand.Next(-16, 17), num79 * 16 - 16, 613);
                                            }
                                            else
                                            {
                                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + Main.rand.Next(-16, 17), num79 * 16 - 16, 612);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num79 * 16, 608);
                                    }
                                    break;
                                default:
                                    if (Main.rand.Next(5) == 0 && (num3 == 2 || num3 == 477))
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num79 * 16, 616);
                                    }
                                    else
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(2) == 0 ? 362 : 364);
                                    }
                                    break;
                            }
                        }
                        else if (num3 == 53 && tileX > WorldGen.beachDistance && tileX < Main.maxTilesX - WorldGen.beachDistance)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 607);
                        }
                        else if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 592);
                        }
                        else
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 55);
                        }
                    }
                    else if (num3 == 53 && tileX > WorldGen.beachDistance && tileX < Main.maxTilesX - WorldGen.beachDistance)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 607);
                    }
                    else if (player.RollLuck(NPC.goldCritterChance) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 592);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 55);
                    }
                }
                else if (num45 == 147 || num45 == 161)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(2) == 0 ? 148 : 149);
                }
                else if (num45 == 60)
                {
                    if (Main.dayTime && Main.rand.Next(3) != 0)
                    {
                        switch (Main.rand.Next(5))
                        {
                            case 0:
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 671);
                                break;
                            case 1:
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 672);
                                break;
                            case 2:
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 673);
                                break;
                            case 3:
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 674);
                                break;
                            default:
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 675);
                                break;
                        }
                    }
                    else
                    {
                        NPC.SpawnNPC_SpawnFrog(tileX, tileY, k);
                    }
                }
                else if (num45 == 53)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(366, 368));
                }
                else
                {
                    if (num45 != 2 && num45 != 477 && num45 != 109 && num45 != 492 && !(tileY > Main.worldSurface))
                    {
                        break;
                    }
                    bool flag33 = flag20;
                    if (Main.raining && tileY <= Main.UnderworldLayer)
                    {
                        if (flag21 && Main.rand.Next(5) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, NPC.SpawnNPC_GetGemSquirrelToSpawn());
                        }
                        else if (flag21 && Main.rand.Next(5) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, NPC.SpawnNPC_GetGemBunnyToSpawn());
                        }
                        else if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 448);
                        }
                        else if (Main.rand.Next(3) != 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 357);
                        }
                        else if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 593);
                        }
                        else
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 230);
                        }
                    }
                    else if (!Main.dayTime && Main.numClouds <= 55 && Main.cloudBGActive == 0f && Star.starfallBoost > 3f && flag33 && player.RollLuck(2) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 484);
                    }
                    else if (!tooWindyForButterflies && !Main.dayTime && Main.rand.Next(NPC.fireFlyFriendly) == 0 && flag33)
                    {
                        int num85 = 355;
                        if (num45 == 109)
                        {
                            num85 = 358;
                        }
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, num85);
                        if (Main.rand.Next(NPC.fireFlyMultiple) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX - 16, spawnY, num85);
                        }
                        if (Main.rand.Next(NPC.fireFlyMultiple) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + 16, spawnY, num85);
                        }
                        if (Main.rand.Next(NPC.fireFlyMultiple) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY - 16, num85);
                        }
                        if (Main.rand.Next(NPC.fireFlyMultiple) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY + 16, num85);
                        }
                    }
                    else if (Main.cloudAlpha == 0f && !Main.dayTime && Main.rand.Next(5) == 0 && flag33)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 611);
                    }
                    else if (Main.dayTime && Main.time < 18000.0 && Main.rand.Next(3) != 0 && flag33)
                    {
                        int num86 = Main.rand.Next(4);
                        if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 442);
                        }
                        else
                        {
                            switch (num86)
                            {
                                case 0:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 297);
                                    break;
                                case 1:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 298);
                                    break;
                                default:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 74);
                                    break;
                            }
                        }
                    }
                    else if (!tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(NPC.stinkBugChance) == 0 && flag33)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 669);
                        if (Main.rand.Next(4) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX - 16, spawnY, 669);
                        }
                        if (Main.rand.Next(4) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + 16, spawnY, 669);
                        }
                    }
                    else if (!tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(NPC.butterflyChance) == 0 && flag33)
                    {
                        if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 444);
                        }
                        else
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 356);
                        }
                        if (Main.rand.Next(4) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX - 16, spawnY, 356);
                        }
                        if (Main.rand.Next(4) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + 16, spawnY, 356);
                        }
                    }
                    else if (tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(NPC.butterflyChance / 2) == 0 && flag33)
                    {
                        if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 605);
                        }
                        else
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 604);
                        }
                        if (Main.rand.Next(3) != 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 604);
                        }
                        if (Main.rand.Next(2) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 604);
                        }
                        if (Main.rand.Next(3) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 604);
                        }
                        if (Main.rand.Next(4) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 604);
                        }
                    }
                    else if (Main.rand.Next(2) == 0 && flag33)
                    {
                        int num87 = Main.rand.Next(4);
                        if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 442);
                        }
                        else
                        {
                            switch (num87)
                            {
                                case 0:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 297);
                                    break;
                                case 1:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 298);
                                    break;
                                default:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 74);
                                    break;
                            }
                        }
                    }
                    else if (tileY > Main.UnderworldLayer)
                    {
                        if (Main.remixWorld && (double)(player.Center.X / 16f) > Main.maxTilesX * 0.39 + 50.0 && (double)(player.Center.X / 16f) < Main.maxTilesX * 0.61 && Main.rand.Next(2) == 0)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, NPC.SpawnNPC_GetGemSquirrelToSpawn());
                            }
                            else
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, NPC.SpawnNPC_GetGemBunnyToSpawn());
                            }
                        }
                        else
                        {
                            newNPC = NPC.SpawnNPC_SpawnLavaBaitCritters(tileX, tileY);
                        }
                    }
                    else if (player.RollLuck(NPC.goldCritterChance) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 443);
                    }
                    else if (player.RollLuck(NPC.goldCritterChance) == 0 && flag33)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 539);
                    }
                    else if (Main.halloween && Main.rand.Next(3) != 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 303);
                    }
                    else if (Main.xMas && Main.rand.Next(3) != 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 337);
                    }
                    else if (BirthdayParty.PartyIsUp && Main.rand.Next(3) != 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 540);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        if (Main.remixWorld)
                        {
                            if (tileY < Main.rockLayer && tileY > Main.worldSurface)
                            {
                                if (Main.rand.Next(5) == 0)
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, NPC.SpawnNPC_GetGemSquirrelToSpawn());
                                }
                            }
                            else if (flag33)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Utils.SelectRandom(Main.rand, new short[2] { 299, 538 }));
                            }
                        }
                        else if (tileY >= Main.rockLayer && tileY <= Main.UnderworldLayer)
                        {
                            if (Main.rand.Next(5) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, NPC.SpawnNPC_GetGemSquirrelToSpawn());
                            }
                        }
                        else if (flag33)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Utils.SelectRandom(Main.rand, new short[2] { 299, 538 }));
                        }
                    }
                    else if (Main.remixWorld)
                    {
                        if (tileY < Main.rockLayer && tileY > Main.worldSurface)
                        {
                            if (tileY >= Main.rockLayer && tileY <= Main.UnderworldLayer)
                            {
                                if (Main.rand.Next(5) == 0)
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, NPC.SpawnNPC_GetGemBunnyToSpawn());
                                }
                            }
                            else
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 46);
                            }
                        }
                    }
                    else if (tileY >= Main.rockLayer && tileY <= Main.UnderworldLayer)
                    {
                        if (Main.rand.Next(5) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, NPC.SpawnNPC_GetGemBunnyToSpawn());
                        }
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 46);
                    }
                }
            }
            else if (player.ZoneDungeon)
            {
                int num88 = 0;
                ushort wall = Main.tile[tileX, tileY].wall;
                ushort wall2 = Main.tile[tileX, tileY - 1].wall;
                if (wall == 94 || wall == 96 || wall == 98 || wall2 == 94 || wall2 == 96 || wall2 == 98)
                {
                    num88 = 1;
                }
                if (wall == 95 || wall == 97 || wall == 99 || wall2 == 95 || wall2 == 97 || wall2 == 99)
                {
                    num88 = 2;
                }
                if (player.RollLuck(7) == 0)
                {
                    num88 = Main.rand.Next(3);
                }
                bool flag34 = !NPC.downedBoss3;
                if (Main.drunkWorld && player.position.Y / 16f < Main.dungeonY + 40)
                {
                    flag34 = false;
                }
                if (flag34)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 68);
                }
                else if (NPC.downedBoss3 && !NPC.savedMech && Main.rand.Next(5) == 0 && !flag7 && !NPC.AnyNPCs(123) && tileY > (Main.worldSurface * 4.0 + Main.rockLayer) / 5.0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 123);
                }
                else if (flag14 && Main.rand.Next(30) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 287);
                }
                else if (flag14 && num88 == 0 && Main.rand.Next(15) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 293);
                }
                else if (flag14 && num88 == 1 && Main.rand.Next(15) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 291);
                }
                else if (flag14 && num88 == 2 && Main.rand.Next(15) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 292);
                }
                else if (flag14 && !NPC.AnyNPCs(290) && num88 == 0 && Main.rand.Next(35) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 290);
                }
                else if (flag14 && (num88 == 1 || num88 == 2) && Main.rand.Next(30) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 289);
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
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, num89);
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
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, num90 + Main.rand.Next(4));
                }
                else if (player.RollLuck(35) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 71);
                }
                else if (num88 == 1 && Main.rand.Next(3) == 0 && !NPC.NearSpikeBall(tileX, tileY))
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 70);
                }
                else if (num88 == 2 && Main.rand.Next(5) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 72);
                }
                else if (num88 == 0 && Main.rand.Next(7) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 34);
                }
                else if (Main.rand.Next(7) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 32);
                }
                else
                {
                    switch (Main.rand.Next(5))
                    {
                        case 0:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 294);
                            break;
                        case 1:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 295);
                            break;
                        case 2:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 296);
                            break;
                        default:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 31);
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
            else if (player.ZoneMeteor)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 23);
            }
            else if (DD2Event.Ongoing && player.ZoneOldOneArmy)
            {
                DD2Event.SpawnNPC(ref newNPC);
            }
            else if ((Main.remixWorld || tileY <= Main.worldSurface) && !Main.dayTime && Main.snowMoon)
            {
                int num91 = NPC.waveNumber;
                if (Main.rand.Next(30) == 0 && NPC.CountNPCS(341) < 4)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 341);
                }
                else if (num91 >= 20)
                {
                    int num92 = Main.rand.Next(3);
                    if (!(num5 >= num4 * num6))
                    {
                        newNPC = num92 switch
                        {
                            0 => NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 345),
                            1 => NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 346),
                            _ => NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 344),
                        };
                    }
                }
                else if (num91 >= 19)
                {
                    newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(345) < 4) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 345) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(346) < 5) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 346) : ((Main.rand.Next(10) != 0 || NPC.CountNPCS(344) >= 7) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 343) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 344))));
                }
                else if (num91 >= 18)
                {
                    newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(345) < 3) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 345) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(346) < 4) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 346) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(344) < 6) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 344) : ((Main.rand.Next(3) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 348) : ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 343) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 351))))));
                }
                else if (num91 >= 17)
                {
                    newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(345) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 345) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(346) < 3) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 346) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(344) < 5) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 344) : ((Main.rand.Next(4) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 347) : ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 343) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 351))))));
                }
                else if (num91 >= 16)
                {
                    newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(345) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 345) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(346) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 346) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(344) < 4) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 344) : ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 343) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 352)))));
                }
                else if (num91 >= 15)
                {
                    newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(345)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 345) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(346) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 346) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(344) < 3) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 344) : ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 343) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 347)))));
                }
                else
                {
                    switch (num91)
                    {
                        case 14:
                            if (Main.rand.Next(10) == 0 && !NPC.AnyNPCs(345))
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 345);
                            }
                            else if (Main.rand.Next(10) == 0 && !NPC.AnyNPCs(346))
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 346);
                            }
                            else if (Main.rand.Next(10) == 0 && !NPC.AnyNPCs(344))
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 344);
                            }
                            else if (Main.rand.Next(3) == 0)
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 343);
                            }
                            break;
                        case 13:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(345)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 345) : ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(346)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 346) : ((Main.rand.Next(3) != 0) ? ((Main.rand.Next(6) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 347) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 342)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 343)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 352))));
                            break;
                        case 12:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(345)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 345) : ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(344)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 344) : ((Main.rand.Next(8) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(338, 341)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 342)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 343))));
                            break;
                        case 11:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(345)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 345) : ((Main.rand.Next(6) != 0) ? ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(338, 341)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 342)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 352)));
                            break;
                        case 10:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(346)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 346) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(344) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 344) : ((Main.rand.Next(6) != 0) ? ((Main.rand.Next(3) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(338, 341)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 347)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 348)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 351))));
                            break;
                        case 9:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(346)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 346) : ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(344)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 344) : ((Main.rand.Next(2) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 342) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 347)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 348))));
                            break;
                        case 8:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(346)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 346) : ((Main.rand.Next(8) != 0) ? ((Main.rand.Next(3) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 350) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 347)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 348)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 351)));
                            break;
                        case 7:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(346)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 346) : ((Main.rand.Next(3) != 0) ? ((Main.rand.Next(4) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(338, 341)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 350)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 342)));
                            break;
                        case 6:
                            newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(344) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 344) : ((Main.rand.Next(4) != 0) ? ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 350) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 348)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 347)));
                            break;
                        case 5:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(344)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 344) : ((Main.rand.Next(4) != 0) ? ((Main.rand.Next(8) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(338, 341)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 348)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 350)));
                            break;
                        case 4:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(344)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 344) : ((Main.rand.Next(4) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(338, 341)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 342)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 350)));
                            break;
                        case 3:
                            newNPC = ((Main.rand.Next(8) != 0) ? ((Main.rand.Next(4) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(338, 341)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 342)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 350)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 348));
                            break;
                        case 2:
                            newNPC = ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(338, 341)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 350));
                            break;
                        default:
                            newNPC = ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(338, 341)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 342));
                            break;
                    }
                }
            }
            else if ((Main.remixWorld || tileY <= Main.worldSurface) && !Main.dayTime && Main.pumpkinMoon)
            {
                int num93 = NPC.waveNumber;
                if (NPC.waveNumber >= 20)
                {
                    if (!(num5 >= num4 * num6))
                    {
                        if (Main.rand.Next(2) == 0 && NPC.CountNPCS(327) < 2)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 327);
                        }
                        else if (Main.rand.Next(3) != 0 && NPC.CountNPCS(325) < 2)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 325);
                        }
                        else if (NPC.CountNPCS(315) < 3)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 315);
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
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 327);
                            }
                            else if (Main.rand.Next(5) == 0 && NPC.CountNPCS(325) < 2)
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 325);
                            }
                            else if (!(num5 >= num4 * num6) && NPC.CountNPCS(315) < 5)
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 315);
                            }
                            break;
                        case 18:
                            if (Main.rand.Next(7) == 0 && NPC.CountNPCS(327) < 2)
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 327);
                            }
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(325) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 325) : ((Main.rand.Next(7) != 0 || NPC.CountNPCS(315) >= 3) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 330) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 315)));
                            break;
                        case 17:
                            if (Main.rand.Next(7) == 0 && NPC.CountNPCS(327) < 2)
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 327);
                            }
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(325) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 325) : ((Main.rand.Next(7) == 0 && NPC.CountNPCS(315) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 315) : ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 329) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 330))));
                            break;
                        case 16:
                            newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(327) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 327) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(315) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 315) : ((Main.rand.Next(6) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 326) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 329)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 330))));
                            break;
                        case 15:
                            if (Main.rand.Next(10) == 0 && !NPC.AnyNPCs(327))
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 327);
                            }
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(325) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 325) : ((Main.rand.Next(5) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(305, 315)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 326)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 330)));
                            break;
                        case 14:
                            if (Main.rand.Next(10) == 0 && !NPC.AnyNPCs(327))
                            {
                                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 327);
                            }
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(325) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 325) : ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(315)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 315) : ((Main.rand.Next(10) != 0) ? ((Main.rand.Next(7) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(305, 315)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 326)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 329)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 330))));
                            break;
                        case 13:
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(325) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 325) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(315) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 315) : ((Main.rand.Next(6) != 0) ? ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 326) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 329)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 330))));
                            break;
                        case 12:
                            newNPC = ((Main.rand.Next(5) != 0 || NPC.AnyNPCs(327)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 330) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 327));
                            break;
                        case 11:
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(325) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 325) : ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 326) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 330)));
                            break;
                        case 10:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(327)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 327) : ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(305, 315)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 329)));
                            break;
                        case 9:
                            newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(325) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 325) : ((Main.rand.Next(8) != 0) ? ((Main.rand.Next(5) != 0) ? ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(305, 315)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 326)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 329)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 330)));
                            break;
                        case 8:
                            newNPC = ((Main.rand.Next(8) == 0 && NPC.CountNPCS(315) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 315) : ((Main.rand.Next(4) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 329) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 330)));
                            break;
                        case 7:
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(325) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 325) : ((Main.rand.Next(4) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 329) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 330)));
                            break;
                        case 6:
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(325) < 2) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 325) : ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(305, 315)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 326)));
                            break;
                        case 5:
                            newNPC = ((Main.rand.Next(10) != 0 || NPC.AnyNPCs(315)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 329) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 315));
                            break;
                        case 4:
                            newNPC = ((Main.rand.Next(8) == 0 && !NPC.AnyNPCs(325)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 330) : ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(305, 315)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 326)));
                            break;
                        case 3:
                            newNPC = ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 326) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 329));
                            break;
                        case 2:
                            newNPC = ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(305, 315)) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 326));
                            break;
                        default:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(305, 315));
                            break;
                    }
                }
            }
            else if ((tileY <= Main.worldSurface || (Main.remixWorld && tileY > Main.rockLayer)) && Main.dayTime && Main.eclipse)
            {
                bool flag35 = false;
                if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                {
                    flag35 = true;
                }
                newNPC = ((NPC.downedPlantBoss && Main.rand.Next(80) == 0 && !NPC.AnyNPCs(477))
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 477)
                    : ((Main.rand.Next(50) == 0 && !NPC.AnyNPCs(251))
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 251)
                    : ((NPC.downedPlantBoss && Main.rand.Next(5) == 0 && !NPC.AnyNPCs(NPCID.Psycho))
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, NPCID.Psycho)
                    : ((NPC.downedPlantBoss && Main.rand.Next(20) == 0 && !NPC.AnyNPCs(463))
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 463)
                    : ((NPC.downedPlantBoss && Main.rand.Next(20) == 0 && NPC.CountNPCS(467) < 2)
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 467)
                    : ((Main.rand.Next(15) == 0)
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 159)
                    : ((flag35 && Main.rand.Next(13) == 0)
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 253)
                    : ((Main.rand.Next(8) == 0)
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 469)
                    : ((NPC.downedPlantBoss && Main.rand.Next(7) == 0)
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 468)
                    : ((NPC.downedPlantBoss && Main.rand.Next(5) == 0)
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 460)
                    : ((Main.rand.Next(4) == 0)
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 162)
                    : ((Main.rand.Next(3) == 0)
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 461)
                    : ((Main.rand.Next(2) != 0)
                    ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 166)
                    : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 462))))))))))))));
            }
            else if (NPC.SpawnNPC_CheckToSpawnUndergroundFairy(tileX, tileY, k))
            {
                int num94 = Main.rand.Next(583, 586);
                if (Main.tenthAnniversaryWorld && !Main.getGoodWorld && Main.rand.Next(4) != 0)
                {
                    num94 = 583;
                }
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, num94);
                Main.npc[newNPC].ai[2] = 2f;
                Main.npc[newNPC].TargetClosest();
                Main.npc[newNPC].ai[3] = 0f;
            }
            else if (!Main.remixWorld && !flag7 && (!Main.dayTime || Main.tile[tileX, tileY].wall > 0) && Main.tile[num8, num9].wall == 244 && !Main.eclipse && !Main.bloodMoon && player.RollLuck(30) == 0 && NPC.CountNPCS(624) <= Main.rand.Next(3))
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 624);
            }
            else if (!player.ZoneCorrupt && !player.ZoneCrimson && !flag7 && !Main.eclipse && !Main.bloodMoon && player.RollLuck(range) == 0 && ((!Main.remixWorld && tileY >= Main.worldSurface * 0.800000011920929 && tileY < Main.worldSurface * 1.100000023841858) || (Main.remixWorld && tileY > Main.rockLayer && tileY < Main.maxTilesY - 350)) && NPC.CountNPCS(624) <= Main.rand.Next(3) && (!Main.dayTime || Main.tile[tileX, tileY].wall > 0) && (Main.tile[tileX, tileY].wall == 63 || Main.tile[tileX, tileY].wall == 2 || Main.tile[tileX, tileY].wall == 196 || Main.tile[tileX, tileY].wall == 197 || Main.tile[tileX, tileY].wall == 198 || Main.tile[tileX, tileY].wall == 199))
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 624);
            }
            else if (Main.hardMode && num3 == 70 && flag7)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 256);
            }
            else if (num3 == 70 && tileY <= Main.worldSurface && Main.rand.Next(3) != 0)
            {
                if ((!Main.hardMode && Main.rand.Next(6) == 0) || Main.rand.Next(12) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 360);
                }
                else if (Main.rand.Next(3) != 0)
                {
                    newNPC = ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 255) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 254));
                }
                else if (Main.rand.Next(4) != 0)
                {
                    newNPC = ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 258) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 257));
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, (Main.hardMode && Main.rand.Next(3) != 0) ? 260 : 259);
                    Main.npc[newNPC].ai[0] = tileX;
                    Main.npc[newNPC].ai[1] = tileY;
                    Main.npc[newNPC].netUpdate = true;
                }
            }
            else if (num3 == 70 && Main.hardMode && tileY >= Main.worldSurface && Main.rand.Next(3) != 0 && (!Main.remixWorld || Main.getGoodWorld || tileY < Main.maxTilesY - 360))
            {
                if (Main.hardMode && player.RollLuck(5) == 0)
                //if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && player.RollLuck(5) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, NPCID.TruffleWorm);
                }
                else if ((!Main.hardMode && Main.rand.Next(4) == 0) || Main.rand.Next(8) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, NPCID.GlowingSnail);
                }
                else if (Main.rand.Next(4) != 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, (Main.rand.Next(2) != 0) ? NPCID.MushiLadybug : NPCID.AnomuraFungus);
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, (Main.hardMode && Main.rand.Next(3) != 0) ? NPCID.GiantFungiBulb : NPCID.FungiBulb);
                    Main.npc[newNPC].ai[0] = tileX;
                    Main.npc[newNPC].ai[1] = tileY;
                    Main.npc[newNPC].netUpdate = true;
                }
            }
            else if (player.ZoneCorrupt && Main.rand.Next(maxValue) == 0 && !flag5)
            {
                newNPC = ((!Main.hardMode || Main.rand.Next(4) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 7, 1) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 98, 1));
            }
            else if (Main.remixWorld && !Main.hardMode && tileY > Main.worldSurface && player.RollLuck(100) == 0)
            {
                newNPC = ((!player.ZoneSnow) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 85) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 629));
            }
            else if (Main.hardMode && tileY > Main.worldSurface && player.RollLuck(Main.tenthAnniversaryWorld ? 25 : 75) == 0)
            {
                newNPC = ((Main.rand.Next(2) == 0 && player.ZoneCorrupt && !NPC.AnyNPCs(473)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 473) : ((Main.rand.Next(2) == 0 && player.ZoneCrimson && !NPC.AnyNPCs(474)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 474) : ((Main.rand.Next(2) == 0 && player.ZoneHallow && !NPC.AnyNPCs(475)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 475) : ((Main.tenthAnniversaryWorld && Main.rand.Next(2) == 0 && player.ZoneJungle && !NPC.AnyNPCs(476)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 476) : ((!player.ZoneSnow) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 85) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 629))))));
            }
            else if (Main.hardMode && Main.tile[tileX, tileY].wall == 2 && Main.rand.Next(20) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 85);
            }
            else if (Main.hardMode && tileY <= Main.worldSurface && !Main.dayTime && (Main.rand.Next(20) == 0 || (Main.rand.Next(5) == 0 && Main.moonPhase == 4)))
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 82);
            }
            else if (Main.hardMode && Main.halloween && tileY <= Main.worldSurface && !Main.dayTime && Main.rand.Next(10) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 304);
            }
            else if (num45 == 60 && player.RollLuck(500) == 0 && !Main.dayTime)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 52);
            }
            else if (num45 == 60 && tileY > Main.worldSurface && Main.rand.Next(60) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 219);
            }
            else if (tileY > Main.worldSurface && tileY < Main.maxTilesY - 210 && !player.ZoneSnow && !player.ZoneCrimson && !player.ZoneCorrupt && !player.ZoneJungle && !player.ZoneHallow && Main.rand.Next(8) == 0)
            {
                if (player.RollLuck(NPC.goldCritterChance) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 448);
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 357);
                }
            }
            else if (tileY > Main.worldSurface && tileY < Main.maxTilesY - 210 && !player.ZoneSnow && !player.ZoneCrimson && !player.ZoneCorrupt && !player.ZoneJungle && !player.ZoneHallow && Main.rand.Next(13) == 0)
            {
                if (player.RollLuck(NPC.goldCritterChance) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 447);
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 300);
                }
            }
            else if (tileY > Main.worldSurface && tileY < (Main.rockLayer + Main.maxTilesY) / 2.0 && !player.ZoneSnow && !player.ZoneCrimson && !player.ZoneCorrupt && !player.ZoneHallow && Main.rand.Next(13) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 359);
            }
            else if (flag20 && player.ZoneJungle && !player.ZoneCrimson && !player.ZoneCorrupt && Main.rand.Next(7) == 0)
            {
                if (Main.dayTime && Main.time < 43200.00064373016 && Main.rand.Next(3) != 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 671 + Main.rand.Next(5));
                }
                else
                {
                    NPC.SpawnNPC_SpawnFrog(tileX, tileY, k);
                }
            }
            else if (num45 == 225 && Main.rand.Next(2) == 0)
            {
                if (Main.hardMode && Main.rand.Next(4) != 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 176);
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
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 231);
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
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 232);
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
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 233);
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
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 234);
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
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 235);
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
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 42);
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
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 152);
                }
                else if (flag20 && Main.dayTime && Main.rand.Next(4) != 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 177);
                }
                else if (tileY > Main.worldSurface && Main.rand.Next(100) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 205);
                }
                else if (tileY > Main.worldSurface && Main.rand.Next(5) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 236);
                }
                else if (tileY > Main.worldSurface && Main.rand.Next(4) != 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 176);
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
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 175);
                    Main.npc[newNPC].ai[0] = tileX;
                    Main.npc[newNPC].ai[1] = tileY;
                    Main.npc[newNPC].netUpdate = true;
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 153);
                }
            }
            else if (((num45 == 226 || num45 == 232) && flag4) || (Main.remixWorld && flag4))
            {
                newNPC = ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 198) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 226));
            }
            else if (num46 == 86 && Main.rand.Next(8) != 0)
            {
                switch (Main.rand.Next(8))
                {
                    case 0:
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 231);
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
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 232);
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
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 233);
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
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 234);
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
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 235);
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
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 42);
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
            else if (num45 == 60 && ((!Main.remixWorld && tileY > (Main.worldSurface + Main.rockLayer) / 2.0) || (Main.remixWorld && (tileY < Main.rockLayer || Main.rand.Next(2) == 0))))
            {
                if (Main.rand.Next(4) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 204);
                }
                else if (Main.rand.Next(4) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 43);
                    Main.npc[newNPC].ai[0] = tileX;
                    Main.npc[newNPC].ai[1] = tileY;
                    Main.npc[newNPC].netUpdate = true;
                }
                else
                {
                    switch (Main.rand.Next(8))
                    {
                        case 0:
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 231);
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
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 232);
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
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 233);
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
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 234);
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
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 235);
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
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 42);
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
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 51);
            }
            else if (num45 == 60 && Main.rand.Next(8) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 56);
                Main.npc[newNPC].ai[0] = tileX;
                Main.npc[newNPC].ai[1] = tileY;
                Main.npc[newNPC].netUpdate = true;
            }
            else if (Sandstorm.Happening && player.ZoneSandstorm && TileID.Sets.Conversion.Sand[num45] && NPC.Spawning_SandstoneCheck(tileX, tileY))
            {
                if (!NPC.downedBoss1 && !Main.hardMode)
                {
                    newNPC = ((Main.rand.Next(2) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 546) : ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 69) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 61)));
                }
                else if (Main.hardMode && Main.rand.Next(20) == 0 && !NPC.AnyNPCs(541))
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 541);
                }
                else if (Main.hardMode && !flag5 && Main.rand.Next(3) == 0 && NPC.CountNPCS(510) < 4)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, (tileY + 10) * 16, 510);
                }
                else if (!Main.hardMode || flag5 || Main.rand.Next(2) != 0)
                {
                    newNPC = ((Main.hardMode && num45 == 53 && Main.rand.Next(3) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 78) : ((Main.hardMode && num45 == 112 && Main.rand.Next(3) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 79) : ((Main.hardMode && num45 == 234 && Main.rand.Next(3) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 630) : ((Main.hardMode && num45 == 116 && Main.rand.Next(3) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 80) : ((Main.rand.Next(2) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 546) : ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 581) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 580)))))));
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
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, num95);
                }
            }
            else if (Main.hardMode && num45 == 53 && Main.rand.Next(3) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 78);
            }
            else if (Main.hardMode && num45 == 112 && Main.rand.Next(2) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 79);
            }
            else if (Main.hardMode && num45 == 234 && Main.rand.Next(2) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 630);
            }
            else if (Main.hardMode && num45 == 116 && Main.rand.Next(2) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 80);
            }
            else if (Main.hardMode && !flag7 && flag17 && (num45 == 116 || num45 == 117 || num45 == 109 || num45 == 164))
            {
                if (NPC.downedPlantBoss && (Main.remixWorld || (!Main.dayTime && Main.time < 16200.0)) && flag20 && player.RollLuck(10) == 0 && !NPC.AnyNPCs(661))
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 661);
                }
                else if (!flag24 || NPC.AnyNPCs(244) || Main.rand.Next(12) != 0)
                {
                    newNPC = ((!Main.dayTime && Main.rand.Next(2) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 122) : ((Main.rand.Next(10) != 0 && (!player.ZoneWaterCandle || Main.rand.Next(10) != 0)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 75) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 86)));
                }
                else
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 244);
                }
            }
            else if (!flag5 && Main.hardMode && Main.rand.Next(50) == 0 && !flag7 && flag21 && (num45 == 116 || num45 == 117 || num45 == 109 || num45 == 164))
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 84);
            }
            else if ((num45 == 204 && player.ZoneCrimson) || num45 == 199 || num45 == 200 || num45 == 203 || num45 == 234 || num45 == 662)
            {
                bool flag36 = tileY >= Main.rockLayer;
                if (Main.remixWorld)
                {
                    flag36 = tileY <= Main.rockLayer;
                }
                if (Main.hardMode && flag36 && Main.rand.Next(40) == 0 && !flag5)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 179);
                }
                else if (Main.hardMode && flag36 && Main.rand.Next(5) == 0 && !flag5)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 182);
                }
                else if (Main.hardMode && flag36 && Main.rand.Next(2) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 268);
                }
                else if (Main.hardMode && Main.rand.Next(3) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 183);
                    if (Main.rand.Next(3) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-24);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-25);
                    }
                }
                else if (Main.hardMode && (Main.rand.Next(2) == 0 || (tileY > Main.worldSurface && !Main.remixWorld)))
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 174);
                }
                else if ((Main.tile[tileX, tileY].wall > 0 && Main.rand.Next(4) != 0) || Main.rand.Next(8) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 239);
                }
                else if (Main.rand.Next(2) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 181);
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 173);
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
            else if ((num45 == 22 && player.ZoneCorrupt) || num45 == 23 || num45 == 25 || num45 == 112 || num45 == 163 || num45 == 661)
            {
                bool flag37 = tileY >= Main.rockLayer;
                if (Main.remixWorld)
                {
                    flag37 = tileY <= Main.rockLayer;
                }
                if (Main.hardMode && flag37 && Main.rand.Next(40) == 0 && !flag5)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 83);
                }
                else if (Main.hardMode && flag37 && Main.rand.Next(3) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 101);
                    Main.npc[newNPC].ai[0] = tileX;
                    Main.npc[newNPC].ai[1] = tileY;
                    Main.npc[newNPC].netUpdate = true;
                }
                else if (Main.hardMode && Main.rand.Next(3) == 0)
                {
                    newNPC = ((Main.rand.Next(3) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 81) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 121));
                }
                else if (Main.hardMode && (Main.rand.Next(2) == 0 || flag37))
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 94);
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 6);
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
                bool flag38 = Math.Abs(tileX - Main.maxTilesX / 2) / (float)(Main.maxTilesX / 2) > 0.33f;
                if (flag38 && NPC.AnyDanger())
                {
                    flag38 = false;
                }
                if (player.ZoneGraveyard && !flag7 && (num3 == 2 || num3 == 477) && Main.rand.Next(10) == 0)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 606);
                    }
                    else
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 610);
                    }
                }
                else if (player.ZoneSnow && Main.hardMode && flag24 && !NPC.AnyNPCs(243) && player.RollLuck(20) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 243);
                }
                else if (!player.ZoneSnow && Main.hardMode && flag24 && NPC.CountNPCS(250) < 2 && Main.rand.Next(10) == 0)
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 250);
                }
                else if (flag38 && Main.hardMode && NPC.downedGolemBoss && ((!NPC.downedMartians && Main.rand.Next(100) == 0) || Main.rand.Next(400) == 0) && !NPC.AnyNPCs(399))
                {
                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 399);
                }
                else if (!player.ZoneGraveyard && Main.dayTime)
                {
                    int num96 = Math.Abs(tileX - Main.spawnTileX);
                    if (!flag7 && num96 < Main.maxTilesX / 2 && Main.rand.Next(15) == 0 && (num45 == 2 || num45 == 477 || num45 == 109 || num45 == 492 || num45 == 147 || num45 == 161))
                    {
                        if (num45 == 147 || num45 == 161)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 148);
                            }
                            else
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 149);
                            }
                        }
                        else if (!tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(NPC.stinkBugChance) == 0 && flag20)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 669);
                            if (Main.rand.Next(4) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX - 16, spawnY, 669);
                            }
                            if (Main.rand.Next(4) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + 16, spawnY, 669);
                            }
                        }
                        else if (!tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(NPC.butterflyChance) == 0 && flag20)
                        {
                            if (player.RollLuck(NPC.goldCritterChance) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 444);
                            }
                            else
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 356);
                            }
                            if (Main.rand.Next(4) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX - 16, spawnY, 356);
                            }
                            if (Main.rand.Next(4) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + 16, spawnY, 356);
                            }
                        }
                        else if (tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(NPC.butterflyChance / 2) == 0 && flag20)
                        {
                            if (player.RollLuck(NPC.goldCritterChance) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 605);
                            }
                            else
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 604);
                            }
                            if (Main.rand.Next(3) != 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 604);
                            }
                            if (Main.rand.Next(2) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 604);
                            }
                            if (Main.rand.Next(3) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 604);
                            }
                            if (Main.rand.Next(4) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 604);
                            }
                        }
                        else if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 443);
                        }
                        else if (player.RollLuck(NPC.goldCritterChance) == 0 && tileY <= Main.worldSurface)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 539);
                        }
                        else if (Main.halloween && Main.rand.Next(3) != 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 303);
                        }
                        else if (Main.xMas && Main.rand.Next(3) != 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 337);
                        }
                        else if (BirthdayParty.PartyIsUp && Main.rand.Next(3) != 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 540);
                        }
                        else if (Main.rand.Next(3) == 0 && tileY <= Main.worldSurface)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Utils.SelectRandom(Main.rand, new short[2] { 299, 538 }));
                        }
                        else
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 46);
                        }
                    }
                    else if (!flag7 && tileX > WorldGen.beachDistance && tileX < Main.maxTilesX - WorldGen.beachDistance && Main.rand.Next(12) == 0 && num45 == 53)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(366, 368));
                    }
                    else if ((num45 == 2 || num45 == 477 || num45 == 53) && !tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(3) != 0 && (tileY <= Main.worldSurface || Main.remixWorld) && NPC.FindCattailTop(tileX, tileY, out cattailX, out cattailY))
                    {
                        if (player.RollLuck(NPC.goldCritterChance) == 0)
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
                    else if (!flag7 && num96 < Main.maxTilesX / 3 && Main.dayTime && Main.time < 18000.0 && (num45 == 2 || num45 == 477 || num45 == 109 || num45 == 492) && Main.rand.Next(4) == 0 && tileY <= Main.worldSurface && NPC.CountNPCS(74) + NPC.CountNPCS(297) + NPC.CountNPCS(298) < 6)
                    {
                        int num97 = Main.rand.Next(4);
                        if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 442);
                        }
                        else
                        {
                            switch (num97)
                            {
                                case 0:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 297);
                                    break;
                                case 1:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 298);
                                    break;
                                default:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 74);
                                    break;
                            }
                        }
                    }
                    else if (!flag7 && num96 < Main.maxTilesX / 3 && Main.rand.Next(15) == 0 && (num45 == 2 || num45 == 477 || num45 == 109 || num45 == 492 || num45 == 147))
                    {
                        int num98 = Main.rand.Next(4);
                        if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 442);
                        }
                        else
                        {
                            switch (num98)
                            {
                                case 0:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 297);
                                    break;
                                case 1:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 298);
                                    break;
                                default:
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 74);
                                    break;
                            }
                        }
                    }
                    else if (!flag7 && num96 > Main.maxTilesX / 3 && num45 == 2 && Main.rand.Next(300) == 0 && !NPC.AnyNPCs(50))
                    {
                        NPC.SpawnOnPlayer(k, 50);
                    }
                    else if (!flag15 && num45 == 53 && (tileX < WorldGen.beachDistance || tileX > Main.maxTilesX - WorldGen.beachDistance))
                    {
                        if (!flag7 && Main.rand.Next(10) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 602);
                        }
                        else if (flag7)
                        {
                            int num99 = -1;
                            int num100 = -1;
                            if (tileY < Main.worldSurface && tileY > 50)
                            {
                                for (int num101 = tileY - 1; num101 > tileY - 50; num101--)
                                {
                                    if (Main.tile[tileX, num101].liquid == 0 && !WorldGen.SolidTile(tileX, num101) && !WorldGen.SolidTile(tileX, num101 + 1) && !WorldGen.SolidTile(tileX, num101 + 2))
                                    {
                                        num99 = num101 + 2;
                                        if (!WorldGen.SolidTile(tileX, num99 + 1) && !WorldGen.SolidTile(tileX, num99 + 2))
                                        {
                                            num100 = num99 + 2;
                                        }
                                        break;
                                    }
                                }
                                if (num99 > tileY)
                                {
                                    num99 = tileY;
                                }
                                if (num100 > tileY)
                                {
                                    num100 = tileY;
                                }
                            }
                            if (Main.rand.Next(10) == 0)
                            {
                                int num102 = Main.rand.Next(3);
                                if (num102 == 0 && num99 > 0)
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num99 * 16, 625);
                                }
                                else if (num102 == 1 && num100 > 0)
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num100 * 16, 615);
                                }
                                else if (num102 == 2)
                                {
                                    int num103 = tileY;
                                    if (num100 > 0)
                                    {
                                        num103 = num100;
                                    }
                                    if (player.RollLuck(NPC.goldCritterChance) == 0)
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num103 * 16, 627);
                                    }
                                    else
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num103 * 16, 626);
                                    }
                                }
                            }
                        }
                    }
                    else if (!flag7 && num45 == 53 && Main.rand.Next(5) == 0 && NPC.Spawning_SandstoneCheck(tileX, tileY) && !flag7)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 69);
                    }
                    else if (num45 == 53 && !flag7)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 61);
                    }
                    else if (!flag7 && (num96 > Main.maxTilesX / 3 || Main.remixWorld) && (Main.rand.Next(15) == 0 || (!NPC.downedGoblins && WorldGen.shadowOrbSmashed && Main.rand.Next(7) == 0)))
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 73);
                    }
                    else if (Main.raining && Main.rand.Next(4) == 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 224);
                    }
                    else if (!flag7 && Main.raining && Main.rand.Next(2) == 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 225);
                    }
                    else if (!flag7 && num46 == 0 && isItAHappyWindyDay && flag19 && Main.rand.Next(3) != 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 594);
                    }
                    else if (!flag7 && num46 == 0 && (num3 == 2 || num3 == 477) && isItAHappyWindyDay && flag19 && Main.rand.Next(10) != 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 628);
                    }
                    else if (!flag7)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 1);
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
                    if (!player.ZoneGraveyard && !tooWindyForButterflies && (num45 == 2 || num45 == 477 || num45 == 109 || num45 == 492) && !Main.raining && Main.rand.Next(NPC.fireFlyChance) == 0 && tileY <= Main.worldSurface)
                    {
                        int num104 = 355;
                        if (num45 == 109)
                        {
                            num104 = 358;
                        }
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, num104);
                        if (Main.rand.Next(NPC.fireFlyMultiple) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX - 16, spawnY, num104);
                        }
                        if (Main.rand.Next(NPC.fireFlyMultiple) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + 16, spawnY, num104);
                        }
                        if (Main.rand.Next(NPC.fireFlyMultiple) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY - 16, num104);
                        }
                        if (Main.rand.Next(NPC.fireFlyMultiple) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY + 16, num104);
                        }
                    }
                    else if ((Main.halloween || player.ZoneGraveyard) && Main.rand.Next(12) == 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 301);
                    }
                    else if (player.ZoneGraveyard && Main.rand.Next(30) == 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 316);
                    }
                    else if (player.ZoneGraveyard && Main.hardMode && tileY <= Main.worldSurface && Main.rand.Next(10) == 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 304);
                    }
                    else if (Main.rand.Next(6) == 0 || (Main.moonPhase == 4 && Main.rand.Next(2) == 0))
                    {
                        if (Main.hardMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 133);
                        }
                        else if (Main.halloween && Main.rand.Next(2) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(317, 319));
                        }
                        else if (Main.rand.Next(2) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 2);
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
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 190);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Main.npc[newNPC].SetDefaults(-38);
                                    }
                                    break;
                                case 1:
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 191);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Main.npc[newNPC].SetDefaults(-39);
                                    }
                                    break;
                                case 2:
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 192);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Main.npc[newNPC].SetDefaults(-40);
                                    }
                                    break;
                                case 3:
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 193);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Main.npc[newNPC].SetDefaults(-41);
                                    }
                                    break;
                                case 4:
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 194);
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
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 109);
                    }
                    else if (Main.rand.Next(250) == 0 && (Main.bloodMoon || player.ZoneGraveyard))
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 53);
                    }
                    else if (Main.rand.Next(250) == 0 && (Main.bloodMoon || player.ZoneGraveyard))
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 536);
                    }
                    else if (!Main.dayTime && Main.moonPhase == 0 && Main.hardMode && Main.rand.Next(3) != 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 104);
                    }
                    else if (!Main.dayTime && Main.hardMode && Main.rand.Next(3) == 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 140);
                    }
                    else if (Main.bloodMoon && Main.rand.Next(5) < 2)
                    {
                        newNPC = ((Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 490) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 489));
                    }
                    else if (num3 == 147 || num3 == 161 || num3 == 163 || num3 == 164 || num3 == 162)
                    {
                        newNPC = ((!player.ZoneGraveyard && Main.hardMode && Main.rand.Next(4) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 169) : ((!player.ZoneGraveyard && Main.hardMode && Main.rand.Next(3) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 155) : ((!Main.expertMode || Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 161) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 431))));
                    }
                    else if (Main.raining && Main.rand.Next(2) == 0)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 223);
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
                        if (player.statLifeMax <= 100)
                        {
                            num106 = 5;
                            num106 -= Main.CurrentFrameFlags.ActivePlayersCount / 2;
                            if (num106 < 2)
                            {
                                num106 = 2;
                            }
                        }
                        if (player.ZoneGraveyard && Main.rand.Next(maxValue4) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 632);
                        }
                        else if (Main.rand.Next(num106) == 0)
                        {
                            newNPC = ((!Main.expertMode || Main.rand.Next(2) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 590) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 591));
                        }
                        else if (Main.halloween && Main.rand.Next(2) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(319, 322));
                        }
                        else if (Main.xMas && Main.rand.Next(2) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(331, 333));
                        }
                        else if (num105 == 0 && Main.expertMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 430);
                        }
                        else if (num105 == 2 && Main.expertMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 432);
                        }
                        else if (num105 == 3 && Main.expertMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 433);
                        }
                        else if (num105 == 4 && Main.expertMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 434);
                        }
                        else if (num105 == 5 && Main.expertMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 435);
                        }
                        else if (num105 == 6 && Main.expertMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 436);
                        }
                        else
                        {
                            switch (num105)
                            {
                                case 0:
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 3);
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
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 132);
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
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 186);
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
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 187);
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
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 188);
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
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 189);
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
                                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 200);
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
                    if (player.ZoneGraveyard)
                    {
                        Main.npc[newNPC].target = k;
                    }
                }
            }
            else if (flag17)
            {
                if (!flag5 && Main.rand.Next(50) == 0 && !player.ZoneSnow)
                {
                    newNPC = ((!Main.hardMode) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 10, 1) : ((Main.rand.Next(3) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 10, 1) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 95, 1)));
                }
                else if (Main.hardMode && Main.rand.Next(3) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 140);
                }
                else if (Main.hardMode && Main.rand.Next(4) != 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 141);
                }
                else if (Main.remixWorld)
                {
                    if (num3 == 147 || num3 == 161 || num3 == 163 || num3 == 164 || num3 == 162 || player.ZoneSnow)
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 147);
                    }
                    else
                    {
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 1);
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
                else if (num45 == 147 || num45 == 161 || player.ZoneSnow)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 147);
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 1);
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
            else if (tileY > Main.maxTilesY - 190)
            {
                newNPC = ((Main.remixWorld && tileX > Main.maxTilesX * 0.38 + 50.0 && tileX < Main.maxTilesX * 0.62) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 59) : ((Main.hardMode && !NPC.savedTaxCollector && Main.rand.Next(20) == 0 && !NPC.AnyNPCs(534)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 534) : ((Main.rand.Next(8) == 0) ? NPC.SpawnNPC_SpawnLavaBaitCritters(tileX, tileY) : ((Main.rand.Next(40) == 0 && !NPC.AnyNPCs(39)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 39, 1) : ((Main.rand.Next(14) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 24) : ((Main.rand.Next(7) == 0) ? ((Main.rand.Next(10) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 66) : ((!Main.hardMode || !NPC.downedMechBossAny || Main.rand.Next(5) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 62) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 156))) : ((Main.rand.Next(3) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 59) : ((!Main.hardMode || !NPC.downedMechBossAny || Main.rand.Next(5) == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 60) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 151)))))))));
            }
            else if (NPC.SpawnNPC_CheckToSpawnRockGolem(tileX, tileY, k, num45))
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 631);
            }
            else if (Main.rand.Next(60) == 0)
            {
                newNPC = ((!player.ZoneSnow) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 217) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 218));
            }
            else if ((num45 == 116 || num45 == 117 || num45 == 164) && Main.hardMode && !flag5 && Main.rand.Next(8) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 120);
            }
            else if ((num3 == 147 || num3 == 161 || num3 == 162 || num3 == 163 || num3 == 164 || num3 == 200) && !flag5 && Main.hardMode && player.ZoneCorrupt && Main.rand.Next(30) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 170);
            }
            else if ((num3 == 147 || num3 == 161 || num3 == 162 || num3 == 163 || num3 == 164 || num3 == 200) && !flag5 && Main.hardMode && player.ZoneHallow && Main.rand.Next(30) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 171);
            }
            else if ((num3 == 147 || num3 == 161 || num3 == 162 || num3 == 163 || num3 == 164 || num3 == 200) && !flag5 && Main.hardMode && player.ZoneCrimson && Main.rand.Next(30) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 180);
            }
            else if (Main.hardMode && player.ZoneSnow && Main.rand.Next(10) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 154);
            }
            else if (!flag5 && Main.rand.Next(100) == 0 && !player.ZoneHallow)
            {
                newNPC = (Main.hardMode ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 95, 1) : ((!player.ZoneSnow) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 10, 1) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 185)));
            }
            else if (player.ZoneSnow && Main.rand.Next(20) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 185);
            }
            else if ((!Main.hardMode && Main.rand.Next(10) == 0) || (Main.hardMode && Main.rand.Next(20) == 0))
            {
                if (player.ZoneSnow)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 184);
                }
                else if (Main.rand.Next(3) == 0)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 1);
                    Main.npc[newNPC].SetDefaults(-6);
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 16);
                }
            }
            else if (!Main.hardMode && Main.rand.Next(4) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 1);
                if (player.ZoneJungle)
                {
                    Main.npc[newNPC].SetDefaults(-10);
                }
                else if (player.ZoneSnow)
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
                newNPC = ((Main.hardMode && (player.ZoneHallow & (Main.rand.Next(2) == 0))) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 138) : (player.ZoneJungle ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 51) : ((player.ZoneGlowshroom && (num3 == 70 || num3 == 190)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 634) : ((Main.hardMode && player.ZoneHallow) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 137) : ((Main.hardMode && Main.rand.Next(6) > 0) ? ((Main.rand.Next(3) != 0 || (num3 != 147 && num3 != 161 && num3 != 162)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 93) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 150)) : ((num3 != 147 && num3 != 161 && num3 != 162) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 49) : ((!Main.hardMode) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 150) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 169))))))));
            }
            else if (Main.rand.Next(35) == 0 && NPC.CountNPCS(453) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 453);
            }
            else if (Main.rand.Next(80) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 195);
            }
            else if (Main.hardMode && (Main.remixWorld || tileY > (Main.rockLayer + Main.maxTilesY) / 2.0) && Main.rand.Next(200) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 172);
            }
            else if ((Main.remixWorld || tileY > (Main.rockLayer + Main.maxTilesY) / 2.0) && (Main.rand.Next(200) == 0 || (Main.rand.Next(50) == 0 && (player.armor[1].type == 4256 || (player.armor[1].type >= 1282 && player.armor[1].type <= 1287)) && player.armor[0].type != 238)))
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 45);
            }
            else if (flag10 && Main.rand.Next(4) != 0)
            {
                newNPC = ((Main.rand.Next(6) == 0 || NPC.AnyNPCs(480) || !Main.hardMode) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 481) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 480));
            }
            else if (flag9 && Main.rand.Next(5) != 0)
            {
                newNPC = ((Main.rand.Next(6) == 0 || NPC.AnyNPCs(483)) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 482) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 483));
            }
            else if (Main.hardMode && Main.rand.Next(10) != 0)
            {
                if (Main.rand.Next(2) != 0)
                {
                    newNPC = ((!player.ZoneSnow) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 110) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 206));
                }
                else if (player.ZoneSnow)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 197);
                }
                else
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 77);
                    if ((Main.remixWorld || tileY > (Main.rockLayer + Main.maxTilesY) / 2.0) && Main.rand.Next(5) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(-15);
                    }
                }
            }
            else if (!flag5 && (Main.halloween || player.ZoneGraveyard) && Main.rand.Next(30) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 316);
            }
            else if (Main.rand.Next(20) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 44);
            }
            else if (num3 == 147 || num3 == 161 || num3 == 162)
            {
                newNPC = ((Main.rand.Next(15) != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 167) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 185));
            }
            else if (player.ZoneSnow)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 185);
            }
            else if (Main.rand.Next(3) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, NPC.cavernMonsterType[Main.rand.Next(2), Main.rand.Next(3)]);
            }
            else if (player.ZoneGlowshroom && (num3 == 70 || num3 == 190))
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 635);
            }
            else if (Main.halloween && Main.rand.Next(2) == 0)
            {
                newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, Main.rand.Next(322, 325));
            }
            else if (Main.expertMode && Main.rand.Next(3) == 0)
            {
                int num107 = Main.rand.Next(4);
                newNPC = ((num107 == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 449) : ((num107 == 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 450) : ((num107 != 0) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 452) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 451))));
            }
            else
            {
                switch (Main.rand.Next(4))
                {
                    case 0:
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 21);
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
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 201);
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
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 202);
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
                        newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, spawnY, 203);
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
            if (Main.npc[newNPC].type == 1 && player.RollLuck(180) == 0)
            {
                Main.npc[newNPC].SetDefaults(-4);
            }
            if (Main.tenthAnniversaryWorld && Main.npc[newNPC].type == 1 && player.RollLuck(180) == 0)
            {
                Main.npc[newNPC].SetDefaults(667);
            }
            if (newNPC < 200)
            {
                NetMessage.SendData(23, -1, -1, null, newNPC);
            }
            break;
        }
    }
    public static void TransformElderSlime(int npcIndex)
    {
        if (!Main.npc.IndexInRange(npcIndex))
        {
            return;
        }
        var npc = Main.npc[npcIndex];
        if (npc.type != 685)
        {
            return;
        }
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
    public static void UpdateNPC(NPC npc, int i)
    {
        npc.whoAmI = i;
        if (!npc.active)
        {
            return;
        }
        npc.netOffset *= 0f;
        npc.UpdateAltTexture();
        if (npc.type == 368)
        {
            NPC.travelNPC = true;
        }
        npc.UpdateNPC_TeleportVisuals();
        npc.UpdateNPC_CritterSounds();
        npc.TrySyncingUniqueTownNPCData(i);
        if (npc.aiStyle == 7 && npc.position.Y > Main.bottomWorld - 640f + npc.height && (MainConfigInfo.StaticDisableKillNPCWhenNPCUnderWorldBottomXMasCheck || !Main.xMas))
        {
            npc.StrikeNPCNoInteraction(9999, 0f, 0);
            NetMessage.SendData(28, -1, -1, null, npc.whoAmI, 9999f);
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
        for (int j = 0; j < 256; j++)
        {
            if (npc.immune[j] > 0)
            {
                npc.immune[j]--;
            }
        }
        if (!npc.noGravity && !npc.noTileCollide)
        {
            int x = (int)(npc.position.X + npc.width / 2) / 16;
            int y = (int)(npc.position.Y + npc.height / 2) / 16;
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
        if (npc.velocity.X < 0.005 && npc.velocity.X > -0.005)
        {
            npc.velocity.X = 0f;
        }
        if (npc.type != 37 && (npc.friendly || NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[npc.type]))
        {
            if (npc.townNPC)
            {
                npc.CheckDrowning();
            }
            npc.CheckLifeRegen();
            if (npc.townNPC)
            {
                if (!MainConfigInfo.StaticDisableNPCStrikeTownNPC)
                {
                    npc.GetHurtByOtherNPCs(NPCID.Sets.AllNPCs);
                }
            }
            else
            {
                npc.GetHurtByOtherNPCs(NPCID.Sets.AllNPCs);
            }
        }
        if (!MainConfigInfo.StaticDisableQueenBeeAndBeeHurtOtherNPC && (NPC.npcsFoundForCheckActive[210] || NPC.npcsFoundForCheckActive[211]) && !NPCID.Sets.HurtingBees[npc.type])
        {
            npc.GetHurtByOtherNPCs(NPCID.Sets.HurtingBees);
        }
        if (!npc.noTileCollide)
        {
            npc.UpdateCollision();
        }
        else
        {
            npc.oldPosition = npc.position;
            npc.oldDirection = npc.direction;
            npc.position += npc.velocity;
            if (npc.onFire && npc.boss && Collision.WetCollision(npc.position, npc.width, npc.height))
            {
                for (int k = 0; k < NPC.maxBuffs; k++)
                {
                    if (npc.buffType[k] == 24)
                    {
                        npc.DelBuff(k);
                        break;
                    }
                }
            }
        }
        if (!npc.noTileCollide && npc.lifeMax > 1 && Collision.SwitchTiles(npc.position, npc.width, npc.height, npc.oldPosition, 2, npc) && (npc.type == 46 || npc.type == 148 || npc.type == 149 || npc.type == 303 || npc.type == 361 || npc.type == 362 || npc.type == 364 || npc.type == 366 || npc.type == 367 || (npc.type >= 442 && npc.type <= 448) || npc.type == 602 || npc.type == 608 || npc.type == 614 || npc.type == 687))
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
    public static void HitEffect(NPC self, int hitDirection = 0, double dmg = 10.0)
    {
        if (!self.active)
        {
            return;
        }
        if (self.daybreak && self.life <= 0)
        {
            for (int i = 0; i < 200; i++)
            {
                NPC nPC = Main.npc[i];
                if (nPC.active && !nPC.buffImmune[189] && self.Distance(nPC.Center) < 100f && !nPC.dontTakeDamage && nPC.lifeMax > 5 && !nPC.friendly && !nPC.townNPC)
                {
                    nPC.AddBuff(189, 300);
                }
            }
        }
        if (self.type == 686 && self.life <= 0)
        {
            Vector2 vector6 = self.Bottom + new Vector2(0f, 48f);
            Vector2 vector7 = self.velocity;
            self.Transform(680);
            self.position = vector6;
            self.velocity = vector7;
            self.netUpdate = true;
            if (!NPC.unlockedSlimePurpleSpawn)
            {
                NPC.unlockedSlimePurpleSpawn = true;
                NetMessage.SendData(7);
            }
        }
        if (self.type == 594 && self.life <= 0)
        {
            NPC nPC2 = self.AI_113_WindyBalloon_GetSlaveNPC();
            if (nPC2 != null)
            {
                nPC2.ai[0] = 0f;
                nPC2.position.Y -= 10f;
                nPC2.netUpdate = true;
            }
        }
        if (self.type == 551 && self.life > 0)
        {
            int num154 = (int)(self.life / (float)self.lifeMax * 100f);
            int num149 = (int)((self.life + dmg) / (double)(float)self.lifeMax * 100.0);
            if (num154 != num149)
            {
                DD2Event.CheckProgress(self.type);
            }
        }
        if (self.type == 488)
        {
            self.localAI[0] = (int)dmg;
            if (self.localAI[0] < 20f)
            {
                self.localAI[0] = 20f;
            }
            if (self.localAI[0] > 120f)
            {
                self.localAI[0] = 120f;
            }
            self.localAI[1] = hitDirection;
        }
        if (self.type == 426 && self.life <= 0)
        {
            int num155 = NPC.CountNPCS(428) + NPC.CountNPCS(427) + NPC.CountNPCS(426) * 3;
            int num120 = 20;
            if (num155 < num120)
            {
                for (int num121 = 0; num121 < 3; num121++)
                {
                    int num122 = NPC.NewNPC(self.GetSpawnSource_NPCHurt(), (int)self.Center.X, (int)self.Center.Y, 428, self.whoAmI);
                    Main.npc[num122].velocity = -Vector2.UnitY.RotatedByRandom(6.2831854820251465) * Main.rand.Next(3, 6) - Vector2.UnitY * 2f;
                    Main.npc[num122].netUpdate = true;
                }
            }
        }
        else if (self.type == 429 && self.life <= 0)
        {
            Point point = self.Center.ToTileCoordinates();
            Point point2 = Main.player[self.target].Center.ToTileCoordinates();
            Vector2 vector2 = Main.player[self.target].Center - self.Center;
            int num123 = 20;
            int num124 = 3;
            int num125 = 7;
            int num126 = 2;
            int num127 = 0;
            bool flag2 = false;
            if (vector2.Length() > 2000f)
            {
                flag2 = true;
            }
            while (!flag2 && num127 < 100)
            {
                num127++;
                int num128 = Main.rand.Next(point2.X - num123, point2.X + num123 + 1);
                int num129 = Main.rand.Next(point2.Y - num123, point2.Y - Math.Abs(num128 - point2.X) + 1);
                if ((num129 < point2.Y - num125 || num129 > point2.Y + num125 || num128 < point2.X - num125 || num128 > point2.X + num125) && (num129 < point.Y - num124 || num129 > point.Y + num124 || num128 < point.X - num124 || num128 > point.X + num124) && !Main.tile[num128, num129].nactive())
                {
                    bool flag3 = true;
                    if (flag3 && Main.tile[num128, num129].lava())
                    {
                        flag3 = false;
                    }
                    if (flag3 && Collision.SolidTiles(num128 - num126, num128 + num126, num129 - num126, num129 + num126))
                    {
                        flag3 = false;
                    }
                    if (flag3 && !Collision.CanHitLine(self.Center, 0, 0, Main.player[self.target].Center, 0, 0))
                    {
                        flag3 = false;
                    }
                    if (flag3)
                    {
                        Projectile.NewProjectile(self.GetSpawnSource_ForProjectile(), num128 * 16 + 8, num129 * 16 + 8, 0f, 0f, 578, 0, 1f, Main.myPlayer);
                        break;
                    }
                }
            }
        }
        else if (self.type == 405 && self.life <= 0)
        {
            int num156 = NPC.CountNPCS(406) + NPC.CountNPCS(405);
            int num130 = 4;
            if (num156 >= 4)
            {
                num130 = 3;
            }
            if (num156 >= 7)
            {
                num130 = 2;
            }
            if (num156 >= 10)
            {
                num130 = 1;
            }
            for (int num131 = 0; num131 < num130; num131++)
            {
                Vector2 vector3 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * (3f + Main.rand.NextFloat() * 4f);
                int num132 = NPC.NewNPC(self.GetSpawnSource_NPCHurt(), (int)self.Center.X, (int)self.Bottom.Y, 406, self.whoAmI);
                Main.npc[num132].velocity = vector3;
            }
        }
        else if (self.type == 491 && self.life <= 0)
        {
            for (int num133 = 0; num133 < 4; num133++)
            {
                float num134 = (num133 < 2).ToDirectionInt() * ((float)Math.PI / 8f + (float)Math.PI / 4f * Main.rand.NextFloat());
                Vector2 vector4 = new Vector2(0f, (0f - Main.rand.NextFloat()) * 0.5f - 0.5f).RotatedBy(num134) * 6f;
                Projectile.NewProjectile(self.GetSpawnSource_ForProjectile(), self.Center.X, self.Center.Y, vector4.X, vector4.Y, 594, 0, 0f, Main.myPlayer);
            }
        }
        if (self.type == 16 && self.life <= 0)
        {
            int num135 = Main.rand.Next(2) + 2;
            for (int num136 = 0; num136 < num135; num136++)
            {
                int num137 = NPC.NewNPC(self.GetSpawnSource_NPCHurt(), (int)(self.position.X + self.width / 2), (int)(self.position.Y + self.height), 1);
                Main.npc[num137].SetDefaults(-5);
                Main.npc[num137].velocity.X = self.velocity.X * 2f;
                Main.npc[num137].velocity.Y = self.velocity.Y;
                Main.npc[num137].velocity.X += Main.rand.Next(-20, 20) * 0.1f + num136 * self.direction * 0.3f;
                Main.npc[num137].velocity.Y -= Main.rand.Next(0, 10) * 0.1f + num136;
                Main.npc[num137].ai[0] = -1000 * Main.rand.Next(3);
                if (num137 < 200)
                {
                    NetMessage.SendData(23, -1, -1, null, num137);
                }
            }
        }
        if (self.type == 246 && self.life <= 0)
        {
            NPC.NewNPC(self.GetSpawnSource_NPCHurt(), (int)self.Center.X, (int)self.position.Y + self.height, 249, self.whoAmI);
        }
        if (self.type == 677 && self.life <= 0)
        {
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ShimmerArrow, new ParticleOrchestraSettings
            {
                PositionInWorld = self.Center,
                MovementVector = self.velocity
            });
        }
        if ((self.type == 81 || self.type == 121) && self.life <= 0)
        {
            if (self.type == 121)
            {
                int num138 = NPC.NewNPC(self.GetSpawnSource_NPCHurt(), (int)(self.position.X + self.width / 2), (int)(self.position.Y + self.height), 81);
                Main.npc[num138].SetDefaults(-2);
                Main.npc[num138].velocity.X = self.velocity.X;
                Main.npc[num138].velocity.Y = self.velocity.Y;
                if (num138 < 200)
                {
                    NetMessage.SendData(23, -1, -1, null, num138);
                }
            }
            else if (self.scale >= 1f)
            {
                int num139 = Main.rand.Next(2) + 2;
                for (int num140 = 0; num140 < num139; num140++)
                {
                    int num141 = NPC.NewNPC(self.GetSpawnSource_NPCHurt(), (int)(self.position.X + self.width / 2), (int)(self.position.Y + self.height), 1);
                    Main.npc[num141].SetDefaults(-1);
                    Main.npc[num141].velocity.X = self.velocity.X * 3f;
                    Main.npc[num141].velocity.Y = self.velocity.Y;
                    Main.npc[num141].velocity.X += Main.rand.Next(-10, 10) * 0.1f + num140 * self.direction * 0.3f;
                    Main.npc[num141].velocity.Y -= Main.rand.Next(0, 10) * 0.1f + num140;
                    Main.npc[num141].ai[1] = num140;
                    if (num141 < 200)
                    {
                        NetMessage.SendData(23, -1, -1, null, num141);
                    }
                }
            }
        }
        if ((self.type == 59 || self.type == 60 || self.type == 151) && self.life <= 0)
        {
            if (MainConfigInfo.StaticDisableSpawnLavaWhenNPCDead.IsFalseRet(Main.expertMode && self.type == 59 && !Main.remixWorld) || ((self.type == 151 || self.type == 60) && Main.remixWorld && Main.getGoodWorld))
            {
                try
                {
                    int num142 = (int)(self.Center.X / 16f);
                    int num143 = (int)(self.Center.Y / 16f);
                    if (!WorldGen.SolidTile(num142, num143) && Main.tile[num142, num143].liquid == 0)
                    {
                        Main.tile[num142, num143].liquid = (byte)Main.rand.Next(50, 150);
                        Main.tile[num142, num143].lava(lava: true);
                        Main.tile[num142, num143].honey(honey: false);
                        WorldGen.SquareTileFrame(num142, num143);
                    }
                }
                catch
                {
                }
            }
        }
        else if (self.type == 50 && self.life <= 0)
        {
            int num146 = Main.rand.Next(4) + 4;
            for (int num147 = 0; num147 < num146; num147++)
            {
                int x = (int)(self.position.X + Main.rand.Next(self.width - 32));
                int y = (int)(self.position.Y + Main.rand.Next(self.height - 32));
                int num148 = NPC.NewNPC(self.GetSpawnSource_NPCHurt(), x, y, 1, self.whoAmI + 1);
                Main.npc[num148].SetDefaults(1);
                Main.npc[num148].velocity.X = Main.rand.Next(-15, 16) * 0.1f;
                Main.npc[num148].velocity.Y = Main.rand.Next(-30, 1) * 0.1f;
                Main.npc[num148].ai[0] = -1000 * Main.rand.Next(3);
                Main.npc[num148].ai[1] = 0f;
                if (num148 < 200)
                {
                    NetMessage.SendData(23, -1, -1, null, num148);
                }
            }
        }
        else if ((self.type == 687 || self.type == 685) && self.life <= 0)
        {
            Terraria.Utils.PoofOfSmoke(self.Center - new Vector2(20f));
        }
        else if (self.type == 133)
        {
            if (self.life > 0 && self.life < self.lifeMax * 0.5f && self.localAI[0] == 0f)
            {
                self.localAI[0] = 1f;
            }
        }
        else if (self.type == 632 && self.life <= 0)
        {
            int num150 = Main.rand.Next(2) + 2;
            for (int num151 = 0; num151 < num150; num151++)
            {
                Vector2 vector5 = new Vector2(Main.rand.Next(-10, 10) * 0.2f, -3.5f - Main.rand.Next(5, 10) * 0.3f - num151 * 0.5f);
                int num152 = NPC.NewNPC(self.GetSpawnSource_NPCHurt(), (int)(self.position.X + self.width / 2), (int)self.position.Y + Main.rand.Next(self.height / 2) + 10, 606);
                Main.npc[num152].velocity = vector5;
                Main.npc[num152].netUpdate = true;
            }
        }
        else if (self.type == 115 && self.life <= 0)
        {
            NPC.NewNPC(self.GetSpawnSource_NPCHurt(), (int)(self.position.X + self.width / 2), (int)(self.position.Y + self.height), 116);
        }
        else if (self.type == 135 && self.life > 0)
        {
            int maxValue = 25;
            if (NPC.IsMechQueenUp)
            {
                maxValue = 50;
            }
            if (self.ai[2] == 0f && Main.rand.Next(maxValue) == 0)
            {
                self.ai[2] = 1f;
                int num153 = NPC.NewNPC(self.GetSpawnSource_NPCHurt(), (int)(self.position.X + self.width / 2), (int)(self.position.Y + self.height), 139);
                if (num153 < 200)
                {
                    NetMessage.SendData(23, -1, -1, null, num153);
                }
                self.netUpdate = true;
            }
        }
    }
    public static void DoDeathEvents_AdvanceSlimeRain(this NPC self, Player closestPlayer)
    {
        if (Main.slimeRain && Main.slimeRainNPC[self.type] && !NPC.AnyNPCs(50))
        {
            int needCount = MainConfigInfo.StaticSpawnSlimeKingWhenSlimeRainNeedKillCount;
            if (NPC.downedSlimeKing)
            {
                //needCount /= 2;
                needCount = MainConfigInfo.StaticSpawnSlimeKingWhenSlimeRainAndDownedSlimeKingNeedKillCount;
            }
            Main.slimeRainKillCount++;
            if (Main.slimeRainKillCount >= needCount)
            {
                NPC.SpawnOnPlayer(closestPlayer.whoAmI, 50);
                Main.slimeRainKillCount = -needCount / 2;
            }
        }
    }
    public static void DoDeathEvents(this NPC self, Player closestPlayer)
    {
        var needSend = false;
        self.DoDeathEvents_AdvanceSlimeRain(closestPlayer);
        self.DoDeathEvents_SummonDungeonSpirit(closestPlayer);
        if (Main.remixWorld && !NPC.downedSlimeKing && self.AnyInteractions() && Main.AnyPlayerReadyToFightKingSlime() && self.type == 1 && !NPC.AnyNPCs(50) && Main.rand.Next(200) == 0)
        {
            NPC.SpawnOnPlayer(closestPlayer.whoAmI, 50);
        }
        switch (self.type)
        {
            case 216:
                NPC.SpawnBoss((int)self.position.X, (int)self.position.Y, 662, self.target);
                break;
            case 327:
                if (Main.pumpkinMoon)
                {
                    NPC.SetEventFlagCleared(ref NPC.downedHalloweenKing, 5);
                }
                break;
            case 325:
                if (Main.pumpkinMoon)
                {
                    NPC.SetEventFlagCleared(ref NPC.downedHalloweenTree, 4);
                }
                break;
            case 344:
                if (Main.snowMoon)
                {
                    NPC.SetEventFlagCleared(ref NPC.downedChristmasTree, 21);
                }
                break;
            case 345:
                if (Main.snowMoon)
                {
                    NPC.SetEventFlagCleared(ref NPC.downedChristmasIceQueen, 20);
                }
                break;
            case 346:
                if (Main.snowMoon)
                {
                    NPC.SetEventFlagCleared(ref NPC.downedChristmasSantank, 22);
                }
                break;
            case 552:
            case 553:
            case 554:
                if (DD2Event.Ongoing)
                {
                    DD2Event.AnnounceGoblinDeath(self);
                    if (DD2Event.ShouldDropCrystals())
                    {
                        Item.NewItem(self.GetItemSource_Loot(), self.position, self.Size, 3822);
                    }
                }
                break;
            case 555:
            case 556:
            case 557:
            case 558:
            case 559:
            case 560:
            case 561:
            case 562:
            case 563:
            case 564:
            case 565:
            case 568:
            case 569:
            case 570:
            case 571:
            case 572:
            case 573:
            case 574:
            case 575:
            case 576:
            case 577:
            case 578:
                if (DD2Event.ShouldDropCrystals())
                {
                    Item.NewItem(self.GetItemSource_Loot(), self.position, self.Size, 3822);
                }
                break;
            case 412:
            case 413:
            case 414:
            case 415:
            case 416:
            case 417:
            case 418:
            case 419:
            case 518:
                if (NPC.ShieldStrengthTowerSolar > 0)
                {
                    Projectile.NewProjectile(self.GetSpawnSource_ForProjectile(), self.Center.X, self.Center.Y, 0f, 0f, 629, 0, 0f, Main.myPlayer, NPC.FindFirstNPC(517));
                }
                break;
            case 425:
            case 426:
            case 427:
            case 429:
                if (NPC.ShieldStrengthTowerVortex > 0)
                {
                    Projectile.NewProjectile(self.GetSpawnSource_ForProjectile(), self.Center.X, self.Center.Y, 0f, 0f, 629, 0, 0f, Main.myPlayer, NPC.FindFirstNPC(422));
                }
                break;
            case 420:
            case 421:
            case 423:
            case 424:
                if (NPC.ShieldStrengthTowerNebula > 0)
                {
                    Projectile.NewProjectile(self.GetSpawnSource_ForProjectile(), self.Center.X, self.Center.Y, 0f, 0f, 629, 0, 0f, Main.myPlayer, NPC.FindFirstNPC(507));
                }
                break;
            case 402:
            case 405:
            case 407:
            case 409:
            case 411:
                if (NPC.ShieldStrengthTowerStardust > 0)
                {
                    Projectile.NewProjectile(self.GetSpawnSource_ForProjectile(), self.Center.X, self.Center.Y, 0f, 0f, 629, 0, 0f, Main.myPlayer, NPC.FindFirstNPC(493));
                }
                break;
            case 517:
                NPC.downedTowerSolar = true;
                NPC.TowerActiveSolar = false;
                WorldGen.UpdateLunarApocalypse();
                WorldGen.MessageLunarApocalypse();
                break;
            case 422:
                NPC.downedTowerVortex = true;
                NPC.TowerActiveVortex = false;
                WorldGen.UpdateLunarApocalypse();
                WorldGen.MessageLunarApocalypse();
                break;
            case 507:
                NPC.downedTowerNebula = true;
                NPC.TowerActiveNebula = false;
                WorldGen.UpdateLunarApocalypse();
                WorldGen.MessageLunarApocalypse();
                break;
            case 493:
                NPC.downedTowerStardust = true;
                NPC.TowerActiveStardust = false;
                WorldGen.UpdateLunarApocalypse();
                WorldGen.MessageLunarApocalypse();
                break;
            case 245:
                NPC.SetEventFlagCleared(ref NPC.downedGolemBoss, 6);
                break;
            case 370:
                NPC.SetEventFlagCleared(ref NPC.downedFishron, 7);
                break;
            case 636:
                NPC.SetEventFlagCleared(ref NPC.downedEmpressOfLight, 23);
                break;
            case 668:
                NPC.SetEventFlagCleared(ref NPC.downedDeerclops, 25);
                break;
            case 657:
                NPC.SetEventFlagCleared(ref NPC.downedQueenSlime, 24);
                break;
            case 22:
                if (Collision.LavaCollision(self.position, self.width, self.height))
                {
                    NPC.SpawnWOF(self.position);
                }
                break;
            case 614:
                {
                    int projDamage = 175;
                    if (self.SpawnedFromStatue)
                    {
                        projDamage = 0;
                    }
                    Projectile.NewProjectile(self.GetSpawnSource_ForProjectile(), self.Center.X, self.Center.Y, 0f, 0f, 281, projDamage, 0f, Main.myPlayer, -2f, self.releaseOwner + 1);
                    break;
                }
            case 109:
                if (!NPC.downedClown)
                {
                    NPC.downedClown = true;
                    needSend = true;
                }
                break;
            case 222:
                if (!NPC.downedQueenBee)
                {
                    needSend = true;
                }
                NPC.SetEventFlagCleared(ref NPC.downedQueenBee, 8);
                break;
            case 439:
                NPC.SetEventFlagCleared(ref NPC.downedAncientCultist, 9);
                WorldGen.TriggerLunarApocalypse();
                break;
            case 398:
                NPC.SetEventFlagCleared(ref NPC.downedMoonlord, 10);
                NPC.LunarApocalypseIsUp = false;
                break;
            case 50:
                if (Main.slimeRain && !MainConfigInfo.StaticNoStopSlimeRainAfterKillSlimeKingWhenSlimeRaining)
                {
                    needSend = true;
                    Main.StopSlimeRain();
                    AchievementsHelper.NotifyProgressionEvent(16);
                }
                if (!NPC.unlockedSlimeBlueSpawn)
                {
                    needSend = true;
                    NPC.unlockedSlimeBlueSpawn = true;
                    self.ViolentlySpawnNerdySlime();
                }
                if (!NPC.downedSlimeKing)
                {
                    needSend = true;
                }
                NPC.SetEventFlagCleared(ref NPC.downedSlimeKing, 11);
                break;
            case 125:
            case 126:
                if (self.boss)
                {
                    NPC.SetEventFlagCleared(ref NPC.downedMechBoss2, 17);
                    NPC.downedMechBossAny = true;
                }
                break;
            case 262:
                {
                    bool num3 = NPC.downedPlantBoss;
                    NPC.SetEventFlagCleared(ref NPC.downedPlantBoss, 12);
                    if (!num3)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[33].Key), new Color(50, 255, 130));
                    }
                    break;
                }
            case 4:
                NPC.SetEventFlagCleared(ref NPC.downedBoss1, 13);
                break;
            case 13:
            case 14:
            case 15:
            case 266:
                if (self.boss)
                {
                    if (!NPC.downedBoss2 || Main.rand.Next(2) == 0)
                    {
                        WorldGen.spawnMeteor = true;
                    }
                    NPC.SetEventFlagCleared(ref NPC.downedBoss2, 14);
                }
                break;
            case 35:
                if (self.boss)
                {
                    NPC.SetEventFlagCleared(ref NPC.downedBoss3, 15);
                }
                break;
            case 127:
                if (self.boss)
                {
                    NPC.SetEventFlagCleared(ref NPC.downedMechBoss3, 18);
                    NPC.downedMechBossAny = true;
                }
                break;
            case 134:
                if (self.boss)
                {
                    NPC.SetEventFlagCleared(ref NPC.downedMechBoss1, 16);
                    NPC.downedMechBossAny = true;
                }
                break;
            case 113:
                {
                    self.CreateBrickBoxForWallOfFlesh();
                    bool eventFlag = Main.hardMode;
                    WorldGen.StartHardmode();
                    if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && !eventFlag)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[32].Key), new Color(50, 255, 130));
                    }
                    NPC.SetEventFlagCleared(ref eventFlag, 19);
                    break;
                }
            case 661:
                if (self.GetWereThereAnyInteractions())
                {
                    int num = 636;
                    if (!NPC.AnyNPCs(num))
                    {
                        Vector2 vector = self.Center + new Vector2(0f, -200f) + Main.rand.NextVector2Circular(50f, 50f);
                        NPC.SpawnBoss((int)vector.X, (int)vector.Y, num, closestPlayer.whoAmI);
                    }
                }
                break;
        }
        if (self.boss)
        {
            self.DoDeathEvents_DropBossPotionsAndHearts();
            self.DoDeathEvents_CelebrateBossDeath();
            needSend = true;
        }
        if (needSend)
        {
            NetMessage.SendData(7);
        }
    }
}