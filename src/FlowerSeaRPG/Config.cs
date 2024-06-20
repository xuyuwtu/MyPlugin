using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Xna.Framework;

using Terraria;

using TShockAPI;

using Newtonsoft.Json;

using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1;

namespace FlowerSeaRPG;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

public class Config
{
    public int MaxLevel = 100;
    public string ChatFormat = "{0} {1}";
    public float StrengthenCoefficient = 0.1f;
    public float HardModeStrengthenCoefficient = 0.5f;
    public RGBColor ItemColor = new(23, 86, 18);
    public AttributeAddInfo AttributeAddInfo = new(); 
    public DatabaseInfo DBInfo = new();
    public ConfigGradeInfo[] UpgradeInfo = Array.Empty<ConfigGradeInfo>(); 
    public NPCSpawnLineInfo[] NPCSpawnLineInfos = Array.Empty<NPCSpawnLineInfo>();
    public EventDoCommandInfo[] EventDoCommands = new EventDoCommandInfo[]
    {
        new EventDoCommandInfo(){ EventId = 13, Commands = new string[]{ "/fsc region serverload config" }, Player = 0 }
    };
    public static bool Load(TSPlayer player)
    {
        try
        {
            var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Strings.ConfigPath)) ?? throw new Exception($"配置文件 '{Strings.ConfigPath}' 转换错误");
            FlowerSeaRPG.MainConfig = config;
            return true;
        }
        catch (Exception e)
        {
            FlowerSeaRPG.MainConfig ??= new();
            player.SendErrorMessage(player.RealPlayer ? "读取配置文件错误" : e.ToString());
            TShock.Log.Error(e.ToString());
            return false;
        }
    }
}
public class ChangeConfig
{
    public List<Vector2> LockSign = new();
    public int FindOffsetX = -20;
    public int FindOffsetY = 3;
    public int PlaceTileX;
    public int PlaceTileY;
    public bool Left = true;
    public string PlacePoint;
    public string[] PlaceFile = new string[] {"ship-left", "ship-right"};
    public string RegionName = "船";
    public int SwitchOffsetX;
    public int SwitchOffsetY;
    public List<string> SwitchCommands = new();
    public static bool Load(TSPlayer player)
    {
        try
        {
            var config = JsonConvert.DeserializeObject<ChangeConfig>(File.ReadAllText(Strings.ChangePath)) ?? throw new Exception($"配置文件 '{Strings.ChangePath}' 转换错误");
            FlowerSeaRPG.ChangeConfig = config;
            return true;
        }
        catch (Exception e)
        {
            FlowerSeaRPG.ChangeConfig ??= new();
            player.SendErrorMessage(player.RealPlayer ? "读取配置文件错误" : e.ToString());
            TShock.Log.Error(e.ToString());
            return false;
        }
    }
    public void Save()
    {
        File.WriteAllText(Strings.ChangePath, JsonConvert.SerializeObject(this, Formatting.Indented));
    }
}
public class AttributeAddInfo
{
    public double AddDamage = 0.1;
    public int DamageMaxCount = 20;
    public double AddKnockBack = 0.1;
    public int KnockBackMaxCount = 20;
    public double AddSpeed = 0.1;
    public int SpeedMaxCount = 9;
    public int SpeedMinValue = 2;
    public double AddScale = 0.1;
    public int ScaleMaxCount = 10;
}
public class ConfigGradeInfo
{
    public string Level;
    public int AttributePoints;
    public string Title;
    public string Life;
    public string Magic;
    public Item[] UpgradeItems;
    public Item[] GiveItems;
    public bool GetGradeInfo(TSPlayer player, int maxLevel, out List<GradeInfo> gradeInfos)
    {
        var levels = Utils.GetNums(Level, maxLevel);
        gradeInfos = new List<GradeInfo>();
        foreach (var level in levels)
        {
            if (level < 0 || level > maxLevel)
            {
                player.SendErrorMessage($"level:{Level} > MaxLevel({maxLevel})");
                return false;
            }
            gradeInfos.Add(new GradeInfo(level, AttributePoints, Title, Life, Magic, UpgradeItems, GiveItems));
        }
        return true;
    }
}
public class GradeInfo
{
    public int Level;
    public int AttributePoints;
    public string Title;
    public AddInfo Life;
    public AddInfo Magic;
    public Item[] UpgradeItems;
    public Item[] GiveItems;
    public GradeInfo(int level, int attributePoints, string title, string life, string magic, Item[] upgradeItems, Item[] giveItems)
    {
        Level = level;
        AttributePoints = attributePoints;
        Title = title;
        Life = AddInfo.GetAddInfo(life);
        Magic = AddInfo.GetAddInfo(magic);
        UpgradeItems = upgradeItems;
        GiveItems = giveItems;
    }
    public string GetUpgradeItemsIconStringToGame()
    {
        if (UpgradeItems is null || UpgradeItems.Length == 0)
        {
            return "无";
        }
        else
        {
            var list = new List<Item>(UpgradeItems.Length);
            var insertInfos = new List<(int index, Item[] items)>();
            for (int i = 0; i < UpgradeItems.Length; i++)
            {
                var item = UpgradeItems[i];
                var stack = item.stack;
                item.SetDefaults(item.type);
                item.stack = stack;
                if (stack > item.maxStack)
                {
                    var quotient = Math.DivRem(stack, item.maxStack, out var remainder);
                    var items = new Item[quotient];
                    for (int j = 0; j < quotient; j++)
                    {
                        items[j] = new Item
                        {
                            type = item.type,
                            stack = item.maxStack
                        };
                    }
                    insertInfos.Add((i, items));
                    item.stack = remainder;
                }
                if (item.stack != 0)
                {
                    list.Add(item);
                }
            }
            var count = 0;
            for (int i = 0; i < insertInfos.Count; i++)
            {
                for (int j = 0; j < insertInfos[i].items.Length; j++)
                {
                    list.Insert(insertInfos[i].index + count, insertInfos[i].items[j]);
                    count++;
                }
            }
            return $"{string.Join("", list.Select(x => $"[i/s{x.stack}:{x.type}]"))}";
        }
    }
    public string GetUpgradeItemsNameStringToGame()
    {
        if (UpgradeItems is null || UpgradeItems.Length == 0)
        {
            return "无";
        }
        return $"{string.Join("\n", UpgradeItems.Select(x => $"{Lang.GetItemNameValue(x.type)}({x.type}): {x.stack}个"))}";
    }
    public void Update(GradeInfo gradeInfo)
    {
        if (gradeInfo.Title is not null)
        {
            Title = gradeInfo.Title;
        }
        if (gradeInfo.Life is not null)
        {
            Life = gradeInfo.Life;
        }
        if (gradeInfo.Magic is not null)
        {
            Magic = gradeInfo.Magic;
        }
        if (gradeInfo.UpgradeItems is not null)
        {
            UpgradeItems = gradeInfo.UpgradeItems;
        }
        if (gradeInfo.GiveItems is not null)
        {
            GiveItems = gradeInfo.GiveItems;
        }
    }
    public string GetGiveItemsString()
    {
        if (GiveItems is null || GiveItems.Length  == 0)
        {
            return "无";
        }
        else
        {
            return string.Join("\n", GiveItems.Select((value, index) => $"[{index}]{Utils.ItemTag(value)}"));
        }
    }
}
public class DatabaseInfo
{
    public string DBType = "sqlite";
    public string DBPath = "tshock/tshock.sqlite";
    public string MysqlHost = "localhost";
    public uint MysqlPort = 3306;
    public string MysqlDatabase = "root";
    public string MysqlUser = "root";
    public string MysqlPass = "123456";
    public IDbConnection GetDbConnection()
    {
        switch (DBType.ToLower())
        {
            case "sqlite":
                return new SqliteConnection(new SqliteConnectionStringBuilder() { DataSource = DBPath }.ConnectionString);
            case "mysql":
                return new MySqlConnection(new MySqlConnectionStringBuilder() { Server = MysqlHost, Port = MysqlPort, Database = MysqlDatabase, UserID = MysqlUser, Password = MysqlPass }.ConnectionString);
            default:
                throw new NotImplementedException();
        }
    }
}
public enum AddType
{
    None, Set, Add
}
public class AddInfo
{
    public static AddInfo Empty = new();
    public AddType Type = AddType.None;
    public int Value;
    public static AddInfo GetAddInfo(string info)
    {
        if (string.IsNullOrEmpty(info))
        {
            return new AddInfo() { Type = AddType.None, Value = 0 };
        }
        else if (info[0] == '+')
        {
            return new AddInfo() { Type = AddType.Add, Value = int.Parse(info[1..]) };
        }
        else
        {
            return new AddInfo() { Type = AddType.Set, Value = int.Parse(info) };
        }
    }
    public override string ToString()
    {
        return $"{(Type == AddType.Set ? "" : "+")}{Value}";
    }
}
public class NPCSpawnLineInfo
{
    public int Type;
    public LineInfo[] Lines = Array.Empty<LineInfo>();
}
public class LineInfo
{
    public string Text;
    public RGBColor Color = Microsoft.Xna.Framework.Color.White;
}
public class EventDoCommandInfo
{
    public int EventId;
    public int Player = 0;
    public string[] Commands = Array.Empty<string>();
}
public struct RGBColor
{
    public byte R, G, B;

    public RGBColor(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    }
    public static implicit operator Color(RGBColor color) => new(color.R, color.G, color.B);
    public static implicit operator RGBColor(Color color) => new(color.R, color.G, color.B);
}