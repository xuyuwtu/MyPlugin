using System.Data;
using System.Reflection;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using TerrariaApi.Server;

using TShockAPI;
using TShockAPI.DB;

using Newtonsoft.Json;

using VBY.Common;
using VBY.Common.CommandV2;
using VBY.Common.Hook;

namespace FlowerSeaRPG;

[ApiVersion(2, 1)]
public partial class FlowerSeaRPG : CommonPlugin
{
    public override string Name => "FlowerSeaRPG";
    public override Version Version => new(1, 0, 0, 1);
    public override string Author => "yu";
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    public static IDbConnection DB;
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    //public Command[] AddCommands;
    public static GradeInfo[] GradeInfos = Array.Empty<GradeInfo>();
    public static FSPlayer?[] Players = new FSPlayer[byte.MaxValue + 1];

    internal static Config MainConfig = new();
    internal static ChangeConfig ChangeConfig = new();

    private GetDataHandlerManager<GetDataHandlers.TileEditEventArgs> OnTileEditManager;
    private SubCmdRoot CtlCommand, CmdCommand;
    private Dictionary<int, TileRegionRecord> RegionRecord = new();
    private static Dictionary<int, NPCSpawnLineInfo> NPCSpawnLineInfos = new();
    private static Dictionary<int, EventDoCommandInfo> EventDoCommands = new();
    private List<IDisposable> Disposables = new();
    private bool LockSign = false;
    public FlowerSeaRPG(Main game) : base(game)
    {
        Main.ignoreErrors = false;
        OnTileEditManager = GetDataHandlers.TileEdit.GetHookManager(OnTileEdit, false);
        CmdCommand = new("FlowerSeaRPG","fs.cmd")
        {
            new SubCmdRun("Upgrade", "升级", CmdUpgrade, "up", "upgrade"),
            new SubCmdRun("Info", "查看信息", CmdInfo, "info"),
            new SubCmdRun("Shimmer", "开启微光转换武器", CmdShimmer, "sh", "shimmer"){ Enabled = false},
            new SubCmdRun("Add", "分配属性点", CmdAdd),
            new SubCmdRun("Refresh", "刷新武器", CmdRefresh, "re", "refresh")
        };
        CmdCommand.SetAllNode(new SetAllowInfo() { AllowServer = false }, x => x is SubCmdRun);
        CtlCommand = new("FlowerSeaRPGCtl", "fs.ctl")
        {
            new SubCmdRun("Reload", "重载", args => CtlReload(args.Player)),
            new SubCmdRun("Purity", "查看纯净度",args => args.Player.SendInfoMessage($"{WorldGen.tGood}%神圣 {WorldGen.tEvil}%腐化 {WorldGen.tBlood}%猩红")),
            new SubCmdRun("Signlock", "显示下一个编辑的标牌的坐标并临时锁定", args => LockSign = true),
            new SubCmdList("Region", "区域克隆")
            {
                new SubCmdRun("Set1", "设置一号点", args => CtlRegionSet(args.Player, 1)){ AllowInfo = new(allowServer: false) },
                new SubCmdRun("Set2", "设置二号点", args => CtlRegionSet(args.Player, 2)){ AllowInfo = new(allowServer: false) },
                new SubCmdRun("Set3", "设置特殊点", args => CtlRegionSet(args.Player, 3)){ AllowInfo = new(allowServer: false) },
                new SubCmdRun("Set12", "设置一二号点", args => CtlRegionSet(args.Player, 1,2)){ AllowInfo = new(allowServer: false) },
                new SubCmdRun("Load", "加载区域", CtlRegionLoad){ MinArgsCount = 1, ArgsHelpText = "<文件名>", AllowInfo = new(allowServer: false) },
                new SubCmdRun("Save", "保存区域", CtlRegionSave){ MinArgsCount = 1, ArgsHelpText = "<文件名>", AllowInfo = new(allowServer: false) },
                new SubCmdRun("ServerLoad", "服务器加载区域", args => 
                {
                    if(args.Parameters[0] == "config")
                    {
                        Utils.LoadRegionData(
                            ChangeConfig.PlaceTileX,
                            ChangeConfig.PlaceTileY,
                            ChangeConfig.PlaceFile[ChangeConfig.Left ? 0 : 1],
                            ChangeConfig.PlacePoint, false);
                    }
                    else
                    {
                        Utils.LoadRegionData(int.Parse(args.Parameters[0]), int.Parse(args.Parameters[1]), args.Parameters[2], args.Parameters[3],args.Parameters.Contains("-se"));
                    }
                }){ ArgsHelpText = "config|(<x> <y> <文件名> [point=lu] [-se])", MinArgsCount = 1},
                new SubCmdRun("Find","查找位置", CtlRegionFind),
                new SubCmdRun("SpecialAdd", "添加特殊点", CtlRegionSpecialadd){MinArgsCount = 1}
            },
            new SubCmdRun("Del", "删除玩家数据", CtlDel){MinArgsCount = 1},
            new SubCmdList("ChangeConfig", "change配置文件")
            {
                new SubCmdRun("Info", "查看配置信息", args => args.Player.SendInfoMessage(JsonConvert.SerializeObject(ChangeConfig,Formatting.Indented))),
                new SubCmdRun("Save", "保存ChangeConfig", args => ChangeConfig.Save())
            },
        };
        //AddCommands = new Command[] { CmdCommand.GetCommand(new string[] { "fs" }), CtlCommand.GetCommand(new string[] { "fsc" }) };
        AddCommands.Add(CmdCommand.GetCommand(new string[] { "fs" }));
        AddCommands.Add(CtlCommand.GetCommand(new string[] { "fsc" }));

        Directory.CreateDirectory(TShock.SavePath);
        if (!File.Exists(Strings.ConfigPath))
        {
            File.WriteAllText(Strings.ConfigPath, JsonConvert.SerializeObject(new Config(), Formatting.Indented));
        }
        if (!File.Exists(Strings.ChangePath))
        {
            File.WriteAllText(Strings.ChangePath, JsonConvert.SerializeObject(new ChangeConfig(), Formatting.Indented));
        }
        var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Strings.ConfigPath));
        if (config is null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("配置文件转换错误");
            Console.ResetColor();
            throw new Exception("配置文件转换错误");
        }
        MainConfig = config;
        DB = config.DBInfo.GetDbConnection();
        var creator = DB.GetTableCreator();
        foreach (var tableInfo in typeof(TableInfo).GetNestedTypes())
        {
            var members = tableInfo.GetMembers(BindingFlags.Instance | BindingFlags.Public);
            var columns = new List<SqlColumn>(members.Length);
            var instance = Activator.CreateInstance(tableInfo);
            var tableHeaders = new List<string>(members.Length);
            foreach (var member in tableInfo.GetMembers(BindingFlags.Instance | BindingFlags.Public).OrderBy(x => x.MetadataToken))
            {
                if (member.MemberType == MemberTypes.Field)
                {
                    columns.Add(new SqlColumn(member.Name, Utils.TypeToDbType[((FieldInfo)member).FieldType])
                    {
                        DefaultValue = ((FieldInfo)member)!.GetValue(instance)!.ToString(),
                        NotNull = true,
                        Primary = member.Name == "Name"
                    });
                    tableHeaders.Add(member.Name);
                }
            }
            creator.EnsureTableStructure(new SqlTable(tableInfo.Name, columns));
            Utils.TableHeaders[tableInfo.Name] = string.Join(',', tableHeaders);
            Utils.InsertStr[tableInfo.Name] = $"INSERT INTO {tableInfo.Name}({Utils.TableHeaders[tableInfo.Name]}) VALUES({string.Join(',', Enumerable.Range(0, tableHeaders.Count).Select(x => "@" + x))})";
            tableHeaders.Remove("Name");
            Utils.SelectStr[tableInfo.Name] = $"SELECT {Utils.TableHeaders[tableInfo.Name]} FROM {tableInfo.Name} WHERE Name = @0";
            Utils.UpdateStr[tableInfo.Name] = $"UPDATE {tableInfo.Name} SET {string.Join(',', tableHeaders.Select((value, index) => $"{value} = @{index + 1}"))} WHERE Name = @0";
        }
    }
    #region Initialize And Dispose
    protected override void PreInitialize()
    {
        Disposables.Add(OnTileEditManager);
        //Disposables.Add(GetDataHandlers.Sign.GetHookManager(OnSign));
        //Disposables.Add(ServerApi.Hooks.ServerJoin.GetHookManager(this, OnServerJoin));
        //Disposables.Add(ServerApi.Hooks.ServerLeave.GetHookManager(this, OnServerLevel));
        AttachHooks.Add(GetDataHandlers.Sign.GetHook(OnSign));
        //AttachHooks.Add(ServerApi.Hooks.ServerJoin.GetHook(this, OnServerJoin));
        AttachHooks.Add(new ActionHook(() => TShockAPI.Hooks.PlayerHooks.PlayerPostLogin += OnPlayerHooks_PlayerPostLogin));
        AttachHooks.Add(ServerApi.Hooks.ServerLeave.GetHook(this, OnServerLevel));
        Disposables.OfType<IHookManager>().Where(x => x.Init).ForEach(x => x.Initialize());
        CtlReload(TSPlayer.Server);
        //Commands.ChatCommands.AddRange(AddCommands);
        AttachHooks.Add(new MultiActionHook(() =>
        {
            On.Terraria.NPC.NewNPC += OnNPC_NewNPC;
            On.Terraria.NPC.SetDefaults += OnNPC_SetDefaults;
            On.Terraria.NPC.OnGameEventClearedForTheFirstTime += OnNPC_OnGameEventClearedForTheFirstTime;
            //On.Terraria.Item.CanShimmer += OnItem_CanShimmer;
            //On.Terraria.NetMessage.SendData += OnNetMessage_SendData;
            TShockAPI.Hooks.PlayerHooks.PlayerChat += OnPlayerChat;
            VBY.GameContentModify.GameContentModify.PreStartDay += OnPreStartDay;
            VBY.GameContentModify.GameContentModify.PreStartNight += OnPreStartNight;
        }));
    }

    private void OnPlayerHooks_PlayerPostLogin(TShockAPI.Hooks.PlayerPostLoginEventArgs e)
    {
        Players[e.Player.Index] = new FSPlayer(e.Player);
        Players[e.Player.Index]!.CheckAttributePoint();
    }

    protected override void PreDispose(bool disposing)
    {
        if (disposing)
        {
            Disposables.ForEach(x => x.Dispose());
            //Commands.ChatCommands.RemoveRange(AddCommands);
            //On.Terraria.NPC.NewNPC -= OnNPC_NewNPC;
            //On.Terraria.NPC.SetDefaults -= OnNPC_SetDefaults;
            //On.Terraria.NPC.OnGameEventClearedForTheFirstTime -= OnNPC_OnGameEventClearedForTheFirstTime;
            //On.Terraria.Item.CanShimmer -= OnItem_CanShimmer;
            //On.Terraria.NetMessage.SendData -= OnNetMessage_SendData;
            //TShockAPI.Hooks.PlayerHooks.PlayerChat -= OnPlayerChat;
            //VBY.GameContentModify.GameContentModify.PreStartDay -= OnPreStartDay;
            //VBY.GameContentModify.GameContentModify.PreStartNight -= OnPreStartNight;
            //Utils.ClearOwner(OnNPC_NewNPC);
            Array.Fill(GradeInfos, null);
            Array.Fill(Players, null);
        }
        //base.Dispose(disposing);
    }
    #endregion
    #region Hook
    //private void OnServerJoin(JoinEventArgs args)
    //{
    //    if (args.Who == 255)
    //    {
    //        return;
    //    }
    //    Players[args.Who] = new FSPlayer(TShock.Players[args.Who]);
    //    Players[args.Who].CheckAttributePoint();
    //}
    private void OnServerLevel(LeaveEventArgs args)
    {
        if (args.Who == 255)
        {
            return;
        }
        var player = Players[args.Who];
        if (player is not null)
        {
            player.Save();
            Players[args.Who] = null;
        }
    }
    private int OnNPC_NewNPC(On.Terraria.NPC.orig_NewNPC orig, Terraria.DataStructures.IEntitySource source, int X, int Y, int Type, int Start, float ai0, float ai1, float ai2, float ai3, int Target)
    {
        var num = orig(source, X, Y, Type, Start, ai0, ai1, ai2, ai3, Target);
        if (Main.npc[num].type == Type && Main.npc[num].active)
        {
            if (NPCSpawnLineInfos.TryGetValue(Type, out var infos) && infos.Lines.Length != 0)
            {
                foreach (var info in infos.Lines)
                {
                    TSPlayer.All.SendMessage(info.Text, info.Color);
                }
            }
        }
        return num;
    }
    private void OnNPC_SetDefaults(On.Terraria.NPC.orig_SetDefaults orig, NPC self, int Type, NPCSpawnParams spawnparams)
    {
        if (!spawnparams.strengthMultiplierOverride.HasValue)
        {
            spawnparams.strengthMultiplierOverride *= (Main.hardMode ? MainConfig.HardModeStrengthenCoefficient : MainConfig.StrengthenCoefficient) * (WorldGen.tBlood + WorldGen.tEvil);
        }
        orig(self, Type, spawnparams);
    }
    private void OnNPC_OnGameEventClearedForTheFirstTime(On.Terraria.NPC.orig_OnGameEventClearedForTheFirstTime orig, int gameEventId)
    {
        orig(gameEventId);
        if(EventDoCommands.TryGetValue(gameEventId, out var info))
        {
            if (info.Player == 0)
            {
                info.Commands.ForEach(x => Commands.HandleCommand(TSPlayer.Server, x));
            }
            else
            {
                TShock.Players.Where(x => x is not null && x.Active).ForEach(ply => info.Commands.ForEach(command => Commands.HandleCommand(ply, command)));
            }
        }
    }
    private static bool OnItem_CanShimmer(On.Terraria.Item.orig_CanShimmer orig, Item self)
    {
        Console.WriteLine($"damage:{self.damage} {self.playerIndexTheItemIsReservedFor} {!(Players[self.playerIndexTheItemIsReservedFor]?.ShimmerAddDamage ?? false)}");
        //if (self.damage == -1 || self.type >= 71 && self.type <= 74|| !(Players[self.playerIndexTheItemIsReservedFor]?.ShimmerAddDamage ?? false))
        //{
        //    return orig(self);
        //}
        if (self.active && !self.shimmered && self.damage > 0 && (self.type < 71 || self.type > 74) && (Players[self.playerIndexTheItemIsReservedFor]?.ShimmerAddDamage ?? false))
        {
            self.shimmerTime = 1f;
            self.shimmered = true;
            self.shimmerWet = true;
            self.wet = true;
            self.velocity *= 0.1f;
            NetMessage.SendData(MessageID.ShimmerActions, -1, -1, null, 0, (int)self.Center.X, (int)self.Center.Y);
            NetMessage.SendData(MessageID.SyncItemsWithShimmer, -1, -1, null, self.whoAmI, 1f);
            return false;
        }
        return orig(self);
    }
    private void OnNetMessage_SendData(On.Terraria.NetMessage.orig_SendData orig, int msgType, int remoteClient, int ignoreClient, Terraria.Localization.NetworkText text, int number, float number2, float number3, float number4, int number5, int number6, int number7)
    {
        orig(msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7);
        if (msgType is 21 or 90 or 145 or 148)
        {
            var item = Main.item[number];
            //Console.WriteLine("SendData whoAmi:{0} owner:{1}", item.whoAmI, item.playerIndexTheItemIsReservedFor);
            if (item.active && item.shimmered && item.damage != -1 && item.playerIndexTheItemIsReservedFor != 255)
            {
                Utils.UpdateItem(Players[item.playerIndexTheItemIsReservedFor], item);
            }
        }
    }
    private void OnPlayerChat(TShockAPI.Hooks.PlayerChatEventArgs e)
    {
        if (e.Player.GetFSPlayer(out var fsplayer))
        {
            e.TShockFormattedText = string.Format(MainConfig.ChatFormat, fsplayer.Title, e.TShockFormattedText);
        }
    }
    private void OnTileEdit(object? sender, GetDataHandlers.TileEditEventArgs e)
    {
        if (RegionRecord.ContainsKey(e.Player.Index))
        {
            var record = RegionRecord[e.Player.Index];
            switch (record.Point)
            {
                case 1:
                    record.Point1 = new(e.X, e.Y);
                    record.Point = record.NextPoint;
                    record.NextPoint = 0;
                    e.Player.SendSuccessMessage("记录点1 已设置");
                    e.Player.SendTileRect((short)e.X, (short)e.Y);
                    e.Handled = true;
                    break;
                case 2:
                    record.Point2 = new(e.X, e.Y);
                    record.Point = record.NextPoint;
                    record.NextPoint = 0;
                    e.Player.SendSuccessMessage("记录点2 已设置");
                    e.Player.SendTileRect((short)e.X, (short)e.Y);
                    e.Handled = true;
                    break;
                case 3:
                    record.Point3 = new(e.X, e.Y);
                    record.Point = record.NextPoint;
                    record.NextPoint = 0;
                    e.Player.SendSuccessMessage("记录点3 已设置");
                    e.Player.SendTileRect((short)e.X, (short)e.Y);
                    e.Handled = true;
                    break;
            }
        }
    }
    private void OnSign(object? sender, GetDataHandlers.SignEventArgs e)
    {
        if (LockSign)
        {
            ChangeConfig.LockSign.Add(new Vector2(e.X,e.Y));
            LockSign = false;
            e.Player.SendInfoMessage("X:{0} Y:{1}", e.X, e.Y);
        }
        if (ChangeConfig.LockSign.Contains(new Vector2(e.X, e.Y)))
        {
            for (int i = 0; i < 1000; i++)
            {
                if (Main.sign[i].x == e.X && Main.sign[i].y == e.Y)
                {
                    e.Player.SendData(PacketTypes.SignNew, "", i, e.X, e.Y);
                    e.Handled = true;
                }
            }
        }
    }
    private static void OnPreStartDay(VBY.GameContentModify.Config.MainConfigInfo config)
    {
        var num = 1 - (WorldGen.tEvil + WorldGen.tBlood) / 100.0;
        if(num < 0)
        {
            num = 0;
        }
        var moreThanHalf = num > 0.5;
        config.Invasion.DownedGoblinsStartInvasionRandomNum = (int)(30 * num);
        config.Invasion.HardModeDownedGoblinsStartInvasionRandomNum = (int)(60 * num);
        config.Invasion.DownedPiratesStartInvasionRandomNum = (int)(60 * num);
        config.Extension.SpawnTravelNPCWhenStartDay = moreThanHalf;
        config.Extension.SpawnTravelNPCWhenStartDayRandomNum = (int)(10 * (1 - num));
    }
    private static void OnPreStartNight(VBY.GameContentModify.Config.MainConfigInfo config)
    {
        var num = 1 - (WorldGen.tEvil + WorldGen.tBlood) / 100.0;
        if (num < 0)
        {
            num = 0;
        }
        var moreThanHalf = num > 0.5;
        config.BloodMoon.RandomNum = (int)(9 * num);
        config.BloodMoon.TenthAnniversaryWorldRandomNum = (int)(6 * num);
        config.BloodMoon.SpawnEyeCheck = !moreThanHalf;
        config.Spawn.EyeOfCthulhu.RandomNum = (int)(3 * num);
        config.Spawn.EyeOfCthulhu.DownedCheck = !moreThanHalf;
        config.Spawn.MechBoss.SpawnRandomNum = (int)(10 * num);
        config.Spawn.MechBoss.SpawnDownedCheck = !moreThanHalf;
        config.Spawn.MechBoss.SpawnEyeCheck = !moreThanHalf;
    }
    #endregion
}
public class TileRegionRecord
{
    public int Point;
    public Point? Point1, Point2, Point3;
    public int NextPoint;
    public string PointName = "";
}
public class Point
{
    public int X;
    public int Y;
    public Point(int x,int y)
    {
        X = x;
        Y = y;
    }
    public static implicit operator Microsoft.Xna.Framework.Point(Point point) => new(point.X, point.Y);
}