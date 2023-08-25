using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ObjectData;

namespace VBY.GameContentModify;

public static class ReplaceProjectile
{
    public static bool GasTrapCheck(int x, int y, Player user)
    {
        //if (Main.netMode == 1)
        //{
        //    return false;
        //}
        int num = Chest.FindChest(x, y);
        if (num < 0)
        {
            return false;
        }
        Chest chest = Main.chest[num];
        int num2 = 16;
        int num3 = 16;
        bool flag = false;
        int trapCount = 0;
        for (int i = 0; i < 40; i++)
        {
            ushort chestType = Main.tile[chest.x, chest.y].type;
            if (TileObjectData.CustomPlace(chestType, 0))
            {
                TileObjectData tileData = TileObjectData.GetTileData(chestType, 0);
                if (tileData != null)
                {
                    int num5 = (int)Math.Ceiling(tileData.Width / 2f);
                    int num6 = (int)Math.Ceiling(tileData.Width / 2f);
                    num2 = num5 * 16;
                    num3 = num6 * 16;
                    if (num5 % 2 != 0)
                    {
                        num2 += 8;
                    }
                    if (num6 % 2 != 0)
                    {
                        num3 += 8;
                    }
                }
            }
            if (chest.item[i] != null && chest.item[i].type == 5346)
            {
                Projectile.UseGasTrapInChest(num, chest, i, num2, num3);
                flag = true;
                if (GameContentModify.MainConfig.Instance.GasTrapsSuperpose)
                {
                    trapCount++;
                }
                else
                {
                    break;
                }
            }
        }
        if (flag)
        {
            num3 -= 8;
            Projectile.NewProjectile(user.GetProjectileSource_TileInteraction(x, y), new Vector2(x * 16 + num2, y * 16 + num3), Vector2.Zero, GameContentModify.MainConfig.Instance.GasTrapsProjType, GameContentModify.MainConfig.Instance.GasTraosProjDamage * trapCount, 0f, Main.myPlayer);
        }
        return flag;
    }
}
