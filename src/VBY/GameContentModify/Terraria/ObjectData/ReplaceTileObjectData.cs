using Terraria;
using Terraria.ObjectData;

namespace VBY.GameContentModify.ObjectData;

[ReplaceType(typeof(TileObjectData))]
public  static class ReplaceTileObjectData
{
    public static TileObjectData? GetTileData(On.Terraria.ObjectData.TileObjectData.orig_GetTileData_ITile orig, ITile? getTile)
    {
        if (getTile == null || !getTile.active())
        {
            return null;
        }
        int type = getTile.type;
        if (type < 0 || type >= TileObjectData._data.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(getTile), "Function called with a bad tile type");
        }
        TileObjectData tileObjectData = TileObjectData._data[type];
        if (tileObjectData == null)
        {
            return null;
        }
        int num = getTile.frameX / tileObjectData.CoordinateFullWidth;
        int num2 = getTile.frameY / tileObjectData.CoordinateFullHeight;
        int num3 = tileObjectData.StyleWrapLimit;
        if (num3 == 0)
        {
            num3 = 1;
        }
        int num4 = ((!tileObjectData.StyleHorizontal) ? (num * num3 + num2) : (num2 * num3 + num));
        int num5 = num4 / tileObjectData.StyleMultiplier;
        int num6 = num4 % tileObjectData.StyleMultiplier;
        int styleLineSkip = tileObjectData.StyleLineSkip;
        if (styleLineSkip > 1)
        {
            if (tileObjectData.StyleHorizontal)
            {
                num5 = num2 / styleLineSkip * num3 + num;
                num6 = num2 % styleLineSkip;
            }
            else
            {
                num5 = num / styleLineSkip * num3 + num2;
                num6 = num % styleLineSkip;
            }
        }
        if (tileObjectData.SubTiles != null && num5 >= 0 && num5 < tileObjectData.SubTiles.Count)
        {
            TileObjectData tileObjectData2 = tileObjectData.SubTiles[num5];
            if (tileObjectData2 != null)
            {
                //tileObjectData = tileObjectData2;
                return tileObjectData2;
            }
        }
        if (tileObjectData._alternates != null)
        {
            for (int i = 0; i < tileObjectData.Alternates.Count; i++)
            {
                TileObjectData tileObjectData3 = tileObjectData.Alternates[i];
                if (tileObjectData3 != null && num6 >= tileObjectData3.Style && num6 <= tileObjectData3.Style + tileObjectData3.RandomStyleRange)
                {
                    return tileObjectData3;
                }
            }
        }
        return tileObjectData;
    }

}
