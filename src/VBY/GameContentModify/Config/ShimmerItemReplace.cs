using System.Text.RegularExpressions;

using Terraria;
using Terraria.ID;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
    private static JsonSerializer jsonSerializer = JsonSerializer.CreateDefault();
    public static void Load(ItemTransformConfig config)
    {
        var setInfo = new ItemTransformInfo();
        foreach (var info in config.TransformInfos)
        {
            setInfo.SetDefault();
            if(info is string str)
            {
                var match = Regex.Match(str, @"^(\d{1,4})([ -])(\d{1,4}) ?(\d{0,2})$");
                if (!match.Success)
                {
                    continue;
                }
                setInfo.srcType = short.Parse(match.Groups[1].Value);
                setInfo.mutual = match.Groups[2].Value == "-";
                setInfo.destType = short.Parse(match.Groups[3].Value);
                if (match.Groups[4].Value.Length > 0)
                {
                    setInfo.progress = byte.Parse(match.Groups[4].Value);
                }
            }
            else if (info is JObject obj)
            {
                jsonSerializer.Populate(obj.CreateReader(), setInfo);
            }
            if (ItemID.Sets.ShimmerTransformToItem.IndexInRange(setInfo.srcType) && ItemID.Sets.ShimmerTransformToItem.IndexInRange(setInfo.destType))
            {
                ItemID.Sets.ShimmerTransformToItem[setInfo.srcType] = setInfo.destType;
                if (setInfo.mutual)
                {
                    ItemID.Sets.ShimmerTransformToItem[setInfo.destType] = setInfo.srcType;
                }
                if (DownedFuncs.IndexInRange(setInfo.progress))
                {
                    CanShimmerFuncs[setInfo.srcType] = DownedFuncs[setInfo.progress];
                }
            }
        }
        foreach(var id in config.ClearIDs)
        {
            ItemID.Sets.ShimmerTransformToItem[id] = -1;
        }
    }
    public static void Reset()
    {
        ItemID.Sets.ShimmerTransformToItem = (int[])DefaultShimmerTransformToItem.Clone();
        Array.Fill(CanShimmerFuncs, null);
    }
}
public class ItemTransformConfig
{
    public List<object> TransformInfos = new();
    public int[] ClearIDs = Array.Empty<int>(); 
}

public class ItemTransformInfo
{
    [JsonProperty("src")]
    public short srcType = -1;
    [JsonProperty("dest")]
    public short destType = -1;
    [JsonProperty("pg")]
    public byte progress = 0;
    public bool mutual = false;
    public ItemTransformInfo() { }
    public ItemTransformInfo(short srcType, short destType, byte progress)
    {
        this.srcType = srcType;
        this.destType = destType;
        this.progress = progress;
    }
    public void SetDefault()
    {
        srcType = -1;
        destType = -1;
        progress = 0;
        mutual = true;
    }
    public override string ToString() => $"{srcType}{(mutual ? "-" : " ")}{destType} {progress}";
}