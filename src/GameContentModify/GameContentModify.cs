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

using VBY.GameContentModify.Config;
using VBY.GameContentModify.ID;

namespace VBY.GameContentModify;
[ApiVersion(2, 1)]
public partial class GameContentModify : TerrariaPlugin
{
    public override string Name => "VBY.GameContentModify";
    public override string Author => "yu";
    public override string Description => "一些游戏内容的修改 For Terraria v1.4.4.9";
    public override Version Version => GetType().Assembly.GetName().Version!;
    public static event Action<ReloadEventArgs, MainConfigInfo>? PostReload;
    public static event Action<MainConfigInfo>? PreStartDay;
    public static event Action<MainConfigInfo>? PreStartNight;
    public static ConfigBase<MainConfigInfo> MainConfig = new(Strings.ConfigDirectory, Strings.MainConfigPath, () => new());
    public static ConfigBase<ChestSpawnInfo[]> ChestSpawnConfig = new(Strings.ConfigDirectory, Strings.ChestSpawnConfigPath, () => new ChestSpawnInfo[]
    {
        new ChestSpawnNPCInfo(ItemID.LightKey, NPCID.BigMimicHallow),
        new ChestSpawnNPCInfo(ItemID.NightKey, NPCID.BigMimicCorruption, NPCID.BigMimicCrimson),
        new ChestSpawnNPCInfo(ItemID.GoldenKey, NPCID.Mimic){ ItemStack = 3 }
    }) { Converter = new ChestSpawnConverter() };
    public static ConfigBase<ItemTransformInfo[]> ItemTrasnfromConfig = new(Strings.ConfigDirectory, Strings.ItemTrasnfromConfigPath, () => new ItemTransformInfo[]
    {
        new(ItemID.RodofDiscord,            ItemID.RodOfHarmony,            ProgressQueryID.Moonlord),
        new(ItemID.Clentaminator,           ItemID.Clentaminator2,          ProgressQueryID.Moonlord),
        new(ItemID.BottomlessBucket,        ItemID.BottomlessShimmerBucket, ProgressQueryID.Moonlord){ mutual = true },
        //new(ItemID.BottomlessShimmerBucket, ItemID.BottomlessBucket,        ProgressQueryID.Moonlord),
        new(ItemID.JungleKey,               ItemID.PiranhaGun,              ProgressQueryID.PlantBoss),
        new(ItemID.CorruptionKey,           ItemID.ScourgeoftheCorruptor,   ProgressQueryID.PlantBoss),
        new(ItemID.CrimsonKey,              ItemID.VampireKnives,           ProgressQueryID.PlantBoss),
        new(ItemID.HallowedKey,             ItemID.RainbowGun,              ProgressQueryID.PlantBoss),
        new(ItemID.FrozenKey,               ItemID.StaffoftheFrostHydra,    ProgressQueryID.PlantBoss),
        new(ItemID.DungeonDesertKey,        ItemID.StormTigerStaff,         ProgressQueryID.PlantBoss)
    }) { SerializerSettings = new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore } };
    internal Command AddCommand;
    private static readonly List<Detour> Detours = new()
    {
        Utils.GetDetour<Main>(ReplaceMain.UpdateTime),
        Utils.GetDetour<Main>(ReplaceMain.UpdateTime_StartDay),
        Utils.GetDetour<Main>(ReplaceMain.UpdateTime_StartNight),
        Utils.GetDetour<NPC>(ReplaceNPC.BigMimicSummonCheck),
        Utils.GetDetour<Item>(ReplaceItem.CanShimmer),
        Utils.GetDetour<Item>(ReplaceItem.GetShimmered),
        Utils.GetDetour<Projectile>(ReplaceProjectile.GasTrapCheck),
        Utils.GetDetour<WorldGen>(ReplaceWorldGen.UpdateWorld),
        Utils.GetDetour<WorldGen>(ReplaceWorldGen.hardUpdateWorld),
        Utils.GetDetour<WorldGen>(ReplaceWorldGen.CheckOrb),
        Utils.GetDetour<NetMessage>(ReplaceNetMessage.orig_SendData)
    };
    public GameContentModify(Main game) : base(game)
    {
        AddCommand = new Command(Cmd, "gcm");
        MainConfig.Instance.LoadToStatic();
    }
    public override void Initialize()
    {
        Commands.ChatCommands.Add(AddCommand);

        MainConfig.Load(TSPlayer.Server);
        ChestSpawnConfig.Load(TSPlayer.Server);
        ItemTrasnfromConfig.Load(TSPlayer.Server);

        if (MainConfig.Instance.BoundTownSlimeOldSpawnAtUnlock)
        {
            Detours.Add(Utils.GetDetour<NPC>(ReplaceNPC.TransformElderSlime));
            Detours.Add(Utils.GetDetour<NPC>(ReplaceNPC.SpawnNPC));
        }
        if (MainConfig.Instance.DisableQueenBeeAndBeeHurtOtherNPC)
        {
            Detours.Add(Utils.GetDetour<NPC>(ReplaceNPC.UpdateNPC));
        }

        ShimmerItemReplaceInfo.Reset();
        ShimmerItemReplaceInfo.Load(ItemTrasnfromConfig.Instance);

        if (Main.versionNumber == "v1.4.4.9")
        {
            Detours.ForEach(x => x.Apply());
        }
        On.Terraria.Projectile.ExplodeTiles += OnProjectile_ExplodeTiles;
        if (SpawnInfo.TownNPCInfo.StaticSpawnAtNight)
        {
            On.Terraria.Main.UpdateTime += OnMain_UpdateTime;
        }
        GeneralHooks.ReloadEvent += OnTShockReload;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Commands.ChatCommands.Remove(AddCommand);
            Detours.ForEach(x => x.Dispose());
            On.Terraria.Projectile.ExplodeTiles -= OnProjectile_ExplodeTiles;
            //On.Terraria.Chat.ChatHelper.BroadcastChatMessage -= OnChatHelper_BroadcastChatMessage;
            if (MainConfig.Instance.Spawn.TownNPC.SpawnAtNight)
            {
                On.Terraria.Main.UpdateTime -= OnMain_UpdateTime;
            }
            GeneralHooks.ReloadEvent -= OnTShockReload;
            Utils.ClearOwner(OnProjectile_ExplodeTiles);
        }
        base.Dispose(disposing);
    }
    #region On
    private static void OnProjectile_ExplodeTiles(On.Terraria.Projectile.orig_ExplodeTiles orig, Projectile self, Vector2 compareSpot, int radius, int minI, int maxI, int minJ, int maxJ, bool wallSplode)
    {
        if (MainConfig.Instance.DisableProjectile_ExplodeTilesIDs.Contains(self.type))
        {
            return;
        }
        orig(self, compareSpot, radius, minI, maxI, minJ, maxJ, wallSplode);
    }
    private static void OnMain_UpdateTime(On.Terraria.Main.orig_UpdateTime orig)
    {
        orig();
        if (!Main.dayTime)
        {
            Main.UpdateTime_SpawnTownNPCs();
        }
    }
    private void OnTShockReload(ReloadEventArgs e)
    {
        MainConfig.Load(e.Player);
        MainConfig.Instance.LoadToStatic();
        ChestSpawnConfig.Load(e.Player);
        ItemTrasnfromConfig.Load(e.Player);
        ShimmerItemReplaceInfo.Reset();
        ShimmerItemReplaceInfo.Load(ItemTrasnfromConfig.Instance);

        e.Player.SendSuccessMessage("[VBY.GameContentModify]重载完成");
        OnPostReload(e);
    }
    private static void OnPostReload(ReloadEventArgs e)
    {
        PostReload?.Invoke(e, MainConfig.Instance);
    }
    #endregion
    internal static void OnPreStartDay() => PreStartDay?.Invoke(MainConfig.Instance);
    internal static void OnPreStartNight() => PreStartNight?.Invoke(MainConfig.Instance);
    internal void Cmd(CommandArgs args)
    {
        if(args.Parameters.Count == 0)
        {
            args.Player.SendInfoMessage($"/{AddCommand.Name} main 查看主配置文件");
            args.Player.SendInfoMessage($"/{AddCommand.Name} chest 查看箱子文件");
            return;
        }
        switch (args.Parameters[0])
        {
            case "main":
                ShowConfig(args.Player, "", typeof(MainConfigInfo), MainConfig.Instance);
                break;
            case "chest":
                args.Player.SendInfoMessage(JsonConvert.SerializeObject(ChestSpawnConfig.Instance, Formatting.Indented));
                break;
        }
    }
    internal static void ShowConfig(TSPlayer player, string baseName, Type type, object target)
    {
        foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
        {
            if (field.FieldType.IsValueType)
            {
                var sendStr = $"{field.GetCustomAttribute<DescriptionAttribute>()?.Description ?? field.Name}: {(field.FieldType == typeof(int) ? "1/" : "")}{field.GetValue(target)}";
                if (!string.IsNullOrEmpty(baseName))
                {
                    sendStr = $"{baseName}.{sendStr}";
                }
                player.SendInfoMessage(sendStr);
            }
            else if (field.FieldType.IsArray)
            {
                //type.GetElementType()
                if (field.FieldType == typeof(int[]))
                {
                    var sendStr = $"{field.GetCustomAttribute<DescriptionAttribute>()?.Description ?? field.Name}: [{string.Join(", ", (int[])field.GetValue(target)!)}]";
                    if (!string.IsNullOrEmpty(baseName))
                    {
                        sendStr = $"{baseName}.{sendStr}";
                    }
                    player.SendInfoMessage(sendStr);
                }
                else
                {
                    var sendStr = $"{field.GetCustomAttribute<DescriptionAttribute>()?.Description ?? field.Name}: 隐藏";
                    if (!string.IsNullOrEmpty(baseName))
                    {
                        sendStr = $"{baseName}.{sendStr}";
                    }
                    player.SendInfoMessage(sendStr);
                }
            }
            else
            {
                ShowConfig(player, $"{(string.IsNullOrEmpty(baseName) ? "" : baseName + ".")}{field.FieldType.GetCustomAttribute<DescriptionAttribute>()?.Description ?? field.Name}", field.FieldType, field.GetValue(target)!);
            }
        }
    }
}
public class NaturalSpawnNPCSpawnInfo
{
    public int X;
    public int Y;

    public NaturalSpawnNPCSpawnInfo(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int NewNPC(int type, int start = 0) => NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), X, Y, type, start);
}