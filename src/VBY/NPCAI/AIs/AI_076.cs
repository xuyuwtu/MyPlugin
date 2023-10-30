namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_076(this NPC npc)
    {
        if (npc.localAI[3] == 0f && Main.netMode != 1 && npc.type == 395)
        {
            npc.localAI[3] = 1f;
            int[] array4 = new int[4];
            int num1131 = 0;
            for (int num1132 = 0; num1132 < 2; num1132++)
            {
                int num1133 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X + num1132 * 300 - 150, (int)npc.Center.Y, 393, npc.whoAmI);
                Main.npc[num1133].ai[1] = num1132;
                Main.npc[num1133].netUpdate = true;
                array4[num1131++] = num1133;
            }
            for (int num1134 = 0; num1134 < 2; num1134++)
            {
                int num1135 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X + num1134 * 300 - 150, (int)npc.Center.Y, 394, npc.whoAmI);
                Main.npc[num1135].ai[1] = num1134;
                Main.npc[num1135].netUpdate = true;
                array4[num1131++] = num1135;
            }
            int num1136 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X, (int)npc.Center.Y, 392, npc.whoAmI);
            Main.npc[num1136].ai[0] = npc.whoAmI;
            Main.npc[num1136].netUpdate = true;
            for (int num1137 = 0; num1137 < 4; num1137++)
            {
                Main.npc[array4[num1137]].ai[0] = npc.whoAmI;
            }
            for (int num1138 = 0; num1138 < 4; num1138++)
            {
                Main.npc[num1136].localAI[num1138] = array4[num1138];
            }
        }
        if (npc.ai[0] == 3f)
        {
            npc.StrikeNPCNoInteraction(9999, 0f, 0);
            return;
        }
        Vector2 center14 = npc.Center;
        Player player8 = Main.player[npc.target];
        float num1139 = 5600f;
        if (npc.target < 0 || npc.target == 255 || player8.dead || !player8.active || Vector2.Distance(player8.Center, center14) > num1139)
        {
            npc.TargetClosest();
            player8 = Main.player[npc.target];
            npc.netUpdate = true;
        }
        if ((player8.dead || !player8.active || Vector2.Distance(player8.Center, center14) > num1139) && npc.ai[0] != 1f)
        {
            if (npc.ai[0] == 0f)
            {
                npc.ai[0] = -1f;
            }
            if (npc.ai[0] == 2f)
            {
                npc.ai[0] = -2f;
            }
            npc.netUpdate = true;
        }
        if (npc.ai[0] == -1f || npc.ai[0] == -2f)
        {
            npc.velocity.Y -= 0.4f;
            npc.EncourageDespawn(10);
            if (!player8.dead)
            {
                npc.timeLeft = 300;
                if (npc.ai[0] == -2f)
                {
                    npc.ai[0] = 2f;
                }
                if (npc.ai[0] == 0f)
                {
                    npc.ai[0] = 0f;
                }
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 0f)
        {
            int num1140 = 0;
            if (npc.ai[3] >= 580f)
            {
                num1140 = 0;
            }
            else if (npc.ai[3] >= 440f)
            {
                num1140 = 5;
            }
            else if (npc.ai[3] >= 420f)
            {
                num1140 = 4;
            }
            else if (npc.ai[3] >= 280f)
            {
                num1140 = 3;
            }
            else if (npc.ai[3] >= 260f)
            {
                num1140 = 2;
            }
            else if (npc.ai[3] >= 20f)
            {
                num1140 = 1;
            }
            npc.ai[3]++;
            if (npc.ai[3] >= 600f)
            {
                npc.ai[3] = 0f;
            }
            int num1141 = num1140;
            if (npc.ai[3] >= 580f)
            {
                num1140 = 0;
            }
            else if (npc.ai[3] >= 440f)
            {
                num1140 = 5;
            }
            else if (npc.ai[3] >= 420f)
            {
                num1140 = 4;
            }
            else if (npc.ai[3] >= 280f)
            {
                num1140 = 3;
            }
            else if (npc.ai[3] >= 260f)
            {
                num1140 = 2;
            }
            else if (npc.ai[3] >= 20f)
            {
                num1140 = 1;
            }
            if (num1140 != num1141)
            {
                if (num1140 == 0)
                {
                    npc.ai[2] = 0f;
                }
                if (num1140 == 1)
                {
                    npc.ai[2] = ((Math.Sign((player8.Center - center14).X) == 1) ? 1 : (-1));
                }
                if (num1140 == 2)
                {
                    npc.ai[2] = 0f;
                }
                npc.netUpdate = true;
            }
            if (num1140 == 0)
            {
                if (npc.ai[2] == 0f)
                {
                    npc.ai[2] = -600 * Math.Sign((center14 - player8.Center).X);
                }
                Vector2 vector158 = player8.Center + new Vector2(npc.ai[2], -250f) - center14;
                if (vector158.Length() < 50f)
                {
                    npc.ai[3] = 19f;
                }
                else
                {
                    vector158.Normalize();
                    npc.velocity = Vector2.Lerp(npc.velocity, vector158 * 16f, 0.1f);
                }
            }
            if (num1140 == 1)
            {
                int x17 = (int)npc.Center.X / 16;
                int num1142 = (int)(npc.position.Y + npc.height) / 16;
                int num1143 = 0;
                if (Main.tile[x17, num1142].nactive() && Main.tileSolid[Main.tile[x17, num1142].type] && !Main.tileSolidTop[Main.tile[x17, num1142].type])
                {
                    num1143 = 1;
                }
                else
                {
                    for (; num1143 < 150 && num1142 + num1143 < Main.maxTilesY; num1143++)
                    {
                        int y9 = num1142 + num1143;
                        if (Main.tile[x17, y9].nactive() && Main.tileSolid[Main.tile[x17, y9].type] && !Main.tileSolidTop[Main.tile[x17, y9].type])
                        {
                            num1143--;
                            break;
                        }
                    }
                }
                float num1144 = num1143 * 16;
                float num1145 = 250f;
                if (num1144 < num1145)
                {
                    float num1146 = -4f;
                    if (0f - num1146 > num1144)
                    {
                        num1146 = 0f - num1144;
                    }
                    npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, num1146, 0.05f);
                }
                else
                {
                    npc.velocity.Y *= 0.95f;
                }
                npc.velocity.X = 3.5f * npc.ai[2];
            }
            switch (num1140)
            {
                case 2:
                    {
                        if (npc.ai[2] == 0f)
                        {
                            npc.ai[2] = 300 * Math.Sign((center14 - player8.Center).X);
                        }
                        Vector2 vector159 = player8.Center + new Vector2(npc.ai[2], -170f) - center14;
                        int x19 = (int)npc.Center.X / 16;
                        int num1153 = (int)(npc.position.Y + npc.height) / 16;
                        int num1154 = 0;
                        if (Main.tile[x19, num1153].nactive() && Main.tileSolid[Main.tile[x19, num1153].type] && !Main.tileSolidTop[Main.tile[x19, num1153].type])
                        {
                            num1154 = 1;
                        }
                        else
                        {
                            for (; num1154 < 150 && num1153 + num1154 < Main.maxTilesY; num1154++)
                            {
                                int y11 = num1153 + num1154;
                                if (Main.tile[x19, y11].nactive() && Main.tileSolid[Main.tile[x19, y11].type] && !Main.tileSolidTop[Main.tile[x19, y11].type])
                                {
                                    num1154--;
                                    break;
                                }
                            }
                        }
                        float num1155 = num1154 * 16;
                        float num1156 = 170f;
                        if (num1155 < num1156)
                        {
                            vector159.Y -= num1156 - num1155;
                        }
                        if (vector159.Length() < 70f)
                        {
                            npc.ai[3] = 279f;
                            break;
                        }
                        vector159.Normalize();
                        npc.velocity = Vector2.Lerp(npc.velocity, vector159 * 20f, 0.1f);
                        break;
                    }
                case 3:
                    {
                        float num1147 = 0.85f;
                        int x18 = (int)npc.Center.X / 16;
                        int num1148 = (int)(npc.position.Y + npc.height) / 16;
                        int num1149 = 0;
                        if (Main.tile[x18, num1148].nactive() && Main.tileSolid[Main.tile[x18, num1148].type] && !Main.tileSolidTop[Main.tile[x18, num1148].type])
                        {
                            num1149 = 1;
                        }
                        else
                        {
                            for (; num1149 < 150 && num1148 + num1149 < Main.maxTilesY; num1149++)
                            {
                                int y10 = num1148 + num1149;
                                if (Main.tile[x18, y10].nactive() && Main.tileSolid[Main.tile[x18, y10].type] && !Main.tileSolidTop[Main.tile[x18, y10].type])
                                {
                                    num1149--;
                                    break;
                                }
                            }
                        }
                        float num1150 = num1149 * 16;
                        float num1151 = 170f;
                        if (num1150 < num1151)
                        {
                            float num1152 = -4f;
                            if (0f - num1152 > num1150)
                            {
                                num1152 = 0f - num1150;
                            }
                            npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, num1152, 0.05f);
                        }
                        else
                        {
                            npc.velocity.Y *= num1147;
                        }
                        npc.velocity.X *= num1147;
                        break;
                    }
            }
            switch (num1140)
            {
                case 4:
                    {
                        Vector2 vector160 = player8.Center + new Vector2(0f, -250f) - center14;
                        if (vector160.Length() < 50f)
                        {
                            npc.ai[3] = 439f;
                            break;
                        }
                        vector160.Normalize();
                        npc.velocity = Vector2.Lerp(npc.velocity, vector160 * 16f, 0.1f);
                        break;
                    }
                case 5:
                    npc.velocity *= 0.85f;
                    break;
            }
        }
        else if (npc.ai[0] == 1f)
        {
            npc.dontTakeDamage = false;
            npc.velocity *= 0.96f;
            float num1157 = 150f;
            npc.ai[1]++;
            if (npc.ai[1] >= num1157)
            {
                npc.ai[0] = 2f;
                npc.ai[1] = 0f;
                npc.rotation = 0f;
                npc.netUpdate = true;
            }
            else if (npc.ai[1] < 40f)
            {
                npc.rotation = Vector2.UnitY.RotatedBy(npc.ai[1] / 40f * ((float)Math.PI * 2f)).Y * 0.2f;
            }
            else if (npc.ai[1] < 80f)
            {
                npc.rotation = Vector2.UnitY.RotatedBy(npc.ai[1] / 20f * ((float)Math.PI * 2f)).Y * 0.3f;
            }
            else if (npc.ai[1] < 120f)
            {
                npc.rotation = Vector2.UnitY.RotatedBy(npc.ai[1] / 10f * ((float)Math.PI * 2f)).Y * 0.4f;
            }
            else
            {
                npc.rotation = (npc.ai[1] - 120f) / 30f * ((float)Math.PI * 2f);
            }
        }
        else if (npc.ai[0] == 2f)
        {
            int num1158 = 80;
            float num1159 = 3600f;
            float num1160 = 120f;
            float num1161 = 60f;
            int num1162 = 0;
            if (npc.ai[3] % num1160 >= num1161)
            {
                num1162 = 1;
            }
            int num1163 = num1162;
            num1162 = 0;
            npc.ai[3]++;
            if (npc.ai[3] % num1160 >= num1161)
            {
                num1162 = 1;
            }
            if (num1162 != num1163)
            {
                if (num1162 == 1)
                {
                    npc.ai[2] = ((Math.Sign((player8.Center - center14).X) == 1) ? 1 : (-1));
                    if (Main.netMode != 1)
                    {
                        Vector2 center15 = npc.Center;
                        Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), center15.X, center15.Y, 0f, 0f, 447, num1158, 0f, Main.myPlayer, npc.whoAmI + 1);
                    }
                    SoundEngine.PlaySound(SoundID.Item12, npc.Center);
                }
                npc.netUpdate = true;
            }
            if (npc.ai[3] >= num1159)
            {
                npc.ai[0] = 2f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
            else if (num1162 == 0)
            {
                Vector2 vector161 = player8.Center + new Vector2(npc.ai[2] * 350f, -250f) - center14;
                vector161.Normalize();
                npc.velocity = Vector2.Lerp(npc.velocity, vector161 * 16f, 0.1f);
            }
            else
            {
                int x20 = (int)npc.Center.X / 16;
                int num1164 = (int)(npc.position.Y + npc.height) / 16;
                int num1165 = 0;
                if (Main.tile[x20, num1164].nactive() && Main.tileSolid[Main.tile[x20, num1164].type] && !Main.tileSolidTop[Main.tile[x20, num1164].type])
                {
                    num1165 = 1;
                }
                else
                {
                    for (; num1165 < 150 && num1164 + num1165 < Main.maxTilesY; num1165++)
                    {
                        int y12 = num1164 + num1165;
                        if (Main.tile[x20, y12].nactive() && Main.tileSolid[Main.tile[x20, y12].type] && !Main.tileSolidTop[Main.tile[x20, y12].type])
                        {
                            num1165--;
                            break;
                        }
                    }
                }
                float num1166 = num1165 * 16;
                float num1167 = 250f;
                if (num1166 < num1167)
                {
                    float num1168 = -4f;
                    if (0f - num1168 > num1166)
                    {
                        num1168 = 0f - num1166;
                    }
                    npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, num1168, 0.05f);
                }
                else
                {
                    npc.velocity.Y *= 0.95f;
                }
                npc.velocity.X = 8f * npc.ai[2];
            }
            npc.rotation = 0f;
        }
        bool flag72 = false;
        if (npc.position.Y < -100f)
        {
            flag72 = true;
        }
        if (npc.position.X < -100f)
        {
            flag72 = true;
        }
        if (npc.position.Y > Main.maxTilesY * 16 + 100)
        {
            flag72 = true;
        }
        if (npc.position.X > Main.maxTilesX * 16 + 100)
        {
            flag72 = true;
        }
        if (flag72)
        {
            npc.position = Vector2.Clamp(npc.position, new Vector2(-100f), new Vector2(100f) + new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f);
            npc.active = false;
            npc.netUpdate = true;
        }
    }
}
