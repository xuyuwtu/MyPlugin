namespace VBY.NPCTest;

partial class NPCAIs
{
    public static void AI_052(this NPC npc)
    {
        bool flag42 = false;
        bool flag43 = false;
        if (NPC.plantBoss < 0)
        {
            npc.StrikeNPCNoInteraction(9999, 0f, 0);
            npc.netUpdate = true;
            return;
        }
        if (Main.player[Main.npc[NPC.plantBoss].target].dead)
        {
            flag43 = true;
        }
        if ((NPC.plantBoss != -1 && !Main.player[Main.npc[NPC.plantBoss].target].ZoneJungle) || Main.player[Main.npc[NPC.plantBoss].target].position.Y < Main.worldSurface * 16.0 || Main.player[Main.npc[NPC.plantBoss].target].position.Y > Main.UnderworldLayer * 16 || flag43)
        {
            npc.localAI[0] -= 4f;
            flag42 = true;
        }
        if (Main.netMode == 1)
        {
            if (npc.ai[0] == 0f)
            {
                npc.ai[0] = (int)(npc.Center.X / 16f);
            }
            if (npc.ai[1] == 0f)
            {
                npc.ai[1] = (int)(npc.Center.X / 16f);
            }
        }
        if (Main.netMode != 1)
        {
            if (npc.ai[0] == 0f || npc.ai[1] == 0f)
            {
                npc.localAI[0] = 0f;
            }
            npc.localAI[0] -= 1f;
            if (Main.npc[NPC.plantBoss].life < Main.npc[NPC.plantBoss].lifeMax / 2)
            {
                npc.localAI[0] -= 2f;
            }
            if (Main.npc[NPC.plantBoss].life < Main.npc[NPC.plantBoss].lifeMax / 4)
            {
                npc.localAI[0] -= 2f;
            }
            if (flag42)
            {
                npc.localAI[0] -= 6f;
            }
            if (!flag43 && npc.localAI[0] <= 0f && npc.ai[0] != 0f)
            {
                for (int num800 = 0; num800 < 200; num800++)
                {
                    if (num800 != npc.whoAmI && Main.npc[num800].active && Main.npc[num800].type == npc.type && (Main.npc[num800].velocity.X != 0f || Main.npc[num800].velocity.Y != 0f))
                    {
                        npc.localAI[0] = Main.rand.Next(60, 300);
                    }
                }
            }
            if (npc.localAI[0] <= 0f)
            {
                npc.localAI[0] = Main.rand.Next(300, 600);
                bool flag44 = false;
                int num801 = 0;
                while (!flag44 && num801 <= 1000)
                {
                    num801++;
                    int num802 = (int)(Main.player[Main.npc[NPC.plantBoss].target].Center.X / 16f);
                    int num803 = (int)(Main.player[Main.npc[NPC.plantBoss].target].Center.Y / 16f);
                    if (npc.ai[0] == 0f)
                    {
                        num802 = (int)((Main.player[Main.npc[NPC.plantBoss].target].Center.X + Main.npc[NPC.plantBoss].Center.X) / 32f);
                        num803 = (int)((Main.player[Main.npc[NPC.plantBoss].target].Center.Y + Main.npc[NPC.plantBoss].Center.Y) / 32f);
                    }
                    if (flag43)
                    {
                        num802 = (int)Main.npc[NPC.plantBoss].position.X / 16;
                        num803 = (int)(Main.npc[NPC.plantBoss].position.Y + 400f) / 16;
                    }
                    int num804 = 20;
                    num804 += (int)(100f * (num801 / 1000f));
                    int num805 = num802 + Main.rand.Next(-num804, num804 + 1);
                    int num806 = num803 + Main.rand.Next(-num804, num804 + 1);
                    if (Main.npc[NPC.plantBoss].life < Main.npc[NPC.plantBoss].lifeMax / 2 && Main.rand.Next(6) == 0)
                    {
                        npc.TargetClosest();
                        int num807 = (int)(Main.player[npc.target].Center.X / 16f);
                        int num808 = (int)(Main.player[npc.target].Center.Y / 16f);
                        if (Main.tile[num807, num808].wall > 0)
                        {
                            num805 = num807;
                            num806 = num808;
                        }
                    }
                    try
                    {
                        if (WorldGen.InWorld(num805, num806) && (WorldGen.SolidTile(num805, num806) || (Main.tile[num805, num806].wall > 0 && (num801 > 500 || Main.npc[NPC.plantBoss].life < Main.npc[NPC.plantBoss].lifeMax / 2))))
                        {
                            flag44 = true;
                            npc.ai[0] = num805;
                            npc.ai[1] = num806;
                            npc.netUpdate = true;
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
        if (!(npc.ai[0] > 0f) || !(npc.ai[1] > 0f))
        {
            return;
        }
        float num809 = 6f;
        if (Main.npc[NPC.plantBoss].life < Main.npc[NPC.plantBoss].lifeMax / 2)
        {
            num809 = 8f;
        }
        if (Main.npc[NPC.plantBoss].life < Main.npc[NPC.plantBoss].lifeMax / 4)
        {
            num809 = 10f;
        }
        if (Main.expertMode)
        {
            num809 += 1f;
        }
        if (Main.expertMode && Main.npc[NPC.plantBoss].life < Main.npc[NPC.plantBoss].lifeMax / 2)
        {
            num809 += 1f;
        }
        if (flag42)
        {
            num809 *= 2f;
        }
        if (flag43)
        {
            num809 *= 2f;
        }
        Vector2 vector101 = new(npc.Center.X, npc.Center.Y);
        float num810 = npc.ai[0] * 16f - 8f - vector101.X;
        float num811 = npc.ai[1] * 16f - 8f - vector101.Y;
        float num812 = (float)Math.Sqrt(num810 * num810 + num811 * num811);
        if (num812 < 12f + num809)
        {
            if (Main.netMode != 1 && Main.getGoodWorld && npc.localAI[3] == 1f)
            {
                npc.localAI[3] = 0f;
                WorldGen.SpawnPlanteraThorns(npc.Center);
            }
            npc.velocity.X = num810;
            npc.velocity.Y = num811;
        }
        else
        {
            if (Main.netMode != 1 && Main.getGoodWorld)
            {
                npc.localAI[3] = 1f;
            }
            num812 = num809 / num812;
            npc.velocity.X = num810 * num812;
            npc.velocity.Y = num811 * num812;
        }
        Vector2 vector102 = new(npc.Center.X, npc.Center.Y);
        float num813 = Main.npc[NPC.plantBoss].Center.X - vector102.X;
        float num814 = Main.npc[NPC.plantBoss].Center.Y - vector102.Y;
        npc.rotation = (float)Math.Atan2(num814, num813) - 1.57f;
    }
}
