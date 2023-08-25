namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_075(this NPC npc)
    {
        int num1089 = -1;
        Vector2 vector144 = Vector2.Zero;
        int num1090 = 0;
        if (npc.type == 390)
        {
            if (npc.localAI[0] == 0f && Main.netMode != 1)
            {
                npc.localAI[0] = 1f;
                int num1091 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X, (int)npc.Center.Y, 391, npc.whoAmI);
                npc.ai[0] = num1091;
                npc.netUpdate = true;
            }
            int num1092 = (int)npc.ai[0];
            if (Main.npc[num1092].active && Main.npc[num1092].type == 391)
            {
                npc.DiscourageDespawn(60);
                num1089 = num1092;
                vector144 = Vector2.UnitY * -14f;
            }
        }
        if (npc.type == 416)
        {
            if (npc.localAI[0] == 0f && Main.netMode != 1)
            {
                npc.localAI[0] = 1f;
                int num1093 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X, (int)npc.Center.Y, 415, npc.whoAmI);
                npc.ai[0] = num1093;
                npc.netUpdate = true;
            }
            int num1094 = (int)npc.ai[0];
            if (Main.npc[num1094].active && Main.npc[num1094].type == 415)
            {
                npc.DiscourageDespawn(60);
                num1089 = num1094;
                vector144 = new Vector2(-Main.npc[num1094].spriteDirection * 10, -30f);
            }
        }
        else if (npc.type == 392)
        {
            int num1095 = (int)npc.ai[0];
            if (Main.npc[num1095].active && Main.npc[num1095].type == 395)
            {
                npc.DiscourageDespawn(60);
                num1089 = num1095;
                vector144 = Vector2.UnitY * 2f;
                vector144 *= Main.npc[num1095].scale;
                float num1096 = Main.npc[num1095].rotation;
                vector144 = vector144.RotatedBy(num1096);
                npc.rotation = num1096;
                if (Main.netMode != 1)
                {
                    bool flag63 = true;
                    if (Main.npc[num1095].ai[0] >= 1f || Main.npc[num1095].ai[0] < 0f)
                    {
                        flag63 = false;
                    }
                    if (flag63)
                    {
                        for (int num1097 = 0; num1097 < 2; num1097++)
                        {
                            if (Main.npc[(int)npc.localAI[num1097]].active && Main.npc[(int)npc.localAI[num1097]].type == 393)
                            {
                                flag63 = false;
                            }
                        }
                        for (int num1098 = 2; num1098 < 4; num1098++)
                        {
                            if (Main.npc[(int)npc.localAI[num1098]].active && Main.npc[(int)npc.localAI[num1098]].type == 394)
                            {
                                flag63 = false;
                            }
                        }
                    }
                    if (Main.npc[num1095].ai[3] % 200f == 0f && Main.npc[num1095].ai[0] != 1f)
                    {
                        for (int num1099 = 0; num1099 < 2; num1099++)
                        {
                            if (Main.npc[(int)npc.localAI[num1099]].active && Main.npc[(int)npc.localAI[num1099]].type == 393)
                            {
                                Main.npc[(int)npc.localAI[num1099]].netUpdate = true;
                            }
                        }
                        for (int num1100 = 2; num1100 < 4; num1100++)
                        {
                            if (Main.npc[(int)npc.localAI[num1100]].active && Main.npc[(int)npc.localAI[num1100]].type == 394)
                            {
                                Main.npc[(int)npc.localAI[num1100]].netUpdate = true;
                            }
                        }
                        npc.netUpdate = true;
                    }
                    if (flag63)
                    {
                        if (!Main.expertMode)
                        {
                            Main.npc[num1095].ai[0] = 3f;
                            Main.npc[num1095].ai[1] = 0f;
                            Main.npc[num1095].ai[2] = 0f;
                            Main.npc[num1095].ai[3] = 0f;
                            Main.npc[num1095].netUpdate = true;
                        }
                        else
                        {
                            Main.npc[num1095].ai[0] = 1f;
                            Main.npc[num1095].ai[1] = 0f;
                            Main.npc[num1095].ai[2] = 0f;
                            Main.npc[num1095].ai[3] = 0f;
                            Main.npc[num1095].netUpdate = true;
                        }
                    }
                }
            }
        }
        else if (npc.type == 393)
        {
            int num1101 = (int)npc.ai[0];
            if (Main.npc[num1101].active && Main.npc[num1101].type == 395)
            {
                npc.DiscourageDespawn(60);
                num1089 = num1101;
                vector144 = Vector2.UnitY * 29f + ((npc.ai[1] == 1f) ? Vector2.UnitX : (-Vector2.UnitX)) * 60f;
                vector144 *= Main.npc[num1101].scale;
                float num1102 = Main.npc[num1101].rotation;
                vector144 = vector144.RotatedBy(num1102);
                npc.rotation = num1102;
            }
        }
        else if (npc.type == 394)
        {
            int num1103 = (int)npc.ai[0];
            if (Main.npc[num1103].active && Main.npc[num1103].type == 395)
            {
                npc.DiscourageDespawn(60);
                num1089 = num1103;
                vector144 = Vector2.UnitY * -13f + ((npc.ai[1] == 1f) ? Vector2.UnitX : (-Vector2.UnitX)) * 49f;
                vector144 *= Main.npc[num1103].scale;
                float num1104 = Main.npc[num1103].rotation;
                vector144 = vector144.RotatedBy(num1104);
                npc.rotation = num1104;
                num1090 = ((npc.ai[1] == 1f) ? 1 : (-1));
            }
        }
        else if (npc.type == 492)
        {
            int num1105 = (int)npc.ai[0];
            if (Main.npc[num1105].active && Main.npc[num1105].type == 491)
            {
                npc.DiscourageDespawn(60);
                num1089 = num1105;
                vector144 = new Vector2((-122f + 68f * npc.ai[1]) * ((Main.npc[num1105].spriteDirection != 1) ? 1 : (-1)), -6f);
                vector144 *= Main.npc[num1105].scale;
                float num1106 = Main.npc[num1105].rotation;
                vector144 = vector144.RotatedBy(num1106);
                npc.rotation = num1106;
            }
        }
        if (num1089 != -1)
        {
            NPC nPC5 = Main.npc[num1089];
            npc.velocity = Vector2.Zero;
            npc.position = nPC5.Center;
            npc.position.X -= npc.width / 2;
            npc.position.Y -= npc.height / 2;
            npc.position += vector144;
            npc.gfxOffY = nPC5.gfxOffY;
            npc.direction = nPC5.direction;
            if (num1090 == 0)
            {
                npc.spriteDirection = nPC5.spriteDirection;
            }
            else
            {
                npc.spriteDirection = num1090;
            }
            if (npc.type == 390)
            {
                npc.timeLeft = nPC5.timeLeft;
                npc.velocity = nPC5.velocity;
                npc.target = nPC5.target;
                if (npc.ai[1] < 60f)
                {
                    npc.ai[1]++;
                }
                if (npc.justHit)
                {
                    npc.ai[1] = -30f;
                }
                int num1107 = 438;
                int num1108 = 30;
                float num1109 = 7f;
                if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    Vector2 vector145 = Main.player[npc.target].Center - npc.Center;
                    Vector2 vector146 = Vector2.Normalize(vector145);
                    float num1110 = vector145.Length();
                    float num1111 = 700f;
                    if (npc.type == 214)
                    {
                        num1111 = 550f;
                    }
                    if (npc.type == 215)
                    {
                        num1111 = 800f;
                    }
                    if (num1110 < num1111)
                    {
                        if (npc.ai[1] == 60f && Math.Sign(vector145.X) == npc.direction)
                        {
                            npc.ai[1] = -60f;
                            Vector2 center10 = Main.player[npc.target].Center;
                            Vector2 vector147 = npc.Center - Vector2.UnitY * 4f;
                            Vector2 vector148 = center10 - vector147;
                            vector148.X += Main.rand.Next(-50, 51);
                            vector148.Y += Main.rand.Next(-50, 51);
                            vector148.X *= Main.rand.Next(80, 121) * 0.01f;
                            vector148.Y *= Main.rand.Next(80, 121) * 0.01f;
                            vector148.Normalize();
                            if (float.IsNaN(vector148.X) || float.IsNaN(vector148.Y))
                            {
                                vector148 = -Vector2.UnitY;
                            }
                            vector148 *= num1109;
                            Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector147.X, vector147.Y, vector148.X, vector148.Y, num1107, num1108, 0f, Main.myPlayer);
                            npc.netUpdate = true;
                        }
                        else
                        {
                            float num1112 = npc.ai[2];
                            npc.velocity.X *= 0.5f;
                            npc.ai[2] = 3f;
                            if (Math.Abs(vector146.Y) > Math.Abs(vector146.X) * 2f)
                            {
                                if (vector146.Y > 0f)
                                {
                                    npc.ai[2] = 1f;
                                }
                                else
                                {
                                    npc.ai[2] = 5f;
                                }
                            }
                            else if (Math.Abs(vector146.X) > Math.Abs(vector146.Y) * 2f)
                            {
                                npc.ai[2] = 3f;
                            }
                            else if (vector146.Y > 0f)
                            {
                                npc.ai[2] = 2f;
                            }
                            else
                            {
                                npc.ai[2] = 4f;
                            }
                            if (npc.ai[2] != num1112)
                            {
                                npc.netUpdate = true;
                            }
                        }
                    }
                }
            }
            if (npc.type == 492)
            {
                npc.timeLeft = nPC5.timeLeft;
                npc.velocity = nPC5.velocity;
                if (npc.ai[3] < 240f)
                {
                    npc.ai[3]++;
                }
                if (npc.ai[3] == 2f)
                {
                    npc.TargetClosest(faceTarget: false);
                }
                if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    Vector2 vector149 = Main.player[npc.target].Center - npc.Center;
                    _ = Vector2.Normalize(vector149);
                    if (npc.ai[3] >= 240f)
                    {
                        npc.ai[3] = 0f;
                        Vector2 center11 = Main.player[npc.target].Center;
                        Vector2 center12 = npc.Center;
                        Vector2 vector151 = Vector2.Normalize(center11 - center12);
                        if (float.IsNaN(vector151.X) || float.IsNaN(vector151.Y))
                        {
                            vector151 = Vector2.UnitY;
                        }
                        vector151 *= 14f;
                        vector151 += Vector2.UnitY * -5f;
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), center12.X, center12.Y, vector151.X, vector151.Y, 240, 30, 0f, Main.myPlayer);
                        }
                        npc.netUpdate = true;
                    }
                    else
                    {
                        float num1113 = npc.ai[2];
                        float[] array3 = new float[8];
                        for (int num1114 = 0; num1114 < array3.Length; num1114++)
                        {
                            array3[num1114] = Vector2.Distance(npc.Center + Vector2.UnitY.RotatedBy(num1114 * (-(float)Math.PI / 4f)) * 50f, Main.player[npc.target].Center);
                        }
                        int num1115 = 0;
                        for (int num1116 = 1; num1116 < array3.Length; num1116++)
                        {
                            if (array3[num1115] > array3[num1116])
                            {
                                num1115 = num1116;
                            }
                        }
                        npc.ai[2] = num1115 + 1;
                        if (npc.spriteDirection == 1)
                        {
                            npc.ai[2] = 9f - npc.ai[2];
                        }
                        if (npc.ai[2] != num1113)
                        {
                            npc.netUpdate = true;
                        }
                    }
                }
                else
                {
                    if (npc.ai[2] != 0f)
                    {
                        npc.netUpdate = true;
                    }
                    npc.ai[2] = 0f;
                }
            }
            if (npc.type == 394)
            {
                npc.timeLeft = nPC5.timeLeft;
                int attackDamage_ForProjectiles9 = npc.GetAttackDamage_ForProjectiles(50f, 37f);
                npc.ai[3] = nPC5.ai[3];
                float num1117 = 440f;
                float num1118 = 140f;
                if (npc.ai[3] >= num1117 && npc.ai[3] < num1117 + num1118)
                {
                    float num1119 = npc.ai[3] - num1117;
                    if (num1119 % 20f == 0f)
                    {
                        if (Main.netMode != 1)
                        {
                            Vector2 spinningpoint3 = num1090 * Vector2.UnitX;
                            spinningpoint3 = spinningpoint3.RotatedBy((Main.rand.NextDouble() - 0.5) * 0.7853981852531433);
                            spinningpoint3 *= 8f;
                            Vector2 vector152 = num1090 * Vector2.UnitX * 36f + npc.Center + Vector2.UnitY * 8f;
                            Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector152.X, vector152.Y, spinningpoint3.X, spinningpoint3.Y, 448, attackDamage_ForProjectiles9, 0f, Main.myPlayer, 0f, 20f);
                        }
                        SoundEngine.PlaySound(SoundID.Item39, npc.Center);
                    }
                }
            }
            if (npc.type == 393)
            {
                npc.timeLeft = nPC5.timeLeft;
                int attackDamage_ForProjectiles10 = npc.GetAttackDamage_ForProjectiles(35f, 30f);
                npc.ai[3] = nPC5.ai[3];
                float num1120 = 280f;
                float num1121 = 140f;
                bool flag64 = npc.ai[3] >= num1120 && npc.ai[3] < num1120 + num1121;
                if (!flag64)
                {
                    npc.TargetClosest(faceTarget: false);
                    Player player7 = Main.player[npc.target];
                    Vector2 v3 = player7.Center - npc.Center;
                    if (v3.Y < 0f)
                    {
                        v3.Y = 0f;
                    }
                    v3.Normalize();
                    if (float.IsNaN(v3.X) || float.IsNaN(v3.Y))
                    {
                        v3 = Vector2.UnitY;
                    }
                    npc.ai[2] = v3.ToRotation();
                }
                if (flag64)
                {
                    float num1122 = npc.ai[3] - num1120;
                    if (num1122 % 6f == 0f)
                    {
                        if (Main.netMode != 1)
                        {
                            Vector2 spinningpoint4 = npc.ai[2].ToRotationVector2();
                            spinningpoint4 = spinningpoint4.RotatedBy((Main.rand.NextDouble() - 0.5) * 0.7853981852531433 / 3.0);
                            spinningpoint4 *= 16f;
                            Vector2 vector153 = npc.Center + spinningpoint4 * 1f;
                            Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector153.X, vector153.Y, spinningpoint4.X, spinningpoint4.Y, 449, attackDamage_ForProjectiles10, 0f, Main.myPlayer);
                        }
                        SoundEngine.PlaySound(SoundID.Item12, npc.Center);
                    }
                }
            }
            if (npc.type != 392)
            {
                return;
            }
            npc.timeLeft = nPC5.timeLeft;
            int attackDamage_ForProjectiles11 = npc.GetAttackDamage_ForProjectiles(50f, 50f);
            npc.ai[3] = nPC5.ai[3];
            float num1123 = 20f;
            float num1124 = 240f;
            if (npc.ai[3] >= num1123 && npc.ai[3] < num1123 + num1124 && nPC5.ai[0] == 0f)
            {
                float num1125 = npc.ai[3] - num1123;
                if (num1125 == 0f)
                {
                    if (Main.netMode != 1)
                    {
                        Vector2 center13 = npc.Center;
                        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), center13.X, center13.Y, 0f, 0f, 447, attackDamage_ForProjectiles11, 0f, Main.myPlayer, npc.whoAmI + 1);
                    }
                    SoundEngine.PlaySound(SoundID.Item12, npc.Center);
                }
            }
            bool flag65 = false;
            int maxValue4 = 1000;
            int maxValue5 = 1000;
            int num1126 = 450;
            int attackDamage_ForProjectiles12 = npc.GetAttackDamage_ForProjectiles(30f, 25f);
            if (nPC5.ai[0] == 2f)
            {
                flag65 = true;
                maxValue5 = 120;
                maxValue4 = 120;
            }
            if (!flag65)
            {
                num1123 = 280f;
                num1124 = 120f;
                flag65 = flag65 || (npc.ai[3] >= num1123 && npc.ai[3] < num1123 + num1124);
                if (flag65)
                {
                    maxValue5 = 90;
                    maxValue4 = 60;
                }
            }
            if (!flag65)
            {
                num1123 = 440f;
                num1124 = 140f;
                flag65 = flag65 || (npc.ai[3] >= num1123 && npc.ai[3] < num1123 + num1124);
                if (flag65)
                {
                    maxValue5 = 60;
                    maxValue4 = 90;
                }
            }
            bool flag66 = true;
            bool flag67 = true;
            bool flag68 = true;
            bool flag69 = true;
            if (Main.npc[(int)npc.localAI[0]].active && Main.npc[(int)npc.localAI[0]].type == 393)
            {
                flag66 = false;
            }
            if (Main.npc[(int)npc.localAI[1]].active && Main.npc[(int)npc.localAI[1]].type == 393)
            {
                flag67 = false;
            }
            if (Main.npc[(int)npc.localAI[2]].active && Main.npc[(int)npc.localAI[2]].type == 394)
            {
                flag68 = false;
            }
            if (Main.npc[(int)npc.localAI[3]].active && Main.npc[(int)npc.localAI[3]].type == 394)
            {
                flag69 = false;
            }
            if (flag65)
            {
                if (flag66 && Main.rand.Next(maxValue4) == 0)
                {
                    if (Main.netMode != 1)
                    {
                        Vector2 spinningpoint5 = new(-1f * (float)Main.rand.NextDouble() * 3f, 1f);
                        spinningpoint5 = spinningpoint5.RotatedBy((Main.rand.NextDouble() - 0.5) * 0.7853981852531433);
                        spinningpoint5 *= 3f;
                        Vector2 vector154 = -1f * Vector2.UnitX * Main.rand.Next(50, 70) + npc.Center + Vector2.UnitY * Main.rand.Next(30, 45);
                        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector154.X, vector154.Y, spinningpoint5.X, spinningpoint5.Y, num1126, attackDamage_ForProjectiles12, 0f, Main.myPlayer);
                    }
                    SoundEngine.PlaySound(SoundID.Item39, npc.Center);
                }
                if (flag67 && Main.rand.Next(maxValue4) == 0)
                {
                    if (Main.netMode != 1)
                    {
                        Vector2 spinningpoint6 = new(1f * (float)Main.rand.NextDouble() * 3f, 1f);
                        spinningpoint6 = spinningpoint6.RotatedBy((Main.rand.NextDouble() - 0.5) * 0.7853981852531433);
                        spinningpoint6 *= 3f;
                        Vector2 vector155 = 1f * Vector2.UnitX * Main.rand.Next(50, 70) + npc.Center + Vector2.UnitY * Main.rand.Next(30, 45);
                        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector155.X, vector155.Y, spinningpoint6.X, spinningpoint6.Y, num1126, attackDamage_ForProjectiles12, 0f, Main.myPlayer);
                    }
                    SoundEngine.PlaySound(SoundID.Item39, npc.Center);
                }
            }
            if (flag65)
            {
                if (flag68 && Main.rand.Next(maxValue5) == 0)
                {
                    if (Main.netMode != 1)
                    {
                        Vector2 spinningpoint7 = new(-1f * (float)Main.rand.NextDouble() * 2f, -1f);
                        spinningpoint7 = spinningpoint7.RotatedBy((Main.rand.NextDouble() - 0.5) * 0.7853981852531433);
                        spinningpoint7 *= 3f;
                        Vector2 vector156 = -1f * Vector2.UnitX * Main.rand.Next(30, 60) + npc.Center + Vector2.UnitY * Main.rand.Next(-30, -10);
                        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector156.X, vector156.Y, spinningpoint7.X, spinningpoint7.Y, num1126, attackDamage_ForProjectiles12, 0f, Main.myPlayer);
                    }
                    SoundEngine.PlaySound(SoundID.Item39, npc.Center);
                }
                if (flag69 && Main.rand.Next(maxValue5) == 0)
                {
                    if (Main.netMode != 1)
                    {
                        Vector2 spinningpoint8 = new(1f * (float)Main.rand.NextDouble() * 2f, -1f);
                        spinningpoint8 = spinningpoint8.RotatedBy((Main.rand.NextDouble() - 0.5) * 0.7853981852531433);
                        spinningpoint8 *= 3f;
                        Vector2 vector157 = 1f * Vector2.UnitX * Main.rand.Next(30, 60) + npc.Center + Vector2.UnitY * Main.rand.Next(-30, -10);
                        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector157.X, vector157.Y, spinningpoint8.X, spinningpoint8.Y, num1126, attackDamage_ForProjectiles12, 0f, Main.myPlayer);
                    }
                    SoundEngine.PlaySound(SoundID.Item39, npc.Center);
                }
            }
            if (flag66 && Main.rand.Next(8) == 0)
            {
                int num1127 = Dust.NewDust(-1f * Vector2.UnitX * Main.rand.Next(50, 70) + npc.Center + Vector2.UnitY * Main.rand.Next(15, 30), 4, 16, (Main.rand.Next(4) != 0) ? 31 : 228, 0f, 0f, 100, default, 1.2f);
                Main.dust[num1127].velocity = new Vector2(-1f * (float)Main.rand.NextDouble() * 3f, 1f).RotatedBy((Main.rand.NextDouble() - 0.5) * 0.7853981852531433);
                Dust dust = Main.dust[num1127];
                dust.velocity *= 0.5f;
                Main.dust[num1127].velocity.Y = 0f - Math.Abs(Main.dust[num1127].velocity.Y);
            }
            if (flag67 && Main.rand.Next(8) == 0)
            {
                int num1128 = Dust.NewDust(Vector2.UnitX * Main.rand.Next(50, 70) + npc.Center + Vector2.UnitY * Main.rand.Next(15, 30), 4, 16, (Main.rand.Next(4) != 0) ? 31 : 228, 0f, 0f, 100, default, 1.2f);
                Main.dust[num1128].velocity = new Vector2((float)Main.rand.NextDouble() * 3f, 1f).RotatedBy((Main.rand.NextDouble() - 0.5) * 0.7853981852531433);
                Dust dust = Main.dust[num1128];
                dust.velocity *= 0.5f;
                Main.dust[num1128].velocity.Y = 0f - Math.Abs(Main.dust[num1128].velocity.Y);
            }
            if (flag68 && Main.rand.Next(8) == 0)
            {
                int num1129 = Dust.NewDust(-1f * Vector2.UnitX * Main.rand.Next(30, 60) + npc.Center + Vector2.UnitY * Main.rand.Next(-30, -10), 4, 16, (Main.rand.Next(4) != 0) ? 31 : 228, 0f, 0f, 100, default, 1.2f);
                Main.dust[num1129].velocity = new Vector2(-1f * (float)Main.rand.NextDouble() * 2f, 1f).RotatedBy((Main.rand.NextDouble() - 0.5) * 0.7853981852531433);
                Dust dust = Main.dust[num1129];
                dust.velocity *= 0.5f;
                Main.dust[num1129].velocity.Y = 0f - Math.Abs(Main.dust[num1129].velocity.Y);
            }
            if (flag69 && Main.rand.Next(8) == 0)
            {
                int num1130 = Dust.NewDust(Vector2.UnitX * Main.rand.Next(30, 60) + npc.Center + Vector2.UnitY * Main.rand.Next(-30, -10), 4, 16, (Main.rand.Next(4) != 0) ? 31 : 228, 0f, 0f, 100, default, 1.2f);
                Main.dust[num1130].velocity = new Vector2((float)Main.rand.NextDouble() * 2f, 1f).RotatedBy((Main.rand.NextDouble() - 0.5) * 0.7853981852531433);
                Dust dust = Main.dust[num1130];
                dust.velocity *= 0.5f;
                Main.dust[num1130].velocity.Y = 0f - Math.Abs(Main.dust[num1130].velocity.Y);
            }
        }
        else if (npc.type == 390)
        {
            npc.Transform(382);
        }
        else if (npc.type == 416)
        {
            npc.Transform(518);
        }
        else
        {
            npc.life = 0;
            npc.HitEffect();
            npc.active = false;
        }
    }
}
