namespace VBY.NPCTest;

partial class NPCAIs
{
    public static void AI_084_LunaticCultist(NPC npc)
    {
        if (npc.ai[0] != -1f && Main.rand.Next(1000) == 0)
        {
            SoundEngine.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, Main.rand.Next(88, 92));
        }
        bool expertMode = Main.expertMode;
        bool flag = npc.life <= npc.lifeMax / 2;
        int num = 120;
        int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(35f, 25f);
        if (expertMode)
        {
            num = 90;
        }
        if (Main.getGoodWorld)
        {
            num -= 30;
        }
        int num2 = 18;
        int num3 = 3;
        int attackDamage_ForProjectiles2 = npc.GetAttackDamage_ForProjectiles(30f, 20f);
        if (expertMode)
        {
            num2 = 12;
            num3 = 4;
        }
        if (Main.getGoodWorld)
        {
            num2 = 10;
            num3 = 5;
        }
        int num4 = 80;
        int attackDamage_ForProjectiles3 = npc.GetAttackDamage_ForProjectiles(45f, 30f);
        if (expertMode)
        {
            num4 = 40;
        }
        if (Main.getGoodWorld)
        {
            num4 -= 20;
        }
        int num5 = 20;
        int num6 = 2;
        if (expertMode)
        {
            num5 = 30;
            num6 = 2;
        }
        int num7 = 20;
        int num8 = 3;
        bool flag2 = npc.type == 439;
        bool flag3 = false;
        bool flag4 = false;
        if (flag)
        {
            npc.defense = (int)(npc.defDefense * 0.65f);
        }
        if (!flag2)
        {
            if (npc.ai[3] < 0f || !Main.npc[(int)npc.ai[3]].active || Main.npc[(int)npc.ai[3]].type != 439)
            {
                npc.life = 0;
                npc.HitEffect();
                npc.active = false;
                return;
            }
            npc.ai[0] = Main.npc[(int)npc.ai[3]].ai[0];
            npc.ai[1] = Main.npc[(int)npc.ai[3]].ai[1];
            if (npc.ai[0] == 5f)
            {
                if (npc.justHit)
                {
                    npc.life = 0;
                    npc.HitEffect();
                    npc.active = false;
                    if (Main.netMode != 1)
                    {
                        NetMessage.SendData(23, -1, -1, null, npc.whoAmI);
                    }
                    NPC obj = Main.npc[(int)npc.ai[3]];
                    obj.ai[0] = 6f;
                    obj.ai[1] = 0f;
                    obj.netUpdate = true;
                }
            }
            else
            {
                flag3 = true;
                flag4 = true;
            }
        }
        else if (npc.ai[0] == 5f && npc.ai[1] >= 120f && npc.ai[1] < 420f && npc.justHit)
        {
            npc.ai[0] = 0f;
            npc.ai[1] = 0f;
            npc.ai[3] += 1f;
            npc.velocity = Vector2.Zero;
            npc.netUpdate = true;
            List<int> list = new();
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == 440 && Main.npc[i].ai[3] == npc.whoAmI)
                {
                    list.Add(i);
                }
            }
            int num9 = 10;
            if (Main.expertMode)
            {
                num9 = 3;
            }
            foreach (int item in list)
            {
                NPC nPC = Main.npc[item];
                if (nPC.localAI[1] == npc.localAI[1] && num9 > 0)
                {
                    num9--;
                    nPC.life = 0;
                    nPC.HitEffect();
                    nPC.active = false;
                    if (Main.netMode != 1)
                    {
                        NetMessage.SendData(23, -1, -1, null, item);
                    }
                }
                else if (num9 > 0)
                {
                    num9--;
                    nPC.life = 0;
                    nPC.HitEffect();
                    nPC.active = false;
                }
            }
            Main.projectile[(int)npc.ai[2]].ai[1] = -1f;
            Main.projectile[(int)npc.ai[2]].netUpdate = true;
        }
        Vector2 npcCenter = npc.Center;
        Player targetPlayer = Main.player[npc.target];
        float num10 = 5600f;
        if (npc.target < 0 || npc.target == 255 || targetPlayer.dead || !targetPlayer.active || Vector2.Distance(targetPlayer.Center, npcCenter) > num10)
        {
            npc.TargetClosest(faceTarget: false);
            targetPlayer = Main.player[npc.target];
            npc.netUpdate = true;
        }
        if (targetPlayer.dead || !targetPlayer.active || Vector2.Distance(targetPlayer.Center, npcCenter) > num10)
        {
            npc.life = 0;
            npc.HitEffect();
            npc.active = false;
            if (Main.netMode != 1)
            {
                NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f);
            }
            new List<int>().Add(npc.whoAmI);
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].active && Main.npc[j].type == 440 && Main.npc[j].ai[3] == npc.whoAmI)
                {
                    Main.npc[j].life = 0;
                    Main.npc[j].HitEffect();
                    Main.npc[j].active = false;
                    if (Main.netMode != 1)
                    {
                        NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f);
                    }
                }
            }
        }
        float num11 = npc.ai[3];
        if (npc.localAI[0] == 0f)
        {
            SoundEngine.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 89);
            npc.localAI[0] = 1f;
            npc.alpha = 255;
            npc.rotation = 0f;
            if (Main.netMode != 1)
            {
                npc.ai[0] = -1f;
                npc.netUpdate = true;
            }
        }
        if (npc.ai[0] == -1f)
        {
            npc.alpha -= 5;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }
            npc.ai[1] += 1f;
            if (npc.ai[1] >= 420f)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.netUpdate = true;
            }
            else if (npc.ai[1] > 360f)
            {
                npc.velocity *= 0.95f;
                if (npc.localAI[2] != 13f)
                {
                    SoundEngine.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 105);
                }
                npc.localAI[2] = 13f;
            }
            else if (npc.ai[1] > 300f)
            {
                npc.velocity = -Vector2.UnitY;
                npc.localAI[2] = 10f;
            }
            else if (npc.ai[1] > 120f)
            {
                npc.localAI[2] = 1f;
            }
            else
            {
                npc.localAI[2] = 0f;
            }
            flag3 = true;
            flag4 = true;
        }
        if (npc.ai[0] == 0f)
        {
            if (npc.ai[1] == 0f)
            {
                npc.TargetClosest(faceTarget: false);
            }
            npc.localAI[2] = 10f;
            int num12 = Math.Sign(targetPlayer.Center.X - npcCenter.X);
            if (num12 != 0)
            {
                npc.direction = (npc.spriteDirection = num12);
            }
            npc.ai[1] += 1f;
            if (npc.ai[1] >= 40f && flag2)
            {
                int num13 = 0;
                if (flag)
                {
                    switch ((int)npc.ai[3])
                    {
                        case 0:
                            num13 = 0;
                            break;
                        case 1:
                            num13 = 1;
                            break;
                        case 2:
                            num13 = 0;
                            break;
                        case 3:
                            num13 = 5;
                            break;
                        case 4:
                            num13 = 0;
                            break;
                        case 5:
                            num13 = 3;
                            break;
                        case 6:
                            num13 = 0;
                            break;
                        case 7:
                            num13 = 5;
                            break;
                        case 8:
                            num13 = 0;
                            break;
                        case 9:
                            num13 = 2;
                            break;
                        case 10:
                            num13 = 0;
                            break;
                        case 11:
                            num13 = 3;
                            break;
                        case 12:
                            num13 = 0;
                            break;
                        case 13:
                            num13 = 4;
                            npc.ai[3] = -1f;
                            break;
                        default:
                            npc.ai[3] = -1f;
                            break;
                    }
                }
                else
                {
                    switch ((int)npc.ai[3])
                    {
                        case 0:
                            num13 = 0;
                            break;
                        case 1:
                            num13 = 1;
                            break;
                        case 2:
                            num13 = 0;
                            break;
                        case 3:
                            num13 = 2;
                            break;
                        case 4:
                            num13 = 0;
                            break;
                        case 5:
                            num13 = 3;
                            break;
                        case 6:
                            num13 = 0;
                            break;
                        case 7:
                            num13 = 1;
                            break;
                        case 8:
                            num13 = 0;
                            break;
                        case 9:
                            num13 = 2;
                            break;
                        case 10:
                            num13 = 0;
                            break;
                        case 11:
                            num13 = 4;
                            npc.ai[3] = -1f;
                            break;
                        default:
                            npc.ai[3] = -1f;
                            break;
                    }
                }
                int maxValue = 6;
                if (npc.life < npc.lifeMax / 3)
                {
                    maxValue = 4;
                }
                if (npc.life < npc.lifeMax / 4)
                {
                    maxValue = 3;
                }
                if (expertMode && flag && Main.rand.Next(maxValue) == 0 && num13 != 0 && num13 != 4 && num13 != 5 && NPC.CountNPCS(523) < 10)
                {
                    num13 = 6;
                }
                if (num13 == 0)
                {
                    float num14 = (float)Math.Ceiling((targetPlayer.Center + new Vector2(0f, -100f) - npcCenter).Length() / 50f);
                    if (num14 == 0f)
                    {
                        num14 = 1f;
                    }
                    List<int> list2 = new();
                    int num15 = 0;
                    list2.Add(npc.whoAmI);
                    for (int k = 0; k < 200; k++)
                    {
                        if (Main.npc[k].active && Main.npc[k].type == 440 && Main.npc[k].ai[3] == npc.whoAmI)
                        {
                            list2.Add(k);
                        }
                    }
                    bool flag5 = list2.Count % 2 == 0;
                    foreach (int item2 in list2)
                    {
                        NPC nPC2 = Main.npc[item2];
                        Vector2 center2 = nPC2.Center;
                        float num16 = (num15 + flag5.ToInt() + 1) / 2 * ((float)Math.PI * 2f) * 0.4f / list2.Count;
                        if (num15 % 2 == 1)
                        {
                            num16 *= -1f;
                        }
                        if (list2.Count == 1)
                        {
                            num16 = 0f;
                        }
                        Vector2 vector = new Vector2(0f, -1f).RotatedBy(num16) * new Vector2(300f, 200f);
                        Vector2 vector2 = targetPlayer.Center + vector - center2;
                        nPC2.ai[0] = 1f;
                        nPC2.ai[1] = num14 * 2f;
                        nPC2.velocity = vector2 / num14;
                        if (npc.whoAmI >= nPC2.whoAmI)
                        {
                            nPC2.position -= nPC2.velocity;
                        }
                        nPC2.netUpdate = true;
                        num15++;
                    }
                }
                switch (num13)
                {
                    case 1:
                        npc.ai[0] = 3f;
                        npc.ai[1] = 0f;
                        break;
                    case 2:
                        npc.ai[0] = 2f;
                        npc.ai[1] = 0f;
                        break;
                    case 3:
                        npc.ai[0] = 4f;
                        npc.ai[1] = 0f;
                        break;
                    case 4:
                        npc.ai[0] = 5f;
                        npc.ai[1] = 0f;
                        break;
                }
                if (num13 == 5)
                {
                    npc.ai[0] = 7f;
                    npc.ai[1] = 0f;
                }
                if (num13 == 6)
                {
                    npc.ai[0] = 8f;
                    npc.ai[1] = 0f;
                }
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 1f)
        {
            flag3 = true;
            npc.localAI[2] = 10f;
            if ((int)npc.ai[1] % 2f != 0f && npc.ai[1] != 1f)
            {
                npc.position -= npc.velocity;
            }
            npc.ai[1] -= 1f;
            if (npc.ai[1] <= 0f)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[3] += 1f;
                npc.velocity = Vector2.Zero;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 2f)
        {
            npc.localAI[2] = 11f;
            Vector2 vec = Vector2.Normalize(targetPlayer.Center - npcCenter);
            if (npc.ai[1] >= 4f && flag2 && (int)(npc.ai[1] - 4f) % num == 0)
            {
                //if (Main.netMode != 1)
                //{
                //    List<int> list3 = new List<int>();
                //    for (int l = 0; l < 200; l++)
                //    {
                //        if (Main.npc[l].active && Main.npc[l].type == 440 && Main.npc[l].ai[3] == (float)npc.whoAmI)
                //        {
                //            list3.Add(l);
                //        }
                //    }
                //    foreach (int item3 in list3)
                //    {
                //        NPC nPC3 = Main.npc[item3];
                //        Vector2 center3 = nPC3.Center;
                //        int num17 = Math.Sign(player.Center.X - center3.X);
                //        if (num17 != 0)
                //        {
                //            nPC3.direction = (nPC3.spriteDirection = num17);
                //        }
                //        if (Main.netMode != 1)
                //        {
                //            vec = Vector2.Normalize(player.Center - center3 + player.velocity * 20f);
                //            if (vec.HasNaNs())
                //            {
                //                vec = new Vector2(npc.direction, 0f);
                //            }
                //            Vector2 vector3 = center3 + new Vector2(npc.direction * 30, 12f);
                //            for (int m = 0; m < 1; m++)
                //            {
                //                Vector2 spinninpoint = vec * (6f + (float)Main.rand.NextDouble() * 4f);
                //                spinninpoint = spinninpoint.RotatedByRandom(0.5235987901687622);
                //                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector3.X, vector3.Y, spinninpoint.X, spinninpoint.Y, 468, 18, 0f, Main.myPlayer);
                //            }
                //        }
                //    }
                //}
                //if (Main.netMode != 1)
                //{
                //    vec = Vector2.Normalize(player.Center - center + player.velocity * 20f);
                //    if (vec.HasNaNs())
                //    {
                //        vec = new Vector2(npc.direction, 0f);
                //    }
                //    Vector2 vector4 = npc.Center + new Vector2(npc.direction * 30, 12f);
                //    for (int n = 0; n < 1; n++)
                //    {
                //        Vector2 vector5 = vec * 4f;
                //        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector4.X, vector4.Y, vector5.X, vector5.Y, 464, attackDamage_ForProjectiles, 0f, Main.myPlayer, 0f, 1f);
                //    }
                //}

                List<int> shadowNPCs = new() { npc.whoAmI };
                for (int l = 0; l < 200; l++)
                {
                    if (Main.npc[l].active && Main.npc[l].type == 440 && Main.npc[l].ai[3] == npc.whoAmI)
                    {
                        shadowNPCs.Add(l);
                    }
                }
                foreach (int shadowNPCIndex in shadowNPCs)
                {
                    NPC shadowNPC = Main.npc[shadowNPCIndex];
                    Vector2 shadowNPCCenter = shadowNPC.Center;
                    int num17 = Math.Sign(targetPlayer.Center.X - shadowNPCCenter.X);
                    if (num17 != 0)
                    {
                        shadowNPC.direction = (shadowNPC.spriteDirection = num17);
                    }

                    vec = Vector2.Normalize(targetPlayer.Center - shadowNPCCenter + targetPlayer.velocity * 20f);
                    if (vec.HasNaNs())
                    {
                        vec = new Vector2(npc.direction, 0f);
                    }
                    Vector2 vector3 = shadowNPCCenter + new Vector2(npc.direction * 30, 12f);
                    for (int m = 0; m < 1; m++)
                    {
                        //Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector3.X, vector3.Y, spinninpoint.X, spinninpoint.Y, 468, 18, 0f, Main.myPlayer);
                        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector3, vec * 4f, 464, npc.GetMultiplierDamage(attackDamage_ForProjectiles), 0f, Main.myPlayer, 0, 1f);
                    }
                }
            }
            npc.ai[1] += 1f;
            if (npc.ai[1] >= 4 + num)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[3] += 1f;
                npc.velocity = Vector2.Zero;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 3f)
        {
            npc.localAI[2] = 11f;
            Vector2 vec = Vector2.Normalize(targetPlayer.Center - npcCenter);
            if (npc.ai[1] >= 4f && flag2 && (int)(npc.ai[1] - 4f) % num2 == 0)
            {
                List<int> shadowNPCs = new() { npc.whoAmI };
                for (int l = 0; l < 200; l++)
                {
                    if (Main.npc[l].active && Main.npc[l].type == 440 && Main.npc[l].ai[3] == npc.whoAmI)
                    {
                        shadowNPCs.Add(l);
                    }
                }
                foreach (int shadowNPCIndex in shadowNPCs)
                {
                    NPC shadowNPC = Main.npc[shadowNPCIndex];
                    Vector2 shadowNPCCenter = shadowNPC.Center;
                    int sign = Math.Sign(targetPlayer.Center.X - shadowNPCCenter.X);
                    if (sign != 0)
                    {
                        shadowNPC.direction = (shadowNPC.spriteDirection = sign);
                    }

                    vec = Vector2.Normalize(targetPlayer.Center - shadowNPCCenter + targetPlayer.velocity * 20f);
                    if (vec.HasNaNs())
                    {
                        vec = new Vector2(npc.direction, 0f);
                    }
                    Vector2 position = shadowNPCCenter + new Vector2(npc.direction * 30, 12f);
                    for (int m = 0; m < 1; m++)
                    {
                        Vector2 velocity = vec * (6f + (float)Main.rand.NextDouble() * 4f);
                        velocity = velocity.RotatedByRandom(0.5235987901687622);
                        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), position, velocity, 467, attackDamage_ForProjectiles, 0f, Main.myPlayer);
                    }
                }
            }
            npc.ai[1] += 1f;
            if (npc.ai[1] >= 4 + num2 * num3)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[3] += 1f;
                npc.velocity = Vector2.Zero;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 4f)
        {
            if (flag2)
            {
                npc.localAI[2] = 12f;
            }
            else
            {
                npc.localAI[2] = 11f;
            }
            if (npc.ai[1] == 20f && flag2 && Main.netMode != 1)
            {
                if ((int)(npc.ai[1] - 20f) % num4 == 0)
                {
                    List<int> shadowNPCs = new();
                    for (int l = 0; l < 200; l++)
                    {
                        if (Main.npc[l].active && Main.npc[l].type == 440 && Main.npc[l].ai[3] == npc.whoAmI)
                        {
                            shadowNPCs.Add(l);
                        }
                    }
                    foreach(var shadowNPCIndex in  shadowNPCs)
                    {
                        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), Main.npc[shadowNPCIndex].Center.X, Main.npc[shadowNPCIndex].Center.Y - 100f, 0f, 0f, 465, attackDamage_ForProjectiles3, 0f, Main.myPlayer);
                    }
                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X, npc.Center.Y - 100f, 0f, 0f, 465, attackDamage_ForProjectiles3, 0f, Main.myPlayer);
                }
            }
            npc.ai[1] += 1f;
            if (npc.ai[1] >= 20 + num4)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[3] += 1f;
                npc.velocity = Vector2.Zero;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 5f)
        {
            npc.localAI[2] = 10f;
            if (Vector2.Normalize(targetPlayer.Center - npcCenter).HasNaNs())
            {
                new Vector2(npc.direction, 0f);
            }
            if (npc.ai[1] >= 0f && npc.ai[1] < 30f)
            {
                flag3 = true;
                flag4 = true;
                float num26 = (npc.ai[1] - 0f) / 30f;
                npc.alpha = (int)(num26 * 255f);
            }
            else if (npc.ai[1] >= 30f && npc.ai[1] < 90f)
            {
                if (npc.ai[1] == 30f && Main.netMode != 1 && flag2)
                {
                    npc.localAI[1] += 1f;
                    Vector2 spinningpoint = new(180f, 0f);
                    List<int> list6 = new();
                    for (int num27 = 0; num27 < 200; num27++)
                    {
                        if (Main.npc[num27].active && Main.npc[num27].type == 440 && Main.npc[num27].ai[3] == npc.whoAmI)
                        {
                            list6.Add(num27);
                        }
                    }
                    int num28 = 6 - list6.Count;
                    if (num28 > 2)
                    {
                        num28 = 2;
                    }
                    int num29 = list6.Count + num28 + 1;
                    float[] array = new float[num29];
                    for (int num30 = 0; num30 < array.Length; num30++)
                    {
                        array[num30] = Vector2.Distance(npc.Center + spinningpoint.RotatedBy(num30 * ((float)Math.PI * 2f) / num29 - (float)Math.PI / 2f), targetPlayer.Center);
                    }
                    int num31 = 0;
                    for (int num32 = 1; num32 < array.Length; num32++)
                    {
                        if (array[num31] > array[num32])
                        {
                            num31 = num32;
                        }
                    }
                    num31 = ((num31 >= num29 / 2) ? (num31 - num29 / 2) : (num31 + num29 / 2));
                    int num33 = num28;
                    for (int num34 = 0; num34 < array.Length; num34++)
                    {
                        if (num31 != num34)
                        {
                            Vector2 center6 = npc.Center + spinningpoint.RotatedBy(num34 * ((float)Math.PI * 2f) / num29 - (float)Math.PI / 2f);
                            if (num33-- > 0)
                            {
                                int num35 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)center6.X, (int)center6.Y + npc.height / 2, 440, npc.whoAmI);
                                Main.npc[num35].ai[3] = npc.whoAmI;
                                Main.npc[num35].netUpdate = true;
                                Main.npc[num35].localAI[1] = npc.localAI[1];
                            }
                            else
                            {
                                int num36 = list6[-num33 - 1];
                                Main.npc[num36].Center = center6;
                                NetMessage.SendData(23, -1, -1, null, num36);
                            }
                        }
                    }
                    npc.ai[2] = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X, npc.Center.Y, 0f, 0f, 490, 0, 0f, Main.myPlayer, 0f, npc.whoAmI);
                    npc.Center += spinningpoint.RotatedBy(num31 * ((float)Math.PI * 2f) / num29 - (float)Math.PI / 2f);
                    npc.netUpdate = true;
                    list6.Clear();
                }
                flag3 = true;
                flag4 = true;
                npc.alpha = 255;
                if (flag2)
                {
                    Vector2 vector10 = Main.projectile[(int)npc.ai[2]].Center;
                    vector10 -= npc.Center;
                    if (vector10 == Vector2.Zero)
                    {
                        vector10 = -Vector2.UnitY;
                    }
                    vector10.Normalize();
                    if (Math.Abs(vector10.Y) < 0.77f)
                    {
                        npc.localAI[2] = 11f;
                    }
                    else if (vector10.Y < 0f)
                    {
                        npc.localAI[2] = 12f;
                    }
                    else
                    {
                        npc.localAI[2] = 10f;
                    }
                    int num37 = Math.Sign(vector10.X);
                    if (num37 != 0)
                    {
                        npc.direction = (npc.spriteDirection = num37);
                    }
                }
                else
                {
                    Vector2 vector11 = Main.projectile[(int)Main.npc[(int)npc.ai[3]].ai[2]].Center;
                    vector11 -= npc.Center;
                    if (vector11 == Vector2.Zero)
                    {
                        vector11 = -Vector2.UnitY;
                    }
                    vector11.Normalize();
                    if (Math.Abs(vector11.Y) < 0.77f)
                    {
                        npc.localAI[2] = 11f;
                    }
                    else if (vector11.Y < 0f)
                    {
                        npc.localAI[2] = 12f;
                    }
                    else
                    {
                        npc.localAI[2] = 10f;
                    }
                    int num38 = Math.Sign(vector11.X);
                    if (num38 != 0)
                    {
                        npc.direction = (npc.spriteDirection = num38);
                    }
                }
            }
            else if (npc.ai[1] >= 90f && npc.ai[1] < 120f)
            {
                flag3 = true;
                flag4 = true;
                float num39 = (npc.ai[1] - 90f) / 30f;
                npc.alpha = 255 - (int)(num39 * 255f);
            }
            else if (npc.ai[1] >= 120f && npc.ai[1] < 420f)
            {
                flag4 = true;
                npc.alpha = 0;
                if (flag2)
                {
                    Vector2 vector12 = Main.projectile[(int)npc.ai[2]].Center;
                    vector12 -= npc.Center;
                    if (vector12 == Vector2.Zero)
                    {
                        vector12 = -Vector2.UnitY;
                    }
                    vector12.Normalize();
                    if (Math.Abs(vector12.Y) < 0.77f)
                    {
                        npc.localAI[2] = 11f;
                    }
                    else if (vector12.Y < 0f)
                    {
                        npc.localAI[2] = 12f;
                    }
                    else
                    {
                        npc.localAI[2] = 10f;
                    }
                    int num40 = Math.Sign(vector12.X);
                    if (num40 != 0)
                    {
                        npc.direction = (npc.spriteDirection = num40);
                    }
                }
                else
                {
                    Vector2 vector13 = Main.projectile[(int)Main.npc[(int)npc.ai[3]].ai[2]].Center;
                    vector13 -= npc.Center;
                    if (vector13 == Vector2.Zero)
                    {
                        vector13 = -Vector2.UnitY;
                    }
                    vector13.Normalize();
                    if (Math.Abs(vector13.Y) < 0.77f)
                    {
                        npc.localAI[2] = 11f;
                    }
                    else if (vector13.Y < 0f)
                    {
                        npc.localAI[2] = 12f;
                    }
                    else
                    {
                        npc.localAI[2] = 10f;
                    }
                    int num41 = Math.Sign(vector13.X);
                    if (num41 != 0)
                    {
                        npc.direction = (npc.spriteDirection = num41);
                    }
                }
            }
            npc.ai[1] += 1f;
            if (npc.ai[1] >= 420f)
            {
                flag4 = true;
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[3] += 1f;
                npc.velocity = Vector2.Zero;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 6f)
        {
            npc.localAI[2] = 13f;
            npc.ai[1] += 1f;
            if (npc.ai[1] >= 120f)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[3] += 1f;
                npc.velocity = Vector2.Zero;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 7f)
        {
            npc.localAI[2] = 11f;
            Vector2 vec3 = Vector2.Normalize(targetPlayer.Center - npcCenter);
            if (vec3.HasNaNs())
            {
                vec3 = new Vector2(npc.direction, 0f);
            }
            if (npc.ai[1] >= 4f && flag2 && (int)(npc.ai[1] - 4f) % num5 == 0)
            {
                if ((int)(npc.ai[1] - 4f) / num5 == 2)
                {
                    List<int> list7 = new();
                    for (int num42 = 0; num42 < 200; num42++)
                    {
                        if (Main.npc[num42].active && Main.npc[num42].type == 440 && Main.npc[num42].ai[3] == npc.whoAmI)
                        {
                            list7.Add(num42);
                        }
                    }
                    foreach (int item6 in list7)
                    {
                        NPC nPC6 = Main.npc[item6];
                        Vector2 center7 = nPC6.Center;
                        int num43 = Math.Sign(targetPlayer.Center.X - center7.X);
                        if (num43 != 0)
                        {
                            nPC6.direction = (nPC6.spriteDirection = num43);
                        }
                        if (Main.netMode != 1)
                        {
                            vec3 = Vector2.Normalize(targetPlayer.Center - center7 + targetPlayer.velocity * 20f);
                            if (vec3.HasNaNs())
                            {
                                vec3 = new Vector2(npc.direction, 0f);
                            }
                            Vector2 vector14 = center7 + new Vector2(npc.direction * 30, 12f);
                            for (int num44 = 0; num44 < 5f; num44++)
                            {
                                Vector2 spinninpoint5 = vec3 * (6f + (float)Main.rand.NextDouble() * 4f);
                                spinninpoint5 = spinninpoint5.RotatedByRandom(1.2566370964050293);
                                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector14.X, vector14.Y, spinninpoint5.X, spinninpoint5.Y, 468, 18, 0f, Main.myPlayer);
                            }
                        }
                    }
                }
                int num45 = Math.Sign(targetPlayer.Center.X - npcCenter.X);
                if (num45 != 0)
                {
                    npc.direction = (npc.spriteDirection = num45);
                }
                if (Main.netMode != 1)
                {
                    vec3 = Vector2.Normalize(targetPlayer.Center - npcCenter + targetPlayer.velocity * 20f);
                    if (vec3.HasNaNs())
                    {
                        vec3 = new Vector2(npc.direction, 0f);
                    }
                    Vector2 vector15 = npc.Center + new Vector2(npc.direction * 30, 12f);
                    float num46 = 8f;
                    float num47 = (float)Math.PI * 2f / 25f;
                    for (int num48 = 0; num48 < 5f; num48++)
                    {
                        Vector2 spinningpoint2 = vec3 * num46;
                        spinningpoint2 = spinningpoint2.RotatedBy(num47 * num48 - ((float)Math.PI * 2f / 5f - num47) / 2f);
                        float ai = (Main.rand.NextFloat() - 0.5f) * 0.3f * ((float)Math.PI * 2f) / 60f;
                        int num49 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)vector15.X, (int)vector15.Y + 7, 522, 0, 0f, ai, spinningpoint2.X, spinningpoint2.Y);
                        Main.npc[num49].velocity = spinningpoint2;
                        Main.npc[num49].netUpdate = true;
                    }
                }
            }
            npc.ai[1] += 1f;
            if (npc.ai[1] >= 4 + num5 * num6)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[3] += 1f;
                npc.velocity = Vector2.Zero;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 8f)
        {
            npc.localAI[2] = 13f;
            if (npc.ai[1] >= 4f && flag2 && (int)(npc.ai[1] - 4f) % num7 == 0)
            {
                List<int> list8 = new();
                for (int num50 = 0; num50 < 200; num50++)
                {
                    if (Main.npc[num50].active && Main.npc[num50].type == 440 && Main.npc[num50].ai[3] == npc.whoAmI)
                    {
                        list8.Add(num50);
                    }
                }
                int num51 = list8.Count + 1;
                if (num51 > 3)
                {
                    num51 = 3;
                }
                int num52 = Math.Sign(targetPlayer.Center.X - npcCenter.X);
                if (num52 != 0)
                {
                    npc.direction = (npc.spriteDirection = num52);
                }
                if (Main.netMode != 1)
                {
                    for (int num53 = 0; num53 < num51; num53++)
                    {
                        Point point = npc.Center.ToTileCoordinates();
                        Point point2 = Main.player[npc.target].Center.ToTileCoordinates();
                        Vector2 vector16 = Main.player[npc.target].Center - npc.Center;
                        int num54 = 20;
                        int num55 = 3;
                        int num56 = 7;
                        int num57 = 2;
                        int num58 = 0;
                        bool flag6 = false;
                        if (vector16.Length() > 2000f)
                        {
                            flag6 = true;
                        }
                        while (!flag6 && num58 < 100)
                        {
                            num58++;
                            int num59 = Main.rand.Next(point2.X - num54, point2.X + num54 + 1);
                            int num60 = Main.rand.Next(point2.Y - num54, point2.Y + num54 + 1);
                            if ((num60 < point2.Y - num56 || num60 > point2.Y + num56 || num59 < point2.X - num56 || num59 > point2.X + num56) && (num60 < point.Y - num55 || num60 > point.Y + num55 || num59 < point.X - num55 || num59 > point.X + num55) && !Main.tile[num59, num60].nactive())
                            {
                                bool flag7 = true;
                                if (flag7 && Collision.SolidTiles(num59 - num57, num59 + num57, num60 - num57, num60 + num57))
                                {
                                    flag7 = false;
                                }
                                if (flag7)
                                {
                                    NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), num59 * 16 + 8, num60 * 16 + 8, 523, 0, npc.whoAmI);
                                    flag6 = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            npc.ai[1] += 1f;
            if (npc.ai[1] >= 4 + num7 * num8)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[3] += 1f;
                npc.velocity = Vector2.Zero;
                npc.netUpdate = true;
            }
        }
        if (!flag2)
        {
            npc.ai[3] = num11;
        }
        npc.dontTakeDamage = flag3;
        npc.chaseable = !flag4;
    }
}
