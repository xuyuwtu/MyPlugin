using System.IO.Streams;
using System.Text;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using TerrariaApi.Server;

using TShockAPI;
using static MonoMod.InlineRT.MonoModRule;

namespace Game1;

internal static class MiniGame
{
    internal static bool Started;
    internal static int DefGameDuration;
    internal static int GameDuration;
    internal static int DefGameUpdateBuffDuration = 60 * 60;
    internal static int GameUpdateBuffDuration;
    internal static int MinPlayerCount = 2;
    internal static int MinTeamCount = 2;
    internal static Rectangle GameRegion = new();
    internal static ITile[] GameRegionTiles = Array.Empty<ITile>();
    internal static Dictionary<int, Dictionary<int, PlayerData>> SwitchData = new();
    internal static List<IHookManager> PlayHooks = new()
    {
        GetDataHandlers.PlayerSpawn.GetHookManager(OnPlayerSpawn, false),
        GetDataHandlers.PlayerUpdate.GetHookManager(OnPlayerUpdate, false),
        new EventHookHandlerManager(
        () => On.Terraria.Projectile.Kill += OnProjectile_Kill,
        () => On.Terraria.Projectile.Kill -= OnProjectile_Kill,
        false),
        new EventHookHandlerManager(
        () => On.Terraria.NetMessage.SendData += OnNetMessageSendData,
        () => On.Terraria.NetMessage.SendData -= OnNetMessageSendData,
        false)
    };

    internal static byte[] TeamConvertColors = new byte[TeamID.Count];
    internal static Point[] TeamSpawnPoint = new Point[TeamID.Count];
    internal static Rectangle[] TeamSpawnRegion = new Rectangle[TeamID.Count];
    internal static int[] ProjectlileConvertSize = new int[ProjectileID.Count];
    internal static int[] ViewerBuffs =
    {
        BuffID.NoBuilding,
        BuffID.Invisibility,
        BuffID.NightOwl,
        BuffID.Silenced,
        BuffID.Cursed
    };

    internal static List<TSPlayer> ViewPlayers = new();
    internal static List<int> ViewPlayersID = new();

    //add
    internal static List<TSPlayer> PlayPlayers = new();
    internal static List<string> PlayPlayersName = new();
    internal static List<int> PlayPlayersID = new();
    //init
    internal static int[] PlayersTeam = new int[255];
    internal static int[] PlayersConvertCount = new int[255];
    internal static Dictionary<string, int> PlayPlayersNameTeam = new();
    internal static Dictionary<string, int> PlayPlayersNameConvertCount = new();

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    internal static PlayerData DefaultPlayerData;
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    internal static Dictionary<int, int> SpawnItemStack = new();
    #region PlayerUpdate
    internal static void AddPlayPlayer(TSPlayer player)
    {
        AddPlayingPlayer(player);
        PlayPlayersName.Add(player.Name);
    }
    internal static void RemovePlayPlayer(TSPlayer player)
    {
        RemovePlayingPlayer(player);
        PlayPlayersName.Remove(player.Name);
    }
    internal static void AddPlayingPlayer(TSPlayer player)
    {
        PlayPlayers.Add(player);
        PlayPlayersID.Add(player.Index);
    }
    internal static void RemovePlayingPlayer(TSPlayer player)
    {
        PlayPlayers.Remove(player);
        PlayPlayersID.Remove(player.Index);
    }
    internal static void AddViewPlayer(TSPlayer player)
    {
        ViewPlayers.Add(player);
        ViewPlayersID.Add(player.Index);
    }
    internal static void RemoveViewPlayer(TSPlayer player)
    {
        ViewPlayers.Remove(player);
        ViewPlayersID.Remove(player.Index);
    }
    #endregion
    internal static (bool success, string message) CanStart()
    {
        if (Started)
        {
            return (false, "游戏已开始");
        }
        var playPlayers = TShock.Players.Where(x => x is not null && x.Active && x.Team != 0);
        if (playPlayers.Count() < MinPlayerCount)
        {
            return (false, $"人数不足,最少需要{MinPlayerCount}个玩家");
        }
        var teamInfo = playPlayers.GroupBy(x => x.Team);
        if(teamInfo.Where(x => x.Key != 0).Count() < MinTeamCount)
        {
            return (false, $"队伍数量不足,最少需要{MinTeamCount}个队伍");
        }
        var teamMembersCount = playPlayers
            .GroupBy(x => x.Team)
            .Select(x => x.Count());
        if (teamMembersCount.Max() - teamMembersCount.Min() > 1)
        {
            return (false, "玩家队伍人数分配不均");
        }
        return (true,"");
    }
    internal static void Start()
    {
        GameRegionTiles = new ITile[GameRegion.Width * GameRegion.Height];
        int num = 0;
        for (int i = GameRegion.X; i < GameRegion.Right; i++)
        {
            for (int j = GameRegion.Y; j < GameRegion.Bottom; j++)
            {
                if (Main.tile[i, j] is null)
                {
                    Main.tile[i, j] = OTAPI.Hooks.Tile.InvokeCreate();
                }
                GameRegionTiles[num] = (ITile)Main.tile[i, j].Clone();
                num++;
            }
        }
        Array.Fill(PlayersTeam, 0);
        Array.Fill(PlayersConvertCount, 0);
        foreach(var player in TShock.Players)
        {
            if(player is not null && player.Active && player.Team != 0)
            {
                AddPlayPlayer(player);
            }
        }
        foreach(var player in PlayPlayers)
        {
            PlayersTeam[player.Index] = player.Team;
        }
        PlayPlayersNameTeam = PlayPlayers.ToDictionary(x => x.Name, x => x.Team);

        GameDuration = 0;
        Started = true;
        Config.Load(Game1.Config);
        PlayHooks.ForEach(x => x.Register());

        ViewPlayers.AddRange(TShock.Players.Where(x => x is not null && x.Active && !PlayPlayers.Contains(x)));
        ViewPlayers.ForEach(x =>
        {
            x.TPlayer.ghost = true;
            TSPlayer.All.SendData(PacketTypes.PlayerInfo, "", x.Index);
            TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", x.Index);
        });
        UpdateViewerBuff();

        DefaultPlayerData = new PlayerData(null) { health = 400, maxHealth = 400 };
        DefaultPlayerData.StoreSlot(Game1.Config.InitialBagItems);
        SpawnItemStack = Game1.Config.InitialBagItems.Where(x => x.stack > 1).ToDictionary(x => x.type, x => x.stack);

        SwitchData.Clear();
        foreach(var info in Game1.Config.SwitchItemInfos)
        {
            foreach(var point in info.Points)
            {
                var addData = DefaultPlayerData.Copy();
                addData.StoreSlot(info.ItemInfos);
                if (SwitchData.TryGetValue(point.X, out var xlist))
                {
                    xlist[point.Y] = addData;
                }
                else
                {
                    SwitchData[point.X] = new Dictionary<int, PlayerData>() { [point.Y] = addData };
                }
            }
        }

        PlayPlayers.ForEach(x => DefaultPlayerData.RestoreCharacter(x));
        for(int i = 0; i < PlayPlayers.Count; i++)
        {
            var spawnTileX = Main.spawnTileX;
            var spawnTileY = Main.spawnTileY;
            var teamSpawnPoint = TeamSpawnPoint[PlayPlayers[i].Team];
            PlayPlayers[i].sX = teamSpawnPoint.X;
            PlayPlayers[i].sY = teamSpawnPoint.Y;
            PlayPlayers[i].TPlayer.SpawnX = teamSpawnPoint.X;
            PlayPlayers[i].TPlayer.SpawnY = teamSpawnPoint.Y;
            Main.spawnTileX = teamSpawnPoint.X;
            Main.spawnTileY = teamSpawnPoint.Y;
            PlayPlayers[i].SendData(PacketTypes.PlayerSpawn, "", PlayPlayers[i].Index);
            PlayPlayers[i].SendData(PacketTypes.WorldInfo);
            PlayPlayers[i].Teleport(Main.spawnTileX * 16, Main.spawnTileY * 16);
            Main.spawnTileX = spawnTileX;
            Main.spawnTileY = spawnTileY;
        }
        TSPlayer.All.SendInfoMessage("游戏已开始");
        TSPlayer.Server.SendInfoMessage("游戏已开始");
    }
    internal static (bool success, string message) CanStop()
    {
        if (!Started)
        {
            return (false, "游戏未开始");
        }
        return (true, "");
    }
    internal static void Stop(string message = "")
    {
        PlayPlayersNameTeam.Clear();
        GameDuration = 0;
        Started = false; 
        PlayHooks.ForEach(x => x.Unregister());

        ViewPlayers.ForEach(x =>
        {
            x.TPlayer.ghost = false;
            TSPlayer.All.SendData(PacketTypes.PlayerInfo, "", x.Index);
            TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", x.Index);
            for(int i = 0;i< x.TPlayer.buffType.Length;i++)
            {
                x.TPlayer.buffType[i] = 0;
                x.TPlayer.buffTime[i] = 0;
            }
            NetMessage.SendData(MessageID.WorldData);
            TSPlayer.All.SendData(PacketTypes.PlayerBuff, message, x.Index);
            x.Spawn(PlayerSpawnContext.SpawningIntoWorld);
        });
        ViewPlayers.Clear(); 
        SpawnItemStack.Clear();
        if (!string.IsNullOrEmpty(message))
        {
            TSPlayer.All.SendInfoMessage($"游戏被终止,理由:{message}");
            TSPlayer.Server.SendInfoMessage($"游戏被终止,理由:{message}");
        }
        TSPlayer.Server.SendInfoMessage("游戏已结束");
        TSPlayer.All.SendInfoMessage("游戏已结束,正在进行油漆计数...");
        Task.Run(() => Utils.TileColorStatistic()).ContinueWith(t =>
        {
            var counts = t.Result;
            var team2PaintedCount = TeamConvertColors.Select(x => counts[x]).ToArray();

            var winTeams = new List<int>();
            var maxPaintCount = 0;
            foreach (var i in Game1.Config.CanSelectTeams)
            {
                if (team2PaintedCount[i] > maxPaintCount)
                {
                    winTeams.Clear();
                    winTeams.Add(i);
                    maxPaintCount = team2PaintedCount[i];
                    continue;
                }
                if (team2PaintedCount[i] == maxPaintCount)
                {
                    winTeams.Add(i);
                    continue;
                }
            }

            TSPlayer.All.SendInfoMessage($"胜利的队伍为：{string.Join(", ", winTeams.Select(x => Utils.TeamChinese[x]))}");
            var sb = new StringBuilder();
            sb.AppendLine("油漆计数: ");
            foreach (var i in Game1.Config.CanSelectTeams)
            {
                sb.AppendLine($"{Utils.TeamChinese[i]}: {team2PaintedCount[i]}");
            }
            TSPlayer.All.SendInfoMessage(sb.ToString());
            PlayPlayers.Clear();
            PlayPlayersName.Clear();

            int num = 0;
            for (int i = GameRegion.X; i < GameRegion.Right; i++)
            {
                for (int j = GameRegion.Y; j < GameRegion.Bottom; j++)
                {
                    if (Main.tile[i, j] is null)
                    {
                        Main.tile[i, j] = OTAPI.Hooks.Tile.InvokeCreate();
                    }
                    Main.tile[i, j].CopyFrom(GameRegionTiles[num]);
                    num++;
                }
            }
            TSPlayer.All.SendInfoMessage($"喷漆数量:\n{string.Join('\n', PlayPlayersID.Select(index => (TShock.Players[index].Name, Count: PlayersConvertCount[index])).OrderBy(x => x.Count).Select(x => $"{x.Name}: {x.Count}"))}");
            //怎么无效啊
            //Utils.SendTileRect(GameRegion);
        });
    }
    internal static void UpdateViewerBuff()
    {
        foreach (var i in ViewPlayers)
        {
            foreach (var buff in ViewerBuffs)
            {
                i.SetBuff(buff, 7200);
            }
        }
    }
    #region Hooks
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:删除未使用的参数", Justification = "<挂起>")]
    internal static void OnGameUpdate(EventArgs args)
    {
        GameDuration++;
        GameUpdateBuffDuration++;
        if (GameDuration == DefGameDuration)
        {
            Stop("时间到");
        }
        if (DefGameDuration - GameDuration == 60 * 60)
        {
            TSPlayer.All.SendInfoMessage("游戏还剩一分钟结束");
        }
        if (GameUpdateBuffDuration == DefGameDuration)
        {
            UpdateViewerBuff();
        }
    }
    internal static void OnNetGetData(GetDataEventArgs args)
    {
        if(args.MsgID == PacketTypes.HitSwitch)
        {
            int x, y;
            using (var stream = new MemoryStream(args.Msg.readBuffer, args.Index, args.Length))
            {
                x = stream.ReadInt16();
                y = stream.ReadInt16();
            }
            if (SwitchData.TryGetValue(x, out var xlist) && xlist.TryGetValue(y, out var data))
            {
                var tsplayer = TShock.Players[args.Msg.whoAmI];
                data.RestoreCharacter(tsplayer);
            }
        }
    }
    internal static void OnNetMessageSendData(On.Terraria.NetMessage.orig_SendData orig, int msgType, int remoteClient, int ignoreClient, Terraria.Localization.NetworkText text, int number, float number2, float number3, float number4, int number5, int number6, int number7)
    {
        if (msgType == MessageID.WorldData)
        {
            if (remoteClient == -1)
            {
                return;
            }
            var spawnTileX = Main.spawnTileX;
            var spawnTileY = Main.spawnTileY;
            var player = Main.player[remoteClient];
            var teamSpawnPoint = TeamSpawnPoint[player.team];
            Main.spawnTileX = teamSpawnPoint.X;
            Main.spawnTileY = teamSpawnPoint.Y;
            orig(msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7);
            Main.spawnTileX = spawnTileX;
            Main.spawnTileY = spawnTileY;
            return;
        }
        orig(msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7);
    }
    internal static void OnProjectile_Kill(On.Terraria.Projectile.orig_Kill orig, Projectile self)
    {
        if (!(self?.active ?? false))
        {
            return;
        }
        if (ProjectlileConvertSize[self.type] > 0) // Can convert
        {
            Utils.ColorTile((int)(self.Center.X / 16), (int)(self.Center.Y / 16), Main.player[self.owner].team, self.owner, ProjectlileConvertSize[self.type] - 1);
        }
        orig(self);
    }
    private static void OnPlayerSpawn(object? sender, GetDataHandlers.SpawnEventArgs e)
    {
        if (MiniGame.PlayPlayers.Contains(e.Player))
        {
            var bag = e.Player.TPlayer.inventory.Take(NetItem.InventorySlots).Where(x => x.active && x.type > 0 && x.stack > 0).ToDictionary(x => x.type);
            foreach (var (type, defStack) in MiniGame.SpawnItemStack)
            {
                if (bag.ContainsKey(type))
                {
                    for (int slot = 0; slot < NetItem.InventorySlots; slot++)
                    {
                        if (e.Player.TPlayer.inventory[slot].type == type)
                        {
                            e.Player.TPlayer.inventory[slot].stack += defStack - bag[type].stack;
                            TSPlayer.All.SendData(PacketTypes.PlayerSlot, "", e.Player.Index, slot);
                            break;
                        }
                    }
                }
                else
                {
                    for (int slot = 0; slot < NetItem.InventorySlots; slot++)
                    {
                        if (e.Player.TPlayer.inventory[slot].type == 0)
                        {
                            e.Player.TPlayer.inventory[slot].SetDefaults(type);
                            e.Player.TPlayer.inventory[slot].stack = defStack;
                            TSPlayer.All.SendData(PacketTypes.PlayerSlot, "", e.Player.Index, slot);
                            break;
                        }
                    }
                }
            }
        }
        e.Handled = true;
    }
    private static void OnPlayerUpdate(object? sender, GetDataHandlers.PlayerUpdateEventArgs e)
    {
        if (MiniGame.ViewPlayersID.Contains(e.PlayerId))
        {
            if (!e.MiscData1.IsGhosted)
            {
                Main.player[e.PlayerId].ghost = true;
                NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, e.PlayerId);
            }
        }
    }
    #endregion
}
