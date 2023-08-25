using Newtonsoft.Json;
using Terraria.ID;

namespace ShimmerItemReplace;

internal class Config
{
    public bool AddToReload = false;
    public string[] CommandNames = new string[] { "sirc" };
    public string CommandPermission = "sirc";
    public TransformInfo[] Replace = new TransformInfo[]
    {
        new(ItemID.RodofDiscord,    ItemID.RodOfHarmony, 22),
        new(ItemID.Clentaminator,   ItemID.Clentaminator2, 22),
        new(ItemID.BottomlessBucket,ItemID.BottomlessShimmerBucket,22),
        new(ItemID.BottomlessShimmerBucket,ItemID.BottomlessBucket,22),
        new(ItemID.JungleKey,       ItemID.PiranhaGun,13),
        new(ItemID.CorruptionKey,   ItemID.ScourgeoftheCorruptor,13),
        new(ItemID.CrimsonKey,      ItemID.VampireKnives,13),
        new(ItemID.HallowedKey,     ItemID.RainbowGun,13),
        new(ItemID.FrozenKey,       ItemID.StaffoftheFrostHydra,13),
        new(ItemID.DungeonDesertKey,ItemID.StormTigerStaff,13)
    };
}
public class TransformInfo
{
    [JsonProperty("src")]
    public short srcType;
    [JsonProperty("dest")]
    public short destType = -1;
    [JsonProperty("pg")]
    public byte progress = 0;
    public TransformInfo() { }
    public TransformInfo(short srcType, short destType, byte progress)
    {
        this.srcType = srcType;
        this.destType = destType;
        this.progress = progress;
    }
}