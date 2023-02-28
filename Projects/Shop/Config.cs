using VBY.Basic.Config;

namespace VBY.Shop;

public class Config : MainConfig<Root>
{
    public Config(string configDirectory, string? fileName = null) : base(configDirectory, fileName)
    {
    }
    public Dictionary<string, Show> Shops = new();
}
public class Root : MainRoot
{
    public DBInfo DBInfo = new();
    public CShops Shops = new();
    public Money Money = new();
}
public class DBInfo
{
    public string DBType = "sqlite";
    public string DBPath = "tshock/Shop/shop.sqlite";
    public string MysqlHost = "localhost";
    public uint MysqlPort = 3306;
    public string MysqlDatabase = "root";
    public string MysqlUser = "root";
    public string MysqlPass = "123456";
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
public class Show : ICheck
{
    public string SystemFormat = "";
    public string PlayerFormat = "";
    public string Progress { get; set; } = "";
    public string Zone { get; set; } = "";
    public string FGroup { get; set; } = "";
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