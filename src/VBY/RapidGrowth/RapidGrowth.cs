using System.ComponentModel;

using Terraria;
using Terraria.ID;
using TerrariaApi.Server;

using VBY.Common;
using VBY.Common.Hook;

namespace VBY.RapidGrowth;

[ApiVersion(2, 1)]
[Description("快速生长")]
public class RapidGrowth : CommonPlugin
{
    public override string Author => "yu";
    public RapidGrowth(Main game) : base(game)
    {
    }
    [AutoHook]
    public static bool OnWorldGen_PlaceObject(On.Terraria.WorldGen.orig_PlaceObject orig, int x, int y, int type, bool mute, int style, int alternate, int random, int direction)
    {
        //if(type == TileID.Tables)
        //{
        //    type = TileID.LargePiles2;
        //    style = 17;
        //    Console.WriteLine(nameof(OnWorldGen_PlaceObject));
        //    var r = orig(x, y, type, mute, style, alternate, random, direction);
        //    NetMessage.SendTileSquare(-1, x, y, 3);
        //    return r;
        //}
        var result = orig(x, y, type, mute, style, alternate, random, direction);
        if (result && type == TileID.Saplings && style == 0 && Main.rand.Next(2) == 0)
        {
            WorldGen.TryGrowingTreeByType(type, x, y);
            WorldGen.GrowTree(x, y);
        }
        return result;
    }
}