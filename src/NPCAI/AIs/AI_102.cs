using OTAPI;

namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_102(this NPC npc)
    {
        bool flag98 = false;
        bool flag99 = false;
        bool flag100 = true;
        bool flag101 = false;
        int num1521 = 4;
        int num1522 = 3;
        int num1523 = 0;
        float num1524 = 0.2f;
        float num1525 = 2f;
        float num1526 = -0.2f;
        float num1527 = -4f;
        bool flag102 = true;
        float num1528 = 2f;
        float num1529 = 0.1f;
        float num1530 = 1f;
        float num1531 = 0.04f;
        bool flag103 = false;
        float num1532 = 0.96f;
        bool flag104 = true;
        if (npc.type == 541)
        {
            flag102 = false;
            npc.rotation = npc.velocity.X * 0.04f;
            npc.spriteDirection = ((npc.direction > 0) ? 1 : (-1));
            num1523 = 3;
            num1526 = -0.1f;
            num1524 = 0.1f;
            float num1533 = npc.life / (float)npc.lifeMax;
            num1528 += (1f - num1533) * 2f;
            num1529 += (1f - num1533) * 0.02f;
            if (num1533 < 0.5f)
            {
                npc.knockBackResist = 0f;
            }
            npc.position += npc.netOffset;
            Vector2 vector301 = npc.BottomLeft + new Vector2(0f, -12f);
            Vector2 bottomRight = npc.BottomRight;
            Vector2 vector302 = new(-npc.spriteDirection * 10, -4f);
            Color color = new Color(222, 108, 48) * 0.7f;
            float num1534 = -0.3f + MathHelper.Max(npc.velocity.Y * 2f, 0f);
            for (int num1535 = 0; num1535 < 2; num1535++)
            {
                if (Main.rand.Next(2) != 0)
                {
                    Dust dust24 = Main.dust[Dust.NewDust(npc.Bottom, 0, 0, 268)];
                    dust24.position = new Vector2(MathHelper.Lerp(vector301.X, bottomRight.X, Main.rand.NextFloat()), MathHelper.Lerp(vector301.Y, bottomRight.Y, Main.rand.NextFloat())) + vector302;
                    if (num1535 == 1)
                    {
                        dust24.position = npc.Bottom + Utils.RandomVector2(Main.rand, -6f, 6f);
                    }
                    dust24.color = color;
                    dust24.scale = 0.8f;
                    dust24.velocity.Y += num1534;
                    dust24.velocity.X += npc.spriteDirection * 0.2f;
                }
            }
            npc.position -= npc.netOffset;
            npc.localAI[2] = 0f;
            if (npc.ai[0] < 0f)
            {
                npc.ai[0] = MathHelper.Min(npc.ai[0] + 1f, 0f);
            }
            if (npc.ai[0] > 0f)
            {
                flag104 = false;
                flag103 = true;
                npc.ai[0]++;
                if (npc.ai[0] >= 135f)
                {
                    npc.ai[0] = -300f;
                    npc.netUpdate = true;
                }

                _ = npc.Center + Vector2.UnitX * npc.direction * 200f;
                Vector2 vector304 = npc.Center + Vector2.UnitX * npc.direction * 50f - Vector2.UnitY * 6f;
                if (npc.ai[0] == 54f && Main.netMode != 1)
                {
                    List<Point> list = new();
                    Vector2 vector305 = Main.player[npc.target].Center + new Vector2(Main.player[npc.target].velocity.X * 30f, 0f);
                    if (npc.Distance(vector305) < 2000f)
                    {
                        Point point16 = vector305.ToTileCoordinates();
                        for (int num1536 = 0; num1536 < 1000; num1536++)
                        {
                            if (list.Count >= 3)
                            {
                                break;
                            }
                            bool flag105 = false;
                            int num1537 = Main.rand.Next(point16.X - 30, point16.X + 30 + 1);
                            foreach (Point item in list)
                            {
                                if (Math.Abs(item.X - num1537) < 10)
                                {
                                    flag105 = true;
                                    break;
                                }
                            }
                            if (!flag105)
                            {
                                int startY = point16.Y - 20;
                                Collision.ExpandVertically(num1537, startY, out var _, out var bottomY, 1, 51);
                                if (StrayMethods.CanSpawnSandstormHostile(new Vector2(num1537, bottomY - 15) * 16f, 15, 15))
                                {
                                    list.Add(new Point(num1537, bottomY - 15));
                                }
                            }
                        }
                        foreach (Point item2 in list)
                        {
                            Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), item2.X * 16, item2.Y * 16, 0f, 0f, 658, 0, 0f, Main.myPlayer);
                        }
                    }
                    else
                    {
                        npc.ai[0] = -200f;
                        npc.netUpdate = true;
                    }
                }

                _ = new Vector2(0.9f, 2f);
                if (npc.ai[0] < 114f && npc.ai[0] > 0f)
                {
                    List<Vector2> list2 = new();
                    for (int num1538 = 0; num1538 < 1000; num1538++)
                    {
                        Projectile projectile12 = Main.projectile[num1538];
                        if (projectile12.active && projectile12.type == 658)
                        {
                            list2.Add(projectile12.Center);
                        }
                    }
                    Vector2 vector307 = new(0f, 1500f);
                    float num1539 = (npc.ai[0] - 54f) / 30f;
                    if (num1539 < 0.95f && num1539 >= 0f)
                    {
                        foreach (Vector2 item3 in list2)
                        {
                            Vector2 value13 = Vector2.CatmullRom(vector304 + vector307, vector304, item3, item3 + vector307, num1539);
                            Vector2 value14 = Vector2.CatmullRom(vector304 + vector307, vector304, item3, item3 + vector307, num1539 + 0.05f);
                            float num1540 = num1539;
                            if (num1540 > 0.5f)
                            {
                                _ = 1f - num1540;
                            }
                            float num1541 = 2f;
                            if (Vector2.Distance(value13, value14) > 5f)
                            {
                                num1541 = 3f;
                            }
                            if (Vector2.Distance(value13, value14) > 10f)
                            {
                                num1541 = 4f;
                            }
                            for (float num1542 = 0f; num1542 < num1541; num1542++)
                            {
                                Dust dust25 = Main.dust[Dust.NewDust(vector304, 0, 0, 269)];
                                dust25.position = Vector2.Lerp(value13, value14, num1542 / num1541) + Utils.RandomVector2(Main.rand, -2f, 2f);
                                dust25.noLight = true;
                                dust25.scale = 0.3f + num1539;
                            }
                        }
                    }
                }
                _ = npc.ai[0];
                _ = 100f;
            }
            if (npc.ai[0] == 0f)
            {
                npc.ai[0] = 1f;
                npc.netUpdate = true;
                flag103 = true;
            }
        }
        if (npc.justHit)
        {
            npc.localAI[2] = 0f;
        }
        if (!flag99)
        {
            if (npc.localAI[2] >= 0f)
            {
                float num1543 = 16f;
                bool flag106 = false;
                bool flag107 = false;
                if (npc.position.X > npc.localAI[0] - num1543 && npc.position.X < npc.localAI[0] + num1543)
                {
                    flag106 = true;
                }
                else if ((npc.velocity.X < 0f && npc.direction > 0) || (npc.velocity.X > 0f && npc.direction < 0))
                {
                    flag106 = true;
                    num1543 += 24f;
                }
                if (npc.position.Y > npc.localAI[1] - num1543 && npc.position.Y < npc.localAI[1] + num1543)
                {
                    flag107 = true;
                }
                if (flag106 && flag107)
                {
                    npc.localAI[2] += 1f;
                    if (npc.localAI[2] >= 30f && num1543 == 16f)
                    {
                        flag98 = true;
                    }
                    if (npc.localAI[2] >= 60f)
                    {
                        npc.localAI[2] = -180f;
                        npc.direction *= -1;
                        npc.velocity.X *= -1f;
                        npc.collideX = false;
                    }
                }
                else
                {
                    npc.localAI[0] = npc.position.X;
                    npc.localAI[1] = npc.position.Y;
                    npc.localAI[2] = 0f;
                }
                if (flag104)
                {
                    npc.TargetClosest();
                }
            }
            else
            {
                npc.localAI[2] += 1f;
                npc.direction = ((Main.player[npc.target].Center.X > npc.Center.X) ? 1 : (-1));
            }
        }
        int x25 = (int)((npc.position.X + npc.width / 2) / 16f) + npc.direction * 2;
        int num1544 = (int)((npc.position.Y + npc.height) / 16f);
        int num1545 = (int)npc.Bottom.Y / 16;
        int x26 = (int)npc.Bottom.X / 16;
        if (flag103)
        {
            npc.velocity *= num1532;
            return;
        }
        for (int num1546 = num1544; num1546 < num1544 + num1521; num1546++)
        {
            if (Main.tile[x25, num1546] == null)
            {
                Main.tile[x25, num1546] = Hooks.Tile.InvokeCreate();
            }
            if ((Main.tile[x25, num1546].nactive() && Main.tileSolid[Main.tile[x25, num1546].type]) || Main.tile[x25, num1546].liquid > 0)
            {
                if (num1546 <= num1544 + 1)
                {
                    flag101 = true;
                }
                flag100 = false;
                break;
            }
        }
        for (int num1547 = num1545; num1547 < num1545 + num1523; num1547++)
        {
            if (Main.tile[x26, num1547] == null)
            {
                Main.tile[x26, num1547] = Hooks.Tile.InvokeCreate();
            }
            if ((Main.tile[x26, num1547].nactive() && Main.tileSolid[Main.tile[x26, num1547].type]) || Main.tile[x26, num1547].liquid > 0)
            {
                flag101 = true;
                flag100 = false;
                break;
            }
        }
        if (flag102)
        {
            for (int num1548 = num1544 - num1522; num1548 < num1544; num1548++)
            {
                if (Main.tile[x25, num1548] == null)
                {
                    Main.tile[x25, num1548] = Hooks.Tile.InvokeCreate();
                }
                if ((Main.tile[x25, num1548].nactive() && Main.tileSolid[Main.tile[x25, num1548].type]) || Main.tile[x25, num1548].liquid > 0)
                {
                    flag101 = false;
                    flag98 = true;
                    break;
                }
            }
        }
        if (flag98)
        {
            flag101 = false;
            flag100 = true;
        }
        if (flag100)
        {
            npc.velocity.Y += num1524;
            if (npc.velocity.Y > num1525)
            {
                npc.velocity.Y = num1525;
            }
        }
        else
        {
            if ((npc.directionY < 0 && npc.velocity.Y > 0f) || flag101)
            {
                npc.velocity.Y += num1526;
            }
            if (npc.velocity.Y < num1527)
            {
                npc.velocity.Y = num1527;
            }
        }
        if (npc.collideX)
        {
            npc.velocity.X = npc.oldVelocity.X * -0.4f;
            if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 1f)
            {
                npc.velocity.X = 1f;
            }
            if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -1f)
            {
                npc.velocity.X = -1f;
            }
        }
        if (npc.collideY)
        {
            npc.velocity.Y = npc.oldVelocity.Y * -0.25f;
            if (npc.velocity.Y > 0f && npc.velocity.Y < 1f)
            {
                npc.velocity.Y = 1f;
            }
            if (npc.velocity.Y < 0f && npc.velocity.Y > -1f)
            {
                npc.velocity.Y = -1f;
            }
        }
        if (npc.direction == -1 && npc.velocity.X > 0f - num1528)
        {
            npc.velocity.X -= num1529;
            if (npc.velocity.X > num1528)
            {
                npc.velocity.X -= num1529;
            }
            else if (npc.velocity.X > 0f)
            {
                npc.velocity.X += num1529 / 2f;
            }
            if (npc.velocity.X < 0f - num1528)
            {
                npc.velocity.X = 0f - num1528;
            }
        }
        else if (npc.direction == 1 && npc.velocity.X < num1528)
        {
            npc.velocity.X += num1529;
            if (npc.velocity.X < 0f - num1528)
            {
                npc.velocity.X += num1529;
            }
            else if (npc.velocity.X < 0f)
            {
                npc.velocity.X -= num1529 / 2f;
            }
            if (npc.velocity.X > num1528)
            {
                npc.velocity.X = num1528;
            }
        }
        if (npc.directionY == -1 && npc.velocity.Y > 0f - num1530)
        {
            npc.velocity.Y -= num1531;
            if (npc.velocity.Y > num1530)
            {
                npc.velocity.Y -= num1531 * 1.25f;
            }
            else if (npc.velocity.Y > 0f)
            {
                npc.velocity.Y += num1531 * 0.75f;
            }
            if (npc.velocity.Y < 0f - num1530)
            {
                npc.velocity.Y = 0f - num1528;
            }
        }
        else if (npc.directionY == 1 && npc.velocity.Y < num1530)
        {
            npc.velocity.Y += num1531;
            if (npc.velocity.Y < 0f - num1530)
            {
                npc.velocity.Y += num1531 * 1.25f;
            }
            else if (npc.velocity.Y < 0f)
            {
                npc.velocity.Y -= num1531 * 0.75f;
            }
            if (npc.velocity.Y > num1530)
            {
                npc.velocity.Y = num1530;
            }
        }
    }
}
