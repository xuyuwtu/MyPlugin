namespace VBY.Shop;

public static class PrintName
{
    public const string BuyId = nameof(BuyId);
    public const string Price = nameof(Price);
    public const string MoneyName = nameof(MoneyName);

    public const string Type = nameof(Type);
    public const string Stack = nameof(Stack);
    public const string Prefix = nameof(Prefix);
    public const string ItemName = nameof(ItemName);
    public const string PrefixName = nameof(PrefixName);

    public const string Start = nameof(Start);
    public const string End = nameof(End);

    public const string BuffName = nameof(BuffName);

    public const string Name = nameof(Name);

    public const string MaxStack = nameof(MaxStack);
    public const string NpcName = nameof(NpcName);

    public const string Style = nameof(Style);
    public const string Size = nameof(Size);
    public static Dictionary<string, string> Formats = new();
    static PrintName()
    {
        var shops = string.Join(',', BuyId, Price, MoneyName);
        var itemShops = string.Join(',', shops, Type, Stack, Prefix, ItemName, PrefixName);
        var lifeShops = string.Join(',', shops, Start, End);
        Formats.Add(nameof(TableInfo.BuffShop), string.Join(',', shops, Type, BuffName));
        Formats.Add(nameof(TableInfo.ItemChangeShop), string.Join(',', shops, Name));
        Formats.Add(nameof(TableInfo.ItemSystemShop), string.Join(',', itemShops));
        Formats.Add(nameof(TableInfo.ItemPayShop), string.Join(',', itemShops));
        Formats.Add(nameof(TableInfo.LifeHealShop), string.Join(',', lifeShops));
        Formats.Add(nameof(TableInfo.LifeMaxShop), string.Join(',', lifeShops));
        Formats.Add(nameof(TableInfo.NpcShop), string.Join(',', shops, Type, MaxStack, NpcName));
        Formats.Add(nameof(TableInfo.TileShop), string.Join(',', shops, Style, Size));
    }
}
