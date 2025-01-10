using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;

namespace VBY.GameContentModify;

[ReplaceType(typeof(Player))]
public static class ReplacePlayer
{
    public static void Shellphone_Spawn(Player self)
    {
        if (self.lastDeathPostion == default)
        {
            int floorX = Main.spawnTileX;
            int floorY = Main.spawnTileY;
            self.Spawn_GetPositionAtWorldSpawn(ref floorX, ref floorY);
            if (Main.netMode != NetModeID.Client && !self.Spawn_IsAreaAValidWorldSpawn(floorX, floorY))
            {
                Player.Spawn_ForceClearArea(floorX, floorY);
            }
            Vector2 newPos = new Point(floorX, floorY).ToWorldCoordinates(8f, 0f) - new Vector2(self.width / 2, self.height);
            self.Teleport(newPos, TeleportationStyleID.ShellphoneSpawn);
            self.velocity = Vector2.Zero;
            if (Main.netMode == NetModeID.Server)
            {
                RemoteClient.CheckSection(self.whoAmI, self.position);
                NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, self.whoAmI, newPos.X, newPos.Y, TeleportationStyleID.ShellphoneSpawn);
            }
        }
        else
        {
            Vector2 newPos = self.lastDeathPostion.ToTileCoordinates().ToWorldCoordinates(8f, 0f) - new Vector2(self.width / 2, self.height);
            self.Teleport(newPos, TeleportationStyleID.ShellphoneSpawn);
            self.velocity = Vector2.Zero;
            if (Main.netMode == NetModeID.Server)
            {
                RemoteClient.CheckSection(self.whoAmI, self.position);
                NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, self.whoAmI, newPos.X, newPos.Y, TeleportationStyleID.ShellphoneSpawn);
            }
        }
    }

}
