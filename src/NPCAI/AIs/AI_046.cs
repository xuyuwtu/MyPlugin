namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_046(this NPC npc)
    {
        float num695 = npc.GetMyBalance();
        if (Main.getGoodWorld)
        {
            num695 += 3f;
        }
        if ((!Main.player[npc.target].ZoneLihzhardTemple && !Main.player[npc.target].ZoneJungle) || Main.player[npc.target].Center.Y < Main.worldSurface * 16.0)
        {
            num695 *= 2f;
        }
        npc.noTileCollide = true;
        if (NPC.golemBoss < 0)
        {
            npc.StrikeNPCNoInteraction(9999, 0f, 0);
            return;
        }
        float num696 = 100f;
        Vector2 vector89 = new(npc.Center.X, npc.Center.Y);
        float num697 = Main.npc[NPC.golemBoss].Center.X - vector89.X;
        float num698 = Main.npc[NPC.golemBoss].Center.Y - vector89.Y;
        num698 -= 57f * npc.scale;
        num697 -= 3f * npc.scale;
        float num699 = (float)Math.Sqrt(num697 * num697 + num698 * num698);
        if (num699 < num696)
        {
            npc.rotation = 0f;
            npc.velocity.X = num697;
            npc.velocity.Y = num698;
        }
        else
        {
            num699 = num696 / num699;
            npc.velocity.X = num697 * num699;
            npc.velocity.Y = num698 * num699;
            npc.rotation = npc.velocity.X * 0.1f;
        }
        if (npc.alpha > 0)
        {
            npc.alpha -= 10;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }
            npc.ai[1] = 30f;
        }
        if (npc.ai[0] == 0f)
        {
            npc.ai[1] += 1f;
            int num700 = 300;
            if (npc.ai[1] < 20f || npc.ai[1] > num700 - 20)
            {
                npc.ai[1] += 2f * (num695 - 1f) / 3f;
                npc.localAI[0] = 1f;
            }
            else
            {
                npc.ai[1] += 1f * (num695 - 1f) / 2f;
                npc.localAI[0] = 0f;
            }
            if (npc.ai[1] >= num700)
            {
                npc.TargetClosest();
                npc.ai[1] = 0f;
                Vector2 vector90 = new(npc.Center.X, npc.Center.Y + 10f * npc.scale);
                float num701 = 8f;
                float num702 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector90.X;
                float num703 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector90.Y;
                float num704 = (float)Math.Sqrt(num702 * num702 + num703 * num703);
                num704 = num701 / num704;
                num702 *= num704;
                num703 *= num704;
                int num705 = 18;
                int num706 = 258;
                if (Main.netMode != 1)
                {
                    _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector90.X, vector90.Y, num702, num703, num706, num705, 0f, Main.myPlayer);
                }
            }
        }
        else if (npc.ai[0] == 1f)
        {
            npc.TargetClosest();
            Vector2 vector91 = new(npc.Center.X, npc.Center.Y + 10f * npc.scale);
            if (Main.player[npc.target].Center.X < npc.Center.X - npc.width)
            {
                npc.localAI[1] = -1f;
                vector91.X -= 40f * npc.scale;
            }
            else if (Main.player[npc.target].Center.X > npc.Center.X + npc.width)
            {
                npc.localAI[1] = 1f;
                vector91.X += 40f * npc.scale;
            }
            else
            {
                npc.localAI[1] = 0f;
            }
            float num708 = (num695 + 3f) / 4f;
            npc.ai[1] += num708;
            if (npc.life < npc.lifeMax * 0.4)
            {
                npc.ai[1] += num708;
            }
            if (npc.life < npc.lifeMax * 0.2)
            {
                npc.ai[1] += num708;
            }
            int num709 = 300;
            if (npc.ai[1] < 20f || npc.ai[1] > num709 - 20)
            {
                npc.localAI[0] = 1f;
            }
            else
            {
                npc.localAI[0] = 0f;
            }
            if (npc.ai[1] >= num709)
            {
                npc.TargetClosest();
                npc.ai[1] = 0f;
                float num710 = 8f;
                float num711 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector91.X;
                float num712 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector91.Y;
                float num713 = (float)Math.Sqrt(num711 * num711 + num712 * num712);
                num713 = num710 / num713;
                num711 *= num713;
                num712 *= num713;
                int num714 = 24;
                int num715 = 258;
                if (Main.netMode != 1)
                {
                    _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector91.X, vector91.Y, num711, num712, num715, num714, 0f, Main.myPlayer);
                }
            }
            npc.ai[2] += num708;
            if (npc.life < npc.lifeMax / 3)
            {
                npc.ai[2] += num708;
            }
            if (npc.life < npc.lifeMax / 4)
            {
                npc.ai[2] += num708;
            }
            if (npc.life < npc.lifeMax / 5)
            {
                npc.ai[2] += num708;
            }
            if (!Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
            {
                npc.ai[2] += 4f;
            }
            if (npc.ai[2] > 60 + Main.rand.Next(600))
            {
                npc.ai[2] = 0f;
                int num717 = 28;
                int num718 = 259;
                if (npc.localAI[1] == 0f)
                {
                    for (int num719 = 0; num719 < 2; num719++)
                    {
                        vector91 = new Vector2(npc.Center.X, npc.Center.Y - 22f * npc.scale);
                        if (num719 == 0)
                        {
                            vector91.X -= 18f * npc.scale;
                        }
                        else
                        {
                            vector91.X += 18f * npc.scale;
                        }
                        float num720 = 11f;
                        float num721 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector91.X;
                        float num722 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector91.Y;
                        float num723 = (float)Math.Sqrt(num721 * num721 + num722 * num722);
                        num723 = num720 / num723;
                        num721 *= num723;
                        num722 *= num723;
                        vector91.X += num721 * 3f;
                        vector91.Y += num722 * 3f;
                        if (Main.netMode != 1)
                        {
                            int num724 = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector91.X, vector91.Y, num721, num722, num718, num717, 0f, Main.myPlayer);
                            Main.projectile[num724].timeLeft = 300;
                        }
                    }
                }
                else if (npc.localAI[1] != 0f)
                {
                    vector91 = new Vector2(npc.Center.X, npc.Center.Y - 22f * npc.scale);
                    if (npc.localAI[1] == -1f)
                    {
                        vector91.X -= 30f * npc.scale;
                    }
                    else if (npc.localAI[1] == 1f)
                    {
                        vector91.X += 30f * npc.scale;
                    }
                    float num725 = 12f;
                    float num726 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector91.X;
                    float num727 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector91.Y;
                    float num728 = (float)Math.Sqrt(num726 * num726 + num727 * num727);
                    num728 = num725 / num728;
                    num726 *= num728;
                    num727 *= num728;
                    vector91.X += num726 * 3f;
                    vector91.Y += num727 * 3f;
                    if (Main.netMode != 1)
                    {
                        int num729 = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector91.X, vector91.Y, num726, num727, num718, num717, 0f, Main.myPlayer);
                        Main.projectile[num729].timeLeft = 300;
                    }
                }
            }
        }
        if (npc.life < npc.lifeMax / 2)
        {
            npc.ai[0] = 1f;
        }
        else
        {
            npc.ai[0] = 0f;
        }
    }
}
