namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_034(this NPC npc)
    {
        npc.spriteDirection = -(int)npc.ai[0];
        Vector2 vector61 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
        float num519 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 200f * npc.ai[0] - vector61.X;
        float num520 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector61.Y;
        float num521 = (float)Math.Sqrt(num519 * num519 + num520 * num520);
        if (npc.ai[2] != 99f)
        {
            if (num521 > 800f)
            {
                npc.ai[2] = 99f;
            }
        }
        else if (num521 < 400f)
        {
            npc.ai[2] = 0f;
        }
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
                    Vector2 vector62 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                    float num522 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector62.X;
                    float num523 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector62.Y;
                    float num524 = (float)Math.Sqrt(num522 * num522 + num523 * num523);
                    num524 = 12f / num524;
                    num522 *= num524;
                    num523 *= num524;
                    npc.rotation = (float)Math.Atan2(num523, num522) - 1.57f;
                    if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < 2f)
                    {
                        npc.rotation = (float)Math.Atan2(num523, num522) - 1.57f;
                        npc.velocity.X = num522;
                        npc.velocity.Y = num523;
                        npc.netUpdate = true;
                    }
                    else
                    {
                        npc.velocity *= 0.97f;
                    }
                    npc.ai[3] += 1f;
                    if (npc.ai[3] >= 600f)
                    {
                        npc.ai[2] = 0f;
                        npc.ai[3] = 0f;
                        npc.netUpdate = true;
                    }
                }
            }
            else
            {
                npc.ai[3] += 1f;
                if (npc.ai[3] >= 600f)
                {
                    npc.ai[2] += 1f;
                    npc.ai[3] = 0f;
                    npc.netUpdate = true;
                }
                if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y + 300f)
                {
                    if (npc.velocity.Y > 0f)
                    {
                        npc.velocity.Y *= 0.96f;
                    }
                    npc.velocity.Y -= 0.1f;
                    if (npc.velocity.Y > 3f)
                    {
                        npc.velocity.Y = 3f;
                    }
                }
                else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y + 230f)
                {
                    if (npc.velocity.Y < 0f)
                    {
                        npc.velocity.Y *= 0.96f;
                    }
                    npc.velocity.Y += 0.1f;
                    if (npc.velocity.Y < -3f)
                    {
                        npc.velocity.Y = -3f;
                    }
                }
                if (npc.position.X + npc.width / 2 > Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 + 250f)
                {
                    if (npc.velocity.X > 0f)
                    {
                        npc.velocity.X *= 0.94f;
                    }
                    npc.velocity.X -= 0.3f;
                    if (npc.velocity.X > 9f)
                    {
                        npc.velocity.X = 9f;
                    }
                }
                if (npc.position.X + npc.width / 2 < Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2)
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X *= 0.94f;
                    }
                    npc.velocity.X += 0.2f;
                    if (npc.velocity.X < -8f)
                    {
                        npc.velocity.X = -8f;
                    }
                }
            }
            Vector2 vector63 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num525 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 200f * npc.ai[0] - vector63.X;
            float num526 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector63.Y;
            _ = (float)Math.Sqrt(num525 * num525 + num526 * num526);
            npc.rotation = (float)Math.Atan2(num526, num525) + 1.57f;
        }
        else if (npc.ai[2] == 1f)
        {
            if (npc.velocity.Y > 0f)
            {
                npc.velocity.Y *= 0.9f;
            }
            Vector2 vector64 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num528 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 280f * npc.ai[0] - vector64.X;
            float num529 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector64.Y;
            _ = (float)Math.Sqrt(num528 * num528 + num529 * num529);
            npc.rotation = (float)Math.Atan2(num529, num528) + 1.57f;
            npc.velocity.X = (npc.velocity.X * 5f + Main.npc[(int)npc.ai[1]].velocity.X) / 6f;
            npc.velocity.X += 0.5f;
            npc.velocity.Y -= 0.5f;
            if (npc.velocity.Y < -9f)
            {
                npc.velocity.Y = -9f;
            }
            if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 280f)
            {
                npc.TargetClosest();
                npc.ai[2] = 2f;
                vector64 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                num528 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector64.X;
                num529 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector64.Y;
                float num530 = (float)Math.Sqrt(num528 * num528 + num529 * num529);
                num530 = 20f / num530;
                npc.velocity.X = num528 * num530;
                npc.velocity.Y = num529 * num530;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[2] == 2f)
        {
            if (npc.position.Y > Main.player[npc.target].position.Y || npc.velocity.Y < 0f)
            {
                if (npc.ai[3] >= 4f)
                {
                    npc.ai[2] = 3f;
                    npc.ai[3] = 0f;
                }
                else
                {
                    npc.ai[2] = 1f;
                    npc.ai[3] += 1f;
                }
            }
        }
        else if (npc.ai[2] == 4f)
        {
            Vector2 vector65 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num531 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 200f * npc.ai[0] - vector65.X;
            float num532 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector65.Y;
            _ = (float)Math.Sqrt(num531 * num531 + num532 * num532);
            npc.rotation = (float)Math.Atan2(num532, num531) + 1.57f;
            npc.velocity.Y = (npc.velocity.Y * 5f + Main.npc[(int)npc.ai[1]].velocity.Y) / 6f;
            npc.velocity.X += 0.5f;
            if (npc.velocity.X > 12f)
            {
                npc.velocity.X = 12f;
            }
            if (npc.position.X + npc.width / 2 < Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 500f || npc.position.X + npc.width / 2 > Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 + 500f)
            {
                npc.TargetClosest();
                npc.ai[2] = 5f;
                vector65 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                num531 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector65.X;
                num532 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector65.Y;
                float num533 = (float)Math.Sqrt(num531 * num531 + num532 * num532);
                num533 = 17f / num533;
                npc.velocity.X = num531 * num533;
                npc.velocity.Y = num532 * num533;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[2] == 5f && npc.position.X + npc.width / 2 < Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - 100f)
        {
            if (npc.ai[3] >= 4f)
            {
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
            }
            else
            {
                npc.ai[2] = 4f;
                npc.ai[3] += 1f;
            }
        }
    }
}
