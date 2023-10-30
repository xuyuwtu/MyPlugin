namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_030(this NPC npc)
    {
        if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
        {
            npc.TargetClosest();
        }
        bool dead2 = Main.player[npc.target].dead;
        float num401 = npc.position.X + npc.width / 2 - Main.player[npc.target].position.X - Main.player[npc.target].width / 2;
        float num402 = npc.position.Y + npc.height - 59f - Main.player[npc.target].position.Y - Main.player[npc.target].height / 2;
        float num403 = (float)Math.Atan2(num402, num401) + 1.57f;
        if (num403 < 0f)
        {
            num403 += 6.283f;
        }
        else if ((double)num403 > 6.283)
        {
            num403 -= 6.283f;
        }
        float num404 = 0.1f;
        if (npc.rotation < num403)
        {
            if ((double)(num403 - npc.rotation) > 3.1415)
            {
                npc.rotation -= num404;
            }
            else
            {
                npc.rotation += num404;
            }
        }
        else if (npc.rotation > num403)
        {
            if ((double)(npc.rotation - num403) > 3.1415)
            {
                npc.rotation += num404;
            }
            else
            {
                npc.rotation -= num404;
            }
        }
        if (npc.rotation > num403 - num404 && npc.rotation < num403 + num404)
        {
            npc.rotation = num403;
        }
        if (npc.rotation < 0f)
        {
            npc.rotation += 6.283f;
        }
        else if (npc.rotation > 6.283)
        {
            npc.rotation -= 6.283f;
        }
        if (npc.rotation > num403 - num404 && npc.rotation < num403 + num404)
        {
            npc.rotation = num403;
        }
        if (Main.rand.Next(5) == 0)
        {
            int num405 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + npc.height * 0.25f), npc.width, (int)(npc.height * 0.5f), 5, npc.velocity.X, 2f);
            Main.dust[num405].velocity.X *= 0.5f;
            Main.dust[num405].velocity.Y *= 0.1f;
        }
        if (Main.netMode != 1 && !Main.IsItDay() && !dead2 && npc.timeLeft < 10)
        {
            for (int num406 = 0; num406 < 200; num406++)
            {
                if (num406 != npc.whoAmI && Main.npc[num406].active && (Main.npc[num406].type == 125 || Main.npc[num406].type == 126))
                {
                    npc.DiscourageDespawn(Main.npc[num406].timeLeft - 1);
                }
            }
        }
        Vector2 vector41 = Vector2.Zero;
        if (NPC.IsMechQueenUp)
        {
            NPC nPC = Main.npc[NPC.mechQueen];
            Vector2 mechQueenCenter = nPC.GetMechQueenCenter();
            Vector2 vector42 = new(-150f, -250f);
            vector42 *= 0.75f;
            float num407 = nPC.velocity.X * 0.025f;
            vector41 = mechQueenCenter + vector42;
            vector41 = vector41.RotatedBy(num407, mechQueenCenter);
        }
        npc.reflectsProjectiles = false;
        if (Main.IsItDay() || dead2)
        {
            npc.velocity.Y -= 0.04f;
            npc.EncourageDespawn(10);
            return;
        }
        if (npc.ai[0] == 0f)
        {
            if (npc.ai[1] == 0f)
            {
                float num408 = 7f;
                float num409 = 0.1f;
                if (Main.expertMode)
                {
                    num408 = 8.25f;
                    num409 = 0.115f;
                }
                if (Main.getGoodWorld)
                {
                    num408 *= 1.15f;
                    num409 *= 1.15f;
                }
                int num410 = 1;
                if (npc.position.X + npc.width / 2 < Main.player[npc.target].position.X + Main.player[npc.target].width)
                {
                    num410 = -1;
                }
                Vector2 vector43 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float num411 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 + num410 * 300 - vector43.X;
                float num412 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - 300f - vector43.Y;
                if (NPC.IsMechQueenUp)
                {
                    num408 = 14f;
                    num411 = vector41.X;
                    num412 = vector41.Y;
                    num411 -= vector43.X;
                    num412 -= vector43.Y;
                }
                float num413 = (float)Math.Sqrt(num411 * num411 + num412 * num412);
                float num414 = num413;
                if (NPC.IsMechQueenUp)
                {
                    if (num413 > num408)
                    {
                        num413 = num408 / num413;
                        num411 *= num413;
                        num412 *= num413;
                    }
                    float num415 = 60f;
                    npc.velocity.X = (npc.velocity.X * (num415 - 1f) + num411) / num415;
                    npc.velocity.Y = (npc.velocity.Y * (num415 - 1f) + num412) / num415;
                }
                else
                {
                    num413 = num408 / num413;
                    num411 *= num413;
                    num412 *= num413;
                    if (npc.velocity.X < num411)
                    {
                        npc.velocity.X += num409;
                        if (npc.velocity.X < 0f && num411 > 0f)
                        {
                            npc.velocity.X += num409;
                        }
                    }
                    else if (npc.velocity.X > num411)
                    {
                        npc.velocity.X -= num409;
                        if (npc.velocity.X > 0f && num411 < 0f)
                        {
                            npc.velocity.X -= num409;
                        }
                    }
                    if (npc.velocity.Y < num412)
                    {
                        npc.velocity.Y += num409;
                        if (npc.velocity.Y < 0f && num412 > 0f)
                        {
                            npc.velocity.Y += num409;
                        }
                    }
                    else if (npc.velocity.Y > num412)
                    {
                        npc.velocity.Y -= num409;
                        if (npc.velocity.Y > 0f && num412 < 0f)
                        {
                            npc.velocity.Y -= num409;
                        }
                    }
                }
                int num416 = 600;
                int num417 = 60;
                if (NPC.IsMechQueenUp)
                {
                    num416 = 1200;
                    num417 = ((!NPC.npcsFoundForCheckActive[135]) ? 90 : 120);
                }
                npc.ai[2] += 1f;
                if (npc.ai[2] >= num416)
                {
                    npc.ai[1] = 1f;
                    npc.ai[2] = 0f;
                    npc.ai[3] = 0f;
                    npc.target = 255;
                    npc.netUpdate = true;
                }
                else if (npc.position.Y + npc.height < Main.player[npc.target].position.Y && num414 < 400f)
                {
                    if (!Main.player[npc.target].dead)
                    {
                        npc.ai[3] += 1f;
                        if (Main.expertMode && npc.life < npc.lifeMax * 0.9)
                        {
                            npc.ai[3] += 0.3f;
                        }
                        if (Main.expertMode && npc.life < npc.lifeMax * 0.8)
                        {
                            npc.ai[3] += 0.3f;
                        }
                        if (Main.expertMode && npc.life < npc.lifeMax * 0.7)
                        {
                            npc.ai[3] += 0.3f;
                        }
                        if (Main.expertMode && npc.life < npc.lifeMax * 0.6)
                        {
                            npc.ai[3] += 0.3f;
                        }
                        if (Main.getGoodWorld)
                        {
                            npc.ai[3] += 0.5f;
                        }
                    }
                    if (npc.ai[3] >= num417)
                    {
                        npc.ai[3] = 0f;
                        vector43 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                        num411 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector43.X;
                        num412 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector43.Y;
                        if (Main.netMode != 1)
                        {
                            float num418 = 9f;
                            int attackDamage_ForProjectiles3 = npc.GetAttackDamage_ForProjectiles(20f, 19f);
                            int num419 = 83;
                            if (Main.expertMode)
                            {
                                num418 = 10.5f;
                            }
                            num413 = (float)Math.Sqrt(num411 * num411 + num412 * num412);
                            num413 = num418 / num413;
                            num411 *= num413;
                            num412 *= num413;
                            num411 += Main.rand.Next(-40, 41) * 0.08f;
                            num412 += Main.rand.Next(-40, 41) * 0.08f;
                            vector43.X += num411 * 15f;
                            vector43.Y += num412 * 15f;
                            _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector43.X, vector43.Y, num411, num412, num419, attackDamage_ForProjectiles3, 0f, Main.myPlayer);
                        }
                    }
                }
            }
            else if (npc.ai[1] == 1f)
            {
                npc.rotation = num403;
                float num421 = 12f;
                if (Main.expertMode)
                {
                    num421 = 15f;
                }
                if (Main.getGoodWorld)
                {
                    num421 += 2f;
                }
                Vector2 vector44 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float num422 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector44.X;
                float num423 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector44.Y;
                float num424 = (float)Math.Sqrt(num422 * num422 + num423 * num423);
                num424 = num421 / num424;
                npc.velocity.X = num422 * num424;
                npc.velocity.Y = num423 * num424;
                npc.ai[1] = 2f;
            }
            else if (npc.ai[1] == 2f)
            {
                npc.ai[2] += 1f;
                if (npc.ai[2] >= 25f)
                {
                    npc.velocity.X *= 0.96f;
                    npc.velocity.Y *= 0.96f;
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
                if (npc.ai[2] >= 70f)
                {
                    npc.ai[3] += 1f;
                    npc.ai[2] = 0f;
                    npc.target = 255;
                    npc.rotation = num403;
                    if (npc.ai[3] >= 4f)
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
            if (npc.life < npc.lifeMax * 0.4)
            {
                npc.ai[0] = 1f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
            return;
        }
        if (npc.ai[0] == 1f || npc.ai[0] == 2f)
        {
            if (NPC.IsMechQueenUp)
            {
                npc.reflectsProjectiles = true;
            }
            if (npc.ai[0] == 1f)
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
            if (npc.ai[1] >= 100f)
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
                    for (int num425 = 0; num425 < 2; num425++)
                    {
                        Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 143);
                        Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 7);
                        Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 6);
                    }
                    for (int num426 = 0; num426 < 20; num426++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, 5, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f);
                    }
                    SoundEngine.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
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
        npc.damage = (int)(npc.defDamage * 1.5);
        npc.defense = npc.defDefense + 10;
        npc.HitSound = SoundID.NPCHit4;
        if (npc.ai[1] == 0f)
        {
            float num427 = 8f;
            float num428 = 0.15f;
            if (Main.expertMode)
            {
                num427 = 9.5f;
                num428 = 0.175f;
            }
            if (Main.getGoodWorld)
            {
                num427 *= 1.15f;
                num428 *= 1.15f;
            }
            Vector2 vector45 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num429 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector45.X;
            float num430 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - 300f - vector45.Y;
            if (NPC.IsMechQueenUp)
            {
                num427 = 14f;
                num429 = vector41.X;
                num430 = vector41.Y;
                num429 -= vector45.X;
                num430 -= vector45.Y;
            }
            float num431 = (float)Math.Sqrt(num429 * num429 + num430 * num430);
            if (NPC.IsMechQueenUp)
            {
                if (num431 > num427)
                {
                    num431 = num427 / num431;
                    num429 *= num431;
                    num430 *= num431;
                }
                npc.velocity.X = (npc.velocity.X * 4f + num429) / 5f;
                npc.velocity.Y = (npc.velocity.Y * 4f + num430) / 5f;
            }
            else
            {
                num431 = num427 / num431;
                num429 *= num431;
                num430 *= num431;
                if (npc.velocity.X < num429)
                {
                    npc.velocity.X += num428;
                    if (npc.velocity.X < 0f && num429 > 0f)
                    {
                        npc.velocity.X += num428;
                    }
                }
                else if (npc.velocity.X > num429)
                {
                    npc.velocity.X -= num428;
                    if (npc.velocity.X > 0f && num429 < 0f)
                    {
                        npc.velocity.X -= num428;
                    }
                }
                if (npc.velocity.Y < num430)
                {
                    npc.velocity.Y += num428;
                    if (npc.velocity.Y < 0f && num430 > 0f)
                    {
                        npc.velocity.Y += num428;
                    }
                }
                else if (npc.velocity.Y > num430)
                {
                    npc.velocity.Y -= num428;
                    if (npc.velocity.Y > 0f && num430 < 0f)
                    {
                        npc.velocity.Y -= num428;
                    }
                }
            }
            int num432 = 300;
            if (NPC.IsMechQueenUp)
            {
                num432 = 1200;
            }
            npc.ai[2] += 1f;
            if (npc.ai[2] >= num432)
            {
                npc.ai[1] = 1f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.TargetClosest();
                npc.netUpdate = true;
            }
            vector45 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            num429 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector45.X;
            num430 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector45.Y;
            npc.rotation = (float)Math.Atan2(num430, num429) - 1.57f;
            if (Main.netMode == 1)
            {
                return;
            }
            npc.localAI[1] += 1f;
            if (npc.life < npc.lifeMax * 0.75)
            {
                npc.localAI[1] += 1f;
            }
            if (npc.life < npc.lifeMax * 0.5)
            {
                npc.localAI[1] += 1f;
            }
            if (npc.life < npc.lifeMax * 0.25)
            {
                npc.localAI[1] += 1f;
            }
            if (npc.life < npc.lifeMax * 0.1)
            {
                npc.localAI[1] += 2f;
            }
            if (npc.localAI[1] > 180f && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
            {
                npc.localAI[1] = 0f;
                float num433 = 8.5f;
                int attackDamage_ForProjectiles4 = npc.GetAttackDamage_ForProjectiles(25f, 23f);
                int num434 = 100;
                if (Main.expertMode)
                {
                    num433 = 10f;
                }
                num431 = (float)Math.Sqrt(num429 * num429 + num430 * num430);
                num431 = num433 / num431;
                num429 *= num431;
                num430 *= num431;
                vector45.X += num429 * 15f;
                vector45.Y += num430 * 15f;
                _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector45.X, vector45.Y, num429, num430, num434, attackDamage_ForProjectiles4, 0f, Main.myPlayer);
            }
            return;
        }
        int num436 = 1;
        if (npc.position.X + npc.width / 2 < Main.player[npc.target].position.X + Main.player[npc.target].width)
        {
            num436 = -1;
        }
        float num437 = 8f;
        float num438 = 0.2f;
        if (Main.expertMode)
        {
            num437 = 9.5f;
            num438 = 0.25f;
        }
        if (Main.getGoodWorld)
        {
            num437 *= 1.15f;
            num438 *= 1.15f;
        }
        Vector2 vector46 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
        float num439 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 + num436 * 340 - vector46.X;
        float num440 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector46.Y;
        float num441 = (float)Math.Sqrt(num439 * num439 + num440 * num440);
        num441 = num437 / num441;
        num439 *= num441;
        num440 *= num441;
        if (npc.velocity.X < num439)
        {
            npc.velocity.X += num438;
            if (npc.velocity.X < 0f && num439 > 0f)
            {
                npc.velocity.X += num438;
            }
        }
        else if (npc.velocity.X > num439)
        {
            npc.velocity.X -= num438;
            if (npc.velocity.X > 0f && num439 < 0f)
            {
                npc.velocity.X -= num438;
            }
        }
        if (npc.velocity.Y < num440)
        {
            npc.velocity.Y += num438;
            if (npc.velocity.Y < 0f && num440 > 0f)
            {
                npc.velocity.Y += num438;
            }
        }
        else if (npc.velocity.Y > num440)
        {
            npc.velocity.Y -= num438;
            if (npc.velocity.Y > 0f && num440 < 0f)
            {
                npc.velocity.Y -= num438;
            }
        }
        vector46 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
        num439 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector46.X;
        num440 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector46.Y;
        npc.rotation = (float)Math.Atan2(num440, num439) - 1.57f;
        if (Main.netMode != 1)
        {
            npc.localAI[1] += 1f;
            if (npc.life < npc.lifeMax * 0.75)
            {
                npc.localAI[1] += 0.5f;
            }
            if (npc.life < npc.lifeMax * 0.5)
            {
                npc.localAI[1] += 0.75f;
            }
            if (npc.life < npc.lifeMax * 0.25)
            {
                npc.localAI[1] += 1f;
            }
            if (npc.life < npc.lifeMax * 0.1)
            {
                npc.localAI[1] += 1.5f;
            }
            if (Main.expertMode)
            {
                npc.localAI[1] += 1.5f;
            }
            if (npc.localAI[1] > 60f && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
            {
                npc.localAI[1] = 0f;
                float num442 = 9f;
                int attackDamage_ForProjectiles5 = npc.GetAttackDamage_ForProjectiles(18f, 17f);
                int num443 = 100;
                num441 = (float)Math.Sqrt(num439 * num439 + num440 * num440);
                num441 = num442 / num441;
                num439 *= num441;
                num440 *= num441;
                vector46.X += num439 * 15f;
                vector46.Y += num440 * 15f;
                _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector46.X, vector46.Y, num439, num440, num443, attackDamage_ForProjectiles5, 0f, Main.myPlayer);
            }
        }
        npc.ai[2] += 1f;
        if (npc.ai[2] >= 180f)
        {
            npc.ai[1] = 0f;
            npc.ai[2] = 0f;
            npc.ai[3] = 0f;
            npc.TargetClosest();
            npc.netUpdate = true;
        }
    }
}
