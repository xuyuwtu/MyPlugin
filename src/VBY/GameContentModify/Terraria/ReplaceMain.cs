using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Events;
using Terraria.ID;

using VBY.GameContentModify.Config;

using static VBY.GameContentModify.GameContentModify;

namespace VBY.GameContentModify;

[ReplaceType(typeof(Main))]
public static class ReplaceMain
{
    private readonly static int[] TownNPCIDs;
    internal static Dictionary<int, int> TownNPCIDIndexMap;
    internal static int[] DisableSpawnTownNPCIndices = [];
    internal static int[] AlwaysSpawnTownNPCIDs = [];
    internal static int[] SpawnTownNPCs_NPCCount;
    private static Range[] princessCheckRanges;
    private static (int type, Func<int, bool>? condition)[] TownNPCSpawnFuncs;
    static ReplaceMain()
    {
        TownNPCIDs = [
            NPCID.Merchant,
            NPCID.Nurse,
            NPCID.Dryad,
            NPCID.ArmsDealer,
            NPCID.Guide,

            NPCID.OldMan,

            NPCID.Demolitionist,
            NPCID.Clothier,
            NPCID.Wizard,
            NPCID.GoblinTinkerer,
            NPCID.Mechanic,

            NPCID.SantaClaus,

            NPCID.Truffle,
            NPCID.Steampunker,
            NPCID.DyeTrader,
            NPCID.PartyGirl,
            NPCID.Cyborg,
            NPCID.Painter,
            NPCID.WitchDoctor,
            NPCID.Pirate,
            NPCID.Stylist,
            NPCID.Angler,
            NPCID.TaxCollector,
            NPCID.DD2Bartender,
            NPCID.Golfer,
            NPCID.BestiaryGirl,

            NPCID.TownCat,
            NPCID.TownDog,
            NPCID.TownBunny,
            NPCID.TownSlimeBlue,
            NPCID.TownSlimeGreen,
            NPCID.TownSlimeOld,
            NPCID.TownSlimePurple,
            NPCID.TownSlimeRainbow,
            NPCID.TownSlimeRed,
            NPCID.TownSlimeYellow,
            NPCID.TownSlimeCopper,
            NPCID.Princess
       ];
        Array.Sort(TownNPCIDs);
        SpawnTownNPCs_NPCCount = new int[TownNPCIDs.Length];
        TownNPCIDIndexMap = [];
        for(int i = 0; i < TownNPCIDs.Length; i++)
        {
            TownNPCIDIndexMap.Add(TownNPCIDs[i], i);
        }
        TownNPCSpawnFuncsInit();
        princessCheckRanges = new Range[3];
        princessCheckRanges[0] = new Range(0, TownNPCIDs.IndexOf(NPCID.OldMan));
        princessCheckRanges[1] = new Range(princessCheckRanges[0].Start.Value + 1, Array.IndexOf(TownNPCIDs, NPCID.SantaClaus, princessCheckRanges[0].Start.Value + 1));
        princessCheckRanges[2] = new Range(princessCheckRanges[1].Start.Value + 1, Array.IndexOf(TownNPCIDs, NPCID.TownCat, princessCheckRanges[1].Start.Value + 1));
    }
    internal static bool IsCanSpawnTownType(int type) => TownNPCIDs.Contains(type);
    static bool ExistsByNPCID(int type) => SpawnTownNPCs_NPCCount[TownNPCIDIndexMap[type]] > 0;
    static bool CanSpawnPrincess()
    {
        if ((Main.tenthAnniversaryWorld && !Main.remixWorld) || NPC.unlockedPrincessSpawn)
        {
            return true;
        }
        var canSpawn = true;
        for (int i = 0; i < princessCheckRanges.Length; i++)
        {
            if (SpawnTownNPCs_NPCCount[princessCheckRanges[i]].Contains(0))
            {
                canSpawn = false;
                break;
            }
        }
        return canSpawn;
    }
    [MemberNotNull(nameof(TownNPCSpawnFuncs))]
    static void TownNPCSpawnFuncsInit()
    {
        TownNPCSpawnFuncs = [
            (NPCID.Guide, null),
            (NPCID.Merchant, static _ => NPC.SpawnAllowed_Merchant()),
            (NPCID.Nurse, static _ => NPC.SpawnAllowed_Nurse() && ExistsByNPCID(NPCID.Merchant)),
            (NPCID.ArmsDealer, static _ => NPC.SpawnAllowed_ArmsDealer()),
            (NPCID.Dryad, static _ => NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3),
            (NPCID.Demolitionist, static _ => NPC.SpawnAllowed_Demolitionist() && ExistsByNPCID(NPCID.Merchant)),
            (NPCID.Stylist , static _ => NPC.savedStylist),
            (NPCID.Angler , static _ => NPC.savedAngler),
            (NPCID.Clothier , static _ => NPC.downedBoss3),
            (NPCID.GoblinTinkerer , static _ => NPC.savedGoblin),
            (NPCID.TaxCollector , static _ => NPC.savedTaxCollector),
            (NPCID.Wizard , static _ => NPC.savedWizard),
            (NPCID.Mechanic , static _ => NPC.savedMech),
            (NPCID.SantaClaus , static _ => NPC.downedFrost && Main.xMas),
            (NPCID.Steampunker , static _ => (Main.tenthAnniversaryWorld && !Main.remixWorld) || NPC.downedMechBossAny),
            (NPCID.DyeTrader , static totalTownNPC => NPC.SpawnAllowed_DyeTrader() && totalTownNPC >= 4),
            (NPCID.WitchDoctor , static _ => NPC.downedQueenBee),
            (NPCID.Pirate , static _ => NPC.downedPirates),
            (NPCID.Truffle , static _ => Main.hardMode),
            (NPCID.Cyborg , static _ => Main.hardMode && NPC.downedPlantBoss),
            (NPCID.Painter , static totalTownNPC => totalTownNPC >= 8),
            (NPCID.PartyGirl , static totalTownNPC => (Main.rand.Next(40) == 0 && totalTownNPC >= 14) || NPC.unlockedPartyGirlSpawn),
            (NPCID.DD2Bartender , static _ => NPC.savedBartender),
            (NPCID.Golfer , static _ => NPC.savedGolfer),
            (NPCID.BestiaryGirl , static _ => Main.GetBestiaryProgressReport().CompletionPercent >= 0.1f),
            (NPCID.TownCat , static _ => NPC.boughtCat),
            (NPCID.TownDog , static _ => NPC.boughtDog),
            (NPCID.TownBunny , static _ => NPC.boughtBunny),
            (NPCID.TownSlimeBlue , static _ => NPC.unlockedSlimeBlueSpawn),
            (NPCID.TownSlimeGreen , static _ => (BirthdayParty.GenuineParty || NPC.unlockedSlimeGreenSpawn)),
            (NPCID.TownSlimeOld , static _ => NPC.unlockedSlimeOldSpawn),
            (NPCID.TownSlimePurple , static _ => NPC.unlockedSlimePurpleSpawn),
            (NPCID.TownSlimeRainbow , static _ => NPC.unlockedSlimeRainbowSpawn),
            (NPCID.TownSlimeRed , static _ => NPC.unlockedSlimeRedSpawn),
            (NPCID.TownSlimeYellow , static _ => NPC.unlockedSlimeYellowSpawn),
            (NPCID.TownSlimeCopper , static _ => NPC.unlockedSlimeCopperSpawn),
            (NPCID.Princess, static _ => CanSpawnPrincess())
        ];
    }
    [DetourMethod]
    public static void UpdateTime(On.Terraria.Main.orig_UpdateTime orig)
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
            NetMessage.SendData(MessageID.WorldData);
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
                NPC.SpawnOnPlayer(Player.FindClosest(new Vector2(Main.maxTilesX / 2, (float)Main.worldSurface / 2f) * 16f, 0, 0), NPCID.MoonLordCore);
            }
        }

        Main.UpdateSlimeRainWarning();

        if (NPC.travelNPC && !SpawnInfo.TownNPCInfo.StaticTravelNPCNotLeavingAtNight)
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
                    if (Main.npc[i].active && Main.npc[i].townNPC && Main.npc[i].type != NPCID.OldMan && Main.npc[i].type != NPCID.SkeletonMerchant)
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
                            NPC.SpawnOnPlayer(j, NPCID.EyeofCthulhu);
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
                                            NPC.SpawnOnPlayer(l, NPCID.TheDestroyer);
                                        }
                                        if ((WorldGen.spawnHardBoss & 2) == 2)
                                        {
                                            NPC.SpawnOnPlayer(l, NPCID.Retinazer);
                                            NPC.SpawnOnPlayer(l, NPCID.Spazmatism);
                                        }
                                        if ((WorldGen.spawnHardBoss & 4) == 4)
                                        {
                                            NPC.SpawnOnPlayer(l, NPCID.SkeletronPrime);
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
                                        NPC.SpawnOnPlayer(l, NPCID.TheDestroyer);
                                    }
                                    else if (WorldGen.spawnHardBoss == 2)
                                    {
                                        NPC.SpawnOnPlayer(l, NPCID.Retinazer);
                                        NPC.SpawnOnPlayer(l, NPCID.Spazmatism);
                                    }
                                    else if (WorldGen.spawnHardBoss == 3)
                                    {
                                        NPC.SpawnOnPlayer(l, NPCID.SkeletronPrime);
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
                            NPC.SpawnOnPlayer(m, NPCID.Deerclops);
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
    [DetourMethod]
    public static void UpdateTime_StartDay(On.Terraria.Main.orig_UpdateTime_StartDay orig, ref bool stopEvents)
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
        AchievementsHelper.NotifyProgressionEvent(AchievementHelperID.Events.DayStart);
        if (stopEvents)
        {
            NetMessage.SendData(MessageID.WorldData);
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
            NetMessage.SendData(MessageID.WorldData);
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
            if (Main.invasionType == InvasionID.None && Main.hardMode && WorldGen.altarCount > 0 && ((NPC.downedPirates && Main.rand.NextIsZero(MainConfig.Instance.Invasion.DownedPiratesStartInvasionRandomNum)) || (!NPC.downedPirates && Main.rand.NextIsZero(MainConfig.Instance.Invasion.NoDownedPiratesStartInvasionRandomNum))))
            {
                Main.StartInvasion(InvasionID.PirateInvasion);
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
    [DetourMethod]
    public static void UpdateTime_StartNight(On.Terraria.Main.orig_UpdateTime_StartNight orig, ref bool stopEvents)
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
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        if (Main.player[i] is { active: true, statLifeMax: >= 200, statDefense: > 10 })
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
                        for (int j = 0; j < Main.maxNPCs; j++)
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
                    for (int k = 0; k < Main.maxNPCs; k++)
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
            if (MainConfig.Instance.BloodMoon.SpawnEyeCheck.IsTrueRet(!WorldGen.spawnEye) && Main.moonPhase != (int)Terraria.Enums.MoonPhase.Empty && Main.rand.NextIsZero(maxValue))
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
        NetMessage.SendData(MessageID.WorldData);
    }
    [DetourMethod]
    public static void UpdateTime_SpawnTownNPCs(On.Terraria.Main.orig_UpdateTime_SpawnTownNPCs orig)
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
        if (WorldGen.prioritizedTownNPCType != 0)
        {
            return;
        }
        Array.Fill(Main.townNPCCanSpawn, false);
        WorldGen.prioritizedTownNPCType = 0;
        var npcCount = SpawnTownNPCs_NPCCount;
        Array.Fill(npcCount, 0);
        int totalTownNPC = 0;
        for (int k = 0; k < Main.maxNPCs; k++)
        {
            if (!Main.npc[k].active || !Main.npc[k].townNPC)
            {
                continue;
            }
            if (Main.npc[k].type != NPCID.TravellingMerchant && Main.npc[k].type != NPCID.OldMan && Main.npc[k].type != NPCID.SkeletonMerchant && !Main.npc[k].homeless)
            {
                WorldGen.QuickFindHome(k);
            }
            var index = Array.BinarySearch(TownNPCIDs, Main.npc[k].type);
            if (index >= 0)
            {
                npcCount[index]++;
            }
            totalTownNPC++;
        }
        foreach (var index in DisableSpawnTownNPCIndices)
        {
            npcCount[index] = 1;
        }
        if (!NPC.downedBoss3 && npcCount[TownNPCIDIndexMap[NPCID.OldMan]] == 0)
        {
            int num41 = NPC.NewNPC(NPC.GetSpawnSourceForTownSpawn(), Main.dungeonX * 16 + 8, Main.dungeonY * 16, NPCID.OldMan);
            Main.npc[num41].homeless = false;
            Main.npc[num41].homeTileX = Main.dungeonX;
            Main.npc[num41].homeTileY = Main.dungeonY;
        }
        foreach (var item in TownNPCSpawnFuncs)
        {
            if(!ExistsByNPCID(item.type) && ((item.condition?.Invoke(totalTownNPC) ?? true) || AlwaysSpawnTownNPCIDs.Contains(item.type)))
            {
                Main.townNPCCanSpawn[item.type] = true;
                if (WorldGen.prioritizedTownNPCType == NPCID.None)
                {
                    WorldGen.prioritizedTownNPCType = item.type;
                }
            }
        }
    }
    [DetourMethod]
    public static void Moondialing(On.Terraria.Main.orig_Moondialing orig)
    {
        if (Main.moondialCooldown == 0)
        {
            Main.fastForwardTimeToDusk = true;
            Main.moondialCooldown = MainConfig.Instance.MoondialCooldown;
            NetMessage.SendData(MessageID.WorldData);
        }
    }
    [DetourMethod]
    public static void Sundialing(On.Terraria.Main.orig_Sundialing orig)
    {
        if (Main.sundialCooldown == 0)
        {
            Main.fastForwardTimeToDawn = true;
            Main.sundialCooldown = MainConfig.Instance.SundialCooldown;
            NetMessage.SendData(MessageID.WorldData);
        }
    }
}
