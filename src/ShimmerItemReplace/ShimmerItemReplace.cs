using Terraria;
using Terraria.Enums;
using Terraria.ID;
using TerrariaApi.Server;

using TShockAPI;

using Newtonsoft.Json;

namespace ShimmerItemReplace;
[ApiVersion(2, 1)]
public class ShimmerItemReplace : TerrariaPlugin
{
    public override string Name => "ShimmerItemReplace";
    public override string Author => "yu";
    public override Version Version => new(1, 0);
    public override string Description => "修改微光变换的物品";
    internal MonoMod.RuntimeDetour.Detour? CanShimmerDetour, GetShimmeredDetour;
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
    private bool AddToReload = false;
    private Command Command;
    public ShimmerItemReplace(Main game) : base(game) 
    {
        Command = new Command("sirc", Cmd, "sirc");
        try
        {
            var path = Path.Combine(TShock.SavePath, "ShimmerItemReplace.json");
            if (!Directory.Exists(TShock.SavePath))
            {
                Directory.CreateDirectory(TShock.SavePath);
            }
            if (!File.Exists(path))
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(new Config(), Formatting.Indented));
            }
            var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
            if (config is null)
            {
                TSPlayer.Server.SendErrorMessage("配置文件转换失败");
            }
            else
            {
                if (config.AddToReload)
                {
                    AddToReload = true;
                    TShockAPI.Hooks.GeneralHooks.ReloadEvent += OnReload;
                }
                foreach(var info in config.Replace)
                {
                    if (info.destType != -1)
                    {
                        ItemID.Sets.ShimmerTransformToItem[info.srcType] = info.destType;
                    }
                    if(info.progress >= 0 &&  info.progress < DownedFuncs.Length)
                    {
                        CanShimmerFuncs[info.srcType] = DownedFuncs[info.progress];
                    }
                }
                Command = new Command(config.CommandPermission, Cmd, config.CommandNames);
            }
        }
        catch (Exception ex)
        {
            TSPlayer.Server.SendErrorMessage(ex.ToString());
            TShock.Log.Error(ex.ToString());
        }
    }
    public override void Initialize()
    {
        Commands.ChatCommands.Add(Command);
        CanShimmerDetour = new MonoMod.RuntimeDetour.Detour(typeof(Item).GetMethod("CanShimmer"), typeof(ShimmerItemReplace).GetMethod("CanShimmer"));
        GetShimmeredDetour = new MonoMod.RuntimeDetour.Detour(typeof(Item).GetMethod("GetShimmered"), typeof(ShimmerItemReplace).GetMethod("GetShimmered"));
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Commands.ChatCommands.Remove(Command);
            CanShimmerDetour?.Dispose();
            GetShimmeredDetour?.Dispose();
            if (AddToReload)
            {
                TShockAPI.Hooks.GeneralHooks.ReloadEvent -= OnReload;
            }
        }
        base.Dispose(disposing);
    }
    private void OnReload(TShockAPI.Hooks.ReloadEventArgs e)
    {
        Load(e.Player);
    }
    private static void Load(TSPlayer ply, bool detailed = false)
    {
        try
        {
            var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Path.Combine(TShock.SavePath, "ShimmerItemReplace.json")));
            if (config is null)
            {
                ply.SendErrorMessage("配置文件转换失败");
                return;
            }
            foreach (var info in config.Replace)
            {
                if (info.destType != -1)
                {
                    ItemID.Sets.ShimmerTransformToItem[info.srcType] = info.destType;
                    if (detailed)
                    {
                        ply.SendInfoMessage($"Shimmer:{Lang.GetItemNameValue(info.srcType)} => {Lang.GetItemNameValue(info.destType)}");
                    }
                }
                if (info.progress >= 0 && info.progress < DownedFuncs.Length)
                {
                    CanShimmerFuncs[info.srcType] = DownedFuncs[info.progress];
                }
                else if(detailed)
                {
                    ply.SendInfoMessage("无效progress值:{0}", info.progress);
                }
            }
            ply.SendInfoMessage("加载完成");
        }
        catch (Exception ex)
        {
            ply.SendErrorMessage(ex.ToString());
            TShock.Log.Error(ex.ToString());
            return;
        }
    }
    private void Cmd(CommandArgs args)
    {
        if(args.Parameters.Count == 0)
        {
            args.Player.SendInfoMessage(
                "/{0} reset 重置微光变换为默认值\n" +
                "/{0} load 加载配置\n" +
                "/{0} reload 重置并加载", Command.Name);
            return;
        }
        switch(args.Parameters[0]) 
        {
            case "reset":
                ItemID.Sets.ShimmerTransformToItem = (int[])DefaultShimmerTransformToItem.Clone();
                Array.Fill(CanShimmerFuncs, null);
                args.Player.SendInfoMessage("重置完成");
                break;
            case "load":
                Load(args.Player, args.Parameters.Count > 1 && args.Parameters[1] == "-d");
                break;
            case "reload":
                ItemID.Sets.ShimmerTransformToItem = (int[])DefaultShimmerTransformToItem.Clone();
                Array.Fill(CanShimmerFuncs, null);
                args.Player.SendInfoMessage("重置完成");
                Load(args.Player, args.Parameters.Count > 1 && args.Parameters[1] == "-d");
                break;
            default:
                args.Player.SendErrorMessage("未知参数:{0}", args.Parameters[0]);
                break;
        }
    }
    /// <summary>
    /// 反编译的，改了点东西
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static bool CanShimmer(Item item)
    {
        var func = CanShimmerFuncs[item.type];
        if (func is not null)
        {
            return func();
        }

        int shimmerEquivalentType = item.GetShimmerEquivalentType();
        if (Terraria.GameContent.ShimmerTransforms.IsItemTransformLocked(shimmerEquivalentType))
        {
            return false;
        }

        bool flag = false;
        //if ((type == 1326 || type == 779 || type == 3031 || type == 5364) && NPC.downedMoonlord)
        //{
        //    flag = true;
        //}

        if (item.type == 4986 && !NPC.unlockedSlimeRainbowSpawn)
        {
            flag = true;
        }

        if (item.type == 3461 || item.createTile == 139)
        {
            flag = true;
        }

        if (!flag && ItemID.Sets.ShimmerTransformToItem[shimmerEquivalentType] <= 0 && item.FindDecraftAmount() <= 0 && !ItemID.Sets.CommonCoin[item.type])
        {
            return item.makeNPC > 0;
        }

        return true;
    }
    /// <summary>
    /// 反编译的，改了点东西
    /// </summary>
    /// <param name="item"></param>
    public static void GetShimmered(Item item)
    {
        int shimmerEquivalentType = item.GetShimmerEquivalentType();
        int decraftingRecipeIndex = Terraria.GameContent.ShimmerTransforms.GetDecraftingRecipeIndex(shimmerEquivalentType);
        if (ItemID.Sets.CommonCoin[shimmerEquivalentType])
        {
            switch (shimmerEquivalentType)
            {
                case ItemID.SilverCoin:
                    item.stack *= 100;
                    break;
                case ItemID.GoldCoin:
                    item.stack *= 10000;
                    break;
                case ItemID.PlatinumCoin:
                    if (item.stack > 1)
                    {
                        item.stack = 1;
                    }
                    item.stack *= 1000000;
                    break;
            }
            Main.player[Main.myPlayer].AddCoinLuck(item.Center, item.stack);
            NetMessage.SendData(MessageID.ShimmerActions, -1, -1, null, 1, (int)item.Center.X, (int)item.Center.Y, item.stack);
            item.type = 0;
            item.stack = 0;
        }
        //else if (shimmerEquivalentType == ItemID.RodofDiscord && NPC.downedMoonlord)
        //{
        //    int stack = item.stack;
        //    item.SetDefaults(ItemID.RodOfHarmony);
        //    item.stack = stack;
        //    item.shimmered = true;
        //}
        //else if (shimmerEquivalentType == ItemID.Clentaminator && NPC.downedMoonlord)
        //{
        //    int stack = item.stack;
        //    item.SetDefaults(ItemID.Clentaminator2);
        //    item.stack = stack;
        //    item.shimmered = true;
        //}
        //else if (shimmerEquivalentType == ItemID.BottomlessBucket && NPC.downedMoonlord)
        //{
        //    int stack = item.stack;
        //    item.SetDefaults(ItemID.BottomlessShimmerBucket);
        //    item.stack = stack;
        //    item.shimmered = true;
        //}
        //else if (shimmerEquivalentType == ItemID.BottomlessShimmerBucket && NPC.downedMoonlord)
        //{
        //    int stack = item.stack;
        //    item.SetDefaults(ItemID.BottomlessBucket);
        //    item.stack = stack;
        //    item.shimmered = true;
        //}
        else if (shimmerEquivalentType == ItemID.LunarBrick)
        {
            short num5 = 3461;
            num5 = Main.GetMoonPhase() switch
            {
                MoonPhase.QuarterAtRight => ItemID.StarRoyaleBrick,
                MoonPhase.HalfAtRight => ItemID.CryocoreBrick,
                MoonPhase.ThreeQuartersAtRight => ItemID.CosmicEmberBrick,
                MoonPhase.Full => ItemID.HeavenforgeBrick,
                MoonPhase.ThreeQuartersAtLeft => ItemID.LunarRustBrick,
                MoonPhase.HalfAtLeft => ItemID.AstraBrick,
                MoonPhase.QuarterAtLeft => ItemID.DarkCelestialBrick,
                _ => ItemID.MercuryBrick,
            };
            int num6 = item.stack;
            item.SetDefaults(num5);
            item.stack = num6;
            item.shimmered = true;
        }
        else if (item.createTile == TileID.MusicBoxes)
        {
            int num7 = item.stack;
            item.SetDefaults(ItemID.MusicBox);
            item.stack = num7;
            item.shimmered = true;
        }
        else if (ItemID.Sets.ShimmerTransformToItem[shimmerEquivalentType] > 0)
        {
            //int num8 = item.stack;
            //item.SetDefaults(ItemID.Sets.ShimmerTransformToItem[shimmerEquivalentType]);
            //item.stack = num8;
            //item.shimmered = true;
            var oldstack = item.stack;
            item.SetDefaults(ItemID.Sets.ShimmerTransformToItem[shimmerEquivalentType]);
            if(oldstack > item.maxStack)
            {
                item.stack = item.maxStack;
                Main.item[Item.NewItem(null, item.Center, item.velocity, shimmerEquivalentType, oldstack - item.maxStack)].shimmered = true;
            }
            else
            {
                item.stack = oldstack;
            }
            item.shimmered = true;
        }
        else if (item.type == ItemID.GelBalloon)
        {
            if (NPC.unlockedSlimeRainbowSpawn)
            {
                return;
            }
            NPC.unlockedSlimeRainbowSpawn = true;
            NetMessage.SendData(MessageID.WorldData);
            int num9 = NPC.NewNPC(item.GetNPCSource_FromThis(), (int)item.Center.X + 4, (int)item.Center.Y, 681);
            if (num9 >= 0)
            {
                NPC obj = Main.npc[num9];
                obj.velocity = item.velocity;
                obj.netUpdate = true;
                obj.shimmerTransparency = 1f;
                NetMessage.SendData(MessageID.ShimmerActions, -1, -1, null, 2, num9);
            }
            WorldGen.CheckAchievement_RealEstateAndTownSlimes();
            item.stack--;
            if (item.stack <= 0)
            {
                item.type = 0;
            }
        }
        else if (item.makeNPC > 0)
        {
            int num10 = 50;
            int highestNPCSlotIndexWeWillPick = 200;
            int num11 = NPC.GetAvailableAmountOfNPCsToSpawnUpToSlot(item.stack, highestNPCSlotIndexWeWillPick);
            while (num10 > 0 && num11 > 0 && item.stack > 0)
            {
                num10--;
                num11--;
                item.stack--;
                int num12 = ((NPCID.Sets.ShimmerTransformToNPC[item.makeNPC] < 0) ? NPC.ReleaseNPC((int)item.Center.X, (int)item.Bottom.Y, item.makeNPC, item.placeStyle, Main.myPlayer) : NPC.ReleaseNPC((int)item.Center.X, (int)item.Bottom.Y, NPCID.Sets.ShimmerTransformToNPC[item.makeNPC], 0, Main.myPlayer));
                if (num12 >= 0)
                {
                    Main.npc[num12].shimmerTransparency = 1f;
                    NetMessage.SendData(MessageID.ShimmerActions, -1, -1, null, 2, num12);
                }
            }
            item.shimmered = true;
            if (item.stack <= 0)
            {
                item.type = 0;
            }
        }
        else if (decraftingRecipeIndex >= 0)
        {
            int num13 = item.FindDecraftAmount();
            Recipe recipe = Main.recipe[decraftingRecipeIndex];
            int num14 = 0;
            bool flag = recipe.requiredItem[1].stack > 0;
            IEnumerable<Item> enumerable = recipe.requiredItem;
            if (recipe.customShimmerResults != null)
            {
                enumerable = recipe.customShimmerResults;
            }
            int num15 = 0;
            foreach (Item item2 in enumerable)
            {
                if (item2.type <= 0)
                {
                    break;
                }
                num15++;
                int num16 = num13 * item2.stack;
                if (recipe.alchemy)
                {
                    for (int num17 = num16; num17 > 0; num17--)
                    {
                        if (Main.rand.Next(3) == 0)
                        {
                            num16--;
                        }
                    }
                }
                while (num16 > 0)
                {
                    int num18 = num16;
                    if (num18 > 9999)
                    {
                        num18 = 9999;
                    }
                    num16 -= num18;
                    int num19 = Item.NewItem(item.GetItemSource_Misc(8), (int)item.position.X, (int)item.position.Y, item.width, item.height, item2.type);
                    Item newItem = Main.item[num19];
                    newItem.stack = num18;
                    newItem.shimmerTime = 1f;
                    newItem.shimmered = true;
                    newItem.shimmerWet = true;
                    newItem.wet = true;
                    newItem.velocity *= 0.1f;
                    newItem.playerIndexTheItemIsReservedFor = Main.myPlayer;
                    if (flag)
                    {
                        newItem.velocity.X = 1f * num15;
                        newItem.velocity.X *= 1f + num15 * 0.05f;
                        if (num14 % 2 == 0)
                        {
                            newItem.velocity.X *= -1f;
                        }
                    }
                    NetMessage.SendData(MessageID.SyncItemsWithShimmer, -1, -1, null, num19, 1f);
                }
            }
            item.stack -= num13 * recipe.createItem.stack;
            if (item.stack <= 0)
            {
                item.stack = 0;
                item.type = 0;
            }
        }
        item.shimmerTime = item.stack > 0 ? 1f : 0f;
        item.shimmerWet = true;
        item.wet = true;
        item.velocity *= 0.1f;
        if (Main.netMode == 0)
        {
            Item.ShimmerEffect(item.Center);
        }
        else
        {
            NetMessage.SendData(MessageID.ShimmerActions, -1, -1, null, 0, (int)item.Center.X, (int)item.Center.Y);
            NetMessage.SendData(MessageID.SyncItemsWithShimmer, -1, -1, null, item.whoAmI, 1f);
        }
        Terraria.GameContent.Achievements.AchievementsHelper.NotifyProgressionEvent(27);
        if (item.stack == 0)
        {
            item.makeNPC = -1;
            item.active = false;
        }
    }
}