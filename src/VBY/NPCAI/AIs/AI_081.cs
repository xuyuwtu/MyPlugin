namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_081(this NPC npc)
    {
        if (Main.rand.Next(420) == 0)
        {
            SoundEngine.PlaySound(29, (int)npc.Center.X, (int)npc.Center.Y, Main.rand.Next(100, 101));
        }
        Vector2 vector229 = new(30f);
        if (!Main.npc[(int)npc.ai[3]].active || Main.npc[(int)npc.ai[3]].type != 398)
        {
            npc.life = 0;
            npc.HitEffect();
            npc.active = false;
        }

        float num1256 = 0f;
        float num1257 = npc.ai[0];
        npc.ai[1]++;
        int num1258 = 0;
        int num1259 = 0;
        for (; num1258 < 10; num1258++)
        {
            num1256 = NPC.MoonLordAttacksArray2[1, num1258];
            if (!(num1256 + num1259 <= npc.ai[1]))
            {
                break;
            }
            num1259 += (int)num1256;
        }
        if (num1258 == 10)
        {
            num1258 = 0;
            npc.ai[1] = 0f;
            num1256 = NPC.MoonLordAttacksArray2[1, num1258];
            num1259 = 0;
        }
        npc.ai[0] = NPC.MoonLordAttacksArray2[0, num1258];
        float num1255 = (int)npc.ai[1] - num1259;
        if (npc.ai[0] != num1257)
        {
            npc.netUpdate = true;
        }
        if (npc.ai[0] == -1f)
        {
            npc.ai[1]++;
            if (npc.ai[1] > 180f)
            {
                npc.ai[1] = 0f;
            }
            float num1260;
            if (npc.ai[1] < 60f)
            {
                num1260 = 0.75f;
                npc.localAI[0] = 0f;
                npc.localAI[1] = (float)Math.Sin(npc.ai[1] * ((float)Math.PI * 2f) / 15f) * 0.35f;
                if (npc.localAI[1] < 0f)
                {
                    npc.localAI[0] = (float)Math.PI;
                }
            }
            else if (npc.ai[1] < 120f)
            {
                num1260 = 1f;
                if (npc.localAI[1] < 0.5f)
                {
                    npc.localAI[1] += 0.025f;
                }
                npc.localAI[0] += (float)Math.PI / 15f;
            }
            else
            {
                num1260 = 1.15f;
                npc.localAI[1] -= 0.05f;
                if (npc.localAI[1] < 0f)
                {
                    npc.localAI[1] = 0f;
                }
            }
            npc.localAI[2] = MathHelper.Lerp(npc.localAI[2], num1260, 0.3f);
        }
        if (npc.ai[0] == 0f)
        {
            npc.TargetClosest(faceTarget: false);
            Vector2 v10 = Main.player[npc.target].Center + Main.player[npc.target].velocity * 20f - npc.Center;
            npc.localAI[0] = npc.localAI[0].AngleLerp(v10.ToRotation(), 0.5f);
            npc.localAI[1] += 0.05f;
            if (npc.localAI[1] > 0.7f)
            {
                npc.localAI[1] = 0.7f;
            }
            npc.localAI[2] = MathHelper.Lerp(npc.localAI[2], 1f, 0.2f);
            float num1261 = 24f;
            Vector2 center25 = npc.Center;
            Vector2 center26 = Main.player[npc.target].Center;
            Vector2 vector230 = center26 - center25;
            Vector2 vector231 = vector230 - Vector2.UnitY * 200f;
            vector231 = Vector2.Normalize(vector231) * num1261;
            int num1262 = 30;
            npc.velocity.X = (npc.velocity.X * (num1262 - 1) + vector231.X) / num1262;
            npc.velocity.Y = (npc.velocity.Y * (num1262 - 1) + vector231.Y) / num1262;
            float num1263 = 0.25f;
            for (int num1264 = 0; num1264 < 200; num1264++)
            {
                if (num1264 != npc.whoAmI && Main.npc[num1264].active && Main.npc[num1264].type == 400 && Vector2.Distance(npc.Center, Main.npc[num1264].Center) < 150f)
                {
                    if (npc.position.X < Main.npc[num1264].position.X)
                    {
                        npc.velocity.X -= num1263;
                    }
                    else
                    {
                        npc.velocity.X += num1263;
                    }
                    if (npc.position.Y < Main.npc[num1264].position.Y)
                    {
                        npc.velocity.Y -= num1263;
                    }
                    else
                    {
                        npc.velocity.Y += num1263;
                    }
                }
            }
        }
        else if (npc.ai[0] == 1f)
        {
            if (num1255 == 0f)
            {
                npc.TargetClosest(faceTarget: false);
                npc.netUpdate = true;
            }
            npc.velocity *= 0.95f;
            if (npc.velocity.Length() < 1f)
            {
                npc.velocity = Vector2.Zero;
            }
            Vector2 v11 = Main.player[npc.target].Center + Main.player[npc.target].velocity * 20f - npc.Center;
            npc.localAI[0] = npc.localAI[0].AngleLerp(v11.ToRotation(), 0.5f);
            npc.localAI[1] += 0.05f;
            if (npc.localAI[1] > 1f)
            {
                npc.localAI[1] = 1f;
            }
            if (num1255 < 20f)
            {
                npc.localAI[2] = MathHelper.Lerp(npc.localAI[2], 1.1f, 0.2f);
            }
            else
            {
                npc.localAI[2] = MathHelper.Lerp(npc.localAI[2], 0.4f, 0.2f);
            }
            if (num1255 == num1256 - 35f)
            {
                SoundEngine.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 6);
            }
            if ((num1255 == num1256 - 14f || num1255 == num1256 - 7f || num1255 == num1256) && Main.netMode != 1)
            {
                Vector2 vector232 = Utils.Vector2FromElipse(npc.localAI[0].ToRotationVector2(), vector229 * npc.localAI[1]);
                Vector2 vector233 = Vector2.Normalize(v11) * 8f;
                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X + vector232.X, npc.Center.Y + vector232.Y, vector233.X, vector233.Y, 462, 35, 0f, Main.myPlayer);
            }
        }
        else if (npc.ai[0] == 2f)
        {
            if (num1255 < 15f)
            {
                npc.localAI[1] -= 0.07f;
                if (npc.localAI[1] < 0f)
                {
                    npc.localAI[1] = 0f;
                }
                npc.localAI[2] = MathHelper.Lerp(npc.localAI[2], 0.4f, 0.2f);
                npc.velocity *= 0.8f;
                if (npc.velocity.Length() < 1f)
                {
                    npc.velocity = Vector2.Zero;
                }
            }
            else if (num1255 < 75f)
            {
                float num1265 = (num1255 - 15f) / 10f;
                int num1266 = 0;
                int num1267 = 0;
                switch ((int)num1265)
                {
                    case 0:
                        num1266 = 0;
                        num1267 = 2;
                        break;
                    case 1:
                        num1266 = 2;
                        num1267 = 5;
                        break;
                    case 2:
                        num1266 = 5;
                        num1267 = 3;
                        break;
                    case 3:
                        num1266 = 3;
                        num1267 = 1;
                        break;
                    case 4:
                        num1266 = 1;
                        num1267 = 4;
                        break;
                    case 5:
                        num1266 = 4;
                        num1267 = 0;
                        break;
                }
                Vector2 spinningpoint10 = Vector2.UnitY * -30f;
                Vector2 value8 = spinningpoint10.RotatedBy(num1266 * ((float)Math.PI * 2f) / 6f);
                Vector2 value9 = spinningpoint10.RotatedBy(num1267 * ((float)Math.PI * 2f) / 6f);
                Vector2 vector234 = Vector2.Lerp(value8, value9, num1265 - (int)num1265);
                float value10 = vector234.Length() / 30f;
                npc.localAI[0] = vector234.ToRotation();
                npc.localAI[1] = MathHelper.Lerp(npc.localAI[1], value10, 0.5f);
                for (int num1268 = 0; num1268 < 2; num1268++)
                {
                    int num1269 = Dust.NewDust(npc.Center + vector234 - Vector2.One * 4f, 0, 0, 229);
                    Dust dust = Main.dust[num1269];
                    dust.velocity += vector234 / 15f;
                    Main.dust[num1269].noGravity = true;
                }
                if ((num1255 - 15f) % 10f == 0f && Main.netMode != 1)
                {
                    Vector2 vec3 = Vector2.Normalize(vector234);
                    if (vec3.HasNaNs())
                    {
                        vec3 = Vector2.UnitY * -1f;
                    }
                    vec3 *= 4f;
                    _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X + vector234.X, npc.Center.Y + vector234.Y, vec3.X, vec3.Y, 454, 40, 0f, Main.myPlayer, 30f, npc.whoAmI);
                }
            }
            else if (num1255 < 105f)
            {
                npc.localAI[0] = npc.localAI[0].AngleLerp(npc.ai[2] - (float)Math.PI / 2f, 0.2f);
                npc.localAI[2] = MathHelper.Lerp(npc.localAI[2], 0.75f, 0.2f);
                if (num1255 == 75f)
                {
                    npc.TargetClosest(faceTarget: false);
                    npc.netUpdate = true;
                    npc.velocity = Vector2.UnitY * -7f;
                    for (int num1271 = 0; num1271 < 1000; num1271++)
                    {
                        Projectile projectile7 = Main.projectile[num1271];
                        if (projectile7.active && projectile7.type == 454 && projectile7.ai[1] == npc.whoAmI && projectile7.ai[0] != -1f)
                        {
                            Projectile projectile8 = projectile7;
                            projectile8.velocity += npc.velocity;
                            projectile7.netUpdate = true;
                        }
                    }
                }
                npc.velocity.Y *= 0.96f;
                npc.ai[2] = (Main.player[npc.target].Center - npc.Center).ToRotation() + (float)Math.PI / 2f;
                npc.rotation = npc.rotation.AngleTowards(npc.ai[2], (float)Math.PI / 30f);
            }
            else if (num1255 < 120f)
            {
                SoundEngine.PlaySound(29, (int)npc.Center.X, (int)npc.Center.Y, 102);
                if (num1255 == 105f)
                {
                    npc.netUpdate = true;
                }
                Vector2 vector235 = (npc.ai[2] - (float)Math.PI / 2f).ToRotationVector2() * 12f;
                npc.velocity = vector235 * 2f;
                for (int num1272 = 0; num1272 < 1000; num1272++)
                {
                    Projectile projectile9 = Main.projectile[num1272];
                    if (projectile9.active && projectile9.type == 454 && projectile9.ai[1] == npc.whoAmI && projectile9.ai[0] != -1f)
                    {
                        projectile9.ai[0] = -1f;
                        projectile9.velocity = vector235;
                        projectile9.netUpdate = true;
                    }
                }
            }
            else
            {
                npc.velocity *= 0.92f;
                npc.rotation = npc.rotation.AngleLerp(0f, 0.2f);
            }
        }
        else if (npc.ai[0] == 3f)
        {
            if (num1255 < 15f)
            {
                npc.localAI[1] -= 0.07f;
                if (npc.localAI[1] < 0f)
                {
                    npc.localAI[1] = 0f;
                }
                npc.localAI[2] = MathHelper.Lerp(npc.localAI[2], 0.4f, 0.2f);
                npc.velocity *= 0.9f;
                if (npc.velocity.Length() < 1f)
                {
                    npc.velocity = Vector2.Zero;
                }
            }
            else if (num1255 < 45f)
            {
                npc.localAI[0] = 0f;
                npc.localAI[1] = (float)Math.Sin((num1255 - 15f) * ((float)Math.PI * 2f) / 15f) * 0.5f;
                if (npc.localAI[1] < 0f)
                {
                    npc.localAI[0] = (float)Math.PI;
                }
            }
            else if (num1255 < 185f)
            {
                if (num1255 == 45f)
                {
                    npc.ai[2] = (Main.rand.Next(2) == 0).ToDirectionInt() * ((float)Math.PI * 2f) / 40f;
                    npc.netUpdate = true;
                }
                if ((num1255 - 15f - 30f) % 40f == 0f)
                {
                    npc.ai[2] *= 0.95f;
                }
                npc.localAI[0] += npc.ai[2];
                npc.localAI[1] += 0.05f;
                if (npc.localAI[1] > 1f)
                {
                    npc.localAI[1] = 1f;
                }
                Vector2 vector236 = npc.localAI[0].ToRotationVector2() * vector229 * npc.localAI[1];
                float num1273 = MathHelper.Lerp(8f, 20f, (num1255 - 15f - 30f) / 140f);
                npc.velocity = Vector2.Normalize(vector236) * num1273;
                npc.rotation = npc.rotation.AngleLerp(npc.velocity.ToRotation() + (float)Math.PI / 2f, 0.2f);
                if ((num1255 - 15f - 30f) % 10f == 0f && Main.netMode != 1)
                {
                    Vector2 vector237 = npc.Center + Vector2.Normalize(vector236) * vector229.Length() * 0.4f;
                    Vector2 vector238 = Vector2.Normalize(vector236) * 8f;
                    float ai3 = ((float)Math.PI * 2f * (float)Main.rand.NextDouble() - (float)Math.PI) / 30f + (float)Math.PI / 180f * npc.ai[2];
                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector237.X, vector237.Y, vector238.X, vector238.Y, 452, 35, 0f, Main.myPlayer, 0f, ai3);
                }
            }
            else
            {
                npc.velocity *= 0.88f;
                npc.rotation = npc.rotation.AngleLerp(0f, 0.2f);
                npc.localAI[1] -= 0.07f;
                if (npc.localAI[1] < 0f)
                {
                    npc.localAI[1] = 0f;
                }
                npc.localAI[2] = MathHelper.Lerp(npc.localAI[2], 1f, 0.2f);
            }
        }
        else
        {
            if (npc.ai[0] != 4f)
            {
                return;
            }
            if (num1255 == 0f)
            {
                npc.TargetClosest(faceTarget: false);
                npc.netUpdate = true;
            }
            if (num1255 < 180f)
            {
                npc.localAI[2] = MathHelper.Lerp(npc.localAI[2], 1f, 0.2f);
                npc.localAI[1] -= 0.05f;
                if (npc.localAI[1] < 0f)
                {
                    npc.localAI[1] = 0f;
                }
                npc.velocity *= 0.95f;
                if (npc.velocity.Length() < 1f)
                {
                    npc.velocity = Vector2.Zero;
                }
                if (!(num1255 >= 60f))
                {
                    return;
                }
                Vector2 center27 = npc.Center;
                int num1274 = 0;
                if (num1255 >= 120f)
                {
                    num1274 = 1;
                }
                for (int num1275 = 0; num1275 < 1 + num1274; num1275++)
                {
                    int num1276 = 229;
                    float num1277 = 0.8f;
                    if (num1275 % 2 == 1)
                    {
                        num1276 = 229;
                        num1277 = 1.65f;
                    }
                    Vector2 vector239 = center27 + ((float)Main.rand.NextDouble() * ((float)Math.PI * 2f)).ToRotationVector2() * vector229 / 2f;
                    int num1278 = Dust.NewDust(vector239 - Vector2.One * 8f, 16, 16, num1276, npc.velocity.X / 2f, npc.velocity.Y / 2f);
                    Main.dust[num1278].velocity = Vector2.Normalize(center27 - vector239) * 3.5f * (10f - num1274 * 2f) / 10f;
                    Main.dust[num1278].noGravity = true;
                    Main.dust[num1278].scale = num1277;
                    Main.dust[num1278].customData = npc;
                }
            }
            else if (num1255 < num1256 - 15f)
            {
                if (num1255 == 180f && Main.netMode != 1)
                {
                    npc.TargetClosest(faceTarget: false);
                    Vector2 spinningpoint11 = Main.player[npc.target].Center - npc.Center;
                    spinningpoint11.Normalize();
                    float num1279 = -1f;
                    if (spinningpoint11.X < 0f)
                    {
                        num1279 = 1f;
                    }
                    spinningpoint11 = spinningpoint11.RotatedBy((0f - num1279) * ((float)Math.PI * 2f) / 6f);
                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X, npc.Center.Y, spinningpoint11.X, spinningpoint11.Y, 455, 50, 0f, Main.myPlayer, num1279 * ((float)Math.PI * 2f) / 540f, npc.whoAmI);
                    npc.ai[2] = (spinningpoint11.ToRotation() + (float)Math.PI * 3f) * num1279;
                    npc.netUpdate = true;
                }
                npc.localAI[1] += 0.05f;
                if (npc.localAI[1] > 1f)
                {
                    npc.localAI[1] = 1f;
                }
                float num1280 = (npc.ai[2] >= 0f).ToDirectionInt();
                float num1281 = npc.ai[2];
                if (num1281 < 0f)
                {
                    num1281 *= -1f;
                }
                num1281 += (float)Math.PI * -3f;
                num1281 += num1280 * ((float)Math.PI * 2f) / 540f;
                npc.localAI[0] = num1281;
                npc.ai[2] = (num1281 + (float)Math.PI * 3f) * num1280;
            }
            else
            {
                npc.localAI[1] -= 0.07f;
                if (npc.localAI[1] < 0f)
                {
                    npc.localAI[1] = 0f;
                }
            }
        }
    }
}
