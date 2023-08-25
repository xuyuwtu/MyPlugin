namespace VBY.ProjectileTest;

public static class Utils
{
    public static Dictionary<int, Action<Projectile, Vector2>> NewProjectileAction = new()
    {
        [919] = (projectile, velocity) => Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center, Main.rand.Next(2) == 0 ? projectile.velocity.RotatedByDegress(90) : projectile.velocity.RotatedByDegress(-90), 919, projectile.damage, 0, Main.myPlayer, velocity.ToRotation(), projectile.ai[0] / 100)
    };
    public static Vector2 RotatedByDegress(this Vector2 vector2, float degress)
    {
        return vector2.RotatedBy(MathHelper.ToRadians(degress));
    }
}
