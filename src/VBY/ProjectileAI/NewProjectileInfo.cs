using System.Runtime.InteropServices;

namespace VBY.ProjectileAI;

[StructLayout(LayoutKind.Explicit)]
public struct NewProjectileInfo
{
    [FieldOffset(0)]
    public int Type;
    [FieldOffset(4)]
    public int Damage;
    [FieldOffset(8)]
    public float KnockBack;
    [FieldOffset(12)]
    public float SpeedX;
    [FieldOffset(16)]
    public float SpeedY;
    [FieldOffset(20)]
    public float AI0;
    [FieldOffset(24)]
    public float AI1;
    [FieldOffset(28)]
    public float AI2;
}