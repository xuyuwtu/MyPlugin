namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_063(this NPC npc)
    {
        npc.TargetClosest();
        float num992 = 11f;
        Vector2 vector133 = new(npc.Center.X + npc.direction * 20, npc.Center.Y + 6f);
        float num993 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector133.X;
        float num994 = Main.player[npc.target].Center.Y - vector133.Y;
        float num995 = (float)Math.Sqrt(num993 * num993 + num994 * num994);
        float num996 = num992 / num995;
        num993 *= num996;
        num994 *= num996;
        if (Main.dayTime)
        {
            num993 = 0f - num993;
            num994 = 0f - num994;
        }
        npc.ai[0] -= 1f;
        if (num995 < 200f || npc.ai[0] > 0f)
        {
            if (num995 < 200f)
            {
                npc.ai[0] = 20f;
            }
            if (npc.velocity.X < 0f)
            {
                npc.direction = -1;
            }
            else
            {
                npc.direction = 1;
            }
            npc.rotation += npc.direction * 0.3f;
            return;
        }
        npc.velocity.X = (npc.velocity.X * 50f + num993) / 51f;
        npc.velocity.Y = (npc.velocity.Y * 50f + num994) / 51f;
        if (num995 < 350f)
        {
            npc.velocity.X = (npc.velocity.X * 10f + num993) / 11f;
            npc.velocity.Y = (npc.velocity.Y * 10f + num994) / 11f;
        }
        if (num995 < 300f)
        {
            npc.velocity.X = (npc.velocity.X * 7f + num993) / 8f;
            npc.velocity.Y = (npc.velocity.Y * 7f + num994) / 8f;
        }
        npc.rotation = npc.velocity.X * 0.15f;
    }
}
