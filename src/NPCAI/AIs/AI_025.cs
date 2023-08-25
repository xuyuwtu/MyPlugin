namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_025(this NPC npc)
    {
        bool flag25 = npc.type == 341 && !Main.snowMoon;
        if (npc.ai[3] == 0f)
        {
            npc.position.X += 8f;
            if (npc.position.Y / 16f > Main.UnderworldLayer)
            {
                npc.ai[3] = 3f;
            }
            else if ((double)(npc.position.Y / 16f) > Main.worldSurface)
            {
                npc.TargetClosest();
                npc.ai[3] = 2f;
            }
            else
            {
                npc.ai[3] = 1f;
            }
        }
        if (npc.type == 341 || npc.type == 629)
        {
            npc.ai[3] = 1f;
        }
        if (npc.ai[0] == 0f)
        {
            if (!flag25)
            {
                npc.TargetClosest();
            }
            if (Main.netMode == 1)
            {
                return;
            }
            if (npc.velocity.X != 0f || npc.velocity.Y < 0f || npc.velocity.Y > 0.3)
            {
                npc.ai[0] = 1f;
                npc.netUpdate = true;
                return;
            }
            Rectangle rectangle3 = new((int)Main.player[npc.target].position.X, (int)Main.player[npc.target].position.Y, Main.player[npc.target].width, Main.player[npc.target].height);
            if (new Rectangle((int)npc.position.X - 100, (int)npc.position.Y - 100, npc.width + 200, npc.height + 200).Intersects(rectangle3) || npc.life < npc.lifeMax)
            {
                npc.ai[0] = 1f;
                npc.netUpdate = true;
            }
        }
        else if (npc.velocity.Y == 0f)
        {
            npc.ai[2] += 1f;
            int num337 = 20;
            if (npc.ai[1] == 0f)
            {
                num337 = 12;
            }
            if (npc.ai[2] < num337)
            {
                npc.velocity.X *= 0.9f;
                return;
            }
            npc.ai[2] = 0f;
            if (!flag25)
            {
                npc.TargetClosest();
            }
            if (npc.direction == 0)
            {
                npc.direction = -1;
            }
            npc.spriteDirection = npc.direction;
            npc.ai[1] += 1f;
            if (npc.ai[1] == 2f)
            {
                npc.velocity.X = npc.direction * 2.5f;
                npc.velocity.Y = -8f;
                npc.ai[1] = 0f;
            }
            else
            {
                npc.velocity.X = npc.direction * 3.5f;
                npc.velocity.Y = -4f;
            }
            npc.netUpdate = true;
        }
        else if (npc.direction == 1 && npc.velocity.X < 1f)
        {
            npc.velocity.X += 0.1f;
        }
        else if (npc.direction == -1 && npc.velocity.X > -1f)
        {
            npc.velocity.X -= 0.1f;
        }
    }
}
