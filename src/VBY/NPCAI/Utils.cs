namespace VBY.NPCAI;

public static class TerrariaExtension
{
    public static Vector2 Normalize(this Vector2 vector2, float num)
    {
        float num1 = num / (float)Math.Sqrt(vector2.X * vector2.X + vector2.Y * vector2.Y);
        return new Vector2(vector2.X * num1, vector2.Y * num1);
    }
    public static void AIOutput(this NPC npc)
    {
        Console.WriteLine(string.Join(",", npc.ai));
    }
    public static Player GetTargetPlayer(this NPC npc) => Main.player[npc.target];
    public static Vector2 GetToTargetVector2(this NPC npc) => Main.player[npc.target].Center - npc.Center;
    public static int NewProjectile(this NPC npc, int Type)
    {
        return Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center, default, Type, npc.damage, 0);
    }
    public static int NewProjectile(this NPC npc, Vector2 velocity, int Type)
    {
        return Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center, velocity, Type, npc.damage, 0);
    }
    public static int NewProjectile(this NPC npc, Vector2 velocity, int Type, int Damage)
    {
        return Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center, velocity, Type, Damage, 0);
    }
    public static int NewProjectile(this NPC npc,Vector2 position, Vector2 velocity, int Type, int Damage)
    {
        return Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), position, velocity, Type, Damage, 0);
    }
    public static int NewProjectile(this NPC npc, Vector2 position, Vector2 velocity, int Type, int Damage, float ai0 = 0, float ai1 = 0)
    {
        return Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), position, velocity, Type, Damage, 0, -1, ai0, ai1);
    }
    public static Vector2 RotatedByDegress(this Vector2 vector2, float degress)
    {
        return vector2.RotatedBy(MathHelper.ToRadians(degress));
    }
    public static void NewThreeProjectile(this NPC npc, Vector2 velocity, float degress, int Type,int Damage)
    {
        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center, velocity, Type, Damage, 0);
        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center, velocity.RotatedBy(MathHelper.ToRadians(degress)), Type, Damage, 0);
        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center, velocity.RotatedBy(MathHelper.ToRadians(-degress)), Type, Damage, 0);
    }
}
