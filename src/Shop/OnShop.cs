using VBY.Common.Config;
using VBY.Shop.Config;

namespace VBY.Shop;

public partial class ShopPlugin
{
    public static void OnPostRead(ConfigBase<Root> config)
    {
        if (Shops.PrintFormats.Count == 0)
        {
            return;
        }

        var shops = config.Root.Shops;
        ReadConfig.Shops = new()
        {
            { nameof(TableInfo.ItemSystemShop), shops.ItemShop.System },
            { nameof(TableInfo.ItemChangeShop), shops.ItemShop.Change },
            { nameof(TableInfo.ItemPayShop), shops.ItemShop.Pay },
            { nameof(TableInfo.LifeHealShop), shops.LifeShop.Heal },
            { nameof(TableInfo.LifeMaxShop), shops.LifeShop.Max },
            { nameof(TableInfo.BuffShop), shops.BuffShop },
            { nameof(TableInfo.TileShop), shops.TileShop },
            { nameof(TableInfo.NpcShop), shops.NpcShop }
        };
        foreach (var shop in ReadConfig.Shops)
        {
            Common.Utils.FormatReplace(ref shop.Value.SystemFormat, Shops.PrintFormats[shop.Key]);
            Common.Utils.FormatReplace(ref shop.Value.PlayerFormat, Shops.PrintFormats[shop.Key]);
        }
    }
}
