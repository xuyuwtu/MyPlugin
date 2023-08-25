using IL.Terraria.GameContent.UI.States;
using System.ComponentModel;
using Terraria.DataStructures;
using Terraria.ID;

namespace VBY.GameContentModify.Config;

public class MainConfigInfo
{
    public SpawnInfo Spawn = new();
    public InvasionInfo Invasion = new();
    public BloodMoonInfo BloodMoon = new();
    public WorldInfo World = new();
    public OrbInfo Orb = new();
    [Description("自然生成日食几率")]
    public int EclipseRandomNum = 20;
    [Description("屏蔽射弹破坏物块ID列表")]
    public int[] DisableProjectile_ExplodeTilesIDs = new int[] { ProjectileID.BombSkeletronPrime };
    [Description("老旧摇摇箱在长者史莱姆解锁后依旧生成")]
    public bool BoundTownSlimeOldSpawnAtUnlock = false;
    [Description("老旧摇摇箱开启后生成的物品ID列表")]
    public int[] BoundTownSlimeOldSpawnItemIDs = Array.Empty<int>();
    [Description("禁止蜂王和小蜜蜂伤害其他NPC")]
    public bool DisableQueenBeeAndBeeHurtOtherNPC = false;
    [Description("毒气陷阱叠加")]
    public bool GasTrapsSuperpose = false;
    [Description("毒气陷阱生成射弹ID")]
    public int GasTrapsProjType = 1007;
    [Description("毒气陷阱射弹伤害")]
    public int GasTraosProjDamage = 10;
    public void LoadToStatic()
    {
        SpawnInfo.TownNPCInfo.StaticSpawnAtNight = Spawn.TownNPC.SpawnAtNight;
        SpawnInfo.TownNPCInfo.StaticSpawnAtEclipse = Spawn.TownNPC.SpawnAtEclipse;
        SpawnInfo.TownNPCInfo.StaticSpawnAtInvasion = Spawn.TownNPC.SpawnAtInvasion;
        WorldInfo.StatichardUpdateWorldCheck = World.hardUpdateWorldCheck;
    }
}
[Description("生成")]
public class SpawnInfo
{
    [Description("城镇NPC")]
    public class TownNPCInfo
    {
        public static bool StaticSpawnAtNight = false;
        [Description("晚上生成")]
        public bool SpawnAtNight = false;
        public static bool StaticSpawnAtInvasion = false;
        [Description("有入侵时生成")]
        public bool SpawnAtInvasion = false;
        public static bool StaticSpawnAtEclipse = false;
        [Description("日食时生成")]
        public bool SpawnAtEclipse = false;
    }
    public TownNPCInfo TownNPC = new();
    [Description("自然生成陨石的几率")]
    public int SpawnMeteorRandomNum = 50;
    [Description("自然生成克眼击败检测")]
    public bool EyeSpawnDownedCheck = true;
    [Description("自然生成克眼生命和防御检测")]
    public bool EyeSpawnLifeAndDefenseCheck = true;
    [Description("未击败时自然生成克眼的几率")]
    public int EyeSpawnRandomNum = 3;
    [Description("已击败时自然生成克眼的几率")]
    public int DownedEyeSpawnRandomNum = 3;
    [Description("自然生成克眼城镇NPC数量检测检测")]
    public bool EyeSpawnTownNPCCountCheck = true;
    [Description("自然生成机械Boss的几率")]
    public int MechBossSpawnRandomNum = 10;
    [Description("自然生成机械Boss击败检测")]
    public bool MechBossSpawnDownedCheck = true;
    [Description("自然生成机械Boss克眼自然生成检测")]
    public bool MechBossSpawnEyeCheck = true;
    [Description("自然生成机械Boss时三王为或者的关系")]
    public bool MechBossSpawnIsOr = false;
    [Description("自然生成机械Boss时世界上是否有Boss检测")]
    public bool MechBossSpawnHaveBossInWorldCheck = true;
    [Description("在每天开始的时候生成旅商")]
    public bool SpawnTravelNPCAtDay = false;
    [Description("在每天开始的时候生成旅商的几率")]
    public int SpawnTravelNPCAtDayRandomNum = 10;
}
[Description("入侵")]
public class InvasionInfo
{
    [Description("未击败时自然生成哥布林入侵的几率")]
    public int NoDownedGoblinsStartInvasionRandomNum = 3;
    [Description("已击败时自然生成哥布林入侵的几率")]
    public int DownedGoblinsStartInvasionRandomNum = 30;
    [Description("困难模式且已击败时自然生成哥布林入侵的几率")]
    public int HardModeDownedGoblinsStartInvasionRandomNum = 60;
    [Description("未击败时自然生成海盗入侵的几率")]
    public int NoDownedPiratesStartInvasionRandomNum = 30;
    [Description("已击败时自然生成海盗入侵的几率")]
    public int DownedPiratesStartInvasionRandomNum = 60;
}
[Description("血月")]
public class BloodMoonInfo
{
    [Description("自然生成几率")]
    public int RandomNum = 9;
    [Description("十周年世界自然生成几率")]
    public int TenthAnniversaryWorldRandomNum = 6;
    [Description("自然生成玩家血量检测")]
    public bool LifeCheck = true;
    [Description("自然生成克眼检测")]
    public bool SpawnEyeCheck = true;
    [Description("清除日冕和月冕冷却")]
    public bool ClearSundialAndMoondialCooldown = true;
}
[Description("世界")]
public class WorldInfo
{
    public static bool StatichardUpdateWorldCheck = true;
    [Description("困难模式世界更新的是否为困难模式检测")]
    public bool hardUpdateWorldCheck = true;
}
[Description("球体")]
public class OrbInfo
{
    public int SpanwNPCSmashedCount = 3;
    [Description("心脏掉落物品")]
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
    public int NewItem(IEntitySource source, int X, int Y, int Width, int Height) => Terraria.Item.NewItem(source, X, Y, Width, Height, type, stack, false, prefix);
}