using Terraria.GameContent.Events;

namespace VBY.NPCTest;

partial class NPCAIs
{
    public static void AI_077(this NPC npc)
    {
        if (npc.ai[0] != -1f && npc.ai[0] != 2f && Main.rand.Next(200) == 0)
        {
            SoundEngine.PlaySound(29, (int)npc.Center.X, (int)npc.Center.Y, Main.rand.Next(93, 100));
        }
        if (npc.localAI[3] == 0f)
        {
            npc.netUpdate = true;
            npc.localAI[3] = 1f;
            npc.ai[0] = -1f;
        }
        if (npc.ai[0] == -2f)
        {
            npc.dontTakeDamage = true;
            npc.ai[1]++;
            if (npc.ai[1] == 30f)
            {
                SoundEngine.PlaySound(29, (int)npc.Center.X, (int)npc.Center.Y, 92);
            }
            if (npc.ai[1] < 60f)
            {
                MoonlordDeathDrama.RequestLight(npc.ai[1] / 30f, npc.Center);
            }
            if (npc.ai[1] == 60f)
            {
                npc.ai[1] = 0f;
                npc.ai[0] = 0f;
                if (Main.netMode != 1 && npc.type == 398)
                {
                    npc.ai[2] = Main.rand.Next(3);
                    npc.ai[2] = 0f;
                    npc.netUpdate = true;
                }
            }
        }
        if (npc.ai[0] == -1f)
        {
            npc.dontTakeDamage = true;
            npc.ai[1]++;
            if (npc.ai[1] == 30f)
            {
                SoundEngine.PlaySound(29, (int)npc.Center.X, (int)npc.Center.Y, 92);
            }
            if (npc.ai[1] < 60f)
            {
                MoonlordDeathDrama.RequestLight(npc.ai[1] / 30f, npc.Center);
            }
            if (npc.ai[1] == 60f)
            {
                npc.ai[1] = 0f;
                npc.ai[0] = 0f;
                if (Main.netMode != 1 && npc.type == 398)
                {
                    npc.ai[2] = Main.rand.Next(3);
                    npc.ai[2] = 0f;
                    npc.netUpdate = true;
                    int[] array5 = new int[3];
                    int num1169 = 0;
                    for (int i = 0; i < 2; i++)
                    {
                        int num1171 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X + i * 800 - 400, (int)npc.Center.Y - 100, 397, npc.whoAmI);
                        Main.npc[num1171].ai[2] = i;
                        Main.npc[num1171].netUpdate = true;
                        array5[num1169++] = num1171;
                    }
                    int num1172 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X, (int)npc.Center.Y - 400, 396, npc.whoAmI);
                    Main.npc[num1172].netUpdate = true;
                    array5[num1169++] = num1172;
                    for (int i = 0; i < 3; i++)
                    {
                        Main.npc[array5[i]].ai[3] = npc.whoAmI;
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        npc.localAI[i] = array5[i];
                    }
                }
            }
        }
        if (npc.ai[0] == 0f)
        {
            npc.dontTakeDamage = true;
            npc.TargetClosest(faceTarget: false);
            Vector2 vector162 = Main.player[npc.target].Center - npc.Center + new Vector2(0f, 130f);
            if (vector162.Length() > 20f)
            {
                Vector2 desiredVelocity = Vector2.Normalize(vector162 - npc.velocity) * 8f;
                Vector2 value2 = npc.velocity;
                npc.SimpleFlyMovement(desiredVelocity, 0.5f);
                npc.velocity = Vector2.Lerp(npc.velocity, value2, 0.5f);
            }
            if (Main.netMode != 1)
            {
                bool flag73 = false;
                if (npc.localAI[0] < 0f || npc.localAI[1] < 0f || npc.localAI[2] < 0f)
                {
                    flag73 = true;
                }
                else if (!Main.npc[(int)npc.localAI[0]].active || Main.npc[(int)npc.localAI[0]].type != 397)
                {
                    flag73 = true;
                }
                else if (!Main.npc[(int)npc.localAI[1]].active || Main.npc[(int)npc.localAI[1]].type != 397)
                {
                    flag73 = true;
                }
                else if (!Main.npc[(int)npc.localAI[2]].active || Main.npc[(int)npc.localAI[2]].type != 396)
                {
                    flag73 = true;
                }
                if (flag73)
                {
                    npc.life = 0;
                    npc.HitEffect();
                    npc.active = false;
                }
                bool flag74 = true;
                if (Main.npc[(int)npc.localAI[0]].ai[0] != -2f)
                {
                    flag74 = false;
                }
                if (Main.npc[(int)npc.localAI[1]].ai[0] != -2f)
                {
                    flag74 = false;
                }
                if (Main.npc[(int)npc.localAI[2]].ai[0] != -2f)
                {
                    flag74 = false;
                }
                if (flag74)
                {
                    npc.ai[0] = 1f;
                    npc.dontTakeDamage = false;
                    npc.netUpdate = true;
                }
            }
        }
        else if (npc.ai[0] == 1f)
        {
            npc.dontTakeDamage = false;
            npc.TargetClosest(faceTarget: false);
            Vector2 vector163 = Main.player[npc.target].Center - npc.Center + new Vector2(0f, 130f);
            if (vector163.Length() > 20f)
            {
                Vector2 desiredVelocity2 = Vector2.Normalize(vector163 - npc.velocity) * 8f;
                Vector2 value3 = npc.velocity;
                npc.SimpleFlyMovement(desiredVelocity2, 0.5f);
                npc.velocity = Vector2.Lerp(npc.velocity, value3, 0.5f);
            }
        }
        else if (npc.ai[0] == 2f)
        {
            npc.dontTakeDamage = true;
            npc.velocity = Vector2.Lerp(value2: new Vector2(npc.direction, -0.5f), value1: npc.velocity, amount: 0.98f);
            npc.ai[1]++;
            if (npc.ai[1] < 60f)
            {
                MoonlordDeathDrama.RequestLight(npc.ai[1] / 60f, npc.Center);
            }
            if (npc.ai[1] == 60f)
            {
                for (int num1175 = 0; num1175 < 1000; num1175++)
                {
                    Projectile projectile = Main.projectile[num1175];
                    if (projectile.active && (projectile.type == 456 || projectile.type == 462 || projectile.type == 455 || projectile.type == 452 || projectile.type == 454))
                    {
                        projectile.Kill();
                    }
                }
                for (int num1176 = 0; num1176 < 200; num1176++)
                {
                    NPC nPC6 = Main.npc[num1176];
                    if (nPC6.active && nPC6.type == 400)
                    {
                        nPC6.HitEffect(0, 9999.0);
                        nPC6.active = false;
                    }
                }
            }
            if (npc.ai[1] % 3f == 0f && npc.ai[1] < 580f && npc.ai[1] > 60f)
            {
                Vector2 vector164 = Utils.RandomVector2(Main.rand, -1f, 1f);
                if (vector164 != Vector2.Zero)
                {
                    vector164.Normalize();
                }
                vector164 *= 20f + Main.rand.NextFloat() * 400f;
                bool flag75 = true;
                Vector2 vector165 = npc.Center + vector164;
                Point point7 = vector165.ToTileCoordinates();
                if (!WorldGen.InWorld(point7.X, point7.Y))
                {
                    flag75 = false;
                }
                if (flag75 && WorldGen.SolidTile(point7.X, point7.Y))
                {
                    flag75 = false;
                }
                float num1177 = Main.rand.Next(6, 19);
                float num1178 = (float)Math.PI * 2f / num1177;
                float num1179 = (float)Math.PI * 2f * Main.rand.NextFloat();
                float num1180 = 1f + Main.rand.NextFloat() * 2f;
                float num1181 = 1f + Main.rand.NextFloat();
                float fadeIn = 0.4f + Main.rand.NextFloat();
                int num1182 = Utils.SelectRandom<int>(Main.rand, 31, 229);
                if (flag75 && !Main.dedServ)
                {
                    MoonlordDeathDrama.AddExplosion(vector165);
                    for (float num1183 = 0f; num1183 < num1177 * 2f; num1183++)
                    {
                        Dust dust6 = Main.dust[Dust.NewDust(vector165, 0, 0, 229)];
                        dust6.noGravity = true;
                        dust6.position = vector165;
                        dust6.velocity = Vector2.UnitY.RotatedBy(num1179 + num1178 * num1183) * num1180 * (Main.rand.NextFloat() * 1.6f + 1.6f);
                        dust6.fadeIn = fadeIn;
                        dust6.scale = num1181;
                    }
                }
                for (float num1184 = 0f; num1184 < npc.ai[1] / 60f; num1184++)
                {
                    Vector2 vector166 = Utils.RandomVector2(Main.rand, -1f, 1f);
                    if (vector166 != Vector2.Zero)
                    {
                        vector166.Normalize();
                    }
                    vector166 *= 20f + Main.rand.NextFloat() * 800f;
                    Vector2 vec = npc.Center + vector166;
                    Point point8 = vec.ToTileCoordinates();
                    bool flag76 = true;
                    if (!WorldGen.InWorld(point8.X, point8.Y))
                    {
                        flag76 = false;
                    }
                    if (flag76 && WorldGen.SolidTile(point8.X, point8.Y))
                    {
                        flag76 = false;
                    }
                    if (flag76)
                    {
                        Dust dust7 = Main.dust[Dust.NewDust(vec, 0, 0, num1182)];
                        dust7.noGravity = true;
                        dust7.position = vec;
                        dust7.velocity = -Vector2.UnitY * num1180 * (Main.rand.NextFloat() * 0.9f + 1.6f);
                        dust7.fadeIn = fadeIn;
                        dust7.scale = num1181;
                    }
                }
            }
            if (npc.ai[1] % 15f == 0f && npc.ai[1] < 480f && npc.ai[1] >= 90f && Main.netMode != 1)
            {
                Vector2 vector167 = Utils.RandomVector2(Main.rand, -1f, 1f);
                if (vector167 != Vector2.Zero)
                {
                    vector167.Normalize();
                }
                vector167 *= 20f + Main.rand.NextFloat() * 400f;
                bool flag77 = true;
                Vector2 vec2 = npc.Center + vector167;
                Point point9 = vec2.ToTileCoordinates();
                if (!WorldGen.InWorld(point9.X, point9.Y))
                {
                    flag77 = false;
                }
                if (flag77 && WorldGen.SolidTile(point9.X, point9.Y))
                {
                    flag77 = false;
                }
                if (flag77)
                {
                    float num1185 = (Main.rand.Next(4) < 2).ToDirectionInt() * ((float)Math.PI / 8f + (float)Math.PI / 4f * Main.rand.NextFloat());
                    Vector2 vector168 = new Vector2(0f, (0f - Main.rand.NextFloat()) * 0.5f - 0.5f).RotatedBy(num1185) * 6f;
                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vec2.X, vec2.Y, vector168.X, vector168.Y, 622, 0, 0f, Main.myPlayer);
                }
            }
            if (npc.ai[1] == 1f)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath61, npc.Center);
            }
            if (npc.ai[1] >= 480f)
            {
                MoonlordDeathDrama.RequestLight((npc.ai[1] - 480f) / 120f, npc.Center);
            }
            if (npc.ai[1] >= 600f)
            {
                npc.life = 0;
                npc.HitEffect(0, 1337.0);
                npc.checkDead();
                return;
            }
        }
        else if (npc.ai[0] == 3f)
        {
            npc.dontTakeDamage = true;
            npc.velocity = Vector2.Lerp(value2: new Vector2(npc.direction, -0.5f), value1: npc.velocity, amount: 0.98f);
            npc.ai[1]++;
            if (npc.ai[1] < 60f)
            {
                MoonlordDeathDrama.RequestLight(npc.ai[1] / 40f, npc.Center);
            }
            if (npc.ai[1] == 40f)
            {
                for (int num1186 = 0; num1186 < 1000; num1186++)
                {
                    Projectile projectile2 = Main.projectile[num1186];
                    if (projectile2.active && (projectile2.type == 456 || projectile2.type == 462 || projectile2.type == 455 || projectile2.type == 452 || projectile2.type == 454))
                    {
                        projectile2.active = false;
                        if (Main.netMode != 1)
                        {
                            NetMessage.SendData(27, -1, -1, null, num1186);
                        }
                    }
                }
                for (int num1187 = 0; num1187 < 200; num1187++)
                {
                    NPC nPC7 = Main.npc[num1187];
                    if (nPC7.active && nPC7.type == 400)
                    {
                        nPC7.active = false;
                        if (Main.netMode != 1)
                        {
                            NetMessage.SendData(23, -1, -1, null, nPC7.whoAmI);
                        }
                    }
                }
                for (int num1188 = 0; num1188 < 600; num1188++)
                {
                    Gore gore2 = Main.gore[num1188];
                    if (gore2.active && gore2.type >= 619 && gore2.type <= 622)
                    {
                        gore2.active = false;
                    }
                }
            }
            if (npc.ai[1] >= 60f)
            {
                for (int num1189 = 0; num1189 < 200; num1189++)
                {
                    NPC nPC8 = Main.npc[num1189];
                    if (nPC8.active && (nPC8.type == 400 || nPC8.type == 397 || nPC8.type == 396))
                    {
                        nPC8.active = false;
                        if (Main.netMode != 1)
                        {
                            NetMessage.SendData(23, -1, -1, null, nPC8.whoAmI);
                        }
                    }
                }
                npc.active = false;
                if (Main.netMode != 1)
                {
                    NetMessage.SendData(23, -1, -1, null, npc.whoAmI);
                }
                NPC.LunarApocalypseIsUp = false;
                if (Main.netMode == 2)
                {
                    NetMessage.SendData(7);
                }
                return;
            }
        }
        bool flag78 = false;
        if (npc.ai[0] == -2f || npc.ai[0] == -1f || npc.ai[0] == 2f || npc.ai[0] == 3f)
        {
            flag78 = true;
        }
        if (Main.player[npc.target].active && !Main.player[npc.target].dead)
        {
            flag78 = true;
        }
        if (!flag78)
        {
            for (int num1190 = 0; num1190 < 255; num1190++)
            {
                if (Main.player[num1190].active && !Main.player[num1190].dead)
                {
                    flag78 = true;
                    break;
                }
            }
        }
        if (!flag78)
        {
            npc.ai[0] = 3f;
            npc.ai[1] = 0f;
            npc.netUpdate = true;
        }
        if (!(npc.ai[0] >= 0f) || !(npc.ai[0] < 2f) || Main.netMode == 1 || !(npc.Distance(Main.player[npc.target].Center) > 2400f))
        {
            return;
        }
        npc.ai[0] = -2f;
        npc.netUpdate = true;
        Vector2 vector169 = Main.player[npc.target].Center - Vector2.UnitY * 150f - npc.Center;
        npc.position += vector169;
        if (Main.npc[(int)npc.localAI[0]].active)
        {
            NPC nPC3 = Main.npc[(int)npc.localAI[0]];
            nPC3.position += vector169;
            Main.npc[(int)npc.localAI[0]].netUpdate = true;
        }
        if (Main.npc[(int)npc.localAI[1]].active)
        {
            NPC nPC3 = Main.npc[(int)npc.localAI[1]];
            nPC3.position += vector169;
            Main.npc[(int)npc.localAI[1]].netUpdate = true;
        }
        if (Main.npc[(int)npc.localAI[2]].active)
        {
            NPC nPC3 = Main.npc[(int)npc.localAI[2]];
            nPC3.position += vector169;
            Main.npc[(int)npc.localAI[2]].netUpdate = true;
        }
        for (int num1191 = 0; num1191 < 200; num1191++)
        {
            NPC nPC9 = Main.npc[num1191];
            if (nPC9.active && nPC9.type == 400)
            {
                NPC nPC3 = nPC9;
                nPC3.position += vector169;
                nPC9.netUpdate = true;
            }
        }
    }
}
