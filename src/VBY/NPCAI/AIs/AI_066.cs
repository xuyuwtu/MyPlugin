namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_066(this NPC npc)
    {
        if (npc.type == 484)
        {
            float num1020 = Main.rand.Next(90, 111) * 0.01f;
            num1020 *= (Main.essScale + 0.5f) / 2f;
            Lighting.AddLight((int)((npc.position.X + npc.width / 2) / 16f), (int)((npc.position.Y + npc.height / 2) / 16f), 0.3f * num1020, 0.1f * num1020, 0.25f * num1020);
        }
        if (npc.velocity.Y == 0f)
        {
            if (npc.ai[0] == 1f)
            {
                if (npc.direction == 0)
                {
                    npc.TargetClosest();
                }
                if (npc.collideX)
                {
                    npc.direction *= -1;
                }
                float num1021 = 0.2f;
                if (npc.type == 485)
                {
                    num1021 = 0.25f;
                }
                if (npc.type == 486)
                {
                    num1021 = 0.325f;
                }
                if (npc.type == 487)
                {
                    num1021 = 0.4f;
                }
                npc.velocity.X = num1021 * npc.direction;
                if (npc.type == 374)
                {
                    npc.velocity.X *= 3f;
                }
            }
            else
            {
                npc.velocity.X = 0f;
            }
            if (Main.netMode != 1)
            {
                npc.localAI[1] -= 1f;
                if (npc.localAI[1] <= 0f)
                {
                    if (npc.ai[0] == 1f)
                    {
                        npc.ai[0] = 0f;
                        npc.localAI[1] = Main.rand.Next(300, 900);
                    }
                    else
                    {
                        npc.ai[0] = 1f;
                        npc.localAI[1] = Main.rand.Next(600, 1800);
                    }
                    npc.netUpdate = true;
                }
            }
        }
        else if (npc.direction == 0)
        {
            npc.direction = 1;
            if (npc.velocity.X < 0f)
            {
                npc.direction = -1;
            }
        }
        if (npc.type != 374)
        {
            return;
        }
        npc.spriteDirection = npc.direction;
        bool flag52 = false;
        for (int num1022 = 0; num1022 < 255; num1022++)
        {
            Player player5 = Main.player[num1022];
            if (player5.active && !player5.dead && !(Vector2.Distance(player5.Center, npc.Center) > 160f))
            {
                flag52 = true;
                break;
            }
        }
        int num1023 = 90;
        if (flag52 && npc.ai[1] < num1023)
        {
            npc.ai[1]++;
        }
        if (npc.ai[1] == num1023 && Main.netMode != 1)
        {
            npc.position.Y += 16f;
            npc.Transform(375);
            npc.netUpdate = true;
        }
    }
}
