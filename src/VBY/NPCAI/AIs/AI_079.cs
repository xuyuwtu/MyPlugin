namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_079(this NPC npc)
    {
        if (!Main.npc[(int)npc.ai[3]].active || Main.npc[(int)npc.ai[3]].type != 398)
        {
            npc.life = 0;
            npc.HitEffect();
            npc.active = false;
        }
        if (npc.localAI[3] == 13f && !npc.dontTakeDamage)
        {
            npc.PopAllAttachedProjectilesAndTakeDamageForThem();
        }
        npc.dontTakeDamage = npc.localAI[3] >= 15f;
        npc.velocity = Vector2.Zero;
        npc.Center = Main.npc[(int)npc.ai[3]].Center + new Vector2(0f, -400f);
        Vector2 vector211 = new(27f, 59f);
        float num1221 = 0f;
        float num1222 = 0f;
        int num1223 = 0;
        int num1224 = 0;
        if (npc.ai[0] >= 0f)
        {
            float num1225 = npc.ai[0];
            npc.ai[1]++;
            int num1226 = (int)Main.npc[(int)npc.ai[3]].ai[2];
            int num1227 = 2;
            int num1228 = 0;
            int num1229 = 0;
            for (; num1228 < 5; num1228++)
            {
                num1222 = NPC.MoonLordAttacksArray[num1226, num1227, 1, num1228];
                if (!(num1222 + num1229 <= npc.ai[1]))
                {
                    break;
                }
                num1229 += (int)num1222;
            }
            if (num1228 == 5)
            {
                num1228 = 0;
                npc.ai[1] = 0f;
                num1222 = NPC.MoonLordAttacksArray[num1226, num1227, 1, num1228];
                num1229 = 0;
            }
            npc.ai[0] = NPC.MoonLordAttacksArray[num1226, num1227, 0, num1228];
            num1221 = (int)npc.ai[1] - num1229;
            if (npc.ai[0] != num1225)
            {
                npc.netUpdate = true;
            }
        }
        if (npc.ai[0] == -3f)
        {
            npc.damage = 0;
            npc.dontTakeDamage = true;
            npc.rotation = MathHelper.Lerp(npc.rotation, (float)Math.PI / 12f, 0.07f);
            npc.ai[1]++;
            if (npc.ai[1] >= 32f)
            {
                npc.ai[1] = 0f;
            }
            if (npc.ai[1] < 0f)
            {
                npc.ai[1] = 0f;
            }
            if (npc.localAI[2] < 14f)
            {
                npc.localAI[2]++;
            }
        }
        else if (npc.ai[0] == -2f)
        {
            if (Main.npc[(int)npc.ai[3]].ai[0] == 2f)
            {
                npc.ai[0] = -3f;
                return;
            }
            npc.damage = 80;
            npc.dontTakeDamage = true;
            npc.ai[1]++;
            if (npc.ai[1] >= 32f)
            {
                npc.ai[1] = 0f;
            }
            if (npc.ai[1] < 0f)
            {
                npc.ai[1] = 0f;
            }
            npc.ai[2]++;
            if (npc.ai[2] >= 555f)
            {
                npc.ai[2] = 0f;
            }
            if (npc.ai[2] >= 120f)
            {
                num1221 = npc.ai[2] - 120f;
                num1222 = 555f;
                num1223 = 2;
                Vector2 vector212 = new(0f, 216f);
                if (num1221 == 0f && Main.netMode != 1)
                {
                    Vector2 vector213 = npc.Center + vector212;
                    for (int num1230 = 0; num1230 < 255; num1230++)
                    {
                        Player player9 = Main.player[num1230];
                        if (player9.active && !player9.dead && Vector2.Distance(player9.Center, vector213) <= 3000f)
                        {
                            Vector2 vector214 = Main.player[npc.target].Center - vector213;
                            if (vector214 != Vector2.Zero)
                            {
                                vector214.Normalize();
                            }
                            Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector213.X, vector213.Y, vector214.X, vector214.Y, 456, 0, 0f, Main.myPlayer, npc.whoAmI + 1, num1230);
                        }
                    }
                }
                if ((num1221 == 120f || num1221 == 180f || num1221 == 240f) && Main.netMode != 1)
                {
                    for (int num1231 = 0; num1231 < 1000; num1231++)
                    {
                        Projectile projectile5 = Main.projectile[num1231];
                        if (projectile5.active && projectile5.type == 456 && Main.player[(int)projectile5.ai[1]].FindBuffIndex(145) != -1)
                        {
                            Vector2 center21 = Main.player[npc.target].Center;
                            int num1232 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)center21.X, (int)center21.Y, 401);
                            Main.npc[num1232].netUpdate = true;
                            Main.npc[num1232].ai[0] = npc.whoAmI + 1;
                            Main.npc[num1232].ai[1] = num1231;
                        }
                    }
                }
            }
        }
        else if (npc.ai[0] == 0f)
        {
            num1224 = 3;
            npc.TargetClosest(faceTarget: false);
            Vector2 v6 = Main.player[npc.target].Center - npc.Center - new Vector2(0f, -22f);
            float num1233 = v6.Length() / 500f;
            if (num1233 > 1f)
            {
                num1233 = 1f;
            }
            num1233 = 1f - num1233;
            num1233 *= 2f;
            if (num1233 > 1f)
            {
                num1233 = 1f;
            }
            npc.localAI[0] = v6.ToRotation();
            npc.localAI[1] = num1233;
            npc.localAI[2] = MathHelper.Lerp(npc.localAI[2], 1f, 0.2f);
        }
        if (npc.ai[0] == 1f)
        {
            if (num1221 < 180f)
            {
                npc.localAI[1] -= 0.05f;
                if (npc.localAI[1] < 0f)
                {
                    npc.localAI[1] = 0f;
                }
                if (num1221 >= 60f)
                {
                    Vector2 center22 = npc.Center;
                    int num1234 = 0;
                    if (num1221 >= 120f)
                    {
                        num1234 = 1;
                    }
                    for (int num1235 = 0; num1235 < 1 + num1234; num1235++)
                    {
                        int num1236 = 229;
                        float num1237 = 0.8f;
                        if (num1235 % 2 == 1)
                        {
                            num1236 = 229;
                            num1237 = 1.65f;
                        }
                        Vector2 vector215 = center22 + ((float)Main.rand.NextDouble() * ((float)Math.PI * 2f)).ToRotationVector2() * vector211 / 2f;
                        int num1238 = Dust.NewDust(vector215 - Vector2.One * 8f, 16, 16, num1236, npc.velocity.X / 2f, npc.velocity.Y / 2f);
                        Main.dust[num1238].velocity = Vector2.Normalize(center22 - vector215) * 3.5f * (10f - num1234 * 2f) / 10f;
                        Main.dust[num1238].noGravity = true;
                        Main.dust[num1238].scale = num1237;
                        Main.dust[num1238].customData = npc;
                    }
                }
            }
            else if (num1221 < num1222 - 15f)
            {
                if (num1221 == 180f && Main.netMode != 1)
                {
                    npc.TargetClosest(faceTarget: false);
                    Vector2 spinningpoint9 = Main.player[npc.target].Center - npc.Center;
                    spinningpoint9.Normalize();
                    float num1239 = -1f;
                    if (spinningpoint9.X < 0f)
                    {
                        num1239 = 1f;
                    }
                    spinningpoint9 = spinningpoint9.RotatedBy((0f - num1239) * ((float)Math.PI * 2f) / 6f);
                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X, npc.Center.Y, spinningpoint9.X, spinningpoint9.Y, 455, 75, 0f, Main.myPlayer, num1239 * ((float)Math.PI * 2f) / 540f, npc.whoAmI);
                    npc.ai[2] = (spinningpoint9.ToRotation() + (float)Math.PI * 3f) * num1239;
                    npc.netUpdate = true;
                }
                npc.localAI[1] += 0.05f;
                if (npc.localAI[1] > 1f)
                {
                    npc.localAI[1] = 1f;
                }
                float num1240 = (npc.ai[2] >= 0f).ToDirectionInt();
                float num1241 = npc.ai[2];
                if (num1241 < 0f)
                {
                    num1241 *= -1f;
                }
                num1241 += (float)Math.PI * -3f;
                num1241 += num1240 * ((float)Math.PI * 2f) / 540f;
                npc.localAI[0] = num1241;
                npc.ai[2] = (num1241 + (float)Math.PI * 3f) * num1240;
            }
            else
            {
                npc.localAI[1] -= 0.07f;
                if (npc.localAI[1] < 0f)
                {
                    npc.localAI[1] = 0f;
                    if (Main.netMode != 1 && Main.getGoodWorld && Main.remixWorld)
                    {
                        for (int num1242 = 0; num1242 < 30; num1242++)
                        {
                            if (!WorldGen.SolidTile((int)(npc.Center.X / 16f), (int)(npc.Center.Y / 16f)))
                            {
                                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X, npc.Center.Y, Main.rand.Next(-1599, 1600) * 0.01f, Main.rand.Next(-1599, 1) * 0.01f, 1021, 70, 10f);
                            }
                        }
                    }
                }
                num1224 = 3;
            }
        }
        else if (npc.ai[0] == 2f)
        {
            num1223 = 2;
            num1224 = 3;
            Vector2 vector216 = new(0f, 216f);
            if (num1221 == 0f && Main.netMode != 1)
            {
                Vector2 vector217 = npc.Center + vector216;
                for (int num1243 = 0; num1243 < 255; num1243++)
                {
                    Player player10 = Main.player[num1243];
                    if (player10.active && !player10.dead && Vector2.Distance(player10.Center, vector217) <= 3000f)
                    {
                        Vector2 vector218 = Main.player[npc.target].Center - vector217;
                        if (vector218 != Vector2.Zero)
                        {
                            vector218.Normalize();
                        }
                        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector217.X, vector217.Y, vector218.X, vector218.Y, 456, 0, 0f, Main.myPlayer, npc.whoAmI + 1, num1243);
                    }
                }
            }
            if ((num1221 == 120f || num1221 == 180f || num1221 == 240f) && Main.netMode != 1)
            {
                for (int num1244 = 0; num1244 < 1000; num1244++)
                {
                    Projectile projectile6 = Main.projectile[num1244];
                    if (projectile6.active && projectile6.type == 456 && Main.player[(int)projectile6.ai[1]].FindBuffIndex(145) != -1)
                    {
                        Vector2 center23 = Main.player[npc.target].Center;
                        int num1245 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)center23.X, (int)center23.Y, 401);
                        Main.npc[num1245].netUpdate = true;
                        Main.npc[num1245].ai[0] = npc.whoAmI + 1;
                        Main.npc[num1245].ai[1] = num1244;
                    }
                }
            }
        }
        else if (npc.ai[0] == 3f)
        {
            if ((double)num1221 == 1.0)
            {
                npc.TargetClosest(faceTarget: false);
                npc.netUpdate = true;
            }
            Vector2 v7 = Main.player[npc.target].Center + Main.player[npc.target].velocity * 20f - npc.Center;
            npc.localAI[0] = npc.localAI[0].AngleLerp(v7.ToRotation(), 0.5f);
            npc.localAI[1] += 0.05f;
            if (npc.localAI[1] > 1f)
            {
                npc.localAI[1] = 1f;
            }
            if (num1221 == num1222 - 35f)
            {
                SoundEngine.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 6);
            }
            if ((num1221 == num1222 - 14f || num1221 == num1222 - 7f || num1221 == num1222) && Main.netMode != 1)
            {
                Vector2 vector219 = Utils.Vector2FromElipse(npc.localAI[0].ToRotationVector2(), vector211 * npc.localAI[1]);
                Vector2 vector220 = Vector2.Normalize(v7) * 8f;
                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X + vector219.X, npc.Center.Y + vector219.Y, vector220.X, vector220.Y, 462, 30, 0f, Main.myPlayer);
            }
        }
        int num1246 = num1223 * 7;
        if (num1246 > npc.localAI[2])
        {
            npc.localAI[2]++;
        }
        if (num1246 < npc.localAI[2])
        {
            npc.localAI[2]--;
        }
        if (npc.localAI[2] < 0f)
        {
            npc.localAI[2] = 0f;
        }
        if (npc.localAI[2] > 14f)
        {
            npc.localAI[2] = 14f;
        }
        int num1247 = num1224 * 5;
        if (num1247 > npc.localAI[3])
        {
            npc.localAI[3]++;
        }
        if (num1247 < npc.localAI[3])
        {
            npc.localAI[3]--;
        }
        if (npc.localAI[3] < 0f)
        {
            npc.localAI[2] = 0f;
        }
        if (npc.localAI[3] > 15f)
        {
            npc.localAI[2] = 15f;
        }
        int num1248 = 0;
        if (num1248 == 1)
        {
            _ = new Vector2(27f, 59f);
            npc.TargetClosest(faceTarget: false);
            Vector2 v8 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - npc.Center;
            float num1249 = v8.Length() / 200f;
            if (num1249 > 1f)
            {
                num1249 = 1f;
            }
            num1249 = 1f - num1249;
            num1249 *= 2f;
            if (num1249 > 1f)
            {
                num1249 = 1f;
            }
            npc.localAI[0] = v8.ToRotation();
            npc.localAI[1] = num1249;
            npc.localAI[1] = 1f;
        }
        if (num1248 == 2)
        {
            Vector2 vector222 = new(27f, 59f);
            float num1250 = (float)Math.PI * 2f * ((float)Main.timeForVisualEffects % 600f) / 600f;
            npc.localAI[0] = new Vector2((float)Math.Cos(num1250) * vector222.X, (float)Math.Sin(num1250) * vector222.Y).ToRotation();
            npc.localAI[1] = 0.75f;
            if (npc.ai[1] == 0f)
            {
                _ = num1250.ToRotationVector2();
                Vector2 vector223 = Vector2.One;
                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X, npc.Center.Y, vector223.X, vector223.Y, 455, 1, 0f, Main.myPlayer, (float)Math.PI / 300f, npc.whoAmI);
            }
            npc.ai[1]++;
            if (npc.ai[1] >= 600f)
            {
                npc.ai[1] = 0f;
            }
        }
        if (num1248 == 3)
        {
            Vector2 vector224 = new(0f, 216f);
            if (npc.ai[1] == 0f)
            {
                npc.TargetClosest(faceTarget: false);
                Vector2 vector225 = Main.player[npc.target].Center - npc.Center;
                vector225.Normalize();
                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X + vector224.X, npc.Center.Y + vector224.Y, vector225.X, vector225.Y, 456, 0, 0f, Main.myPlayer, npc.whoAmI + 1, npc.target);
            }
            npc.ai[1]++;
            if (npc.ai[1] >= 600f)
            {
                npc.ai[1] = 0f;
            }
        }
        if (num1248 == 4)
        {
            _ = new Vector2(27f, 59f);
            npc.TargetClosest(faceTarget: false);
            Vector2 v9 = Main.player[npc.target].Center + Main.player[npc.target].velocity * 20f - npc.Center;
            npc.localAI[0] = npc.localAI[0].AngleLerp(v9.ToRotation(), 0.5f);
            npc.localAI[1] = 1f;
            npc.ai[1]++;
            if (npc.ai[1] == 55f)
            {
                SoundEngine.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 6);
            }
            if (npc.ai[1] == 76f || npc.ai[1] == 83f || npc.ai[1] == 90f)
            {
                Vector2 vector227 = Utils.Vector2FromElipse(elipseSizes: new Vector2(27f, 59f) * npc.localAI[1], angleVector: npc.localAI[0].ToRotationVector2());
                Vector2 vector228 = Vector2.Normalize(v9) * 8f;
                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X + vector227.X, npc.Center.Y + vector227.Y, vector228.X, vector228.Y, 462, 5, 0f, Main.myPlayer);
            }
            if (npc.ai[1] >= 90f)
            {
                npc.ai[1] = 0f;
            }
        }
    }
}
