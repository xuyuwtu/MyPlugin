namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_070(this NPC npc)
    {
        if (npc.target == 255)
        {
            npc.TargetClosest();
            npc.ai[3] = Main.rand.Next(80, 121) / 100f;
            float num1042 = Main.rand.Next(165, 265) / 15f;
            npc.velocity = Vector2.Normalize(Main.player[npc.target].Center - npc.Center + new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101))) * num1042;
            npc.netUpdate = true;
        }
        Vector2 vector135 = Vector2.Normalize(Main.player[npc.target].Center - npc.Center);
        npc.velocity = (npc.velocity * 40f + vector135 * 20f) / 41f;
        npc.scale = npc.ai[3];
        npc.alpha -= 30;
        if (npc.alpha < 50)
        {
            npc.alpha = 50;
        }
        npc.alpha = 50;
        npc.velocity.X = (npc.velocity.X * 50f + Main.windSpeedCurrent * 2f + Main.rand.Next(-10, 11) * 0.1f) / 51f;
        npc.velocity.Y = (npc.velocity.Y * 50f + -0.25f + Main.rand.Next(-10, 11) * 0.2f) / 51f;
        if (npc.velocity.Y > 0f)
        {
            npc.velocity.Y -= 0.04f;
        }
        if (npc.ai[0] == 0f)
        {
            int num1043 = 40;
            Rectangle rect = npc.getRect();
            rect.X -= num1043 + npc.width / 2;
            rect.Y -= num1043 + npc.height / 2;
            rect.Width += num1043 * 2;
            rect.Height += num1043 * 2;
            for (int num1044 = 0; num1044 < 255; num1044++)
            {
                Player player6 = Main.player[num1044];
                if (player6.active && !player6.dead && rect.Intersects(player6.getRect()))
                {
                    npc.ai[0] = 1f;
                    npc.ai[1] = 4f;
                    npc.netUpdate = true;
                    break;
                }
            }
        }
        if (npc.ai[0] == 0f)
        {
            npc.ai[1]++;
            if (npc.ai[1] >= 150f)
            {
                npc.ai[0] = 1f;
                npc.ai[1] = 4f;
            }
        }
        if (npc.ai[0] == 1f)
        {
            npc.ai[1]--;
            if (npc.ai[1] <= 0f)
            {
                npc.life = 0;
                npc.HitEffect();
                npc.active = false;
                return;
            }
        }
        if (npc.justHit || npc.ai[0] == 1f)
        {
            npc.dontTakeDamage = true;
            npc.position = npc.Center;
            npc.width = (npc.height = 100);
            npc.position = new Vector2(npc.position.X - npc.width / 2, npc.position.Y - npc.height / 2);
            npc.EncourageDespawn(3);
        }
    }
}
