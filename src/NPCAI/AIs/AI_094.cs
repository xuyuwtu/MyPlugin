using Terraria.WorldBuilding;

namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_094(this NPC npc)
    {
        if (npc.ai[2] == 1f)
        {
            npc.velocity = Vector2.UnitY * npc.velocity.Length();
            if (npc.velocity.Y < 0.25f)
            {
                npc.velocity.Y += 0.02f;
            }
            if (npc.velocity.Y > 0.25f)
            {
                npc.velocity.Y -= 0.02f;
            }
            npc.dontTakeDamage = true;
            npc.ai[1]++;
            if (npc.ai[1] > 120f)
            {
                npc.Opacity = 1f - (npc.ai[1] - 120f) / 60f;
            }
            int num1435 = 6;
            switch (npc.type)
            {
                case 517:
                    num1435 = 127;
                    break;
                case 422:
                    num1435 = 229;
                    break;
                case 507:
                    num1435 = 242;
                    break;
                case 493:
                    num1435 = 135;
                    break;
            }
            if (Main.rand.Next(5) == 0 && npc.ai[1] < 120f)
            {
                for (int num1436 = 0; num1436 < 3; num1436++)
                {
                    Dust dust9 = Main.dust[Dust.NewDust(npc.Left, npc.width, npc.height / 2, num1435)];
                    dust9.position = npc.Center + Vector2.UnitY.RotatedByRandom(4.188790321350098) * new Vector2(npc.width * 1.5f, npc.height * 1.1f) * 0.8f * (0.8f + Main.rand.NextFloat() * 0.2f);
                    dust9.velocity.X = 0f;
                    dust9.velocity.Y = (0f - Math.Abs(dust9.velocity.Y - num1436 + npc.velocity.Y - 4f)) * 3f;
                    dust9.noGravity = true;
                    dust9.fadeIn = 1f;
                    dust9.scale = 1f + Main.rand.NextFloat() + num1436 * 0.3f;
                }
            }
            if (npc.ai[1] < 150f)
            {
                for (int num1437 = 0; num1437 < 3; num1437++)
                {
                    if (Main.rand.Next(4) == 0)
                    {
                        Dust dust10 = Main.dust[Dust.NewDust(npc.Top + new Vector2(-npc.width * (0.33f - 0.11f * num1437), -20f), (int)(npc.width * (0.66f - 0.22f * num1437)), 20, num1435)];
                        dust10.velocity.X = 0f;
                        dust10.velocity.Y = (0f - Math.Abs(dust10.velocity.Y - num1437 + npc.velocity.Y - 4f)) * (1f + npc.ai[1] / 180f * 0.5f);
                        dust10.noGravity = true;
                        dust10.fadeIn = 1f;
                        dust10.scale = 1f + Main.rand.NextFloat() + num1437 * 0.3f;
                    }
                }
            }
            if (Main.rand.Next(5) == 0 && npc.ai[1] < 150f)
            {
                for (int num1438 = 0; num1438 < 3; num1438++)
                {
                    Vector2 vector283 = npc.Center + Vector2.UnitY.RotatedByRandom(4.188790321350098) * new Vector2(npc.width, npc.height) * 0.7f * Main.rand.NextFloat();
                    float num1439 = 1f + Main.rand.NextFloat() * 2f + npc.ai[1] / 180f * 4f;
                    for (int num1440 = 0; num1440 < 6; num1440++)
                    {
                        Dust dust11 = Main.dust[Dust.NewDust(vector283, 4, 4, num1435)];
                        dust11.position = vector283;
                        dust11.velocity.X *= num1439;
                        dust11.velocity.Y = (0f - Math.Abs(dust11.velocity.Y)) * num1439;
                        dust11.noGravity = true;
                        dust11.fadeIn = 1f;
                        dust11.scale = 1.5f + Main.rand.NextFloat() + num1440 * 0.13f;
                    }
                    SoundEngine.PlaySound(3, vector283, Utils.SelectRandom<int>(Main.rand, 1, 18));
                }
            }
            if (Main.rand.Next(3) != 0 && npc.ai[1] < 150f)
            {
                Dust dust12 = Main.dust[Dust.NewDust(npc.Left, npc.width, npc.height / 2, 241)];
                dust12.position = npc.Center + Vector2.UnitY.RotatedByRandom(4.188790321350098) * new Vector2(npc.width / 2, npc.height / 2) * (0.8f + Main.rand.NextFloat() * 0.2f);
                dust12.velocity.X = 0f;
                dust12.velocity.Y = Math.Abs(dust12.velocity.Y) * 0.25f;
            }
            if (npc.ai[1] % 60f == 1f)
            {
                SoundEngine.PlaySound(4, npc.Center, 22);
            }
            if (npc.ai[1] >= 180f)
            {
                npc.life = 0;
                npc.HitEffect(0, 1337.0);
                npc.checkDead();
            }
            return;
        }
        if (npc.ai[3] > 0f)
        {
            bool flag88 = npc.dontTakeDamage;
            switch (npc.type)
            {
                case 517:
                    flag88 = NPC.ShieldStrengthTowerSolar != 0;
                    break;
                case 422:
                    flag88 = NPC.ShieldStrengthTowerVortex != 0;
                    break;
                case 507:
                    flag88 = NPC.ShieldStrengthTowerNebula != 0;
                    break;
                case 493:
                    flag88 = NPC.ShieldStrengthTowerStardust != 0;
                    break;
            }
            if (flag88 != npc.dontTakeDamage)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath58, npc.position);
            }
            else if (npc.ai[3] == 1f)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath3, npc.position);
            }
            npc.ai[3]++;
            if (npc.ai[3] > 120f)
            {
                npc.ai[3] = 0f;
            }
        }
        switch (npc.type)
        {
            case 517:
                npc.dontTakeDamage = NPC.ShieldStrengthTowerSolar != 0;
                break;
            case 422:
                npc.dontTakeDamage = NPC.ShieldStrengthTowerVortex != 0;
                break;
            case 507:
                npc.dontTakeDamage = NPC.ShieldStrengthTowerNebula != 0;
                break;
            case 493:
                npc.dontTakeDamage = NPC.ShieldStrengthTowerStardust != 0;
                break;
        }
        npc.TargetClosest(faceTarget: false);
        if (Main.player[npc.target].Distance(npc.Center) > 2000f)
        {
            npc.localAI[0]++;
        }
        if (npc.localAI[0] >= 60f && Main.netMode != 1)
        {
            npc.localAI[0] = 0f;
            npc.netUpdate = true;
            npc.life = (int)MathHelper.Clamp(npc.life + 200, 0f, npc.lifeMax);
        }
        else
        {
            npc.localAI[0] = 0f;
        }
        npc.velocity = new Vector2(0f, (float)Math.Sin((float)Math.PI * 2f * npc.ai[0] / 300f) * 0.5f);
        Point origin = npc.Bottom.ToTileCoordinates();
        int maxDistance = 10;
        int num1441 = 20;
        int num1442 = 30;
        if (WorldGen.InWorld(origin.X, origin.Y, 20) && Main.tile[origin.X, origin.Y] != null)
        {
            if (WorldUtils.Find(origin, Searches.Chain(new Searches.Down(maxDistance), new Conditions.IsSolid()), out var result))
            {
                float num1443 = 1f - Math.Abs(origin.Y - result.Y) / 10f;
                npc.position.Y -= 1.5f * num1443;
            }
            else if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(num1441), new Conditions.IsSolid()), out _))
            {
                float num1444 = 1f;
                if (WorldUtils.Find(origin, Searches.Chain(new Searches.Down(num1442), new Conditions.IsSolid()), out result))
                {
                    num1444 = Utils.GetLerpValue(num1441, num1442, Math.Abs(origin.Y - result.Y), clamped: true);
                }
                npc.position.Y += 1.5f * num1444;
            }
        }
        if (!Main.remixWorld && !Main.getGoodWorld && npc.Bottom.Y > Main.worldSurface * 16.0 - 100.0)
        {
            npc.position.Y = (float)Main.worldSurface * 16f - npc.height - 100f;
        }
        npc.ai[0]++;
        if (npc.ai[0] >= 300f)
        {
            npc.ai[0] = 0f;
            npc.netUpdate = true;
        }
        if (npc.type == 493)
        {
            if (Main.rand.Next(5) == 0)
            {
                Dust dust13 = Main.dust[Dust.NewDust(npc.Left, npc.width, npc.height / 2, 241)];
                dust13.position = npc.Center + Vector2.UnitY.RotatedByRandom(2.094395160675049) * new Vector2(npc.width / 2, npc.height / 2) * (0.8f + Main.rand.NextFloat() * 0.2f);
                dust13.velocity.X = 0f;
                dust13.velocity.Y = Math.Abs(dust13.velocity.Y) * 0.25f;
            }
            for (int num1445 = 0; num1445 < 3; num1445++)
            {
                if (Main.rand.Next(5) == 0)
                {
                    Dust dust14 = Main.dust[Dust.NewDust(npc.Top + new Vector2(-npc.width * (0.33f - 0.11f * num1445), -20f), (int)(npc.width * (0.66f - 0.22f * num1445)), 20, 135)];
                    dust14.velocity.X = 0f;
                    dust14.velocity.Y = (0f - Math.Abs(dust14.velocity.Y - num1445 + npc.velocity.Y - 4f)) * 1f;
                    dust14.noGravity = true;
                    dust14.fadeIn = 1f;
                    dust14.scale = 1f + Main.rand.NextFloat() + num1445 * 0.3f;
                }
            }
            if (npc.ai[1] > 0f)
            {
                npc.ai[1]--;
            }
            if (Main.netMode != 1 && npc.ai[1] <= 0f && Main.player[npc.target].active && !Main.player[npc.target].dead && npc.Distance(Main.player[npc.target].Center) < 1080f && Main.player[npc.target].position.Y - npc.position.Y < 400f)
            {
                npc.SpawnStardustMark_StardustTower();
            }
        }
        if (npc.type == 507)
        {
            if (Main.rand.Next(5) == 0)
            {
                Dust dust15 = Main.dust[Dust.NewDust(npc.Left, npc.width, npc.height / 2, 241)];
                dust15.position = npc.Center + Vector2.UnitY.RotatedByRandom(2.094395160675049) * new Vector2(npc.width / 2, npc.height / 2) * (0.8f + Main.rand.NextFloat() * 0.2f);
                dust15.velocity.X = 0f;
                dust15.velocity.Y = Math.Abs(dust15.velocity.Y) * 0.25f;
            }
            for (int num1446 = 0; num1446 < 3; num1446++)
            {
                if (Main.rand.Next(5) == 0)
                {
                    Dust dust16 = Main.dust[Dust.NewDust(npc.Top + new Vector2(-npc.width * (0.33f - 0.11f * num1446), -20f), (int)(npc.width * (0.66f - 0.22f * num1446)), 20, 242)];
                    dust16.velocity.X = 0f;
                    dust16.velocity.Y = (0f - Math.Abs(dust16.velocity.Y - num1446 + npc.velocity.Y - 4f)) * 1f;
                    dust16.noGravity = true;
                    dust16.fadeIn = 1f;
                    dust16.color = Color.Black;
                    dust16.scale = 1f + Main.rand.NextFloat() + num1446 * 0.3f;
                }
            }
        }
        if (npc.type == 422)
        {
            if (Main.rand.Next(5) == 0)
            {
                Dust dust17 = Main.dust[Dust.NewDust(npc.Left, npc.width, npc.height / 2, 241)];
                dust17.position = npc.Center + Vector2.UnitY.RotatedByRandom(2.094395160675049) * new Vector2(npc.width / 2, npc.height / 2) * (0.8f + Main.rand.NextFloat() * 0.2f);
                dust17.velocity.X = 0f;
                dust17.velocity.Y = Math.Abs(dust17.velocity.Y) * 0.25f;
            }
            for (int num1447 = 0; num1447 < 3; num1447++)
            {
                if (Main.rand.Next(5) == 0)
                {
                    Dust dust18 = Main.dust[Dust.NewDust(npc.Top + new Vector2(-npc.width * (0.33f - 0.11f * num1447), -20f), (int)(npc.width * (0.66f - 0.22f * num1447)), 20, 229)];
                    dust18.velocity.X = 0f;
                    dust18.velocity.Y = (0f - Math.Abs(dust18.velocity.Y - num1447 + npc.velocity.Y - 4f)) * 1f;
                    dust18.noGravity = true;
                    dust18.fadeIn = 1f;
                    dust18.color = Color.Black;
                    dust18.scale = 1f + Main.rand.NextFloat() + num1447 * 0.3f;
                }
            }
            if (npc.ai[1] > 0f)
            {
                npc.ai[1]--;
            }
            if (Main.netMode != 1 && npc.ai[1] <= 0f && Main.player[npc.target].active && !Main.player[npc.target].dead && npc.Distance(Main.player[npc.target].Center) < 3240f && !Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
            {
                npc.ai[1] = 60 + Main.rand.Next(120);
                Point point11 = Main.player[npc.target].Top.ToTileCoordinates();
                bool flag89 = NPC.CountNPCS(428) + NPC.CountNPCS(427) + NPC.CountNPCS(426) < 14;
                for (int num1448 = 0; num1448 < 10; num1448++)
                {
                    if (WorldGen.SolidTile(point11.X, point11.Y))
                    {
                        break;
                    }
                    if (point11.Y <= 10)
                    {
                        break;
                    }
                    point11.Y--;
                }
                if (flag89)
                {
                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), point11.X * 16 + 8, point11.Y * 16 + 24, 0f, 0f, 579, 0, 0f, Main.myPlayer);
                }
                else
                {
                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), point11.X * 16 + 8, point11.Y * 16 + 17, 0f, 0f, 578, 0, 1f, Main.myPlayer);
                }
            }
            if (Main.netMode != 1 && npc.ai[1] <= 0f && Main.player[npc.target].active && !Main.player[npc.target].dead && npc.Distance(Main.player[npc.target].Center) < 1080f && Main.player[npc.target].position.Y - npc.position.Y < 400f && NPC.CountNPCS(427) + NPC.CountNPCS(426) * 3 + NPC.CountNPCS(428) < 20)
            {
                npc.ai[1] = 420 + Main.rand.Next(360);
                Point point12 = npc.Center.ToTileCoordinates();
                Point point13 = Main.player[npc.target].Center.ToTileCoordinates();
                Vector2 vector284 = Main.player[npc.target].Center - npc.Center;
                int num1449 = 20;
                int num1450 = 3;
                int num1451 = 8;
                int num1452 = 2;
                int num1453 = 0;
                bool flag90 = false;
                if (vector284.Length() > 2000f)
                {
                    flag90 = true;
                }
                while (!flag90 && num1453 < 100)
                {
                    num1453++;
                    int num1454 = Main.rand.Next(point13.X - num1449, point13.X + num1449 + 1);
                    int num1455 = Main.rand.Next(point13.Y - num1449, point13.Y + num1449 + 1);
                    if ((num1455 < point13.Y - num1451 || num1455 > point13.Y + num1451 || num1454 < point13.X - num1451 || num1454 > point13.X + num1451) && (num1455 < point12.Y - num1450 || num1455 > point12.Y + num1450 || num1454 < point12.X - num1450 || num1454 > point12.X + num1450) && !Main.tile[num1454, num1455].nactive())
                    {
                        bool flag91 = true;
                        if (flag91 && Main.tile[num1454, num1455].lava())
                        {
                            flag91 = false;
                        }
                        if (flag91 && Collision.SolidTiles(num1454 - num1452, num1454 + num1452, num1455 - num1452, num1455 + num1452))
                        {
                            flag91 = false;
                        }
                        if (flag91 && !Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                        {
                            flag91 = false;
                        }
                        if (flag91)
                        {
                            Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), num1454 * 16 + 8, num1455 * 16 + 8, 0f, 0f, 579, 0, 0f, Main.myPlayer);
                            break;
                        }
                    }
                }
            }
        }
        if (npc.type != 517)
        {
            return;
        }
        if (Main.rand.Next(5) == 0)
        {
            Dust dust19 = Main.dust[Dust.NewDust(npc.Left, npc.width, npc.height / 2, 241)];
            dust19.position = npc.Center + Vector2.UnitY.RotatedByRandom(2.094395160675049) * new Vector2(npc.width / 2, npc.height / 2) * (0.8f + Main.rand.NextFloat() * 0.2f);
            dust19.velocity.X = 0f;
            dust19.velocity.Y = Math.Abs(dust19.velocity.Y) * 0.25f;
        }
        for (int num1456 = 0; num1456 < 3; num1456++)
        {
            if (Main.rand.Next(5) == 0)
            {
                Dust dust20 = Main.dust[Dust.NewDust(npc.Top + new Vector2(-npc.width * (0.33f - 0.11f * num1456), -20f), (int)(npc.width * (0.66f - 0.22f * num1456)), 20, 6)];
                dust20.velocity.X = 0f;
                dust20.velocity.Y = (0f - Math.Abs(dust20.velocity.Y - num1456 + npc.velocity.Y - 4f)) * 1f;
                dust20.noGravity = true;
                dust20.fadeIn = 1f;
                dust20.scale = 1f + Main.rand.NextFloat() + num1456 * 0.3f;
            }
        }
        if (npc.ai[1] > 0f)
        {
            npc.ai[1]--;
        }
        if (Main.netMode != 1 && npc.ai[1] <= 0f && Main.player[npc.target].active && !Main.player[npc.target].dead && npc.Distance(Main.player[npc.target].Center) < 1080f && Main.player[npc.target].position.Y - npc.position.Y < 700f)
        {
            Vector2 vector285 = npc.Top + new Vector2(-npc.width * 0.33f, -20f) + new Vector2(npc.width * 0.66f, 20f) * Utils.RandomVector2(Main.rand, 0f, 1f);
            Vector2 vector286 = -Vector2.UnitY.RotatedByRandom(0.7853981852531433) * (7f + Main.rand.NextFloat() * 5f);
            int num1457 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)vector285.X, (int)vector285.Y, 519, npc.whoAmI);
            Main.npc[num1457].velocity = vector286;
            Main.npc[num1457].netUpdate = true;
            npc.ai[1] = 60f;
        }
    }
}
