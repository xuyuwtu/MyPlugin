using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

using Terraria;
using TShockAPI;

namespace VBY.MoreSpawnPoint;

public static class Utils
{
    public static SpawnPointInfo AddSpawnPoint(string name, short x,short y)
    {
        var info = new SpawnPointInfo(name, x, y);
        MoreSpawnPoint.SpawnPointInfos.Add(name, info);
        return info;
    }
    public static SpawnPointInfo AddSpawnPoint(string name, TSPlayer setPlayer)
    {
        return AddSpawnPoint(name, (short)(setPlayer.TileX + MoreSpawnPoint.MainConfig.Instance.SetOffsetX), (short)(setPlayer.TileY + MoreSpawnPoint.MainConfig.Instance.SetOffsetY));
    }
    public static void BindSpawnPoint(TSPlayer execPlayer, TSPlayer setPlayer, string name)
    {
        if(MoreSpawnPoint.SpawnPointInfos.TryGetValue(name, out var info))
        {
            var distance = setPlayer.TPlayer.position.Distance(new Vector2(info.X * 16, info.Y * 16));
            if (MoreSpawnPoint.MainConfig.Instance.DistanceLimit != 0 && distance > MoreSpawnPoint.MainConfig.Instance.DistanceLimit * 16)
            {
                execPlayer.SendInfoMessage($"距离绑定出生点过远,需要在出生点{MoreSpawnPoint.MainConfig.Instance.DistanceLimit}格距离内,当前距离:{(int)distance / 16}格");
                return;
            }
            MoreSpawnPoint.PlayerSpawnPoints[setPlayer.Name] = info;
            setPlayer.sX = info.X;
            setPlayer.sY = info.Y;
            setPlayer.SendData(PacketTypes.WorldInfo);
            execPlayer.SendSuccessMessage("设置成功");
        }
        else        
        {
            execPlayer.SendWarningMessage($"出生点:{name} 未找到");
        }
    }
    public static void BindSpawnPoint(TSPlayer execPlayer, string setPlayerName, string name)
    {

        if (MoreSpawnPoint.SpawnPointInfos.TryGetValue(name, out var info))
        {
            MoreSpawnPoint.PlayerSpawnPoints[setPlayerName] = info;
        }
        else
        {
            execPlayer.SendWarningMessage($"出生点:{name} 未找到");
        }
    }
    public static void SetDefaultPoint(TSPlayer setPlayer)
    {
        if(MoreSpawnPoint.PlayerSpawnPoints.TryGetValue(setPlayer.Name, out var info) && info.X == setPlayer.sX && info.Y == setPlayer.sY)
        {
            setPlayer.sX = Terraria.Main.spawnTileX;
            setPlayer.sY = Terraria.Main.spawnTileY;
        }
        setPlayer.SendData(PacketTypes.WorldInfo);
    }
    public static bool FindPlayer(TSPlayer execPlayer, string search, [MaybeNullWhen(false)]out TSPlayer player)
    {
        var list = TSPlayer.FindByNameOrID(search);
        player = null;
        if (list.Count == 0)
        {
            execPlayer.SendInfoMessage("没有找到玩家");
            return false;
        }
        else if(list.Count > 1)
        {
            execPlayer.SendMultipleMatchError(list.Select(x => x.Name));
            return false;
        }
        player = list[0];
        return true;
    }
}
