using System.Reflection.Emit;
using Microsoft.Xna.Framework;

using Terraria;

using TShockAPI;
using TShockAPI.DB;

using VBY.Basic;

namespace VBY.Shop;

public interface IItemShop
{
    public int Type { get; set; }
    public short Stack { get; set; }
    public short Prefix { get; set; }
}
public interface ICheck
{
    public string Progress { get; set; }
    public string Zone { get; set; }
    public string FGroup { get; set; }
}
public abstract class Shops
{
    [Unique]
    public int BuyId;
    public long Price = 1;
    public Shops() { }
    public Shops(int buyId, long price)
    {
        BuyId = buyId;
        Price = price;
    }
    public abstract void Buy(TSPlayer player, short count);
    public virtual bool CanBuy(TSPlayer player, short count)
    {
        return CanPrint(player) && Shop.Players[player.Index].Money < Price * count;
    }
    public abstract bool CanPrint(TSPlayer player);
    public virtual bool Delete(TSPlayer player)
    {
        return Shop.DB.Query($"DELETE FROM {GetType().Name} WHERE BuyId = @0", BuyId) == 1;
    }
    public static Dictionary<string, Func<Shops, object[]>> PrintGetArrFuncs = new();
    public static Dictionary<string, string> PrintFormats = new();
    public void Print(TSPlayer player)
    {
        var typeName = GetType().Name;
        player.SendInfoMessage(Shop.ReadConfig.Shops[typeName].GetFormat(player), PrintGetArrFuncs[typeName](this));
    }
}
public abstract class ItemShops : Shops, IItemShop
{
    public int Type { get; set; }
    public short Stack { get; set; }
    public short Prefix { get; set; }
    static ItemShops()
    {
        Utils.PrintEmit["ItemName"] = x =>
        {
            x.Emit(OpCodes.Ldsfld, TypeOf.TShock.GetField("Utils")!);
            x.Emit(OpCodes.Ldarg_0);
            x.Emit(OpCodes.Call, typeof(ItemShops).GetProperty("Type")!.GetGetMethod()!);
            x.Emit(OpCodes.Callvirt, typeof(TShockAPI.Utils).GetMethod("GetItemById")!);
            x.Emit(OpCodes.Callvirt, typeof(Item).GetProperty("Name")!.GetGetMethod()!);
        };
        Utils.PrintEmit["PrefixName"] = x =>
        {
            x.Emit(OpCodes.Ldsfld, TypeOf.TShock.GetField("Utils")!);
            x.Emit(OpCodes.Ldarg_0);
            x.Emit(OpCodes.Call, typeof(ItemShops).GetProperty("Prefix")!.GetGetMethod()!);
            x.Emit(OpCodes.Callvirt, typeof(TShockAPI.Utils).GetMethod("GetPrefixById")!);
        };
    }
    public ItemShops() { }
    public ItemShops(int buyId, long price, int type, short stack, short prefix) : base(buyId, price)
    {
        Type = type;
        Stack = stack;
        Prefix = prefix;
    }
    public override void Buy(TSPlayer player, short count)
    {
        if (!player.FindPlayer(out var shopPlayer))
            return;
        shopPlayer.ReduceMoney(this, count);
        var item = new Item();
        item.SetDefaults(Type);
        var giveCount = Stack * count;
        int num = 0, surplus;
        if (giveCount > item.maxStack)
            num = Math.DivRem(giveCount, item.maxStack, out surplus);
        else
            surplus = giveCount;
        for (int i = 0; i < num; i++)
            player.GiveItem(Type, item.maxStack, Prefix);
        if (surplus > 0)
            player.GiveItem(Type, surplus, Prefix);
    }
    public abstract override bool CanPrint(TSPlayer player);
}
public abstract class LifeShops : Shops, ICheck
{
    public int Start, End;
    public string Progress { get; set; } = "";
    public string Zone { get; set; } = "";
    public string FGroup { get; set; } = "";
    public LifeShops() { }
    public LifeShops(int buyId, long price, short start, short end, string progress, string zone, string fGroup) : base(buyId, price)
    {
        Start = start;
        End = end;
        Progress = progress;
        Zone = zone;
        FGroup = fGroup;
    }
    public override bool CanPrint(TSPlayer player) => this.CheckAllow(player);
    public abstract override void Buy(TSPlayer player, short count);
}
public static class TableInfo
{
    public class BuffShop : Shops, ICheck
    {
        public int Type;
        public string Progress { get; set; } = "";
        public string Zone { get; set; } = "";
        public string FGroup { get; set; } = "";
        static BuffShop()
        {
            Utils.PrintEmit["BuffName"] = x =>
            {
                x.Emit(OpCodes.Ldsfld, TypeOf.TShock.GetField("Utils")!);
                x.Emit(OpCodes.Ldarg_0);
                x.Emit(OpCodes.Ldfld, typeof(BuffShop).GetField("Type")!);
                x.Emit(OpCodes.Callvirt, typeof(TShockAPI.Utils).GetMethod("GetBuffName")!);
            };
        }
        public BuffShop() { }
        public BuffShop(int buyId, int price, int type, string progress, string zone, string fGroup) : base(buyId, price)
        {
            Type = type;
            Progress = progress;
            Zone = zone;
            FGroup = fGroup;
        }
        public override void Buy(TSPlayer player, short count)
        {
            if (!player.FindPlayer(out var shopPlayer))
                return;
            shopPlayer.ReduceMoney(this, count);
            if (shopPlayer.HaveBuff.Add(Type))
                player.SendSuccessMessage("购买成功:{0} {1}", Type, TShock.Utils.GetBuffName(Type));
            else
                player.SendErrorMessage("购买失败:{0} {1}", Type, TShock.Utils.GetBuffName(Type));
        }
        public override bool CanPrint(TSPlayer player) => this.CheckAllow(player);
        public const string PrintFormat = "BuyId,Price,MoneyName,Type,BuffName";
    }
    public class ItemChangeShop : ItemShops
    {
        public string Name = "";
        public ItemChangeShop() { }
        public ItemChangeShop(int buyId, long price, int type, short stack, short prefix, string name) : base(buyId, price, type, stack, prefix)
        {
            Name = name;
        }
        public override bool CanPrint(TSPlayer player) => true;
        public const string PrintFormat = "BuyId,Price,MoneyName,Type,Stack,Prefix,Name,ItemName,PrefixName";
    }
    public class ItemSystemShop : ItemShops, ICheck
    {
        public string Progress { get; set; } = "";
        public string Zone { get; set; } = "";
        public string FGroup { get; set; } = "";
        public ItemSystemShop() { }
        public ItemSystemShop(int buyId, long price, int type, short stack, short prefix, string progress, string zone, string fGroup) : base(buyId, price, type, stack, prefix)
        {
            Progress = progress;
            Zone = zone;
            FGroup = fGroup;
        }
        public override bool CanPrint(TSPlayer player) => this.CheckAllow(player);
        public const string PrintFormat = "BuyId,Price,MoneyName,Type,Stack,Prefix,ItemName,PrefixName";
    }
    public class ItemPayShop : ItemSystemShop
    {
        public ItemPayShop() { }
        public ItemPayShop(int buyId, long price, int type, short stack, short prefix, string progress, string zone, string fGroup) : base(buyId, price, type, stack, prefix, progress, zone, fGroup) { }
        public override bool CanBuy(TSPlayer player, short count)
        {
            var inventory = player.TPlayer.inventory;
            var itemcount = inventory.Where(x => x.type == Type && (Prefix == -1 || x.prefix == Prefix)).Sum(x => x.stack);
            return itemcount >= Stack * count;
        }
        public override void Buy(TSPlayer player, short count)
        {
            if (!player.FindPlayer(out var shopPlayer))
                return;
            shopPlayer.AddMoney(this, count);
            var inventory = player.TPlayer.inventory;
            var needCount = Stack * count;
            var clearIndex = new List<int>(inventory.Length - 1);
            for (int i = 0; i < clearIndex.Count; i++)
            {
                var item = inventory[i];
                if (item.type == Type && (Prefix == -1 || item.prefix == Prefix))
                {
                    if (item.stack > needCount)
                    {
                        item.stack -= needCount;
                        break;
                    }
                    else
                    {
                        needCount -= item.stack;
                        item.SetDefaults(0);
                        item.prefix = 0;
                    }
                    clearIndex.Add(i);
                }
            }
            foreach (var index in clearIndex)
                TSPlayer.All.SendData(PacketTypes.PlayerSlot, "", player.Index, inventory[index].stack, inventory[index].prefix);
        }
        public override bool CanPrint(TSPlayer player) => this.CheckAllow(player);
        public new const string PrintFormat = "BuyId,Price,MoneyName,Type,Stack,Prefix,ItemName,PrefixName";
    }
    public class LifeHealShop : LifeShops
    {
        public LifeHealShop()
        {
            Start = 0;
            End = 50;
        }
        public LifeHealShop(int buyId, long price, short start, short end, string progress, string zone, string fGroup) : base(buyId, price, start, end, progress, zone, fGroup) { }
        public override bool CanBuy(TSPlayer player, short count)
        {
            if (!player.FindPlayer(out var shopPlayer))
                return false;
            return shopPlayer.HealStack >= Start && shopPlayer.HealStack < End && base.CanBuy(player, count);
        }
        public override void Buy(TSPlayer player, short count)
        {
            if (!player.FindPlayer(out var shopPlayer))
                return;
            short add = count;
            if (shopPlayer.HealStack + count > End)
                add = (short)(End - shopPlayer.HealStack);
            shopPlayer.ReduceMoney(this, add);
            shopPlayer.HealStack += add;
        }
        public const string PrintFormat = "BuyId,Price,MoneyName,Start,End";
    }
    public class LifeMaxShop : LifeShops
    {
        public LifeMaxShop()
        {
            Start = 100;
            End = 400;
        }
        public LifeMaxShop(int buyId, long price, short start, short end, string progress, string zone, string fGroup) : base(buyId, price, start, end, progress, zone, fGroup) { }
        public override bool CanBuy(TSPlayer player, short count)
        {
            if (!player.FindPlayer(out var _))
                return false;
            var tplayer = player.TPlayer;
            return tplayer.statLifeMax >= Start && tplayer.statLifeMax < End && base.CanBuy(player, count);
        }
        public override void Buy(TSPlayer player, short count)
        {
            if (!player.FindPlayer(out var shopPlayer))
                return;
            short add = count;
            var tplayer = player.TPlayer;
            if (tplayer.statLifeMax + count > End)
                add = (short)(End - tplayer.statLifeMax);
            shopPlayer.ReduceMoney(this, add);
            tplayer.statLifeMax += add;
            TSPlayer.All.SendData(PacketTypes.PlayerHp, "", player.Index);
        }
        public const string PrintFormat = "BuyId,Price,MoneyName,Start,End";
    }
    public class NpcShop : Shops, ICheck
    {
        public int Type;
        public short MaxStack;
        public string Progress { get; set; } = "";
        public string Zone { get; set; } = "";
        public string FGroup { get; set; } = "";
        static NpcShop()
        {
            Utils.PrintEmit["NpcName"] = x =>
            {
                x.Emit(OpCodes.Ldsfld, TypeOf.TShock.GetField("Utils")!);
                x.Emit(OpCodes.Ldarg_0);
                x.Emit(OpCodes.Ldfld, typeof(NpcShop).GetField("Type")!);
                x.Emit(OpCodes.Callvirt, typeof(TShockAPI.Utils).GetMethod("GetNPCById")!);
                x.Emit(OpCodes.Callvirt, typeof(NPC).GetProperty("FullName")!.GetGetMethod()!);
            };
        }
        public NpcShop() { }
        public NpcShop(int buyId, int price, int type, short maxStack, string progress, string zone, string fGroup) : base(buyId, price)
        {
            Type = type;
            MaxStack = maxStack;
            Progress = progress;
            Zone = zone;
            FGroup = fGroup;
        }
        public override bool CanBuy(TSPlayer player, short count)
        {
            return Main.npc.Where(x => x.type == Type && x.active).Count() < MaxStack && base.CanBuy(player, count);
        }
        public override bool CanPrint(TSPlayer player) => this.CheckAllow(player);
        public override void Buy(TSPlayer player, short count)
        {
            var havecount = Main.npc.Where(x => x.type == Type && x.active).Count();
            int buycount = count;
            if (havecount + count > MaxStack)
                buycount = MaxStack - havecount;
            TSPlayer.All.SendInfoMessage("玩家:{0} 购买了NPC:{1} {2}个", player.Name, TShock.Utils.GetNPCById(Type).FullName, buycount);
            Task.Run(() =>
            {
                for (int i = 0; i < buycount; i++)
                {
                    if (TShock.Players[player.Index] is not null)
                    {
                        NPC.NewNPC(null, (int)player.TPlayer.Center.X, (int)player.TPlayer.Center.Y, Type);
                        Task.Delay(1000);
                    }
                    else
                    {
                        TSPlayer.All.SendInfoMessage("购买NPC玩家已退出,取消剩余NPC到达");
                        break;
                    }
                }
                player.SendInfoMessage("NPC已全部到达");
            });
        }
        public const string PrintFormat = "BuyId,Price,MoneyName,Type,MaxStack,NpcName";
    }
    public class TileShop : Shops, ICheck
    {
        public int Type;
        public string Size = "";
        public short Style;
        public string Walls = "", Bottoms = "";
        public string Progress { get; set; } = "";
        public string Zone { get; set; } = "";
        public string FGroup { get; set; } = "";
        public TileShop() { }
        public TileShop(int buyId, long price, int type, string size, short style, string walls, string bottoms, string progress, string zone, string fGroup) : base(buyId, price)
        {
            Type = type;
            Size = size;
            Style = style;
            Walls = walls;
            Bottoms = bottoms;
            Progress = progress;
            Zone = zone;
            FGroup = fGroup;
        }
        public override bool CanPrint(TSPlayer player) => this.CheckAllow(player);
        public override bool CanBuy(TSPlayer player, short count)
        {
            var tplayer = player.TPlayer;
            if (tplayer.velocity != Vector2.Zero)
                return false;
            if (tplayer.height > 54)
                return false;
            if (!base.CanBuy(player, count))
                return false;
            short placeX = (short)(player.TileX + Shop.ReadConfig.Root.Shops.TileShop.SpawnOffset.X);
            short placeY = (short)(player.TileY + Shop.ReadConfig.Root.Shops.TileShop.SpawnOffset.Y);
            TileCheck(placeX, placeY, byte.Parse(Size[0].ToString()), byte.Parse(Size[1].ToString()), Walls, Bottoms, out var allow);
            return allow;
        }
        public static (bool wall, bool bottom) TileCheck(int x, int y, int width, int height, string wall, string bottom, out bool allow)
        {
            bool flag1 = true, flag2 = true;
            if (!string.IsNullOrEmpty(wall))
            {
                var walls = wall.Split(',', StringSplitOptions.RemoveEmptyEntries);
                if (walls.Length < width * height)
                {
                    allow = false;
                    return (false, false);
                }
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (Main.tile[x - i, y - j] is null)
                            Main.tile[x - i, y - j] = new Tile();
                        if (walls[i] != "-1" && Main.tile[x - i, y - j].wall != ushort.Parse(walls[i]))
                            flag1 = false;
                    }
                }
                if (flag1 && !string.IsNullOrEmpty(bottom))
                {
                    var bottoms = bottom.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (bottoms.Length < width)
                    {
                        allow = false;
                        return (false, false);
                    }
                    for (int i = 0; i < width; i++)
                    {
                        if (Main.tile[x - i, y + 1] is not null || (bottoms[i] != "-1" && Main.tile[x - i, y + 1].type != ushort.Parse(bottoms[i])))
                            flag2 = false;
                    }
                }
            }
            allow = flag1 && flag2;
            return (flag1, flag2);
        }
        public static Dictionary<string, (Action<int, int, ushort, int> action, (byte width, byte height) size, (int X, int Y) placeoffset, (short X, short Y) sendoffset)> PlaceInfo = new()
        {
            ["22"] = (WorldGen.Place2x2, (2, 2), (0, 0), (-1, -1)),
            ["32"] = (WorldGen.Place3x2, (3, 2), (-1, 0), (-1, -1)),
            ["33"] = (WorldGen.Place3x3, (3, 3), (-1, 0), (-1, -2))
        };
        public override void Buy(TSPlayer player, short count)
        {
            if (!player.FindPlayer(out var shopPlayer))
                return;
            short placeX = (short)(player.TileX + Shop.ReadConfig.Root.Shops.TileShop.SpawnOffset.X);
            short placeY = (short)(player.TileY + Shop.ReadConfig.Root.Shops.TileShop.SpawnOffset.Y);
            if (PlaceInfo.TryGetValue(Size, out var info))
            {
                shopPlayer.ReduceMoney(this, count);
                info.action(placeX + info.placeoffset.X, placeY + info.placeoffset.Y, (ushort)Type, Style);
                TSPlayer.All.SendTileRect((short)(placeX + info.sendoffset.X), (short)(placeY + info.sendoffset.Y), info.size.width, info.size.height);
            }
            else
            {
                player.SendErrorMessage("未知Size");
            }
        }
        public const string PrintFormat = "BuyId,Price,MoneyName,Style,Size";
    }
    public class PlayerInfo
    {
        [PrimaryKey]
        [AutoIncrement]
        public int PlayerId;
        public string Name;
        public long Money;
        public string HaveBuff, OpenBuff;
        public int BuffTime, HealStack;
        public PlayerInfo()
        {
            PlayerId = 0;
            Name = "";
            Money = 0;
            HaveBuff = OpenBuff = string.Empty;
            BuffTime = HealStack = 0;
        }
        public PlayerInfo(int playerId, string name, long money, string haveBuff, string openBuff, int buffTime, int healStack)
        {
            PlayerId = playerId;
            Name = name;
            Money = money;
            HaveBuff = haveBuff;
            OpenBuff = openBuff;
            BuffTime = buffTime;
            HealStack = healStack;
        }
        public bool Save()
        {
            int count;
            var type = GetType();
            if (PlayerId == -1)
            {
                count = Shop.DB.Query($"INSERT INTO {type.Name}(Money,HaveBuff,OpenBuff,BuffTime,HealStack,Name) VALUES(@0,@1,@2,@3,@4,@5)",
                    Money, HaveBuff, OpenBuff, BuffTime, HealStack, Name);
            }
            else
            {
                count = Shop.DB.Query($"UPDATE {type.Name} SET Money = @0, HaveBuff = @1, OpenBuff = @2, BuffTime = @3, HealStack = @4 WHERE PlayerName = @5",
                    Money, HaveBuff, OpenBuff, BuffTime, HealStack, Name);
            }
            return count == 1;
        }
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class UniqueAttribute : Attribute
{
}
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class PrimaryKeyAttribute : Attribute
{
}
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class AutoIncrementAttribute : Attribute
{
}