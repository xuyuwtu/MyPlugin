namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_012(this NPC npc)
    {
        npc.spriteDirection = -(int)npc.ai[0];
        if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].aiStyle != 11)
        {
            npc.ai[2] += 10f;
            if (npc.ai[2] > 50f || Main.netMode != 2)
            {
                npc.life = -1;
                npc.HitEffect();
                npc.active = false;
            }
        }
        if (npc.ai[2] == 0f || npc.ai[2] == 3f)
        {
            if (Main.npc[(int)npc.ai[1]].ai[1] == 3f)
            {
                npc.EncourageDespawn(10);
            }
            if (Main.npc[(int)npc.ai[1]].ai[1] != 0f)
            {
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
                if (Main.expertMode)
                {
                    npc.ai[3] += 0.5f;
                }
                if (npc.ai[3] >= 300f)
                {
                    npc.ai[2] += 1f;
                    npc.ai[3] = 0f;
                    npc.netUpdate = true;
                }
                if (Main.expertMode)
                {
                    if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y + 230f)
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
                    else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y + 230f)
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
                    if (npc.position.X + npc.width / 2 > Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 200f * npc.ai[0])
                    {
                        if (npc.velocity.X > 0f)
                        {
                            npc.velocity.X *= 0.96f;
                        }
                        npc.velocity.X -= 0.07f;
                        if (npc.velocity.X > 8f)
                        {
                            npc.velocity.X = 8f;
                        }
                    }
                    if (npc.position.X + npc.width / 2 < Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 200f * npc.ai[0])
                    {
                        if (npc.velocity.X < 0f)
                        {
                            npc.velocity.X *= 0.96f;
                        }
                        npc.velocity.X += 0.07f;
                        if (npc.velocity.X < -8f)
                        {
                            npc.velocity.X = -8f;
                        }
                    }
                }
                if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y + 230f)
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
                else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y + 230f)
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
                if (npc.position.X + npc.width / 2 > Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 200f * npc.ai[0])
                {
                    if (npc.velocity.X > 0f)
                    {
                        npc.velocity.X *= 0.96f;
                    }
                    npc.velocity.X -= 0.07f;
                    if (npc.velocity.X > 8f)
                    {
                        npc.velocity.X = 8f;
                    }
                }
                if (npc.position.X + npc.width / 2 < Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 200f * npc.ai[0])
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X *= 0.96f;
                    }
                    npc.velocity.X += 0.07f;
                    if (npc.velocity.X < -8f)
                    {
                        npc.velocity.X = -8f;
                    }
                }
            }
            Vector2 vector22 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num181 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 200f * npc.ai[0] - vector22.X;
            float num182 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector22.Y;
            _ = (float)Math.Sqrt(num181 * num181 + num182 * num182);
            npc.rotation = (float)Math.Atan2(num182, num181) + 1.57f;
        }
        else if (npc.ai[2] == 1f)
        {
            Vector2 vector23 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num184 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 200f * npc.ai[0] - vector23.X;
            float num185 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector23.Y;
            _ = (float)Math.Sqrt(num184 * num184 + num185 * num185);
            npc.rotation = (float)Math.Atan2(num185, num184) + 1.57f;
            npc.velocity.X *= 0.95f;
            npc.velocity.Y -= 0.1f;
            if (Main.expertMode)
            {
                npc.velocity.Y -= 0.06f;
                if (npc.velocity.Y < -13f)
                {
                    npc.velocity.Y = -13f;
                }
            }
            else if (npc.velocity.Y < -8f)
            {
                npc.velocity.Y = -8f;
            }
            if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 200f)
            {
                npc.TargetClosest();
                npc.ai[2] = 2f;
                vector23 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                num184 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector23.X;
                num185 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector23.Y;
                float num186 = (float)Math.Sqrt(num184 * num184 + num185 * num185);
                num186 = ((!Main.expertMode) ? (18f / num186) : (21f / num186));
                npc.velocity.X = num184 * num186;
                npc.velocity.Y = num185 * num186;
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
            Vector2 vector24 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num187 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 200f * npc.ai[0] - vector24.X;
            float num188 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector24.Y;
            _ = (float)Math.Sqrt(num187 * num187 + num188 * num188);
            npc.rotation = (float)Math.Atan2(num188, num187) + 1.57f;
            npc.velocity.Y *= 0.95f;
            npc.velocity.X += 0.1f * (0f - npc.ai[0]);
            if (Main.expertMode)
            {
                npc.velocity.X += 0.07f * (0f - npc.ai[0]);
                if (npc.velocity.X < -12f)
                {
                    npc.velocity.X = -12f;
                }
                else if (npc.velocity.X > 12f)
                {
                    npc.velocity.X = 12f;
                }
            }
            else if (npc.velocity.X < -8f)
            {
                npc.velocity.X = -8f;
            }
            else if (npc.velocity.X > 8f)
            {
                npc.velocity.X = 8f;
            }
            if (npc.position.X + npc.width / 2 < Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 500f || npc.position.X + npc.width / 2 > Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 + 500f)
            {
                npc.TargetClosest();
                npc.ai[2] = 5f;
                vector24 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                num187 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector24.X;
                num188 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector24.Y;
                float num189 = (float)Math.Sqrt(num187 * num187 + num188 * num188);
                num189 = ((!Main.expertMode) ? (17f / num189) : (22f / num189));
                npc.velocity.X = num187 * num189;
                npc.velocity.Y = num188 * num189;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[2] == 5f && ((npc.velocity.X > 0f && npc.position.X + npc.width / 2 > Main.player[npc.target].position.X + Main.player[npc.target].width / 2) || (npc.velocity.X < 0f && npc.position.X + npc.width / 2 < Main.player[npc.target].position.X + Main.player[npc.target].width / 2)))
        {
            npc.ai[2] = 0f;
        }
    }
}
