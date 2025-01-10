using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;

using OTAPI;

using VBY.GameContentModify.Config;

namespace VBY.GameContentModify;

[ReplaceType(typeof(Wiring))]
public static class ReplaceWiring
{
    [DetourMethod]
    public static void HitWireSingle(int i, int j)
    {
        ITile tile = Main.tile[i, j];
        bool? forcedStateWhereTrueIsOn = null;
        bool doSkipWires = true;
        int type = tile.type;
        if (tile.actuator())
        {
            Wiring.ActuateForced(i, j);
        }
        if (!tile.active())
        {
            return;
        }
        switch (type)
        {
            case TileID.Timers:
                Wiring.HitSwitch(i, j);
                WorldGen.SquareTileFrame(i, j);
                NetMessage.SendTileSquare(-1, i, j);
                break;
            case TileID.ConveyorBeltLeft:
                if (!tile.actuator())
                {
                    tile.type = TileID.ConveyorBeltRight;
                    WorldGen.SquareTileFrame(i, j);
                    NetMessage.SendTileSquare(-1, i, j);
                }
                break;
            case TileID.ConveyorBeltRight:
                if (!tile.actuator())
                {
                    tile.type = TileID.ConveyorBeltLeft;
                    WorldGen.SquareTileFrame(i, j);
                    NetMessage.SendTileSquare(-1, i, j);
                }
                break;
        }
        if (type >= 255 && type <= 268)
        {
            if (!tile.actuator())
            {
                if (type >= 262)
                {
                    tile.type -= 7;
                }
                else
                {
                    tile.type += 7;
                }
                WorldGen.SquareTileFrame(i, j);
                NetMessage.SendTileSquare(-1, i, j);
            }
            return;
        }
        if (type == 419)
        {
            int num = 18;
            if (tile.frameX >= num)
            {
                num = -num;
            }
            if (tile.frameX == 36)
            {
                num = 0;
            }
            Wiring.SkipWire(i, j);
            tile.frameX = (short)(tile.frameX + num);
            WorldGen.SquareTileFrame(i, j);
            NetMessage.SendTileSquare(-1, i, j);
            Wiring._LampsToCheck.Enqueue(new Point16(i, j));
            return;
        }
        if (type == 406)
        {
            int num70 = tile.frameX % 54 / 18;
            int num81 = tile.frameY % 54 / 18;
            int num92 = i - num70;
            int num103 = j - num81;
            int num113 = 54;
            if (Main.tile[num92, num103].frameY >= 108)
            {
                num113 = -108;
            }
            for (int k = num92; k < num92 + 3; k++)
            {
                for (int l = num103; l < num103 + 3; l++)
                {
                    Wiring.SkipWire(k, l);
                    Main.tile[k, l].frameY = (short)(Main.tile[k, l].frameY + num113);
                }
            }
            NetMessage.SendTileSquare(-1, num92 + 1, num103 + 1, 3);
            return;
        }
        if (type == 452)
        {
            int num122 = tile.frameX % 54 / 18;
            int num132 = tile.frameY % 54 / 18;
            int num143 = i - num122;
            int num2 = j - num132;
            int num13 = 54;
            if (Main.tile[num143, num2].frameX >= 54)
            {
                num13 = -54;
            }
            for (int m = num143; m < num143 + 3; m++)
            {
                for (int n = num2; n < num2 + 3; n++)
                {
                    Wiring.SkipWire(m, n);
                    Main.tile[m, n].frameX = (short)(Main.tile[m, n].frameX + num13);
                }
            }
            NetMessage.SendTileSquare(-1, num143 + 1, num2 + 1, 3);
            return;
        }
        if (type == 411)
        {
            int num24 = tile.frameX % 36 / 18;
            int num35 = tile.frameY % 36 / 18;
            int num46 = i - num24;
            int num57 = j - num35;
            int num66 = 36;
            if (Main.tile[num46, num57].frameX >= 36)
            {
                num66 = -36;
            }
            for (int num67 = num46; num67 < num46 + 2; num67++)
            {
                for (int num68 = num57; num68 < num57 + 2; num68++)
                {
                    Wiring.SkipWire(num67, num68);
                    Main.tile[num67, num68].frameX = (short)(Main.tile[num67, num68].frameX + num66);
                }
            }
            NetMessage.SendTileSquare(-1, num46, num57, 2, 2);
            return;
        }
        if (type == 356)
        {
            int num69 = tile.frameX % 36 / 18;
            int num71 = tile.frameY % 54 / 18;
            int num72 = i - num69;
            int num73 = j - num71;
            for (int num74 = num72; num74 < num72 + 2; num74++)
            {
                for (int num75 = num73; num75 < num73 + 3; num75++)
                {
                    Wiring.SkipWire(num74, num75);
                }
            }
            if (!Main.fastForwardTimeToDawn && Main.sundialCooldown == 0)
            {
                Main.Sundialing();
            }
            NetMessage.SendTileSquare(-1, num72, num73, 2, 2);
            return;
        }
        if (type == 663)
        {
            int num76 = tile.frameX % 36 / 18;
            int num77 = tile.frameY % 54 / 18;
            int num78 = i - num76;
            int num79 = j - num77;
            for (int num80 = num78; num80 < num78 + 2; num80++)
            {
                for (int num82 = num79; num82 < num79 + 3; num82++)
                {
                    Wiring.SkipWire(num80, num82);
                }
            }
            if (!Main.fastForwardTimeToDusk && Main.moondialCooldown == 0)
            {
                Main.Moondialing();
            }
            NetMessage.SendTileSquare(-1, num78, num79, 2, 2);
            return;
        }
        if (type == 425)
        {
            int num83 = tile.frameX % 36 / 18;
            int num84 = tile.frameY % 36 / 18;
            int num85 = i - num83;
            int num86 = j - num84;
            for (int num87 = num85; num87 < num85 + 2; num87++)
            {
                for (int num88 = num86; num88 < num86 + 2; num88++)
                {
                    Wiring.SkipWire(num87, num88);
                }
            }
            if (Main.AnnouncementBoxDisabled)
            {
                return;
            }
            Color pink = Color.Pink;
            int num89 = Sign.ReadSign(num85, num86, CreateIfMissing: false);
            if (num89 == -1 || Main.sign[num89] == null || string.IsNullOrWhiteSpace(Main.sign[num89].text) || !Hooks.Wiring.InvokeAnnouncementBox(i, j, num89))
            {
                return;
            }
            if (Main.AnnouncementBoxRange == -1)
            {
                NetMessage.SendData(MessageID.SmartTextMessage, -1, -1, NetworkText.FromLiteral(Main.sign[num89].text), 255, pink.R, pink.G, pink.B, 460);
                return;
            }
            for (int num90 = 0; num90 < 255; num90++)
            {
                if (Main.player[num90].active && Main.player[num90].Distance(new Vector2(num85 * 16 + 16, num86 * 16 + 16)) <= Main.AnnouncementBoxRange)
                {
                    NetMessage.SendData(MessageID.SmartTextMessage, num90, -1, NetworkText.FromLiteral(Main.sign[num89].text), 255, pink.R, pink.G, pink.B, 460);
                }
            }
            return;
        }
        if (type == 405)
        {
            Wiring.ToggleFirePlace(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
            return;
        }
        if (type == 209)
        {
            int num91 = tile.frameX % 72 / 18;
            int num93 = tile.frameY % 54 / 18;
            int num94 = i - num91;
            int num95 = j - num93;
            int num96 = tile.frameY / 54;
            int num97 = tile.frameX / 72;
            int num98 = -1;
            if (num91 == 1 || num91 == 2)
            {
                num98 = num93;
            }
            int num99 = 0;
            if (num91 == 3)
            {
                num99 = -54;
            }
            if (num91 == 0)
            {
                num99 = 54;
            }
            if (num96 >= 8 && num99 > 0)
            {
                num99 = 0;
            }
            if (num96 == 0 && num99 < 0)
            {
                num99 = 0;
            }
            bool flag = false;
            if (num99 != 0)
            {
                for (int num100 = num94; num100 < num94 + 4; num100++)
                {
                    for (int num101 = num95; num101 < num95 + 3; num101++)
                    {
                        Wiring.SkipWire(num100, num101);
                        Main.tile[num100, num101].frameY = (short)(Main.tile[num100, num101].frameY + num99);
                    }
                }
                flag = true;
            }
            if ((num97 == 3 || num97 == 4) && (num98 == 0 || num98 == 1))
            {
                num99 = ((num97 == 3) ? 72 : (-72));
                for (int num102 = num94; num102 < num94 + 4; num102++)
                {
                    for (int num104 = num95; num104 < num95 + 3; num104++)
                    {
                        Wiring.SkipWire(num102, num104);
                        Main.tile[num102, num104].frameX = (short)(Main.tile[num102, num104].frameX + num99);
                    }
                }
                flag = true;
            }
            if (flag)
            {
                NetMessage.SendTileSquare(-1, num94, num95, 4, 3);
            }
            if (num98 != -1)
            {
                bool flag5 = true;
                if ((num97 == 3 || num97 == 4) && num98 < 2)
                {
                    flag5 = false;
                }
                if (Wiring.CheckMech(num94, num95, 30) && flag5)
                {
                    WorldGen.ShootFromCannon(num94, num95, num96, num97 + 1, 0, 0f, Wiring.CurrentUser, fromWire: true);
                }
            }
            return;
        }
        if (type == 212)
        {
            int num105 = tile.frameX % 54 / 18;
            int num106 = tile.frameY % 54 / 18;
            int num107 = i - num105;
            int num108 = j - num106;
            int num156 = tile.frameX / 54;
            int num109 = -1;
            if (num105 == 1)
            {
                num109 = num106;
            }
            int num110 = 0;
            if (num105 == 0)
            {
                num110 = -54;
            }
            if (num105 == 2)
            {
                num110 = 54;
            }
            if (num156 >= 1 && num110 > 0)
            {
                num110 = 0;
            }
            if (num156 == 0 && num110 < 0)
            {
                num110 = 0;
            }
            bool flag6 = false;
            if (num110 != 0)
            {
                for (int num111 = num107; num111 < num107 + 3; num111++)
                {
                    for (int num112 = num108; num112 < num108 + 3; num112++)
                    {
                        Wiring.SkipWire(num111, num112);
                        Main.tile[num111, num112].frameX = (short)(Main.tile[num111, num112].frameX + num110);
                    }
                }
                flag6 = true;
            }
            if (flag6)
            {
                NetMessage.SendTileSquare(-1, num107, num108, 3, 3);
            }
            if (num109 != -1 && Wiring.CheckMech(num107, num108, 10))
            {
                float num157 = 12f + Main.rand.Next(450) * 0.01f;
                float num114 = Main.rand.Next(85, 105);
                float num158 = Main.rand.Next(-35, 11);
                int type2 = 166;
                int damage = 0;
                float knockBack = 0f;
                Vector2 vector = new((num107 + 2) * 16 - 8, (num108 + 2) * 16 - 8);
                if (tile.frameX / 54 == 0)
                {
                    num114 *= -1f;
                    vector.X -= 12f;
                }
                else
                {
                    vector.X += 12f;
                }
                float num115 = num114;
                float num116 = num158;
                float num117 = (float)Math.Sqrt(num115 * num115 + num116 * num116);
                num117 = num157 / num117;
                num115 *= num117;
                num116 *= num117;
                Projectile.NewProjectile(Wiring.GetProjectileSource(num107, num108), vector.X, vector.Y, num115, num116, type2, damage, knockBack, Wiring.CurrentUser);
            }
            return;
        }
        if (type == 215)
        {
            Wiring.ToggleCampFire(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
            return;
        }
        if (type == 130)
        {
            if (Main.tile[i, j - 1] == null || !Main.tile[i, j - 1].active() || (!TileID.Sets.BasicChest[Main.tile[i, j - 1].type] && !TileID.Sets.BasicChestFake[Main.tile[i, j - 1].type] && Main.tile[i, j - 1].type != TileID.Dressers))
            {
                tile.type = TileID.InactiveStoneBlock;
                WorldGen.SquareTileFrame(i, j);
                NetMessage.SendTileSquare(-1, i, j);
            }
            return;
        }
        if (type == 131)
        {
            tile.type = TileID.ActiveStoneBlock;
            WorldGen.SquareTileFrame(i, j);
            NetMessage.SendTileSquare(-1, i, j);
            return;
        }
        if (type == 387 || type == 386)
        {
            bool value = type == 387;
            int num118 = WorldGen.ShiftTrapdoor(i, j, playerAbove: true).ToInt();
            if (num118 == 0)
            {
                num118 = -WorldGen.ShiftTrapdoor(i, j, playerAbove: false).ToInt();
            }
            if (num118 != 0)
            {
                NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 3 - value.ToInt(), i, j, num118);
            }
            return;
        }
        if (type == 389 || type == 388)
        {
            bool flag7 = type == 389;
            WorldGen.ShiftTallGate(i, j, flag7);
            NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 4 + flag7.ToInt(), i, j);
            return;
        }
        if (type == 11)
        {
            if (WorldGen.CloseDoor(i, j, forced: true))
            {
                NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 1, i, j);
            }
            return;
        }
        if (type == 10)
        {
            int num119 = 1;
            if (Main.rand.Next(2) == 0)
            {
                num119 = -1;
            }
            if (!WorldGen.OpenDoor(i, j, num119))
            {
                if (WorldGen.OpenDoor(i, j, -num119))
                {
                    NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, i, j, -num119);
                }
            }
            else
            {
                NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, i, j, num119);
            }
            return;
        }
        if (type == 216)
        {
            WorldGen.LaunchRocket(i, j, fromWiring: true);
            Wiring.SkipWire(i, j);
            return;
        }
        if (type == TileID.Toilets || (type == TileID.Chairs && tile.frameY / 40 is TileStyleID.Chairs.Toilet or TileStyleID.Chairs.GoldenToilet))
        {
            int num120 = j - tile.frameY % 40 / 18;
            Wiring.SkipWire(i, num120);
            Wiring.SkipWire(i, num120 + 1);
            Wiring.CheckMech(i, num120, 60);
            if (Wiring.CheckMech(i, num120, 60) && !MainConfigInfo.StaticDisableToiletSpawnProj)
            {
                Projectile.NewProjectile(Wiring.GetProjectileSource(i, num120), i * 16 + 8, num120 * 16 + 12, 0f, 0f, ProjectileID.ToiletEffect, 0, 0f, Main.myPlayer);
            }
            return;
        }
        switch (type)
        {
            case TileID.FireworksBox:
                {
                    int num64 = j - tile.frameY / 18;
                    int num65 = i - tile.frameX / 18;
                    Wiring.SkipWire(num65, num64);
                    Wiring.SkipWire(num65, num64 + 1);
                    Wiring.SkipWire(num65 + 1, num64);
                    Wiring.SkipWire(num65 + 1, num64 + 1);
                    if (Wiring.CheckMech(num65, num64, 30))
                    {
                        WorldGen.LaunchRocketSmall(num65, num64, fromWiring: true);
                    }
                    break;
                }
            case TileID.FireworkFountain:
                {
                    int num128 = j - tile.frameY / 18;
                    int num129 = i - tile.frameX / 18;
                    Wiring.SkipWire(num129, num128);
                    Wiring.SkipWire(num129, num128 + 1);
                    if (!Wiring.CheckMech(num129, num128, 30))
                    {
                        break;
                    }
                    bool flag8 = false;
                    for (int num130 = 0; num130 < 1000; num130++)
                    {
                        if (Main.projectile[num130].active && Main.projectile[num130].aiStyle == 73 && Main.projectile[num130].ai[0] == num129 && Main.projectile[num130].ai[1] == num128)
                        {
                            flag8 = true;
                            break;
                        }
                    }
                    if (!flag8)
                    {
                        int type3 = 419 + Main.rand.Next(4);
                        Projectile.NewProjectile(Wiring.GetProjectileSource(num129, num128), num129 * 16 + 8, num128 * 16 + 2, 0f, 0f, type3, 0, 0f, Main.myPlayer, num129, num128);
                    }
                    break;
                }
            case TileID.Teleporter:
                {
                    int num10 = i - tile.frameX / 18;
                    if (tile.wall == WallID.LihzahrdBrickUnsafe && j > Main.worldSurface && !NPC.downedPlantBoss)
                    {
                        break;
                    }
                    if (Wiring._teleport[0].X == -1f)
                    {
                        Wiring._teleport[0].X = num10;
                        Wiring._teleport[0].Y = j;
                        if (tile.halfBrick())
                        {
                            Wiring._teleport[0].Y += 0.5f;
                        }
                    }
                    else if (Wiring._teleport[0].X != num10 || Wiring._teleport[0].Y != j)
                    {
                        Wiring._teleport[1].X = num10;
                        Wiring._teleport[1].Y = j;
                        if (tile.halfBrick())
                        {
                            Wiring._teleport[1].Y += 0.5f;
                        }
                    }
                    break;
                }
            case TileID.Torches:
                Wiring.ToggleTorch(i, j, tile, forcedStateWhereTrueIsOn);
                break;
            case TileID.WireBulb:
                {
                    int num159 = Main.tile[i, j].frameX / 18;
                    bool flag9 = num159 % 2 >= 1;
                    bool flag10 = num159 % 4 >= 2;
                    bool flag11 = num159 % 8 >= 4;
                    bool flag12 = num159 % 16 >= 8;
                    bool flag2 = false;
                    short num131 = 0;
                    switch (Wiring._currentWireColor)
                    {
                        case 1:
                            num131 = 18;
                            flag2 = !flag9;
                            break;
                        case 2:
                            num131 = 72;
                            flag2 = !flag11;
                            break;
                        case 3:
                            num131 = 36;
                            flag2 = !flag10;
                            break;
                        case 4:
                            num131 = 144;
                            flag2 = !flag12;
                            break;
                    }
                    if (flag2)
                    {
                        tile.frameX += num131;
                    }
                    else
                    {
                        tile.frameX -= num131;
                    }
                    NetMessage.SendTileSquare(-1, i, j);
                    break;
                }
            case TileID.HolidayLights:
                Wiring.ToggleHolidayLight(i, j, tile, forcedStateWhereTrueIsOn);
                break;
            case TileID.BubbleMachine:
                {
                    int num37;
                    for (num37 = tile.frameX / 18; num37 >= 3; num37 -= 3)
                    {
                    }
                    int num38;
                    for (num38 = tile.frameY / 18; num38 >= 3; num38 -= 3)
                    {
                    }
                    int num39 = i - num37;
                    int num40 = j - num38;
                    int num41 = 54;
                    if (Main.tile[num39, num40].frameX >= 54)
                    {
                        num41 = -54;
                    }
                    for (int num42 = num39; num42 < num39 + 3; num42++)
                    {
                        for (int num43 = num40; num43 < num40 + 2; num43++)
                        {
                            Wiring.SkipWire(num42, num43);
                            Main.tile[num42, num43].frameX = (short)(Main.tile[num42, num43].frameX + num41);
                        }
                    }
                    NetMessage.SendTileSquare(-1, num39, num40, 3, 2);
                    break;
                }
            case TileID.FogMachine:
                {
                    int num154;
                    for (num154 = tile.frameX / 18; num154 >= 2; num154 -= 2)
                    {
                    }
                    int num155;
                    for (num155 = tile.frameY / 18; num155 >= 2; num155 -= 2)
                    {
                    }
                    int num3 = i - num154;
                    int num4 = j - num155;
                    int num5 = 36;
                    if (Main.tile[num3, num4].frameX >= 36)
                    {
                        num5 = -36;
                    }
                    for (int num6 = num3; num6 < num3 + 2; num6++)
                    {
                        for (int num7 = num4; num7 < num4 + 2; num7++)
                        {
                            Wiring.SkipWire(num6, num7);
                            Main.tile[num6, num7].frameX = (short)(Main.tile[num6, num7].frameX + num5);
                        }
                    }
                    NetMessage.SendTileSquare(-1, num3, num4, 2, 2);
                    break;
                }
            case TileID.HangingLanterns:
                Wiring.ToggleHangingLantern(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
                break;
            case TileID.Lamps:
                Wiring.ToggleLamp(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
                break;
            case TileID.ChineseLanterns:
            case TileID.Candelabras:
            case TileID.DiscoBall:
            case TileID.PlatinumCandelabra:
            case TileID.PlasmaLamp:
                Wiring.Toggle2x2Light(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
                break;
            case TileID.VolcanoSmall:
                {
                    Wiring.SkipWire(i, j);
                    short num8 = (short)((Main.tile[i, j].frameX != 0) ? (-18) : 18);
                    Main.tile[i, j].frameX += num8;
                    NetMessage.SendTileSquare(-1, i, j, 1, 1);
                    int num9 = ((num8 > 0) ? 4 : 3);
                    Animation.NewTemporaryAnimation(num9, 593, i, j);
                    NetMessage.SendTemporaryAnimation(-1, num9, 593, i, j);
                    break;
                }
            case TileID.VolcanoLarge:
                {
                    int num133;
                    for (num133 = tile.frameY / 18; num133 >= 2; num133 -= 2)
                    {
                    }
                    num133 = j - num133;
                    int num134 = tile.frameX / 18;
                    if (num134 > 1)
                    {
                        num134 -= 2;
                    }
                    num134 = i - num134;
                    Wiring.SkipWire(num134, num133);
                    Wiring.SkipWire(num134, num133 + 1);
                    Wiring.SkipWire(num134 + 1, num133);
                    Wiring.SkipWire(num134 + 1, num133 + 1);
                    short num135 = (short)((Main.tile[num134, num133].frameX != 0) ? (-36) : 36);
                    for (int num136 = 0; num136 < 2; num136++)
                    {
                        for (int num137 = 0; num137 < 2; num137++)
                        {
                            Main.tile[num134 + num136, num133 + num137].frameX += num135;
                        }
                    }
                    NetMessage.SendTileSquare(-1, num134, num133, 2, 2);
                    int num138 = ((num135 > 0) ? 4 : 3);
                    Animation.NewTemporaryAnimation(num138, 594, num134, num133);
                    NetMessage.SendTemporaryAnimation(-1, num138, 594, num134, num133);
                    break;
                }
            case TileID.Chandeliers:
                Wiring.ToggleChandelier(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
                break;
            case TileID.MinecartTrack:
                if (Wiring.CheckMech(i, j, 5))
                {
                    Minecart.FlipSwitchTrack(i, j);
                }
                break;
            case TileID.Candles:
            case TileID.WaterCandle:
            case TileID.PlatinumCandle:
            case TileID.PeaceCandle:
            case TileID.ShadowCandle:
                Wiring.ToggleCandle(i, j, tile, forcedStateWhereTrueIsOn);
                break;
            case TileID.Lampposts:
                Wiring.ToggleLampPost(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
                break;
            case TileID.Traps:
                {
                    int num44 = tile.frameY / 18;
                    Vector2 projPosition = Vector2.Zero;
                    float speedX = 0f;
                    float speedY = 0f;
                    int projType = 0;
                    int damage3 = 0;
                    switch (num44)
                    {
                        case TileStyleID.Traps.DartTrap:
                        case TileStyleID.Traps.SuperDartTrap:
                        case TileStyleID.Traps.FlameTrap:
                        case TileStyleID.Traps.VenomDartTrap:
                            if (Wiring.CheckMech(i, j, MechInfo.StaticDartTrapCoolingTime))
                            {
                                int num54 = ((tile.frameX == 0) ? (-1) : ((tile.frameX == 18) ? 1 : 0));
                                int num55 = ((tile.frameX >= 36) ? ((tile.frameX >= 72) ? 1 : (-1)) : 0);
                                projPosition = new Vector2(i * 16 + 8 + 10 * num54, j * 16 + 8 + 10 * num55);
                                float num56 = 3f;
                                if (num44 == TileStyleID.Traps.DartTrap)
                                {
                                    projType = ProjectileID.PoisonDart;
                                    damage3 = 20;
                                    num56 = 12f;
                                }
                                if (num44 == TileStyleID.Traps.SuperDartTrap)
                                {
                                    projType = ProjectileID.PoisonDartTrap;
                                    damage3 = 40;
                                    num56 = 12f;
                                }
                                if (num44 == TileStyleID.Traps.FlameTrap)
                                {
                                    projType = ProjectileID.FlamethrowerTrap;
                                    damage3 = 40;
                                    num56 = 5f;
                                }
                                if (num44 == TileStyleID.Traps.VenomDartTrap)
                                {
                                    projType = ProjectileID.VenomDartTrap;
                                    damage3 = 30;
                                    num56 = 12f;
                                }
                                speedX = num54 * num56;
                                speedY = num55 * num56;
                            }
                            break;
                        case TileStyleID.Traps.SpikyBallTrap:
                            {
                                if (!Wiring.CheckMech(i, j, MechInfo.StaticSpikyBallTrapCoolingTime))
                                {
                                    break;
                                }
                                int num49 = 200;
                                for (int num50 = 0; num50 < 1000; num50++)
                                {
                                    if (Main.projectile[num50].active && Main.projectile[num50].type == projType)
                                    {
                                        float num51 = (new Vector2(i * 16 + 8, j * 18 + 8) - Main.projectile[num50].Center).Length();
                                        num49 = ((num51 < 50f) ? (num49 - 50) : ((num51 < 100f) ? (num49 - 15) : ((num51 < 200f) ? (num49 - 10) : ((num51 < 300f) ? (num49 - 8) : ((num51 < 400f) ? (num49 - 6) : ((num51 < 500f) ? (num49 - 5) : ((num51 < 700f) ? (num49 - 4) : ((num51 < 900f) ? (num49 - 3) : ((!(num51 < 1200f)) ? (num49 - 1) : (num49 - 2))))))))));
                                    }
                                }
                                if (num49 > 0)
                                {
                                    projType = ProjectileID.SpikyBallTrap;
                                    damage3 = 40;
                                    int num52 = 0;
                                    int num53 = 0;
                                    switch (tile.frameX / 18)
                                    {
                                        case 0:
                                        case 1:
                                            num52 = 0;
                                            num53 = 1;
                                            break;
                                        case 2:
                                            num52 = 0;
                                            num53 = -1;
                                            break;
                                        case 3:
                                            num52 = -1;
                                            num53 = 0;
                                            break;
                                        case 4:
                                            num52 = 1;
                                            num53 = 0;
                                            break;
                                    }
                                    speedX = 4 * num52 + Main.rand.Next(-20 + ((num52 == 1) ? 20 : 0), 21 - ((num52 == -1) ? 20 : 0)) * 0.05f;
                                    speedY = 4 * num53 + Main.rand.Next(-20 + ((num53 == 1) ? 20 : 0), 21 - ((num53 == -1) ? 20 : 0)) * 0.05f;
                                    projPosition = new Vector2(i * 16 + 8 + 14 * num52, j * 16 + 8 + 14 * num53);
                                }
                                break;
                            }
                        case TileStyleID.Traps.SpearTrap:
                            if (Wiring.CheckMech(i, j, MechInfo.StaticSpearTrapCoolingTime))
                            {
                                int num47 = 0;
                                int num48 = 0;
                                switch (tile.frameX / 18)
                                {
                                    case 0:
                                    case 1:
                                        num47 = 0;
                                        num48 = 1;
                                        break;
                                    case 2:
                                        num47 = 0;
                                        num48 = -1;
                                        break;
                                    case 3:
                                        num47 = -1;
                                        num48 = 0;
                                        break;
                                    case 4:
                                        num47 = 1;
                                        num48 = 0;
                                        break;
                                }
                                speedX = 8 * num47;
                                speedY = 8 * num48;
                                damage3 = 60;
                                projType = ProjectileID.SpearTrap;
                                projPosition = new Vector2(i * 16 + 8 + 18 * num47, j * 16 + 8 + 18 * num48);
                            }
                            break;
                    }
                    switch (num44)
                    {
                        case -10:
                            if (Wiring.CheckMech(i, j, MechInfo.StaticDartTrapCoolingTime))
                            {
                                int num62 = -1;
                                if (tile.frameX != 0)
                                {
                                    num62 = 1;
                                }
                                speedX = 12 * num62;
                                damage3 = 20;
                                projType = ProjectileID.PoisonDart;
                                projPosition = new Vector2(i * 16 + 8, j * 16 + 7);
                                projPosition.X += 10 * num62;
                                projPosition.Y += 2f;
                            }
                            break;
                        case -9:
                            if (Wiring.CheckMech(i, j, MechInfo.StaticDartTrapCoolingTime))
                            {
                                int num58 = -1;
                                if (tile.frameX != 0)
                                {
                                    num58 = 1;
                                }
                                speedX = 12 * num58;
                                damage3 = 40;
                                projType = ProjectileID.PoisonDartTrap;
                                projPosition = new Vector2(i * 16 + 8, j * 16 + 7);
                                projPosition.X += 10 * num58;
                                projPosition.Y += 2f;
                            }
                            break;
                        case -8:
                            if (Wiring.CheckMech(i, j, MechInfo.StaticDartTrapCoolingTime))
                            {
                                int num63 = -1;
                                if (tile.frameX != 0)
                                {
                                    num63 = 1;
                                }
                                speedX = 5 * num63;
                                damage3 = 40;
                                projType = ProjectileID.FlamethrowerTrap;
                                projPosition = new Vector2(i * 16 + 8, j * 16 + 7);
                                projPosition.X += 10 * num63;
                                projPosition.Y += 2f;
                            }
                            break;
                        case -7:
                            {
                                if (!Wiring.CheckMech(i, j, MechInfo.StaticSpikyBallTrapCoolingTime))
                                {
                                    break;
                                }
                                projType = 185;
                                int num59 = 200;
                                for (int num60 = 0; num60 < 1000; num60++)
                                {
                                    if (Main.projectile[num60].active && Main.projectile[num60].type == projType)
                                    {
                                        float num61 = (new Vector2(i * 16 + 8, j * 18 + 8) - Main.projectile[num60].Center).Length();
                                        num59 = ((num61 < 50f) ? (num59 - 50) : ((num61 < 100f) ? (num59 - 15) : ((num61 < 200f) ? (num59 - 10) : ((num61 < 300f) ? (num59 - 8) : ((num61 < 400f) ? (num59 - 6) : ((num61 < 500f) ? (num59 - 5) : ((num61 < 700f) ? (num59 - 4) : ((num61 < 900f) ? (num59 - 3) : ((!(num61 < 1200f)) ? (num59 - 1) : (num59 - 2))))))))));
                                    }
                                }
                                if (num59 > 0)
                                {
                                    speedX = Main.rand.Next(-20, 21) * 0.05f;
                                    speedY = 4f + Main.rand.Next(0, 21) * 0.05f;
                                    damage3 = 40;
                                    projPosition = new Vector2(i * 16 + 8, j * 16 + 16);
                                    projPosition.Y += 6f;
                                    Projectile.NewProjectile(Wiring.GetProjectileSource(i, j), (int)projPosition.X, (int)projPosition.Y, speedX, speedY, projType, damage3, 2f, Main.myPlayer);
                                }
                                break;
                            }
                        case -6:
                            if (Wiring.CheckMech(i, j, MechInfo.StaticSpearTrapCoolingTime))
                            {
                                speedX = 0f;
                                speedY = 8f;
                                damage3 = 60;
                                projType = ProjectileID.SpearTrap;
                                projPosition = new Vector2(i * 16 + 8, j * 16 + 16);
                                projPosition.Y += 10f;
                            }
                            break;
                    }
                    if (projType != 0)
                    {
                        Projectile.NewProjectile(Wiring.GetProjectileSource(i, j), (int)projPosition.X, (int)projPosition.Y, speedX, speedY, projType, damage3, 2f, Main.myPlayer);
                    }
                    break;
                }
            case TileID.GeyserTrap:
                //Wiring.GeyserTrap(i, j);
                int num = tile.frameX / 36;
                int num2 = i - (tile.frameX - num * 36) / 18;
                if (Wiring.CheckMech(num2, j, MechInfo.StaticGeyserTrapCoolingTime))
                {
                    Vector2 position;
                    Vector2 speed;
                    if (num < 2)
                    {
                        position = new Vector2(num2 + 1, j) * 16f;
                        speed = new Vector2(0f, -8f);
                    }
                    else
                    {
                        position = new Vector2(num2 + 1, j + 1) * 16f;
                        speed = new Vector2(0f, 8f);
                    }

                    Projectile.NewProjectile(Wiring.GetProjectileSource(num2, j), (int)position.X, (int)position.Y, speed.X, speed.Y, ProjectileID.GeyserTrap, 20, 2f, Main.myPlayer);
                }
                break;
            case TileID.BoulderStatue:
                {
                    int num31 = tile.frameX / 36;
                    int num32 = tile.frameY / 54;
                    int num33 = i - (tile.frameX - (num31 * 36)) / 18;
                    int num34 = j - (tile.frameY - (num32 * 54)) / 18;
                    if (Wiring.CheckMech(num33, num34, MechInfo.StaticBoulderStatueCoolingTime))
                    {
                        Vector2 vector2 = new Vector2(num33 + 1, num34) * 16f;
                        vector2.Y += 28f;
                        Projectile.NewProjectile(Wiring.GetProjectileSource(num33, num34), (int)vector2.X, (int)vector2.Y, 0f, 0f, ProjectileID.Boulder, 70, (float)10f, Main.myPlayer);
                    }
                    break;
                }
            case TileID.Jackolanterns:
            case TileID.MusicBoxes:
                WorldGen.SwitchMB(i, j);
                break;
            case TileID.WaterFountain:
                WorldGen.SwitchFountain(i, j);
                break;
            case TileID.LunarMonolith:
            case TileID.BloodMoonMonolith:
            case TileID.VoidMonolith:
            case TileID.EchoMonolith:
            case TileID.ShimmerMonolith:
                WorldGen.SwitchMonolith(i, j);
                break;
            case TileID.PartyMonolith:
                BirthdayParty.ToggleManualParty();
                break;
            case TileID.Explosives:
                WorldGen.KillTile(i, j, fail: false, effectOnly: false, noItem: true);
                NetMessage.SendTileSquare(-1, i, j);
                Projectile.NewProjectile(Wiring.GetProjectileSource(i, j), i * 16 + 8, j * 16 + 8, 0f, 0f, ProjectileID.Explosives, 500, 10f, Main.myPlayer);
                break;
            case TileID.LandMine:
                WorldGen.ExplodeMine(i, j, fromWiring: true);
                break;
            case TileID.InletPump:
            case TileID.OutletPump:
                {
                    int num146 = j - tile.frameY / 18;
                    int num147 = tile.frameX / 18;
                    if (num147 > 1)
                    {
                        num147 -= 2;
                    }
                    num147 = i - num147;
                    Wiring.SkipWire(num147, num146);
                    Wiring.SkipWire(num147, num146 + 1);
                    Wiring.SkipWire(num147 + 1, num146);
                    Wiring.SkipWire(num147 + 1, num146 + 1);
                    if (type == 142)
                    {
                        for (int num148 = 0; num148 < 4; num148++)
                        {
                            if (Wiring._numInPump >= 19)
                            {
                                break;
                            }
                            int num150;
                            int num152;
                            switch (num148)
                            {
                                case 0:
                                    num150 = num147;
                                    num152 = num146 + 1;
                                    break;
                                case 1:
                                    num150 = num147 + 1;
                                    num152 = num146 + 1;
                                    break;
                                case 2:
                                    num150 = num147;
                                    num152 = num146;
                                    break;
                                default:
                                    num150 = num147 + 1;
                                    num152 = num146;
                                    break;
                            }
                            Wiring._inPumpX[Wiring._numInPump] = num150;
                            Wiring._inPumpY[Wiring._numInPump] = num152;
                            Wiring._numInPump++;
                        }
                        break;
                    }
                    for (int num153 = 0; num153 < 4; num153++)
                    {
                        if (Wiring._numOutPump >= 19)
                        {
                            break;
                        }
                        int num149;
                        int num151;
                        switch (num153)
                        {
                            case 0:
                                num149 = num147;
                                num151 = num146 + 1;
                                break;
                            case 1:
                                num149 = num147 + 1;
                                num151 = num146 + 1;
                                break;
                            case 2:
                                num149 = num147;
                                num151 = num146;
                                break;
                            default:
                                num149 = num147 + 1;
                                num151 = num146;
                                break;
                        }
                        Wiring._outPumpX[Wiring._numOutPump] = num149;
                        Wiring._outPumpY[Wiring._numOutPump] = num151;
                        Wiring._numOutPump++;
                    }
                    break;
                }
            case TileID.Statues:
                {
                    int num11;
                    int num12 = tile.frameX / 18;
                    int num14 = 0;
                    while (num12 >= 2)
                    {
                        num12 -= 2;
                        num14++;
                    }
                    num12 = i - tile.frameX % 36 / 18;
                    num11 = j - tile.frameY % 54 / 18;
                    int num15 = tile.frameY / 54;
                    num15 %= 3;
                    num14 = tile.frameX / 36 + num15 * 55;
                    Wiring.SkipWire(num12, num11);
                    Wiring.SkipWire(num12, num11 + 1);
                    Wiring.SkipWire(num12, num11 + 2);
                    Wiring.SkipWire(num12 + 1, num11);
                    Wiring.SkipWire(num12 + 1, num11 + 1);
                    Wiring.SkipWire(num12 + 1, num11 + 2);
                    int num16 = num12 * 16 + 16;
                    int num17 = (num11 + 3) * 16;
                    int newNpcIndex = -1;
                    int newNpcType = -1;
                    bool flag3 = true;
                    bool flag4 = false;
                    switch (num14)
                    {
                        case 5:
                            newNpcType = 73;
                            break;
                        case 13:
                            newNpcType = 24;
                            break;
                        case 30:
                            newNpcType = 6;
                            break;
                        case 35:
                            newNpcType = 2;
                            break;
                        case 51:
                            newNpcType = Terraria.Utils.SelectRandom(Main.rand, new short[2] { 299, 538 });
                            break;
                        case 52:
                            newNpcType = 356;
                            break;
                        case 53:
                            newNpcType = 357;
                            break;
                        case 54:
                            newNpcType = Terraria.Utils.SelectRandom(Main.rand, new short[2] { 355, 358 });
                            break;
                        case 55:
                            newNpcType = Terraria.Utils.SelectRandom(Main.rand, new short[2] { 367, 366 });
                            break;
                        case 56:
                            newNpcType = Terraria.Utils.SelectRandom(Main.rand, new short[5] { 359, 359, 359, 359, 360 });
                            break;
                        case 57:
                            newNpcType = 377;
                            break;
                        case 58:
                            newNpcType = 300;
                            break;
                        case 59:
                            newNpcType = Terraria.Utils.SelectRandom(Main.rand, new short[2] { 364, 362 });
                            break;
                        case 60:
                            newNpcType = 148;
                            break;
                        case 61:
                            newNpcType = 361;
                            break;
                        case 62:
                            newNpcType = Terraria.Utils.SelectRandom(Main.rand, new short[3] { 487, 486, 485 });
                            break;
                        case 63:
                            newNpcType = 164;
                            flag3 &= NPC.MechSpawn(num16, num17, NPCID.WallCreeperWall);
                            break;
                        case 64:
                            newNpcType = 86;
                            flag4 = true;
                            break;
                        case 65:
                            newNpcType = 490;
                            break;
                        case 66:
                            newNpcType = 82;
                            break;
                        case 67:
                            newNpcType = 449;
                            break;
                        case 68:
                            newNpcType = 167;
                            break;
                        case 69:
                            newNpcType = 480;
                            break;
                        case 70:
                            newNpcType = 48;
                            break;
                        case 71:
                            newNpcType = Terraria.Utils.SelectRandom(Main.rand, new short[3] { 170, 180, 171 });
                            flag4 = true;
                            break;
                        case 72:
                            newNpcType = 481;
                            break;
                        case 73:
                            newNpcType = 482;
                            break;
                        case 74:
                            newNpcType = 430;
                            break;
                        case 75:
                            newNpcType = 489;
                            break;
                        case 76:
                            newNpcType = 611;
                            break;
                        case 77:
                            newNpcType = 602;
                            break;
                        case 78:
                            newNpcType = Terraria.Utils.SelectRandom(Main.rand, new short[6] { 595, 596, 599, 597, 600, 598 });
                            break;
                        case 79:
                            newNpcType = Terraria.Utils.SelectRandom(Main.rand, new short[2] { 616, 617 });
                            break;
                        case 80:
                            newNpcType = Terraria.Utils.SelectRandom(Main.rand, new short[2] { 671, 672 });
                            break;
                        case 81:
                            newNpcType = 673;
                            break;
                        case 82:
                            newNpcType = Terraria.Utils.SelectRandom(Main.rand, new short[2] { 674, 675 });
                            break;
                    }
                    if (newNpcType != -1 && Wiring.CheckMech(num12, num11, MechInfo.StaticNPCSpawnCoolingTime) && NPC.MechSpawn(num16, num17, newNpcType) && flag3)
                    {
                        if (!flag4 || !Collision.SolidTiles(num12 - 2, num12 + 3, num11, num11 + 2))
                        {
                            newNpcIndex = NPC.NewNPC(Wiring.GetNPCSource(num12, num11), num16, num17, newNpcType);
                        }
                        else
                        {
                            Vector2 position = new Vector2(num16 - 4, num17 - 22) - new Vector2(10f);
                            Terraria.Utils.PoofOfSmoke(position);
                            NetMessage.SendData(MessageID.PoofOfSmoke, -1, -1, null, (int)position.X, position.Y);
                        }
                    }
                    if (newNpcIndex <= -1)
                    {
                        switch (num14)
                        {
                            case 4:
                                if (Wiring.CheckMech(num12, num11, MechInfo.StaticNPCSpawnCoolingTime) && NPC.MechSpawn(num16, num17, NPCID.BlueSlime))
                                {
                                    newNpcIndex = NPC.NewNPC(Wiring.GetNPCSource(num12, num11), num16, num17 - 12, NPCID.BlueSlime);
                                }
                                break;
                            case 7:
                                if (Wiring.CheckMech(num12, num11, MechInfo.StaticNPCSpawnCoolingTime) && NPC.MechSpawn(num16, num17, NPCID.CaveBat))
                                {
                                    newNpcIndex = NPC.NewNPC(Wiring.GetNPCSource(num12, num11), num16 - 4, num17 - 6, NPCID.CaveBat);
                                }
                                break;
                            case 8:
                                if (Wiring.CheckMech(num12, num11, MechInfo.StaticNPCSpawnCoolingTime) && NPC.MechSpawn(num16, num17, NPCID.Goldfish))
                                {
                                    newNpcIndex = NPC.NewNPC(Wiring.GetNPCSource(num12, num11), num16, num17 - 12, NPCID.Goldfish);
                                }
                                break;
                            case 9:
                                {
                                    int type4 = 46;
                                    if (BirthdayParty.PartyIsUp)
                                    {
                                        type4 = 540;
                                    }
                                    if (Wiring.CheckMech(num12, num11, MechInfo.StaticNPCSpawnCoolingTime) && NPC.MechSpawn(num16, num17, type4))
                                    {
                                        newNpcIndex = NPC.NewNPC(Wiring.GetNPCSource(num12, num11), num16, num17 - 12, type4);
                                    }
                                    break;
                                }
                            case 10:
                                if (Wiring.CheckMech(num12, num11, MechInfo.StaticNPCSpawnCoolingTime) && NPC.MechSpawn(num16, num17, NPCID.Skeleton))
                                {
                                    newNpcIndex = NPC.NewNPC(Wiring.GetNPCSource(num12, num11), num16, num17, NPCID.Skeleton);
                                }
                                break;
                            case 16:
                                if (Wiring.CheckMech(num12, num11, MechInfo.StaticNPCSpawnCoolingTime) && NPC.MechSpawn(num16, num17, NPCID.Hornet))
                                {
                                    if (!Collision.SolidTiles(num12 - 1, num12 + 1, num11, num11 + 1))
                                    {
                                        newNpcIndex = NPC.NewNPC(Wiring.GetNPCSource(num12, num11), num16, num17 - 12, NPCID.Hornet);
                                        break;
                                    }
                                    Vector2 position3 = new Vector2(num16 - 4, num17 - 22) - new Vector2(10f);
                                    Terraria.Utils.PoofOfSmoke(position3);
                                    NetMessage.SendData(MessageID.PoofOfSmoke, -1, -1, null, (int)position3.X, position3.Y);
                                }
                                break;
                            case 18:
                                if (Wiring.CheckMech(num12, num11, MechInfo.StaticNPCSpawnCoolingTime) && NPC.MechSpawn(num16, num17, NPCID.Crab))
                                {
                                    newNpcIndex = NPC.NewNPC(Wiring.GetNPCSource(num12, num11), num16, num17 - 12, NPCID.Crab);
                                }
                                break;
                            case 23:
                                if (Wiring.CheckMech(num12, num11, MechInfo.StaticNPCSpawnCoolingTime) && NPC.MechSpawn(num16, num17, NPCID.BlueJellyfish))
                                {
                                    newNpcIndex = NPC.NewNPC(Wiring.GetNPCSource(num12, num11), num16, num17 - 12, NPCID.BlueJellyfish);
                                }
                                break;
                            case 27:
                                if (Wiring.CheckMech(num12, num11, MechInfo.StaticNPCSpawnCoolingTime) && NPC.MechSpawn(num16, num17, NPCID.Mimic))
                                {
                                    newNpcIndex = NPC.NewNPC(Wiring.GetNPCSource(num12, num11), num16 - 9, num17, NPCID.Mimic);
                                }
                                break;
                            case 28:
                                if (Wiring.CheckMech(num12, num11, MechInfo.StaticNPCSpawnCoolingTime) && NPC.MechSpawn(num16, num17, NPCID.Bird))
                                {
                                    newNpcIndex = NPC.NewNPC(Wiring.GetNPCSource(num12, num11), num16, num17 - 12, Terraria.Utils.SelectRandom(Main.rand, new short[3] { 74, 297, 298 }));
                                }
                                break;
                            case 34:
                                {
                                    for (int num29 = 0; num29 < 2; num29++)
                                    {
                                        for (int num30 = 0; num30 < 3; num30++)
                                        {
                                            ITile tile2 = Main.tile[num12 + num29, num11 + num30];
                                            tile2.type = TileID.MushroomStatue;
                                            tile2.frameX = (short)(num29 * 18 + 216);
                                            tile2.frameY = (short)(num30 * 18);
                                        }
                                    }
                                    Animation.NewTemporaryAnimation(0, 349, num12, num11);
                                    NetMessage.SendTileSquare(-1, num12, num11, 2, 3);
                                    break;
                                }
                            case 42:
                                if (Wiring.CheckMech(num12, num11, MechInfo.StaticNPCSpawnCoolingTime) && NPC.MechSpawn(num16, num17, NPCID.Piranha))
                                {
                                    newNpcIndex = NPC.NewNPC(Wiring.GetNPCSource(num12, num11), num16, num17 - 12, NPCID.Piranha);
                                }
                                break;
                            case 37:
                                if (Wiring.CheckMech(num12, num11, MechInfo.StaticItemSpawnCoolingTime) && Item.MechSpawn(num16, num17, 58) && Item.MechSpawn(num16, num17, 1734) && Item.MechSpawn(num16, num17, 1867))
                                {
                                    Item.NewItem(Wiring.GetItemSource(num16, num17), num16, num17 - 16, 0, 0, ItemID.Heart);
                                }
                                break;
                            case 50:
                                if (Wiring.CheckMech(num12, num11, MechInfo.StaticNPCSpawnCoolingTime) && NPC.MechSpawn(num16, num17, NPCID.Shark))
                                {
                                    if (!Collision.SolidTiles(num12 - 2, num12 + 3, num11, num11 + 2))
                                    {
                                        newNpcIndex = NPC.NewNPC(Wiring.GetNPCSource(num12, num11), num16, num17 - 12, NPCID.Shark);
                                        break;
                                    }
                                    Vector2 position2 = new Vector2(num16 - 4, num17 - 22) - new Vector2(10f);
                                    Terraria.Utils.PoofOfSmoke(position2);
                                    NetMessage.SendData(MessageID.PoofOfSmoke, -1, -1, null, (int)position2.X, position2.Y);
                                }
                                break;
                            case 2:
                                if (Wiring.CheckMech(num12, num11, MechInfo.StaticItemSpawnCoolingTime) && Item.MechSpawn(num16, num17, 184) && Item.MechSpawn(num16, num17, 1735) && Item.MechSpawn(num16, num17, 1868))
                                {
                                    Item.NewItem(Wiring.GetItemSource(num16, num17), num16, num17 - 16, 0, 0, ItemID.Star);
                                }
                                break;
                            case 17:
                                if (Wiring.CheckMech(num12, num11, MechInfo.StaticItemSpawnCoolingTime) && Item.MechSpawn(num16, num17, 166))
                                {
                                    Item.NewItem(Wiring.GetItemSource(num16, num17), num16, num17 - 20, 0, 0, ItemID.Bomb);
                                }
                                break;
                            case 40:
                                {
                                    if (!Wiring.CheckMech(num12, num11, 300))
                                    {
                                        break;
                                    }
                                    int num25 = 50;
                                    int[] array2 = new int[num25];
                                    int num26 = 0;
                                    for (int num27 = 0; num27 < 200; num27++)
                                    {
                                        if (Main.npc[num27].active && (Main.npc[num27].type == NPCID.Merchant || Main.npc[num27].type == NPCID.ArmsDealer || Main.npc[num27].type == NPCID.Guide || Main.npc[num27].type == NPCID.Demolitionist || Main.npc[num27].type == NPCID.Clothier || Main.npc[num27].type == NPCID.GoblinTinkerer || Main.npc[num27].type == NPCID.Wizard || Main.npc[num27].type == NPCID.SantaClaus || Main.npc[num27].type == NPCID.Truffle || Main.npc[num27].type == NPCID.DyeTrader || Main.npc[num27].type == NPCID.Cyborg || Main.npc[num27].type == NPCID.Painter || Main.npc[num27].type == NPCID.WitchDoctor || Main.npc[num27].type == NPCID.Pirate || Main.npc[num27].type == NPCID.TravellingMerchant || Main.npc[num27].type == NPCID.Angler || Main.npc[num27].type == NPCID.DD2Bartender || Main.npc[num27].type == NPCID.TaxCollector || Main.npc[num27].type == NPCID.Golfer))
                                        {
                                            array2[num26] = num27;
                                            num26++;
                                            if (num26 >= num25)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    if (num26 > 0)
                                    {
                                        int num28 = array2[Main.rand.Next(num26)];
                                        Main.npc[num28].position.X = num16 - Main.npc[num28].width / 2;
                                        Main.npc[num28].position.Y = num17 - Main.npc[num28].height - 1;
                                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num28);
                                    }
                                    break;
                                }
                            case 41:
                                {
                                    if (!Wiring.CheckMech(num12, num11, 300))
                                    {
                                        break;
                                    }
                                    int num20 = 50;
                                    int[] array = new int[num20];
                                    int num21 = 0;
                                    for (int num22 = 0; num22 < 200; num22++)
                                    {
                                        if (Main.npc[num22].active && (Main.npc[num22].type == NPCID.Nurse || Main.npc[num22].type == NPCID.Dryad || Main.npc[num22].type == NPCID.Mechanic || Main.npc[num22].type == NPCID.Steampunker || Main.npc[num22].type == NPCID.PartyGirl || Main.npc[num22].type == NPCID.Stylist || Main.npc[num22].type == NPCID.BestiaryGirl || Main.npc[num22].type == NPCID.Princess))
                                        {
                                            array[num21] = num22;
                                            num21++;
                                            if (num21 >= num20)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    if (num21 > 0)
                                    {
                                        int num23 = array[Main.rand.Next(num21)];
                                        Main.npc[num23].position.X = num16 - Main.npc[num23].width / 2;
                                        Main.npc[num23].position.Y = num17 - Main.npc[num23].height - 1;
                                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num23);
                                    }
                                    break;
                                }
                        }
                    }
                    if (newNpcIndex >= 0)
                    {
                        Main.npc[newNpcIndex].value = 0f;
                        Main.npc[newNpcIndex].npcSlots = 0f;
                        Main.npc[newNpcIndex].SpawnedFromStatue = true;
                        Main.npc[newNpcIndex].CanBeReplacedByOtherNPCs = true;
                    }
                    break;
                }
            case TileID.MushroomStatue:
                {
                    int num139 = tile.frameY / 18;
                    num139 %= 3;
                    int num140 = j - num139;
                    int num141;
                    for (num141 = tile.frameX / 18; num141 >= 2; num141 -= 2)
                    {
                    }
                    num141 = i - num141;
                    Wiring.SkipWire(num141, num140);
                    Wiring.SkipWire(num141, num140 + 1);
                    Wiring.SkipWire(num141, num140 + 2);
                    Wiring.SkipWire(num141 + 1, num140);
                    Wiring.SkipWire(num141 + 1, num140 + 1);
                    Wiring.SkipWire(num141 + 1, num140 + 2);
                    short num142 = (short)((Main.tile[num141, num140].frameX != 0) ? (-216) : 216);
                    for (int num144 = 0; num144 < 2; num144++)
                    {
                        for (int num145 = 0; num145 < 3; num145++)
                        {
                            Main.tile[num141 + num144, num140 + num145].frameX += num142;
                        }
                    }
                    NetMessage.SendTileSquare(-1, num141, num140, 2, 3);
                    Animation.NewTemporaryAnimation((num142 <= 0) ? 1 : 0, 349, num141, num140);
                    break;
                }
            case TileID.CatBast:
                {
                    int num121 = tile.frameY / 18;
                    num121 %= 3;
                    int num123 = j - num121;
                    int num124;
                    for (num124 = tile.frameX / 18; num124 >= 2; num124 -= 2)
                    {
                    }
                    num124 = i - num124;
                    Wiring.SkipWire(num124, num123);
                    Wiring.SkipWire(num124, num123 + 1);
                    Wiring.SkipWire(num124, num123 + 2);
                    Wiring.SkipWire(num124 + 1, num123);
                    Wiring.SkipWire(num124 + 1, num123 + 1);
                    Wiring.SkipWire(num124 + 1, num123 + 2);
                    short num125 = (short)((Main.tile[num124, num123].frameX >= 72) ? (-72) : 72);
                    for (int num126 = 0; num126 < 2; num126++)
                    {
                        for (int num127 = 0; num127 < 3; num127++)
                        {
                            Main.tile[num124 + num126, num123 + num127].frameX += num125;
                        }
                    }
                    NetMessage.SendTileSquare(-1, num124, num123, 2, 3);
                    break;
                }
            case TileID.Grate:
                tile.type = TileID.GrateClosed;
                WorldGen.SquareTileFrame(i, j);
                NetMessage.SendTileSquare(-1, i, j);
                break;
            case TileID.GrateClosed:
                tile.type = TileID.Grate;
                WorldGen.SquareTileFrame(i, j);
                NetMessage.SendTileSquare(-1, i, j);
                break;
        }
    }

}
