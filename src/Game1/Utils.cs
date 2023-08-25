using System.Collections;
using System.Reflection;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;

using TShockAPI;

using MonoMod.RuntimeDetour.HookGen;

namespace Game1;

internal static class Utils
{
    public static string[] TeamChinese = new string[] { "无", "红", "绿", "蓝", "黄", "粉" };
    public static void ColorTile(int x, int y, int team, int owner, int size = 4)
    {
        byte color = MiniGame.TeamConvertColors[team];
        bool colorSuccess = false;
        for (int i = x - size; i <= x + size; i++)
        {
            for (int j = y - size; j <= y + size; j++)
            {
                if (!WorldGen.InWorld(i, j, 1) || !MiniGame.GameRegion.Contains(i, j) || MiniGame.TeamSpawnRegion[team].Contains(i, j))
                {
                    continue;
                }
                //if (!MiniGame.GameRegion.Contains(i, j))
                //{
                //    Console.WriteLine($"{i} {j} not in gameregion");
                //    continue;
                //}
                //if(MiniGame.TeamSpawnRegion[team].Contains(i, j))
                //{
                //    Console.WriteLine($"{i} {j} in spawn region");
                //    continue;
                //}
                ITile tile = Main.tile[i, j];
                if(tile is not null && tile.type != 0)
                {
                    if (tile.color() != color)
                    {
                        tile.color(color);
                        colorSuccess = true;
                        MiniGame.PlayersConvertCount[owner]++;
                    }
                    if(tile.wall != 0 && tile.wallColor() != color)
                    {
                        tile.wallColor(color);
                        colorSuccess = true;
                    }
                }
            }
        }
        if (colorSuccess)
        {
            NetMessage.SendTileSquare(-1, x - size, y - size, size * 2 + 1, size * 2 + 1);
        }
    }
    public static int[] TileColorStatistic()
    {
        var statistic = new int[32];
        for (int i = 0; i < Main.maxTilesX; i++)
        {
            for (int j = 0; j < Main.maxTilesY; j++)
            {
                var tile = Main.tile[i, j];
                var color = tile.color();
                if (tile is not null && tile.active() && color != PaintID.None)
                {
                    statistic[color]++;
                }
            }
        }
        return statistic;
    }
    public static void ClearOwner(Delegate hook)
    {
        var dic = (IDictionary)typeof(HookEndpointManager).GetField("OwnedHookLists", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!;
        var owner = HookEndpointManager.GetOwner(hook);
        if (owner is not null)
        {
            dic.Remove(owner);
        }
    }
    public static Rectangle GetRectangle(Point point1, Point point2)
    {
        int x = point1.X;
        int y = point1.Y;
        if(point1.X > point2.X)
        {
            x = point2.X;
        }
        if(point1.Y > point2.Y)
        {
            y = point2.Y;
        }
        int width = Math.Abs(point1.X - point2.X + 1);
        int height = Math.Abs(point1.Y - point2.Y + 1);
        return new(x, y, width, height);
    }
    public static void SendTileRect(Rectangle rectangle) => SendTileRect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
    public static void SendTileRect(int x, int y, int width, int height)
    {
        //const byte sendMax = byte.MaxValue / 3;
        const byte sendMax = 100;
        if (width <= sendMax && height <= sendMax)
        {
            NetMessage.SendTileSquare(-1, x, y, width, height);
        }
        else
        {
            var widthQuotient = Math.DivRem(width, sendMax, out var widthRemainder);
            var heightQuotient = Math.DivRem(height, sendMax, out var heightRemainder);
            var sendX = new byte[widthQuotient + 1];
            var sendY = new byte[heightQuotient + 1];
            Array.Fill(sendX, sendMax);
            sendX[^1] = (byte)widthRemainder;
            Array.Fill(sendY, sendMax);
            sendY[^1] = (byte)heightRemainder;
            var curY = y;
            for (int j = 0; j < sendY.Length; j++)
            {
                var curX = x;
                for (int i = 0; i < sendX.Length; i++)
                {
                    if (sendX[i] != 0 && sendY[j] != 0)
                    {
                        NetMessage.SendTileSquare(-1, curX, curY, sendX[i], sendY[j]);
                    }
                    curX += sendX[i];
                }
                curY += sendY[j];
            }
        }
    }
    #region Extension
    public static PlayerData Copy(this PlayerData data)
    {
        var targetData = new PlayerData(null)
        {
            health = data.health,
            maxHealth = data.maxHealth
        };
        for (int i = 0; i < NetItem.MaxInventory; i++) 
        {
            targetData.StoreSlot(i, data.inventory[i]);
        }
        return targetData;
    }
    public static void StoreSlot(this PlayerData data, ItemInfo info) => data.StoreSlot(info.slot, info.type, info.prefix, info.stack);
    public static void StoreSlot(this PlayerData data, ItemInfo[] infos)
    {
        foreach(var info in infos)
        {
            data.StoreSlot(info);
        }
    }
    public static void StoreSlot(this PlayerData data, int slot, NetItem info) => data.StoreSlot(slot, info.NetId, info.PrefixId, info.Stack);
    public static GetDataHandlerManager<T> GetHookManager<T>(this HandlerList<T> handlerList, EventHandler<T> handler, bool init = true) where T : EventArgs
    {
        return new(handlerList, handler, init);
    }
    public static ServerHookHandlerManager<T> GetHookManager<T>(this TerrariaApi.Server.HandlerCollection<T> handlerList, TerrariaApi.Server.TerrariaPlugin plugin, TerrariaApi.Server.HookHandler<T> handler, bool init = true) where T : EventArgs
    {
        return new(handlerList, plugin, handler, init);
    }
    #endregion
}
