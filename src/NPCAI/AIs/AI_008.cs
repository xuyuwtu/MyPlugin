namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_008(this NPC npc)
    {
        npc.TargetClosest();
        npc.velocity.X *= 0.93f;
        if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
        {
            npc.velocity.X = 0f;
        }
        if (npc.ai[0] == 0f)
        {
            npc.ai[0] = 500f;
        }
        if (npc.type == 172)
        {
            if (npc.alpha < 255)
            {
                npc.alpha++;
            }
            if (npc.justHit)
            {
                npc.alpha = 0;
            }
        }
        if (npc.ai[2] != 0f && npc.ai[3] != 0f)
        {
            npc.position += npc.netOffset;
            if (npc.type == 172)
            {
                npc.alpha = 255;
            }
            SoundEngine.PlaySound(SoundID.Item8, npc.position);
            for (int num70 = 0; num70 < 50; num70++)
            {
                if (npc.type == 29 || npc.type == 45)
                {
                    int num71 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 27, 0f, 0f, 100, default, Main.rand.Next(1, 3));
                    Dust dust = Main.dust[num71];
                    dust.velocity *= 3f;
                    if (Main.dust[num71].scale > 1f)
                    {
                        Main.dust[num71].noGravity = true;
                    }
                }
                else if (npc.type == 32)
                {
                    int num72 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 172, 0f, 0f, 100, default, 1.5f);
                    Dust dust = Main.dust[num72];
                    dust.velocity *= 3f;
                    Main.dust[num72].noGravity = true;
                }
                else if (npc.type == 283 || npc.type == 284)
                {
                    int num73 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 173);
                    Dust dust = Main.dust[num73];
                    dust.velocity *= 2f;
                    Main.dust[num73].scale = 1.4f;
                }
                else if (npc.type == 285 || npc.type == 286)
                {
                    int num74 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 174, 0f, 0f, 100, default, 1.5f);
                    Dust dust = Main.dust[num74];
                    dust.velocity *= 3f;
                    Main.dust[num74].noGravity = true;
                }
                else if (npc.type == 281 || npc.type == 282)
                {
                    int num75 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 175, 0f, 0f, 100, default, 1.5f);
                    Dust dust = Main.dust[num75];
                    dust.velocity *= 3f;
                    Main.dust[num75].noGravity = true;
                }
                else if (npc.type == 172)
                {
                    int num76 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 106, 0f, 0f, 100, default, 2.5f);
                    Dust dust = Main.dust[num76];
                    dust.velocity *= 3f;
                    Main.dust[num76].noGravity = true;
                }
                else if (npc.type == 533)
                {
                    int num77 = Dust.NewDust(npc.position, npc.width, npc.height, 27, 0f, 0f, 100, default, 2.5f);
                    Dust dust = Main.dust[num77];
                    dust.velocity *= 3f;
                    Main.dust[num77].noGravity = true;
                }
                else
                {
                    int num78 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default, 2.5f);
                    Dust dust = Main.dust[num78];
                    dust.velocity *= 3f;
                    Main.dust[num78].noGravity = true;
                }
            }
            npc.position -= npc.netOffset;
            npc.position.X = npc.ai[2] * 16f - npc.width / 2 + 8f;
            npc.position.Y = npc.ai[3] * 16f - npc.height;
            npc.netOffset *= 0f;
            npc.velocity.X = 0f;
            npc.velocity.Y = 0f;
            npc.ai[2] = 0f;
            npc.ai[3] = 0f;
            SoundEngine.PlaySound(SoundID.Item8, npc.position);
            for (int num79 = 0; num79 < 50; num79++)
            {
                if (npc.type == 29 || npc.type == 45)
                {
                    int num80 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 27, 0f, 0f, 100, default, Main.rand.Next(1, 3));
                    Dust dust = Main.dust[num80];
                    dust.velocity *= 3f;
                    if (Main.dust[num80].scale > 1f)
                    {
                        Main.dust[num80].noGravity = true;
                    }
                }
                else if (npc.type == 32)
                {
                    int num81 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 172, 0f, 0f, 100, default, 1.5f);
                    Dust dust = Main.dust[num81];
                    dust.velocity *= 3f;
                    Main.dust[num81].noGravity = true;
                }
                else if (npc.type == 172)
                {
                    int num82 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 106, 0f, 0f, 100, default, 2.5f);
                    Dust dust = Main.dust[num82];
                    dust.velocity *= 3f;
                    Main.dust[num82].noGravity = true;
                }
                else if (npc.type == 283 || npc.type == 284)
                {
                    int num83 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 173);
                    Dust dust = Main.dust[num83];
                    dust.velocity *= 2f;
                    Main.dust[num83].scale = 1.4f;
                }
                else if (npc.type == 285 || npc.type == 286)
                {
                    int num84 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 174, 0f, 0f, 100, default, 1.5f);
                    Dust dust = Main.dust[num84];
                    dust.velocity *= 3f;
                    Main.dust[num84].noGravity = true;
                }
                else if (npc.type == 281 || npc.type == 282)
                {
                    int num85 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 175, 0f, 0f, 100, default, 1.5f);
                    Dust dust = Main.dust[num85];
                    dust.velocity *= 3f;
                    Main.dust[num85].noGravity = true;
                }
                else if (npc.type == 533)
                {
                    int num86 = Dust.NewDust(npc.position, npc.width, npc.height, 27, 0f, 0f, 100, default, 2.5f);
                    Dust dust = Main.dust[num86];
                    dust.velocity *= 3f;
                    Main.dust[num86].noGravity = true;
                }
                else
                {
                    int num87 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default, 2.5f);
                    Dust dust = Main.dust[num87];
                    dust.velocity *= 3f;
                    Main.dust[num87].noGravity = true;
                }
            }
        }
        npc.ai[0] += 1f;
        if (npc.type == 283 || npc.type == 284)
        {
            if (npc.ai[0] == 100f || npc.ai[0] == 150f || npc.ai[0] == 200f || npc.ai[0] == 250f || npc.ai[0] == 300f)
            {
                npc.ai[1] = 30f;
                npc.netUpdate = true;
            }
            if (npc.ai[0] >= 450f)
            {
                npc.ai[0] = 700f;
            }
        }
        else if (npc.type == 172)
        {
            if (npc.ai[0] == 75f || npc.ai[0] == 150f || npc.ai[0] == 225f || npc.ai[0] == 300f || npc.ai[0] == 375f || npc.ai[0] == 450f)
            {
                npc.ai[1] = 30f;
                npc.netUpdate = true;
            }
        }
        else if (npc.type == 533)
        {
            if (npc.ai[0] == 180f)
            {
                npc.ai[1] = 181f;
                npc.netUpdate = true;
            }
        }
        else if (npc.type == 281 || npc.type == 282)
        {
            if (npc.ai[0] == 100f || npc.ai[0] == 120f || npc.ai[0] == 140f || npc.ai[0] == 200f || npc.ai[0] == 220f || npc.ai[0] == 240f || npc.ai[0] == 300f || npc.ai[0] == 320f || npc.ai[0] == 340f)
            {
                npc.ai[1] = 30f;
                npc.netUpdate = true;
            }
            if (npc.ai[0] >= 540f)
            {
                npc.ai[0] = 700f;
            }
        }
        else
        {
            if (Main.getGoodWorld && npc.type == 24 && NPC.AnyNPCs(113))
            {
                npc.ai[0] += 1f;
                if (npc.ai[0] % 2f == 1f)
                {
                    npc.ai[0] -= 1f;
                }
            }
            if (npc.ai[0] == 100f || npc.ai[0] == 200f || npc.ai[0] == 300f)
            {
                npc.ai[1] = 30f;
                npc.netUpdate = true;
            }
        }
        if ((npc.type == 285 || npc.type == 286) && npc.ai[0] > 400f)
        {
            npc.ai[0] = 650f;
        }
        if (npc.type == 533 && npc.ai[0] >= 360f)
        {
            npc.ai[0] = 650f;
        }
        if (npc.ai[0] >= 650f && Main.netMode != 1)
        {
            npc.ai[0] = 1f;
            int targetTileX = (int)Main.player[npc.target].Center.X / 16;
            int targetTileY = (int)Main.player[npc.target].Center.Y / 16;
            Vector2 chosenTile = Vector2.Zero;
            if (npc.AI_AttemptToFindTeleportSpot(ref chosenTile, targetTileX, targetTileY))
            {
                npc.ai[1] = 20f;
                npc.ai[2] = chosenTile.X;
                npc.ai[3] = chosenTile.Y;
            }
            npc.netUpdate = true;
        }
        if (npc.ai[1] > 0f)
        {
            npc.ai[1] -= 1f;
            if (npc.type == 533)
            {
                if (npc.ai[1] % 30f == 0f && npc.ai[1] / 30f < 5f)
                {
                    SoundEngine.PlaySound(SoundID.Item8, npc.position);
                    if (Main.netMode != 1)
                    {
                        Point point = npc.Center.ToTileCoordinates();
                        Point point2 = Main.player[npc.target].Center.ToTileCoordinates();
                        Vector2 vector12 = Main.player[npc.target].Center - npc.Center;
                        int num88 = 6;
                        int num89 = 6;
                        int num90 = 0;
                        int num91 = 2;
                        int num92 = 0;
                        bool flag4 = false;
                        if (vector12.Length() > 2000f)
                        {
                            flag4 = true;
                        }
                        while (!flag4 && num92 < 50)
                        {
                            num92++;
                            int num93 = Main.rand.Next(point2.X - num88, point2.X + num88 + 1);
                            int num94 = Main.rand.Next(point2.Y - num88, point2.Y + num88 + 1);
                            if ((num94 < point2.Y - num90 || num94 > point2.Y + num90 || num93 < point2.X - num90 || num93 > point2.X + num90) && (num94 < point.Y - num89 || num94 > point.Y + num89 || num93 < point.X - num89 || num93 > point.X + num89) && !Main.tile[num93, num94].nactive())
                            {
                                bool flag5 = true;
                                if (flag5 && Main.tile[num93, num94].lava())
                                {
                                    flag5 = false;
                                }
                                if (flag5 && Collision.SolidTiles(num93 - num91, num93 + num91, num94 - num91, num94 + num91))
                                {
                                    flag5 = false;
                                }
                                if (flag5)
                                {
                                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), num93 * 16 + 8, num94 * 16 + 8, 0f, 0f, 596, 0, 1f, Main.myPlayer, npc.target);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else if (npc.ai[1] == 25f)
            {
                if (npc.type >= 281 && npc.type <= 286)
                {
                    if (Main.netMode != 1)
                    {
                        float num95 = 6f;
                        if (npc.type == 285 || npc.type == 286)
                        {
                            num95 = 8f;
                        }
                        if (npc.type == 281 || npc.type == 282)
                        {
                            num95 = 4f;
                        }
                        Vector2 vector13 = new(npc.position.X + npc.width * 0.5f, npc.position.Y);
                        float num96 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector13.X;
                        float num97 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector13.Y;
                        if (npc.type == 283 || npc.type == 284)
                        {
                            num96 += Main.rand.Next(-30, 31);
                            num97 += Main.rand.Next(-30, 31);
                            num96 -= Main.player[npc.target].velocity.X * 10f;
                            num97 -= Main.player[npc.target].velocity.Y * 10f;
                        }
                        float num98 = (float)Math.Sqrt(num96 * num96 + num97 * num97);
                        num98 = num95 / num98;
                        num96 *= num98;
                        num97 *= num98;
                        int num99 = 30;
                        int num100 = 290;
                        if (npc.type == 285 || npc.type == 286)
                        {
                            num100 = 291;
                            num99 = 40;
                        }
                        if (npc.type == 281 || npc.type == 282)
                        {
                            num100 = 293;
                            num99 = 40;
                        }
                        num99 = npc.GetAttackDamage_ForProjectiles(num99, num99 * 0.8f);
                        int num101 = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector13.X, vector13.Y, num96, num97, num100, num99, 0f, Main.myPlayer);
                        Main.projectile[num101].timeLeft = 300;
                        if (num100 == 291)
                        {
                            Main.projectile[num101].ai[0] = Main.player[npc.target].Center.X;
                            Main.projectile[num101].ai[1] = Main.player[npc.target].Center.Y;
                            Main.projectile[num101].netUpdate = true;
                        }
                        npc.localAI[0] = 0f;
                    }
                }
                else
                {
                    if (npc.type != 172)
                    {
                        SoundEngine.PlaySound(SoundID.Item8, npc.position);
                    }
                    if (Main.netMode != 1)
                    {
                        if (npc.type == 29)
                        {
                            NPC.NewNPC(npc.GetSpawnSourceForProjectileNPC(), (int)npc.position.X + npc.width / 2, (int)npc.position.Y - 8, 30);
                        }
                        else if (npc.type == 45)
                        {
                            NPC.NewNPC(npc.GetSpawnSourceForProjectileNPC(), (int)npc.position.X + npc.width / 2, (int)npc.position.Y - 8, 665);
                        }
                        else if (npc.type == 32)
                        {
                            NPC.NewNPC(npc.GetSpawnSourceForProjectileNPC(), (int)npc.position.X + npc.width / 2, (int)npc.position.Y - 8, 33);
                        }
                        else if (npc.type == 172)
                        {
                            float num102 = 10f;
                            Vector2 vector14 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                            float num103 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector14.X + Main.rand.Next(-10, 11);
                            float num104 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector14.Y + Main.rand.Next(-10, 11);
                            float num105 = (float)Math.Sqrt(num103 * num103 + num104 * num104);
                            num105 = num102 / num105;
                            num103 *= num105;
                            num104 *= num105;
                            int num106 = 40;
                            int num107 = 129;
                            int num108 = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector14.X, vector14.Y, num103, num104, num107, num106, 0f, Main.myPlayer);
                            Main.projectile[num108].timeLeft = 300;
                            npc.localAI[0] = 0f;
                        }
                        else
                        {
                            NPC.NewNPC(npc.GetSpawnSourceForProjectileNPC(), (int)npc.position.X + npc.width / 2 + npc.direction * 8, (int)npc.position.Y + 20, 25);
                        }
                    }
                }
            }
        }
        npc.position += npc.netOffset;
        if (npc.type == 29 || npc.type == 45)
        {
            if (Main.rand.Next(5) == 0)
            {
                int num109 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, 27, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, default, 1.5f);
                Main.dust[num109].noGravity = true;
                Main.dust[num109].velocity.X *= 0.5f;
                Main.dust[num109].velocity.Y = -2f;
            }
        }
        else if (npc.type == 32)
        {
            if (Main.rand.Next(3) != 0)
            {
                int num110 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, 172, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, default, 0.9f);
                Main.dust[num110].noGravity = true;
                Main.dust[num110].velocity.X *= 0.3f;
                Main.dust[num110].velocity.Y *= 0.2f;
                Main.dust[num110].velocity.Y -= 1f;
            }
        }
        else if (npc.type == 172)
        {
            int num111 = 1;
            if (npc.alpha == 255)
            {
                num111 = 2;
            }
            for (int num112 = 0; num112 < num111; num112++)
            {
                if (Main.rand.Next(255) > 255 - npc.alpha)
                {
                    int num113 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, 106, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, default, 1.2f);
                    Main.dust[num113].noGravity = true;
                    Main.dust[num113].velocity.X *= 0.1f + Main.rand.Next(30) * 0.01f;
                    Main.dust[num113].velocity.Y *= 0.1f + Main.rand.Next(30) * 0.01f;
                    Dust dust = Main.dust[num113];
                    dust.scale *= 1f + Main.rand.Next(6) * 0.1f;
                }
            }
        }
        else if (npc.type == 283 || npc.type == 284)
        {
            if (Main.rand.Next(2) == 0)
            {
                int num114 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, 173);
                Main.dust[num114].velocity.X *= 0.5f;
                Main.dust[num114].velocity.Y *= 0.5f;
            }
        }
        else if (npc.type == 285 || npc.type == 286)
        {
            if (Main.rand.Next(2) == 0)
            {
                int num115 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, 174, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100);
                Main.dust[num115].noGravity = true;
                Dust dust = Main.dust[num115];
                dust.velocity *= 0.4f;
                Main.dust[num115].velocity.Y -= 0.7f;
            }
        }
        else if (npc.type == 281 || npc.type == 282)
        {
            if (Main.rand.Next(2) == 0)
            {
                int num116 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, 175, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, default, 0.1f);
                Main.dust[num116].noGravity = true;
                Dust dust = Main.dust[num116];
                dust.velocity *= 0.5f;
                Main.dust[num116].fadeIn = 1.2f;
            }
        }
        else if (npc.type == 533)
        {
            Lighting.AddLight(npc.Top, 0.6f, 0.6f, 0.3f);
        }
        else if (Main.rand.Next(2) == 0)
        {
            int num117 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, 6, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, default, 2f);
            Main.dust[num117].noGravity = true;
            Main.dust[num117].velocity.X *= 1f;
            Main.dust[num117].velocity.Y *= 1f;
        }
        npc.position -= npc.netOffset;
    }
}
