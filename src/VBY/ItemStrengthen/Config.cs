using VBY.Common.Config;

namespace VBY.ItemStrengthen;

public class Config : ConfigBase<Root>
{
    public Config(string configDirectory, string fileName = "ItemStrengthen.json") : base(configDirectory, fileName)
    {
    }
}
public class Root : RootBase
{
    public List<ItemInfo> ItemInfos = new();
}
public class ItemInfo
{
    public int type;
    public byte prefix;
    public string? scale;
    public string? width;
    public string? height;
    public string? damage;
    public string? useTime;
    public string? knockBack;
    public string? shootSpeed;
    public string? useAnimation;
    public string? ammo;
    public string? shoot;
    public string? color;
    public string? useAmmo;
    public bool notAmmo;
    public ItemModifyInfo ToModifyInfo()
    {
        var result = new ItemModifyInfo
        {
            type = type,
            prefix = prefix,
            scale = ParseModifyInfo(scale),
            width = ParseModifyInfo(width),
            height = ParseModifyInfo(height),
            damage = ParseModifyInfo(damage),
            useTime = ParseModifyInfo(useTime),
            knockBack = ParseModifyInfo(knockBack),
            shootSpeed = ParseModifyInfo(shootSpeed),
            useAnimation = ParseModifyInfo(useAnimation),
            ammo = ParseModifyInfo(ammo),
            shoot = ParseModifyInfo(shoot),
            color = ParseModifyInfo(color),
            useAmmo = ParseModifyInfo(useAmmo),
            notAmmo = notAmmo
        };
        return result;
    }
    public static ModifyInfo? ParseModifyInfo(string? s)
    {
        if (string.IsNullOrEmpty(s))
            return null;
        var result = new ModifyInfo();
        if (s.StartsWith('+') || s.StartsWith('-'))
        {
            if(s.EndsWith('%'))
            {
                result.ModifyType = ModifyType.Increase;
                result.Value = float.Parse(s[1..^1]);
            }
            else
            {
                result.ModifyType = ModifyType.Add;
                result.Value = float.Parse(s[1..]);
            }
        }
        else if (s.StartsWith("="))
        {
            result.ModifyType = ModifyType.Change;
            result.Value = float.Parse(s[1..]);
        }
        return result;
    }
}
public enum ModifyType
{
    Add, Change, Increase
}
public class ModifyInfo
{
    public ModifyType ModifyType;
    public float Value;
}
public class ItemModifyInfo
{
    public int type;
    public byte prefix;
    public ModifyInfo? scale, width, height, damage, useTime, knockBack, shootSpeed, useAnimation, ammo, shoot, color, useAmmo;
    public bool notAmmo;
}