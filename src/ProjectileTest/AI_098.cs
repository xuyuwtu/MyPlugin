namespace VBY.ProjectileTest;

public static partial class ProjectileAIs
{
    public static void AI_098(Projectile projectile)
    {
        Vector2 destination = new(projectile.ai[0], projectile.ai[1]);
        Vector2 value12 = destination - projectile.Center;
        if (value12.Length() < projectile.velocity.Length())
        {
            if (projectile.localAI[2] != 0f)
            {
                var velocity = new Vector2(projectile.localAI[0], projectile.localAI[1]);
                if (projectile.ai[2] != -1f)
                {
                    var targetPlayer = Main.player[(int)projectile.ai[2]];
                    if (targetPlayer.active) 
                    {
                        velocity = (targetPlayer.Center - projectile.Center).Normalize(5f);
                    }
                }
                if (projectile.localAI[2] == 919f) 
                {
                    Main.projectile[Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center, Vector2.Zero, (int)projectile.localAI[2], projectile.damage, 0, -1, velocity.ToRotation(), (float)Main.rand.Next(100) / 100)].timeLeft = 600;
                }
                else
                {
                    Main.projectile[Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center, velocity, (int)projectile.localAI[2], projectile.damage, 0)].timeLeft = 600;
                }
            }
            projectile.Kill();
            return;
        }
        value12.Normalize();
        value12 *= 15f;
        projectile.velocity = Vector2.Lerp(projectile.velocity, value12, 0.1f);
        //for (int num818 = 0; num818 < 2; num818++)
        //{
        //    int num819 = Dust.NewDust(projectile.Center, 0, 0, 228, 0f, 0f, 100);
        //    Main.dust[num819].noGravity = true;
        //    Dust dust2 = Main.dust[num819];
        //    dust2.position += new Vector2(4f);
        //    dust2 = Main.dust[num819];
        //    dust2.scale += Main.rand.NextFloat() * 1f;
        //}
    }
}
