using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Events;

using VBY.GameContentModify.Config;
using static VBY.GameContentModify.GameContentModify;

namespace VBY.GameContentModify;

[ReplaceType(typeof(Main))]
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
            //int startRainRandomNumberBase = 86400;
            int startRainBaseRandomNum = MainConfigInfo.StaticStartRainBaseRandomNum;
            startRainBaseRandomNum /= (Main.dayRate == 0) ? 1 : Main.dayRate;
            if (!CreativePowerManager.Instance.GetPower<CreativePowers.FreezeRainPower>().Enabled && Main.dayRate != 0)
            {
                if (Main.rand.NextIsZero((int)(startRainBaseRandomNum * 5.75)))
                {
                    Main.StartRain();
                }
                else if (Main.cloudBGActive >= 1f && Main.rand.NextIsZero((int)(startRainBaseRandomNum * 4.25)))
                {
                    Main.StartRain();
                }
                else if (Main.ladyBugRainBoost > 0 && Main.rand.NextIsZero(startRainBaseRandomNum))
                {
                    Main.StartRain();
                }
            }
            if (!Main.raining && !NPC.BusyWithAnyInvasionOfSorts() && Main.dayTime && Main.time < 27000.0 && Main.dayRate > 0)
            {
                //int randomNumOfStartSlimeRain = (int)(450000.00000000006 / Main.dayRate);
                int randomNumOfStartSlimeRain = (int)(MainConfigInfo.StaticDividendOfStartSlimeRainRandomNum / Main.dayRate);
                if (!NPC.downedSlimeKing)
                {
                    randomNumOfStartSlimeRain /= 2;
                }
                if (Main.hardMode)
                {
                    randomNumOfStartSlimeRain = (int)(randomNumOfStartSlimeRain * 1.5);
                }
                bool anyPlayerReadyToFightKingSlime = Main.AnyPlayerReadyToFightKingSlime();
                if (!anyPlayerReadyToFightKingSlime)
                {
                    randomNumOfStartSlimeRain *= 5;
                }
                if (randomNumOfStartSlimeRain > 0 && (anyPlayerReadyToFightKingSlime || Main.expertMode) && Main.rand.Next(randomNumOfStartSlimeRain) == 0)
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
                    //if (MainConfig.Instance.Spawn.MechBossSpawnHaveBossInWorldCheck)
                    if (SpawnInfo.MechBossInfo.StaticSpawnHaveBossInWorldCheck)
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
                                //if (MainConfig.Instance.Spawn.MechBossSpawnIsOr)
                                if (SpawnInfo.MechBossInfo.StaticSpawnIsOr)
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
        if (Main.hardMode && NPC.downedMechBossAny && Main.rand.NextIsZero(MainConfig.Instance.EclipseRandomNum))
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
                    if (Main.rand.NextIsZero(MainConfig.Instance.Invasion.NoDownedGoblinsStartInvasionRandomNum))
                    {
                        Main.StartInvasion();
                    }
                }
                else if ((Main.hardMode && Main.rand.NextIsZero(MainConfig.Instance.Invasion.HardModeDownedGoblinsStartInvasionRandomNum)) || (!Main.hardMode && Main.rand.NextIsZero(MainConfig.Instance.Invasion.DownedGoblinsStartInvasionRandomNum)))
                {
                    Main.StartInvasion();
                }
            }
            if (Main.invasionType == 0 && Main.hardMode && WorldGen.altarCount > 0 && ((NPC.downedPirates && Main.rand.NextIsZero(MainConfig.Instance.Invasion.DownedPiratesStartInvasionRandomNum)) || (!NPC.downedPirates && Main.rand.NextIsZero(MainConfig.Instance.Invasion.NoDownedPiratesStartInvasionRandomNum))))
            {
                Main.StartInvasion(3);
            }
        }
        //if (!NPC.travelNPC && (MainConfig.Instance.Spawn.SpawnTravelNPCAtDay))
        if (!NPC.travelNPC && (MainConfig.Instance.Extension.SpawnTravelNPCWhenStartDay))
        {
            if (Main.rand.NextIsZero(MainConfig.Instance.Extension.SpawnTravelNPCWhenStartDayRandomNum))
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
        if (Main.rand.NextIsZero(MainConfig.Instance.Spawn.SpawnMeteorRandomNum) && NPC.downedBoss2)
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
            //if (MainConfig.Instance.Spawn.EyeSpawnDownedCheck.IsTrueRet(!NPC.downedBoss1))
            if (SpawnInfo.EyeOfCthulhuInfo.StaticDownedCheck.IsTrueRet(!NPC.downedBoss1))
            {
                bool flag = false;
                //if (MainConfig.Instance.Spawn.EyeSpawnLifeAndDefenseCheck)
                if (SpawnInfo.EyeOfCthulhuInfo.StaticLifeAndDefenseCheck)
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
                //if (flag && Main.rand.NextIsZero(NPC.downedBoss1 ? MainConfig.Instance.Spawn.DownedEyeSpawnRandomNum : MainConfig.Instance.Spawn.EyeSpawnRandomNum))
                if (flag && Main.rand.NextIsZero(NPC.downedBoss1 ? SpawnInfo.EyeOfCthulhuInfo.StaticDownedRandomNum : SpawnInfo.EyeOfCthulhuInfo.StaticRandomNum))
                {
                    int townNPCCount = 0;
                    //if (MainConfig.Instance.Spawn.EyeSpawnTownNPCCountCheck)
                    if (SpawnInfo.EyeOfCthulhuInfo.StaticTownNPCCountCheck)
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
            //if (!Main.pumpkinMoon && !DD2Event.Ongoing && !Main.snowMoon && WorldGen.altarCount > 0 && Main.hardMode && MainConfig.Instance.Spawn.MechBossSpawnEyeCheck.IsTrueRet(!WorldGen.spawnEye) && Main.rand.NextIsZero(MainConfig.Instance.Spawn.MechBossSpawnRandomNum))
            if (!Main.pumpkinMoon && !DD2Event.Ongoing && !Main.snowMoon && WorldGen.altarCount > 0 && Main.hardMode && SpawnInfo.MechBossInfo.StaticSpawnEyeCheck.IsTrueRet(!WorldGen.spawnEye) && Main.rand.NextIsZero(SpawnInfo.MechBossInfo.StaticSpawnRandomNum))
            {
                bool haveBossInWorld = false;
                //if (MainConfig.Instance.Spawn.MechBossSpawnHaveBossInWorldCheck)
                if (SpawnInfo.MechBossInfo.StaticSpawnHaveBossInWorldCheck)
                {
                    for (int k = 0; k < 200; k++)
                    {
                        if (Main.npc[k].active && Main.npc[k].boss)
                        {
                            haveBossInWorld = true;
                        }
                    }
                }
                //if (!haveBossInWorld && MainConfig.Instance.Spawn.MechBossSpawnDownedCheck.IsTrueRet(!NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3))
                if (!haveBossInWorld && SpawnInfo.MechBossInfo.StaticSpawnDownedCheck.IsTrueRet(!NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3))
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
                            //if (MainConfig.Instance.Spawn.MechBossSpawnIsOr)
                            if (SpawnInfo.MechBossInfo.StaticSpawnIsOr)
                            {
                                int spawnHardBossNum = Main.rand.Next(7) + 1;
                                var spawnHardBoss = false;
                                //if ((spawnHardBossNum & 1) == 1 && (!(MainConfig.Instance.Spawn.MechBossSpawnDownedCheck) || !NPC.downedMechBoss1))
                                if ((spawnHardBossNum & 1) == 1 && (!(SpawnInfo.MechBossInfo.StaticSpawnDownedCheck) || !NPC.downedMechBoss1))
                                {
                                    spawnHardBoss = true;
                                }
                                //if ((spawnHardBossNum & 2) == 2 && (!(MainConfig.Instance.Spawn.MechBossSpawnDownedCheck) || !NPC.downedMechBoss2))
                                if ((spawnHardBossNum & 2) == 2 && (!(SpawnInfo.MechBossInfo.StaticSpawnDownedCheck) || !NPC.downedMechBoss2))
                                {
                                    spawnHardBoss = true;
                                }
                                //if ((spawnHardBossNum & 4) == 4 && (!(MainConfig.Instance.Spawn.MechBossSpawnDownedCheck) || !NPC.downedMechBoss3))
                                if ((spawnHardBossNum & 4) == 4 && (!(SpawnInfo.MechBossInfo.StaticSpawnDownedCheck) || !NPC.downedMechBoss3))
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
                                //if (spawnHardBossNum == 1 && (!(MainConfig.Instance.Spawn.MechBossSpawnDownedCheck) || !NPC.downedMechBoss1))
                                if (spawnHardBossNum == 1 && (!(SpawnInfo.MechBossInfo.StaticSpawnDownedCheck) || !NPC.downedMechBoss1))
                                {
                                    spawnHardBoss = true;
                                }
                                //if (spawnHardBossNum == 2 && (!(MainConfig.Instance.Spawn.MechBossSpawnDownedCheck) || !NPC.downedMechBoss2))
                                if (spawnHardBossNum == 2 && (!(SpawnInfo.MechBossInfo.StaticSpawnDownedCheck) || !NPC.downedMechBoss2))
                                {
                                    spawnHardBoss = true;
                                }
                                //if (spawnHardBossNum == 3 && (!(MainConfig.Instance.Spawn.MechBossSpawnDownedCheck) || !NPC.downedMechBoss3))
                                if (spawnHardBossNum == 3 && (!(SpawnInfo.MechBossInfo.StaticSpawnDownedCheck) || !NPC.downedMechBoss3))
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
            if (MainConfig.Instance.BloodMoon.SpawnEyeCheck.IsTrueRet(!WorldGen.spawnEye) && Main.moonPhase != 4 && Main.rand.NextIsZero(maxValue))
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
    public static void UpdateTime_SpawnTownNPCs()
    {
        if (SpawnInfo.TownNPCInfo.StaticDisableSpawn)
        {
            return;
        }
        int worldUpdateRate = WorldGen.GetWorldUpdateRate();
        if (worldUpdateRate <= 0)
        {
            if (!SpawnInfo.TownNPCInfo.StaticSpawnStillWhenTimeRateIsZero)
            {
                return;
            }
            worldUpdateRate = SpawnInfo.TownNPCInfo.StaticSpawnTimeRateWhenTimeRateIsZero;
        }
        Main.checkForSpawns++;
        if (Main.checkForSpawns < 7200 / worldUpdateRate)
        {
            return;
        }
        Main.checkForSpawns = 0;
        Array.Fill(Main.townNPCCanSpawn, false);
        WorldGen.prioritizedTownNPCType = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        int num5 = 0;
        int num6 = 0;
        int num7 = 0;
        int num8 = 0;
        int num9 = 0;
        int num10 = 0;
        int num11 = 0;
        int num12 = 0;
        int num13 = 0;
        int num14 = 0;
        int num15 = 0;
        int num16 = 0;
        int num17 = 0;
        int num18 = 0;
        int num19 = 0;
        int num20 = 0;
        int num21 = 0;
        int num22 = 0;
        int num23 = 0;
        int num24 = 0;
        int num25 = 0;
        int num26 = 0;
        int num27 = 0;
        int num28 = 0;
        int num29 = 0;
        int num30 = 0;
        int num31 = 0;
        int num32 = 0;
        int num33 = 0;
        int num34 = 0;
        int num35 = 0;
        int num36 = 0;
        int num37 = 0;
        int num38 = 0;
        int num39 = 0;
        int num40 = 0;
        for (int k = 0; k < 200; k++)
        {
            if (Main.npc[k].active && Main.npc[k].townNPC)
            {
                if (Main.npc[k].type != 368 && Main.npc[k].type != 37 && Main.npc[k].type != 453 && !Main.npc[k].homeless)
                {
                    WorldGen.QuickFindHome(k);
                }
                if (Main.npc[k].type == 37)
                {
                    num7++;
                }
                if (Main.npc[k].type == 17)
                {
                    num2++;
                }
                if (Main.npc[k].type == 18)
                {
                    num3++;
                }
                if (Main.npc[k].type == 19)
                {
                    num5++;
                }
                if (Main.npc[k].type == 20)
                {
                    num4++;
                }
                if (Main.npc[k].type == 22)
                {
                    num6++;
                }
                if (Main.npc[k].type == 38)
                {
                    num8++;
                }
                if (Main.npc[k].type == 54)
                {
                    num9++;
                }
                if (Main.npc[k].type == 107)
                {
                    num11++;
                }
                if (Main.npc[k].type == 108)
                {
                    num10++;
                }
                if (Main.npc[k].type == 124)
                {
                    num12++;
                }
                if (Main.npc[k].type == 142)
                {
                    num13++;
                }
                if (Main.npc[k].type == 160)
                {
                    num14++;
                }
                if (Main.npc[k].type == 178)
                {
                    num15++;
                }
                if (Main.npc[k].type == 207)
                {
                    num16++;
                }
                if (Main.npc[k].type == 208)
                {
                    num17++;
                }
                if (Main.npc[k].type == 209)
                {
                    num18++;
                }
                if (Main.npc[k].type == 227)
                {
                    num19++;
                }
                if (Main.npc[k].type == 228)
                {
                    num20++;
                }
                if (Main.npc[k].type == 229)
                {
                    num21++;
                }
                if (Main.npc[k].type == 353)
                {
                    num22++;
                }
                if (Main.npc[k].type == 369)
                {
                    num23++;
                }
                if (Main.npc[k].type == 441)
                {
                    num24++;
                }
                if (Main.npc[k].type == 550)
                {
                    num25++;
                }
                if (Main.npc[k].type == 588)
                {
                    num26++;
                }
                if (Main.npc[k].type == 633)
                {
                    num27++;
                }
                if (Main.npc[k].type == 637)
                {
                    num28++;
                }
                if (Main.npc[k].type == 638)
                {
                    num29++;
                }
                if (Main.npc[k].type == 656)
                {
                    num30++;
                }
                if (Main.npc[k].type == 670)
                {
                    num31++;
                }
                if (Main.npc[k].type == 678)
                {
                    num32++;
                }
                if (Main.npc[k].type == 679)
                {
                    num33++;
                }
                if (Main.npc[k].type == 680)
                {
                    num34++;
                }
                if (Main.npc[k].type == 681)
                {
                    num35++;
                }
                if (Main.npc[k].type == 682)
                {
                    num36++;
                }
                if (Main.npc[k].type == 683)
                {
                    num37++;
                }
                if (Main.npc[k].type == 684)
                {
                    num38++;
                }
                if (Main.npc[k].type == 663)
                {
                    num39++;
                }
                num40++;
            }
        }
        if (WorldGen.prioritizedTownNPCType == 0)
        {
            bool flag = NPC.SpawnAllowed_Merchant();
            bool flag2 = NPC.SpawnAllowed_ArmsDealer();
            bool flag3 = NPC.SpawnAllowed_Nurse();
            bool flag4 = NPC.SpawnAllowed_DyeTrader();
            bool flag5 = NPC.SpawnAllowed_Demolitionist();
            BestiaryUnlockProgressReport bestiaryProgressReport = Main.GetBestiaryProgressReport();
            if (!NPC.downedBoss3 && num7 == 0)
            {
                int num41 = NPC.NewNPC(NPC.GetSpawnSourceForTownSpawn(), Main.dungeonX * 16 + 8, Main.dungeonY * 16, 37);
                Main.npc[num41].homeless = false;
                Main.npc[num41].homeTileX = Main.dungeonX;
                Main.npc[num41].homeTileY = Main.dungeonY;
            }
            bool flag6 = false;
            if (Main.rand.Next(40) == 0)
            {
                flag6 = true;
            }
            bool flag7 = flag6 && num40 >= 14;
            if (NPC.unlockedPartyGirlSpawn)
            {
                flag7 = true;
            }
            bool flag8 = BirthdayParty.GenuineParty;
            if (NPC.unlockedSlimeGreenSpawn)
            {
                flag8 = true;
            }
            if (num6 < 1)
            {
                Main.townNPCCanSpawn[22] = true;
            }
            if (flag && num2 < 1)
            {
                Main.townNPCCanSpawn[17] = true;
            }
            if (flag3 && num3 < 1 && num2 > 0)
            {
                Main.townNPCCanSpawn[18] = true;
            }
            if (flag2 && num5 < 1)
            {
                Main.townNPCCanSpawn[19] = true;
            }
            if ((NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3) && num4 < 1)
            {
                Main.townNPCCanSpawn[20] = true;
            }
            if (flag5 && num2 > 0 && num8 < 1)
            {
                Main.townNPCCanSpawn[38] = true;
            }
            if (NPC.savedStylist && num22 < 1)
            {
                Main.townNPCCanSpawn[353] = true;
            }
            if (NPC.savedAngler && num23 < 1)
            {
                Main.townNPCCanSpawn[369] = true;
            }
            if (NPC.downedBoss3 && num9 < 1)
            {
                Main.townNPCCanSpawn[54] = true;
            }
            if (NPC.savedGoblin && num11 < 1)
            {
                Main.townNPCCanSpawn[107] = true;
            }
            if (NPC.savedTaxCollector && num24 < 1)
            {
                Main.townNPCCanSpawn[441] = true;
            }
            if (NPC.savedWizard && num10 < 1)
            {
                Main.townNPCCanSpawn[108] = true;
            }
            if (NPC.savedMech && num12 < 1)
            {
                Main.townNPCCanSpawn[124] = true;
            }
            if (NPC.downedFrost && num13 < 1 && Main.xMas)
            {
                Main.townNPCCanSpawn[142] = true;
            }
            if (((Main.tenthAnniversaryWorld && !Main.remixWorld) || NPC.downedMechBossAny) && num15 < 1)
            {
                Main.townNPCCanSpawn[178] = true;
            }
            if (flag4 && num16 < 1 && num40 >= 4)
            {
                Main.townNPCCanSpawn[207] = true;
            }
            if (NPC.downedQueenBee && num20 < 1)
            {
                Main.townNPCCanSpawn[228] = true;
            }
            if (NPC.downedPirates && num21 < 1)
            {
                Main.townNPCCanSpawn[229] = true;
            }
            if (num14 < 1 && Main.hardMode)
            {
                Main.townNPCCanSpawn[160] = true;
            }
            if (Main.hardMode && NPC.downedPlantBoss && num18 < 1)
            {
                Main.townNPCCanSpawn[209] = true;
            }
            if (num40 >= 8 && num19 < 1)
            {
                Main.townNPCCanSpawn[227] = true;
            }
            if (flag7 && num17 < 1)
            {
                Main.townNPCCanSpawn[208] = true;
            }
            if (NPC.savedBartender && num25 < 1)
            {
                Main.townNPCCanSpawn[550] = true;
            }
            if (NPC.savedGolfer && num26 < 1)
            {
                Main.townNPCCanSpawn[588] = true;
            }
            if (bestiaryProgressReport.CompletionPercent >= 0.1f && num27 < 1)
            {
                Main.townNPCCanSpawn[633] = true;
            }
            if (NPC.boughtCat && num28 < 1)
            {
                Main.townNPCCanSpawn[637] = true;
            }
            if (NPC.boughtDog && num29 < 1)
            {
                Main.townNPCCanSpawn[638] = true;
            }
            if (NPC.boughtBunny && num30 < 1)
            {
                Main.townNPCCanSpawn[656] = true;
            }
            if (NPC.unlockedSlimeBlueSpawn && num31 < 1)
            {
                Main.townNPCCanSpawn[670] = true;
            }
            if (flag8 && num32 < 1)
            {
                Main.townNPCCanSpawn[678] = true;
            }
            if (NPC.unlockedSlimeOldSpawn && num33 < 1)
            {
                Main.townNPCCanSpawn[679] = true;
            }
            if (NPC.unlockedSlimePurpleSpawn && num34 < 1)
            {
                Main.townNPCCanSpawn[680] = true;
            }
            if (NPC.unlockedSlimeRainbowSpawn && num35 < 1)
            {
                Main.townNPCCanSpawn[681] = true;
            }
            if (NPC.unlockedSlimeRedSpawn && num36 < 1)
            {
                Main.townNPCCanSpawn[682] = true;
            }
            if (NPC.unlockedSlimeYellowSpawn && num37 < 1)
            {
                Main.townNPCCanSpawn[683] = true;
            }
            if (NPC.unlockedSlimeCopperSpawn && num38 < 1)
            {
                Main.townNPCCanSpawn[684] = true;
            }
            bool flag9 = num2 > 0 && num3 > 0 && num4 > 0 && num5 > 0 && num6 > 0 && num8 > 0 && num9 > 0 && num10 > 0 && num11 > 0 && num12 > 0 && num14 > 0 && num15 > 0 && num16 > 0 && num17 > 0 && num18 > 0 && num19 > 0 && num20 > 0 && num21 > 0 && num22 > 0 && num23 > 0 && num24 > 0 && num25 > 0 && num26 > 0 && num27 > 0;
            if (Main.tenthAnniversaryWorld && !Main.remixWorld)
            {
                flag9 = true;
            }
            if (NPC.unlockedPrincessSpawn)
            {
                flag9 = true;
            }
            if (flag9 && num39 < 1)
            {
                Main.townNPCCanSpawn[663] = true;
            }
            int num42 = WorldGen.prioritizedTownNPCType;
            if (num42 == 0 && NPC.boughtCat && num28 < 1)
            {
                num42 = 637;
            }
            if (num42 == 0 && NPC.boughtDog && num29 < 1)
            {
                num42 = 638;
            }
            if (num42 == 0 && NPC.boughtBunny && num30 < 1)
            {
                num42 = 656;
            }
            if (num42 == 0 && NPC.unlockedSlimeBlueSpawn && num31 < 1)
            {
                num42 = 670;
            }
            if (num42 == 0 && flag8 && num32 < 1)
            {
                num42 = 678;
            }
            if (num42 == 0 && NPC.unlockedSlimeOldSpawn && num33 < 1)
            {
                num42 = 679;
            }
            if (num42 == 0 && NPC.unlockedSlimePurpleSpawn && num34 < 1)
            {
                num42 = 680;
            }
            if (num42 == 0 && NPC.unlockedSlimeRainbowSpawn && num35 < 1)
            {
                num42 = 681;
            }
            if (num42 == 0 && NPC.unlockedSlimeRedSpawn && num36 < 1)
            {
                num42 = 682;
            }
            if (num42 == 0 && NPC.unlockedSlimeYellowSpawn && num37 < 1)
            {
                num42 = 683;
            }
            if (num42 == 0 && NPC.unlockedSlimeCopperSpawn && num38 < 1)
            {
                num42 = 684;
            }
            if (num42 == 0 && num6 < 1)
            {
                num42 = 22;
            }
            if (num42 == 0 && flag && num2 < 1)
            {
                num42 = 17;
            }
            if (num42 == 0 && flag3 && num3 < 1 && num2 > 0)
            {
                num42 = 18;
            }
            if (num42 == 0 && flag2 && num5 < 1)
            {
                num42 = 19;
            }
            if (num42 == 0 && NPC.savedGoblin && num11 < 1)
            {
                num42 = 107;
            }
            if (num42 == 0 && NPC.savedTaxCollector && num24 < 1)
            {
                num42 = 441;
            }
            if (num42 == 0 && NPC.savedWizard && num10 < 1)
            {
                num42 = 108;
            }
            if (num42 == 0 && Main.hardMode && num14 < 1)
            {
                num42 = 160;
            }
            if (num42 == 0 && (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3) && num4 < 1)
            {
                num42 = 20;
            }
            if (num42 == 0 && flag5 && num2 > 0 && num8 < 1)
            {
                num42 = 38;
            }
            if (num42 == 0 && NPC.downedQueenBee && num20 < 1)
            {
                num42 = 228;
            }
            if (num42 == 0 && NPC.downedMechBossAny && num15 < 1)
            {
                num42 = 178;
            }
            if (num42 == 0 && NPC.savedMech && num12 < 1)
            {
                num42 = 124;
            }
            if (num42 == 0 && NPC.savedAngler && num23 < 1)
            {
                num42 = 369;
            }
            if (num42 == 0 && Main.hardMode && NPC.downedPlantBoss && num18 < 1)
            {
                num42 = 209;
            }
            if (num42 == 0 && NPC.downedPirates && num21 < 1)
            {
                num42 = 229;
            }
            if (num42 == 0 && NPC.downedBoss3 && num9 < 1)
            {
                num42 = 54;
            }
            if (num42 == 0 && NPC.savedStylist && num22 < 1)
            {
                num42 = 353;
            }
            if (num42 == 0 && num40 >= 4 && flag4 && num16 < 1)
            {
                num42 = 207;
            }
            if (num42 == 0 && num40 >= 8 && num19 < 1)
            {
                num42 = 227;
            }
            if (num42 == 0 && flag7 && num17 < 1)
            {
                num42 = 208;
            }
            if (num42 == 0 && NPC.downedFrost && num13 < 1 && Main.xMas)
            {
                num42 = 142;
            }
            if (num42 == 0 && NPC.savedBartender && num25 < 1)
            {
                num42 = 550;
            }
            if (num42 == 0 && NPC.savedGolfer && num26 < 1)
            {
                num42 = 588;
            }
            if (num42 == 0 && bestiaryProgressReport.CompletionPercent >= 0.1f && num27 < 1)
            {
                num42 = 633;
            }
            if (num42 == 0 && flag9 && num39 < 1)
            {
                num42 = 663;
            }
            WorldGen.prioritizedTownNPCType = num42;
        }
    }
    public static void Moondialing()
    {
        if (Main.moondialCooldown == 0)
        {
            Main.fastForwardTimeToDusk = true;
            Main.moondialCooldown = MainConfig.Instance.MoondialCooldown;
            NetMessage.SendData(7);
        }
    }
    public static void Sundialing()
    {
        if (Main.sundialCooldown == 0)
        {
            Main.fastForwardTimeToDawn = true;
            Main.sundialCooldown = MainConfig.Instance.SundialCooldown;
            NetMessage.SendData(7);
        }
    }
}
