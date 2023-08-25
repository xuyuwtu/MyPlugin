using Terraria;
using Terraria.ID;

using Newtonsoft.Json;

namespace VBY.GameContentModify.Config;

public class ShimmerItemReplaceInfo
{
    public static readonly int[] DefaultShimmerTransformToItem = (int[])ItemID.Sets.ShimmerTransformToItem.Clone();
    internal static Func<bool>?[] CanShimmerFuncs = new Func<bool>[ItemID.Count];
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
    public static void Load(ItemTransformInfo[] infos)
    {
        foreach (var info in infos)
        {
            if (info.clear)
            {
                ItemID.Sets.ShimmerTransformToItem[info.srcType] = -1;
            }
            else
            {
                if (info.destType != -1)
                {
                    ItemID.Sets.ShimmerTransformToItem[info.srcType] = info.destType;
                    if (info.mutual)
                    {
                        ItemID.Sets.ShimmerTransformToItem[info.destType] = info.srcType;
                    }
                }
                if (info.progress >= 0 && info.progress < DownedFuncs.Length)
                {
                    CanShimmerFuncs[info.srcType] = DownedFuncs[info.progress];
                }
            }
        }
    }
    public static void Reset()
    {
        ItemID.Sets.ShimmerTransformToItem = (int[])DefaultShimmerTransformToItem.Clone();
        Array.Fill(CanShimmerFuncs, null);
    }
}

public class ItemTransformInfo
{
    [JsonProperty("src")]
    public short srcType;
    [JsonProperty("dest")]
    public short destType = -1;
    [JsonProperty("pg")]
    public byte progress = 0;
    public bool mutual = false;
    public bool clear = false;
    public ItemTransformInfo() { }
    public ItemTransformInfo(short srcType, short destType, byte progress)
    {
        this.srcType = srcType;
        this.destType = destType;
        this.progress = progress;
    }
}