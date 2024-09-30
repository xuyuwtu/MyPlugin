using Terraria.Audio;
using VBY.NPCAI;

namespace VBY.NPCTest;

static partial class NPCAIs
{
    public static void AI_069_DukeFishron(NPC npc)
    {
        bool expertMode = Main.expertMode;
        float num = (expertMode ? 1.2f : 1f);
        bool flag = npc.life <= npc.lifeMax * 0.5;
        bool flag2 = expertMode && npc.life <= npc.lifeMax * 0.15;
        bool flag3 = npc.ai[0] > 4f;
        bool flag4 = npc.ai[0] > 9f;
        bool flag5 = npc.ai[3] < 10f;
        if (flag4)
        {
            npc.damage = (int)(npc.defDamage * 1.1f * num);
            npc.defense = 0;
        }
        else if (flag3)
        {
            npc.damage = (int)(npc.defDamage * 1.2f * num);
            npc.defense = (int)(npc.defDefense * 0.8f);
        }
        else
        {
            npc.damage = npc.defDamage;
            npc.defense = npc.defDefense;
        }
        int num2 = (expertMode ? 40 : 60);
        float num3 = (expertMode ? 0.55f : 0.45f);
        float num4 = (expertMode ? 8.5f : 7.5f);
        if (flag4)
        {
            num3 = 0.7f;
            num4 = 12f;
            num2 = 30;
        }
        else if (flag3 && flag5)
        {
            num3 = (expertMode ? 0.6f : 0.5f);
            num4 = (expertMode ? 10f : 8f);
            num2 = (expertMode ? 40 : 20);
        }
        else if (flag5 && !flag3 && !flag4)
        {
            num2 = 30;
        }
        //num2 = 0;
        int firstOrderSprintFrames = (expertMode ? 28 : 30);
        float num6 = (expertMode ? 17f : 16f);
        if (flag4)
        {
            firstOrderSprintFrames = 25;
            num6 = 27f;
        }
        else if (flag5 && flag3)
        {
            firstOrderSprintFrames = (expertMode ? 27 : 30);
            if (expertMode)
            {
                num6 = 21f;
            }
        }
        int num8 = 4;
        float num9 = 0.3f;
        float num10 = 5f;
        int num11 = 90;
        int num12 = 180;
        int num13 = 180;
        int num15 = 120;
        float num18 = 20f;
        float num19 = (float)Math.PI * 2f / (num15 / 2);
        int num20 = 75;
        Vector2 npcCenter = npc.Center;
        Player targetPlayer = Main.player[npc.target];
        if (npc.target < 0 || npc.target == 255 || targetPlayer.dead || !targetPlayer.active || Vector2.Distance(targetPlayer.Center, npcCenter) > 5600f)
        {
            npc.TargetClosest();
            targetPlayer = Main.player[npc.target];
            npc.netUpdate = true;
        }
        if (targetPlayer.dead || Vector2.Distance(targetPlayer.Center, npcCenter) > 5600f)
        {
            npc.velocity.Y -= 0.4f;
            npc.EncourageDespawn(10);
            if (npc.ai[0] > 4f)
            {
                npc.ai[0] = 5f;
            }
            else
            {
                npc.ai[0] = 0f;
            }
            npc.ai[2] = 0f;
        }
        bool flag6 = targetPlayer.position.Y < 800f || targetPlayer.position.Y > Main.worldSurface * 16.0 || (targetPlayer.position.X > 6400f && targetPlayer.position.X < Main.maxTilesX * 16 - 6400);
        if (flag6)
        {
            num2 = 10;
            npc.damage = npc.defDamage * 2;
            npc.defense = npc.defDefense * 2;
            num6 += 6f;
        }
        bool takeDamage = true;
        if (npc.localAI[0] == 0f)
        {
            npc.localAI[0] = 1f;
            npc.alpha = 255;
            npc.rotation = 0f;
            if (Main.netMode != 1)
            {
                npc.ai[0] = -1f;
                npc.netUpdate = true;
            }
        }
        float num21 = (float)Math.Atan2(targetPlayer.Center.Y - npcCenter.Y, targetPlayer.Center.X - npcCenter.X);
        if (npc.spriteDirection == 1)
        {
            num21 += (float)Math.PI;
        }
        if (num21 < 0f)
        {
            num21 += (float)Math.PI * 2f;
        }
        if (num21 > (float)Math.PI * 2f)
        {
            num21 -= (float)Math.PI * 2f;
        }
        if (npc.ai[0] == -1f)
        {
            num21 = 0f;
        }
        if (npc.ai[0] == 3f)
        {
            num21 = 0f;
        }
        if (npc.ai[0] == 4f)
        {
            num21 = 0f;
        }
        if (npc.ai[0] == 8f)
        {
            num21 = 0f;
        }
        float num22 = 0.04f;
        if (npc.ai[0] == 1f || npc.ai[0] == 6f)
        {
            num22 = 0f;
        }
        if (npc.ai[0] == 7f)
        {
            num22 = 0f;
        }
        if (npc.ai[0] == 3f)
        {
            num22 = 0.01f;
        }
        if (npc.ai[0] == 4f)
        {
            num22 = 0.01f;
        }
        if (npc.ai[0] == 8f)
        {
            num22 = 0.01f;
        }
        if (npc.rotation < num21)
        {
            if ((double)(num21 - npc.rotation) > Math.PI)
            {
                npc.rotation -= num22;
            }
            else
            {
                npc.rotation += num22;
            }
        }
        if (npc.rotation > num21)
        {
            if ((double)(npc.rotation - num21) > Math.PI)
            {
                npc.rotation += num22;
            }
            else
            {
                npc.rotation -= num22;
            }
        }
        if (npc.rotation > num21 - num22 && npc.rotation < num21 + num22)
        {
            npc.rotation = num21;
        }
        if (npc.rotation < 0f)
        {
            npc.rotation += (float)Math.PI * 2f;
        }
        if (npc.rotation > (float)Math.PI * 2f)
        {
            npc.rotation -= (float)Math.PI * 2f;
        }
        if (npc.rotation > num21 - num22 && npc.rotation < num21 + num22)
        {
            npc.rotation = num21;
        }
        if (npc.ai[0] != -1f && npc.ai[0] < 9f)
        {
            if (Collision.SolidCollision(npc.position, npc.width, npc.height))
            {
                npc.alpha += 15;
            }
            else
            {
                npc.alpha -= 15;
            }
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }
            if (npc.alpha > 150)
            {
                npc.alpha = 150;
            }
        }
        if (npc.ai[0] == -1f)
        {
            takeDamage = false;
            npc.velocity *= 0.98f;
            int num23 = Math.Sign(targetPlayer.Center.X - npcCenter.X);
            if (num23 != 0)
            {
                npc.direction = num23;
                npc.spriteDirection = -npc.direction;
            }
            if (npc.ai[2] > 20f)
            {
                npc.velocity.Y = -2f;
                npc.alpha -= 5;
                if (Collision.SolidCollision(npc.position, npc.width, npc.height))
                {
                    npc.alpha += 15;
                }
                if (npc.alpha < 0)
                {
                    npc.alpha = 0;
                }
                if (npc.alpha > 150)
                {
                    npc.alpha = 150;
                }
            }
            if (npc.ai[2] == num11 - 30)
            {
                //int num24 = 36;
                //for (int i = 0; i < num24; i++)
                //{
                //    Vector2 vector = (Vector2.Normalize(npc.velocity) * new Vector2(npc.width / 2f, npc.height) * 0.75f * 0.5f).RotatedBy((i - (num24 / 2 - 1)) * ((float)Math.PI * 2f) / num24) + npc.Center;
                //    Vector2 vector2 = vector - npc.Center;
                //    int num25 = Dust.NewDust(vector + vector2, 0, 0, 172, vector2.X * 2f, vector2.Y * 2f, 100, default, 1.4f);
                //    Main.dust[num25].noGravity = true;
                //    Main.dust[num25].noLight = true;
                //    Main.dust[num25].velocity = Vector2.Normalize(vector2) * 3f;
                //}
                //SoundEngine.PlaySound(29, (int)npcCenter.X, (int)npcCenter.Y, 20);
            }
            npc.ai[2] += 1f;
            if (npc.ai[2] >= num20)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 0f && !targetPlayer.dead)
        {
            if (npc.ai[1] == 0f)
            {
                npc.ai[1] = 300 * Math.Sign((npcCenter - targetPlayer.Center).X);
            }
            Vector2 vector3 = Vector2.Normalize(targetPlayer.Center + new Vector2(npc.ai[1], -200f) - npcCenter - npc.velocity) * num4;
            if (npc.velocity.X < vector3.X)
            {
                npc.velocity.X += num3;
                if (npc.velocity.X < 0f && vector3.X > 0f)
                {
                    npc.velocity.X += num3;
                }
            }
            else if (npc.velocity.X > vector3.X)
            {
                npc.velocity.X -= num3;
                if (npc.velocity.X > 0f && vector3.X < 0f)
                {
                    npc.velocity.X -= num3;
                }
            }
            if (npc.velocity.Y < vector3.Y)
            {
                npc.velocity.Y += num3;
                if (npc.velocity.Y < 0f && vector3.Y > 0f)
                {
                    npc.velocity.Y += num3;
                }
            }
            else if (npc.velocity.Y > vector3.Y)
            {
                npc.velocity.Y -= num3;
                if (npc.velocity.Y > 0f && vector3.Y < 0f)
                {
                    npc.velocity.Y -= num3;
                }
            }
            int num26 = Math.Sign(targetPlayer.Center.X - npcCenter.X);
            if (num26 != 0)
            {
                if (npc.ai[2] == 0f && num26 != npc.direction)
                {
                    npc.rotation += (float)Math.PI;
                }
                npc.direction = num26;
                if (npc.spriteDirection != -npc.direction)
                {
                    npc.rotation += (float)Math.PI;
                }
                npc.spriteDirection = -npc.direction;
            }
            npc.ai[2] += 1f;
            if (npc.ai[2] >= num2)
            {
                int num27 = 0;
                switch ((int)npc.ai[3])
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        num27 = 1;
                        break;
                    case 10:
                        npc.ai[3] = 1f;
                        num27 = 2;
                        break;
                    case 11:
                        npc.ai[3] = 0f;
                        num27 = 3;
                        break;
                }
                if (flag6 && num27 == 2)
                {
                    num27 = 3;
                }
                if (flag)
                {
                    num27 = 4;
                }
                switch (num27)
                {
                    case 1:
                        npc.ai[0] = 1f;
                        npc.ai[1] = 0f;
                        npc.ai[2] = 0f;
                        npc.velocity = Vector2.Normalize(targetPlayer.Center - npcCenter) * num6 * (1f + (1 - ((float)npc.life / npc.lifeMax)));
                        //var newTargetPosition = targetPlayer.Center + targetPlayer.velocity * firstOrderSprintFrames / 2;
                        //if ((newTargetPosition - npc.Center).Length() <= npc.velocity.Length() * firstOrderSprintFrames / 2)
                        //{
                        //    npc.velocity = Vector2.Normalize((newTargetPosition - npcCenter) * 2) * num6 * (1f + (1 - ((float)npc.life / npc.lifeMax)));
                        //    Console.WriteLine("modify");
                        //}
                        npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X);
                        if (num26 != 0)
                        {
                            npc.direction = num26;
                            if (npc.spriteDirection == 1)
                            {
                                npc.rotation += (float)Math.PI;
                            }
                            npc.spriteDirection = -npc.direction;
                        }
                        break;
                    case 2:
                        npc.ai[0] = 2f;
                        npc.ai[1] = 0f;
                        npc.ai[2] = 0f;
                        break;
                    case 3:
                        npc.ai[0] = 3f;
                        npc.ai[1] = 0f;
                        npc.ai[2] = 0f;
                        if (flag6)
                        {
                            npc.ai[2] = num11 - 40;
                        }
                        break;
                    case 4:
                        npc.ai[0] = 4f;
                        npc.ai[1] = 0f;
                        npc.ai[2] = 0f;
                        break;
                }
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 1f)
        {
            //int num28 = 7;
            //for (int j = 0; j < num28; j++)
            //{
            //    Vector2 vector4 = (Vector2.Normalize(npc.velocity) * new Vector2((npc.width + 50) / 2f, npc.height) * 0.75f).RotatedBy((j - (num28 / 2 - 1)) * Math.PI / (double)(float)num28) + npcCenter;
            //    Vector2 vector5 = ((float)(Main.rand.NextDouble() * 3.1415927410125732) - (float)Math.PI / 2f).ToRotationVector2() * Main.rand.Next(3, 8);
            //    int num29 = Dust.NewDust(vector4 + vector5, 0, 0, 172, vector5.X * 2f, vector5.Y * 2f, 100, default, 1.4f);
            //    Main.dust[num29].noGravity = true;
            //    Main.dust[num29].noLight = true;
            //    Main.dust[num29].velocity /= 4f;
            //    Main.dust[num29].velocity -= npc.velocity;
            //}
            if (npc.ai[2] % 4f == 0)
            {
                npc.NewProjectile(Vector2.Zero, 288, npc.damage / 10);
            }
            npc.ai[2] += 1f;
            if (npc.ai[2] >= firstOrderSprintFrames)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] += 2f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 2f)
        {
            if (npc.ai[1] == 0f)
            {
                npc.ai[1] = 300 * Math.Sign((npcCenter - targetPlayer.Center).X);
            }
            Vector2 vector6 = Vector2.Normalize(targetPlayer.Center + new Vector2(npc.ai[1], -200f) - npcCenter - npc.velocity) * num10;
            if (npc.velocity.X < vector6.X)
            {
                npc.velocity.X += num9;
                if (npc.velocity.X < 0f && vector6.X > 0f)
                {
                    npc.velocity.X += num9;
                }
            }
            else if (npc.velocity.X > vector6.X)
            {
                npc.velocity.X -= num9;
                if (npc.velocity.X > 0f && vector6.X < 0f)
                {
                    npc.velocity.X -= num9;
                }
            }
            if (npc.velocity.Y < vector6.Y)
            {
                npc.velocity.Y += num9;
                if (npc.velocity.Y < 0f && vector6.Y > 0f)
                {
                    npc.velocity.Y += num9;
                }
            }
            else if (npc.velocity.Y > vector6.Y)
            {
                npc.velocity.Y -= num9;
                if (npc.velocity.Y > 0f && vector6.Y < 0f)
                {
                    npc.velocity.Y -= num9;
                }
            }
            //if (npc.ai[2] == 0f)
            //{
            //    SoundEngine.PlaySound(29, (int)npcCenter.X, (int)npcCenter.Y, 20);
            //}
            if (npc.ai[2] % num8 == 0f)
            {
                //SoundEngine.PlaySound(4, (int)npc.Center.X, (int)npc.Center.Y, 19);
                if (Main.netMode != 1)
                {
                    Vector2 vector7 = Vector2.Normalize(targetPlayer.Center - npcCenter) * (npc.width + 20) / 2f + npcCenter;
                    NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)vector7.X, (int)vector7.Y + 45, 371);
                }
            }
            int num30 = Math.Sign(targetPlayer.Center.X - npcCenter.X);
            if (num30 != 0)
            {
                npc.direction = num30;
                if (npc.spriteDirection != -npc.direction)
                {
                    npc.rotation += (float)Math.PI;
                }
                npc.spriteDirection = -npc.direction;
            }
            npc.ai[2] += 1f;
            int num7 = 80;
            if (npc.ai[2] >= num7)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;

                if(npc.life < npc.lifeMax)
                {
                    var heal = Math.Min(500, npc.lifeMax - npc.life);
                    npc.life += heal;
                    NPC.HealEffect(Utils.CenteredRectangle(npc.Center, new Vector2(50)), heal);
                }
                
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 3f)
        {
            npc.velocity *= 0.98f;
            npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, 0f, 0.02f);
            if (npc.ai[2] == num11 - 30)
            {
                SoundEngine.PlaySound(29, (int)npcCenter.X, (int)npcCenter.Y, 9);
            }
            if (Main.netMode != 1 && npc.ai[2] == num11 - 30)
            {
                Vector2 vector8 = npc.rotation.ToRotationVector2() * (Vector2.UnitX * npc.direction) * (npc.width + 20) / 2f + npcCenter;
                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector8.X, vector8.Y, npc.direction * 2, 8f, 385, 0, 0f, Main.myPlayer);
                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector8.X, vector8.Y, -npc.direction * 2, 8f, 385, 0, 0f, Main.myPlayer);

                //Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector8.X, vector8.Y, npc.direction * 4, 8f, 385, 0, 0f, Main.myPlayer);
                //Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector8.X, vector8.Y, -npc.direction * 4, 8f, 385, 0, 0f, Main.myPlayer);
                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector8.X, vector8.Y, npc.direction * 6, 8f, 385, 0, 0f, Main.myPlayer);
                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector8.X, vector8.Y, -npc.direction * 6, 8f, 385, 0, 0f, Main.myPlayer);
            }
            npc.ai[2] += 1f;
            if (npc.ai[2] >= num11)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;

                if (npc.life < npc.lifeMax)
                {
                    var heal = Math.Min(500, npc.lifeMax - npc.life);
                    npc.life += heal;
                    NPC.HealEffect(Utils.CenteredRectangle(npc.Center, new Vector2(50)), heal);
                }

                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 4f)
        {
            takeDamage = false;
            npc.velocity *= 0.98f;
            npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, 0f, 0.02f);
            if (npc.ai[2] == num12 - 60)
            {
                SoundEngine.PlaySound(29, (int)npcCenter.X, (int)npcCenter.Y, 20);
            }

            if (npc.ai[2] % 10 == 0)
            {
                var position = npc.position;
                position.X += Main.rand.Next(npc.width);
                position.X += Main.rand.Next(npc.height);
                var distance = position.Distance(Main.player[npc.target].Center);
                var velocity = Main.player[npc.target].Center - position;
                velocity.X *= 4 / distance;
                velocity.Y *= 4 / distance;
                //Projectile.NewProjectile(npc.GetSpawnSourceForNPCFromNPCAI(), npc.Center, default, 919, 1, 1, -1, npc.rotation);
                Main.projectile[Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), position, velocity, 293, 100, 0)].timeLeft = 300;
            }

            npc.ai[2] += 1f;
            if (npc.ai[2] >= num12)
            {
                npc.ai[0] = 5f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 5f && !targetPlayer.dead)
        {
            if (npc.ai[1] == 0f)
            {
                npc.ai[1] = 300 * Math.Sign((npcCenter - targetPlayer.Center).X);
            }
            Vector2 vector9 = Vector2.Normalize(targetPlayer.Center + new Vector2(npc.ai[1], -200f) - npcCenter - npc.velocity) * num4;
            if (npc.velocity.X < vector9.X)
            {
                npc.velocity.X += num3;
                if (npc.velocity.X < 0f && vector9.X > 0f)
                {
                    npc.velocity.X += num3;
                }
            }
            else if (npc.velocity.X > vector9.X)
            {
                npc.velocity.X -= num3;
                if (npc.velocity.X > 0f && vector9.X < 0f)
                {
                    npc.velocity.X -= num3;
                }
            }
            if (npc.velocity.Y < vector9.Y)
            {
                npc.velocity.Y += num3;
                if (npc.velocity.Y < 0f && vector9.Y > 0f)
                {
                    npc.velocity.Y += num3;
                }
            }
            else if (npc.velocity.Y > vector9.Y)
            {
                npc.velocity.Y -= num3;
                if (npc.velocity.Y > 0f && vector9.Y < 0f)
                {
                    npc.velocity.Y -= num3;
                }
            }
            int num31 = Math.Sign(targetPlayer.Center.X - npcCenter.X);
            if (num31 != 0)
            {
                if (npc.ai[2] == 0f && num31 != npc.direction)
                {
                    npc.rotation += (float)Math.PI;
                }
                npc.direction = num31;
                if (npc.spriteDirection != -npc.direction)
                {
                    npc.rotation += (float)Math.PI;
                }
                npc.spriteDirection = -npc.direction;
            }
            npc.ai[2] += 1f;
            if (npc.ai[2] >= num2)
            {
                int num32 = 0;
                switch ((int)npc.ai[3])
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        num32 = 1;
                        break;
                    case 6:
                        npc.ai[3] = 1f;
                        num32 = 2;
                        break;
                    case 7:
                        npc.ai[3] = 0f;
                        num32 = 3;
                        break;
                }
                if (flag2)
                {
                    num32 = 4;
                }
                if (flag6 && num32 == 2)
                {
                    num32 = 3;
                }
                switch (num32)
                {
                    case 1:
                        npc.ai[0] = 6f;
                        npc.ai[1] = 0f;
                        npc.ai[2] = 0f;
                        npc.velocity = Vector2.Normalize(targetPlayer.Center - npcCenter) * num6 * 1.2f; 
                        //var newTargetPosition = targetPlayer.Center + targetPlayer.velocity * firstOrderSprintFrames / 2;
                        //if ((newTargetPosition - npc.Center).Length() <= npc.velocity.Length() * firstOrderSprintFrames / 2)
                        //{
                        //    npc.velocity = Vector2.Normalize((newTargetPosition - npcCenter) * 2) * num6 * (1f + (1 - ((float)npc.life / npc.lifeMax)));
                        //    Console.WriteLine("modify");
                        //}
                        npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X);
                        if (num31 != 0)
                        {
                            npc.direction = num31;
                            if (npc.spriteDirection == 1)
                            {
                                npc.rotation += (float)Math.PI;
                            }
                            npc.spriteDirection = -npc.direction;
                        }
                        break;
                    case 2:
                        npc.velocity = Vector2.Normalize(targetPlayer.Center - npcCenter) * num18 * 1.2f;
                        npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X);
                        if (num31 != 0)
                        {
                            npc.direction = num31;
                            if (npc.spriteDirection == 1)
                            {
                                npc.rotation += (float)Math.PI;
                            }
                            npc.spriteDirection = -npc.direction;
                        }
                        npc.ai[0] = 7f;
                        npc.ai[1] = 0f;
                        npc.ai[2] = 0f;
                        break;
                    case 3:
                        npc.ai[0] = 8f;
                        npc.ai[1] = 0f;
                        npc.ai[2] = 0f;
                        break;
                    case 4:
                        npc.ai[0] = 9f;
                        npc.ai[1] = 0f;
                        npc.ai[2] = 0f;
                        break;
                }
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 6f)
        {
            //int num33 = 7;
            //for (int k = 0; k < num33; k++)
            //{
            //    Vector2 vector10 = (Vector2.Normalize(npc.velocity) * new Vector2((npc.width + 50) / 2f, npc.height) * 0.75f).RotatedBy((k - (num33 / 2 - 1)) * Math.PI / (double)(float)num33) + npcCenter;
            //    Vector2 vector11 = ((float)(Main.rand.NextDouble() * 3.1415927410125732) - (float)Math.PI / 2f).ToRotationVector2() * Main.rand.Next(3, 8);
            //    int num34 = Dust.NewDust(vector10 + vector11, 0, 0, 172, vector11.X * 2f, vector11.Y * 2f, 100, default, 1.4f);
            //    Main.dust[num34].noGravity = true;
            //    Main.dust[num34].noLight = true;
            //    Main.dust[num34].velocity /= 4f;
            //    Main.dust[num34].velocity -= npc.velocity;
            //}
            npc.ai[2] += 1f;
            if (npc.ai[2] >= firstOrderSprintFrames)
            {
                npc.ai[0] = 5f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] += 2f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 7f)
        {
            if (npc.ai[2] == 0f)
            {
                SoundEngine.PlaySound(29, (int)npcCenter.X, (int)npcCenter.Y, 20);
            }
            int num16 = 4;
            float num17 = 6f;
            if (npc.ai[2] % num16 == 0f)
            {
                SoundEngine.PlaySound(4, (int)npc.Center.X, (int)npc.Center.Y, 19);
                if (Main.netMode != 1)
                {
                    Vector2 vector12 = Vector2.Normalize(npc.velocity) * (npc.width + 20) / 2f + npcCenter;
                    int num35 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)vector12.X, (int)vector12.Y + 45, 371);
                    Main.npc[num35].target = npc.target;
                    Main.npc[num35].velocity = Vector2.Normalize(npc.velocity).RotatedBy((float)Math.PI / 2f * npc.direction) * num17;
                    Main.npc[num35].netUpdate = true;
                    Main.npc[num35].ai[3] = Main.rand.Next(80, 121) / 100f;

                    Projectile.NewProjectile(npc.GetSpawnSourceForNPCFromNPCAI(), npc.Center, Main.npc[num35].velocity.Normalize(4), 1021, Main.npc[num35].damage / 5, 0);
                }
            }
            //npc.NewProjectile(npc.Center, Vector2.Zero, 919, npc.damage, (Vector2.Normalize(npc.velocity).RotatedBy((float)Math.PI / 2f * npc.direction) * num17).ToRotation(), npc.ai[2] / 100);
            //if (npc.ai[2] % 8 == 0f)
            //{
            //    npc.NewProjectile(npc.Center, Vector2.Zero, 465, npc.damage / 5, 0, 919);
            //}
            npc.velocity = npc.velocity.RotatedBy((0f - num19) * npc.direction);
            npc.rotation -= num19 * npc.direction;
            npc.ai[2] += 1f;
            if (npc.ai[2] >= num15)
            {
                npc.ai[0] = 5f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 8f)
        {
            npc.velocity *= 0.98f;
            npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, 0f, 0.02f);
            if (npc.ai[2] == num11 - 30)
            {
                SoundEngine.PlaySound(29, (int)npcCenter.X, (int)npcCenter.Y, 20);
            }
            if (Main.netMode != 1 && npc.ai[2] == num11 - 30)
            {
                for(int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && Vector2.Distance(npc.Center, Main.player[i].Center) < 3000 && i != npc.target && Main.rand.Next(3) == 0)
                    {
                        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npcCenter.X, npcCenter.Y, 0f, 0f, 385, 0, 0f, Main.myPlayer, 1f, i + 1, flag6 ? 1 : 0);
                    }
                }
                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npcCenter.X, npcCenter.Y, 0f, 0f, 385, 0, 0f, Main.myPlayer, 1f, npc.target + 1, flag6 ? 1 : 0);
            }
            npc.ai[2] += 1f;
            if (npc.ai[2] >= num11)
            {
                npc.ai[0] = 5f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 9f)
        {
            takeDamage = false;
            if (npc.ai[2] < num13 - 90)
            {
                if (Collision.SolidCollision(npc.position, npc.width, npc.height))
                {
                    npc.alpha += 15;
                }
                else
                {
                    npc.alpha -= 15;
                }
                if (npc.alpha < 0)
                {
                    npc.alpha = 0;
                }
                if (npc.alpha > 150)
                {
                    npc.alpha = 150;
                }
            }
            else if (npc.alpha < 255)
            {
                npc.alpha += 4;
                if (npc.alpha > 255)
                {
                    npc.alpha = 255;
                }
            }
            npc.velocity *= 0.98f;
            npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, 0f, 0.02f);
            if (npc.ai[2] == num13 - 60)
            {
                SoundEngine.PlaySound(29, (int)npcCenter.X, (int)npcCenter.Y, 20);
            }
            npc.ai[2] += 1f;
            if (npc.ai[2] >= num13)
            {
                npc.ai[0] = 10f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 10f && !targetPlayer.dead)
        {
            npc.chaseable = false;
            if (npc.alpha < 255)
            {
                npc.alpha += 25;
                if (npc.alpha > 255)
                {
                    npc.alpha = 255;
                }
            }
            if (npc.ai[1] == 0f)
            {
                npc.ai[1] = 360 * Math.Sign((npcCenter - targetPlayer.Center).X);
            }
            Vector2 desiredVelocity = Vector2.Normalize(targetPlayer.Center + new Vector2(npc.ai[1], -200f) - npcCenter - npc.velocity) * num4;
            npc.SimpleFlyMovement(desiredVelocity, num3);
            int num36 = Math.Sign(targetPlayer.Center.X - npcCenter.X);
            if (num36 != 0)
            {
                if (npc.ai[2] == 0f && num36 != npc.direction)
                {
                    npc.rotation += (float)Math.PI;
                    for (int l = 0; l < npc.oldPos.Length; l++)
                    {
                        npc.oldPos[l] = Vector2.Zero;
                    }
                }
                npc.direction = num36;
                if (npc.spriteDirection != -npc.direction)
                {
                    npc.rotation += (float)Math.PI;
                }
                npc.spriteDirection = -npc.direction;
            }
            npc.ai[2] += 1f;
            if (npc.ai[2] >= num2)
            {
                int num37 = 0;
                switch ((int)npc.ai[3])
                {
                    case 0:
                    case 2:
                    case 3:
                    case 5:
                    case 6:
                    case 7:
                        num37 = 1;
                        break;
                    case 1:
                    case 4:
                    case 8:
                        num37 = 2;
                        break;
                }
                switch (num37)
                {
                    case 1:
                        npc.ai[0] = 11f;
                        npc.ai[1] = 0f;
                        npc.ai[2] = 0f;
                        npc.velocity = Vector2.Normalize(targetPlayer.Center - npcCenter) * num6;
                        npc.NewProjectile(npc.velocity.Normalize(1f), ProjectileID.DemonSickle, npc.damage / 7);
                        npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X);
                        if (num36 != 0)
                        {
                            npc.direction = num36;
                            if (npc.spriteDirection == 1)
                            {
                                npc.rotation += (float)Math.PI;
                            }
                            npc.spriteDirection = -npc.direction;
                        }
                        break;
                    case 2:
                        npc.ai[0] = 12f;
                        npc.ai[1] = 0f;
                        npc.ai[2] = 0f;
                        break;
                    case 3:
                        npc.ai[0] = 13f;
                        npc.ai[1] = 0f;
                        npc.ai[2] = 0f;
                        break;
                }
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 11f)
        {
            npc.chaseable = true;
            npc.alpha -= 25;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }
            //int num38 = 7;
            //for (int m = 0; m < num38; m++)
            //{
            //    Vector2 vector13 = (Vector2.Normalize(npc.velocity) * new Vector2((npc.width + 50) / 2f, npc.height) * 0.75f).RotatedBy((m - (num38 / 2 - 1)) * Math.PI / (double)(float)num38) + npcCenter;
            //    Vector2 vector14 = ((float)(Main.rand.NextDouble() * 3.1415927410125732) - (float)Math.PI / 2f).ToRotationVector2() * Main.rand.Next(3, 8);
            //    int num39 = Dust.NewDust(vector13 + vector14, 0, 0, 172, vector14.X * 2f, vector14.Y * 2f, 100, default, 1.4f);
            //    Main.dust[num39].noGravity = true;
            //    Main.dust[num39].noLight = true;
            //    Main.dust[num39].velocity /= 4f;
            //    Main.dust[num39].velocity -= npc.velocity;
            //}
            npc.ai[2] += 1f;
            if (npc.ai[2] >= firstOrderSprintFrames)
            {
                npc.ai[0] = 10f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] += 1f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 12f)
        {
            takeDamage = false;
            npc.chaseable = false;
            if (npc.alpha < 255)
            {
                npc.alpha += 17;
                if (npc.alpha > 255)
                {
                    npc.alpha = 255;
                }
            }
            int num14 = 30;
            npc.velocity *= 0.98f;
            npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, 0f, 0.02f);
            if (npc.ai[2] == num14 / 2)
            {
                SoundEngine.PlaySound(29, (int)npcCenter.X, (int)npcCenter.Y, 20);
            }
            //if (Main.netMode != 1 && npc.ai[2] == num14 / 2)
            if (npc.ai[2] == num14 / 2)
            {
                if (npc.ai[1] == 0f)
                {
                    npc.ai[1] = 300 * Math.Sign(npcCenter.X - targetPlayer.Center.X);
                }

                int teleportY = -200;
                if (Main.rand.Next(2) == 0)
                {
                    npc.ai[1] = -npc.ai[1];
                }
                if (Main.rand.Next(2) == 0)
                {
                    teleportY = 400 + (int)targetPlayer.velocity.Y * 20;
                }
                
                //npcCenter = npc.Center = targetPlayer.Center + new Vector2(-npc.ai[1], -200f);
                npcCenter = npc.Center = targetPlayer.Center + new Vector2(-npc.ai[1], teleportY);
                int signX = Math.Sign(targetPlayer.Center.X - npcCenter.X);
                if (signX != 0)
                {
                    if (npc.ai[2] == 0f && signX != npc.direction)
                    {
                        npc.rotation += (float)Math.PI;
                        for (int n = 0; n < npc.oldPos.Length; n++)
                        {
                            npc.oldPos[n] = Vector2.Zero;
                        }
                    }
                    npc.direction = signX;
                    if (npc.spriteDirection != -npc.direction)
                    {
                        npc.rotation += (float)Math.PI;
                    }
                    npc.spriteDirection = -npc.direction;
                }
            }
            npc.ai[2] += 1f;
            if (npc.ai[2] >= num14)
            {
                npc.ai[0] = 10f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                //npc.ai[3] += 1f;
                npc.ai[3] = Utils.SelectRandom(Main.rand, 0, 2, 5);
                if (npc.ai[3] >= 9f)
                {
                    npc.ai[3] = 0f;
                }
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 13f)
        {
            if (npc.ai[2] == 0f)
            {
                SoundEngine.PlaySound(29, (int)npcCenter.X, (int)npcCenter.Y, 20);
            }
            npc.velocity = npc.velocity.RotatedBy((0f - num19) * npc.direction);
            npc.rotation -= num19 * npc.direction;
            npc.ai[2] += 1f;
            if (npc.ai[2] >= num15)
            {
                npc.ai[0] = 10f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] += 1f;
                npc.netUpdate = true;
            }
        }
        npc.dontTakeDamage = !takeDamage;
    }
}