namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_042(this NPC npc)
    {
        npc.TargetClosest();
        if (npc.ai[0] == 0f)
        {
            if (npc.target >= 0)
            {
                Vector2 vector81 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float num629 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector81.X;
                float num630 = Main.player[npc.target].position.Y - vector81.Y;
                float num631 = (float)Math.Sqrt(num629 * num629 + num630 * num630);
                if (num631 < 200f && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    npc.ai[0] = 1f;
                }
            }
            if (npc.velocity.X != 0f || npc.velocity.Y < 0f || npc.velocity.Y > 2f || npc.life != npc.lifeMax)
            {
                npc.ai[0] = 1f;
            }
        }
        else
        {
            npc.ai[0] += 1f;
            if (npc.ai[0] >= 21f)
            {
                npc.ai[0] = 21f;
                npc.Transform(196);
            }
        }
    }
}
