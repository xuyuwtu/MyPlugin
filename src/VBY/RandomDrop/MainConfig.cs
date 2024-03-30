using System.Text.RegularExpressions;
using Terraria;
using Terraria.ID;
using TShockAPI;

namespace VBY.RandomDrop;

internal class MainConfig
{
    public string? ImportAliases = null;
    public Dictionary<string, string> Aliases = new();
    public Dictionary<string, IncludeIDInfo> ItemIDGroups = new();
    public Dictionary<string, IncludeIDInfo> ItemIDs = new();
    public ItemProgressInfo[] ProgressInfos = Array.Empty<ItemProgressInfo>();
    public static MainConfig GetDefaultConfig()
    {
        var config = new MainConfig()
        {
            Aliases = Utils.GetDefaultAliases()
        };
        config.ItemIDGroups.Add("普通矿物及锭", new(new()
        {
            ItemID.CopperOre, ItemID.CopperBar,
            ItemID.TinOre, ItemID.TinBar,
            ItemID.IronOre, ItemID.IronBar,
            ItemID.LeadOre, ItemID.LeadBar,
            ItemID.SilverOre, ItemID.SilverBar,
            ItemID.TungstenOre, ItemID.TungstenBar,
            ItemID.GoldOre, ItemID.GoldBar,
            ItemID.PlatinumOre, ItemID.PlatinumBar
        }));
        config.ItemIDGroups.Add("新三矿及锭", new (new() 
        {
            ItemID.CobaltOre, ItemID.CobaltBar,
            ItemID.MythrilOre, ItemID.MythrilBar,
            ItemID.AdamantiteOre, ItemID.AdamantiteBar,
            ItemID.PalladiumOre, ItemID.PalladiumBar,
            ItemID.OrichalcumOre, ItemID.OrichalcumBar,
            ItemID.TitaniumOre, ItemID.TitaniumBar
        }));
        config.ItemIDGroups.Add("计时器", new (Utils.GetPredicateItemID(item => item.createTile == TileID.Timers)));
        config.ItemIDGroups.Add("宝石", new (Utils.GetPredicateItemID(item => item.createTile == TileID.ExposedGems)));
        config.ItemIDGroups.Add("宝石块和墙", new(null, new() { "1970-1976", "2677-2690"}));
        config.ItemIDGroups.Add("笼子及其里面动物", new(Utils.GetMaterialsType(ItemID.Terrarium)));
        config.ItemIDGroups["笼子及其里面动物"].Ints.Remove(ItemID.TruffleWorm);
        config.ItemIDGroups["笼子及其里面动物"].Ints.Remove(ItemID.MagmaSnail);
        config.ItemIDGroups.Add("放置火箭", new (Utils.GetPredicateItemID(item => item.createTile == TileID.Firework)));
        config.ItemIDGroups.Add("墓碑", new(Utils.GetPredicateItemID(item => item.createTile == TileID.Tombstones)));
        config.ItemIDGroups.Add("八音盒", new (Utils.GetPredicateItemID(item => item.createTile == TileID.MusicBoxes)));
        config.ItemIDGroups.Add("喷泉", new (Utils.GetPredicateItemID(item => item.createTile == TileID.WaterFountain)));
        config.ItemIDGroups.Add("文字雕像", new (Utils.GetPredicateItemID(item => item.createTile == TileID.AlphabetStatues)));
        config.ItemIDGroups.Add("竹子及其制品", new (Utils.GetMaterialCreateItemType(ItemID.BambooBlock)));
        config.ItemIDGroups.Add("苔藓", new(null, new() { "4349–4353", "4354", "4377–4389", "5127", "5128" }));
        config.ItemIDs = Utils.GetEmptyItemIDs(config.Aliases);
        //var trueInfo = config.ItemIDs["true"];
        var trueInfo = new ItemProgressInfo();
        Utils.GetCommonTileItemID().ForEach(x => trueInfo.Ints.AddRange(Utils.GetMaterialType(x)));
        trueInfo.Ints
            .AddPredicateItemID(item => item is { createTile: TileID.Statues, placeStyle: not (>= 43 and <= 49) })
            .AddOreAndBar(config.ItemIDGroups["普通矿物及锭"].Ints);
        trueInfo.Strings.Add("墓碑");
        trueInfo.Strings.Add("宝石");
        trueInfo.Strings.Add("文字雕像");
        trueInfo.Strings.Add("宝石块和墙");
        trueInfo.Strings.Add("笼子及其里面动物");
        var bools = new bool[ItemID.Count];
        var progressArray = Utils.GetCraftingStationsProgressArray();
        foreach (var recipe in Main.recipe)
        {
            if (recipe is null) continue;
            if (recipe.GetRequiredItemIsNotCommonCount(bools) == 0 && recipe.GetRequiredTileIsNotCommonCount(progressArray) == 0)
            {
                bools[recipe.createItem.type] = true;
            }
        }
        foreach(var id in Utils.GetCommonSpawnNPCID())
        {
            Utils.SetNPCCommonDropItemID(id, bools);
        }
        Utils.GetTrueIndexAddToList(trueInfo.Ints, bools);
        config.ItemIDs["骷髅王"].Ints
            .AddPredicateItemID(item => item is { createTile: TileID.Statues, placeStyle:  >= 46 and <= 49 })
            .AddOreAndBar(ItemID.Hellstone, ItemID.HellstoneBar);
        config.ItemIDs["血肉墙"].Ints
            .AddOreAndBar(config.ItemIDGroups["新三矿及锭"].Ints);
        config.ItemIDs["世纪之花"].Ints
            .AddOreAndBar(ItemID.ChlorophyteOre, ItemID.ChlorophyteBar);
        config.ItemIDs["石巨人"].Ints
            .AddPredicateItemID(item => item is { createTile: TileID.Statues, placeStyle: >= 43 and <= 45 });
        config.ItemIDs["月亮领主"].Ints
            .AddOreAndBar(ItemID.LunarOre, ItemID.LunarBar, false);
        config.ItemIDs.PostInitDictionary();
        config.ItemIDGroups.PostInitDictionary();
        return config;
    }
}
public class IncludeIDInfo
{
    public List<int> Ints;
    public List<string> Strings;
    public Dictionary<string, IncludeIDInfo>? SubItemIDs;
    public IncludeIDInfo(List<int>? ints = null, List<string>? strings = null, Dictionary<string, IncludeIDInfo>? subItemIDs = null)
    {
        Ints = ints ?? new List<int>();
        Strings = strings ?? new List<string>();
        SubItemIDs = subItemIDs;
    }
    public void MakeIntsShort()
    {
        if(Ints.Count == 0)
        {
            return;
        }
        Ints.Sort();
        List<int> removeIndexs = new();
        int start, end;
        int startIndex, endIndex;
        for (int curIndex = 0; curIndex < Ints.Count - 1; curIndex++)
        {
            if (Ints[curIndex] + 1 == Ints[curIndex + 1])
            {
                start = Ints[curIndex];
                startIndex = curIndex;
                curIndex++;
                while (curIndex < Ints.Count - 1 && Ints[curIndex] + 1 == Ints[curIndex + 1])
                {
                    curIndex++;
                }
                end = Ints[curIndex];
                endIndex = curIndex;
                Strings.Add($"{start}-{end}");
                for (int j = startIndex; j <= endIndex; j++)
                {
                    removeIndexs.Add(j);
                }
            }
        }
        Ints.RemoveIndexes(removeIndexs);
    }
    public void SetEmptyListNull()
    {
        if (Ints.Count == 0) 
        {
            Ints = null!;
        }
        if (Strings.Count == 0)
        {
            Strings = null!;
        }
    }
    public HashSet<int> GetIntsHashSet(Dictionary<string, IncludeIDInfo> itemGroup)
    {
        var hashset = Ints.ToHashSet();
        if(Strings.Count != 0)
        {
            foreach (var str in Strings)
            {
                if (Regex.IsMatch(str, @"^\d{1,4}$"))
                {
                    hashset.Add(int.Parse(str));
                }
                else if (Regex.IsMatch(str, @"^\d{1,4}-\d{1,4}$"))
                {
                    int num1 = int.Parse(str.AsSpan(0, str.IndexOf('-')));
                    int num2 = int.Parse(str.AsSpan(str.IndexOf('-') + 1));
                    int start = Math.Min(num1, num2);
                    int end = Math.Max(num1, num2);
                    Enumerable.Range(start, end - start + 1).ForEach(x => hashset.Add(x));
                }
                else if (Regex.IsMatch(str, @"^\d{1,4}\+\d{1,2}$"))
                {
                    int start = int.Parse(str.AsSpan(0, str.IndexOf('+')));
                    int count = int.Parse(str.AsSpan(str.IndexOf('+') + 1));
                    Enumerable.Range(start, count).ForEach(x => hashset.Add(x));
                }
                else if (Regex.IsMatch(str, @"^\d{1,3}\[\d{1,3}-\d{1,3}\]$"))
                {
                    var match = Regex.Match(str, @"^(\d{1,3})\[(\d{1,3})-(\d{1,3})\]$");
                    var baseStr = match.Groups[1].Value;
                    int num1 = int.Parse(baseStr + match.Groups[2].Value);
                    int num2 = int.Parse(baseStr + match.Groups[3].Value);
                    int start = Math.Min(num1, num2);
                    int end = Math.Max(num1, num2);
                    Enumerable.Range(start, end - start + 1).ForEach(x => hashset.Add(x));
                }
                else if (itemGroup.TryGetValue(str, out var groupInfo))
                {
                    groupInfo.GetIntsHashSet(itemGroup).ForEach(x => hashset.Add(x));
                }
            }
        }
        return hashset;
    }
}
public class ItemProgressInfo
{
    public List<string> Progress = new();
    public List<int> Ints = new();
    public List<string> Strings = new();
}