using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.IO;
using Terraria.Net;
using TerrariaApi.Server;
using TShockAPI;

using VBY.Common;
using VBY.Common.Hook;
using VBY.Common.Loader;

namespace VBY.VirtualPlayer;

[ApiVersion(2, 1)]
public class VirtualPlayer : CommonPlugin
{
    public override string Name => "VBY.VirtualPlayer";
    public static List<byte> VirtualPlayersIndex = new();
    public static PlayerFileData[] VirtualPlayersData = new PlayerFileData[255];
    public static Point[] VirtualPlayersSwitchPoint = new Point[255];
    public static byte VPlayerIndex;
    public static int[] BindIDs = new int[255];
    public static ConfigBase<List<VirtualPlayerInfo>> MainConfig = new("Config", "VBY.VirtualPlayer.json", () => new());
    private const byte maxPlayerIndex = 254;
    static VirtualPlayer()
    {
        Array.Fill(BindIDs, -1);
    }
    public VirtualPlayer(Main game) : base(game)
    {
        AddCommands.Add(new(Cmd, "vp"));
    }

    protected override void PreInitialize()
    {
        MainConfig.Load(TSPlayer.Server);
        byte index = maxPlayerIndex; 
        for (int i = 0; i < MainConfig.Instance.Count; i++) 
        {
            var instance = MainConfig.Instance[i];
            if (!string.IsNullOrEmpty(instance.FileName))
            {
                var data = Utils.LoadPlayer(Path.Combine("VirtualPlayer", instance.FileName));
                var player = data.Player;

                instance.PlayerName.NotNullAndEmptySet(ref player.name);
                instance.LifeMax.NotNullSet(ref player.statLifeMax);

                if (instance.SwitchPoint.HasValue)
                {
                    VirtualPlayersSwitchPoint[index] = instance.SwitchPoint.Value;
                }
                player.whoAmI = index;
                player.position = instance.Position + new Vector2(0, 6f);
                player.direction = instance.Direction;
                player.statLife = player.statLifeMax;
                player.active = true;

                VirtualPlayersData[index] = data;
                VirtualPlayersIndex.Add(index);

                VPlayerIndex = index;
                index--;
            }
        }
        new ActionHook(() => On.Terraria.MessageBuffer.GetData += OnGetData).AddTo(this);
       
        //GetDataHandlers.PlayerSpawn.GetHook(OnPlayerSpawn).AddTo(this);
    }
    private void OnGetData(On.Terraria.MessageBuffer.orig_GetData orig, MessageBuffer self, int start, int length, out int messageType)
    {
        if (self.readBuffer[start] == MessageID.PlayerSpawn && Netplay.Clients[self.whoAmI].State == 3)
        {
            orig(self, start, length, out messageType);
            if(Netplay.Clients[self.whoAmI].State == 10)
            {
                foreach (var index in VirtualPlayersIndex)
                {
                    SyncPlayer(index);
                }
            }
            return;
        }
        if (self.readBuffer[start] == MessageID.HitSwitch && TShock.Players[self.whoAmI] is var player and { Active: true})
        {
            var span = new ReadOnlySpan<byte>(self.readBuffer, start + 1, sizeof(UInt16) * 2);
            var x = System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(span);
            var y = System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(span.Slice(sizeof(UInt16)));
            for (int whoAmi = maxPlayerIndex; whoAmi > maxPlayerIndex - VirtualPlayersIndex.Count; whoAmi--)
            {
                if (VirtualPlayersSwitchPoint[whoAmi] == new Point(x, y))
                {
                    var index = player.GetData<int>("Virtual");
                    if (index >= MainConfig.Instance[maxPlayerIndex - whoAmi].Texts.Length)
                    {
                        index = 0;
                    }
                    var vplayer = VirtualPlayersData[whoAmi].Player;
                    if (vplayer.direction == 0 && player.TPlayer.Center.X > vplayer.Center.X)
                    {
                        vplayer.direction = 1;
                        NetSender.PlayerControls(vplayer);
                    }
                    else if (vplayer.direction == 1 && player.TPlayer.Center.X < vplayer.Center.X)
                    {
                        vplayer.direction = 0;
                        NetSender.PlayerControls(vplayer);
                    }
                    NetSender.PlayerText((byte)whoAmi, MainConfig.Instance[maxPlayerIndex - whoAmi].Texts[index]);
                    index++;
                    player.SetData("Virtual", index);
                    break;
                }
            }
        }
        orig(self, start, length, out messageType);
    }
    private void SyncPlayer(byte index)
    {
        var vplayer = VirtualPlayersData[index].Player;
        NetSender.PlayerActive(vplayer);
        NetSender.SyncPlayer(vplayer);
        NetSender.PlayerControls(index, (vplayer.position == Vector2.Zero ? new((Main.spawnTileX - 1) * 16, (Main.spawnTileY - 3) * 16) : vplayer.position), Vector2.Zero, vplayer.direction == 1);
        NetSender.PlayerLifeMana(vplayer);
        Utils.RestoreInventory(vplayer);
    }
    private void Cmd(CommandArgs args)
    {
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
                    SyncPlayer(VPlayerIndex);
                //    var vplayer = VirtualPlayersData[VPlayerIndex].Player;
                //    NetSender.PlayerActive(vplayer);
                //    NetSender.SyncPlayer(vplayer);
                //    NetSender.PlayerControls(vplayer);
                //    Utils.RestoreInventory(vplayer);
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
                NetSender.PlayerActive(VPlayerIndex, true);
                args.Player.SendInfoMessage("active");
                break;
            case "sync":
                NetSender.SyncPlayer(VirtualPlayersData[VPlayerIndex].Player); 
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
                    VirtualPlayersData[VPlayerIndex].Player.position = player.TPlayer.position;
                    VirtualPlayersData[VPlayerIndex].Player.velocity = player.TPlayer.velocity;
                    NetSender.PlayerControls(VPlayerIndex, player.TPlayer.position, player.TPlayer.velocity, player.TPlayer.direction == 1);
                    args.Player.SendInfoMessage("controls");
                }
                break;
            case "spawn":
                {
                    VirtualPlayersData[VPlayerIndex].Player.position = new(Main.spawnTileX * 16, Main.spawnTileY * 16);
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
            case "pvp":
                VirtualPlayersData[VPlayerIndex].Player.hostile = !VirtualPlayersData[VPlayerIndex].Player.hostile;
                NetSender.TogglePVP(VirtualPlayersData[VPlayerIndex].Player);
                args.Player.SendInfoMessage($"pvp:{VirtualPlayersData[VPlayerIndex].Player.hostile}");
                break;
            case "text":
                NetManager.Instance.Broadcast(NetTextModule.SerializeServerMessage(Terraria.Localization.NetworkText.FromLiteral(args.Parameters[1]), Color.White, VPlayerIndex), -1);
                break;
            case "pos":
                if (args.Player.RealPlayer)
                {
                    args.Player.SendInfoMessage($"{args.Player.TPlayer.position}");
                }
                break;
        }
    }
}