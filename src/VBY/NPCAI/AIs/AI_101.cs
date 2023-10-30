namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_101(this NPC npc)
    {
        float num1515 = 420f;
        float num1516 = 120f;
        int num1517 = 1;
        float value11 = 0f;
        float value12 = 1f;
        float num1518 = 4f;
        bool flag97 = !(npc.ai[1] >= 0f) || !Main.npc[(int)npc.ai[0]].active;
        if (Main.npc[(int)npc.ai[0]].type == 439)
        {
            if (Main.npc[(int)npc.ai[0]].life < Main.npc[(int)npc.ai[0]].lifeMax / 2)
            {
                num1517 = 2;
            }
            if (Main.npc[(int)npc.ai[0]].life < Main.npc[(int)npc.ai[0]].lifeMax / 4)
            {
                num1517 = 3;
            }
        }
        else
        {
            flag97 = true;
        }
        npc.ai[1] += num1517;
        float num1519 = npc.ai[1] / num1516;
        num1519 = MathHelper.Clamp(num1519, 0f, 1f);
        npc.position = npc.Center;
        npc.scale = MathHelper.Lerp(value11, value12, num1519);
        npc.Center = npc.position;
        npc.alpha = (int)(255f - num1519 * 255f);
        if (Main.rand.Next(6) == 0)
        {
            Vector2 vector297 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
            Dust dust21 = Main.dust[Dust.NewDust(npc.Center - vector297 * 20f, 0, 0, 27)];
            dust21.noGravity = true;
            dust21.position = npc.Center - vector297 * Main.rand.Next(10, 21) * npc.scale;
            dust21.velocity = vector297.RotatedBy(1.5707963705062866) * 4f;
            dust21.scale = 0.5f + Main.rand.NextFloat();
            dust21.fadeIn = 0.5f;
        }
        if (Main.rand.Next(6) == 0)
        {
            Vector2 vector298 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
            Dust dust22 = Main.dust[Dust.NewDust(npc.Center - vector298 * 30f, 0, 0, 240)];
            dust22.noGravity = true;
            dust22.position = npc.Center - vector298 * 20f * npc.scale;
            dust22.velocity = vector298.RotatedBy(-1.5707963705062866) * 2f;
            dust22.scale = 0.5f + Main.rand.NextFloat();
            dust22.fadeIn = 0.5f;
        }
        if (Main.rand.Next(6) == 0)
        {
            Vector2 vector299 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
            Dust dust23 = Main.dust[Dust.NewDust(npc.Center - vector299 * 30f, 0, 0, 240)];
            dust23.position = npc.Center - vector299 * 20f * npc.scale;
            dust23.velocity = Vector2.Zero;
            dust23.scale = 0.5f + Main.rand.NextFloat();
            dust23.fadeIn = 0.5f;
            dust23.noLight = true;
        }
        npc.localAI[0] += (float)Math.PI / 60f;
        npc.localAI[1] = 0.25f + Vector2.UnitY.RotatedBy(npc.ai[1] * ((float)Math.PI * 2f) / 60f).Y * 0.25f;
        if (npc.ai[1] >= num1515)
        {
            flag97 = true;
            if (Main.netMode != 1)
            {
                for (int num1520 = 0; num1520 < 4; num1520++)
                {
                    Vector2 vector300 = new Vector2(0f, 0f - num1518).RotatedBy((float)Math.PI / 2f * num1520);
                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X, npc.Center.Y, vector300.X, vector300.Y, 593, npc.damage, 0f, Main.myPlayer);
                }
            }
        }
        if (flag97)
        {
            npc.HitEffect(0, 9999.0);
            npc.active = false;
        }
    }
}
