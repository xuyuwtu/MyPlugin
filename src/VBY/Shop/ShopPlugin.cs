using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

using Terraria;
using TerrariaApi.Server;

using TShockAPI;
using TShockAPI.DB;

using MySql.Data.MySqlClient;

using VBY.Common;
using VBY.Common.Command;
using VBY.Common.Extension;
using VBY.Common.Hook;

namespace VBY.Shop;

[ApiVersion(2, 1)]
[Description("一个商店插件")]
public partial class ShopPlugin : CommonPlugin
{
    public override string Author => "yu";
    public readonly static ShopPlayer[] Players = new ShopPlayer[byte.MaxValue];
    public static IDbConnection DB { get; internal set; }
    public static Config.MainConfig ReadConfig { get; internal set; }
    public SubCmdRoot CmdCommand, CtlCommand;
    static ShopPlugin()
    {
        var typeName = typeof(ShopPlugin).Name;
        ReadConfig = new();
        ReadConfig.Write(corver: true);
        ReadConfig.Read(true);
        var root = ReadConfig.Root;
        var dbinfo = root.DBInfo;
        DB = root.DBInfo.GetDbConnection();
        var creator = DB.GetTableCreator();
        foreach (var table in typeof(TableInfo).GetNestedTypes())
        {
            //Utils.GetFuncs.Add(table, Utils.GetReaderNewFunc(table));
            var members = table.GetMembers(BindingFlags.Instance | BindingFlags.Public);
            var tableHeaders = new List<string>(members.Length);
            var columns = new List<SqlColumn>(members.Length);
            var instance = Activator.CreateInstance(table)!;
            foreach (var member in table.GetMembers(BindingFlags.Instance | BindingFlags.Public).OrderBy(x => x.MetadataToken))
            {
                if (member.MemberType is MemberTypes.Field or MemberTypes.Property)
                {
                    MySqlDbType dbType = MySqlDbType.Decimal;
                    string? defaultValue = string.Empty;
                    switch (member.MemberType)
                    {
                        case MemberTypes.Field:
                        case MemberTypes.Property:
                            {
                                Type getType;
                                if (member.MemberType == MemberTypes.Field)
                                {
                                    getType = ((FieldInfo)member).FieldType;
                                    dbType = Info.TypeToDbType[getType];
                                    defaultValue = ((FieldInfo)member).GetValue(instance)!.ToString();
                                }
                                else
                                {
                                    getType = ((PropertyInfo)member).PropertyType; ;
                                    dbType = Info.TypeToDbType[getType];
                                    defaultValue = ((PropertyInfo)member).GetValue(instance)!.ToString();
                                }
                                var name = $"{member.DeclaringType!.Name}.{member.Name}";
                                if (!Utils.PrintEmit.ContainsKey(name))
                                {
                                    Utils.PrintEmit.Add(name, x =>
                                    {
                                        x.Emit(OpCodes.Ldarg_0);
                                        if (member.MemberType == MemberTypes.Field)
                                        {
                                            x.Emit(OpCodes.Ldfld, (FieldInfo)member);
                                        }
                                        else
                                        {
                                            x.Emit(OpCodes.Callvirt, ((PropertyInfo)member).GetGetMethod()!);
                                        }
                                        if (getType.IsValueType)
                                        {
                                            x.Emit(OpCodes.Box, getType);
                                        }
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
            Utils.ShopInstances.Add(table, (Shops)instance);
            Utils.TableHeaders.Add(table, string.Join(',', tableHeaders));
            creator.EnsureTableStructure(new SqlTable(table.Name, columns));

            if(PrintName.Formats.TryGetValue(table.Name, out var format))
            {
                Shops.PrintGetArrFuncs[table.Name] = Utils.GetPrintArgsFunc(table, format);
            }
        }
        Utils.GetFuncsInitialize();
        ReadConfig.PostRead += OnPostRead;
        OnPostRead(ReadConfig);
    }
    public ShopPlugin(Main game) : base(game)
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
        CmdCommand.SetAllNode(new SetAllowInfo(true, null, null), x => x is SubCmdList { CmdName: "List" });
        AddCommands.AddRange(ReadConfig.Root.Commands.GetCommands(CmdCommand, CtlCommand));
    }
    protected override void PreInitialize()
    {
        AttachHooks.Add(ServerApi.Hooks.ServerJoin.GetHook(this, OnServerJoin));
        AttachHooks.Add(ServerApi.Hooks.ServerLeave.GetHook(this, OnServerLeave));
        AttachHooks.Add(ServerApi.Hooks.NetGetData.GetHook(this, OnNetGetData));
    }

    public static void OnNetGetData(GetDataEventArgs args)
    {
        if(args.MsgID == PacketTypes.EffectHeal)
        {
            var shopPlayer = Players[args.Msg.whoAmI];
            if(shopPlayer is not null)
            {
                if(ReadConfig.Root.Shops.LifeShop.Heal.HealType == Config.HealType.FixedValue)
                {
                    shopPlayer.TSPlayer.Heal((int)shopPlayer.HealStack);
                }
                else
                {
                    shopPlayer.TSPlayer.Heal((int)(shopPlayer.HealStack * BitConverter.ToInt16(args.Msg.readBuffer, args.Index + 1)));
                }
            }
        }
    }
    #region Hooks
    public static void OnServerJoin(JoinEventArgs args)
    {
        if(args.Who == 255 || TShock.Players[args.Who] is null)
        {
            return;
        }
        var shopPlayer = Utils.SelectShopPlayer(TShock.Players[args.Who]);
        Players[args.Who] = shopPlayer;
        foreach(var buff in shopPlayer.OpenBuff)
        {
            shopPlayer.TSPlayer.SetBuff(buff, shopPlayer.BuffTime);
        }
    }
    public static void OnServerLeave(LeaveEventArgs args)
    {
        if (args.Who == 255 || Players[args.Who] is null)
        {
            return;
        }
        Players[args.Who].ToPlayerInfo().Save();
    }
    #endregion
    #region Cmd
    public static void Buy<T>(SubCmdArgs args) where T : Shops
    {
        if (!int.TryParse(args.Parameters[0], out int buyId))
        {
            args.Player.SendErrorMessage("转换ID失败");
            return;
        }
        if (!short.TryParse(args.Parameters.ElementAtOrDefault(1, "1"), out var count))
        {
            args.Player.SendErrorMessage("转换数量失败");
            return;
        }
        Buy<T>(args.Player, buyId, count);
    }
    public static void Buy1<T>(SubCmdArgs args) where T : Shops
    {
        if (!int.TryParse(args.Parameters[0], out int buyId))
        {
            args.Player.SendErrorMessage("转换ID失败");
            return;
        }
        Buy<T>(args.Player, buyId, 1);
    }
    public static void Buy<T>(TSPlayer player, int buyId, short count) where T : Shops
    {
        var shop = Utils.SelectShop<T>(buyId);
        if (shop is null)
        {
            player.SendErrorMessage("商品ID:{0} 未找到", buyId);
            return;
        }
        if (!player.GetShopPlayer(out var shopPlayer))
        {
            return;
        }
        if (!shop.CanBuy(shopPlayer, count, out var message))
        {
            player.SendErrorMessage(message);
            return;
        }
        shop.Buy(shopPlayer, count);
    }
    public static void Del<T>(SubCmdArgs args) where T : Shops
    {
        if (args.Parameters.Count == 0)
        {
            args.Player.SendInfoMessage("请输入要删除的商品ID");
            return;
        }
        var successIDs = new List<int>();
        var errorIDs = new List<int>();
        foreach (var item in args.Parameters)
        {
            if (int.TryParse(item, out var buyId))
            {
                Shops? shop = Utils.SelectShop<T>(buyId);
                if (shop is null)
                {
                    args.Player.SendErrorMessage("商品ID:{0} 未找到", buyId);
                    continue;
                }
                if (shop.Delete(args.Player))
                {
                    successIDs.Add(buyId);
                }
                else
                {
                    errorIDs.Add(buyId);
                }
            }
        }
        if (successIDs.Count > 0)
        {
            args.Player.SendSuccessMessage("[{0}]删除商品成功: {1} ", typeof(T).Name, string.Join(',', successIDs));
        }
        if (errorIDs.Count > 0)
        {
            args.Player.SendErrorMessage("[{0}]删除商品失败: {1} ", typeof(T).Name, string.Join(',', errorIDs));
        }
    }
    public static void List<T>(SubCmdArgs args) where T : Shops
    {
        var sql = $"SELECT * FROM {typeof(T).Name}";
        using var reader = DB.QueryReader(sql);
        var func = Utils.GetFunc<T>();
        if (!reader.Read())
        {
            args.Player.SendInfoMessage("当前没有商品");
            return;
        }
        reader.Reader.DoForEach(x => func(x).Print(args.Player));
    }
    #endregion
    public static string MoneyName => ReadConfig.Root.Money.Name;
}
public class ShopPlayer
{
    public TSPlayer TSPlayer;
    public int PlayerId;
    public string Name { get => TSPlayer.Name; }
    public long Money;
    public HashSet<int> HaveBuff, OpenBuff;
    public int BuffTime;
    public double HealStack;
    public ShopPlayer(TSPlayer player)
    {
        TSPlayer = player;
        PlayerId = -1;
        HaveBuff = new();
        OpenBuff = new();
    }
    public ShopPlayer(TSPlayer player, int playerId, long money, string haveBuff, string openBuff, int buffTime, double healStack)
    {
        TSPlayer = player;
        PlayerId = playerId;
        Money = money;
        HaveBuff = haveBuff.Split(',', StringSplitOptions.RemoveEmptyEntries).Where(x => int.TryParse(x, out int buff) && buff > 0 && buff < Terraria.ID.BuffID.Count).Select(x => int.Parse(x)).ToHashSet();
        OpenBuff = HaveBuff.Intersect(openBuff.Split(',', StringSplitOptions.RemoveEmptyEntries).Where(x => int.TryParse(x, out int buff) && buff > 0 && buff < Terraria.ID.BuffID.Count).Select(x => int.Parse(x))).ToHashSet();
        BuffTime = buffTime;
        HealStack = healStack;
    }
    public void AddMoney(Shops shop, short count) => Money += shop.Price * count;
    public void ReduceMoney(Shops shop, short count) => Money -= shop.Price * count;
    public TableInfo.PlayerInfo ToPlayerInfo() => new(PlayerId, Name, Money, string.Join(',', HaveBuff), string.Join(',', OpenBuff), BuffTime, HealStack);
}