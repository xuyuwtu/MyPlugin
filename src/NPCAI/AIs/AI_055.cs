namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_055(this NPC npc)
    {
        if (NPC.crimsonBoss < 0)
        {
            npc.active = false;
            npc.netUpdate = true;
            return;
        }
        if (npc.ai[0] == 0f)
        {
            npc.ai[1] = 0f;
            Vector2 vector106 = new(npc.Center.X, npc.Center.Y);
            float num855 = Main.npc[NPC.crimsonBoss].Center.X - vector106.X;
            float num856 = Main.npc[NPC.crimsonBoss].Center.Y - vector106.Y;
            float num857 = (float)Math.Sqrt(num855 * num855 + num856 * num856);
            if (num857 > 90f)
            {
                num857 = 8f / num857;
                num855 *= num857;
                num856 *= num857;
                npc.velocity.X = (npc.velocity.X * 15f + num855) / 16f;
                npc.velocity.Y = (npc.velocity.Y * 15f + num856) / 16f;
                return;
            }
            if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < 8f)
            {
                npc.velocity.Y *= 1.05f;
                npc.velocity.X *= 1.05f;
            }
            if (Main.netMode != 1 && ((Main.expertMode && Main.rand.Next(100) == 0) || Main.rand.Next(200) == 0))
            {
                npc.TargetClosest();
                vector106 = new Vector2(npc.Center.X, npc.Center.Y);
                num855 = Main.player[npc.target].Center.X - vector106.X;
                num856 = Main.player[npc.target].Center.Y - vector106.Y;
                num857 = (float)Math.Sqrt(num855 * num855 + num856 * num856);
                num857 = 8f / num857;
                npc.velocity.X = num855 * num857;
                npc.velocity.Y = num856 * num857;
                npc.ai[0] = 1f;
                npc.netUpdate = true;
            }
            return;
        }
        if (Main.expertMode)
        {
            Vector2 vector107 = Main.player[npc.target].Center - npc.Center;
            vector107.Normalize();
            if (Main.getGoodWorld)
            {
                vector107 *= 12f;
                npc.velocity = (npc.velocity * 49f + vector107) / 50f;
            }
            else
            {
                vector107 *= 9f;
                npc.velocity = (npc.velocity * 99f + vector107) / 100f;
            }
        }
        Vector2 vector108 = new(npc.Center.X, npc.Center.Y);
        float num858 = Main.npc[NPC.crimsonBoss].Center.X - vector108.X;
        float num859 = Main.npc[NPC.crimsonBoss].Center.Y - vector108.Y;
        float num860 = (float)Math.Sqrt(num858 * num858 + num859 * num859);
        if (num860 > 700f)
        {
            npc.ai[0] = 0f;
        }
        else
        {
            if (!npc.justHit)
            {
                return;
            }
            if (npc.knockBackResist == 0f)
            {
                npc.ai[1] += 1f;
                if (npc.ai[1] > 5f)
                {
                    npc.ai[0] = 0f;
                }
            }
            else
            {
                npc.ai[0] = 0f;
            }
        }
    }
}
