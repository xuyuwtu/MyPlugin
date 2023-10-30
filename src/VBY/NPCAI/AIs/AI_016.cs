using OTAPI;

namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_016(this NPC npc)
    {
        if (npc.direction == 0)
        {
            npc.TargetClosest();
        }
        if (npc.type == 615)
        {
            if (npc.ai[2] == 0f)
            {
                int num260 = Main.rand.Next(300, 1200);
                if ((npc.ai[3] += 1f) >= num260)
                {
                    npc.ai[2] = Main.rand.Next(1, 3);
                    if (npc.ai[2] == 1f && !Collision.CanHitLine(npc.position, npc.width, npc.height, new Vector2(npc.position.X, npc.position.Y - 128f), npc.width, npc.height))
                    {
                        npc.ai[2] = 2f;
                    }
                    if (npc.ai[2] == 2f)
                    {
                        npc.TargetClosest();
                    }
                    npc.ai[3] = 0f;
                    npc.netUpdate = true;
                }
            }
            if (npc.ai[2] == 1f)
            {
                if (npc.collideY || npc.collideX)
                {
                    npc.ai[2] = 0f;
                    npc.ai[3] = 0f;
                    npc.netUpdate = true;
                }
                else if (npc.wet)
                {
                    npc.velocity.Y -= 0.4f;
                    if (npc.velocity.Y < -6f)
                    {
                        npc.velocity.Y = -6f;
                    }
                    npc.rotation = npc.velocity.Y * npc.direction * 0.3f;
                    if (npc.rotation < (float)Math.PI * -2f / 5f)
                    {
                        npc.rotation = (float)Math.PI * -2f / 5f;
                    }
                    if (npc.rotation > (float)Math.PI * 2f / 5f)
                    {
                        npc.rotation = (float)Math.PI * 2f / 5f;
                    }
                    if (npc.ai[3] == 1f)
                    {
                        npc.ai[2] = 0f;
                        npc.ai[3] = 0f;
                        npc.netUpdate = true;
                    }
                }
                else
                {
                    npc.rotation += npc.direction * 0.2f;
                    npc.ai[3] = 1f;
                    npc.velocity.Y += 0.3f;
                    if (npc.velocity.Y > 10f)
                    {
                        npc.velocity.Y = 10f;
                    }
                }
                return;
            }
            if (npc.ai[2] == 2f)
            {
                if (npc.collideY || npc.collideX)
                {
                    npc.ai[2] = 0f;
                    npc.ai[3] = 0f;
                    npc.netUpdate = true;
                }
                else if (npc.wet)
                {
                    npc.velocity.Y -= 0.4f;
                    if (npc.velocity.Y < -6f)
                    {
                        npc.velocity.Y = -6f;
                    }
                    npc.rotation = npc.velocity.Y * npc.direction * 0.3f;
                    if (npc.rotation < (float)Math.PI * -2f / 5f)
                    {
                        npc.rotation = (float)Math.PI * -2f / 5f;
                    }
                    if (npc.rotation > (float)Math.PI * 2f / 5f)
                    {
                        npc.rotation = (float)Math.PI * 2f / 5f;
                    }
                    if (Collision.GetWaterLine(npc.Top.ToTileCoordinates(), out var waterLineHeight))
                    {
                        float y3 = waterLineHeight + 0f - npc.position.Y;
                        npc.velocity.Y = y3;
                        npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -2f, 0.5f);
                        npc.rotation = -(float)Math.PI / 5f * npc.direction;
                        npc.velocity.X *= 0.95f;
                        if (npc.ai[3] == 0f)
                        {
                            npc.netUpdate = true;
                        }
                        npc.ai[3]++;
                        if (npc.ai[3] >= 300f)
                        {
                            npc.ai[2] = 0f;
                            npc.ai[3] = 0f;
                            npc.netUpdate = true;
                            npc.velocity.Y = 4f;
                        }
                        if (npc.ai[3] == 60f && Main.rand.Next(2) == 0)
                        {
                            SoundEngine.PlaySound(45, (int)npc.position.X, (int)npc.position.Y);
                        }
                    }
                }
                else
                {
                    npc.ai[2] = 0f;
                    npc.ai[3] = 0f;
                    npc.netUpdate = true;
                    npc.velocity.Y += 0.3f;
                    if (npc.velocity.Y > 10f)
                    {
                        npc.velocity.Y = 10f;
                    }
                }
                return;
            }
        }
        if (npc.wet)
        {
            bool flag11 = false;
            if (npc.type != 55 && npc.type != 592 && npc.type != 607 && npc.type != 615)
            {
                npc.TargetClosest(faceTarget: false);
                if (Main.player[npc.target].wet && !Main.player[npc.target].dead && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    flag11 = true;
                }
            }
            int x2 = (int)npc.Center.X / 16;
            int num261 = (int)(npc.position.Y + npc.height) / 16;
            if (Main.tile[x2, num261].topSlope())
            {
                if (Main.tile[x2, num261].leftSlope())
                {
                    npc.direction = -1;
                    npc.velocity.X = Math.Abs(npc.velocity.X) * -1f;
                }
                else
                {
                    npc.direction = 1;
                    npc.velocity.X = Math.Abs(npc.velocity.X);
                }
            }
            else if (Main.tile[x2, num261 + 1].topSlope())
            {
                if (Main.tile[x2, num261 + 1].leftSlope())
                {
                    npc.direction = -1;
                    npc.velocity.X = Math.Abs(npc.velocity.X) * -1f;
                }
                else
                {
                    npc.direction = 1;
                    npc.velocity.X = Math.Abs(npc.velocity.X);
                }
            }
            if (!flag11)
            {
                if (npc.collideX)
                {
                    npc.velocity.X *= -1f;
                    npc.direction *= -1;
                    npc.netUpdate = true;
                }
                if (npc.collideY)
                {
                    npc.netUpdate = true;
                    if (npc.velocity.Y > 0f)
                    {
                        npc.velocity.Y = Math.Abs(npc.velocity.Y) * -1f;
                        npc.directionY = -1;
                        npc.ai[0] = -1f;
                    }
                    else if (npc.velocity.Y < 0f)
                    {
                        npc.velocity.Y = Math.Abs(npc.velocity.Y);
                        npc.directionY = 1;
                        npc.ai[0] = 1f;
                    }
                }
            }
            if (npc.type == 102)
            {
                Lighting.AddLight((int)(npc.position.X + npc.width / 2 + npc.direction * (npc.width + 8)) / 16, (int)(npc.position.Y + 2f) / 16, 0.07f, 0.04f, 0.025f);
            }
            if (flag11)
            {
                npc.TargetClosest();
                if (npc.type == 157)
                {
                    if (npc.velocity.X > 0f && npc.direction < 0)
                    {
                        npc.velocity.X *= 0.95f;
                    }
                    if (npc.velocity.X < 0f && npc.direction > 0)
                    {
                        npc.velocity.X *= 0.95f;
                    }
                    npc.velocity.X += npc.direction * 0.25f;
                    npc.velocity.Y += npc.directionY * 0.2f;
                    if (npc.velocity.X > 8f)
                    {
                        npc.velocity.X = 7f;
                    }
                    if (npc.velocity.X < -8f)
                    {
                        npc.velocity.X = -7f;
                    }
                    if (npc.velocity.Y > 5f)
                    {
                        npc.velocity.Y = 4f;
                    }
                    if (npc.velocity.Y < -5f)
                    {
                        npc.velocity.Y = -4f;
                    }
                }
                else if (npc.type == 65 || npc.type == 102)
                {
                    npc.velocity.X += npc.direction * 0.15f;
                    npc.velocity.Y += npc.directionY * 0.15f;
                    if (npc.velocity.X > 5f)
                    {
                        npc.velocity.X = 5f;
                    }
                    if (npc.velocity.X < -5f)
                    {
                        npc.velocity.X = -5f;
                    }
                    if (npc.velocity.Y > 3f)
                    {
                        npc.velocity.Y = 3f;
                    }
                    if (npc.velocity.Y < -3f)
                    {
                        npc.velocity.Y = -3f;
                    }
                }
                else
                {
                    npc.velocity.X += npc.direction * 0.1f;
                    npc.velocity.Y += npc.directionY * 0.1f;
                    if (npc.velocity.X > 3f)
                    {
                        npc.velocity.X = 3f;
                    }
                    if (npc.velocity.X < -3f)
                    {
                        npc.velocity.X = -3f;
                    }
                    if (npc.velocity.Y > 2f)
                    {
                        npc.velocity.Y = 2f;
                    }
                    if (npc.velocity.Y < -2f)
                    {
                        npc.velocity.Y = -2f;
                    }
                }
            }
            else
            {
                if (npc.type == 157)
                {
                    if (Main.player[npc.target].position.Y > npc.position.Y)
                    {
                        npc.directionY = 1;
                    }
                    else
                    {
                        npc.directionY = -1;
                    }
                    npc.velocity.X += npc.direction * 0.2f;
                    if (npc.velocity.X < -2f || npc.velocity.X > 2f)
                    {
                        npc.velocity.X *= 0.95f;
                    }
                    if (npc.ai[0] == -1f)
                    {
                        float num262 = -0.6f;
                        if (npc.directionY < 0)
                        {
                            num262 = -1f;
                        }
                        if (npc.directionY > 0)
                        {
                            num262 = -0.2f;
                        }
                        npc.velocity.Y -= 0.02f;
                        if (npc.velocity.Y < num262)
                        {
                            npc.ai[0] = 1f;
                        }
                    }
                    else
                    {
                        float num263 = 0.6f;
                        if (npc.directionY < 0)
                        {
                            num263 = 0.2f;
                        }
                        if (npc.directionY > 0)
                        {
                            num263 = 1f;
                        }
                        npc.velocity.Y += 0.02f;
                        if (npc.velocity.Y > num263)
                        {
                            npc.ai[0] = -1f;
                        }
                    }
                }
                else
                {
                    npc.velocity.X += npc.direction * 0.1f;
                    float num264 = 1f;
                    if (npc.type == 615)
                    {
                        num264 = 3f;
                    }
                    if (npc.velocity.X < 0f - num264 || npc.velocity.X > num264)
                    {
                        npc.velocity.X *= 0.95f;
                    }
                    if (npc.ai[0] == -1f)
                    {
                        npc.velocity.Y -= 0.01f;
                        if (npc.velocity.Y < -0.3)
                        {
                            npc.ai[0] = 1f;
                        }
                    }
                    else
                    {
                        npc.velocity.Y += 0.01f;
                        if (npc.velocity.Y > 0.3)
                        {
                            npc.ai[0] = -1f;
                        }
                    }
                }
                int x3 = (int)(npc.position.X + npc.width / 2) / 16;
                int num265 = (int)(npc.position.Y + npc.height / 2) / 16;
                if (Main.tile[x3, num265 - 1] == null)
                {
                    Main.tile[x3, num265 - 1] = Hooks.Tile.InvokeCreate();
                }
                if (Main.tile[x3, num265 + 1] == null)
                {
                    Main.tile[x3, num265 + 1] = Hooks.Tile.InvokeCreate();
                }
                if (Main.tile[x3, num265 + 2] == null)
                {
                    Main.tile[x3, num265 + 2] = Hooks.Tile.InvokeCreate();
                }
                if (Main.tile[x3, num265 - 1].liquid > 128)
                {
                    if (Main.tile[x3, num265 + 1].active())
                    {
                        npc.ai[0] = -1f;
                    }
                    else if (Main.tile[x3, num265 + 2].active())
                    {
                        npc.ai[0] = -1f;
                    }
                }
                if (npc.type != 157 && (npc.velocity.Y > 0.4 || npc.velocity.Y < -0.4))
                {
                    npc.velocity.Y *= 0.95f;
                }
            }
        }
        else
        {
            if (npc.velocity.Y == 0f)
            {
                if (npc.type == 65)
                {
                    npc.velocity.X *= 0.94f;
                    if (npc.velocity.X > -0.2 && npc.velocity.X < 0.2)
                    {
                        npc.velocity.X = 0f;
                    }
                }
                else if (Main.netMode != 1)
                {
                    npc.velocity.Y = Main.rand.Next(-50, -20) * 0.1f;
                    npc.velocity.X = Main.rand.Next(-20, 20) * 0.1f;
                    npc.netUpdate = true;
                }
            }
            npc.velocity.Y += 0.3f;
            if (npc.velocity.Y > 10f)
            {
                npc.velocity.Y = 10f;
            }
            npc.ai[0] = 1f;
        }
        npc.rotation = npc.velocity.Y * npc.direction * 0.1f;
        if (npc.rotation < -0.2)
        {
            npc.rotation = -0.2f;
        }
        if (npc.rotation > 0.2)
        {
            npc.rotation = 0.2f;
        }

    }
}
