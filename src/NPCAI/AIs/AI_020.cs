namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_020(this NPC npc)
    {
        if (npc.ai[0] == 0f)
        {
            if (Main.netMode != 1)
            {
                npc.TargetClosest();
                npc.direction *= -1;
                npc.directionY *= -1;
                npc.position.Y += npc.height / 2 + 8;
                npc.ai[1] = npc.position.X + npc.width / 2;
                npc.ai[2] = npc.position.Y + npc.height / 2;
                if (npc.direction == 0)
                {
                    npc.direction = 1;
                }
                if (npc.directionY == 0)
                {
                    npc.directionY = 1;
                }
                npc.ai[3] = 1f + Main.rand.Next(15) * 0.1f;
                npc.velocity.Y = npc.directionY * 6 * npc.ai[3];
                npc.ai[0] += 1f;
                npc.netUpdate = true;
            }
            else
            {
                npc.ai[1] = npc.position.X + npc.width / 2;
                npc.ai[2] = npc.position.Y + npc.height / 2;
            }
            return;
        }
        float num284 = 6f * npc.ai[3];
        float num285 = 0.2f * npc.ai[3];
        float num286 = num284 / num285 / 2f;
        if (npc.ai[0] >= 1f && npc.ai[0] < (int)num286)
        {
            npc.velocity.Y = npc.directionY * num284;
            npc.ai[0] += 1f;
            return;
        }
        if (npc.ai[0] >= (int)num286)
        {
            npc.velocity.Y = 0f;
            npc.directionY *= -1;
            npc.velocity.X = num284 * npc.direction;
            npc.ai[0] = -1f;
            return;
        }
        if (npc.directionY > 0)
        {
            if (npc.velocity.Y >= num284)
            {
                npc.directionY *= -1;
                npc.velocity.Y = num284;
            }
        }
        else if (npc.directionY < 0 && npc.velocity.Y <= 0f - num284)
        {
            npc.directionY *= -1;
            npc.velocity.Y = 0f - num284;
        }
        if (npc.direction > 0)
        {
            if (npc.velocity.X >= num284)
            {
                npc.direction *= -1;
                npc.velocity.X = num284;
            }
        }
        else if (npc.direction < 0 && npc.velocity.X <= 0f - num284)
        {
            npc.direction *= -1;
            npc.velocity.X = 0f - num284;
        }
        npc.velocity.X += num285 * npc.direction;
        npc.velocity.Y += num285 * npc.directionY;
    }
}
