using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;

using Terraria;

using TShockAPI;
using TShockAPI.DB;

using Newtonsoft.Json;

using MySql.Data.MySqlClient;

using MonoMod.RuntimeDetour.HookGen;

namespace FlowerSeaRPG;

internal static class Utils
{
    internal static Dictionary<string, string> TableHeaders = new();
    internal static Dictionary<string, string> SelectStr = new();
    internal static Dictionary<string, string> InsertStr = new();
    internal static Dictionary<string, string> UpdateStr = new();
    private static Item TempItem = new();
    public static string GetMultiString(string baseStr, string[] values, string endStr)
    {
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = $"{baseStr} {values[i]} {endStr}";
        }
        return string.Join('\n', values);
    }
    public static List<int> GetNums(string numStr, int maxLevel)
    {
        List<int> result = new();
        if (numStr.Contains(','))
        {
            var splits = numStr.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in splits)
            {
                result.AddRange(GetNums(s, maxLevel));
            }

        }
        else if (Regex.IsMatch(numStr, "([0-9]*)-([0-9]*)"))
        {
            var match = Regex.Match(numStr, "([0-9]*)-([0-9]*)");
            var start = string.IsNullOrEmpty(match.Groups[1].Value) ? 0 : int.Parse(match.Groups[1].Value);
            var end = string.IsNullOrEmpty(match.Groups[2].Value) ? maxLevel : int.Parse(match.Groups[2].Value);
            result.AddRange(Enumerable.Range(start, end - start + 1));
        }
        else if (Regex.IsMatch(numStr, "([0-9]*)x"))
        {
            var match = Regex.Match(numStr, "([0-9]+)x");
            var num = int.Parse(match.Groups[1].Value);
            result.AddRange(Enumerable.Range(1, maxLevel).Where(x => x % num == 0));
        }
        else
        {
            result.Add(int.Parse(numStr));
        }
        return result;
    }
    public static Dictionary<Type, MySqlDbType> TypeToDbType = new()
    {
        [typeof(int)] = MySqlDbType.Int32,
        [typeof(string)] = MySqlDbType.String,
    };
    public static string ItemTag(int type, int stack, int prefix)
    {
        string arg = (stack > 1) ? ("/s" + stack) : ((prefix != 0) ? ("/p" + prefix) : "");
        return $"[i{arg}:{type}]";
    }
    public static string ItemTag(Item item) => ItemTag(item.type, item.stack, item.prefix);
    public static string PointStrToChinese(string point)
    {
        return (point[0] == 'l' ? "左" : "右") + (point[1] == 'u' ? "上" : "下");
    }
    public static void GetRealPoint(ref int x, ref int y, int width, int height, string point, RegionInfo region)
    {
        switch (point)
        {
            case "lu":
                break;
            case "ld":
                y -= height - 1;
                break;
            case "ru":
                x -= width - 1;
                break;
            case "rd":
                x -= width - 1;
                y -= height - 1;
                break;
            default:
                var names = region.SpecialPoints.ToDictionary(x => x.Name);
                if (names.TryGetValue(point, out var info)) 
                {
                    x -= info.Point.X;
                    y -= info.Point.Y;
                }
                break;
        }
    }
    public static bool GetRegionInfo(string fileName, [MaybeNullWhen(false)] out RegionInfo regionInfo)
    {
        regionInfo = null;
        var path = Path.Combine(TShock.SavePath, nameof(FlowerSeaRPG), fileName + ".json");
        if (!File.Exists(path))
        {
            return false;
        }
        regionInfo = JsonConvert.DeserializeObject<RegionInfo>(File.ReadAllText(path));
        if (regionInfo is null)
        {
            return false;
        }
        return true;
    }
    public static RegionInfo SaveRegionData(Rectangle rectangle, string fileName, TileRegionRecord record) => SaveRegionData(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, fileName, record);
    public static RegionInfo SaveRegionData(int x, int y, int width, int height, string fileName,TileRegionRecord record)
    {
        var regionInfo = new RegionInfo(width, height);
        for (int yOffset = 0; yOffset < height; yOffset++)
        {
            for (int xOffset = 0; xOffset < width; xOffset++)
            {
                regionInfo.TileInfos[yOffset * width + xOffset] = new TileInfo(Main.tile[x + xOffset, y + yOffset]);
            }
        }
        if(record.Point3 is not null && !string.IsNullOrEmpty(record.PointName))
        {
            regionInfo.SpecialPoints.Add(new SpecialPoint(record.PointName, new Point(record.Point3.X - x, record.Point3.Y - y)));
        }
        int x2 = x + width - 1;
        int y2 = y + height - 1;
        for(int i = 0;i< 1000;i++)
        {
            var sign = Main.sign[i];
            if (sign is not null && sign.x >= x && sign.x <= x2 && sign.y >= y && sign.y <= y2)
            {
                regionInfo.SignInfos.Add(new SignInfo(sign.x - x, sign.y - y, sign.text));
            }
        }
        for (int i = 0; i < 8000; i++)
        {
            var chest = Main.chest[i];
            if (chest is not null && chest.x >= x && chest.x <= x2 && chest.y >= y && chest.y <= y2)
            {
                regionInfo.ChestInfos.Add(new ChestInfo(chest.x - x, chest.y - y, chest.name, chest.item.Select(x => new ItemInfo(x.type, x.stack, x.prefix)).ToArray()));
            }
        }
        Directory.CreateDirectory(Strings.ConfigDirectory);
        File.WriteAllText(Path.Combine(Strings.ConfigDirectory, fileName + ".json"), JsonConvert.SerializeObject(regionInfo));
        return regionInfo;
    }
    public static (bool success,string errorMessgae) LoadRegionData(int x, int y, string fileName, string point, bool skipEmpty)
    {
        var path = Path.Combine(TShock.SavePath, nameof(FlowerSeaRPG), fileName + ".json");
        if (!File.Exists(path))
        {
            return (false, $"文件 '{fileName}.json' 未找到");
        }
        var regionInfo = JsonConvert.DeserializeObject<RegionInfo>(File.ReadAllText(path));
        if (regionInfo is null)
        {
            return (false, $"反序列化失败");
        }
        GetRealPoint(ref x, ref y, regionInfo.Width, regionInfo.Height, point, regionInfo);
        for (int yOffset = 0; yOffset < regionInfo.Height; yOffset++)
        {
            for (int xOffset = 0; xOffset < regionInfo.Width; xOffset++)
            {
                if (Main.tile[x + xOffset, y + yOffset] is null)
                {
                    Main.tile[x + xOffset, y + yOffset] = new Tile();
                }
                var from = regionInfo.TileInfos[yOffset * regionInfo.Width + xOffset];
                if (!(skipEmpty && from is { type: 0, wall: 0, liquid: 0, bTileHeader: 0, bTileHeader2: 0, bTileHeader3: 0, sTileHeader: 0 })) 
                {
                    Main.tile[x + xOffset, y + yOffset].CopyFrom(from);
                }
            }
        }
        for (int i = 0; i < regionInfo.SignInfos.Count; i++)
        {
            Sign.ReadSign(regionInfo.SignInfos[i].X, regionInfo.SignInfos[i].Y);
        }
        for (int i = 0; i < regionInfo.ChestInfos.Count; i++)
        {
            var info = regionInfo.ChestInfos[i];
            var num = Chest.CreateChest(info.X, info.Y);
            if (num != -1)
            {
                var chest = Main.chest[num];
                for (int j = 0; j < 40; j++)
                {
                    chest.item[i].SetDefaults(info.Items[i].Type);
                    chest.item[i].stack = info.Items[i].Stack;
                    chest.item[i].Prefix(info.Items[i].Prefix);
                }
            }
        }
        if (regionInfo.Width <= byte.MaxValue && regionInfo.Height <= byte.MaxValue)
        {
            TSPlayer.All.SendTileRect((short)x, (short)y, (byte)regionInfo.Width, (byte)regionInfo.Height);
        }
        else
        {
            var widthQuotient = Math.DivRem(regionInfo.Width, byte.MaxValue, out var widthRemainder);
            var heightQuotient = Math.DivRem(regionInfo.Height, byte.MaxValue, out var heightRemainder);
            var sendX = new byte[widthQuotient + 1];
            var sendY = new byte[heightQuotient + 1];
            Array.Fill(sendX, byte.MaxValue);
            sendX[^1] = (byte)widthRemainder;
            Array.Fill(sendY, byte.MaxValue);
            sendY[^1] = (byte)heightRemainder;
            short curY = (short)y;
            for (int j = 0; j < sendY.Length; j++)
            {
                short curX = (short)x;
                for (int i = 0; i < sendX.Length; i++)
                {
                    if (sendX[i] != 0 && sendY[j] != 0)
                    {
                        TSPlayer.All.SendTileRect(curX, curY, sendX[i], sendY[j]);
                    }
                    curX += sendX[i];
                }
                curY += sendY[j];
            }
        }
        return (true, "");
    }
    public static (int x,int y) FindBeachPoint(bool left,int xOffset,int yOffset)
    {
        int startX;
        int findXOffset;
        int findX = 0,findY = 0;
        if (left)
        {
            startX = 50;
            findXOffset = 1;
        }
        else
        {
            startX = Main.maxTilesX - 50;
            findXOffset = -1;
            xOffset = -xOffset;
        }
        for (int i = 0; i < Main.maxTilesY; i++) 
        {
            if (Main.tile[startX, i] is null)
            {
                Main.tile[startX, i] = new Tile();
            }
            if (Main.tile[startX,i].type == 0 && Main.tile[startX,i].liquid > 0)
            {
                findY = i;
                break;
            }
        }
        for (int i = startX; i >= 0; i += findXOffset)
        {
            if (Main.tile[i, findY] is null)
            {
                Main.tile[i, findY] = new Tile();
            }
            if (Main.tile[i, findY].type != 0)
            {
                findX = i;
                break;
            }
        }
        return (findX + xOffset, findY + yOffset);
    }
    public static Rectangle GetRectangle(Point point1, Point point2)
    {
        int x = point1.X;
        int y = point1.Y;
        if (point1.X > point2.X)
        {
            x = point2.X;
        }
        if (point1.Y > point2.Y)
        {
            y = point2.Y;
        }
        int width = Math.Abs(point1.X - point2.X + 1);
        int height = Math.Abs(point1.Y - point2.Y + 1);
        return new(x, y, width, height);
    }
    public static void ClearOwner(Delegate hook)
    {
        var dic = (System.Collections.IDictionary)typeof(HookEndpointManager).GetField("OwnedHookLists", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!;
        var owner = HookEndpointManager.GetOwner(hook);
        if (owner is not null)
        {
            dic.Remove(owner);
        }
    }
    #region Extension
    public static SqlTableCreator GetTableCreator(this IDbConnection db) => new(db, db.GetSqlType() == SqlType.Sqlite ? new SqliteQueryCreator() : new MysqlQueryCreator());
    public static void GiveItemStackSplite(this TSPlayer tSPlayer, int type, int stack, int prefix = 0)
    {
        var item = new Item();
        item.SetDefaults(type);
        if (stack > item.maxStack)
        {
            var quotient = Math.DivRem(stack, item.maxStack, out var remainder);
            for (int i = 0; i < quotient; i++)
            {
                tSPlayer.GiveItem(type, item.maxStack, prefix);
            }
            if (remainder != 0)
            {
                tSPlayer.GiveItem(type, remainder, prefix);
            }
        }
        else
        {
            tSPlayer.GiveItem(type, stack, prefix);
        }
    }
    public static bool GetFSPlayer(this TSPlayer tSPlayer, [MaybeNullWhen(false)] out FSPlayer player)
    {
        player = FlowerSeaRPG.Players[tSPlayer.Index];
        if (player is null)
        {
            tSPlayer.SendErrorMessage("获取玩家对象失败");
            return false;
        }
        return true;
    }
    public static void UpdateItem(FSPlayer fsplayer, Item item)
    {
        if (fsplayer?.ShimmerAddDamage ?? false)
        {
            var flag = TweakItemFlags.Color;
            var flag2 = TweakItemFlags2.None;
            TempItem.SetDefaults(item.type);
            if (fsplayer.Damage != 0)
            {
                flag |= TweakItemFlags.Damage;
                TempItem.damage = (int)(TempItem.damage * (1 + (fsplayer.Damage * FlowerSeaRPG.MainConfig.AttributeAddInfo.AddDamage)));
            }
            if (fsplayer.KnockBack != 0)
            {
                flag |= TweakItemFlags.KnockBack;
                TempItem.knockBack = (int)(TempItem.knockBack * (1 + (fsplayer.KnockBack * FlowerSeaRPG.MainConfig.AttributeAddInfo.AddKnockBack)));
            }
            if (fsplayer.Scale != 0)
            {
                flag |= TweakItemFlags.NextFlags;
                flag2 |= TweakItemFlags2.Scale;
                TempItem.scale = (int)(TempItem.scale * (1 + (fsplayer.Scale * FlowerSeaRPG.MainConfig.AttributeAddInfo.AddScale)));
            }
            if (fsplayer.Speed != 0)
            {
                flag |= TweakItemFlags.UseAnimation;
                flag |= TweakItemFlags.UseTime;
                TempItem.useTime = (int)(TempItem.useTime * (1 - (fsplayer.Speed * FlowerSeaRPG.MainConfig.AttributeAddInfo.AddSpeed)));
                TempItem.useAnimation = (int)(TempItem.useAnimation * (1 - (fsplayer.Speed * FlowerSeaRPG.MainConfig.AttributeAddInfo.AddSpeed)));
            }
            TempItem.Prefix(item.prefix);
            item.color = FlowerSeaRPG.MainConfig.ItemColor;
            item.damage = TempItem.damage;
            item.knockBack = TempItem.knockBack;
            item.scale = TempItem.scale;
            item.useAnimation = TempItem.useAnimation;
            item.useTime = TempItem.useTime;
            if (item.useAnimation < FlowerSeaRPG.MainConfig.AttributeAddInfo.SpeedMinValue)
            {
                item.useAnimation = FlowerSeaRPG.MainConfig.AttributeAddInfo.SpeedMinValue;
            }
            if (item.useTime < FlowerSeaRPG.MainConfig.AttributeAddInfo.SpeedMinValue)
            {
                item.useTime = FlowerSeaRPG.MainConfig.AttributeAddInfo.SpeedMinValue;
            }
            fsplayer.Player.SendData(PacketTypes.TweakItem, "", item.whoAmI, (int)flag, (int)flag2);
        }
    }
    public static void RemoveRange<T>(this List<T> list, IEnumerable<T> collection)
    {
        foreach (var item in collection)
        {
            list.Remove(item);
        }
    }
    public static T GetValueOrInput<T>(this IList<T> values, int index, T input)
    {
        if (values.Count > index)
        {
            return values[index];
        }
        else
        {
            return input;
        }
    }
    public static GetDataHandlerManager<T> GetHookManager<T>(this HandlerList<T> handlerList, EventHandler<T> handler,bool init = true) where T : EventArgs
    {
        return new(handlerList, handler, init);
    }
    public static ServerHookHandlerManager<T> GetHookManager<T>(this TerrariaApi.Server.HandlerCollection<T> handlerList, TerrariaApi.Server.TerrariaPlugin plugin,TerrariaApi.Server.HookHandler<T> handler,bool init = true) where T : EventArgs
    {
        return new(handlerList, plugin, handler, init);
    }
    public static void ForEachAdd<TKey, TValue>(this Dictionary<TKey, TValue> keyValuePairs, TValue[] values, Func<TValue, TKey> func) where TKey : notnull
    {
        foreach(var  value in values)
        {
            keyValuePairs.Add(func(value), value);
        }
    }
    #endregion
}
