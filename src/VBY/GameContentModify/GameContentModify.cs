using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Text;

using Microsoft.Xna.Framework;

using MonoMod.RuntimeDetour;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;

using TerrariaApi.Server;

using TShockAPI;
using TShockAPI.Hooks;

using VBY.Common;
using VBY.Common.Config;
using VBY.Common.Hook;
using VBY.Common.Loader;
using VBY.GameContentModify.Config;
using VBY.GameContentModify.ID;

namespace VBY.GameContentModify;
[ApiVersion(2, 1)]
public partial class GameContentModify : CommonPlugin
{
    public override string Name => "VBY.GameContentModify";
    public override string Author => "yu";
    public override string Description => "一些游戏内容的修改 For Terraria v1.4.4.9";
    public static bool Debug = false;
    public static event Action<ReloadEventArgs, MainConfigInfo>? PostReload;
    public static event Action<MainConfigInfo>? PreStartDay;
    public static event Action<MainConfigInfo>? PreStartNight;
    public static ConfigManager<MainConfigInfo> MainConfig = new(Strings.ConfigDirectory, Strings.MainConfigFileName, () => new()) { PostLoadAction = OnPostLoad };

    private static void OnPostLoad(MainConfigInfo info, TSPlayer? player, bool first)
    {
        if (info.Spawn.TownNPC.DisableSpawnTownNPC.Length > 0)
        {
            var list = new HashSet<int>();
            foreach (var type in info.Spawn.TownNPC.DisableSpawnTownNPC)
            {
                if (ReplaceMain.TownNPCIDIndexMap.TryGetValue(type, out var index))
                {
                    list.Add(index);
                }
                else
                {
                    player?.SendInfoMessage($"无效NPCID: {type}");
                }
            }
            ReplaceMain.DisableSpawnTownNPCIndices = [.. list];
        }
    }

    public static ConfigManager<ChestSpawnInfo[]> ChestSpawnConfig = new(Strings.ConfigDirectory, Strings.ChestSpawnConfigFileName, () =>
    [
        new ChestSpawnNPCInfo(ItemID.LightKey, NPCID.BigMimicHallow),
        new ChestSpawnNPCInfo(ItemID.NightKey, NPCID.BigMimicCorruption, NPCID.BigMimicCrimson),
        new ChestSpawnNPCInfo(ItemID.GoldenKey, NPCID.Mimic){ ItemStack = 3 }
    ]) { Converter = new ChestSpawnConverter() };
#pragma warning disable format
    public static ConfigManager<ItemTransformConfig> ItemTrasnfromConfig = new(Strings.ConfigDirectory, Strings.ItemTrasnfromConfigFileName, () =>
    {
        var config = new ItemTransformConfig();
        config.TransformInfos.AddRange(new ItemTransformInfo[]
        {
            new(ItemID.RodofDiscord,            ItemID.RodOfHarmony,            ProgressQueryID.Moonlord),
            new(ItemID.Clentaminator,           ItemID.Clentaminator2,          ProgressQueryID.Moonlord),
            new(ItemID.BottomlessBucket,        ItemID.BottomlessShimmerBucket, ProgressQueryID.Moonlord){ mutual = true },
            new(ItemID.JungleKey,               ItemID.PiranhaGun,              ProgressQueryID.PlantBoss),
            new(ItemID.CorruptionKey,           ItemID.ScourgeoftheCorruptor,   ProgressQueryID.PlantBoss),
            new(ItemID.CrimsonKey,              ItemID.VampireKnives,           ProgressQueryID.PlantBoss),
            new(ItemID.HallowedKey,             ItemID.RainbowGun,              ProgressQueryID.PlantBoss),
            new(ItemID.FrozenKey,               ItemID.StaffoftheFrostHydra,    ProgressQueryID.PlantBoss),
            new(ItemID.DungeonDesertKey,        ItemID.StormTigerStaff,         ProgressQueryID.PlantBoss)
        }.Select(x => x.ToString()).Cast<object>());
        return config;
    }) { SerializerSettings = new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore } };
    public static ConfigManager<MemberSetterInfo> MemberSetterConfig = new(Strings.ConfigDirectory, Strings.MemberSetterFileName, () => new MemberSetterInfo()
    {
        MemberName = new() 
        { 
            { "可掉落图格", JToken.Parse("""{ "Name": "Terraria.ID.TileID+Sets.Falling", "Tip": "可掉落图格，可以用来把沙子一类设置为false来防止掉落，不要给原本不可掉落的设置为true，那些并没有相应弹幕承载" }""") },
            { "物品免疫熔岩摧毁", "Terraria.ID.ItemID+Sets.IsLavaImmuneRegardlessOfRarity" },
            { "检测宽度", JToken.Parse("""{ "Name": "Terraria.NPC.sWidth", "Tip": "本意是屏幕宽度，tr用此来计算怪物生成，npc传送回家之类需要不在玩家屏幕内的操作" }""") },
            { "检测高度", "Terraria.NPC.sHeight" },
        }
    });
#pragma warning restore format
    private static readonly List<Hook> Hooks = new();
    internal static readonly ReadOnlyDictionary<string, Hook> NamedHooks = new(new Dictionary<string, Hook>()
    {
        { DetourNames.Item_CheckLavaDeath, Utils.GetHook(ReplaceItem.CheckLavaDeath) },
        { DetourNames.Item_MechSpawn, Utils.GetHook(ReplaceItem.MechSpawn) },
        //{ DetourNames.MessageBuffer_GetData, Utils.GetDetour(ReplaceMessageBuffer.GetData) },
        { DetourNames.Liquid_DelWater, Utils.GetHook(ReplaceLiquid.DelWater) },
        //{ DetourNames.Main_UpdateTime_SpawnTownNPCs, Utils.GetDetour(ReplaceMain.UpdateTime_SpawnTownNPCs) },
        { DetourNames.NetMessage_orig_SendData, Utils.GetHook(ReplaceNetMessage.orig_SendData) },
        { DetourNames.NPC_CountKillForBannersAndDropThem, Utils.GetHook(ReplaceNPC.CountKillForBannersAndDropThem) },
        { DetourNames.NPC_MechSpawn, Utils.GetHook(ReplaceNPC.MechSpawn) },
        { DetourNames.NPC_SpawnNPC, Utils.GetHook(ReplaceNPC.SpawnNPC) },
        { DetourNames.NPC_TransformElderSlime, Utils.GetHook(ReplaceNPC.TransformElderSlime) },
        { DetourNames.ObjectData_TileObjectData_GetTileData, Utils.GetParamHook(ObjectData.ReplaceTileObjectData.GetTileData) },
        { DetourNames.Player_Shellphone_Spawn, Utils.GetHook(ReplacePlayer.Shellphone_Spawn) },
        //{ DetourNames.Wiring_HitWireSingle, Utils.GetDetour(ReplaceWiring.HitWireSingle) },
        { DetourNames.WorldGen_ShakeTree, Utils.GetHook(ReplaceWorldGen.ShakeTree) },
        { DetourNames.WorldGen_GrowAlch, Utils.GetHook(ReplaceWorldGen.GrowAlch) },
        { DetourNames.WorldGen_SpawnThingsFromPot, Utils.GetHook(ReplaceWorldGen.SpawnThingsFromPot) },
        { DetourNames.WorldGen_ScoreRoom, Utils.GetHook(ReplaceWorldGen.ScoreRoom) },
        { DetourNames.WorldGen_IsHarvestableHerbWithSeed, Utils.GetHook(ReplaceWorldGen.IsHarvestableHerbWithSeed) },
        { DetourNames.WorldGen_UpdateWorld_OvergroundTile, Utils.GetHook(ReplaceWorldGen.UpdateWorld_OvergroundTile) },
        { DetourNames.WorldGen_CheckJunglePlant, Utils.GetHook(ReplaceWorldGen.CheckJunglePlant) },
    });
    internal static readonly ReadOnlyDictionary<string, ActionHook> NamedActionHooks = new(new Dictionary<string, ActionHook>()
    {
        { ActionHookNames.Main_UpdateTime, new ActionHook(static () => On.Terraria.Main.UpdateTime += SpawnInfo.TownNPCInfo.OnMain_UpdateTime) },
        { ActionHookNames.NPC_DoDeathEvents, new ActionHook(static () => On.Terraria.NPC.DoDeathEvents += ExtensionInfo.OnNPC_DoDeathEvents) },
        { ActionHookNames.NPC_NewNPC, new ActionHook(static () => On.Terraria.NPC.NewNPC += ExtensionInfo.OnNPC_NewNPC) },
        { ActionHookNames.Projectile_AI, new ActionHook(static () => On.Terraria.Projectile.AI += MainConfigInfo.OnProjectile_AI) },
        { ActionHookNames.Projectile_Kill, new ActionHook(static () => On.Terraria.Projectile.Kill += MainConfigInfo.OnProjectile_Kill) },
        { ActionHookNames.WorldGen_CheckSpecialTownNPCSpawningConditions, new ActionHook(static() => On.Terraria.WorldGen.CheckSpecialTownNPCSpawningConditions += SpawnInfo.TownNPCInfo.OnCheckSpecialTownNPCSpawningConditions) }
    });
    static GameContentModify()
    {
        RegisterDetours(typeof(ReplaceMain), typeof(ReplaceMessageBuffer), typeof(ReplaceItem), typeof(ReplaceNPC), typeof(ReplaceProjectile), typeof(ReplaceWiring), typeof(ReplaceWorldGen), typeof(GameContent.ReplaceTeleportPylonsSystem), typeof(ReplacePlayer));
    }
    public GameContentModify(Main game) : base(game)
    {
        AddCommands.Add(new Command("gcm.ctl", Cmd, "gcm"));
        AttachHooks.Add(new ActionHook(static () => GeneralHooks.ReloadEvent += OnTShockReload));
        Loaders.Add(Hooks.GetLoader(static x => x.Apply(), static x => x.Dispose(), static () => Main.versionNumber == "v1.4.4.9"));
        Loaders.Add(NamedHooks.GetLoader(static x => x.Value.Apply(), static x => x.Value.Dispose(), null, false, true));
        Loaders.Add(NamedActionHooks.GetLoader(static x => x.Value.Register(), static x => x.Value.Unregister(), null, false, true));
        PreStartDay += config =>
        {
            if (ExtensionInfo.StaticTravelNPCRefreshOnStartDay)
            {
                Chest.SetupTravelShop();
                NetMessage.SendTravelShop(-1);
            }
        };
        //On.Terraria.NPC.NewNPC += OnNPC_NewNPC;
        AttachOnPostInitializeHook(OnGamePostInitialize);
    }

    //private int OnNPC_NewNPC(On.Terraria.NPC.orig_NewNPC orig, Terraria.DataStructures.IEntitySource source, int X, int Y, int Type, int Start, float ai0, float ai1, float ai2, float ai3, int Target)
    //{
    //    if(Type == NPCID.Bee)
    //    {
    //        Console.WriteLine(new System.Diagnostics.StackTrace());
    //    }
    //    return orig(source, X, Y, Type, Start, ai0, ai1, ai2, ai3, Target);
    //}
    private static void RegisterDetours(params Type[] types)
    {
        foreach (var type in types) {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            var replaceType = type.GetCustomAttribute<ReplaceTypeAttribute>()!.Type;
            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<DetourMethodAttribute>();
                if(attr is null)
                {
                    continue;
                }
                if (attr.UseParam)
                {
                    Hooks.Add(Utils.GetParamHook(replaceType, method));
                }
                else
                {
                    Hooks.Add(Utils.GetNameHook(replaceType, method));
                }
            }
        }
    }
    protected override void PreInitialize() => LoadConfig(TSPlayer.Server);

    protected override void PreDispose(bool disposing)
    {
        if (disposing)
        {
            MainConfigInfo.Reset();
            ShimmerItemReplaceInfo.Reset();
            MemberSetterInfo.RestoreValue();
        }
    }
    #region On
    [AutoHook]
    public static void OnProjectile_ExplodeTiles(On.Terraria.Projectile.orig_ExplodeTiles orig, Projectile self, Vector2 compareSpot, int radius, int minI, int maxI, int minJ, int maxJ, bool wallSplode)
    {
        if (MainConfig.Instance.DisableProjectile_ExplodeTilesIDs.Contains(self.type))
        {
            return;
        }
        orig(self, compareSpot, radius, minI, maxI, minJ, maxJ, wallSplode);
    }
    [AutoHook]
    public static void OnNetManager_SendData(On.Terraria.Net.NetManager.orig_SendData orig, Terraria.Net.NetManager self, Terraria.Net.Sockets.ISocket socket, Terraria.Net.NetPacket packet)
    {
        if (ExtensionInfo.NotSendNetPacketIDs.Contains(packet.Id))
        {
            return;
        }
        orig(self, socket, packet);
    }
    [AutoHook]
    public static void OnWorldGen_StartImpendingDoom(On.Terraria.WorldGen.orig_StartImpendingDoom orig, int countdownTime)
    {
        if(countdownTime == 720)
        {
            countdownTime = SpawnInfo.StaticMoonLordCountdownOfSummon;
        }
        else if(countdownTime == 3600)
        {
            countdownTime = SpawnInfo.StaticMoonLordCountdownOfTowerKilled;
        }
        orig(countdownTime);
    }
    [AutoHook]
    public static void ChatHelper_BroadcastChatMessageAs(On.Terraria.Chat.ChatHelper.orig_BroadcastChatMessageAs orig, byte messageAuthor, Terraria.Localization.NetworkText text, Color color, int excludedPlayer)
    {
        if (text._mode == Terraria.Localization.NetworkText.Mode.LocalizationKey && MainConfig.Instance.Extension.NotSendNetworkTextKeys.Contains(text._text))
        {
            return;
        }
        orig(messageAuthor, text, color, excludedPlayer);
    }
    private static void OnTShockReload(ReloadEventArgs e)
    {
        LoadConfig(e.Player);
        e.Player.SendSuccessMessage("[VBY.GameContentModify]重载完成");
        OnPostReload(e);
    }

    private static void OnPostReload(ReloadEventArgs e) => PostReload?.Invoke(e, MainConfig.Instance);
    //private void OnGamePostInitialize(EventArgs e) => MainConfig.Instance.LoadNotSendPacketID();
    private static void OnGamePostInitialize(EventArgs e)
    {
        MainConfig.Instance.Extension.LoadNotSendPacketID();
        MainConfig.Instance.Extension.LoadIgnoreNPCs();
        ExtensionInfo.SetIgnore();
        MainConfig.Instance.Liquid.Restore();
        MainConfig.Instance.Liquid.SetValue();
        MemberSetterConfig.Instance.Resolve();
        MemberSetterConfig.Instance.SetValue();
        PreStartDay = null;
        PreStartNight = null;
    }
    #endregion
    internal static void OnPreStartDay() => PreStartDay?.Invoke(MainConfig.Instance);
    internal static void OnPreStartNight() => PreStartNight?.Invoke(MainConfig.Instance);
    private static int ShowRuleID;
    internal void Cmd(CommandArgs args)
    {
        var enumerator = args.Parameters.GetEnumerator();
        var player = args.Player;
        if (!enumerator.MoveNext())
        {
            var name = AddCommands[0].Name;
            player.SendInfoMessage($"/{name} show main [-nd] [-code]查看主配置文件");
            player.SendInfoMessage($"/{name} show chest 查看箱子文件");
            player.SendInfoMessage($"/{name} set <属性> <值> 设置主配置的值");
            player.SendInfoMessage($"/{name} memberrestore 还原所有MemberSetter设置的值");
            player.SendInfoMessage($"/{name} save 保存配置");
            player.SendInfoMessage($"/{name} debug 切换调试信息显示");
            return;
        }
        switch (enumerator.Current.ToLowerInvariant())
        {
            case "show":
                if(!enumerator.MoveNext())
                {
                    player.SendInfoMessage($"/{AddCommands[0].Name} show main 查看主配置文件");
                    player.SendInfoMessage($"/{AddCommands[0].Name} show chest 查看箱子文件");
                    break;
                }
                switch (enumerator.Current)
                {
                    case "main":
                        //ShowConfig(player, null, typeof(MainConfigInfo), MainConfig.Instance, args.Parameters.Contains("-nd"), MainConfig.GetDefaultFunc(), !args.Parameters.Contains("-code"));
                        object target = MainConfig.Instance;
                        string? baseName = null;
                        var useDescription = !args.Parameters.Contains("-code");
                        if (enumerator.MoveNext())
                        {
                            var name = enumerator.Current;
                            if (!name.Contains('-'))
                            {
                                if (name.Contains('.'))
                                {
                                    name = name[..name.IndexOf('.')];
                                }
                                if (!string.IsNullOrEmpty(name))
                                {
                                    var type = target.GetType();
                                    var members = type.GetMember(name, MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public);
                                    if (members.Length == 1)
                                    {
                                        var member = members[0];
                                        target = Utils.GetFieldOrPropertyValue(member, target)!;
                                        baseName = (useDescription ? Utils.GetFieldOrPropertyType(member).GetCustomAttribute<DescriptionAttribute>()?.Description : null) ?? member.Name;
                                    }
                                }
                            }
                        }
                        ShowConfig(player, baseName, target.GetType(), target, args.Parameters.Contains("-nd"), useDescription);
                        break;
                    case "chest":
                        player.SendInfoMessage(JsonConvert.SerializeObject(ChestSpawnConfig.Instance, Formatting.Indented));
                        break;
                }
                break;
            case "set":
                if (!enumerator.MoveNext())
                {
                    player.SendInfoMessage($"/{AddCommands[0].Name} set <属性> <值> 设置主配置的值");
                    break;
                }
                var propertyName = enumerator.Current;
                if (!enumerator.MoveNext())
                {
                    player.SendInfoMessage("请输入值");
                    break;
                }
                var value = enumerator.Current;
                if (Utils.SetMemberValue(player, MainConfig.Instance, propertyName, propertyName, value))
                {
                    //MainConfig.Instance.Extension.LoadNotSendPacketID();
                    //MainConfig.Instance.Extension.LoadIgnoreNPCs();
                    //ExtensionInfo.SetIgnore();
                }
                break;
            case "memberrestore":
                MemberSetterInfo.RestoreValue();
                player.SendInfoMessage("还原完成");
                break;
            case "save":
                MainConfig.Save();
                player.SendInfoMessage("保存成功");
                break;
            case "debug":
                Debug = !Debug;
                args.Player.SendInfoMessage($"Debug => {Debug}");
                break;
            case "showrule":
                if (!enumerator.MoveNext())
                {
                    args.Player.SendInfoMessage("global|<npcid>");
                    break;
                }
                const string tabString = "  ";
                if (enumerator.Current.Equals("global", StringComparison.OrdinalIgnoreCase))
                {
                    if (!enumerator.MoveNext())
                    {
                        args.Player.SendInfoMessage("当前有 {0} 个全局规则", Main.ItemDropsDB._globalEntries.Count);
                        break;
                    }
                    if(int.TryParse(enumerator.Current, out var index))
                    {
                        if (index < 1 || index > Main.ItemDropsDB._globalEntries.Count)
                        {
                            args.Player.SendInfoMessage("无效index");
                            break;
                        }
                        index--;
                        var rules = Main.ItemDropsDB._globalEntries;
                        Utils.hasUnknown = false;
                        if (args.Player == TSPlayer.Server)
                        {
                            using var tw = new System.CodeDom.Compiler.IndentedTextWriter(Console.Out, tabString);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            tw.Write("规则{0}: ", index + 1);
                            tw.Indent++;
                            var rule = rules[index];
                            Utils.WriteItemDropRuleInfo(tw, rule);
                            tw.Indent--;
                            Console.ResetColor();
                        }
                        else
                        {
                            var sb = new StringBuilder();
                            using var tw = new System.CodeDom.Compiler.IndentedTextWriter(new StringWriter(sb), tabString);
                            tw.Write("规则{0}: ", index + 1);
                            tw.Indent++;
                            var rule = rules[index];
                            Utils.WriteItemDropRuleInfo(tw, rule);
                            tw.Indent--;
                            args.Player.SendInfoMessage(sb.ToString());
                        }
                        if (Utils.hasUnknown)
                        {
                            args.Player.SendErrorMessage("hasUnknown");
                        }
                    }
                    else
                    {
                        args.Player.SendInfoMessage("无效数字");
                    }
                }
                else
                {
                    var netID = -1;
                    var fromNext = false;
                    if (enumerator.Current.Equals("next", StringComparison.OrdinalIgnoreCase))
                    {
                        ShowRuleID++;
                        if (ShowRuleID >= NPCID.Count)
                        {
                            ShowRuleID = NPCID.Count - 1;
                            args.Player.SendInfoMessage("已经是最后一个ID了");
                        }
                        fromNext = true;
                        netID = ShowRuleID;
                    }
                    if(netID == -1)
                    {
                        if (int.TryParse(enumerator.Current, out netID))
                        {
                            ShowRuleID = netID;
                        }
                        else
                        {
                            args.Player.SendInfoMessage("无效数字");
                            break;
                        }
                    }
                    if (Main.ItemDropsDB._entriesByNpcNetId.TryGetValue(netID, out var rules))
                    {
                        Utils.hasUnknown = false;
                        if (args.Player == TSPlayer.Server)
                        {
                            using var tw = new System.CodeDom.Compiler.IndentedTextWriter(Console.Out, tabString);
                            for (int i = 0; i < rules.Count; i++)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                tw.Write("规则{0}: ", i + 1);
                                tw.Indent++;
                                var rule = rules[i];
                                Utils.WriteItemDropRuleInfo(tw, rule);
                                tw.Indent--;
                                Console.ResetColor();
                            }
                        }
                        else
                        {
                            var sb = new StringBuilder();
                            using var tw = new System.CodeDom.Compiler.IndentedTextWriter(new StringWriter(sb), tabString);

                            for (int i = 0; i < rules.Count; i++)
                            {
                                tw.Write("规则{0}: ", i + 1);
                                tw.Indent++;
                                var rule = rules[i];
                                Utils.WriteItemDropRuleInfo(tw, rule);
                                tw.Indent--;
                                args.Player.SendInfoMessage(sb.ToString());
                            }
                        }
                        if (fromNext)
                        {
                            args.Player.SendInfoMessage("当前ID: {0}", netID);
                        }
                        if (Utils.hasUnknown)
                        {
                            args.Player.SendErrorMessage("hasUnknown");
                        }
                    }
                    else
                    {
                        args.Player.SendInfoMessage("找不到 netID={0} 的规则", netID);
                    }
                }
                break;
        }
    }
    internal static void LoadConfig(TSPlayer player)
    {
        MainConfig.Load(player);

        var ids = new List<byte>();
        foreach(var id in MainConfig.Instance.World.GrowLifeFruitRequireProgressIDs)
        {
            if (ShimmerItemReplaceInfo.DownedFuncs.IndexInRange(id))
            {
                ids.Add((byte)id);
            }
            else if (Debug)
            {
                Console.WriteLine($"'{id}' out of range 0-{ShimmerItemReplaceInfo.DownedFuncs.Length}");
            }
        }
        WorldInfo.StaticGrowLifeFruitRequireProgressIDs = ids.ToArray();
        Utils.HandleNamedHook(!Utils.MembersValueAllEqualDefault(MainConfig.Instance.Mech, nameof(MechInfo.NPCSpawnLimitOfRange200), nameof(MechInfo.NPCSpawnLimitOfRange600), nameof(MechInfo.NPCSpawnLimitOfWorld)), DetourNames.NPC_MechSpawn);
        Utils.HandleNamedHook(!Utils.MembersValueAllEqualDefault(MainConfig.Instance.Mech, nameof(MechInfo.ItemSpawnLimitUseStack), nameof(MechInfo.ItemSpawnLimitOfRange300), nameof(MechInfo.ItemSpawnLimitOfRange800), nameof(MechInfo.ItemSpawnLimitOfWorld)), DetourNames.Item_MechSpawn);
        Utils.HandleNamedHook(BitConverter.IsLittleEndian && Main.versionNumber == "v1.4.4.9" && MainConfig.Instance.NetMessage.EnableSendMessageOptimization, DetourNames.NetMessage_orig_SendData);

        ChestSpawnConfig.Load(player);
        ItemTrasnfromConfig.Load(player);
        ShimmerItemReplaceInfo.Reset();
        ShimmerItemReplaceInfo.Load(ItemTrasnfromConfig.Instance);
        MemberSetterConfig.Load(player);
        if(Main.netMode == 2)
        {
            OnGamePostInitialize(EventArgs.Empty);
        }
    }
    //internal static void ShowConfig(TSPlayer player, string? baseName, Type type, object target, bool noSendDefault, object? defaultValue = null, bool useDescription = true)
    internal static void ShowConfig(TSPlayer player, string? baseName, Type type, object target, bool noSendDefault, bool useDescription = true)
    {
        foreach (var member in type.GetMembers().Where(x => x is FieldInfo { IsStatic: false } || x is PropertyInfo { GetMethod.IsStatic: false }))
        {
            var memberType = Utils.GetFieldOrPropertyType(member, out var getFunc, out var setFunc);
            if (memberType.IsValueType)
            {
                string sendStr;
                var descAttr = member.GetCustomAttribute<DescriptionAttribute>();
                if (useDescription && descAttr is not null)
                {
                    sendStr = string.Format(descAttr.Description, getFunc(target));
                }
                else
                {
                    sendStr = $"{member.Name}: {getFunc(target)}";
                }
                if (!string.IsNullOrEmpty(baseName))
                {
                    sendStr = $"{baseName}.{sendStr}";
                }
                var defaultValue = member.GetCustomAttribute<DefaultValueAttribute>()!.Value;
                //if(noSendDefault && defaultValue is not null && getFunc(defaultValue)!.Equals(getFunc(target)))
                if(noSendDefault && defaultValue!.Equals(getFunc(target)))
                {
                    continue;
                }
                player.SendInfoMessage(sendStr);
            }
            else if (memberType.IsArray)
            {
                var send = true;
                string sendStr = "";
                if (memberType == typeof(int[]))
                {
                    ShowConfigArray<int>(baseName, member, memberType, getFunc, target, noSendDefault, useDescription, ref send, ref sendStr);
                }
                else if(memberType == typeof(string[]))
                {
                    ShowConfigArray<string>(baseName, member, memberType, getFunc, target, noSendDefault, useDescription, ref send, ref sendStr);
                }
                else
                {
                    sendStr = useDescription && member.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute d ? $"{d.Description}: 隐藏(懒得写)" : $"{member.Name}: 隐藏(懒得写)";
                    if (!string.IsNullOrEmpty(baseName))
                    {
                        sendStr = $"{baseName}.{sendStr}";
                    }
                }
                if (send)
                {
                    player.SendInfoMessage(sendStr);
                }
            }
            else
            {
                if (memberType.IsAssignableTo(typeof(System.Collections.IDictionary)))
                {
                    continue;
                }
                //ShowConfig(player, $"{(string.IsNullOrEmpty(baseName) ? "" : baseName + ".")}{(useDescription ? (memberType.GetCustomAttribute<DescriptionAttribute>()?.Description ?? member.Name) : member.Name)}", memberType, getFunc(target)!, noSendDefault, defaultValue is null ? null : getFunc(defaultValue)!, useDescription);
                ShowConfig(player, $"{(string.IsNullOrEmpty(baseName) ? "" : baseName + ".")}{(useDescription ? (memberType.GetCustomAttribute<DescriptionAttribute>()?.Description ?? member.Name) : member.Name)}", memberType, getFunc(target)!, noSendDefault, useDescription);
            }
        }
    }
    internal static void ShowConfigArray<T>(string? baseName, MemberInfo member,Type memberType, Func<object?, object?> getFunc,object target, bool noSendDefault, bool useDescription , ref bool send, ref string sendStr)
    {
        sendStr = useDescription && member.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute d
            ? $"{d.Description}: [{string.Join(", ", (T[])getFunc(target)!)}]"
            : $"{member.Name}: [{string.Join(", ", (T[])getFunc(target)!)}]";
        if (!string.IsNullOrEmpty(baseName))
        {
            sendStr = $"{baseName}.{sendStr}";
        }
        var defaultValueAttr = memberType.GetCustomAttribute<DefaultValueAttribute>();
        //if (noSendDefault && defaultValue is not null && JsonConvert.SerializeObject(getFunc(defaultValue)) == JsonConvert.SerializeObject(getFunc(target)))
        //{
        //    send = false;
        //}
        var arr = (T[])getFunc(target)!;
        if (noSendDefault)
        {
            if (defaultValueAttr is null)
            {
                if (arr.Length == 0)
                {
                    send = false;
                }
            }
            else if (arr.Length == 1 && defaultValueAttr.Value!.Equals(arr[0]))
            {
                send = false;
            }
        }
    }
}