using Terraria.GameContent.Events;
using Terraria.Graphics.Effects;

namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_105(this NPC npc)
    {
        if (npc.alpha == 0)
        {
            Lighting.AddLight(npc.Center, 1.3f, 0.5f, 1.5f);
        }
        if (npc.ai[1] == 0f)
        {
            if (npc.ai[0] > 0f)
            {
                npc.ai[0]--;
            }
            if (npc.ai[0] != 0f)
            {
                return;
            }
            npc.ai[0] = 180f;
            npc.netUpdate = true;
            if (npc.localAI[0] == 0f)
            {
                StrayMethods.CheckArenaScore(npc.Bottom, out var xLeftEnd, out var xRightEnd);
                npc.localAI[0] = 1f;
                xLeftEnd.X += 2;
                xRightEnd.X -= 2;
                int num1556 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), xLeftEnd.X, xLeftEnd.Y, 549);
                Main.npc[num1556].Bottom = xLeftEnd.ToWorldCoordinates(8f, 16f);
                num1556 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), xRightEnd.X, xRightEnd.Y, 549);
                Main.npc[num1556].Bottom = xRightEnd.ToWorldCoordinates(8f, 16f);
                if (Main.netMode != 1)
                {
                    DD2Event.FindArenaHitbox();
                }
            }
        }
        else if (npc.ai[1] == 2f)
        {
            npc.dontTakeDamageFromHostiles = true;
            npc.life = npc.lifeMax;
            if (npc.ai[0] == 3f)
            {
                SoundEngine.PlayTrackedSound(SoundID.DD2_WinScene, npc.Center);
                for (int num1557 = 0; num1557 < 200; num1557++)
                {
                    NPC nPC11 = Main.npc[num1557];
                    if (nPC11.active && nPC11.type == 549)
                    {
                        nPC11.ai[1] = 1f;
                        nPC11.ai[0] = 0f;
                        nPC11.netUpdate = true;
                    }
                }
                if (Main.netMode != 1)
                {
                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center - Vector2.UnitY * 40f, Vector2.Zero, 713, 0, 0f, Main.myPlayer);
                }
            }
            npc.ai[0]++;
            npc.noGravity = true;
            if (npc.ai[0] <= 120f)
            {
                float num1558 = npc.ai[0] / 120f;
                npc.velocity.Y = (float)Math.Cos(num1558 * ((float)Math.PI * 2f)) * 0.25f - 0.25f;
            }
            else
            {
                npc.velocity.Y = 0f;
            }
            float lerpValue = Utils.GetLerpValue(480f, 570f, npc.ai[0], clamped: true);
            if (lerpValue != 0f)
            {
                MoonlordDeathDrama.RequestLight(lerpValue, npc.Center);
            }
            if (npc.ai[0] >= 600f)
            {
                DD2Event.StopInvasion(win: true);
                npc.dontTakeDamage = false;
                npc.life = 0;
                npc.checkDead();
                npc.netUpdate = true;
            }
            if (true)
            {
                Vector2 vector310 = npc.Center + new Vector2(0f, -20f);
                float num1559 = 0.99f;
                if (npc.ai[0] >= 60f)
                {
                    num1559 = 0.79f;
                }
                if (npc.ai[0] >= 120f)
                {
                    num1559 = 0.58f;
                }
                if (npc.ai[0] >= 180f)
                {
                    num1559 = 0.43f;
                }
                if (npc.ai[0] >= 240f)
                {
                    num1559 = 0.33f;
                }
                if (npc.ai[0] >= 540f)
                {
                    num1559 = 1f;
                }
                for (int num1560 = 0; num1560 < 9; num1560++)
                {
                    if (!(Main.rand.NextFloat() < num1559))
                    {
                        float num1561 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
                        float num1562 = Main.rand.NextFloat();
                        Vector2 vector311 = vector310 + num1561.ToRotationVector2() * (110f + 600f * num1562);
                        Vector2 vector312 = (num1561 - (float)Math.PI).ToRotationVector2() * (14f + 0f * Main.rand.NextFloat() + 8f * num1562);
                        Dust dust26 = Dust.NewDustPerfect(vector311, 264, vector312);
                        dust26.scale = 0.9f;
                        dust26.fadeIn = 1.15f + num1562 * 0.3f;
                        dust26.color = new Color(1f, 1f, 1f, num1559) * (1f - num1559);
                        dust26.noGravity = true;
                        dust26.noLight = true;
                    }
                }
            }
            if (npc.ai[0] == 100f || npc.ai[0] == 160f || npc.ai[0] == 220f || npc.ai[0] == 280f || npc.ai[0] == 340f || npc.ai[0] == 370f || npc.ai[0] == 400f || npc.ai[0] == 430f || npc.ai[0] == 460f || npc.ai[0] == 500f || npc.ai[0] == 520f || npc.ai[0] == 540f)
            {
                _ = Main.rand.NextFloat() * ((float)Math.PI * 2f);
                float num1564 = 120f;
                for (int num1565 = 0; num1565 < num1564; num1565++)
                {
                    float num1566 = num1565 / num1564 * ((float)Math.PI * 2f);
                    float num1567 = Main.rand.NextFloat();
                    Vector2 vector313 = npc.Center + new Vector2(0f, -20f) + num1566.ToRotationVector2() * (810f - npc.ai[0]);
                    Vector2 vector314 = (num1566 - (float)Math.PI).ToRotationVector2() * (14f + 5f * (npc.ai[0] / 600f) + 8f * num1567);
                    Dust dust27 = Dust.NewDustPerfect(vector313, 264, vector314);
                    dust27.scale = 0.9f;
                    dust27.fadeIn = 1.15f + num1567 * 0.3f;
                    dust27.color = new Color(1f, 1f, 1f, 0f);
                    dust27.noGravity = true;
                    dust27.noLight = true;
                }
            }
        }
        else
        {
            if (npc.ai[1] != 1f)
            {
                return;
            }
            npc.dontTakeDamageFromHostiles = true;
            npc.life = npc.lifeMax;
            if (npc.ai[0] == 0f)
            {
                for (int num1568 = 0; num1568 < 200; num1568++)
                {
                    NPC nPC12 = Main.npc[num1568];
                    if (nPC12.active && nPC12.type == 549)
                    {
                        nPC12.ai[1] = 1f;
                        nPC12.ai[0] = 0f;
                        nPC12.netUpdate = true;
                    }
                }
                if (Main.netMode != 1)
                {
                    DD2Event.ReportLoss();
                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center - Vector2.UnitY * 40f, Vector2.Zero, 672, 0, 0f, Main.myPlayer);
                }
            }
            npc.ai[0]++;
            float lerpValue2 = Utils.GetLerpValue(480f, 600f, npc.ai[0], clamped: true);
            if (lerpValue2 != 0f)
            {
                MoonlordDeathDrama.RequestLight(lerpValue2, npc.Center);
            }
            float num1569 = 96f;
            if (npc.ai[0] < num1569)
            {
                npc.velocity.Y = MathHelper.Lerp(0f, -1f, npc.ai[0] / num1569);
            }
            if (npc.ai[0] >= num1569)
            {
                npc.alpha += 50;
                if (npc.alpha > 255)
                {
                    npc.alpha = 255;
                }
            }
            if (true)
            {
                Vector2 vector315 = npc.Center + new Vector2(0f, MathHelper.Lerp(0f, -70f, Utils.GetLerpValue(0f, 300f, npc.ai[0], clamped: true)));
                float num1570 = 0.99f;
                if (npc.ai[0] >= 60f)
                {
                    num1570 = 0.79f;
                }
                if (npc.ai[0] >= 120f)
                {
                    num1570 = 0.58f;
                }
                if (npc.ai[0] >= 180f)
                {
                    num1570 = 0.23f;
                }
                if (npc.ai[0] >= 240f)
                {
                    num1570 = 0.35f;
                }
                if (npc.ai[0] >= 300f)
                {
                    num1570 = 0.6f;
                }
                if (npc.ai[0] >= 360f)
                {
                    num1570 = 0.98f;
                }
                if (npc.ai[0] >= 420f)
                {
                    num1570 = 0.995f;
                }
                if (npc.ai[0] >= 450f)
                {
                    num1570 = 1f;
                }
                for (int num1571 = 0; num1571 < 12; num1571++)
                {
                    if (!(Main.rand.NextFloat() < num1570))
                    {
                        float num1572 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
                        float num1573 = Main.rand.NextFloat();
                        Vector2 vector316 = vector315 + num1572.ToRotationVector2() * (70f + 600f * num1573);
                        Vector2 vector317 = (num1572 - (float)Math.PI / 2f - (float)Math.PI / 8f).ToRotationVector2() * (12f + 9f * Main.rand.NextFloat() + 4f * num1573);
                        Dust dust28 = Dust.NewDustPerfect(vector316, 240, vector317);
                        dust28.scale = 0.8f;
                        dust28.fadeIn = 0.95f + num1573 * 0.3f;
                        dust28.noGravity = true;
                    }
                }
            }
            if (npc.ai[0] >= 600f)
            {
                DD2Event.StopInvasion();
                if (!Main.dedServ)
                {
                    Filters.Scene.Deactivate("CrystalDestructionVortex");
                    Filters.Scene.Deactivate("CrystalDestructionColor");
                    Filters.Scene.Deactivate("CrystalWin");
                }
                npc.dontTakeDamage = false;
                npc.life = 0;
                npc.checkDead();
                npc.netUpdate = true;
            }
        }
    }
}
