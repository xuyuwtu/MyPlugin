namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_061(this NPC npc)
    {
        float num955 = 2f;
        npc.noGravity = true;
        npc.noTileCollide = true;
        if (!Main.dayTime)
        {
            npc.TargetClosest();
        }
        bool flag47 = false;
        if (npc.life < npc.lifeMax * 0.75)
        {
            num955 = 3f;
        }
        if (npc.life < npc.lifeMax * 0.5)
        {
            num955 = 4f;
        }
        if (npc.life < npc.lifeMax * 0.25)
        {
            num955 = 5f;
        }
        Vector2 center4 = npc.Center;
        Point point5 = center4.ToTileCoordinates();
        if (WorldGen.InWorld(point5.X, point5.Y) && !WorldGen.SolidTile(point5.X, point5.Y))
        {
            Lighting.AddLight(center4, 0.3f, 0.26f, 0.05f);
        }
        if (Main.dayTime)
        {
            npc.EncourageDespawn(10);
            num955 = 8f;
            if (npc.velocity.X == 0f)
            {
                npc.velocity.X = 0.1f;
            }
        }
        else if (npc.ai[0] == 0f)
        {
            npc.ai[1] += 1f;
            if (npc.ai[1] >= 300f && Main.netMode != 1)
            {
                npc.TargetClosest();
                npc.ai[1] = 0f;
                npc.ai[0] = 1f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 1f)
        {
            npc.ai[1] += 1f;
            flag47 = true;
            int num956 = 16;
            if (npc.life < npc.lifeMax * 0.25)
            {
                num956 = 8;
            }
            else if (npc.life < npc.lifeMax * 0.5)
            {
                num956 = 11;
            }
            else if (npc.life < npc.lifeMax * 0.75)
            {
                num956 = 14;
            }
            if (npc.ai[1] % num956 == 0f)
            {
                Vector2 vector127 = new(npc.Center.X + npc.direction * 50, npc.Center.Y + Main.rand.Next(15, 36));
                float num957 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector127.X;
                float num958 = Main.player[npc.target].Center.Y - vector127.Y;
                num957 += Main.rand.Next(-40, 41);
                num958 += Main.rand.Next(-40, 41);
                float num959 = (float)Math.Sqrt(num957 * num957 + num958 * num958);
                float num960 = 15f;
                num959 = num960 / num959;
                num957 *= num959;
                num958 *= num959;
                num957 *= 1f + Main.rand.Next(-20, 21) * 0.015f;
                num958 *= 1f + Main.rand.Next(-20, 21) * 0.015f;
                _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector127.X, vector127.Y, num957, num958, 180, 36, 0f, Main.myPlayer);
            }
            if (npc.ai[1] > 240f)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
            }
        }
        if (Main.netMode != 1)
        {
            int num962 = 600;
            int num963 = 1200;
            int num964 = 2700;
            if (npc.life < npc.lifeMax * 0.25)
            {
                num962 = (int)(num962 * 0.5);
                num963 = (int)(num963 * 0.5);
                num964 = (int)(num964 * 0.5);
            }
            else if (npc.life < npc.lifeMax * 0.5)
            {
                num962 = (int)(num962 * 0.75);
                num963 = (int)(num963 * 0.75);
                num964 = (int)(num964 * 0.75);
            }
            else if (npc.life < npc.lifeMax * 0.75)
            {
                num962 = (int)(num962 * 0.9);
                num963 = (int)(num963 * 0.9);
                num964 = (int)(num964 * 0.9);
            }
            if (Main.rand.Next(num962) == 0)
            {
                Vector2 vector128 = new(npc.Center.X - npc.direction * 24, npc.Center.Y - 64f);
                float num965 = Main.rand.Next(1, 100) * npc.direction;
                float num966 = 1f;
                float num967 = (float)Math.Sqrt(num965 * num965 + num966 * num966);
                float num968 = 1f;
                num967 = num968 / num967;
                num965 *= num967;
                num966 *= num967;
                _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector128.X, vector128.Y, num965, num966, 352, 80, 0f, Main.myPlayer);
            }
            if (Main.rand.Next(num963) == 0)
            {
                npc.localAI[1] = 1f;
            }
            if (npc.localAI[1] >= 1f)
            {
                npc.localAI[1] += 1f;
                int num970 = 12;
                if (npc.localAI[1] % num970 == 0f)
                {
                    Vector2 vector129 = new(npc.Center.X - npc.direction * 24, npc.Center.Y - 64f);
                    float num971 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector129.X;
                    float num972 = Main.player[npc.target].Center.Y - vector129.Y;
                    num971 += Main.rand.Next(-50, 51);
                    num972 += Main.rand.Next(-50, 51);
                    float num973 = (float)Math.Sqrt(num971 * num971 + num972 * num972);
                    float num974 = 12.5f;
                    num973 = num974 / num973;
                    num971 *= num973;
                    num972 *= num973;
                    num971 *= 1f + Main.rand.Next(-20, 21) * 0.015f;
                    num972 *= 1f + Main.rand.Next(-20, 21) * 0.015f;
                    _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector129.X, vector129.Y, num971, num972, 350, 42, 0f, Main.myPlayer);
                }
                if (npc.localAI[1] >= 100f)
                {
                    npc.localAI[1] = 0f;
                }
            }
            if (Main.rand.Next(num964) == 0)
            {
                npc.localAI[2] = 2f;
            }
            if (npc.localAI[2] > 0f)
            {
                npc.localAI[2] += 1f;
                int num976 = 9;
                if (npc.localAI[2] % num976 == 0f)
                {
                    Vector2 vector130 = new(npc.Center.X - npc.direction * 24, npc.Center.Y - 64f);
                    float num977 = Main.rand.Next(-100, 101);
                    float num978 = -300f;
                    float num979 = (float)Math.Sqrt(num977 * num977 + num978 * num978);
                    float num980 = 11f;
                    num979 = num980 / num979;
                    num977 *= num979;
                    num978 *= num979;
                    num977 *= 1f + Main.rand.Next(-20, 21) * 0.01f;
                    num978 *= 1f + Main.rand.Next(-20, 21) * 0.01f;
                    _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector130.X, vector130.Y, num977, num978, 351, 50, 0f, Main.myPlayer);
                }
                if (npc.localAI[2] >= 100f)
                {
                    npc.localAI[2] = 0f;
                }
            }
        }
        if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) < 50f)
        {
            flag47 = true;
        }
        if (flag47)
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
                npc.velocity.X = (npc.velocity.X * 20f + num955) / 21f;
            }
            if (npc.direction < 0)
            {
                npc.velocity.X = (npc.velocity.X * 20f - num955) / 21f;
            }
        }
        int num982 = 80;
        int num983 = 20;
        Vector2 vector131 = new(npc.Center.X - num982 / 2, npc.position.Y + npc.height - num983);
        bool flag48 = false;
        if (npc.position.X < Main.player[npc.target].position.X && npc.position.X + npc.width > Main.player[npc.target].position.X + Main.player[npc.target].width && npc.position.Y + npc.height < Main.player[npc.target].position.Y + Main.player[npc.target].height - 16f)
        {
            flag48 = true;
        }
        if (flag48)
        {
            npc.velocity.Y += 0.5f;
        }
        else if (Collision.SolidCollision(vector131, num982, num983))
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
