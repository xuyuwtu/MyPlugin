using System.Data;
using System.Reflection;

using Terraria;
using Terraria.ID;
using TerrariaApi.Server;

using TShockAPI;
using TShockAPI.DB;

namespace VBY.OtherCommand;
[ApiVersion(2, 1)]
public class OtherCommand : TerrariaPlugin
{
    public override string Name => "VBY.OtherCommand";
    public override string Author => "yu";
    public override string Description => "一些其他的辅助命令";
    private Command[] AddCommands;
    public OtherCommand(Main game) : base(game) 
    {
        AddCommands = new Command[] {
            new Command(Permissions.spawnmob, SpawnMob, "spawnmobply", "smp")
            {
                HelpText = "生成一定数量的NPC到玩家附近."
            },
            new Command(Permissions.user, ManageUsers, "ouser")
            {
                HelpText = "用户管理"
            },
            new Command("other.admin", PaintAnyTile, "paintworld")
            {
                HelpText = "给整个世界涂漆"
            }
        };
    }

    public override void Initialize()
    {
        Commands.ChatCommands.AddRange(AddCommands);
    }
    protected override void Dispose(bool disposing)
    {
        if(disposing)
        {
            AddCommands.ForEach(x => Commands.ChatCommands.Remove(x));
        }
        base.Dispose(disposing);
    }
    private static void ManageUsers(CommandArgs args)
    {
        if (args.Parameters.Count < 1)
        {
            args.Player.SendErrorMessage("指令ouser的用法错误. 使用 {0}ouser 获取帮助.", Commands.Specifier);
            return;
        }
        string text = args.Parameters[0];
        if (text == "del" && args.Parameters.Count == 2)
        {
            UserAccount account = new()
            {
                Name = args.Parameters[1]
            };
            TShock.Players.Where((TSPlayer p) => p != null && p.IsLoggedIn && p.Account.Name == account.Name).ForEach(p => p.Logout());
            int updateRowCount = ((IDbConnection)typeof(UserAccountManager).GetField("_database", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(TShock.UserAccounts)!).Query("DELETE FROM Users WHERE Username=@0", account.Name);
            if (updateRowCount < 1)
            {
                args.Player.SendErrorMessage("没有找到用户 {0}", account.Name);
            }
            else if(updateRowCount == 1)
            {
                args.Player.SendSuccessMessage("删除成功");
            }
            else
            {
                args.Player.SendInfoMessage("删除了{1}个名为 {0} 的用户", account.Name, updateRowCount);
            }
            return;
        }
        else if (text == "exists" && args.Parameters.Count == 2)
        {
            UserAccount account = new()
            {
                Name = args.Parameters[1]
            };
            TShock.Players.Where((TSPlayer p) => p != null && p.IsLoggedIn && p.Account.Name == account.Name).ForEach(p => p.Logout());
            using var reader = ((IDbConnection)typeof(UserAccountManager).GetField("_database", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(TShock.UserAccounts)!).QueryReader("SELECT * FROM Users WHERE Username=@0", account.Name);
            int userCount = 0;
            while (reader.Read())
            {
                userCount++;
            }
            if(userCount == 0)
            {
                args.Player.SendInfoMessage("用户不存在");
            }
            else if(userCount == 1)
            {
                args.Player.SendInfoMessage("用户存在");
            }
            else
            {
                args.Player.SendInfoMessage("用户 {0} 存在，数量:{1}", account.Name, userCount);
            }
            return;
        }
        if (text == "help")
        {
            args.Player.SendInfoMessage("{0}ouser del username -- 移除指定用户", Commands.Specifier);
            args.Player.SendInfoMessage("{0}ouser exists username -- 查看指定用户是否存在", Commands.Specifier);
        }
        else
        {
            args.Player.SendErrorMessage("指令ouser的用法错误. 使用 {0}ouser 获取帮助.", Commands.Specifier);
        }
    }
    public static void SpawnMob(CommandArgs args)
    {
        if (args.Parameters.Count < 2 || args.Parameters.Count > 3)
        {
            args.Player.SendErrorMessage("参数错误. 正确格式: {0}spawnmobply <player> <npc> [count=1].", Commands.Specifier);
            return;
        }

        var findPlayers = TSPlayer.FindByNameOrID(args.Parameters[0]);
        if(findPlayers.Count == 0)
        {
            args.Player.SendInfoMessage("没有找到玩家");
            return;
        }
        if(findPlayers.Count > 1)
        {
            args.Player.SendMultipleMatchError(findPlayers.Select(p => p.Name));
            return;
        }

        var findNPCs = TShock.Utils.GetNPCByIdOrName(args.Parameters[1]);
        if (findNPCs.Count == 0)
        {
            args.Player.SendErrorMessage("无效NPC类型!");
            return;
        }
        if (findNPCs.Count > 1)
        {
            args.Player.SendMultipleMatchError(findNPCs.Select((NPC n) => $"{n.FullName}({n.type})"));
            return;
        }

        int spawnCount = 1;
        if (args.Parameters.Count == 3 && (!int.TryParse(args.Parameters[2], out spawnCount) || spawnCount < 1))
        {
            args.Player.SendErrorMessage("数量输入错误");
            return;
        }
        spawnCount = Math.Min(spawnCount, 200);
        NPC npc = findNPCs[0];
        if (npc.type > 0 && npc.type < NPCID.Count && npc.type != 113)
        {
            TSPlayer.Server.SpawnNPC(npc.netID, npc.FullName, spawnCount, findPlayers[0].TileX, findPlayers[0].TileY, 50, 20);
            if (args.Silent)
            {
                args.Player.SendSuccessMessage("生成了 {0}个 {1}.", spawnCount, npc.FullName);
            }
            else
            {
                TSPlayer.All.SendSuccessMessage("{0} 生成了 {1}个 {2} 在 {3} 附近.", args.Player.Name, spawnCount, npc.FullName, findPlayers[0].Name);
            }
        }
        else if (npc.type == 113)
        {
            if (Main.wofNPCIndex != -1 || findPlayers[0].Y / 16f < (Main.maxTilesY - 205))
            {
                args.Player.SendErrorMessage("无法生成血肉墙.不在地狱世界上已有血肉墙存在");
                return;
            }
            NPC.SpawnWOF(findPlayers[0].TPlayer.position);
            if (args.Silent)
            {
                args.Player.SendSuccessMessage("生成了一个血肉墙");
                return;
            }
            TSPlayer.All.SendSuccessMessage("{0} 生成了一个血肉墙在 {1} 附近.", args.Player.Name, findPlayers[0]);
        }
        else
        {
            args.Player.SendErrorMessage("无效NPC类型.");
        }
    }
    public static void PaintAnyTile(CommandArgs args)
    {
        if(args.Parameters.Count == 0)
        {
            args.Player.SendInfoMessage("请输入油漆id 或者 random");
            return;
        }
        if (args.Parameters[0].ToLower() == "random")
        {
            var max = typeof(PaintID).GetFields().Where(x => x is { IsStatic: true, IsLiteral: true } && x.FieldType == typeof(byte)).Select(x => (byte)x.GetValue(null)!).Max() + 1;
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    if (Main.tile[x, y] is not null)
                    {
                        var paintColor = (byte)Main.rand.Next(0, max);
                        if(y - 1 >= 0 && Main.tile[x, y - 1] is not null)
                        {
                            paintColor = Main.tile[x, y - 1].color();
                            if (Main.tile[x, y - 1].wall != 0) 
                            {
                                paintColor = Main.tile[x, y - 1].wallColor();
                            }
                        }
                        Main.tile[x, y].color(paintColor);
                        if (Main.tile[x, y].wall != 0)
                        {
                            Main.tile[x, y].wallColor(paintColor);
                        }
                    }
                }
            }
            args.Player.SendSuccessMessage("涂漆完成");
            return;
        }
        if (byte.TryParse(args.Parameters[0], out var paintid) && paintid >= 0 && paintid < 32)
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    if (Main.tile[x, y] is not null)
                    {
                        Main.tile[x, y].color(paintid);
                        if (Main.tile[x, y].wall != 0)
                        {
                            Main.tile[x, y].wallColor(paintid);
                        }
                    }
                }
            }
            args.Player.SendSuccessMessage("涂漆完成");
        }
        else
        {
            args.Player.SendErrorMessage("无效ID");
        }
    }
}