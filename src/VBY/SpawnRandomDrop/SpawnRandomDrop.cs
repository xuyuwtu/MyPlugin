using System.Reflection;

using MonoMod.RuntimeDetour.HookGen;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using TerrariaApi.Server;

using TShockAPI;

using VBY.Common.Config;

namespace VBY.SpawnRandomDrop;

[ApiVersion(2, 1)]
public class SpawnRandomDrop : TerrariaPlugin
{
    public override string Name => GetType().Namespace!;
    public override string Author => "yu";
    public override string Description => "随机掉落";
    public override Version Version => GetType().Assembly.GetName().Version!;

    public static bool[] ItemCanDrop = ItemID.Sets.Factory.CreateBoolSet();
    public static bool[] NpcNotRandomDrop = NPCID.Sets.Factory.CreateBoolSet();
    public static bool[] TileCanUse = TileID.Sets.Factory.CreateBoolSet();
    public static bool[] RecipeChecked = new bool[Main.recipe.Length];
    public static List<int> CanDropItemIDs = new(ItemID.Count);
    public static List<int> RequiredTileIDs = new();
    public static ConfigManager<Config> MainConfig = new(Config.GetDefault) { PostLoadAction = PostLoad, PreSaveAction = PreSave };

    public static int RandomDropRandomNumber = 4;
    public static bool SkipEmptyDrop = true;
    public static bool SkipStatue = true;

    private static int RecipeStartIndex = 0;
    private static bool[] AdditionalBools = ItemID.Sets.Factory.CreateBoolSet();
    private static Dictionary<int, object[]> AdditionalGroup = new();

    public Command AddCommand;
    public SpawnRandomDrop(Main game) : base(game)
    {
        AddCommand = new($"srd.admin", Cmd, "srd");
    }

    public override void Initialize()
    {
        MainConfig.Load(TSPlayer.Server);
        Commands.ChatCommands.Add(AddCommand);
        On.Terraria.NPC.NPCLoot_DropItems += OnNPC_NPCLoot_DropItems;
        On.Terraria.Item.NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool += OnItem_NewItem;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            MainConfig.Save();
            Commands.ChatCommands.Remove(AddCommand);
            On.Terraria.NPC.NPCLoot_DropItems -= OnNPC_NPCLoot_DropItems;
            On.Terraria.Item.NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool -= OnItem_NewItem;
            ((System.Collections.IDictionary)typeof(HookEndpointManager).GetField("OwnedHookLists", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!).Remove(HookEndpointManager.GetOwner(OnNPC_NPCLoot_DropItems));
        }
    }
    private void OnNPC_NPCLoot_DropItems(On.Terraria.NPC.orig_NPCLoot_DropItems orig, NPC self, Player closestPlayer)
    {
        if (CanDropItemIDs.Count == 0 || (SkipEmptyDrop && !Main.ItemDropsDB._entriesByNpcNetId.ContainsKey(self.netID)) || (SkipStatue && self.SpawnedFromStatue))
        {
            orig(self, closestPlayer);
            return;
        }
        if(MainConfig.Instance.DropType != DropType.Add)
        {
            if (NpcNotRandomDrop[self.type] || !(Main.rand.Next(RandomDropRandomNumber) == 0))
            {
                orig(self, closestPlayer);
                return;
            }
        }
        Item.NewItem(new EntitySource_DebugCommand(), self.position, self.Size, Utils.SelectRandom(CanDropItemIDs, Main.rand));
    }

    private int OnItem_NewItem(On.Terraria.Item.orig_NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool orig, IEntitySource source, int X, int Y, int Width, int Height, int Type, int Stack, bool noBroadcast, int pfix, bool noGrabDelay, bool reverseLookup)
    {
        int itemIndex = orig(source, X, Y, Width, Height, Type, Stack, noBroadcast, pfix, noGrabDelay, reverseLookup);
        if (!ItemCanDrop[Type])
        {
            if (source is not (EntitySource_Sync or EntitySource_DebugCommand))
            {
                AddCanDropItem(Main.item[itemIndex]);
            }
        }
        return itemIndex;
    }

    private void Cmd(CommandArgs args)
    {
        var enumerator = args.Parameters.GetEnumerator();
        if(!enumerator.MoveNext())
        {
            args.Player.SendInfoMessage("/srd add item <ids As params int[]>");
            args.Player.SendInfoMessage("/srd add tile <ids As params int[]>");
            args.Player.SendInfoMessage("/srd recipeupdate");
            args.Player.SendInfoMessage("/srd init");
            args.Player.SendInfoMessage("/srd clear");
            args.Player.SendInfoMessage("/srd save");
            args.Player.SendInfoMessage("/srd reload");
            return;
        }

        void AddIDs(Func<int, bool> addFunc)
        {
            if (!enumerator.MoveNext())
            {
                args.Player.SendInfoMessage("请输入ID");
                return;
            }
            do
            {
                if (int.TryParse(enumerator.Current, out var result))
                {
                    if (addFunc(result))
                    {
                        args.Player.SendSuccessMessage($"type:{result} 添加成功");
                    }
                    else
                    {
                        args.Player.SendSuccessMessage($"type:{result} 已存在或者错误ID");
                    }
                }
            } while (enumerator.MoveNext());
        }

        switch (enumerator.Current)
        {
            case "add":
                {
                    if (!enumerator.MoveNext())
                    {
                        args.Player.SendInfoMessage("/srd add item <ids As params int[]>");
                        args.Player.SendInfoMessage("/srd add tile <ids As params int[]>");
                        break;
                    }
                    switch(enumerator.Current)
                    {
                        case "item":
                            AddIDs(AddCanDropItemID);
                            break;
                        case "tile":
                            AddIDs(AddCanUseTileID);
                            break;
                    }
                }
                break;
            case "init":
                {
                    var t1 = MainConfig.Instance.InitAddIDs;
                    var t2 = MainConfig.Instance.IDGroup;
                    var t3 = MainConfig.Instance.AdditionalGroup;

                    MainConfig.Instance = MainConfig.GetDefaultFunc();

                    MainConfig.Instance.InitAddIDs = t1;
                    MainConfig.Instance.IDGroup = t2;
                    MainConfig.Instance.AdditionalGroup = t3;

                    MainConfig.PostLoadAction?.Invoke(MainConfig.Instance, args.Player, false);
                    for (int x = 0; x < Main.maxTilesX; x++)
                    {
                        for (int y = 0; y < Main.maxTilesY; y++)
                        {
                            if (Main.tile[x, y] is not null)
                            {
                                TileCanUse[Main.tile[x, y].type] = true;
                            }
                        }
                    }
                    foreach(var id in Utils.GetIndexes(MainConfig.Instance.InitAddIDs, MainConfig.Instance.IDGroup))
                    {
                        AddCanDropItemID(id);
                    }
                    foreach (var id in Utils.GetNotLockedChestItemIDs())
                    {
                        AddCanDropItemID(id);
                    }
                    MainConfig.Save();
                }
                break;
            case "recipeupdate":
                RecipeCheck();
                args.Player.SendSuccessMessage("更新完成");
                break;
            case "clear":
                Array.Fill(ItemCanDrop, false);
                Array.Fill(NpcNotRandomDrop, false);
                Array.Fill(TileCanUse, false);
                Array.Fill(RecipeChecked, false);
                MainConfig.Save();
                args.Player.SendSuccessMessage("清除成功");
                break;
            case "save":
                MainConfig.Save();
                args.Player.SendSuccessMessage("保存成功");
                break;
            case "reload":
                if (MainConfig.Load(args.Player))
                {
                    args.Player.SendSuccessMessage("重读成功");
                }
                else
                {
                    args.Player.SendInfoMessage("读取配置文件失败");
                }
                break;
        }
    }

    private static bool AddCanDropItemID(int id)
    {
        if (id < 0 || id >= ItemCanDrop.Length || ItemCanDrop[id])
        {
            return false;
        }
        ItemCanDrop[id] = true;
        CanDropItemIDs.Add(id);
        if (AdditionalGroup.TryGetValue(id, out var group))
        {
            Utils.SetBools(AdditionalBools, group, MainConfig.Instance.IDGroup);
            foreach (var addId in Terraria.Utils.GetTrueIndexes(AdditionalBools))
            {
                AddCanDropItemID(addId);
            }
        }
        if (ItemID.Sets.IsAMaterial[id])
        {
            RecipeCheck();
        }
        return true;
    }

    private static bool AddCanUseTileID(int id)
    {
        if (id < 0 || id > TileCanUse.Length)
        {
            return false;
        }
        TileCanUse[id] = true;
        if (RequiredTileIDs.Contains(id))
        {
            RecipeCheck();
        }
        return true;
    }

    private static void AddCanDropItem(Item item)
    {
        AddCanUseTileID(item.createTile);
        AddCanDropItemID(item.type);
    }

    private static void UpdateRecipeStartIndex()
    {
        for (int i = RecipeStartIndex; i < RecipeChecked.Length; i++)
        {
            if (!RecipeChecked[i])
            {
                RecipeStartIndex = i;
                break;
            }
        }
    }

    private static void RecipeCheck()
    {
        for (int recipeIndex = RecipeStartIndex; recipeIndex < Recipe.numRecipes; recipeIndex++)
        {
            if (RecipeChecked[recipeIndex])
            {
                continue;
            }
            var recipe = Main.recipe[recipeIndex];
            if (ItemCanDrop[recipe.createItem.type])
            {
                RecipeChecked[recipeIndex] = true;
                AddCanUseTileID(recipe.createItem.createTile);
                UpdateRecipeStartIndex();
                continue;
            }
            var tileCheck = true;
            for (int tileIndex = 0; tileIndex < recipe.requiredTile.Length; tileIndex++)
            {
                if (recipe.requiredTile[tileIndex] == -1)
                {
                    break;
                }
                if (!TileCanUse[recipe.requiredTile[tileIndex]])
                {
                    tileCheck = false;
                    break;
                }
            }
            if (!tileCheck)
            {
                continue;
            }
            bool itemCheck = true;
            for (int itemIndex = 0; itemIndex < recipe.requiredItem.Length; itemIndex++)
            {
                if (recipe.requiredItem[itemIndex].IsAir)
                {
                    break;
                }
                if (!ItemCanDrop[recipe.requiredItem[itemIndex].type])
                {
                    itemCheck = false;
                    break;
                }
            }
            if (itemCheck)
            {
                AddCanDropItem(recipe.createItem);
            }
        }
    }

    private static void PostLoad(Config config, TSPlayer? player, bool first)
    {
        Utils.SetBools(ItemCanDrop, config.CanDropItemIDs, config.IDGroup);
        Utils.SetBools(NpcNotRandomDrop, config.NotRandomDropNPCIDs, config.IDGroup);
        Utils.SetBools(TileCanUse, config.CanUseTileIDs, config.IDGroup);

        var requiredTileHashSet = new HashSet<int>();
        RecipeStartIndex = 0;
        for (int recipeIndex = RecipeStartIndex; recipeIndex < Recipe.numRecipes; recipeIndex++)
        {
            var recipe = Main.recipe[recipeIndex];
            if (ItemCanDrop[recipe.createItem.type] || recipe.createItem.type is > 71 and < 74)
            {
                RecipeChecked[recipeIndex] = true;
            }
            for (int tileIndex = 0; tileIndex < recipe.requiredTile.Length; tileIndex++) 
            {
                if (recipe.requiredTile[tileIndex] == -1)
                {
                    break;
                }
                requiredTileHashSet.Add(recipe.requiredTile[tileIndex]);
            }
        }
        UpdateRecipeStartIndex();
        RequiredTileIDs.Clear();
        RequiredTileIDs.AddRange(requiredTileHashSet);
        RequiredTileIDs.Sort();

        AdditionalGroup.Clear();
        foreach(var keyValue in config.AdditionalGroup)
        {
            AdditionalGroup.Add(Utils.ParseSingleNumber(keyValue.Key), keyValue.Value);
        }
        RandomDropRandomNumber = config.RandomDropRandomNumber;
        SkipEmptyDrop = config.SkipEmptyDrop;
        SkipStatue = config.SkipStatue;
        CanDropItemIDs.Clear();
        Utils.AddTrueIndexToList(ItemCanDrop, CanDropItemIDs);
    }

    private static void PreSave(Config config)
    {
        Utils.AddTrueIndexExpressionToList(ItemCanDrop, config.CanDropItemIDs);
        Utils.AddTrueIndexExpressionToList(NpcNotRandomDrop, config.NotRandomDropNPCIDs);
        Utils.AddTrueIndexExpressionToList(TileCanUse, config.CanUseTileIDs);
    }
}