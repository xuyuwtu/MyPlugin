using Newtonsoft.Json;

using Terraria;
namespace VBY.ItemStrengthen;

public class Configuration
{
    internal static Func<bool>[] DownedFuncs = new Func<bool>[]
    {
        () => true, //0 无
        () => NPC.downedSlimeKing, // 1 史莱姆王
        () => NPC.downedBoss1, // 2 克眼
        () => NPC.downedBoss2, // 3 世吞 或 克脑
        () => NPC.downedQueenBee, // 4 蜂王
        () => NPC.downedBoss3, // 5 骷髅王
        () => NPC.downedDeerclops, // 6 鹿角怪
        () => Main.hardMode, // 7 困难模式(肉山)
        () => NPC.downedQueenSlime, // 8 史莱姆皇后
        () => NPC.downedMechBossAny, // 9 任意机械Boss
        () => NPC.downedMechBoss1, // 10 毁灭者
        () => NPC.downedMechBoss2, // 11 双子魔眼
        () => NPC.downedMechBoss3, // 12 机械骷髅王
        () => NPC.downedPlantBoss, // 13 世纪之花
        () => NPC.downedGolemBoss, // 14 石巨人
        () => NPC.downedFishron, // 15 猪鲨
        () => NPC.downedEmpressOfLight, // 16 光女
        () => NPC.downedAncientCultist, // 17 教徒
        () => NPC.downedTowerSolar, // 18 日耀柱
        () => NPC.downedTowerNebula, // 19 星云柱
        () => NPC.downedTowerVortex, // 20 星旋柱
        () => NPC.downedTowerStardust, // 21 星尘柱
        () => NPC.downedMoonlord, // 22 月亮领主
        () => NPC.downedHalloweenTree, // 23 哀木
        () => NPC.downedHalloweenKing, // 24 南瓜王
        () => NPC.downedChristmasTree, // 25 常绿尖叫怪
        () => NPC.downedChristmasSantank, // 26 圣诞坦克
        () => NPC.downedChristmasIceQueen, // 27 冰雪女王
        () => NPC.downedTowers, // 28 四柱
        () => NPC.downedClown, // 29 小丑
        () => NPC.downedGoblins, // 30 哥布林入侵
        () => NPC.downedPirates, // 31 海盗入侵
        () => NPC.downedMartians // 32 火星暴乱
    };
    [JsonProperty(Order = 0)]
    public List<ItemInfo> ItemInfos { get; set; } = new();
    [JsonProperty(Order = 1)]
    public Dictionary<string, int> IDs { get; set; } = new();
}
public class ItemInfo
{
    public int progress;
    public int priority;
    public int type;
    public int prefix;
    public string? scale;
    public string? width;
    public string? height;
    public string? damage;
    public string? useTime;
    public string? knockBack;
    public string? shootSpeed;
    public string? useAnimation;
    public string? ammo;
    public string? shoot;
    public string? color;
    public string? useAmmo;
    public bool? notAmmo;
}
