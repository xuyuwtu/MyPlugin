using Terraria.ID;
using static Terraria.ID.ItemID;

namespace VBY.RandomDrop;

partial class Utils
{
    public static int[] GetCommonTileItemID() => new int[] {
            DirtBlock,
            StoneBlock,
            ClayBlock,
            MudBlock,
            SiltBlock,
            SandBlock,
            AshBlock,
            SnowBlock,
            IceBlock,
            SlushBlock,
            HoneyBlock,
            CrispyHoneyBlock,
            SunplateBlock,
            EbonsandBlock,
            EbonstoneBlock,
            CrimstoneBlock,
            CrimsandBlock,
            ActiveStoneBlock,
            InactiveStoneBlock,
            Marble,
            Granite,
            SandFallBlock,
            SnowFallBlock,
            SnowCloudBlock,
            BambooBlock,
            ReefBlock,
            ShimmerBlock,
            PoopBlock
        };
    public static string?[] GetCraftingStationsProgressArray()
    {
        var array = new string[TileID.Count];
        array[TileID.Bottles] = "true";
        array[TileID.Tables] = "true";
        array[TileID.Chairs] = "true";
        array[TileID.Anvils] = "true";
        array[TileID.Furnaces] = "true";
        array[TileID.WorkBenches] = "true";
        array[TileID.DemonAltar] = "true";
        array[TileID.Hellforge] = "骷髅王";
        array[TileID.Loom] = "true";
        array[TileID.Kegs] = "true";
        array[TileID.CookingPots] = "true";
        array[TileID.Bookcases] = "骷髅王";
        array[TileID.Sawmill] = "true";
        array[TileID.TinkerersWorkbench] = "拯救哥布林";
        array[TileID.AdamantiteForge] = "血肉墙";
        array[TileID.MythrilAnvil] = "血肉墙";
        array[TileID.Blendomatic] = "任意机械BOSS";
        array[TileID.MeatGrinder] = "血肉墙";
        array[TileID.Solidifier] = "史莱姆王";
        array[TileID.DyeVat] = "true";
        array[TileID.ImbuingStation] = "蜂王";
        array[TileID.Autohammer] = "世纪之花";
        array[TileID.HeavyWorkBench] = "true";
        array[TileID.BoneWelder] = "骷髅王";
        array[TileID.FleshCloningVat] = "任意机械BOSS";
        array[TileID.GlassKiln] = "true";
        array[TileID.LihzahrdFurnace] = "石巨人";
        array[TileID.LivingLoom] = "true";
        array[TileID.SkyMill] = "true";
        array[TileID.IceMachine] = "true";
        array[TileID.SteampunkBoiler] = "任意机械BOSS";
        array[TileID.HoneyDispenser] = "true";
        return array;
    }
    public static int[] GetBossNPCID()
    {
        return new int[] { 4, 5, 13, 14, 15, 35, 36, 50, 113, 114, 115, 116, 117, 118, 119, 125, 126, 127, 128, 129, 130, 131, 134, 135, 136, 139, 222, 245, 246, 247, 248, 249, 262, 263, 264, 265, 266, 267, 370, 371, 372, 373, 369, 397, 398, 400, 401, 422, 439, 440, 493, 507, 517, 636, 657, 668 };
    }
    public static int[] GetCommonSpawnNPCID()
    {
        return ToInts(^65..^25, ^23..^21, ^17..^15, ^12..^2, 1..3, 6..12, 16..22, 24..26, 37..50, 51..68, 69..70, 73..75, 132..133, 147..150, 161..162, 167..169, 173..174, 181..182, 184..197, 200..204, 207..209, 210..212, 217..222, 223..226, 227..228, 230..236, 239..241, 254..260, 297..304, 316..325, 331..338, 353..370, 376..378, 430..437, 442..454, 464..466, 470..471, 481..491, 494..507, 508..510, 513..516, 535..541, 546..547, 580..618, 624..629, 632..636, 637..657, 667..668, 669..685);
    }
    public static int[] GetGoblinsCommonSpawnNPCID()
    {
        return ToInts(26..31, 105..106, 107..108, 111..112);
    }
    public static int[] GetQueenBeeCommonSpawnNPCID()
    {
        return ToInts(228..229);
    }
    public static int[] GetDungeonCommonSpawnNPCID()
    {
        return ToInts(^14..^12, 31..35, 68..69, 70..73, 123..125);
    }
    public static int[] GetHardModeCommonSpawnNPCID()
    {
        return ToInts(^25..^23, ^21..^17, ^15..^14, ^2..0, 75..105, 106..107, 108..111, 112..113, 120..123, 133..134, 137..139, 140..146, 150..156, 157..158, 160..161, 163..166, 169..173, 174..178, 179..181, 182..184, 206..207, 236..239, 241..245, 250..251, 252..253, 260..262, 268..269, 304..305, 374..376, 378..379, 441..442, 471..477, 480..481, 510..513, 524..535, 541..546, 618..624, 629..632);
    }
    public static int[] GetPiratesCommonSpawnNPCID()
    {
        return ToInts(212..217, 269..297, 491..493, 662..663);
    }
    public static int[] GetAnyMechBossCommonSpawnNPCID()
    {
        return ToInts(156..157, 158..160, 162..163, 166..167, 178..179, 205..206, 209..210, 251..252, 461..463, 469..470);
    }
    public static int[] GetAllMechBossCommonSpawnNPCID()
    {
        return ToInts(253..254);
    }
    public static int[] GetPlantBossCommonSpawnNPCID()
    {
        return ToInts(198..200, 226..227, 229..230, 305..316, 325..331, 338..353, 460..461, 463..464, 466..469, 477..480, 661..662, 663..664);
    }
    public static int[] GetGolemBossCommonSpawnNPCID()
    {
        return ToInts(379..381, 399..400, 437..439);
    }
    public static int[] GetMartiansCommonSpawnNPCID()
    {
        //Terraria.NPC.downed
        return ToInts(381..396, 520..521);
    }
    public static int[] GetAncientCultistCommonSpanwNPCID()
    {
        return ToInts(402..422, 423..430, 454..460, 516..517, 518..520, 521..524);
    }
    public static int[] GetDD2EventSpawnNPPCID()
    {
        return ToInts(548..550,551..579);
    }
}
