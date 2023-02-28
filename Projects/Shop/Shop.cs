using System.Data;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Data.Sqlite;

using Terraria;
using TerrariaApi.Server;

using TShockAPI;
using TShockAPI.DB;

using MySql.Data.MySqlClient;

using VBY.Basic;
using VBY.Basic.Command;
using VBY.Basic.Extension;

namespace VBY.Shop;

[ApiVersion(2, 1)]
public class Shop : TerrariaPlugin
{
    public override string Name => GetType().Name;
    public override string Description => "一个商店插件";
    public override string Author => "yu";
    public override Version Version => GetType().Assembly.GetName().Version!;
    public readonly static ShopPlayer[] Players = new ShopPlayer[byte.MaxValue];
    public static IDbConnection DB { get; internal set; }
    public static Config ReadConfig { get; internal set; }
    public SubCmdRoot CmdCommand, CtlCommand;
    public Command[] AddCommands;
    static Shop()
    {
        var typeName = typeof(Shop).Name;
        ReadConfig = new(Path.Combine(TShock.SavePath, "Shop"), typeName + ".json");
        ReadConfig.PostRead += x =>
        {
            var shops = x.Root.Shops;
            ReadConfig.Shops.Clear();
            ReadConfig.Shops.Add(nameof(TableInfo.ItemSystemShop), shops.ItemShop.System);
            ReadConfig.Shops.Add(nameof(TableInfo.ItemChangeShop), shops.ItemShop.Change);
            ReadConfig.Shops.Add(nameof(TableInfo.ItemPayShop), shops.ItemShop.Pay);
            ReadConfig.Shops.Add(nameof(TableInfo.LifeHealShop), shops.LifeShop.Heal);
            ReadConfig.Shops.Add(nameof(TableInfo.LifeMaxShop), shops.LifeShop.Max);
            ReadConfig.Shops.Add(nameof(TableInfo.BuffShop), shops.BuffShop);
            ReadConfig.Shops.Add(nameof(TableInfo.TileShop), shops.TileShop);
            ReadConfig.Shops.Add(nameof(TableInfo.NpcShop), shops.NpcShop);
            foreach(var shop in ReadConfig.Shops)
            {
                Utils.FormatReplace(shop.Value, Shops.PrintFormats[shop.Key]);
            }
        };
        ReadConfig.Write(corver: true);
        ReadConfig.Read(true);
        var root = ReadConfig.Root;
        var dbinfo = root.DBInfo;
        DB = root.DBInfo.DBType.ToLower() == "sqlite"
            ? new SqliteConnection(new SqliteConnectionStringBuilder()
            {
                DataSource = dbinfo.DBPath
            }.ConnectionString)
            : new MySqlConnection(new MySqlConnectionStringBuilder()
            {
                Server = dbinfo.MysqlHost,
                Port = dbinfo.MysqlPort,
                Database = dbinfo.MysqlDatabase,
                UserID = dbinfo.MysqlUser,
                Password = dbinfo.MysqlPass
            }.ConnectionString);
        var creator = new SqlTableCreator(DB, DB.GetSqlType() == SqlType.Sqlite ? new SqliteQueryCreator() : new MysqlQueryCreator());
        foreach (var table in typeof(TableInfo).GetNestedTypes())
        {
            Utils.GetFuncs.Add(table, Utils.GetReaderNewFunc(table));
            var members = table.GetMembers(BindingFlags.Instance | BindingFlags.Public);
            var tableHeaders = new List<string>(members.Length);
            var columns = new List<SqlColumn>(members.Length);
            var instance = Activator.CreateInstance(table);
            foreach (var member in table.GetMembers(BindingFlags.Instance | BindingFlags.Public).OrderBy(x => x.MetadataToken))
            {
                if (member.MemberType is MemberTypes.Field or MemberTypes.Property)
                {
                    MySqlDbType dbType = MySqlDbType.Decimal;
                    string? defaultValue = string.Empty;
                    switch (member.MemberType)
                    {
                        case MemberTypes.Field:
                            {
                                var fieldInfo = (FieldInfo)member;
                                dbType = Info.TypeToDbType[fieldInfo.FieldType];
                                defaultValue = fieldInfo.GetValue(instance)!.ToString();
                                var name = $"{member.DeclaringType!.Name}.{member.Name}";
                                if (!Utils.PrintEmit.ContainsKey(name))
                                {
                                    Utils.PrintEmit.Add(name, x =>
                                    {
                                        x.Emit(OpCodes.Ldarg_0);
                                        x.Emit(OpCodes.Ldfld, fieldInfo);
                                        if (fieldInfo.FieldType.IsValueType)
                                            x.Emit(OpCodes.Box, fieldInfo.FieldType);
                                    });
                                }
                                break;
                            }
                        case MemberTypes.Property:
                            {
                                var propertyInfo = (PropertyInfo)member;
                                dbType = Info.TypeToDbType[propertyInfo.PropertyType];
                                defaultValue = propertyInfo.GetValue(instance)!.ToString();
                                var name = $"{member.DeclaringType!.Name}.{member.Name}";
                                if (!Utils.PrintEmit.ContainsKey(name))
                                {
                                    Utils.PrintEmit.Add(name, x =>
                                    {
                                        x.Emit(OpCodes.Ldarg_0);
                                        x.Emit(OpCodes.Call, propertyInfo.GetGetMethod()!);
                                        if (propertyInfo.PropertyType.IsValueType)
                                            x.Emit(OpCodes.Box, propertyInfo.PropertyType);
                                    });
                                }
                                break;
                            }
                    }
                    tableHeaders.Add(member.Name);
                    columns.Add(new SqlColumn(member.Name, dbType)
                    {
                        NotNull = true,
                        DefaultValue = defaultValue,
                        Primary = member.GetCustomAttribute<PrimaryKeyAttribute>() is not null,
                        AutoIncrement = member.GetCustomAttribute<AutoIncrementAttribute>() is not null,
                        Unique = member.GetCustomAttribute<UniqueAttribute>() is not null
                    });
                }
            }
            Utils.TableHeaders.Add(table, string.Join(',', tableHeaders));
            creator.EnsureTableStructure(new SqlTable(table.Name, columns));
            var printformat = table.GetField("PrintFormat");
            if (printformat is not null)
            {
                var format = printformat.GetValue(null)!.ToString()!;
                Shops.PrintGetArrFuncs[table.Name] = Utils.GetPrintArgsFunc(table, format);
                Shops.PrintFormats[table.Name] = format;
            }
        }
    }
    public Shop(Main game) : base(game)
    {
        CmdCommand = new("Shop");
        CtlCommand = new("Shopctl");
        CmdCommand.AddList("Item", "物品商店", 2).AddBuyAndList<TableInfo.ItemSystemShop>()
            .AddList("Change", "交易商店", 2).AddBuy1AndList<TableInfo.ItemChangeShop>()
            .AddList("Pay", "充值商店", 2).AddBuyAndList<TableInfo.ItemPayShop>()
            .AddList("Buff", "增益商店", 2).AddBuy1AndList<TableInfo.BuffShop>()
            .AddList("Npc", "Npc商店", 2).AddBuyAndList<TableInfo.NpcShop>()
            .AddList("Tile", "物块商店", 2).AddBuy1AndList<TableInfo.TileShop>()
            .AddList("Heal", "恢复商店", 2).AddBuyAndList<TableInfo.LifeHealShop>()
            .AddList("Max", "生命商店", 2).AddBuyAndList<TableInfo.LifeMaxShop>();
        CmdCommand.SetAllNode(new AllowInfo(true, null, null), x => x.NodeType == NodeType.Run && x.CmdName == "List");
        AddCommands = ReadConfig.Root.Commands.GetCommands(CmdCommand, CtlCommand);
    }
    public override void Initialize()
    {
        Commands.ChatCommands.AddRange(AddCommands);
        ServerApi.Hooks.GamePostInitialize.Register(this, x => Commands.HandleCommand(TSPlayer.Server, "/shop c l"));
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Commands.ChatCommands.RemoveRange(AddCommands);
        }
        base.Dispose(disposing);
    }
    public static void Buy<T>(SubCmdArgs args) where T : Shops
    {
        if (!int.TryParse(args.Parameters[0], out int buyId))
        {
            args.Player.SendErrorMessage("转换ID失败");
            return;
        }
        if (!short.TryParse(args.Parameters.GetIndexOrValue(1, "1"), out var count))
        {
            args.Player.SendErrorMessage("转换数量失败");
            return;
        }
        var shop = Utils.SelectShop<T>(buyId);
        if (shop is null)
        {
            args.Player.SendErrorMessage("商品ID:{0} 未找到", buyId);
            return;
        }
        if (!shop.CanBuy(args.Player, count))
        {
            args.Player.SendErrorMessage("你还没有达到购买要求");
            return;
        }
        shop.Buy(args.Player, count);
    }
    public static void Buy1<T>(SubCmdArgs args) where T : Shops
    {
        if (!int.TryParse(args.Parameters[0], out int buyId))
        {
            args.Player.SendErrorMessage("转换ID失败");
            return;
        }
        var shop = Utils.SelectShop<T>(buyId);
        if (shop is null)
        {
            args.Player.SendErrorMessage("商品ID:{0} 未找到", buyId);
            return;
        }
        if (!shop.CanBuy(args.Player, 1))
        {
            args.Player.SendErrorMessage("你还没有达到购买要求");
            return;
        }
        shop.Buy(args.Player, 1);
    }
    public static void List<T>(SubCmdArgs args) where T : Shops
    {
        var sql = $"SELECT * FROM {typeof(T).Name}";
        Console.WriteLine(sql);
        using var reader = DB.QueryReader(sql);
        var func = Utils.GetFunc<T>();
        if (!reader.Read())
        {
            args.Player.SendInfoMessage("当前没有商品");
            return;
        }
        reader.Reader.DoForEach(x => func(x).Print(args.Player));
    }
    public static string MoneyName => ReadConfig.Root.Money.Name;
}
public class ShopPlayer
{
    public TSPlayer Player;
    public int PlayerId;
    public string Name { get => Player.Name; }
    public long Money;
    public HashSet<int> HaveBuff, OpenBuff;
    public int BuffTime, HealStack;

    public ShopPlayer(TSPlayer player, int playerId, long money, string haveBuff, string openBuff, int buffTime, int healStack)
    {
        Player = player;
        PlayerId = playerId;
        Money = money;
        HaveBuff = haveBuff.Split(',', StringSplitOptions.RemoveEmptyEntries).Where(x => int.TryParse(x, out int buff) && buff > 0 && buff < Terraria.ID.BuffID.Count).Select(x => int.Parse(x)).ToHashSet();
        OpenBuff = HaveBuff.Intersect(openBuff.Split(',', StringSplitOptions.RemoveEmptyEntries).Where(x => int.TryParse(x, out int buff) && buff > 0 && buff < Terraria.ID.BuffID.Count).Select(x => int.Parse(x))).ToHashSet();
        BuffTime = buffTime;
        HealStack = healStack;
    }
    public void AddMoney(Shops shops, short count) => Money += shops.Price * count;
    public void ReduceMoney(Shops shop, short count) => Money -= shop.Price * count;
    public TableInfo.PlayerInfo ToPlayerInfo() => new(PlayerId, Name, Money, string.Join(',', HaveBuff), string.Join(',', OpenBuff), BuffTime, HealStack);
}