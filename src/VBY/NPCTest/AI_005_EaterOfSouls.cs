using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Utilities;

namespace VBY.NPCTest;
public partial class NPCAIs
{
    public static void AI_005_EaterOfSouls(NPC npc)
    {
        if (npc.type == 210 || npc.type == 211)
        {
            NPCUtils.TargetClosestNonBees(npc);
        }
        else if (npc.target < 0 || npc.target <= 255 || Main.player[npc.target].dead)
        {
            npc.TargetClosest();
        }
        if (npc.type == 619)
        {
            if (Main.dayTime)
            {
                npc.velocity.Y -= 0.3f;
                npc.EncourageDespawn(60);
            }
            npc.position += npc.netOffset;
            if (npc.alpha == 255)
            {
                npc.spriteDirection = npc.direction;
                npc.velocity.Y = -6f;
                for (int i = 0; i < 35; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 5);
                    dust.velocity *= 1f;
                    dust.scale = 1f + Main.rand.NextFloat() * 0.5f;
                    dust.fadeIn = 1.5f + Main.rand.NextFloat() * 0.5f;
                    dust.velocity += npc.velocity * 0.5f;
                }
            }
            npc.alpha -= 15;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }
            if (npc.alpha != 0)
            {
                for (int j = 0; j < 2; j++)
                {
                    Dust dust2 = Dust.NewDustDirect(npc.position, npc.width, npc.height, 5);
                    dust2.velocity *= 1f;
                    dust2.scale = 1f + Main.rand.NextFloat() * 0.5f;
                    dust2.fadeIn = 1.5f + Main.rand.NextFloat() * 0.5f;
                    dust2.velocity += npc.velocity * 0.3f;
                }
            }
            npc.position -= npc.netOffset;
        }
        NPCAimedTarget targetData = npc.GetTargetData();
        bool targetDead = false;
        if (targetData.Type == NPCTargetType.Player)
        {
            targetDead = Main.player[npc.target].dead;
        }
        float num = 6f;
        float num2 = 0.05f;
        if (npc.type == 6 || npc.type == 173)
        {
            num = 4f;
            num2 = 0.02f;
            if (npc.type == 6 && Main.expertMode)
            {
                num2 = 0.035f;
            }
            if (Main.remixWorld)
            {
                num2 = 0.06f;
                num = 5f;
            }
        }
        else if (npc.type == 94)
        {
            num = 4.2f;
            num2 = 0.022f;
        }
        else if (npc.type == 619)
        {
            num = 6f;
            num2 = 0.1f;
        }
        else if (npc.type == 252)
        {
            if (targetData.Type != 0 && Collision.CanHit(npc, targetData))
            {
                num = 6f;
                num2 = 0.1f;
            }
            else
            {
                num2 = 0.01f;
                num = 2f;
            }
        }
        else if (npc.type == 42 || (npc.type >= 231 && npc.type <= 235))
        {
            num = 3.5f;
            num2 = 0.021f;
            if (npc.type == 231)
            {
                num = 3f;
                num2 = 0.017f;
            }
            num *= 1f - npc.scale;
            num2 *= 1f - npc.scale;
            if ((double)(npc.position.Y / 16f) < Main.worldSurface)
            {
                if (Main.player[npc.target].position.Y - npc.position.Y > 300f && npc.velocity.Y < 0f)
                {
                    npc.velocity.Y *= 0.97f;
                }
                if (Main.player[npc.target].position.Y - npc.position.Y < 80f && npc.velocity.Y > 0f)
                {
                    npc.velocity.Y *= 0.97f;
                }
            }
        }
        else if (npc.type == 205)
        {
            num = 3.25f;
            num2 = 0.018f;
        }
        else if (npc.type == 176)
        {
            num = 4f;
            num2 = 0.017f;
        }
        else if (npc.type == 23)
        {
            num = 1f;
            num2 = 0.03f;
        }
        else if (npc.type == 5)
        {
            num = 5f;
            num2 = 0.03f;
        }
        else if (npc.type == 210 || npc.type == 211)
        {
            npc.ai[1] += 1f;
            float num3 = (npc.ai[1] - 60f) / 60f;
            if (num3 > 1f)
            {
                num3 = 1f;
            }
            else
            {
                if (npc.velocity.X > 6f)
                {
                    npc.velocity.X = 6f;
                }
                if (npc.velocity.X < -6f)
                {
                    npc.velocity.X = -6f;
                }
                if (npc.velocity.Y > 6f)
                {
                    npc.velocity.Y = 6f;
                }
                if (npc.velocity.Y < -6f)
                {
                    npc.velocity.Y = -6f;
                }
            }
            num = 5f;
            num2 = 0.1f;
            num2 *= num3;
        }
        else if (npc.type == 139 && Main.zenithWorld)
        {
            num = 3f;
        }
        Vector2 vector = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
        float num4 = targetData.Position.X + targetData.Width / 2;
        float num5 = targetData.Position.Y + targetData.Height / 2;
        num4 = (int)(num4 / 8f) * 8;
        num5 = (int)(num5 / 8f) * 8;
        vector.X = (int)(vector.X / 8f) * 8;
        vector.Y = (int)(vector.Y / 8f) * 8;
        num4 -= vector.X;
        num5 -= vector.Y;
        float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
        float num7 = num6;
        bool flag2 = false;
        if (num6 > 600f)
        {
            flag2 = true;
        }
        if (num6 == 0f)
        {
            num4 = npc.velocity.X;
            num5 = npc.velocity.Y;
        }
        else
        {
            num6 = num / num6;
            num4 *= num6;
            num5 *= num6;
        }
        bool num8 = npc.type == 6 || npc.type == 139 || npc.type == 173 || npc.type == 205;
        bool flag3 = npc.type == 42 || npc.type == 94 || npc.type == 619 || npc.type == 176 || npc.type == 210 || npc.type == 211 || (npc.type >= 231 && npc.type <= 235);
        bool flag4 = npc.type != 173 && npc.type != 6 && npc.type != 42 && (npc.type < 231 || npc.type > 235) && npc.type != 94 && npc.type != 139 && npc.type != 619;
        if (num8 || flag3)
        {
            if (num7 > 100f || flag3)
            {
                npc.ai[0] += 1f;
                if (npc.ai[0] > 0f)
                {
                    npc.velocity.Y += 0.023f;
                }
                else
                {
                    npc.velocity.Y -= 0.023f;
                }
                if (npc.ai[0] < -100f || npc.ai[0] > 100f)
                {
                    npc.velocity.X += 0.023f;
                }
                else
                {
                    npc.velocity.X -= 0.023f;
                }
                if (npc.ai[0] > 200f)
                {
                    npc.ai[0] = -200f;
                }
            }
            if (num7 < 150f && (npc.type == 6 || npc.type == 94 || npc.type == 173 || npc.type == 619))
            {
                npc.velocity.X += num4 * 0.007f;
                npc.velocity.Y += num5 * 0.007f;
            }
        }
        if (targetDead)
        {
            num4 = npc.direction * num / 2f;
            num5 = (0f - num) / 2f;
        }
        else if (npc.type == 619 && npc.Center.Y > targetData.Center.Y - 200f)
        {
            npc.velocity.Y -= 0.3f;
        }
        if (npc.type == 139 && npc.ai[3] != 0f)
        {
            if (NPC.IsMechQueenUp)
            {
                NPC nPC = Main.npc[NPC.mechQueen];
                Vector2 vector2 = new(26f * npc.ai[3], 0f);
                int num9 = (int)npc.ai[2];
                if (num9 < 0 || num9 >= 200)
                {
                    num9 = NPC.FindFirstNPC(134);
                    npc.ai[2] = num9;
                    npc.netUpdate = true;
                }
                if (num9 > -1)
                {
                    NPC nPC2 = Main.npc[num9];
                    if (!nPC2.active || nPC2.type != 134)
                    {
                        npc.dontTakeDamage = false;
                        if (npc.ai[3] > 0f)
                        {
                            npc.netUpdate = true;
                        }
                        npc.ai[3] = 0f;
                    }
                    else
                    {
                        Vector2 spinningpoint = nPC2.Center + vector2;
                        spinningpoint = spinningpoint.RotatedBy(nPC2.rotation, nPC2.Center);
                        npc.Center = spinningpoint;
                        npc.velocity = nPC.velocity;
                        npc.dontTakeDamage = true;
                    }
                }
                else
                {
                    npc.dontTakeDamage = false;
                    if (npc.ai[3] > 0f)
                    {
                        npc.netUpdate = true;
                    }
                    npc.ai[3] = 0f;
                }
            }
            else
            {
                npc.dontTakeDamage = false;
                if (npc.ai[3] > 0f)
                {
                    npc.netUpdate = true;
                }
                npc.ai[3] = 0f;
            }
        }
        else
        {
            if (npc.type == 139)
            {
                npc.dontTakeDamage = false;
            }
            if (npc.velocity.X < num4)
            {
                npc.velocity.X += num2;
                if (flag4 && npc.velocity.X < 0f && num4 > 0f)
                {
                    npc.velocity.X += num2;
                }
            }
            else if (npc.velocity.X > num4)
            {
                npc.velocity.X -= num2;
                if (flag4 && npc.velocity.X > 0f && num4 < 0f)
                {
                    npc.velocity.X -= num2;
                }
            }
            if (npc.velocity.Y < num5)
            {
                npc.velocity.Y += num2;
                if (flag4 && npc.velocity.Y < 0f && num5 > 0f)
                {
                    npc.velocity.Y += num2;
                }
            }
            else if (npc.velocity.Y > num5)
            {
                npc.velocity.Y -= num2;
                if (flag4 && npc.velocity.Y > 0f && num5 < 0f)
                {
                    npc.velocity.Y -= num2;
                }
            }
        }
        if (npc.type == 23)
        {
            if (num4 > 0f)
            {
                npc.spriteDirection = 1;
                npc.rotation = (float)Math.Atan2(num5, num4);
            }
            else if (num4 < 0f)
            {
                npc.spriteDirection = -1;
                npc.rotation = (float)Math.Atan2(num5, num4) + 3.14f;
            }
        }
        else if (npc.type == 139)
        {
            npc.localAI[0] += 1f;
            if (npc.ai[3] != 0f)
            {
                npc.localAI[0] += 2f;
            }
            if (npc.justHit)
            {
                npc.localAI[0] = 0f;
            }
            float num10 = 120f;
            if (NPC.IsMechQueenUp)
            {
                num10 = 360f;
            }
            if (Main.netMode != 1 && npc.localAI[0] >= num10)
            {
                npc.localAI[0] = 0f;
                if (targetData.Type != 0 && Collision.CanHit(npc, targetData))
                {
                    int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(25f, 22f);
                    int num11 = 84;
                    Vector2 vector3 = new(num4, num5);
                    if (NPC.IsMechQueenUp)
                    {
                        Vector2 v = targetData.Center - npc.Center - targetData.Velocity * 20f;
                        float num12 = 8f;
                        vector3 = v.SafeNormalize(Vector2.UnitY) * num12;
                    }
                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector.X, vector.Y, vector3.X, vector3.Y, num11, attackDamage_ForProjectiles, 0f, Main.myPlayer);
                }
            }
            int num13 = (int)npc.position.X + npc.width / 2;
            int num14 = (int)npc.position.Y + npc.height / 2;
            num13 /= 16;
            num14 /= 16;
            if (WorldGen.InWorld(num13, num14) && !WorldGen.SolidTile(num13, num14))
            {
                Lighting.AddLight((int)((npc.position.X + npc.width / 2) / 16f), (int)((npc.position.Y + npc.height / 2) / 16f), 0.3f, 0.1f, 0.05f);
            }
            if (num4 > 0f)
            {
                npc.spriteDirection = 1;
                npc.rotation = (float)Math.Atan2(num5, num4);
            }
            if (num4 < 0f)
            {
                npc.spriteDirection = -1;
                npc.rotation = (float)Math.Atan2(num5, num4) + 3.14f;
            }
        }
        else if (npc.type == 6 || npc.type == 94 || npc.type == 173 || npc.type == 619)
        {
            npc.rotation = (float)Math.Atan2(num5, num4) - 1.57f;
        }
        else if (npc.type == 42 || npc.type == 176 || npc.type == 205 || (npc.type >= 231 && npc.type <= 235))
        {
            if (npc.velocity.X > 0f)
            {
                npc.spriteDirection = 1;
            }
            if (npc.velocity.X < 0f)
            {
                npc.spriteDirection = -1;
            }
            npc.rotation = npc.velocity.X * 0.1f;
        }
        else
        {
            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - 1.57f;
        }
        if (npc.type == 6 || npc.type == 619 || npc.type == 23 || npc.type == 42 || npc.type == 94 || npc.type == 139 || npc.type == 173 || npc.type == 176 || npc.type == 205 || npc.type == 210 || npc.type == 211 || (npc.type >= 231 && npc.type <= 235))
        {
            float num15 = 0.7f;
            if (npc.type == 6 || npc.type == 173)
            {
                num15 = 0.4f;
            }
            if (npc.collideX)
            {
                npc.netUpdate = true;
                npc.velocity.X = npc.oldVelocity.X * (0f - num15);
                if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 2f)
                {
                    npc.velocity.X = 2f;
                }
                if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -2f)
                {
                    npc.velocity.X = -2f;
                }
            }
            if (npc.collideY)
            {
                npc.netUpdate = true;
                npc.velocity.Y = npc.oldVelocity.Y * (0f - num15);
                if (npc.velocity.Y > 0f && npc.velocity.Y < 1.5)
                {
                    npc.velocity.Y = 2f;
                }
                if (npc.velocity.Y < 0f && npc.velocity.Y > -1.5)
                {
                    npc.velocity.Y = -2f;
                }
            }
            npc.position += npc.netOffset;
            if (npc.type == 619)
            {
                int num16 = Dust.NewDust(npc.position, npc.width, npc.height, 5, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100);
                Main.dust[num16].velocity *= 0.5f;
            }
            else if (npc.type != 42 && npc.type != 139 && npc.type != 176 && npc.type != 205 && npc.type != 210 && npc.type != 211 && npc.type != 252 && (npc.type < 231 || npc.type > 235) && Main.rand.Next(20) == 0)
            {
                int num17 = 18;
                if (npc.type == 173)
                {
                    num17 = 5;
                }
                int num18 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + npc.height * 0.25f), npc.width, (int)(npc.height * 0.5f), num17, npc.velocity.X, 2f, 75, npc.color, npc.scale);
                Main.dust[num18].velocity.X *= 0.5f;
                Main.dust[num18].velocity.Y *= 0.1f;
            }
            npc.position -= npc.netOffset;
        }
        else if (npc.type != 252 && Main.rand.Next(40) == 0)
        {
            int num19 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + npc.height * 0.25f), npc.width, (int)(npc.height * 0.5f), 5, npc.velocity.X, 2f);
            Main.dust[num19].velocity.X *= 0.5f;
            Main.dust[num19].velocity.Y *= 0.1f;
        }
        if ((npc.type == 6 || npc.type == 94 || npc.type == 173 || npc.type == 619) && npc.wet)
        {
            if (npc.velocity.Y > 0f)
            {
                npc.velocity.Y *= 0.95f;
            }
            npc.velocity.Y -= 0.3f;
            if (npc.velocity.Y < -2f)
            {
                npc.velocity.Y = -2f;
            }
        }
        if (npc.type == 205 && npc.wet)
        {
            if (npc.velocity.Y > 0f)
            {
                npc.velocity.Y *= 0.95f;
            }
            npc.velocity.Y -= 0.5f;
            if (npc.velocity.Y < -4f)
            {
                npc.velocity.Y = -4f;
            }
            npc.TargetClosest();
        }
        if (npc.type == 42 || npc.type == 176 || (npc.type >= 231 && npc.type <= 235))
        {
            if (npc.wet)
            {
                if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y *= 0.95f;
                }
                npc.velocity.Y -= 0.5f;
                if (npc.velocity.Y < -4f)
                {
                    npc.velocity.Y = -4f;
                }
                npc.TargetClosest();
            }
            if (npc.ai[1] == 101f)
            {
                SoundEngine.PlaySound(SoundID.Item17, npc.position);
                npc.ai[1] = 0f;
            }
            if (Main.netMode != 1)
            {
                npc.ai[1] += Main.rand.Next(5, 20) * 0.1f * npc.scale;
                if (npc.type == 176)
                {
                    npc.ai[1] += Main.rand.Next(5, 20) * 0.1f * npc.scale;
                }
                if (Main.getGoodWorld)
                {
                    npc.ai[1] += Main.rand.Next(5, 20) * 0.1f * npc.scale;
                }
                if (targetData.Type == NPCTargetType.Player)
                {
                    Player player = Main.player[npc.target];
                    if (player != null && player.stealth == 0f && player.itemAnimation == 0)
                    {
                        npc.ai[1] = 0f;
                    }
                }
                if (npc.ai[1] >= 130f)
                {
                    if (targetData.Type != 0 && Collision.CanHit(npc, targetData))
                    {
                        float num20 = 8f;
                        Vector2 vector4 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height / 2);
                        float num21 = targetData.Center.X - vector4.X + Main.rand.Next(-20, 21);
                        float num22 = targetData.Center.Y - vector4.Y + Main.rand.Next(-20, 21);
                        if ((num21 < 0f && npc.velocity.X < 0f) || (num21 > 0f && npc.velocity.X > 0f))
                        {
                            float num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
                            num23 = num20 / num23;
                            num21 *= num23;
                            num22 *= num23;
                            int num24 = (int)(10f * npc.scale);
                            if (npc.type == 176)
                            {
                                num24 = (int)(30f * npc.scale);
                            }
                            int num25 = 55;
                            int num26 = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector4.X, vector4.Y, num21, num22, num25, num24, 0f, Main.myPlayer);
                            Main.projectile[num26].timeLeft = 300;
                            npc.ai[1] = 101f;
                            npc.netUpdate = true;
                        }
                        else
                        {
                            npc.ai[1] = 0f;
                        }
                    }
                    else
                    {
                        npc.ai[1] = 0f;
                    }
                }
            }
        }
        if (npc.type == 139 && flag2)
        {
            if ((npc.velocity.X > 0f && num4 > 0f) || (npc.velocity.X < 0f && num4 < 0f))
            {
                int num27 = 12;
                if (NPC.IsMechQueenUp)
                {
                    num27 = 5;
                }
                if (Math.Abs(npc.velocity.X) < num27)
                {
                    npc.velocity.X *= 1.05f;
                }
            }
            else
            {
                npc.velocity.X *= 0.9f;
            }
        }
        if (npc.type == 139 && NPC.IsMechQueenUp && npc.ai[2] == 0f)
        {
            Vector2 center = npc.GetTargetData().Center;
            Vector2 v2 = center - npc.Center;
            int num28 = 120;
            if (v2.Length() < num28)
            {
                npc.Center = center - v2.SafeNormalize(Vector2.UnitY) * num28;
            }
        }
        if (Main.netMode != 1)
        {
            if (Main.getGoodWorld && npc.type == 6 && NPC.AnyNPCs(13))
            {
                if (npc.justHit)
                {
                    npc.localAI[0] = 0f;
                }
                npc.localAI[0] += 1f;
                if (npc.localAI[0] == 60f)
                {
                    if (targetData.Type != 0 && Collision.CanHit(npc, targetData))
                    {
                        NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2 + npc.velocity.X), (int)(npc.position.Y + npc.height / 2 + npc.velocity.Y), 666);
                    }
                    npc.localAI[0] = 0f;
                }
            }
            if (npc.type == 94 && !targetDead)
            {
                if (npc.justHit)
                {
                    npc.localAI[0] = 0f;
                }
                npc.localAI[0] += 1f;
                if (npc.localAI[0] == 180f)
                {
                    if (targetData.Type != 0 && Collision.CanHit(npc, targetData))
                    {
                        NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2 + npc.velocity.X), (int)(npc.position.Y + npc.height / 2 + npc.velocity.Y), 112);
                    }
                    npc.localAI[0] = 0f;
                }
            }
            if (npc.type == 619 && !targetDead)
            {
                if (npc.justHit)
                {
                    npc.localAI[0] += 10f;
                }
                npc.localAI[0] += 1f;
                if (npc.localAI[0] >= 120f)
                {
                    if (targetData.Type != 0 && Collision.CanHit(npc, targetData))
                    {
                        if ((npc.Center - targetData.Center).Length() < 400f)
                        {
                            Vector2 vector5 = npc.DirectionTo(new Vector2(targetData.Center.X, targetData.Position.Y));
                            npc.velocity = -vector5 * 5f;
                            npc.netUpdate = true;
                            npc.localAI[0] = 0f;
                            vector5 = npc.DirectionTo(new Vector2(targetData.Center.X + Main.rand.Next(-100, 101), targetData.Position.Y + Main.rand.Next(-100, 101)));
                            Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center, vector5 * 15f, 811, 35, 1f, Main.myPlayer);
                        }
                        else
                        {
                            npc.localAI[0] = 50f;
                        }
                    }
                    else
                    {
                        npc.localAI[0] = 50f;
                    }
                }
            }
        }
        if ((Main.IsItDay() && npc.type != 173 && npc.type != 619 && npc.type != 6 && npc.type != 23 && npc.type != 42 && npc.type != 94 && npc.type != 176 && npc.type != 205 && npc.type != 210 && npc.type != 211 && npc.type != 252 && (npc.type < 231 || npc.type > 235)) || targetDead)
        {
            npc.velocity.Y -= num2 * 2f;
            npc.EncourageDespawn(10);
        }
        if(npc.type == 5 && npc.Distance(Main.player[npc.target].Center) < 16 * 3)
        {
            Projectile.NewProjectile(npc.GetSpawnSourceForNPCFromNPCAI(), npc.Center, Vector2.Zero, 1002, npc.damage, 0);
            //npc.StrikeNPC(npc.life, 0, 0);
        }
        if (((npc.velocity.X > 0f && npc.oldVelocity.X < 0f) || (npc.velocity.X < 0f && npc.oldVelocity.X > 0f) || (npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f) || (npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f)) && !npc.justHit)
        {
            npc.netUpdate = true;
        }
    }
}