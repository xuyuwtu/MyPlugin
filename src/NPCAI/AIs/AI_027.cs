namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_027(this NPC npc)
    {
        if (npc.position.X < 160f || npc.position.X > (Main.maxTilesX - 10) * 16)
        {
            npc.active = false;
        }
        if (npc.localAI[0] == 0f)
        {
            npc.localAI[0] = 1f;
            Main.wofDrawAreaBottom = -1;
            Main.wofDrawAreaTop = -1;
        }
        if (Main.getGoodWorld && Main.netMode != 1 && Main.rand.Next(180) == 0 && NPC.CountNPCS(24) < 4)
        {
            int num338 = 1;
            for (int num339 = 0; num339 < num338; num339++)
            {
                int num340 = 1000;
                for (int num341 = 0; num341 < num340; num341++)
                {
                    int num342 = (int)(npc.Center.X / 16f);
                    int num343 = (int)(npc.Center.Y / 16f);
                    if (npc.target >= 0)
                    {
                        num342 = (int)(Main.player[npc.target].Center.X / 16f);
                        num343 = (int)(Main.player[npc.target].Center.Y / 16f);
                    }
                    num342 += Main.rand.Next(-50, 51);
                    for (num343 += Main.rand.Next(-50, 51); num343 < Main.maxTilesY - 10 && !WorldGen.SolidTile(num342, num343); num343++)
                    {
                    }
                    num343--;
                    if (!WorldGen.SolidTile(num342, num343))
                    {
                        int num344 = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num342 * 16 + 8, num343 * 16, 24);
                        if (Main.netMode == 2 && num344 < 200)
                        {
                            NetMessage.SendData(23, -1, -1, null, num344);
                        }
                        break;
                    }
                }
            }
        }
        npc.ai[1] += 1f;
        if (npc.ai[2] == 0f)
        {
            if (npc.life < npc.lifeMax * 0.5)
            {
                npc.ai[1] += 1f;
            }
            if (npc.life < npc.lifeMax * 0.2)
            {
                npc.ai[1] += 1f;
            }
            if (npc.ai[1] > 2700f)
            {
                npc.ai[2] = 1f;
            }
        }
        int num345 = 60;
        if (npc.ai[2] > 0f && npc.ai[1] > num345)
        {
            int num346 = 3;
            if (npc.life < npc.lifeMax * 0.3)
            {
                num346++;
            }
            npc.ai[2] += 1f;
            npc.ai[1] = 0f;
            if (npc.ai[2] > num346)
            {
                npc.ai[2] = 0f;
            }
            if (Main.netMode != 1 && NPC.CountNPCS(117) < 10)
            {
                int num347 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height / 2 + 20f), 117, 1);
                Main.npc[num347].velocity.X = npc.direction * 8;
            }
        }
        npc.localAI[3] += 1f;
        if (npc.localAI[3] >= 600 + Main.rand.Next(1000))
        {
            npc.localAI[3] = -Main.rand.Next(200);
            SoundEngine.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 10);
        }
        int num348 = Main.UnderworldLayer + 10;
        int num349 = num348 + 70;
        Main.wofNPCIndex = npc.whoAmI;
        int num350 = (int)(npc.position.X / 16f);
        int num351 = (int)((npc.position.X + npc.width) / 16f);
        int num352 = (int)((npc.position.Y + npc.height / 2) / 16f);
        int num353 = 0;
        int num354 = num352 + 7;
        while (num353 < 15 && num354 > Main.UnderworldLayer)
        {
            num354++;
            if (num354 > Main.maxTilesY - 10)
            {
                num354 = Main.maxTilesY - 10;
                break;
            }
            if (num354 < num348)
            {
                continue;
            }
            for (int num355 = num350; num355 <= num351; num355++)
            {
                try
                {
                    if (WorldGen.InWorld(num355, num354, 2) && (WorldGen.SolidTile(num355, num354) || Main.tile[num355, num354].liquid > 0))
                    {
                        num353++;
                    }
                }
                catch
                {
                    num353 += 15;
                }
            }
        }
        num354 += 4;
        if (Main.wofDrawAreaBottom == -1)
        {
            Main.wofDrawAreaBottom = num354 * 16;
        }
        else if (Main.wofDrawAreaBottom > num354 * 16)
        {
            Main.wofDrawAreaBottom--;
            if (Main.wofDrawAreaBottom < num354 * 16)
            {
                Main.wofDrawAreaBottom = num354 * 16;
            }
        }
        else if (Main.wofDrawAreaBottom < num354 * 16)
        {
            Main.wofDrawAreaBottom++;
            if (Main.wofDrawAreaBottom > num354 * 16)
            {
                Main.wofDrawAreaBottom = num354 * 16;
            }
        }
        num353 = 0;
        num354 = num352 - 7;
        while (num353 < 15 && num354 < Main.maxTilesY - 10)
        {
            num354--;
            if (num354 <= 10)
            {
                num354 = 10;
                break;
            }
            if (num354 > num349)
            {
                continue;
            }
            if (num354 < num348)
            {
                num354 = num348;
                break;
            }
            for (int num356 = num350; num356 <= num351; num356++)
            {
                try
                {
                    if (WorldGen.InWorld(num356, num354, 2) && (WorldGen.SolidTile(num356, num354) || Main.tile[num356, num354].liquid > 0))
                    {
                        num353++;
                    }
                }
                catch
                {
                    num353 += 15;
                }
            }
        }
        num354 -= 4;
        if (Main.wofDrawAreaTop == -1)
        {
            Main.wofDrawAreaTop = num354 * 16;
        }
        else if (Main.wofDrawAreaTop > num354 * 16)
        {
            Main.wofDrawAreaTop--;
            if (Main.wofDrawAreaTop < num354 * 16)
            {
                Main.wofDrawAreaTop = num354 * 16;
            }
        }
        else if (Main.wofDrawAreaTop < num354 * 16)
        {
            Main.wofDrawAreaTop++;
            if (Main.wofDrawAreaTop > num354 * 16)
            {
                Main.wofDrawAreaTop = num354 * 16;
            }
        }
        Main.wofDrawAreaTop = (int)MathHelper.Clamp(Main.wofDrawAreaTop, num348 * 16f, num349 * 16f);
        Main.wofDrawAreaBottom = (int)MathHelper.Clamp(Main.wofDrawAreaBottom, num348 * 16f, num349 * 16f);
        if (Main.wofDrawAreaTop > Main.wofDrawAreaBottom - 160)
        {
            Main.wofDrawAreaTop = Main.wofDrawAreaBottom - 160;
        }
        else if (Main.wofDrawAreaBottom < Main.wofDrawAreaTop + 160)
        {
            Main.wofDrawAreaBottom = Main.wofDrawAreaTop + 160;
        }
        float num357 = (Main.wofDrawAreaBottom + Main.wofDrawAreaTop) / 2 - npc.height / 2;
        if (npc.position.Y > num357 + 1f)
        {
            npc.velocity.Y = -1f;
        }
        else if (npc.position.Y < num357 - 1f)
        {
            npc.velocity.Y = 1f;
        }
        npc.velocity.Y = 0f;
        npc.position.Y = num357;
        float num358 = 1.5f;
        if (npc.life < npc.lifeMax * 0.75)
        {
            num358 += 0.25f;
        }
        if (npc.life < npc.lifeMax * 0.5)
        {
            num358 += 0.4f;
        }
        if (npc.life < npc.lifeMax * 0.25)
        {
            num358 += 0.5f;
        }
        if (npc.life < npc.lifeMax * 0.1)
        {
            num358 += 0.6f;
        }
        if (npc.life < npc.lifeMax * 0.66 && Main.expertMode)
        {
            num358 += 0.3f;
        }
        if (npc.life < npc.lifeMax * 0.33 && Main.expertMode)
        {
            num358 += 0.3f;
        }
        if (npc.life < npc.lifeMax * 0.05 && Main.expertMode)
        {
            num358 += 0.6f;
        }
        if (npc.life < npc.lifeMax * 0.035 && Main.expertMode)
        {
            num358 += 0.6f;
        }
        if (npc.life < npc.lifeMax * 0.025 && Main.expertMode)
        {
            num358 += 0.6f;
        }
        if (Main.expertMode)
        {
            num358 *= 1.35f;
            num358 += 0.35f;
        }
        if (Main.getGoodWorld)
        {
            num358 *= 1.1f;
            num358 += 0.2f;
        }
        if (npc.velocity.X == 0f)
        {
            npc.TargetClosest();
            if (Main.player[npc.target].dead)
            {
                float num359 = float.PositiveInfinity;
                int num360 = 0;
                for (int num361 = 0; num361 < 255; num361++)
                {
                    Player player = Main.player[npc.target];
                    if (player.active)
                    {
                        float num362 = npc.Distance(player.Center);
                        if (num359 > num362)
                        {
                            num359 = num362;
                            num360 = ((npc.Center.X < player.Center.X) ? 1 : (-1));
                        }
                    }
                }
                npc.direction = num360;
            }
            npc.velocity.X = npc.direction;
        }
        if (npc.velocity.X < 0f)
        {
            npc.velocity.X = 0f - num358;
            npc.direction = -1;
        }
        else
        {
            npc.velocity.X = num358;
            npc.direction = 1;
        }
        if (Main.player[npc.target].dead || !Main.player[npc.target].gross)
        {
            npc.TargetClosest_WOF();
        }
        if (Main.player[npc.target].dead)
        {
            npc.localAI[1] += 1f / 180f;
            if (npc.localAI[1] >= 1f)
            {
                SoundEngine.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 10);
                npc.life = 0;
                npc.active = false;
                if (Main.netMode != 1)
                {
                    NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f);
                }
                return;
            }
        }
        else
        {
            npc.localAI[1] = MathHelper.Clamp(npc.localAI[1] - 1f / 30f, 0f, 1f);
        }
        npc.spriteDirection = npc.direction;
        Vector2 vector38 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
        float num363 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector38.X;
        float num364 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector38.Y;
        float num365 = (float)Math.Sqrt(num363 * num363 + num364 * num364);
        num363 *= num365;
        num364 *= num365;
        if (npc.direction > 0)
        {
            if (Main.player[npc.target].position.X + Main.player[npc.target].width / 2 > npc.position.X + npc.width / 2)
            {
                npc.rotation = (float)Math.Atan2(0f - num364, 0f - num363) + 3.14f;
            }
            else
            {
                npc.rotation = 0f;
            }
        }
        else if (Main.player[npc.target].position.X + Main.player[npc.target].width / 2 < npc.position.X + npc.width / 2)
        {
            npc.rotation = (float)Math.Atan2(num364, num363) + 3.14f;
        }
        else
        {
            npc.rotation = 0f;
        }
        if (Main.expertMode && Main.netMode != 1)
        {
            int num367 = (int)(1f + npc.life / (float)npc.lifeMax * 10f);
            num367 *= num367;
            if (num367 < 400)
            {
                num367 = (num367 * 19 + 400) / 20;
            }
            if (num367 < 60)
            {
                num367 = (num367 * 3 + 60) / 4;
            }
            if (num367 < 20)
            {
                num367 = (num367 + 20) / 2;
            }
            num367 = (int)(num367 * 0.7);
            if (Main.rand.Next(num367) == 0)
            {
                int num368 = 0;
                float[] array = new float[10];
                for (int num369 = 0; num369 < 200; num369++)
                {
                    if (num368 < 10 && Main.npc[num369].active && Main.npc[num369].type == 115)
                    {
                        array[num368] = Main.npc[num369].ai[0];
                        num368++;
                    }
                }
                int maxValue = 1 + num368 * 2;
                if (num368 < 10 && Main.rand.Next(maxValue) <= 1)
                {
                    int num370 = -1;
                    for (int num371 = 0; num371 < 1000; num371++)
                    {
                        int num372 = Main.rand.Next(10);
                        float num373 = num372 * 0.1f - 0.05f;
                        bool flag26 = true;
                        for (int num374 = 0; num374 < num368; num374++)
                        {
                            if (num373 == array[num374])
                            {
                                flag26 = false;
                                break;
                            }
                        }
                        if (flag26)
                        {
                            num370 = num372;
                            break;
                        }
                    }
                    if (num370 >= 0)
                    {
                        NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.position.X, (int)num357, 115, npc.whoAmI, num370 * 0.1f - 0.05f);
                    }
                }
            }
        }
        if (Main.netMode != 1 && npc.localAI[0] == 1f)
        {
            npc.localAI[0] = 2f;
            float num375 = (npc.Center.Y + Main.wofDrawAreaTop) / 2f;
            _ = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.position.X, (int)num375, 114, npc.whoAmI, 1f);
            float num377 = (npc.Center.Y + Main.wofDrawAreaBottom) / 2f;
            _ = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.position.X, (int)num377, 114, npc.whoAmI, -1f);
            float num378 = (npc.Center.Y + Main.wofDrawAreaBottom) / 2f;
            for (int num379 = 0; num379 < 11; num379++)
            {
                _ = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.position.X, (int)num378, 115, npc.whoAmI, num379 * 0.1f - 0.05f);
            }
        }
    }
}
