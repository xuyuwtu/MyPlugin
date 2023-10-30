namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_033(this NPC npc)
    {
        Vector2 vector56 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
        float num504 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 200f * npc.ai[0] - vector56.X;
        float num505 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector56.Y;
        float num506 = (float)Math.Sqrt(num504 * num504 + num505 * num505);
        if (npc.ai[2] != 99f)
        {
            if (num506 > 800f)
            {
                npc.ai[2] = 99f;
            }
        }
        else if (num506 < 400f)
        {
            npc.ai[2] = 0f;
        }
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
        if (npc.ai[2] == 99f)
        {
            if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y)
            {
                if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y *= 0.96f;
                }
                npc.velocity.Y -= 0.1f;
                if (npc.velocity.Y > 8f)
                {
                    npc.velocity.Y = 8f;
                }
            }
            else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y)
            {
                if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y *= 0.96f;
                }
                npc.velocity.Y += 0.1f;
                if (npc.velocity.Y < -8f)
                {
                    npc.velocity.Y = -8f;
                }
            }
            if (npc.position.X + npc.width / 2 > Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2)
            {
                if (npc.velocity.X > 0f)
                {
                    npc.velocity.X *= 0.96f;
                }
                npc.velocity.X -= 0.5f;
                if (npc.velocity.X > 12f)
                {
                    npc.velocity.X = 12f;
                }
            }
            if (npc.position.X + npc.width / 2 < Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2)
            {
                if (npc.velocity.X < 0f)
                {
                    npc.velocity.X *= 0.96f;
                }
                npc.velocity.X += 0.5f;
                if (npc.velocity.X < -12f)
                {
                    npc.velocity.X = -12f;
                }
            }
        }
        else if (npc.ai[2] == 0f || npc.ai[2] == 3f)
        {
            if (Main.npc[(int)npc.ai[1]].ai[1] == 3f)
            {
                npc.EncourageDespawn(10);
            }
            if (Main.npc[(int)npc.ai[1]].ai[1] != 0f)
            {
                npc.TargetClosest();
                if (Main.player[npc.target].dead)
                {
                    npc.velocity.Y += 0.1f;
                    if (npc.velocity.Y > 16f)
                    {
                        npc.velocity.Y = 16f;
                    }
                }
                else
                {
                    Vector2 vector57 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                    float num507 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector57.X;
                    float num508 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector57.Y;
                    float num509 = (float)Math.Sqrt(num507 * num507 + num508 * num508);
                    num509 = 7f / num509;
                    num507 *= num509;
                    num508 *= num509;
                    npc.rotation = (float)Math.Atan2(num508, num507) - 1.57f;
                    if (npc.velocity.X > num507)
                    {
                        if (npc.velocity.X > 0f)
                        {
                            npc.velocity.X *= 0.97f;
                        }
                        npc.velocity.X -= 0.05f;
                    }
                    if (npc.velocity.X < num507)
                    {
                        if (npc.velocity.X < 0f)
                        {
                            npc.velocity.X *= 0.97f;
                        }
                        npc.velocity.X += 0.05f;
                    }
                    if (npc.velocity.Y > num508)
                    {
                        if (npc.velocity.Y > 0f)
                        {
                            npc.velocity.Y *= 0.97f;
                        }
                        npc.velocity.Y -= 0.05f;
                    }
                    if (npc.velocity.Y < num508)
                    {
                        if (npc.velocity.Y < 0f)
                        {
                            npc.velocity.Y *= 0.97f;
                        }
                        npc.velocity.Y += 0.05f;
                    }
                }
                npc.ai[3] += 1f;
                if (npc.ai[3] >= 600f)
                {
                    npc.ai[2] = 0f;
                    npc.ai[3] = 0f;
                    npc.netUpdate = true;
                }
            }
            else
            {
                npc.ai[3] += 1f;
                if (npc.ai[3] >= 300f)
                {
                    npc.ai[2] += 1f;
                    npc.ai[3] = 0f;
                    npc.netUpdate = true;
                }
                if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y + 320f)
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
                else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y + 260f)
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
                if (npc.position.X + npc.width / 2 > Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2)
                {
                    if (npc.velocity.X > 0f)
                    {
                        npc.velocity.X *= 0.96f;
                    }
                    npc.velocity.X -= 0.3f;
                    if (npc.velocity.X > 12f)
                    {
                        npc.velocity.X = 12f;
                    }
                }
                if (npc.position.X + npc.width / 2 < Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 250f)
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X *= 0.96f;
                    }
                    npc.velocity.X += 0.3f;
                    if (npc.velocity.X < -12f)
                    {
                        npc.velocity.X = -12f;
                    }
                }
            }
            Vector2 vector58 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num510 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 200f * npc.ai[0] - vector58.X;
            float num511 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector58.Y;
            _ = (float)Math.Sqrt(num510 * num510 + num511 * num511);
            npc.rotation = (float)Math.Atan2(num511, num510) + 1.57f;
        }
        else if (npc.ai[2] == 1f)
        {
            Vector2 vector59 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num513 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 200f * npc.ai[0] - vector59.X;
            float num514 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector59.Y;
            _ = (float)Math.Sqrt(num513 * num513 + num514 * num514);
            npc.rotation = (float)Math.Atan2(num514, num513) + 1.57f;
            npc.velocity.X *= 0.95f;
            npc.velocity.Y -= 0.1f;
            if (npc.velocity.Y < -8f)
            {
                npc.velocity.Y = -8f;
            }
            if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 200f)
            {
                npc.TargetClosest();
                npc.ai[2] = 2f;
                vector59 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                num513 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector59.X;
                num514 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector59.Y;
                float num515 = (float)Math.Sqrt(num513 * num513 + num514 * num514);
                num515 = 22f / num515;
                npc.velocity.X = num513 * num515;
                npc.velocity.Y = num514 * num515;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[2] == 2f)
        {
            if (npc.position.Y > Main.player[npc.target].position.Y || npc.velocity.Y < 0f)
            {
                npc.ai[2] = 3f;
            }
        }
        else if (npc.ai[2] == 4f)
        {
            npc.TargetClosest();
            Vector2 vector60 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num516 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector60.X;
            float num517 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector60.Y;
            float num518 = (float)Math.Sqrt(num516 * num516 + num517 * num517);
            num518 = 7f / num518;
            num516 *= num518;
            num517 *= num518;
            if (npc.velocity.X > num516)
            {
                if (npc.velocity.X > 0f)
                {
                    npc.velocity.X *= 0.97f;
                }
                npc.velocity.X -= 0.05f;
            }
            if (npc.velocity.X < num516)
            {
                if (npc.velocity.X < 0f)
                {
                    npc.velocity.X *= 0.97f;
                }
                npc.velocity.X += 0.05f;
            }
            if (npc.velocity.Y > num517)
            {
                if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y *= 0.97f;
                }
                npc.velocity.Y -= 0.05f;
            }
            if (npc.velocity.Y < num517)
            {
                if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y *= 0.97f;
                }
                npc.velocity.Y += 0.05f;
            }
            npc.ai[3] += 1f;
            if (npc.ai[3] >= 600f)
            {
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
            vector60 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            num516 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 200f * npc.ai[0] - vector60.X;
            num517 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector60.Y;
            _ = (float)Math.Sqrt(num516 * num516 + num517 * num517);
            npc.rotation = (float)Math.Atan2(num517, num516) + 1.57f;
        }
        else if (npc.ai[2] == 5f && ((npc.velocity.X > 0f && npc.position.X + npc.width / 2 > Main.player[npc.target].position.X + Main.player[npc.target].width / 2) || (npc.velocity.X < 0f && npc.position.X + npc.width / 2 < Main.player[npc.target].position.X + Main.player[npc.target].width / 2)))
        {
            npc.ai[2] = 0f;
        }
    }
}
