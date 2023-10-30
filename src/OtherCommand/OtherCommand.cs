using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;

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
            },
            new Command("other.loadworld", LoadWorld, "loadworld")
            {
                HelpText = "切换世界"
            },
            new Command("other.admin", SpawnCultistRitual, "spawncultistritual")
            {
                HelpText = "生成拜月祭祀活动"
            },
            new Command("other.get", GetObjInfo, "getobjinfo")
            {
                HelpText = "获取对象信息"
            },
            new Command("other.query", ItemQuery, "itemquery")
            {
                HelpText = "查找游戏内物品的一些内容"
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
    public static void LoadWorld(CommandArgs args)
    {
        var inputPath = args.Parameters[0];
        if (File.Exists(inputPath))
        {
            Main.ActiveWorldFileData._path = inputPath;
        }
        else
        {
            var findPath = Path.Combine(Terraria.Program.SavePath, inputPath);
            if (!File.Exists(findPath))
            {
                args.Player.SendErrorMessage($"'{inputPath}' 未找到");
                args.Player.SendErrorMessage($"'{findPath}' 未找到");
                return;
            }
            Main.ActiveWorldFileData._path = findPath;
        }
        WorldGen.serverLoadWorld();
    }
    public static void SpawnCultistRitual(CommandArgs args)
    {
        var x = Main.dungeonX;
        var y = Main.dungeonY;
        var skipCheck = args.Parameters.Any(x => x == "-n");
        if(!skipCheck)
        {
            if (NPC.AnyDanger())
            {
                args.Player.SendInfoMessage("世界处于危险状态");
                return;
            }
            if (WorldGen.PlayerLOS(x - 6, y) || WorldGen.PlayerLOS(x + 6, y))
            {
                args.Player.SendInfoMessage("玩家离刷新点太近");
                return;
            }
            if (!Main.hardMode)
            {
                args.Player.SendInfoMessage("非困难模式");
                return;
            }
            if (!NPC.downedGolemBoss)
            {
                args.Player.SendInfoMessage("未击败石巨人");
                return;
            }
            if (!NPC.downedBoss3)
            {
                args.Player.SendInfoMessage("未击败骷髅王");
                return;
            }
            if (NPC.AnyNPCs(NPCID.CultistTablet))
            {
                args.Player.SendInfoMessage("神秘碑牌已存在");
                return;
            }
        }
        if(y < 7)
        {
            args.Player.SendInfoMessage("地牢点高度<7");
            return;
        }
        if(WorldGen.SolidTile(Main.tile[x, y - 7]))
        {
            args.Player.SendInfoMessage("神秘碑牌生成位置实体物块阻挡");
            return;
        }
        if(!Terraria.GameContent.Events.CultistRitual.CheckFloor(new Vector2(x * 16 + 8, y * 16 - 64 - 8 - 27), out _))
        {
            args.Player.SendInfoMessage("教徒弓箭手站立位置不可用");
            return;
        }
        Main.npc.Any(npc => npc.type is NPCID.CultistTablet or NPCID.CultistDevote or NPCID.CultistArcherBlue, npc => npc.SetActive(false));
        NPC.NewNPC(new Terraria.DataStructures.EntitySource_WorldEvent(), x * 16 + 8, (y - 4) * 16 - 8, NPCID.CultistTablet);
        args.Player.SendSuccessMessage("生成成功");
    }
    internal static Dictionary<string, Type> Types = new List<Type>() { typeof(NPC), typeof(Projectile), typeof(Item) }.ToDictionary(x => x.Name.ToLower());
    public static void GetObjInfo(CommandArgs args)
    {
        if(args.Parameters.Count < 3)
        {
            args.Player.SendInfoMessage("需要3个参数, <obj name> <field name> <default value>");
            return;
        }
        if (Types.TryGetValue(args.Parameters[0].ToLower(), out var type))
        {
            var infos = type.GetMember(args.Parameters[1]);
            if (infos.Length == 0)
            {
                args.Player.SendInfoMessage($"没找到成员:{args.Parameters[1]}");
            }
            else
            {
                if (infos.Length != 1)
                {
                    args.Player.SendInfoMessage("检测到多个成员,不执行");
                    return;
                }
                var memberinfo = infos[0];
                if (memberinfo.MemberType is MemberTypes.Property or MemberTypes.Field)
                {
                    if (!int.TryParse(args.Parameters[2], out var id))
                    {
                        args.Player.SendInfoMessage($"'{args.Parameters[2]}' 转换为 'int' 失败");
                        return;
                    }
                    var obj = Activator.CreateInstance(type);
                    if(type == typeof(NPC))
                    {
                        type.GetMethod(nameof(NPC.SetDefaults))!.Invoke(obj, new object[] { id , default(NPCSpawnParams)});
                    }
                    else
                    {
                        type.GetMethod(nameof(Item.SetDefaults), new Type[] { typeof(int) })!.Invoke(obj, new object[] { id });
                    }
                    if(memberinfo.MemberType == MemberTypes.Property)
                    {
                        args.Player.SendInfoMessage($"{type.Name}.{memberinfo.Name}='{((PropertyInfo)memberinfo).GetGetMethod()!.Invoke(obj, null)}'");
                    }
                    else
                    {
                        args.Player.SendInfoMessage($"{type.Name}.{memberinfo.Name}='{((FieldInfo)memberinfo).GetValue(obj)}'");
                    }
                }
                else
                {
                    args.Player.SendInfoMessage("成员不是属性或方法");
                }
            }
        }
        else
        {
            args.Player.SendInfoMessage($"没找到名称:{args.Parameters[0].ToLower()}");
            args.Player.SendInfoMessage($"全部名称:{Types.Keys.ToList().Join(',')}");
        }
    }
    internal static Dictionary<string, Func<Expression, Expression, BinaryExpression>> OperatorFuncs = new()
    {
        { "=", Expression.Equal },
        { "!=", Expression.NotEqual },
        { "<", Expression.LessThan },
        { "<=", Expression.LessThanOrEqual },
        { ">", Expression.GreaterThan },
        { ">=", Expression.GreaterThanOrEqual }
    };
    internal static List<string> Operators = OperatorFuncs.Keys.ToList();
    public static void ItemQuery(CommandArgs args) 
    {
        if (args.Parameters.Count == 0)
        {
            args.Player.SendInfoMessage("/itemquery chest <member> <operator> <value>");
            return;
        }
        switch (args.Parameters[0])
        {
            case "chest":
                {
                    if (args.Parameters.Count < 4)
                    {
                        args.Player.SendInfoMessage("参数少于4个");
                        break;
                    }
                    var expression = args.Message.RemoveRecpat(' ').SubstringAfter(' ', 2);
                    var index = 1;
                    var memberName = args.Parameters[index++];
                    var operatorStr = args.Parameters[index++];
                    var operandStr = args.Parameters[index++];
                    var type = typeof(Item);
                    if(!Utils.GetMemeber(type, memberName, args.Player, out var member))
                    {
                        break;
                    }
                    if (!Operators.Contains(operatorStr))
                    {
                        args.Player.SendErrorMessage($"'{operandStr}' 无效操作符");
                        break;
                    }
                    if (!Regex.IsMatch(operandStr, @"-{0,1}\d{1,4}"))
                    {
                        args.Player.SendErrorMessage($"无效操作数");
                        break;
                    }
                    var parameterExpression = Expression.Parameter(type);
                    var lambdaExpression = Expression.Lambda<Func<Item, bool>>(
                        OperatorFuncs[operatorStr](
                            member.MemberType == MemberTypes.Field
                            ? Expression.Field(parameterExpression, (FieldInfo)member)
                            : Expression.Property(parameterExpression, (PropertyInfo)member),
                            Expression.Constant(int.Parse(operandStr), typeof(int))),
                        parameterExpression);
                    var func = lambdaExpression.Compile();
                    var findItems = new List<(int index, Item item)>();
                    var find = false;
                    foreach (var chest in Main.chest)
                    {
                        if(chest is null)
                        {
                            continue;
                        }
                        findItems.Clear();
                        bool itemFind = false;
                        var chestItems = chest.item;
                        for(int i = 0; i < chestItems.Length; i++)
                        {
                            var chestItem = chestItems[i];
                            if (chestItem is not null && func(chestItem))
                            {
                                itemFind = true;
                                findItems.Add((i, chestItem));
                            }
                        }
                        if (itemFind) 
                        {
                            string heightText;
                            if(chest.y < Main.worldSurface * 0.3499999940395355)
                            {
                                heightText = $"太空";
                            }
                            else if(chest.y < Main.worldSurface)
                            {
                                heightText = $"地表";
                            }
                            else if(chest.y > Main.UnderworldLayer)
                            {
                                heightText = $"地狱";
                            }
                            else if(chest.y > Main.rockLayer)
                            {
                                heightText = $"洞穴";
                            }
                            else
                            {
                                heightText = $"地下";
                            }
                            heightText = $"{heightText}{Math.Abs((chest.y - Main.worldSurface) * 2)}";
                            args.Player.SendInfoMessage($"chest.x:{chest.x,-4} chest.y:{chest.y,-4} {(chest.x > Main.maxTilesX / 2 ? "西" : "东")}{Math.Abs((chest.x - Main.maxTilesX / 2) * 2),-4} {heightText}");
                            findItems.ForEach(x => args.Player.SendInfoMessage($"index:{x.index,-2} id:{x.item.type,-4} stack:{x.item.stack,-4} name:{x.item.Name}"));
                            find = true;
                        }

                        foreach(var ply in TShock.Players)
                        {
                            if(ply is null) continue;
                            if (ply.Dead)
                            {
                                //ply.SendInfoMessage($"你还有{}秒复活")
                            }
                        }
                    }
                    if (!find)
                    {
                        args.Player.SendInfoMessage("没有搜索到符合条件的箱子内物品");
                    }
                }
                break;
            default:
                args.Player.SendInfoMessage($"unknown parameter '{args.Parameters[0]}'");
                break;
        }
    }
}

