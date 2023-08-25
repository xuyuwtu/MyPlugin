using Terraria.GameContent.Events;

namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_083(this NPC npc)
    {
        if (npc.type == 437)
        {
            if (npc.localAI[3] == 0f && Main.netMode != 1)
            {
                npc.localAI[3] = 1f;
                npc.netUpdate = true;
                if (!CultistRitual.CheckFloor(npc.Center, out var spawnPoints))
                {
                    npc.life = 0;
                    npc.HitEffect();
                    npc.active = false;
                    return;
                }
                int num1307 = 0;
                int num1308 = 1;
                for (int num1309 = 0; num1309 < 4; num1309++)
                {
                    bool flag81 = num1309 == 1 || num1309 == 2;
                    int num1310 = ((!flag81) ? 379 : 438);
                    int num1311 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), spawnPoints[num1309].X * 16 + 8, spawnPoints[num1309].Y * 16 - 48, num1310);
                    if (flag81)
                    {
                        npc.localAI[num1308++] = num1311 + 1;
                        Main.npc[num1311].ai[3] = -(npc.whoAmI + 1);
                    }
                    else
                    {
                        npc.ai[num1307++] = num1311 + 1;
                        Main.npc[num1311].ai[3] = -(npc.whoAmI + 1);
                    }
                    Main.npc[num1311].netUpdate = true;
                }
            }
            if (npc.localAI[0] == 1f && Main.netMode != 1)
            {
                npc.localAI[0] = 2f;
                for (int num1312 = 0; num1312 < 2; num1312++)
                {
                    Main.npc[(int)npc.localAI[num1312 + 1] - 1].ai[1] = 1f;
                    Main.npc[(int)npc.localAI[num1312 + 1] - 1].netUpdate = true;
                    Main.npc[(int)npc.ai[num1312] - 1].ai[3] = 0f;
                    Main.npc[(int)npc.ai[num1312] - 1].TargetClosest();
                    Main.npc[(int)npc.ai[num1312] - 1].netUpdate = true;
                }
            }
            if (npc.ai[0] != -1f && Main.netMode != 1)
            {
                bool flag82 = true;
                for (int num1313 = 0; num1313 < 2; num1313++)
                {
                    if (Main.npc[(int)npc.localAI[num1313 + 1] - 1].active && Main.npc[(int)npc.localAI[num1313 + 1] - 1].type == 438)
                    {
                        flag82 = false;
                    }
                    if (Main.npc[(int)npc.ai[num1313] - 1].active && Main.npc[(int)npc.ai[num1313] - 1].type == 379)
                    {
                        flag82 = false;
                    }
                }
                if (flag82)
                {
                    npc.ai[0] = -1f;
                    npc.ai[1] = 0f;
                    npc.ai[3] = 0f;
                    int num1314 = (int)npc.Center.X / 16 + 11 * (Main.rand.Next(2) == 0).ToDirectionInt();
                    int num1315 = 0;
                    for (int num1316 = -5; num1316 < 12; num1316++)
                    {
                        int num1317 = num1314;
                        int num1318 = (int)npc.Center.Y / 16 + num1316;
                        if (WorldGen.SolidTile(num1317, num1318) && !Collision.SolidTiles(num1317 - 1, num1317 + 1, num1318 - 3, num1318 - 1))
                        {
                            num1315 = num1318;
                            break;
                        }
                        if (num1316 == 11)
                        {
                            num1315 = num1318;
                        }
                    }
                    int num1319 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), num1314 * 16 + 10, num1315 * 16 - 2, 439);
                    Main.npc[num1319].direction = (Main.npc[num1319].spriteDirection = Math.Sign(npc.Center.X - num1314 * 16 - 10f));
                    npc.ai[2] = num1319;
                    npc.netUpdate = true;
                    CultistRitual.TabletDestroyed();
                }
            }
            if (npc.ai[0] == -1f)
            {
                npc.ai[3]++;
                if (npc.ai[3] > 300f)
                {
                    npc.life = 0;
                    npc.HitEffect(0, 9999.0);
                    npc.active = false;
                    if (Main.netMode != 1)
                    {
                        for (int num1320 = 0; num1320 < 6; num1320++)
                        {
                            float num1321 = 3f + Main.rand.NextFloat() * 6f;
                            Vector2 vector241 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                            Vector2 center28 = npc.Center;
                            center28 += vector241 * 30f;
                            Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), center28.X, center28.Y, vector241.X * num1321, vector241.Y * num1321, 526, 0, 0f, Main.myPlayer, Main.npc[(int)npc.ai[2]].Center.X, Main.npc[(int)npc.ai[2]].Center.Y);
                        }
                        for (int num1322 = 0; num1322 < 20; num1322++)
                        {
                            if (Main.rand.Next(2) != 0)
                            {
                                float num1323 = 3f + Main.rand.NextFloat() * 6f;
                                Vector2 vector242 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                                Vector2 center29 = npc.Center;
                                center29 += vector242 * 30f;
                                Vector2 vector243 = npc.Center + vector242 * (Main.rand.NextFloat() * 45f + 45f) + Vector2.UnitY * 20f;
                                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), center29.X, center29.Y, vector242.X * num1323, -20f, 526, 0, 0f, Main.myPlayer, vector243.X, vector243.Y);
                            }
                        }
                    }
                }
                else if (npc.ai[3] % 10f == 1f && npc.ai[3] > 120f && Main.netMode != 1)
                {
                    float num1324 = 3f + Main.rand.NextFloat() * 6f;
                    Vector2 vector244 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                    Vector2 center30 = npc.Center;
                    center30 += vector244 * 25f;
                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), center30.X, center30.Y, vector244.X * num1324, vector244.Y * num1324, 526, 0, 0f, Main.myPlayer, Main.npc[(int)npc.ai[2]].Center.X, Main.npc[(int)npc.ai[2]].Center.Y);
                }
            }
        }
        if (npc.type == 438)
        {
            npc.velocity.X *= 0.93f;
            if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
            {
                npc.velocity.X = 0f;
            }
            int num1325 = (int)(0f - npc.ai[3] - 1f);
            if (num1325 == -1)
            {
                npc.life = 0;
                npc.HitEffect();
                npc.active = false;
                return;
            }
            int num1326 = Math.Sign(Main.npc[num1325].Center.X - npc.Center.X);
            if (num1326 != npc.direction)
            {
                npc.velocity.X = 0f;
                npc.direction = (npc.spriteDirection = num1326);
                npc.netUpdate = true;
            }
            if (npc.justHit && Main.netMode != 1 && Main.npc[num1325].localAI[0] == 0f)
            {
                Main.npc[num1325].localAI[0] = 1f;
            }
            if ((npc.ai[0] += 1f) >= 300f)
            {
                npc.ai[0] = 0f;
                npc.netUpdate = true;
            }
        }
        if (npc.type == 437)
        {
            Lighting.AddLight(npc.Center, 0.8f, 0.75f, 0.55f);
        }
    }
}
