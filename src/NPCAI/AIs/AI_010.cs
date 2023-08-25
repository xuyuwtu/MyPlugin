namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_010(this NPC npc)
    {
        float num138 = 1f;
        float num139 = 0.011f;
        npc.TargetClosest();
        Vector2 vector18 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
        float num140 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector18.X;
        float num141 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector18.Y;
        float num142 = (float)Math.Sqrt(num140 * num140 + num141 * num141);
        float num143 = num142;
        npc.ai[1] += 1f;
        if (npc.ai[1] > 600f)
        {
            num139 *= 8f;
            num138 = 4f;
            if (npc.ai[1] > 650f)
            {
                npc.ai[1] = 0f;
            }
        }
        else if (num143 < 250f)
        {
            npc.ai[0] += 0.9f;
            if (npc.ai[0] > 0f)
            {
                npc.velocity.Y += 0.019f;
            }
            else
            {
                npc.velocity.Y -= 0.019f;
            }
            if (npc.ai[0] < -100f || npc.ai[0] > 100f)
            {
                npc.velocity.X += 0.019f;
            }
            else
            {
                npc.velocity.X -= 0.019f;
            }
            if (npc.ai[0] > 200f)
            {
                npc.ai[0] = -200f;
            }
        }
        if (num143 > 350f)
        {
            num138 = 5f;
            num139 = 0.3f;
        }
        else if (num143 > 300f)
        {
            num138 = 3f;
            num139 = 0.2f;
        }
        else if (num143 > 250f)
        {
            num138 = 1.5f;
            num139 = 0.1f;
        }
        num142 = num138 / num142;
        num140 *= num142;
        num141 *= num142;
        if (Main.player[npc.target].dead)
        {
            num140 = npc.direction * num138 / 2f;
            num141 = (0f - num138) / 2f;
        }
        if (npc.velocity.X < num140)
        {
            npc.velocity.X += num139;
        }
        else if (npc.velocity.X > num140)
        {
            npc.velocity.X -= num139;
        }
        if (npc.velocity.Y < num141)
        {
            npc.velocity.Y += num139;
        }
        else if (npc.velocity.Y > num141)
        {
            npc.velocity.Y -= num139;
        }
        if (num140 > 0f)
        {
            npc.spriteDirection = -1;
            npc.rotation = (float)Math.Atan2(num141, num140);
        }
        if (num140 < 0f)
        {
            npc.spriteDirection = 1;
            npc.rotation = (float)Math.Atan2(num141, num140) + 3.14f;
        }
        if (npc.type != 289)
        {
            return;
        }
        if (npc.justHit)
        {
            npc.ai[2] = 0f;
            npc.ai[3] = 0f;
        }
        vector18 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
        num140 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector18.X;
        num141 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector18.Y;
        num142 = (float)Math.Sqrt(num140 * num140 + num141 * num141);
        if (num142 <= 500f)
        {
            npc.ai[2] += 1f;
            if (npc.ai[3] == 0f)
            {
                if (npc.ai[2] > 120f)
                {
                    npc.ai[2] = 0f;
                    npc.ai[3] = 1f;
                    npc.netUpdate = true;
                }
                return;
            }
            if (npc.ai[2] > 40f)
            {
                npc.ai[3] = 0f;
            }
            if (Main.netMode != 1 && npc.ai[2] == 20f)
            {
                float num144 = 6f;
                int num145 = 25;
                int num146 = 299;
                num142 = num144 / num142;
                num140 *= num142;
                num141 *= num142;
                _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector18.X, vector18.Y, num140, num141, num146, num145, 0f, Main.myPlayer);
            }
        }
        else
        {
            npc.ai[2] = 0f;
            npc.ai[3] = 0f;
        }
    }
}
