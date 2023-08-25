namespace VBY.NPCTest;

partial class NPCAIs
{
    public static void AI_054(this NPC npc)
    {
        NPC.crimsonBoss = npc.whoAmI;
        if (Main.netMode != 1 && npc.localAI[0] == 0f)
        {
            npc.localAI[0] = 1f;
            int brainOfCthuluCreepersCount = NPC.GetBrainOfCthuluCreepersCount();
            for (int num826 = 0; num826 < brainOfCthuluCreepersCount; num826++)
            {
                float x11 = npc.Center.X;
                float y6 = npc.Center.Y;
                x11 += Main.rand.Next(-npc.width, npc.width);
                y6 += Main.rand.Next(-npc.height, npc.height);
                int num827 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)x11, (int)y6, 267);
                Main.npc[num827].velocity = new Vector2(Main.rand.Next(-30, 31) * 0.1f, Main.rand.Next(-30, 31) * 0.1f);
                Main.npc[num827].netUpdate = true;
            }
        }
        if (Main.netMode != 1)
        {
            npc.TargetClosest();
            int num828 = 6000;
            if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) + Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y) > num828)
            {
                npc.active = false;
                npc.life = 0;
                if (Main.netMode == 2)
                {
                    NetMessage.SendData(23, -1, -1, null, npc.whoAmI);
                }
            }
        }
        if (npc.ai[0] < 0f)
        {
            if (Main.getGoodWorld)
            {
                NPC.brainOfGravity = npc.whoAmI;
            }
            //if (npc.localAI[2] == 0f)
            //{
            //    SoundEngine.PlaySound(3, (int)npc.position.X, (int)npc.position.Y);
            //    npc.localAI[2] = 1f;
            //    Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 392);
            //    Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 393);
            //    Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 394);
            //    Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 395);
            //    for (int num829 = 0; num829 < 20; num829++)
            //    {
            //        Dust.NewDust(npc.position, npc.width, npc.height, 5, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f);
            //    }
            //    SoundEngine.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
            //}
            npc.dontTakeDamage = false;
            npc.TargetClosest();
            Vector2 vector104 = new(npc.Center.X, npc.Center.Y);
            float num830 = Main.player[npc.target].Center.X - vector104.X;
            float num831 = Main.player[npc.target].Center.Y - vector104.Y;
            float num832 = (float)Math.Sqrt(num830 * num830 + num831 * num831);
            float num833 = 8f;
            num832 = num833 / num832;
            num830 *= num832;
            num831 *= num832;
            npc.velocity.X = (npc.velocity.X * 50f + num830) / 51f;
            npc.velocity.Y = (npc.velocity.Y * 50f + num831) / 51f;
            if (npc.ai[0] == -1f)
            {
                if (Main.netMode != 1)
                {
                    npc.localAI[1] += 1f;
                    if (npc.justHit)
                    {
                        npc.localAI[1] -= Main.rand.Next(5);
                    }
                    int num834 = 60 + Main.rand.Next(120);
                    if (Main.netMode != 0)
                    {
                        num834 += Main.rand.Next(30, 90);
                    }
                    if (npc.localAI[1] >= num834)
                    {
                        npc.localAI[1] = 0f;
                        npc.TargetClosest();
                        int num835 = 0;
                        Player player2 = Main.player[npc.target];
                        do
                        {
                            num835++;
                            int num836 = (int)player2.Center.X / 16;
                            int num837 = (int)player2.Center.Y / 16;
                            int minValue = 10;
                            int num838 = 12;
                            float num839 = 16f;
                            int num840 = Main.rand.Next(minValue, num838 + 1);
                            int num841 = Main.rand.Next(minValue, num838 + 1);
                            if (Main.rand.Next(2) == 0)
                            {
                                num840 *= -1;
                            }
                            if (Main.rand.Next(2) == 0)
                            {
                                num841 *= -1;
                            }
                            Vector2 v = new(num840 * 16, num841 * 16);
                            if (Vector2.Dot(player2.velocity.SafeNormalize(Vector2.UnitY), v.SafeNormalize(Vector2.UnitY)) > 0f)
                            {
                                v += v.SafeNormalize(Vector2.Zero) * num839 * player2.velocity.Length();
                            }
                            num836 += (int)(v.X / 16f);
                            num837 += (int)(v.Y / 16f);
                            if (num835 > 100 || !WorldGen.SolidTile(num836, num837))
                            {
                                npc.ai[3] = 0f;
                                npc.ai[0] = -2f;
                                npc.ai[1] = num836;
                                npc.ai[2] = num837;
                                npc.netUpdate = true;
                                npc.netSpam = 0;
                                break;
                            }
                        }
                        while (num835 <= 100);
                    }
                }
            }
            else if (npc.ai[0] == -2f)
            {
                npc.velocity *= 0.9f;
                if (Main.netMode != 0)
                {
                    npc.ai[3] += 15f;
                }
                else
                {
                    npc.ai[3] += 25f;
                }
                if (npc.ai[3] >= 255f)
                {
                    npc.ai[3] = 255f;
                    //二阶段瞬移
                    npc.position.X = npc.ai[1] * 16f - npc.width / 2;
                    npc.position.Y = npc.ai[2] * 16f - npc.height / 2;
                    //SoundEngine.PlaySound(SoundID.Item8, npc.Center);
                    npc.ai[0] = -3f;
                    npc.netUpdate = true;
                    npc.netSpam = 0;
                }
                npc.alpha = (int)npc.ai[3];
            }
            else if (npc.ai[0] == -3f)
            {
                if (Main.netMode == 0)
                {
                    npc.ai[3] -= 25f;
                }
                else
                {
                    npc.ai[3] -= 15f;
                }

                if (npc.ai[3] % 60 == 0 && npc.ai[3] != 0)
                {
                    var targetCenter = npc.GetTargetCenter();
                    var projVelocity = targetCenter - npc.Center;
                    switch(npc.ai[3] / 60)
                    {
                        case 4f:
                            //npc.NewProjectile(npc.Center, Vector2.Zero, 919, npc.damage / 5, projVelocity.ToRotation(), Main.rand.Next(100) / 100);
                            Main.projectile[npc.NewProjectile(npc.Center, projVelocity.Normalize(2), 348, npc.damage / 5)].timeLeft = 300;
                            break;
                        case 3f:
                            //npc.NewProjectile(targetCenter + projVelocity, Vector2.Zero, 919, npc.damage / 5, (-projVelocity).ToRotation(), Main.rand.Next(100) / 100);
                            Main.projectile[npc.NewProjectile(targetCenter + projVelocity, (-projVelocity).Normalize(2), 348, npc.damage / 5)].timeLeft = 300;
                            break;
                        case 2f:
                            //npc.NewProjectile(targetCenter + new Vector2(projVelocity.X, -projVelocity.Y), Vector2.Zero, 919, npc.damage / 5, new Vector2(-projVelocity.X, projVelocity.Y).ToRotation(), Main.rand.Next(100) / 100);
                            Main.projectile[npc.NewProjectile(targetCenter + new Vector2(projVelocity.X, -projVelocity.Y), new Vector2(-projVelocity.X, projVelocity.Y).Normalize(2), 348, npc.damage / 5)].timeLeft = 300;
                            break;
                        case 1f:
                            //npc.NewProjectile(targetCenter + new Vector2(-projVelocity.X, projVelocity.Y), Vector2.Zero, 919, npc.damage / 5, new Vector2(projVelocity.X, -projVelocity.Y).ToRotation(), Main.rand.Next(100) / 100);
                            Main.projectile[npc.NewProjectile(targetCenter + new Vector2(-projVelocity.X, projVelocity.Y), new Vector2(projVelocity.X, -projVelocity.Y).Normalize(2), 348, npc.damage / 5)].timeLeft = 300;
                            break;
                    }
                }

                if (npc.ai[3] <= 0f)
                {
                    npc.ai[3] = 0f;
                    npc.ai[0] = -1f;
                    npc.netUpdate = true;
                    npc.netSpam = 0;
                }
                npc.alpha = (int)npc.ai[3];
            }
        }
        else
        {
            npc.TargetClosest();
            Vector2 vector105 = new(npc.Center.X, npc.Center.Y);
            float num842 = Main.player[npc.target].Center.X - vector105.X;
            float num843 = Main.player[npc.target].Center.Y - vector105.Y;
            float num844 = (float)Math.Sqrt(num842 * num842 + num843 * num843);
            float num845 = 1f;
            if (Main.getGoodWorld)
            {
                num845 *= 3f;
            }
            if (num844 < num845)
            {
                npc.velocity.X = num842;
                npc.velocity.Y = num843;
            }
            else
            {
                num844 = num845 / num844;
                npc.velocity.X = num842 * num844;
                npc.velocity.Y = num843 * num844;
            }
            if (npc.ai[0] == 0f)
            {
                if (Main.netMode != 1)
                {
                    int num846 = 0;
                    for (int num847 = 0; num847 < 200; num847++)
                    {
                        if (Main.npc[num847].active && Main.npc[num847].type == 267)
                        {
                            num846++;
                        }
                    }
                    if (num846 == 0)
                    {
                        npc.ai[0] = -1f;
                        npc.localAI[1] = 0f;
                        npc.alpha = 0;
                        npc.netUpdate = true;
                    }
                    npc.localAI[1] += 1f;
                    if (npc.localAI[1] >= 120 + Main.rand.Next(300))
                    {
                        npc.localAI[1] = 0f;
                        npc.TargetClosest();
                        int num848 = 0;
                        Player player3 = Main.player[npc.target];
                        do
                        {
                            num848++;
                            int num849 = (int)player3.Center.X / 16;
                            int num850 = (int)player3.Center.Y / 16;
                            int minValue2 = 12;
                            int num851 = 40;
                            float num852 = 16f;
                            int num853 = Main.rand.Next(minValue2, num851 + 1);
                            int num854 = Main.rand.Next(minValue2, num851 + 1);
                            if (Main.rand.Next(2) == 0)
                            {
                                num853 *= -1;
                            }
                            if (Main.rand.Next(2) == 0)
                            {
                                num854 *= -1;
                            }
                            Vector2 v2 = new(num853 * 16, num854 * 16);
                            if (Vector2.Dot(player3.velocity.SafeNormalize(Vector2.UnitY), v2.SafeNormalize(Vector2.UnitY)) > 0f)
                            {
                                v2 += v2.SafeNormalize(Vector2.Zero) * num852 * player3.velocity.Length();
                            }
                            num849 += (int)(v2.X / 16f);
                            num850 += (int)(v2.Y / 16f);
                            if (num848 > 100 || (!WorldGen.SolidTile(num849, num850) && (num848 > 75 || Collision.CanHit(new Vector2(num849 * 16, num850 * 16), 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))))
                            {
                                npc.ai[0] = 1f;
                                npc.ai[1] = num849;
                                npc.ai[2] = num850;
                                npc.netUpdate = true;
                                break;
                            }
                        }
                        while (num848 <= 100);
                    }
                }
            }
            else if (npc.ai[0] == 1f)
            {
                npc.alpha += 5;
                if (npc.alpha >= 255)
                {
                    //SoundEngine.PlaySound(SoundID.Item8, npc.Center);
                    npc.alpha = 255;
                    npc.position.X = npc.ai[1] * 16f - npc.width / 2;
                    npc.position.Y = npc.ai[2] * 16f - npc.height / 2;



                    npc.ai[0] = 2f;
                }
            }
            else if (npc.ai[0] == 2f)
            {
                npc.alpha -= 5;
                if (npc.alpha <= 0)
                {
                    npc.alpha = 0;
                    npc.ai[0] = 0f;
                }
            }
        }
        if (Main.player[npc.target].dead || !Main.player[npc.target].ZoneCrimson)
        {
            if (npc.localAI[3] < 120f)
            {
                npc.localAI[3]++;
            }
            if (npc.localAI[3] > 60f)
            {
                npc.velocity.Y += (npc.localAI[3] - 60f) * 0.25f;
            }
            npc.ai[0] = 2f;
            npc.alpha = 10;
        }
        else if (npc.localAI[3] > 0f)
        {
            npc.localAI[3]--;
        }
    }
}
