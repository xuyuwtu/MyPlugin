namespace VBY.NPCTest;

partial class NPCAIs
{
    public static void AI_078(this NPC npc)
    {
        NPC.InitializeMoonLordAttacks();
        if (!Main.npc[(int)npc.ai[3]].active || Main.npc[(int)npc.ai[3]].type != 398)
        {
            npc.life = 0;
            npc.HitEffect();
            npc.active = false;
        }
        bool flag79 = npc.ai[2] == 0f;
        float num1192 = -flag79.ToDirectionInt();
        npc.spriteDirection = (int)num1192;
        if (npc.frameCounter == 19.0 && !npc.dontTakeDamage)
        {
            npc.PopAllAttachedProjectilesAndTakeDamageForThem();
        }
        npc.dontTakeDamage = npc.frameCounter >= 21.0;
        Vector2 vector170 = new(30f, 66f);
        float num1193 = 0f;
        float num1194 = 0f;
        bool flag80 = true;
        int num1195 = 0;
        if (npc.ai[0] != -2f)
        {
            float num1196 = npc.ai[0];
            npc.ai[1]++;
            int num1197 = (int)Main.npc[(int)npc.ai[3]].ai[2];
            int num1198 = ((!flag79) ? 1 : 0);
            int num1199 = 0;
            int num1200 = 0;
            for (; num1199 < 5; num1199++)
            {
                num1194 = NPC.MoonLordAttacksArray[num1197, num1198, 1, num1199];
                if (!(num1194 + num1200 <= npc.ai[1]))
                {
                    break;
                }
                num1200 += (int)num1194;
            }
            if (num1199 == 5)
            {
                num1199 = 0;
                npc.ai[1] = 0f;
                num1194 = NPC.MoonLordAttacksArray[num1197, num1198, 1, num1199];
                num1200 = 0;
            }
            npc.ai[0] = NPC.MoonLordAttacksArray[num1197, num1198, 0, num1199];
            num1193 = (int)npc.ai[1] - num1200;
            if (npc.ai[0] != num1196)
            {
                npc.netUpdate = true;
            }
        }
        if (npc.ai[0] == -2f)
        {
            npc.damage = 80;
            num1195 = 0;
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
            Vector2 center16 = Main.npc[(int)npc.ai[3]].Center;
            Vector2 vector171 = center16 + new Vector2(350f * num1192, -100f);
            Vector2 vector172 = vector171 - npc.Center;
            if (vector172.Length() > 20f)
            {
                vector172.Normalize();
                vector172 *= 6f;
                Vector2 value6 = npc.velocity;
                if (vector172 != Vector2.Zero)
                {
                    npc.SimpleFlyMovement(vector172, 0.3f);
                }
                npc.velocity = Vector2.Lerp(value6, npc.velocity, 0.5f);
            }
        }
        else if (npc.ai[0] == 0f)
        {
            num1195 = 3;
            npc.localAI[1] -= 0.05f;
            if (npc.localAI[1] < 0f)
            {
                npc.localAI[1] = 0f;
            }
            Vector2 center17 = Main.npc[(int)npc.ai[3]].Center;
            Vector2 vector173 = center17 + new Vector2(350f * num1192, -100f);
            Vector2 vector174 = vector173 - npc.Center;
            if (vector174.Length() > 20f)
            {
                vector174.Normalize();
                vector174 *= 6f;
                Vector2 value7 = npc.velocity;
                if (vector174 != Vector2.Zero)
                {
                    npc.SimpleFlyMovement(vector174, 0.3f);
                }
                npc.velocity = Vector2.Lerp(value7, npc.velocity, 0.5f);
            }
        }
        else if (npc.ai[0] == 1f)
        {
            num1195 = 0;
            int num1201 = 7;
            int num1202 = 4;
            if (num1193 >= num1201 * num1202 * 2)
            {
                npc.localAI[1] -= 0.07f;
                if (npc.localAI[1] < 0f)
                {
                    npc.localAI[1] = 0f;
                }
            }
            else if (num1193 >= num1201 * num1202)
            {
                npc.localAI[1] += 0.05f;
                if (npc.localAI[1] > 0.75f)
                {
                    npc.localAI[1] = 0.75f;
                }
                float num1203 = (float)Math.PI * 2f * (num1193 % (num1201 * num1202)) / (num1201 * num1202) - (float)Math.PI / 2f;
                npc.localAI[0] = new Vector2((float)Math.Cos(num1203) * vector170.X, (float)Math.Sin(num1203) * vector170.Y).ToRotation();
                if (num1193 % num1202 == 0f)
                {
                    Vector2 vector175 = new(1f * (0f - num1192), 3f);
                    Vector2 vector176 = Utils.Vector2FromElipse(npc.localAI[0].ToRotationVector2(), vector170 * npc.localAI[1]);
                    Vector2 vector177 = npc.Center + Vector2.Normalize(vector176) * vector170.Length() * 0.4f + vector175;
                    Vector2 vector178 = Vector2.Normalize(vector176) * 8f;
                    float ai = ((float)Math.PI * 2f * (float)Main.rand.NextDouble() - (float)Math.PI) / 30f + (float)Math.PI / 180f * num1192;
                    //Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector177.X, vector177.Y, vector178.X, vector178.Y, 452, 30, 0f, Main.myPlayer, 0f, ai);
                    var proj = Main.projectile[Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector177.X, vector177.Y, vector178.X, vector178.Y, 464, 30, 0f, Main.myPlayer, 0f, 1f, 452)];
                    proj.localAI[0] = 40;
                    proj.localAI[1] = 2;
                }
            }
            else
            {
                npc.localAI[1] += 0.02f;
                if (npc.localAI[1] > 0.75f)
                {
                    npc.localAI[1] = 0.75f;
                }
                float num1204 = (float)Math.PI * 2f * (num1193 % (num1201 * num1202)) / (num1201 * num1202) - (float)Math.PI / 2f;
                npc.localAI[0] = new Vector2((float)Math.Cos(num1204) * vector170.X, (float)Math.Sin(num1204) * vector170.Y).ToRotation();
            }
        }
        else if (npc.ai[0] == 2f)
        {
            npc.localAI[1] -= 0.05f;
            if (npc.localAI[1] < 0f)
            {
                npc.localAI[1] = 0f;
            }
            Vector2 center18 = Main.npc[(int)npc.ai[3]].Center;
            Vector2 vector179 = new Vector2(220f * num1192, -60f) + center18;
            vector179 += new Vector2(num1192 * 100f, -50f);
            Vector2 vector180 = new(400f * num1192, -60f);
            if (num1193 < 30f)
            {
                Vector2 vector181 = vector179 - npc.Center;
                if (vector181 != Vector2.Zero)
                {
                    Vector2 vector182 = vector181;
                    vector182.Normalize();
                    npc.velocity = Vector2.SmoothStep(npc.velocity, vector182 * Math.Min(8f, vector181.Length()), 0.2f);
                }
            }
            else if (num1193 < 210f)
            {
                num1195 = 1;
                int num1205 = (int)num1193 - 30;
                if (num1205 % 30 == 0 && Main.netMode != 1)
                {
                    Vector2 vector183 = new(5f * num1192, -8f);
                    int num1206 = num1205 / 30;
                    vector183.X += (num1206 - 3.5f) * num1192 * 3f;
                    vector183.Y += (num1206 - 4.5f) * 1f;
                    vector183 *= 1.2f;
                    _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X, npc.Center.Y, vector183.X, vector183.Y, 454, 40, 1f, Main.myPlayer, 0f, npc.whoAmI);
                }
                Vector2 vector184 = Vector2.SmoothStep(vector179, vector179 + vector180, (num1193 - 30f) / 180f) - npc.Center;
                if (vector184 != Vector2.Zero)
                {
                    Vector2 vector185 = vector184;
                    vector185.Normalize();
                    npc.velocity = Vector2.Lerp(npc.velocity, vector185 * Math.Min(20f, vector184.Length()), 0.5f);
                }
            }
            else if (num1193 < 282f)
            {
                num1195 = 0;
                npc.velocity *= 0.9f;
            }
            else if (num1193 < 287f)
            {
                num1195 = 1;
                npc.velocity *= 0.9f;
            }
            else if (num1193 < 292f)
            {
                num1195 = 2;
                npc.velocity *= 0.9f;
            }
            else if (num1193 < 300f)
            {
                num1195 = 3;
                if (num1193 == 292f && Main.netMode != 1)
                {
                    int num1208 = Player.FindClosest(npc.position, npc.width, npc.height);
                    Vector2 vector186 = Vector2.Normalize(Main.player[num1208].Center - (npc.Center + Vector2.UnitY * -350f));
                    if (float.IsNaN(vector186.X) || float.IsNaN(vector186.Y))
                    {
                        vector186 = Vector2.UnitY;
                    }
                    vector186 *= 12f;
                    for (int num1209 = 0; num1209 < 1000; num1209++)
                    {
                        Projectile projectile3 = Main.projectile[num1209];
                        if (projectile3.active && projectile3.type == 454 && projectile3.ai[1] == npc.whoAmI && projectile3.ai[0] != -1f)
                        {
                            projectile3.ai[0] = -1f;
                            projectile3.velocity = vector186;
                            projectile3.netUpdate = true;
                            NetMessage.SendData(27, -1, -1, null, num1209);
                        }
                    }
                }
                Vector2 vector187 = Vector2.SmoothStep(vector179, vector179 + vector180, 1f - (num1193 - 270f) / 30f) - npc.Center;
                if (vector187 != Vector2.Zero)
                {
                    Vector2 vector188 = vector187;
                    vector188.Normalize();
                    npc.velocity = Vector2.Lerp(npc.velocity, vector188 * Math.Min(14f, vector187.Length()), 0.1f);
                }
            }
            else
            {
                num1195 = 3;
                Vector2 vector189 = vector179 - npc.Center;
                if (vector189 != Vector2.Zero)
                {
                    Vector2 vector190 = vector189;
                    vector190.Normalize();
                    npc.velocity = Vector2.SmoothStep(npc.velocity, vector190 * Math.Min(8f, vector189.Length()), 0.2f);
                }
            }
        }
        else if (npc.ai[0] == 3f)
        {
            if (num1193 == 0f)
            {
                npc.TargetClosest(faceTarget: false);
                npc.netUpdate = true;
            }
            Vector2 v4 = Main.player[npc.target].Center + Main.player[npc.target].velocity * 20f - npc.Center;
            npc.localAI[0] = npc.localAI[0].AngleLerp(v4.ToRotation(), 0.5f);
            npc.localAI[1] += 0.05f;
            if (npc.localAI[1] > 1f)
            {
                npc.localAI[1] = 1f;
            }
            if (num1193 == num1194 - 35f)
            {
                SoundEngine.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 6);
            }
            if ((num1193 == num1194 - 14f || num1193 == num1194 - 7f || num1193 == num1194) && Main.netMode != 1)
            {
                Vector2 vector191 = Utils.Vector2FromElipse(npc.localAI[0].ToRotationVector2(), vector170 * npc.localAI[1]);
                Vector2 vector192 = Vector2.Normalize(v4) * 8f;
                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X + vector191.X, npc.Center.Y + vector191.Y, vector192.X, vector192.Y, 462, 30, 0f, Main.myPlayer);
            }
        }
        if (flag80)
        {
            Vector2 center19 = Main.npc[(int)npc.ai[3]].Center;
            Vector2 vector193 = new Vector2(220f * num1192, -60f) + center19;
            Vector2 vector194 = vector193 + new Vector2(num1192 * 110f, -150f);
            Vector2 max = vector194 + new Vector2(num1192 * 370f, 150f);
            if (vector194.X > max.X)
            {
                Utils.Swap(ref vector194.X, ref max.X);
            }
            if (vector194.Y > max.Y)
            {
                Utils.Swap(ref vector194.Y, ref max.Y);
            }
            Vector2 vector195 = Vector2.Clamp(npc.Center + npc.velocity, vector194, max);
            if (vector195 != npc.Center + npc.velocity)
            {
                npc.Center = vector195 - npc.velocity;
            }
        }
        int num1210 = num1195 * 7;
        if (num1210 > npc.frameCounter)
        {
            npc.frameCounter++;
        }
        if (num1210 < npc.frameCounter)
        {
            npc.frameCounter--;
        }
        if (npc.frameCounter < 0.0)
        {
            npc.frameCounter = 0.0;
        }
        if (npc.frameCounter > 21.0)
        {
            npc.frameCounter = 21.0;
        }
        int num1211 = 0;
        if (flag79)
        {
            num1211 = 0;
        }
        switch (num1211)
        {
            case 1:
                if (npc.ai[0] == 0f)
                {
                    if ((npc.ai[1] += 1f) >= 20f)
                    {
                        npc.ai[1] = 0f;
                        npc.ai[0] = 1f;
                        npc.netUpdate = true;
                    }
                    npc.velocity = Vector2.UnitX * 4f;
                }
                else if (npc.ai[0] == 1f)
                {
                    if ((npc.ai[1] += 1f) >= 20f)
                    {
                        npc.ai[1] = 0f;
                        npc.ai[0] = 2f;
                        npc.netUpdate = true;
                    }
                    npc.velocity = Vector2.UnitX * -4f;
                }
                else if (npc.ai[0] == 2f || npc.ai[0] == 4f)
                {
                    if ((npc.ai[1] += 1f) >= 20f)
                    {
                        npc.ai[1] = 0f;
                        npc.ai[0]++;
                        npc.netUpdate = true;
                    }
                    npc.velocity = Vector2.UnitY * -4f * (flag79 ? 1 : (-1));
                }
                else
                {
                    if (npc.ai[0] != 3f && npc.ai[0] != 5f)
                    {
                        break;
                    }
                    if ((npc.ai[1] += 1f) >= 20f)
                    {
                        npc.ai[1] = 0f;
                        npc.ai[0]++;
                        if (npc.ai[0] == 6f)
                        {
                            npc.ai[0] = 0f;
                        }
                        npc.netUpdate = true;
                    }
                    npc.velocity = Vector2.UnitY * 4f * (flag79 ? 1 : (-1));
                }
                break;
            case 2:
                {
                    _ = new Vector2(30f, 66f);
                    npc.TargetClosest(faceTarget: false);
                    Vector2 v5 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - npc.Center;
                    float num1212 = v5.Length() / 200f;
                    if (num1212 > 1f)
                    {
                        num1212 = 1f;
                    }
                    num1212 = 1f - num1212;
                    num1212 *= 2f;
                    if (num1212 > 1f)
                    {
                        num1212 = 1f;
                    }
                    npc.localAI[0] = v5.ToRotation();
                    npc.localAI[1] = num1212;
                    npc.localAI[1] = 1f;
                    break;
                }
            case 3:
                {
                    int num1218 = 7;
                    int num1219 = 4;
                    npc.ai[1]++;
                    if (npc.ai[1] >= num1218 * num1219 * 10)
                    {
                        npc.ai[1] = 0f;
                        break;
                    }
                    if (npc.ai[1] >= num1218 * num1219)
                    {
                        npc.localAI[1] -= 0.07f;
                        if (npc.localAI[1] < 0f)
                        {
                            npc.localAI[1] = 0f;
                        }
                        break;
                    }
                    npc.localAI[1] += 0.05f;
                    if (npc.localAI[1] > 0.75f)
                    {
                        npc.localAI[1] = 0.75f;
                    }
                    float num1220 = (float)Math.PI * 2f * (npc.ai[1] % (num1218 * num1219)) / (num1218 * num1219) - (float)Math.PI / 2f;
                    npc.localAI[0] = new Vector2((float)Math.Cos(num1220) * vector170.X, (float)Math.Sin(num1220) * vector170.Y).ToRotation();
                    if (npc.ai[1] % num1219 == 0f)
                    {
                        Vector2 vector207 = new(1f * (0f - num1192), 3f);
                        Vector2 vector208 = Utils.Vector2FromElipse(npc.localAI[0].ToRotationVector2(), vector170 * npc.localAI[1]);
                        Vector2 vector209 = npc.Center + Vector2.Normalize(vector208) * vector170.Length() * 0.4f + vector207;
                        Vector2 vector210 = Vector2.Normalize(vector208) * 8f;
                        float ai2 = ((float)Math.PI * 2f * (float)Main.rand.NextDouble() - (float)Math.PI) / 30f + (float)Math.PI / 180f * num1192;
                        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector209.X, vector209.Y, vector210.X, vector210.Y, 452, 5, 0f, Main.myPlayer, 0f, ai2);
                    }
                    break;
                }
            case 4:
                {
                    Vector2 center20 = Main.npc[(int)npc.ai[3]].Center;
                    Vector2 vector197 = new Vector2(220f * num1192, -60f) + center20;
                    vector197 += new Vector2(num1192 * 100f, -50f);
                    Vector2 vector198 = new(400f * num1192, -60f);
                    npc.ai[1]++;
                    if (npc.ai[1] < 30f)
                    {
                        Vector2 vector199 = vector197 - npc.Center;
                        if (vector199 != Vector2.Zero)
                        {
                            Vector2 vector200 = vector199;
                            vector200.Normalize();
                            npc.velocity = Vector2.SmoothStep(npc.velocity, vector200 * Math.Min(8f, vector199.Length()), 0.2f);
                        }
                    }
                    else if (npc.ai[1] < 210f)
                    {
                        int num1213 = (int)npc.ai[1] - 30;
                        if (num1213 % 30 == 0 && Main.netMode != 1)
                        {
                            Vector2 vector201 = new(5f * num1192, -8f);
                            int num1214 = num1213 / 30;
                            vector201.X += (num1214 - 3.5f) * num1192 * 3f;
                            vector201.Y += (num1214 - 4.5f) * 1f;
                            vector201 *= 1.2f;
                            _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X, npc.Center.Y, vector201.X, vector201.Y, 454, 1, 1f, Main.myPlayer, 0f, npc.whoAmI);
                        }
                        Vector2 vector202 = Vector2.SmoothStep(vector197, vector197 + vector198, (npc.ai[1] - 30f) / 180f) - npc.Center;
                        if (vector202 != Vector2.Zero)
                        {
                            Vector2 vector203 = vector202;
                            vector203.Normalize();
                            npc.velocity = Vector2.Lerp(npc.velocity, vector203 * Math.Min(4f, vector202.Length()), 0.1f);
                        }
                    }
                    else if (npc.ai[1] < 270f)
                    {
                        npc.velocity *= 0.9f;
                    }
                    else if (npc.ai[1] < 300f)
                    {
                        if (npc.ai[1] == 270f && Main.netMode != 1)
                        {
                            int num1216 = Player.FindClosest(npc.position, npc.width, npc.height);
                            Vector2 vector204 = Vector2.Normalize(Main.player[num1216].Center - (npc.Center + Vector2.UnitY * -350f));
                            if (float.IsNaN(vector204.X) || float.IsNaN(vector204.Y))
                            {
                                vector204 = Vector2.UnitY;
                            }
                            vector204 *= 12f;
                            for (int num1217 = 0; num1217 < 1000; num1217++)
                            {
                                Projectile projectile4 = Main.projectile[num1217];
                                if (projectile4.active && projectile4.type == 454 && projectile4.ai[1] == npc.whoAmI && projectile4.ai[0] != -1f)
                                {
                                    projectile4.ai[0] = -1f;
                                    projectile4.velocity = vector204;
                                    projectile4.netUpdate = true;
                                }
                            }
                        }
                        Vector2 vector205 = Vector2.SmoothStep(vector197, vector197 + vector198, 1f - (npc.ai[1] - 270f) / 30f) - npc.Center;
                        if (vector205 != Vector2.Zero)
                        {
                            Vector2 vector206 = vector205;
                            vector206.Normalize();
                            npc.velocity = Vector2.Lerp(npc.velocity, vector206 * Math.Min(14f, vector205.Length()), 0.1f);
                        }
                    }
                    else
                    {
                        npc.ai[1] = 0f;
                    }
                    break;
                }
            case 5:
                npc.dontTakeDamage = true;
                npc.ai[1]++;
                if (npc.ai[1] >= 40f)
                {
                    npc.ai[1] = 0f;
                }
                break;
        }
    }
}
