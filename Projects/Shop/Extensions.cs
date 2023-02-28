using TShockAPI;

using VBY.Basic.Command;

namespace VBY.Shop;

public static class Extensions
{
    public static bool CheckAllow(this ICheck check, TSPlayer player)
    {
        return ProgressQuery.ProgressQuery.BossCheck(check.Progress) && Utils.ZoneCheck(player, check.Zone) && Utils.GroupCheck(player, check.FGroup);
    }
    public static bool FindPlayer(this TSPlayer player, out ShopPlayer shopPlayer)
    {
        shopPlayer = Shop.Players[player.Index];
        if (shopPlayer is null)
        {
            player.SendErrorMessage("你的ShopPlayer不存在,请尝试重进服务器");
            return false;
        }
        return true;
    }
    public static SubCmdNodeList AddBuyAndList<T>(this SubCmdNodeList list) where T : Shops
    {
        list.AddCmd(Shop.Buy<T>, "购买商品", "<商品ID> [商品数量=1]", 2);
        list.AddCmd(Shop.List<T>, "列出商品", 2);
        return list.Parent!;
    }
    public static SubCmdNodeList AddBuy1AndList<T>(this SubCmdNodeList list) where T : Shops
    {
        list.AddCmd(Shop.Buy1<T>, "购买商品", "<商品ID>", 2);
        list.AddCmd(Shop.List<T>, "列出商品", 2);
        return list.Parent!;
    }
}
