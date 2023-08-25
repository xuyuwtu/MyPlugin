using VBY.NPCAI;

namespace VBY.NPCTest;

public static partial class NPCAIs
{
    public static void AI_015(this NPC npc)
    {
        float num236 = 1f;
        float num237 = 1f;
        bool flag6 = false;
        bool flag7 = false;
        bool flag8 = false;
        float num238 = 2f;
        if (Main.getGoodWorld)
        {
            num238 -= 1f - npc.life / (float)npc.lifeMax;
            num237 *= num238;
        }
        npc.aiAction = 0;
        if (npc.ai[3] == 0f && npc.life > 0)
        {
            npc.ai[3] = npc.lifeMax;
        }
        if (npc.localAI[3] == 0f)
        {
            npc.localAI[3] = 1f;
            flag6 = true;

            npc.ai[0] = -100f;
            npc.TargetClosest();
            npc.netUpdate = true;

        }
        int distance = 3000;
        if (Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > distance)
        {
            npc.TargetClosest();
            if (Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > distance)
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
                if (npc.ai[1] != 5f)
                {
                    npc.netUpdate = true;
                    npc.ai[2] = 0f;
                    npc.ai[0] = 0f;
                    npc.ai[1] = 5f;
                    npc.localAI[1] = Main.maxTilesX * 16;
                    npc.localAI[2] = Main.maxTilesY * 16;
                }
            }
        }
        if (!Main.player[npc.target].dead && npc.timeLeft > 10 && npc.ai[2] >= 300f && npc.ai[1] < 5f && npc.velocity.Y == 0f)
        {
            //准备传送
            npc.ai[2] = 0f;
            npc.ai[0] = 0f;
            npc.ai[1] = 5f;

            npc.TargetClosest(faceTarget: false);
            Point point3 = npc.Center.ToTileCoordinates();
            Point point4 = Main.player[npc.target].Center.ToTileCoordinates();
            Vector2 vector30 = Main.player[npc.target].Center - npc.Center;
            int num240 = 10;
            int num241 = 0;
            int num242 = 7;
            int num243 = 0;
            bool flag9 = false;
            if (npc.localAI[0] >= 360f || vector30.Length() > 2000f)
            {
                if (npc.localAI[0] >= 360f)
                {
                    npc.localAI[0] = 360f;
                }
                flag9 = true;
                num243 = 100;
            }
            while (!flag9 && num243 < 100)
            {
                num243++;
                int num244 = Main.rand.Next(point4.X - num240, point4.X + num240 + 1);
                int num245 = Main.rand.Next(point4.Y - num240, point4.Y + 1);
                if ((num245 >= point4.Y - num242 && num245 <= point4.Y + num242 && num244 >= point4.X - num242 && num244 <= point4.X + num242) || (num245 >= point3.Y - num241 && num245 <= point3.Y + num241 && num244 >= point3.X - num241 && num244 <= point3.X + num241) || Main.tile[num244, num245].nactive())
                {
                    continue;
                }
                int num246 = num245;
                int num247 = 0;
                if (Main.tile[num244, num246].nactive() && Main.tileSolid[Main.tile[num244, num246].type] && !Main.tileSolidTop[Main.tile[num244, num246].type])
                {
                    num247 = 1;
                }
                else
                {
                    for (; num247 < 150 && num246 + num247 < Main.maxTilesY; num247++)
                    {
                        int y = num246 + num247;
                        if (Main.tile[num244, y].nactive() && Main.tileSolid[Main.tile[num244, y].type] && !Main.tileSolidTop[Main.tile[num244, y].type])
                        {
                            num247--;
                            break;
                        }
                    }
                }
                num245 += num247;
                bool flag10 = true;
                if (flag10 && Main.tile[num244, num245].lava())
                {
                    flag10 = false;
                }
                if (flag10 && !Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                {
                    flag10 = false;
                }
                if (flag10)
                {
                    npc.localAI[1] = num244 * 16 + 8;
                    npc.localAI[2] = num245 * 16 + 16;
                    break;
                }

                if (num243 >= 100)
                {
                    Vector2 bottom = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].Bottom;
                    npc.localAI[1] = bottom.X;
                    npc.localAI[2] = bottom.Y;
                }
            }
        }
        if (!Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0) || Math.Abs(npc.Top.Y - Main.player[npc.target].Bottom.Y) > 160f)
        {
            npc.ai[2]++;

            npc.localAI[0]++;
        }

        npc.localAI[0]--;
        if (npc.localAI[0] < 0f)
        {
            npc.localAI[0] = 0f;
        }

        if (npc.timeLeft < 10 && (npc.ai[0] != 0f || npc.ai[1] != 0f))
        {
            npc.ai[0] = 0f;
            npc.ai[1] = 0f;
            npc.netUpdate = true;
            flag7 = false;
        }
        Dust dust;
        if (npc.ai[1] == 5f)
        {
            flag7 = true;
            npc.aiAction = 1;
            npc.ai[0]++;
            num236 = MathHelper.Clamp((60f - npc.ai[0]) / 60f, 0f, 1f);
            num236 = 0.5f + num236 * 0.5f;
            if (npc.ai[0] >= 60f)
            {
                flag8 = true;
            }
            if (npc.ai[0] == 60f)
            {
                Gore.NewGore(npc.Center + new Vector2(-40f, -npc.height / 2), npc.velocity, 734);
            }
            //传送
            if (npc.ai[0] >= 60f)
            {
                npc.Bottom = new Vector2(npc.localAI[1], npc.localAI[2]);
                npc.ai[1] = 6f;
                npc.ai[0] = 0f;
                npc.netUpdate = true;
            }
            if (Main.netMode == 1 && npc.ai[0] >= 120f)
            {
                npc.ai[1] = 6f;
                npc.ai[0] = 0f;
            }
            if (!flag8)
            {
                for (int num248 = 0; num248 < 10; num248++)
                {
                    int num249 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 4, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
                    Main.dust[num249].noGravity = true;
                    dust = Main.dust[num249];
                    dust.velocity *= 0.5f;
                }
            }
        }
        else if (npc.ai[1] == 6f)
        {
            flag7 = true;
            npc.aiAction = 0;
            npc.ai[0]++;
            num236 = MathHelper.Clamp(npc.ai[0] / 30f, 0f, 1f);
            num236 = 0.5f + num236 * 0.5f;
            if (npc.ai[0] >= 30f && Main.netMode != 1)
            {
                npc.ai[1] = 0f;
                npc.ai[0] = 0f;
                npc.netUpdate = true;
                npc.TargetClosest();
            }
            if (Main.netMode == 1 && npc.ai[0] >= 60f)
            {
                npc.ai[1] = 0f;
                npc.ai[0] = 0f;
                npc.TargetClosest();
            }
            for (int num250 = 0; num250 < 10; num250++)
            {
                int num251 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 4, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
                Main.dust[num251].noGravity = true;
                dust = Main.dust[num251];
                dust.velocity *= 2f;
            }
        }
        npc.dontTakeDamage = npc.hide = flag8;
        if (npc.velocity.Y == 0f)
        {
            npc.velocity.X *= 0.8f;
            if (npc.ai[0] == -120 || npc.ai[0] == -200)
            {
                npc.NewProjectile(npc.BottomLeft + new Vector2(-48, 0), new Vector2(-1, -10), ProjectileID.DeerclopsIceSpike, npc.damage / 5, 0, 0.1f + (float)npc.life / npc.lifeMax * 2);
                npc.NewProjectile(npc.BottomLeft + new Vector2(-72, 0), new Vector2(-1, -10), ProjectileID.DeerclopsIceSpike, npc.damage / 5, 0, 0.1f + (float)npc.life / npc.lifeMax * 2);
                npc.NewProjectile(npc.BottomRight + new Vector2(48, 0), new Vector2(1, -10), ProjectileID.DeerclopsIceSpike, npc.damage / 5, 0, 0.1f + (float)npc.life / npc.lifeMax * 2);
                npc.NewProjectile(npc.BottomRight + new Vector2(72, 0), new Vector2(1, -10), ProjectileID.DeerclopsIceSpike, npc.damage / 5, 0, 0.1f + (float)npc.life / npc.lifeMax * 2);
            }
            if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
            {
                npc.velocity.X = 0f;
            }
            if (!flag7)
            {
                npc.ai[0] += 2f;
                if (npc.life < npc.lifeMax * 0.8)
                {
                    npc.ai[0] += 1f;
                }
                if (npc.life < npc.lifeMax * 0.6)
                {
                    npc.ai[0] += 1f;
                }
                if (npc.life < npc.lifeMax * 0.4)
                {
                    npc.ai[0] += 2f;
                }
                if (npc.life < npc.lifeMax * 0.2)
                {
                    npc.ai[0] += 3f;
                }
                if (npc.life < npc.lifeMax * 0.1)
                {
                    npc.ai[0] += 4f;
                }
                if (npc.ai[0] >= 0f)
                {
                    //跳
                    npc.netUpdate = true;
                    npc.TargetClosest();
                    if (npc.ai[1] == 3f)
                    {
                        //大跳
                        npc.velocity.Y = -13f;
                        npc.velocity.X += 3.5f * npc.direction;
                        npc.ai[0] = -200f;
                        npc.ai[1] = 0f;
                    }
                    else
                    {
                        if (Collision.CanHitLine(npc.position, 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                        {
                            var velocity = Main.player[npc.target].Top - npc.Center;
                            velocity.Y -= Main.rand.Next(0, 200);
                            var distance2 = Vector2.Distance(npc.Center, Main.player[npc.target].Center);
                            var num = 15f / distance2;
                            velocity *= num;
                            npc.NewProjectile(velocity, 174, npc.damage / 7);
                            npc.NewProjectile(velocity.RotatedBy(MathHelper.ToRadians(5)), 174, npc.damage / 7);
                            npc.NewProjectile(velocity.RotatedBy(MathHelper.ToRadians(-5)), 174, npc.damage / 7);
                        }
                        if (npc.ai[1] == 2f)
                        {
                            //小小跳
                            npc.velocity.Y = -6f;
                            npc.velocity.X += 4.5f * npc.direction;
                        }
                        else
                        {
                            //小跳
                            npc.velocity.Y = -8f;
                            npc.velocity.X += 4f * npc.direction;
                        }
                        npc.ai[0] = -120f;
                        npc.ai[1] += 1f;
                    }
                }
                else if (npc.ai[0] >= -30f)
                {
                    npc.aiAction = 1;
                }
            }
        }
        else if (npc.target < 255)
        {
            float num252 = 3f;
            if (Main.getGoodWorld)
            {
                num252 = 6f;
            }
            if ((npc.direction == 1 && npc.velocity.X < num252) || (npc.direction == -1 && npc.velocity.X > 0f - num252))
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

        //npc.AIOutput();

        if (npc.life <= 0)
        {
            return;
        }
        float num254 = npc.life / (float)npc.lifeMax;
        num254 = num254 * 0.5f + 0.75f;
        num254 *= num236;
        num254 *= num237;
        if (num254 != npc.scale || flag6)
        {
            npc.position.X += npc.width / 2;
            npc.position.Y += npc.height;
            npc.scale = num254;
            npc.width = (int)(98f * npc.scale);
            npc.height = (int)(92f * npc.scale);
            npc.position.X -= npc.width / 2;
            npc.position.Y -= npc.height;
        }
        if (!(npc.life + (int)(npc.lifeMax * 0.05) < npc.ai[3]))
        {
            return;
        }
        npc.ai[3] = npc.life;
        int spawnCount = Main.rand.Next(1, 4);
        for (int i = 0; i < spawnCount; i++)
        {
            int x = (int)(npc.position.X + Main.rand.Next(npc.width - 32));
            int y2 = (int)(npc.position.Y + Main.rand.Next(npc.height - 32));
            int npcType = 1;
            if (Main.expertMode && Main.rand.Next(4) == 0)
            {
                npcType = 535;
            }
            int npcIndex = NPC.NewNPC(npc.GetSpawnSourceForProjectileNPC(), x, y2, npcType);
            Main.npc[npcIndex].SetDefaults(npcType);
            Main.npc[npcIndex].velocity.X = Main.rand.Next(-15, 16) * 0.1f;
            Main.npc[npcIndex].velocity.Y = Main.rand.Next(-30, 1) * 0.1f;
            Main.npc[npcIndex].ai[0] = -1000 * Main.rand.Next(3);
            Main.npc[npcIndex].ai[1] = 0f;
            if (npcIndex < 200)
            {
                NetMessage.SendData(23, -1, -1, null, npcIndex);
            }
        }

    }
}
