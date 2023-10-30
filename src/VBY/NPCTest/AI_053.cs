namespace VBY.NPCTest;

partial class NPCAIs
{
    public static void AI_053(this NPC npc)
    {
        if (Main.getGoodWorld)
        {
            if (Main.rand.Next(10) == 0)
            {
                npc.reflectsProjectiles = true;
            }
            else
            {
                npc.reflectsProjectiles = false;
            }
        }
        if (NPC.plantBoss < 0)
        {
            npc.StrikeNPCNoInteraction(9999, 0f, 0);
            npc.netUpdate = true;
            return;
        }
        int num815 = NPC.plantBoss;
        if (npc.ai[3] > 0f)
        {
            num815 = (int)npc.ai[3] - 1;
        }
        if (Main.netMode != 1)
        {
            npc.localAI[0] -= 1f;
            if (npc.localAI[0] <= 0f)
            {
                npc.localAI[0] = Main.rand.Next(120, 480);
                npc.ai[0] = Main.rand.Next(-100, 101);
                npc.ai[1] = Main.rand.Next(-100, 101);
                npc.netUpdate = true;
            }
        }
        npc.TargetClosest();
        float num816 = 0.2f;
        float num817 = 200f;
        if (Main.npc[NPC.plantBoss].life < Main.npc[NPC.plantBoss].lifeMax * 0.25)
        {
            num817 += 100f;
        }
        if (Main.npc[NPC.plantBoss].life < Main.npc[NPC.plantBoss].lifeMax * 0.1)
        {
            num817 += 100f;
        }
        if (Main.expertMode)
        {
            float num818 = 1f - npc.life / (float)npc.lifeMax;
            num817 += num818 * 300f;
            num816 += 0.3f;
        }
        if (Main.getGoodWorld)
        {
            num816 += 4f;
        }
        if (!Main.npc[num815].active || NPC.plantBoss < 0)
        {
            npc.active = false;
            return;
        }
        float num819 = Main.npc[num815].position.X + Main.npc[num815].width / 2;
        float num820 = Main.npc[num815].position.Y + Main.npc[num815].height / 2;
        Vector2 vector103 = new(num819, num820);
        float num821 = num819 + npc.ai[0];
        float num822 = num820 + npc.ai[1];
        float num823 = num821 - vector103.X;
        float num824 = num822 - vector103.Y;
        float num825 = (float)Math.Sqrt(num823 * num823 + num824 * num824);
        num825 = num817 / num825;
        num823 *= num825;
        num824 *= num825;
        if (npc.position.X < num819 + num823)
        {
            npc.velocity.X += num816;
            if (npc.velocity.X < 0f && num823 > 0f)
            {
                npc.velocity.X *= 0.9f;
            }
        }
        else if (npc.position.X > num819 + num823)
        {
            npc.velocity.X -= num816;
            if (npc.velocity.X > 0f && num823 < 0f)
            {
                npc.velocity.X *= 0.9f;
            }
        }
        if (npc.position.Y < num820 + num824)
        {
            npc.velocity.Y += num816;
            if (npc.velocity.Y < 0f && num824 > 0f)
            {
                npc.velocity.Y *= 0.9f;
            }
        }
        else if (npc.position.Y > num820 + num824)
        {
            npc.velocity.Y -= num816;
            if (npc.velocity.Y > 0f && num824 < 0f)
            {
                npc.velocity.Y *= 0.9f;
            }
        }
        if (npc.velocity.X > 8f)
        {
            npc.velocity.X = 8f;
        }
        if (npc.velocity.X < -8f)
        {
            npc.velocity.X = -8f;
        }
        if (npc.velocity.Y > 8f)
        {
            npc.velocity.Y = 8f;
        }
        if (npc.velocity.Y < -8f)
        {
            npc.velocity.Y = -8f;
        }
        if (num823 > 0f)
        {
            npc.spriteDirection = 1;
            npc.rotation = (float)Math.Atan2(num824, num823);
        }
        if (num823 < 0f)
        {
            npc.spriteDirection = -1;
            npc.rotation = (float)Math.Atan2(num824, num823) + 3.14f;
        }
    }
}
