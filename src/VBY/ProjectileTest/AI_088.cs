using Terraria.Utilities;

namespace VBY.ProjectileTest;

public static partial class ProjectileAIs
{
    public static void AI_088(Projectile self)
    {
        if (self.type == 465)
        {
            if (self.localAI[1] == 0f)
            {
                self.localAI[1] = 1f;
            }
            if (self.ai[0] < 180f)
            {
                self.alpha -= 5;
                if (self.alpha < 0)
                {
                    self.alpha = 0;
                }
            }
            else
            {
                self.alpha += 5;
                if (self.alpha > 255)
                {
                    self.alpha = 255;
                    self.Kill();
                    return;
                }
            }
            self.ai[0]++;
            var inv = 30f;
            if (self.ai[2] != 0)
            {
                inv = self.ai[2];
            }
            if (self.ai[0] % inv == 0f && self.ai[0] < 180f)
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
                    if (Vector2.Distance(targetCenter, self.Center) < findDistance && Collision.CanHit(self.Center, 1, 1, targetCenter, 1, 1))
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
                if (self.ai[1] == 0)
                {
                    for (int i = 0; i < targetCount; i++)
                    {
                        Vector2 vector85 = targetCenters[i] - self.Center;
                        float ai = Main.rand.Next(100);
                        Vector2 vector86 = Vector2.Normalize(vector85.RotatedByRandom(0.7853981852531433)) * 7f;
                        Projectile.NewProjectile(self.GetProjectileSource_FromThis(), self.Center.X, self.Center.Y, vector86.X, vector86.Y, 466, self.damage, 0f, Main.myPlayer, vector85.ToRotation(), ai);
                    }
                }
                else
                {
                    for (int i = 0; i < targetCount; i++)
                    {
                        Vector2 velocity = targetCenters[i] - self.Center;
                        if(Utils.NewProjectileFunc.TryGetValue((int)self.ai[1], out var action))
                        {
                            action(self, velocity);
                        }
                        else
                        {
                            Projectile.NewProjectile(self.GetProjectileSource_FromThis(), self.Center.X, self.Center.Y, velocity.X, velocity.Y, (int)self.ai[1], self.damage, 0f);
                        }
                    }
                }
            }
            if (++self.frameCounter >= 4)
            {
                self.frameCounter = 0;
                if (++self.frame >= Main.projFrames[self.type])
                {
                    self.frame = 0;
                }
            }
            if (self.alpha >= 150 || !(self.ai[0] < 180f))
            {
                return;
            }
        }
        else if (self.type == 466)
        {
            self.frameCounter++;
            Lighting.AddLight(self.Center, 0.3f, 0.45f, 0.5f);
            if (self.velocity == Vector2.Zero)
            {
                if (self.frameCounter >= self.extraUpdates * 2)
                {
                    self.frameCounter = 0;
                    bool flag35 = true;
                    for (int num760 = 1; num760 < self.oldPos.Length; num760++)
                    {
                        if (self.oldPos[num760] != self.oldPos[0])
                        {
                            flag35 = false;
                        }
                    }
                    if (flag35)
                    {
                        self.Kill();
                        return;
                    }
                }
            }
            else
            {
                if (self.frameCounter < self.extraUpdates * 2)
                {
                    return;
                }
                self.frameCounter = 0;
                float num766 = self.velocity.Length();
                UnifiedRandom unifiedRandom = new((int)self.ai[1]);
                int num767 = 0;
                Vector2 spinningpoint15 = -Vector2.UnitY;
                while (true)
                {
                    int num768 = unifiedRandom.Next();
                    self.ai[1] = num768;
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
                    if (vector91.X * (float)(self.extraUpdates + 1) * 2f * num766 + self.localAI[0] > 40f)
                    {
                        flag36 = true;
                    }
                    if (vector91.X * (float)(self.extraUpdates + 1) * 2f * num766 + self.localAI[0] < -40f)
                    {
                        flag36 = true;
                    }
                    if (flag36)
                    {
                        if (num767++ >= 100)
                        {
                            self.velocity = Vector2.Zero;
                            self.localAI[1] = 1f;
                            break;
                        }
                        continue;
                    }
                    spinningpoint15 = vector91;
                    break;
                }
                if (self.velocity != Vector2.Zero)
                {
                    self.localAI[0] += spinningpoint15.X * (float)(self.extraUpdates + 1) * 2f * num766;
                    self.velocity = spinningpoint15.RotatedBy(self.ai[0] + (float)Math.PI / 2f) * num766;
                    self.rotation = self.velocity.ToRotation() + (float)Math.PI / 2f;
                }
            }
        }
        else
        {
            if (self.type != 580)
            {
                return;
            }
            if (self.localAI[1] == 0f && self.ai[0] >= 900f)
            {
                self.ai[0] -= 1000f;
                self.localAI[1] = -1f;
            }
            self.frameCounter++;
            Lighting.AddLight(self.Center, 0.3f, 0.45f, 0.5f);
            if (self.velocity == Vector2.Zero)
            {
                if (self.frameCounter >= self.extraUpdates * 2)
                {
                    self.frameCounter = 0;
                    bool flag37 = true;
                    for (int num769 = 1; num769 < self.oldPos.Length; num769++)
                    {
                        if (self.oldPos[num769] != self.oldPos[0])
                        {
                            flag37 = false;
                        }
                    }
                    if (flag37)
                    {
                        self.Kill();
                        return;
                    }
                }
            }
            else
            {
                if (self.frameCounter < self.extraUpdates * 2)
                {
                    return;
                }
                self.frameCounter = 0;
                float num775 = self.velocity.Length();
                UnifiedRandom unifiedRandom2 = new((int)self.ai[1]);
                int num776 = 0;
                Vector2 spinningpoint16 = -Vector2.UnitY;
                while (true)
                {
                    int num777 = unifiedRandom2.Next();
                    self.ai[1] = num777;
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
                    if (vector94.X * (float)(self.extraUpdates + 1) * 2f * num775 + self.localAI[0] > 40f)
                    {
                        flag38 = true;
                    }
                    if (vector94.X * (float)(self.extraUpdates + 1) * 2f * num775 + self.localAI[0] < -40f)
                    {
                        flag38 = true;
                    }
                    if (flag38)
                    {
                        if (num776++ >= 100)
                        {
                            self.velocity = Vector2.Zero;
                            if (self.localAI[1] < 1f)
                            {
                                self.localAI[1] += 2f;
                            }
                            break;
                        }
                        continue;
                    }
                    spinningpoint16 = vector94;
                    break;
                }
                if (!(self.velocity != Vector2.Zero))
                {
                    return;
                }
                self.localAI[0] += spinningpoint16.X * (float)(self.extraUpdates + 1) * 2f * num775;
                self.velocity = spinningpoint16.RotatedBy(self.ai[0] + (float)Math.PI / 2f) * num775;
                self.rotation = self.velocity.ToRotation() + (float)Math.PI / 2f;
                if (Main.rand.Next(4) == 0 && Main.netMode != 1 && self.localAI[1] == 0f)
                {
                    float num778 = (float)Main.rand.Next(-3, 4) * ((float)Math.PI / 3f) / 3f;
                    Vector2 vector95 = self.ai[0].ToRotationVector2().RotatedBy(num778) * self.velocity.Length();
                    if (!Collision.CanHitLine(self.Center, 0, 0, self.Center + vector95 * 50f, 0, 0))
                    {
                        Projectile.NewProjectile(self.GetProjectileSource_FromThis(), self.Center.X - vector95.X, self.Center.Y - vector95.Y, vector95.X, vector95.Y, self.type, self.damage, self.knockBack, self.owner, vector95.ToRotation() + 1000f, self.ai[1]);
                    }
                }
            }
        }
    }
}
