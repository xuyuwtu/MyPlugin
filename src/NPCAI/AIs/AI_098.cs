namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_098(this NPC npc)
    {
        npc.noTileCollide = false;
        if (npc.ai[0] == 0f)
        {
            npc.TargetClosest();
            npc.ai[0] = 1f;
            npc.ai[1] = 0f;
            npc.ai[2] = 0f;
            npc.ai[3] = 0f;
        }
        bool flag95 = Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].position, 1, 1);
        bool flag96 = true;
        if (!flag95 || Main.player[npc.target].dead)
        {
            flag96 = false;
        }
        else
        {
            int num1479 = (int)(Main.player[npc.target].Center.X / 16f);
            int num1480 = (int)(Main.player[npc.target].Center.Y / 16f);
            for (int num1481 = num1479 - 2; num1481 <= num1479 + 2; num1481++)
            {
                for (int num1482 = num1480; num1482 <= num1480 + 25; num1482++)
                {
                    if (WorldGen.SolidTile2(num1481, num1482))
                    {
                        flag96 = false;
                    }
                }
            }
        }
        if (npc.ai[0] < 0f)
        {
            Vector2 vector293 = Main.player[npc.target].Center - npc.Center;
            float num1483 = vector293.Length();
            if (npc.ai[0] == -1f)
            {
                vector293.Normalize();
                if (vector293.HasNaNs())
                {
                    vector293 = new Vector2(npc.direction, 0f);
                }
                float num1484 = 8f + num1483 / 100f;
                float num1485 = 12f;
                if (Main.player[npc.target].velocity.Length() > num1485)
                {
                    num1485 = Main.player[npc.target].velocity.Length();
                }
                if (num1484 > num1485)
                {
                    num1484 = num1485;
                }
                vector293 *= num1484;
                float num1486 = 10f;
                npc.velocity = (npc.velocity * (num1486 - 1f) + vector293) / num1486;
                for (int num1487 = 0; num1487 < 200; num1487++)
                {
                    if (Main.npc[num1487].active && Main.npc[num1487].type == npc.type && num1487 != npc.whoAmI)
                    {
                        Vector2 vector294 = Main.npc[num1487].Center - npc.Center;
                        if (vector294.Length() < 40f)
                        {
                            vector294.Normalize();
                            vector294 *= 1f;
                            npc.velocity -= vector294;
                        }
                    }
                }
                npc.rotation += npc.velocity.X * 0.03f;
                if (npc.rotation < -6.2831)
                {
                    npc.rotation += 6.2831f;
                }
                if (npc.rotation > 6.2831)
                {
                    npc.rotation -= 6.2831f;
                }
                if (npc.velocity.X > 0f)
                {
                    npc.direction = 1;
                }
                else if (npc.velocity.X < 0f)
                {
                    npc.direction = -1;
                }
                npc.spriteDirection = npc.direction;
            }
            npc.ai[1] += 1f;
            if (npc.ai[1] >= 60f && !flag96)
            {
                npc.ai[0] = 0f;
            }
        }
        else if (npc.ai[0] == 2f)
        {
            npc.rotation *= 0.92f;
            if ((double)Math.Abs(npc.rotation) < 0.02)
            {
                npc.rotation = 0f;
            }
            int num1488 = 300;
            float num1489 = Math.Abs(npc.Center.X - Main.player[npc.target].Center.X);
            if (num1489 < num1488 && Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].position, 1, 1))
            {
                npc.velocity.X *= 0.96f;
                npc.velocity.Y *= 0.96f;
                npc.ai[1] += 1f;
                if (npc.ai[1] == 20f)
                {
                    if (Main.netMode != 1)
                    {
                        _ = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X, (int)npc.Center.Y + 26, 516, 0, 0f, 0f, 0f, 0f, npc.target);
                    }
                }
                else if (npc.ai[1] >= 30f)
                {
                    npc.ai[1] = 0f;
                }
                for (int num1491 = 0; num1491 < 200; num1491++)
                {
                    if (Main.npc[num1491].active && Main.npc[num1491].type == npc.type && num1491 != npc.whoAmI)
                    {
                        Vector2 vector295 = Main.npc[num1491].Center - npc.Center;
                        if (vector295.Length() < 100f)
                        {
                            vector295.Normalize();
                            vector295 *= 0.1f;
                            npc.velocity -= vector295;
                        }
                    }
                }
            }
            else
            {
                npc.ai[0] = 0f;
            }
            if (Main.player[npc.target].Center.X < npc.Center.X)
            {
                npc.direction = -1;
            }
            else if (Main.player[npc.target].Center.X > npc.Center.X)
            {
                npc.direction = 1;
            }
            npc.spriteDirection = npc.direction;
        }
        if (npc.ai[0] != 1f)
        {
            return;
        }
        npc.rotation *= 0.92f;
        if ((double)Math.Abs(npc.rotation) < 0.02)
        {
            npc.rotation = 0f;
        }
        if (flag96)
        {
            npc.ai[0] = -1f;
            npc.ai[1] = 0f;
            npc.ai[2] = 0f;
            npc.ai[3] = 0f;
        }
        int num1492 = 300;
        for (int num1493 = 0; num1493 < 200; num1493++)
        {
            if (Main.npc[num1493].active && Main.npc[num1493].type == npc.type && num1493 != npc.whoAmI)
            {
                Vector2 vector296 = Main.npc[num1493].Center - npc.Center;
                if (vector296.Length() < 50f)
                {
                    vector296.Normalize();
                    vector296 *= 0.1f;
                    npc.velocity -= vector296;
                    npc.velocity.X -= vector296.X * 1f;
                }
            }
        }
        int num1494 = 800;
        float num1495 = Math.Abs(npc.Center.X - Main.player[npc.target].Center.X);
        if (num1495 < num1492 && flag95)
        {
            npc.ai[0] = 2f;
            npc.ai[1] = 0f;
        }
        else
        {
            if (npc.collideX)
            {
                npc.velocity.X *= -0.5f;
                npc.ai[1] = 60f;
                npc.direction *= -1;
            }
            if (npc.ai[1] > 0f)
            {
                npc.ai[1] -= 1f;
            }
            else if (flag95)
            {
                if (npc.Center.X > Main.player[npc.target].Center.X)
                {
                    npc.direction = -1;
                }
                else
                {
                    npc.direction = 1;
                }
            }
            else if (num1495 > num1494)
            {
                if (npc.Center.X > Main.player[npc.target].Center.X)
                {
                    npc.direction = -1;
                }
                else
                {
                    npc.direction = 1;
                }
            }
            float num1496 = 2f;
            float num1497 = 0.1f;
            if (npc.velocity.X > num1496 || npc.velocity.X < 0f - num1496)
            {
                if (Math.Abs(npc.velocity.X) < num1496 + num1497 * 2f)
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X = 0f - num1496;
                    }
                    else
                    {
                        npc.velocity.X = num1496;
                    }
                }
                else
                {
                    npc.velocity.X *= 0.99f;
                }
            }
            else
            {
                npc.velocity.X += npc.direction * num1497;
            }
            npc.spriteDirection = npc.direction;
        }
        if (npc.collideY)
        {
            npc.ai[2] = 60f;
            npc.directionY *= -1;
            npc.velocity.Y *= -0.5f;
        }
        if (npc.ai[2] > 0f)
        {
            npc.ai[2] -= 1f;
        }
        else
        {
            int num1498 = (int)(npc.Center.Y / 16f);
            int num1499 = (int)((npc.Center.X - 8f) / 16f);
            int num1500 = 30;
            int num1501 = 15;
            int num1502 = 0;
            for (int num1503 = num1498; num1503 < num1498 + num1500; num1503++)
            {
                for (int num1504 = num1499; num1504 <= num1499 + 1; num1504++)
                {
                    if (WorldGen.SolidTile(num1504, num1503) || Main.tile[num1504, num1503].liquid > 0)
                    {
                        num1502 = num1503 - num1498;
                        break;
                    }
                }
                if (num1502 != 0)
                {
                    break;
                }
            }
            if (num1502 == 0)
            {
                npc.directionY = 1;
            }
            else if (num1502 < num1501)
            {
                npc.directionY = -1;
            }
        }
        float num1505 = 2f;
        float num1506 = 0.1f;
        if (npc.velocity.Y > num1505 || npc.velocity.Y < 0f - num1505)
        {
            if (Math.Abs(npc.velocity.Y) < num1505 + num1506 * 2f)
            {
                if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y = 0f - num1505;
                }
                else
                {
                    npc.velocity.Y = num1505;
                }
            }
            else
            {
                npc.velocity.Y *= 0.99f;
            }
        }
        else
        {
            npc.velocity.Y += npc.directionY * num1506;
        }
    }
}
