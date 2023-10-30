namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_099(this NPC npc)
    {
        if (npc.velocity.Y == 0f && npc.ai[0] == 0f)
        {
            npc.ai[0] = 1f;
            npc.ai[1] = 0f;
            npc.netUpdate = true;
            return;
        }
        if (npc.ai[0] == 1f)
        {
            npc.velocity = Vector2.Zero;
            npc.position = npc.oldPosition;
            npc.ai[1]++;
            if (npc.ai[1] >= 5f)
            {
                npc.HitEffect(0, 9999.0);
                npc.active = false;
            }
            return;
        }
        npc.velocity.Y += 0.2f;
        if (npc.velocity.Y > 12f)
        {
            npc.velocity.Y = 12f;
        }
        npc.rotation = npc.velocity.ToRotation() - (float)Math.PI / 2f;
        if (npc.type != 519)
        {
            return;
        }
        if (npc.localAI[0] == 0f)
        {
            npc.localAI[0] = 1f;
            for (int num1507 = 0; num1507 < 13; num1507++)
            {
                int num1508 = Dust.NewDust(npc.position, npc.width, npc.height, 6, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 90, default, 2.5f);
                Main.dust[num1508].noGravity = true;
                Main.dust[num1508].fadeIn = 1f;
                Dust dust = Main.dust[num1508];
                dust.velocity *= 4f;
                Main.dust[num1508].noLight = true;
            }
        }
        for (int num1509 = 0; num1509 < 3; num1509++)
        {
            if (Main.rand.Next(3) < 2)
            {
                int num1510 = Dust.NewDust(npc.position, npc.width, npc.height, 6, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 90, default, 2.5f);
                Main.dust[num1510].noGravity = true;
                Dust dust = Main.dust[num1510];
                dust.velocity *= 0.2f;
                Main.dust[num1510].fadeIn = 1f;
                if (Main.rand.Next(6) == 0)
                {
                    dust = Main.dust[num1510];
                    dust.velocity *= 30f;
                    Main.dust[num1510].noGravity = false;
                    Main.dust[num1510].noLight = true;
                }
                else
                {
                    Main.dust[num1510].velocity = npc.DirectionFrom(Main.dust[num1510].position) * Main.dust[num1510].velocity.Length();
                }
            }
        }
    }
}
