namespace VBY.NPCTest;

partial class NPCAIs
{
    public static void AI_051(this NPC npc)
    {
        bool flag39 = false;
        bool flag40 = false;
        npc.TargetClosest();
        if (Main.player[npc.target].dead)
        {
            flag40 = true;
            flag39 = true;
        }
        else if (npc.target >= 0 && npc.target < 255)
        {
            int num762 = 4800;
            if (npc.timeLeft < NPC.activeTime && Vector2.Distance(npc.Center, Main.player[npc.target].Center) < num762)
            {
                npc.timeLeft = NPC.activeTime;
            }
        }
        NPC.plantBoss = npc.whoAmI;
        if (npc.localAI[0] == 0f)
        {
            npc.localAI[0] = 1f;
            NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X, (int)npc.Center.Y, 263, npc.whoAmI);
            NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X, (int)npc.Center.Y, 263, npc.whoAmI);
            NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X, (int)npc.Center.Y, 263, npc.whoAmI);
        }
        int[] array2 = new int[3];
        float num764 = 0f;
        float num765 = 0f;
        int num766 = 0;
        for (int num767 = 0; num767 < 200; num767++)
        {
            if (Main.npc[num767].active && Main.npc[num767].aiStyle == 52)
            {
                num764 += Main.npc[num767].Center.X;
                num765 += Main.npc[num767].Center.Y;
                array2[num766] = num767;
                num766++;
                if (num766 > 2)
                {
                    break;
                }
            }
        }
        num764 /= num766;
        num765 /= num766;
        float num768 = 2.5f;
        float num769 = 0.025f;
        if (npc.life < npc.lifeMax / 2)
        {
            num768 = 5f;
            num769 = 0.05f;
        }
        if (npc.life < npc.lifeMax / 4)
        {
            num768 = 7f;
        }
        if (!Main.player[npc.target].ZoneJungle || Main.player[npc.target].position.Y < Main.worldSurface * 16.0 || Main.player[npc.target].position.Y > Main.UnderworldLayer * 16)
        {
            flag39 = true;
            num768 += 8f;
            num769 = 0.15f;
        }
        if (Main.expertMode)
        {
            num768 += 1f;
            num768 *= 1.1f;
            num769 += 0.01f;
            num769 *= 1.1f;
        }
        if (Main.getGoodWorld)
        {
            num768 *= 1.15f;
            num769 *= 1.15f;
        }
        Vector2 vector97 = new(num764, num765);
        float num770 = Main.player[npc.target].Center.X - vector97.X;
        float num771 = Main.player[npc.target].Center.Y - vector97.Y;
        if (flag40)
        {
            num771 *= -1f;
            num770 *= -1f;
            num768 += 8f;
        }
        float num772 = (float)Math.Sqrt(num770 * num770 + num771 * num771);
        int num773 = 500;
        if (flag39)
        {
            num773 += 350;
        }
        if (Main.expertMode)
        {
            num773 += 150;
        }
        if (num772 >= num773)
        {
            num772 = num773 / num772;
            num770 *= num772;
            num771 *= num772;
        }
        num764 += num770;
        num765 += num771;
        vector97 = new Vector2(npc.Center.X, npc.Center.Y);
        num770 = num764 - vector97.X;
        num771 = num765 - vector97.Y;
        num772 = (float)Math.Sqrt(num770 * num770 + num771 * num771);
        if (num772 < num768)
        {
            num770 = npc.velocity.X;
            num771 = npc.velocity.Y;
        }
        else
        {
            num772 = num768 / num772;
            num770 *= num772;
            num771 *= num772;
        }
        if (npc.velocity.X < num770)
        {
            npc.velocity.X += num769;
            if (npc.velocity.X < 0f && num770 > 0f)
            {
                npc.velocity.X += num769 * 2f;
            }
        }
        else if (npc.velocity.X > num770)
        {
            npc.velocity.X -= num769;
            if (npc.velocity.X > 0f && num770 < 0f)
            {
                npc.velocity.X -= num769 * 2f;
            }
        }
        if (npc.velocity.Y < num771)
        {
            npc.velocity.Y += num769;
            if (npc.velocity.Y < 0f && num771 > 0f)
            {
                npc.velocity.Y += num769 * 2f;
            }
        }
        else if (npc.velocity.Y > num771)
        {
            npc.velocity.Y -= num769;
            if (npc.velocity.Y > 0f && num771 < 0f)
            {
                npc.velocity.Y -= num769 * 2f;
            }
        }
        Vector2 vector98 = new(npc.Center.X, npc.Center.Y);
        float num774 = Main.player[npc.target].Center.X - vector98.X;
        float num775 = Main.player[npc.target].Center.Y - vector98.Y;
        npc.rotation = (float)Math.Atan2(num775, num774) + 1.57f;
        if (npc.life > npc.lifeMax / 2)
        {
            npc.defense = 36;
            int num776 = 50;
            if (flag39)
            {
                npc.defense *= 2;
                num776 *= 2;
            }
            npc.damage = npc.GetAttackDamage_ScaledByStrength(num776);
            if (Main.netMode == 1)
            {
                return;
            }
            npc.localAI[1] += 1f;
            if (npc.life < npc.lifeMax * 0.9)
            {
                npc.localAI[1] += 1f;
            }
            if (npc.life < npc.lifeMax * 0.8)
            {
                npc.localAI[1] += 1f;
            }
            if (npc.life < npc.lifeMax * 0.7)
            {
                npc.localAI[1] += 1f;
            }
            if (npc.life < npc.lifeMax * 0.6)
            {
                npc.localAI[1] += 1f;
            }
            if (flag39)
            {
                npc.localAI[1] += 3f;
            }
            if (Main.expertMode)
            {
                npc.localAI[1] += 1f;
            }
            if (Main.expertMode && npc.justHit && Main.rand.Next(2) == 0)
            {
                npc.localAI[3] = 1f;
            }
            if (Main.getGoodWorld)
            {
                npc.localAI[1] += 1f;
            }
            if (!(npc.localAI[1] > 80f))
            {
                return;
            }
            npc.localAI[1] = 0f;
            bool canHit = Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height);
            if (npc.localAI[3] > 0f)
            {
                canHit = true;
                npc.localAI[3] = 0f;
            }
            if (canHit)
            {
                Vector2 vector99 = new(npc.Center.X, npc.Center.Y);
                float num777 = 15f;
                if (Main.expertMode)
                {
                    num777 = 17f;
                }
                float num778 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector99.X;
                float num779 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector99.Y;
                float num780 = (float)Math.Sqrt(num778 * num778 + num779 * num779);
                num780 = num777 / num780;
                num778 *= num780;
                num779 *= num780;
                int projDamage = 22;
                int projType = 275;
                int maxValue2 = 4;
                int maxValue3 = 8;
                if (Main.expertMode)
                {
                    maxValue2 = 2;
                    maxValue3 = 6;
                }
                if (npc.life < npc.lifeMax * 0.8 && Main.rand.Next(maxValue2) == 0)
                {
                    projDamage = 27;
                    npc.localAI[1] = -30f;
                    projType = 276;
                }
                else if (npc.life < npc.lifeMax * 0.8 && Main.rand.Next(maxValue3) == 0)
                {
                    projDamage = 31;
                    npc.localAI[1] = -120f;
                    projType = 277;
                }
                if (flag39)
                {
                    projDamage *= 2;
                }
                projDamage = npc.GetAttackDamage_ForProjectiles(projDamage, projDamage * 0.9f);
                vector99.X += num778 * 3f;
                vector99.Y += num779 * 3f;
                int newProjIndex = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector99.X, vector99.Y, num778, num779, projType, projDamage, 0f, Main.myPlayer);
                if (projType != 277)
                {
                    Main.projectile[newProjIndex].timeLeft = 300;
                }

                var velocity = new Vector2(num778, num779);
                newProjIndex = npc.NewProjectile(vector99, velocity.RotatedByDegress(5), projType, projDamage);
                if (projType != 277)
                {
                    Main.projectile[newProjIndex].timeLeft = 300;
                }
                newProjIndex = npc.NewProjectile(vector99, velocity.RotatedByDegress(-5), projType, projDamage);
                if (projType != 277)
                {
                    Main.projectile[newProjIndex].timeLeft = 300;
                }
            }
            return;
        }
        npc.defense = 10;
        int num784 = 70;
        if (flag39)
        {
            npc.defense *= 4;
            num784 *= 2;
        }
        npc.damage = npc.GetAttackDamage_ScaledByStrength(num784);
        if (Main.netMode != 1)
        {
            if (npc.localAI[0] == 1f)
            {
                npc.localAI[0] = 2f;
                int spawnCount = 8;
                if (Main.getGoodWorld)
                {
                    spawnCount += 6;
                }
                for (int num786 = 0; num786 < spawnCount; num786++)
                {
                    NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X, (int)npc.Center.Y, 264, npc.whoAmI);
                }
                if (Main.expertMode)
                {
                    for (int num788 = 0; num788 < 200; num788++)
                    {
                        if (Main.npc[num788].active && Main.npc[num788].aiStyle == 52)
                        {
                            for (int num789 = 0; num789 < spawnCount / 2 - 1; num789++)
                            {
                                int num790 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X, (int)npc.Center.Y, 264, npc.whoAmI);
                                Main.npc[num790].ai[3] = num788 + 1;
                            }
                        }
                    }
                }
            }
            else if (Main.expertMode && Main.rand.Next(60) == 0)
            {
                int num791 = 0;
                for (int num792 = 0; num792 < 200; num792++)
                {
                    if (Main.npc[num792].active && Main.npc[num792].type == 264 && Main.npc[num792].ai[3] == 0f)
                    {
                        num791++;
                    }
                }
                if (num791 < 8 && Main.rand.Next((num791 + 1) * 10) <= 1)
                {
                    NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X, (int)npc.Center.Y, 264, npc.whoAmI);
                }
            }
        }
        if (npc.localAI[2] == 0f)
        {
            Gore.NewGore(new Vector2(npc.position.X + Main.rand.Next(npc.width), npc.position.Y + Main.rand.Next(npc.height)), npc.velocity, 378, npc.scale);
            Gore.NewGore(new Vector2(npc.position.X + Main.rand.Next(npc.width), npc.position.Y + Main.rand.Next(npc.height)), npc.velocity, 379, npc.scale);
            Gore.NewGore(new Vector2(npc.position.X + Main.rand.Next(npc.width), npc.position.Y + Main.rand.Next(npc.height)), npc.velocity, 380, npc.scale);
            npc.localAI[2] = 1f;
        }
        if (Main.netMode == 1)
        {
            return;
        }
        npc.localAI[1] += 1f;
        if (npc.life < npc.lifeMax * 0.4)
        {
            npc.localAI[1] += 1f;
        }
        if (npc.life < npc.lifeMax * 0.3)
        {
            npc.localAI[1] += 1f;
        }
        if (npc.life < npc.lifeMax * 0.2)
        {
            npc.localAI[1] += 1f;
        }
        if (npc.life < npc.lifeMax * 0.1)
        {
            npc.localAI[1] += 1f;
        }
        if (npc.localAI[1] >= 350f)
        {
            float num794 = 8f;
            Vector2 vector100 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num795 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector100.X + Main.rand.Next(-10, 11);
            float num796 = Math.Abs(num795 * 0.2f);
            float num797 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector100.Y + Main.rand.Next(-10, 11);
            if (num797 > 0f)
            {
                num796 = 0f;
            }
            num797 -= num796;
            float num798 = (float)Math.Sqrt(num795 * num795 + num797 * num797);
            num798 = num794 / num798;
            num795 *= num798;
            num797 *= num798;
            int num799 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X, (int)npc.Center.Y, 265);
            Main.npc[num799].velocity.X = num795;
            Main.npc[num799].velocity.Y = num797;
            Main.npc[num799].netUpdate = true;
            npc.localAI[1] = 0f;
        }
    }
}
