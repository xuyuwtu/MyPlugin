using System.Diagnostics.CodeAnalysis;

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
    private Dictionary<string, int>? progressNames;
    private Dictionary<string, int>? itemNames;
    private Dictionary<string, int>? prefixNames;
    public List<ItemInfo> ItemInfos = new();

    public Dictionary<string, int> ProgressNames
    {
        get => (progressNames ??= new Dictionary<string, int>(StringComparer.Ordinal));
        set => progressNames = value;
    }
    public Dictionary<string, int> ItemNames
    {
        get => (itemNames ??= new Dictionary<string, int>(StringComparer.Ordinal));
        set => itemNames = value;
    }
    public Dictionary<string, int> PrefixNames
    {
        get => (prefixNames ??= new Dictionary<string, int>(StringComparer.Ordinal));
        set => prefixNames = value;
    }
}
public class ItemInfo
{
    public object? type;
    public object? prefix;
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
    public bool TryToModifyInfo([MaybeNullWhen(false)] out ItemModifyInfo info, [MaybeNullWhen(true)]out string message)
    {
        info = null;
        if(type is null)
        {
            message = "type is null";
            return false;
        }
        if (prefix is null)
        {
            message = "prefix is null";
            return false;
        }
        if(type is long)
        {

        }
        else if(type is string)
        {

        }
        if(prefix is long)
        {

        }
        else if(prefix is string)
        {

        }
        info = new ItemModifyInfo
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
        return true;
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