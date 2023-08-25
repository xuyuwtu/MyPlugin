namespace FlowerSeaRPG;
[Flags]
public enum TweakItemFlags
{
    None = 0,
    Color = 1,
    Damage = 2,
    KnockBack = 4,
    UseAnimation = 8,
    UseTime = 16,
    Shoot = 32, 
    ShootSpeed = 64,
    NextFlags = 128
}
[Flags]
public enum TweakItemFlags2
{
    None = 0,   
    Width = 1,
    Height = 2,
    Scale = 4,
    Ammo = 8,
    UseAmmo = 16,
    NotAmmo = 32
}