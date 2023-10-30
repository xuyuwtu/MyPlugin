namespace VBY.NPCTest;

partial class NPCAIs
{
    public static void AI_045_Golem(NPC npc)
    {
        NPC.golemBoss = npc.whoAmI;
        float num = npc.GetMyBalance();
        if (Main.getGoodWorld)
        {
            num += 2f;
        }
        if ((!Main.player[npc.target].ZoneLihzhardTemple && !Main.player[npc.target].ZoneJungle) || (double)Main.player[npc.target].Center.Y < Main.worldSurface * 16.0)
        {
            num *= 2f;
        }
        if (npc.localAI[0] == 0f)
        {
            npc.localAI[0] = 1f;
            NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X - 84, (int)npc.Center.Y - 9, 247);
            NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X + 78, (int)npc.Center.Y - 9, 248);
            NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X - 3, (int)npc.Center.Y - 57, 246);
        }
        if (npc.target >= 0 && Main.player[npc.target].dead)
        {
            npc.TargetClosest();
            if (Main.player[npc.target].dead)
            {
                npc.noTileCollide = true;
            }
        }
        if (npc.alpha > 0)
        {
            npc.alpha -= 10;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }
            npc.ai[1] = 0f;
        }
        bool flag = false;
        bool flag2 = false;
        bool flag3 = false;
        npc.dontTakeDamage = false;
        for (int i = 0; i < 200; i++)
        {
            if (Main.npc[i].active && Main.npc[i].type == 246)
            {
                flag = true;
            }
            if (Main.npc[i].active && Main.npc[i].type == 247)
            {
                flag2 = true;
            }
            if (Main.npc[i].active && Main.npc[i].type == 248)
            {
                flag3 = true;
            }
        }
        npc.dontTakeDamage = flag;
        if (Main.netMode != 1 && Main.getGoodWorld && npc.velocity.Y > 0f)
        {
            for (int j = (int)(npc.position.X / 16f); (float)j < (npc.position.X + (float)npc.width) / 16f; j++)
            {
                for (int k = (int)(npc.position.Y / 16f); (float)k < (npc.position.Y + (float)npc.width) / 16f; k++)
                {
                    if (Main.tile[j, k].type == 4)
                    {
                        Main.tile[j, k].active(active: false);
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendTileSquare(-1, j, k);
                        }
                    }
                }
            }
        }
        npc.position += npc.netOffset;
        if (!Main.getGoodWorld)
        {
            if (!flag2)
            {
                int num2 = Dust.NewDust(new Vector2(npc.Center.X - 80f * npc.scale, npc.Center.Y - 9f), 8, 8, 31, 0f, 0f, 100);
                Main.dust[num2].alpha += Main.rand.Next(100);
                Main.dust[num2].velocity *= 0.2f;
                Main.dust[num2].velocity.Y -= 0.5f + (float)Main.rand.Next(10) * 0.1f;
                Main.dust[num2].fadeIn = 0.5f + (float)Main.rand.Next(10) * 0.1f;
                if (Main.rand.Next(10) == 0)
                {
                    num2 = Dust.NewDust(new Vector2(npc.Center.X - 80f * npc.scale, npc.Center.Y - 9f), 8, 8, 6);
                    if (Main.rand.Next(20) != 0)
                    {
                        Main.dust[num2].noGravity = true;
                        Main.dust[num2].scale *= 1f + (float)Main.rand.Next(10) * 0.1f;
                        Main.dust[num2].velocity.Y -= 1f;
                    }
                }
            }
            if (!flag3)
            {
                int num3 = Dust.NewDust(new Vector2(npc.Center.X + 62f * npc.scale, npc.Center.Y - 9f), 8, 8, 31, 0f, 0f, 100);
                Main.dust[num3].alpha += Main.rand.Next(100);
                Main.dust[num3].velocity *= 0.2f;
                Main.dust[num3].velocity.Y -= 0.5f + (float)Main.rand.Next(10) * 0.1f;
                Main.dust[num3].fadeIn = 0.5f + (float)Main.rand.Next(10) * 0.1f;
                if (Main.rand.Next(10) == 0)
                {
                    num3 = Dust.NewDust(new Vector2(npc.Center.X + 62f * npc.scale, npc.Center.Y - 9f), 8, 8, 6);
                    if (Main.rand.Next(20) != 0)
                    {
                        Main.dust[num3].noGravity = true;
                        Main.dust[num3].scale *= 1f + (float)Main.rand.Next(10) * 0.1f;
                        Main.dust[num3].velocity.Y -= 1f;
                    }
                }
            }
        }
        npc.position -= npc.netOffset;
        if (npc.noTileCollide && !Main.player[npc.target].dead)
        {
            if (npc.velocity.Y > 0f && npc.Bottom.Y > Main.player[npc.target].Top.Y)
            {
                npc.noTileCollide = false;
            }
            else if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].Center, 1, 1) && !Collision.SolidTiles(npc.position, npc.width, npc.height))
            {
                npc.noTileCollide = false;
            }
        }
        if (npc.ai[0] == 0f)
        {
            if (npc.velocity.Y == 0f)
            {
                npc.velocity.X *= 0.8f;
                float num4 = 1f;
                if (npc.ai[1] > 0f)
                {
                    if (!flag2)
                    {
                        num4 += 2f;
                    }
                    if (!flag3)
                    {
                        num4 += 2f;
                    }
                    if (!flag)
                    {
                        num4 += 2f;
                    }
                    if (npc.life < npc.lifeMax)
                    {
                        num4 += 1f;
                    }
                    if (npc.life < npc.lifeMax / 2)
                    {
                        num4 += 4f;
                    }
                    if (npc.life < npc.lifeMax / 3)
                    {
                        num4 += 8f;
                    }
                    num4 *= num;
                    if (Main.getGoodWorld)
                    {
                        num4 += 100f;
                    }
                }
                npc.ai[1] += num4;
                if (npc.ai[1] >= 300f)
                {
                    npc.ai[1] = -20f;
                    npc.frameCounter = 0.0;
                }
                else if (npc.ai[1] == -1f)
                {
                    npc.noTileCollide = true;
                    npc.TargetClosest();
                    npc.velocity.X = 4 * npc.direction;
                    if (npc.life < npc.lifeMax)
                    {
                        npc.velocity.Y = -12.1f * (num + 9f) / 10f;
                        if ((double)npc.velocity.Y < -19.1)
                        {
                            npc.velocity.Y = -19.1f;
                        }
                    }
                    else
                    {
                        npc.velocity.Y = -12.1f;
                    }
                    npc.ai[0] = 1f;
                    npc.ai[1] = 0f;
                }
            }
        }
        else if (npc.ai[0] == 1f)
        {
            if (npc.velocity.Y == 0f)
            {
                SoundEngine.PlaySound(SoundID.Item14, npc.position);
                npc.ai[0] = 0f;
                var point = npc.Bottom.ToTileCoordinates();
                point.Y += 1;
                for(int i = -5; i <= 5; i++)
                {
                    if (WorldGen.InWorld(point.X + i, point.Y, 10) && Main.tile[point.X + i, point.Y] is not null && WorldGen.SolidTile(Main.tile[point.X + i, point.Y]))
                    {
                        var proj = Main.projectile[npc.NewProjectile(npc.Center, Vector2.UnitY.RotatedByDegress(Main.rand.Next(45) - 90), 526, 50, (point.X + i) << 4, point.Y)];
                        proj.localAI[0] = 0f;
                        proj.localAI[1] = -1f;
                        proj.localAI[2] = 654;
                        break;
                    }
                }
                
                //654
                //for (int l = (int)npc.position.X - 20; l < (int)npc.position.X + npc.width + 40; l += 20)
                //{
                //    for (int m = 0; m < 4; m++)
                //    {
                //        int num5 = Dust.NewDust(new Vector2(npc.position.X - 20f, npc.position.Y + (float)npc.height), npc.width + 20, 4, 31, 0f, 0f, 100, default(Color), 1.5f);
                //        Main.dust[num5].velocity *= 0.2f;
                //    }
                //    int num6 = Gore.NewGore(new Vector2(l - 20, npc.position.Y + (float)npc.height - 8f), default(Vector2), Main.rand.Next(61, 64));
                //    Main.gore[num6].velocity *= 0.4f;
                //}
            }
            else
            {
                npc.TargetClosest();
                if (npc.position.X < Main.player[npc.target].position.X && npc.position.X + (float)npc.width > Main.player[npc.target].position.X + (float)Main.player[npc.target].width)
                {
                    npc.velocity.X *= 0.9f;
                    if (npc.Bottom.Y < Main.player[npc.target].position.Y)
                    {
                        npc.velocity.Y += 0.2f * (num + 1f) / 2f;
                    }
                }
                else
                {
                    if (npc.direction < 0)
                    {
                        npc.velocity.X -= 0.2f;
                    }
                    else if (npc.direction > 0)
                    {
                        npc.velocity.X += 0.2f;
                    }
                    float num7 = 3f;
                    if (npc.life < npc.lifeMax)
                    {
                        num7 += 1f;
                    }
                    if (npc.life < npc.lifeMax / 2)
                    {
                        num7 += 1f;
                    }
                    if (npc.life < npc.lifeMax / 4)
                    {
                        num7 += 1f;
                    }
                    num7 *= (num + 1f) / 2f;
                    if (npc.velocity.X < 0f - num7)
                    {
                        npc.velocity.X = 0f - num7;
                    }
                    if (npc.velocity.X > num7)
                    {
                        npc.velocity.X = num7;
                    }
                }
            }
        }
        if (npc.target <= 0 || npc.target == 255 || Main.player[npc.target].dead)
        {
            npc.TargetClosest();
        }
        int num8 = 3000;
        if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) + Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y) > (float)num8)
        {
            npc.TargetClosest();
            if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) + Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y) > (float)num8)
            {
                npc.active = false;
            }
        }
    }
}