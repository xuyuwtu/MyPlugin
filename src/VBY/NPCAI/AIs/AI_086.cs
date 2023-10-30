namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_086(this NPC npc)
    {
        if (npc.alpha > 0)
        {
            npc.alpha -= 30;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }
        }
        npc.noGravity = true;
        npc.noTileCollide = true;
        npc.knockBackResist = 0f;
        for (int num1342 = 0; num1342 < 200; num1342++)
        {
            if (num1342 == npc.whoAmI || !Main.npc[num1342].active || Main.npc[num1342].type != npc.type)
            {
                continue;
            }
            Vector2 vector252 = Main.npc[num1342].Center - npc.Center;
            if (!(vector252.Length() < 50f))
            {
                continue;
            }
            vector252.Normalize();
            if (vector252.X == 0f && vector252.Y == 0f)
            {
                if (num1342 > npc.whoAmI)
                {
                    vector252.X = 1f;
                }
                else
                {
                    vector252.X = -1f;
                }
            }
            vector252 *= 0.4f;
            npc.velocity -= vector252;
            NPC nPC3 = Main.npc[num1342];
            nPC3.velocity += vector252;
        }
        if (npc.type == 472)
        {
            float num1343 = 120f;
            if (npc.localAI[0] < num1343)
            {
                if (npc.localAI[0] == 0f)
                {
                    SoundEngine.PlaySound(SoundID.Item8, npc.Center);
                    npc.TargetClosest();
                    if (npc.direction > 0)
                    {
                        npc.velocity.X += 2f;
                    }
                    else
                    {
                        npc.velocity.X -= 2f;
                    }
                    npc.position += npc.netOffset;
                    for (int num1344 = 0; num1344 < 20; num1344++)
                    {
                        Vector2 center34 = npc.Center;
                        center34.Y -= 18f;
                        Vector2 vector253 = new(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                        vector253.Normalize();
                        vector253 *= Main.rand.Next(0, 100) * 0.1f;
                        center34 += vector253;
                        vector253.Normalize();
                        vector253 *= Main.rand.Next(50, 90) * 0.2f;
                        int num1345 = Dust.NewDust(center34, 1, 1, 27);
                        Main.dust[num1345].velocity = -vector253 * 0.3f;
                        Main.dust[num1345].alpha = 100;
                        if (Main.rand.Next(2) == 0)
                        {
                            Main.dust[num1345].noGravity = true;
                            Dust dust = Main.dust[num1345];
                            dust.scale += 0.3f;
                        }
                    }
                    npc.position -= npc.netOffset;
                }
                npc.localAI[0] += 1f;
                float num1346 = 1f - npc.localAI[0] / num1343;
                float num1347 = num1346 * 20f;
                for (int num1348 = 0; num1348 < num1347; num1348++)
                {
                    if (Main.rand.Next(5) == 0)
                    {
                        npc.position += npc.netOffset;
                        int num1349 = Dust.NewDust(npc.position, npc.width, npc.height, 27);
                        Main.dust[num1349].alpha = 100;
                        Dust dust = Main.dust[num1349];
                        dust.velocity *= 0.3f;
                        dust = Main.dust[num1349];
                        dust.velocity += npc.velocity * 0.75f;
                        Main.dust[num1349].noGravity = true;
                        npc.position -= npc.netOffset;
                    }
                }
            }
        }
        if (npc.type == 521)
        {
            float num1350 = 120f;
            if (npc.localAI[0] < num1350)
            {
                if (npc.localAI[0] == 0f)
                {
                    SoundEngine.PlaySound(SoundID.Item8, npc.Center);
                    npc.TargetClosest();
                    if (npc.direction > 0)
                    {
                        npc.velocity.X += 2f;
                    }
                    else
                    {
                        npc.velocity.X -= 2f;
                    }
                }
                npc.localAI[0] += 1f;
                int num1351 = 10;
                for (int num1352 = 0; num1352 < 2; num1352++)
                {
                    npc.position += npc.netOffset;
                    int num1353 = Dust.NewDust(npc.position - new Vector2(num1351), npc.width + num1351 * 2, npc.height + num1351 * 2, 228, 0f, 0f, 100, default, 2f);
                    Main.dust[num1353].noGravity = true;
                    Main.dust[num1353].noLight = true;
                    npc.position -= npc.netOffset;
                }
            }
        }
        if (npc.ai[0] == 0f)
        {
            npc.TargetClosest();
            npc.ai[0] = 1f;
            npc.ai[1] = npc.direction;
        }
        else if (npc.ai[0] == 1f)
        {
            npc.TargetClosest();
            float num1354 = 0.3f;
            float num1355 = 7f;
            float num1356 = 4f;
            float num1357 = 660f;
            float num1358 = 4f;
            if (npc.type == 521)
            {
                num1354 = 0.7f;
                num1355 = 14f;
                num1357 = 500f;
                num1356 = 6f;
                num1358 = 3f;
            }
            npc.velocity.X += npc.ai[1] * num1354;
            if (npc.velocity.X > num1355)
            {
                npc.velocity.X = num1355;
            }
            if (npc.velocity.X < 0f - num1355)
            {
                npc.velocity.X = 0f - num1355;
            }
            float num1359 = Main.player[npc.target].Center.Y - npc.Center.Y;
            if (Math.Abs(num1359) > num1356)
            {
                num1358 = 15f;
            }
            if (num1359 > num1356)
            {
                num1359 = num1356;
            }
            else if (num1359 < 0f - num1356)
            {
                num1359 = 0f - num1356;
            }
            npc.velocity.Y = (npc.velocity.Y * (num1358 - 1f) + num1359) / num1358;
            if ((npc.ai[1] > 0f && Main.player[npc.target].Center.X - npc.Center.X < 0f - num1357) || (npc.ai[1] < 0f && Main.player[npc.target].Center.X - npc.Center.X > num1357))
            {
                npc.ai[0] = 2f;
                npc.ai[1] = 0f;
                if (npc.Center.Y + 20f > Main.player[npc.target].Center.Y)
                {
                    npc.ai[1] = -1f;
                }
                else
                {
                    npc.ai[1] = 1f;
                }
            }
        }
        else if (npc.ai[0] == 2f)
        {
            float num1360 = 0.4f;
            float num1361 = 0.95f;
            float num1362 = 5f;
            if (npc.type == 521)
            {
                num1360 = 0.3f;
                num1362 = 7f;
                num1361 = 0.9f;
            }
            npc.velocity.Y += npc.ai[1] * num1360;
            if (npc.velocity.Length() > num1362)
            {
                npc.velocity *= num1361;
            }
            if (npc.velocity.X > -1f && npc.velocity.X < 1f)
            {
                npc.TargetClosest();
                npc.ai[0] = 3f;
                npc.ai[1] = npc.direction;
            }
        }
        else if (npc.ai[0] == 3f)
        {
            float num1363 = 0.4f;
            float num1364 = 0.2f;
            float num1365 = 5f;
            float num1366 = 0.95f;
            if (npc.type == 521)
            {
                num1363 = 0.6f;
                num1364 = 0.3f;
                num1365 = 7f;
                num1366 = 0.9f;
            }
            npc.velocity.X += npc.ai[1] * num1363;
            if (npc.Center.Y > Main.player[npc.target].Center.Y)
            {
                npc.velocity.Y -= num1364;
            }
            else
            {
                npc.velocity.Y += num1364;
            }
            if (npc.velocity.Length() > num1365)
            {
                npc.velocity *= num1366;
            }
            if (npc.velocity.Y > -1f && npc.velocity.Y < 1f)
            {
                npc.TargetClosest();
                npc.ai[0] = 0f;
                npc.ai[1] = npc.direction;
            }
        }
        if (npc.type == 521)
        {
            int num1367 = 10;
            npc.position += npc.netOffset;
            for (int num1368 = 0; num1368 < 1; num1368++)
            {
                int num1369 = Dust.NewDust(npc.position - new Vector2(num1367), npc.width + num1367 * 2, npc.height + num1367 * 2, 228, 0f, 0f, 100, default, 2f);
                Main.dust[num1369].noGravity = true;
                Main.dust[num1369].noLight = true;
            }
            npc.position -= npc.netOffset;
        }
    }
}
