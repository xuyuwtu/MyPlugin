using System.Reflection.Emit;
using TShockAPI;
using VBY.Common;
using VBY.Common.Command;

namespace VBY.Shop;

public static class Extensions
{
    public static bool CheckAllow(this ICheck check, TSPlayer player)
    {
        return ProgressQuery.ProgressQuery.BossCheck(check.Progress) && Utils.ZoneCheck(player, check.Zone) && Utils.GroupCheck(player, check.FGroup);
    }
    public static bool GetShopPlayer(this TSPlayer player, out ShopPlayer shopPlayer)
    {
        shopPlayer = ShopPlugin.Players[player.Index];
        if (shopPlayer is null)
        {
            player.SendErrorMessage("你的ShopPlayer不存在,请尝试重进服务器");
            return false;
        }
        return true;
    }
    public static SubCmdList AddBuyAndList<T>(this SubCmdList list) where T : Shops
    {
        list.AddCmd(ShopPlugin.Buy<T>, "购买商品", "<商品ID> [商品数量=1]", 2);
        list.AddCmd(ShopPlugin.List<T>, "列出商品", 2);
        return list.Parent!;
    }
    public static SubCmdList AddAddAndDel<T>(this SubCmdList list) where T : Shops
    {
        list.AddCmd(ShopPlugin.Add<T>, 2);
        list.AddCmd(ShopPlugin.Del<T>, 2);
        return list.Parent!;
    }
    public static SubCmdList AddBuy1AndList<T>(this SubCmdList list) where T : Shops
    {
        list.AddCmd(ShopPlugin.Buy1<T>, "购买商品", "<商品ID>", 2);
        list.AddCmd(ShopPlugin.List<T>, "列出商品", 2);
        return list.Parent!;
    }
#nullable disable
    public static void EmitGetProperty<T>(this ILGenerator iLGenerator,string propertyName)
    {
        iLGenerator.Emit(OpCodes.Callvirt, typeof(T).GetProperty(propertyName).GetGetMethod());
    }
    public static void EmitTShockMethod<T>(this ILGenerator iLGenerator,string fieldName,string methodName)
    {
        iLGenerator.Emit(OpCodes.Ldsfld, TypeOf.TShock.GetField(nameof(TShock.Utils))); 
        iLGenerator.Emit(OpCodes.Ldarg_0);
        iLGenerator.Emit(OpCodes.Ldfld, typeof(T).GetField(fieldName));
        iLGenerator.Emit(OpCodes.Callvirt, typeof(TShockAPI.Utils).GetMethod(methodName));
    }
#nullable enable
}
