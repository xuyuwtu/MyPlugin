using Terraria.Utilities;

namespace VBY.ProjectileTest;

public static partial class ProjectileAIs
{
    public static void AI_088(Projectile projectile)
    {
        if (projectile.type == 465)
        {
            if (projectile.localAI[1] == 0f)
            {
                projectile.localAI[1] = 1f;
            }
            if (projectile.ai[0] < 180f)
            {
                projectile.alpha -= 5;
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }
            }
            else
            {
                projectile.alpha += 5;
                if (projectile.alpha > 255)
                {
                    projectile.alpha = 255;
                    projectile.Kill();
                    return;
                }
            }
            projectile.ai[0]++;
            var inv = 30f;
            if (projectile.ai[2] != 0)
            {
                inv = projectile.ai[2];
            }
            if (projectile.ai[0] % inv == 0f && projectile.ai[0] < 180f)
            {
                int[] targetIDs = new int[5];
                Vector2[] targetCenters = new Vector2[5];
                int targetCount = 0;
                float findDistance = 2000f;
                for (int i = 0; i < 255; i++)
                {
                    if (!Main.player[i].active || Main.player[i].dead)
                    {
                        continue;
                    }
                    Vector2 targetCenter = Main.player[i].Center;
                    if (Vector2.Distance(targetCenter, projectile.Center) < findDistance && Collision.CanHit(projectile.Center, 1, 1, targetCenter, 1, 1))
                    {
                        targetIDs[targetCount] = i;
                        targetCenters[targetCount] = targetCenter;
                        targetCount++;
                        if (targetCount >= targetCenters.Length)
                        {
                            break;
                        }
                    }
                }
                if (projectile.ai[1] == 0)
                {
                    for (int i = 0; i < targetCount; i++)
                    {
                        Vector2 vector85 = targetCenters[i] - projectile.Center;
                        float ai = Main.rand.Next(100);
                        Vector2 vector86 = Vector2.Normalize(vector85.RotatedByRandom(0.7853981852531433)) * 7f;
                        Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center.X, projectile.Center.Y, vector86.X, vector86.Y, 466, projectile.damage, 0f, Main.myPlayer, vector85.ToRotation(), ai);
                    }
                }
                else
                {
                    for (int i = 0; i < targetCount; i++)
                    {
                        Vector2 velocity = targetCenters[i] - projectile.Center;
                        if(Utils.NewProjectileAction.TryGetValue((int)projectile.ai[1], out var action))
                        {
                            action(projectile, velocity);
                        }
                        else
                        {
                            Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center.X, projectile.Center.Y, velocity.X, velocity.Y, (int)projectile.ai[1], projectile.damage, 0f);
                        }
                    }
                }
            }
            if (++projectile.frameCounter >= 4)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
            if (projectile.alpha >= 150 || !(projectile.ai[0] < 180f))
            {
                return;
            }
        }
        else if (projectile.type == 466)
        {
            projectile.frameCounter++;
            Lighting.AddLight(projectile.Center, 0.3f, 0.45f, 0.5f);
            if (projectile.velocity == Vector2.Zero)
            {
                if (projectile.frameCounter >= projectile.extraUpdates * 2)
                {
                    projectile.frameCounter = 0;
                    bool flag35 = true;
                    for (int num760 = 1; num760 < projectile.oldPos.Length; num760++)
                    {
                        if (projectile.oldPos[num760] != projectile.oldPos[0])
                        {
                            flag35 = false;
                        }
                    }
                    if (flag35)
                    {
                        projectile.Kill();
                        return;
                    }
                }
            }
            else
            {
                if (projectile.frameCounter < projectile.extraUpdates * 2)
                {
                    return;
                }
                projectile.frameCounter = 0;
                float num766 = projectile.velocity.Length();
                UnifiedRandom unifiedRandom = new((int)projectile.ai[1]);
                int num767 = 0;
                Vector2 spinningpoint15 = -Vector2.UnitY;
                while (true)
                {
                    int num768 = unifiedRandom.Next();
                    projectile.ai[1] = num768;
                    num768 %= 100;
                    float f = (float)num768 / 100f * ((float)Math.PI * 2f);
                    Vector2 vector91 = f.ToRotationVector2();
                    if (vector91.Y > 0f)
                    {
                        vector91.Y *= -1f;
                    }
                    bool flag36 = false;
                    if (vector91.Y > -0.02f)
                    {
                        flag36 = true;
                    }
                    if (vector91.X * (float)(projectile.extraUpdates + 1) * 2f * num766 + projectile.localAI[0] > 40f)
                    {
                        flag36 = true;
                    }
                    if (vector91.X * (float)(projectile.extraUpdates + 1) * 2f * num766 + projectile.localAI[0] < -40f)
                    {
                        flag36 = true;
                    }
                    if (flag36)
                    {
                        if (num767++ >= 100)
                        {
                            projectile.velocity = Vector2.Zero;
                            projectile.localAI[1] = 1f;
                            break;
                        }
                        continue;
                    }
                    spinningpoint15 = vector91;
                    break;
                }
                if (projectile.velocity != Vector2.Zero)
                {
                    projectile.localAI[0] += spinningpoint15.X * (float)(projectile.extraUpdates + 1) * 2f * num766;
                    projectile.velocity = spinningpoint15.RotatedBy(projectile.ai[0] + (float)Math.PI / 2f) * num766;
                    projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                }
            }
        }
        else
        {
            if (projectile.type != 580)
            {
                return;
            }
            if (projectile.localAI[1] == 0f && projectile.ai[0] >= 900f)
            {
                projectile.ai[0] -= 1000f;
                projectile.localAI[1] = -1f;
            }
            projectile.frameCounter++;
            Lighting.AddLight(projectile.Center, 0.3f, 0.45f, 0.5f);
            if (projectile.velocity == Vector2.Zero)
            {
                if (projectile.frameCounter >= projectile.extraUpdates * 2)
                {
                    projectile.frameCounter = 0;
                    bool flag37 = true;
                    for (int num769 = 1; num769 < projectile.oldPos.Length; num769++)
                    {
                        if (projectile.oldPos[num769] != projectile.oldPos[0])
                        {
                            flag37 = false;
                        }
                    }
                    if (flag37)
                    {
                        projectile.Kill();
                        return;
                    }
                }
            }
            else
            {
                if (projectile.frameCounter < projectile.extraUpdates * 2)
                {
                    return;
                }
                projectile.frameCounter = 0;
                float num775 = projectile.velocity.Length();
                UnifiedRandom unifiedRandom2 = new((int)projectile.ai[1]);
                int num776 = 0;
                Vector2 spinningpoint16 = -Vector2.UnitY;
                while (true)
                {
                    int num777 = unifiedRandom2.Next();
                    projectile.ai[1] = num777;
                    num777 %= 100;
                    float f2 = (float)num777 / 100f * ((float)Math.PI * 2f);
                    Vector2 vector94 = f2.ToRotationVector2();
                    if (vector94.Y > 0f)
                    {
                        vector94.Y *= -1f;
                    }
                    bool flag38 = false;
                    if (vector94.Y > -0.02f)
                    {
                        flag38 = true;
                    }
                    if (vector94.X * (float)(projectile.extraUpdates + 1) * 2f * num775 + projectile.localAI[0] > 40f)
                    {
                        flag38 = true;
                    }
                    if (vector94.X * (float)(projectile.extraUpdates + 1) * 2f * num775 + projectile.localAI[0] < -40f)
                    {
                        flag38 = true;
                    }
                    if (flag38)
                    {
                        if (num776++ >= 100)
                        {
                            projectile.velocity = Vector2.Zero;
                            if (projectile.localAI[1] < 1f)
                            {
                                projectile.localAI[1] += 2f;
                            }
                            break;
                        }
                        continue;
                    }
                    spinningpoint16 = vector94;
                    break;
                }
                if (!(projectile.velocity != Vector2.Zero))
                {
                    return;
                }
                projectile.localAI[0] += spinningpoint16.X * (float)(projectile.extraUpdates + 1) * 2f * num775;
                projectile.velocity = spinningpoint16.RotatedBy(projectile.ai[0] + (float)Math.PI / 2f) * num775;
                projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                if (Main.rand.Next(4) == 0 && Main.netMode != 1 && projectile.localAI[1] == 0f)
                {
                    float num778 = (float)Main.rand.Next(-3, 4) * ((float)Math.PI / 3f) / 3f;
                    Vector2 vector95 = projectile.ai[0].ToRotationVector2().RotatedBy(num778) * projectile.velocity.Length();
                    if (!Collision.CanHitLine(projectile.Center, 0, 0, projectile.Center + vector95 * 50f, 0, 0))
                    {
                        Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center.X - vector95.X, projectile.Center.Y - vector95.Y, vector95.X, vector95.Y, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, vector95.ToRotation() + 1000f, projectile.ai[1]);
                    }
                }
            }
        }
    }
}
