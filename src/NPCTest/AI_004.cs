using Terraria.Audio;
using Terraria.ID;
using VBY.NPCAI;

namespace VBY.NPCTest;

public static partial class NPCAIs
{
    public static void AI_004(this NPC npc)
    {
        bool flag2 = false;
        if (Main.expertMode && npc.life < npc.lifeMax * 0.12)
        {
            flag2 = true;
        }
        bool flag3 = false;
        if (Main.expertMode && npc.life < npc.lifeMax * 0.04)
        {
            flag3 = true;
        }
        float num4 = 20f;
        if (flag3)
        {
            num4 = 10f;
        }
        if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
        {
            npc.TargetClosest();
        }
        bool dead = Main.player[npc.target].dead;
        float num5 = npc.position.X + npc.width / 2 - Main.player[npc.target].position.X - Main.player[npc.target].width / 2;
        float num6 = npc.position.Y + npc.height - 59f - Main.player[npc.target].position.Y - Main.player[npc.target].height / 2;
        float num7 = (float)Math.Atan2(num6, num5) + 1.57f;
        if (num7 < 0f)
        {
            num7 += 6.283f;
        }
        else if ((double)num7 > 6.283)
        {
            num7 -= 6.283f;
        }
        float num8 = 0f;
        if (npc.ai[0] == 0f && npc.ai[1] == 0f)
        {
            num8 = 0.02f;
        }
        if (npc.ai[0] == 0f && npc.ai[1] == 2f && npc.ai[2] > 40f)
        {
            num8 = 0.05f;
        }
        if (npc.ai[0] == 3f && npc.ai[1] == 0f)
        {
            num8 = 0.05f;
        }
        if (npc.ai[0] == 3f && npc.ai[1] == 2f && npc.ai[2] > 40f)
        {
            num8 = 0.08f;
        }
        if (npc.ai[0] == 3f && npc.ai[1] == 4f && npc.ai[2] > num4)
        {
            num8 = 0.15f;
        }
        if (npc.ai[0] == 3f && npc.ai[1] == 5f)
        {
            num8 = 0.05f;
        }
        if (Main.expertMode)
        {
            num8 *= 1.5f;
        }
        if (flag3 && Main.expertMode)
        {
            num8 = 0f;
        }
        if (npc.rotation < num7)
        {
            if ((double)(num7 - npc.rotation) > 3.1415)
            {
                npc.rotation -= num8;
            }
            else
            {
                npc.rotation += num8;
            }
        }
        else if (npc.rotation > num7)
        {
            if ((double)(npc.rotation - num7) > 3.1415)
            {
                npc.rotation += num8;
            }
            else
            {
                npc.rotation -= num8;
            }
        }
        if (npc.rotation > num7 - num8 && npc.rotation < num7 + num8)
        {
            npc.rotation = num7;
        }
        if (npc.rotation < 0f)
        {
            npc.rotation += 6.283f;
        }
        else if (npc.rotation > 6.283)
        {
            npc.rotation -= 6.283f;
        }
        if (npc.rotation > num7 - num8 && npc.rotation < num7 + num8)
        {
            npc.rotation = num7;
        }
        if (Main.rand.Next(5) == 0)
        {
            int num9 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + npc.height * 0.25f), npc.width, (int)(npc.height * 0.5f), 5, npc.velocity.X, 2f);
            Main.dust[num9].velocity.X *= 0.5f;
            Main.dust[num9].velocity.Y *= 0.1f;
        }
        npc.reflectsProjectiles = false;
        if (Main.IsItDay() || dead)
        {
            npc.velocity.Y -= 0.04f;
            npc.EncourageDespawn(10);
            return;
        }
        if (npc.ai[0] == 0f)
        {
            if (npc.ai[1] == 0f)
            {
                float num10 = 5f;
                float num11 = 0.04f;
                if (Main.expertMode)
                {
                    num11 = 0.15f;
                    num10 = 7f;
                }
                if (Main.getGoodWorld)
                {
                    num11 += 0.05f;
                    num10 += 1f;
                }
                Vector2 vector = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float num12 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector.X;
                float num13 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - 200f - vector.Y;
                float num14 = (float)Math.Sqrt(num12 * num12 + num13 * num13);
                float num15 = num14;
                num14 = num10 / num14;
                num12 *= num14;
                num13 *= num14;
                if (npc.velocity.X < num12)
                {
                    npc.velocity.X += num11;
                    if (npc.velocity.X < 0f && num12 > 0f)
                    {
                        npc.velocity.X += num11;
                    }
                }
                else if (npc.velocity.X > num12)
                {
                    npc.velocity.X -= num11;
                    if (npc.velocity.X > 0f && num12 < 0f)
                    {
                        npc.velocity.X -= num11;
                    }
                }
                if (npc.velocity.Y < num13)
                {
                    npc.velocity.Y += num11;
                    if (npc.velocity.Y < 0f && num13 > 0f)
                    {
                        npc.velocity.Y += num11;
                    }
                }
                else if (npc.velocity.Y > num13)
                {
                    npc.velocity.Y -= num11;
                    if (npc.velocity.Y > 0f && num13 < 0f)
                    {
                        npc.velocity.Y -= num11;
                    }
                }
                npc.ai[2] += 1f;
                float num16 = 600f;
                if (Main.expertMode)
                {
                    num16 *= 0.35f;
                }
                if (npc.ai[2] >= num16)
                {
                    npc.ai[1] = 1f;
                    npc.ai[2] = 0f;
                    npc.ai[3] = 0f;
                    npc.target = 255;
                    npc.netUpdate = true;
                }
                else if ((npc.position.Y + npc.height < Main.player[npc.target].position.Y && num15 < 500f) || (Main.expertMode && num15 < 500f))
                {
                    if (!Main.player[npc.target].dead)
                    {
                        npc.ai[3] += 1f;
                    }
                    float num17 = 110f;
                    if (Main.expertMode)
                    {
                        num17 *= 0.4f;
                    }
                    if (Main.getGoodWorld)
                    {
                        num17 *= 0.8f;
                    }
                    if (npc.ai[3] >= num17)
                    {
                        npc.ai[3] = 0f;
                        npc.rotation = num7;
                        float num18 = 5f;
                        if (Main.expertMode)
                        {
                            num18 = 6f;
                        }
                        float num19 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector.X;
                        float num20 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector.Y;
                        float num21 = (float)Math.Sqrt(num19 * num19 + num20 * num20);
                        num21 = num18 / num21;
                        Vector2 vector2 = vector;
                        Vector2 vector3 = default;
                        vector3.X = num19 * num21;
                        vector3.Y = num20 * num21;
                        vector2.X += vector3.X * 10f;
                        vector2.Y += vector3.Y * 10f;
                        if (Main.netMode != 1)
                        {
                            int num22 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)vector2.X, (int)vector2.Y, 5);
                            Main.npc[num22].velocity.X = vector3.X;
                            Main.npc[num22].velocity.Y = vector3.Y;
                            if (Main.netMode == 2 && num22 < 200)
                            {
                                NetMessage.SendData(23, -1, -1, null, num22);
                            }
                        }
                        SoundEngine.PlaySound(3, (int)vector2.X, (int)vector2.Y);
                        for (int m = 0; m < 10; m++)
                        {
                            Dust.NewDust(vector2, 20, 20, 5, vector3.X * 0.4f, vector3.Y * 0.4f);
                        }
                    }
                }
            }
            else if (npc.ai[1] == 1f)
            {
                npc.rotation = num7;
                float num23 = 6f;
                if (Main.expertMode)
                {
                    num23 = 7f;
                }
                if (Main.getGoodWorld)
                {
                    num23 += 1f;
                }
                Vector2 vector4 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float num24 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector4.X;
                float num25 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector4.Y;
                float num26 = (float)Math.Sqrt(num24 * num24 + num25 * num25);
                num26 = num23 / num26;
                npc.velocity.X = num24 * num26;
                npc.velocity.Y = num25 * num26;
                npc.ai[1] = 2f;
                npc.netUpdate = true;
                if (npc.netSpam > 10)
                {
                    npc.netSpam = 10;
                }
            }
            else if (npc.ai[1] == 2f)
            {
                npc.ai[2] += 1f;
                if (npc.ai[2] >= 40f)
                {
                    npc.velocity *= 0.98f;
                    if (Main.expertMode)
                    {
                        npc.velocity *= 0.985f;
                    }
                    if (Main.getGoodWorld)
                    {
                        npc.velocity *= 0.99f;
                    }
                    if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
                    {
                        npc.velocity.X = 0f;
                    }
                    if (npc.velocity.Y > -0.1 && npc.velocity.Y < 0.1)
                    {
                        npc.velocity.Y = 0f;
                    }
                }
                else
                {
                    npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - 1.57f;
                }
                int num27 = 150;
                if (Main.expertMode)
                {
                    num27 = 100;
                }
                if (Main.getGoodWorld)
                {
                    num27 -= 15;
                }
                if (npc.ai[2] >= num27)
                {
                    npc.ai[3] += 1f;
                    npc.ai[2] = 0f;
                    npc.target = 255;
                    npc.rotation = num7;
                    if (npc.ai[3] >= 3f)
                    {
                        npc.ai[1] = 0f;
                        npc.ai[3] = 0f;
                    }
                    else
                    {
                        npc.ai[1] = 1f;
                    }
                }
            }
            float num28 = 0.5f;
            if (Main.expertMode)
            {
                num28 = 0.65f;
            }
            if (npc.life < npc.lifeMax * num28)
            {
                npc.ai[0] = 1f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
                if (npc.netSpam > 10)
                {
                    npc.netSpam = 10;
                }
            }
            return;
        }
        if (npc.ai[0] == 1f || npc.ai[0] == 2f)
        {
            if (npc.ai[0] == 1f || npc.ai[3] == 1f)
            {
                npc.ai[2] += 0.005f;
                if (npc.ai[2] > 0.5)
                {
                    npc.ai[2] = 0.5f;
                }
            }
            else
            {
                npc.ai[2] -= 0.005f;
                if (npc.ai[2] < 0f)
                {
                    npc.ai[2] = 0f;
                }
            }
            npc.rotation += npc.ai[2];
            npc.ai[1] += 1f;
            if (Main.getGoodWorld)
            {
                npc.reflectsProjectiles = true;
            }
            int num29 = 20;
            if (Main.getGoodWorld && npc.life < npc.lifeMax / 3)
            {
                num29 = 10;
            }
            if (Main.expertMode && npc.ai[1] % num29 == 0f)
            {
                float num30 = 5f;
                Vector2 vector5 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float num31 = Main.rand.Next(-200, 200);
                float num32 = Main.rand.Next(-200, 200);
                if (Main.getGoodWorld)
                {
                    num31 *= 3f;
                    num32 *= 3f;
                }
                float num33 = (float)Math.Sqrt(num31 * num31 + num32 * num32);
                num33 = num30 / num33;
                Vector2 vector6 = vector5;
                Vector2 vector7 = default;
                vector7.X = num31 * num33;
                vector7.Y = num32 * num33;
                vector6.X += vector7.X * 10f;
                vector6.Y += vector7.Y * 10f;
                if (Main.netMode != 1)
                {
                    int npcIndex = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)vector6.X, (int)vector6.Y, 5);
                    Main.npc[npcIndex].velocity = vector7;
                    if (Main.netMode == 2 && npcIndex < 200)
                    {
                        NetMessage.SendData(23, -1, -1, null, npcIndex);
                    }

                    Projectile.NewProjectile(npc.GetSpawnSourceForNPCFromNPCAI(), npc.Center, new Vector2(1, 0).RotatedBy(npc.rotation), ProjectileID.FlamingScythe, npc.damage, 0);
                }
            }
            if (npc.ai[1] >= 100f)
            {
                if (npc.ai[3] == 1f)
                {
                    npc.ai[3] = 0f;
                    npc.ai[1] = 0f;
                }
                else
                {
                    npc.ai[0] += 1f;
                    npc.ai[1] = 0f;
                    if (npc.ai[0] == 3f)
                    {
                        npc.ai[2] = 0f;
                    }
                    else
                    {
                        SoundEngine.PlaySound(3, (int)npc.position.X, (int)npc.position.Y);
                        for (int num35 = 0; num35 < 2; num35++)
                        {
                            Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 8);
                            Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 7);
                            Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 6);
                        }
                        for (int num36 = 0; num36 < 20; num36++)
                        {
                            Dust.NewDust(npc.position, npc.width, npc.height, 5, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f);
                        }
                        SoundEngine.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
                    }
                }
            }
            Dust.NewDust(npc.position, npc.width, npc.height, 5, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f);
            npc.velocity.X *= 0.98f;
            npc.velocity.Y *= 0.98f;
            if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
            {
                npc.velocity.X = 0f;
            }
            if (npc.velocity.Y > -0.1 && npc.velocity.Y < 0.1)
            {
                npc.velocity.Y = 0f;
            }
            return;
        }
        npc.defense = 0;
        int num37 = 23;
        int num38 = 18;
        if (Main.expertMode)
        {
            if (flag2)
            {
                npc.defense = -15;
            }
            if (flag3)
            {
                num38 = 20;
                npc.defense = -30;
            }
        }
        npc.damage = npc.GetAttackDamage_LerpBetweenFinalValues(num37, num38);
        npc.damage = npc.GetAttackDamage_ScaledByStrength(npc.damage);
        if (npc.ai[1] == 0f && flag2)
        {
            npc.ai[1] = 5f;
        }
        if (npc.ai[1] == 0f)
        {
            float num39 = 6f;
            float num40 = 0.07f;
            Vector2 vector8 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num41 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector8.X;
            float num42 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - 120f - vector8.Y;
            float num43 = (float)Math.Sqrt(num41 * num41 + num42 * num42);
            if (num43 > 400f && Main.expertMode)
            {
                num39 += 1f;
                num40 += 0.05f;
                if (num43 > 600f)
                {
                    num39 += 1f;
                    num40 += 0.05f;
                    if (num43 > 800f)
                    {
                        num39 += 1f;
                        num40 += 0.05f;
                    }
                }
            }
            if (Main.getGoodWorld)
            {
                num39 += 1f;
                num40 += 0.1f;
            }
            num43 = num39 / num43;
            num41 *= num43;
            num42 *= num43;
            if (npc.velocity.X < num41)
            {
                npc.velocity.X += num40;
                if (npc.velocity.X < 0f && num41 > 0f)
                {
                    npc.velocity.X += num40;
                }
            }
            else if (npc.velocity.X > num41)
            {
                npc.velocity.X -= num40;
                if (npc.velocity.X > 0f && num41 < 0f)
                {
                    npc.velocity.X -= num40;
                }
            }
            if (npc.velocity.Y < num42)
            {
                npc.velocity.Y += num40;
                if (npc.velocity.Y < 0f && num42 > 0f)
                {
                    npc.velocity.Y += num40;
                }
            }
            else if (npc.velocity.Y > num42)
            {
                npc.velocity.Y -= num40;
                if (npc.velocity.Y > 0f && num42 < 0f)
                {
                    npc.velocity.Y -= num40;
                }
            }
            npc.ai[2] += 1f;
            if (npc.ai[2] >= 200f)
            {
                npc.ai[1] = 1f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                if (Main.expertMode && npc.life < npc.lifeMax * 0.35)
                {
                    npc.ai[1] = 3f;
                }
                npc.target = 255;
                npc.netUpdate = true;
            }
            if (Main.expertMode && flag3)
            {
                npc.TargetClosest();
                npc.netUpdate = true;
                npc.ai[1] = 3f;
                npc.ai[2] = 0f;
                npc.ai[3] -= 1000f;
            }
        }
        else if (npc.ai[1] == 1f)
        {
            SoundEngine.PlaySound(36, (int)npc.position.X, (int)npc.position.Y, 0);
            npc.rotation = num7;
            float num44 = 6.8f;
            if (Main.expertMode && npc.ai[3] == 1f)
            {
                num44 *= 1.15f;
            }
            if (Main.expertMode && npc.ai[3] == 2f)
            {
                num44 *= 1.3f;
            }
            if (Main.getGoodWorld)
            {
                num44 *= 1.2f;
            }
            Vector2 vector9 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num45 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector9.X;
            float num46 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector9.Y;
            float num47 = (float)Math.Sqrt(num45 * num45 + num46 * num46);
            num47 = num44 / num47;
            npc.velocity.X = num45 * num47;
            npc.velocity.Y = num46 * num47;
            npc.ai[1] = 2f;
            npc.netUpdate = true;
            if (npc.netSpam > 10)
            {
                npc.netSpam = 10;
            }
        }
        else if (npc.ai[1] == 2f)
        {
            float num48 = 40f;
            npc.ai[2] += 1f;
            if (Main.expertMode)
            {
                num48 = 50f;
            }
            if (npc.ai[2] >= num48)
            {
                npc.velocity *= 0.97f;
                if (Main.expertMode)
                {
                    npc.velocity *= 0.98f;
                }
                if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
                {
                    npc.velocity.X = 0f;
                }
                if (npc.velocity.Y > -0.1 && npc.velocity.Y < 0.1)
                {
                    npc.velocity.Y = 0f;
                }
            }
            else
            {
                npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - 1.57f;
            }
            int num49 = 130;
            if (Main.expertMode)
            {
                num49 = 90;
            }
            if (npc.ai[2] >= num49)
            {
                npc.ai[3] += 1f;
                npc.ai[2] = 0f;
                npc.target = 255;
                npc.rotation = num7;
                if (npc.ai[3] >= 3f)
                {
                    npc.ai[1] = 0f;
                    npc.ai[3] = 0f;
                    if (Main.expertMode && Main.netMode != 1 && npc.life < npc.lifeMax * 0.5)
                    {
                        npc.ai[1] = 3f;
                        npc.ai[3] += Main.rand.Next(1, 4);
                    }
                    npc.netUpdate = true;
                    if (npc.netSpam > 10)
                    {
                        npc.netSpam = 10;
                    }
                }
                else
                {
                    npc.ai[1] = 1f;
                }
            }
        }
        else if (npc.ai[1] == 3f)
        {
            if (npc.ai[3] == 4f && flag2 && npc.Center.Y > Main.player[npc.target].Center.Y)
            {
                npc.TargetClosest();
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
                if (npc.netSpam > 10)
                {
                    npc.netSpam = 10;
                }
            }
            else if (Main.netMode != 1)
            {
                npc.TargetClosest();
                float num50 = 20f;
                Vector2 vector10 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float num51 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector10.X;
                float num52 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector10.Y;
                float num53 = Math.Abs(Main.player[npc.target].velocity.X) + Math.Abs(Main.player[npc.target].velocity.Y) / 4f;
                num53 += 10f - num53;
                if (num53 < 5f)
                {
                    num53 = 5f;
                }
                if (num53 > 15f)
                {
                    num53 = 15f;
                }
                if (npc.ai[2] == -1f && !flag3)
                {
                    num53 *= 4f;
                    num50 *= 1.3f;
                }
                if (flag3)
                {
                    num53 *= 2f;
                }
                num51 -= Main.player[npc.target].velocity.X * num53;
                num52 -= Main.player[npc.target].velocity.Y * num53 / 4f;
                num51 *= 1f + Main.rand.Next(-10, 11) * 0.01f;
                num52 *= 1f + Main.rand.Next(-10, 11) * 0.01f;
                if (flag3)
                {
                    num51 *= 1f + Main.rand.Next(-10, 11) * 0.01f;
                    num52 *= 1f + Main.rand.Next(-10, 11) * 0.01f;
                }
                float num54 = (float)Math.Sqrt(num51 * num51 + num52 * num52);
                float num55 = num54;
                num54 = num50 / num54;
                npc.velocity.X = num51 * num54;
                npc.velocity.Y = num52 * num54;
                npc.velocity.X += Main.rand.Next(-20, 21) * 0.1f;
                npc.velocity.Y += Main.rand.Next(-20, 21) * 0.1f;
                if (flag3)
                {
                    npc.velocity.X += Main.rand.Next(-50, 51) * 0.1f;
                    npc.velocity.Y += Main.rand.Next(-50, 51) * 0.1f;
                    float num56 = Math.Abs(npc.velocity.X);
                    float num57 = Math.Abs(npc.velocity.Y);
                    if (npc.Center.X > Main.player[npc.target].Center.X)
                    {
                        num57 *= -1f;
                    }
                    if (npc.Center.Y > Main.player[npc.target].Center.Y)
                    {
                        num56 *= -1f;
                    }
                    npc.velocity.X = num57 + npc.velocity.X;
                    npc.velocity.Y = num56 + npc.velocity.Y;
                    npc.velocity.Normalize();
                    npc.velocity *= num50;
                    npc.velocity.X += Main.rand.Next(-20, 21) * 0.1f;
                    npc.velocity.Y += Main.rand.Next(-20, 21) * 0.1f;
                }
                else if (num55 < 100f)
                {
                    if (Math.Abs(npc.velocity.X) > Math.Abs(npc.velocity.Y))
                    {
                        float num58 = Math.Abs(npc.velocity.X);
                        float num59 = Math.Abs(npc.velocity.Y);
                        if (npc.Center.X > Main.player[npc.target].Center.X)
                        {
                            num59 *= -1f;
                        }
                        if (npc.Center.Y > Main.player[npc.target].Center.Y)
                        {
                            num58 *= -1f;
                        }
                        npc.velocity.X = num59;
                        npc.velocity.Y = num58;
                    }
                }
                else if (Math.Abs(npc.velocity.X) > Math.Abs(npc.velocity.Y))
                {
                    float num60 = (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) / 2f;
                    float num61 = num60;
                    if (npc.Center.X > Main.player[npc.target].Center.X)
                    {
                        num61 *= -1f;
                    }
                    if (npc.Center.Y > Main.player[npc.target].Center.Y)
                    {
                        num60 *= -1f;
                    }
                    npc.velocity.X = num61;
                    npc.velocity.Y = num60;
                }
                npc.ai[1] = 4f;
                npc.netUpdate = true;
                if (npc.netSpam > 10)
                {
                    npc.netSpam = 10;
                }
            }
        }
        else if (npc.ai[1] == 4f)
        {
            if (npc.ai[2] == 0f)
            {
                SoundEngine.PlaySound(36, (int)npc.position.X, (int)npc.position.Y, -1);

                npc.NewThreeProjectile(npc.velocity.Normalize(4), 30, ProjectileID.FrostWave, npc.damage / 5);
            }
            float num62 = num4;
            npc.ai[2] += 1f;
            if (npc.ai[2] == num62 && Vector2.Distance(npc.position, Main.player[npc.target].position) < 200f)
            {
                npc.ai[2] -= 1f;
            }
            if (npc.ai[2] >= num62)
            {
                npc.velocity *= 0.95f;
                if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
                {
                    npc.velocity.X = 0f;
                }
                if (npc.velocity.Y > -0.1 && npc.velocity.Y < 0.1)
                {
                    npc.velocity.Y = 0f;
                }
            }
            else
            {
                npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - 1.57f;
            }
            float num63 = num62 + 13f;
            if (npc.ai[2] >= num63)
            {
                npc.netUpdate = true;
                if (npc.netSpam > 10)
                {
                    npc.netSpam = 10;
                }
                npc.ai[3] += 1f;
                npc.ai[2] = 0f;
                if (npc.ai[3] >= 5f)
                {
                    npc.ai[1] = 0f;
                    npc.ai[3] = 0f;
                    if (npc.target >= 0 && Main.getGoodWorld && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, npc.width, npc.height))
                    {
                        SoundEngine.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
                        npc.ai[0] = 2f;
                        npc.ai[1] = 0f;
                        npc.ai[2] = 0f;
                        npc.ai[3] = 1f;
                        npc.netUpdate = true; 
                    }
                }
                else
                {
                    npc.ai[1] = 3f;
                }

                if (Vector2.Distance(npc.Center, Main.player[npc.target].Center) < 2000 && Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                {
                    Projectile.NewProjectile(npc.GetSpawnSourceForNPCFromNPCAI(), npc.Center, Vector2.Normalize((Main.player[npc.target].Center - npc.Center).RotatedByRandom(0.7853981852531433)) * 7f, 466, npc.damage / 4, 0f, Main.myPlayer, (Main.player[npc.target].Center - npc.Center).ToRotation(), Main.rand.Next(100));
                }
            }
        }
        else if (npc.ai[1] == 5f)
        {
            float num64 = 600f;
            float num65 = 9f;
            float num66 = 0.3f;
            Vector2 vector11 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num67 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector11.X;
            float num68 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 + num64 - vector11.Y;
            float num69 = (float)Math.Sqrt(num67 * num67 + num68 * num68);
            num69 = num65 / num69;
            num67 *= num69;
            num68 *= num69;
            if (npc.velocity.X < num67)
            {
                npc.velocity.X += num66;
                if (npc.velocity.X < 0f && num67 > 0f)
                {
                    npc.velocity.X += num66;
                }
            }
            else if (npc.velocity.X > num67)
            {
                npc.velocity.X -= num66;
                if (npc.velocity.X > 0f && num67 < 0f)
                {
                    npc.velocity.X -= num66;
                }
            }
            if (npc.velocity.Y < num68)
            {
                npc.velocity.Y += num66;
                if (npc.velocity.Y < 0f && num68 > 0f)
                {
                    npc.velocity.Y += num66;
                }
            }
            else if (npc.velocity.Y > num68)
            {
                npc.velocity.Y -= num66;
                if (npc.velocity.Y > 0f && num68 < 0f)
                {
                    npc.velocity.Y -= num66;
                }
            }
            npc.ai[2] += 1f;
            if (npc.ai[2] >= 70f)
            {
                npc.TargetClosest();
                npc.ai[1] = 3f;
                npc.ai[2] = -1f;
                npc.ai[3] = Main.rand.Next(-3, 1);
                npc.netUpdate = true;
            }
        }
        if (flag3 && npc.ai[1] == 5f)
        {
            npc.ai[1] = 3f;
        }
    }
}
