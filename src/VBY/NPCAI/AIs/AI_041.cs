namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_041(this NPC npc)
    {
        if (npc.ai[2] > 1f)
        {
            npc.ai[2] -= 1f;
        }
        if (npc.ai[2] == 0f)
        {
            npc.ai[0] = -100f;
            npc.ai[2] = 1f;
            npc.TargetClosest();
            npc.spriteDirection = npc.direction;
        }
        if (npc.type == 378)
        {
            Vector2 vector79 = new(-6f, -10f);
            vector79.X *= npc.spriteDirection;
            if (npc.ai[1] != 5f && Main.rand.Next(3) == 0)
            {
                npc.position += npc.netOffset;
                int num618 = Dust.NewDust(npc.Center + vector79 - Vector2.One * 5f, 4, 4, 6);
                Main.dust[num618].scale = 1.5f;
                Main.dust[num618].noGravity = true;
                Main.dust[num618].velocity = Main.dust[num618].velocity * 0.25f + Vector2.Normalize(vector79) * 1f;
                Main.dust[num618].velocity = Main.dust[num618].velocity.RotatedBy(-(float)Math.PI / 2f * npc.direction);
                npc.position -= npc.netOffset;
            }
            if (npc.ai[1] == 5f)
            {
                npc.velocity = Vector2.Zero;
                npc.position.X += npc.width / 2;
                npc.position.Y += npc.height / 2;
                npc.width = 160;
                npc.height = 160;
                npc.position.X -= npc.width / 2;
                npc.position.Y -= npc.height / 2;
                npc.dontTakeDamage = true;
                npc.position += npc.netOffset;
                if (npc.ai[2] > 7f)
                {
                    for (int num619 = 0; num619 < 8; num619++)
                    {
                        _ = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 31, 0f, 0f, 100, default, 1.5f);
                    }
                    for (int num621 = 0; num621 < 32; num621++)
                    {
                        int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default, 2.5f);
                        Main.dust[num622].noGravity = true;
                        Dust dust = Main.dust[num622];
                        dust.velocity *= 3f;
                        num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default, 1.5f);
                        dust = Main.dust[num622];
                        dust.velocity *= 2f;
                        Main.dust[num622].noGravity = true;
                    }
                    for (int num623 = 0; num623 < 2; num623++)
                    {
                        int num624 = Gore.NewGore(npc.position + new Vector2(npc.width * Main.rand.Next(100) / 100f, npc.height * Main.rand.Next(100) / 100f) - Vector2.One * 10f, default, Main.rand.Next(61, 64));
                        Gore gore = Main.gore[num624];
                        gore.velocity *= 0.3f;
                        Main.gore[num624].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                        Main.gore[num624].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
                    }
                    if (npc.ai[2] == 9f)
                    {
                        SoundEngine.PlaySound(SoundID.Item14, npc.position);
                    }
                }
                if (npc.ai[2] == 1f)
                {
                    npc.life = -1;
                    npc.HitEffect();
                    npc.active = false;
                }
                npc.position -= npc.netOffset;
                return;
            }
        }
        if (npc.type == 378 && npc.ai[1] != 5f)
        {
            if (npc.wet || Vector2.Distance(npc.Center, Main.player[npc.target].Center) < 64f)
            {
                npc.ai[1] = 5f;
                npc.ai[2] = 10f;
                npc.netUpdate = true;
                return;
            }
        }
        else if (npc.wet && npc.type != 177)
        {
            if (npc.collideX)
            {
                npc.direction *= -npc.direction;
                npc.spriteDirection = npc.direction;
            }
            if (npc.collideY)
            {
                npc.TargetClosest();
                if (npc.oldVelocity.Y < 0f)
                {
                    npc.velocity.Y = 5f;
                }
                else
                {
                    npc.velocity.Y -= 2f;
                }
                npc.spriteDirection = npc.direction;
            }
            if (npc.velocity.Y > 4f)
            {
                npc.velocity.Y *= 0.95f;
            }
            npc.velocity.Y -= 0.3f;
            if (npc.velocity.Y < -4f)
            {
                npc.velocity.Y = -4f;
            }
        }
        if (npc.velocity.Y == 0f)
        {
            if (npc.ai[3] == npc.position.X)
            {
                npc.direction *= -1;
                npc.ai[2] = 300f;
            }
            npc.ai[3] = 0f;
            npc.velocity.X *= 0.8f;
            if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
            {
                npc.velocity.X = 0f;
            }
            if (npc.type == 177)
            {
                npc.ai[0] += 2f;
            }
            else
            {
                npc.ai[0] += 5f;
            }
            Vector2 vector80 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num625 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector80.X;
            float num626 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector80.Y;
            float num627 = (float)Math.Sqrt(num625 * num625 + num626 * num626);
            float num628 = 400f / num627;
            num628 = ((npc.type != 177) ? (num628 * 10f) : (num628 * 5f));
            if (num628 > 30f)
            {
                num628 = 30f;
            }
            npc.ai[0] += (int)num628;
            if (npc.ai[0] >= 0f)
            {
                npc.netUpdate = true;
                if (npc.ai[2] == 1f)
                {
                    npc.TargetClosest();
                }
                if (npc.type == 177)
                {
                    if (npc.ai[1] == 2f)
                    {
                        npc.velocity.Y = -11.5f;
                        npc.velocity.X += 2f * npc.direction;
                        if (num627 < 350f && num627 > 200f)
                        {
                            npc.velocity.X += npc.direction;
                        }
                        npc.ai[0] = -200f;
                        npc.ai[1] = 0f;
                        npc.ai[3] = npc.position.X;
                    }
                    else
                    {
                        npc.velocity.Y = -7.5f;
                        npc.velocity.X += 4 * npc.direction;
                        if (num627 < 350f && num627 > 200f)
                        {
                            npc.velocity.X += npc.direction;
                        }
                        npc.ai[0] = -120f;
                        npc.ai[1] += 1f;
                    }
                }
                else
                {
                    if (npc.type == 378)
                    {
                        SoundEngine.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 124);
                    }
                    if (npc.ai[1] == 3f)
                    {
                        npc.velocity.Y = -9f;
                        npc.velocity.X += 3 * npc.direction;
                        if (num627 < 350f && num627 > 200f)
                        {
                            npc.velocity.X += npc.direction;
                        }
                        npc.ai[0] = -200f;
                        npc.ai[1] = 0f;
                        npc.ai[3] = npc.position.X;
                    }
                    else
                    {
                        npc.velocity.Y = -5f;
                        npc.velocity.X += 5 * npc.direction;
                        if (num627 < 350f && num627 > 200f)
                        {
                            npc.velocity.X += npc.direction;
                        }
                        npc.ai[0] = -120f;
                        npc.ai[1] += 1f;
                    }
                }
            }
            else if (npc.ai[0] >= -30f)
            {
                npc.aiAction = 1;
            }
            npc.spriteDirection = npc.direction;
        }
        else
        {
            if (npc.target >= 255)
            {
                return;
            }
            if (npc.type == 177)
            {
                bool flag33 = false;
                if (npc.position.Y + npc.height < Main.player[npc.target].position.Y && npc.position.X + npc.width > Main.player[npc.target].position.X && npc.position.X < Main.player[npc.target].position.X + Main.player[npc.target].width)
                {
                    flag33 = true;
                    npc.velocity.X *= 0.92f;
                    if (npc.velocity.Y < 0f)
                    {
                        npc.velocity.Y *= 0.9f;
                        npc.velocity.Y += 0.1f;
                    }
                }
                if (!flag33 && ((npc.direction == 1 && npc.velocity.X < 4f) || (npc.direction == -1 && npc.velocity.X > -4f)))
                {
                    if ((npc.direction == -1 && npc.velocity.X < 0.1) || (npc.direction == 1 && npc.velocity.X > -0.1))
                    {
                        npc.velocity.X += 0.2f * npc.direction;
                    }
                    else
                    {
                        npc.velocity.X *= 0.93f;
                    }
                }
            }
            else if ((npc.direction == 1 && npc.velocity.X < 3f) || (npc.direction == -1 && npc.velocity.X > -3f))
            {
                if ((npc.direction == -1 && npc.velocity.X < 0.1) || (npc.direction == 1 && npc.velocity.X > -0.1))
                {
                    npc.velocity.X += 0.2f * npc.direction;
                }
                else
                {
                    npc.velocity.X *= 0.93f;
                }
            }
        }
    }
}
