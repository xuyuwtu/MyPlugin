namespace VBY.NPCTest;

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
    public static Vector2 GetTargetCenter(this NPC npc) => Main.player[npc.target].Center;
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
    public static int NewProjectile(this NPC npc, Vector2 position, Vector2 velocity, int Type, int Damage)
    {
        return Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), position, velocity, Type, Damage, 0);
    }
    public static int NewProjectile(this NPC npc, Vector2 position, Vector2 velocity, int Type, int Damage, float ai0 = 0, float ai1 = 0, float ai2 = 0)
    {
        return Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), position, velocity, Type, Damage, 0, Main.myPlayer, ai0, ai1, ai2);
    }
    public static Vector2 RotatedByDegress(this Vector2 vector2, float degress)
    {
        return vector2.RotatedBy(MathHelper.ToRadians(degress));
    }
    public static void NewThreeProjectile(this NPC npc, Vector2 velocity, float degress, int Type, int Damage)
    {
        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center, velocity, Type, Damage, 0);
        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center, velocity.RotatedBy(MathHelper.ToRadians(degress)), Type, Damage, 0);
        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center, velocity.RotatedBy(MathHelper.ToRadians(-degress)), Type, Damage, 0);
    }
    public static int GetMultiplierDamage(this NPC npc, int damage)
    {
        float result = 0;
        if(npc.strengthMultiplier > Main.GameModeInfo.EnemyDamageMultiplier)
        {
            result = (int)(damage * (1 + npc.strengthMultiplier / Main.GameModeInfo.EnemyDamageMultiplier));
        }
        if(result > int.MaxValue)
        {
            result = int.MaxValue;
        }
        return (int)result;
    }
}
public static class DeBuffRecord
{
    /// <summary><inheritdoc cref="ProjectileID.DD2OgreSpit"/> 分泌物</summary>
    public const short DD2OgreSpit = ProjectileID.DD2OgreSpit;

    /// <summary><inheritdoc cref="ProjectileID.ShadowBeamHostile"/> 缓慢</summary>
    public const short ShadowBeamHostile = ProjectileID.ShadowBeamHostile;

    /// <summary><inheritdoc cref="ProjectileID.BrainScramblerBolt"/> 困惑</summary>
    public const short BrainScramblerBolt = ProjectileID.BrainScramblerBolt;

    /// <summary><inheritdoc cref="ProjectileID.DD2LightningBugZap"/> 枯萎武器</summary>
    public const short DD2LightningBugZap = ProjectileID.DD2LightningBugZap;

    /// <summary><inheritdoc cref="ProjectileID.GoldenShowerHostile"/> 灵液</summary>
    public const short GoldenShowerHostile = ProjectileID.GoldenShowerHostile;

    /// <summary><inheritdoc cref="ProjectileID.FrostBlastHostile"/> 霜冻</summary>
    public const short FrostBlastHostile = ProjectileID.FrostBlastHostile;

    /// <summary><inheritdoc cref="ProjectileID.VortexAcid"/> 扭曲</summary>
    public const short VortexAcid = ProjectileID.VortexAcid;

    /// <summary><inheritdoc cref="ProjectileID.MartianTurretBolt"/> 带电</summary>
    public const short MartianTurretBolt = ProjectileID.MartianTurretBolt;

    /// <summary><inheritdoc cref="ProjectileID.GigaZapperSpear"/> 带电</summary>
    public const short GigaZapperSpear = ProjectileID.GigaZapperSpear;

    /// <summary><inheritdoc cref="NPCID.VileSpitEaterOfWorlds"/> 虚弱</summary>
    public const short VileSpitEaterOfWorlds = NPCID.VileSpitEaterOfWorlds;

}
