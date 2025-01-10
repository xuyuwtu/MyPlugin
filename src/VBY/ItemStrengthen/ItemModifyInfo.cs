using Microsoft.Xna.Framework;

using Terraria;

namespace VBY.ItemStrengthen;

public class ItemModifyInfo
{
    public int progress;
    public int priority;
    public int type;
    private int _prefix;
    public int prefix 
    { 
        get => _prefix;
        set
        {
            if(value == _prefix)
            {
                return;
            }
            if(value < -1)
            {
                value = -1;
            }
            _prefix = value;
            PrefixChanged?.Invoke(_prefix);
        }
    }
    public ModifyInfo? scale, width, height, damage, useTime, knockBack, shootSpeed, useAnimation;
    public int? ammo;
    public int? shoot;
    public Color? color;
    public int? useAmmo;
    public bool? notAmmo;
    public event Action<int>? PrefixChanged;
    public void ResetStats(int type = 0)
    {
        progress = 0;
        priority = 0;
        this.type = type;
        prefix = -1;
        scale = null;
        width = null;
        height = null;
        damage = null;
        useTime = null;
        knockBack = null;
        shootSpeed = null;
        useAnimation = null;
        ammo = null;
        shoot = null;
        color = null;
        useAmmo = null;
        notAmmo = null;
    }
    public (byte flag1, byte flag2) Apply(Item item)
    {
        byte flag1 = 0;
        byte flag2 = 0;
        width?.Apply(ref item.width, ref flag2, 1);
        height?.Apply(ref item.height, ref flag2, 2);
        scale?.Apply(ref item.scale, ref flag2, 4);
        ammo?.Apply(ref item.ammo, ref flag2, 8);
        useAmmo?.Apply(ref item.useAmmo, ref flag2, 16);
        notAmmo?.Apply(ref item.notAmmo, ref flag2, 32);
        color?.Apply(ref item.color, ref flag1, 1);
        damage?.Apply(ref item.damage, ref flag1, 2);
        knockBack?.Apply(ref item.knockBack, ref flag1, 4);
        useAnimation?.Apply(ref item.useAnimation, ref flag1, 8);
        useTime?.Apply(ref item.useTime, ref flag1, 16);
        shoot?.Apply(ref item.shoot, ref flag1, 32);
        shootSpeed?.Apply(ref item.shootSpeed, ref flag1, 64);
        if (flag2 != 0)
        {
            flag1 += 128;
        }
        return (flag1, flag2);
    }
}