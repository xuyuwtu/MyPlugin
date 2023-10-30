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
    public override string Name => GetType().Name;
    public override string Author => "yu";
    public override string Description => "随机掉落";
    public override Version Version => GetType().Assembly.GetName().Version!;

    public static bool[] ItemCanDrop = ItemID.Sets.Factory.CreateBoolSet();
    public static bool[] NpcNotRandomDrop = NPCID.Sets.Factory.CreateBoolSet();
    public static List<int> CanDropItemIDs = new(ItemID.Count);
    public static ConfigManager<Config> MainConfig = new(Config.GetDefault) { PostLoadAction = PostLoad, PreSaveAction = PreSave };
    public static int RandomDropNumber = 4;
    public Command AddCommand;
    public SpawnRandomDrop(Main game) : base(game)
    {
        AddCommand = new($"{nameof(SpawnRandomDrop)}.admin", Cmd, nameof(SpawnRandomDrop).ToLower());
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
        MainConfig.Save();
        Commands.ChatCommands.Remove(AddCommand);
        On.Terraria.NPC.NPCLoot_DropItems -= OnNPC_NPCLoot_DropItems;
        On.Terraria.Item.NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool -= OnItem_NewItem;
        ((System.Collections.IDictionary)typeof(HookEndpointManager).GetField("OwnedHookLists", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!).Remove(HookEndpointManager.GetOwner(OnNPC_NPCLoot_DropItems));
    }
    private void OnNPC_NPCLoot_DropItems(On.Terraria.NPC.orig_NPCLoot_DropItems orig, NPC self, Player closestPlayer)
    {
        if (CanDropItemIDs.Count == 0 || (MainConfig.Instance.SkipEmptyDrop && !Main.ItemDropsDB._entriesByNpcNetId.ContainsKey(self.netID)))
        {
            orig(self, closestPlayer);
            return;
        }
        if (NpcNotRandomDrop[self.type] || Main.rand.Next(RandomDropNumber) != 0)
        {
            orig(self, closestPlayer);
        }
        else
        {
            Item.NewItem(new EntitySource_DebugCommand(), self.position, self.Size, Utils.SelectRandom(CanDropItemIDs, Main.rand));
        }
    }

    private int OnItem_NewItem(On.Terraria.Item.orig_NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool orig, IEntitySource source, int X, int Y, int Width, int Height, int Type, int Stack, bool noBroadcast, int pfix, bool noGrabDelay, bool reverseLookup)
    {
        if (!ItemCanDrop[Type])
        {
            if (source is not (EntitySource_Sync or EntitySource_DebugCommand))
            {
                ItemCanDrop[Type] = true;
                CanDropItemIDs.Add(Type);
            }
        }
        return orig(source, X, Y, Width, Height, Type, Stack, noBroadcast, pfix, noGrabDelay, reverseLookup);
    }

    private void Cmd(CommandArgs args)
    {
        if(args.Parameters.Count == 0)
        {
            args.Player.SendInfoMessage("/spawnrandomdrop add <ids As params int[]>");
            args.Player.SendInfoMessage("/spawnrandomdrop clear");
            args.Player.SendInfoMessage("/spawnrandomdrop save");
            args.Player.SendInfoMessage("/spawnrandomdrop reload");
            return;
        }
        switch (args.Parameters[0])
        {
            case "add":
                if(args.Parameters.Count == 1)
                {
                    args.Player.SendInfoMessage("请输入ID");
                    break;
                }
                for(int i = 1; i < args.Parameters.Count; i++)
                {
                    if (int.TryParse(args.Parameters[i], out var result))
                    {
                        if(result >= 0 && result < ItemCanDrop.Length)
                        {
                            if(!ItemCanDrop[result])
                            {
                                ItemCanDrop[result] = true;
                                CanDropItemIDs.Add(result);
                                args.Player.SendSuccessMessage($"type:{result} 添加成功");
                            }
                            else
                            {
                                args.Player.SendSuccessMessage($"type:{result} 已存在");
                            }
                        }
                    }
                }
                break;
            case "clear":
                Array.Fill(ItemCanDrop, false);
                CanDropItemIDs.Clear();
                MainConfig.Instance.CanDropItemIDs.Clear();
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
                    args.Player.SendInfoMessage("读取配置文件失败");
                }
                else
                {
                    args.Player.SendSuccessMessage("重读成功");
                }
                break;
        }
    }

    private static void PostLoad(Config config, TSPlayer? _)
    {
        Array.Fill(ItemCanDrop, false);
        Array.Fill(NpcNotRandomDrop, false);
        Utils.SetBools(ItemCanDrop, config.CanDropItemIDs);
        Utils.SetBools(NpcNotRandomDrop, config.NotRandomDropNPCIDs);
        RandomDropNumber = config.RandomDropRandomNumber;
        CanDropItemIDs.Clear();
        Utils.AddTrueIndexToList(ItemCanDrop, CanDropItemIDs);
    }

    private static void PreSave(Config config)
    {
        config.CanDropItemIDs.Clear();
        config.NotRandomDropNPCIDs.Clear();
        Utils.AddTrueIndexExpressionToList(ItemCanDrop, config.CanDropItemIDs);
        Utils.AddTrueIndexExpressionToList(NpcNotRandomDrop, config.NotRandomDropNPCIDs);
    }
}