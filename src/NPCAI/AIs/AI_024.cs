using OTAPI;

namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_024(this NPC npc)
    {
        npc.noGravity = true;
        if (npc.type == 611)
        {
            if (npc.localAI[0] == 0f)
            {
                npc.TargetClosest();
                npc.ai[0] = 1f;
                npc.localAI[0] = 1f;
                npc.netUpdate = true;
            }
            else if (npc.ai[0] == 1f && Main.cloudAlpha == 0f && !Main.dayTime)
            {
                for (int num331 = 0; num331 < 200; num331++)
                {
                    if (num331 == npc.whoAmI || !Main.npc[num331].active)
                    {
                        continue;
                    }
                    if (Main.npc[num331].townNPC)
                    {
                        if (Math.Abs(npc.Center.X - Main.npc[num331].Center.X) < 96f)
                        {
                            float num332 = Main.npc[num331].Center.Y - npc.Center.Y;
                            if (num332 > 32f && num332 < 320f && !Collision.CanHit(npc, Main.npc[num331]))
                            {
                                npc.ai[0] = 2f;
                            }
                        }
                    }
                    else if (Main.npc[num331].type == npc.type && Main.npc[num331].ai[0] != 1f && Math.Abs(npc.Center.X - Main.npc[num331].Center.X) < 320f)
                    {
                        npc.ai[0] = 1f;
                        break;
                    }
                }
            }
        }
        if (npc.ai[0] == 0f)
        {
            npc.noGravity = false;
            if (npc.type == 611 && (Main.cloudAlpha > 0f || Main.dayTime))
            {
                npc.ai[0] = 1f;
            }
            npc.TargetClosest();
            if (Main.netMode != 1)
            {
                if (npc.releaseOwner != 255 || npc.velocity.X != 0f || npc.velocity.Y < 0f || npc.velocity.Y > 0.3)
                {
                    npc.ai[0] = 1f;
                    npc.netUpdate = true;
                    npc.direction = -npc.direction;
                }
                else if (npc.type != 611)
                {
                    Rectangle rectangle2 = new((int)Main.player[npc.target].position.X, (int)Main.player[npc.target].position.Y, Main.player[npc.target].width, Main.player[npc.target].height);
                    if (new Rectangle((int)npc.position.X - 100, (int)npc.position.Y - 100, npc.width + 200, npc.height + 200).Intersects(rectangle2) || npc.life < npc.lifeMax)
                    {
                        npc.ai[0] = 1f;
                        npc.velocity.Y -= 6f;
                        npc.netUpdate = true;
                        npc.direction = -npc.direction;
                    }
                }
            }
        }
        else if (npc.ai[0] == 2f)
        {
            npc.velocity.X *= 0.98f;
            if (npc.velocity.Y == 0f)
            {
                npc.ai[0] = 0f;
                npc.velocity.X = 0f;
            }
            npc.velocity.Y += 0.05f;
            if (npc.velocity.Y > 2f)
            {
                npc.velocity.Y = 2f;
            }
        }
        else if (!Main.player[npc.target].dead)
        {
            float num333 = 3f;
            if (npc.type == 671 || npc.type == 672 || npc.type == 673 || npc.type == 674 || npc.type == 675)
            {
                num333 = 4f;
            }
            if (npc.collideX)
            {
                npc.direction *= -1;
                npc.velocity.X = npc.oldVelocity.X * -0.5f;
                if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < num333 - 1f)
                {
                    npc.velocity.X = num333 - 1f;
                }
                if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > 0f - num333 + 1f)
                {
                    npc.velocity.X = 0f - num333 + 1f;
                }
            }
            if (npc.collideY)
            {
                npc.velocity.Y = npc.oldVelocity.Y * -0.5f;
                if (npc.velocity.Y > 0f && npc.velocity.Y < 1f)
                {
                    npc.velocity.Y = 1f;
                }
                if (npc.velocity.Y < 0f && npc.velocity.Y > -1f)
                {
                    npc.velocity.Y = -1f;
                }
            }
            if (npc.direction == -1 && npc.velocity.X > 0f - num333)
            {
                npc.velocity.X -= 0.1f;
                if (npc.velocity.X > num333)
                {
                    npc.velocity.X -= 0.1f;
                }
                else if (npc.velocity.X > 0f)
                {
                    npc.velocity.X -= 0.05f;
                }
                if (npc.velocity.X < 0f - num333)
                {
                    npc.velocity.X = 0f - num333;
                }
            }
            else if (npc.direction == 1 && npc.velocity.X < num333)
            {
                npc.velocity.X += 0.1f;
                if (npc.velocity.X < 0f - num333)
                {
                    npc.velocity.X += 0.1f;
                }
                else if (npc.velocity.X < 0f)
                {
                    npc.velocity.X += 0.05f;
                }
                if (npc.velocity.X > num333)
                {
                    npc.velocity.X = num333;
                }
            }
            int x10 = (int)((npc.position.X + npc.width / 2) / 16f) + npc.direction;
            int num334 = (int)((npc.position.Y + npc.height) / 16f);
            bool flag23 = true;
            int num335 = 15;
            bool flag24 = false;
            for (int num336 = num334; num336 < num334 + num335; num336++)
            {
                if (!WorldGen.InWorld(x10, num336))
                {
                    continue;
                }
                if (Main.tile[x10, num336] == null)
                {
                    Main.tile[x10, num336] = Hooks.Tile.InvokeCreate();
                }
                if ((Main.tile[x10, num336].nactive() && Main.tileSolid[Main.tile[x10, num336].type]) || Main.tile[x10, num336].liquid > 0)
                {
                    if (num336 < num334 + 5)
                    {
                        flag24 = true;
                    }
                    flag23 = false;
                    break;
                }
            }
            if (flag23)
            {
                npc.velocity.Y += 0.05f;
            }
            else
            {
                npc.velocity.Y -= 0.1f;
            }
            if (flag24)
            {
                npc.velocity.Y -= 0.2f;
            }
            if (npc.velocity.Y > 2f)
            {
                npc.velocity.Y = 2f;
            }
            if (npc.velocity.Y < -4f)
            {
                npc.velocity.Y = -4f;
            }
        }
        if (npc.wet)
        {
            npc.ai[1] = 0f;
            if (npc.velocity.Y > 0f)
            {
                npc.velocity.Y *= 0.95f;
            }
            npc.velocity.Y -= 0.5f;
            if (npc.velocity.Y < -4f)
            {
                npc.velocity.Y = -4f;
            }
            npc.TargetClosest();
        }
    }
}
