namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_095(this NPC npc)
    {
        float num1458 = 300f;
        if (npc.velocity.Length() > 4f)
        {
            npc.velocity *= 0.95f;
        }
        npc.velocity *= 0.99f;
        npc.ai[0]++;
        float num1459 = MathHelper.Clamp(npc.ai[0] / num1458, 0f, 1f);
        npc.scale = 1f + 0.3f * num1459;
        if (npc.ai[0] >= num1458)
        {
            if (Main.netMode != 1)
            {
                npc.Transform(405);
                npc.netUpdate = true;
            }
            return;
        }
        npc.rotation += npc.velocity.X * 0.1f;
        if (!(npc.ai[0] > 20f))
        {
            return;
        }
        Vector2 center39 = npc.Center;
        int num1460 = (int)(npc.ai[0] / (num1458 / 2f));
        for (int num1461 = 0; num1461 < num1460 + 1; num1461++)
        {
            if (Main.rand.Next(2) != 0)
            {
                int num1462 = 226;
                float num1463 = 0.4f;
                if (num1461 % 2 == 1)
                {
                    num1462 = 226;
                    num1463 = 0.65f;
                }
                Vector2 vector287 = center39 + ((float)Main.rand.NextDouble() * ((float)Math.PI * 2f)).ToRotationVector2() * (12f - num1460 * 2);
                int num1464 = Dust.NewDust(vector287 - Vector2.One * 12f, 24, 24, num1462, npc.velocity.X / 2f, npc.velocity.Y / 2f);
                Dust dust = Main.dust[num1464];
                dust.position -= new Vector2(2f);
                Main.dust[num1464].velocity = Vector2.Normalize(center39 - vector287) * 1.5f * (10f - num1460 * 2f) / 10f;
                Main.dust[num1464].noGravity = true;
                Main.dust[num1464].scale = num1463;
                Main.dust[num1464].customData = npc;
            }
        }
    }
}
