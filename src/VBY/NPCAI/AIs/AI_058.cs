namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_058(this NPC npc)
    {
        npc.localAI[0] += 1f;
        if (npc.localAI[0] > 6f)
        {
            npc.localAI[0] = 0f;
            npc.localAI[1] += 1f;
            if (npc.localAI[1] > 4f)
            {
                npc.localAI[1] = 0f;
            }
        }
        if (npc.type == 327)
        {
            Lighting.AddLight(npc.Center, 0.4f, 0.4f, 0.2f);
        }
        if (Main.netMode != 1)
        {
            npc.localAI[2] += 1f;
            if (npc.localAI[2] > 300f)
            {
                npc.ai[3] = Main.rand.Next(3);
                npc.localAI[2] = 0f;
            }
            else if (npc.ai[3] == 0f && npc.localAI[2] % 30f == 0f && npc.localAI[2] > 30f)
            {
                float num900 = 5f;
                Vector2 vector117 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f + 30f);
                if (!WorldGen.SolidTile((int)vector117.X / 16, (int)vector117.Y / 16))
                {
                    float num901 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector117.X;
                    float num902 = Main.player[npc.target].position.Y - vector117.Y;
                    num901 += Main.rand.Next(-50, 51);
                    num902 += Main.rand.Next(50, 201);
                    num902 *= 0.2f;
                    float num903 = (float)Math.Sqrt(num901 * num901 + num902 * num902);
                    num903 = num900 / num903;
                    num901 *= num903;
                    num902 *= num903;
                    num901 *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                    num902 *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                    _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector117.X, vector117.Y, num901, num902, Main.rand.Next(326, 329), 40, 0f, Main.myPlayer);
                }
            }
        }
        if (npc.ai[0] == 0f && Main.netMode != 1)
        {
            npc.TargetClosest();
            npc.ai[0] = 1f;
            int num905 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2), (int)npc.position.Y + npc.height / 2, 328, npc.whoAmI);
            Main.npc[num905].ai[0] = -1f;
            Main.npc[num905].ai[1] = npc.whoAmI;
            Main.npc[num905].target = npc.target;
            Main.npc[num905].netUpdate = true;
            num905 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2), (int)npc.position.Y + npc.height / 2, 328, npc.whoAmI);
            Main.npc[num905].ai[0] = 1f;
            Main.npc[num905].ai[1] = npc.whoAmI;
            Main.npc[num905].ai[3] = 150f;
            Main.npc[num905].target = npc.target;
            Main.npc[num905].netUpdate = true;
        }
        if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 2000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f)
        {
            npc.TargetClosest();
            if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 2000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f)
            {
                npc.ai[1] = 2f;
            }
        }
        if (Main.dayTime)
        {
            npc.velocity.Y += 0.3f;
            npc.velocity.X *= 0.9f;
        }
        else if (npc.ai[1] == 0f)
        {
            npc.ai[2] += 1f;
            if (npc.ai[2] >= 300f)
            {
                if (npc.ai[3] != 1f)
                {
                    npc.ai[1] = 0f;
                    npc.ai[2] = 0f;
                }
                else
                {
                    npc.ai[2] = 0f;
                    npc.ai[1] = 1f;
                    npc.TargetClosest();
                    npc.netUpdate = true;
                }
            }
            Vector2 vector118 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num906 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector118.X;
            float num907 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - 200f - vector118.Y;
            float num908 = (float)Math.Sqrt(num906 * num906 + num907 * num907);
            float num909 = 6f;
            if (npc.ai[3] == 1f)
            {
                if (num908 > 900f)
                {
                    num909 = 12f;
                }
                else if (num908 > 600f)
                {
                    num909 = 10f;
                }
                else if (num908 > 300f)
                {
                    num909 = 8f;
                }
            }
            if (num908 > 50f)
            {
                num908 = num909 / num908;
                npc.velocity.X = (npc.velocity.X * 14f + num906 * num908) / 15f;
                npc.velocity.Y = (npc.velocity.Y * 14f + num907 * num908) / 15f;
            }
        }
        else if (npc.ai[1] == 1f)
        {
            npc.ai[2] += 1f;
            if (npc.ai[2] >= 600f || npc.ai[3] != 1f)
            {
                npc.ai[2] = 0f;
                npc.ai[1] = 0f;
            }
            Vector2 vector119 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num910 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector119.X;
            float num911 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector119.Y;
            float num912 = (float)Math.Sqrt(num910 * num910 + num911 * num911);
            num912 = 16f / num912;
            npc.velocity.X = (npc.velocity.X * 49f + num910 * num912) / 50f;
            npc.velocity.Y = (npc.velocity.Y * 49f + num911 * num912) / 50f;
        }
        else if (npc.ai[1] == 2f)
        {
            npc.ai[1] = 3f;
            npc.velocity.Y += 0.1f;
            if (npc.velocity.Y < 0f)
            {
                npc.velocity.Y *= 0.95f;
            }
            npc.velocity.X *= 0.95f;
            npc.EncourageDespawn(500);
        }
        npc.rotation = npc.velocity.X * -0.02f;
    }
}
