using TShockAPI;

using VBY.Common.Config;

namespace VBY.Shop;

public class Config : ConfigBase<Root>
{
    public Dictionary<string, Show> Shops = new();
}
public class Root : RootBase
{
    public DatabaseInfo DBInfo = new() { DBPath = "Config/shop.db" };
    public CShops Shops = new();
    public Money Money = new();
}
public class Money
{
    public string Name = "余额";
}
public class CShops
{
    public BuffShop BuffShop = new();
    public ItemShop ItemShop = new();
    public LifeShop LifeShop = new();
    public NpcShop NpcShop = new();
    public TileShop TileShop = new();
}
public class Show
{
    public string SystemFormat = "";
    public string PlayerFormat = "";
    public bool Progress, Zone, Group;
    public string GetFormat(TSPlayer player) => player.RealPlayer ? PlayerFormat : SystemFormat;
}
public class ItemShop
{
    public class ChangeShop : Show
    {
    }
    public class PayShop : Show
    {
    }
    public class SystemShop : Show
    {
    }
    public ChangeShop Change = new();
    public PayShop Pay = new();
    public SystemShop System = new();
}
public class LifeShop 
{ 
    public class HealShop : Show
    {
    }
    public class MaxShop : Show
    {
    }
    public HealShop Heal = new();
    public MaxShop Max = new();
}
public class BuffShop : Show { }
public class NpcShop : Show { }
public class TileShop : Show
{
    public TileOffset SpawnOffset = new() { X = 1, Y = 2 };
}
public class TileOffset
{
    public short X;
    public short Y;
}