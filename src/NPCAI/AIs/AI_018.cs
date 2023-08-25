using OTAPI;

namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_018(this NPC npc)
    {
        bool flag12 = false;
        if (npc.wet && npc.ai[1] == 1f)
        {
            flag12 = true;
        }
        else
        {
            npc.dontTakeDamage = false;
        }
        if (Main.expertMode && (npc.type == 63 || npc.type == 64 || npc.type == 103 || npc.type == 242))
        {
            if (npc.wet)
            {
                if (npc.target >= 0 && Main.player[npc.target].wet && !Main.player[npc.target].dead && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height) && (Main.player[npc.target].Center - npc.Center).Length() < 150f)
                {
                    if (npc.ai[1] == 0f)
                    {
                        npc.ai[2] += 2f;
                    }
                    else
                    {
                        npc.ai[2] -= 0.25f;
                    }
                }
                if (flag12)
                {
                    npc.dontTakeDamage = true;
                    npc.ai[2] += 1f;
                    if (npc.ai[2] >= 120f)
                    {
                        npc.ai[1] = 0f;
                    }
                }
                else
                {
                    npc.ai[2] += 1f;
                    if (npc.ai[2] >= 420f)
                    {
                        npc.ai[1] = 1f;
                        npc.ai[2] = 0f;
                    }
                }
            }
            else
            {
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
            }
        }
        float num268 = 1f;
        if (flag12)
        {
            num268 += 0.5f;
        }
        if (npc.type == 63)
        {
            Lighting.AddLight((int)(npc.position.X + npc.height / 2) / 16, (int)(npc.position.Y + npc.height / 2) / 16, 0.05f * num268, 0.15f * num268, 0.4f * num268);
        }
        else if (npc.type == 103)
        {
            Lighting.AddLight((int)(npc.position.X + npc.height / 2) / 16, (int)(npc.position.Y + npc.height / 2) / 16, 0.05f * num268, 0.45f * num268, 0.1f * num268);
        }
        else if (npc.type != 221 && npc.type != 242)
        {
            Lighting.AddLight((int)(npc.position.X + npc.height / 2) / 16, (int)(npc.position.Y + npc.height / 2) / 16, 0.35f * num268, 0.05f * num268, 0.2f * num268);
        }
        if (npc.direction == 0)
        {
            npc.TargetClosest();
        }
        if (flag12)
        {
            return;
        }
        if (npc.wet)
        {
            int x4 = (int)npc.Center.X / 16;
            int num269 = (int)(npc.position.Y + npc.height) / 16;
            if (Main.tile[x4, num269].topSlope())
            {
                if (Main.tile[x4, num269].leftSlope())
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
            else if (Main.tile[x4, num269 + 1].topSlope())
            {
                if (Main.tile[x4, num269 + 1].leftSlope())
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
            if (npc.collideX)
            {
                npc.velocity.X *= -1f;
                npc.direction *= -1;
            }
            if (npc.collideY)
            {
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
            bool flag13 = false;
            if (!npc.friendly)
            {
                npc.TargetClosest(faceTarget: false);
                if (Main.player[npc.target].wet && !Main.player[npc.target].dead && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    flag13 = true;
                }
            }
            if (flag13)
            {
                npc.localAI[2] = 1f;
                npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
                npc.velocity *= 0.98f;
                float num270 = 0.2f;
                if (npc.type == 103)
                {
                    npc.velocity *= 0.98f;
                    num270 = 0.6f;
                }
                if (npc.type == 221)
                {
                    npc.velocity *= 0.99f;
                    num270 = 1f;
                }
                if (npc.type == 242)
                {
                    npc.velocity *= 0.995f;
                    num270 = 3f;
                }
                if (npc.velocity.X > 0f - num270 && npc.velocity.X < num270 && npc.velocity.Y > 0f - num270 && npc.velocity.Y < num270)
                {
                    if (npc.type == 221)
                    {
                        npc.localAI[0] = 1f;
                    }
                    npc.TargetClosest();
                    float num271 = 7f;
                    if (npc.type == 103)
                    {
                        num271 = 9f;
                    }
                    Vector2 vector31 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                    float num272 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector31.X;
                    float num273 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector31.Y;
                    float num274 = (float)Math.Sqrt(num272 * num272 + num273 * num273);
                    num274 = num271 / num274;
                    num272 *= num274;
                    num273 *= num274;
                    npc.velocity.X = num272;
                    npc.velocity.Y = num273;
                }
                return;
            }
            npc.localAI[2] = 0f;
            npc.velocity.X += npc.direction * 0.02f;
            npc.rotation = npc.velocity.X * 0.4f;
            if (npc.velocity.X < -1f || npc.velocity.X > 1f)
            {
                npc.velocity.X *= 0.95f;
            }
            if (npc.ai[0] == -1f)
            {
                npc.velocity.Y -= 0.01f;
                if (npc.velocity.Y < -1f)
                {
                    npc.ai[0] = 1f;
                }
            }
            else
            {
                npc.velocity.Y += 0.01f;
                if (npc.velocity.Y > 1f)
                {
                    npc.ai[0] = -1f;
                }
            }
            int x5 = (int)(npc.position.X + npc.width / 2) / 16;
            int num275 = (int)(npc.position.Y + npc.height / 2) / 16;
            if (Main.tile[x5, num275 - 1] == null)
            {
                Main.tile[x5, num275 - 1] = Hooks.Tile.InvokeCreate();
            }
            if (Main.tile[x5, num275 + 1] == null)
            {
                Main.tile[x5, num275 + 1] = Hooks.Tile.InvokeCreate();
            }
            if (Main.tile[x5, num275 + 2] == null)
            {
                Main.tile[x5, num275 + 2] = Hooks.Tile.InvokeCreate();
            }
            if (Main.tile[x5, num275 - 1].liquid > 128)
            {
                if (Main.tile[x5, num275 + 1].active())
                {
                    npc.ai[0] = -1f;
                }
                else if (Main.tile[x5, num275 + 2].active())
                {
                    npc.ai[0] = -1f;
                }
            }
            else
            {
                npc.ai[0] = 1f;
            }
            if (npc.velocity.Y > 1.2 || npc.velocity.Y < -1.2)
            {
                npc.velocity.Y *= 0.99f;
            }
            return;
        }
        npc.rotation += npc.velocity.X * 0.1f;
        if (npc.velocity.Y == 0f)
        {
            npc.velocity.X *= 0.98f;
            if (npc.velocity.X > -0.01 && npc.velocity.X < 0.01)
            {
                npc.velocity.X = 0f;
            }
        }
        npc.velocity.Y += 0.2f;
        if (npc.velocity.Y > 10f)
        {
            npc.velocity.Y = 10f;
        }
        npc.ai[0] = 1f;

    }
}
