namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_038(this NPC npc)
    {
        float num562 = 4f;
        float num563 = 1f;
        if (npc.type == 143)
        {
            num562 = 3f;
            num563 = 0.7f;
        }
        if (npc.type == 145)
        {
            num562 = 3.5f;
            num563 = 0.8f;
        }
        if (npc.type == 143)
        {
            npc.ai[2] += 1f;
            if (npc.ai[2] >= 120f)
            {
                npc.ai[2] = 0f;
                if (Main.netMode != 1)
                {
                    Vector2 vector70 = new(npc.position.X + npc.width * 0.5f - npc.direction * 12, npc.position.Y + npc.height * 0.5f);
                    float speedX = 12 * npc.spriteDirection;
                    float speedY = 0f;
                    if (Main.netMode != 1)
                    {
                        int num564 = 25;
                        int num565 = 110;
                        int num566 = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector70.X, vector70.Y, speedX, speedY, num565, num564, 0f, Main.myPlayer);
                        Main.projectile[num566].ai[0] = 2f;
                        Main.projectile[num566].timeLeft = 300;
                        Main.projectile[num566].friendly = false;
                        NetMessage.SendData(27, -1, -1, null, num566);
                        npc.netUpdate = true;
                    }
                }
            }
        }
        if (npc.type == 144 && npc.ai[1] >= 3f)
        {
            npc.TargetClosest();
            npc.spriteDirection = npc.direction;
            if (npc.velocity.Y == 0f)
            {
                npc.velocity.X *= 0.9f;
                npc.ai[2] += 1f;
                if (npc.velocity.X > -0.3 && npc.velocity.X < 0.3)
                {
                    npc.velocity.X = 0f;
                }
                if (npc.ai[2] >= 200f)
                {
                    npc.ai[2] = 0f;
                    npc.ai[1] = 0f;
                }
            }
        }
        else if (npc.type == 145 && npc.ai[1] >= 3f)
        {
            npc.TargetClosest();
            if (npc.velocity.Y == 0f)
            {
                npc.velocity.X *= 0.9f;
                npc.ai[2] += 1f;
                if (npc.velocity.X > -0.3 && npc.velocity.X < 0.3)
                {
                    npc.velocity.X = 0f;
                }
                if (npc.ai[2] >= 16f)
                {
                    npc.ai[2] = 0f;
                    npc.ai[1] = 0f;
                }
            }
            if (npc.velocity.X == 0f && npc.velocity.Y == 0f && npc.ai[2] == 8f)
            {
                float num567 = 10f;
                Vector2 vector71 = new(npc.position.X + npc.width * 0.5f - npc.direction * 12, npc.position.Y + npc.height * 0.25f);
                float num568 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector71.X;
                float num569 = Main.player[npc.target].position.Y - vector71.Y;
                float num570 = (float)Math.Sqrt(num568 * num568 + num569 * num569);
                num570 = num567 / num570;
                num568 *= num570;
                num569 *= num570;
                if (Main.netMode != 1)
                {
                    int num571 = 35;
                    int num572 = 109;
                    int num573 = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector71.X, vector71.Y, num568, num569, num572, num571, 0f, Main.myPlayer);
                    Main.projectile[num573].ai[0] = 2f;
                    Main.projectile[num573].timeLeft = 300;
                    Main.projectile[num573].friendly = false;
                    NetMessage.SendData(27, -1, -1, null, num573);
                    npc.netUpdate = true;
                }
            }
        }
        else
        {
            if (npc.velocity.Y == 0f)
            {
                if (npc.localAI[2] == npc.position.X)
                {
                    npc.direction *= -1;
                    npc.ai[3] = 60f;
                }
                npc.localAI[2] = npc.position.X;
                if (npc.ai[3] == 0f)
                {
                    npc.TargetClosest();
                }
                npc.ai[0] += 1f;
                if (npc.ai[0] > 2f)
                {
                    npc.ai[0] = 0f;
                    npc.ai[1] += 1f;
                    npc.velocity.Y = -8.2f;
                    npc.velocity.X += npc.direction * num563 * 1.1f;
                }
                else
                {
                    npc.velocity.Y = -6f;
                    npc.velocity.X += npc.direction * num563 * 0.9f;
                }
                npc.spriteDirection = npc.direction;
            }
            npc.velocity.X += npc.direction * num563 * 0.01f;
        }
        if (npc.ai[3] > 0f)
        {
            npc.ai[3] -= 1f;
        }
        if (npc.velocity.X > num562 && npc.direction > 0)
        {
            npc.velocity.X = num562;
        }
        if (npc.velocity.X < 0f - num562 && npc.direction < 0)
        {
            npc.velocity.X = 0f - num562;
        }
    }
}
