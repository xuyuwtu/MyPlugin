using Terraria.DataStructures;

namespace VBY.NPCTest;
partial class NPCAIs
{
    public static void AI_120_HallowBoss(NPC npc)
    {
        //npc.AIOutput();
        Vector2 vector = new(-150f, -250f);
        Vector2 vector2 = new(150f, -250f);
        Vector2 vector3 = new(0f, -350f);
        Vector2 vector4 = new(0f, -350f);
        Vector2 vector5 = new(-80f, -500f);
        float num = 0.5f;
        float num2 = 12f;
        float num3 = 40f;
        float num4 = 6400f;
        int num5 = 40;
        int fairyQueenLanceProjDamage = 50;
        int num7 = 70;
        int num8 = 45;
        int num9 = 45;
        int fairyQueenSunDanceProjDamage = 50;
        bool flag = npc.AI_120_HallowBoss_IsInPhase2();
        bool flag2 = Main.expertMode;
        bool flag3 = flag && flag2;
        bool flag4 = NPC.ShouldEmpressBeEnraged();
        if (npc.life == npc.lifeMax && flag4 && !npc.AI_120_HallowBoss_IsGenuinelyEnraged())
        {
            npc.ai[3] += 2f;
        }
        bool flag5 = true;
        int num11 = 30;
        int num12 = 30;
        int num13 = 30;
        int num14 = 35;
        int num15 = 65;
        if (flag)
        {
            fairyQueenLanceProjDamage = 60;
            num8 = 50;
            num9 = 50;
            fairyQueenSunDanceProjDamage = 60;
            num7 = 65;
            num11 = 35;
            num12 = 35;
            num13 = 35;
            num14 = 40;
            num15 = 30;
        }
        fairyQueenLanceProjDamage = npc.GetAttackDamage_ForProjectiles(fairyQueenLanceProjDamage, num11);
        num8 = npc.GetAttackDamage_ForProjectiles(num8, num12);
        num9 = npc.GetAttackDamage_ForProjectiles(num9, num13);
        fairyQueenSunDanceProjDamage = npc.GetAttackDamage_ForProjectiles(fairyQueenSunDanceProjDamage, num14);
        num7 = npc.GetAttackDamage_ForProjectiles(num7, num15);
        if (flag4)
        {
            fairyQueenLanceProjDamage = 9999;
            num8 = 9999;
            num9 = 9999;
            fairyQueenSunDanceProjDamage = 9999;
            num7 = 9999;
            flag2 = true;
        }
        float num16 = (flag2 ? 0.3f : 1f);
        bool flag6 = true;
        int num17 = 0;
        if (flag)
        {
            num17 += 15;
        }
        if (flag2)
        {
            num17 += 5;
        }
        switch ((int)npc.ai[0])
        {
            case 0:
                if (npc.ai[1] == 0f)
                {
                    npc.velocity = new Vector2(0f, 5f);
                    if (Main.netMode != 1)
                    {
                        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center + new Vector2(0f, -80f), Vector2.Zero, 874, 0, 0f, Main.myPlayer);
                    }
                }
                if (npc.ai[1] == 10f)
                {
                    SoundEngine.PlaySound(SoundID.Item161, npc.Center);
                }
                npc.velocity *= 0.95f;
                //if (npc.ai[1] > 10f && npc.ai[1] < 150f)
                //{
                //    int num67 = 2;
                //    for (int m = 0; m < num67; m++)
                //    {
                //        float num68 = MathHelper.Lerp(1.3f, 0.7f, npc.Opacity) * Utils.GetLerpValue(0f, 120f, npc.ai[1], clamped: true);
                //        Color newColor2 = Main.hslToRgb(npc.ai[1] / 180f, 1f, 0.5f);
                //        int num69 = Dust.NewDust(npc.position, npc.width, npc.height, 267, 0f, 0f, 0, newColor2);
                //        Main.dust[num69].position = npc.Center + Main.rand.NextVector2Circular(npc.width * 3f, npc.height * 3f) + new Vector2(0f, -150f);
                //        Main.dust[num69].velocity *= Main.rand.NextFloat() * 0.8f;
                //        Main.dust[num69].noGravity = true;
                //        Main.dust[num69].fadeIn = 0.6f + Main.rand.NextFloat() * 0.7f * num68;
                //        Main.dust[num69].velocity += Vector2.UnitY * 3f;
                //        Main.dust[num69].scale = 0.35f;
                //        if (num69 != 6000)
                //        {
                //            Dust dust2 = Dust.CloneDust(num69);
                //            dust2.scale /= 2f;
                //            dust2.fadeIn *= 0.85f;
                //            dust2.color = new Color(255, 255, 255, 255);
                //        }
                //    }
                //}
                npc.ai[1] += 1f;
                flag5 = false;
                flag6 = false;
                npc.Opacity = MathHelper.Clamp(npc.ai[1] / 180f, 0f, 1f);
                if (npc.ai[1] >= 180f)
                {
                    if (flag4 && !npc.AI_120_HallowBoss_IsGenuinelyEnraged())
                    {
                        npc.ai[3] += 2f;
                    }
                    npc.ai[0] = 1f;
                    npc.ai[1] = 0f;
                    npc.netUpdate = true;
                    npc.TargetClosest();
                }
                break;
            case 1:
                {
                    float num34 = (flag ? 20f : 45f);
                    if (Main.getGoodWorld)
                    {
                        num34 /= 2f;
                    }
                    if (npc.ai[1] <= 10f)
                    {
                        if (npc.ai[1] == 0f)
                        {
                            npc.TargetClosest();
                        }
                        NPCAimedTarget targetData4 = npc.GetTargetData();
                        if (targetData4.Invalid)
                        {
                            npc.ai[0] = 13f;
                            npc.ai[1] = 0f;
                            npc.ai[2] += 1f;
                            npc.velocity /= 4f;
                            npc.netUpdate = true;
                            break;
                        }
                        Vector2 center = targetData4.Center;
                        npc.AI_120_HallowBoss_DashTo(center);
                        npc.netUpdate = true;
                    }
                    if (npc.velocity.Length() > 16f && npc.ai[1] > 10f)
                    {
                        npc.velocity /= 2f;
                    }
                    npc.velocity *= 0.92f;
                    npc.ai[1] += 1f;
                    if (!(npc.ai[1] >= num34))
                    {
                        break;
                    }
                    int num35 = (int)npc.ai[2];
                    int num36 = 2;
                    int num37 = 0;
                    if (!flag)
                    {
                        int num38 = num37++;
                        int num39 = num37++;
                        int num40 = num37++;
                        int num41 = num37++;
                        int num42 = num37++;
                        int num43 = num37++;
                        int num44 = num37++;
                        int num45 = num37++;
                        int num46 = num37++;
                        int num47 = num37++;
                        if (num35 % num37 == num38)
                        {
                            num36 = 2;
                        }
                        if (num35 % num37 == num39)
                        {
                            num36 = 8;
                        }
                        if (num35 % num37 == num40)
                        {
                            num36 = 6;
                        }
                        if (num35 % num37 == num41)
                        {
                            num36 = 8;
                        }
                        if (num35 % num37 == num42)
                        {
                            num36 = 5;
                        }
                        if (num35 % num37 == num43)
                        {
                            num36 = 2;
                        }
                        if (num35 % num37 == num44)
                        {
                            num36 = 8;
                        }
                        if (num35 % num37 == num45)
                        {
                            num36 = 4;
                        }
                        if (num35 % num37 == num46)
                        {
                            num36 = 8;
                        }
                        if (num35 % num37 == num47)
                        {
                            num36 = 5;
                        }
                        if (npc.life / (float)npc.lifeMax <= 0.5f)
                        {
                            num36 = 10;
                        }
                    }
                    if (flag)
                    {
                        int num48 = num37++;
                        int num49 = num37++;
                        int num50 = num37++;
                        int num51 = -1;
                        if (flag2)
                        {
                            num51 = num37++;
                        }
                        int num52 = num37++;
                        int num53 = num37++;
                        int num54 = num37++;
                        int num55 = num37++;
                        int num56 = num37++;
                        int num57 = num37++;
                        if (num35 % num37 == num48)
                        {
                            num36 = 7;
                        }
                        if (num35 % num37 == num49)
                        {
                            num36 = 2;
                        }
                        if (num35 % num37 == num50)
                        {
                            num36 = 8;
                        }
                        if (num35 % num37 == num52)
                        {
                            num36 = 5;
                        }
                        if (num35 % num37 == num53)
                        {
                            num36 = 2;
                        }
                        if (num35 % num37 == num54)
                        {
                            num36 = 6;
                        }
                        if (num35 % num37 == num54)
                        {
                            num36 = 6;
                        }
                        if (num35 % num37 == num55)
                        {
                            num36 = 4;
                        }
                        if (num35 % num37 == num56)
                        {
                            num36 = 8;
                        }
                        if (num35 % num37 == num51)
                        {
                            num36 = 11;
                        }
                        if (num35 % num37 == num57)
                        {
                            num36 = 12;
                        }
                    }
                    npc.TargetClosest();
                    NPCAimedTarget targetData5 = npc.GetTargetData();
                    bool flag12 = false;
                    if (npc.AI_120_HallowBoss_IsGenuinelyEnraged())
                    {
                        if (!Main.dayTime)
                        {
                            flag12 = true;
                        }
                        if (Main.dayTime && Main.time >= 53400.0)
                        {
                            flag12 = true;
                        }
                    }
                    if (targetData5.Invalid || npc.Distance(targetData5.Center) > num4 || flag12)
                    {
                        num36 = 13;
                    }
                    if (num36 == 8 && targetData5.Center.X > npc.Center.X)
                    {
                        num36 = 9;
                    }
                    if (flag2 && num36 != 5 && num36 != 12)
                    {
                        npc.velocity = npc.DirectionFrom(targetData5.Center).SafeNormalize(Vector2.Zero).RotatedBy((float)Math.PI / 2f * (targetData5.Center.X > npc.Center.X).ToDirectionInt()) * 20f;
                    }
                    npc.ai[0] = num36;
                    npc.ai[1] = 0f;
                    npc.ai[2] += 1f;
                    npc.netUpdate = true;
                    break;
                }
            case 2:
                {
                    if (npc.ai[1] == 0f)
                    {
                        SoundEngine.PlaySound(SoundID.Item164, npc.Center);
                    }
                    float num90 = 90f - num17;
                    Vector2 vector36 = new(-55f, -30f);
                    NPCAimedTarget targetData11 = npc.GetTargetData();
                    Vector2 vector37 = (targetData11.Invalid ? npc.Center : targetData11.Center);
                    if (npc.Distance(vector37 + vector) > num3)
                    {
                        npc.SimpleFlyMovement(npc.DirectionTo(vector37 + vector).SafeNormalize(Vector2.Zero) * num2, num);
                    }
                    if (npc.ai[1] < 60f)
                    {
                        npc.AI_120_HallowBoss_DoMagicEffect(npc.Center + vector36, 1, Utils.GetLerpValue(0f, 60f, npc.ai[1], clamped: true));
                    }
                    int num91 = 3;
                    if (flag2)
                    {
                        num91 = 2;
                    }
                    if ((int)npc.ai[1] % num91 == 0 && npc.ai[1] < 60f)
                    {
                        float ai3 = npc.ai[1] / 60f;
                        Vector2 vector38 = new Vector2(0f, -6f).RotatedBy((float)Math.PI / 2f * Main.rand.NextFloatDirection());
                        if (flag3)
                        {
                            vector38 = new Vector2(0f, -10f).RotatedBy((float)Math.PI * 2f * Main.rand.NextFloat());
                        }
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center + vector36, vector38, 873, num8, 0f, Main.myPlayer, npc.target, ai3);
                        }
                        if (Main.netMode != 1)
                        {
                            int num92 = (int)(npc.ai[1] / num91);
                            for (int num93 = 0; num93 < 255; num93++)
                            {
                                if (npc.Boss_CanShootExtraAt(num93, num92 % 3, 3, 2400f))
                                {
                                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center + vector36, vector38, 873, num8, 0f, Main.myPlayer, num93, ai3);
                                }
                            }
                        }
                    }
                    npc.ai[1] += 1f;
                    if (npc.ai[1] >= 60f + num90)
                    {
                        npc.ai[0] = 1f;
                        npc.ai[1] = 0f;
                        npc.netUpdate = true;
                    }
                    break;
                }
            case 3:
                {
                    npc.ai[1] += 1f;
                    NPCAimedTarget targetData8 = npc.GetTargetData();
                    Vector2 vector23 = (targetData8.Invalid ? npc.Center : targetData8.Center);
                    if (npc.Distance(vector23 + vector2) > num3)
                    {
                        npc.SimpleFlyMovement(npc.DirectionTo(vector23 + vector2).SafeNormalize(Vector2.Zero) * num2, num);
                    }
                    if ((int)npc.ai[1] % 180 == 0)
                    {
                        Vector2 vector24 = new(0f, -100f);
                        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), targetData8.Center + vector24, Vector2.Zero, 874, num5, 0f, Main.myPlayer);
                    }
                    if (npc.ai[1] >= 120f)
                    {
                        npc.ai[0] = 1f;
                        npc.ai[1] = 0f;
                        npc.netUpdate = true;
                    }
                    break;
                }
            case 4:
                {
                    float num81 = 20 - num17;
                    //if (npc.ai[1] == 0f)
                    //{
                    //    SoundEngine.PlaySound(SoundID.Item162, npc.Center);
                    //}
                    if (npc.ai[1] >= 6f && npc.ai[1] < 54f)
                    {
                        npc.AI_120_HallowBoss_DoMagicEffect(npc.Center + new Vector2(-55f, -20f), 2, Utils.GetLerpValue(0f, 100f, npc.ai[1], clamped: true));
                        npc.AI_120_HallowBoss_DoMagicEffect(npc.Center + new Vector2(55f, -20f), 4, Utils.GetLerpValue(0f, 100f, npc.ai[1], clamped: true));
                    }
                    NPCAimedTarget targetData10 = npc.GetTargetData();
                    Vector2 vector29 = (targetData10.Invalid ? npc.Center : targetData10.Center);
                    if (npc.Distance(vector29 + vector3) > num3)
                    {
                        npc.SimpleFlyMovement(npc.DirectionTo(vector29 + vector3).SafeNormalize(Vector2.Zero) * num2, num);
                    }
                    int num82 = 4;
                    if (flag2)
                    {
                        num82 = 5;
                    }
                    if ((int)npc.ai[1] % 4 == 0 && npc.ai[1] < 100f)
                    {
                        int num83 = 1;
                        for (int n = 0; n < num83; n++)
                        {
                            int num85 = (int)npc.ai[1] / 4;
                            Vector2 vector30 = Vector2.UnitX.RotatedBy((float)Math.PI / (num82 * 2) + num85 * ((float)Math.PI / num82) + 0f);
                            if (!flag2)
                            {
                                vector30.X += ((vector30.X > 0f) ? 0.5f : (-0.5f));
                            }
                            vector30.Normalize();
                            float num86 = 300f;
                            if (flag2)
                            {
                                num86 = 450f;
                            }
                            Vector2 center4 = targetData10.Center;
                            if (npc.Distance(center4) > 2400f)
                            {
                                continue;
                            }
                            if (Vector2.Dot(targetData10.Velocity.SafeNormalize(Vector2.UnitY), vector30) > 0f)
                            {
                                vector30 *= -1f;
                            }
                            int num87 = 90;
                            Vector2 vector31 = center4 + targetData10.Velocity * num87;
                            Vector2 vector32 = center4 + vector30 * num86 - targetData10.Velocity * 30f;
                            if (vector32.Distance(center4) < num86)
                            {
                                Vector2 vector33 = center4 - vector32;
                                if (vector33 == Vector2.Zero)
                                {
                                    vector33 = vector30;
                                }
                                vector32 = center4 - Vector2.Normalize(vector33) * num86;
                            }
                            Vector2 v3 = vector31 - vector32;
                            if (Main.netMode != 1)
                            {
                                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector32, Vector2.Zero, 919, fairyQueenLanceProjDamage, 0f, Main.myPlayer, v3.ToRotation(), npc.ai[1] / 100f);
                            }
                            if (Main.netMode == 1)
                            {
                                continue;
                            }
                            int num88 = (int)(npc.ai[1] / 4f);
                            for (int num89 = 0; num89 < 255; num89++)
                            {
                                if (!npc.Boss_CanShootExtraAt(num89, num88 % 3, 3, 2400f))
                                {
                                    continue;
                                }
                                Player player2 = Main.player[num89];
                                center4 = player2.Center;
                                if (Vector2.Dot(player2.velocity.SafeNormalize(Vector2.UnitY), vector30) > 0f)
                                {
                                    vector30 *= -1f;
                                }
                                Vector2 vector34 = center4 + player2.velocity * num87;
                                vector32 = center4 + vector30 * num86 - player2.velocity * 30f;
                                if (vector32.Distance(center4) < num86)
                                {
                                    Vector2 vector35 = center4 - vector32;
                                    if (vector35 == Vector2.Zero)
                                    {
                                        vector35 = vector30;
                                    }
                                    vector32 = center4 - Vector2.Normalize(vector35) * num86;
                                }
                                v3 = vector34 - vector32;
                                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector32, Vector2.Zero, 919, fairyQueenLanceProjDamage, 0f, Main.myPlayer, v3.ToRotation(), npc.ai[1] / 100f);
                            }
                        }
                    }
                    npc.ai[1] += 1f;
                    if (npc.ai[1] >= 100f + num81)
                    {
                        npc.ai[0] = 1f;
                        npc.ai[1] = 0f;
                        npc.netUpdate = true;
                    }
                    break;
                }
            case 5:
                {
                    if (npc.ai[1] == 0f)
                    {
                        SoundEngine.PlaySound(SoundID.Item163, npc.Center);
                    }
                    float num63 = 30f;
                    num63 -= num17;
                    Vector2 vector19 = new(55f, -30f);
                    Vector2 vector20 = npc.Center + vector19;
                    if (npc.ai[1] < 42f)
                    {
                        npc.AI_120_HallowBoss_DoMagicEffect(npc.Center + vector19, 3, Utils.GetLerpValue(0f, 42f, npc.ai[1], clamped: true));
                    }
                    NPCAimedTarget targetData7 = npc.GetTargetData();
                    Vector2 vector21 = (targetData7.Invalid ? npc.Center : targetData7.Center);
                    if (npc.Distance(vector21 + vector4) > num3)
                    {
                        npc.SimpleFlyMovement(npc.DirectionTo(vector21 + vector4).SafeNormalize(Vector2.Zero) * num2, num);
                    }
                    if ((int)npc.ai[1] % 42 == 0 && npc.ai[1] < 42f)
                    {
                        float num64 = (float)Math.PI * 2f * Main.rand.NextFloat();
                        for (float num65 = 0f; num65 < 1f; num65 += 1f / 13f)
                        {
                            float num66 = num65;
                            Vector2 vector22 = Vector2.UnitY.RotatedBy((float)Math.PI / 2f + (float)Math.PI * 2f * num66 + num64);
                            if (Main.netMode != 1)
                            {
                                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector20 + vector22.RotatedBy(-1.5707963705062866) * 30f, vector22 * 8f, 872, num9, 0f, Main.myPlayer, 0f, num66);
                            }
                        }
                    }
                    npc.ai[1] += 1f;
                    if (npc.ai[1] >= 42f + num63)
                    {
                        npc.ai[0] = 1f;
                        npc.ai[1] = 0f;
                        npc.netUpdate = true;
                    }
                    break;
                }
            case 6:
                {
                    float num25 = 120 - num17;
                    Vector2 vector8 = new(0f, -100f);
                    Vector2 vector9 = npc.Center + vector8;
                    NPCAimedTarget targetData2 = npc.GetTargetData();
                    Vector2 vector10 = (targetData2.Invalid ? npc.Center : targetData2.Center);
                    if (npc.Distance(vector10 + vector5) > num3)
                    {
                        npc.SimpleFlyMovement(npc.DirectionTo(vector10 + vector5).SafeNormalize(Vector2.Zero) * num2 * 0.3f, num * 0.7f);
                    }
                    if ((int)npc.ai[1] % 60 == 0 && npc.ai[1] < 180f)
                    {
                        int ainum60 = (int)npc.ai[1] / 60;
                        int num27 = ((targetData2.Center.X > npc.Center.X) ? 1 : 0);
                        float num28 = 6f;
                        if (flag2)
                        {
                            num28 = 8f;
                        }
                        float projNum = 1f / num28;
                        for (float i = 0f; i < 1f; i += projNum)
                        {
                            float num31 = (i + projNum * 0.5f + ainum60 * projNum * 0.5f) % 1f;
                            float ai = (float)Math.PI * 2f * (num31 + num27);
                            if (Main.netMode != 1)
                            {
                                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector9, Vector2.Zero, 923, fairyQueenSunDanceProjDamage, 0f, Main.myPlayer, ai, npc.whoAmI);
                            }
                        }
                    }
                    npc.ai[1] += 1f;
                    if (npc.ai[1] >= 180f + num25)
                    {
                        npc.ai[0] = 1f;
                        npc.ai[1] = 0f;
                        npc.netUpdate = true;
                    }
                    break;
                }
            case 7:
                {
                    float num70 = 20f;
                    float num71 = 60f;
                    float num72 = num71 * 4f;
                    if (flag2)
                    {
                        num70 = 40f;
                        num71 = 40f;
                        num72 = num71 * 6f;
                    }
                    num70 -= num17;
                    NPCAimedTarget targetData9 = npc.GetTargetData();
                    Vector2 vector25 = (targetData9.Invalid ? npc.Center : targetData9.Center);
                    if (npc.Distance(vector25 + vector4) > num3)
                    {
                        npc.SimpleFlyMovement(npc.DirectionTo(vector25 + vector4).SafeNormalize(Vector2.Zero) * num2 * 0.4f, num);
                    }
                    if ((int)npc.ai[1] % num71 == 0f && npc.ai[1] < num72)
                    {
                        SoundEngine.PlaySound(SoundID.Item162, npc.Center);
                        Main.rand.NextFloat();
                        int num73 = (int)npc.ai[1] / (int)num71;
                        float num74 = 13f;
                        float num75 = 150f;
                        float num76 = num74 * num75;
                        Vector2 center3 = targetData9.Center;
                        if (npc.Distance(center3) <= 3200f)
                        {
                            Vector2 vector26 = Vector2.Zero;
                            Vector2 vector27 = Vector2.UnitY;
                            float num77 = 0.4f;
                            float num78 = 1.4f;
                            float num79 = 1f;
                            if (flag2)
                            {
                                num74 += 5f;
                                num75 += 50f;
                                num79 *= 1f;
                                num76 *= 0.5f;
                            }
                            switch (num73)
                            {
                                case 0:
                                    center3 += new Vector2((0f - num76) / 2f, 0f) * num79;
                                    vector26 = new Vector2(0f, num76);
                                    vector27 = Vector2.UnitX;
                                    break;
                                case 1:
                                    center3 += new Vector2(num76 / 2f, num75 / 2f) * num79;
                                    vector26 = new Vector2(0f, num76);
                                    vector27 = -Vector2.UnitX;
                                    break;
                                case 2:
                                    center3 += new Vector2(0f - num76, 0f - num76) * num77 * num79;
                                    vector26 = new Vector2(num76 * num78, 0f);
                                    vector27 = new Vector2(1f, 1f);
                                    break;
                                case 3:
                                    center3 += new Vector2(num76 * num77 + num75 / 2f, (0f - num76) * num77) * num79;
                                    vector26 = new Vector2((0f - num76) * num78, 0f);
                                    vector27 = new Vector2(-1f, 1f);
                                    break;
                                case 4:
                                    center3 += new Vector2(0f - num76, num76) * num77 * num79;
                                    vector26 = new Vector2(num76 * num78, 0f);
                                    vector27 = center3.DirectionTo(targetData9.Center);
                                    break;
                                case 5:
                                    center3 += new Vector2(num76 * num77 + num75 / 2f, num76 * num77) * num79;
                                    vector26 = new Vector2((0f - num76) * num78, 0f);
                                    vector27 = center3.DirectionTo(targetData9.Center);
                                    break;
                            }
                            float add = num74 / 59;
                            float count = 0;
                            for (float num80 = 0f; num80 <= 1f; num80 += 1f / num74)
                            {
                                Vector2 origin = center3 + vector26 * (num80 - 0.5f);
                                Vector2 v2 = vector27;
                                if (flag2)
                                {
                                    Vector2 vector28 = targetData9.Velocity * 20f * num80;
                                    Vector2 value2 = origin.DirectionTo(targetData9.Center + vector28);
                                    v2 = Vector2.Lerp(vector27, value2, 0.75f).SafeNormalize(Vector2.UnitY);
                                }
                                float ai2 = num80;
                                var ro = v2.RotatedByRandom(2);
                                ro *= 3f;
                                if (Main.netMode != 1)
                                {
                                    //原版
                                    //Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), origin, Vector2.Zero, 919, num7, 0f, Main.myPlayer, v2.ToRotation(), ai2);
                                    //迷惑版
                                    npc.NewProjectile(origin - (ro * 59), ro, 919, num7, v2.ToRotation(), ai2);
                                    //预判版
                                    //npc.NewProjectile(origin + targetData9.Velocity * 59, Vector2.Zero, 919, num7, v2.ToRotation(), ai2);
                                    //迷惑预判版
                                    //npc.NewProjectile(origin - (ro * 59) + targetData9.Velocity * 59, ro, 919, num7, v2.ToRotation(), ai2);
                                    //迷惑偏移预判版
                                    //npc.NewProjectile(origin - (ro * 59) + targetData9.Velocity * (59 - count), ro, 919, num7, v2.ToRotation(), ai2);
                                }
                                count += add;
                            }
                        }
                    }
                    npc.ai[1] += 1f;
                    if (npc.ai[1] >= num72 + num70)
                    {
                        npc.ai[0] = 1f;
                        npc.ai[1] = 0f;
                        npc.netUpdate = true;
                    }
                    break;
                }
            case 8:
            case 9:
                {
                    if (npc.ai[1] == 0)
                    {
                        var rotatedVector = new Vector2(npc.velocity.X < 0 ? 1f : -1f, 0);
                        npc.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.FairyQueenSunDance, fairyQueenSunDanceProjDamage, rotatedVector.RotatedByDegress(15).ToRotation(), npc.whoAmI);
                        npc.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.FairyQueenSunDance, fairyQueenSunDanceProjDamage, rotatedVector.RotatedByDegress(-15).ToRotation(), npc.whoAmI);
                        npc.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.FairyQueenSunDance, fairyQueenSunDanceProjDamage, rotatedVector.RotatedByDegress(8).ToRotation(), npc.whoAmI);
                        npc.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.FairyQueenSunDance, fairyQueenSunDanceProjDamage, rotatedVector.RotatedByDegress(-8).ToRotation(), npc.whoAmI);
                    }
                    if (npc.ai[1] >= 50 && npc.ai[1] % 15 == 0)
                    {
                        var right = npc.velocity.X < 0;
                        var rotatedVector = new Vector2(npc.velocity.X < 0 ? 1f : -1f, 0);
                        for (int i = 0; i < Main.maxProjectiles; i++)
                        {
                            if (Main.projectile[i] is not null && Main.projectile[i].active && Main.projectile[i].type == ProjectileID.FairyQueenSunDance && Main.projectile[i].ai[1] == npc.whoAmI)
                            {
                                if (right)
                                {
                                    //npc.NewProjectile(Main.projectile[i].TopRight + new Vector2(Main.projectile[i].width, 0).RotatedByDegress(15), Vector2.Zero, ProjectileID.FairyQueenLance, fairyQueenLanceProjDamage, rotatedVector.RotatedByDegress(15).ToRotation());
                                    npc.NewProjectile(Main.projectile[i].TopRight + Main.projectile[i].ai[0].ToRotationVector2().Normalize(500), Vector2.Zero, ProjectileID.FairyQueenLance, fairyQueenLanceProjDamage, rotatedVector.RotatedByDegress(15).ToRotation(), npc.ai[1] / 100);
                                    npc.NewProjectile(Main.projectile[i].TopRight + Main.projectile[i].ai[0].ToRotationVector2().Normalize(500), Vector2.Zero, ProjectileID.FairyQueenLance, fairyQueenLanceProjDamage, rotatedVector.RotatedByDegress(-15).ToRotation(), npc.ai[1] / 100);
                                }
                                else
                                {
                                    npc.NewProjectile(Main.projectile[i].TopLeft + Main.projectile[i].ai[0].ToRotationVector2().Normalize(500), Vector2.Zero, ProjectileID.FairyQueenLance, fairyQueenLanceProjDamage, rotatedVector.RotatedByDegress(15).ToRotation(), npc.ai[1] / 100);
                                    npc.NewProjectile(Main.projectile[i].TopLeft + Main.projectile[i].ai[0].ToRotationVector2().Normalize(500), Vector2.Zero, ProjectileID.FairyQueenLance, fairyQueenLanceProjDamage, rotatedVector.RotatedByDegress(-15).ToRotation(), npc.ai[1] / 100);
                                }
                            }
                        }
                    }
                    float num32 = 20 - num17;
                    //Vector2 vector11 = new Vector2(0f, -100f);
                    //_ = npc.Center + vector11;
                    flag6 = !(npc.ai[1] >= 6f) || !(npc.ai[1] <= 40f);
                    int num33 = ((npc.ai[0] != 8f) ? 1 : (-1));
                    npc.AI_120_HallowBoss_DoMagicEffect(npc.Center, 5, Utils.GetLerpValue(40f, 90f, npc.ai[1], clamped: true));
                    if (npc.ai[1] <= 40f)
                    {
                        if (npc.ai[1] == 20f)
                        {
                            SoundEngine.PlaySound(SoundID.Item160, npc.Center);
                        }
                        NPCAimedTarget targetData3 = npc.GetTargetData();
                        Vector2 destination = (targetData3.Invalid ? npc.Center : targetData3.Center) + new Vector2(num33 * -550, 0f);
                        npc.SimpleFlyMovement(npc.DirectionTo(destination).SafeNormalize(Vector2.Zero) * num2, num * 2f);
                        if (npc.ai[1] == 40f)
                        {
                            npc.velocity *= 0.3f;
                        }
                    }
                    else if (npc.ai[1] <= 90f)
                    {
                        npc.velocity = Vector2.Lerp(value2: new Vector2(num33 * 50, 0f), value1: npc.velocity, amount: 0.05f);
                        if (npc.ai[1] == 90f)
                        {
                            npc.velocity *= 0.7f;
                        }
                        num16 *= 1.5f;
                    }
                    else
                    {
                        npc.velocity *= 0.92f;
                    }
                    npc.ai[1] += 1f;
                    if (npc.ai[1] >= 90f + num32)
                    {
                        npc.ai[0] = 1f;
                        npc.ai[1] = 0f;
                        npc.netUpdate = true;
                        for (int i = 0; i < Main.maxProjectiles; i++) 
                        {
                            if (Main.projectile[i].active && Main.projectile[i].type == ProjectileID.FairyQueenSunDance && Main.projectile[i].ai[1] == npc.whoAmI)
                            {
                                Main.projectile[i].active = false;
                                NetMessage.SendData(MessageID.KillProjectile, -1, -1, null, i, Main.myPlayer);
                            }
                        }
                    }
                    break;
                }
            case 10:
                {
                    float num94 = 20 - num17;
                    if (npc.ai[1] == 0f)
                    {
                        SoundEngine.PlaySound(SoundID.Item161, npc.Center);
                    }
                    flag6 = !(npc.ai[1] >= 30f) || !(npc.ai[1] <= 170f);
                    npc.velocity *= 0.95f;
                    if (npc.ai[1] == 90f)
                    {
                        if (npc.ai[3] == 0f)
                        {
                            npc.ai[3] = 1f;
                        }
                        if (npc.ai[3] == 2f)
                        {
                            npc.ai[3] = 3f;
                        }
                        npc.Center = npc.GetTargetData().Center + new Vector2(0f, -250f);
                        npc.netUpdate = true;
                    }
                    npc.ai[1] += 1f;
                    if (npc.ai[1] >= 180f + num94)
                    {
                        npc.ai[0] = 1f;
                        npc.ai[1] = 0f;
                        npc.ai[2] = 0f;
                        npc.netUpdate = true;
                    }
                    break;
                }
            case 11:
                {
                    if (npc.ai[1] == 0f)
                    {
                        SoundEngine.PlaySound(SoundID.Item162, npc.Center);
                    }
                    float num58 = 20 - num17;
                    if (npc.ai[1] >= 6f && npc.ai[1] < 54f)
                    {
                        npc.AI_120_HallowBoss_DoMagicEffect(npc.Center + new Vector2(-55f, -20f), 2, Utils.GetLerpValue(0f, 100f, npc.ai[1], clamped: true));
                        npc.AI_120_HallowBoss_DoMagicEffect(npc.Center + new Vector2(55f, -20f), 4, Utils.GetLerpValue(0f, 100f, npc.ai[1], clamped: true));
                    }
                    NPCAimedTarget targetData6 = npc.GetTargetData();
                    Vector2 vector12 = (targetData6.Invalid ? npc.Center : targetData6.Center);
                    if (npc.Distance(vector12 + vector3) > num3)
                    {
                        npc.SimpleFlyMovement(npc.DirectionTo(vector12 + vector3).SafeNormalize(Vector2.Zero) * num2, num);
                    }
                    if ((int)npc.ai[1] % 3 == 0 && npc.ai[1] < 100f)
                    {
                        int num59 = 1;
                        for (int k = 0; k < num59; k++)
                        {
                            Vector2 vector13 = -targetData6.Velocity;
                            vector13.SafeNormalize(-Vector2.UnitY);
                            float num60 = 100f;
                            Vector2 center2 = targetData6.Center;
                            if (npc.Distance(center2) > 2400f)
                            {
                                continue;
                            }
                            int num61 = 90;
                            Vector2 vector14 = center2 + targetData6.Velocity * num61;
                            Vector2 vector15 = center2 + vector13 * num60;
                            if (vector15.Distance(center2) < num60)
                            {
                                Vector2 vector16 = center2 - vector15;
                                if (vector16 == Vector2.Zero)
                                {
                                    vector16 = vector13;
                                }
                                vector15 = center2 - Vector2.Normalize(vector16) * num60;
                            }
                            Vector2 v = vector14 - vector15;
                            if (Main.netMode != 1)
                            {
                                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector15, Vector2.Zero, 919, fairyQueenLanceProjDamage, 0f, Main.myPlayer, v.ToRotation(), npc.ai[1] / 100f);
                            }
                            if (Main.netMode == 1)
                            {
                                continue;
                            }
                            int num62 = (int)(npc.ai[1] / 3f);
                            for (int l = 0; l < 255; l++)
                            {
                                if (!npc.Boss_CanShootExtraAt(l, num62 % 3, 3, 2400f))
                                {
                                    continue;
                                }
                                Player player = Main.player[l];
                                vector13 = -player.velocity;
                                vector13.SafeNormalize(-Vector2.UnitY);
                                num60 = 100f;
                                center2 = player.Center;
                                num61 = 90;
                                Vector2 vector17 = center2 + player.velocity * num61;
                                vector15 = center2 + vector13 * num60;
                                if (vector15.Distance(center2) < num60)
                                {
                                    Vector2 vector18 = center2 - vector15;
                                    if (vector18 == Vector2.Zero)
                                    {
                                        vector18 = vector13;
                                    }
                                    vector15 = center2 - Vector2.Normalize(vector18) * num60;
                                }
                                v = vector17 - vector15;
                                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector15, Vector2.Zero, 919, fairyQueenLanceProjDamage, 0f, Main.myPlayer, v.ToRotation(), npc.ai[1] / 100f);
                            }
                        }
                    }
                    npc.ai[1] += 1f;
                    if (npc.ai[1] >= 100f + num58)
                    {
                        npc.ai[0] = 1f;
                        npc.ai[1] = 0f;
                        npc.netUpdate = true;
                    }
                    break;
                }
            case 12:
                {
                    float num21 = 90f - num17;
                    Vector2 vector6 = new(-55f, -30f);
                    if (npc.ai[1] == 0f)
                    {
                        SoundEngine.PlaySound(SoundID.Item165, npc.Center);
                        npc.velocity = new Vector2(0f, -12f);
                    }
                    npc.velocity *= 0.95f;
                    bool flag11 = npc.ai[1] < 60f && npc.ai[1] >= 10f;
                    if (flag11)
                    {
                        npc.AI_120_HallowBoss_DoMagicEffect(npc.Center + vector6, 1, Utils.GetLerpValue(0f, 60f, npc.ai[1], clamped: true));
                    }
                    int num22 = 6;
                    if (flag2)
                    {
                        num22 = 4;
                    }
                    float num23 = (npc.ai[1] - 10f) / 50f;
                    if ((int)npc.ai[1] % num22 == 0 && flag11)
                    {
                        Vector2 vector7 = new Vector2(0f, -20f).RotatedBy((float)Math.PI * 2f * num23);
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center + vector6, vector7, 873, num8, 0f, Main.myPlayer, npc.target, num23);
                        }
                        if (Main.netMode != 1)
                        {
                            int num24 = (int)(npc.ai[1] % num22);
                            for (int j = 0; j < 255; j++)
                            {
                                if (npc.Boss_CanShootExtraAt(j, num24 % 3, 3, 2400f))
                                {
                                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center + vector6, vector7, 873, num8, 0f, Main.myPlayer, j, num23);
                                }
                            }
                        }
                    }
                    npc.ai[1] += 1f;
                    if (npc.ai[1] >= 60f + num21)
                    {
                        npc.ai[0] = 1f;
                        npc.ai[1] = 0f;
                        npc.netUpdate = true;
                    }
                    break;
                }
            case 13:
                {
                    if (npc.ai[1] == 0f)
                    {
                        SoundEngine.PlaySound(SoundID.Item165, npc.Center);
                        npc.velocity = new Vector2(0f, -7f);
                    }
                    npc.velocity *= 0.95f;
                    npc.TargetClosest();
                    NPCAimedTarget targetData = npc.GetTargetData();
                    flag5 = false;
                    bool flag7 = false;
                    bool flag8 = false;
                    if (!flag7)
                    {
                        if (npc.AI_120_HallowBoss_IsGenuinelyEnraged())
                        {
                            if (!Main.dayTime)
                            {
                                flag8 = true;
                            }
                            if (Main.dayTime && Main.time >= 53400.0)
                            {
                                flag8 = true;
                            }
                        }
                        flag7 = flag7 || flag8;
                    }
                    if (!flag7)
                    {
                        bool flag9 = targetData.Invalid || npc.Distance(targetData.Center) > num4;
                        flag7 = flag7 || flag9;
                    }
                    npc.alpha = Utils.Clamp(npc.alpha + flag7.ToDirectionInt() * 5, 0, 255);
                    bool flag10 = npc.alpha == 0 || npc.alpha == 255;
                    int num18 = 5;
                    for (int i = 0; i < num18; i++)
                    {
                        float num19 = MathHelper.Lerp(1.3f, 0.7f, npc.Opacity);
                        Color newColor = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f);
                        int num20 = Dust.NewDust(npc.position - npc.Size * 0.5f, npc.width * 2, npc.height * 2, 267, 0f, 0f, 0, newColor);
                        Main.dust[num20].position = npc.Center + Main.rand.NextVector2Circular(npc.width, npc.height);
                        Main.dust[num20].velocity *= Main.rand.NextFloat() * 0.8f;
                        Main.dust[num20].noGravity = true;
                        Main.dust[num20].scale = 0.9f + Main.rand.NextFloat() * 1.2f;
                        Main.dust[num20].fadeIn = 0.4f + Main.rand.NextFloat() * 1.2f * num19;
                        Main.dust[num20].velocity += Vector2.UnitY * -2f;
                        Main.dust[num20].scale = 0.35f;
                        if (num20 != 6000)
                        {
                            Dust dust = Dust.CloneDust(num20);
                            dust.scale /= 2f;
                            dust.fadeIn *= 0.85f;
                            dust.color = new Color(255, 255, 255, 255);
                        }
                    }
                    npc.ai[1] += 1f;
                    if (!(npc.ai[1] >= 20f && flag10))
                    {
                        break;
                    }
                    if (npc.alpha == 255)
                    {
                        npc.active = false;
                        if (Main.netMode != 1)
                        {
                            NetMessage.SendData(23, -1, -1, null, npc.whoAmI);
                        }
                        return;
                    }
                    npc.ai[0] = 1f;
                    npc.ai[1] = 0f;
                    npc.netUpdate = true;
                    break;
                }
        }
        npc.dontTakeDamage = !flag6;
        npc.damage = npc.GetAttackDamage_ScaledByStrength(npc.defDamage * num16);
        if (flag4)
        {
            npc.damage = 9999;
        }
        if (flag)
        {
            npc.defense = (int)(npc.defDefense * 1.2f);
        }
        else
        {
            npc.defense = npc.defDefense;
        }
        if ((npc.localAI[0] += 1f) >= 44f)
        {
            npc.localAI[0] = 0f;
        }
        if (flag5)
        {
            npc.alpha = Utils.Clamp(npc.alpha - 5, 0, 255);
        }
        Lighting.AddLight(npc.Center, Vector3.One * npc.Opacity);
    }
}