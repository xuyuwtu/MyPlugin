namespace VBY.NPCTest;

public static partial class NPCAIs
{
    public static void AI_031(this NPC npc)
    {
        if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
        {
            npc.TargetClosest(); 
        }
        bool dead3 = Main.player[npc.target].dead;
        float num445 = npc.position.X + npc.width / 2 - Main.player[npc.target].position.X - Main.player[npc.target].width / 2;
        float num446 = npc.position.Y + npc.height - 59f - Main.player[npc.target].position.Y - Main.player[npc.target].height / 2;
        float num447 = (float)Math.Atan2(num446, num445) + 1.57f;
        if (num447 < 0f)
        {
            num447 += 6.283f;
        }
        else if ((double)num447 > 6.283)
        {
            num447 -= 6.283f;
        }
        float num448 = 0.15f;
        if (NPC.IsMechQueenUp && npc.ai[0] == 3f && npc.ai[1] == 0f)
        {
            num448 *= 0.25f;
        }
        if (npc.rotation < num447)
        {
            if ((double)(num447 - npc.rotation) > 3.1415)
            {
                npc.rotation -= num448;
            }
            else
            {
                npc.rotation += num448;
            }
        }
        else if (npc.rotation > num447)
        {
            if ((double)(npc.rotation - num447) > 3.1415)
            {
                npc.rotation += num448;
            }
            else
            {
                npc.rotation -= num448;
            }
        }
        if (npc.rotation > num447 - num448 && npc.rotation < num447 + num448)
        {
            npc.rotation = num447;
        }
        if (npc.rotation < 0f)
        {
            npc.rotation += 6.283f;
        }
        else if (npc.rotation > 6.283)
        {
            npc.rotation -= 6.283f;
        }
        if (npc.rotation > num447 - num448 && npc.rotation < num447 + num448)
        {
            npc.rotation = num447;
        }
        if (Main.rand.Next(5) == 0)
        {
            int num449 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + npc.height * 0.25f), npc.width, (int)(npc.height * 0.5f), 5, npc.velocity.X, 2f);
            Main.dust[num449].velocity.X *= 0.5f;
            Main.dust[num449].velocity.Y *= 0.1f;
        }
        if (Main.netMode != 1 && !Main.IsItDay() && !dead3 && npc.timeLeft < 10)
        {
            for (int num450 = 0; num450 < 200; num450++)
            {
                if (num450 != npc.whoAmI && Main.npc[num450].active && (Main.npc[num450].type == 125 || Main.npc[num450].type == 126))
                {
                    npc.DiscourageDespawn(Main.npc[num450].timeLeft - 1);
                }
            }
        }
        Vector2 vector47 = Vector2.Zero;
        if (NPC.IsMechQueenUp)
        {
            NPC nPC2 = Main.npc[NPC.mechQueen];
            Vector2 mechQueenCenter2 = nPC2.GetMechQueenCenter();
            Vector2 vector48 = new(150f, -250f);
            vector48 *= 0.75f;
            float num451 = nPC2.velocity.X * 0.025f;
            vector47 = mechQueenCenter2 + vector48;
            vector47 = vector47.RotatedBy(num451, mechQueenCenter2);
        }
        npc.reflectsProjectiles = false;
        if (Main.IsItDay() || dead3)
        {
            npc.velocity.Y -= 0.04f;
            npc.EncourageDespawn(10);
            return;
        }
        if (npc.ai[0] == 0f)
        {
            if (npc.ai[1] == 0f)
            {
                npc.TargetClosest();
                float num452 = 12f;
                float num453 = 0.4f;
                if (Main.getGoodWorld)
                {
                    num452 *= 1.15f;
                    num453 *= 1.15f;
                }
                int num454 = 1;
                if (npc.position.X + npc.width / 2 < Main.player[npc.target].position.X + Main.player[npc.target].width)
                {
                    num454 = -1;
                }
                Vector2 vector49 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float num455 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 + num454 * 400 - vector49.X;
                float num456 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector49.Y;
                if (NPC.IsMechQueenUp)
                {
                    num452 = 14f;
                    num455 = vector47.X;
                    num456 = vector47.Y;
                    num455 -= vector49.X;
                    num456 -= vector49.Y;
                }
                float num457 = (float)Math.Sqrt(num455 * num455 + num456 * num456);
                if (NPC.IsMechQueenUp)
                {
                    if (num457 > num452)
                    {
                        num457 = num452 / num457;
                        num455 *= num457;
                        num456 *= num457;
                    }
                    npc.velocity.X = (npc.velocity.X * 4f + num455) / 5f;
                    npc.velocity.Y = (npc.velocity.Y * 4f + num456) / 5f;
                }
                else
                {
                    num457 = num452 / num457;
                    num455 *= num457;
                    num456 *= num457;
                    if (npc.velocity.X < num455)
                    {
                        npc.velocity.X += num453;
                        if (npc.velocity.X < 0f && num455 > 0f)
                        {
                            npc.velocity.X += num453;
                        }
                    }
                    else if (npc.velocity.X > num455)
                    {
                        npc.velocity.X -= num453;
                        if (npc.velocity.X > 0f && num455 < 0f)
                        {
                            npc.velocity.X -= num453;
                        }
                    }
                    if (npc.velocity.Y < num456)
                    {
                        npc.velocity.Y += num453;
                        if (npc.velocity.Y < 0f && num456 > 0f)
                        {
                            npc.velocity.Y += num453;
                        }
                    }
                    else if (npc.velocity.Y > num456)
                    {
                        npc.velocity.Y -= num453;
                        if (npc.velocity.Y > 0f && num456 < 0f)
                        {
                            npc.velocity.Y -= num453;
                        }
                    }
                }
                int num459 = 600;
                if (NPC.IsMechQueenUp)
                {
                    num459 = 1200;
                }
                npc.ai[2] += 1f;
                if (npc.ai[2] >= num459)
                {
                    npc.ai[1] = 1f;
                    npc.ai[2] = 0f;
                    npc.ai[3] = 0f;
                    npc.target = 255;
                    npc.netUpdate = true;
                }
                else
                {
                    if (!Main.player[npc.target].dead)
                    {
                        npc.ai[3] += 1f;
                        if (Main.expertMode && npc.life < npc.lifeMax * 0.8)
                        {
                            npc.ai[3] += 0.6f;
                        }
                        if (Main.getGoodWorld)
                        {
                            npc.ai[3] += 0.4f;
                        }
                    }
                    if (npc.ai[3] >= 60f)
                    {
                        npc.ai[3] = 0f;
                        vector49 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                        num455 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector49.X;
                        num456 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector49.Y;
                        if (Main.netMode != 1)
                        {
                            float num460 = 12f;
                            int attackDamage_ForProjectiles6 = npc.GetAttackDamage_ForProjectiles(25f, 22f);
                            int projType = 96;
                            if (Main.expertMode)
                            {
                                num460 = 14f;
                            }
                            num457 = (float)Math.Sqrt(num455 * num455 + num456 * num456);
                            num457 = num460 / num457;
                            num455 *= num457;
                            num456 *= num457;
                            num455 += Main.rand.Next(-40, 41) * 0.05f;
                            num456 += Main.rand.Next(-40, 41) * 0.05f;
                            vector49.X += num455 * 4f;
                            vector49.Y += num456 * 4f;
                            Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector49.X, vector49.Y, num455, num456, projType, attackDamage_ForProjectiles6, 0f, Main.myPlayer);

                            var velocity = new Vector2(num455,num456);
                            npc.NewProjectile(vector49, velocity.RotatedByDegress(5), projType, attackDamage_ForProjectiles6);
                            npc.NewProjectile(vector49, velocity.RotatedByDegress(-5), projType, attackDamage_ForProjectiles6);
                        }
                    }
                }
            }
            else if (npc.ai[1] == 1f)
            {
                npc.rotation = num447;
                float num463 = 13f;
                if (Main.expertMode)
                {
                    if (npc.life < npc.lifeMax * 0.9)
                    {
                        num463 += 0.5f;
                    }
                    if (npc.life < npc.lifeMax * 0.8)
                    {
                        num463 += 0.5f;
                    }
                    if (npc.life < npc.lifeMax * 0.7)
                    {
                        num463 += 0.55f;
                    }
                    if (npc.life < npc.lifeMax * 0.6)
                    {
                        num463 += 0.6f;
                    }
                    if (npc.life < npc.lifeMax * 0.5)
                    {
                        num463 += 0.65f;
                    }
                }
                if (Main.getGoodWorld)
                {
                    num463 *= 1.2f;
                }
                Vector2 vector50 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float num464 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector50.X;
                float num465 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector50.Y;
                float num466 = (float)Math.Sqrt(num464 * num464 + num465 * num465);
                num466 = num463 / num466;
                npc.velocity.X = num464 * num466;
                npc.velocity.Y = num465 * num466;
                npc.ai[1] = 2f;
            }
            else if (npc.ai[1] == 2f)
            {
                npc.ai[2] += 1f;
                if (npc.ai[2] >= 8f)
                {
                    npc.velocity.X *= 0.9f;
                    npc.velocity.Y *= 0.9f;
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
                if (npc.ai[2] >= 42f)
                {
                    npc.ai[3] += 1f;
                    npc.ai[2] = 0f;
                    npc.target = 255;
                    npc.rotation = num447;
                    if (npc.ai[3] >= 10f)
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
                    for (int num467 = 0; num467 < 2; num467++)
                    {
                        Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 144);
                        Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 7);
                        Gore.NewGore(npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 6);
                    }
                    for (int num468 = 0; num468 < 20; num468++)
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
        npc.HitSound = SoundID.NPCHit4;
        npc.damage = (int)(npc.defDamage * 1.5);
        npc.defense = npc.defDefense + 18;
        if (npc.ai[1] == 0f)
        {
            float num469 = 4f;
            float num470 = 0.1f;
            int num471 = 1;
            if (npc.position.X + npc.width / 2 < Main.player[npc.target].position.X + Main.player[npc.target].width)
            {
                num471 = -1;
            }
            Vector2 vector51 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num472 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 + num471 * 180 - vector51.X;
            float num473 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector51.Y;
            float num474 = (float)Math.Sqrt(num472 * num472 + num473 * num473);
            if (!NPC.IsMechQueenUp)
            {
                if (Main.expertMode)
                {
                    if (num474 > 300f)
                    {
                        num469 += 0.5f;
                    }
                    if (num474 > 400f)
                    {
                        num469 += 0.5f;
                    }
                    if (num474 > 500f)
                    {
                        num469 += 0.55f;
                    }
                    if (num474 > 600f)
                    {
                        num469 += 0.55f;
                    }
                    if (num474 > 700f)
                    {
                        num469 += 0.6f;
                    }
                    if (num474 > 800f)
                    {
                        num469 += 0.6f;
                    }
                }
                if (Main.getGoodWorld)
                {
                    num469 *= 1.15f;
                    num470 *= 1.15f;
                }
                num474 = num469 / num474;
                num472 *= num474;
                num473 *= num474;
                if (npc.velocity.X < num472)
                {
                    npc.velocity.X += num470;
                    if (npc.velocity.X < 0f && num472 > 0f)
                    {
                        npc.velocity.X += num470;
                    }
                }
                else if (npc.velocity.X > num472)
                {
                    npc.velocity.X -= num470;
                    if (npc.velocity.X > 0f && num472 < 0f)
                    {
                        npc.velocity.X -= num470;
                    }
                }
                if (npc.velocity.Y < num473)
                {
                    npc.velocity.Y += num470;
                    if (npc.velocity.Y < 0f && num473 > 0f)
                    {
                        npc.velocity.Y += num470;
                    }
                }
                else if (npc.velocity.Y > num473)
                {
                    npc.velocity.Y -= num470;
                    if (npc.velocity.Y > 0f && num473 < 0f)
                    {
                        npc.velocity.Y -= num470;
                    }
                }
            }
            int num475 = 400;
            if (NPC.IsMechQueenUp)
            {
                num475 = 1200;
            }
            npc.ai[2] += 1f;
            if (npc.ai[2] >= num475)
            {
                npc.ai[1] = 1f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.target = 255;
                npc.netUpdate = true;
            }
            if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
            {
                npc.localAI[2] += 1f;
                if (npc.localAI[2] > 22f)
                {
                    npc.localAI[2] = 0f;
                    SoundEngine.PlaySound(SoundID.Item34, npc.position);
                }
                if (Main.netMode != 1)
                {
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
                    if (npc.localAI[1] > 8f)
                    {
                        npc.localAI[1] = 0f;
                        float num476 = 6f;
                        int attackDamage_ForProjectiles7 = npc.GetAttackDamage_ForProjectiles(30f, 27f);
                        int projType = 101;
                        vector51 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                        num472 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector51.X;
                        num473 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector51.Y;
                        num474 = (float)Math.Sqrt(num472 * num472 + num473 * num473);
                        num474 = num476 / num474;
                        num472 *= num474;
                        num473 *= num474;
                        num473 += Main.rand.Next(-40, 41) * 0.01f;
                        num472 += Main.rand.Next(-40, 41) * 0.01f;
                        num473 += npc.velocity.Y * 0.5f;
                        num472 += npc.velocity.X * 0.5f;
                        vector51.X -= num472 * 1f;
                        vector51.Y -= num473 * 1f;
                        if (NPC.IsMechQueenUp)
                        {
                            Vector2 vector52 = (npc.rotation + (float)Math.PI / 2f).ToRotationVector2() * num476 + npc.velocity * 0.5f;
                            num472 = vector52.X;
                            num473 = vector52.Y;
                            vector51 = npc.Center - vector52 * 3f;
                        }

                        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector51.X, vector51.Y, num472, num473, projType, attackDamage_ForProjectiles7, 0f, Main.myPlayer);

                        var velocity = new Vector2(num472, num473);
                        npc.NewProjectile(vector51, velocity.RotatedByDegress(15), projType, attackDamage_ForProjectiles7);
                        npc.NewProjectile(vector51, velocity.RotatedByDegress(-15), projType, attackDamage_ForProjectiles7);
                    }
                }
            }
            if (NPC.IsMechQueenUp)
            {
                num469 = 14f;
                _ = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector51.X;
                _ = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - 300f - vector51.Y;
                num472 = vector47.X;
                num473 = vector47.Y;
                num472 -= vector51.X;
                num473 -= vector51.Y;
                num474 = (float)Math.Sqrt(num472 * num472 + num473 * num473);
                if (num474 > num469)
                {
                    num474 = num469 / num474;
                    num472 *= num474;
                    num473 *= num474;
                }
                int num479 = 60;
                npc.velocity.X = (npc.velocity.X * (num479 - 1) + num472) / num479;
                npc.velocity.Y = (npc.velocity.Y * (num479 - 1) + num473) / num479;
            }
        }
        else if (npc.ai[1] == 1f)
        {
            SoundEngine.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
            npc.rotation = num447;
            float num480 = 14f;
            if (Main.expertMode)
            {
                num480 += 2.5f;
            }
            Vector2 vector53 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num481 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector53.X;
            float num482 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector53.Y;
            float num483 = (float)Math.Sqrt(num481 * num481 + num482 * num482);
            num483 = num480 / num483;
            npc.velocity.X = num481 * num483;
            npc.velocity.Y = num482 * num483;
            npc.ai[1] = 2f;
        }
        else
        {
            if (npc.ai[1] != 2f)
            {
                return;
            }
            npc.ai[2] += 1f;
            if (Main.expertMode)
            {
                npc.ai[2] += 0.5f;
            }
            if (npc.ai[2] >= 50f)
            {
                npc.velocity.X *= 0.93f;
                npc.velocity.Y *= 0.93f;
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
            if (npc.ai[2] >= 80f)
            {
                npc.ai[3] += 1f;
                npc.ai[2] = 0f;
                npc.target = 255;
                npc.rotation = num447;
                if (npc.ai[3] >= 6f)
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
    }
}
