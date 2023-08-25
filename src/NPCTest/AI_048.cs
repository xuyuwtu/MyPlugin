namespace VBY.NPCTest;

partial class NPCAIs
{
    public static void AI_048(this NPC npc)
    {
        bool flag37 = false;
        float num730 = npc.GetMyBalance();
        if (Main.getGoodWorld)
        {
            num730 += 3f;
        }
        if ((!Main.player[npc.target].ZoneLihzhardTemple && !Main.player[npc.target].ZoneJungle) || Main.player[npc.target].Center.Y < Main.worldSurface * 16.0)
        {
            num730 *= 2f;
        }
        if (!Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
        {
            npc.noTileCollide = true;
            flag37 = true;
        }
        else if (npc.noTileCollide && Collision.SolidTiles(npc.position, npc.width, npc.height))
        {
            npc.noTileCollide = false;
        }
        if (NPC.golemBoss < 0)
        {
            npc.StrikeNPCNoInteraction(9999, 0f, 0);
            return;
        }
        npc.TargetClosest();
        float num731 = 7f;
        float num732 = 0.05f;
        Vector2 vector92 = new(npc.Center.X, npc.Center.Y);
        float num733 = Main.player[npc.target].Center.X - vector92.X;
        float num734 = Main.player[npc.target].Center.Y - vector92.Y - 300f;
        float num735 = (float)Math.Sqrt(num733 * num733 + num734 * num734);
        num735 = num731 / num735;
        num733 *= num735;
        num734 *= num735;
        if (npc.velocity.X < num733)
        {
            npc.velocity.X += num732;
            if (npc.velocity.X < 0f && num733 > 0f)
            {
                npc.velocity.X += num732;
            }
        }
        else if (npc.velocity.X > num733)
        {
            npc.velocity.X -= num732;
            if (npc.velocity.X > 0f && num733 < 0f)
            {
                npc.velocity.X -= num732;
            }
        }
        if (npc.velocity.Y < num734)
        {
            npc.velocity.Y += num732;
            if (npc.velocity.Y < 0f && num734 > 0f)
            {
                npc.velocity.Y += num732;
            }
        }
        else if (npc.velocity.Y > num734)
        {
            npc.velocity.Y -= num732;
            if (npc.velocity.Y > 0f && num734 < 0f)
            {
                npc.velocity.Y -= num732;
            }
        }
        float num736 = (num730 + 4f) / 5f;
        npc.ai[1] += num736;
        if (Main.npc[NPC.golemBoss].life < Main.npc[NPC.golemBoss].lifeMax * 0.8)
        {
            npc.ai[1] += num736;
        }
        if (Main.npc[NPC.golemBoss].life < Main.npc[NPC.golemBoss].lifeMax * 0.6)
        {
            npc.ai[1] += num736;
        }
        if (Main.npc[NPC.golemBoss].life < Main.npc[NPC.golemBoss].lifeMax * 0.2)
        {
            npc.ai[1] += num736;
        }
        if (Main.npc[NPC.golemBoss].life < Main.npc[NPC.golemBoss].lifeMax * 0.1)
        {
            npc.ai[1] += num736;
        }
        int num737 = 300;
        if (npc.ai[1] < 20f || npc.ai[1] > num737 - 20)
        {
            npc.localAI[0] = 1f;
        }
        else
        {
            npc.localAI[0] = 0f;
        }
        if (flag37)
        {
            npc.ai[1] = 20f;
        }
        if (npc.ai[1] >= num737)
        {
            npc.TargetClosest();
            npc.ai[1] = 0f;
            Vector2 vector93 = new(npc.Center.X, npc.Center.Y - 10f * npc.scale);
            float num738 = 8f;
            int num739 = 20;
            int projType = 258;
            float num741 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector93.X;
            float num742 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector93.Y;
            float num743 = (float)Math.Sqrt(num741 * num741 + num742 * num742);
            num743 = num738 / num743;
            num741 *= num743;
            num742 *= num743;

            Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector93.X, vector93.Y, num741, num742, projType, num739, 0f, Main.myPlayer);
        }
        float num745 = num730;
        npc.ai[2] += num745;
        if (Main.npc[NPC.golemBoss].life < Main.npc[NPC.golemBoss].lifeMax / 1.25)
        {
            npc.ai[2] += num745;
        }
        if (Main.npc[NPC.golemBoss].life < Main.npc[NPC.golemBoss].lifeMax / 1.5)
        {
            npc.ai[2] += num745;
        }
        if (Main.npc[NPC.golemBoss].life < Main.npc[NPC.golemBoss].lifeMax / 2)
        {
            npc.ai[2] += num745;
        }
        if (Main.npc[NPC.golemBoss].life < Main.npc[NPC.golemBoss].lifeMax / 3)
        {
            npc.ai[2] += num745;
        }
        if (Main.npc[NPC.golemBoss].life < Main.npc[NPC.golemBoss].lifeMax / 4)
        {
            npc.ai[2] += num745;
        }
        if (Main.npc[NPC.golemBoss].life < Main.npc[NPC.golemBoss].lifeMax / 5)
        {
            npc.ai[2] += num745;
        }
        if (Main.npc[NPC.golemBoss].life < Main.npc[NPC.golemBoss].lifeMax / 6)
        {
            npc.ai[2] += num745;
        }
        bool flag38 = false;
        if (!Collision.CanHit(Main.npc[NPC.golemBoss].Center, 1, 1, Main.player[npc.target].Center, 1, 1))
        {
            flag38 = true;
        }
        if (flag38)
        {
            npc.ai[2] += num745 * 10f;
        }
        if (npc.ai[2] > 100 + Main.rand.Next(4800))
        {
            npc.ai[2] = 0f;
            for (int num746 = 0; num746 < 2; num746++)
            {
                Vector2 vector94 = new(npc.Center.X, npc.Center.Y - 50f * npc.scale);
                switch (num746)
                {
                    case 0:
                        vector94.X -= 14f * npc.scale;
                        break;
                    case 1:
                        vector94.X += 14f * npc.scale;
                        break;
                }
                float num747 = 11f;
                int num748 = 24;
                int projType = 259;
                if (Main.npc[NPC.golemBoss].life < Main.npc[NPC.golemBoss].lifeMax * 0.5)
                {
                    num748++;
                    num747 += 0.25f;
                }
                if (Main.npc[NPC.golemBoss].life < Main.npc[NPC.golemBoss].lifeMax * 0.4)
                {
                    num748++;
                    num747 += 0.25f;
                }
                if (Main.npc[NPC.golemBoss].life < Main.npc[NPC.golemBoss].lifeMax * 0.3)
                {
                    num748++;
                    num747 += 0.25f;
                }
                if (Main.npc[NPC.golemBoss].life < Main.npc[NPC.golemBoss].lifeMax * 0.2)
                {
                    num748++;
                    num747 += 0.25f;
                }
                if (Main.npc[NPC.golemBoss].life < Main.npc[NPC.golemBoss].lifeMax * 0.1)
                {
                    num748++;
                    num747 += 0.25f;
                }
                float num750 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f;
                float num751 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f;
                if (flag38)
                {
                    num748 = (int)(num748 * 1.5);
                    num747 *= 2.5f;
                    num750 += Main.player[npc.target].velocity.X * Main.rand.NextFloat() * 50f;
                    num751 += Main.player[npc.target].velocity.Y * Main.rand.NextFloat() * 50f;
                }
                num750 -= vector94.X;
                num751 -= vector94.Y;
                float num752 = (float)Math.Sqrt(num750 * num750 + num751 * num751);
                num752 = num747 / num752;
                num750 *= num752;
                num751 *= num752;
                vector94.X += num750 * 3f;
                vector94.Y += num751 * 3f;

                if(Main.rand.Next(4)  == 0)
                {
                    projType = 299;
                    num750 *= 0.8f;
                    num751 *= 0.8f;
                }

                Main.projectile[Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector94.X, vector94.Y, num750, num751, projType, num748, 0f, Main.myPlayer)].timeLeft = 300;
            }
        }
        if (!Main.getGoodWorld)
        {
            npc.position += npc.netOffset;
            int num754 = Main.rand.Next(2) * 2 - 1;
            Vector2 vector95 = npc.Bottom + new Vector2(num754 * 22 * npc.scale, -22f * npc.scale);
            Dust dust5 = Dust.NewDustPerfect(vector95, 228, ((float)Math.PI / 2f + -(float)Math.PI / 2f * num754 + Main.rand.NextFloatDirection() * ((float)Math.PI / 4f)).ToRotationVector2() * (2f + Main.rand.NextFloat()));
            Dust dust = dust5;
            dust.velocity += npc.velocity;
            dust5.noGravity = true;
            dust5 = Dust.NewDustPerfect(npc.Bottom + new Vector2(Main.rand.NextFloatDirection() * 6f * npc.scale, (Main.rand.NextFloat() * -4f - 8f) * npc.scale), 228, Vector2.UnitY * (2f + Main.rand.NextFloat()));
            dust5.fadeIn = 0f;
            dust5.scale = 0.7f + Main.rand.NextFloat() * 0.5f;
            dust5.noGravity = true;
            dust = dust5;
            dust.velocity += npc.velocity;
            npc.position -= npc.netOffset;
        }
    }
}
