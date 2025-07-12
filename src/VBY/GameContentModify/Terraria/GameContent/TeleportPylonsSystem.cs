using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;

namespace VBY.GameContentModify.GameContent;

[ReplaceType(typeof(TeleportPylonsSystem))]
public static class ReplaceTeleportPylonsSystem
{
    [DetourMethod]
    public static void HandleTeleportRequest(On.Terraria.GameContent.TeleportPylonsSystem.orig_HandleTeleportRequest orig, TeleportPylonsSystem self, TeleportPylonInfo info, int playerIndex)
    {
        Player player = Main.player[playerIndex];
        string key = "";
        bool flag = true;
        if (flag)
        {
            //flag &= TeleportPylonsSystem.IsPlayerNearAPylon(player);
            flag &= IsPlayerNearAPylon(player);
            if (!flag)
            {
                key = "Net.CannotTeleportToPylonBecausePlayerIsNotNearAPylon";
            }
        }
        if (flag)
        {
            //flag &= self.DoesPylonHaveEnoughNPCsAroundIt(info, self.HowManyNPCsDoesPylonNeed(info, player));
            flag &= self.DoesPylonHaveEnoughNPCsAroundIt(info, HowManyNPCsDoesPylonNeed(info, player));
            if (!flag)
            {
                key = "Net.CannotTeleportToPylonBecauseNotEnoughNPCs";
            }
        }
        if (flag && GameContentModify.MainConfig.Instance.TeleportPylons.DangerCheck)
        {
            flag &= !NPC.AnyDanger(quickBossNPCCheck: false, ignorePillarsAndMoonlordCountdown: true);
            if (!flag)
            {
                key = "Net.CannotTeleportToPylonBecauseThereIsDanger";
            }
        }
        if (flag && GameContentModify.MainConfig.Instance.TeleportPylons.PreDownedPlantBossTempleCheck)
        {
            if (!NPC.downedPlantBoss && info.PositionInTiles.Y > Main.worldSurface && Framing.GetTileSafely(info.PositionInTiles.X, info.PositionInTiles.Y).wall == WallID.LihzahrdBrickUnsafe)
            {
                flag = false;
            }
            if (!flag)
            {
                key = "Net.CannotTeleportToPylonBecauseAccessingLihzahrdTempleEarly";
            }
        }
        if (flag && GameContentModify.MainConfig.Instance.TeleportPylons.EnvironmentalRequirementsCheck)
        {
            self._sceneMetrics.ScanAndExportToMain(new SceneMetricsScanSettings
            {
                VisualScanArea = null,
                BiomeScanCenterPositionInWorld = info.PositionInTiles.ToWorldCoordinates(),
                ScanOreFinderData = false
            });
            flag = self.DoesPylonAcceptTeleportation(info, player);
            if (!flag)
            {
                key = "Net.CannotTeleportToPylonBecauseNotMeetingBiomeRequirements";
            }
        }
        if (flag)
        {
            bool flag2 = false;
            int num = 0;
            for (int i = 0; i < self._pylons.Count; i++)
            {
                TeleportPylonInfo info2 = self._pylons[i];
                //if (!player.InInteractionRange(info2.PositionInTiles.X, info2.PositionInTiles.Y, TileReachCheckSettings.Pylons))
                if (!player.InInteractionRange(info2.PositionInTiles.X, info2.PositionInTiles.Y, SettingPylons))
                {
                    continue;
                }
                if (num < 1)
                {
                    num = 1;
                }
                //if (self.DoesPylonHaveEnoughNPCsAroundIt(info2, self.HowManyNPCsDoesPylonNeed(info2, player)))
                if (self.DoesPylonHaveEnoughNPCsAroundIt(info2, HowManyNPCsDoesPylonNeed(info2, player)))
                {
                    if (num < 2)
                    {
                        num = 2;
                    }
                    self._sceneMetrics.ScanAndExportToMain(new SceneMetricsScanSettings
                    {
                        VisualScanArea = null,
                        BiomeScanCenterPositionInWorld = info2.PositionInTiles.ToWorldCoordinates(),
                        ScanOreFinderData = false
                    });
                    //if (self.DoesPylonAcceptTeleportation(info2, player))
                    if (!GameContentModify.MainConfig.Instance.TeleportPylons.EnvironmentalRequirementsCheck || self.DoesPylonAcceptTeleportation(info2, player))
                    {
                        flag2 = true;
                        break;
                    }
                }
            }
            if (!flag2)
            {
                flag = false;
                key = num switch
                {
                    1 => "Net.CannotTeleportToPylonBecauseNotEnoughNPCsAtCurrentPylon",
                    2 => "Net.CannotTeleportToPylonBecauseNotMeetingBiomeRequirements",
                    _ => "Net.CannotTeleportToPylonBecausePlayerIsNotNearAPylon",
                };
            }
        }
        if (flag)
        {
            Vector2 newPos = info.PositionInTiles.ToWorldCoordinates() - new Vector2(0f, player.HeightOffsetBoost);
            int num2 = 9;
            int typeOfPylon = (int)info.TypeOfPylon;
            int number = 0;
            player.Teleport(newPos, num2, typeOfPylon);
            player.velocity = Vector2.Zero;
            RemoteClient.CheckSection(player.whoAmI, player.position);
            NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, newPos.X, newPos.Y, num2, number, typeOfPylon);
        }
        else
        {
            ChatHelper.SendChatMessageToClient(NetworkText.FromKey(key), new Color(255, 240, 20), playerIndex);
        }
    }
    public static int HowManyNPCsDoesPylonNeed(TeleportPylonInfo info, Player player)
    {
        TeleportPylonType typeOfPylon = info.TypeOfPylon;
        if (typeOfPylon != TeleportPylonType.Victory)
        {
            return GameContentModify.MainConfig.Instance.TeleportPylons.NeedNPCCount;
        }

        return 0;
    }
    public static bool IsPlayerNearAPylon(Player player)
    {
        return player.IsTileTypeInInteractionRange(TileID.TeleportationPylon, SettingPylons);
    }
    public static TileReachCheckSettings SettingPylons => new()
    {
        OverrideXReach = GameContentModify.MainConfig.Instance.TeleportPylons.ReachX,
        OverrideYReach = GameContentModify.MainConfig.Instance.TeleportPylons.ReachY
    };
}
