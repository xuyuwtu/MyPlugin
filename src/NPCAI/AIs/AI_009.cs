namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_009(this NPC npc)
    {

        if (npc.type == 516)
        {
            if (npc.alpha < 220)
            {
                npc.alpha += 40;
            }
            if (npc.ai[0] == 0f)
            {
                npc.ai[0] = 1f;
                Vector2 vector15 = Main.player[npc.target].Center - npc.Center;
                vector15.Normalize();
                if (vector15.HasNaNs())
                {
                    vector15 = -Vector2.UnitY;
                }
                vector15 = vector15.RotatedByRandom(1.5707963705062866).RotatedBy(-0.7853981852531433);
                if (vector15.Y > 0.2f)
                {
                    vector15.Y = 0.2f;
                }
                npc.velocity = vector15 * (6f + Main.rand.NextFloat() * 4f);
            }
            if (npc.collideX || npc.collideY || npc.Distance(Main.player[npc.target].Center) < 20f)
            {
                npc.StrikeNPCNoInteraction(9999, 0f, npc.direction);
            }
        }
        if (npc.target == 255)
        {
            npc.TargetClosest();
            float num118 = 6f;
            if (npc.type == 25)
            {
                num118 = 5f;
            }
            if (npc.type == 112 || npc.type == 666)
            {
                num118 = 7f;
            }
            if (Main.getGoodWorld)
            {
                if (npc.type == 33 && NPC.AnyNPCs(35))
                {
                    num118 = 10f;
                }
                if (npc.type == 25 && NPC.AnyNPCs(113))
                {
                    num118 = 14f;
                }
                if (npc.type == 666)
                {
                    num118 = 10f;
                }
            }
            //Vector2 vector16 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            Vector2 vector16 = npc.Center;
            float num119 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector16.X;
            float num120 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector16.Y;
            float num121 = (float)Math.Sqrt(num119 * num119 + num120 * num120);
            num121 = num118 / num121;
            npc.velocity.X = num119 * num121;
            npc.velocity.Y = num120 * num121;
        }
        if (Main.getGoodWorld && !npc.dontTakeDamage)
        {
            if (npc.type == 33 && NPC.AnyNPCs(35))
            {
                npc.dontTakeDamage = true;
            }
            else if (npc.type == 25 && NPC.AnyNPCs(113))
            {
                npc.dontTakeDamage = true;
            }
            else if (npc.type == 666 && (double)(npc.Center.Y / 16f) < Main.worldSurface)
            {
                npc.dontTakeDamage = true;
            }
        }
        if (npc.type == 112 || npc.type == 666)
        {
            npc.damage = npc.defDamage;
            if (npc.type == 666)
            {
                npc.damage = npc.GetAttackDamage_ScaledByStrength(32f);
            }
            npc.ai[0] += 1f;
            if (npc.ai[0] > 3f)
            {
                npc.ai[0] = 3f;
            }
            if (npc.ai[0] == 2f)
            {
                npc.position += npc.velocity;
                SoundEngine.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 9);
                for (int num122 = 0; num122 < 20; num122++)
                {
                    int num123 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f) + npc.netOffset, npc.width, npc.height, 18, 0f, 0f, 100, default, 1.8f);
                    Dust dust = Main.dust[num123];
                    dust.velocity *= 1.3f;
                    dust = Main.dust[num123];
                    dust.velocity += npc.velocity;
                    Main.dust[num123].noGravity = true;
                }
            }
        }
        if ((npc.type == 112 || npc.type == 666) && Collision.SolidCollision(npc.position, npc.width, npc.height))
        {
            _ = Main.netMode;
            _ = 1;
            npc.StrikeNPCNoInteraction(9999, 0f, 0);
        }
        npc.EncourageDespawn(100);
        if (npc.type == 516)
        {
            npc.rotation += 0.1f * npc.direction;
            float num124 = 15f;
            float num125 = 1f / 12f;
            Vector2 center = npc.Center;
            Vector2 center2 = Main.player[npc.target].Center;
            Vector2 vector17 = center2 - center;
            vector17.Normalize();
            if (vector17.HasNaNs())
            {
                vector17 = new Vector2(npc.direction, 0f);
            }
            npc.velocity = (npc.velocity * (num124 - 1f) + vector17 * (npc.velocity.Length() + num125)) / num124;
            if (npc.velocity.Length() < 6f)
            {
                npc.velocity *= 1.05f;
            }
            return;
        }
        npc.position += npc.netOffset;
        for (int num126 = 0; num126 < 2; num126++)
        {
            if (npc.type == 30 || npc.type == 665)
            {
                npc.alpha = 255;
                for (int num127 = 0; num127 < 2; num127++)
                {
                    int num128 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, 27, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, default, 1.3f);
                    Main.dust[num128].noGravity = true;
                    Dust dust = Main.dust[num128];
                    dust.velocity *= 0.3f;
                    Main.dust[num128].velocity.X -= npc.velocity.X * 0.2f;
                    Main.dust[num128].velocity.Y -= npc.velocity.Y * 0.2f;
                }
            }
            else if (npc.type == 33)
            {
                for (int num129 = 0; num129 < 3; num129++)
                {
                    float num130 = npc.velocity.X / 3f * num126;
                    float num131 = npc.velocity.Y / 3f * num126;
                    int num132 = 2;
                    int num133 = Dust.NewDust(new Vector2(npc.position.X + num132, npc.position.Y + num132), npc.width - num132 * 2, npc.height - num132 * 2, 172, 0f, 0f, 100, default, 1.2f);
                    Main.dust[num133].noGravity = true;
                    Dust dust = Main.dust[num133];
                    dust.velocity *= 0.1f;
                    dust = Main.dust[num133];
                    dust.velocity += npc.velocity * 0.5f;
                    Main.dust[num133].position.X -= num130;
                    Main.dust[num133].position.Y -= num131;
                }
                if (Main.rand.Next(5) == 0)
                {
                    int num134 = 2;
                    int num135 = Dust.NewDust(new Vector2(npc.position.X + num134, npc.position.Y + num134), npc.width - num134 * 2, npc.height - num134 * 2, 172, 0f, 0f, 100, default, 0.6f);
                    Dust dust = Main.dust[num135];
                    dust.velocity *= 0.25f;
                    dust = Main.dust[num135];
                    dust.velocity += npc.velocity * 0.5f;
                }
            }
            else if (npc.type == 112 || npc.type == 666)
            {
                int num136 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, 18, npc.velocity.X * 0.1f, npc.velocity.Y * 0.1f, 80, default, 1.3f);
                Dust dust = Main.dust[num136];
                dust.velocity *= 0.3f;
                Main.dust[num136].noGravity = true;
            }
            else
            {
                Lighting.AddLight((int)((npc.position.X + npc.width / 2) / 16f), (int)((npc.position.Y + npc.height / 2) / 16f), 1f, 0.3f, 0.1f);
                int num137 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, 6, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num137].noGravity = true;
                Main.dust[num137].velocity.X *= 0.3f;
                Main.dust[num137].velocity.Y *= 0.3f;
            }
        }
        npc.rotation += 0.4f * npc.direction;
        npc.position -= npc.netOffset;
    }
}
