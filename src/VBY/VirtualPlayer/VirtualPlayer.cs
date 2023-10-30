using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using TerrariaApi.Server;
using TShockAPI;

namespace VBY.VirtualPlayer;

[ApiVersion(2, 1)]
public class VirtualPlayer : TerrariaPlugin
{
    public override string Name => "VBY.VirtualPlayer";
    public static List<byte> VirtualPlayersIndex = new();
    public static PlayerFileData[] VirtualPlayersData = new PlayerFileData[255];
    public static int[] BindIDs = new int[255];
    public static int Num;
    public static byte VPlayerIndex = 254;
    public static VPlayer VPlayer = new() { name = "灵梦", whoAmI = VPlayerIndex };
    public static ConfigBase<List<VirtualPlayerInfo>> MainConfig = new("Config", "VBY.VirtualPlayer.json", () => new());
    //public static TSPlayer VTSPlayer = new(VPlayerIndex);
    public static PlayerFileData LoadPlayerData;
    public Command AddCommand;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    static VirtualPlayer()
    {
        Array.Fill(BindIDs, -1);
    }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    public VirtualPlayer(Main game) : base(game)
    {
        AddCommand = new(Cmd, "vp");
        LoadPlayerData = Utils.LoadPlayer(Path.Combine("VirtualPlayer", "灵梦.plr"));
        LoadPlayerData.Player.whoAmI = VPlayerIndex;
    }

    public override void Initialize()
    {
        MainConfig.Load(TSPlayer.Server);
        byte index = 254; 
        for (int i = 0; i < MainConfig.Instance.Count; i++) 
        {
            var instance = MainConfig.Instance[i];
            if (!string.IsNullOrEmpty(instance.FileName))
            {
                VirtualPlayersData[index] = Utils.LoadPlayer(Path.Combine("VirtualPlayer", instance.FileName));
                VirtualPlayersData[index].Player.position = instance.Position;
                VirtualPlayersData[index].Player.direction = instance.Direction;
                VirtualPlayersData[index].Player.whoAmI = index;
                VirtualPlayersIndex.Add(index);
                Console.WriteLine($"{VirtualPlayersData[index].Name} file load");
                index--;
            }
        }
        Commands.ChatCommands.Add(AddCommand);
        //ServerApi.Hooks.GameUpdate.Register(this, OnGameUpdate);
        On.Terraria.Main.DoUpdateInWorld += OnMain_DoUpdateInWorld;
        ServerApi.Hooks.ServerJoin.Register(this, OnServerJoin);
        //GetDataHandlers.NewProjectile.Register(OnNewProjectile);
        //GetDataHandlers.PlayerUpdate.Register(OnPlayerUpdate);
    }

    protected override void Dispose(bool disposing)
    {
        if(disposing)
        {
            Commands.ChatCommands.Remove(AddCommand);
            On.Terraria.Main.DoUpdateInWorld -= OnMain_DoUpdateInWorld;
            ServerApi.Hooks.ServerJoin.Deregister(this, OnServerJoin);
            //GetDataHandlers.NewProjectile.UnRegister(OnNewProjectile);
            //GetDataHandlers.PlayerUpdate.UnRegister(OnPlayerUpdate);
            Utils.ClearOwner(OnMain_DoUpdateInWorld);
        }
        base.Dispose(disposing);
    }
    private void OnServerJoin(JoinEventArgs args)
    {
        if(args.Who == 255)
        {
            return;
        }
        if (TShock.Players[args.Who] is null)
        {
            return;
        }
        foreach(var index in VirtualPlayersIndex)
        {
            var vplayer = VirtualPlayersData[index].Player;
            NetSender.PlayerActive(index, true);
            NetSender.SyncPlayer(vplayer);
            Utils.RestoreInventory(vplayer);
            NetSender.PlayerControls(index, (vplayer.position == Vector2.Zero ? new((Main.spawnTileX - 1) * 16, (Main.spawnTileY - 3) * 16) : vplayer.position), Vector2.Zero, vplayer.direction == 1);
            NetSender.PlayerLifeMana(vplayer);
        }
    }
    private void OnPlayerUpdate(object? sender, GetDataHandlers.PlayerUpdateEventArgs e)
    {
        if(e.Control.IsUsingItem)
        {
            Console.WriteLine(
                $"""
                {string.Join(" ", typeof(TShockAPI.Models.PlayerUpdate.ControlSet).GetProperties().Select(x => $"{x.Name}:{x.GetGetMethod()!.Invoke(e.Control, Array.Empty<object>())}"))}
                {string.Join(" ", typeof(TShockAPI.Models.PlayerUpdate.MiscDataSet1).GetProperties().Select(x => $"{x.Name}:{x.GetGetMethod()!.Invoke(e.MiscData1, Array.Empty<object>())}"))}
                {string.Join(" ", typeof(TShockAPI.Models.PlayerUpdate.MiscDataSet2).GetProperties().Select(x => $"{x.Name}:{x.GetGetMethod()!.Invoke(e.MiscData2, Array.Empty<object>())}"))}
                {string.Join(" ", typeof(TShockAPI.Models.PlayerUpdate.MiscDataSet3).GetProperties().Select(x => $"{x.Name}:{x.GetGetMethod()!.Invoke(e.MiscData3, Array.Empty<object>())}"))}
                ===
                """);
        }
    }
    private  void OnNewProjectile(object? sender,GetDataHandlers.NewProjectileEventArgs e)
    {
        Console.WriteLine(string.Join(',', e.Ai));
    }
    //public void OnGameUpdate(EventArgs args)
    //{
    //    Num++;
    //    if (Num == 3600)
    //    {
    //        //NetMessage.SendData(MessageID.PlayerActive, -1, -1, null, PlayerIndex, 1);
    //        //NetMessage.SendData(MessageID.SyncPlayer, -1, 1, null, PlayerIndex);
    //        NetSender.PlayerActive(VPlayerIndex, true);
    //        Num = 0;
    //    }
    //}
    private void OnMain_DoUpdateInWorld(On.Terraria.Main.orig_DoUpdateInWorld orig, Main self, System.Diagnostics.Stopwatch sw)
    {
        orig(self, sw);
        VPlayer.Update();
        if (VPlayer.ControlUpdate)
        {
            NetSender.PlayerControls(VPlayerIndex, VPlayer.position, VPlayer.velocity);
            NetSender.PlayerControls(VPlayerIndex, VPlayer.position, VPlayer.velocity);
            NetSender.PlayerControls(VPlayerIndex, VPlayer.position, VPlayer.velocity);
            VPlayer.controlUseItem = false;
        }
        if (VPlayer.LifeUpdate)
        {
            NetSender.PlayerLifeMana(VPlayerIndex, (short)VPlayer.statLife, (short)VPlayer.statLifeMax);
        }
    }
    private void Cmd(CommandArgs args)
    {
        #region 演示
        //var givePlayer = args.Player;
        //if (!givePlayer.RealPlayer)
        //{
        //    if (args.Parameters.Count == 0)
        //    {
        //        args.Player.SendInfoMessage("真正的玩家才能领取蛋糕");
        //        args.Player.SendInfoMessage("请输入需要给蛋糕的玩家");
        //        return;
        //    }
        //    else
        //    {
        //        var findList = TSPlayer.FindByNameOrID(args.Parameters[0]);
        //        if (findList.Count != 1)
        //        {
        //            if (findList.Count == 0)
        //            {
        //                args.Player.SendInfoMessage("没有找到玩家");
        //            }
        //            else
        //            {
        //                args.Player.SendInfoMessage("找到多个玩家");
        //                args.Player.SendMultipleMatchError(findList.Select(x => x.Name));
        //            }
        //            return;
        //        }
        //        givePlayer = findList[0];
        //    }
        //}
        //var inventory = givePlayer.TPlayer.inventory;
        //var findSlot = -1;
        //for (int i = 0; i < 50; i++)
        //{
        //    if (inventory[i] != null && inventory[i].type == 0)
        //    {
        //        findSlot = i;
        //        break;
        //    }
        //}
        //if (findSlot == -1)
        //{
        //    args.Player.SendInfoMessage("被给予玩家背包没有空位");
        //}
        //else
        //{
        //    inventory[findSlot].SetDefaults(ItemID.SliceOfCake);
        //    inventory[findSlot].stack = 1;
        //    NetMessage.SendData(MessageID.SyncEquipment, -1, -1, null, args.Player.Index, findSlot, 0);
        //    args.Player.SendInfoMessage("给予成功");
        //}
        #endregion
        if (args.Parameters.Count == 0)
        {
            return;
        }
        switch (args.Parameters[0])
        {
            case "bind":
                {
                    if (byte.TryParse(args.Parameters[1], out var index) && index < 255)
                    {
                        BindIDs[args.Player.Index] = index;
                    }
                }
                break;
            case "init":
                {
                    VPlayer.active = true;
                    NetSender.PlayerActive(VPlayerIndex, true);
                    VPlayer.position = new(Main.spawnTileX * 16, Main.spawnTileY * 16);
                    NetSender.PlayerSpawn();
                    //NetSender.SyncPlayer();
                    NetSender.SyncPlayer(LoadPlayerData.Player);
                    Utils.RestoreInventory(LoadPlayerData.Player);
                    //NetSender.SyncEquipment(VPlayerIndex, 0, 2223, 1, 82);
                    NetSender.PlayerLifeMana(VPlayerIndex, 250, 250);
                }
                break;
            case "inv":
                {
                    if (args.Parameters.Count < 3)
                    {
                        args.Player.SendInfoMessage("need args");
                        break;
                    }
                    Console.WriteLine($"slot: {args.Parameters[1]} type:{args.Parameters[2]} ");
                    NetSender.SyncEquipment(VPlayerIndex, short.Parse(args.Parameters[1]), short.Parse(args.Parameters[2])); 
                    args.Player.SendInfoMessage("inv");
                }
                break;
            case "active":
                VPlayer.active = true;
                NetSender.PlayerActive(VPlayerIndex, true);
                args.Player.SendInfoMessage("active");
                break;
            case "sync":
                NetSender.SyncPlayer(VPlayer); 
                args.Player.SendInfoMessage("sync player");
                break;
            case "control":
                {
                    var player = args.Player;
                    if (args.Parameters.Count > 1)
                    {
                        if (Utils.FindByNameOrID(args.Parameters[1], out var findPlr))
                        {
                            player = findPlr;
                        }
                        else
                        {
                            player.SendErrorMessage($"查找玩家失败");
                            break;
                        }
                    }
                    if (!player.RealPlayer)
                    {
                        player.SendErrorMessage("非真实玩家");
                        break;
                    }
                    VPlayer.position = player.TPlayer.position;
                    VPlayer.velocity = player.TPlayer.velocity;
                    NetSender.PlayerControls(VPlayerIndex, player.TPlayer.position, player.TPlayer.velocity, player.TPlayer.direction == 1);
                    args.Player.SendInfoMessage("controls");
                }
                break;
            case "spawn":
                {
                    VPlayer.position = new(Main.spawnTileX * 16, Main.spawnTileY * 16);
                    NetSender.PlayerSpawn();
                    args.Player.SendInfoMessage("spawn");
                }
                break;
            case "life":
                {
                    NetSender.PlayerLifeMana(VPlayerIndex, 250, 250);
                    args.Player.SendInfoMessage("life mana");
                }
                break;
            case "save":
                break;
            case "loadplayer":
                var account = TShock.UserAccounts.GetUserAccountByName(args.Parameters[0]);
                var playerData = TShock.CharacterDB.GetPlayerData(null, account.ID);
                playerData.RestoreCharacter(new Player() { whoAmI = 254 });
                break;
            //case "ssc":
            //    {
            //        var player = args.Player;
            //        if (args.Parameters.Count > 1)
            //        {
            //            if (Utils.FindByNameOrID(args.Parameters[1], out var findPlr))
            //            {
            //                player = findPlr;
            //            }
            //            else
            //            {
            //                player.SendErrorMessage($"查找玩家失败");
            //                break;
            //            }
            //        }
            //        if (!player.RealPlayer)
            //        {
            //            player.SendErrorMessage("非真实玩家");
            //            break;
            //        }
            //    }
            //    break;
        }
    }
}