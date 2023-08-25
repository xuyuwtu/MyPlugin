namespace VBY.NPCTest;

partial class NPCAIs
{
    public static void AI_121_QueenSlime(NPC npc)
    {
        int num = 30;
        int num2 = 40;
        _ = Main.expertMode;
        float num3 = 1f;
        bool flag = false;
        bool flag2 = npc.life <= npc.lifeMax / 2;
        if (npc.localAI[0] == 0f)
        {
            npc.ai[1] = -100f;
            npc.localAI[0] = npc.lifeMax;
            npc.TargetClosest();
            npc.netUpdate = true;
        }
        Lighting.AddLight(npc.Center, 1f, 0.7f, 0.9f);
        int num4 = 500;
        if (Main.player[npc.target].dead || Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) / 16f > num4)
        {
            npc.TargetClosest();
            if (Main.player[npc.target].dead || Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) / 16f > num4)
            {
                npc.EncourageDespawn(10);
                if (Main.player[npc.target].Center.X < npc.Center.X)
                {
                    npc.direction = 1;
                }
                else
                {
                    npc.direction = -1;
                }
            }
        }
        if (!Main.player[npc.target].dead && npc.timeLeft > 10 && !flag2 && npc.ai[3] >= 300f && npc.ai[0] == 0f && npc.velocity.Y == 0f)
        {
            npc.ai[0] = 2f;
            npc.ai[1] = 0f;
            if (Main.netMode != 1)
            {
                npc.netUpdate = true;
                npc.TargetClosest(faceTarget: false);
                Point point = npc.Center.ToTileCoordinates();
                Point point2 = Main.player[npc.target].Center.ToTileCoordinates();
                Vector2 vector = Main.player[npc.target].Center - npc.Center;
                int num5 = 10;
                int num6 = 0;
                int num7 = 7;
                int num8 = 0;
                bool flag3 = false;
                if (npc.ai[3] >= 360f || vector.Length() > 2000f)
                {
                    if (npc.ai[3] > 360f)
                    {
                        npc.ai[3] = 360f;
                    }
                    flag3 = true;
                    num8 = 100;
                }
                while (!flag3 && num8 < 100)
                {
                    num8++;
                    int num9 = Main.rand.Next(point2.X - num5, point2.X + num5 + 1);
                    int num10 = Main.rand.Next(point2.Y - num5, point2.Y + 1);
                    if ((num10 >= point2.Y - num7 && num10 <= point2.Y + num7 && num9 >= point2.X - num7 && num9 <= point2.X + num7) || (num10 >= point.Y - num6 && num10 <= point.Y + num6 && num9 >= point.X - num6 && num9 <= point.X + num6) || Main.tile[num9, num10].nactive())
                    {
                        continue;
                    }
                    int num11 = num10;
                    int i = 0;
                    if (Main.tile[num9, num11].nactive() && Main.tileSolid[Main.tile[num9, num11].type] && !Main.tileSolidTop[Main.tile[num9, num11].type])
                    {
                        i = 1;
                    }
                    else
                    {
                        for (; i < 150 && num11 + i < Main.maxTilesY; i++)
                        {
                            int y = num11 + i;
                            if (Main.tile[num9, y].nactive() && Main.tileSolid[Main.tile[num9, y].type] && !Main.tileSolidTop[Main.tile[num9, y].type])
                            {
                                i--;
                                break;
                            }
                        }
                    }
                    num10 += i;
                    bool flag4 = true;
                    if (flag4 && Main.tile[num9, num10].lava())
                    {
                        flag4 = false;
                    }
                    if (flag4 && !Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                    {
                        flag4 = false;
                    }
                    if (flag4)
                    {
                        npc.localAI[1] = num9 * 16 + 8;
                        npc.localAI[2] = num10 * 16 + 16;
                        //flag3 = true;
                        break;
                    }
                }
                if (num8 >= 100)
                {
                    Vector2 bottom = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].Bottom;
                    npc.localAI[1] = bottom.X;
                    npc.localAI[2] = bottom.Y;
                    npc.ai[3] = 0f;
                }
            }
        }
        if (!flag2 && (!Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0) || Math.Abs(npc.Top.Y - Main.player[npc.target].Bottom.Y) > 320f))
        {
            npc.ai[3] += 1.5f;
        }
        else
        {
            float num12 = npc.ai[3];
            npc.ai[3] -= 1f;
            if (npc.ai[3] < 0f)
            {
                if (Main.netMode != 1 && num12 > 0f)
                {
                    npc.netUpdate = true;
                }
                npc.ai[3] = 0f;
            }
        }
        if (npc.timeLeft <= 10 && ((flag2 && npc.ai[0] != 0f) || (!flag2 && npc.ai[0] != 3f)))
        {
            if (flag2)
            {
                npc.ai[0] = 0f;
            }
            else
            {
                npc.ai[0] = 3f;
            }
            npc.ai[1] = 0f;
            npc.ai[2] = 0f;
            npc.ai[3] = 0f;
            npc.netUpdate = true;
        }
        npc.noTileCollide = false;
        npc.noGravity = false;
        if (flag2)
        {
            npc.localAI[3] += 1f;
            if (npc.localAI[3] >= 24f)
            {
                npc.localAI[3] = 0f;
            }
            if (npc.ai[0] == 4f && npc.ai[2] == 1f)
            {
                npc.localAI[3] = 6f;
            }
            if (npc.ai[0] == 5f && npc.ai[2] != 1f)
            {
                npc.localAI[3] = 7f;
            }
        }
        switch ((int)npc.ai[0])
        {
            case 0:
                {
                    if (flag2)
                    {
                        npc.AI_121_QueenSlime_FlyMovement();
                    }
                    else
                    {
                        npc.noTileCollide = false;
                        npc.noGravity = false;
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity.X *= 0.8f;
                            if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
                            {
                                npc.velocity.X = 0f;
                            }
                        }
                    }
                    if (npc.timeLeft <= 10 || (!flag2 && npc.velocity.Y != 0f))
                    {
                        break;
                    }
                    npc.ai[1] += 1f;
                    int num17 = 60;
                    if (flag2)
                    {
                        num17 = 120;
                    }
                    if (!(npc.ai[1] > num17))
                    {
                        break;
                    }
                    npc.ai[1] = 0f;
                    if (flag2)
                    {
                        Player player = Main.player[npc.target];
                        int num18 = Main.rand.Next(2);
                        if (num18 != 1)
                        {
                            npc.ai[0] = 4f;
                        }
                        else
                        {
                            npc.ai[0] = 5f;
                        }
                        if (npc.ai[0] == 4f)
                        {
                            npc.ai[2] = 1f;
                            if (player != null && player.active && !player.dead && (player.Bottom.Y < npc.Bottom.Y || Math.Abs(player.Center.X - npc.Center.X) > 250f))
                            {
                                npc.ai[0] = 5f;
                                npc.ai[2] = 0f;
                            }
                        }
                    }
                    else
                    {
                        npc.ai[0] = Main.rand.Next(3) switch
                        {
                            1 => 4f,
                            2 => 5f,
                            _ => 3f,
                        };
                    }
                    npc.netUpdate = true;
                    break;
                }
            case 1:
                {
                    npc.rotation = 0f;
                    npc.ai[1] += 1f;
                    num3 = MathHelper.Clamp(npc.ai[1] / 30f, 0f, 1f);
                    num3 = 0.5f + num3 * 0.5f;
                    if (npc.ai[1] >= 30f && Main.netMode != 1)
                    {
                        npc.ai[0] = 0f;
                        npc.ai[1] = 0f;
                        npc.netUpdate = true;
                        npc.TargetClosest();
                    }
                    if (Main.netMode == 1 && npc.ai[1] >= 60f)
                    {
                        npc.ai[0] = 0f;
                        npc.ai[1] = 0f;
                        npc.TargetClosest();
                    }
                    Color newColor2 = NPC.AI_121_QueenSlime_GetDustColor();
                    newColor2.A = 150;
                    for (int num26 = 0; num26 < 10; num26++)
                    {
                        int num27 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 4, npc.velocity.X, npc.velocity.Y, 50, newColor2, 1.5f);
                        Main.dust[num27].noGravity = true;
                        Main.dust[num27].velocity *= 2f;
                    }
                    break;
                }
            case 2:
                npc.rotation = 0f;
                npc.ai[1] += 1f;
                num3 = MathHelper.Clamp((60f - npc.ai[1]) / 60f, 0f, 1f);
                num3 = 0.5f + num3 * 0.5f;
                if (npc.ai[1] >= 60f)
                {
                    flag = true;
                }
                if (npc.ai[1] == 60f)
                {
                    Gore.NewGore(npc.Center + new Vector2(-40f, -npc.height / 2), npc.velocity, 1258);
                }
                if (npc.ai[1] >= 60f && Main.netMode != 1)
                {
                    npc.Bottom = new Vector2(npc.localAI[1], npc.localAI[2]);
                    npc.ai[0] = 1f;
                    npc.ai[1] = 0f;
                    npc.netUpdate = true;
                }
                if (Main.netMode == 1 && npc.ai[1] >= 120f)
                {
                    npc.ai[0] = 1f;
                    npc.ai[1] = 0f;
                }
                if (!flag)
                {
                    Color newColor = NPC.AI_121_QueenSlime_GetDustColor();
                    newColor.A = 150;
                    for (int n = 0; n < 10; n++)
                    {
                        int num25 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 4, npc.velocity.X, npc.velocity.Y, 50, newColor, 1.5f);
                        Main.dust[num25].noGravity = true;
                        Main.dust[num25].velocity *= 0.5f;
                    }
                }
                break;
            case 3:
                npc.rotation = 0f;
                if (npc.velocity.Y == 0f)
                {
                    npc.velocity.X *= 0.8f;
                    if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
                    {
                        npc.velocity.X = 0f;
                    }
                    npc.ai[1] += 4f;
                    if (npc.life < npc.lifeMax * 0.66)
                    {
                        npc.ai[1] += 4f;
                    }
                    if (npc.life < npc.lifeMax * 0.33)
                    {
                        npc.ai[1] += 4f;
                    }
                    if (!(npc.ai[1] >= 0f))
                    {
                        break;
                    }
                    npc.netUpdate = true;
                    npc.TargetClosest();
                    if (npc.ai[2] == 3f)
                    {
                        npc.velocity.Y = -13f;
                        npc.velocity.X += 3.5f * npc.direction;
                        npc.ai[1] = 0f;
                        npc.ai[2] = 0f;
                        if (npc.timeLeft > 10)
                        {
                            npc.ai[0] = 0f;
                        }
                        else
                        {
                            npc.ai[1] = -60f;
                        }
                    }
                    else if (npc.ai[2] == 2f)
                    {
                        npc.velocity.Y = -6f;
                        npc.velocity.X += 4.5f * npc.direction;
                        npc.ai[1] = -40f;
                        npc.ai[2] += 1f;
                    }
                    else
                    {
                        npc.velocity.Y = -8f;
                        npc.velocity.X += 4f * npc.direction;
                        npc.ai[1] = -40f;
                        npc.ai[2] += 1f;
                    }
                }
                else
                {
                    if (npc.target >= 255)
                    {
                        break;
                    }
                    float num19 = 3f;
                    if (Main.getGoodWorld)
                    {
                        num19 = 7f;
                    }
                    if ((npc.direction == 1 && npc.velocity.X < num19) || (npc.direction == -1 && npc.velocity.X > 0f - num19))
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
                break;
            case 4:
                {
                    npc.rotation *= 0.9f;
                    npc.noTileCollide = true;
                    npc.noGravity = true;
                    if (npc.ai[2] == 1f)
                    {
                        npc.noTileCollide = false;
                        npc.noGravity = false;
                        int num20 = 30;
                        if (flag2)
                        {
                            num20 = 10;
                        }
                        if (Main.getGoodWorld)
                        {
                            num20 = 0;
                        }
                        Player player2 = Main.player[npc.target];
                        _ = npc.Center;
                        if (!player2.dead && player2.active && Math.Abs(npc.Center.X - player2.Center.X) / 16f <= num4)
                        {
                            _ = player2.Center;
                        }
                        if (npc.velocity.Y == 0f)
                        {
                            npc.ai[0] = 0f;
                            npc.ai[1] = 0f;
                            npc.ai[2] = 0f;
                            npc.netUpdate = true;
                            SoundEngine.PlaySound(SoundID.Item167, npc.Center);
                            if (Main.netMode != 1)
                            {
                                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Bottom, Vector2.Zero, 922, num2, 0f, Main.myPlayer);

                                float num64 = (float)Math.PI * 2f * Main.rand.NextFloat();
                                for (float num65 = 0f; num65 < 1f; num65 += 1f / 13f)
                                {
                                    Vector2 vector22 = Vector2.UnitY.RotatedBy((float)Math.PI / 2f + (float)Math.PI * 2f * num65 + num64);
                                    if (Main.netMode != 1)
                                    {
                                        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center + vector22.RotatedBy(-1.5707963705062866) * 30f, vector22 * 8f, 872, npc.damage / 7, 0f, Main.myPlayer, 0f, num65);
                                    }
                                }
                            }
                            //for (int l = 0; l < 20; l++)
                            //{
                            //    int num21 = Dust.NewDust(npc.Bottom - new Vector2(npc.width / 2, 30f), npc.width, 30, 31, npc.velocity.X, npc.velocity.Y, 40, NPC.AI_121_QueenSlime_GetDustColor());
                            //    Main.dust[num21].noGravity = true;
                            //    Main.dust[num21].velocity.Y = -5f + Main.rand.NextFloat() * -3f;
                            //    Main.dust[num21].velocity.X *= 7f;
                            //}
                        }
                        //else if (npc.ai[1] >= num20)
                        //{
                        //    for (int m = 0; m < 4; m++)
                        //    {
                        //        Vector2 vector3 = npc.Bottom - new Vector2(Main.rand.NextFloatDirection() * 16f, Main.rand.Next(8));
                        //        int num22 = Dust.NewDust(vector3, 2, 2, 31, npc.velocity.X, npc.velocity.Y, 40, NPC.AI_121_QueenSlime_GetDustColor(), 1.4f);
                        //        Main.dust[num22].position = vector3;
                        //        Main.dust[num22].noGravity = true;
                        //        Main.dust[num22].velocity.Y = npc.velocity.Y * 0.9f;
                        //        Main.dust[num22].velocity.X = ((Main.rand.Next(2) == 0) ? (-10f) : 10f) + Main.rand.NextFloatDirection() * 3f;
                        //    }
                        //}
                        npc.velocity.X *= 0.8f;
                        float num23 = npc.ai[1];
                        npc.ai[1] += 1f;
                        if (npc.ai[1] >= num20)
                        {
                            if (num23 < num20)
                            {
                                npc.netUpdate = true;
                            }
                            if (flag2 && npc.ai[1] > num20 + 120)
                            {
                                npc.ai[0] = 0f;
                                npc.ai[1] = 0f;
                                npc.ai[2] = 0f;
                                npc.velocity.Y *= 0.8f;
                                npc.netUpdate = true;
                                break;
                            }
                            npc.velocity.Y += 1f;
                            float num24 = 14f;
                            if (Main.getGoodWorld)
                            {
                                npc.velocity.Y += 1f;
                                num24 = 15.99f;
                            }
                            if (npc.velocity.Y == 0f)
                            {
                                npc.velocity.Y = 0.01f;
                            }
                            if (npc.velocity.Y >= num24)
                            {
                                npc.velocity.Y = num24;
                            }
                        }
                        else
                        {
                            npc.velocity.Y *= 0.8f;
                        }
                        break;
                    }
                    if (Main.netMode != 1 && npc.ai[1] == 0f)
                    {
                        npc.TargetClosest();
                        npc.netUpdate = true;
                    }
                    npc.ai[1] += 1f;
                    if (!(npc.ai[1] >= 30f))
                    {
                        break;
                    }
                    if (npc.ai[1] >= 60f)
                    {
                        npc.ai[1] = 60f;
                        if (Main.netMode != 1)
                        {
                            npc.ai[1] = 0f;
                            npc.ai[2] = 1f;
                            npc.velocity.Y = -3f;
                            npc.netUpdate = true;
                        }
                    }
                    Player player3 = Main.player[npc.target];
                    Vector2 center = npc.Center;
                    if (!player3.dead && player3.active && Math.Abs(npc.Center.X - player3.Center.X) / 16f <= num4)
                    {
                        center = player3.Center;
                    }
                    center.Y -= 384f;
                    if (npc.velocity.Y == 0f)
                    {
                        npc.velocity = center - npc.Center;
                        npc.velocity = npc.velocity.SafeNormalize(Vector2.Zero);
                        npc.velocity *= 20f;
                    }
                    else
                    {
                        npc.velocity.Y *= 0.95f;
                    }
                    break;
                }
            case 5:
                {
                    npc.rotation *= 0.9f;
                    npc.noTileCollide = true;
                    npc.noGravity = true;
                    if (flag2)
                    {
                        npc.ai[3] = 0f;
                    }
                    if (npc.ai[2] == 1f)
                    {
                        npc.ai[1] += 1f;
                        if (!(npc.ai[1] >= 10f))
                        {
                            break;
                        }
                        if (Main.netMode != 1)
                        {
                            int num13 = 10;
                            if (Main.getGoodWorld)
                            {
                                num13 = 15;
                            }
                            int num14 = num13;
                            if (!flag2)
                            {
                                num14 = 6;
                            }
                            for (int j = 0; j < num14; j++)
                            {
                                Vector2 spinningpoint = new(9f, 0f);
                                spinningpoint = spinningpoint.RotatedBy(-j * ((float)Math.PI * 2f) / num13, Vector2.Zero);
                                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X, npc.Center.Y, spinningpoint.X, spinningpoint.Y, 926, num, 0f, Main.myPlayer);
                            }
                        }
                        npc.ai[0] = 0f;
                        npc.ai[1] = 0f;
                        npc.ai[2] = 0f;
                        npc.netUpdate = true;
                        break;
                    }
                    if (Main.netMode != 1 && npc.ai[1] == 0f)
                    {
                        npc.TargetClosest();
                        npc.netUpdate = true;
                    }
                    npc.ai[1] += 1f;
                    if (npc.ai[1] >= 50f)
                    {
                        npc.ai[1] = 50f;
                        if (Main.netMode != 1)
                        {
                            npc.ai[1] = 0f;
                            npc.ai[2] = 1f;
                            npc.netUpdate = true;
                        }
                    }
                    float num15 = 100f;
                    for (int k = 0; k < 4; k++)
                    {
                        Vector2 vector2 = npc.Center + Main.rand.NextVector2CircularEdge(num15, num15);
                        if (!flag2)
                        {
                            vector2 += new Vector2(0f, 20f);
                        }
                        Vector2 v = vector2 - npc.Center;
                        v = v.SafeNormalize(Vector2.Zero) * -8f;
                        int num16 = Dust.NewDust(vector2, 2, 2, 31, v.X, v.Y, 40, NPC.AI_121_QueenSlime_GetDustColor(), 1.8f);
                        Main.dust[num16].position = vector2;
                        Main.dust[num16].noGravity = true;
                        Main.dust[num16].alpha = 250;
                        Main.dust[num16].velocity = v;
                        Main.dust[num16].customData = npc;
                    }
                    if (flag2)
                    {
                        npc.AI_121_QueenSlime_FlyMovement();
                    }
                    break;
                }
        }
        npc.dontTakeDamage = (npc.hide = flag);
        if (num3 != npc.scale)
        {
            npc.position.X += npc.width / 2;
            npc.position.Y += npc.height;
            npc.scale = num3;
            npc.width = (int)(114f * npc.scale);
            npc.height = (int)(100f * npc.scale);
            npc.position.X -= npc.width / 2;
            npc.position.Y -= npc.height;
        }
        if (npc.life <= 0)
        {
            return;
        }
        if (Main.rand.Next(360) == 0)
        {
            SoundEngine.PlaySound(65, npc.position);
        }
        if (Main.netMode == 1)
        {
            return;
        }
        if (npc.localAI[0] >= npc.lifeMax / 2 && npc.life < npc.lifeMax / 2)
        {
            npc.localAI[0] = npc.life;
            npc.ai[0] = 0f;
            npc.ai[1] = 0f;
            npc.ai[2] = 0f;
            npc.netUpdate = true;
        }
        int num28 = (int)(npc.lifeMax * 0.02f);
        if (flag2)
        {
            num28 = (int)(npc.lifeMax * 0.015f);
        }
        if (!(npc.life + num28 < npc.localAI[0]))
        {
            return;
        }
        npc.localAI[0] = npc.life;
        int num29 = Main.rand.Next(1, 3);
        for (int num30 = 0; num30 < num29; num30++)
        {
            int x = (int)(npc.position.X + Main.rand.Next(npc.width - 32));
            int y2 = (int)(npc.position.Y + Main.rand.Next(npc.height - 32));
            int newNpcType = 658;
            switch (Main.rand.Next(3))
            {
                case 0:
                    newNpcType = 658;
                    break;
                case 1:
                    newNpcType = 659;
                    break;
                case 2:
                    newNpcType = 660;
                    break;
            }
            int num32 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), x, y2, newNpcType);
            Main.npc[num32].SetDefaults(newNpcType);
            Main.npc[num32].velocity.X = Main.rand.Next(-15, 16) * 0.1f;
            Main.npc[num32].velocity.Y = Main.rand.Next(-30, 1) * 0.1f;
            Main.npc[num32].ai[0] = -500 * Main.rand.Next(3);
            Main.npc[num32].ai[1] = 0f;
            if (Main.netMode == 2 && num32 < 200)
            {
                NetMessage.SendData(23, -1, -1, null, num32);
            }
        }
    }

}
