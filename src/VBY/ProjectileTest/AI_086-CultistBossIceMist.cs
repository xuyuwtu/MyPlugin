namespace VBY.ProjectileTest;

partial class ProjectileAIs
{
    //ai[2] projectileType
    //localai[0] interval
    //localai[1] speedMultiper
    public static void AI_086(Projectile projectile)
    {
        if (projectile.localAI[1] == 0f)
        {
            projectile.localAI[1] = 1f;
            SoundEngine.PlaySound(SoundID.Item120, projectile.position);
        }
        projectile.ai[0]++;
        if (projectile.ai[1] == 1f)
        {
            if (projectile.ai[0] >= 130f)
            {
                projectile.alpha += 10;
            }
            else
            {
                projectile.alpha -= 10;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            if (projectile.alpha > 255)
            {
                projectile.alpha = 255;
            }
            if (projectile.ai[0] >= 150f)
            {
                projectile.Kill();
                return;
            }
            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[0] = 30f;
            }
            //if (projectile.ai[0] % 30f == 0f && Main.netMode != 1)
            if (projectile.ai[0] % projectile.localAI[0] == 0)
            {
                Vector2 vector82 = projectile.rotation.ToRotationVector2();
                vector82 *= projectile.localAI[1] == 0 ? 1f : projectile.localAI[1];
                var projectileType = projectile.ai[2] == 0 ? 464 : (int)projectile.ai[2];
                Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center.X, projectile.Center.Y, vector82.X, vector82.Y, projectileType, projectile.damage, projectile.knockBack, projectile.owner);
            }
            projectile.rotation += (float)Math.PI / 30f;
            Lighting.AddLight(projectile.Center, 0.3f, 0.75f, 0.9f);
            return;
        }
        projectile.position -= projectile.velocity;
        if (projectile.ai[0] >= 40f)
        {
            projectile.alpha += 3;
        }
        else
        {
            projectile.alpha -= 40;
        }
        if (projectile.alpha < 0)
        {
            projectile.alpha = 0;
        }
        if (projectile.alpha > 255)
        {
            projectile.alpha = 255;
        }
        if (projectile.ai[0] >= 45f)
        {
            projectile.Kill();
            return;
        }
    }
}
