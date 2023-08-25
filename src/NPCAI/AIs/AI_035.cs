namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_035(this NPC npc)
    {
        npc.spriteDirection = -(int)npc.ai[0];
        if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].aiStyle != 32)
        {
            npc.ai[2] += 10f;
            if (npc.ai[2] > 50f || Main.netMode != 2)
            {
                npc.life = -1;
                npc.HitEffect();
                npc.active = false;
            }
        }
        if (npc.ai[2] == 0f)
        {
            if (Main.npc[(int)npc.ai[1]].ai[1] == 3f)
            {
                npc.EncourageDespawn(10);
            }
            if (Main.npc[(int)npc.ai[1]].ai[1] != 0f)
            {
                npc.localAI[0] += 2f;
                if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y - 100f)
                {
                    if (npc.velocity.Y > 0f)
                    {
                        npc.velocity.Y *= 0.96f;
                    }
                    npc.velocity.Y -= 0.07f;
                    if (npc.velocity.Y > 6f)
                    {
                        npc.velocity.Y = 6f;
                    }
                }
                else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 100f)
                {
                    if (npc.velocity.Y < 0f)
                    {
                        npc.velocity.Y *= 0.96f;
                    }
                    npc.velocity.Y += 0.07f;
                    if (npc.velocity.Y < -6f)
                    {
                        npc.velocity.Y = -6f;
                    }
                }
                if (npc.position.X + npc.width / 2 > Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 120f * npc.ai[0])
                {
                    if (npc.velocity.X > 0f)
                    {
                        npc.velocity.X *= 0.96f;
                    }
                    npc.velocity.X -= 0.1f;
                    if (npc.velocity.X > 8f)
                    {
                        npc.velocity.X = 8f;
                    }
                }
                if (npc.position.X + npc.width / 2 < Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 120f * npc.ai[0])
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X *= 0.96f;
                    }
                    npc.velocity.X += 0.1f;
                    if (npc.velocity.X < -8f)
                    {
                        npc.velocity.X = -8f;
                    }
                }
            }
            else
            {
                npc.ai[3] += 1f;
                if (npc.ai[3] >= 1100f)
                {
                    npc.localAI[0] = 0f;
                    npc.ai[2] = 1f;
                    npc.ai[3] = 0f;
                    npc.netUpdate = true;
                }
                if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y - 150f)
                {
                    if (npc.velocity.Y > 0f)
                    {
                        npc.velocity.Y *= 0.96f;
                    }
                    npc.velocity.Y -= 0.04f;
                    if (npc.velocity.Y > 3f)
                    {
                        npc.velocity.Y = 3f;
                    }
                }
                else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 150f)
                {
                    if (npc.velocity.Y < 0f)
                    {
                        npc.velocity.Y *= 0.96f;
                    }
                    npc.velocity.Y += 0.04f;
                    if (npc.velocity.Y < -3f)
                    {
                        npc.velocity.Y = -3f;
                    }
                }
                if (npc.position.X + npc.width / 2 > Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 + 200f)
                {
                    if (npc.velocity.X > 0f)
                    {
                        npc.velocity.X *= 0.96f;
                    }
                    npc.velocity.X -= 0.2f;
                    if (npc.velocity.X > 8f)
                    {
                        npc.velocity.X = 8f;
                    }
                }
                if (npc.position.X + npc.width / 2 < Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 + 160f)
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X *= 0.96f;
                    }
                    npc.velocity.X += 0.2f;
                    if (npc.velocity.X < -8f)
                    {
                        npc.velocity.X = -8f;
                    }
                }
            }
            Vector2 vector66 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num534 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 200f * npc.ai[0] - vector66.X;
            float num535 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector66.Y;
            float num536 = (float)Math.Sqrt(num534 * num534 + num535 * num535);
            npc.rotation = (float)Math.Atan2(num535, num534) + 1.57f;
            if (Main.netMode != 1)
            {
                npc.localAI[0] += 1f;
                if (npc.localAI[0] > 140f)
                {
                    npc.localAI[0] = 0f;
                    float num537 = 12f;
                    int num538 = 0;
                    int num539 = 102;
                    num536 = num537 / num536;
                    num534 = (0f - num534) * num536;
                    num535 = (0f - num535) * num536;
                    num534 += Main.rand.Next(-40, 41) * 0.01f;
                    num535 += Main.rand.Next(-40, 41) * 0.01f;
                    vector66.X += num534 * 4f;
                    vector66.Y += num535 * 4f;
                    _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector66.X, vector66.Y, num534, num535, num539, num538, 0f, Main.myPlayer);
                }
            }
        }
        else
        {
            if (npc.ai[2] != 1f)
            {
                return;
            }
            npc.ai[3] += 1f;
            if (npc.ai[3] >= 300f)
            {
                npc.localAI[0] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
            Vector2 vector67 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num541 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - vector67.X;
            _ = Main.npc[(int)npc.ai[1]].position.Y - vector67.Y;
            float num542 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - 80f - vector67.Y;
            float num543 = (float)Math.Sqrt(num541 * num541 + num542 * num542);
            num543 = 6f / num543;
            num541 *= num543;
            num542 *= num543;
            if (npc.velocity.X > num541)
            {
                if (npc.velocity.X > 0f)
                {
                    npc.velocity.X *= 0.9f;
                }
                npc.velocity.X -= 0.04f;
            }
            if (npc.velocity.X < num541)
            {
                if (npc.velocity.X < 0f)
                {
                    npc.velocity.X *= 0.9f;
                }
                npc.velocity.X += 0.04f;
            }
            if (npc.velocity.Y > num542)
            {
                if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y *= 0.9f;
                }
                npc.velocity.Y -= 0.08f;
            }
            if (npc.velocity.Y < num542)
            {
                if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y *= 0.9f;
                }
                npc.velocity.Y += 0.08f;
            }
            npc.TargetClosest();
            vector67 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            num541 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector67.X;
            num542 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector67.Y;
            num543 = (float)Math.Sqrt(num541 * num541 + num542 * num542);
            npc.rotation = (float)Math.Atan2(num542, num541) - 1.57f;
            if (Main.netMode != 1)
            {
                npc.localAI[0] += 1f;
                if (npc.localAI[0] > 40f)
                {
                    npc.localAI[0] = 0f;
                    float num544 = 10f;
                    int num545 = 0;
                    int num546 = 102;
                    num543 = num544 / num543;
                    num541 *= num543;
                    num542 *= num543;
                    num541 += Main.rand.Next(-40, 41) * 0.01f;
                    num542 += Main.rand.Next(-40, 41) * 0.01f;
                    vector67.X += num541 * 4f;
                    vector67.Y += num542 * 4f;
                    _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector67.X, vector67.Y, num541, num542, num546, num545, 0f, Main.myPlayer);
                }
            }
        }
    }
}
