using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Events;

using static VBY.GameContentModify.GameContentModify;

namespace VBY.GameContentModify;

public static class ReplaceMain
{
    public static void UpdateTime()
    {
        if (LanternNight.LanternsUp)
        {
            Main.cloudBGActive = 0f;
            if (Main.numClouds > 30)
            {
                Main.numClouds = 30;
            }
        }
        if (Main.ladyBugRainBoost > 0)
        {
            Main.ladyBugRainBoost -= Main.dayRate;
        }
        if (Main.pumpkinMoon)
        {
            Main.bloodMoon = false;
            Main.snowMoon = false;
        }
        if (Main.snowMoon)
        {
            Main.bloodMoon = false;
        }

        if (Main.slimeRainTime > 0.0)
        {
            Main.slimeRainTime -= Main.dayRate;
            if (Main.slimeRainTime <= 0.0)
            {
                Main.StopSlimeRain();
            }
        }
        else if (Main.slimeRainTime < 0.0)
        {
            Main.slimeRainTime += Main.dayRate;
            if (Main.slimeRainTime > 0.0)
            {
                Main.slimeRainTime = 0.0;
            }
        }
        if (Main.raining)
        {
            if (!CreativePowerManager.Instance.GetPower<CreativePowers.FreezeRainPower>().Enabled)
            {
                if (LanternNight.LanternsUp)
                {
                    Main.StopRain();
                }
                else
                {
                    Main.rainTime -= Main.dayRate;
                    if (Main.dayRate > 0)
                    {
                        if (Main.rainTime <= 0)
                        {
                            Main.StopRain();
                        }
                        else if (Main.rand.Next(86400 / Main.dayRate / 12) == 0)
                        {
                            Main.ChangeRain();
                        }
                    }
                }
            }
        }
        else if (!Main.slimeRain && !LanternNight.LanternsUp && !LanternNight.NextNightIsLanternNight)
        {
            int num2 = 86400;
            num2 /= (Main.dayRate == 0) ? 1 : Main.dayRate;
            if (!CreativePowerManager.Instance.GetPower<CreativePowers.FreezeRainPower>().Enabled && Main.dayRate != 0)
            {
                if (Main.rand.Next((int)(num2 * 5.75)) == 0)
                {
                    Main.StartRain();
                }
                else if (Main.cloudBGActive >= 1f && Main.rand.Next((int)(num2 * 4.25)) == 0)
                {
                    Main.StartRain();
                }
                else if (Main.ladyBugRainBoost > 0 && Main.rand.Next(num2) == 0)
                {
                    Main.StartRain();
                }
            }
            if (!Main.raining && !NPC.BusyWithAnyInvasionOfSorts() && Main.dayTime && Main.time < 27000.0 && Main.dayRate > 0)
            {
                int num3 = (int)(450000.00000000006 / Main.dayRate);
                if (!NPC.downedSlimeKing)
                {
                    num3 /= 2;
                }
                if (Main.hardMode)
                {
                    num3 = (int)(num3 * 1.5);
                }
                bool anyPlayerReadyToFightKingSlime = Main.AnyPlayerReadyToFightKingSlime();
                if (!anyPlayerReadyToFightKingSlime)
                {
                    num3 *= 5;
                }
                if (num3 > 0 && (anyPlayerReadyToFightKingSlime || Main.expertMode) && Main.rand.Next(num3) == 0)
                {
                    Main.StartSlimeRain();
                }
            }
        }

        if (Main.maxRaining != Main.oldMaxRaining)
        {
            NetMessage.SendData(7);
            Main.oldMaxRaining = Main.maxRaining;
        }
        Main.UpdateTimeRate();
        double preUpdateTime = Main.time;
        Main.time += Main.dayRate;
        CultistRitual.UpdateTime();
        BirthdayParty.UpdateTime();
        LanternNight.UpdateTime();
        Sandstorm.UpdateTime();
        DD2Event.UpdateTime();
        CreditsRollEvent.UpdateTime();
        WorldGen.mysticLogsEvent.UpdateTime();
        Main.PylonSystem.Update();

        if (NPC.MoonLordCountdown > 0)
        {
            NPC.MoonLordCountdown--;
            if (NPC.MoonLordCountdown <= 0)
            {
                NPC.SpawnOnPlayer(Player.FindClosest(new Vector2(Main.maxTilesX / 2, (float)Main.worldSurface / 2f) * 16f, 0, 0), 398);
            }
        }

        Main.UpdateSlimeRainWarning();

        if (NPC.travelNPC)
        {
            if (!Main.dayTime || Main.time > 48600.0)
            {
                WorldGen.UnspawnTravelNPC();
            }
        }
        else if (!Main.IsFastForwardingTime() && Main.dayTime && Main.time < 27000.0)
        {
            int dayRate = Main.dayRate;
            if (dayRate < 1)
            {
                dayRate = 1;
            }
            int randomNum = (int)(27000.0 / dayRate);
            randomNum *= 4;
            if (Main.rand.Next(randomNum) == 0)
            {
                int townNPCCount = 0;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].townNPC && Main.npc[i].type != 37 && Main.npc[i].type != 453)
                    {
                        townNPCCount++;
                        if (townNPCCount >= 2)
                        {
                            WorldGen.SpawnTravelNPC();
                            break;
                        }
                    }
                }
            }
        }
        NPC.travelNPC = false;

        bool stopEvents = Main.ShouldNormalEventsBeAbleToStart();
        if (Main.dayTime)
        {
            WorldGen.spawnHardBoss = 0;
            WorldGen.spawnEye = false;
            Main.bloodMoon = false;
            Main.stopMoonEvent();
            if (Main.time > 54000.0)
            {
                Main.UpdateTime_StartNight(ref stopEvents);
            }
            Main.UpdateTime_SpawnTownNPCs();
        }
        else
        {
            Main.eclipse = false;
            if (!Main.IsFastForwardingTime() && !stopEvents)
            {
                if (WorldGen.spawnEye && Main.time > 4860.0)
                {
                    for (int j = 0; j < 255; j++)
                    {
                        if (Main.player[j].active && !Main.player[j].dead && (Main.player[j].position.Y < Main.worldSurface * 16.0 || Main.remixWorld))
                        {
                            NPC.SpawnOnPlayer(j, 4);
                            WorldGen.spawnEye = false;
                            break;
                        }
                    }
                }
                if (WorldGen.spawnHardBoss > 0 && Main.time > 4860.0)
                {
                    bool haveBossInWorld = false;
                    if (MainConfig.Instance.Spawn.MechBossSpawnHaveBossInWorldCheck)
                    {
                        for (int k = 0; k < 200; k++)
                        {
                            if (Main.npc[k].active && Main.npc[k].boss)
                            {
                                haveBossInWorld = true;
                                break;
                            }
                        }
                    }
                    if (!haveBossInWorld)
                    {
                        for (int l = 0; l < 255; l++)
                        {
                            if (Main.player[l].active && !Main.player[l].dead && (Main.player[l].position.Y < Main.worldSurface * 16.0 || Main.remixWorld))
                            {
                                if (MainConfig.Instance.Spawn.MechBossSpawnIsOr)
                                {
                                    if (Main.remixWorld && Main.getGoodWorld)
                                    {
                                        NPC.SpawnMechQueen(l);
                                    }
                                    else
                                    {
                                        if ((WorldGen.spawnHardBoss & 1) == 1)
                                        {
                                            NPC.SpawnOnPlayer(l, 134);
                                        }
                                        if ((WorldGen.spawnHardBoss & 2) == 2)
                                        {
                                            NPC.SpawnOnPlayer(l, 125);
                                            NPC.SpawnOnPlayer(l, 126);
                                        }
                                        if ((WorldGen.spawnHardBoss & 4) == 4)
                                        {
                                            NPC.SpawnOnPlayer(l, 127);
                                        }
                                    }
                                }
                                else
                                {
                                    if (Main.remixWorld && Main.getGoodWorld)
                                    {
                                        NPC.SpawnMechQueen(l);
                                    }
                                    else if (WorldGen.spawnHardBoss == 1)
                                    {
                                        NPC.SpawnOnPlayer(l, 134);
                                    }
                                    else if (WorldGen.spawnHardBoss == 2)
                                    {
                                        NPC.SpawnOnPlayer(l, 125);
                                        NPC.SpawnOnPlayer(l, 126);
                                    }
                                    else if (WorldGen.spawnHardBoss == 3)
                                    {
                                        NPC.SpawnOnPlayer(l, 127);
                                    }
                                }
                                break;
                            }
                        }
                    }
                    WorldGen.spawnHardBoss = 0;
                }

                if (preUpdateTime < 16200.0 && Main.time >= 16200.0 && Main.raining && (!NPC.downedDeerclops || Main.rand.Next(4) == 0))
                {
                    for (int m = 0; m < 255; m++)
                    {
                        Player player = Main.player[m];
                        if (player.active && !player.dead && !(player.position.Y >= Main.worldSurface * 16.0) && player.ZoneSnow && !(player.townNPCs > 0f) && (player.statLifeMax2 >= 200 || player.statDefense >= 9) && !NPC.AnyDanger())
                        {
                            NPC.SpawnOnPlayer(m, 668);
                            break;
                        }
                    }
                }

            }
            if (Main.time > 32400.0)
            {
                Main.UpdateTime_StartDay(ref stopEvents);
            }
            Main.HandleMeteorFall();
        }
    }
    public static void UpdateTime_StartDay(ref bool stopEvents)
    {
        OnPreStartDay();
        WorldGen.ResetTreeShakes();
        if (Main.fastForwardTimeToDawn)
        {
            Main.fastForwardTimeToDawn = false;
            Main.UpdateTimeRate();
        }
        Main.AnglerQuestSwap();
        BirthdayParty.CheckMorning();
        LanternNight.CheckMorning();
        if (Main.invasionDelay > 0)
        {
            Main.invasionDelay--;
        }
        WorldGen.prioritizedTownNPCType = 0;
        Main.checkForSpawns = 0;
        Main.time = 0.0;
        if (Main.bloodMoon)
        {
            AchievementsHelper.NotifyProgressionEvent(5);
        }
        Main.bloodMoon = false;
        Main.CheckForMoonEventsScoreDisplay();
        Main.CheckForMoonEventsStartingTemporarySeasons();
        Main.checkXMas();
        Main.checkHalloween();
        Main.stopMoonEvent();
        Main.dayTime = true;
        if (Main.sundialCooldown > 0)
        {
            Main.sundialCooldown--;
        }
        Main.moonPhase++;
        if (Main.moonPhase >= 8)
        {
            Main.moonPhase = 0;
        }
        if (Main.drunkWorld)
        {
            WorldGen.crimson = !WorldGen.crimson;
        }
        NetMessage.SendData(7);
        AchievementsHelper.NotifyProgressionEvent(1);
        if (stopEvents)
        {
            return;
        }
        if (Main.hardMode && NPC.downedMechBossAny && Main.rand.Next(MainConfig.Instance.EclipseRandomNum) == 0)
        {
            Main.sundialCooldown = 0;
            Main.moondialCooldown = 0;
            Main.eclipse = true;
            AchievementsHelper.NotifyProgressionEvent(2);
            if (Main.eclipse)
            {
                if (Main.remixWorld)
                {
                    ChatHelper.BroadcastChatMessage(Lang.misc[106].ToNetworkText(), new Color(50, 255, 130));
                }
                else
                {
                    ChatHelper.BroadcastChatMessage(Lang.misc[20].ToNetworkText(), new Color(50, 255, 130));
                }
            }
            NetMessage.SendData(7);
        }
        else
        {
            if (Main.snowMoon || Main.pumpkinMoon || DD2Event.Ongoing)
            {
                return;
            }
            if (WorldGen.shadowOrbSmashed)
            {
                if (!NPC.downedGoblins)
                {
                    if (Main.rand.Next(MainConfig.Instance.Invasion.NoDownedGoblinsStartInvasionRandomNum) == 0)
                    {
                        Main.StartInvasion();
                    }
                }
                else if ((Main.hardMode && Main.rand.Next(MainConfig.Instance.Invasion.HardModeDownedGoblinsStartInvasionRandomNum) == 0) || (!Main.hardMode && Main.rand.Next(MainConfig.Instance.Invasion.DownedGoblinsStartInvasionRandomNum) == 0))
                {
                    Main.StartInvasion();
                }
            }
            if (Main.invasionType == 0 && Main.hardMode && WorldGen.altarCount > 0 && ((NPC.downedPirates && Main.rand.Next(MainConfig.Instance.Invasion.DownedPiratesStartInvasionRandomNum) == 0) || (!NPC.downedPirates && Main.rand.Next(MainConfig.Instance.Invasion.NoDownedPiratesStartInvasionRandomNum) == 0)))
            {
                Main.StartInvasion(3);
            }
        }
        if (!NPC.travelNPC && (MainConfig.Instance.Spawn.SpawnTravelNPCAtDay))
        {
            if (Main.rand.Next(MainConfig.Instance.Spawn.SpawnMeteorRandomNum) == 0)
            {
                WorldGen.SpawnTravelNPC();
            }
        }
    }
    public static void UpdateTime_StartNight(ref bool stopEvents)
    {
        OnPreStartNight();
        if (Main.fastForwardTimeToDusk)
        {
            Main.fastForwardTimeToDusk = false;
            Main.UpdateTimeRate();
        }
        if (Main.moondialCooldown > 0)
        {
            Main.moondialCooldown--;
        }
        NPC.ResetBadgerHatTime();
        NPC.freeCake = false;
        Star.NightSetup();
        NPC.setFireFlyChance();
        BirthdayParty.CheckNight();
        LanternNight.CheckNight();
        WorldGen.mysticLogsEvent.StartNight();
        WorldGen.prioritizedTownNPCType = 0;
        Main.checkForSpawns = 0;
        if (Main.rand.Next(MainConfig.Instance.Spawn.SpawnMeteorRandomNum) == 0 && NPC.downedBoss2)
        {
            WorldGen.spawnMeteor = true;
        }
        if (LanternNight.LanternsUp)
        {
            stopEvents = true;
        }
        if (Main.eclipse)
        {
            AchievementsHelper.NotifyProgressionEvent(3);
        }
        Main.eclipse = false;
        AchievementsHelper.NotifyProgressionEvent(0);
        if (!Main.IsFastForwardingTime() && !stopEvents)
        {
            if (!(MainConfig.Instance.Spawn.EyeSpawnDownedCheck) || !NPC.downedBoss1)
            {
                bool flag = false;
                if (MainConfig.Instance.Spawn.EyeSpawnLifeAndDefenseCheck)
                {
                    for (int i = 0; i < 255; i++)
                    {
                        if (Main.player[i].active && Main.player[i].statLifeMax >= 200 && Main.player[i].statDefense > 10)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                else
                {
                    flag = true;
                }
                if (flag && Main.rand.Next(NPC.downedBoss1 ? MainConfig.Instance.Spawn.DownedEyeSpawnRandomNum : MainConfig.Instance.Spawn.EyeSpawnRandomNum) == 0)
                {
                    int townNPCCount = 0;
                    if (MainConfig.Instance.Spawn.EyeSpawnTownNPCCountCheck)
                    {
                        for (int j = 0; j < 200; j++)
                        {
                            if (Main.npc[j].active && Main.npc[j].townNPC)
                            {
                                townNPCCount++;
                            }
                        }
                    }
                    else
                    {
                        townNPCCount = 4;
                    }
                    if (townNPCCount >= 4)
                    {
                        WorldGen.spawnEye = true;
                        ChatHelper.BroadcastChatMessage(Lang.misc[9].ToNetworkText(), new Color(50, 255, 130));
                    }
                }
            }
            if (!Main.pumpkinMoon && !DD2Event.Ongoing && !Main.snowMoon && WorldGen.altarCount > 0 && Main.hardMode && (!(MainConfig.Instance.Spawn.MechBossSpawnEyeCheck) || !WorldGen.spawnEye) && Main.rand.Next(MainConfig.Instance.Spawn.MechBossSpawnRandomNum) == 0)
            {
                bool haveBossInWorld = false;
                if (MainConfig.Instance.Spawn.MechBossSpawnHaveBossInWorldCheck)
                {
                    for (int k = 0; k < 200; k++)
                    {
                        if (Main.npc[k].active && Main.npc[k].boss)
                        {
                            haveBossInWorld = true;
                        }
                    }
                }
                if (!haveBossInWorld && (!(MainConfig.Instance.Spawn.MechBossSpawnDownedCheck) || (!NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3)))
                {
                    if (Main.remixWorld && Main.getGoodWorld)
                    {
                        if (Main.rand.Next(2) == 0)
                        {
                            WorldGen.spawnHardBoss = Main.rand.Next(3) + 1;
                            ChatHelper.BroadcastChatMessage(Lang.misc[108].ToNetworkText(), new Color(50, 255, 130));
                        }
                    }
                    else
                    {
                        for (int l = 0; l < 1000; l++)
                        {
                            if (MainConfig.Instance.Spawn.MechBossSpawnIsOr)
                            {
                                int spawnHardBossNum = Main.rand.Next(7) + 1;
                                var spawnHardBoss = false;
                                if ((spawnHardBossNum & 1) == 1 && (!(MainConfig.Instance.Spawn.MechBossSpawnDownedCheck) || !NPC.downedMechBoss1))
                                {
                                    spawnHardBoss = true;
                                }
                                if ((spawnHardBossNum & 2) == 2 && (!(MainConfig.Instance.Spawn.MechBossSpawnDownedCheck) || !NPC.downedMechBoss2))
                                {
                                    spawnHardBoss = true;
                                }
                                if ((spawnHardBossNum & 4) == 4 && (!(MainConfig.Instance.Spawn.MechBossSpawnDownedCheck) || !NPC.downedMechBoss3))
                                {
                                    spawnHardBoss = true;
                                }
                                if (spawnHardBoss)
                                {
                                    WorldGen.spawnHardBoss = spawnHardBossNum;
                                    if ((spawnHardBossNum & 1) == 1)
                                    {
                                        ChatHelper.BroadcastChatMessage(Lang.misc[27 + 1].ToNetworkText(), new Color(50, 255, 130));
                                    }
                                    if ((spawnHardBossNum & 2) == 2)
                                    {
                                        ChatHelper.BroadcastChatMessage(Lang.misc[27 + 2].ToNetworkText(), new Color(50, 255, 130));
                                    }
                                    if ((spawnHardBossNum & 4) == 4)
                                    {
                                        ChatHelper.BroadcastChatMessage(Lang.misc[27 + 3].ToNetworkText(), new Color(50, 255, 130));
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                int spawnHardBossNum = Main.rand.Next(3) + 1;
                                var spawnHardBoss = false;
                                if (spawnHardBossNum == 1 && (!(MainConfig.Instance.Spawn.MechBossSpawnDownedCheck) || !NPC.downedMechBoss1))
                                {
                                    spawnHardBoss = true;
                                }
                                if (spawnHardBossNum == 2 && (!(MainConfig.Instance.Spawn.MechBossSpawnDownedCheck) || !NPC.downedMechBoss2))
                                {
                                    spawnHardBoss = true;
                                }
                                if (spawnHardBossNum == 3 && (!(MainConfig.Instance.Spawn.MechBossSpawnDownedCheck) || !NPC.downedMechBoss3))
                                {
                                    spawnHardBoss = true;
                                }
                                if (spawnHardBoss)
                                {
                                    WorldGen.spawnHardBoss = spawnHardBossNum;
                                    ChatHelper.BroadcastChatMessage(Lang.misc[27 + spawnHardBossNum].ToNetworkText(), new Color(50, 255, 130));
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            //int maxValue = 9;
            int maxValue = MainConfig.Instance.BloodMoon.RandomNum;
            if (Main.tenthAnniversaryWorld)
            {
                //maxValue = 6;
                maxValue = MainConfig.Instance.BloodMoon.TenthAnniversaryWorldRandomNum;
            }
            if ((!MainConfig.Instance.BloodMoon.SpawnEyeCheck || !WorldGen.spawnEye) && Main.moonPhase != 4 && Main.rand.Next(maxValue) == 0)
            {
                if (MainConfig.Instance.BloodMoon.LifeCheck)
                {
                    for (int m = 0; m < 255; m++)
                    {
                        if (Main.player[m].active && Main.player[m].statLifeMax > 120)
                        {
                            Main.bloodMoon = true;
                            break;
                        }
                    }
                }
                else
                {
                    Main.bloodMoon = true;
                }
                if (Main.bloodMoon)
                {
                    if (MainConfig.Instance.BloodMoon.ClearSundialAndMoondialCooldown)
                    {
                        Main.sundialCooldown = 0;
                        Main.moondialCooldown = 0;
                    }
                    AchievementsHelper.NotifyProgressionEvent(4);
                    ChatHelper.BroadcastChatMessage(Lang.misc[8].ToNetworkText(), new Color(50, 255, 130));
                }
            }
        }
        Main.time = 0.0;
        Main.dayTime = false;
        NetMessage.SendData(7);
    }
}
