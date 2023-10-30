namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_093(this NPC npc)
    {
        if (Main.netMode != 1 && npc.localAI[0] == 0f)
        {
            npc.localAI[0] = 1f;
            for (int num1424 = 0; num1424 < 4; num1424++)
            {
                int num1425 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)npc.Center.X + num1424 * 40 - 150, (int)npc.Center.Y, 492, npc.whoAmI, npc.whoAmI, num1424, 0f, 60 * num1424);
                Main.npc[num1425].TargetClosest(faceTarget: false);
                Main.npc[num1425].timeLeft = 600;
                Main.npc[num1425].netUpdate = true;
                npc.ai[num1424] = num1425;
            }
            npc.netUpdate = true;
        }
        bool flag87 = true;
        for (int num1426 = 0; num1426 < 4; num1426++)
        {
            if (npc.ai[num1426] >= 0f && (!Main.npc[(int)npc.ai[num1426]].active || Main.npc[(int)npc.ai[num1426]].type != 492))
            {
                npc.ai[num1426] = -1f;
                npc.netUpdate = true;
            }
            else if (npc.ai[num1426] >= 0f)
            {
                flag87 = false;
            }
        }
        if (flag87)
        {
            npc.StrikeNPCNoInteraction(9999, 0f, 0);
            return;
        }
        if (Main.netMode != 1 && Main.rand.Next(300) == 0)
        {
            Vector2 vector281 = new Vector2((Main.rand.NextFloat() - 0.5f) * (npc.width - 70), (Main.rand.NextFloat() - 0.5f) * 20f - npc.height / 2 - 20f).RotatedBy(npc.rotation);
            vector281 += npc.Center;
            int num1427 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)vector281.X, (int)vector281.Y, Utils.SelectRandom<int>(Main.rand, 213, 215, 214, 212));
            Main.npc[num1427].velocity = new Vector2((Main.rand.NextFloat() - 0.5f) * 5f, -8.01f) + npc.velocity;
            Main.npc[num1427].netUpdate = true;
            Main.npc[num1427].timeLeft = 600;
        }
        if ((npc.localAI[3] += 1f) >= 64f)
        {
            npc.localAI[3] = 0f;
        }
        npc.TargetClosest();
        int x24 = (int)npc.Center.X / 16 + Math.Sign(npc.velocity.X) * 10;
        int num1428 = (int)(npc.position.Y + npc.height) / 16;
        int num1429 = 0;
        if (Main.tile[x24, num1428].nactive() && Main.tileSolid[Main.tile[x24, num1428].type] && !Main.tileSolidTop[Main.tile[x24, num1428].type])
        {
            num1429 = 1;
        }
        else
        {
            for (; num1429 < 150 && num1428 + num1429 < Main.maxTilesY; num1429++)
            {
                int y16 = num1428 + num1429;
                if (Main.tile[x24, y16].nactive() && Main.tileSolid[Main.tile[x24, y16].type] && !Main.tileSolidTop[Main.tile[x24, y16].type])
                {
                    num1429--;
                    break;
                }
            }
        }
        float num1430 = num1429 * 16;
        if (num1430 < 350f)
        {
            float num1431 = num1430 - 350f;
            if (num1431 < -4f)
            {
                num1431 = -4f;
            }
            npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, num1431, 0.05f);
        }
        else if (num1430 > 450f)
        {
            float num1432 = num1430 - 350f;
            if (num1432 > 4f)
            {
                num1432 = 4f;
            }
            npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, num1432, 0.05f);
        }
        else
        {
            npc.velocity.Y *= 0.95f;
        }
        float num1433 = Main.player[npc.target].Center.X - npc.Center.X;
        if (Math.Abs(num1433) >= 300f && (Math.Abs(npc.velocity.X) < 6f || Math.Sign(npc.velocity.X) != npc.direction))
        {
            npc.velocity.X += npc.direction * 0.06f;
        }
        npc.rotation = npc.velocity.X * 0.025f;
        npc.spriteDirection = -Math.Sign(npc.velocity.X);
        for (int num1434 = 0; num1434 < 2; num1434++)
        {
            if (Main.rand.Next(2) != 0)
            {
                Vector2 vector282 = new Vector2((Main.rand.NextFloat() - 0.5f) * (npc.width - 70), (Main.rand.NextFloat() - 0.5f) * 20f + npc.height / 2 + 10f).RotatedBy(npc.rotation);
                Dust dust8 = Main.dust[Dust.NewDust(npc.Center, 0, 0, 228)];
                dust8.position = npc.Center + vector282;
                dust8.velocity = Vector2.Zero;
                dust8.noGravity = true;
                dust8.noLight = true;
                dust8.fadeIn = 1.5f;
                dust8.scale = 0.5f;
            }
        }
    }
}
