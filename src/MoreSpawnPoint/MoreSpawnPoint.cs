using System.IO.Streams;
using System.Runtime.CompilerServices;
using System.Reflection;

using Terraria;
using Terraria.ID;
using TerrariaApi.Server;

using TShockAPI;

namespace VBY.MoreSpawnPoint;

[ApiVersion(2, 1)]
public class MoreSpawnPoint : TerrariaPlugin
{
    public override string Name => "MoreSpawnPoint";
    public override string Author => "yu";
    public override string Description => "更多出生点";
#if TShock513
    private static Func<TSPlayer, MemoryStream, byte, int, int, int, PlayerSpawnContext,bool> OnPlayerSpawnv513 = 
                  (Func<TSPlayer, MemoryStream, byte, int, int, int, PlayerSpawnContext, bool>)Delegate.CreateDelegate(typeof(Func<TSPlayer, MemoryStream, byte, int, int, int, PlayerSpawnContext, bool>), typeof(GetDataHandlers).GetMethod("OnPlayerSpawn", BindingFlags.Static | BindingFlags.NonPublic)!);
#endif
#if TShock520
    private static Func<TSPlayer, MemoryStream, byte, int, int, int, int, int, PlayerSpawnContext, bool> OnPlayerSpawnv520 = 
                  (Func<TSPlayer, MemoryStream, byte, int, int, int, int, int, PlayerSpawnContext, bool>)Delegate.CreateDelegate(typeof(Func<TSPlayer, MemoryStream, byte, int, int, int, int, int, PlayerSpawnContext, bool>), typeof(GetDataHandlers).GetMethod("OnPlayerSpawn", BindingFlags.Static | BindingFlags.NonPublic)!);
#endif
    public static Dictionary<string, SpawnPointInfo> SpawnPointInfos = new();
    public static Dictionary<string, SpawnPointInfo> PlayerSpawnPoints = new();
    private static GetDataHandlerDelegate? OldGetDataHandler;
    public static ConfigBase<MainConfig> MainConfig = new(TShock.SavePath, Path.Combine("Config", "MoreSpawnPoint.json"), () => new())
    {
        PostRead = (config) =>
        {
            SpawnPointInfos = config.SpawnPoints.ToDictionary(x => x.Name);
            config.BindInfo.ForEach(x => Utils.BindSpawnPoint(TSPlayer.Server, x.Key, x.Value));
        },
        PreSave = (config) =>
        {
            config.SpawnPoints = SpawnPointInfos.Values.ToList();
            config.BindInfo = PlayerSpawnPoints.ToDictionary(x => x.Key, x => x.Value.Name);
        }
    };
    private Command[] AddCommands;
    public MoreSpawnPoint(Main game) : base(game)
    {
        AddCommands = new Command[] { new("spawnpoint.use", Cmd, "spawnpoint"), new(Permissions.worldspawn, Ctl, "spawnpointctl") };
        Order = 10;
    }
    public override void Initialize()
    {
        Commands.ChatCommands.AddRange(AddCommands);

        var dic = (Dictionary<PacketTypes, GetDataHandlerDelegate>)typeof(GetDataHandlers).GetField("GetDataHandlerDelegates", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!;
        OldGetDataHandler = dic[PacketTypes.PlayerSpawn];
#if TShock513
        dic[PacketTypes.PlayerSpawn] = HandleSpawnv513;
#endif
#if TShock520
        dic[PacketTypes.PlayerSpawn] = HandleSpawnv520;
#endif

        OTAPI.Hooks.NetMessage.SendBytes += OnNetMessage_SendBytes;
        MainConfig.Load(TSPlayer.Server);
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach(var cmd in AddCommands)
            {
                Commands.ChatCommands.Remove(cmd);
            }
            if(OldGetDataHandler != null)
                ((Dictionary<PacketTypes, GetDataHandlerDelegate>)typeof(GetDataHandlers).GetField("GetDataHandlerDelegates", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!)[PacketTypes.PlayerSpawn] = OldGetDataHandler;

            OTAPI.Hooks.NetMessage.SendBytes -= OnNetMessage_SendBytes;
            MainConfig.Save();
        }
        base.Dispose(disposing);
    }
    private void OnNetMessage_SendBytes(object? sender, OTAPI.Hooks.NetMessage.SendBytesEventArgs e)
    {
        if (e.Data[2] == MessageID.WorldData)
        {
            if (TShock.Players[e.RemoteClient] is not null)
            {
                var tsplayer = TShock.Players[e.RemoteClient];
                if (PlayerSpawnPoints.TryGetValue(tsplayer.Name, out var spawnInfo))
                {
                    Unsafe.As<byte, short>(ref e.Data[13]) = spawnInfo.X;
                    Unsafe.As<byte, short>(ref e.Data[15]) = spawnInfo.Y;
                }
                else
                {
                    if (!string.IsNullOrEmpty(MainConfig.Instance.DefaultBind))
                    {
                        Utils.BindSpawnPoint(TSPlayer.Server, tsplayer, MainConfig.Instance.DefaultBind);
                    }
                }
            }
        }
    }
#if TShock513
    private static bool HandleSpawnv513(GetDataHandlerArgs args)
    {
        if (args.Player.Dead && args.Player.RespawnTimer > 0)
        {
            return true;
        }
        byte pid = args.Data.ReadInt8();
        short num = args.Data.ReadInt16();
        short num2 = args.Data.ReadInt16();
        int num3 = args.Data.ReadInt32();
        PlayerSpawnContext spawnContext = (PlayerSpawnContext)args.Data.ReadByte();
        if (OnPlayerSpawnv513(args.Player, args.Data, pid, num, num2, num3, spawnContext))
        {
            return true;
        }
        if (Main.ServerSideCharacter && num == -1 && num2 == -1)
        {
            if (PlayerSpawnPoints.TryGetValue(args.Player.Name, out var spawnInfo))
            {
                args.Player.sX = spawnInfo.X;
                args.Player.sY = spawnInfo.Y;
            }
            else
            {
                args.Player.sX = Main.spawnTileX;
                args.Player.sY = Main.spawnTileY;
            }
            args.Player.Teleport(args.Player.sX * 16, args.Player.sY * 16 - 48, 1);
        }
        else if (Main.ServerSideCharacter && args.Player.sX > 0 && args.Player.sY > 0 && args.TPlayer.SpawnX > 0 && args.TPlayer.SpawnX != args.Player.sX && args.TPlayer.SpawnY != args.Player.sY)
        {
            args.Player.sX = args.TPlayer.SpawnX;
            args.Player.sY = args.TPlayer.SpawnY;
            if (Main.tile[args.Player.sX, args.Player.sY - 1].active() && Main.tile[args.Player.sX, args.Player.sY - 1].type == 79 && WorldGen.StartRoomCheck(args.Player.sX, args.Player.sY - 1))
            {
                args.Player.Teleport(args.Player.sX * 16, args.Player.sY * 16 - 48, 1);
            }
        }
        else if (Main.ServerSideCharacter && args.Player.sX > 0 && args.Player.sY > 0 && Main.tile[args.Player.sX, args.Player.sY - 1].active() && Main.tile[args.Player.sX, args.Player.sY - 1].type == 79 && WorldGen.StartRoomCheck(args.Player.sX, args.Player.sY - 1))
        {
            args.Player.Teleport(args.Player.sX * 16, args.Player.sY * 16 - 48, 1);
        }
        if (num3 > 0)
        {
            args.Player.Dead = true;
        }
        else
        {
            args.Player.Dead = false;
        }
        return false;
    }
#endif
#if TShock520
    private static bool HandleSpawnv520(GetDataHandlerArgs args)
    {
        if (args.Player.Dead && args.Player.RespawnTimer > 0)
        {
            //TShock.Log.ConsoleDebug(I18n.GetString("GetDataHandlers / HandleSpawn rejected dead player spawn request {0}", args.Player.Name));
            return true;
        }
        byte pid = args.Data.ReadInt8();
        short num = args.Data.ReadInt16();
        short num2 = args.Data.ReadInt16();
        int num3 = args.Data.ReadInt32();
        short numberOfDeathsPVE = args.Data.ReadInt16();
        short numberOfDeathsPVP = args.Data.ReadInt16();
        PlayerSpawnContext spawnContext = (PlayerSpawnContext)args.Data.ReadByte();
        if (OnPlayerSpawnv520(args.Player, args.Data, pid, num, num2, num3, numberOfDeathsPVE, numberOfDeathsPVP, spawnContext))
        {
            return true;
        }
        if (Main.ServerSideCharacter && num == -1 && num2 == -1)
        {
            if (PlayerSpawnPoints.TryGetValue(args.Player.Name, out var spawnInfo))
            {
                args.Player.sX = spawnInfo.X;
                args.Player.sY = spawnInfo.Y;
            }
            else
            {
                args.Player.sX = Main.spawnTileX;
                args.Player.sY = Main.spawnTileY;
            }
            args.Player.Teleport(args.Player.sX * 16, args.Player.sY * 16 - 48, 1);
            //TShock.Log.ConsoleDebug(I18n.GetString("GetDataHandlers / HandleSpawn force teleport 'vanilla spawn' {0}", args.Player.Name));
        }
        else if (Main.ServerSideCharacter && args.Player.sX > 0 && args.Player.sY > 0 && args.TPlayer.SpawnX > 0 && args.TPlayer.SpawnX != args.Player.sX && args.TPlayer.SpawnY != args.Player.sY)
        {
            args.Player.sX = args.TPlayer.SpawnX;
            args.Player.sY = args.TPlayer.SpawnY;
            if (Main.tile[args.Player.sX, args.Player.sY - 1].active() && Main.tile[args.Player.sX, args.Player.sY - 1].type == 79 && WorldGen.StartRoomCheck(args.Player.sX, args.Player.sY - 1))
            {
                args.Player.Teleport(args.Player.sX * 16, args.Player.sY * 16 - 48, 1);
                //.Log.ConsoleDebug(I18n.GetString("GetDataHandlers / HandleSpawn force teleport phase 1 {0}", args.Player.Name));
            }
        }
        else if (Main.ServerSideCharacter && args.Player.sX > 0 && args.Player.sY > 0 && Main.tile[args.Player.sX, args.Player.sY - 1].active() && Main.tile[args.Player.sX, args.Player.sY - 1].type == 79 && WorldGen.StartRoomCheck(args.Player.sX, args.Player.sY - 1))
        {
            args.Player.Teleport(args.Player.sX * 16, args.Player.sY * 16 - 48, 1);
            //TShock.Log.ConsoleDebug(I18n.GetString("GetDataHandlers / HandleSpawn force teleport phase 2 {0}", args.Player.Name));
        }
        if (num3 > 0)
        {
            args.Player.Dead = true;
        }
        else
        {
            args.Player.Dead = false;
        }
        return false;
    }
#endif
    private void Cmd(CommandArgs args)
    {
        if(args.Parameters.Count == 0)
        {
            args.Player.SendInfoMessage($"/{AddCommands[0].Name} bind <name> 绑定一个出生点");
            args.Player.SendInfoMessage($"/{AddCommands[0].Name} info 查看当前出生点");
            args.Player.SendInfoMessage($"/{AddCommands[0].Name} list 列出可用出生点");
            args.Player.SendInfoMessage($"/{AddCommands[0].Name} reset 设置回默认出生点");
            return;
        }
        switch (args.Parameters[0].ToLower())
        {
            case "bind":
                if (!args.Player.RealPlayer)
                {
                    args.Player.SendErrorMessage("非真实玩家不可设置出生点");
                    break;
                }
                if(args.Parameters.Count == 1)
                {
                    args.Player.SendInfoMessage($"请输入出生点名称");
                    break;
                }
                Utils.BindSpawnPoint(args.Player, args.Player, args.Parameters[1]);
                break;
            case "info":
                {
                    args.Player.SendInfoMessage("当前绑定的出生点为:" + (PlayerSpawnPoints.TryGetValue(args.Player.Name, out var info) ? info.Name : "无"));
                }
                break;
            case "list":
                if (SpawnPointInfos.Count == 0)
                {
                    args.Player.SendInfoMessage("无可用出生点");
                }
                else
                {
                    args.Player.SendInfoMessage(string.Join(", ", SpawnPointInfos.Select(x => $"{x.Key} X:{x.Value.X} Y:{x.Value.Y}")));
                }
                break;
            case "reset":
                {
                    PlayerSpawnPoints.Remove(args.Player.Name);
                    Utils.SetDefaultPoint(args.Player);
                    args.Player.SendSuccessMessage("重置成功");
                }
                break;
            default:
                args.Player.SendInfoMessage("未知参数 '{0}'", args.Parameters[0]);
                break;
        }
    }
    private void Ctl(CommandArgs args)
    {
        if(args.Parameters.Count == 0)
        {
            args.Player.SendInfoMessage($"/{AddCommands[1].Name} add <name> [player] 以玩家当前坐标添加出生点");
            args.Player.SendInfoMessage($"/{AddCommands[1].Name} del <name> [name2] [..] | all 删除出生点");
            args.Player.SendInfoMessage($"/{AddCommands[1].Name} set <player name> <point name> 设置在线玩家出生点");
            args.Player.SendInfoMessage($"/{AddCommands[1].Name} save 保存配置文件");
            args.Player.SendInfoMessage($"/{AddCommands[1].Name} reload 重新加载配置文件");
            return;
        }
        switch (args.Parameters[0].ToLower())
        {
            case "add":
                {
                    if (args.Parameters.Count == 1)
                    {
                        args.Player.SendInfoMessage($"/{AddCommands[1].Name} add <name> [player] 以玩家当前坐标添加出生点");
                        break;
                    }
                    var addName = args.Parameters[1];
                    TSPlayer? findPlayer = null;
                    if (args.Parameters.Count == 2)
                    {
                        if (!args.Player.RealPlayer)
                        {
                            args.Player.SendInfoMessage("请输入[player]参数来寻找真实玩家");
                            break;
                        }
                    }
                    if (findPlayer is not null || (args.Parameters.Count > 2 && Utils.FindPlayer(args.Player, args.Parameters[2], out findPlayer)))
                    {
                        if (SpawnPointInfos.ContainsKey(addName))
                        {
                            args.Player.SendWarningMessage($"出生点:{addName} 已存在");
                            return;
                        }
                        var addInfo = Utils.AddSpawnPoint(addName, args.Player);
                        args.Player.SendSuccessMessage($"出生点:{addName} 添加成功 x:{addInfo.X} y:{addInfo.Y}");
                    }
                }
                break;
            case "del":
                if (args.Parameters.Count == 1)
                {
                    args.Player.SendInfoMessage($"/{AddCommands[1].Name} del <name> [name2] [..] | all 删除出生点");
                    break;
                }
                if (args.Parameters.Contains("all"))
                {
                    SpawnPointInfos.Clear();
                    MainConfig.Instance.DefaultBind = "";
                    PlayerSpawnPoints.Clear();
                    args.Player.SendInfoMessage("清空完成");
                    break;
                }
                for (int i = 1; i < args.Parameters.Count; i++)
                {
                    if (SpawnPointInfos.ContainsKey(args.Parameters[i]))
                    {
                        SpawnPointInfos.Remove(args.Parameters[i]);
                        PlayerSpawnPoints.Where(x => x.Value.Name == args.Parameters[i]).ToList().ForEach(x => 
                        {
                            PlayerSpawnPoints.Remove(x.Key);
                            var setPlayer = TShock.Players.FirstOrDefault(y => y is not null && y.Active && y.Name == x.Key);
                            if (setPlayer is not null)
                            {
                                Utils.SetDefaultPoint(setPlayer);
                            }
                        });
                    }
                    else
                    {
                        args.Player.SendInfoMessage($"出生点:{args.Parameters[i]} 不存在");
                    }
                }
                break;
            case "set":
                {
                    if (args.Parameters.Count == 1)
                    {
                        args.Player.SendInfoMessage($"/{AddCommands[1].Name} set <player name> <point name|null> 设置在线玩家出生点");
                        break;
                    }
                    if (args.Parameters.Count < 3)
                    {
                        args.Player.SendInfoMessage("参数不足");
                        break;
                    }
                    var pointName = args.Parameters[2];
                    var list = TSPlayer.FindByNameOrID(args.Parameters[1]);
                    if (list.Count == 0)
                    {
                        args.Player.SendInfoMessage("没有找到玩家");
                        break;
                    }
                    else if (list.Count > 1)
                    {
                        args.Player.SendMultipleMatchError(list.Select(x => x.Name));
                        break;
                    }
                    var setPlayer = list[0];
                    if (pointName == "null")
                    {
                        if (PlayerSpawnPoints.Remove(setPlayer.Name))
                        {
                            Utils.SetDefaultPoint(setPlayer);
                            args.Player.SendSuccessMessage("移除成功");
                        }
                        else
                        {
                            args.Player.SendErrorMessage("移除失败");
                        }
                    }
                    else
                    {
                        Utils.BindSpawnPoint(args.Player, setPlayer, pointName);
                    }
                }
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
                break;
            default:
                args.Player.SendInfoMessage("未知参数 '{0}'", args.Parameters[0]);
                break;
        }
    }
}
public class SpawnPointInfo
{
    public string Name;
    public short X,Y;

    public SpawnPointInfo(string name,short x,short y)
    {
        Name = name;
        X = x;
        Y = y;
    }
}