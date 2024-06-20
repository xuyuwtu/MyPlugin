using Terraria;

namespace VBY.GameContentModify;

[ReplaceType(typeof(Liquid))]
public static class ReplaceLiquid
{
    public static void DelWater(int l)
    {
        int num = Main.liquid[l].x;
        int num2 = Main.liquid[l].y;
        ITile tile = Main.tile[num - 1, num2];
        ITile tile2 = Main.tile[num + 1, num2];
        ITile tile3 = Main.tile[num, num2 + 1];
        ITile tile4 = Main.tile[num, num2];
        byte b = 2;
        if (tile4.liquid < b)
        {
            tile4.liquid = 0;
            if (tile.liquid < b)
            {
                tile.liquid = 0;
            }
            else
            {
                Liquid.AddWater(num - 1, num2);
            }
            if (tile2.liquid < b)
            {
                tile2.liquid = 0;
            }
            else
            {
                Liquid.AddWater(num + 1, num2);
            }
        }
        else if (tile4.liquid < 20)
        {
            if ((tile.liquid < tile4.liquid && (!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type])) || (tile2.liquid < tile4.liquid && (!tile2.nactive() || !Main.tileSolid[tile2.type] || Main.tileSolidTop[tile2.type])) || (tile3.liquid < byte.MaxValue && (!tile3.nactive() || !Main.tileSolid[tile3.type] || Main.tileSolidTop[tile3.type])))
            {
                tile4.liquid = 0;
            }
        }
        else if (tile3.liquid < byte.MaxValue && (!tile3.nactive() || !Main.tileSolid[tile3.type] || Main.tileSolidTop[tile3.type]) && !Liquid.stuck && (!Main.tile[num, num2].nactive() || !Main.tileSolid[Main.tile[num, num2].type] || Main.tileSolidTop[Main.tile[num, num2].type]))
        {
            Main.liquid[l].kill = 0;
            return;
        }
        if (tile4.liquid < 250 && Main.tile[num, num2 - 1].liquid > 0)
        {
            Liquid.AddWater(num, num2 - 1);
        }
        if (tile4.liquid == 0)
        {
            tile4.liquidType(0);
        }
        else
        {
            if (tile2.liquid > 0 && tile2.liquid < 250 && (!tile2.nactive() || !Main.tileSolid[tile2.type] || Main.tileSolidTop[tile2.type]) && tile4.liquid != tile2.liquid)
            {
                Liquid.AddWater(num + 1, num2);
            }
            if (tile.liquid > 0 && tile.liquid < 250 && (!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]) && tile4.liquid != tile.liquid)
            {
                Liquid.AddWater(num - 1, num2);
            }
            if (tile4.lava())
            {
                Liquid.LavaCheck(num, num2); 
                //for (int i = num - 1; i <= num + 1; i++)
                //{
                //    for (int j = num2 - 1; j <= num2 + 1; j++)
                //    {
                //        ITile tile5 = Main.tile[i, j];
                //        if (!tile5.active())
                //        {
                //            continue;
                //        }
                //        if (tile5.type == 2 || tile5.type == 23 || tile5.type == 109 || tile5.type == 199 || tile5.type == 477 || tile5.type == 492)
                //        {
                //            tile5.type = 0;
                //            WorldGen.SquareTileFrame(i, j);
                //            if (Main.netMode == 2)
                //            {
                //                NetMessage.SendTileSquare(-1, num, num2, 3);
                //            }
                //        }
                //        else if (tile5.type == 60 || tile5.type == 70 || tile5.type == 661 || tile5.type == 662)
                //        {
                //            tile5.type = 59;
                //            WorldGen.SquareTileFrame(i, j);
                //            if (Main.netMode == 2)
                //            {
                //                NetMessage.SendTileSquare(-1, num, num2, 3);
                //            }
                //        }
                //    }
                //}
            }
            else if (tile4.honey())
            {
                Liquid.HoneyCheck(num, num2);
            }
            else if (tile4.shimmer())
            {
                Liquid.ShimmerCheck(num, num2);
            }
        }
        Liquid.NetSendLiquid(num, num2);
        Liquid.numLiquid--;
        Main.tile[Main.liquid[l].x, Main.liquid[l].y].checkingLiquid(checkingLiquid: false);
        Main.liquid[l].x = Main.liquid[Liquid.numLiquid].x;
        Main.liquid[l].y = Main.liquid[Liquid.numLiquid].y;
        Main.liquid[l].kill = Main.liquid[Liquid.numLiquid].kill;
        if (Main.tileAlch[tile4.type])
        {
            WorldGen.CheckAlch(num, num2);
        }
        else if (tile4.type == 518)
        {
            if (Liquid.quickFall)
            {
                WorldGen.CheckLilyPad(num, num2);
            }
            else if (Main.tile[num, num2 + 1].liquid < byte.MaxValue || Main.tile[num, num2 - 1].liquid > 0)
            {
                WorldGen.SquareTileFrame(num, num2);
            }
            else
            {
                WorldGen.CheckLilyPad(num, num2);
            }
        }
    }
}
