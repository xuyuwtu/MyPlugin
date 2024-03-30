using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ObjectData;
using TerrariaApi.Server;

using TShockAPI;
using TShockAPI.DB;

using VBY.Common;
using VBY.Common.Loader;

namespace VBY.OtherCommand;
[ApiVersion(2, 1)]
[Description("一些其他的辅助命令")]
public class OtherCommand : CommonPlugin
{
    public override string Author => "yu";
    public OtherCommand(Main game) : base(game) 
    {
        AddCommands.AddRange(new Command[] {
            new (Permissions.spawnmob, SpawnMob, "spawnmobply", "smp")
            {
                HelpText = "生成一定数量的NPC到玩家附近."
            },
            new (Permissions.user, ManageUsers, "ouser")
            {
                HelpText = "用户管理"
            },
            new ("other.admin", PaintAnyTile, "paintworld")
            {
                HelpText = "给整个世界涂漆"
            },
            new ("other.admin", LoadWorld, "loadworld")
            {
                HelpText = "切换世界"
            },
            new ("other.admin", SpawnCultistRitual, "spawncultistritual")
            {
                HelpText = "生成拜月祭祀活动"
            },
            new ("other.admin", GetObjInfo, "getobjinfo")
            {
                HelpText = "获取对象信息"
            },
            new ("other.admin", ItemQuery, "itemquery")
            {
                HelpText = "查找游戏内物品的一些内容"
            },
            new ("other.admin", ClearWorld, "clearworld")
            {
                HelpText = "清理世界"
            },
            new (Permissions.kick, ListConnected, "conwho"),
            new (Permissions.kick, Kick, "conkick")
        });
        Loaders.Add(new ReplaceCommand[]{ new("warp", Warp) }.GetLoader(x => x.Replace(), x => x.Restore(), null, true));
    }
    public static void ManageUsers(CommandArgs args)
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
        else if (text == "hasperm" && args.Parameters.Count == 3)
        {
            var group = TShock.Groups.GetGroupByName(args.Parameters[1]);
            if (group is null)
            {
                args.Player.SendInfoMessage($"组 '{args.Parameters[1]}' 未找到");
                return;
            }
            if (group.HasPermission(args.Parameters[2]))
            {
                args.Player.SendSuccessMessage($"组 '{args.Parameters[1]}' 有权限 '{args.Parameters[2]}'");
            }
            else
            {
                args.Player.SendInfoMessage($"组 '{args.Parameters[1]}' 无权限 '{args.Parameters[2]}'");
            }
        }
        else if (text == "help")
        {
            args.Player.SendInfoMessage("{0}ouser del username -- 移除指定用户", Commands.Specifier);
            args.Player.SendInfoMessage("{0}ouser exists username -- 查看指定用户是否存在", Commands.Specifier);
            args.Player.SendInfoMessage("{0}ouser hasperm groupname perm -- 查看指定组是否有特定权限", Commands.Specifier);
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
        if(args.Parameters.Count == 0)
        {
            args.Player.SendInfoMessage("/loadworld <filename>");
            return;
        }
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
        TShock.Players.Where(x => x is not null && x.Active).ForEach(x => x.Kick("正在重新加载地图...", true, true, null, true));
        WorldGen.clearWorld();
        WorldGen.serverLoadWorld().Wait();
        WorldGen.setWorldSize();
        for (int i = 0; i < Netplay.Clients.Length; i++)
        {
            Netplay.Clients[i].orig_ctor_RemoteClient();
        }
        Console.WriteLine($"WorldSize:{Main.maxTilesX} {Main.maxTilesY}");
    }
    public static void SpawnCultistRitual(CommandArgs args)
    {
        var x = Main.dungeonX;
        var y = Main.dungeonY;
        var skipCheck = args.Parameters.Any(x => x == "-i");
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
        NPC.NewNPC(new EntitySource_WorldEvent(), x * 16 + 8, (y - 4) * 16 - 8, NPCID.CultistTablet);
        args.Player.SendSuccessMessage("生成成功");
    }
    internal static Dictionary<string, Type> Types = new List<Type>() { typeof(NPC), typeof(Projectile), typeof(Item) }.ToDictionary(x => x.Name.ToLower());
    public static void GetObjInfo(CommandArgs args)
    {
        var enumerator = args.Parameters.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            args.Player.SendInfoMessage("select <member name> [member2 name]");
            args.Player.SendInfoMessage("<obj name> <member name> <default value>");
            return;
        }
        if (enumerator.Current.Equals("select", StringComparison.OrdinalIgnoreCase))
        {
            if (!args.Player.RealPlayer)
            {
                args.Player.SendInfoMessage("非真实玩家不可执行");
                return;
            }
            if (args.Player.SelectedItem.IsAir)
            {
                args.Player.SendInfoMessage("当前所选空物品");
                return;
            }
            if (!enumerator.MoveNext())
            {
                args.Player.SendInfoMessage("请输入属性名");
                return;
            }
            do
            {
                var members = typeof(Item).GetMember(enumerator.Current, MemberTypes.Field | MemberTypes.Property, BindingFlags.Public | BindingFlags.Instance);
                if (members.Length == 0)
                {
                    args.Player.SendInfoMessage($"没找到属性 '{enumerator.Current}'");
                    return;
                }
                foreach (var member in members)
                {
                    if (member.MemberType == MemberTypes.Field)
                    {
                        args.Player.SendInfoMessage($"{member.Name}:'{((FieldInfo)member).GetValue(args.Player.SelectedItem)}'");
                    }
                    else
                    {
                        args.Player.SendInfoMessage($"{member.Name}:'{((PropertyInfo)member).GetValue(args.Player.SelectedItem)}'");
                    }
                }
            } while(enumerator.MoveNext());
            return;
        }
        if(enumerator.Current.Equals("worlduuid", StringComparison.OrdinalIgnoreCase))
        {
            args.Player.SendInfoMessage(Main.ActiveWorldFileData.UniqueId.ToString());
            return;
        }
        if(args.Parameters.Count < 3)
        {
            args.Player.SendInfoMessage("需要3个参数, <obj name> <member name> <type>");
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
                        args.Player.SendInfoMessage($"{type.Name}.{memberinfo.Name}='{((PropertyInfo)memberinfo).GetValue(obj)}'");
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
    public static void ItemQuery(CommandArgs args) 
    {
        var enumerator = args.Parameters.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            args.Player.SendInfoMessage("/itemquery chest <member> <operator> <value>");
            args.Player.SendInfoMessage("/itemquery player <member> <operator> <value>");
            return;
        }
        switch (enumerator.Current)
        {
            case "chest":
                {
                    if (args.Parameters.Count < 4)
                    {
                        args.Player.SendInfoMessage("参数少于4个");
                        break;
                    }
                    if(!Utils.GetLambdaExpression(args.Player, args.Parameters, ref enumerator, out var lambdaExpression))
                    {
                        break;
                    }
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
                    }
                    if (!find)
                    {
                        args.Player.SendInfoMessage("没有搜索到符合条件的箱子内物品");
                    }
                }
                break;
            case "player":
                {
                    if (args.Parameters.Count < 4)
                    {
                        args.Player.SendInfoMessage("参数少于4个");
                        break;
                    }
                    if(!Utils.GetLambdaExpression(args.Player, args.Parameters, ref enumerator, out var lambdaExpression))
                    {
                        break;
                    }
                    var func = lambdaExpression.Compile();
                    var findPlayers = new List<(Player player, List<(string name, List<(int index, Item item)> findItems)> groups)>();
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        ref var player = ref Main.player[i];
                        if (player is null || !player.active) continue;
                        var arrays = new List<(string name, Item[] items)>() {
                            ("背包", player.inventory),
                            ("盔甲", player.armor),
                            ("染料", player.dye),
                            ("杂项", player.miscEquips),
                            ("杂项染料", player.miscDyes),
                            ("猪猪", player.bank.item),
                            ("保险柜", player.bank2.item),
                            ("垃圾桶", new Item[] { player.trashItem }),
                            ("护卫熔炉", player.bank3.item),
                            ("虚空袋", player.bank4.item)};
                        var findGroups = new List<(string, List<(int, Item)>)>();
                        foreach (var arrayInfo in arrays)
                        {
                            var findItems = new List<(int, Item)>();
                            for (int j = 0; j < arrayInfo.items.Length; j++)
                            {
                                var item = arrayInfo.items[j];
                                if (item is not null && func(item))
                                {
                                    findItems.Add((j, item));
                                }
                            }
                            if (findItems.Count > 0)
                            {
                                findGroups.Add((arrayInfo.name, findItems));
                            }
                        }
                        if (findGroups.Count > 0)
                        {
                            findPlayers.Add((player, findGroups));
                        }
                    }
                    if (findPlayers.Count == 0)
                    {
                        args.Player.SendInfoMessage("没有玩家有符合条件的物品");
                    }
                    else
                    {
                        foreach (var playerInfo in findPlayers)
                        {
                            args.Player.SendInfoMessage($"player.name:'{playerInfo.player.name}'");
                            foreach (var groupInfo in playerInfo.groups)
                            {
                                args.Player.SendInfoMessage($"group:{groupInfo.name}");
                                foreach (var itemInfo in groupInfo.findItems)
                                {
                                    args.Player.SendInfoMessage($"index:{itemInfo.index} id:{itemInfo.item.type} stack:{itemInfo.item.stack} name:{itemInfo.item.Name}");
                                }
                            }
                        }
                    }
                }
                break;
            default:
                args.Player.SendInfoMessage($"unknown parameter '{args.Parameters[0]}'");
                break;
        }
    }
    public static void ClearWorld(CommandArgs args)
    {
        var enumerator = args.Parameters.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            args.Player.SendInfoMessage("/clearworld empty");
            args.Player.SendInfoMessage("/clearworld airisland");
            return;
        }
        switch(enumerator.Current)
        {
            case "empty":
                foreach(ref var chest in Main.chest.AsSpan())
                {
                    chest = null;
                }
                for (int x = 0; x < Main.maxTilesX; x++)
                {
                    for (int y = 0; y < Main.maxTilesX; y++) 
                    {
                        Main.tile[x, y]?.ClearEverything();
                    }
                }
                break;
            case "airisland":
                {
                    var skipTile = new bool[Main.maxTilesX, Main.maxTilesY];
                    var skipTileObject = new bool[Main.maxTilesX, Main.maxTilesY];
                    for (int i = 0; i < Main.maxChests; i++)
                    {
                        ref var chest = ref Main.chest[i];
                        if (chest is null)
                        {
                            continue;
                        }
                        int width = TileID.Sets.BasicChest[Main.tile[chest.x, chest.y].type] ? 2 : 3;
                        for (int x = 0; x < width; x++)
                        {
                            int y = 0;
                            int tx = chest.x + x, ty;
                            for (; y < 2; y++)
                            {
                                ty = chest.y + y;
                                Main.tile[tx, ty].Clear(TileDataType.Wall | TileDataType.Liquid | TileDataType.Slope);
                                skipTile[tx, ty] = true;
                                skipTileObject[tx, ty] = true;
                            }
                            ty = chest.y + y;
                            Main.tile[tx, ty].Clear(TileDataType.Wall | TileDataType.Liquid);
                            skipTile[tx, ty] = true;
                        }
                    }

                    for (int x = 0; x < Main.maxTilesX; x++)
                    {
                        for (int y = 0; y < Main.maxTilesY; y++)
                        {
                            var tile = Main.tile[x, y];
                            if (tile is null || skipTile[x, y])
                            {
                                continue;
                            }
                            if (tile.shimmer())
                            {
                                int shimmerStartX = x;
                                int shimmerStartY = y;
                                int shimmerRight = x;
                                int shimmerBottom = y;
                                for (int right = x; right < Main.maxTilesX; right++)
                                {
                                    if (!Main.tile[right, y].shimmer())
                                    {
                                        shimmerRight = right;
                                        break;
                                    }
                                }
                                for (int right = x; right < shimmerRight; right++)
                                {
                                    for (int bottom = y; bottom < Main.maxTilesY; bottom++)
                                    {
                                        if (!Main.tile[right, bottom].shimmer())
                                        {
                                            if (bottom > shimmerBottom)
                                            {
                                                shimmerBottom = bottom;
                                            }
                                            break;
                                        }
                                    }
                                }
                                for (int start = shimmerStartX - 2; start < shimmerRight + 2; start++)
                                {
                                    for (int end = shimmerStartY; end < shimmerBottom + 2; end++)
                                    {
                                        skipTile[start, end] = true;
                                    }
                                }
                            }
                            else if (tile.type == TileID.DemonAltar)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    int tx = x + i;
                                    int ty;
                                    int j = 0;
                                    for (; j < 2; j++)
                                    {
                                        ty = y + j;
                                        Main.tile[tx, ty].Clear(TileDataType.Wall | TileDataType.Liquid);
                                        skipTile[tx, ty] = true;
                                    }
                                    ty = y + j;
                                    Main.tile[tx, ty].Clear(TileDataType.Wall | TileDataType.Liquid);
                                    skipTile[tx, ty] = true;
                                }
                            }
                        }
                    }

                    for (int x = 0; x < Main.maxTilesX; x++)
                    {
                        for (int y = 0; y < Main.maxTilesY; y++)
                        {
                            var tile = Main.tile[x, y];
                            if (tile is null)
                            {
                                skipTile[x, y] = true;
                                continue;
                            }
                            if (skipTile[x, y])
                            {
                                continue;
                            }

                            bool @continue = false;
                            foreach (var info in Utils.AirIsland.TileClearInfos)
                            {
                                if (info.TileIDContains(tile.type))
                                {
                                    if (info.ClearWall && !info.WallIDContains(tile.wall))
                                    {
                                        tile.Clear(TileDataType.Wall);
                                    }
                                    @continue = true;
                                    break;
                                }
                                if (info.WallIDContains(tile.wall))
                                {
                                    @continue = true;
                                    break;
                                }
                            }
                            if (@continue || Utils.AirIsland.SkipWallIDs.Contains(tile.wall))
                            {
                                skipTile[x, y] = true;
                                continue;
                            }
                        }
                    }

                    for (int x = 0; x < Main.maxTilesX; x++)
                    {
                        for (int y = 0; y < Main.maxTilesY; y++)
                        {
                            var tile = Main.tile[x, y];
                            if (tile is null || skipTileObject[x, y])
                            {
                                continue;
                            }

                            var data = TileObjectData.GetTileData(tile);
                            if (data is null)
                            {
                                if (!skipTile[x, y])
                                {
                                    tile.ClearEverything();
                                }
                                continue;
                            }
                            bool allSkip = true;
                            for (int i = 0; i < data.Width; i++)
                            {
                                for (int j = 0; j < data.Height; j++)
                                {
                                    allSkip = skipTile[x + i, y + j];
                                    if (!allSkip)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (allSkip)
                            {
                                for (int i = 0; i < data.Width; i++)
                                {
                                    for (int j = 0; j < data.Height; j++)
                                    {
                                        skipTileObject[x + i, y + j] = true;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < data.Width; i++)
                                {
                                    for (int j = 0; j < data.Height; j++)
                                    {
                                        int tx = x + i, ty = y + j;
                                        if (skipTile[tx, ty])
                                        {
                                            Main.tile[tx, ty].Clear(TileDataType.Tile);
                                        }
                                        else
                                        {
                                            Main.tile[tx, ty].ClearEverything();
                                        }
                                        skipTileObject[tx, ty] = true;
                                    }
                                }
                            }
                        }
                    }

                    for (int x = Main.spawnTileX - 5; x <= Main.spawnTileX + 5; x++)
                    {
                        for (int y = Main.spawnTileY; y <= Main.spawnTileY + 10; y++)
                        {
                            if (Main.tile[x, y] is null)
                            {
                                Main.tile[x, y] = OTAPI.Hooks.Tile.InvokeCreate();
                            }
                            Main.tile[x, y].type = 2;
                            Main.tile[x, y].active(true);
                        }
                    }
                    WorldGen.Place3x2(Main.spawnTileX, Main.spawnTileY - 1, 26, 0);
                    WorldGen.GrowTree(Main.spawnTileX - 4, Main.spawnTileY);
                    WorldGen.GrowTree(Main.spawnTileX + 4, Main.spawnTileY);
                }
                break;
        }
    }
    public static void Warp(CommandArgs args)
    {
        bool hasManageWarpPermission = args.Player.HasPermission(Permissions.managewarp);
        if (args.Parameters.Count < 1)
        {
            if (hasManageWarpPermission)
            {
                args.Player.SendInfoMessage(I18n.GetString("Invalid syntax. Proper syntax: {0}warp [command] [arguments].", Commands.Specifier));
                args.Player.SendInfoMessage(I18n.GetString("Commands: add, del, hide, list, send, [warpname]."));
                args.Player.SendInfoMessage(I18n.GetString("Arguments: add [warp name], del [warp name], list [page]."));
                args.Player.SendInfoMessage(I18n.GetString("Arguments: send [player] [warp name], hide [warp name] [Enable(true/false)]."));
                args.Player.SendInfoMessage(I18n.GetString("Examples: {0}warp add foobar, {0}warp hide foobar true, {0}warp foobar.", Commands.Specifier));
            }
            else
            {
                args.Player.SendErrorMessage(I18n.GetString("Invalid syntax. Proper syntax: {0}warp [name] or {0}warp list <page>.", Commands.Specifier));
            }
            return;
        }
        if (args.Parameters[0].Equals("list"))
        {
            if (PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out var pageNumber))
            {
                IEnumerable<string> warpNames = from warp in TShock.Warps.Warps
                                                where !warp.IsPrivate
                                                select warp.Name;
                PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(warpNames), new PaginationTools.Settings
                {
                    HeaderFormat = I18n.GetString("Warps ({{0}}/{{1}}):"),
                    FooterFormat = I18n.GetString("Type {0}warp list {{0}} for more.", Commands.Specifier),
                    NothingToDisplayString = I18n.GetString("There are currently no warps defined.")
                });
            }
            return;
        }
        if (args.Parameters[0].ToLower() == "add" && hasManageWarpPermission)
        {
            if (args.Parameters.Count == 2)
            {
                string warpName5 = args.Parameters[1];
                switch (warpName5)
                {
                    case "list":
                    case "hide":
                    case "del":
                    case "add":
                        args.Player.SendErrorMessage(I18n.GetString("Invalid warp name. The names 'list', 'hide', 'del' and 'add' are reserved for commands."));
                        return;
                }
                if(warpName5.Length > 6)
                {
                    args.Player.SendErrorMessage("名称过长");
                    return;
                }
                if (TShock.Warps.Add(args.Player.TileX, args.Player.TileY, warpName5))
                {
                    args.Player.SendSuccessMessage(I18n.GetString($"Warp added: {warpName5}."));
                }
                else
                {
                    args.Player.SendErrorMessage(I18n.GetString($"Warp {warpName5} already exists."));
                }
            }
            else
            {
                args.Player.SendErrorMessage(I18n.GetString("Invalid syntax. Proper syntax: {0}warp add [name].", Commands.Specifier));
            }
            return;
        }
        if (args.Parameters[0].ToLower() == "del" && hasManageWarpPermission)
        {
            if (args.Parameters.Count == 2)
            {
                string warpName4 = args.Parameters[1];
                if (TShock.Warps.Remove(warpName4))
                {
                    args.Player.SendSuccessMessage(I18n.GetString($"Warp deleted: {warpName4}"));
                }
                else
                {
                    args.Player.SendErrorMessage(I18n.GetString($"Could not find a warp named {warpName4} to remove."));
                }
            }
            else
            {
                args.Player.SendErrorMessage(I18n.GetString("Invalid syntax. Proper syntax: {0}warp del [name].", Commands.Specifier));
            }
            return;
        }
        if (args.Parameters[0].ToLower() == "hide" && hasManageWarpPermission)
        {
            if (args.Parameters.Count == 3)
            {
                string warpName3 = args.Parameters[1];
                bool state = false;
                if (bool.TryParse(args.Parameters[2], out state))
                {
                    if (TShock.Warps.Hide(args.Parameters[1], state))
                    {
                        if (state)
                        {
                            args.Player.SendSuccessMessage(I18n.GetString("Warp {0} is now private.", warpName3));
                        }
                        else
                        {
                            args.Player.SendSuccessMessage(I18n.GetString("Warp {0} is now public.", warpName3));
                        }
                    }
                    else
                    {
                        args.Player.SendErrorMessage(I18n.GetString("Could not find specified warp."));
                    }
                }
                else
                {
                    args.Player.SendErrorMessage(I18n.GetString("Invalid syntax. Proper syntax: {0}warp hide [name] <true/false>.", Commands.Specifier));
                }
            }
            else
            {
                args.Player.SendErrorMessage(I18n.GetString("Invalid syntax. Proper syntax: {0}warp hide [name] <true/false>.", Commands.Specifier));
            }
            return;
        }
        if (args.Parameters[0].ToLower() == "send" && args.Player.HasPermission(Permissions.tpothers))
        {
            if (args.Parameters.Count < 3)
            {
                args.Player.SendErrorMessage(I18n.GetString("Invalid syntax. Proper syntax: {0}warp send [player] [warpname].", Commands.Specifier));
                return;
            }
            List<TSPlayer> foundplr = TSPlayer.FindByNameOrID(args.Parameters[1]);
            if (foundplr.Count == 0)
            {
                args.Player.SendErrorMessage(I18n.GetString("Invalid target player."));
                return;
            }
            if (foundplr.Count > 1)
            {
                args.Player.SendMultipleMatchError(foundplr.Select((TSPlayer p) => p.Name));
                return;
            }
            string warpName2 = args.Parameters[2];
            Warp warp3 = TShock.Warps.Find(warpName2);
            TSPlayer plr = foundplr[0];
            if (warp3 != null)
            {
                if (plr.Teleport(warp3.Position.X * 16, warp3.Position.Y * 16, 1))
                {
                    plr.SendSuccessMessage(I18n.GetString("{0} warped you to {1}.", args.Player.Name, warpName2));
                    args.Player.SendSuccessMessage(I18n.GetString("You warped {0} to {1}.", plr.Name, warpName2));
                }
            }
            else
            {
                args.Player.SendErrorMessage(I18n.GetString($"The destination warp, {warpName2}, was not found."));
            }
            return;
        }
        string warpName = string.Join(" ", args.Parameters);
        Warp warp2 = TShock.Warps.Find(warpName);
        if (warp2 != null)
        {
            if (args.Player.Teleport(warp2.Position.X * 16, warp2.Position.Y * 16, 1))
            {
                args.Player.SendSuccessMessage(I18n.GetString($"Warped to {warpName}."));
            }
        }
        else
        {
            args.Player.SendErrorMessage(I18n.GetString($"The destination warp, {warpName}, was not found."));
        }
    }
    public static void ListConnected(CommandArgs args)
    {
        if (Utils.GetActiveConnectionCount() == 0)
        {
            args.Player.SendMessage("当前没有活动连接", Color.White);
            return;
        }
        var players = new List<string>();
        foreach (TSPlayer ply in TShock.Players)
        {
            if (ply is { Active: true } or { ConnectionAlive: true })
            {
                if (ply.Active)
                {
                    if (ply.Account is null)
                    {
                        players.Add(I18n.GetString($"{ply.Name} (Index: {ply.Index})"));
                    }
                    else
                    {
                        players.Add(I18n.GetString($"{ply.Name} (Index: {ply.Index}, Account ID: {ply.Account.ID})"));
                    }
                }
                else
                {
                    if (ply.Account is null)
                    {
                        players.Add(I18n.GetString($"{ply.Name} (Index: {ply.Index})") + "(Connected Only)");
                    }
                    else
                    {
                        players.Add(I18n.GetString($"{ply.Name} (Index: {ply.Index}, Account ID: {ply.Account.ID})") + "(Connected Only)");
                    }
                }
            }
        }
        PaginationTools.SendPage(args.Player, 1, PaginationTools.BuildLinesFromTerms(players), new PaginationTools.Settings(){ IncludeHeader = false });
    }
    private static void Kick(CommandArgs args)
    {
        if (args.Parameters.Count < 1)
        {
            args.Player.SendErrorMessage(I18n.GetString("Invalid syntax. Proper syntax: {0}kick <player> [reason].", Commands.Specifier));
            return;
        }
        if (args.Parameters[0].Length == 0)
        {
            args.Player.SendErrorMessage(I18n.GetString("A player name must be provided to kick a player. Please provide one."));
            return;
        }
        var list = Utils.FindByNameOrID(args.Parameters[0]);
        if (list.Count == 0)
        {
            args.Player.SendErrorMessage(I18n.GetString("Player not found. Unable to kick the player."));
            return;
        }
        if (list.Count > 1)
        {
            args.Player.SendMultipleMatchError(list.Select((TSPlayer p) => p.Name));
            return;
        }
        string reason = ((args.Parameters.Count > 1) ? string.Join(" ", args.Parameters.GetRange(1, args.Parameters.Count - 1)) : I18n.GetString("Misbehaviour."));
        if (list[0].Kick(reason, !args.Player.RealPlayer, silent: false, args.Player.Name))
        {
            Netplay.Clients[list[0].Index].Socket.Close();
        }
        else
        {
            args.Player.SendErrorMessage(I18n.GetString("You can't kick another admin."));
        }
    }
}

