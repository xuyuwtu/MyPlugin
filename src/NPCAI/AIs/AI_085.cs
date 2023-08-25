namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_085(this NPC npc)
    {
        npc.noTileCollide = false;
        if (npc.ai[0] == 0f)
        {
            npc.TargetClosest();
            if (Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
            {
                npc.ai[0] = 1f;
            }
            else
            {
                Vector2 vector245 = Main.player[npc.target].Center - npc.Center;
                vector245.Y -= Main.player[npc.target].height / 4;
                float num1327 = vector245.Length();
                if (num1327 > 800f)
                {
                    npc.ai[0] = 2f;
                }
                else
                {
                    Vector2 center31 = npc.Center;
                    center31.X = Main.player[npc.target].Center.X;
                    Vector2 vector246 = center31 - npc.Center;
                    if (vector246.Length() > 8f && Collision.CanHit(npc.Center, 1, 1, center31, 1, 1))
                    {
                        npc.ai[0] = 3f;
                        npc.ai[1] = center31.X;
                        npc.ai[2] = center31.Y;
                        Vector2 center32 = npc.Center;
                        center32.Y = Main.player[npc.target].Center.Y;
                        if (vector246.Length() > 8f && Collision.CanHit(npc.Center, 1, 1, center32, 1, 1) && Collision.CanHit(center32, 1, 1, Main.player[npc.target].position, 1, 1))
                        {
                            npc.ai[0] = 3f;
                            npc.ai[1] = center32.X;
                            npc.ai[2] = center32.Y;
                        }
                    }
                    else
                    {
                        center31 = npc.Center;
                        center31.Y = Main.player[npc.target].Center.Y;
                        if ((center31 - npc.Center).Length() > 8f && Collision.CanHit(npc.Center, 1, 1, center31, 1, 1))
                        {
                            npc.ai[0] = 3f;
                            npc.ai[1] = center31.X;
                            npc.ai[2] = center31.Y;
                        }
                    }
                    if (npc.ai[0] == 0f)
                    {
                        npc.localAI[0] = 0f;
                        vector245.Normalize();
                        vector245 *= 0.5f;
                        npc.velocity += vector245;
                        npc.ai[0] = 4f;
                        npc.ai[1] = 0f;
                    }
                }
            }
        }
        else if (npc.ai[0] == 1f)
        {
            npc.rotation += npc.direction * 0.3f;
            Vector2 vector247 = Main.player[npc.target].Center - npc.Center;
            if (npc.type == 421)
            {
                vector247 = Main.player[npc.target].Top - npc.Center;
            }
            float num1328 = vector247.Length();
            float num1329 = 5.5f;
            if (npc.type == 405)
            {
                num1329 = 8f;
            }
            num1329 += num1328 / 100f;
            int num1330 = 50;
            vector247.Normalize();
            vector247 *= num1329;
            npc.velocity = (npc.velocity * (num1330 - 1) + vector247) / num1330;
            if (!Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
            }
            if (npc.type == 421 && num1328 < 40f && Main.player[npc.target].active && !Main.player[npc.target].dead)
            {
                bool flag83 = true;
                for (int num1331 = 0; num1331 < 200; num1331++)
                {
                    NPC nPC10 = Main.npc[num1331];
                    if (nPC10.active && nPC10.type == npc.type && nPC10.ai[0] == 5f && nPC10.target == npc.target)
                    {
                        flag83 = false;
                        break;
                    }
                }
                if (flag83)
                {
                    npc.Center = Main.player[npc.target].Top;
                    npc.velocity = Vector2.Zero;
                    npc.ai[0] = 5f;
                    npc.ai[1] = 0f;
                    npc.netUpdate = true;
                }
            }
        }
        else if (npc.ai[0] == 2f)
        {
            npc.rotation = npc.velocity.X * 0.1f;
            npc.noTileCollide = true;
            Vector2 vector248 = Main.player[npc.target].Center - npc.Center;
            float num1332 = vector248.Length();
            float num1333 = 3f;
            if (npc.type == 405)
            {
                num1333 = 6f;
            }
            int num1334 = 3;
            vector248.Normalize();
            vector248 *= num1333;
            npc.velocity = (npc.velocity * (num1334 - 1) + vector248) / num1334;
            if (num1332 < 600f && !Collision.SolidCollision(npc.position, npc.width, npc.height))
            {
                npc.ai[0] = 0f;
            }
        }
        else if (npc.ai[0] == 3f)
        {
            npc.rotation = npc.velocity.X * 0.1f;
            Vector2 vector249 = new(npc.ai[1], npc.ai[2]);
            Vector2 vector250 = vector249 - npc.Center;
            float num1335 = vector250.Length();
            float num1336 = 2f;
            if (npc.type == 405)
            {
                num1336 = 3f;
            }
            float num1337 = 3f;
            vector250.Normalize();
            vector250 *= num1336;
            npc.velocity = (npc.velocity * (num1337 - 1f) + vector250) / num1337;
            if (npc.collideX || npc.collideY)
            {
                npc.ai[0] = 4f;
                npc.ai[1] = 0f;
            }
            if (num1335 < num1336 || num1335 > 800f || Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
            {
                npc.ai[0] = 0f;
            }
        }
        else if (npc.ai[0] == 4f)
        {
            npc.rotation = npc.velocity.X * 0.1f;
            if (npc.collideX)
            {
                npc.velocity.X *= -0.8f;
            }
            if (npc.collideY)
            {
                npc.velocity.Y *= -0.8f;
            }
            Vector2 vector251;
            if (npc.velocity.X == 0f && npc.velocity.Y == 0f)
            {
                vector251 = Main.player[npc.target].Center - npc.Center;
                vector251.Y -= Main.player[npc.target].height / 4;
                vector251.Normalize();
                npc.velocity = vector251 * 0.1f;
            }
            float num1338 = 2f;
            if (npc.type == 405)
            {
                num1338 = 3f;
            }
            float num1339 = 20f;
            vector251 = npc.velocity;
            vector251.Normalize();
            vector251 *= num1338;
            npc.velocity = (npc.velocity * (num1339 - 1f) + vector251) / num1339;
            npc.ai[1] += 1f;
            if (npc.ai[1] > 180f)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
            }
            if (Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
            {
                npc.ai[0] = 0f;
            }
            npc.localAI[0] += 1f;
            if (npc.localAI[0] >= 5f && !Collision.SolidCollision(npc.position - new Vector2(10f, 10f), npc.width + 20, npc.height + 20))
            {
                npc.localAI[0] = 0f;
                Vector2 center33 = npc.Center;
                center33.X = Main.player[npc.target].Center.X;
                if (Collision.CanHit(npc.Center, 1, 1, center33, 1, 1) && Collision.CanHit(npc.Center, 1, 1, center33, 1, 1) && Collision.CanHit(Main.player[npc.target].Center, 1, 1, center33, 1, 1))
                {
                    npc.ai[0] = 3f;
                    npc.ai[1] = center33.X;
                    npc.ai[2] = center33.Y;
                }
                else
                {
                    center33 = npc.Center;
                    center33.Y = Main.player[npc.target].Center.Y;
                    if (Collision.CanHit(npc.Center, 1, 1, center33, 1, 1) && Collision.CanHit(Main.player[npc.target].Center, 1, 1, center33, 1, 1))
                    {
                        npc.ai[0] = 3f;
                        npc.ai[1] = center33.X;
                        npc.ai[2] = center33.Y;
                    }
                }
            }
        }
        else if (npc.ai[0] == 5f)
        {
            Player player11 = Main.player[npc.target];
            if (!player11.active || player11.dead)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.netUpdate = true;
            }
            else
            {
                npc.Center = ((player11.gravDir == 1f) ? player11.Top : player11.Bottom) + new Vector2(player11.direction * 4, 0f);
                npc.gfxOffY = player11.gfxOffY;
                npc.velocity = Vector2.Zero;
                if (!player11.creativeGodMode)
                {
                    player11.AddBuff(163, 59);
                }
            }
        }
        if (npc.type == 405)
        {
            npc.rotation = 0f;
            for (int num1340 = 0; num1340 < 200; num1340++)
            {
                if (num1340 != npc.whoAmI && Main.npc[num1340].active && Main.npc[num1340].type == npc.type && Math.Abs(npc.position.X - Main.npc[num1340].position.X) + Math.Abs(npc.position.Y - Main.npc[num1340].position.Y) < npc.width)
                {
                    if (npc.position.X < Main.npc[num1340].position.X)
                    {
                        npc.velocity.X -= 0.05f;
                    }
                    else
                    {
                        npc.velocity.X += 0.05f;
                    }
                    if (npc.position.Y < Main.npc[num1340].position.Y)
                    {
                        npc.velocity.Y -= 0.05f;
                    }
                    else
                    {
                        npc.velocity.Y += 0.05f;
                    }
                }
            }
        }
        else
        {
            if (npc.type != 421)
            {
                return;
            }
            npc.hide = npc.ai[0] == 5f;
            npc.rotation = npc.velocity.X * 0.1f;
            for (int num1341 = 0; num1341 < 200; num1341++)
            {
                if (num1341 != npc.whoAmI && Main.npc[num1341].active && Main.npc[num1341].type == npc.type && Math.Abs(npc.position.X - Main.npc[num1341].position.X) + Math.Abs(npc.position.Y - Main.npc[num1341].position.Y) < npc.width)
                {
                    if (npc.position.X < Main.npc[num1341].position.X)
                    {
                        npc.velocity.X -= 0.05f;
                    }
                    else
                    {
                        npc.velocity.X += 0.05f;
                    }
                    if (npc.position.Y < Main.npc[num1341].position.Y)
                    {
                        npc.velocity.Y -= 0.05f;
                    }
                    else
                    {
                        npc.velocity.Y += 0.05f;
                    }
                }
            }
        }
    }
}
