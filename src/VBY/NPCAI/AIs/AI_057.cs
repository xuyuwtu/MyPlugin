namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_057(this NPC npc)
    {
        float num866 = 2f;
        npc.noGravity = true;
        npc.noTileCollide = true;
        if (!Main.dayTime)
        {
            npc.TargetClosest();
        }
        bool flag45 = false;
        if (npc.life < npc.lifeMax * 0.75)
        {
            num866 = 3f;
        }
        if (npc.life < npc.lifeMax * 0.5)
        {
            num866 = 4f;
        }
        if (npc.type == 344)
        {
            Lighting.AddLight(npc.Bottom + new Vector2(0f, -30f), 0.3f, 0.16f, 0.125f);
        }
        if (npc.type == 325)
        {
            Lighting.AddLight(npc.Bottom + new Vector2(0f, -30f), 0.3f, 0.125f, 0.06f);
        }
        if (Main.dayTime)
        {
            npc.EncourageDespawn(10);
            num866 = 8f;
        }
        else if (npc.ai[0] == 0f)
        {
            npc.ai[1] += 1f;
            if (npc.life < npc.lifeMax * 0.5)
            {
                npc.ai[1] += 1f;
            }
            if (npc.life < npc.lifeMax * 0.25)
            {
                npc.ai[1] += 1f;
            }
            if (npc.ai[1] >= 300f && Main.netMode != 1)
            {
                npc.ai[1] = 0f;
                if (npc.life < npc.lifeMax * 0.25 && npc.type != 344)
                {
                    npc.ai[0] = Main.rand.Next(3, 5);
                }
                else
                {
                    npc.ai[0] = Main.rand.Next(1, 3);
                }
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 1f)
        {
            if (npc.type == 344)
            {
                if (Main.rand.Next(5) == 0)
                {
                    int num867 = Dust.NewDust(npc.position + Main.rand.NextVector2Square(0f, 1f) * npc.Size - new Vector2(1f, 2f), 10, 14, 245, 0f, 0f, 254, Color.Transparent, 0.25f);
                    Dust dust = Main.dust[num867];
                    dust.velocity *= 0.2f;
                }
                flag45 = true;
                npc.ai[1] += 1f;
                if (npc.ai[1] % 5f == 0f)
                {
                    Vector2 vector110 = new(npc.position.X + 20f + Main.rand.Next(npc.width - 40), npc.position.Y + 20f + Main.rand.Next(npc.height - 40));
                    float num868 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector110.X;
                    float num869 = Main.player[npc.target].position.Y - vector110.Y;
                    num868 += Main.rand.Next(-50, 51);
                    num869 += Main.rand.Next(-50, 51);
                    num869 -= Math.Abs(num868) * (Main.rand.Next(0, 21) * 0.01f);
                    float num870 = (float)Math.Sqrt(num868 * num868 + num869 * num869);
                    float num871 = 12.5f;
                    num870 = num871 / num870;
                    num868 *= num870;
                    num869 *= num870;
                    num868 *= 1f + Main.rand.Next(-20, 21) * 0.02f;
                    num869 *= 1f + Main.rand.Next(-20, 21) * 0.02f;
                    _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector110.X, vector110.Y, num868, num869, 345, 43, 0f, Main.myPlayer, Main.rand.Next(0, 31));
                }
                if (npc.ai[1] >= 180f)
                {
                    npc.ai[1] = 0f;
                    npc.ai[0] = 0f;
                }
            }
            else
            {
                flag45 = true;
                npc.ai[1] += 1f;
                if (npc.ai[1] % 15f == 0f)
                {
                    Vector2 vector111 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f + 30f);
                    float num873 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector111.X;
                    float num874 = Main.player[npc.target].position.Y - vector111.Y;
                    float num875 = (float)Math.Sqrt(num873 * num873 + num874 * num874);
                    float num876 = 10f;
                    num875 = num876 / num875;
                    num873 *= num875;
                    num874 *= num875;
                    num873 *= 1f + Main.rand.Next(-20, 21) * 0.01f;
                    num874 *= 1f + Main.rand.Next(-20, 21) * 0.01f;
                    _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector111.X, vector111.Y, num873, num874, 325, 50, 0f, Main.myPlayer);
                }
                if (npc.ai[1] >= 120f)
                {
                    npc.ai[1] = 0f;
                    npc.ai[0] = 0f;
                }
            }
        }
        else if (npc.ai[0] == 2f)
        {
            if (npc.type == 344)
            {
                flag45 = true;
                npc.ai[1] += 1f;
                if (npc.ai[1] > 60f && npc.ai[1] < 240f && npc.ai[1] % 15f == 0f)
                {
                    float num878 = 4.5f;
                    Vector2 vector112 = new(npc.position.X + 20f + Main.rand.Next(npc.width - 40), npc.position.Y + 60f + Main.rand.Next(npc.height - 80));
                    float num879 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector112.X;
                    float num880 = Main.player[npc.target].position.Y - vector112.Y;
                    num880 -= Math.Abs(num879) * 0.3f;
                    num878 += Math.Abs(num879) * 0.004f;
                    num879 += Main.rand.Next(-50, 51);
                    num880 -= Main.rand.Next(50, 201);
                    float num881 = (float)Math.Sqrt(num879 * num879 + num880 * num880);
                    num881 = num878 / num881;
                    num879 *= num881;
                    num880 *= num881;
                    num879 *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                    num880 *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                    _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector112.X, vector112.Y, num879, num880, 346, 57, 0f, Main.myPlayer, 0f, Main.rand.Next(2));
                }
                if (npc.ai[1] >= 300f)
                {
                    npc.ai[1] = 0f;
                    npc.ai[0] = 0f;
                }
            }
            else
            {
                flag45 = true;
                npc.ai[1] += 1f;
                if (npc.ai[1] > 60f && npc.ai[1] < 240f && npc.ai[1] % 8f == 0f)
                {
                    float num883 = 10f;
                    Vector2 vector113 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f + 30f);
                    float num884 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector113.X;
                    float num885 = Main.player[npc.target].position.Y - vector113.Y;
                    num885 -= Math.Abs(num884) * 0.3f;
                    num883 += Math.Abs(num884) * 0.004f;
                    if (num883 > 14f)
                    {
                        num883 = 14f;
                    }
                    num884 += Main.rand.Next(-50, 51);
                    num885 -= Main.rand.Next(50, 201);
                    float num886 = (float)Math.Sqrt(num884 * num884 + num885 * num885);
                    num886 = num883 / num886;
                    num884 *= num886;
                    num885 *= num886;
                    num884 *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                    num885 *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                    _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector113.X, vector113.Y, num884, num885, Main.rand.Next(326, 329), 40, 0f, Main.myPlayer);
                }
                if (npc.ai[1] >= 300f)
                {
                    npc.ai[1] = 0f;
                    npc.ai[0] = 0f;
                }
            }
        }
        else if (npc.ai[0] == 3f)
        {
            num866 = 4f;
            npc.ai[1] += 1f;
            if (npc.ai[1] % 30f == 0f)
            {
                Vector2 vector114 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f + 30f);
                float num888 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector114.X;
                float num889 = Main.player[npc.target].position.Y - vector114.Y;
                float num890 = (float)Math.Sqrt(num888 * num888 + num889 * num889);
                float num891 = 16f;
                num890 = num891 / num890;
                num888 *= num890;
                num889 *= num890;
                num888 *= 1f + Main.rand.Next(-20, 21) * 0.001f;
                num889 *= 1f + Main.rand.Next(-20, 21) * 0.001f;
                _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector114.X, vector114.Y, num888, num889, 325, 75, 0f, Main.myPlayer);
            }
            if (npc.ai[1] >= 120f)
            {
                npc.ai[1] = 0f;
                npc.ai[0] = 0f;
            }
        }
        else if (npc.ai[0] == 4f)
        {
            num866 = 4f;
            npc.ai[1] += 1f;
            if (npc.ai[1] % 10f == 0f)
            {
                float num893 = 12f;
                Vector2 vector115 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f + 30f);
                float num894 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector115.X;
                float num895 = Main.player[npc.target].position.Y - vector115.Y;
                num895 -= Math.Abs(num894) * 0.2f;
                num893 += Math.Abs(num894) * 0.002f;
                if (num893 > 16f)
                {
                    num893 = 16f;
                }
                num894 += Main.rand.Next(-50, 51);
                num895 -= Main.rand.Next(50, 201);
                float num896 = (float)Math.Sqrt(num894 * num894 + num895 * num895);
                num896 = num893 / num896;
                num894 *= num896;
                num895 *= num896;
                num894 *= 1f + Main.rand.Next(-30, 31) * 0.005f;
                num895 *= 1f + Main.rand.Next(-30, 31) * 0.005f;
                _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector115.X, vector115.Y, num894, num895, Main.rand.Next(326, 329), 50, 0f, Main.myPlayer);
            }
            if (npc.ai[1] >= 240f)
            {
                npc.ai[1] = 0f;
                npc.ai[0] = 0f;
            }
        }
        if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) < 50f)
        {
            flag45 = true;
        }
        if (flag45)
        {
            npc.velocity.X *= 0.9f;
            if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
            {
                npc.velocity.X = 0f;
            }
        }
        else
        {
            if (npc.direction > 0)
            {
                npc.velocity.X = (npc.velocity.X * 20f + num866) / 21f;
            }
            if (npc.direction < 0)
            {
                npc.velocity.X = (npc.velocity.X * 20f - num866) / 21f;
            }
        }
        int num898 = 80;
        int num899 = 20;
        Vector2 vector116 = new(npc.Center.X - num898 / 2, npc.position.Y + npc.height - num899);
        bool flag46 = false;
        if (npc.position.X < Main.player[npc.target].position.X && npc.position.X + npc.width > Main.player[npc.target].position.X + Main.player[npc.target].width && npc.position.Y + npc.height < Main.player[npc.target].position.Y + Main.player[npc.target].height - 16f)
        {
            flag46 = true;
        }
        if (flag46)
        {
            npc.velocity.Y += 0.5f;
        }
        else if (Collision.SolidCollision(vector116, num898, num899))
        {
            if (npc.velocity.Y > 0f)
            {
                npc.velocity.Y = 0f;
            }
            if (npc.velocity.Y > -0.2)
            {
                npc.velocity.Y -= 0.025f;
            }
            else
            {
                npc.velocity.Y -= 0.2f;
            }
            if (npc.velocity.Y < -4f)
            {
                npc.velocity.Y = -4f;
            }
        }
        else
        {
            if (npc.velocity.Y < 0f)
            {
                npc.velocity.Y = 0f;
            }
            if (npc.velocity.Y < 0.1)
            {
                npc.velocity.Y += 0.025f;
            }
            else
            {
                npc.velocity.Y += 0.5f;
            }
        }
        if (npc.velocity.Y > 10f)
        {
            npc.velocity.Y = 10f;
        }
    }
}
