namespace VBY.NPCTest;

partial class NPCAIs
{
    public static void AI_006_Worms(NPC npc)
    {
        if (npc.type == 117 && npc.localAI[1] == 0f)
        {
            npc.localAI[1] = 1f;
            //SoundEngine.PlaySound(SoundID.NPCDeath13, npc.position);
            //int num = 1;
            //if (npc.velocity.X < 0f)
            //{
                //num = -1;
            //}
            //for (int i = 0; i < 20; i++)
            //{
            //    Dust.NewDust(new Vector2(npc.position.X - 20f, npc.position.Y - 20f), npc.width + 40, npc.height + 40, 5, num * 8, -1f);
            //}
        }
        if (npc.type == 454 && npc.localAI[3] == 0f)
        {
            //SoundEngine.PlaySound(SoundID.Item119, npc.position);
            npc.localAI[3] = 1f;
        }
        if (npc.type >= 454 && npc.type <= 459)
        {
            npc.dontTakeDamage = npc.alpha > 0;
            if (npc.type == 454 || (npc.type != 454 && Main.npc[(int)npc.ai[1]].alpha < 85))
            {
                //if (npc.dontTakeDamage)
                //{
                //    //for (int j = 0; j < 2; j++)
                //    //{
                //    //    int num2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 228, 0f, 0f, 100, default(Color), 2f);
                //    //    Main.dust[num2].noGravity = true;
                //    //    Main.dust[num2].noLight = true;
                //    //}
                //}
                npc.alpha -= 42;
                if (npc.alpha < 0)
                {
                    npc.alpha = 0;
                }
            }
        }
        if (npc.type >= 621 && npc.type <= 623)
        {
            npc.position += npc.netOffset;
            npc.dontTakeDamage = npc.alpha > 0;
            if (npc.type == 621 || (npc.type != 621 && Main.npc[(int)npc.ai[1]].alpha < 85))
            {
                //if (npc.dontTakeDamage)
                //{
                //    for (int k = 0; k < 2; k++)
                //    {
                //        Dust.NewDust(npc.position, npc.width, npc.height, 5, 0f, 0f, 100);
                //    }
                //}
                npc.alpha -= 42;
                if (npc.alpha < 0)
                {
                    npc.alpha = 0;
                }
            }
            //if (npc.alpha == 0 && Main.rand.Next(5) == 0)
            //{
            //    Dust.NewDust(npc.position, npc.width, npc.height, 5, 0f, 0f, 100);
            //}
            npc.position -= npc.netOffset;
        }
        else if (npc.type == 402)
        {
            npc.ai[2] += 1f;
            float num3 = 600f;
            float num4 = num3 - 30f;
            if (npc.velocity.Length() >= 1f && npc.ai[2] <= num4)
            {
                npc.velocity *= Utils.Remap(npc.ai[2], num4 * 0.5f, num4, 1f, 0.5f);
            }
            if (npc.ai[2] == num4)
            {
                //for (int l = 0; l < 50; l++)
                //{
                //    Vector2 vector = Main.rand.NextVector2Circular(8f, 8f);
                //    if (Main.rand.Next(2) == 0)
                //    {
                //        int num5 = Dust.NewDust(npc.position, npc.width, npc.height, 180, 0f, 0f, 100);
                //        Main.dust[num5].scale += (float)Main.rand.Next(50) * 0.04f;
                //        Main.dust[num5].noGravity = true;
                //        Main.dust[num5].velocity = vector;
                //        Main.dust[num5].fadeIn = Main.rand.NextFloat() * 1.5f;
                //    }
                //    if (Main.rand.Next(2) == 0)
                //    {
                //        int num6 = Dust.NewDust(npc.position, npc.width, npc.height, 176, 0f, 0f, 100);
                //        Main.dust[num6].scale += 0.3f + (float)Main.rand.Next(50) * 0.01f;
                //        Main.dust[num6].noGravity = true;
                //        Main.dust[num6].velocity = vector;
                //        Main.dust[num6].fadeIn = Main.rand.NextFloat() * 1.5f;
                //    }
                //}
                //if (Main.netMode != 1)
                //{
                    npc.SpawnStardustMark_StardustWorm();
                    npc.velocity = npc.velocity.SafeNormalize(Vector2.Zero) * 6f;
                    npc.netUpdate = true;
                //}
            }
            if (npc.ai[2] >= num3 && Main.netMode != 1)
            {
                npc.ai[2] = 0f;
                npc.netUpdate = true;
            }
        }
        if (Main.netMode != 1 && Main.expertMode)
        {
            if (npc.type == 14 && ((double)(npc.position.Y / 16f) < Main.worldSurface || Main.getGoodWorld))
            {
                int x = (int)(npc.Center.X / 16f);
                int y = (int)(npc.Center.Y / 16f);
                if (WorldGen.InWorld(x, y) && Main.tile[x, y].wall == 0)
                {
                    int num7 = 900;
                    if (Main.getGoodWorld)
                    {
                        num7 /= 2;
                    }
                    if (Main.rand.Next(num7) == 0)
                    {
                        npc.TargetClosest();
                        if (Collision.CanHitLine(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                npc.NewProjectile(npc.GetToTargetVector2().Normalize(1), ProjectileID.DemonSickle, npc.damage / 4);
                            }
                            else
                            {
                                NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2 + npc.velocity.X), (int)(npc.position.Y + npc.height / 2 + npc.velocity.Y), 666, 0, 0f, 1f);
                            }
                        }
                    }
                }
            }
            else if (npc.type == NPCID.EaterofWorldsHead)
            {
                int num8 = 90;
                num8 += (int)(npc.life / (float)npc.lifeMax * 60f * 5f);
                if (Main.rand.Next(num8) == 0)
                {
                    npc.TargetClosest();
                    if (Collision.CanHitLine(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                    {
                        NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2 + npc.velocity.X), (int)(npc.position.Y + npc.height / 2 + npc.velocity.Y), 666, 0, 0f, 1f);
                    }
                }
            }
        }
        bool flag = false;
        float num9 = 0.2f;
        switch (npc.type)
        {
            case 513:
                flag = !Main.player[npc.target].ZoneUndergroundDesert;
                num9 = 0.1f;
                break;
            case 10:
            case 39:
            case 95:
            case 117:
            case 510:
                flag = true;
                break;
            case 621:
                flag = false;
                break;
        }
        if (npc.type >= 13 && npc.type <= 15)
        {
            npc.realLife = -1;
        }
        else if (npc.ai[3] > 0f)
        {
            npc.realLife = (int)npc.ai[3];
        }
        if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || (flag && Main.player[npc.target].position.Y < Main.worldSurface * 16.0))
        {
            npc.TargetClosest();
        }
        if (Main.player[npc.target].dead || (flag && Main.player[npc.target].position.Y < Main.worldSurface * 16.0))
        {
            npc.EncourageDespawn(300);
            if (flag)
            {
                npc.velocity.Y += num9;
            }
        }
        if (npc.type == 621 && Main.dayTime)
        {
            npc.EncourageDespawn(60);
            npc.velocity.Y += 1f;
        }
        if (Main.netMode != 1)
        {
            if (npc.type == 87 && npc.ai[0] == 0f)
            {
                npc.ai[3] = npc.whoAmI;
                npc.realLife = npc.whoAmI;
                int num10;
                int num11 = npc.whoAmI;
                for (int m = 0; m < 14; m++)
                {
                    int num12 = 89;
                    switch (m)
                    {
                        case 1:
                        case 8:
                            num12 = 88;
                            break;
                        case 11:
                            num12 = 90;
                            break;
                        case 12:
                            num12 = 91;
                            break;
                        case 13:
                            num12 = 92;
                            break;
                    }
                    num10 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), num12, npc.whoAmI);
                    Main.npc[num10].ai[3] = npc.whoAmI;
                    Main.npc[num10].realLife = npc.whoAmI;
                    Main.npc[num10].ai[1] = num11;
                    Main.npc[num10].CopyInteractions(npc);
                    Main.npc[num11].ai[0] = num10;
                    NetMessage.SendData(23, -1, -1, null, num10);
                    num11 = num10;
                }
            }
            if (npc.type == 454 && npc.ai[0] == 0f)
            {
                npc.ai[3] = npc.whoAmI;
                npc.realLife = npc.whoAmI;
                int num13;
                int num14 = npc.whoAmI;
                for (int n = 0; n < 30; n++)
                {
                    int num15 = 456;
                    if ((n - 2) % 4 == 0 && n < 26)
                    {
                        num15 = 455;
                    }
                    else
                    {
                        switch (n)
                        {
                            case 27:
                                num15 = 457;
                                break;
                            case 28:
                                num15 = 458;
                                break;
                            case 29:
                                num15 = 459;
                                break;
                        }
                    }
                    num13 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), num15, npc.whoAmI);
                    Main.npc[num13].ai[3] = npc.whoAmI;
                    Main.npc[num13].realLife = npc.whoAmI;
                    Main.npc[num13].ai[1] = num14;
                    Main.npc[num13].CopyInteractions(npc);
                    Main.npc[num14].ai[0] = num13;
                    NetMessage.SendData(23, -1, -1, null, num13);
                    num14 = num13;
                }
            }
            if (npc.type == 513 && npc.ai[0] == 0f)
            {
                npc.ai[3] = npc.whoAmI;
                npc.realLife = npc.whoAmI;
                int num16;
                int num17 = npc.whoAmI;
                int num18 = Main.rand.Next(6, 10);
                for (int num19 = 0; num19 < num18; num19++)
                {
                    int num20 = 514;
                    if (num19 == num18 - 1)
                    {
                        num20 = 515;
                    }
                    num16 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), num20, npc.whoAmI);
                    Main.npc[num16].ai[3] = npc.whoAmI;
                    Main.npc[num16].realLife = npc.whoAmI;
                    Main.npc[num16].ai[1] = num17;
                    Main.npc[num16].CopyInteractions(npc);
                    Main.npc[num17].ai[0] = num16;
                    NetMessage.SendData(23, -1, -1, null, num16);
                    num17 = num16;
                }
            }
            if (npc.type == 510 && npc.ai[0] == 0f)
            {
                npc.ai[3] = npc.whoAmI;
                npc.realLife = npc.whoAmI;
                int newNPCIndex;
                int num22 = npc.whoAmI;
                int num23 = Main.rand.Next(12, 21);
                for (int num24 = 0; num24 < num23; num24++)
                {
                    int num25 = 511;
                    if (num24 == num23 - 1)
                    {
                        num25 = 512;
                    }
                    newNPCIndex = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), num25, npc.whoAmI);
                    Main.npc[newNPCIndex].ai[3] = npc.whoAmI;
                    Main.npc[newNPCIndex].realLife = npc.whoAmI;
                    Main.npc[newNPCIndex].ai[1] = num22;
                    Main.npc[newNPCIndex].CopyInteractions(npc);
                    Main.npc[num22].ai[0] = newNPCIndex;
                    NetMessage.SendData(23, -1, -1, null, newNPCIndex);
                    num22 = newNPCIndex;
                }
            }
            if (npc.type == 621 && npc.ai[0] == 0f)
            {
                npc.ai[3] = npc.whoAmI;
                npc.realLife = npc.whoAmI;
                int num26;
                int num27 = npc.whoAmI;
                int num28 = 16;
                for (int num29 = 0; num29 < num28; num29++)
                {
                    int num30 = 622;
                    if (num29 == num28 - 1)
                    {
                        num30 = 623;
                    }
                    num26 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), num30, npc.whoAmI);
                    Main.npc[num26].ai[3] = npc.whoAmI;
                    Main.npc[num26].realLife = npc.whoAmI;
                    Main.npc[num26].ai[1] = num27;
                    Main.npc[num26].CopyInteractions(npc);
                    Main.npc[num27].ai[0] = num26;
                    NetMessage.SendData(23, -1, -1, null, num26);
                    num27 = num26;
                }
            }
            else if ((npc.type == 7 || npc.type == 8 || npc.type == 10 || npc.type == 11 || npc.type == 13 || npc.type == 14 || npc.type == 39 || npc.type == 40 || npc.type == 95 || npc.type == 96 || npc.type == 98 || npc.type == 99 || npc.type == 117 || npc.type == 118) && npc.ai[0] == 0f)
            {
                if (npc.type == 7 || npc.type == 10 || npc.type == 13 || npc.type == 39 || npc.type == 95 || npc.type == 98 || npc.type == 117)
                {
                    if (npc.type < 13 || npc.type > 15)
                    {
                        npc.ai[3] = npc.whoAmI;
                        npc.realLife = npc.whoAmI;
                    }
                    npc.ai[2] = Main.rand.Next(8, 13);
                    if (npc.type == 10)
                    {
                        npc.ai[2] = Main.rand.Next(4, 7);
                    }
                    if (npc.type == 13)
                    {
                        npc.ai[2] = NPC.GetEaterOfWorldsSegmentsCount();
                    }
                    if (npc.type == 39)
                    {
                        npc.ai[2] = Main.rand.Next(14, 23);
                        if (Main.getGoodWorld)
                        {
                            npc.ai[2] += 3f;
                            if (Main.remixWorld)
                            {
                                npc.ai[2] += 4f;
                            }
                        }
                    }
                    if (npc.type == 95)
                    {
                        npc.ai[2] = Main.rand.Next(6, 12);
                    }
                    if (npc.type == 98)
                    {
                        npc.ai[2] = Main.rand.Next(20, 26);
                    }
                    if (npc.type == 117)
                    {
                        npc.ai[2] = Main.rand.Next(3, 6);
                    }
                    if (npc.type == 7 && Main.remixWorld)
                    {
                        npc.ai[2] *= 2f;
                    }
                    npc.ai[0] = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), npc.type + 1, npc.whoAmI);
                    Main.npc[(int)npc.ai[0]].CopyInteractions(npc);
                }
                else if ((npc.type == 8 || npc.type == 11 || npc.type == 14 || npc.type == 40 || npc.type == 96 || npc.type == 99 || npc.type == 118) && npc.ai[2] > 0f)
                {
                    npc.ai[0] = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), npc.type, npc.whoAmI);
                    Main.npc[(int)npc.ai[0]].CopyInteractions(npc);
                }
                else
                {
                    npc.ai[0] = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), npc.type + 1, npc.whoAmI);
                    Main.npc[(int)npc.ai[0]].CopyInteractions(npc);
                }
                if (npc.type < 13 || npc.type > 15)
                {
                    Main.npc[(int)npc.ai[0]].ai[3] = npc.ai[3];
                    Main.npc[(int)npc.ai[0]].realLife = npc.realLife;
                }
                Main.npc[(int)npc.ai[0]].ai[1] = npc.whoAmI;
                Main.npc[(int)npc.ai[0]].ai[2] = npc.ai[2] - 1f;
                npc.netUpdate = true;
            }
            if (npc.type == 412 && npc.ai[0] == 0f)
            {
                npc.ai[3] = npc.whoAmI;
                npc.realLife = npc.whoAmI;
                int num31;
                int num32 = npc.whoAmI;
                int num33 = 30;
                for (int num34 = 0; num34 < num33; num34++)
                {
                    int num35 = 413;
                    if (num34 == num33 - 1)
                    {
                        num35 = 414;
                    }
                    num31 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), num35, npc.whoAmI);
                    Main.npc[num31].ai[3] = npc.whoAmI;
                    Main.npc[num31].realLife = npc.whoAmI;
                    Main.npc[num31].ai[1] = num32;
                    Main.npc[num31].CopyInteractions(npc);
                    Main.npc[num32].ai[0] = num31;
                    NetMessage.SendData(23, -1, -1, null, num31);
                    num32 = num31;
                }
            }
            switch (npc.type)
            {
                case 8:
                case 9:
                case 11:
                case 12:
                case 40:
                case 41:
                case 88:
                case 89:
                case 90:
                case 91:
                case 92:
                case 96:
                case 97:
                case 99:
                case 100:
                case 118:
                case 119:
                case 413:
                case 414:
                case 455:
                case 456:
                case 457:
                case 458:
                case 459:
                case 511:
                case 512:
                case 514:
                case 515:
                case 622:
                case 623:
                    if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].aiStyle != npc.aiStyle)
                    {
                        npc.life = 0;
                        npc.HitEffect();
                        npc.checkDead();
                        npc.active = false;
                        NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f);
                        return;
                    }
                    break;
            }
            switch (npc.type)
            {
                case 7:
                case 8:
                case 10:
                case 11:
                case 39:
                case 40:
                case 87:
                case 88:
                case 89:
                case 90:
                case 91:
                case 95:
                case 96:
                case 98:
                case 99:
                case 117:
                case 118:
                case 412:
                case 413:
                case 454:
                case 455:
                case 456:
                case 457:
                case 458:
                case 510:
                case 511:
                case 513:
                case 514:
                case 621:
                case 622:
                    if (!Main.npc[(int)npc.ai[0]].active || Main.npc[(int)npc.ai[0]].aiStyle != npc.aiStyle)
                    {
                        npc.life = 0;
                        npc.HitEffect();
                        npc.checkDead();
                        npc.active = false;
                        NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f);
                        return;
                    }
                    break;
            }
            if (npc.type == 13 || npc.type == 14 || npc.type == 15)
            {
                if (!Main.npc[(int)npc.ai[1]].active && !Main.npc[(int)npc.ai[0]].active)
                {
                    npc.life = 0;
                    npc.HitEffect();
                    npc.checkDead();
                    npc.active = false;
                    NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f);
                    return;
                }
                if (npc.type == 13 && !Main.npc[(int)npc.ai[0]].active)
                {
                    npc.life = 0;
                    npc.HitEffect();
                    npc.checkDead();
                    npc.active = false;
                    NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f);
                    return;
                }
                if (npc.type == 15 && !Main.npc[(int)npc.ai[1]].active)
                {
                    npc.life = 0;
                    npc.HitEffect();
                    npc.checkDead();
                    npc.active = false;
                    NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f);
                    return;
                }
                if (npc.type == 14 && (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].aiStyle != npc.aiStyle))
                {
                    npc.type = 13;
                    int num36 = npc.whoAmI;
                    float num37 = npc.life / (float)npc.lifeMax;
                    float num38 = npc.ai[0];
                    npc.SetDefaultsKeepPlayerInteraction(npc.type);
                    npc.life = (int)(npc.lifeMax * num37);
                    npc.ai[0] = num38;
                    npc.TargetClosest();
                    npc.netUpdate = true;
                    npc.whoAmI = num36;
                    npc.alpha = 0;
                }
                if (npc.type == 14 && (!Main.npc[(int)npc.ai[0]].active || Main.npc[(int)npc.ai[0]].aiStyle != npc.aiStyle))
                {
                    npc.type = 15;
                    int num39 = npc.whoAmI;
                    float num40 = npc.life / (float)npc.lifeMax;
                    float num41 = npc.ai[1];
                    npc.SetDefaultsKeepPlayerInteraction(npc.type);
                    npc.life = (int)(npc.lifeMax * num40);
                    npc.ai[1] = num41;
                    npc.TargetClosest();
                    npc.netUpdate = true;
                    npc.whoAmI = num39;
                    npc.alpha = 0;
                }
            }
            if (!npc.active && Main.netMode == 2)
            {
                NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f);
            }
        }
        int num42 = (int)(npc.position.X / 16f) - 1;
        int num43 = (int)((npc.position.X + npc.width) / 16f) + 2;
        int num44 = (int)(npc.position.Y / 16f) - 1;
        int num45 = (int)((npc.position.Y + npc.height) / 16f) + 2;
        if (num42 < 0)
        {
            num42 = 0;
        }
        if (num43 > Main.maxTilesX)
        {
            num43 = Main.maxTilesX;
        }
        if (num44 < 0)
        {
            num44 = 0;
        }
        if (num45 > Main.maxTilesY)
        {
            num45 = Main.maxTilesY;
        }
        bool flag2 = false;
        if (npc.type >= 87 && npc.type <= 92)
        {
            flag2 = true;
        }
        if (npc.type >= 454 && npc.type <= 459)
        {
            flag2 = true;
        }
        if (npc.type >= 621 && npc.type <= 623)
        {
            flag2 = true;
        }
        if (npc.type >= 412 && npc.type <= 414)
        {
            flag2 = true;
        }
        if (npc.type == 402)
        {
            flag2 = true;
        }
        if (!flag2)
        {
            Vector2 vector2 = default;
            for (int num46 = num42; num46 < num43; num46++)
            {
                for (int num47 = num44; num47 < num45; num47++)
                {
                    if (Main.tile[num46, num47] == null || ((!Main.tile[num46, num47].nactive() || (!Main.tileSolid[Main.tile[num46, num47].type] && (!Main.tileSolidTop[Main.tile[num46, num47].type] || Main.tile[num46, num47].frameY != 0))) && Main.tile[num46, num47].liquid <= 64))
                    {
                        continue;
                    }
                    vector2.X = num46 * 16;
                    vector2.Y = num47 * 16;
                    if (npc.position.X + npc.width > vector2.X && npc.position.X < vector2.X + 16f && npc.position.Y + npc.height > vector2.Y && npc.position.Y < vector2.Y + 16f)
                    {
                        flag2 = true;
                        if (Main.rand.Next(100) == 0 && npc.type != 117 && Main.tile[num46, num47].nactive() && Main.tileSolid[Main.tile[num46, num47].type])
                        {
                            WorldGen.KillTile(num46, num47, fail: true, effectOnly: true);
                        }
                    }
                }
            }
        }
        if (!flag2 && (npc.type == 7 || npc.type == 10 || npc.type == 13 || npc.type == 39 || npc.type == 95 || npc.type == 98 || npc.type == 117 || npc.type == 375 || npc.type == 454 || npc.type == 510 || npc.type == 513 || npc.type == 621))
        {
            Rectangle rectangle = new((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
            int num48 = 1000;
            bool flag3 = true;
            for (int num49 = 0; num49 < 255; num49++)
            {
                if (Main.player[num49].active)
                {
                    Rectangle rectangle2 = new((int)Main.player[num49].position.X - num48, (int)Main.player[num49].position.Y - num48, num48 * 2, num48 * 2);
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
        if ((npc.type >= 87 && npc.type <= 92) || (npc.type >= 454 && npc.type <= 459) || (npc.type >= 621 && npc.type <= 623))
        {
            if (npc.velocity.X < 0f)
            {
                npc.spriteDirection = 1;
            }
            else if (npc.velocity.X > 0f)
            {
                npc.spriteDirection = -1;
            }
        }
        if (npc.type == 414)
        {
            if (npc.justHit)
            {
                npc.localAI[3] = 3f;
            }
            if (npc.localAI[2] > 0f)
            {
                npc.localAI[2] -= 16f;
                if (npc.localAI[2] == 0f)
                {
                    npc.localAI[2] = -128f;
                }
            }
            else if (npc.localAI[2] < 0f)
            {
                npc.localAI[2] += 16f;
            }
            else if (npc.localAI[3] > 0f)
            {
                npc.localAI[2] = 128f;
                npc.localAI[3] -= 1f;
            }
        }
        if (npc.type == 412)
        {
            npc.position += npc.netOffset;
            Vector2 vector3 = npc.Center + (npc.rotation - (float)Math.PI / 2f).ToRotationVector2() * 8f;
            Vector2 vector4 = npc.rotation.ToRotationVector2() * 16f;
            Dust obj = Main.dust[Dust.NewDust(vector3 + vector4, 0, 0, 6, npc.velocity.X, npc.velocity.Y, 100, Color.Transparent, 1f + Main.rand.NextFloat() * 3f)];
            obj.noGravity = true;
            obj.noLight = true;
            obj.position -= new Vector2(4f);
            obj.fadeIn = 1f;
            obj.velocity = Vector2.Zero;
            Dust obj2 = Main.dust[Dust.NewDust(vector3 - vector4, 0, 0, 6, npc.velocity.X, npc.velocity.Y, 100, Color.Transparent, 1f + Main.rand.NextFloat() * 3f)];
            obj2.noGravity = true;
            obj2.noLight = true;
            obj2.position -= new Vector2(4f);
            obj2.fadeIn = 1f;
            obj2.velocity = Vector2.Zero;
            npc.position -= npc.netOffset;
        }
        float num50 = 8f;
        float num51 = 0.07f;
        if (npc.type == 95)
        {
            num50 = 5.5f;
            num51 = 0.045f;
        }
        if (npc.type == 10)
        {
            num50 = 6f;
            num51 = 0.05f;
        }
        if (npc.type == 513)
        {
            num50 = 7f;
            num51 = 0.1f;
        }
        if (npc.type == 7)
        {
            num50 = 9f;
            num51 = 0.1f;
        }
        if (npc.type == 13)
        {
            num50 = 10f;
            num51 = 0.07f;
            if (Main.expertMode)
            {
                num50 = 12f;
                num51 = 0.15f;
            }
            if (Main.getGoodWorld)
            {
                num50 += 4f;
                num51 += 0.05f;
            }
        }
        if (npc.type == 510)
        {
            if (!Main.player[npc.target].dead && Main.player[npc.target].ZoneSandstorm)
            {
                num50 = 16f;
                num51 = 0.35f;
            }
            else
            {
                num50 = 10f;
                num51 = 0.25f;
            }
        }
        if (npc.type == 87)
        {
            num50 = 11f;
            num51 = 0.25f;
        }
        if (npc.type == 621)
        {
            num50 = 15f;
            num51 = 0.45f;
        }
        if (npc.type == 375)
        {
            num50 = 6f;
            num51 = 0.15f;
        }
        if (npc.type == 454)
        {
            num50 = 20f;
            num51 = 0.55f;
        }
        if (npc.type == 402)
        {
            num50 = 9f;
            num51 = 0.3f;
        }
        if (npc.type == 117 && Main.wofNPCIndex >= 0)
        {
            float num52 = Main.npc[Main.wofNPCIndex].life / (float)Main.npc[Main.wofNPCIndex].lifeMax;
            if ((double)num52 < 0.5)
            {
                num50 += 1f;
                num51 += 0.1f;
            }
            if ((double)num52 < 0.25)
            {
                num50 += 1f;
                num51 += 0.1f;
            }
            if ((double)num52 < 0.1)
            {
                num50 += 2f;
                num51 += 0.1f;
            }
        }
        if (npc.type == 39)
        {
            num50 = 9f;
            num51 = 0.1f;
            if (Main.getGoodWorld)
            {
                num50 = 10f;
                num51 = 0.12f;
            }
        }
        Vector2 vector5 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
        float num53 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2;
        float num54 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2;
        if (npc.type == 412)
        {
            num50 = 10f;
            num51 = 0.3f;
            int num55 = -1;
            int num56 = (int)(Main.player[npc.target].Center.X / 16f);
            int num57 = (int)(Main.player[npc.target].Center.Y / 16f);
            for (int num58 = num56 - 2; num58 <= num56 + 2; num58++)
            {
                for (int num59 = num57; num59 <= num57 + 15; num59++)
                {
                    if (WorldGen.SolidTile2(num58, num59))
                    {
                        num55 = num59;
                        break;
                    }
                }
                if (num55 > 0)
                {
                    break;
                }
            }
            if (num55 > 0)
            {
                num55 *= 16;
                float num60 = num55 - 800;
                if (Main.player[npc.target].position.Y > num60)
                {
                    num54 = num60;
                    if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) < 500f)
                    {
                        num53 = ((!(npc.velocity.X > 0f)) ? (Main.player[npc.target].Center.X - 600f) : (Main.player[npc.target].Center.X + 600f));
                    }
                }
            }
            else
            {
                num50 = 14f;
                num51 = 0.5f;
            }
            float num61 = num50 * 1.3f;
            float num62 = num50 * 0.7f;
            float num63 = npc.velocity.Length();
            if (num63 > 0f)
            {
                if (num63 > num61)
                {
                    npc.velocity.Normalize();
                    npc.velocity *= num61;
                }
                else if (num63 < num62)
                {
                    npc.velocity.Normalize();
                    npc.velocity *= num62;
                }
            }
            if (num55 > 0)
            {
                for (int num64 = 0; num64 < 200; num64++)
                {
                    if (Main.npc[num64].active && Main.npc[num64].type == npc.type && num64 != npc.whoAmI)
                    {
                        Vector2 vector6 = Main.npc[num64].Center - npc.Center;
                        if (vector6.Length() < 400f)
                        {
                            vector6.Normalize();
                            vector6 *= 1000f;
                            num53 -= vector6.X;
                            num54 -= vector6.Y;
                        }
                    }
                }
            }
            else
            {
                for (int num65 = 0; num65 < 200; num65++)
                {
                    if (Main.npc[num65].active && Main.npc[num65].type == npc.type && num65 != npc.whoAmI)
                    {
                        Vector2 vector7 = Main.npc[num65].Center - npc.Center;
                        if (vector7.Length() < 60f)
                        {
                            vector7.Normalize();
                            vector7 *= 200f;
                            num53 -= vector7.X;
                            num54 -= vector7.Y;
                        }
                    }
                }
            }
        }
        num53 = (int)(num53 / 16f) * 16;
        num54 = (int)(num54 / 16f) * 16;
        vector5.X = (int)(vector5.X / 16f) * 16;
        vector5.Y = (int)(vector5.Y / 16f) * 16;
        num53 -= vector5.X;
        num54 -= vector5.Y;
        if (npc.type == 375)
        {
            num53 *= -1f;
            num54 *= -1f;
        }
        float num66 = (float)Math.Sqrt(num53 * num53 + num54 * num54);
        if (npc.ai[1] > 0f && npc.ai[1] < Main.npc.Length)
        {
            try
            {
                vector5 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                num53 = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - vector5.X;
                num54 = Main.npc[(int)npc.ai[1]].position.Y + Main.npc[(int)npc.ai[1]].height / 2 - vector5.Y;
            }
            catch
            {
            }
            npc.rotation = (float)Math.Atan2(num54, num53) + 1.57f;
            num66 = (float)Math.Sqrt(num53 * num53 + num54 * num54);
            int num67 = npc.width;
            if (npc.type >= 87 && npc.type <= 92)
            {
                num67 = 42;
            }
            if (npc.type >= 454 && npc.type <= 459)
            {
                num67 = 36;
            }
            if (npc.type >= 13 && npc.type <= 15)
            {
                num67 = (int)(num67 * npc.scale);
            }
            if (npc.type >= 513 && npc.type <= 515)
            {
                num67 -= 6;
            }
            if (npc.type >= 412 && npc.type <= 414)
            {
                num67 += 6;
            }
            if (npc.type >= 621 && npc.type <= 623)
            {
                num67 = 24;
            }
            if (Main.getGoodWorld && npc.type >= 13 && npc.type <= 15)
            {
                num67 = 62;
            }
            num66 = (num66 - num67) / num66;
            num53 *= num66;
            num54 *= num66;
            npc.velocity = Vector2.Zero;
            npc.position.X += num53;
            npc.position.Y += num54;
            if (npc.type >= 87 && npc.type <= 92)
            {
                if (num53 < 0f)
                {
                    npc.spriteDirection = 1;
                }
                else if (num53 > 0f)
                {
                    npc.spriteDirection = -1;
                }
            }
            if (npc.type >= 454 && npc.type <= 459)
            {
                if (num53 < 0f)
                {
                    npc.spriteDirection = 1;
                }
                else if (num53 > 0f)
                {
                    npc.spriteDirection = -1;
                }
            }
            if (npc.type >= 621 && npc.type <= 623)
            {
                if (num53 < 0f)
                {
                    npc.spriteDirection = 1;
                }
                else if (num53 > 0f)
                {
                    npc.spriteDirection = -1;
                }
            }
        }
        else
        {
            if (!flag2)
            {
                npc.TargetClosest();
                if (npc.type == 39 && npc.velocity.Y < 0f)
                {
                    npc.velocity.Y += 0.08f;
                }
                else
                {
                    npc.velocity.Y += 0.11f;
                }
                if (npc.velocity.Y > num50)
                {
                    npc.velocity.Y = num50;
                }
                if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)num50 * 0.4)
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X -= num51 * 1.1f;
                    }
                    else
                    {
                        npc.velocity.X += num51 * 1.1f;
                    }
                }
                else if (npc.velocity.Y == num50)
                {
                    if (npc.velocity.X < num53)
                    {
                        npc.velocity.X += num51;
                    }
                    else if (npc.velocity.X > num53)
                    {
                        npc.velocity.X -= num51;
                    }
                }
                else if (npc.velocity.Y > 4f)
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X += num51 * 0.9f;
                    }
                    else
                    {
                        npc.velocity.X -= num51 * 0.9f;
                    }
                }
            }
            else
            {
                if (npc.type != 621 && npc.type != 87 && npc.type != 117 && npc.type != 454 && npc.type != 412 && npc.soundDelay == 0)
                {
                    float num68 = num66 / 40f;
                    if (num68 < 10f)
                    {
                        num68 = 10f;
                    }
                    if (num68 > 20f)
                    {
                        num68 = 20f;
                    }
                    npc.soundDelay = (int)num68;
                    SoundEngine.PlaySound(15, (int)npc.position.X, (int)npc.position.Y);
                }
                num66 = (float)Math.Sqrt(num53 * num53 + num54 * num54);
                float num69 = Math.Abs(num53);
                float num70 = Math.Abs(num54);
                float num71 = num50 / num66;
                num53 *= num71;
                num54 *= num71;
                bool flag4 = false;
                if ((npc.type == 7 || npc.type == 13) && ((!Main.player[npc.target].ZoneCorrupt && !Main.player[npc.target].ZoneCrimson) || Main.player[npc.target].dead))
                {
                    flag4 = true;
                }
                if ((npc.type == 513 && Main.player[npc.target].position.Y < Main.worldSurface * 16.0 && !Main.player[npc.target].ZoneSandstorm && !Main.player[npc.target].ZoneUndergroundDesert) || Main.player[npc.target].dead)
                {
                    flag4 = true;
                }
                if ((npc.type == 510 && Main.player[npc.target].position.Y < Main.worldSurface * 16.0 && !Main.player[npc.target].ZoneSandstorm && !Main.player[npc.target].ZoneUndergroundDesert) || Main.player[npc.target].dead)
                {
                    flag4 = true;
                }
                if (flag4)
                {
                    bool flag5 = true;
                    for (int num72 = 0; num72 < 255; num72++)
                    {
                        if (Main.player[num72].active && !Main.player[num72].dead && Main.player[num72].ZoneCorrupt)
                        {
                            flag5 = false;
                        }
                    }
                    if (flag5)
                    {
                        if (Main.netMode != 1 && (double)(npc.position.Y / 16f) > (Main.rockLayer + Main.maxTilesY) / 2.0)
                        {
                            npc.active = false;
                            int num73 = (int)npc.ai[0];
                            while (num73 > 0 && num73 < 200 && Main.npc[num73].active && Main.npc[num73].aiStyle == npc.aiStyle)
                            {
                                int num74 = (int)Main.npc[num73].ai[0];
                                Main.npc[num73].active = false;
                                npc.life = 0;
                                if (Main.netMode == 2)
                                {
                                    NetMessage.SendData(23, -1, -1, null, num73);
                                }
                                num73 = num74;
                            }
                            if (Main.netMode == 2)
                            {
                                NetMessage.SendData(23, -1, -1, null, npc.whoAmI);
                            }
                        }
                        num53 = 0f;
                        num54 = num50;
                    }
                }
                bool flag6 = false;
                if (npc.type == 87)
                {
                    if (((npc.velocity.X > 0f && num53 < 0f) || (npc.velocity.X < 0f && num53 > 0f) || (npc.velocity.Y > 0f && num54 < 0f) || (npc.velocity.Y < 0f && num54 > 0f)) && Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) > num51 / 2f && num66 < 300f)
                    {
                        flag6 = true;
                        if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num50)
                        {
                            npc.velocity *= 1.1f;
                        }
                    }
                    if (npc.position.Y > Main.player[npc.target].position.Y || (double)(Main.player[npc.target].position.Y / 16f) > Main.worldSurface || Main.player[npc.target].dead)
                    {
                        flag6 = true;
                        if (Math.Abs(npc.velocity.X) < num50 / 2f)
                        {
                            if (npc.velocity.X == 0f)
                            {
                                npc.velocity.X -= npc.direction;
                            }
                            npc.velocity.X *= 1.1f;
                        }
                        else if (npc.velocity.Y > 0f - num50)
                        {
                            npc.velocity.Y -= num51;
                        }
                    }
                }
                if (npc.type == 454 || npc.type == 621)
                {
                    float num75 = 300f;
                    if (npc.type == 621)
                    {
                        num75 = 120f;
                    }
                    if (((npc.velocity.X > 0f && num53 < 0f) || (npc.velocity.X < 0f && num53 > 0f) || (npc.velocity.Y > 0f && num54 < 0f) || (npc.velocity.Y < 0f && num54 > 0f)) && Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) > num51 / 2f && num66 < num75)
                    {
                        flag6 = true;
                        if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num50)
                        {
                            npc.velocity *= 1.1f;
                        }
                    }
                    if (npc.position.Y > Main.player[npc.target].position.Y || Main.player[npc.target].dead)
                    {
                        flag6 = true;
                        if (Math.Abs(npc.velocity.X) < num50 / 2f)
                        {
                            if (npc.velocity.X == 0f)
                            {
                                npc.velocity.X -= npc.direction;
                            }
                            npc.velocity.X *= 1.1f;
                        }
                        else if (npc.velocity.Y > 0f - num50)
                        {
                            npc.velocity.Y -= num51;
                        }
                    }
                }
                if (!flag6)
                {
                    if ((npc.velocity.X > 0f && num53 > 0f) || (npc.velocity.X < 0f && num53 < 0f) || (npc.velocity.Y > 0f && num54 > 0f) || (npc.velocity.Y < 0f && num54 < 0f))
                    {
                        if (npc.velocity.X < num53)
                        {
                            npc.velocity.X += num51;
                        }
                        else if (npc.velocity.X > num53)
                        {
                            npc.velocity.X -= num51;
                        }
                        if (npc.velocity.Y < num54)
                        {
                            npc.velocity.Y += num51;
                        }
                        else if (npc.velocity.Y > num54)
                        {
                            npc.velocity.Y -= num51;
                        }
                        if ((double)Math.Abs(num54) < (double)num50 * 0.2 && ((npc.velocity.X > 0f && num53 < 0f) || (npc.velocity.X < 0f && num53 > 0f)))
                        {
                            if (npc.velocity.Y > 0f)
                            {
                                npc.velocity.Y += num51 * 2f;
                            }
                            else
                            {
                                npc.velocity.Y -= num51 * 2f;
                            }
                        }
                        if ((double)Math.Abs(num53) < (double)num50 * 0.2 && ((npc.velocity.Y > 0f && num54 < 0f) || (npc.velocity.Y < 0f && num54 > 0f)))
                        {
                            if (npc.velocity.X > 0f)
                            {
                                npc.velocity.X += num51 * 2f;
                            }
                            else
                            {
                                npc.velocity.X -= num51 * 2f;
                            }
                        }
                    }
                    else if (num69 > num70)
                    {
                        if (npc.velocity.X < num53)
                        {
                            npc.velocity.X += num51 * 1.1f;
                        }
                        else if (npc.velocity.X > num53)
                        {
                            npc.velocity.X -= num51 * 1.1f;
                        }
                        if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)num50 * 0.5)
                        {
                            if (npc.velocity.Y > 0f)
                            {
                                npc.velocity.Y += num51;
                            }
                            else
                            {
                                npc.velocity.Y -= num51;
                            }
                        }
                    }
                    else
                    {
                        if (npc.velocity.Y < num54)
                        {
                            npc.velocity.Y += num51 * 1.1f;
                        }
                        else if (npc.velocity.Y > num54)
                        {
                            npc.velocity.Y -= num51 * 1.1f;
                        }
                        if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)num50 * 0.5)
                        {
                            if (npc.velocity.X > 0f)
                            {
                                npc.velocity.X += num51;
                            }
                            else
                            {
                                npc.velocity.X -= num51;
                            }
                        }
                    }
                }
            }
            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
            if (npc.type == 7 || npc.type == 10 || npc.type == 13 || npc.type == 39 || npc.type == 95 || npc.type == 98 || npc.type == 117 || npc.type == 510 || npc.type == 513 || npc.type == 621)
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
            if (npc.type == 454)
            {
                float num76 = Vector2.Distance(Main.player[npc.target].Center, npc.Center);
                int num77 = 0;
                if (Vector2.Normalize(Main.player[npc.target].Center - npc.Center).ToRotation().AngleTowards(npc.velocity.ToRotation(), (float)Math.PI / 2f) == npc.velocity.ToRotation() && num76 < 350f)
                {
                    num77 = 4;
                }
                if (num77 > npc.frameCounter)
                {
                    npc.frameCounter += 1.0;
                }
                if (num77 < npc.frameCounter)
                {
                    npc.frameCounter -= 1.0;
                }
                if (npc.frameCounter < 0.0)
                {
                    npc.frameCounter = 0.0;
                }
                if (npc.frameCounter > 4.0)
                {
                    npc.frameCounter = 4.0;
                }
            }
        }
        if (npc.type < 13 || npc.type > 15 || (npc.type != 13 && (npc.type == 13 || Main.npc[(int)npc.ai[1]].alpha >= 85)))
        {
            return;
        }
        if (npc.alpha > 0 && npc.life > 0)
        {
            for (int num78 = 0; num78 < 2; num78++)
            {
                int num79 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 14, 0f, 0f, 100, default, 2f);
                Main.dust[num79].noGravity = true;
                Main.dust[num79].noLight = true;
            }
        }
        if ((npc.position - npc.oldPosition).Length() > 2f)
        {
            npc.alpha -= 42;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }
        }
    }

}
