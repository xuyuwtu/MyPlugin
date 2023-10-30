namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_100(this NPC npc)
    {
        if (npc.velocity.Y == 0f && npc.ai[0] >= 0f)
        {
            npc.ai[0] = -1f;
            npc.ai[1] = 0f;
            npc.netUpdate = true;
            return;
        }
        if (npc.ai[0] == -1f)
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
        npc.rotation = npc.velocity.ToRotation() - (float)Math.PI / 2f;
        if (npc.type != 522)
        {
            return;
        }
        if (npc.localAI[0] == 0f)
        {
            npc.localAI[0] = 1f;
            npc.velocity.X = npc.ai[2];
            npc.velocity.Y = npc.ai[3];
            for (int num1511 = 0; num1511 < 13; num1511++)
            {
                int num1512 = Dust.NewDust(npc.position, npc.width, npc.height, 261, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 90, default, 2.5f);
                Main.dust[num1512].noGravity = true;
                Main.dust[num1512].fadeIn = 1f;
                Dust dust = Main.dust[num1512];
                dust.velocity *= 4f;
                Main.dust[num1512].noLight = true;
            }
        }
        for (int num1513 = 0; num1513 < 2; num1513++)
        {
            if (Main.rand.Next(10 - (int)Math.Min(7f, npc.velocity.Length())) < 1)
            {
                int num1514 = Dust.NewDust(npc.position, npc.width, npc.height, 261, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 90, default, 2.5f);
                Main.dust[num1514].noGravity = true;
                Dust dust = Main.dust[num1514];
                dust.velocity *= 0.2f;
                Main.dust[num1514].fadeIn = 0.4f;
                if (Main.rand.Next(6) == 0)
                {
                    dust = Main.dust[num1514];
                    dust.velocity *= 5f;
                    Main.dust[num1514].noLight = true;
                }
                else
                {
                    Main.dust[num1514].velocity = npc.DirectionFrom(Main.dust[num1514].position) * Main.dust[num1514].velocity.Length();
                }
            }
        }
        if (npc.ai[0] >= 0f)
        {
            npc.ai[0]++;
            if (npc.ai[0] > 60f)
            {
                npc.velocity = npc.velocity.RotatedBy(npc.ai[1]);
            }
            if (npc.ai[0] > 120f)
            {
                npc.velocity *= 0.98f;
            }
            if (npc.velocity.Length() < 0.2f)
            {
                npc.velocity = Vector2.Zero;
            }
        }
    }
}
