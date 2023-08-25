namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_082(this NPC npc)
    {
        float num1282 = 90f;
        Vector2 vector240 = new(0f, 216f);
        int num1283 = (int)Math.Abs(npc.ai[0]) - 1;
        int num1284 = (int)npc.ai[1];
        if (!Main.npc[num1283].active || Main.npc[num1283].type != 396)
        {
            npc.life = 0;
            npc.HitEffect();
            npc.active = false;
            return;
        }
        npc.ai[2]++;
        if (npc.ai[2] >= num1282)
        {
            if (Main.netMode != 1)
            {
                int num1285 = (int)Main.npc[num1283].ai[3];
                int num1286 = -1;
                int num1287 = -1;
                int num1288 = num1283;
                for (int num1289 = 0; num1289 < 200; num1289++)
                {
                    if (Main.npc[num1289].active && Main.npc[num1289].ai[3] == num1285)
                    {
                        if (num1286 == -1 && Main.npc[num1289].type == 397 && Main.npc[num1289].ai[2] == 0f)
                        {
                            num1286 = num1289;
                        }
                        if (num1287 == -1 && Main.npc[num1289].type == 397 && Main.npc[num1289].ai[2] == 1f)
                        {
                            num1287 = num1289;
                        }
                        if (num1286 != -1 && num1287 != -1 && num1288 != -1)
                        {
                            break;
                        }
                    }
                }
                int num1290 = 1000;
                int num1291 = Main.npc[num1285].lifeMax - Main.npc[num1285].life;
                int num1292 = Main.npc[num1286].lifeMax - Main.npc[num1286].life;
                int num1293 = Main.npc[num1287].lifeMax - Main.npc[num1287].life;
                int num1294 = Main.npc[num1288].lifeMax - Main.npc[num1288].life;
                if (num1294 > 0 && num1290 > 0)
                {
                    int num1295 = num1294 - num1290;
                    if (num1295 > 0)
                    {
                        num1295 = 0;
                    }
                    int num1296 = num1290 + num1295;
                    num1290 -= num1296;
                    NPC nPC3 = Main.npc[num1288];
                    nPC3.life += num1296;
                    NPC.HealEffect(Utils.CenteredRectangle(Main.npc[num1288].Center, new Vector2(50f)), num1296);
                }
                if (num1291 > 0 && num1290 > 0)
                {
                    int num1297 = num1291 - num1290;
                    if (num1297 > 0)
                    {
                        num1297 = 0;
                    }
                    int num1298 = num1290 + num1297;
                    num1290 -= num1298;
                    NPC nPC3 = Main.npc[num1285];
                    nPC3.life += num1298;
                    NPC.HealEffect(Utils.CenteredRectangle(Main.npc[num1285].Center, new Vector2(50f)), num1298);
                }
                if (num1292 > 0 && num1290 > 0)
                {
                    int num1299 = num1292 - num1290;
                    if (num1299 > 0)
                    {
                        num1299 = 0;
                    }
                    int num1300 = num1290 + num1299;
                    num1290 -= num1300;
                    NPC nPC3 = Main.npc[num1286];
                    nPC3.life += num1300;
                    NPC.HealEffect(Utils.CenteredRectangle(Main.npc[num1286].Center, new Vector2(50f)), num1300);
                }
                if (num1293 > 0 && num1290 > 0)
                {
                    int num1301 = num1293 - num1290;
                    if (num1301 > 0)
                    {
                        num1301 = 0;
                    }
                    int num1302 = num1290 + num1301;
                    NPC nPC3 = Main.npc[num1287];
                    nPC3.life += num1302;
                    NPC.HealEffect(Utils.CenteredRectangle(Main.npc[num1287].Center, new Vector2(50f)), num1302);
                }
            }
            npc.life = 0;
            npc.HitEffect();
            npc.active = false;
        }
        else
        {
            npc.velocity = Vector2.Zero;
            npc.Center = Vector2.Lerp(Main.projectile[num1284].Center, Main.npc[(int)Math.Abs(npc.ai[0]) - 1].Center + vector240, npc.ai[2] / num1282);
            Vector2 spinningpoint12 = Vector2.UnitY * -npc.height / 2f;
            for (int num1303 = 0; num1303 < 6; num1303++)
            {
                int num1304 = Dust.NewDust(npc.Center - Vector2.One * 4f + spinningpoint12.RotatedBy(num1303 * ((float)Math.PI * 2f) / 6f), 0, 0, 229);
                Main.dust[num1304].velocity = -Vector2.UnitY;
                Main.dust[num1304].noGravity = true;
                Main.dust[num1304].scale = 0.7f;
                Main.dust[num1304].customData = npc;
            }
            spinningpoint12 = Vector2.UnitY * -npc.height / 6f;
            for (int num1305 = 0; num1305 < 3; num1305++)
            {
                int num1306 = Dust.NewDust(npc.Center - Vector2.One * 4f + spinningpoint12.RotatedBy(num1305 * ((float)Math.PI * 2f) / 6f), 0, 0, 229, 0f, -2f);
                Main.dust[num1306].noGravity = true;
                Main.dust[num1306].scale = 1.5f;
                Main.dust[num1306].customData = npc;
            }
        }
    }
}
