namespace VBY.ProjectileTest;

public static class Utils
{
    public static Dictionary<int, Func<Projectile, Vector2, int>> NewProjectileFunc = new()
    {
        [919] = static (projectile, velocity) => Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center, Main.rand.Next(2) == 0 ? projectile.velocity.RotatedByDegress(90) : projectile.velocity.RotatedByDegress(-90), 919, projectile.damage, 0, Main.myPlayer, velocity.ToRotation(), projectile.ai[0] / 100)
    };
    public static Vector2 RotatedByDegress(this Vector2 vector2, float degress)
    {
        return vector2.RotatedBy(MathHelper.ToRadians(degress));
    }
    public static Vector2 Normalize(this Vector2 vector2, float num)
    {
        float num1 = num / (float)Math.Sqrt(vector2.X * vector2.X + vector2.Y * vector2.Y);
        return new Vector2(vector2.X * num1, vector2.Y * num1);
    }
}
