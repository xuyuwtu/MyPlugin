namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_073(this NPC npc)
    {
        npc.TargetClosest(faceTarget: false);
        npc.spriteDirection = npc.direction;
        npc.velocity.X *= 0.93f;
        if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
        {
            npc.velocity.X = 0f;
        }
        if (npc.type == 387)
        {
            float num1050 = 120f;
            float num1051 = 60f;
            if (npc.ai[1] < num1050)
            {
                npc.ai[1]++;
                if (npc.ai[1] > 60f)
                {
                    float num1052 = (npc.ai[1] - num1051) / (num1050 - num1051);
                    npc.alpha = (int)((1f - num1052) * 255f);
                }
                else
                {
                    npc.alpha = 255;
                }
                npc.dontTakeDamage = true;
                npc.frameCounter = 0.0;
                npc.frame.Y = 0;
                float num1053 = npc.ai[1] / num1051;
                Vector2 spinningpoint = new(0f, -30f);
                spinningpoint = spinningpoint.RotatedBy(num1053 * 1.5f * ((float)Math.PI * 2f)) * new Vector2(1f, 0.4f);
                for (int num1054 = 0; num1054 < 4; num1054++)
                {
                    Vector2 vector137 = Vector2.Zero;
                    float num1055 = 1f;
                    if (num1054 == 0)
                    {
                        vector137 = Vector2.UnitY * -15f;
                        num1055 = 0.15f;
                    }
                    if (num1054 == 1)
                    {
                        vector137 = Vector2.UnitY * -5f;
                        num1055 = 0.3f;
                    }
                    if (num1054 == 2)
                    {
                        vector137 = Vector2.UnitY * 5f;
                        num1055 = 0.6f;
                    }
                    if (num1054 == 3)
                    {
                        vector137 = Vector2.UnitY * 20f;
                        num1055 = 0.45f;
                    }
                    int num1056 = Dust.NewDust(npc.Center, 0, 0, 226, 0f, 0f, 100, default, 0.5f);
                    Main.dust[num1056].noGravity = true;
                    Main.dust[num1056].position = npc.Center + spinningpoint * num1055 + vector137;
                    Main.dust[num1056].velocity = Vector2.Zero;
                    spinningpoint *= -1f;
                    num1056 = Dust.NewDust(npc.Center, 0, 0, 226, 0f, 0f, 100, default, 0.5f);
                    Main.dust[num1056].noGravity = true;
                    Main.dust[num1056].position = npc.Center + spinningpoint * num1055 + vector137;
                    Main.dust[num1056].velocity = Vector2.Zero;
                }
                Lighting.AddLight((int)npc.Center.X / 16, (int)(npc.Center.Y - 10f) / 16, 0.1f * num1053, 0.5f * num1053, 0.7f * num1053);
                return;
            }
            if (npc.ai[1] == num1050)
            {
                npc.ai[1]++;
                npc.netUpdate = true;
            }
            Lighting.AddLight((int)npc.Center.X / 16, (int)(npc.Center.Y - 10f) / 16, 0.1f, 0.5f, 0.7f);
            npc.dontTakeDamage = false;
        }
        if (npc.ai[0] < 60f)
        {
            npc.ai[0]++;
        }
        if (npc.justHit)
        {
            npc.ai[0] = -30f;
            npc.netUpdate = true;
        }
        if (npc.ai[0] == 60f && Main.netMode != 1)
        {
            npc.ai[0] = -120f;
            npc.netUpdate = true;
            Vector2 center5 = Main.player[npc.target].Center;
            Vector2 vector138 = npc.Center - Vector2.UnitY * 10f;
            Vector2 vector139 = center5 - vector138;
            vector139.X += Main.rand.Next(-100, 101);
            vector139.Y += Main.rand.Next(-100, 101);
            vector139.X *= Main.rand.Next(70, 131) * 0.01f;
            vector139.Y *= Main.rand.Next(70, 131) * 0.01f;
            vector139.Normalize();
            if (float.IsNaN(vector139.X) || float.IsNaN(vector139.Y))
            {
                vector139 = -Vector2.UnitY;
            }
            vector139 *= 14f;
            int projDamage = 35;
            float num1058 = 1f;
            if (npc.type >= 381 && npc.type <= 392)
            {
                num1058 = 0.8f;
            }
            projDamage = npc.GetAttackDamage_ForProjectiles(projDamage, projDamage * num1058);
            Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector138.X, vector138.Y, vector139.X, vector139.Y, 435, projDamage, 0f, Main.myPlayer);
        }
    }
}
