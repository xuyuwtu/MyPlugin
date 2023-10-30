namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_089(this NPC npc)
    {
        if (npc.velocity.Y == 0f)
        {
            npc.velocity.X *= 0.9f;
            npc.rotation += npc.velocity.X * 0.02f;
        }
        else
        {
            npc.velocity.X *= 0.99f;
            npc.rotation += npc.velocity.X * 0.04f;
        }
        int num1401 = 900;
        if (Main.expertMode)
        {
            num1401 = 600;
        }
        if (npc.justHit)
        {
            npc.ai[0] -= Main.rand.Next(10, 21);
            if (!Main.expertMode)
            {
                npc.ai[0] -= Main.rand.Next(10, 21);
            }
        }
        npc.ai[0] += 1f;
        if (npc.ai[0] >= num1401)
        {
            npc.Transform(479);
        }
        if (Main.netMode != 1 && npc.velocity.Y == 0f && (double)Math.Abs(npc.velocity.X) < 0.2 && npc.ai[0] >= num1401 * 0.75)
        {
            float num1402 = npc.ai[0] - num1401 * 0.75f;
            num1402 /= num1401 * 0.25f;
            if (Main.rand.Next(-10, 120) < num1402 * 100f)
            {
                npc.velocity.Y -= Main.rand.Next(20, 40) * 0.025f;
                npc.velocity.X += Main.rand.Next(-20, 20) * 0.025f;
                npc.velocity *= 1f + num1402 * 2f;
                npc.netUpdate = true;
            }
        }
    }
}
