namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_064(this NPC npc)
    {
        float num997 = npc.ai[0];
        float num998 = npc.ai[1];
        if (Main.netMode != 1)
        {
            npc.localAI[0] -= 1f;
            if (npc.ai[3] == 0f)
            {
                npc.ai[3] = Main.rand.Next(75, 111) * 0.01f;
            }
            if (npc.localAI[0] <= 0f)
            {
                npc.TargetClosest();
                npc.localAI[0] = Main.rand.Next(60, 180);
                float num999 = Math.Abs(npc.Center.X - Main.player[npc.target].Center.X);
                if (num999 > 700f && npc.localAI[3] == 0f)
                {
                    float num1000 = Main.rand.Next(50, 151) * 0.01f;
                    if (num999 > 1000f)
                    {
                        num1000 = Main.rand.Next(150, 201) * 0.01f;
                    }
                    else if (num999 > 850f)
                    {
                        num1000 = Main.rand.Next(100, 151) * 0.01f;
                    }
                    int num1001 = npc.direction * Main.rand.Next(100, 251);
                    int num1002 = Main.rand.Next(-50, 51);
                    if (npc.position.Y > Main.player[npc.target].position.Y - 100f)
                    {
                        num1002 -= Main.rand.Next(100, 251);
                    }
                    float num1003 = num1000 / (float)Math.Sqrt(num1001 * num1001 + num1002 * num1002);
                    num997 = num1001 * num1003;
                    num998 = num1002 * num1003;
                }
                else
                {
                    npc.localAI[3] = 1f;
                    float num1004 = Main.rand.Next(5, 151) * 0.01f;
                    int num1005 = Main.rand.Next(-100, 101);
                    int num1006 = Main.rand.Next(-100, 101);
                    float num1007 = num1004 / (float)Math.Sqrt(num1005 * num1005 + num1006 * num1006);
                    num997 = num1005 * num1007;
                    num998 = num1006 * num1007;
                }
                npc.netUpdate = true;
            }
        }
        npc.scale = npc.ai[3];
        if (npc.type == 677)
        {
            bool flag50 = true;
            Point point6 = npc.Center.ToTileCoordinates();
            int num1008 = 40;
            if (point6.X < num1008)
            {
                num997 += 0.5f;
                if (num997 > 3f)
                {
                    num997 = 3f;
                }
                flag50 = false;
            }
            else if (point6.X > Main.maxTilesX - num1008)
            {
                num997 -= 0.5f;
                if (num997 < -3f)
                {
                    num997 = -3f;
                }
                flag50 = false;
            }
            if (point6.Y < num1008)
            {
                num998 += 0.5f;
                if (num998 > 3f)
                {
                    npc.velocity.Y = 3f;
                }
                flag50 = false;
            }
            else if (point6.Y > Main.maxTilesY - num1008)
            {
                num998 -= 0.5f;
                if (num998 < -3f)
                {
                    num998 = -3f;
                }
                flag50 = false;
            }
            if (npc.localAI[1] > 0f)
            {
                npc.localAI[1]--;
            }
            else if (flag50)
            {
                npc.localAI[1] = 15f;
                float num1009 = 0f;
                Vector2 zero = Vector2.Zero;
                for (int num1010 = 0; num1010 < 200; num1010++)
                {
                    NPC nPC4 = Main.npc[num1010];
                    if (nPC4.active && nPC4.damage > 0 && !nPC4.friendly && nPC4.Hitbox.Distance(npc.Center) <= 100f)
                    {
                        num1009++;
                        zero += npc.DirectionFrom(nPC4.Center);
                    }
                }
                for (int num1011 = 0; num1011 < 255; num1011++)
                {
                    Player player4 = Main.player[num1011];
                    if (player4.active && player4.Hitbox.Distance(npc.Center) <= 150f)
                    {
                        num1009++;
                        zero += npc.DirectionFrom(player4.Center);
                    }
                }
                if (num1009 > 0f)
                {
                    float num1012 = 2f;
                    zero /= num1009;
                    zero *= num1012;
                    npc.velocity += zero;
                    if (npc.velocity.Length() > 8f)
                    {
                        npc.velocity = npc.velocity.SafeNormalize(Vector2.Zero) * 8f;
                    }

                    _ = npc.Center + zero * 10f;
                    npc.localAI[0] = 10f;
                    npc.netUpdate = true;
                }
            }
        }
        else if (npc.localAI[2] > 0f)
        {
            int i3 = (int)npc.Center.X / 16;
            int j3 = (int)npc.Center.Y / 16;
            if (npc.localAI[2] > 3f)
            {
                if (npc.type == 358)
                {
                    Lighting.AddLight(i3, j3, 0.10124999f * npc.scale, 0.21374999f * npc.scale, 0.225f * npc.scale);
                }
                else if (npc.type == 654)
                {
                    Lighting.AddLight(i3, j3, 0.225f * npc.scale, 0.105000004f * npc.scale, 0.060000002f * npc.scale);
                }
                else
                {
                    Lighting.AddLight(i3, j3, 0.109500006f * npc.scale, 0.15f * npc.scale, 0.0615f * npc.scale);
                }
            }
            npc.localAI[2] -= 1f;
        }
        else if (npc.localAI[1] > 0f)
        {
            npc.localAI[1] -= 1f;
        }
        else
        {
            npc.localAI[1] = Main.rand.Next(30, 180);
            if (!Main.dayTime || (double)(npc.position.Y / 16f) > Main.worldSurface + 10.0)
            {
                npc.localAI[2] = Main.rand.Next(10, 30);
            }
        }
        int num1013 = 80;
        npc.velocity.X = (npc.velocity.X * (num1013 - 1) + num997) / num1013;
        npc.velocity.Y = (npc.velocity.Y * (num1013 - 1) + num998) / num1013;
        if (npc.velocity.Y > 0f)
        {
            int num1014 = 4;
            int x12 = (int)npc.Center.X / 16;
            int num1015 = (int)npc.Center.Y / 16;
            for (int num1016 = num1015; num1016 < num1015 + num1014; num1016++)
            {
                if (WorldGen.InWorld(x12, num1016, 2) && Main.tile[x12, num1016] != null && ((Main.tile[x12, num1016].nactive() && Main.tileSolid[Main.tile[x12, num1016].type]) || Main.tile[x12, num1016].liquid > 0))
                {
                    num998 *= -1f;
                    if (npc.velocity.Y > 0f)
                    {
                        npc.velocity.Y *= 0.9f;
                    }
                }
            }
        }
        if (npc.velocity.Y < 0f)
        {
            int num1017 = 30;
            bool flag51 = false;
            int x13 = (int)npc.Center.X / 16;
            int num1018 = (int)npc.Center.Y / 16;
            for (int num1019 = num1018; num1019 < num1018 + num1017; num1019++)
            {
                if (WorldGen.InWorld(x13, num1019, 2) && Main.tile[x13, num1019] != null && Main.tile[x13, num1019].nactive() && Main.tileSolid[Main.tile[x13, num1019].type])
                {
                    flag51 = true;
                }
            }
            if (!flag51)
            {
                num998 *= -1f;
                if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y *= 0.9f;
                }
            }
        }
        if (npc.collideX)
        {
            num997 = ((!(npc.velocity.X < 0f)) ? (0f - Math.Abs(num997)) : Math.Abs(num997));
            npc.velocity.X *= -0.2f;
        }
        if (npc.type == 677)
        {
            npc.rotation = npc.velocity.X * 0.3f;
        }
        if (npc.velocity.X < 0f)
        {
            npc.direction = -1;
        }
        if (npc.velocity.X > 0f)
        {
            npc.direction = 1;
        }
        npc.ai[0] = num997;
        npc.ai[1] = num998;
    }
}
