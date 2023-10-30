using Microsoft.Xna.Framework;
using System.Data;
using Terraria;

using TShockAPI;
using TShockAPI.DB;
using VBY.Common.Command;
using TUtils = TShockAPI.Utils;

namespace VBY.Shop;

public interface ICheck
{
    public string Progress { get; set; }
    public string Zone { get; set; }
    public string FGroup { get; set; }
}
public abstract class Shops
{
    public static Dictionary<string, Func<Shops, object[]>> PrintGetArrFuncs = new();
    public static Dictionary<string, string> PrintFormats = new();
    [Unique]
    public int BuyId;
    public long Price = 1;
    public Shops() { }
    public Shops(int buyId, long price)
    {
        BuyId = buyId;
        Price = price;
    }
    public abstract void Add(SubCmdArgs args);
    public abstract void Buy(ShopPlayer shopPlayer, short count);
    public virtual bool CanBuy(ShopPlayer shopPlayer, short count, out string message)
    {
        message = "";
        if(!CanPrint(shopPlayer.TSPlayer))
        {
            message = "你还没达到购买要求";
            return false;
        }
        if(!(shopPlayer.Money < Price * count))
        {
            message = "你钱不够";
            return false;
        }
        return true;
    }
    public abstract bool CanPrint(TSPlayer player);
    public virtual bool Delete(TSPlayer player) => ShopPlugin.DB.Query($"DELETE FROM {GetType().Name} WHERE BuyId = @0", BuyId) == 1;
    public void Print(TSPlayer player)
    {
        var typeName = GetType().Name;
        player.SendInfoMessage(ShopPlugin.ReadConfig.Shops[typeName].GetFormat(player), PrintGetArrFuncs[typeName](this));
    }
}
public abstract class CheckShops : Shops, ICheck
{
    public string Progress { get; set; } = "";
    public string Zone { get; set; } = "";
    public string FGroup { get; set; } = "";
    public override bool CanPrint(TSPlayer player) => this.CheckAllow(player);
    public CheckShops() { }
    public CheckShops(int buyId, long price, string progress = "", string zone = "", string fGroup = "") : base(buyId, price)
    {
        Progress = progress;
        Zone = zone;
        FGroup = fGroup;
    }
}
public abstract class ItemShops : Shops
{
    public int Type;
    public short Stack, Prefix;
    static ItemShops()
    {
        Utils.PrintEmit[PrintName.ItemName] = x =>
        {
            x.EmitTShockMethod<ItemShops>(nameof(Type), nameof(TUtils.GetItemById));
            x.EmitGetProperty<Item>(nameof(Item.Name));
        };
        Utils.PrintEmit[PrintName.PrefixName] = x =>
        {
            x.EmitTShockMethod<ItemShops>(nameof(Prefix), nameof(TUtils.GetPrefixById));
        };
    }
    public ItemShops() { }
    public ItemShops(int buyId, long price, int type, short stack, short prefix) : base(buyId, price)
    {
        Type = type;
        Stack = stack;
        Prefix = prefix;
    }
    public override void Buy(ShopPlayer shopPlayer, short count)
    {
        shopPlayer.ReduceMoney(this, count);
        var item = new Item();
        item.SetDefaults(Type);
        var giveCount = Stack * count;
        int num = 0, surplus;
        if (giveCount > item.maxStack)
        {
            num = Math.DivRem(giveCount, item.maxStack, out surplus);
        }
        else
        {
            surplus = giveCount;
        }

        for (int i = 0; i < num; i++)
        {
            shopPlayer.TSPlayer.GiveItem(Type, item.maxStack, Prefix);
        }

        if (surplus > 0)
        {
            shopPlayer.TSPlayer.GiveItem(Type, surplus, Prefix);
        }
    }
    public abstract override bool CanPrint(TSPlayer player);
}
public abstract class LifeShops : CheckShops
{
    public int Start, End;
    public LifeShops() { }
    public LifeShops(int buyId, long price, string progress, string zone, string fGroup, int start, int end) : base(buyId, price, progress, zone, fGroup)
    {
        Start = start;
        End = end;
    }
    public abstract override void Buy(ShopPlayer shopPlayer, short count);
}
public static partial class TableInfo
{
    public partial class BuffShop : CheckShops
    {
        public int Type;
        static BuffShop()
        {
            Utils.PrintEmit[PrintName.BuffName] = x =>
            {
                x.EmitTShockMethod<BuffShop>(nameof(Type), nameof(TUtils.GetBuffName));
            };
        }
        public BuffShop(int buyId, long price, string progress, string zone, string fGroup, int type) : base(buyId, price, progress, zone, fGroup)
        {
            Type = type;
        }
        public override void Buy(ShopPlayer shopPlayer, short count)
        {
            shopPlayer.ReduceMoney(this, count);
            if (shopPlayer.HaveBuff.Add(Type))
            {
                shopPlayer.TSPlayer.SendSuccessMessage("购买成功:{0} {1}", Type, TShock.Utils.GetBuffName(Type));
            }
            else
            {
                shopPlayer.TSPlayer.SendErrorMessage("购买失败:{0} {1}", Type, TShock.Utils.GetBuffName(Type));
            }
        }
    }
    public partial class ItemChangeShop : ItemShops
    {
        public string Name = "";
        public ItemChangeShop(int buyId, long price, int type, short stack, short prefix, string name) : base(buyId, price, type, stack, prefix)
        {
            Name = name;
        }
        public override void Buy(ShopPlayer shopPlayer, short count)
        {
            base.Buy(shopPlayer, count);
            if (Utils.TryGetOnlineShopPlayer(Name, out var shopOwnerPlayer))
            {
                shopOwnerPlayer.AddMoney(this, count);
            }
            else
            {
                using var reader = ShopPlugin.DB.QueryReader($"SELECT {Utils.TableHeaders[typeof(PlayerInfo)]} FROM {nameof(PlayerInfo)} WHERE Name = @0", Name);
                if (reader.Read())
                {
                    var info = Utils.GetFunc<PlayerInfo>()(reader.Reader);
                    info.Money += Price * count;
                    info.Save();
                }
            }
        }
        public override bool CanPrint(TSPlayer player) => true;
    }
    public partial class ItemSystemShop : ItemShops, ICheck
    {
        public string Progress { get; set; } = "";
        public string Zone { get; set; } = "";
        public string FGroup { get; set; } = "";
        public ItemSystemShop(int buyId, long price, int type, short stack, short prefix, string progress, string zone, string fGroup) : base(buyId, price, type, stack, prefix)
        {
            Progress = progress;
            Zone = zone;
            FGroup = fGroup;
        }
        public override bool CanPrint(TSPlayer player) => this.CheckAllow(player);
    }
    public partial class ItemPayShop : ItemSystemShop
    {
        public ItemPayShop(int buyId, long price, int type, short stack, short prefix, string progress, string zone, string fGroup) : base(buyId, price, type, stack, prefix, progress, zone, fGroup) { }
        public override bool CanBuy(ShopPlayer shopPlayer, short count, out string message)
        {
            message = "";
            var inventory = shopPlayer.TSPlayer.TPlayer.inventory;
            var itemcount = inventory.Where(x => x.type == Type && (Prefix == -1 || x.prefix == Prefix)).Sum(x => x.stack);
            if(itemcount < Stack * count)
            {
                message = "你要的物品不够哦";
                return false;
            }
            return true;
        }
        public override void Buy(ShopPlayer shopPlayer, short count)
        {
            shopPlayer.AddMoney(this, count);
            var inventory = shopPlayer.TSPlayer.TPlayer.inventory;
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
            {
                TSPlayer.All.SendData(PacketTypes.PlayerSlot, "", shopPlayer.TSPlayer.Index, inventory[index].stack, inventory[index].prefix);
            }
        }
    }
    public partial class LifeHealShop : LifeShops
    {
        public LifeHealShop()
        {
            Start = 0;
            End = 50;
        }
        public LifeHealShop(int buyId, long price, string progress, string zone, string fGroup, short start, short end) : base(buyId, price, progress, zone, fGroup, start, end) { }
        public override bool CanBuy(ShopPlayer shopPlayer, short count, out string message)
        {
            if(shopPlayer.HealStack < Start || shopPlayer.HealStack >= End)
            {
                message = "你的恢复值不在范围内";
                return false;
            }
            return base.CanBuy(shopPlayer, count, out message);
        }
        public override void Buy(ShopPlayer shopPlayer, short count)
        {
            short add = count;
            if (shopPlayer.HealStack + count > End)
            {
                add = (short)(End - shopPlayer.HealStack);
            }
            shopPlayer.ReduceMoney(this, add);
            shopPlayer.HealStack += add;
        }
    }
    public partial class LifeMaxShop : LifeShops
    {
        public LifeMaxShop()
        {
            Start = 100;
            End = 400;
        }
        public LifeMaxShop(int buyId, long price, string progress, string zone, string fGroup, short start, short end) : base(buyId, price, progress, zone, fGroup, start, end) { }
        public override bool CanBuy(ShopPlayer shopPlayer, short count, out string message)
        {
            var tplayer = shopPlayer.TSPlayer.TPlayer;
            if (tplayer.statLifeMax < Start && tplayer.statLifeMax >= End)
            {
                message = "你的血量不在范围内";
                return false;
            }
            return base.CanBuy(shopPlayer, count, out message);
        }
        public override void Buy(ShopPlayer shopPlayer, short count)
        {
            short add = count;
            var tplayer = shopPlayer.TSPlayer.TPlayer;
            if (tplayer.statLifeMax + count > End)
            {
                add = (short)(End - tplayer.statLifeMax);
            }

            shopPlayer.ReduceMoney(this, add);
            tplayer.statLifeMax += add;
            TSPlayer.All.SendData(PacketTypes.PlayerHp, "", shopPlayer.TSPlayer.Index);
        }
    }
    public partial class NpcShop : CheckShops
    {
        public int Type;
        public short MaxStack;
        static NpcShop()
        {
            Utils.PrintEmit[PrintName.NpcName] = x =>
            {
                x.EmitTShockMethod<NpcShop>(nameof(Type), nameof(TUtils.GetNPCById));
                x.EmitGetProperty<NPC>(nameof(NPC.FullName));
            };
        }
        public NpcShop(int buyId, int price, string progress, string zone, string fGroup, int type, short maxStack) : base(buyId, price, progress, zone, fGroup)
        {
            Type = type;
            MaxStack = maxStack;
        }
        public override bool CanBuy(ShopPlayer shopPlayer, short count, out string message)
        {
            if(Main.npc.Where(x => x.type == Type && x.active).Count() >= MaxStack)
            {
                message = "npc已存在数量超过指定数量";
                return false;
            }
            return base.CanBuy(shopPlayer, count, out message);
        }
        public override void Buy(ShopPlayer shopPlayer, short count)
        {
            var havecount = Main.npc.Where(x => x.type == Type && x.active).Count();
            int buycount = count;
            if (havecount + count > MaxStack)
            {
                buycount = MaxStack - havecount;
            }

            TSPlayer.All.SendInfoMessage("玩家:{0} 购买了NPC:{1} {2}个", shopPlayer.TSPlayer.Name, TShock.Utils.GetNPCById(Type).FullName, buycount);
            Task.Run(() =>
            {
                for (int i = 0; i < buycount; i++)
                {
                    if (TShock.Players[shopPlayer.TSPlayer.Index] is not null)
                    {
                        NPC.NewNPC(null, (int)shopPlayer.TSPlayer.TPlayer.Center.X, (int)shopPlayer.TSPlayer.TPlayer.Center.Y, Type);
                        Task.Delay(1000).Wait();
                    }
                    else
                    {
                        TSPlayer.All.SendInfoMessage("购买NPC玩家已退出,取消剩余NPC到达");
                        break;
                    }
                }
                shopPlayer.TSPlayer.SendInfoMessage("NPC已全部到达");
            });
        }
    }
    public partial class TileShop : CheckShops
    {
        public int Type;
        public string Size = "";
        public short Style;
        public string Walls = "", Bottoms = "";
        public TileShop(int buyId, long price, string progress, string zone, string fGroup, int type, string size, short style, string walls, string bottoms) : base(buyId, price, progress, zone, fGroup)
        {
            Type = type;
            Size = size;
            Style = style;
            Walls = walls;
            Bottoms = bottoms;
        }
        public override bool CanBuy(ShopPlayer shopPlayer, short count, out string message)
        {
            var tsplayer = shopPlayer.TSPlayer;
            var tplayer = tsplayer.TPlayer;
            if (tplayer.velocity != Vector2.Zero)
            {
                message = "移动中不可购买";
                return false;
            }
            if (tplayer.height > 54)
            {
                message = "坐骑中不可购买";
                return false;
            }

            if (!base.CanBuy(shopPlayer, count, out message))
            {
                return false;
            }

            short placeX = (short)(tsplayer.TileX + ShopPlugin.ReadConfig.Root.Shops.TileShop.SpawnOffset.X);
            short placeY = (short)(tsplayer.TileY + ShopPlugin.ReadConfig.Root.Shops.TileShop.SpawnOffset.Y);
            var (wall, bottom) = TileCheck(placeX, placeY, byte.Parse(Size[0].ToString()), byte.Parse(Size[1].ToString()), Walls, Bottoms, out var allow);
            var list = new List<string>();
            if (!wall)
            {
                list.Add("墙");
            }
            if (!bottom)
            {
                list.Add("底部");
            }
            if (!allow)
            {
                message = $"当前站立位置{string.Join(',', list)}不符合要求";
            }
            return allow;
        }
        public static (bool wall, bool bottom) TileCheck(int x, int y, int width, int height, string wall, string bottom, out bool allow)
        {
            bool wallAllow = true, bottomAlloq = true;
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
                        {
                            Main.tile[x - i, y - j] = new Tile();
                        }

                        if (walls[i] != "-1" && Main.tile[x - i, y - j].wall != ushort.Parse(walls[i]))
                        {
                            wallAllow = false;
                        }
                    }
                }
                if (wallAllow && !string.IsNullOrEmpty(bottom))
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
                        {
                            bottomAlloq = false;
                        }
                    }
                }
            }
            allow = wallAllow && bottomAlloq;
            return (wallAllow, bottomAlloq);
        }
        public static Dictionary<string, (
            Action<int, int, ushort, int> placeAction, 
            (byte width, byte height) size, 
            (int X, int Y) placeoffset, 
            (short X, short Y) sendoffset
            )> PlaceInfo = new()
        {
            ["22"] = (WorldGen.Place2x2, (2, 2), (0, 0), (-1, -1)),
            ["32"] = (WorldGen.Place3x2, (3, 2), (-1, 0), (-1, -1)),
            ["33"] = (WorldGen.Place3x3, (3, 3), (-1, 0), (-1, -2))
        };
        public override void Buy(ShopPlayer shopPlayer, short count)
        {
            short placeX = (short)(shopPlayer.TSPlayer.TileX + ShopPlugin.ReadConfig.Root.Shops.TileShop.SpawnOffset.X);
            short placeY = (short)(shopPlayer.TSPlayer.TileY + ShopPlugin.ReadConfig.Root.Shops.TileShop.SpawnOffset.Y);
            if (PlaceInfo.TryGetValue(Size, out var info))
            {
                shopPlayer.ReduceMoney(this, count);
                info.placeAction(placeX + info.placeoffset.X, placeY + info.placeoffset.Y, (ushort)Type, Style);
                TSPlayer.All.SendTileRect((short)(placeX + info.sendoffset.X), (short)(placeY + info.sendoffset.Y), info.size.width, info.size.height);
            }
            else
            {
                shopPlayer.TSPlayer.SendErrorMessage("未知Size");
            }
        }
    }
    public partial class PlayerInfo
    {
        private const string TableName = nameof(PlayerInfo);
        [PrimaryKey]
        [AutoIncrement]
        public int PlayerId;
        public string Name;
        public long Money;
        public string HaveBuff, OpenBuff;
        public int BuffTime;
        public double HealStack;
        public PlayerInfo()
        {
            PlayerId = 0;
            Name = "";
            Money = 0;
            HaveBuff = "";
            OpenBuff = "";
            BuffTime = 0;
            HealStack = 0;
        }
        public PlayerInfo(int playerId, string name, long money, string haveBuff, string openBuff, int buffTime, double healStack)
        {
            PlayerId = playerId;
            Name = name;
            Money = money;
            HaveBuff = haveBuff;
            OpenBuff = openBuff;
            BuffTime = buffTime;
            HealStack = healStack;
        }
        public ShopPlayer GetShopPlayer(TSPlayer player) => new(player, PlayerId, Money, HaveBuff, OpenBuff, BuffTime, HealStack);
        public bool Save()
        {
            int count;
            if (PlayerId == -1)
            {
                count = ShopPlugin.DB.Query($"INSERT INTO {TableName}(Money,HaveBuff,OpenBuff,BuffTime,HealStack,Name) VALUES(@0,@1,@2,@3,@4,@5)",
                    Money, HaveBuff, OpenBuff, BuffTime, HealStack, Name);
            }
            else
            {
                count = ShopPlugin.DB.Query($"UPDATE {TableName} SET Money = @0, HaveBuff = @1, OpenBuff = @2, BuffTime = @3, HealStack = @4 WHERE PlayerName = @5",
                    Money, HaveBuff, OpenBuff, BuffTime, HealStack, Name);
            }
            return count == 1;
        }
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class UniqueAttribute : Attribute { }
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class PrimaryKeyAttribute : Attribute { }
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class AutoIncrementAttribute : Attribute { }