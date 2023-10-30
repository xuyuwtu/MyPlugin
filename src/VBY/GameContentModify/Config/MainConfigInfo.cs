using System.ComponentModel;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Net;

namespace VBY.GameContentModify.Config;

public class MainConfigInfo
{
    public SpawnInfo Spawn = new();
    public InvasionInfo Invasion = new();
    public BloodMoonInfo BloodMoon = new();
    public WorldInfo World = new();
    public OrbInfo Orb = new();
    public TeleportPylonsInfo TeleportPylons = new();
    [Description("自然生成日食几率: 1/{0}")]
    public int EclipseRandomNum = 20;
    [Description("屏蔽射弹破坏物块ID列表")]
    public int[] DisableProjectile_ExplodeTilesIDs = new int[] { ProjectileID.BombSkeletronPrime };
    [Description("老旧摇摇箱在长者史莱姆解锁后依旧生成: {0}")]
    public bool BoundTownSlimeOldSpawnAtUnlock = false;
    [Description("老旧摇摇箱开启后生成的物品ID列表")]
    public int[] BoundTownSlimeOldSpawnItemIDs = Array.Empty<int>();
    [Description("禁止蜂王和小蜜蜂伤害其他NPC: {0}")]
    public bool DisableQueenBeeAndBeeHurtOtherNPC = false;
    [Description("毒气陷阱叠加: {0}")]
    public bool GasTrapsSuperpose = false;
    [Description("毒气陷阱生成射弹ID: {0}")]
    public int GasTrapsProjType = 1007;
    [Description("毒气陷阱射弹伤害: {0}")]
    public int GasTraosProjDamage = 10;
    [Description("渔夫在海边水里死亡时会生成猪鲨: {0}")]
    public bool SpawnDukeFishronWhenAnglerDeadAtSeaZoneInWater = false;
    private static bool _StaticSpawnDukeFishronWhenAnglerDeadAtSeaZoneInWater = false;
    public static bool StaticSpawnDukeFishronWhenAnglerDeadAtSeaZoneInWater
    {
        get => _StaticSpawnDukeFishronWhenAnglerDeadAtSeaZoneInWater;
        set
        {
            if(value != _StaticSpawnDukeFishronWhenAnglerDeadAtSeaZoneInWater)
            {
                if (value)
                {
                    On.Terraria.NPC.DoDeathEvents += OnNPC_DoDeathEvents;
                }
                else
                {
                    On.Terraria.NPC.DoDeathEvents -= OnNPC_DoDeathEvents;
                }
                _StaticSpawnDukeFishronWhenAnglerDeadAtSeaZoneInWater = value;
            }
        }
    }
    public static HashSet<ushort> NotSendNetPacketIDs = new();
    [Description("不发送的NetPacket类名称")]
    public string[] NotSendNetPacketNames = Array.Empty<string>();
    private static void OnNPC_DoDeathEvents(On.Terraria.NPC.orig_DoDeathEvents orig, NPC self, Player closestPlayer)
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
    public void LoadToStatic()
    {
        StaticSpawnDukeFishronWhenAnglerDeadAtSeaZoneInWater = SpawnDukeFishronWhenAnglerDeadAtSeaZoneInWater;
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
        SpawnInfo.TownNPCInfo.StaticSpawnAtNight = Spawn.TownNPC.SpawnAtNight;
        SpawnInfo.TownNPCInfo.StaticSpawnAtEclipse = Spawn.TownNPC.SpawnAtEclipse;
        SpawnInfo.TownNPCInfo.StaticSpawnAtInvasion = Spawn.TownNPC.SpawnAtInvasion;
        WorldInfo.StatichardUpdateWorldCheck = World.hardUpdateWorldCheck;
    }
    public static void ResetStatic()
    {
        StaticSpawnDukeFishronWhenAnglerDeadAtSeaZoneInWater = false;
        NotSendNetPacketIDs.Clear();
        SpawnInfo.TownNPCInfo.StaticSpawnAtNight = false;
        SpawnInfo.TownNPCInfo.StaticSpawnAtEclipse = false;
        SpawnInfo.TownNPCInfo.StaticSpawnAtInvasion = false;
        WorldInfo.StatichardUpdateWorldCheck = true;
    }
}
[Description("生成")]
public class SpawnInfo
{
    [Description("城镇NPC")]
    public class TownNPCInfo
    {
        private static bool _StaticSpawnAtNight = false;
        public static bool StaticSpawnAtNight
        {
            get => _StaticSpawnAtNight;
            set
            {
                if(value != _StaticSpawnAtNight)
                {
                    if (value)
                    {
                        On.Terraria.Main.UpdateTime += OnMain_UpdateTime;
                    }
                    else
                    {
                        On.Terraria.Main.UpdateTime -= OnMain_UpdateTime;
                    }
                    _StaticSpawnAtNight = value;
                }
            }
        }
        [Description("晚上生成: {0}")]
        public bool SpawnAtNight = false;
        public static bool StaticSpawnAtInvasion = false;
        [Description("有入侵时生成: {0}")]
        public bool SpawnAtInvasion = false;
        public static bool StaticSpawnAtEclipse = false;
        [Description("日食时生成: {0}")]
        public bool SpawnAtEclipse = false;
        private static void OnMain_UpdateTime(On.Terraria.Main.orig_UpdateTime orig)
        {
            orig();
            if (!Main.dayTime)
            {
                Main.UpdateTime_SpawnTownNPCs();
            }
        }
    }
    public TownNPCInfo TownNPC = new();
    [Description("自然生成陨石的几率: 1/{0}")]
    public int SpawnMeteorRandomNum = 50;
    [Description("自然生成克眼击败检测: {0}")]
    public bool EyeSpawnDownedCheck = true;
    [Description("自然生成克眼生命和防御检测: {0}")]
    public bool EyeSpawnLifeAndDefenseCheck = true;
    [Description("未击败时自然生成克眼的几率: 1/{0}")]
    public int EyeSpawnRandomNum = 3;
    [Description("已击败时自然生成克眼的几率: 1/{0}")]
    public int DownedEyeSpawnRandomNum = 3;
    [Description("自然生成克眼城镇NPC数量检测检测: {0}")]
    public bool EyeSpawnTownNPCCountCheck = true;
    [Description("自然生成机械Boss的几率: 1/{0}")]
    public int MechBossSpawnRandomNum = 10;
    [Description("自然生成机械Boss击败检测: {0}")]
    public bool MechBossSpawnDownedCheck = true;
    [Description("自然生成机械Boss克眼自然生成检测: {0}")]
    public bool MechBossSpawnEyeCheck = true;
    [Description("自然生成机械Boss时三王为或者的关系: {0}")]
    public bool MechBossSpawnIsOr = false;
    [Description("自然生成机械Boss时世界上是否有Boss检测: {0}")]
    public bool MechBossSpawnHaveBossInWorldCheck = true;
    [Description("在每天开始的时候生成旅商: {0}")]
    public bool SpawnTravelNPCAtDay = false;
    [Description("在每天开始的时候生成旅商的几率: 1/{0}")]
    public int SpawnTravelNPCAtDayRandomNum = 10;
}
[Description("入侵")]
public class InvasionInfo
{
    [Description("未击败时自然生成哥布林入侵的几率: 1/{0}")]
    public int NoDownedGoblinsStartInvasionRandomNum = 3;
    [Description("已击败时自然生成哥布林入侵的几率: 1/{0}")]
    public int DownedGoblinsStartInvasionRandomNum = 30;
    [Description("困难模式且已击败时自然生成哥布林入侵的几率: 1/{0}")]
    public int HardModeDownedGoblinsStartInvasionRandomNum = 60;
    [Description("未击败时自然生成海盗入侵的几率: 1/{0}")]
    public int NoDownedPiratesStartInvasionRandomNum = 30;
    [Description("已击败时自然生成海盗入侵的几率: 1/{0}")]
    public int DownedPiratesStartInvasionRandomNum = 60;
}
[Description("血月")]
public class BloodMoonInfo
{
    [Description("自然生成几率: 1/{0}")]
    public int RandomNum = 9;
    [Description("十周年世界自然生成几率: 1/{0}")]
    public int TenthAnniversaryWorldRandomNum = 6;
    [Description("自然生成玩家血量检测: {0}")]
    public bool LifeCheck = true;
    [Description("自然生成克眼检测: {0}")]
    public bool SpawnEyeCheck = true;
    [Description("清除日冕和月冕冷却: {0}")]
    public bool ClearSundialAndMoondialCooldown = true;
}
[Description("世界")]
public class WorldInfo
{
    public static bool StatichardUpdateWorldCheck = true;
    [Description("困难模式世界更新的是否为困难模式检测: {0}")]
    public bool hardUpdateWorldCheck = true;
}
[Description("球体")]
public class OrbInfo
{
    [Description("生成Boss需要的数量")]
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
[Description("晶塔")]
public class TeleportPylonsInfo
{
    [Description("危险检测: {0}")]
    public bool DangerCheck = true;
    [Description("花前神庙墙检测: {0}")]
    public bool PreDownedPlantBossTempleCheck = true;
    [Description("环境要求检测: {0}")]
    public bool EnvironmentalRequirementsCheck = true;
    [Description("需要的城镇NPC数量: {0}")]
    public int NeedNPCCount = 2;
    [Description("使用时检测距离X: {0}")]
    public int ReachX = 60;
    [Description("使用时检测距离Y: {0}")]
    public int ReachY = 60;
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