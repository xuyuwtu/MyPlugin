using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Xna.Framework;

using Terraria;
using TerrariaApi.Server;

using TShockAPI;

using Newtonsoft.Json;

using MonoMod.RuntimeDetour;

using VBY.Common.CommandV2;

namespace Game1;

[ApiVersion(2, 1)]
public class Game1 : TerrariaPlugin
{
    public override string Name => "Game1";
    public override string Author => "yu";
    public override string Description => "某个喷喷游戏";
    public override Version Version => GetType().Assembly.GetName().Version!;


    internal Command[] AddCommands;
    internal static Config Config = new();
    internal static string ConfigSavePath = Path.Combine(TShock.SavePath, "Game1.json");

    internal static List<string> GameStopVoterList = new();

    internal static PlayerData? EmptyPlayerData;
    internal static Detour SpawnNPCDetour = new(new Action(NPC.SpawnNPC), SpawnNPC);//, new DetourConfig() { ManualApply = true });

    internal static HookManager HookManager = new();
    private static GetDataHandlerManager<GetDataHandlers.TileEditEventArgs> OnTileHook = GetDataHandlers.TileEdit.GetHookManager(OnTileEdit, false);
    private static int RecordTileID = -1;
    private static Point[] TempPoints = new Point[2];

    internal static List<IDisposable> Disposables = new();

    internal SubCmdRoot CmdCommand, CtlCommand;
    public Game1(Main game) : base(game)
    {
        FileTools.CreateIfNot(ConfigSavePath, JsonConvert.SerializeObject(new Config(), Formatting.Indented));
        Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigSavePath))!;
        Config.Load(Config);
        MiniGame.PlayHooks.Add(ServerApi.Hooks.GameUpdate.GetHookManager(this, MiniGame.OnGameUpdate, false));
        MiniGame.PlayHooks.Add(ServerApi.Hooks.NetGetData.GetHookManager(this, MiniGame.OnNetGetData, false));
        CmdCommand = new("GameCmd")
        {
            CmdStart
        };
        CtlCommand = new("GameCtl")
        {
            new SubCmdRun(CtlStart),
            new SubCmdRun(CtlReload),
            new SubCmdRun(CtlLoadbag),
            new SubCmdRun(CtlRecordbag),
            new SubCmdRun("SetTeamSpawnPoint", "设置队伍出生点", CtlSetteamspawnpoint, "stp"),
            new SubCmdRun(CtlStop),
            new SubCmdRun(CtlSet1),
            new SubCmdRun(CtlSet2),
            new SubCmdRun(CtlSetregion)
        };
        AddCommands = new Command[] { CmdCommand.GetCommand("game.cmd", new[]{ "game" }), CtlCommand.GetCommand("game.ctl", new[]{ "gamectl" })};
    }

    #region Initialize / Dispose

    public override void Initialize()
    {
        Commands.ChatCommands.AddRange(AddCommands);
        SpawnNPCDetour.Apply();
        EmptyPlayerData = new(null);
        for (int i = 0; i < NetItem.MaxInventory; i++)
        {
            EmptyPlayerData.inventory[i] = default;
        }
        HookManager.AddRange(MiniGame.PlayHooks);
        HookManager.AddRange(
            OnTileHook,
            GetDataHandlers.PlayerTeam.GetHookManager(OnPlayerTeam),
            ServerApi.Hooks.ServerJoin.GetHookManager(this, OnServerJoin),
            ServerApi.Hooks.ServerLeave.GetHookManager(this, OnServerLeave),
            new EventHookHandlerManager(
                    () => TShockAPI.Hooks.PlayerHooks.PlayerPermission += OnPlayerHooks_PlayerPermission,
                    () => TShockAPI.Hooks.PlayerHooks.PlayerPermission -= OnPlayerHooks_PlayerPermission
                )
            );
        HookManager.Init();
        Disposables.Add(HookManager);
        Disposables.Add(SpawnNPCDetour);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Commands.ChatCommands.RemoveRange(AddCommands); 
            Disposables.ForEach(x => x.Dispose());
            Disposables.Clear();
            Utils.ClearOwner(MiniGame.OnProjectile_Kill);
        }
        base.Dispose(disposing);
    }

    #endregion

    #region Detour
    public static void SpawnNPC() { }
    #endregion
    #region Hooks
    //开了后 新进的为幽灵模式
    private static void OnServerJoin(JoinEventArgs args)
    {
        if(args.Who == 255)
        {
            return;
        }
        var plr = TShock.Players[args.Who];
        if(plr is null)
        {
            return;
        }
        if (MiniGame.Started)
        {
            var player = Main.player[args.Who];
            if (player is not null)
            {
                if (MiniGame.PlayPlayersNameTeam.TryGetValue(player.name, out var team))
                {
                    MiniGame.PlayersTeam[args.Who] = team;
                    TShock.Players[args.Who].SetTeam(team);
                    MiniGame.AddPlayingPlayer(plr);
                    MiniGame.PlayersConvertCount[args.Who] = MiniGame.PlayPlayersNameConvertCount[player.name];
                }
                else
                {
                    MiniGame.AddViewPlayer(plr);
                }
            }
        }
        else
        {
            EmptyPlayerData?.RestoreCharacter(plr);
        }
    }
    private void OnServerLeave(LeaveEventArgs args)
    {
        if (args.Who == 255)
        {
            return;
        }
        if (TShock.Players[args.Who] is null)
        {
            return;
        }
        if (MiniGame.Started)
        {
            if (MiniGame.ViewPlayersID.Contains(args.Who))
            {
                MiniGame.RemoveViewPlayer(TShock.Players[args.Who]);
            }
            else if (MiniGame.PlayPlayersID.Contains(args.Who))
            {
                MiniGame.RemovePlayingPlayer(TShock.Players[args.Who]);
                MiniGame.PlayPlayersNameConvertCount[TShock.Players[args.Who].Name] = MiniGame.PlayersConvertCount[args.Who];
                MiniGame.PlayersConvertCount[args.Who] = 0;
            }
            if (TShock.Utils.GetActivePlayerCount() == 0)
            {
                MiniGame.Stop("全部玩家均已退出");
            }
        }
        else
        {
            MiniGame.RemovePlayPlayer(TShock.Players[args.Who]);
        }
    }
    private static void OnPlayerTeam(object? sender, GetDataHandlers.PlayerTeamEventArgs e)
    {
        if (MiniGame.Started)
        {
            //开始后禁止修改队伍
            if (MiniGame.PlayPlayersNameTeam.TryGetValue(e.Player.Name, out var team))
            {
                e.Handled = true;
                e.Player.SetTeam(team);
            }
            else
            {
                if(!(e.Team == TeamID.None)) 
                {
                    e.Handled = true;
                    e.Player.SetTeam(TeamID.None);
                }
            }
        }
        else
        {
            //if(e.Player.Team != TeamID.None && e.Team == TeamID.None)
            //{
            //    MiniGame.RemovePlayPlayer(e.Player);
            //}
            //只允许选择指定队伍
            //else if (!Config.CanSelectTeams.Contains(e.Team))
            if (!Config.CanSelectTeams.Contains(e.Team))
            {
                e.Player.SendInfoMessage($"你只能选择:{string.Join(", ", Config.CanSelectTeams.Select(x => Utils.TeamChinese[x]))}");
                e.Handled = true;
                e.Player.SetTeam(TeamID.None);
            }
            //else
            //{
            //    MiniGame.AddPlayPlayer(e.Player);
            //}
        }
    }
    private static void OnTileEdit(object? sender, GetDataHandlers.TileEditEventArgs e)
    {
        if (RecordTileID != -1)
        {
            TempPoints[RecordTileID].X = e.X;
            TempPoints[RecordTileID].Y = e.Y;
            e.Player.SendSuccessMessage($"记录点 {RecordTileID + 1} 已设置");
            e.Player.SendSuccessMessage($"x:{e.X} y:{e.Y}");
            RecordTileID = -1;
            e.Player.SendTileRect((short)e.X, (short)e.Y);
            e.Handled = true;
            OnTileHook.Unregister();
        }
    }
    private void OnPlayerHooks_PlayerPermission(TShockAPI.Hooks.PlayerPermissionEventArgs e)
    {
        if(e.Permission == Permissions.bypassssc)
        {
            if (MiniGame.Started)
            {
                if (MiniGame.ViewPlayers.Contains(e.Player))
                {
                    e.Result = TShockAPI.Hooks.PermissionHookResult.Granted;
                }
                else if (MiniGame.PlayPlayers.Contains(e.Player))
                {
                    e.Result = TShockAPI.Hooks.PermissionHookResult.Denied;
                }
            }
            else
            {
                e.Result = TShockAPI.Hooks.PermissionHookResult.Granted;
            }
        }
    }
    #endregion

    #region Commands

    [Description("开始游戏")]
    private void CmdStart(SubCmdArgs args)
    {
        TSPlayer.All.SendInfoMessage($"{args.Player.Name} 尝试开启游戏");
        var (success, message) = MiniGame.CanStart();
        if (!success)
        {
            TSPlayer.All.SendInfoMessage(message); 
            return;
        }
        MiniGame.Start();
    }
    //[Description("请求结束游戏")]
    //// TODO: 投票结束游戏
    //private void CmdStop(SubCmdArgs args)
    //{
    //    var (success, message) = MiniGame.CanStop();
    //    if (!success)
    //    {
    //        args.Player.SendInfoMessage(message);
    //        return;
    //    }
    //    MiniGame.Stop();
    //}
    [Description("重载")]
    private void CtlReload(SubCmdArgs args)
    {
        Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigSavePath))!;
        Config.Load(Config);
        args.Player.SendSuccessMessage("重载成功");
    }
    [Description("加载配置文件的初始背包")]
    private void CtlLoadbag(SubCmdArgs args)
    {
        var DefaultPlayerData = new PlayerData(null) { health = 400, maxHealth = 400 };
        DefaultPlayerData.StoreSlot(Config.InitialBagItems);
        DefaultPlayerData.RestoreCharacter(args.Player);
    }
    [Description("复制当前背包为初始背包")]
    private void CtlRecordbag(SubCmdArgs args)
    {
        var data = new PlayerData(null);
        data.CopyCharacter(args.Player);
        Config.InitialBagItems = data.inventory.Where(x => x.NetId != 0).Select((data, index) => new ItemInfo(data, index)).ToArray();
        Config.Save(ConfigSavePath);
    }
    private void CtlSetteamspawnpoint(SubCmdArgs args)
    {
        if(args.Player.Team == TeamID.None)
        {
            args.Player.SendInfoMessage("无法为无队伍设置出生点");
            return;
        }
        Config.TeamSpawnPoint[args.Player.Team] = new Point(args.Player.TileX, args.Player.TileY);
        args.Player.SendSuccessMessage("设置成功");
        Config.Save(ConfigSavePath);
    }
    [Description("强制开始游戏")]
    private void CtlStart(SubCmdArgs args)
    {
        MiniGame.Start();
    }
    [Description("强制结束游戏")]
    private void CtlStop(SubCmdArgs args)
    {
        MiniGame.Stop(args.Parameters.ElementAtOrDefault(0, "无"));
    }
    [Description("设置点1")]
    private void CtlSet1(SubCmdArgs args)
    {
        RecordTileID = 0;
        args.Player.SendInfoMessage("setting 1");
        OnTileHook.Register();
    }
    [Description("设置点2")]
    private void CtlSet2(SubCmdArgs args)
    {
        RecordTileID = 1;
        args.Player.SendInfoMessage("setting 2");
        OnTileHook.Register();
    }
    [Description("设置区域")]
    private void CtlSetregion(SubCmdArgs args)
    {
        TSPlayer player;
        if (args.Player.RealPlayer)
        {
            player = args.Player;
        }
        else
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendInfoMessage("请选择一个真实玩家");
                return;
            }
            var list = TSPlayer.FindByNameOrID(args.Parameters[0]);
            if(list.Count != 1)
            {
                args.Player.SendInfoMessage("查找出错，没找到或有多个");
                return;
            }
            player = list[0];
        }
        if (TempPoints[0] is { X: 0, Y: 0 } || TempPoints[1] is { X: 0, Y: 0 })
        {
            args.Player.SendInfoMessage("两点未设置完成");
            return;
        }
        if(player.Team == 0)
        {
            Config.GameRegion = Utils.GetRectangle(TempPoints[0], TempPoints[1]);
            args.Player.SendInfoMessage("游戏区域已设置");
        }
        else
        {
            Config.TeamSpawneRegion[args.Player.Team] = Utils.GetRectangle(TempPoints[0], TempPoints[1]);
            args.Player.SendInfoMessage($"队伍:{Utils.TeamChinese[args.Player.Team]} 出生点区域已设置");
        }
        Config.Save(ConfigSavePath);
    }
    #endregion
}