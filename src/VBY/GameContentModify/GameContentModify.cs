using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using TerrariaApi.Server;

using TShockAPI;
using TShockAPI.Hooks;

using Newtonsoft.Json;

using MonoMod.RuntimeDetour;

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
    public static event Action<ReloadEventArgs, MainConfigInfo>? PostReload;
    public static event Action<MainConfigInfo>? PreStartDay;
    public static event Action<MainConfigInfo>? PreStartNight;
    public static ConfigManager<MainConfigInfo> MainConfig = new(Strings.ConfigDirectory, Strings.MainConfigFileName, () => new()) { PostLoadAction = MainConfigPostLoad };
    public static ConfigManager<ChestSpawnInfo[]> ChestSpawnConfig = new(Strings.ConfigDirectory, Strings.ChestSpawnConfigFileName, () => new ChestSpawnInfo[]
    {
        new ChestSpawnNPCInfo(ItemID.LightKey, NPCID.BigMimicHallow),
        new ChestSpawnNPCInfo(ItemID.NightKey, NPCID.BigMimicCorruption, NPCID.BigMimicCrimson),
        new ChestSpawnNPCInfo(ItemID.GoldenKey, NPCID.Mimic){ ItemStack = 3 }
    }) { Converter = new ChestSpawnConverter() };
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
#pragma warning restore format
    private static readonly List<Detour> Detours = new()
    {
        Utils.GetDetour(ReplaceMain.UpdateTime),
        Utils.GetDetour(ReplaceMain.UpdateTime_StartDay),
        Utils.GetDetour(ReplaceMain.UpdateTime_StartNight),
        Utils.GetDetour(ReplaceMain.Sundialing),
        Utils.GetDetour(ReplaceMain.Moondialing),
        Utils.GetDetour(ReplaceNPC.BigMimicSummonCheck),
        Utils.GetDetour(ReplaceNPC.UpdateNPC),
        Utils.GetDetour(ReplaceNPC.DoDeathEvents),
        Utils.GetDetour(ReplaceNPC.DoDeathEvents_AdvanceSlimeRain),
        Utils.GetDetour(ReplaceNPC.HitEffect),
        Utils.GetDetour(ReplaceItem.CanShimmer),
        Utils.GetDetour(ReplaceItem.GetShimmered),
        Utils.GetDetour(ReplaceProjectile.GasTrapCheck),
        Utils.GetDetour(ReplaceWorldGen.UpdateWorld),
        Utils.GetDetour(ReplaceWorldGen.hardUpdateWorld),
        Utils.GetDetour(ReplaceWorldGen.CheckOrb),
    };
    internal static readonly ReadOnlyDictionary<string, Detour> NamedDetours = new(new Dictionary<string, Detour>()
    {
        { DetourNames.Wiring_HitWireSingle, Utils.GetDetour(ReplaceWiring.HitWireSingle) },
        { DetourNames.WorldGen_ShakeTree, Utils.GetDetour(ReplaceWorldGen.ShakeTree) },
        { DetourNames.WorldGen_GrowAlch, Utils.GetDetour(ReplaceWorldGen.GrowAlch) },
        { DetourNames.WorldGen_SpawnThingsFromPot, Utils.GetDetour(ReplaceWorldGen.SpawnThingsFromPot) },
        { DetourNames.WorldGen_IsHarvestableHerbWithSeed, Utils.GetDetour(ReplaceWorldGen.IsHarvestableHerbWithSeed) }
    });
    static GameContentModify()
    {
        if (BitConverter.IsLittleEndian) 
        {
            Detours.Add(Utils.GetDetour(ReplaceNetMessage.orig_SendData));
        }
    }
    public GameContentModify(Main game) : base(game)
    {
        AddCommands.Add(new Command("gcm.ctl", Cmd, "gcm"));
        AttachHooks.Add(new ActionHook(() => GeneralHooks.ReloadEvent += OnTShockReload));
        Loaders.Add(Detours.GetLoader(x => x.Apply(), x => x.Dispose(), static () => Main.versionNumber == "v1.4.4.9"));
        Loaders.Add(NamedDetours.GetLoader(x => x.Value.Apply(), x => x.Value.Dispose(), static () => Main.versionNumber == "v1.4.4.9", false, true));
        AttachOnPostInitializeHook(OnGamePostInitialize);
    }
    protected override void PreInitialize() => LoadConfig(TSPlayer.Server);
    protected override void PreDispose(bool disposing) => MainConfigInfo.Reset();
    #region On
    [AutoHook]
    private static void OnProjectile_ExplodeTiles(On.Terraria.Projectile.orig_ExplodeTiles orig, Projectile self, Vector2 compareSpot, int radius, int minI, int maxI, int minJ, int maxJ, bool wallSplode)
    {
        if (MainConfig.Instance.DisableProjectile_ExplodeTilesIDs.Contains(self.type))
        {
            return;
        }
        orig(self, compareSpot, radius, minI, maxI, minJ, maxJ, wallSplode);
    }
    [AutoHook]
    private static void OnNetManager_SendData(On.Terraria.Net.NetManager.orig_SendData orig, Terraria.Net.NetManager self, Terraria.Net.Sockets.ISocket socket, Terraria.Net.NetPacket packet)
    {
        //if (MainConfigInfo.NotSendNetPacketIDs.Contains(packet.Id))
        if (ExtensionInfo.NotSendNetPacketIDs.Contains(packet.Id))
        {
            return;
        }
        orig(self, socket, packet);
    }
    private void OnTShockReload(ReloadEventArgs e)
    {
        LoadConfig(e.Player);
        e.Player.SendSuccessMessage("[VBY.GameContentModify]重载完成");
        OnPostReload(e);
    }
    private static void OnPostReload(ReloadEventArgs e) => PostReload?.Invoke(e, MainConfig.Instance);
    //private void OnGamePostInitialize(EventArgs e) => MainConfig.Instance.LoadNotSendPacketID();
    private void OnGamePostInitialize(EventArgs e) => MainConfig.Instance.Extension.LoadNotSendPacketID();
    #endregion
    internal static void OnPreStartDay() => PreStartDay?.Invoke(MainConfig.Instance);
    internal static void OnPreStartNight() => PreStartNight?.Invoke(MainConfig.Instance);
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
            player.SendInfoMessage($"/{name} save 保存配置");
            return;
        }
        switch (enumerator.Current)
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
                        ShowConfig(player, null, typeof(MainConfigInfo), MainConfig.Instance, args.Parameters.Contains("-nd"), !args.Parameters.Contains("-code"));
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
                    MainConfig.Instance.LoadToStatic();
                }
                break;
            case "save":
                MainConfig.Save();
                player.SendInfoMessage("保存成功");
                break;
        }
    }
    internal static void LoadConfig(TSPlayer player)
    {
        MainConfig.Load(player);
        MainConfig.Instance.LoadToStatic();
        ChestSpawnConfig.Load(player);
        ItemTrasnfromConfig.Load(player);
        ShimmerItemReplaceInfo.Reset();
        ShimmerItemReplaceInfo.Load(ItemTrasnfromConfig.Instance);
    }
    //internal static void ShowConfig(TSPlayer player, string? baseName, Type type, object target, bool noSendDefault, object? defaultValue = null, bool useDescription = true)
    internal static void ShowConfig(TSPlayer player, string? baseName, Type type, object target, bool noSendDefault, bool useDescription = true)
    {
        foreach (var member in type.GetMembers().Where(x => x is FieldInfo { IsStatic: false } || x is PropertyInfo { GetMethod.IsStatic: false }))
        {
            var memberType = Utils.GetMemberType(member, out var getFunc, out var setFunc);
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
    private static void MainConfigPostLoad(MainConfigInfo config, TSPlayer? player, bool first)
    {
        if (first)
        {
            if (MainConfig.Instance.BoundTownSlimeOldSpawnAtUnlock)
            {
                Detours.Add(Utils.GetDetour(ReplaceNPC.TransformElderSlime));
                Detours.Add(Utils.GetDetour(ReplaceNPC.SpawnNPC));
            }
            var netMessageInfo = MainConfig.Instance.NetMessage;
            if(netMessageInfo.SyncAllProjectile || netMessageInfo.SyncAllItem || netMessageInfo.SyncAllNPC)
            {
                Detours.Add(Utils.GetDetour(ReplaceMessageBuffer.GetData));
            }
        }
    }
}