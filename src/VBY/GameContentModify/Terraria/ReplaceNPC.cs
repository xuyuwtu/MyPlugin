using IDAnalyzer;

using Microsoft.Xna.Framework;

using OTAPI;

using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;

using VBY.GameContentModify.Config;

using static VBY.GameContentModify.GameContentModify;

namespace VBY.GameContentModify;

[ReplaceType(typeof(NPC))]
public static class ReplaceNPC
{
    [DetourMethod]
    public static bool BigMimicSummonCheck(On.Terraria.NPC.orig_BigMimicSummonCheck orig, int x, int y, Player user)
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
            if (Main.tile[x, y].type == TileID.Containers2)
            {
                action = 5; //Kill Containers2
            }
            NetMessage.SendData(MessageID.ChestUpdates, -1, -1, null, action, x, y, 0f, chestIndex2);
            NetMessage.SendTileSquare(-1, x, y, 3);
            spawnInfo.Execute(x, y, user);
        }
        return false;
    }
    public static void SpawnNPC(On.Terraria.NPC.orig_SpawnNPC orig)
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
            if (!Main.player[i].active)
            {
                continue;
            }
            num4++;
        }
        float num5 = 0f;
        for (int j = 0; j < 200; j++)
        {
            if (!Main.npc[j].active)
            {
                continue;
            }
            switch (Main.npc[j].type)
            {
                case NPCID.HeadlessHorseman:
                case NPCID.MourningWood:
                case NPCID.Pumpking:
                case NPCID.PumpkingBlade:
                case NPCID.Everscream:
                case NPCID.IceQueen:
                case NPCID.SantaNK1:
                    num5 += Main.npc[j].npcSlots;
                    break;
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
            if (player.isNearNPC(NPCID.MoonLordCore, NPC.MoonLordFightingDistance))
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
            if (Main.tile[num8, num9].wall == WallID.LihzahrdBrickUnsafe)
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
            if (Main.drunkWorld && Main.tile[num8, num9].wall == WallID.HiveUnsafe)
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
            if (player.ZoneWaterCandle || player.inventory[player.selectedItem].type == ItemID.WaterCandle)
            {
                if (!player.ZonePeaceCandle && player.inventory[player.selectedItem].type != ItemID.PeaceCandle)
                {
                    NPC.spawnRate = (int)(NPC.spawnRate * 0.75);
                    NPC.maxSpawns = (int)(NPC.maxSpawns * 1.5f);
                }
            }
            else if (player.ZonePeaceCandle || player.inventory[player.selectedItem].type == ItemID.PeaceCandle)
            {
                NPC.spawnRate = (int)(NPC.spawnRate * 1.3);
                NPC.maxSpawns = (int)(NPC.maxSpawns * 0.7f);
            }
            if (player.ZoneShadowCandle || player.inventory[player.selectedItem].type == ItemID.ShadowCandle)
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
                if (player.inventory[player.selectedItem].type == ItemID.SniperRifle || player.inventory[player.selectedItem].type == ItemID.Binoculars || player.scope)
                {
                    float num11 = 1.5f;
                    if (player.inventory[player.selectedItem].type == ItemID.SniperRifle && player.scope)
                    {
                        num11 = 1.25f;
                    }
                    else if (player.inventory[player.selectedItem].type == ItemID.SniperRifle)
                    {
                        num11 = 1.5f;
                    }
                    else if (player.inventory[player.selectedItem].type == ItemID.Binoculars)
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
            var spawnPoint = new SpawnPoint(spawnX, spawnY);
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
                if (player.ZoneDungeon && (!Main.tileDungeon[Main.tile[tileX, tileY].type] || Main.tile[tileX, tileY - 1].wall == WallID.None))
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
                if (Main.tile[tileX, tileY].type == TileID.Marble)
                {
                    flag10 = true;
                }
                else if (Main.tile[tileX, tileY].type == TileID.Granite)
                {
                    flag9 = true;
                }
                else if (Main.tile[num29, num30].type == TileID.Marble)
                {
                    flag10 = true;
                }
                else if (Main.tile[num29, num30].type == TileID.Granite)
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
                            if (Main.tile[num33, num35].type == TileID.Marble)
                            {
                                flag10 = true;
                            }
                            if (Main.tile[num33, num35].type == TileID.Granite)
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
                            if (Main.tile[num36, num38].type == TileID.Marble)
                            {
                                flag10 = true;
                            }
                            if (Main.tile[num36, num38].type == TileID.Granite)
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
                                if (Main.tile[num40, num41].wall == WallID.SpiderUnsafe)
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
                    if (Main.tile[x, y].wall == WallID.SpiderUnsafe)
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
            if (Main.tile[tileX, tileY - 2].wall == WallID.LivingWoodUnsafe || Main.tile[tileX, tileY].wall == WallID.LivingWoodUnsafe)
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
                    num47 = Utils.SelectRandom(Main.rand, 424, 424, 424, 423, 423, 423, 421, 421, 421, 420, 420);
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
                    newNPC = spawnPoint.NewNPC(num47, 1);
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
                    newNPC = spawnPoint.NewNPC(num48, 1);
                }
            }
            else if (player.ZoneTowerStardust)
            {
                int num49 = Utils.SelectRandom<int>(Main.rand, 411, 411, 411, 409, 409, 407, 402, 405);
                if (num49 != 0)
                {
                    newNPC = spawnPoint.NewNPC(num49, 1);
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
                    newNPC = spawnPoint.NewNPC(num50, 1);
                }
            }
            else if (flag3)
            {
                int maxValue2 = 8;
                int maxValue3 = 30;
                bool flag28 = Math.Abs(tileX - Main.maxTilesX / 2) / (float)(Main.maxTilesX / 2) > 0.33f && (Main.wallLight[Main.tile[num8, num9].wall] || Main.tile[num8, num9].wall == WallID.Cloud);
                if (flag28 && NPC.AnyDanger())
                {
                    flag28 = false;
                }
                if (player.ZoneWaterCandle)
                {
                    maxValue2 = 3;
                    maxValue3 = 10;
                }
                if (flag6 && Main.invasionType == InvasionID.MartianMadness)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.MartianDrone);
                }
                else if (flag28 && Main.hardMode && NPC.downedGolemBoss && ((!NPC.downedMartians && Main.rand.Next(maxValue2) == 0) || Main.rand.Next(maxValue3) == 0) && !NPC.AnyNPCs(NPCID.MartianProbe))
                {
                    spawnPoint.NewNPC(NPCID.MartianProbe);
                }
                else if (flag28 && Main.hardMode && NPC.downedGolemBoss && ((!NPC.downedMartians && Main.rand.Next(maxValue2) == 0) || Main.rand.Next(maxValue3) == 0) && !NPC.AnyNPCs(NPCID.MartianProbe) && (player.inventory[player.selectedItem].type == ItemID.WaterCandle || player.ZoneWaterCandle))
                {
                    spawnPoint.NewNPC(NPCID.MartianProbe);
                }
                else if (Main.hardMode && !NPC.AnyNPCs(NPCID.WyvernHead) && !flag5 && Main.rand.Next(10) == 0)
                {
                    spawnPoint.NewNPC(NPCID.WyvernHead, 1);
                }
                else if (Main.hardMode && !NPC.AnyNPCs(NPCID.WyvernHead) && !flag5 && Main.rand.Next(10) == 0 && (player.inventory[player.selectedItem].type == ItemID.WaterCandle || player.ZoneWaterCandle))
                {
                    spawnPoint.NewNPC(NPCID.WyvernHead, 1);
                }
                else if (!NPC.unlockedSlimePurpleSpawn && player.RollLuck(25) == 0 && !NPC.AnyNPCs(NPCID.BoundTownSlimePurple))
                {
                    spawnPoint.NewNPC(NPCID.BoundTownSlimePurple);
                }
                else
                {
                    spawnPoint.NewNPC(NPCID.Harpy);
                }
            }
            else if (flag6)
            {
                if (Main.invasionType == InvasionID.GoblinArmy)
                {
                    if (Main.hardMode && !NPC.AnyNPCs(NPCID.GoblinSummoner) && Main.rand.Next(30) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.GoblinSummoner);
                    }
                    else if (Main.rand.Next(9) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.GoblinSorcerer);
                    }
                    else if (Main.rand.Next(5) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.GoblinPeon);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.GoblinArcher);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.GoblinThief);
                    }
                    else
                    {
                        spawnPoint.NewNPC(NPCID.GoblinWarrior);
                    }
                }
                else if (Main.invasionType == InvasionID.SnowLegion)
                {
                    if (Main.rand.Next(7) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.SnowBalla);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.SnowmanGangsta);
                    }
                    else
                    {
                        spawnPoint.NewNPC(NPCID.MisterStabby);
                    }
                }
                else if (Main.invasionType == InvasionID.PirateInvasion)
                {
                    if (Main.invasionSize < Main.invasionSizeStart / 2 && Main.rand.Next(20) == 0 && !NPC.AnyNPCs(NPCID.PirateShip) && !Collision.SolidTiles(tileX - 20, tileX + 20, tileY - 40, tileY - 10))
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, (tileY - 10) * 16, NPCID.PirateShip);
                    }
                    else if (Main.rand.Next(30) == 0 && !NPC.AnyNPCs(NPCID.PirateCaptain))
                    {
                        spawnPoint.NewNPC(NPCID.PirateCaptain);
                    }
                    else if (Main.rand.Next(11) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.PirateCrossbower);
                    }
                    else if (Main.rand.Next(9) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.Parrot);
                    }
                    else if (Main.rand.Next(7) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.PirateDeadeye);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.PirateCorsair);
                    }
                    else
                    {
                        spawnPoint.NewNPC(NPCID.PirateDeckhand);
                    }
                }
                else if (Main.invasionType == InvasionID.MartianMadness)
                {
                    int num51 = 0;
                    int num52 = Main.rand.Next(7);
                    bool flag29 = (Main.invasionSizeStart - Main.invasionSize) / (float)Main.invasionSizeStart >= 0.3f && !NPC.AnyNPCs(NPCID.MartianSaucerCore);
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
                            if (!NPC.AnyNPCs(NPCID.MartianWalker))
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
                        newNPC = spawnPoint.NewNPC(num51, 1);
                    }
                }
            }
            else if (num46 == 244 && !Main.remixWorld)
            {
                if (flag7)
                {
                    if (player.RollLuck(NPC.goldCritterChance) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.GoldGoldfish);
                    }
                    else
                    {
                        spawnPoint.NewNPC(NPCID.Goldfish);
                    }
                }
                else if (tileY > Main.worldSurface)
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            spawnPoint.NewNPC(NPCID.GoldMouse);
                        }
                        else
                        {
                            newNPC = spawnPoint.NewNPC(NPCID.Mouse);
                        }
                    }
                    else if (Main.rand.Next(2) == 0)
                    {
                        newNPC = spawnPoint.NewNPC(NPCID.Snail);
                    }
                    else if (player.RollLuck(NPC.goldCritterChance) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.GoldWorm);
                    }
                    else if (Main.rand.Next(3) != 0)
                    {
                        spawnPoint.NewNPC(NPCID.Worm);
                    }
                }
                else if (player.RollLuck(2) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.Gnome);
                    Main.npc[newNPC].timeLeft *= 10;
                }
                else if (player.RollLuck(NPC.goldCritterChance) == 0)
                {
                    spawnPoint.NewNPC(NPCID.GoldBunny);
                }
                else if (player.RollLuck(NPC.goldCritterChance) == 0)
                {
                    spawnPoint.NewNPC(NPCID.SquirrelGold);
                }
                else if (Main.halloween && Main.rand.Next(3) != 0)
                {
                    spawnPoint.NewNPC(NPCID.BunnySlimed);
                }
                else if (Main.xMas && Main.rand.Next(3) != 0)
                {
                    spawnPoint.NewNPC(NPCID.BunnyXmas);
                }
                else if (BirthdayParty.PartyIsUp && Main.rand.Next(3) != 0)
                {
                    spawnPoint.NewNPC(NPCID.PartyBunny);
                }
                else if (Main.rand.Next(3) == 0)
                {
                    spawnPoint.NewNPC(Utils.SelectRandom(Main.rand, new short[2] { 299, 538 }));
                }
                else
                {
                    spawnPoint.NewNPC(NPCID.Bunny);
                }
            }
            else if (!NPC.savedBartender && DD2Event.ReadyToFindBartender && !NPC.AnyNPCs(NPCID.BartenderUnconscious) && Main.rand.Next(80) == 0 && !flag7)
            {
                spawnPoint.NewNPC(NPCID.BartenderUnconscious);
            }
            else if (Main.tile[tileX, tileY].wall == WallID.SpiderUnsafe || flag11)
            {
                bool flag30 = flag21 && tileY < Main.maxTilesY - 210;
                if (Main.dontStarveWorld)
                {
                    flag30 = tileY < Main.maxTilesY - 210;
                }
                if (Main.tile[tileX, tileY].wall == WallID.SpiderUnsafe && Main.rand.Next(8) == 0 && !flag7 && flag30 && !NPC.savedStylist && !NPC.AnyNPCs(NPCID.WebbedStylist))
                {
                    spawnPoint.NewNPC(NPCID.WebbedStylist);
                }
                else if (Main.hardMode && Main.rand.Next(10) != 0)
                {
                    spawnPoint.NewNPC(NPCID.BlackRecluse);
                }
                else
                {
                    spawnPoint.NewNPC(NPCID.WallCreeper);
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
                if (Main.rand.Next(20) == 0 && !flag7 && !NPC.savedGolfer && !NPC.AnyNPCs(NPCID.GolferRescue))
                {
                    spawnPoint.NewNPC(NPCID.GolferRescue);
                }
                else if (Main.hardMode && Main.rand.Next((int)(45f * num56)) == 0 && !flag5 && tileY > Main.worldSurface + 100.0)
                {
                    spawnPoint.NewNPC(NPCID.DuneSplicerHead);
                }
                else if (Main.rand.Next((int)(45f * num56)) == 0 && !flag5 && tileY > Main.worldSurface + 100.0 && NPC.CountNPCS(NPCID.TombCrawlerHead) == 0)
                {
                    spawnPoint.NewNPC(NPCID.TombCrawlerHead);
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
                    spawnPoint.NewNPC(num57);
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
                    spawnPoint.NewNPC(num58);
                }
            }
            else if (Main.hardMode && flag7 && player.ZoneJungle && Main.rand.Next(3) != 0)
            {
                spawnPoint.NewNPC(NPCID.Arapaima);
            }
            else if (Main.hardMode && flag7 && player.ZoneCrimson && Main.rand.Next(3) != 0)
            {
                spawnPoint.NewNPC(NPCID.BloodJelly);
            }
            else if (Main.hardMode && flag7 && player.ZoneCrimson && Main.rand.Next(3) != 0)
            {
                spawnPoint.NewNPC(NPCID.BloodFeeder);
            }
            else if ((!flag12 || (!NPC.savedAngler && !NPC.AnyNPCs(NPCID.SleepingAngler))) && flag7 && flag22)
            {
                bool flag31 = false;
                if (!NPC.savedAngler && !NPC.AnyNPCs(NPCID.SleepingAngler) && (tileY < Main.worldSurface - 10.0 || Main.remixWorld))
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
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num59 * 16, NPCID.SleepingAngler);
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
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num61 * 16, NPCID.Seagull);
                    }
                    else if (Main.rand.Next(10) == 0)
                    {
                        int num64 = Main.rand.Next(3);
                        if (num64 == 0 && num61 > 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num61 * 16, NPCID.SeaTurtle);
                        }
                        else if (num64 == 1 && num62 > 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num62 * 16, NPCID.Dolphin);
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
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num65 * 16, NPCID.GoldSeahorse);
                            }
                            else
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num65 * 16, NPCID.Seahorse);
                            }
                        }
                    }
                    else if (Main.rand.Next(40) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.SeaSnail);
                    }
                    else if (Main.rand.Next(18) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.Squid);
                    }
                    else if (Main.rand.Next(8) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.Shark);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.Crab);
                    }
                    else
                    {
                        spawnPoint.NewNPC(NPCID.PinkJellyfish);
                    }
                }
            }
            else if (!flag7 && !NPC.savedAngler && !NPC.AnyNPCs(NPCID.SleepingAngler) && (tileX < WorldGen.beachDistance || tileX > Main.maxTilesX - WorldGen.beachDistance) && Main.tileSand[num45] && (tileY < Main.worldSurface || Main.remixWorld))
            {
                spawnPoint.NewNPC(NPCID.SleepingAngler);
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
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num66 * 16, NPCID.TurtleJungle);
                        }
                        else if (!flag && Main.cloudAlpha == 0f)
                        {
                            flag32 = true;
                            int num68 = Main.rand.Next(1, 4);
                            for (int num69 = 0; num69 < num68; num69++)
                            {
                                if (player.RollLuck(NPC.goldCritterChance) == 0)
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + Main.rand.Next(-16, 17), num66 * 16 - 16, NPCID.GoldWaterStrider);
                                }
                                else
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + Main.rand.Next(-16, 17), num66 * 16 - 16, NPCID.WaterStrider);
                                }
                            }
                        }
                    }
                }
                if (!flag32)
                {
                    if (Main.hardMode && Main.rand.Next(3) > 0)
                    {
                        spawnPoint.NewNPC(NPCID.AnglerFish);
                    }
                    else
                    {
                        spawnPoint.NewNPC(NPCID.Piranha);
                    }
                }
            }
            else if (!flag12 && flag7 && tileY > Main.worldSurface && Main.rand.Next(3) == 0)
            {
                if (Main.hardMode && Main.rand.Next(3) > 0)
                {
                    spawnPoint.NewNPC(NPCID.GreenJellyfish);
                }
                else
                {
                    spawnPoint.NewNPC(NPCID.BlueJellyfish);
                }
            }
            else if (flag7 && Main.rand.Next(4) == 0 && ((tileX > WorldGen.oceanDistance && tileX < Main.maxTilesX - WorldGen.oceanDistance) || tileY > Main.worldSurface + 50.0))
            {
                if (player.ZoneCorrupt)
                {
                    spawnPoint.NewNPC(NPCID.CorruptGoldfish);
                }
                else if (player.ZoneCrimson)
                {
                    spawnPoint.NewNPC(NPCID.CrimsonGoldfish);
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
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num70 * 16, NPCID.Turtle);
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
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + Main.rand.Next(-16, 17), num70 * 16 - 16, NPCID.GoldWaterStrider);
                                    }
                                    else
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + Main.rand.Next(-16, 17), num70 * 16 - 16, NPCID.WaterStrider);
                                    }
                                }
                            }
                            else
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num70 * 16, NPCID.Grebe);
                            }
                        }
                        else
                        {
                            spawnPoint.NewNPC(Main.rand.Next(2) == 0 ? 362 : 364);
                        }
                    }
                    else if (num3 == 53 && tileX > WorldGen.beachDistance && tileX < Main.maxTilesX - WorldGen.beachDistance)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num70 * 16, NPCID.Pupfish);
                    }
                    else if (player.RollLuck(NPC.goldCritterChance) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.GoldGoldfish);
                    }
                    else
                    {
                        spawnPoint.NewNPC(NPCID.Goldfish);
                    }
                }
                else if (num3 == 53 && tileX > WorldGen.beachDistance && tileX < Main.maxTilesX - WorldGen.beachDistance)
                {
                    spawnPoint.NewNPC(NPCID.Pupfish);
                }
                else if (player.RollLuck(NPC.goldCritterChance) == 0)
                {
                    spawnPoint.NewNPC(NPCID.GoldGoldfish);
                }
                else
                {
                    spawnPoint.NewNPC(NPCID.Goldfish);
                }
            }
            else if (NPC.downedGoblins && player.RollLuck(20) == 0 && !flag7 && flag21 && tileY < Main.maxTilesY - 210 && !NPC.savedGoblin && !NPC.AnyNPCs(NPCID.BoundGoblin))
            {
                spawnPoint.NewNPC(NPCID.BoundGoblin);
            }
            else if (Main.hardMode && player.RollLuck(20) == 0 && !flag7 && flag21 && tileY < Main.maxTilesY - 210 && !NPC.savedWizard && !NPC.AnyNPCs(NPCID.BoundWizard))
            {
                spawnPoint.NewNPC(NPCID.BoundWizard);
            }
            //else if (NPC.downedBoss3 && player.RollLuck(20) == 0 && !flag7 && flag21 && num2 < Main.maxTilesY - 210 && !NPC.unlockedSlimeOldSpawn && !NPC.AnyNPCs(685))
            else if (NPC.downedBoss3 && player.RollLuck(20) == 0 && !flag7 && flag21 && tileY < Main.maxTilesY - 210 && !NPC.AnyNPCs(NPCID.BoundTownSlimeOld))
            {
                spawnPoint.NewNPC(NPCID.BoundTownSlimeOld);
            }
            else if (flag12)
            {
                if (player.ZoneGraveyard)
                {
                    if (!flag7)
                    {
                        spawnPoint.NewNPC(Main.rand.Next(2) == 0 ? 606 : 610);
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
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num74 * 16, NPCID.SeaTurtle);
                            }
                            else if (num77 == 1 && num75 > 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num75 * 16, NPCID.Dolphin);
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
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num78 * 16, NPCID.GoldSeahorse);
                                }
                                else
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num78 * 16, NPCID.Seahorse);
                                }
                            }
                        }
                        else if (num74 > 0 && !flag15)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num74 * 16, NPCID.Seagull);
                        }
                    }
                    else
                    {
                        spawnPoint.NewNPC(NPCID.Seagull);
                    }
                }
                else if ((num45 == 2 || num45 == 477 || num45 == 53) && !tooWindyForButterflies && Main.raining && Main.dayTime && Main.rand.Next(2) == 0 && (tileY <= Main.worldSurface || Main.remixWorld) && NPC.FindCattailTop(tileX, tileY, out cattailX, out cattailY))
                {
                    if (player.RollLuck(NPC.goldCritterChance) == 0)
                    {
                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), cattailX * 16 + 8, cattailY * 16, NPCID.GoldDragonfly);
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
                                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + Main.rand.Next(-16, 17), num79 * 16 - 16, NPCID.GoldWaterStrider);
                                            }
                                            else
                                            {
                                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + Main.rand.Next(-16, 17), num79 * 16 - 16, NPCID.WaterStrider);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num79 * 16, NPCID.TurtleJungle);
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
                                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + Main.rand.Next(-16, 17), num79 * 16 - 16, NPCID.GoldWaterStrider);
                                            }
                                            else
                                            {
                                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + Main.rand.Next(-16, 17), num79 * 16 - 16, NPCID.WaterStrider);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num79 * 16, NPCID.Grebe);
                                    }
                                    break;
                                default:
                                    if (Main.rand.Next(5) == 0 && (num3 == 2 || num3 == 477))
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num79 * 16, NPCID.Turtle);
                                    }
                                    else
                                    {
                                        spawnPoint.NewNPC(Main.rand.Next(2) == 0 ? 362 : 364);
                                    }
                                    break;
                            }
                        }
                        else if (num3 == 53 && tileX > WorldGen.beachDistance && tileX < Main.maxTilesX - WorldGen.beachDistance)
                        {
                            spawnPoint.NewNPC(NPCID.Pupfish);
                        }
                        else if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            spawnPoint.NewNPC(NPCID.GoldGoldfish);
                        }
                        else
                        {
                            spawnPoint.NewNPC(NPCID.Goldfish);
                        }
                    }
                    else if (num3 == 53 && tileX > WorldGen.beachDistance && tileX < Main.maxTilesX - WorldGen.beachDistance)
                    {
                        spawnPoint.NewNPC(NPCID.Pupfish);
                    }
                    else if (player.RollLuck(NPC.goldCritterChance) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.GoldGoldfish);
                    }
                    else
                    {
                        spawnPoint.NewNPC(NPCID.Goldfish);
                    }
                }
                else if (num45 == 147 || num45 == 161)
                {
                    spawnPoint.NewNPC(Main.rand.Next(2) == 0 ? 148 : 149);
                }
                else if (num45 == 60)
                {
                    if (Main.dayTime && Main.rand.Next(3) != 0)
                    {
                        switch (Main.rand.Next(5))
                        {
                            case 0:
                                spawnPoint.NewNPC(NPCID.ScarletMacaw);
                                break;
                            case 1:
                                spawnPoint.NewNPC(NPCID.BlueMacaw);
                                break;
                            case 2:
                                spawnPoint.NewNPC(NPCID.Toucan);
                                break;
                            case 3:
                                spawnPoint.NewNPC(NPCID.YellowCockatiel);
                                break;
                            default:
                                spawnPoint.NewNPC(NPCID.GrayCockatiel);
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
                    spawnPoint.NewNPC(Main.rand.Next(366, 368));
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
                            spawnPoint.NewNPC(NPC.SpawnNPC_GetGemSquirrelToSpawn());
                        }
                        else if (flag21 && Main.rand.Next(5) == 0)
                        {
                            spawnPoint.NewNPC(NPC.SpawnNPC_GetGemBunnyToSpawn());
                        }
                        else if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            spawnPoint.NewNPC(NPCID.GoldWorm);
                        }
                        else if (Main.rand.Next(3) != 0)
                        {
                            spawnPoint.NewNPC(NPCID.Worm);
                        }
                        else if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            spawnPoint.NewNPC(NPCID.GoldGoldfishWalker);
                        }
                        else
                        {
                            spawnPoint.NewNPC(NPCID.GoldfishWalker);
                        }
                    }
                    else if (!Main.dayTime && Main.numClouds <= 55 && Main.cloudBGActive == 0f && Star.starfallBoost > 3f && flag33 && player.RollLuck(2) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.EnchantedNightcrawler);
                    }
                    else if (!tooWindyForButterflies && !Main.dayTime && Main.rand.Next(NPC.fireFlyFriendly) == 0 && flag33)
                    {
                        int num85 = 355;
                        if (num45 == 109)
                        {
                            num85 = 358;
                        }
                        spawnPoint.NewNPC(num85);
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
                        spawnPoint.NewNPC(NPCID.Owl);
                    }
                    else if (Main.dayTime && Main.time < 18000.0 && Main.rand.Next(3) != 0 && flag33)
                    {
                        int num86 = Main.rand.Next(4);
                        if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            spawnPoint.NewNPC(NPCID.GoldBird);
                        }
                        else
                        {
                            switch (num86)
                            {
                                case 0:
                                    spawnPoint.NewNPC(NPCID.BirdBlue);
                                    break;
                                case 1:
                                    spawnPoint.NewNPC(NPCID.BirdRed);
                                    break;
                                default:
                                    spawnPoint.NewNPC(NPCID.Bird);
                                    break;
                            }
                        }
                    }
                    else if (!tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(NPC.stinkBugChance) == 0 && flag33)
                    {
                        spawnPoint.NewNPC(NPCID.Stinkbug);
                        if (Main.rand.Next(4) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX - 16, spawnY, NPCID.Stinkbug);
                        }
                        if (Main.rand.Next(4) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + 16, spawnY, NPCID.Stinkbug);
                        }
                    }
                    else if (!tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(NPC.butterflyChance) == 0 && flag33)
                    {
                        if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            spawnPoint.NewNPC(NPCID.GoldButterfly);
                        }
                        else
                        {
                            spawnPoint.NewNPC(NPCID.Butterfly);
                        }
                        if (Main.rand.Next(4) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX - 16, spawnY, NPCID.Butterfly);
                        }
                        if (Main.rand.Next(4) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + 16, spawnY, NPCID.Butterfly);
                        }
                    }
                    else if (tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(NPC.butterflyChance / 2) == 0 && flag33)
                    {
                        if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            spawnPoint.NewNPC(NPCID.GoldLadyBug);
                        }
                        else
                        {
                            spawnPoint.NewNPC(NPCID.LadyBug);
                        }
                        if (Main.rand.Next(3) != 0)
                        {
                            spawnPoint.NewNPC(NPCID.LadyBug);
                        }
                        if (Main.rand.Next(2) == 0)
                        {
                            spawnPoint.NewNPC(NPCID.LadyBug);
                        }
                        if (Main.rand.Next(3) == 0)
                        {
                            spawnPoint.NewNPC(NPCID.LadyBug);
                        }
                        if (Main.rand.Next(4) == 0)
                        {
                            spawnPoint.NewNPC(NPCID.LadyBug);
                        }
                    }
                    else if (Main.rand.Next(2) == 0 && flag33)
                    {
                        int num87 = Main.rand.Next(4);
                        if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            spawnPoint.NewNPC(NPCID.GoldBird);
                        }
                        else
                        {
                            switch (num87)
                            {
                                case 0:
                                    spawnPoint.NewNPC(NPCID.BirdBlue);
                                    break;
                                case 1:
                                    spawnPoint.NewNPC(NPCID.BirdRed);
                                    break;
                                default:
                                    spawnPoint.NewNPC(NPCID.Bird);
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
                                spawnPoint.NewNPC(NPC.SpawnNPC_GetGemSquirrelToSpawn());
                            }
                            else
                            {
                                spawnPoint.NewNPC(NPC.SpawnNPC_GetGemBunnyToSpawn());
                            }
                        }
                        else
                        {
                            newNPC = NPC.SpawnNPC_SpawnLavaBaitCritters(tileX, tileY);
                        }
                    }
                    else if (player.RollLuck(NPC.goldCritterChance) == 0)
                    {
                        spawnPoint.NewNPC(NPCID.GoldBunny);
                    }
                    else if (player.RollLuck(NPC.goldCritterChance) == 0 && flag33)
                    {
                        spawnPoint.NewNPC(NPCID.SquirrelGold);
                    }
                    else if (Main.halloween && Main.rand.Next(3) != 0)
                    {
                        spawnPoint.NewNPC(NPCID.BunnySlimed);
                    }
                    else if (Main.xMas && Main.rand.Next(3) != 0)
                    {
                        spawnPoint.NewNPC(NPCID.BunnyXmas);
                    }
                    else if (BirthdayParty.PartyIsUp && Main.rand.Next(3) != 0)
                    {
                        spawnPoint.NewNPC(NPCID.PartyBunny);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        if (Main.remixWorld)
                        {
                            if (tileY < Main.rockLayer && tileY > Main.worldSurface)
                            {
                                if (Main.rand.Next(5) == 0)
                                {
                                    spawnPoint.NewNPC(NPC.SpawnNPC_GetGemSquirrelToSpawn());
                                }
                            }
                            else if (flag33)
                            {
                                spawnPoint.NewNPC(Utils.SelectRandom(Main.rand, new short[2] { 299, 538 }));
                            }
                        }
                        else if (tileY >= Main.rockLayer && tileY <= Main.UnderworldLayer)
                        {
                            if (Main.rand.Next(5) == 0)
                            {
                                spawnPoint.NewNPC(NPC.SpawnNPC_GetGemSquirrelToSpawn());
                            }
                        }
                        else if (flag33)
                        {
                            spawnPoint.NewNPC(Utils.SelectRandom(Main.rand, new short[2] { 299, 538 }));
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
                                    spawnPoint.NewNPC(NPC.SpawnNPC_GetGemBunnyToSpawn());
                                }
                            }
                            else
                            {
                                spawnPoint.NewNPC(NPCID.Bunny);
                            }
                        }
                    }
                    else if (tileY >= Main.rockLayer && tileY <= Main.UnderworldLayer)
                    {
                        if (Main.rand.Next(5) == 0)
                        {
                            spawnPoint.NewNPC(NPC.SpawnNPC_GetGemBunnyToSpawn());
                        }
                    }
                    else
                    {
                        spawnPoint.NewNPC(NPCID.Bunny);
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
                    newNPC = spawnPoint.NewNPC(NPCID.DungeonGuardian);
                }
                else if (NPC.downedBoss3 && !NPC.savedMech && Main.rand.Next(5) == 0 && !flag7 && !NPC.AnyNPCs(NPCID.BoundMechanic) && tileY > (Main.worldSurface * 4.0 + Main.rockLayer) / 5.0)
                {
                    spawnPoint.NewNPC(NPCID.BoundMechanic);
                }
                else if (flag14 && Main.rand.Next(30) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.BoneLee);
                }
                else if (flag14 && num88 == 0 && Main.rand.Next(15) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.SkeletonCommando);
                }
                else if (flag14 && num88 == 1 && Main.rand.Next(15) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.SkeletonSniper);
                }
                else if (flag14 && num88 == 2 && Main.rand.Next(15) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.TacticalSkeleton);
                }
                else if (flag14 && !NPC.AnyNPCs(NPCID.Paladin) && num88 == 0 && Main.rand.Next(35) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.Paladin);
                }
                else if (flag14 && (num88 == 1 || num88 == 2) && Main.rand.Next(30) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.GiantCursedSkull);
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
                        newNPC = spawnPoint.NewNPC(num89);
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
                    newNPC = spawnPoint.NewNPC(num90 + Main.rand.Next(4));
                }
                else if (player.RollLuck(35) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.DungeonSlime);
                }
                else if (num88 == 1 && Main.rand.Next(3) == 0 && !NPC.NearSpikeBall(tileX, tileY))
                {
                    newNPC = spawnPoint.NewNPC(NPCID.SpikeBall);
                }
                else if (num88 == 2 && Main.rand.Next(5) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.BlazingWheel);
                }
                else if (num88 == 0 && Main.rand.Next(7) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.CursedSkull);
                }
                else if (Main.rand.Next(7) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.DarkCaster);
                }
                else
                {
                    switch (Main.rand.Next(5))
                    {
                        case 0:
                            newNPC = spawnPoint.NewNPC(NPCID.AngryBonesBig);
                            break;
                        case 1:
                            newNPC = spawnPoint.NewNPC(NPCID.AngryBonesBigMuscle);
                            break;
                        case 2:
                            newNPC = spawnPoint.NewNPC(NPCID.AngryBonesBigHelmet);
                            break;
                        default:
                            newNPC = spawnPoint.NewNPC(NPCID.AngryBones);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.BigBoned);
                            }
                            else if (Main.rand.Next(5) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.ShortBones);
                            }
                            break;
                    }
                }
            }
            else if (player.ZoneMeteor)
            {
                newNPC = spawnPoint.NewNPC(NPCID.MeteorHead);
            }
            else if (DD2Event.Ongoing && player.ZoneOldOneArmy)
            {
                DD2Event.SpawnNPC(ref newNPC);
            }
            else if ((Main.remixWorld || tileY <= Main.worldSurface) && !Main.dayTime && Main.snowMoon)
            {
                int num91 = NPC.waveNumber;
                if (Main.rand.Next(30) == 0 && NPC.CountNPCS(NPCID.PresentMimic) < 4)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.PresentMimic);
                }
                else if (num91 >= 20)
                {
                    int num92 = Main.rand.Next(3);
                    if (!(num5 >= num4 * num6))
                    {
                        newNPC = num92 switch
                        {
                            0 => spawnPoint.NewNPC(NPCID.IceQueen),
                            1 => spawnPoint.NewNPC(NPCID.SantaNK1),
                            _ => spawnPoint.NewNPC(NPCID.Everscream),
                        };
                    }
                }
                else if (num91 >= 19)
                {
                    newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.IceQueen) < 4) ? spawnPoint.NewNPC(NPCID.IceQueen) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.SantaNK1) < 5) ? spawnPoint.NewNPC(NPCID.SantaNK1) : ((Main.rand.Next(10) != 0 || NPC.CountNPCS(NPCID.Everscream) >= 7) ? spawnPoint.NewNPC(NPCID.Yeti) : spawnPoint.NewNPC(NPCID.Everscream))));
                }
                else if (num91 >= 18)
                {
                    newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.IceQueen) < 3) ? spawnPoint.NewNPC(NPCID.IceQueen) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.SantaNK1) < 4) ? spawnPoint.NewNPC(NPCID.SantaNK1) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.Everscream) < 6) ? spawnPoint.NewNPC(NPCID.Everscream) : ((Main.rand.Next(3) == 0) ? spawnPoint.NewNPC(NPCID.Nutcracker) : ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(NPCID.Yeti) : spawnPoint.NewNPC(NPCID.Krampus))))));
                }
                else if (num91 >= 17)
                {
                    newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.IceQueen) < 2) ? spawnPoint.NewNPC(NPCID.IceQueen) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.SantaNK1) < 3) ? spawnPoint.NewNPC(NPCID.SantaNK1) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.Everscream) < 5) ? spawnPoint.NewNPC(NPCID.Everscream) : ((Main.rand.Next(4) == 0) ? spawnPoint.NewNPC(NPCID.ElfCopter) : ((Main.rand.Next(2) != 0) ? spawnPoint.NewNPC(NPCID.Yeti) : spawnPoint.NewNPC(NPCID.Krampus))))));
                }
                else if (num91 >= 16)
                {
                    newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.IceQueen) < 2) ? spawnPoint.NewNPC(NPCID.IceQueen) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.SantaNK1) < 2) ? spawnPoint.NewNPC(NPCID.SantaNK1) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.Everscream) < 4) ? spawnPoint.NewNPC(NPCID.Everscream) : ((Main.rand.Next(2) != 0) ? spawnPoint.NewNPC(NPCID.Yeti) : spawnPoint.NewNPC(NPCID.Flocko)))));
                }
                else if (num91 >= 15)
                {
                    newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.IceQueen)) ? spawnPoint.NewNPC(NPCID.IceQueen) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.SantaNK1) < 2) ? spawnPoint.NewNPC(NPCID.SantaNK1) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.Everscream) < 3) ? spawnPoint.NewNPC(NPCID.Everscream) : ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(NPCID.Yeti) : spawnPoint.NewNPC(NPCID.ElfCopter)))));
                }
                else
                {
                    switch (num91)
                    {
                        case 14:
                            if (Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.IceQueen))
                            {
                                newNPC = spawnPoint.NewNPC(NPCID.IceQueen);
                            }
                            else if (Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.SantaNK1))
                            {
                                newNPC = spawnPoint.NewNPC(NPCID.SantaNK1);
                            }
                            else if (Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.Everscream))
                            {
                                newNPC = spawnPoint.NewNPC(NPCID.Everscream);
                            }
                            else if (Main.rand.Next(3) == 0)
                            {
                                newNPC = spawnPoint.NewNPC(NPCID.Yeti);
                            }
                            break;
                        case 13:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.IceQueen)) ? spawnPoint.NewNPC(NPCID.IceQueen) : ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.SantaNK1)) ? spawnPoint.NewNPC(NPCID.SantaNK1) : ((Main.rand.Next(3) != 0) ? ((Main.rand.Next(6) != 0) ? ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(NPCID.ElfCopter) : spawnPoint.NewNPC(NPCID.GingerbreadMan)) : spawnPoint.NewNPC(NPCID.Yeti)) : spawnPoint.NewNPC(NPCID.Flocko))));
                            break;
                        case 12:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.IceQueen)) ? spawnPoint.NewNPC(NPCID.IceQueen) : ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.Everscream)) ? spawnPoint.NewNPC(NPCID.Everscream) : ((Main.rand.Next(8) != 0) ? ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(Main.rand.Next(338, 341)) : spawnPoint.NewNPC(NPCID.GingerbreadMan)) : spawnPoint.NewNPC(NPCID.Yeti))));
                            break;
                        case 11:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.IceQueen)) ? spawnPoint.NewNPC(NPCID.IceQueen) : ((Main.rand.Next(6) != 0) ? ((Main.rand.Next(2) != 0) ? spawnPoint.NewNPC(Main.rand.Next(338, 341)) : spawnPoint.NewNPC(NPCID.GingerbreadMan)) : spawnPoint.NewNPC(NPCID.Flocko)));
                            break;
                        case 10:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.SantaNK1)) ? spawnPoint.NewNPC(NPCID.SantaNK1) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.Everscream) < 2) ? spawnPoint.NewNPC(NPCID.Everscream) : ((Main.rand.Next(6) != 0) ? ((Main.rand.Next(3) != 0) ? ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(Main.rand.Next(338, 341)) : spawnPoint.NewNPC(NPCID.ElfCopter)) : spawnPoint.NewNPC(NPCID.Nutcracker)) : spawnPoint.NewNPC(NPCID.Krampus))));
                            break;
                        case 9:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.SantaNK1)) ? spawnPoint.NewNPC(NPCID.SantaNK1) : ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.Everscream)) ? spawnPoint.NewNPC(NPCID.Everscream) : ((Main.rand.Next(2) != 0) ? ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(NPCID.GingerbreadMan) : spawnPoint.NewNPC(NPCID.ElfCopter)) : spawnPoint.NewNPC(NPCID.Nutcracker))));
                            break;
                        case 8:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.SantaNK1)) ? spawnPoint.NewNPC(NPCID.SantaNK1) : ((Main.rand.Next(8) != 0) ? ((Main.rand.Next(3) != 0) ? ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(NPCID.ElfArcher) : spawnPoint.NewNPC(NPCID.ElfCopter)) : spawnPoint.NewNPC(NPCID.Nutcracker)) : spawnPoint.NewNPC(NPCID.Krampus)));
                            break;
                        case 7:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.SantaNK1)) ? spawnPoint.NewNPC(NPCID.SantaNK1) : ((Main.rand.Next(3) != 0) ? ((Main.rand.Next(4) != 0) ? spawnPoint.NewNPC(Main.rand.Next(338, 341)) : spawnPoint.NewNPC(NPCID.ElfArcher)) : spawnPoint.NewNPC(NPCID.GingerbreadMan)));
                            break;
                        case 6:
                            newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.Everscream) < 2) ? spawnPoint.NewNPC(NPCID.Everscream) : ((Main.rand.Next(4) != 0) ? ((Main.rand.Next(2) != 0) ? spawnPoint.NewNPC(NPCID.ElfArcher) : spawnPoint.NewNPC(NPCID.Nutcracker)) : spawnPoint.NewNPC(NPCID.ElfCopter)));
                            break;
                        case 5:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.Everscream)) ? spawnPoint.NewNPC(NPCID.Everscream) : ((Main.rand.Next(4) != 0) ? ((Main.rand.Next(8) != 0) ? spawnPoint.NewNPC(Main.rand.Next(338, 341)) : spawnPoint.NewNPC(NPCID.Nutcracker)) : spawnPoint.NewNPC(NPCID.ElfArcher)));
                            break;
                        case 4:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.Everscream)) ? spawnPoint.NewNPC(NPCID.Everscream) : ((Main.rand.Next(4) != 0) ? ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(Main.rand.Next(338, 341)) : spawnPoint.NewNPC(NPCID.GingerbreadMan)) : spawnPoint.NewNPC(NPCID.ElfArcher)));
                            break;
                        case 3:
                            newNPC = ((Main.rand.Next(8) != 0) ? ((Main.rand.Next(4) != 0) ? ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(Main.rand.Next(338, 341)) : spawnPoint.NewNPC(NPCID.GingerbreadMan)) : spawnPoint.NewNPC(NPCID.ElfArcher)) : spawnPoint.NewNPC(NPCID.Nutcracker));
                            break;
                        case 2:
                            newNPC = ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(Main.rand.Next(338, 341)) : spawnPoint.NewNPC(NPCID.ElfArcher));
                            break;
                        default:
                            newNPC = ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(Main.rand.Next(338, 341)) : spawnPoint.NewNPC(NPCID.GingerbreadMan));
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
                        if (Main.rand.Next(2) == 0 && NPC.CountNPCS(NPCID.Pumpking) < 2)
                        {
                            newNPC = spawnPoint.NewNPC(NPCID.Pumpking);
                        }
                        else if (Main.rand.Next(3) != 0 && NPC.CountNPCS(NPCID.MourningWood) < 2)
                        {
                            newNPC = spawnPoint.NewNPC(NPCID.MourningWood);
                        }
                        else if (NPC.CountNPCS(NPCID.HeadlessHorseman) < 3)
                        {
                            newNPC = spawnPoint.NewNPC(NPCID.HeadlessHorseman);
                        }
                    }
                }
                else
                {
                    switch (num93)
                    {
                        case 19:
                            if (Main.rand.Next(5) == 0 && NPC.CountNPCS(NPCID.Pumpking) < 2)
                            {
                                newNPC = spawnPoint.NewNPC(NPCID.Pumpking);
                            }
                            else if (Main.rand.Next(5) == 0 && NPC.CountNPCS(NPCID.MourningWood) < 2)
                            {
                                newNPC = spawnPoint.NewNPC(NPCID.MourningWood);
                            }
                            else if (!(num5 >= num4 * num6) && NPC.CountNPCS(NPCID.HeadlessHorseman) < 5)
                            {
                                newNPC = spawnPoint.NewNPC(NPCID.HeadlessHorseman);
                            }
                            break;
                        case 18:
                            if (Main.rand.Next(7) == 0 && NPC.CountNPCS(NPCID.Pumpking) < 2)
                            {
                                newNPC = spawnPoint.NewNPC(NPCID.Pumpking);
                            }
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(NPCID.MourningWood) < 2) ? spawnPoint.NewNPC(NPCID.MourningWood) : ((Main.rand.Next(7) != 0 || NPC.CountNPCS(NPCID.HeadlessHorseman) >= 3) ? spawnPoint.NewNPC(NPCID.Poltergeist) : spawnPoint.NewNPC(NPCID.HeadlessHorseman)));
                            break;
                        case 17:
                            if (Main.rand.Next(7) == 0 && NPC.CountNPCS(NPCID.Pumpking) < 2)
                            {
                                newNPC = spawnPoint.NewNPC(NPCID.Pumpking);
                            }
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(NPCID.MourningWood) < 2) ? spawnPoint.NewNPC(NPCID.MourningWood) : ((Main.rand.Next(7) == 0 && NPC.CountNPCS(NPCID.HeadlessHorseman) < 2) ? spawnPoint.NewNPC(NPCID.HeadlessHorseman) : ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(NPCID.Hellhound) : spawnPoint.NewNPC(NPCID.Poltergeist))));
                            break;
                        case 16:
                            newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.Pumpking) < 2) ? spawnPoint.NewNPC(NPCID.Pumpking) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.HeadlessHorseman) < 2) ? spawnPoint.NewNPC(NPCID.HeadlessHorseman) : ((Main.rand.Next(6) != 0) ? ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(NPCID.Splinterling) : spawnPoint.NewNPC(NPCID.Hellhound)) : spawnPoint.NewNPC(NPCID.Poltergeist))));
                            break;
                        case 15:
                            if (Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.Pumpking))
                            {
                                newNPC = spawnPoint.NewNPC(NPCID.Pumpking);
                            }
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(NPCID.MourningWood) < 2) ? spawnPoint.NewNPC(NPCID.MourningWood) : ((Main.rand.Next(5) != 0) ? ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(Main.rand.Next(305, 315)) : spawnPoint.NewNPC(NPCID.Splinterling)) : spawnPoint.NewNPC(NPCID.Poltergeist)));
                            break;
                        case 14:
                            if (Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.Pumpking))
                            {
                                newNPC = spawnPoint.NewNPC(NPCID.Pumpking);
                            }
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(NPCID.MourningWood) < 2) ? spawnPoint.NewNPC(NPCID.MourningWood) : ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.HeadlessHorseman)) ? spawnPoint.NewNPC(NPCID.HeadlessHorseman) : ((Main.rand.Next(10) != 0) ? ((Main.rand.Next(7) != 0) ? ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(Main.rand.Next(305, 315)) : spawnPoint.NewNPC(NPCID.Splinterling)) : spawnPoint.NewNPC(NPCID.Hellhound)) : spawnPoint.NewNPC(NPCID.Poltergeist))));
                            break;
                        case 13:
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(NPCID.MourningWood) < 2) ? spawnPoint.NewNPC(NPCID.MourningWood) : ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.HeadlessHorseman) < 2) ? spawnPoint.NewNPC(NPCID.HeadlessHorseman) : ((Main.rand.Next(6) != 0) ? ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(NPCID.Splinterling) : spawnPoint.NewNPC(NPCID.Hellhound)) : spawnPoint.NewNPC(NPCID.Poltergeist))));
                            break;
                        case 12:
                            newNPC = ((Main.rand.Next(5) != 0 || NPC.AnyNPCs(NPCID.Pumpking)) ? spawnPoint.NewNPC(NPCID.Poltergeist) : spawnPoint.NewNPC(NPCID.Pumpking));
                            break;
                        case 11:
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(NPCID.MourningWood) < 2) ? spawnPoint.NewNPC(NPCID.MourningWood) : ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(NPCID.Splinterling) : spawnPoint.NewNPC(NPCID.Poltergeist)));
                            break;
                        case 10:
                            newNPC = ((Main.rand.Next(10) == 0 && !NPC.AnyNPCs(NPCID.Pumpking)) ? spawnPoint.NewNPC(NPCID.Pumpking) : ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(Main.rand.Next(305, 315)) : spawnPoint.NewNPC(NPCID.Hellhound)));
                            break;
                        case 9:
                            newNPC = ((Main.rand.Next(10) == 0 && NPC.CountNPCS(NPCID.MourningWood) < 2) ? spawnPoint.NewNPC(NPCID.MourningWood) : ((Main.rand.Next(8) != 0) ? ((Main.rand.Next(5) != 0) ? ((Main.rand.Next(2) != 0) ? spawnPoint.NewNPC(Main.rand.Next(305, 315)) : spawnPoint.NewNPC(NPCID.Splinterling)) : spawnPoint.NewNPC(NPCID.Hellhound)) : spawnPoint.NewNPC(NPCID.Poltergeist)));
                            break;
                        case 8:
                            newNPC = ((Main.rand.Next(8) == 0 && NPC.CountNPCS(NPCID.HeadlessHorseman) < 2) ? spawnPoint.NewNPC(NPCID.HeadlessHorseman) : ((Main.rand.Next(4) != 0) ? spawnPoint.NewNPC(NPCID.Hellhound) : spawnPoint.NewNPC(NPCID.Poltergeist)));
                            break;
                        case 7:
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(NPCID.MourningWood) < 2) ? spawnPoint.NewNPC(NPCID.MourningWood) : ((Main.rand.Next(4) != 0) ? spawnPoint.NewNPC(NPCID.Hellhound) : spawnPoint.NewNPC(NPCID.Poltergeist)));
                            break;
                        case 6:
                            newNPC = ((Main.rand.Next(7) == 0 && NPC.CountNPCS(NPCID.MourningWood) < 2) ? spawnPoint.NewNPC(NPCID.MourningWood) : ((Main.rand.Next(2) != 0) ? spawnPoint.NewNPC(Main.rand.Next(305, 315)) : spawnPoint.NewNPC(NPCID.Splinterling)));
                            break;
                        case 5:
                            newNPC = ((Main.rand.Next(10) != 0 || NPC.AnyNPCs(NPCID.HeadlessHorseman)) ? spawnPoint.NewNPC(NPCID.Hellhound) : spawnPoint.NewNPC(NPCID.HeadlessHorseman));
                            break;
                        case 4:
                            newNPC = ((Main.rand.Next(8) == 0 && !NPC.AnyNPCs(NPCID.MourningWood)) ? spawnPoint.NewNPC(NPCID.Poltergeist) : ((Main.rand.Next(2) != 0) ? spawnPoint.NewNPC(Main.rand.Next(305, 315)) : spawnPoint.NewNPC(NPCID.Splinterling)));
                            break;
                        case 3:
                            newNPC = ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(NPCID.Splinterling) : spawnPoint.NewNPC(NPCID.Hellhound));
                            break;
                        case 2:
                            newNPC = ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(Main.rand.Next(305, 315)) : spawnPoint.NewNPC(NPCID.Splinterling));
                            break;
                        default:
                            newNPC = spawnPoint.NewNPC(Main.rand.Next(305, 315));
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
                newNPC = ((NPC.downedPlantBoss && Main.rand.Next(80) == 0 && !NPC.AnyNPCs(NPCID.Mothron))
                    ? spawnPoint.NewNPC(NPCID.Mothron)
                    : ((Main.rand.Next(50) == 0 && !NPC.AnyNPCs(NPCID.Eyezor))
                    ? spawnPoint.NewNPC(NPCID.Eyezor)
                    : ((NPC.downedPlantBoss && Main.rand.Next(5) == 0 && !NPC.AnyNPCs(NPCID.Psycho))
                    ? spawnPoint.NewNPC(NPCID.Psycho)
                    : ((NPC.downedPlantBoss && Main.rand.Next(20) == 0 && !NPC.AnyNPCs(NPCID.Nailhead))
                    ? spawnPoint.NewNPC(NPCID.Nailhead)
                    : ((NPC.downedPlantBoss && Main.rand.Next(20) == 0 && NPC.CountNPCS(NPCID.DeadlySphere) < 2)
                    ? spawnPoint.NewNPC(NPCID.DeadlySphere)
                    : ((Main.rand.Next(15) == 0)
                    ? spawnPoint.NewNPC(NPCID.Vampire)
                    : ((flag35 && Main.rand.Next(13) == 0)
                    ? spawnPoint.NewNPC(NPCID.Reaper)
                    : ((Main.rand.Next(8) == 0)
                    ? spawnPoint.NewNPC(NPCID.ThePossessed)
                    : ((NPC.downedPlantBoss && Main.rand.Next(7) == 0)
                    ? spawnPoint.NewNPC(NPCID.DrManFly)
                    : ((NPC.downedPlantBoss && Main.rand.Next(5) == 0)
                    ? spawnPoint.NewNPC(NPCID.Butcher)
                    : ((Main.rand.Next(4) == 0)
                    ? spawnPoint.NewNPC(NPCID.Frankenstein)
                    : ((Main.rand.Next(3) == 0)
                    ? spawnPoint.NewNPC(NPCID.CreatureFromTheDeep)
                    : ((Main.rand.Next(2) != 0)
                    ? spawnPoint.NewNPC(NPCID.SwampThing)
                    : spawnPoint.NewNPC(NPCID.Fritz))))))))))))));
            }
            else if (NPC.SpawnNPC_CheckToSpawnUndergroundFairy(tileX, tileY, k))
            {
                int num94 = Main.rand.Next(583, 586);
                if (Main.tenthAnniversaryWorld && !Main.getGoodWorld && Main.rand.Next(4) != 0)
                {
                    num94 = 583;
                }
                newNPC = spawnPoint.NewNPC(num94);
                Main.npc[newNPC].ai[2] = 2f;
                Main.npc[newNPC].TargetClosest();
                Main.npc[newNPC].ai[3] = 0f;
            }
            else if (!Main.remixWorld && !flag7 && (!Main.dayTime || Main.tile[tileX, tileY].wall > 0) && Main.tile[num8, num9].wall == WallID.LivingWoodUnsafe && !Main.eclipse && !Main.bloodMoon && player.RollLuck(30) == 0 && NPC.CountNPCS(NPCID.Gnome) <= Main.rand.Next(3))
            {
                newNPC = spawnPoint.NewNPC(NPCID.Gnome);
            }
            else if (!player.ZoneCorrupt && !player.ZoneCrimson && !flag7 && !Main.eclipse && !Main.bloodMoon && player.RollLuck(range) == 0 && ((!Main.remixWorld && tileY >= Main.worldSurface * 0.800000011920929 && tileY < Main.worldSurface * 1.100000023841858) || (Main.remixWorld && tileY > Main.rockLayer && tileY < Main.maxTilesY - 350)) && NPC.CountNPCS(NPCID.Gnome) <= Main.rand.Next(3) && (!Main.dayTime || Main.tile[tileX, tileY].wall > 0) && (Main.tile[tileX, tileY].wall == WallID.GrassUnsafe || Main.tile[tileX, tileY].wall == WallID.DirtUnsafe || Main.tile[tileX, tileY].wall == WallID.DirtUnsafe1 || Main.tile[tileX, tileY].wall == WallID.DirtUnsafe2 || Main.tile[tileX, tileY].wall == WallID.DirtUnsafe3 || Main.tile[tileX, tileY].wall == WallID.DirtUnsafe4))
            {
                newNPC = spawnPoint.NewNPC(NPCID.Gnome);
            }
            else if (Main.hardMode && num3 == 70 && flag7)
            {
                newNPC = spawnPoint.NewNPC(NPCID.FungoFish);
            }
            else if (num3 == 70 && tileY <= Main.worldSurface && Main.rand.Next(3) != 0)
            {
                if ((!Main.hardMode && Main.rand.Next(6) == 0) || Main.rand.Next(12) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.GlowingSnail);
                }
                else if (Main.rand.Next(3) != 0)
                {
                    newNPC = ((Main.rand.Next(2) != 0) ? spawnPoint.NewNPC(NPCID.ZombieMushroomHat) : spawnPoint.NewNPC(NPCID.ZombieMushroom));
                }
                else if (Main.rand.Next(4) != 0)
                {
                    newNPC = ((Main.rand.Next(2) != 0) ? spawnPoint.NewNPC(NPCID.MushiLadybug) : spawnPoint.NewNPC(NPCID.AnomuraFungus));
                }
                else
                {
                    newNPC = spawnPoint.NewNPC((Main.hardMode && Main.rand.Next(3) != 0) ? 260 : 259);
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
                    newNPC = spawnPoint.NewNPC(NPCID.TruffleWorm);
                }
                else if ((!Main.hardMode && Main.rand.Next(4) == 0) || Main.rand.Next(8) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.GlowingSnail);
                }
                else if (Main.rand.Next(4) != 0)
                {
                    newNPC = spawnPoint.NewNPC((Main.rand.Next(2) != 0) ? NPCID.MushiLadybug : NPCID.AnomuraFungus);
                }
                else
                {
                    newNPC = spawnPoint.NewNPC((Main.hardMode && Main.rand.Next(3) != 0) ? NPCID.GiantFungiBulb : NPCID.FungiBulb);
                    Main.npc[newNPC].ai[0] = tileX;
                    Main.npc[newNPC].ai[1] = tileY;
                    Main.npc[newNPC].netUpdate = true;
                }
            }
            else if (player.ZoneCorrupt && Main.rand.Next(maxValue) == 0 && !flag5)
            {
                newNPC = ((!Main.hardMode || Main.rand.Next(4) == 0) ? spawnPoint.NewNPC(NPCID.DevourerHead, 1) : spawnPoint.NewNPC(NPCID.SeekerHead, 1));
            }
            else if (Main.remixWorld && !Main.hardMode && tileY > Main.worldSurface && player.RollLuck(100) == 0)
            {
                newNPC = ((!player.ZoneSnow) ? spawnPoint.NewNPC(NPCID.Mimic) : spawnPoint.NewNPC(NPCID.IceMimic));
            }
            else if (Main.hardMode && tileY > Main.worldSurface && player.RollLuck(Main.tenthAnniversaryWorld ? 25 : 75) == 0)
            {
                newNPC = ((Main.rand.Next(2) == 0 && player.ZoneCorrupt && !NPC.AnyNPCs(NPCID.BigMimicCorruption)) ? spawnPoint.NewNPC(NPCID.BigMimicCorruption) : ((Main.rand.Next(2) == 0 && player.ZoneCrimson && !NPC.AnyNPCs(NPCID.BigMimicCrimson)) ? spawnPoint.NewNPC(NPCID.BigMimicCrimson) : ((Main.rand.Next(2) == 0 && player.ZoneHallow && !NPC.AnyNPCs(NPCID.BigMimicHallow)) ? spawnPoint.NewNPC(NPCID.BigMimicHallow) : ((Main.tenthAnniversaryWorld && Main.rand.Next(2) == 0 && player.ZoneJungle && !NPC.AnyNPCs(NPCID.BigMimicJungle)) ? spawnPoint.NewNPC(NPCID.BigMimicJungle) : ((!player.ZoneSnow) ? spawnPoint.NewNPC(NPCID.Mimic) : spawnPoint.NewNPC(NPCID.IceMimic))))));
            }
            else if (Main.hardMode && Main.tile[tileX, tileY].wall == WallID.DirtUnsafe && Main.rand.Next(20) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.Mimic);
            }
            else if (Main.hardMode && tileY <= Main.worldSurface && !Main.dayTime && (Main.rand.Next(20) == 0 || (Main.rand.Next(5) == 0 && Main.moonPhase == 4)))
            {
                newNPC = spawnPoint.NewNPC(NPCID.Wraith);
            }
            else if (Main.hardMode && Main.halloween && tileY <= Main.worldSurface && !Main.dayTime && Main.rand.Next(10) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.HoppinJack);
            }
            else if (num45 == 60 && player.RollLuck(500) == 0 && !Main.dayTime)
            {
                newNPC = spawnPoint.NewNPC(NPCID.DoctorBones);
            }
            else if (num45 == 60 && tileY > Main.worldSurface && Main.rand.Next(60) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.LacBeetle);
            }
            else if (tileY > Main.worldSurface && tileY < Main.maxTilesY - 210 && !player.ZoneSnow && !player.ZoneCrimson && !player.ZoneCorrupt && !player.ZoneJungle && !player.ZoneHallow && Main.rand.Next(8) == 0)
            {
                if (player.RollLuck(NPC.goldCritterChance) == 0)
                {
                    spawnPoint.NewNPC(NPCID.GoldWorm);
                }
                else
                {
                    newNPC = spawnPoint.NewNPC(NPCID.Worm);
                }
            }
            else if (tileY > Main.worldSurface && tileY < Main.maxTilesY - 210 && !player.ZoneSnow && !player.ZoneCrimson && !player.ZoneCorrupt && !player.ZoneJungle && !player.ZoneHallow && Main.rand.Next(13) == 0)
            {
                if (player.RollLuck(NPC.goldCritterChance) == 0)
                {
                    spawnPoint.NewNPC(NPCID.GoldMouse);
                }
                else
                {
                    newNPC = spawnPoint.NewNPC(NPCID.Mouse);
                }
            }
            else if (tileY > Main.worldSurface && tileY < (Main.rockLayer + Main.maxTilesY) / 2.0 && !player.ZoneSnow && !player.ZoneCrimson && !player.ZoneCorrupt && !player.ZoneHallow && Main.rand.Next(13) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.Snail);
            }
            else if (flag20 && player.ZoneJungle && !player.ZoneCrimson && !player.ZoneCorrupt && Main.rand.Next(7) == 0)
            {
                if (Main.dayTime && Main.time < 43200.00064373016 && Main.rand.Next(3) != 0)
                {
                    spawnPoint.NewNPC(671 + Main.rand.Next(5));
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
                    newNPC = spawnPoint.NewNPC(NPCID.MossHornet);
                    if (Main.rand.Next(10) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(NPCID.TinyMossHornet);
                    }
                    if (Main.rand.Next(10) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(NPCID.LittleMossHornet);
                    }
                    if (Main.rand.Next(10) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(NPCID.BigMossHornet);
                    }
                    if (Main.rand.Next(10) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(NPCID.GiantMossHornet);
                    }
                }
                else
                {
                    switch (Main.rand.Next(8))
                    {
                        case 0:
                            newNPC = spawnPoint.NewNPC(NPCID.HornetFatty);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.LittleHornetFatty);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.BigHornetFatty);
                            }
                            break;
                        case 1:
                            newNPC = spawnPoint.NewNPC(NPCID.HornetHoney);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.LittleHornetHoney);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.BigHornetHoney);
                            }
                            break;
                        case 2:
                            newNPC = spawnPoint.NewNPC(NPCID.HornetLeafy);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.LittleHornetLeafy);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.BigHornetLeafy);
                            }
                            break;
                        case 3:
                            newNPC = spawnPoint.NewNPC(NPCID.HornetSpikey);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.LittleHornetSpikey);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.BigHornetSpikey);
                            }
                            break;
                        case 4:
                            newNPC = spawnPoint.NewNPC(NPCID.HornetStingy);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.LittleHornetStingy);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.BigHornetStingy);
                            }
                            break;
                        default:
                            newNPC = spawnPoint.NewNPC(NPCID.Hornet);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.LittleStinger);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.BigStinger);
                            }
                            break;
                    }
                }
            }
            else if (num45 == 60 && Main.hardMode && Main.rand.Next(3) != 0)
            {
                if (flag20 && !Main.dayTime && Main.rand.Next(3) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.GiantFlyingFox);
                }
                else if (flag20 && Main.dayTime && Main.rand.Next(4) != 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.Derpling);
                }
                else if (tileY > Main.worldSurface && Main.rand.Next(100) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.Moth);
                }
                else if (tileY > Main.worldSurface && Main.rand.Next(5) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.JungleCreeper);
                }
                else if (tileY > Main.worldSurface && Main.rand.Next(4) != 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.MossHornet);
                    if (Main.rand.Next(10) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(NPCID.TinyMossHornet);
                    }
                    if (Main.rand.Next(10) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(NPCID.LittleMossHornet);
                    }
                    if (Main.rand.Next(10) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(NPCID.BigMossHornet);
                    }
                    if (Main.rand.Next(10) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(NPCID.GiantMossHornet);
                    }
                }
                else if (Main.rand.Next(3) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.AngryTrapper);
                    Main.npc[newNPC].ai[0] = tileX;
                    Main.npc[newNPC].ai[1] = tileY;
                    Main.npc[newNPC].netUpdate = true;
                }
                else
                {
                    newNPC = spawnPoint.NewNPC(NPCID.GiantTortoise);
                }
            }
            else if (((num45 == 226 || num45 == 232) && flag4) || (Main.remixWorld && flag4))
            {
                newNPC = ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(NPCID.Lihzahrd) : spawnPoint.NewNPC(NPCID.FlyingSnake));
            }
            else if (num46 == 86 && Main.rand.Next(8) != 0)
            {
                switch (Main.rand.Next(8))
                {
                    case 0:
                        newNPC = spawnPoint.NewNPC(NPCID.HornetFatty);
                        if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(NPCID.LittleHornetFatty);
                        }
                        else if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(NPCID.BigHornetFatty);
                        }
                        break;
                    case 1:
                        newNPC = spawnPoint.NewNPC(NPCID.HornetHoney);
                        if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(NPCID.LittleHornetHoney);
                        }
                        else if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(NPCID.BigHornetHoney);
                        }
                        break;
                    case 2:
                        newNPC = spawnPoint.NewNPC(NPCID.HornetLeafy);
                        if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(NPCID.LittleHornetLeafy);
                        }
                        else if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(NPCID.BigHornetLeafy);
                        }
                        break;
                    case 3:
                        newNPC = spawnPoint.NewNPC(NPCID.HornetSpikey);
                        if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(NPCID.LittleHornetSpikey);
                        }
                        else if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(NPCID.BigHornetSpikey);
                        }
                        break;
                    case 4:
                        newNPC = spawnPoint.NewNPC(NPCID.HornetStingy);
                        if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(NPCID.LittleHornetStingy);
                        }
                        else if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(NPCID.BigHornetStingy);
                        }
                        break;
                    default:
                        newNPC = spawnPoint.NewNPC(NPCID.Hornet);
                        if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(NPCID.LittleStinger);
                        }
                        else if (Main.rand.Next(4) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(NPCID.BigStinger);
                        }
                        break;
                }
            }
            else if (num45 == 60 && ((!Main.remixWorld && tileY > (Main.worldSurface + Main.rockLayer) / 2.0) || (Main.remixWorld && (tileY < Main.rockLayer || Main.rand.Next(2) == 0))))
            {
                if (Main.rand.Next(4) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.SpikedJungleSlime);
                }
                else if (Main.rand.Next(4) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.ManEater);
                    Main.npc[newNPC].ai[0] = tileX;
                    Main.npc[newNPC].ai[1] = tileY;
                    Main.npc[newNPC].netUpdate = true;
                }
                else
                {
                    switch (Main.rand.Next(8))
                    {
                        case 0:
                            newNPC = spawnPoint.NewNPC(NPCID.HornetFatty);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.LittleHornetFatty);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.BigHornetFatty);
                            }
                            break;
                        case 1:
                            newNPC = spawnPoint.NewNPC(NPCID.HornetHoney);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.LittleHornetHoney);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.BigHornetHoney);
                            }
                            break;
                        case 2:
                            newNPC = spawnPoint.NewNPC(NPCID.HornetLeafy);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.LittleHornetLeafy);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.BigHornetLeafy);
                            }
                            break;
                        case 3:
                            newNPC = spawnPoint.NewNPC(NPCID.HornetSpikey);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.LittleHornetSpikey);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.BigHornetSpikey);
                            }
                            break;
                        case 4:
                            newNPC = spawnPoint.NewNPC(NPCID.HornetStingy);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.LittleHornetStingy);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.BigHornetStingy);
                            }
                            break;
                        default:
                            newNPC = spawnPoint.NewNPC(NPCID.Hornet);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.LittleStinger);
                            }
                            else if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.BigStinger);
                            }
                            break;
                    }
                }
            }
            else if (num45 == 60 && Main.rand.Next(4) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.JungleBat);
            }
            else if (num45 == 60 && Main.rand.Next(8) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.Snatcher);
                Main.npc[newNPC].ai[0] = tileX;
                Main.npc[newNPC].ai[1] = tileY;
                Main.npc[newNPC].netUpdate = true;
            }
            else if (Sandstorm.Happening && player.ZoneSandstorm && TileID.Sets.Conversion.Sand[num45] && NPC.Spawning_SandstoneCheck(tileX, tileY))
            {
                if (!NPC.downedBoss1 && !Main.hardMode)
                {
                    newNPC = ((Main.rand.Next(2) == 0) ? spawnPoint.NewNPC(NPCID.Tumbleweed) : ((Main.rand.Next(2) != 0) ? spawnPoint.NewNPC(NPCID.Antlion) : spawnPoint.NewNPC(NPCID.Vulture)));
                }
                else if (Main.hardMode && Main.rand.Next(20) == 0 && !NPC.AnyNPCs(NPCID.SandElemental))
                {
                    newNPC = spawnPoint.NewNPC(NPCID.SandElemental);
                }
                else if (Main.hardMode && !flag5 && Main.rand.Next(3) == 0 && NPC.CountNPCS(NPCID.DuneSplicerHead) < 4)
                {
                    newNPC = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, (tileY + 10) * 16, NPCID.DuneSplicerHead);
                }
                else if (!Main.hardMode || flag5 || Main.rand.Next(2) != 0)
                {
                    newNPC = ((Main.hardMode && num45 == 53 && Main.rand.Next(3) == 0) ? spawnPoint.NewNPC(NPCID.Mummy) : ((Main.hardMode && num45 == 112 && Main.rand.Next(3) == 0) ? spawnPoint.NewNPC(NPCID.DarkMummy) : ((Main.hardMode && num45 == 234 && Main.rand.Next(3) == 0) ? spawnPoint.NewNPC(NPCID.BloodMummy) : ((Main.hardMode && num45 == 116 && Main.rand.Next(3) == 0) ? spawnPoint.NewNPC(NPCID.LightMummy) : ((Main.rand.Next(2) == 0) ? spawnPoint.NewNPC(NPCID.Tumbleweed) : ((Main.rand.Next(2) != 0) ? spawnPoint.NewNPC(NPCID.FlyingAntlion) : spawnPoint.NewNPC(NPCID.WalkingAntlion)))))));
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
                    newNPC = spawnPoint.NewNPC(num95);
                }
            }
            else if (Main.hardMode && num45 == 53 && Main.rand.Next(3) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.Mummy);
            }
            else if (Main.hardMode && num45 == 112 && Main.rand.Next(2) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.DarkMummy);
            }
            else if (Main.hardMode && num45 == 234 && Main.rand.Next(2) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.BloodMummy);
            }
            else if (Main.hardMode && num45 == 116 && Main.rand.Next(2) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.LightMummy);
            }
            else if (Main.hardMode && !flag7 && flag17 && (num45 == 116 || num45 == 117 || num45 == 109 || num45 == 164))
            {
                if (NPC.downedPlantBoss && (Main.remixWorld || (!Main.dayTime && Main.time < 16200.0)) && flag20 && player.RollLuck(10) == 0 && !NPC.AnyNPCs(NPCID.EmpressButterfly))
                {
                    newNPC = spawnPoint.NewNPC(NPCID.EmpressButterfly);
                }
                else if (!flag24 || NPC.AnyNPCs(NPCID.RainbowSlime) || Main.rand.Next(12) != 0)
                {
                    newNPC = ((!Main.dayTime && Main.rand.Next(2) == 0) ? spawnPoint.NewNPC(NPCID.Gastropod) : ((Main.rand.Next(10) != 0 && (!player.ZoneWaterCandle || Main.rand.Next(10) != 0)) ? spawnPoint.NewNPC(NPCID.Pixie) : spawnPoint.NewNPC(NPCID.Unicorn)));
                }
                else
                {
                    spawnPoint.NewNPC(NPCID.RainbowSlime);
                }
            }
            else if (!flag5 && Main.hardMode && Main.rand.Next(50) == 0 && !flag7 && flag21 && (num45 == 116 || num45 == 117 || num45 == 109 || num45 == 164))
            {
                newNPC = spawnPoint.NewNPC(NPCID.EnchantedSword);
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
                    newNPC = spawnPoint.NewNPC(NPCID.CrimsonAxe);
                }
                else if (Main.hardMode && flag36 && Main.rand.Next(5) == 0 && !flag5)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.FloatyGross);
                }
                else if (Main.hardMode && flag36 && Main.rand.Next(2) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.IchorSticker);
                }
                else if (Main.hardMode && Main.rand.Next(3) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.Crimslime);
                    if (Main.rand.Next(3) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(NPCID.LittleCrimslime);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(NPCID.BigCrimslime);
                    }
                }
                else if (Main.hardMode && (Main.rand.Next(2) == 0 || (tileY > Main.worldSurface && !Main.remixWorld)))
                {
                    newNPC = spawnPoint.NewNPC(NPCID.Herpling);
                }
                else if ((Main.tile[tileX, tileY].wall > 0 && Main.rand.Next(4) != 0) || Main.rand.Next(8) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.BloodCrawler);
                }
                else if (Main.rand.Next(2) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.FaceMonster);
                }
                else
                {
                    newNPC = spawnPoint.NewNPC(NPCID.Crimera);
                    if (Main.rand.Next(3) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(NPCID.LittleCrimera);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(NPCID.BigCrimera);
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
                    newNPC = spawnPoint.NewNPC(NPCID.CursedHammer);
                }
                else if (Main.hardMode && flag37 && Main.rand.Next(3) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.Clinger);
                    Main.npc[newNPC].ai[0] = tileX;
                    Main.npc[newNPC].ai[1] = tileY;
                    Main.npc[newNPC].netUpdate = true;
                }
                else if (Main.hardMode && Main.rand.Next(3) == 0)
                {
                    newNPC = ((Main.rand.Next(3) != 0) ? spawnPoint.NewNPC(NPCID.CorruptSlime) : spawnPoint.NewNPC(NPCID.Slimer));
                }
                else if (Main.hardMode && (Main.rand.Next(2) == 0 || flag37))
                {
                    newNPC = spawnPoint.NewNPC(NPCID.Corruptor);
                }
                else
                {
                    newNPC = spawnPoint.NewNPC(NPCID.EaterofSouls);
                    if (Main.rand.Next(3) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(NPCID.LittleEater);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(NPCID.BigEater);
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
                        spawnPoint.NewNPC(NPCID.Maggot);
                    }
                    else
                    {
                        spawnPoint.NewNPC(NPCID.Rat);
                    }
                }
                else if (player.ZoneSnow && Main.hardMode && flag24 && !NPC.AnyNPCs(NPCID.IceGolem) && player.RollLuck(20) == 0)
                {
                    spawnPoint.NewNPC(NPCID.IceGolem);
                }
                else if (!player.ZoneSnow && Main.hardMode && flag24 && NPC.CountNPCS(NPCID.AngryNimbus) < 2 && Main.rand.Next(10) == 0)
                {
                    spawnPoint.NewNPC(NPCID.AngryNimbus);
                }
                else if (flag38 && Main.hardMode && NPC.downedGolemBoss && ((!NPC.downedMartians && Main.rand.Next(100) == 0) || Main.rand.Next(400) == 0) && !NPC.AnyNPCs(NPCID.MartianProbe))
                {
                    spawnPoint.NewNPC(NPCID.MartianProbe);
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
                                spawnPoint.NewNPC(NPCID.Penguin);
                            }
                            else
                            {
                                spawnPoint.NewNPC(NPCID.PenguinBlack);
                            }
                        }
                        else if (!tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(NPC.stinkBugChance) == 0 && flag20)
                        {
                            spawnPoint.NewNPC(NPCID.Stinkbug);
                            if (Main.rand.Next(4) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX - 16, spawnY, NPCID.Stinkbug);
                            }
                            if (Main.rand.Next(4) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + 16, spawnY, NPCID.Stinkbug);
                            }
                        }
                        else if (!tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(NPC.butterflyChance) == 0 && flag20)
                        {
                            if (player.RollLuck(NPC.goldCritterChance) == 0)
                            {
                                spawnPoint.NewNPC(NPCID.GoldButterfly);
                            }
                            else
                            {
                                spawnPoint.NewNPC(NPCID.Butterfly);
                            }
                            if (Main.rand.Next(4) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX - 16, spawnY, NPCID.Butterfly);
                            }
                            if (Main.rand.Next(4) == 0)
                            {
                                NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX + 16, spawnY, NPCID.Butterfly);
                            }
                        }
                        else if (tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(NPC.butterflyChance / 2) == 0 && flag20)
                        {
                            if (player.RollLuck(NPC.goldCritterChance) == 0)
                            {
                                spawnPoint.NewNPC(NPCID.GoldLadyBug);
                            }
                            else
                            {
                                spawnPoint.NewNPC(NPCID.LadyBug);
                            }
                            if (Main.rand.Next(3) != 0)
                            {
                                spawnPoint.NewNPC(NPCID.LadyBug);
                            }
                            if (Main.rand.Next(2) == 0)
                            {
                                spawnPoint.NewNPC(NPCID.LadyBug);
                            }
                            if (Main.rand.Next(3) == 0)
                            {
                                spawnPoint.NewNPC(NPCID.LadyBug);
                            }
                            if (Main.rand.Next(4) == 0)
                            {
                                spawnPoint.NewNPC(NPCID.LadyBug);
                            }
                        }
                        else if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            spawnPoint.NewNPC(NPCID.GoldBunny);
                        }
                        else if (player.RollLuck(NPC.goldCritterChance) == 0 && tileY <= Main.worldSurface)
                        {
                            spawnPoint.NewNPC(NPCID.SquirrelGold);
                        }
                        else if (Main.halloween && Main.rand.Next(3) != 0)
                        {
                            spawnPoint.NewNPC(NPCID.BunnySlimed);
                        }
                        else if (Main.xMas && Main.rand.Next(3) != 0)
                        {
                            spawnPoint.NewNPC(NPCID.BunnyXmas);
                        }
                        else if (BirthdayParty.PartyIsUp && Main.rand.Next(3) != 0)
                        {
                            spawnPoint.NewNPC(NPCID.PartyBunny);
                        }
                        else if (Main.rand.Next(3) == 0 && tileY <= Main.worldSurface)
                        {
                            spawnPoint.NewNPC(Utils.SelectRandom(Main.rand, new short[2] { 299, 538 }));
                        }
                        else
                        {
                            spawnPoint.NewNPC(NPCID.Bunny);
                        }
                    }
                    else if (!flag7 && tileX > WorldGen.beachDistance && tileX < Main.maxTilesX - WorldGen.beachDistance && Main.rand.Next(12) == 0 && num45 == 53)
                    {
                        spawnPoint.NewNPC(Main.rand.Next(366, 368));
                    }
                    else if ((num45 == 2 || num45 == 477 || num45 == 53) && !tooWindyForButterflies && !Main.raining && Main.dayTime && Main.rand.Next(3) != 0 && (tileY <= Main.worldSurface || Main.remixWorld) && NPC.FindCattailTop(tileX, tileY, out cattailX, out cattailY))
                    {
                        if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), cattailX * 16 + 8, cattailY * 16, NPCID.GoldDragonfly);
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
                    else if (!flag7 && num96 < Main.maxTilesX / 3 && Main.dayTime && Main.time < 18000.0 && (num45 == 2 || num45 == 477 || num45 == 109 || num45 == 492) && Main.rand.Next(4) == 0 && tileY <= Main.worldSurface && NPC.CountNPCS(NPCID.Bird) + NPC.CountNPCS(NPCID.BirdBlue) + NPC.CountNPCS(NPCID.BirdRed) < 6)
                    {
                        int num97 = Main.rand.Next(4);
                        if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            spawnPoint.NewNPC(NPCID.GoldBird);
                        }
                        else
                        {
                            switch (num97)
                            {
                                case 0:
                                    spawnPoint.NewNPC(NPCID.BirdBlue);
                                    break;
                                case 1:
                                    spawnPoint.NewNPC(NPCID.BirdRed);
                                    break;
                                default:
                                    spawnPoint.NewNPC(NPCID.Bird);
                                    break;
                            }
                        }
                    }
                    else if (!flag7 && num96 < Main.maxTilesX / 3 && Main.rand.Next(15) == 0 && (num45 == 2 || num45 == 477 || num45 == 109 || num45 == 492 || num45 == 147))
                    {
                        int num98 = Main.rand.Next(4);
                        if (player.RollLuck(NPC.goldCritterChance) == 0)
                        {
                            spawnPoint.NewNPC(NPCID.GoldBird);
                        }
                        else
                        {
                            switch (num98)
                            {
                                case 0:
                                    spawnPoint.NewNPC(NPCID.BirdBlue);
                                    break;
                                case 1:
                                    spawnPoint.NewNPC(NPCID.BirdRed);
                                    break;
                                default:
                                    spawnPoint.NewNPC(NPCID.Bird);
                                    break;
                            }
                        }
                    }
                    else if (!flag7 && num96 > Main.maxTilesX / 3 && num45 == 2 && Main.rand.Next(300) == 0 && !NPC.AnyNPCs(NPCID.KingSlime))
                    {
                        NPC.SpawnOnPlayer(k, NPCID.KingSlime);
                    }
                    else if (!flag15 && num45 == 53 && (tileX < WorldGen.beachDistance || tileX > Main.maxTilesX - WorldGen.beachDistance))
                    {
                        if (!flag7 && Main.rand.Next(10) == 0)
                        {
                            spawnPoint.NewNPC(NPCID.Seagull);
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
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num99 * 16, NPCID.SeaTurtle);
                                }
                                else if (num102 == 1 && num100 > 0)
                                {
                                    NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num100 * 16, NPCID.Dolphin);
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
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num103 * 16, NPCID.GoldSeahorse);
                                    }
                                    else
                                    {
                                        NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), spawnX, num103 * 16, NPCID.Seahorse);
                                    }
                                }
                            }
                        }
                    }
                    else if (!flag7 && num45 == 53 && Main.rand.Next(5) == 0 && NPC.Spawning_SandstoneCheck(tileX, tileY) && !flag7)
                    {
                        newNPC = spawnPoint.NewNPC(NPCID.Antlion);
                    }
                    else if (num45 == 53 && !flag7)
                    {
                        newNPC = spawnPoint.NewNPC(NPCID.Vulture);
                    }
                    else if (!flag7 && (num96 > Main.maxTilesX / 3 || Main.remixWorld) && (Main.rand.Next(15) == 0 || (!NPC.downedGoblins && WorldGen.shadowOrbSmashed && Main.rand.Next(7) == 0)))
                    {
                        newNPC = spawnPoint.NewNPC(NPCID.GoblinScout);
                    }
                    else if (Main.raining && Main.rand.Next(4) == 0)
                    {
                        newNPC = spawnPoint.NewNPC(NPCID.FlyingFish);
                    }
                    else if (!flag7 && Main.raining && Main.rand.Next(2) == 0)
                    {
                        newNPC = spawnPoint.NewNPC(NPCID.UmbrellaSlime);
                    }
                    else if (!flag7 && num46 == 0 && isItAHappyWindyDay && flag19 && Main.rand.Next(3) != 0)
                    {
                        newNPC = spawnPoint.NewNPC(NPCID.WindyBalloon);
                    }
                    else if (!flag7 && num46 == 0 && (num3 == 2 || num3 == 477) && isItAHappyWindyDay && flag19 && Main.rand.Next(10) != 0)
                    {
                        newNPC = spawnPoint.NewNPC(NPCID.Dandelion);
                    }
                    else if (!flag7)
                    {
                        newNPC = spawnPoint.NewNPC(NPCID.BlueSlime);
                        switch (num45)
                        {
                            case 60:
                                Main.npc[newNPC].SetDefaults(NPCID.JungleSlime);
                                break;
                            case 147:
                            case 161:
                                Main.npc[newNPC].SetDefaults(NPCID.IceSlime);
                                break;
                            default:
                                if (Main.halloween && Main.rand.Next(3) != 0)
                                {
                                    Main.npc[newNPC].SetDefaults(NPCID.SlimeMasked);
                                }
                                else if (Main.xMas && Main.rand.Next(3) != 0)
                                {
                                    Main.npc[newNPC].SetDefaults(Main.rand.Next(333, 337));
                                }
                                else if (Main.rand.Next(3) == 0 || (num96 < 200 && !Main.expertMode))
                                {
                                    Main.npc[newNPC].SetDefaults(NPCID.GreenSlime);
                                }
                                else if (Main.rand.Next(10) == 0 && (num96 > 400 || Main.expertMode))
                                {
                                    Main.npc[newNPC].SetDefaults(NPCID.PurpleSlime);
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
                        spawnPoint.NewNPC(num104);
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
                        newNPC = spawnPoint.NewNPC(NPCID.Raven);
                    }
                    else if (player.ZoneGraveyard && Main.rand.Next(30) == 0)
                    {
                        newNPC = spawnPoint.NewNPC(NPCID.Ghost);
                    }
                    else if (player.ZoneGraveyard && Main.hardMode && tileY <= Main.worldSurface && Main.rand.Next(10) == 0)
                    {
                        newNPC = spawnPoint.NewNPC(NPCID.HoppinJack);
                    }
                    else if (Main.rand.Next(6) == 0 || (Main.moonPhase == 4 && Main.rand.Next(2) == 0))
                    {
                        if (Main.hardMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = spawnPoint.NewNPC(NPCID.WanderingEye);
                        }
                        else if (Main.halloween && Main.rand.Next(2) == 0)
                        {
                            newNPC = spawnPoint.NewNPC(Main.rand.Next(317, 319));
                        }
                        else if (Main.rand.Next(2) == 0)
                        {
                            newNPC = spawnPoint.NewNPC(NPCID.DemonEye);
                            if (Main.rand.Next(4) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.DemonEye2);
                            }
                        }
                        else
                        {
                            switch (Main.rand.Next(5))
                            {
                                case 0:
                                    newNPC = spawnPoint.NewNPC(NPCID.CataractEye);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Main.npc[newNPC].SetDefaults(NPCID.CataractEye2);
                                    }
                                    break;
                                case 1:
                                    newNPC = spawnPoint.NewNPC(NPCID.SleepyEye);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Main.npc[newNPC].SetDefaults(NPCID.SleepyEye2);
                                    }
                                    break;
                                case 2:
                                    newNPC = spawnPoint.NewNPC(NPCID.DialatedEye);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Main.npc[newNPC].SetDefaults(NPCID.DialatedEye2);
                                    }
                                    break;
                                case 3:
                                    newNPC = spawnPoint.NewNPC(NPCID.GreenEye);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Main.npc[newNPC].SetDefaults(NPCID.GreenEye2);
                                    }
                                    break;
                                case 4:
                                    newNPC = spawnPoint.NewNPC(NPCID.PurpleEye);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        Main.npc[newNPC].SetDefaults(NPCID.PurpleEye2);
                                    }
                                    break;
                            }
                        }
                    }
                    else if (Main.hardMode && Main.rand.Next(50) == 0 && Main.bloodMoon && !NPC.AnyNPCs(NPCID.Clown))
                    {
                        spawnPoint.NewNPC(NPCID.Clown);
                    }
                    else if (Main.rand.Next(250) == 0 && (Main.bloodMoon || player.ZoneGraveyard))
                    {
                        spawnPoint.NewNPC(NPCID.TheGroom);
                    }
                    else if (Main.rand.Next(250) == 0 && (Main.bloodMoon || player.ZoneGraveyard))
                    {
                        spawnPoint.NewNPC(NPCID.TheBride);
                    }
                    else if (!Main.dayTime && Main.moonPhase == 0 && Main.hardMode && Main.rand.Next(3) != 0)
                    {
                        newNPC = spawnPoint.NewNPC(NPCID.Werewolf);
                    }
                    else if (!Main.dayTime && Main.hardMode && Main.rand.Next(3) == 0)
                    {
                        newNPC = spawnPoint.NewNPC(NPCID.PossessedArmor);
                    }
                    else if (Main.bloodMoon && Main.rand.Next(5) < 2)
                    {
                        newNPC = ((Main.rand.Next(2) != 0) ? spawnPoint.NewNPC(NPCID.Drippler) : spawnPoint.NewNPC(NPCID.BloodZombie));
                    }
                    else if (num3 == 147 || num3 == 161 || num3 == 163 || num3 == 164 || num3 == 162)
                    {
                        newNPC = ((!player.ZoneGraveyard && Main.hardMode && Main.rand.Next(4) == 0) ? spawnPoint.NewNPC(NPCID.IceElemental) : ((!player.ZoneGraveyard && Main.hardMode && Main.rand.Next(3) == 0) ? spawnPoint.NewNPC(NPCID.Wolf) : ((!Main.expertMode || Main.rand.Next(2) != 0) ? spawnPoint.NewNPC(NPCID.ZombieEskimo) : spawnPoint.NewNPC(NPCID.ArmedZombieEskimo))));
                    }
                    else if (Main.raining && Main.rand.Next(2) == 0)
                    {
                        newNPC = spawnPoint.NewNPC(NPCID.ZombieRaincoat);
                        if (Main.rand.Next(3) == 0)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.SmallRainZombie);
                            }
                            else
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.BigRainZombie);
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
                            newNPC = spawnPoint.NewNPC(NPCID.MaggotZombie);
                        }
                        else if (Main.rand.Next(num106) == 0)
                        {
                            newNPC = ((!Main.expertMode || Main.rand.Next(2) != 0) ? spawnPoint.NewNPC(NPCID.TorchZombie) : spawnPoint.NewNPC(NPCID.ArmedTorchZombie));
                        }
                        else if (Main.halloween && Main.rand.Next(2) == 0)
                        {
                            newNPC = spawnPoint.NewNPC(Main.rand.Next(319, 322));
                        }
                        else if (Main.xMas && Main.rand.Next(2) == 0)
                        {
                            newNPC = spawnPoint.NewNPC(Main.rand.Next(331, 333));
                        }
                        else if (num105 == 0 && Main.expertMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = spawnPoint.NewNPC(NPCID.ArmedZombie);
                        }
                        else if (num105 == 2 && Main.expertMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = spawnPoint.NewNPC(NPCID.ArmedZombiePincussion);
                        }
                        else if (num105 == 3 && Main.expertMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = spawnPoint.NewNPC(NPCID.ArmedZombieSlimed);
                        }
                        else if (num105 == 4 && Main.expertMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = spawnPoint.NewNPC(NPCID.ArmedZombieSwamp);
                        }
                        else if (num105 == 5 && Main.expertMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = spawnPoint.NewNPC(NPCID.ArmedZombieTwiggy);
                        }
                        else if (num105 == 6 && Main.expertMode && Main.rand.Next(3) == 0)
                        {
                            newNPC = spawnPoint.NewNPC(NPCID.ArmedZombieCenx);
                        }
                        else
                        {
                            switch (num105)
                            {
                                case 0:
                                    newNPC = spawnPoint.NewNPC(NPCID.Zombie);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            Main.npc[newNPC].SetDefaults(NPCID.SmallZombie);
                                        }
                                        else
                                        {
                                            Main.npc[newNPC].SetDefaults(NPCID.BigZombie);
                                        }
                                    }
                                    break;
                                case 1:
                                    newNPC = spawnPoint.NewNPC(NPCID.BaldZombie);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            Main.npc[newNPC].SetDefaults(NPCID.SmallBaldZombie);
                                        }
                                        else
                                        {
                                            Main.npc[newNPC].SetDefaults(NPCID.BigBaldZombie);
                                        }
                                    }
                                    break;
                                case 2:
                                    newNPC = spawnPoint.NewNPC(NPCID.PincushionZombie);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            Main.npc[newNPC].SetDefaults(NPCID.SmallPincushionZombie);
                                        }
                                        else
                                        {
                                            Main.npc[newNPC].SetDefaults(NPCID.BigPincushionZombie);
                                        }
                                    }
                                    break;
                                case 3:
                                    newNPC = spawnPoint.NewNPC(NPCID.SlimedZombie);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            Main.npc[newNPC].SetDefaults(NPCID.SmallSlimedZombie);
                                        }
                                        else
                                        {
                                            Main.npc[newNPC].SetDefaults(NPCID.BigSlimedZombie);
                                        }
                                    }
                                    break;
                                case 4:
                                    newNPC = spawnPoint.NewNPC(NPCID.SwampZombie);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            Main.npc[newNPC].SetDefaults(NPCID.SmallSwampZombie);
                                        }
                                        else
                                        {
                                            Main.npc[newNPC].SetDefaults(NPCID.BigSwampZombie);
                                        }
                                    }
                                    break;
                                case 5:
                                    newNPC = spawnPoint.NewNPC(NPCID.TwiggyZombie);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            Main.npc[newNPC].SetDefaults(NPCID.SmallTwiggyZombie);
                                        }
                                        else
                                        {
                                            Main.npc[newNPC].SetDefaults(NPCID.BigTwiggyZombie);
                                        }
                                    }
                                    break;
                                case 6:
                                    newNPC = spawnPoint.NewNPC(NPCID.FemaleZombie);
                                    if (Main.rand.Next(3) == 0)
                                    {
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            Main.npc[newNPC].SetDefaults(NPCID.SmallFemaleZombie);
                                        }
                                        else
                                        {
                                            Main.npc[newNPC].SetDefaults(NPCID.BigFemaleZombie);
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
                    newNPC = ((!Main.hardMode) ? spawnPoint.NewNPC(NPCID.GiantWormHead, 1) : ((Main.rand.Next(3) == 0) ? spawnPoint.NewNPC(NPCID.GiantWormHead, 1) : spawnPoint.NewNPC(NPCID.DiggerHead, 1)));
                }
                else if (Main.hardMode && Main.rand.Next(3) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.PossessedArmor);
                }
                else if (Main.hardMode && Main.rand.Next(4) != 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.ToxicSludge);
                }
                else if (Main.remixWorld)
                {
                    if (num3 == 147 || num3 == 161 || num3 == 163 || num3 == 164 || num3 == 162 || player.ZoneSnow)
                    {
                        newNPC = spawnPoint.NewNPC(NPCID.IceSlime);
                    }
                    else
                    {
                        newNPC = spawnPoint.NewNPC(NPCID.BlueSlime);
                        if (Main.rand.Next(3) == 0)
                        {
                            Main.npc[newNPC].SetDefaults(NPCID.YellowSlime);
                        }
                        else
                        {
                            Main.npc[newNPC].SetDefaults(NPCID.RedSlime);
                        }
                    }
                }
                else if (num45 == 147 || num45 == 161 || player.ZoneSnow)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.IceSlime);
                }
                else
                {
                    newNPC = spawnPoint.NewNPC(NPCID.BlueSlime);
                    if (Main.rand.Next(5) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(NPCID.YellowSlime);
                    }
                    else if (Main.rand.Next(2) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(NPCID.BlueSlime);
                    }
                    else
                    {
                        Main.npc[newNPC].SetDefaults(NPCID.RedSlime);
                    }
                }
            }
            else if (tileY > Main.maxTilesY - 190)
            {
                newNPC = ((Main.remixWorld && tileX > Main.maxTilesX * 0.38 + 50.0 && tileX < Main.maxTilesX * 0.62) ? spawnPoint.NewNPC(NPCID.LavaSlime) : ((Main.hardMode && !NPC.savedTaxCollector && Main.rand.Next(20) == 0 && !NPC.AnyNPCs(NPCID.DemonTaxCollector)) ? spawnPoint.NewNPC(NPCID.DemonTaxCollector) : ((Main.rand.Next(8) == 0) ? NPC.SpawnNPC_SpawnLavaBaitCritters(tileX, tileY) : ((Main.rand.Next(40) == 0 && !NPC.AnyNPCs(NPCID.BoneSerpentHead)) ? spawnPoint.NewNPC(NPCID.BoneSerpentHead, 1) : ((Main.rand.Next(14) == 0) ? spawnPoint.NewNPC(NPCID.FireImp) : ((Main.rand.Next(7) == 0) ? ((Main.rand.Next(10) == 0) ? spawnPoint.NewNPC(NPCID.VoodooDemon) : ((!Main.hardMode || !NPC.downedMechBossAny || Main.rand.Next(5) == 0) ? spawnPoint.NewNPC(NPCID.Demon) : spawnPoint.NewNPC(NPCID.RedDevil))) : ((Main.rand.Next(3) == 0) ? spawnPoint.NewNPC(NPCID.LavaSlime) : ((!Main.hardMode || !NPC.downedMechBossAny || Main.rand.Next(5) == 0) ? spawnPoint.NewNPC(NPCID.Hellbat) : spawnPoint.NewNPC(NPCID.Lavabat)))))))));
            }
            else if (NPC.SpawnNPC_CheckToSpawnRockGolem(tileX, tileY, k, num45))
            {
                newNPC = spawnPoint.NewNPC(NPCID.RockGolem);
            }
            else if (Main.rand.Next(60) == 0)
            {
                newNPC = ((!player.ZoneSnow) ? spawnPoint.NewNPC(NPCID.CochinealBeetle) : spawnPoint.NewNPC(NPCID.CyanBeetle));
            }
            else if ((num45 == 116 || num45 == 117 || num45 == 164) && Main.hardMode && !flag5 && Main.rand.Next(8) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.ChaosElemental);
            }
            else if ((num3 == 147 || num3 == 161 || num3 == 162 || num3 == 163 || num3 == 164 || num3 == 200) && !flag5 && Main.hardMode && player.ZoneCorrupt && Main.rand.Next(30) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.PigronCorruption);
            }
            else if ((num3 == 147 || num3 == 161 || num3 == 162 || num3 == 163 || num3 == 164 || num3 == 200) && !flag5 && Main.hardMode && player.ZoneHallow && Main.rand.Next(30) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.PigronHallow);
            }
            else if ((num3 == 147 || num3 == 161 || num3 == 162 || num3 == 163 || num3 == 164 || num3 == 200) && !flag5 && Main.hardMode && player.ZoneCrimson && Main.rand.Next(30) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.PigronCrimson);
            }
            else if (Main.hardMode && player.ZoneSnow && Main.rand.Next(10) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.IceTortoise);
            }
            else if (!flag5 && Main.rand.Next(100) == 0 && !player.ZoneHallow)
            {
                newNPC = (Main.hardMode ? spawnPoint.NewNPC(NPCID.DiggerHead, 1) : ((!player.ZoneSnow) ? spawnPoint.NewNPC(NPCID.GiantWormHead, 1) : spawnPoint.NewNPC(NPCID.SnowFlinx)));
            }
            else if (player.ZoneSnow && Main.rand.Next(20) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.SnowFlinx);
            }
            else if ((!Main.hardMode && Main.rand.Next(10) == 0) || (Main.hardMode && Main.rand.Next(20) == 0))
            {
                if (player.ZoneSnow)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.SpikedIceSlime);
                }
                else if (Main.rand.Next(3) == 0)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.BlueSlime);
                    Main.npc[newNPC].SetDefaults(NPCID.BlackSlime);
                }
                else
                {
                    newNPC = spawnPoint.NewNPC(NPCID.MotherSlime);
                }
            }
            else if (!Main.hardMode && Main.rand.Next(4) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.BlueSlime);
                if (player.ZoneJungle)
                {
                    Main.npc[newNPC].SetDefaults(NPCID.JungleSlime);
                }
                else if (player.ZoneSnow)
                {
                    Main.npc[newNPC].SetDefaults(NPCID.SpikedIceSlime);
                }
                else
                {
                    Main.npc[newNPC].SetDefaults(NPCID.BlackSlime);
                }
            }
            else if (Main.rand.Next(2) != 0)
            {
                newNPC = ((Main.hardMode && (player.ZoneHallow & (Main.rand.Next(2) == 0))) ? spawnPoint.NewNPC(NPCID.IlluminantSlime) : (player.ZoneJungle ? spawnPoint.NewNPC(NPCID.JungleBat) : ((player.ZoneGlowshroom && (num3 == 70 || num3 == 190)) ? spawnPoint.NewNPC(NPCID.SporeBat) : ((Main.hardMode && player.ZoneHallow) ? spawnPoint.NewNPC(NPCID.IlluminantBat) : ((Main.hardMode && Main.rand.Next(6) > 0) ? ((Main.rand.Next(3) != 0 || (num3 != 147 && num3 != 161 && num3 != 162)) ? spawnPoint.NewNPC(NPCID.GiantBat) : spawnPoint.NewNPC(NPCID.IceBat)) : ((num3 != 147 && num3 != 161 && num3 != 162) ? spawnPoint.NewNPC(NPCID.CaveBat) : ((!Main.hardMode) ? spawnPoint.NewNPC(NPCID.IceBat) : spawnPoint.NewNPC(NPCID.IceElemental))))))));
            }
            else if (Main.rand.Next(35) == 0 && NPC.CountNPCS(NPCID.SkeletonMerchant) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.SkeletonMerchant);
            }
            else if (Main.rand.Next(80) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.LostGirl);
            }
            else if (Main.hardMode && (Main.remixWorld || tileY > (Main.rockLayer + Main.maxTilesY) / 2.0) && Main.rand.Next(200) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.RuneWizard);
            }
            else if ((Main.remixWorld || tileY > (Main.rockLayer + Main.maxTilesY) / 2.0) && (Main.rand.Next(200) == 0 || (Main.rand.Next(50) == 0 && (player.armor[1].type == ItemID.AmberRobe || (player.armor[1].type >= 1282 && player.armor[1].type <= 1287)) && player.armor[0].type != ItemID.WizardHat)))
            {
                newNPC = spawnPoint.NewNPC(NPCID.Tim);
            }
            else if (flag10 && Main.rand.Next(4) != 0)
            {
                newNPC = ((Main.rand.Next(6) == 0 || NPC.AnyNPCs(NPCID.Medusa) || !Main.hardMode) ? spawnPoint.NewNPC(NPCID.GreekSkeleton) : spawnPoint.NewNPC(NPCID.Medusa));
            }
            else if (flag9 && Main.rand.Next(5) != 0)
            {
                newNPC = ((Main.rand.Next(6) == 0 || NPC.AnyNPCs(NPCID.GraniteFlyer)) ? spawnPoint.NewNPC(NPCID.GraniteGolem) : spawnPoint.NewNPC(NPCID.GraniteFlyer));
            }
            else if (Main.hardMode && Main.rand.Next(10) != 0)
            {
                if (Main.rand.Next(2) != 0)
                {
                    newNPC = ((!player.ZoneSnow) ? spawnPoint.NewNPC(NPCID.SkeletonArcher) : spawnPoint.NewNPC(NPCID.IcyMerman));
                }
                else if (player.ZoneSnow)
                {
                    newNPC = spawnPoint.NewNPC(NPCID.ArmoredViking);
                }
                else
                {
                    newNPC = spawnPoint.NewNPC(NPCID.ArmoredSkeleton);
                    if ((Main.remixWorld || tileY > (Main.rockLayer + Main.maxTilesY) / 2.0) && Main.rand.Next(5) == 0)
                    {
                        Main.npc[newNPC].SetDefaults(NPCID.HeavySkeleton);
                    }
                }
            }
            else if (!flag5 && (Main.halloween || player.ZoneGraveyard) && Main.rand.Next(30) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.Ghost);
            }
            else if (Main.rand.Next(20) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPCID.UndeadMiner);
            }
            else if (num3 == 147 || num3 == 161 || num3 == 162)
            {
                newNPC = ((Main.rand.Next(15) != 0) ? spawnPoint.NewNPC(NPCID.UndeadViking) : spawnPoint.NewNPC(NPCID.SnowFlinx));
            }
            else if (player.ZoneSnow)
            {
                newNPC = spawnPoint.NewNPC(NPCID.SnowFlinx);
            }
            else if (Main.rand.Next(3) == 0)
            {
                newNPC = spawnPoint.NewNPC(NPC.cavernMonsterType[Main.rand.Next(2), Main.rand.Next(3)]);
            }
            else if (player.ZoneGlowshroom && (num3 == 70 || num3 == 190))
            {
                newNPC = spawnPoint.NewNPC(NPCID.SporeSkeleton);
            }
            else if (Main.halloween && Main.rand.Next(2) == 0)
            {
                newNPC = spawnPoint.NewNPC(Main.rand.Next(322, 325));
            }
            else if (Main.expertMode && Main.rand.Next(3) == 0)
            {
                int num107 = Main.rand.Next(4);
                newNPC = ((num107 == 0) ? spawnPoint.NewNPC(NPCID.BoneThrowingSkeleton) : ((num107 == 0) ? spawnPoint.NewNPC(NPCID.BoneThrowingSkeleton2) : ((num107 != 0) ? spawnPoint.NewNPC(NPCID.BoneThrowingSkeleton4) : spawnPoint.NewNPC(NPCID.BoneThrowingSkeleton3))));
            }
            else
            {
                switch (Main.rand.Next(4))
                {
                    case 0:
                        newNPC = spawnPoint.NewNPC(NPCID.Skeleton);
                        if (Main.rand.Next(3) == 0)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.BigSkeleton);
                            }
                            else
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.SmallSkeleton);
                            }
                        }
                        break;
                    case 1:
                        newNPC = spawnPoint.NewNPC(NPCID.HeadacheSkeleton);
                        if (Main.rand.Next(3) == 0)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.BigHeadacheSkeleton);
                            }
                            else
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.SmallHeadacheSkeleton);
                            }
                        }
                        break;
                    case 2:
                        newNPC = spawnPoint.NewNPC(NPCID.MisassembledSkeleton);
                        if (Main.rand.Next(3) == 0)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.BigMisassembledSkeleton);
                            }
                            else
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.SmallMisassembledSkeleton);
                            }
                        }
                        break;
                    case 3:
                        newNPC = spawnPoint.NewNPC(NPCID.PantlessSkeleton);
                        if (Main.rand.Next(3) == 0)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.BigPantlessSkeleton);
                            }
                            else
                            {
                                Main.npc[newNPC].SetDefaults(NPCID.SmallPantlessSkeleton);
                            }
                        }
                        break;
                }
            }
            if (Main.npc[newNPC].type == NPCID.BlueSlime && player.RollLuck(180) == 0)
            {
                Main.npc[newNPC].SetDefaults(NPCID.Pinky);
            }
            if (Main.tenthAnniversaryWorld && Main.npc[newNPC].type == NPCID.BlueSlime && player.RollLuck(180) == 0)
            {
                Main.npc[newNPC].SetDefaults(NPCID.GoldenSlime);
            }
            if (newNPC < 200)
            {
                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, newNPC);
            }
            break;
        }
    }
    public static void TransformElderSlime(On.Terraria.NPC.orig_TransformElderSlime orig, int npcIndex)
    {
        if (!Main.npc.IndexInRange(npcIndex))
        {
            return;
        }
        var npc = Main.npc[npcIndex];
        if (npc.type != NPCID.BoundTownSlimeOld)
        {
            return;
        }
        if (NPC.unlockedSlimeOldSpawn)
        {
            if (MainConfig.Instance.BoundTownSlimeOldSpawnItemIDs.Length > 0)
            {
                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, Item.NewItem(null, npc.Center, Vector2.Zero, Utils.SelectRandom(Main.rand, MainConfig.Instance.BoundTownSlimeOldSpawnItemIDs)));
                npc.active = false;
                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcIndex);
            }
        }
        else
        {
            NPC.unlockedSlimeOldSpawn = true;
            NetMessage.SendData(MessageID.WorldData);
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
    [DetourMethod]
    public static void UpdateNPC(On.Terraria.NPC.orig_UpdateNPC orig, NPC npc, int i)
    {
        npc.whoAmI = i;
        if (!npc.active)
        {
            return;
        }
        npc.netOffset *= 0f;
        npc.UpdateAltTexture();
        if (npc.type == NPCID.TravellingMerchant)
        {
            NPC.travelNPC = true;
        }
        npc.UpdateNPC_TeleportVisuals();
        npc.UpdateNPC_CritterSounds();
        npc.TrySyncingUniqueTownNPCData(i);
        if (npc.aiStyle == 7 && npc.position.Y > Main.bottomWorld - 640f + npc.height && (MainConfigInfo.StaticDisableKillNPCWhenNPCUnderWorldBottomXMasCheck || !Main.xMas))
        {
            npc.StrikeNPCNoInteraction(9999, 0f, 0);
            NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, 9999f);
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
        if (npc.type != NPCID.OldMan && (npc.friendly || NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[npc.type]))
        {
            if (npc.townNPC && !MainConfigInfo.StaticTownNPCDrowningImmunity)
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
        if (!MainConfigInfo.StaticDisableQueenBeeAndBeeHurtOtherNPC && (NPC.npcsFoundForCheckActive[NPCID.Bee] || NPC.npcsFoundForCheckActive[NPCID.BeeSmall]) && !NPCID.Sets.HurtingBees[npc.type])
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
                    if (npc.buffType[k] == BuffID.OnFire)
                    {
                        npc.DelBuff(k);
                        break;
                    }
                }
            }
        }
        if (!npc.noTileCollide && npc.lifeMax > 1 && Collision.SwitchTiles(npc.position, npc.width, npc.height, npc.oldPosition, 2, npc) && (npc.type == NPCID.Bunny || npc.type == NPCID.Penguin || npc.type == NPCID.PenguinBlack || npc.type == NPCID.BunnySlimed || npc.type == NPCID.Frog || npc.type == NPCID.Duck || npc.type == NPCID.DuckWhite || npc.type == NPCID.ScorpionBlack || npc.type == NPCID.Scorpion || (npc.type >= 442 && npc.type <= 448) || npc.type == NPCID.Seagull || npc.type == NPCID.Grebe || npc.type == NPCID.ExplosiveBunny || npc.type == NPCID.BoundTownSlimeYellow))
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
    [DetourMethod]
    public static void HitEffect(On.Terraria.NPC.orig_HitEffect orig, NPC self, int hitDirection, double dmg)
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
                if (nPC.active && !nPC.buffImmune[BuffID.Daybreak] && self.Distance(nPC.Center) < 100f && !nPC.dontTakeDamage && nPC.lifeMax > 5 && !nPC.friendly && !nPC.townNPC)
                {
                    nPC.AddBuff(BuffID.Daybreak, 300);
                }
            }
        }
        if (self.type == NPCID.BoundTownSlimePurple && self.life <= 0)
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
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        if (self.type == NPCID.WindyBalloon && self.life <= 0)
        {
            NPC nPC2 = self.AI_113_WindyBalloon_GetSlaveNPC();
            if (nPC2 != null)
            {
                nPC2.ai[0] = 0f;
                nPC2.position.Y -= 10f;
                nPC2.netUpdate = true;
            }
        }
        if (self.type == NPCID.DD2Betsy && self.life > 0)
        {
            int num154 = (int)(self.life / (float)self.lifeMax * 100f);
            int num149 = (int)((self.life + dmg) / (double)(float)self.lifeMax * 100.0);
            if (num154 != num149)
            {
                DD2Event.CheckProgress(self.type);
            }
        }
        if (self.type == NPCID.TargetDummy)
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
        if (self.type == NPCID.VortexHornetQueen && self.life <= 0)
        {
            int num155 = NPC.CountNPCS(NPCID.VortexLarva) + NPC.CountNPCS(NPCID.VortexHornet) + NPC.CountNPCS(NPCID.VortexHornetQueen) * 3;
            int num120 = 20;
            if (num155 < num120)
            {
                for (int num121 = 0; num121 < 3; num121++)
                {
                    int num122 = NPC.NewNPC(self.GetSpawnSource_NPCHurt(), (int)self.Center.X, (int)self.Center.Y, NPCID.VortexLarva, self.whoAmI);
                    Main.npc[num122].velocity = -Vector2.UnitY.RotatedByRandom(6.2831854820251465) * Main.rand.Next(3, 6) - Vector2.UnitY * 2f;
                    Main.npc[num122].netUpdate = true;
                }
            }
        }
        else if (self.type == NPCID.VortexSoldier && self.life <= 0)
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
                        Projectile.NewProjectile(self.GetSpawnSource_ForProjectile(), num128 * 16 + 8, num129 * 16 + 8, 0f, 0f, ProjectileID.VortexVortexLightning, 0, 1f, Main.myPlayer);
                        break;
                    }
                }
            }
        }
        else if (self.type == NPCID.StardustCellBig && self.life <= 0)
        {
            int num156 = NPC.CountNPCS(NPCID.StardustCellSmall) + NPC.CountNPCS(NPCID.StardustCellBig);
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
                int num132 = NPC.NewNPC(self.GetSpawnSource_NPCHurt(), (int)self.Center.X, (int)self.Bottom.Y, NPCID.StardustCellSmall, self.whoAmI);
                Main.npc[num132].velocity = vector3;
            }
        }
        else if (self.type == NPCID.PirateShip && self.life <= 0)
        {
            for (int num133 = 0; num133 < 4; num133++)
            {
                float num134 = (num133 < 2).ToDirectionInt() * ((float)Math.PI / 8f + (float)Math.PI / 4f * Main.rand.NextFloat());
                Vector2 vector4 = new Vector2(0f, (0f - Main.rand.NextFloat()) * 0.5f - 0.5f).RotatedBy(num134) * 6f;
                Projectile.NewProjectile(self.GetSpawnSource_ForProjectile(), self.Center.X, self.Center.Y, vector4.X, vector4.Y, ProjectileID.BlowupSmoke, 0, 0f, Main.myPlayer);
            }
        }
        if (self.type == NPCID.MotherSlime && self.life <= 0)
        {
            int num135 = Main.rand.Next(2) + 2;
            for (int num136 = 0; num136 < num135; num136++)
            {
                int num137 = NPC.NewNPC(self.GetSpawnSource_NPCHurt(), (int)(self.position.X + self.width / 2), (int)(self.position.Y + self.height), NPCID.BlueSlime);
                Main.npc[num137].SetDefaults(NPCID.BabySlime);
                Main.npc[num137].velocity.X = self.velocity.X * 2f;
                Main.npc[num137].velocity.Y = self.velocity.Y;
                Main.npc[num137].velocity.X += Main.rand.Next(-20, 20) * 0.1f + num136 * self.direction * 0.3f;
                Main.npc[num137].velocity.Y -= Main.rand.Next(0, 10) * 0.1f + num136;
                Main.npc[num137].ai[0] = -1000 * Main.rand.Next(3);
                if (num137 < 200)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num137);
                }
            }
        }
        if (self.type == NPCID.GolemHead && self.life <= 0)
        {
            NPC.NewNPC(self.GetSpawnSource_NPCHurt(), (int)self.Center.X, (int)self.position.Y + self.height, NPCID.GolemHeadFree, self.whoAmI);
        }
        if (self.type == NPCID.Shimmerfly && self.life <= 0)
        {
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ShimmerArrow, new ParticleOrchestraSettings
            {
                PositionInWorld = self.Center,
                MovementVector = self.velocity
            });
        }
        if ((self.type == NPCID.CorruptSlime || self.type == NPCID.Slimer) && self.life <= 0)
        {
            if (self.type == NPCID.Slimer)
            {
                int num138 = NPC.NewNPC(self.GetSpawnSource_NPCHurt(), (int)(self.position.X + self.width / 2), (int)(self.position.Y + self.height), NPCID.CorruptSlime);
                Main.npc[num138].SetDefaults(NPCID.Slimer2);
                Main.npc[num138].velocity.X = self.velocity.X;
                Main.npc[num138].velocity.Y = self.velocity.Y;
                if (num138 < 200)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num138);
                }
            }
            else if (self.scale >= 1f)
            {
                int num139 = Main.rand.Next(2) + 2;
                for (int num140 = 0; num140 < num139; num140++)
                {
                    int num141 = NPC.NewNPC(self.GetSpawnSource_NPCHurt(), (int)(self.position.X + self.width / 2), (int)(self.position.Y + self.height), NPCID.BlueSlime);
                    Main.npc[num141].SetDefaults(NPCID.Slimeling);
                    Main.npc[num141].velocity.X = self.velocity.X * 3f;
                    Main.npc[num141].velocity.Y = self.velocity.Y;
                    Main.npc[num141].velocity.X += Main.rand.Next(-10, 10) * 0.1f + num140 * self.direction * 0.3f;
                    Main.npc[num141].velocity.Y -= Main.rand.Next(0, 10) * 0.1f + num140;
                    Main.npc[num141].ai[1] = num140;
                    if (num141 < 200)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num141);
                    }
                }
            }
        }
        if ((self.type == NPCID.LavaSlime || self.type == NPCID.Hellbat || self.type == NPCID.Lavabat) && self.life <= 0)
        {
            if (MainConfigInfo.StaticDisableSpawnLavaWhenNPCDead.IsFalseRet(Main.expertMode && self.type == NPCID.LavaSlime && !Main.remixWorld) || ((self.type == NPCID.Lavabat || self.type == NPCID.Hellbat) && Main.remixWorld && Main.getGoodWorld))
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
        else if (self.type == NPCID.KingSlime && self.life <= 0)
        {
            int num146 = Main.rand.Next(4) + 4;
            for (int num147 = 0; num147 < num146; num147++)
            {
                int x = (int)(self.position.X + Main.rand.Next(self.width - 32));
                int y = (int)(self.position.Y + Main.rand.Next(self.height - 32));
                int num148 = NPC.NewNPC(self.GetSpawnSource_NPCHurt(), x, y, NPCID.BlueSlime, self.whoAmI + 1);
                Main.npc[num148].SetDefaults(NPCID.BlueSlime);
                Main.npc[num148].velocity.X = Main.rand.Next(-15, 16) * 0.1f;
                Main.npc[num148].velocity.Y = Main.rand.Next(-30, 1) * 0.1f;
                Main.npc[num148].ai[0] = -1000 * Main.rand.Next(3);
                Main.npc[num148].ai[1] = 0f;
                if (num148 < 200)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num148);
                }
            }
        }
        else if ((self.type == NPCID.BoundTownSlimeYellow || self.type == NPCID.BoundTownSlimeOld) && self.life <= 0)
        {
            Terraria.Utils.PoofOfSmoke(self.Center - new Vector2(20f));
        }
        else if (self.type == NPCID.WanderingEye)
        {
            if (self.life > 0 && self.life < self.lifeMax * 0.5f && self.localAI[0] == 0f)
            {
                self.localAI[0] = 1f;
            }
        }
        else if (self.type == NPCID.MaggotZombie && self.life <= 0)
        {
            int num150 = Main.rand.Next(2) + 2;
            for (int num151 = 0; num151 < num150; num151++)
            {
                var vector5 = new Vector2(Main.rand.Next(-10, 10) * 0.2f, -3.5f - Main.rand.Next(5, 10) * 0.3f - num151 * 0.5f);
                int num152 = NPC.NewNPC(self.GetSpawnSource_NPCHurt(), (int)(self.position.X + self.width / 2), (int)self.position.Y + Main.rand.Next(self.height / 2) + 10, NPCID.Maggot);
                Main.npc[num152].velocity = vector5;
                Main.npc[num152].netUpdate = true;
            }
        }
        else if (self.type == NPCID.TheHungry && self.life <= 0)
        {
            NPC.NewNPC(self.GetSpawnSource_NPCHurt(), (int)(self.position.X + self.width / 2), (int)(self.position.Y + self.height), NPCID.TheHungryII);
        }
        else if (self.type == NPCID.TheDestroyerBody && self.life > 0)
        {
            int maxValue = 25;
            if (NPC.IsMechQueenUp)
            {
                maxValue = 50;
            }
            if (self.ai[2] == 0f && Main.rand.Next(maxValue) == 0)
            {
                self.ai[2] = 1f;
                int num153 = NPC.NewNPC(self.GetSpawnSource_NPCHurt(), (int)(self.position.X + self.width / 2), (int)(self.position.Y + self.height), NPCID.Probe);
                if (num153 < 200)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num153);
                }
                self.netUpdate = true;
            }
        }
    }
    [DetourMethod]
    public static void DoDeathEvents_AdvanceSlimeRain(On.Terraria.NPC.orig_DoDeathEvents_AdvanceSlimeRain orig, NPC self, Player closestPlayer)
    {
        if (Main.slimeRain && Main.slimeRainNPC[self.type] && !NPC.AnyNPCs(NPCID.KingSlime))
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
                NPC.SpawnOnPlayer(closestPlayer.whoAmI, NPCID.KingSlime);
                Main.slimeRainKillCount = -needCount / 2;
            }
        }
    }
    [DetourMethod]
    public static void DoDeathEvents(On.Terraria.NPC.orig_DoDeathEvents orig, NPC self, Player closestPlayer)
    {
        var needSend = false;
        self.DoDeathEvents_AdvanceSlimeRain(closestPlayer);
        self.DoDeathEvents_SummonDungeonSpirit(closestPlayer);
        if (Main.remixWorld && !NPC.downedSlimeKing && self.AnyInteractions() && Main.AnyPlayerReadyToFightKingSlime() && self.type == NPCID.BlueSlime && !NPC.AnyNPCs(NPCID.KingSlime) && Main.rand.Next(200) == 0)
        {
            NPC.SpawnOnPlayer(closestPlayer.whoAmI, NPCID.KingSlime);
        }
        switch (self.type)
        {
            case NPCID.PirateCaptain:
                NPC.SpawnBoss((int)self.position.X, (int)self.position.Y, 662, self.target);
                break;
            case NPCID.Pumpking:
                if (Main.pumpkinMoon)
                {
                    NPC.SetEventFlagCleared(ref NPC.downedHalloweenKing, GameEventClearedID.DefeatedHalloweenKing);
                }
                break;
            case NPCID.MourningWood:
                if (Main.pumpkinMoon)
                {
                    NPC.SetEventFlagCleared(ref NPC.downedHalloweenTree, GameEventClearedID.DefeatedHalloweenTree);
                }
                break;
            case NPCID.Everscream:
                if (Main.snowMoon)
                {
                    NPC.SetEventFlagCleared(ref NPC.downedChristmasTree, GameEventClearedID.DefeatedChristmassTree);
                }
                break;
            case NPCID.IceQueen:
                if (Main.snowMoon)
                {
                    NPC.SetEventFlagCleared(ref NPC.downedChristmasIceQueen, GameEventClearedID.DefeatedIceQueen);
                }
                break;
            case NPCID.SantaNK1:
                if (Main.snowMoon)
                {
                    NPC.SetEventFlagCleared(ref NPC.downedChristmasSantank, GameEventClearedID.DefeatedSantank);
                }
                break;
            case NPCID.DD2GoblinT1:
            case NPCID.DD2GoblinT2:
            case NPCID.DD2GoblinT3:
                if (DD2Event.Ongoing)
                {
                    DD2Event.AnnounceGoblinDeath(self);
                    if (DD2Event.ShouldDropCrystals())
                    {
                        Item.NewItem(self.GetItemSource_Loot(), self.position, self.Size, ItemID.DD2EnergyCrystal);
                    }
                }
                break;
            case NPCID.DD2GoblinBomberT1:
            case NPCID.DD2GoblinBomberT2:
            case NPCID.DD2GoblinBomberT3:
            case NPCID.DD2WyvernT1:
            case NPCID.DD2WyvernT2:
            case NPCID.DD2WyvernT3:
            case NPCID.DD2JavelinstT1:
            case NPCID.DD2JavelinstT2:
            case NPCID.DD2JavelinstT3:
            case NPCID.DD2DarkMageT1:
            case NPCID.DD2DarkMageT3:
            case NPCID.DD2WitherBeastT2:
            case NPCID.DD2WitherBeastT3:
            case NPCID.DD2DrakinT2:
            case NPCID.DD2DrakinT3:
            case NPCID.DD2KoboldWalkerT2:
            case NPCID.DD2KoboldWalkerT3:
            case NPCID.DD2KoboldFlyerT2:
            case NPCID.DD2KoboldFlyerT3:
            case NPCID.DD2OgreT2:
            case NPCID.DD2OgreT3:
            case NPCID.DD2LightningBugT3:
                if (DD2Event.ShouldDropCrystals())
                {
                    Item.NewItem(self.GetItemSource_Loot(), self.position, self.Size, ItemID.DD2EnergyCrystal);
                }
                break;
            case NPCID.SolarCrawltipedeHead:
            case NPCID.SolarCrawltipedeBody:
            case NPCID.SolarCrawltipedeTail:
            case NPCID.SolarDrakomire:
            case NPCID.SolarDrakomireRider:
            case NPCID.SolarSroller:
            case NPCID.SolarCorite:
            case NPCID.SolarSolenian:
            case NPCID.SolarSpearman:
                if (NPC.ShieldStrengthTowerSolar > 0)
                {
                    Projectile.NewProjectile(self.GetSpawnSource_ForProjectile(), self.Center.X, self.Center.Y, 0f, 0f, ProjectileID.TowerDamageBolt, 0, 0f, Main.myPlayer, NPC.FindFirstNPC(NPCID.LunarTowerSolar));
                }
                break;
            case NPCID.VortexRifleman:
            case NPCID.VortexHornetQueen:
            case NPCID.VortexHornet:
            case NPCID.VortexSoldier:
                if (NPC.ShieldStrengthTowerVortex > 0)
                {
                    Projectile.NewProjectile(self.GetSpawnSource_ForProjectile(), self.Center.X, self.Center.Y, 0f, 0f, ProjectileID.TowerDamageBolt, 0, 0f, Main.myPlayer, NPC.FindFirstNPC(NPCID.LunarTowerVortex));
                }
                break;
            case NPCID.NebulaBrain:
            case NPCID.NebulaHeadcrab:
            case NPCID.NebulaBeast:
            case NPCID.NebulaSoldier:
                if (NPC.ShieldStrengthTowerNebula > 0)
                {
                    Projectile.NewProjectile(self.GetSpawnSource_ForProjectile(), self.Center.X, self.Center.Y, 0f, 0f, ProjectileID.TowerDamageBolt, 0, 0f, Main.myPlayer, NPC.FindFirstNPC(NPCID.LunarTowerNebula));
                }
                break;
            case NPCID.StardustWormHead:
            case NPCID.StardustCellBig:
            case NPCID.StardustJellyfishBig:
            case NPCID.StardustSpiderBig:
            case NPCID.StardustSoldier:
                if (NPC.ShieldStrengthTowerStardust > 0)
                {
                    Projectile.NewProjectile(self.GetSpawnSource_ForProjectile(), self.Center.X, self.Center.Y, 0f, 0f, ProjectileID.TowerDamageBolt, 0, 0f, Main.myPlayer, NPC.FindFirstNPC(NPCID.LunarTowerStardust));
                }
                break;
            case NPCID.LunarTowerSolar:
                NPC.downedTowerSolar = true;
                NPC.TowerActiveSolar = false;
                WorldGen.UpdateLunarApocalypse();
                WorldGen.MessageLunarApocalypse();
                break;
            case NPCID.LunarTowerVortex:
                NPC.downedTowerVortex = true;
                NPC.TowerActiveVortex = false;
                WorldGen.UpdateLunarApocalypse();
                WorldGen.MessageLunarApocalypse();
                break;
            case NPCID.LunarTowerNebula:
                NPC.downedTowerNebula = true;
                NPC.TowerActiveNebula = false;
                WorldGen.UpdateLunarApocalypse();
                WorldGen.MessageLunarApocalypse();
                break;
            case NPCID.LunarTowerStardust:
                NPC.downedTowerStardust = true;
                NPC.TowerActiveStardust = false;
                WorldGen.UpdateLunarApocalypse();
                WorldGen.MessageLunarApocalypse();
                break;
            case NPCID.Golem:
                NPC.SetEventFlagCleared(ref NPC.downedGolemBoss, GameEventClearedID.DefeatedGolem);
                break;
            case NPCID.DukeFishron:
                NPC.SetEventFlagCleared(ref NPC.downedFishron, GameEventClearedID.DefeatedFishron);
                break;
            case NPCID.HallowBoss:
                NPC.SetEventFlagCleared(ref NPC.downedEmpressOfLight, GameEventClearedID.DefeatedEmpressOfLight);
                break;
            case NPCID.Deerclops:
                NPC.SetEventFlagCleared(ref NPC.downedDeerclops, GameEventClearedID.DefeatedDeerclops);
                break;
            case NPCID.QueenSlimeBoss:
                NPC.SetEventFlagCleared(ref NPC.downedQueenSlime, GameEventClearedID.DefeatedQueenSlime);
                break;
            case NPCID.Guide:
                if (Collision.LavaCollision(self.position, self.width, self.height))
                {
                    NPC.SpawnWOF(self.position);
                }
                break;
            case NPCID.ExplosiveBunny:
                {
                    int projDamage = 175;
                    if (self.SpawnedFromStatue)
                    {
                        projDamage = 0;
                    }
                    Projectile.NewProjectile(self.GetSpawnSource_ForProjectile(), self.Center.X, self.Center.Y, 0f, 0f, ProjectileID.ExplosiveBunny, projDamage, 0f, Main.myPlayer, -2f, self.releaseOwner + 1);
                    break;
                }
            case NPCID.Clown:
                if (!NPC.downedClown)
                {
                    NPC.downedClown = true;
                    needSend = true;
                }
                break;
            case NPCID.QueenBee:
                if (!NPC.downedQueenBee)
                {
                    needSend = true;
                }
                NPC.SetEventFlagCleared(ref NPC.downedQueenBee, GameEventClearedID.DefeatedQueenBee);
                break;
            case NPCID.CultistBoss:
                NPC.SetEventFlagCleared(ref NPC.downedAncientCultist, GameEventClearedID.DefeatedAncientCultist);
                WorldGen.TriggerLunarApocalypse();
                break;
            case NPCID.MoonLordCore:
                NPC.SetEventFlagCleared(ref NPC.downedMoonlord, GameEventClearedID.DefeatedMoonlord);
                NPC.LunarApocalypseIsUp = false;
                break;
            case NPCID.KingSlime:
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
                NPC.SetEventFlagCleared(ref NPC.downedSlimeKing, GameEventClearedID.DefeatedSlimeKing);
                break;
            case NPCID.Retinazer:
            case NPCID.Spazmatism:
                if (self.boss)
                {
                    NPC.SetEventFlagCleared(ref NPC.downedMechBoss2, GameEventClearedID.DefeatedTheTwins);
                    NPC.downedMechBossAny = true;
                }
                break;
            case NPCID.Plantera:
                {
                    bool num3 = NPC.downedPlantBoss;
                    NPC.SetEventFlagCleared(ref NPC.downedPlantBoss, GameEventClearedID.DefeatedPlantera);
                    if (!num3)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[33].Key), new Color(50, 255, 130));
                    }
                    break;
                }
            case NPCID.EyeofCthulhu:
                NPC.SetEventFlagCleared(ref NPC.downedBoss1, GameEventClearedID.DefeatedEyeOfCthulu);
                break;
            case NPCID.EaterofWorldsHead:
            case NPCID.EaterofWorldsBody:
            case NPCID.EaterofWorldsTail:
            case NPCID.BrainofCthulhu:
                if (self.boss)
                {
                    if (!NPC.downedBoss2 || Main.rand.Next(2) == 0)
                    {
                        WorldGen.spawnMeteor = true;
                    }
                    NPC.SetEventFlagCleared(ref NPC.downedBoss2, GameEventClearedID.DefeatedEaterOfWorldsOrBrainOfChtulu);
                }
                break;
            case NPCID.SkeletronHead:
                if (self.boss)
                {
                    NPC.SetEventFlagCleared(ref NPC.downedBoss3, GameEventClearedID.DefeatedSkeletron);
                }
                break;
            case NPCID.SkeletronPrime:
                if (self.boss)
                {
                    NPC.SetEventFlagCleared(ref NPC.downedMechBoss3, GameEventClearedID.DefeatedSkeletronPrime);
                    NPC.downedMechBossAny = true;
                }
                break;
            case NPCID.TheDestroyer:
                if (self.boss)
                {
                    NPC.SetEventFlagCleared(ref NPC.downedMechBoss1, GameEventClearedID.DefeatedDestroyer);
                    NPC.downedMechBossAny = true;
                }
                break;
            case NPCID.WallofFlesh:
                {
                    self.CreateBrickBoxForWallOfFlesh();
                    bool eventFlag = Main.hardMode;
                    WorldGen.StartHardmode();
                    if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && !eventFlag)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[32].Key), new Color(50, 255, 130));
                    }
                    NPC.SetEventFlagCleared(ref eventFlag, GameEventClearedID.DefeatedWallOfFleshAndStartedHardmode);
                    break;
                }
            case NPCID.EmpressButterfly:
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
            NetMessage.SendData(MessageID.WorldData);
        }
    }
    [DetourMethod]
    public static void checkDead(On.Terraria.NPC.orig_checkDead orig, NPC self)
    {
        if (!self.active || (self.realLife >= 0 && self.realLife != self.whoAmI) || self.life > 0)
        {
            return;
        }
        if (self.type == NPCID.LadyBug || self.type == NPCID.GoldLadyBug)
        {
            NPC.LadyBugKilled(self.Center, self.type == NPCID.GoldLadyBug);
        }
        if (self.type == NPCID.MoonLordHand || self.type == NPCID.MoonLordHead)
        {
            if (self.ai[0] != -2f)
            {
                self.ai[0] = -2f;
                self.life = self.lifeMax;
                self.netUpdate = true;
                self.dontTakeDamage = true;
                if (Main.netMode != 1)
                {
                    int num = NPC.NewNPC(self.GetSpawnSourceForNPCFromNPCAI(), (int)self.Center.X, (int)self.Center.Y, NPCID.MoonLordFreeEye);
                    Main.npc[num].ai[3] = self.ai[3];
                    Main.npc[num].netUpdate = true;
                }
            }
            return;
        }
        if (self.type == NPCID.MoonLordCore && self.ai[0] != 2f)
        {
            self.ai[0] = 2f;
            self.life = self.lifeMax;
            self.netUpdate = true;
            self.dontTakeDamage = true;
            return;
        }
        if ((self.type == NPCID.LunarTowerSolar || self.type == NPCID.LunarTowerVortex || self.type == NPCID.LunarTowerNebula || self.type == NPCID.LunarTowerStardust) && self.ai[2] != 1f)
        {
            self.ai[2] = 1f;
            self.ai[1] = 0f;
            self.life = self.lifeMax;
            self.dontTakeDamage = true;
            self.netUpdate = true;
            return;
        }
        if (self.type == NPCID.DD2EterniaCrystal && self.ai[1] != 1f)
        {
            self.ai[1] = 1f;
            self.ai[0] = 0f;
            self.life = self.lifeMax;
            self.dontTakeDamageFromHostiles = true;
            self.netUpdate = true;
            return;
        }
        if (Main.netMode != 1 && Main.getGoodWorld && (self.type == NPCID.Hornet || self.type == NPCID.MossHornet || (self.type >= 231 && self.type <= 235)))
        {
            self.StingerExplosion();
        }
        if (Main.netMode != 1 && Main.getGoodWorld)
        {
            if (self.type == NPCID.EaterofWorldsHead)
            {
                int num2 = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), (int)self.Center.X, (int)(self.position.Y + (float)self.height), -12);
                if (Main.netMode == 2 && num2 < 200)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num2);
                }
            }
            if (self.type == NPCID.SkeletronHand)
            {
                int num3 = 3;
                for (int i = 0; i < num3; i++)
                {
                    int num4 = 1000;
                    for (int j = 0; j < num4; j++)
                    {
                        int num5 = (int)(self.Center.X / 16f) + Main.rand.Next(-50, 51);
                        int k;
                        for (k = (int)(self.Center.Y / 16f) + Main.rand.Next(-50, 51); k < Main.maxTilesY - 200 && !WorldGen.SolidTile(num5, k); k++)
                        {
                        }
                        k--;
                        if (!WorldGen.SolidTile(num5, k))
                        {
                            int num6 = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num5 * 16 + 8, k * 16, NPCID.DarkCaster);
                            if (Main.netMode == 2 && num6 < 200)
                            {
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num6);
                            }
                            break;
                        }
                    }
                }
            }
        }
        NPC.noSpawnCycle = true;
        if (self.townNPC && self.type != NPCID.OldMan && self.type != NPCID.SkeletonMerchant)
        {
            bool dropTombstone = true;
            NetworkText fullNetName = self.GetFullNetName();
            int num7 = 19;
            if (self.type == NPCID.Angler || self.type == NPCID.Princess || NPCID.Sets.IsTownPet[self.type])
            {
                num7 = 36;
                dropTombstone = false;
            }
            NetworkText networkText = NetworkText.FromKey(Lang.misc[num7].Key, fullNetName);
            if (dropTombstone && !MainConfigInfo.StaticTownNPCDropTombstoneWhenDead)
            {
                for (int l = 0; l < 255; l++)
                {
                    Player player = Main.player[l];
                    if (player != null && player.active && player.difficulty != PlayerDifficultyID.Hardcore)
                    {
                        dropTombstone = false;
                        break;
                    }
                }
            }
            if (dropTombstone)
            {
                self.DropTombstoneTownNPC(networkText);
            }
            ChatHelper.BroadcastChatMessage(networkText, new Color(255, 25, 25));
        }
        if (Main.netMode != 1 && !Main.IsItDay() && self.type == NPCID.Clothier && !NPC.AnyNPCs(NPCID.SkeletronHead))
        {
            for (int m = 0; m < 255; m++)
            {
                if (Main.player[m].active && !Main.player[m].dead && Main.player[m].killClothier)
                {
                    NPC.SpawnSkeletron(m);
                    break;
                }
            }
        }
        if (self.townNPC && Main.netMode != 1 && self.homeless && WorldGen.prioritizedTownNPCType == self.type)
        {
            WorldGen.prioritizedTownNPCType = 0;
        }
        if (self.type == NPCID.EaterofWorldsHead || self.type == NPCID.EaterofWorldsBody || self.type == NPCID.EaterofWorldsTail)
        {
            self.DropEoWLoot();
        }
        else if (self.type == NPCID.TheDestroyer)
        {
            Vector2 vector = self.position;
            Vector2 center = Main.player[self.target].Center;
            float num8 = 100000000f;
            Vector2 vector2 = self.position;
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && (Main.npc[n].type == NPCID.TheDestroyer || Main.npc[n].type == NPCID.TheDestroyerBody || Main.npc[n].type == NPCID.TheDestroyerTail))
                {
                    float num9 = Math.Abs(Main.npc[n].Center.X - center.X) + Math.Abs(Main.npc[n].Center.Y - center.Y);
                    if (num9 < num8)
                    {
                        num8 = num9;
                        vector2 = Main.npc[n].position;
                    }
                }
            }
            self.position = vector2;
            self.NPCLoot();
            self.position = vector;
        }
        else
        {
            self.NPCLoot();
        }
        OTAPI.Hooks.NPC.InvokeKilled(self);
        self.active = false;
        if (Main.getGoodWorld && Main.netMode != 1 && self.type == NPCID.RockGolem)
        {
            Projectile.NewProjectile(self.GetSpawnSource_ForProjectile(), self.Center, Vector2.Zero, ProjectileID.Boulder, 70, 10f, Main.myPlayer);
        }
        DD2Event.CheckProgress(self.type);
        self.CheckProgressFrostMoon();
        self.CheckProgressPumpkinMoon();
        int nPCInvasionGroup = NPC.GetNPCInvasionGroup(self.type);
        if (nPCInvasionGroup <= 0 || nPCInvasionGroup != Main.invasionType)
        {
            return;
        }
        int num10 = 1;
        switch (self.type)
        {
            case NPCID.PirateCaptain:
                num10 = 5;
                break;
            case NPCID.MartianSaucerCore:
                num10 = 10;
                break;
            case NPCID.PirateShip:
                num10 = 10;
                break;
            case NPCID.GoblinSummoner:
                num10 = 10;
                break;
            case NPCID.ShadowFlameApparition:
                num10 = 0;
                break;
            case NPCID.MartianTurret:
                num10 = 0;
                break;
        }
        if (num10 > 0)
        {
            Main.invasionSize -= num10;
            if (Main.invasionSize < 0)
            {
                Main.invasionSize = 0;
            }
            if (Main.netMode != 1)
            {
                Main.ReportInvasionProgress(Main.invasionSizeStart - Main.invasionSize, Main.invasionSizeStart, nPCInvasionGroup + 3, 0);
            }
            if (Main.netMode == 2)
            {
                NetMessage.SendData(MessageID.InvasionProgressReport, -1, -1, null, Main.invasionProgress, Main.invasionProgressMax, Main.invasionProgressIcon);
            }
        }
    }
    public static bool MechSpawn(On.Terraria.NPC.orig_MechSpawn orig, float x, float y, int type)
    {
        int numOfAll = 0;
        int numOf200 = 0;
        int numOf600 = 0;
        for (int i = 0; i < 200; i++)
        {
            if (!Main.npc[i].active)
            {
                continue;
            }
            bool flag = false;
            var npcType = Main.npc[i].type;
            if (npcType == type)
            {
                flag = true;
            }
            else if (type == 74 || type == 297 || type == 298)
            {
                if (npcType == 74 || npcType == 297 || npcType == 298)
                {
                    flag = true;
                }
            }
            else if (type == 46 || type == 540 || type == 303 || type == 337)
            {
                if (npcType == 46 || npcType == 540 || npcType == 303 || npcType == 337)
                {
                    flag = true;
                }
            }
            else if (type == 362 || type == 364)
            {
                if (npcType == 362 || npcType == 363 || npcType == 364 || npcType == 365)
                {
                    flag = true;
                }
            }
            else if (type == 602)
            {
                if (npcType == 602 || npcType == 603)
                {
                    flag = true;
                }
            }
            else if (type == 608)
            {
                if (npcType == 608 || npcType == 609)
                {
                    flag = true;
                }
            }
            else if (type == 616 || type == 617)
            {
                if (npcType == 616 || npcType == 617)
                {
                    flag = true;
                }
            }
            else if (type == 55 && npcType == 230)
            {
                flag = true;
            }
            else if (NPCID.Sets.IsDragonfly[type] && NPCID.Sets.IsDragonfly[npcType])
            {
                flag = true;
            }
            if (flag)
            {
                numOfAll++;
                var length = (Main.npc[i].position - new Vector2(x, y)).Length();
                if (length < 200f)
                {
                    numOf200++;
                }
                if (length < 600f)
                {
                    numOf600++;
                }
            }
        }
        if (numOf200 >= MechInfo.StaticNPCSpawnLimitOfRange200 || numOf600 >= MechInfo.StaticNPCSpawnLimitOfRange600 || numOfAll >= MechInfo.StaticNPCSpawnLimitOfWorld)
        {
            return Hooks.NPC.InvokeMechSpawn(result: false, x, y, type, numOfAll, numOf200, numOf600);
        }
        return Hooks.NPC.InvokeMechSpawn(result: true, x, y, type, numOfAll, numOf200, numOf600);
    }
    public static void CountKillForBannersAndDropThem(On.Terraria.NPC.orig_CountKillForBannersAndDropThem orig, NPC self)
    {
        int num = Item.NPCtoBanner(self.BannerID());
        if (num <= 0 || self.ExcludedFromDeathTally())
        {
            return;
        }
        NPC.killCount[num]++;
        if (Main.netMode == 2)
        {
            NetMessage.SendData(MessageID.NPCKillCountDeathTally, -1, -1, null, num);
        }
        int num2 = ItemID.Sets.KillsToBanner[Item.BannerToItem(num)];
        if (NPC.killCount[num] % num2 == 0 && num > 0)
        {
            int num3 = Item.BannerToNPC(num);
            int num4 = self.lastInteraction;
            if (!Main.player[num4].active || Main.player[num4].dead)
            {
                num4 = self.FindClosestPlayer();
            }
            NetworkText networkText = NetworkText.FromKey("Game.EnemiesDefeatedAnnouncement", NPC.killCount[num], NetworkText.FromKey(Lang.GetNPCName(num3).Key));
            if (num4 >= 0 && num4 < 255)
            {
                networkText = NetworkText.FromKey("Game.EnemiesDefeatedByAnnouncement", Main.player[num4].name, NPC.killCount[num], NetworkText.FromKey(Lang.GetNPCName(num3).Key));
            }
            ChatHelper.BroadcastChatMessage(networkText, new Color(250, 250, 0));
            if (!MainConfigInfo.StaticNPCNotDropBanner)
            {
                int num5 = Item.BannerToItem(num);
                Vector2 vector = self.position;
                if (num4 >= 0 && num4 < 255)
                {
                    vector = Main.player[num4].position;
                }
                Item.NewItem(self.GetItemSource_Loot(), (int)vector.X, (int)vector.Y, self.width, self.height, num5);
            }
        }
    }
    [DetourMethod]
    public static bool SpawnMechQueen(On.Terraria.NPC.orig_SpawnMechQueen orig, int onWhichPlayer)
    {
        if (NPC.AnyNPCs(NPCID.SkeletronPrime) || NPC.AnyNPCs(NPCID.TheDestroyer) || NPC.AnyNPCs(NPCID.Retinazer) || NPC.AnyNPCs(NPCID.Spazmatism))
        {
            return false;
        }
        if (!Main.remixWorld && !Main.getGoodWorld)
        {
            return false;
        }
        if (SpawnInfo.MechBossInfo.StaticBulkMechQueen)
        {
            NPC.SpawnOnPlayer(onWhichPlayer, NPCID.SkeletronPrime);
            var index = NPC.FindFirstNPC(NPCID.SkeletronPrime);
            NPC.NewNPC(NPC.GetBossSpawnSource(onWhichPlayer), (int)Main.npc[index].Center.X, (int)Main.npc[index].Center.Y, NPCID.Retinazer);
            NPC.NewNPC(NPC.GetBossSpawnSource(onWhichPlayer), (int)Main.npc[index].Center.X, (int)Main.npc[index].Center.Y, NPCID.Spazmatism);
            NPC.NewNPC(NPC.GetBossSpawnSource(onWhichPlayer), (int)Main.npc[index].Center.X, (int)Main.npc[index].Center.Y, NPCID.TheDestroyer);
        }
        else
        {
            NPC.mechQueen = -2;
            NPC.SpawnOnPlayer(onWhichPlayer, NPCID.SkeletronPrime);
            NPC.mechQueen = NPC.FindFirstNPC(NPCID.SkeletronPrime);
            NPC.NewNPC(NPC.GetBossSpawnSource(onWhichPlayer), (int)Main.npc[NPC.mechQueen].Center.X, (int)Main.npc[NPC.mechQueen].Center.Y, NPCID.Retinazer, 1);
            NPC.NewNPC(NPC.GetBossSpawnSource(onWhichPlayer), (int)Main.npc[NPC.mechQueen].Center.X, (int)Main.npc[NPC.mechQueen].Center.Y, NPCID.Spazmatism, 1);
            int num = NPC.NewNPC(NPC.GetBossSpawnSource(onWhichPlayer), (int)Main.npc[NPC.mechQueen].Center.X, (int)Main.npc[NPC.mechQueen].Center.Y, NPCID.TheDestroyer, 1);
            NPC.NewNPC(NPC.GetBossSpawnSource(onWhichPlayer), (int)Main.npc[NPC.mechQueen].Center.X, (int)Main.npc[NPC.mechQueen].Center.Y, NPCID.Probe, 1, 0f, 0f, num, -1f);
            NPC.NewNPC(NPC.GetBossSpawnSource(onWhichPlayer), (int)Main.npc[NPC.mechQueen].Center.X, (int)Main.npc[NPC.mechQueen].Center.Y, NPCID.Probe, 1, 0f, 0f, num, 1f);
        }
        return true;
    }

}
file struct SpawnPoint
{
    public int SpawnX;
    public int SpawnY;

    public SpawnPoint(int spawnX, int spawnY)
    {
        SpawnX = spawnX;
        SpawnY = spawnY;
    }
    public readonly int NewNPC([IDType(nameof(NPCID))] int Type) => NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), SpawnX, SpawnY, Type);
    public readonly int NewNPC([IDType(nameof(NPCID))] int Type, int Start) => NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), SpawnX, SpawnY, Type, Start);
}