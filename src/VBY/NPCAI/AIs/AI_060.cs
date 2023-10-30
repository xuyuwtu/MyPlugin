namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_060(this NPC npc)
    {
        if (Main.dayTime)
        {
            if (npc.velocity.X > 0f)
            {
                npc.velocity.X += 0.25f;
            }
            else
            {
                npc.velocity.X -= 0.25f;
            }
            npc.velocity.Y -= 0.1f;
            npc.rotation = npc.velocity.X * 0.05f;
        }
        else if (npc.ai[0] == 0f)
        {
            if (npc.ai[2] == 0f)
            {
                npc.TargetClosest();
                if (npc.Center.X < Main.player[npc.target].Center.X)
                {
                    npc.ai[2] = 1f;
                }
                else
                {
                    npc.ai[2] = -1f;
                }
            }
            npc.TargetClosest();
            int num931 = 800;
            float num932 = Math.Abs(npc.Center.X - Main.player[npc.target].Center.X);
            if (npc.Center.X < Main.player[npc.target].Center.X && npc.ai[2] < 0f && num932 > num931)
            {
                npc.ai[2] = 0f;
            }
            if (npc.Center.X > Main.player[npc.target].Center.X && npc.ai[2] > 0f && num932 > num931)
            {
                npc.ai[2] = 0f;
            }
            float num933 = 0.45f;
            float num934 = 7f;
            if (npc.life < npc.lifeMax * 0.75)
            {
                num933 = 0.55f;
                num934 = 8f;
            }
            if (npc.life < npc.lifeMax * 0.5)
            {
                num933 = 0.7f;
                num934 = 10f;
            }
            if (npc.life < npc.lifeMax * 0.25)
            {
                num933 = 0.8f;
                num934 = 11f;
            }
            npc.velocity.X += npc.ai[2] * num933;
            if (npc.velocity.X > num934)
            {
                npc.velocity.X = num934;
            }
            if (npc.velocity.X < 0f - num934)
            {
                npc.velocity.X = 0f - num934;
            }
            float num935 = Main.player[npc.target].position.Y - (npc.position.Y + npc.height);
            if (num935 < 150f)
            {
                npc.velocity.Y -= 0.2f;
            }
            if (num935 > 200f)
            {
                npc.velocity.Y += 0.2f;
            }
            if (npc.velocity.Y > 8f)
            {
                npc.velocity.Y = 8f;
            }
            if (npc.velocity.Y < -8f)
            {
                npc.velocity.Y = -8f;
            }
            npc.rotation = npc.velocity.X * 0.05f;
            if ((num932 < 500f || npc.ai[3] < 0f) && npc.position.Y < Main.player[npc.target].position.Y)
            {
                npc.ai[3] += 1f;
                int num936 = 13;
                if (npc.life < npc.lifeMax * 0.75)
                {
                    num936 = 12;
                }
                if (npc.life < npc.lifeMax * 0.5)
                {
                    num936 = 11;
                }
                if (npc.life < npc.lifeMax * 0.25)
                {
                    num936 = 10;
                }
                num936++;
                if (npc.ai[3] > num936)
                {
                    npc.ai[3] = -num936;
                }
                if (npc.ai[3] == 0f && Main.netMode != 1)
                {
                    Vector2 vector124 = new(npc.Center.X, npc.Center.Y);
                    vector124.X += npc.velocity.X * 7f;
                    float num937 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector124.X;
                    float num938 = Main.player[npc.target].Center.Y - vector124.Y;
                    float num939 = (float)Math.Sqrt(num937 * num937 + num938 * num938);
                    float num940 = 6f;
                    if (npc.life < npc.lifeMax * 0.75)
                    {
                        num940 = 7f;
                    }
                    if (npc.life < npc.lifeMax * 0.5)
                    {
                        num940 = 8f;
                    }
                    if (npc.life < npc.lifeMax * 0.25)
                    {
                        num940 = 9f;
                    }
                    num939 = num940 / num939;
                    num937 *= num939;
                    num938 *= num939;
                    _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector124.X, vector124.Y, num937, num938, 348, 42, 0f, Main.myPlayer);
                }
            }
            else if (npc.ai[3] < 0f)
            {
                npc.ai[3] += 1f;
            }
            if (Main.netMode != 1)
            {
                npc.ai[1] += Main.rand.Next(1, 4);
                if (npc.ai[1] > 800f && num932 < 600f)
                {
                    npc.ai[0] = -1f;
                }
            }
        }
        else if (npc.ai[0] == 1f)
        {
            npc.TargetClosest();
            float num942 = 0.15f;
            float num943 = 7f;
            if (npc.life < npc.lifeMax * 0.75)
            {
                num942 = 0.17f;
                num943 = 8f;
            }
            if (npc.life < npc.lifeMax * 0.5)
            {
                num942 = 0.2f;
                num943 = 9f;
            }
            if (npc.life < npc.lifeMax * 0.25)
            {
                num942 = 0.25f;
                num943 = 10f;
            }
            num942 -= 0.05f;
            num943 -= 1f;
            if (npc.Center.X < Main.player[npc.target].Center.X)
            {
                npc.velocity.X += num942;
                if (npc.velocity.X < 0f)
                {
                    npc.velocity.X *= 0.98f;
                }
            }
            if (npc.Center.X > Main.player[npc.target].Center.X)
            {
                npc.velocity.X -= num942;
                if (npc.velocity.X > 0f)
                {
                    npc.velocity.X *= 0.98f;
                }
            }
            if (npc.velocity.X > num943 || npc.velocity.X < 0f - num943)
            {
                npc.velocity.X *= 0.95f;
            }
            float num944 = Main.player[npc.target].position.Y - (npc.position.Y + npc.height);
            if (num944 < 180f)
            {
                npc.velocity.Y -= 0.1f;
            }
            if (num944 > 200f)
            {
                npc.velocity.Y += 0.1f;
            }
            if (npc.velocity.Y > 6f)
            {
                npc.velocity.Y = 6f;
            }
            if (npc.velocity.Y < -6f)
            {
                npc.velocity.Y = -6f;
            }
            npc.rotation = npc.velocity.X * 0.01f;
            if (Main.netMode != 1)
            {
                npc.ai[3] += 1f;
                int num945 = 15;
                if (npc.life < npc.lifeMax * 0.75)
                {
                    num945 = 14;
                }
                if (npc.life < npc.lifeMax * 0.5)
                {
                    num945 = 12;
                }
                if (npc.life < npc.lifeMax * 0.25)
                {
                    num945 = 10;
                }
                if (npc.life < npc.lifeMax * 0.1)
                {
                    num945 = 8;
                }
                num945 += 3;
                if (npc.ai[3] >= num945)
                {
                    npc.ai[3] = 0f;
                    Vector2 vector125 = new(npc.Center.X, npc.position.Y + npc.height - 14f);
                    int i2 = (int)(vector125.X / 16f);
                    int j2 = (int)(vector125.Y / 16f);
                    if (!WorldGen.SolidTile(i2, j2))
                    {
                        float num946 = npc.velocity.Y;
                        if (num946 < 0f)
                        {
                            num946 = 0f;
                        }
                        num946 += 3f;
                        float speedX2 = npc.velocity.X * 0.25f;
                        _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector125.X, vector125.Y, speedX2, num946, 349, 37, 0f, Main.myPlayer, Main.rand.Next(5));
                    }
                }
            }
            if (Main.netMode != 1)
            {
                npc.ai[1] += Main.rand.Next(1, 4);
                if (npc.ai[1] > 600f)
                {
                    npc.ai[0] = -1f;
                }
            }
        }
        else if (npc.ai[0] == 2f)
        {
            npc.TargetClosest();
            Vector2 vector126 = new(npc.Center.X, npc.Center.Y - 20f);
            float num948 = Main.rand.Next(-1000, 1001);
            float num949 = Main.rand.Next(-1000, 1001);
            float num950 = (float)Math.Sqrt(num948 * num948 + num949 * num949);
            float num951 = 15f;
            npc.velocity *= 0.95f;
            num950 = num951 / num950;
            num948 *= num950;
            num949 *= num950;
            npc.rotation += 0.2f;
            vector126.X += num948 * 4f;
            vector126.Y += num949 * 4f;
            npc.ai[3] += 1f;
            int num952 = 7;
            if (npc.life < npc.lifeMax * 0.75)
            {
                num952--;
            }
            if (npc.life < npc.lifeMax * 0.5)
            {
                num952 -= 2;
            }
            if (npc.life < npc.lifeMax * 0.25)
            {
                num952 -= 3;
            }
            if (npc.life < npc.lifeMax * 0.1)
            {
                num952 -= 4;
            }
            if (npc.ai[3] > num952)
            {
                npc.ai[3] = 0f;
                _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector126.X, vector126.Y, num948, num949, 349, 35, 0f, Main.myPlayer);
            }
            if (Main.netMode != 1)
            {
                npc.ai[1] += Main.rand.Next(1, 4);
                if (npc.ai[1] > 500f)
                {
                    npc.ai[0] = -1f;
                }
            }
        }
        if (npc.ai[0] == -1f)
        {
            int num954 = Main.rand.Next(3);
            npc.TargetClosest();
            if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) > 1000f)
            {
                num954 = 0;
            }
            npc.ai[0] = num954;
            npc.ai[1] = 0f;
            npc.ai[2] = 0f;
            npc.ai[3] = 0f;
        }
    }
}
