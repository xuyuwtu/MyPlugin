namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_059(this NPC npc)
    {
        npc.spriteDirection = -(int)npc.ai[0];
        if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].aiStyle != 58)
        {
            npc.ai[2] += 10f;
            if (npc.ai[2] > 50f || Main.netMode != 2)
            {
                npc.life = -1;
                npc.HitEffect();
                npc.active = false;
            }
        }
        if (Main.netMode != 1 && Main.npc[(int)npc.ai[1]].ai[3] == 2f)
        {
            npc.localAI[1] += 1f;
            if (npc.localAI[1] > 90f)
            {
                npc.localAI[1] = 0f;
                float num913 = 0.01f;
                Vector2 vector120 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f + 30f);
                float num914 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector120.X;
                float num915 = Main.player[npc.target].position.Y - vector120.Y;
                float num916 = (float)Math.Sqrt(num914 * num914 + num915 * num915);
                num916 = num913 / num916;
                num914 *= num916;
                num915 *= num916;
                _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X, npc.Center.Y, num914, num915, 329, 60, 0f, Main.myPlayer, npc.rotation, npc.spriteDirection);
            }
        }
        if (Main.dayTime)
        {
            npc.velocity.Y += 0.3f;
            npc.velocity.X *= 0.9f;
        }
        else if (npc.ai[2] == 0f || npc.ai[2] == 3f)
        {
            if (Main.npc[(int)npc.ai[1]].ai[1] == 3f)
            {
                npc.EncourageDespawn(10);
            }
            npc.ai[3] += 1f;
            if (npc.ai[3] >= 180f)
            {
                npc.ai[2] += 1f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
            Vector2 vector121 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num918 = (Main.player[npc.target].Center.X + Main.npc[(int)npc.ai[1]].Center.X) / 2f;
            float num919 = (Main.player[npc.target].Center.Y + Main.npc[(int)npc.ai[1]].Center.Y) / 2f;
            num918 += -170f * npc.ai[0] - vector121.X;
            num919 += 90f - vector121.Y;
            float num920 = Math.Abs(Main.player[npc.target].Center.X - Main.npc[(int)npc.ai[1]].Center.X) + Math.Abs(Main.player[npc.target].Center.Y - Main.npc[(int)npc.ai[1]].Center.Y);
            if (num920 > 700f)
            {
                num918 = Main.npc[(int)npc.ai[1]].Center.X - 170f * npc.ai[0] - vector121.X;
                num919 = Main.npc[(int)npc.ai[1]].Center.Y + 90f - vector121.Y;
            }
            float num921 = (float)Math.Sqrt(num918 * num918 + num919 * num919);
            float num922 = 6f;
            if (num921 > 1000f)
            {
                num922 = 21f;
            }
            else if (num921 > 800f)
            {
                num922 = 18f;
            }
            else if (num921 > 600f)
            {
                num922 = 15f;
            }
            else if (num921 > 400f)
            {
                num922 = 12f;
            }
            else if (num921 > 200f)
            {
                num922 = 9f;
            }
            if (npc.ai[0] < 0f && npc.Center.X > Main.npc[(int)npc.ai[1]].Center.X)
            {
                num918 -= 4f;
            }
            if (npc.ai[0] > 0f && npc.Center.X < Main.npc[(int)npc.ai[1]].Center.X)
            {
                num918 += 4f;
            }
            num921 = num922 / num921;
            npc.velocity.X = (npc.velocity.X * 14f + num918 * num921) / 15f;
            npc.velocity.Y = (npc.velocity.Y * 14f + num919 * num921) / 15f;
            num921 = (float)Math.Sqrt(num918 * num918 + num919 * num919);
            if (num921 > 20f)
            {
                npc.rotation = (float)Math.Atan2(num919, num918) + 1.57f;
            }
        }
        else if (npc.ai[2] == 1f)
        {
            Vector2 vector122 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num923 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 200f * npc.ai[0] - vector122.X;
            float num924 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector122.Y;
            _ = (float)Math.Sqrt(num923 * num923 + num924 * num924);
            npc.rotation = (float)Math.Atan2(num924, num923) + 1.57f;
            npc.velocity.X *= 0.95f;
            npc.velocity.Y -= 0.3f;
            if (npc.velocity.Y < -14f)
            {
                npc.velocity.Y = -14f;
            }
            if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 200f)
            {
                npc.TargetClosest();
                npc.ai[2] = 2f;
                vector122 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                num923 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector122.X;
                num924 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector122.Y;
                float num925 = (float)Math.Sqrt(num923 * num923 + num924 * num924);
                num925 = 18f / num925;
                npc.velocity.X = num923 * num925;
                npc.velocity.Y = num924 * num925;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[2] == 2f)
        {
            float num926 = Math.Abs(npc.Center.X - Main.npc[(int)npc.ai[1]].Center.X) + Math.Abs(npc.Center.Y - Main.npc[(int)npc.ai[1]].Center.Y);
            if (npc.position.Y > Main.player[npc.target].position.Y || npc.velocity.Y < 0f || num926 > 800f)
            {
                npc.ai[2] = 3f;
            }
        }
        else if (npc.ai[2] == 4f)
        {
            Vector2 vector123 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num927 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 200f * npc.ai[0] - vector123.X;
            float num928 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector123.Y;
            _ = (float)Math.Sqrt(num927 * num927 + num928 * num928);
            npc.rotation = (float)Math.Atan2(num928, num927) + 1.57f;
            npc.velocity.Y *= 0.95f;
            npc.velocity.X += 0.3f * (0f - npc.ai[0]);
            if (npc.velocity.X < -14f)
            {
                npc.velocity.X = -14f;
            }
            if (npc.velocity.X > 14f)
            {
                npc.velocity.X = 14f;
            }
            if (npc.position.X + npc.width / 2 < Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 500f || npc.position.X + npc.width / 2 > Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 + 500f)
            {
                npc.TargetClosest();
                npc.ai[2] = 5f;
                vector123 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                num927 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector123.X;
                num928 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector123.Y;
                float num929 = (float)Math.Sqrt(num927 * num927 + num928 * num928);
                num929 = 17f / num929;
                npc.velocity.X = num927 * num929;
                npc.velocity.Y = num928 * num929;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[2] == 5f)
        {
            float num930 = Math.Abs(npc.Center.X - Main.npc[(int)npc.ai[1]].Center.X) + Math.Abs(npc.Center.Y - Main.npc[(int)npc.ai[1]].Center.Y);
            if ((npc.velocity.X > 0f && npc.position.X + npc.width / 2 > Main.player[npc.target].position.X + Main.player[npc.target].width / 2) || (npc.velocity.X < 0f && npc.position.X + npc.width / 2 < Main.player[npc.target].position.X + Main.player[npc.target].width / 2) || num930 > 800f)
            {
                npc.ai[2] = 0f;
            }
        }
    }
}
