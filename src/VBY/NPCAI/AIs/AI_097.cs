namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_097(this NPC npc)
    {
        float num1467 = 7f;
        int num1468 = 480;
        int num1469 = 30;
        int maxValue6 = 6;
        if (npc.localAI[2] < 180f)
        {
            npc.localAI[2]++;
            if (Main.netMode != 1 && npc.localAI[2] % 60f == 0f)
            {
                Vector2 vector289 = Vector2.Zero;
                while (Math.Abs(vector289.X) < 1.5f)
                {
                    vector289 = Vector2.UnitY.RotatedByRandom(1.5707963705062866) * new Vector2(4f, 2.5f);
                }
                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X, npc.Center.Y, vector289.X, vector289.Y, 574, 0, 0f, Main.myPlayer, 0f, npc.whoAmI);
            }
        }
        if (npc.localAI[1] == 1f)
        {
            npc.localAI[1] = 0f;
            if (Main.rand.Next(maxValue6) == 0)
            {
                npc.ai[0] = num1468;
            }
        }
        npc.TargetClosest();
        if (Main.netMode != 1 && (!Main.player[npc.target].active || Main.player[npc.target].dead))
        {
            npc.ai[0] = 0f;
            npc.ai[1] = 1f;
            npc.ai[2] = 0f;
            npc.ai[3] = 0f;
            npc.netUpdate = true;
        }
        npc.rotation = Math.Abs(npc.velocity.X) * npc.direction * 0.1f;
        npc.spriteDirection = -npc.direction;
        Vector2 vector290 = npc.Center + new Vector2(npc.direction * 20, 6f);
        Vector2 vector291 = Main.player[npc.target].Center - vector290;
        bool flag92 = Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1);
        bool flag93 = false;
        if (npc.ai[1] == 1f)
        {
            if (npc.localAI[3] == 0f)
            {
                npc.localAI[3] = 1f;
                npc.ai[3] = 3f;
                SoundEngine.PlaySound(SoundID.Item8, npc.Center);
                for (int num1470 = 0; num1470 < 20; num1470++)
                {
                    int num1471 = Dust.NewDust(npc.position, npc.width, npc.height, 242);
                    Dust dust = Main.dust[num1471];
                    dust.velocity *= 3f;
                    Main.dust[num1471].noGravity = true;
                    Main.dust[num1471].scale = 2.5f;
                }
            }
            npc.ai[3]--;
            if (npc.ai[3] <= 0f)
            {
                npc.active = false;
                npc.netUpdate = true;
            }
            return;
        }
        if (vector291.Length() > 400f || !flag92)
        {
            Vector2 vector292 = vector291;
            if (vector292.Length() > num1467)
            {
                vector292.Normalize();
                vector292 *= num1467;
            }
            int num1472 = 30;
            npc.velocity = (npc.velocity * (num1472 - 1) + vector292) / num1472;
        }
        else
        {
            npc.velocity *= 0.98f;
            flag93 = true;
        }
        if (npc.ai[2] != 0f && npc.ai[3] != 0f)
        {
            SoundEngine.PlaySound(SoundID.Item8, npc.Center);
            for (int num1473 = 0; num1473 < 20; num1473++)
            {
                int num1474 = Dust.NewDust(npc.position, npc.width, npc.height, 242);
                Dust dust = Main.dust[num1474];
                dust.velocity *= 3f;
                Main.dust[num1474].noGravity = true;
                Main.dust[num1474].scale = 2.5f;
            }
            npc.Center = new Vector2(npc.ai[2] * 16f, npc.ai[3] * 16f);
            npc.velocity = Vector2.Zero;
            npc.ai[2] = 0f;
            npc.ai[3] = 0f;
            SoundEngine.PlaySound(SoundID.Item8, npc.Center);
            for (int num1475 = 0; num1475 < 20; num1475++)
            {
                int num1476 = Dust.NewDust(npc.position, npc.width, npc.height, 242);
                Dust dust = Main.dust[num1476];
                dust.velocity *= 3f;
                Main.dust[num1476].noGravity = true;
                Main.dust[num1476].scale = 2.5f;
            }
        }
        npc.ai[0]++;
        if (npc.ai[0] >= num1468 && Main.netMode != 1)
        {
            npc.ai[0] = 0f;
            _ = npc.Center.ToTileCoordinates();
            Point point15 = Main.player[npc.target].Center.ToTileCoordinates();
            Vector2 chosenTile2 = Vector2.Zero;
            if (npc.AI_AttemptToFindTeleportSpot(ref chosenTile2, point15.X, point15.Y, 20, 12, 1, solidTileCheckCentered: true, teleportInAir: true))
            {
                npc.ai[1] = 20f;
                npc.ai[2] = chosenTile2.X;
                npc.ai[3] = chosenTile2.Y;
                bool flag94 = true;
                for (int num1477 = 0; num1477 < 1000; num1477++)
                {
                    Projectile projectile10 = Main.projectile[num1477];
                    if (projectile10.active && projectile10.type == 574 && projectile10.ai[1] == npc.whoAmI && !(projectile10.ai[0] >= 0f))
                    {
                        flag94 = false;
                        break;
                    }
                }
                if (flag94)
                {
                    for (int num1478 = 0; num1478 < 1000; num1478++)
                    {
                        Projectile projectile11 = Main.projectile[num1478];
                        if (projectile11.active && projectile11.type == 574 && projectile11.ai[1] == npc.whoAmI)
                        {
                            projectile11.ai[0] -= num1469;
                        }
                    }
                }
            }
            npc.netUpdate = true;
        }
        if (flag93 && npc.velocity.Length() < 2f && Main.netMode != 1)
        {
            npc.localAI[0] += 1f;
            _ = npc.localAI[0];
            _ = 13f;
        }
    }
}
