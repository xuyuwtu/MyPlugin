using System.ComponentModel;
using System.Reflection;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Net;
using Terraria.ObjectData;

using Newtonsoft.Json;

using VBY.Common.Config;
using VBY.GameContentModify.ID;

namespace VBY.GameContentModify.Config;

public sealed partial class MainConfigInfo : ISetDefaultsable
{
    [JsonProperty("生成")]
    public SpawnInfo Spawn = new();
    [JsonProperty("入侵")]
    public InvasionInfo Invasion = new();
    [JsonProperty("血月")]
    public BloodMoonInfo BloodMoon = new();
    [JsonProperty("世界")]
    public WorldInfo World = new();
    [JsonProperty("球体")]
    public OrbInfo Orb = new();
    [JsonProperty("晶塔")]
    public TeleportPylonsInfo TeleportPylons = new();
    [JsonProperty("网络消息")]
    public NetMessageInfo NetMessage = new();
    [JsonProperty("扩展")]
    public ExtensionInfo Extension = new();
    [JsonProperty("液体")]
    public LiquidInfo Liquid = new();
    [JsonProperty("机械")]
    public MechInfo Mech = new();

    [Description("自然生成日食几率: 1/{0}")]
    [JsonProperty("自然生成日食几率")]
    [DefaultValue(20)]
    public int EclipseRandomNum = 20;

    [Description("屏蔽射弹破坏物块ID列表")]
    [JsonProperty("屏蔽射弹破坏物块ID列表")]
    [DefaultValue((int)ProjectileID.BombSkeletronPrime)]
    public int[] DisableProjectile_ExplodeTilesIDs = new int[] { ProjectileID.BombSkeletronPrime };

    [MemberData("老旧摇摇箱在长者史莱姆解锁后依旧生成", false, PrivateField = true)]
    public static bool StaticBoundTownSlimeOldSpawnAtUnlock
    {
        get => _StaticBoundTownSlimeOldSpawnAtUnlock;
        set => Utils.HandleNamedDetour(ref _StaticBoundTownSlimeOldSpawnAtUnlock, value, DetourNames.NPC_SpawnNPC, DetourNames.NPC_TransformElderSlime);
    }
    [Description("老旧摇摇箱开启后生成的物品ID列表")]
    [JsonProperty("老旧摇摇箱开启后生成的物品ID列表")]
    public int[] BoundTownSlimeOldSpawnItemIDs = Array.Empty<int>();

    [MemberData("禁止蜂王和小蜜蜂伤害其他敌对NPC")]
    public static bool StaticDisableQueenBeeAndBeeHurtOtherNPC = false;

    [MemberData("禁止掉出世界时杀死城镇NPC的圣诞节检查")]
    public static bool StaticDisableKillNPCWhenNPCUnderWorldBottomXMasCheck = false;

    [MemberData("禁止敌怪NPC碰撞伤害城镇NPC")]
    public static bool StaticDisableNPCStrikeTownNPC = false;

    [MemberData("禁用微光分解")]
    public static bool StaticDisableShimmerDecrafte = false;

    [MemberData("每天可以摇树的数量: {0}", 500)]
    public static int StaticMaxTreeShakes
    {
        get => WorldGen.maxTreeShakes;
        set
        {
            if (value < 0)
            {
                return;
            }
            if (value != WorldGen.maxTreeShakes)
            {
                WorldGen.maxTreeShakes = value;
                if(WorldGen.numTreeShakes > value)
                {
                    WorldGen.numTreeShakes = value;
                }
                var source = WorldGen.treeShakeX;
                WorldGen.treeShakeX = new int[value];
                Array.Copy(source, WorldGen.treeShakeX, Math.Min(source.Length, WorldGen.treeShakeX.Length));
                source = WorldGen.treeShakeY;
                WorldGen.treeShakeY = new int[value];
                Array.Copy(source, WorldGen.treeShakeY, Math.Min(source.Length, WorldGen.treeShakeY.Length));
            }
        }
    }

    [MemberData("摇树不掉落炸弹射弹", false, PrivateField = true)]
    public static bool StaticDisableShakeTreeDropBombProj
    {
        get => _StaticDisableShakeTreeDropBombProj;
        set => Utils.HandleNamedDetour(ref _StaticDisableShakeTreeDropBombProj, value, DetourNames.WorldGen_ShakeTree);
    }
    [MemberData("罐子不掉落炸弹射弹", false, PrivateField = true)]
    public static bool StaticDisablePotDropBombProj
    {
        get => _StaticDisablePotDropBombProj;
        set => Utils.HandleNamedDetour(ref _StaticDisablePotDropBombProj, value, DetourNames.WorldGen_SpawnThingsFromPot);
    }

    [MemberData("禁止马桶生成射弹", false)]
    public static bool StaticDisableToiletSpawnProj;

    [Description("毒气陷阱叠加: {0}")]
    [JsonProperty("毒气陷阱叠加")]
    [DefaultValue(false)]
    public bool GasTrapsSuperpose = false;
    [Description("毒气陷阱生成射弹ID: {0}")]
    [JsonProperty("毒气陷阱生成射弹ID")]
    [DefaultValue(1007)]
    public int GasTrapsProjType = 1007;
    [Description("毒气陷阱射弹伤害: {0}")]
    [JsonProperty("毒气陷阱射弹伤害")]
    [DefaultValue(10)]
    public int GasTraosProjDamage = 10;

    [Description("日晷冷却天数: {0}")]
    [JsonProperty("日晷冷却天数")]
    [DefaultValue(8)]
    [CorrelationMethod(typeof(ReplaceMain), nameof(ReplaceMain.Moondialing))]
    public int MoondialCooldown = 8;
    [Description("月晷冷却天数: {0}")]
    [JsonProperty("月晷冷却天数")]
    [DefaultValue(8)]
    [CorrelationMethod(typeof(ReplaceMain), nameof(ReplaceMain.Sundialing))]
    public int SundialCooldown = 8;
    [MemberData("开始史莱姆雨的随机数的被除数: {0}")]
    public static double StaticDividendOfStartSlimeRainRandomNum = 450000.0;
    [MemberData("史莱姆雨时生成史莱姆王需要击杀的史莱姆数量: {0}")]
    public static int StaticSpawnSlimeKingWhenSlimeRainNeedKillCount = 150;
    [MemberData("史莱姆王被击败后史莱姆雨时生成史莱姆王需要击杀的史莱姆数量: {0}")]
    public static int StaticSpawnSlimeKingWhenSlimeRainAndDownedSlimeKingNeedKillCount = 75;
    [MemberData("史莱姆雨期间杀死史莱姆王不停止史莱姆雨")]
    [CorrelationMethod(typeof(ReplaceNPC), nameof(ReplaceNPC.DoDeathEvents))]
    public static bool StaticNoStopSlimeRainAfterKillSlimeKingWhenSlimeRaining = false;

    [MemberData("禁止NPC死亡时生成熔岩")]
    [CorrelationMethod(typeof(ReplaceNPC), nameof(ReplaceNPC.HitEffect))]
    public static bool StaticDisableSpawnLavaWhenNPCDead = false;
    private static void CheckProjectileKillHook() 
        => Utils.HandleNamedActionHook(_StaticDisableHostileSnowBallFromGeneratingSnowBlock || _StaticDisableSandBallOfAntlionFromGeneratingSandBlock || StaticSandBallOfAntlionCanDropItem, ActionHookNames.Projectile_Kill);
    internal static void OnProjectile_Kill(On.Terraria.Projectile.orig_Kill orig, Projectile self)
    {
        if (!self.active)
        {
            return;
        }
        if (!self.noDropItem && self.aiStyle == 10 && (self.type is ProjectileID.SnowBallHostile or ProjectileID.SandBallFalling))
        {
            //int tileType = 0;
            //int itemType = 2;
            int tileType, itemType;
            if (self.type == ProjectileID.SandBallFalling)
            {
                tileType = TileID.Sand;
                itemType = ItemID.SandBlock;
                if (self.ai[0] == 2f)
                {
                    if (_StaticDisableSandBallOfAntlionFromGeneratingSandBlock)
                    {
                        tileType = -1;
                    }
                    if (!StaticSandBallOfAntlionCanDropItem)
                    {
                        itemType = 0;
                    }
                }
            }
            else
            //if (self.type == ProjectileID.SnowBallHostile)
            {
                tileType = TileID.SnowBlock;
                itemType = 0;
            }
            if (self.type == ProjectileID.SnowBallHostile)
            {
                if (_StaticDisableHostileSnowBallFromGeneratingSnowBlock || ((double)(self.Center - Main.player[Player.FindClosest(self.position, self.width, self.height)].Center).Length() > Main.LogicCheckScreenWidth * 0.75))
                {
                    tileType = -1;
                    if (!StaticDisableHostileSnowBallDropItem)
                    {
                        itemType = ItemID.SnowBlock;
                    }
                }
            }
            int tileX = (int)(self.position.X + (self.width / 2)) / 16;
            int tileY = (int)(self.position.Y + (self.height / 2)) / 16;
            if (Main.tile[tileX, tileY].nactive() && Main.tile[tileX, tileY].halfBrick() && self.velocity.Y > 0f && Math.Abs(self.velocity.Y) > Math.Abs(self.velocity.X))
            {
                tileY--;
            }
            if (!Main.tile[tileX, tileY].active() && tileType >= 0)
            {
                bool placeSuccess = false;
                bool cannotPlace = false;
                if (tileY < Main.maxTilesY - 2)
                {
                    ITile tile2 = Main.tile[tileX, tileY + 1];
                    if (tile2 != null && tile2.active())
                    {
                        if (tile2.active() && tile2.type == 314)
                        {
                            cannotPlace = true;
                        }
                        if (tile2.active() && WorldGen.BlockBelowMakesSandFall(tileX, tileY))
                        {
                            cannotPlace = true;
                        }
                    }
                }
                if (!cannotPlace)
                {
                    placeSuccess = WorldGen.PlaceTile(tileX, tileY, tileType, mute: false, forced: true);
                }
                if (!cannotPlace && Main.tile[tileX, tileY].active() && Main.tile[tileX, tileY].type == tileType)
                {
                    if (Main.tile[tileX, tileY + 1].halfBrick() || Main.tile[tileX, tileY + 1].slope() != 0)
                    {
                        WorldGen.SlopeTile(tileX, tileY + 1);
                        Terraria.NetMessage.SendData(17, -1, -1, null, 14, tileX, tileY + 1);
                    }
                    Terraria.NetMessage.SendData(17, -1, -1, null, 1, tileX, tileY, tileType);
                }
                else if (!placeSuccess && itemType > 0)
                {
                    Item.NewItem(self.GetItemSource_DropAsItem(), (int)self.position.X, (int)self.position.Y, self.width, self.height, itemType);
                }
            }
            else if (itemType > 0)
            {
                Item.NewItem(self.GetItemSource_DropAsItem(), (int)self.position.X, (int)self.position.Y, self.width, self.height, itemType);
            }
            self.active = false;
        }
        else
        {
            orig(self);
        }
    }

    [MemberData("禁止敌对雪球生成雪块", false, PrivateField = true)]
    public static bool StaticDisableHostileSnowBallFromGeneratingSnowBlock
    {
        get => _StaticDisableHostileSnowBallFromGeneratingSnowBlock;
        set
        {
            _StaticDisableHostileSnowBallFromGeneratingSnowBlock = value;
            CheckProjectileKillHook();
        }
    }
    [MemberData("禁止敌对雪球掉落物品")]
    public static bool StaticDisableHostileSnowBallDropItem = false;
    [MemberData("禁止蚁狮的沙球生成沙块", false, PrivateField = true)]
    public static bool StaticDisableSandBallOfAntlionFromGeneratingSandBlock
    {
        get => _StaticDisableSandBallOfAntlionFromGeneratingSandBlock;
        set
        {
            _StaticDisableSandBallOfAntlionFromGeneratingSandBlock = value;
            CheckProjectileKillHook();
        }
    }
    [MemberData("蚁狮的沙球会掉落物品")]
    public static bool StaticSandBallOfAntlionCanDropItem = false;
    [MemberData("禁止墓碑射弹放置墓碑并掉落墓碑物品", false)]
    public static bool StaticDisableTombstoneProjPlaceTombstoneAndDropTombstoneItem
    {
        get => Utils.NamedActionHookIsRegistered(ActionHookNames.Projectile_AI);
        set => Utils.HandleNamedActionHook(value, ActionHookNames.Projectile_AI);
    }
    internal static void OnProjectile_AI(On.Terraria.Projectile.orig_AI orig, Projectile self)
    {
        if (self.aiStyle == 17)
        {
            if (self.velocity.Y == 0f)
            {
                self.velocity.X *= 0.98f;
            }
            self.rotation += self.velocity.X * 0.1f;
            self.velocity.Y += 0.2f;
            if (Main.getGoodWorld && Math.Abs(self.velocity.X) + Math.Abs(self.velocity.Y) < 1f)
            {
                self.damage = 0;
                self.knockBack = 0f;
            }
            if (self.owner != Main.myPlayer)
            {
                return;
            }
            int num173 = (int)((self.position.X + self.width / 2) / 16f);
            int num174 = (int)((self.position.Y + self.height - 4f) / 16f);
            if (Main.tile[num173, num174] == null)
            {
                return;
            }
            int style = 0;
            if (self.type >= 201 && self.type <= 205)
            {
                style = self.type - 200;
            }
            if (self.type >= 527 && self.type <= 531)
            {
                style = self.type - 527 + 6;
            }
            if (TileObject.CanPlace(num173, num174, 85, style, self.direction, out _, true))
            {
                Item.NewItem(self.GetItemSource_FromThis(), self.Center, Vector2.Zero, style switch
                {
                    TileStyleID.Tombstones.GraveMarker => ItemID.GraveMarker,
                    TileStyleID.Tombstones.CrossGraveMarker => ItemID.CrossGraveMarker,
                    TileStyleID.Tombstones.Headstone => ItemID.Headstone,
                    TileStyleID.Tombstones.Gravestone => ItemID.Gravestone,
                    TileStyleID.Tombstones.Obelisk => ItemID.Obelisk,
                    TileStyleID.Tombstones.RichGravestone1 => ItemID.RichGravestone1,
                    TileStyleID.Tombstones.RichGravestone2 => ItemID.RichGravestone2,
                    TileStyleID.Tombstones.RichGravestone3 => ItemID.RichGravestone3,
                    TileStyleID.Tombstones.RichGravestone4 => ItemID.RichGravestone4,
                    TileStyleID.Tombstones.RichGravestone5 => ItemID.RichGravestone5,
                    _ => ItemID.Tombstone,
                });
                self.Kill();
            }
        }
        else
        {
            orig(self);
        }
    }
    [MemberData("药草生长改为随机", false, PrivateField = true)]
    public static bool StaticHerbGrowIsRandom
    {
        get => _StaticHerbGrowIsRandom;
        set => Utils.HandleNamedDetour(ref _StaticHerbGrowIsRandom, value, DetourNames.WorldGen_GrowAlch, DetourNames.WorldGen_IsHarvestableHerbWithSeed);
    }
    [MemberData("药草随机生长几率: 1/{0}")]
    public static int StaticHerbGrowRandomNumWhenIsRandom = 50;
    [MemberData("下雨基础随机数: 1/{0}")]
    public static int StaticStartRainBaseRandomNum = 86000;
    [JsonProperty("附魔剑冢掉落物品信息")]
    [Description("附魔剑冢掉落物品信息")]
    public DropItemInfo[] EnchantedSwordInStoneDropItemInfo = new DropItemInfo()
    {
        RandomValue = 50,
        Items = new RandomItemInfo(ItemID.Terragrim).MakeArray(),
        Else = new DropItemInfo()
        {
            Items = new RandomItemInfo(ItemID.EnchantedSword).MakeArray()
        }.MakeArray(),
    }.MakeArray();
    [MemberData("城镇NPC死亡时掉落墓碑")]
    public static bool StaticTownNPCDropTombstoneWhenDead = false;
    [MemberData("城镇NPC不会淹死")]
    public static bool StaticTownNPCDrowningImmunity = false;
    [MemberData("向导巫毒娃娃生成血肉墙时可以没有向导", false, PrivateField = true)]
    public static bool StaticGuideVoodooDollSpawnWOFCanWithoutGuide
    {
        get => _StaticGuideVoodooDollSpawnWOFCanWithoutGuide;
        set => Utils.HandleNamedDetour(ref _StaticGuideVoodooDollSpawnWOFCanWithoutGuide, value, DetourNames.Item_CheckLavaDeath);
    }
    [MemberData("NPC不掉落旗帜", false, PrivateField = true)]
    public static bool StaticNPCNotDropBanner
    {
        get => _StaticNPCNotDropBanner;
        set => Utils.HandleNamedDetour(ref _StaticNPCNotDropBanner, value, DetourNames.NPC_CountKillForBannersAndDropThem);
    }
    [MemberData("破坏蜂巢不生成蜜蜂")]
    public static bool StaticNotSpawnBeeWhenKillHive = false;

    [MemberData("草药盆不生长杂草", false, PrivateField = true)]
    public static bool StaticPlanterBoxNotGrowingWeeds
    {
        get => _StaticPlanterBoxNotGrowingWeeds;
        set => Utils.HandleNamedDetour(ref _StaticPlanterBoxNotGrowingWeeds, value, DetourNames.WorldGen_UpdateWorld_OvergroundTile);
    }
    public static void Reset()
    {
        ExtensionInfo.NotSendNetPacketIDs.Clear();
        GameContentModify.MainConfig.Instance.Liquid.Restore();
        var instance = GameContentModify.MainConfig.GetDefaultFunc();
        Reset(instance);
        GameContentModify.MainConfig.Instance = instance;
    }
    private static void Reset(object instance)
    {
        var members = instance.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public);
        if (instance is ISetDefaultsable defaultsable)
        {
            defaultsable.SetDefaults();
        }
        foreach (var member in members)
        {
            //if (member.MemberType == MemberTypes.Property)
            //{
            //    var property = (PropertyInfo)member;
            //    if (property.GetCustomAttribute<DefaultValueAttribute>() is DefaultValueAttribute defaultValueAttr)
            //    {
            //        property.SetValue(instance, defaultValueAttr.Value);
            //    }
            //}
            //else if (member.MemberType == MemberTypes.Field)
            if (member.MemberType == MemberTypes.Field)
            {
                var field = (FieldInfo)member;
                if (field is { FieldType.IsClass: true, FieldType.IsArray: false })
                {
                    Reset(field.GetValue(instance)!);
                }
                //else if(field.GetCustomAttribute<DefaultValueAttribute>() is DefaultValueAttribute defaultValueAttr)
                //{
                //    if (field.FieldType.IsArray)
                //    {
                //        var array = (Array)field.GetValue(instance)!;
                //        if(array.Length == 1)
                //        {
                //            array.SetValue(defaultValueAttr.Value, 0);
                //        }
                //        else
                //        {
                //            field.SetValue(instance, Array.CreateInstance(field.FieldType.GetElementType()!, 1));
                //        }
                //    }
                //    else
                //    {
                //        field.SetValue(instance, defaultValueAttr.Value);
                //    }
                //}
            }
        }
    }
}
[Description("生成")]
public sealed partial class SpawnInfo
{
    [Description("城镇NPC")]
    public sealed partial class TownNPCInfo
    {
        [MemberData("禁止自然生成")]
        public static bool StaticDisableSpawn = false;
        [MemberData("晚上生成", false)]
        public static bool StaticSpawnAtNight
        {
            get => Utils.NamedActionHookIsRegistered(ActionHookNames.Main_UpdateTime);
            set => Utils.HandleNamedActionHook(value, ActionHookNames.Main_UpdateTime);
        }
        internal static void OnMain_UpdateTime(On.Terraria.Main.orig_UpdateTime orig)
        {
            orig();
            if (!Main.dayTime)
            {
                Main.UpdateTime_SpawnTownNPCs();
            }
        }
        [MemberData("有入侵时生成")]
        public static bool StaticSpawnAtInvasion = false;
        [MemberData("日食时生成")]
        public static bool StaticSpawnAtEclipse = false;
        [MemberData("时间速率为0时仍然生成", false, PrivateField = true)]
        public static bool StaticSpawnStillWhenTimeRateIsZero
        {
            get => _StaticSpawnStillWhenTimeRateIsZero;
            set => Utils.HandleNamedDetour(ref _StaticSpawnStillWhenTimeRateIsZero, value, DetourNames.Main_UpdateTime_SpawnTownNPCs);
        }
        [MemberData("时间速率为0时的生成速率: {0}")]
        public static int StaticSpawnTimeRateWhenTimeRateIsZero = 1;

        [MemberData("旅商晚上不离开")]
        public static bool StaticTravelNPCNotLeavingAtNight = false;

        [MemberData("房屋忽略腐化检测", false, PrivateField = true)]
        public static bool StaticHouseIgnoreEvil
        {
            get => _StaticHouseIgnoreEvil;
            set => Utils.HandleNamedDetour(ref _StaticHouseIgnoreEvil, value, DetourNames.WorldGen_ScoreRoom);
        }
        [MemberData("忽略松露人环境检测", false)]
        public static bool StaticIgnoreTruffEnvCheck
        {
            get => Utils.NamedActionHookIsRegistered(ActionHookNames.WorldGen_CheckSpecialTownNPCSpawningConditions);
            set => Utils.HandleNamedActionHook(value, ActionHookNames.WorldGen_CheckSpecialTownNPCSpawningConditions);
        }
        internal static bool OnCheckSpecialTownNPCSpawningConditions(On.Terraria.WorldGen.orig_CheckSpecialTownNPCSpawningConditions orig, int type)
        {
            return true;
        }
    }
    [JsonProperty("城镇NPC")]
    public TownNPCInfo TownNPC = new();

    [Description("自然生成陨石的几率: 1/{0}")]
    [DefaultValue(50)]
    [JsonProperty("自然生成陨石的几率")]
    public int SpawnMeteorRandomNum = 50;

    [Description("克苏鲁之眼")]
    public sealed partial class EyeOfCthulhuInfo
    {
        [MemberData("自然生成击败检测")]
        public static bool StaticDownedCheck = true;

        [MemberData("自然生成血量和防御检测")]
        public static bool StaticLifeAndDefenseCheck = true;

        [MemberData("自然生成未击败时生成的几率: 1/{0}")]
        public static int StaticRandomNum = 3;

        [MemberData("自然生成已击败时生成的几率: 1/{0}")]
        public static int StaticDownedRandomNum = 3;

        [MemberData("自然生成城镇NPC数量检测检测")]
        public static bool StaticTownNPCCountCheck = true;
    }
    [JsonProperty("克苏鲁之眼")]
    public EyeOfCthulhuInfo EyeOfCthulhu = new();

    [Description("机械Boss")]
    public sealed partial class MechBossInfo
    {
        [MemberData("自然生成的几率: 1/{0}")]
        public static int StaticSpawnRandomNum = 10;
        [MemberData("自然生成击败检测")]
        public static bool StaticSpawnDownedCheck = true;
        [MemberData("自然生成克眼自然生成检测")]
        public static bool StaticSpawnEyeCheck = true;
        [MemberData("自然生成时三王为或者的关系")]
        public static bool StaticSpawnIsOr = false;
        [MemberData("自然生成时世界上是否有Boss检测")]
        public static bool StaticSpawnHaveBossInWorldCheck = true;
    }
    [JsonProperty("机械Boss")]
    public MechBossInfo MechBoss = new();

    [MemberData("召唤月亮领主等待时间")]
    public static int StaticMoonLordCountdownOfSummon = 720;
    [MemberData("柱子死亡后月亮领主等待时间")]
    public static int StaticMoonLordCountdownOfTowerKilled = 3600;

    [MemberData("禁用世花花苞生成世花已存在检测", false, PrivateField = true)]
    public static bool StaticDisablePlanteraBulbSpawnPlanteraExistsCheck
    {
        get => _StaticDisablePlanteraBulbSpawnPlanteraExistsCheck;
        set => Utils.HandleNamedDetour(ref _StaticDisablePlanteraBulbSpawnPlanteraExistsCheck, value, DetourNames.WorldGen_CheckJunglePlant);
    }
}
[Description("入侵")]
public sealed class InvasionInfo
{
    [Description("未击败时自然生成哥布林入侵的几率: 1/{0}")]
    [DefaultValue(3)]
    [JsonProperty("未击败时自然生成哥布林入侵的几率")]
    public int NoDownedGoblinsStartInvasionRandomNum = 3;

    [Description("已击败时自然生成哥布林入侵的几率: 1/{0}")]
    [DefaultValue(30)]
    [JsonProperty("已击败时自然生成哥布林入侵的几率")]
    public int DownedGoblinsStartInvasionRandomNum = 30;

    [Description("困难模式且已击败时自然生成哥布林入侵的几率: 1/{0}")]
    [DefaultValue(60)]
    [JsonProperty("困难模式且已击败时自然生成哥布林入侵的几率")]
    public int HardModeDownedGoblinsStartInvasionRandomNum = 60;

    [Description("未击败时自然生成海盗入侵的几率: 1/{0}")]
    [DefaultValue(30)]
    [JsonProperty("未击败时自然生成海盗入侵的几率")]
    public int NoDownedPiratesStartInvasionRandomNum = 30;

    [Description("已击败时自然生成海盗入侵的几率: 1/{0}")]
    [DefaultValue(60)]
    [JsonProperty("已击败时自然生成海盗入侵的几率")]
    public int DownedPiratesStartInvasionRandomNum = 60;
}
[Description("血月")]
public sealed class BloodMoonInfo
{
    [Description("自然生成几率: 1/{0}")]
    [DefaultValue(9)]
    [JsonProperty("自然生成几率")]
    public int RandomNum = 9;
    [Description("十周年世界自然生成几率: 1/{0}")]
    [DefaultValue(6)]
    [JsonProperty("十周年世界自然生成几率")]
    public int TenthAnniversaryWorldRandomNum = 6;
    [Description("自然生成玩家血量检测: {0}")]
    [DefaultValue(true)]
    [JsonProperty("自然生成玩家血量检测")]
    public bool LifeCheck = true;
    [Description("自然生成克眼检测: {0}")]
    [DefaultValue(true)]
    [JsonProperty("自然生成克眼检测")]
    public bool SpawnEyeCheck = true;
    [Description("清除日晷和月晷冷却: {0}")]
    [DefaultValue(true)]
    [JsonProperty("清除日晷和月晷冷却")]
    public bool ClearSundialAndMoondialCooldown = true;
}
[Description("世界")]
public sealed partial class WorldInfo
{
    [MemberData("非困难模式时启用困难模式更新")]
    [CorrelationMethod(typeof(ReplaceWorldGen), nameof(ReplaceWorldGen.hardUpdateWorld))]
    public static bool StaticEnableHardModeUpdateWhenNotHardMode = false;

    [MemberData("感染传播距离: {0}")]
    public static int StaticInfectionTransmissionDistance = 3;

    [MemberData("不生成落星")]
    public static bool StaticNoSpawnFallenStar = false;
    [MemberData("白天也生成落星")]
    public static bool StaticSpawnFallenStarAtDay = false;

    [JsonProperty("生长生命果需要的进度ID")]
    [Description("生长生命果需要的进度ID")]
    public int[] GrowLifeFruitRequireProgressIDs = new int[] { ProgressQueryID.HardMode, ProgressQueryID.MechBossAny };
    public static byte[] StaticGrowLifeFruitRequireProgressIDs = new byte[] { ProgressQueryID.HardMode, ProgressQueryID.MechBossAny };

    [MemberData("时间速率为0时仍然更新")]
    public static bool StaticUpdateStillWhenTimeRateIsZero = false;
    [MemberData("时间速率为0时的更新速率: {0}")]
    public static int StaticUpdateTimeRateWhenTimeRateIsZero = 1;

    [MemberData("禁止液体更新")]
    public static bool StaticDisableLiquidUpdate = false;

    [MemberData("宝石树可以在地表生长")]
    public static bool StaticGemTreeCanGrowOverground = false;
}
[Description("球体")]
public sealed class OrbInfo
{
    [Description("生成Boss需要的数量: {0}")]
    [DefaultValue(3)]
    [JsonProperty("生成Boss需要的数量")]
    public int SpanwNPCSmashedCount = 3;
    [Description("心脏掉落物品")]
    [JsonProperty("心脏掉落物品")]
    public ItemInfo[][] CrimsonShadowOrbDropItems = new[]
    {
        new ItemInfo[]
        {
            new(800), new(97, 100)
        },
        new ItemInfo[]
        {
            new(1256)
        },
        new ItemInfo[]
        {
            new(802)
        },
        new ItemInfo[]
        {
            new(3062)
        },
        new ItemInfo[]
        {
            new(1290)
        }
    }; 
    [Description("暗影珠掉落物品")]
    [JsonProperty("暗影珠掉落物品")]
    public ItemInfo[][] CorruptionShadowOrbDropItems = new[]
    {
        new ItemInfo[]
        {
            new(96), new(97, 100)
        },
        new ItemInfo[]
        {
            new(64)
        },
        new ItemInfo[]
        {
            new(162)
        },
        new ItemInfo[]
        {
            new(115)
        },
        new ItemInfo[]
        {
            new(111)
        }
    };
}
[Description("晶塔")]
public sealed class TeleportPylonsInfo
{
    [Description("危险检测: {0}")]
    [DefaultValue(true)]
    [JsonProperty("危险检测")]
    public bool DangerCheck = true;
    [Description("花前神庙墙检测: {0}")]
    [DefaultValue(true)]
    [JsonProperty("花前神庙墙检测")]
    public bool PreDownedPlantBossTempleCheck = true;
    [Description("环境要求检测: {0}")]
    [DefaultValue(true)]
    [JsonProperty("环境要求检测")]
    public bool EnvironmentalRequirementsCheck = true;
    [Description("需要的城镇NPC数量: {0}")]
    [DefaultValue(2)]
    [JsonProperty("需要的城镇NPC数量")]
    public int NeedNPCCount = 2;
    [Description("使用时检测距离X: {0}")]
    [DefaultValue(60)]
    [JsonProperty("使用时检测距离X")]
    public int ReachX = 60;
    [Description("使用时检测距离Y: {0}")]
    [DefaultValue(60)]
    [JsonProperty("使用时检测距离Y")]
    public int ReachY = 60;
}
[Description("网络消息")]
public sealed partial class NetMessageInfo
{
    [MemberData("同步所有NPC", false, PrivateField = true)]
    public static bool StaticSyncAllNPC
    {
        get => _StaticSyncAllNPC;
        set
        {
            _StaticSyncAllNPC = value;
            //CheckMessageBufferGetDataHook();
        }
    }
    [MemberData("同步所有物品", false, PrivateField = true)]
    public static bool StaticSyncAllItem
    {
        get => _StaticSyncAllItem;
        set
        {
            _StaticSyncAllItem = value;
            //CheckMessageBufferGetDataHook();
        }
    }
    [MemberData("同步所有射弹", false, PrivateField = true)]
    public static bool StaticSyncAllProjectile
    {
        get => _StaticSyncAllProjectile;
        set
        {
            _StaticSyncAllProjectile = value;
            //CheckMessageBufferGetDataHook();
        }
    }
    //private static bool _HookedMessageBufferGetData = false;
    //private static void CheckMessageBufferGetDataHook() 
    //    => Utils.HandleNamedDetour(ref _HookedMessageBufferGetData, _StaticSyncAllNPC || _StaticSyncAllItem || _StaticSyncAllProjectile, DetourNames.MessageBuffer_GetData);
}
[Description("扩展")]
public sealed partial class ExtensionInfo
{
    [MemberData("渔夫在海边水里死亡时会生成猪鲨", false)]
    public static bool StaticSpawnDukeFishronWhenAnglerDeadAtSeaZoneInWater
    {
        get => Utils.NamedActionHookIsRegistered(ActionHookNames.NPC_DoDeathEvents);
        set => Utils.HandleNamedActionHook(value, ActionHookNames.NPC_DoDeathEvents);
    }
    internal static void OnNPC_DoDeathEvents(On.Terraria.NPC.orig_DoDeathEvents orig, NPC self, Player closestPlayer)
    {
        orig(self, closestPlayer);
        if (!Main.hardMode || self.type != NPCID.Angler)
        {
            return;
        }
        var point = self.position.ToTileCoordinates();
        if (WorldGen.oceanDepths(point.X, point.Y) && Collision.WetCollision(self.position, self.width, self.height))
        {
            NPC.SpawnBoss((int)self.position.X, (int)self.position.Y, NPCID.DukeFishron, closestPlayer.whoAmI);
        }
    }

    public static HashSet<ushort> NotSendNetPacketIDs = new();
    [Description("不发送的NetPacket类名称")]
    [JsonProperty("不发送的NetPacket类名称")]
    public string[] NotSendNetPacketNames = Array.Empty<string>();
    public void LoadNotSendPacketID()
    {
        var getidMethod = NetManager.Instance.GetType().GetMethod(nameof(NetManager.GetId));
        NotSendNetPacketIDs.Clear();
        foreach (var item in NotSendNetPacketNames)
        {
            var type = typeof(Item).Assembly.GetType("Terraria.GameContent.NetModules." + item);
            if (type is null)
            {
                Console.WriteLine($"Terraria.GameContent.NetModules.{item} is get null");
            }
            else
            {
                NotSendNetPacketIDs.Add((ushort)getidMethod!.MakeGenericMethod(new Type[] { type }).Invoke(NetManager.Instance, null)!);
            }
        }
    }

    [Description("在每天开始的时候生成旅商: {0}")]
    [DefaultValue(false)]
    [JsonProperty("在每天开始的时候生成旅商")]
    public bool SpawnTravelNPCWhenStartDay = false;

    [Description("在每天开始的时候生成旅商的几率: 1/{0}")]
    [DefaultValue(10)]
    [JsonProperty("在每天开始的时候生成旅商的几率")]
    public int SpawnTravelNPCWhenStartDayRandomNum = 10;

    public static HashSet<int> StaticIgnoreLavaNPCs = new();
    public static HashSet<int> StaticIgnoreProjectileNPCs = new();
    [Description("免疫熔岩的NPC")]
    [JsonProperty("免疫熔岩的NPC")]
    public object[] IgnoreLavaNPCs = Array.Empty<object>();
    [Description("免疫射弹的NPC")]
    [JsonProperty("免疫射弹的NPC")]
    public object[] IgnoreProjectileNPCs = Array.Empty<object>();
    public void LoadIgnoreNPCs()
    {
        StaticIgnoreLavaNPCs = ConfigUtlis.GetIntsAsHashSet(IgnoreLavaNPCs);
        StaticIgnoreProjectileNPCs = ConfigUtlis.GetIntsAsHashSet(IgnoreProjectileNPCs);
        Utils.HandleNamedActionHook(StaticIgnoreLavaNPCs.Any() || StaticIgnoreProjectileNPCs.Any(), ActionHookNames.NPC_NewNPC);
    }
    public static void SetIgnore()
    {
        foreach(var type in StaticIgnoreLavaNPCs)
        {
            for(int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].netID == type)
                {
                    Main.npc[i].lavaImmune = true;
                }
            }
        }
        foreach (var type in StaticIgnoreProjectileNPCs)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].netID == type)
                {
                    Main.npc[i].dontTakeDamageFromHostiles = true;
                }
            }
        }
    }
    internal static int OnNPC_NewNPC(On.Terraria.NPC.orig_NewNPC orig, IEntitySource source, int X, int Y, int Type, int Start, float ai0, float ai1, float ai2, float ai3, int Target)
    {
        var index = orig(source, X, Y, Type, Start, ai0, ai1, ai2, ai3, Target);
        var npc = Main.npc[index];
        if (StaticIgnoreLavaNPCs.Contains(npc.netID))
        {
            npc.lavaImmune = true;
            Utils.ArgumentWriteLine(npc.type);
            Utils.ArgumentWriteLine(npc.lavaImmune);
        }
        if (StaticIgnoreProjectileNPCs.Contains(npc.netID))
        {
            npc.dontTakeDamageFromHostiles = true;
            Utils.ArgumentWriteLine(npc.type);
            Utils.ArgumentWriteLine(npc.dontTakeDamageFromHostiles);
        }
        return index;
    }

    [MemberData("旅商每天刷新")]
    public static bool StaticTravelNPCRefreshOnStartDay = false;
}
[Description("液体")]
public sealed partial class LiquidInfo : IRestoreable
{
    [MemberData("禁止熔岩破坏草方块", false, PrivateField = true)]
    public static bool StaticDisableLavaDestoryGrassTile
    {
        get => _StaticDisableLavaDestoryGrassTile;
        set => Utils.HandleNamedDetour(ref _StaticDisableLavaDestoryGrassTile, value, DetourNames.Liquid_DelWater);
    }
    public sealed class LiquidDeathInfo
    {
        public bool? LavaDeath;
        public bool? WaterDeath;
    }
    [MemberData("启用液体破坏修改", false, PrivateField = true)]
    public static bool StaticEnableLiquidDeathModify
    {
        get => _StaticEnableLiquidDeathModify;
        set => Utils.HandleNamedDetour(ref _StaticEnableLiquidDeathModify, value, DetourNames.ObjectData_TileObjectData_GetTileData);
    }
    [JsonProperty("破坏信息")]
    public Dictionary<int, Dictionary<int, LiquidDeathInfo>> DestoryInfo = new();
    private static readonly Dictionary<int, Dictionary<int, LiquidDeathInfo>> SettedInfo = new();
    public void SetValue()
    {
        if (!_StaticEnableLiquidDeathModify)
        {
            return;
        }
        try
        {
            TileObjectData.readOnlyData = false;
            foreach (var (type, sub) in DestoryInfo)
            {
                if (sub is null)
                {
                    continue;
                }
                if (type < 0 || type >= TileObjectData._data.Count)
                {
                    continue;
                }
                SettedInfo.Add(type, new());
                foreach (var (style, info) in sub)
                {
                    if (style < 0)
                    {
                        continue;
                    }
                    var data = TileObjectData._data[type];
                    if (data is null)
                    {
                        bool origValue;
                        if (info.LavaDeath.HasValue)
                        {
                            origValue = Main.tileLavaDeath[type];
                            Main.tileLavaDeath[type] = info.LavaDeath.Value;
                            info.LavaDeath = origValue;
                            if (GameContentModify.Debug)
                            {
                                Console.WriteLine($"Set Main.tileLavaDeath[{type}] {origValue} => {Main.tileLavaDeath[type]}");
                            }
                        }
                        if (info.WaterDeath.HasValue)
                        {
                            origValue = Main.tileWaterDeath[type];
                            Main.tileWaterDeath[type] = info.WaterDeath.Value;
                            info.WaterDeath = origValue;
                            if (GameContentModify.Debug)
                            {
                                Console.WriteLine($"Set Main.tileWaterDeath[{type}] {origValue} => {Main.tileWaterDeath[type]}");
                            }
                        }
                    }
                    else
                    {
                        if (!data._hasOwnSubTiles)
                        {
                            data.SubTiles = new();
                        }
                        var subTiles = data.SubTiles;
                        if (subTiles.Count <= style)
                        {
                            for (int i = subTiles.Count; i <= style; i++)
                            {
                                subTiles.Add(null);
                            }
                        }
                        var subData = subTiles[style];
                        if(subData is null)
                        {
                            subData = new(data);
                            subTiles[style] = subData;
                        }

                        bool origValue;
                        if (info.LavaDeath.HasValue)
                        {
                            origValue = subData.LavaDeath;
                            subData.LavaDeath = info.LavaDeath.Value;
                            info.LavaDeath = origValue;
                            if (GameContentModify.Debug)
                            {
                                Console.WriteLine($"Set TileObjectData[{type}][{style}].LavaDeath {origValue} => {subData.LavaDeath}");
                            }
                        }
                        if (info.WaterDeath.HasValue)
                        {
                            origValue = subData.WaterDeath;
                            subData.WaterDeath = info.WaterDeath.Value;
                            info.WaterDeath = origValue;
                            if (GameContentModify.Debug)
                            {
                                Console.WriteLine($"Set TileObjectData[{type}][{style}].WaterDeath {origValue} => {subData.WaterDeath}");
                            }
                        }
                    }
                    SettedInfo[type].Add(style, info);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            TileObjectData.readOnlyData = true;
        }
    }
    public void Restore()
    {
        if(SettedInfo.Count == 0)
        {
            return;
        }
        try
        {
            TileObjectData.readOnlyData = false;
            foreach (var (type, sub) in SettedInfo)
            {
                if (sub is null)
                {
                    continue;
                }
                if (type < 0 || type >= TileObjectData._data.Count)
                {
                    continue;
                }
                foreach (var (style, info) in sub)
                {
                    if (style < 0)
                    {
                        continue;
                    }
                    var data = TileObjectData._data[type];
                    if (data is null)
                    {
                        bool value;
                        if (info.LavaDeath.HasValue)
                        {
                            value = Main.tileLavaDeath[type];
                            Main.tileLavaDeath[type] = info.LavaDeath.Value;
                            info.LavaDeath = value;
                            if (GameContentModify.Debug)
                            {
                                Console.WriteLine($"Restore Main.tileLavaDeath[{type}] {value} => {Main.tileLavaDeath[type]}");
                            }
                        }
                        if (info.WaterDeath.HasValue)
                        {
                            value = Main.tileWaterDeath[type];
                            Main.tileWaterDeath[type] = info.WaterDeath.Value;
                            info.WaterDeath = value;
                            if (GameContentModify.Debug)
                            {
                                Console.WriteLine($"Restore Main.tileWaterDeath[{type}] {value} => {Main.tileWaterDeath[type]}");
                            }
                        }
                    }
                    else
                    {
                        if (!data._hasOwnSubTiles)
                        {
                            continue;
                        }
                        var subTiles = data.SubTiles;
                        if (subTiles is null)
                        {
                            continue;
                        }
                        if (subTiles.Count <= style)
                        {
                            continue;
                        }
                        var subData = subTiles[style];
                        if(subData is null)
                        {
                            continue;
                        }

                        bool value;
                        if (info.LavaDeath.HasValue)
                        {
                            value = subData.LavaDeath;
                            subData.LavaDeath = info.LavaDeath.Value;
                            info.LavaDeath = value;
                            if (GameContentModify.Debug)
                            {
                                Console.WriteLine($"Restore TileObjectData[{type}][{style}].LavaDeath {value} => {subData.LavaDeath}");
                            }
                        }
                        if (info.WaterDeath.HasValue)
                        {
                            value = subData.WaterDeath;
                            subData.WaterDeath = info.WaterDeath.Value;
                            info.WaterDeath = value;
                            if (GameContentModify.Debug)
                            {
                                Console.WriteLine($"Restore TileObjectData[{type}][{style}].WaterDeath {value} => {subData.WaterDeath}");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            SettedInfo.Clear();
            TileObjectData.readOnlyData = true;
        }
    }
}
[Description("机械")]
public sealed partial class MechInfo
{
    [MemberData("300范围内的物品生成数量限制: {0}")]
    public static int StaticItemSpawnLimitOfRange300 = 3;
    [MemberData("800范围内的物品生成数量限制: {0}")]
    public static int StaticItemSpawnLimitOfRange800 = 6;
    [MemberData("世界范围内的物品生成数量限制: {0}")]
    public static int StaticItemSpawnLimitOfWorld = 10;
    [MemberData("物品生成数量限制使用物品数量")]
    public static bool StaticItemSpawnLimitUseStack = false;
    [MemberData("生成物品的冷却时间: {0}")]
    public static int StaticItemSpawnCoolingTime = 600;
    [MemberData("200范围内的NPC生成数量限制: {0}")]
    public static int StaticNPCSpawnLimitOfRange200 = 3;
    [MemberData("600范围内的NPC生成数量限制: {0}")]
    public static int StaticNPCSpawnLimitOfRange600 = 6;
    [MemberData("世界范围内的NPC生成数量限制: {0}")]
    public static int StaticNPCSpawnLimitOfWorld = 10;
    [MemberData("生成NPC的冷却时间: {0}")]
    public static int StaticNPCSpawnCoolingTime = 30;
    [MemberData("巨石雕像冷却时间: {0}")]
    public static int StaticBoulderStatueCoolingTime = 900;
    [MemberData("飞镖机关冷却时间: {0}")]
    public static int StaticDartTrapCoolingTime = 200;
    [MemberData("尖球机关冷却时间: {0}")]
    public static int StaticSpikyBallTrapCoolingTime = 300;
    [MemberData("长矛机关冷却时间: {0}")]
    public static int StaticSpearTrapCoolingTime = 90;
    [MemberData("热喷泉冷却时间: {0}")]
    public static int StaticGeyserTrapCoolingTime = 200;
}
public class ItemInfo
{
    public int type;
    public int stack = 1;
    public int prefix = -1;
    public ItemInfo() { }
    public ItemInfo(int type, int stack = 1, int prefix = -1)
    {
        this.type = type;
        this.stack = stack;
        this.prefix = prefix;
    }
    public virtual int NewItem(IEntitySource source, int tileX, int tileY) => Item.NewItem(source, tileX * 16, tileY * 16, 32, 32, type, stack, false, prefix);
}