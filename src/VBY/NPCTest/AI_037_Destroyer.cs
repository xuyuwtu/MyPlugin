namespace VBY.NPCTest;

partial class NPCAIs
{
    public static void AI_037_Destroyer(NPC npc)
    {
        int num = 0;
        int num2 = 10;
        if (NPC.IsMechQueenUp && npc.type != 134)
        {
            int num3 = (int)npc.ai[1];
            while (num3 > 0 && num3 < 200)
            {
                if (Main.npc[num3].active && Main.npc[num3].type >= 134 && Main.npc[num3].type <= 136)
                {
                    num++;
                    if (Main.npc[num3].type == 134)
                    {
                        break;
                    }
                    if (num >= num2)
                    {
                        num = 0;
                        break;
                    }
                    num3 = (int)Main.npc[num3].ai[1];
                    continue;
                }
                num = 0;
                break;
            }
        }
        if (npc.ai[3] > 0f)
        {
            npc.realLife = (int)npc.ai[3];
        }
        if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
        {
            npc.TargetClosest();
        }
        if (npc.type >= 134 && npc.type <= 136)
        {
            npc.velocity.Length();
            if (npc.type == 134 || (npc.type != 134 && Main.npc[(int)npc.ai[1]].alpha < 128))
            {
                if (npc.alpha != 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int num4 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 182, 0f, 0f, 100, default, 2f);
                        Main.dust[num4].noGravity = true;
                        Main.dust[num4].noLight = true;
                    }
                }
                npc.alpha -= 42;
                if (npc.alpha < 0)
                {
                    npc.alpha = 0;
                }
            }
        }
        if (npc.type > 134)
        {
            bool flag = false;
            if (npc.ai[1] <= 0f)
            {
                flag = true;
            }
            else if (Main.npc[(int)npc.ai[1]].life <= 0)
            {
                flag = true;
            }
            if (flag)
            {
                npc.life = 0;
                npc.HitEffect();
                npc.checkDead();
            }
        }
        if (Main.netMode != 1)
        {
            if (npc.ai[0] == 0f && npc.type == 134)
            {
                npc.ai[3] = npc.whoAmI;
                npc.realLife = npc.whoAmI;
                int newNpcIndex;
                int lastNpcIndex = npc.whoAmI;
                int destroyerSegmentsCount = NPC.GetDestroyerSegmentsCount();
                for (int j = 0; j <= destroyerSegmentsCount; j++)
                {
                    int newNpcType = 135;
                    if (j == destroyerSegmentsCount)
                    {
                        newNpcType = 136;
                    }
                    newNpcIndex = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), newNpcType, npc.whoAmI);
                    Main.npc[newNpcIndex].ai[3] = npc.whoAmI;
                    Main.npc[newNpcIndex].realLife = npc.whoAmI;
                    Main.npc[newNpcIndex].ai[1] = lastNpcIndex;
                    Main.npc[lastNpcIndex].ai[0] = newNpcIndex;
                    NetMessage.SendData(23, -1, -1, null, newNpcIndex);
                    lastNpcIndex = newNpcIndex;
                }
            }
            if (npc.type == 135)
            {
                npc.localAI[0] += Main.rand.Next(4);
                if (npc.localAI[0] >= Main.rand.Next(1400, 26000))
                {
                    npc.localAI[0] = 0f;
                    npc.TargetClosest();
                    //if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                    //{
                    Vector2 vector = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height / 2);
                    float num8 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector.X + Main.rand.Next(-20, 21);
                    float num9 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector.Y + Main.rand.Next(-20, 21);
                    float num10 = (float)Math.Sqrt(num8 * num8 + num9 * num9);
                    num10 = 8f / num10;
                    num8 *= num10;
                    num9 *= num10;
                    num8 += Main.rand.Next(-20, 21) * 0.05f;
                    num9 += Main.rand.Next(-20, 21) * 0.05f;
                    int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(22f, 18f);
                    int projType = 100;

                    if (!Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                    {
                        projType = 259;
                    }

                    vector.X += num8 * 5f;
                    vector.Y += num9 * 5f;
                    int num12 = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector.X, vector.Y, num8, num9, projType, attackDamage_ForProjectiles, 0f, Main.myPlayer);
                    Main.projectile[num12].timeLeft = 300;
                    npc.netUpdate = true;
                    //}
                }
            }
        }
        int num13 = (int)(npc.position.X / 16f) - 1;
        int num14 = (int)((npc.position.X + npc.width) / 16f) + 2;
        int num15 = (int)(npc.position.Y / 16f) - 1;
        int num16 = (int)((npc.position.Y + npc.height) / 16f) + 2;
        if (num13 < 0)
        {
            num13 = 0;
        }
        if (num14 > Main.maxTilesX)
        {
            num14 = Main.maxTilesX;
        }
        if (num15 < 0)
        {
            num15 = 0;
        }
        if (num16 > Main.maxTilesY)
        {
            num16 = Main.maxTilesY;
        }
        bool flag2 = false;
        if (!flag2)
        {
            Vector2 vector2 = default;
            for (int k = num13; k < num14; k++)
            {
                for (int l = num15; l < num16; l++)
                {
                    if (Main.tile[k, l] != null && ((Main.tile[k, l].nactive() && (Main.tileSolid[Main.tile[k, l].type] || (Main.tileSolidTop[Main.tile[k, l].type] && Main.tile[k, l].frameY == 0))) || Main.tile[k, l].liquid > 64))
                    {
                        vector2.X = k * 16;
                        vector2.Y = l * 16;
                        if (npc.position.X + npc.width > vector2.X && npc.position.X < vector2.X + 16f && npc.position.Y + npc.height > vector2.Y && npc.position.Y < vector2.Y + 16f)
                        {
                            flag2 = true;
                            break;
                        }
                    }
                }
            }
        }
        if (!flag2)
        {
            if (npc.type != 135 || npc.ai[2] != 1f)
            {
                Lighting.AddLight((int)((npc.position.X + npc.width / 2) / 16f), (int)((npc.position.Y + npc.height / 2) / 16f), 0.3f, 0.1f, 0.05f);
            }
            npc.localAI[1] = 1f;
            if (npc.type == 134)
            {
                Rectangle rectangle = new((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                int num17 = 1000;
                bool flag3 = true;
                if (npc.position.Y > Main.player[npc.target].position.Y)
                {
                    for (int m = 0; m < 255; m++)
                    {
                        if (Main.player[m].active)
                        {
                            Rectangle rectangle2 = new((int)Main.player[m].position.X - num17, (int)Main.player[m].position.Y - num17, num17 * 2, num17 * 2);
                            if (rectangle.Intersects(rectangle2))
                            {
                                flag3 = false;
                                break;
                            }
                        }
                    }
                    if (flag3)
                    {
                        flag2 = true;
                    }
                }
            }
        }
        else
        {
            npc.localAI[1] = 0f;
        }
        float num18 = 16f;
        if (Main.IsItDay() || Main.player[npc.target].dead)
        {
            flag2 = false;
            npc.velocity.Y += 1f;
            if (npc.position.Y > Main.worldSurface * 16.0)
            {
                npc.velocity.Y += 1f;
                num18 = 32f;
            }
            if (npc.position.Y > Main.rockLayer * 16.0)
            {
                for (int n = 0; n < 200; n++)
                {
                    if (Main.npc[n].aiStyle == npc.aiStyle)
                    {
                        Main.npc[n].active = false;
                    }
                }
            }
        }
        float num19 = 0.1f;
        float num20 = 0.15f;
        if (Main.getGoodWorld)
        {
            num19 *= 1.2f;
            num20 *= 1.2f;
        }
        Vector2 vector3 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
        float num21 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2;
        float num22 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2;
        num21 = (int)(num21 / 16f) * 16;
        num22 = (int)(num22 / 16f) * 16;
        vector3.X = (int)(vector3.X / 16f) * 16;
        vector3.Y = (int)(vector3.Y / 16f) * 16;
        num21 -= vector3.X;
        num22 -= vector3.Y;
        float num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
        if (npc.ai[1] > 0f && npc.ai[1] < Main.npc.Length)
        {
            int num24 = (int)(44f * npc.scale);
            try
            {
                vector3 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                num21 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - vector3.X;
                num22 = Main.npc[(int)npc.ai[1]].position.Y + Main.npc[(int)npc.ai[1]].height / 2 - vector3.Y;
            }
            catch
            {
            }
            if (num > 0)
            {
                float num25 = num24 - num24 * ((num - 1f) * 0.1f);
                if (num25 < 0f)
                {
                    num25 = 0f;
                }
                if (num25 > num24)
                {
                    num25 = num24;
                }
                num22 = Main.npc[(int)npc.ai[1]].position.Y + Main.npc[(int)npc.ai[1]].height / 2 + num25 - vector3.Y;
            }
            npc.rotation = (float)Math.Atan2(num22, num21) + 1.57f;
            num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
            if (num > 0)
            {
                num24 = num24 / num2 * num;
            }
            num23 = (num23 - num24) / num23;
            num21 *= num23;
            num22 *= num23;
            npc.velocity = Vector2.Zero;
            npc.position.X += num21;
            npc.position.Y += num22;
            num21 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - vector3.X;
            num22 = Main.npc[(int)npc.ai[1]].position.Y + Main.npc[(int)npc.ai[1]].height / 2 - vector3.Y;
            npc.rotation = (float)Math.Atan2(num22, num21) + 1.57f;
        }
        else
        {
            if (!flag2)
            {
                npc.TargetClosest();
                npc.velocity.Y += 0.15f;
                if (npc.velocity.Y > num18)
                {
                    npc.velocity.Y = num18;
                }
                if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)num18 * 0.4)
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X -= num19 * 1.1f;
                    }
                    else
                    {
                        npc.velocity.X += num19 * 1.1f;
                    }
                }
                else if (npc.velocity.Y == num18)
                {
                    if (npc.velocity.X < num21)
                    {
                        npc.velocity.X += num19;
                    }
                    else if (npc.velocity.X > num21)
                    {
                        npc.velocity.X -= num19;
                    }
                }
                else if (npc.velocity.Y > 4f)
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X += num19 * 0.9f;
                    }
                    else
                    {
                        npc.velocity.X -= num19 * 0.9f;
                    }
                }
            }
            else
            {
                if (npc.soundDelay == 0)
                {
                    float num26 = num23 / 40f;
                    if (num26 < 10f)
                    {
                        num26 = 10f;
                    }
                    if (num26 > 20f)
                    {
                        num26 = 20f;
                    }
                    npc.soundDelay = (int)num26;
                    SoundEngine.PlaySound(15, (int)npc.position.X, (int)npc.position.Y);
                }
                num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
                float num27 = Math.Abs(num21);
                float num28 = Math.Abs(num22);
                float num29 = num18 / num23;
                num21 *= num29;
                num22 *= num29;
                if (((npc.velocity.X > 0f && num21 > 0f) || (npc.velocity.X < 0f && num21 < 0f)) && ((npc.velocity.Y > 0f && num22 > 0f) || (npc.velocity.Y < 0f && num22 < 0f)))
                {
                    if (npc.velocity.X < num21)
                    {
                        npc.velocity.X += num20;
                    }
                    else if (npc.velocity.X > num21)
                    {
                        npc.velocity.X -= num20;
                    }
                    if (npc.velocity.Y < num22)
                    {
                        npc.velocity.Y += num20;
                    }
                    else if (npc.velocity.Y > num22)
                    {
                        npc.velocity.Y -= num20;
                    }
                }
                if ((npc.velocity.X > 0f && num21 > 0f) || (npc.velocity.X < 0f && num21 < 0f) || (npc.velocity.Y > 0f && num22 > 0f) || (npc.velocity.Y < 0f && num22 < 0f))
                {
                    if (npc.velocity.X < num21)
                    {
                        npc.velocity.X += num19;
                    }
                    else if (npc.velocity.X > num21)
                    {
                        npc.velocity.X -= num19;
                    }
                    if (npc.velocity.Y < num22)
                    {
                        npc.velocity.Y += num19;
                    }
                    else if (npc.velocity.Y > num22)
                    {
                        npc.velocity.Y -= num19;
                    }
                    if ((double)Math.Abs(num22) < (double)num18 * 0.2 && ((npc.velocity.X > 0f && num21 < 0f) || (npc.velocity.X < 0f && num21 > 0f)))
                    {
                        if (npc.velocity.Y > 0f)
                        {
                            npc.velocity.Y += num19 * 2f;
                        }
                        else
                        {
                            npc.velocity.Y -= num19 * 2f;
                        }
                    }
                    if ((double)Math.Abs(num21) < (double)num18 * 0.2 && ((npc.velocity.Y > 0f && num22 < 0f) || (npc.velocity.Y < 0f && num22 > 0f)))
                    {
                        if (npc.velocity.X > 0f)
                        {
                            npc.velocity.X += num19 * 2f;
                        }
                        else
                        {
                            npc.velocity.X -= num19 * 2f;
                        }
                    }
                }
                else if (num27 > num28)
                {
                    if (npc.velocity.X < num21)
                    {
                        npc.velocity.X += num19 * 1.1f;
                    }
                    else if (npc.velocity.X > num21)
                    {
                        npc.velocity.X -= num19 * 1.1f;
                    }
                    if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)num18 * 0.5)
                    {
                        if (npc.velocity.Y > 0f)
                        {
                            npc.velocity.Y += num19;
                        }
                        else
                        {
                            npc.velocity.Y -= num19;
                        }
                    }
                }
                else
                {
                    if (npc.velocity.Y < num22)
                    {
                        npc.velocity.Y += num19 * 1.1f;
                    }
                    else if (npc.velocity.Y > num22)
                    {
                        npc.velocity.Y -= num19 * 1.1f;
                    }
                    if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)num18 * 0.5)
                    {
                        if (npc.velocity.X > 0f)
                        {
                            npc.velocity.X += num19;
                        }
                        else
                        {
                            npc.velocity.X -= num19;
                        }
                    }
                }
            }
            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
            if (npc.type == 134)
            {
                if (flag2)
                {
                    if (npc.localAI[0] != 1f)
                    {
                        npc.netUpdate = true;
                    }
                    npc.localAI[0] = 1f;
                }
                else
                {
                    if (npc.localAI[0] != 0f)
                    {
                        npc.netUpdate = true;
                    }
                    npc.localAI[0] = 0f;
                }
                if (((npc.velocity.X > 0f && npc.oldVelocity.X < 0f) || (npc.velocity.X < 0f && npc.oldVelocity.X > 0f) || (npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f) || (npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f)) && !npc.justHit)
                {
                    npc.netUpdate = true;
                }
            }
        }
        if (NPC.IsMechQueenUp && npc.type == 134)
        {
            NPC nPC = Main.npc[NPC.mechQueen];
            Vector2 mechQueenCenter = nPC.GetMechQueenCenter();
            Vector2 vector4 = new(0f, 100f);
            Vector2 spinningpoint = mechQueenCenter + vector4;
            float num30 = nPC.velocity.X * 0.025f;
            spinningpoint = spinningpoint.RotatedBy(num30, mechQueenCenter);
            npc.position = spinningpoint - npc.Size / 2f + nPC.velocity;
            npc.velocity.X = 0f;
            npc.velocity.Y = 0f;
            npc.rotation = num30 * 0.75f + (float)Math.PI;
        }
    }
}
