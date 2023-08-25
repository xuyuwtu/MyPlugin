namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_074(this NPC npc)
    {
        npc.TargetClosest(faceTarget: false);
        npc.rotation = npc.velocity.ToRotation();
        if (npc.rotation < -(float)Math.PI / 2f)
        {
            npc.rotation += (float)Math.PI;
        }
        if (npc.rotation > (float)Math.PI / 2f)
        {
            npc.rotation -= (float)Math.PI;
        }
        if (Math.Sign(npc.velocity.X) != 0)
        {
            npc.spriteDirection = -Math.Sign(npc.velocity.X);
        }
        if (npc.type == 418)
        {
            npc.spriteDirection = Math.Sign(npc.velocity.X);
        }
        float num1059 = 0.4f;
        float num1060 = 10f;
        float num1061 = 200f;
        float num1062 = 0f;
        float num1063 = 750f;
        float num1064 = 0f;
        float num1065 = 30f;
        float num1066 = 30f;
        float num1067 = 0.95f;
        int num1068 = 50;
        float num1069 = 14f;
        float num1070 = 30f;
        float num1071 = 100f;
        float num1072 = 20f;
        float num1073 = 0f;
        float num1074 = 7f;
        bool flag57 = true;
        if (npc.type == 418)
        {
            num1059 = 0.3f;
            num1060 = 8f;
            num1061 = 175f;
            num1062 = 175f;
            num1063 = 600f;
            num1064 = 80f;
            num1065 = 60f;
            num1066 = 20f;
            num1067 = 0.75f;
            num1068 = 0;
            num1069 = 9f;
            num1070 = 30f;
            num1071 = 150f;
            num1072 = 60f;
            num1073 = 4f / 15f;
            num1074 = 7f;
            flag57 = false;
        }
        num1073 *= num1072;
        if (Main.expertMode)
        {
            num1059 *= Main.GameModeInfo.KnockbackToEnemiesMultiplier;
        }
        if (npc.type == 388 && npc.ai[0] != 3f)
        {
            npc.position += npc.netOffset;
            int num1075 = Dust.NewDust(npc.position, npc.width, npc.height, 226, 0f, 0f, 100, default, 0.5f);
            Main.dust[num1075].noGravity = true;
            Main.dust[num1075].velocity = npc.velocity / 5f;
            Vector2 spinningpoint2 = new(-10f, 10f);
            if (npc.spriteDirection == 1)
            {
                spinningpoint2.X *= -1f;
            }
            spinningpoint2 = spinningpoint2.RotatedBy(npc.rotation);
            Main.dust[num1075].position = npc.Center + spinningpoint2;
            npc.position -= npc.netOffset;
        }
        if (npc.type == 418)
        {
            int num1076 = ((npc.ai[0] != 2f) ? 1 : 2);
            int num1077 = ((npc.ai[0] == 2f) ? 30 : 20);
            for (int num1078 = 0; num1078 < 2; num1078++)
            {
                if (Main.rand.Next(3) < num1076)
                {
                    npc.position += npc.netOffset;
                    int num1079 = Dust.NewDust(npc.Center - new Vector2(num1077), num1077 * 2, num1077 * 2, 6, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 90, default, 1.5f);
                    Main.dust[num1079].noGravity = true;
                    Dust dust = Main.dust[num1079];
                    dust.velocity *= 0.2f;
                    Main.dust[num1079].fadeIn = 1f;
                    npc.position -= npc.netOffset;
                }
            }
        }
        if (npc.ai[0] == 0f)
        {
            npc.knockBackResist = num1059;
            float num1080 = num1060;
            Vector2 center6 = npc.Center;
            Vector2 center7 = Main.player[npc.target].Center;
            Vector2 vector140 = center7 - center6;
            Vector2 vector141 = vector140 - Vector2.UnitY * num1061;
            vector141 += Vector2.UnitX * ((vector140.X < 0f) ? num1062 : (0f - num1062));
            float num1081 = vector140.Length();
            vector140 = Vector2.Normalize(vector140) * num1080;
            vector141 = Vector2.Normalize(vector141) * num1080;
            bool flag58 = Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1);
            if (npc.ai[3] >= 120f)
            {
                flag58 = true;
            }
            float num1082 = 8f;
            flag58 = flag58 && vector140.ToRotation() > (float)Math.PI / num1082 && vector140.ToRotation() < (float)Math.PI - (float)Math.PI / num1082;
            bool flag59 = num1081 < num1064;
            bool flag60 = num1081 > num1063;
            if (flag59 || flag60 || !flag58)
            {
                npc.velocity.X = (npc.velocity.X * (num1065 - 1f) + vector141.X) / num1065;
                npc.velocity.Y = (npc.velocity.Y * (num1065 - 1f) + vector141.Y) / num1065;
                if (!flag58)
                {
                    if (!flag59 && !flag60)
                    {
                        npc.ai[3]++;
                    }
                    if (npc.ai[3] == 120f)
                    {
                        npc.netUpdate = true;
                    }
                }
                else
                {
                    npc.ai[3] = 0f;
                }
            }
            else
            {
                npc.ai[0] = 1f;
                npc.ai[2] = vector140.X;
                npc.ai[3] = vector140.Y;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 1f)
        {
            npc.knockBackResist = 0f;
            bool flag61 = true;
            if (npc.type == 418)
            {
                flag61 = npc.velocity.Length() > 2f;
                if (!flag61 && npc.target >= 0 && !Main.player[npc.target].DeadOrGhost)
                {
                    Vector2 value = (Main.player[npc.target].Center - npc.Center).SafeNormalize(Vector2.Zero) * 0.1f;
                    npc.velocity = Vector2.Lerp(npc.velocity, value, 0.25f);
                }
            }
            if (flag61)
            {
                npc.velocity *= num1067;
            }
            npc.ai[1]++;
            if (npc.ai[1] >= num1066)
            {
                npc.ai[0] = 2f;
                npc.ai[1] = 0f;
                npc.netUpdate = true;
                Vector2 vector142 = new Vector2(npc.ai[2], npc.ai[3]) + new Vector2(Main.rand.Next(-num1068, num1068 + 1), Main.rand.Next(-num1068, num1068 + 1)) * 0.04f;
                vector142.Normalize();
                vector142 *= num1069;
                npc.velocity = vector142;
            }
            if (npc.type == 388 && Main.rand.Next(4) == 0)
            {
                npc.position += npc.netOffset;
                int num1083 = Dust.NewDust(npc.position, npc.width, npc.height, 226, 0f, 0f, 100, default, 0.5f);
                Main.dust[num1083].noGravity = true;
                Dust dust = Main.dust[num1083];
                dust.velocity *= 2f;
                Main.dust[num1083].velocity = Main.dust[num1083].velocity / 2f + Vector2.Normalize(Main.dust[num1083].position - npc.Center);
                npc.position -= npc.netOffset;
            }
        }
        else if (npc.ai[0] == 2f)
        {
            npc.knockBackResist = 0f;
            float num1084 = num1070;
            npc.ai[1]++;
            bool flag62 = Vector2.Distance(npc.Center, Main.player[npc.target].Center) > num1071 && npc.Center.Y > Main.player[npc.target].Center.Y;
            if ((npc.ai[1] >= num1084 && flag62) || npc.velocity.Length() < num1074)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.velocity /= 2f;
                npc.netUpdate = true;
                if (npc.type == 418)
                {
                    npc.ai[1] = 45f;
                    npc.ai[0] = 4f;
                }
            }
            else
            {
                Vector2 center8 = npc.Center;
                Vector2 center9 = Main.player[npc.target].Center;
                Vector2 vector143 = center9 - center8;
                vector143.Normalize();
                if (vector143.HasNaNs())
                {
                    vector143 = new Vector2(npc.direction, 0f);
                }
                npc.velocity = (npc.velocity * (num1072 - 1f) + vector143 * (npc.velocity.Length() + num1073)) / num1072;
            }
            if (flag57 && Collision.SolidCollision(npc.position, npc.width, npc.height))
            {
                npc.ai[0] = 3f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 4f)
        {
            npc.ai[1] -= 3f;
            if (npc.ai[1] <= 0f)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.netUpdate = true;
            }
            npc.velocity *= 0.95f;
        }
        if (flag57 && npc.ai[0] != 3f && Vector2.Distance(npc.Center, Main.player[npc.target].Center) < 64f)
        {
            npc.ai[0] = 3f;
            npc.ai[1] = 0f;
            npc.ai[2] = 0f;
            npc.ai[3] = 0f;
            npc.netUpdate = true;
        }
        if (npc.ai[0] != 3f)
        {
            return;
        }
        npc.position = npc.Center;
        npc.width = (npc.height = 192);
        npc.position.X -= npc.width / 2;
        npc.position.Y -= npc.height / 2;
        npc.velocity = Vector2.Zero;
        npc.damage = npc.GetAttackDamage_ScaledByStrength(80f);
        npc.alpha = 255;
        Lighting.AddLight((int)npc.Center.X / 16, (int)npc.Center.Y / 16, 0.2f, 0.7f, 1.1f);
        for (int num1085 = 0; num1085 < 10; num1085++)
        {
            int num1086 = Dust.NewDust(npc.position, npc.width, npc.height, 31, 0f, 0f, 100, default, 1.5f);
            Dust dust = Main.dust[num1086];
            dust.velocity *= 1.4f;
            Main.dust[num1086].position = ((float)Main.rand.NextDouble() * ((float)Math.PI * 2f)).ToRotationVector2() * ((float)Main.rand.NextDouble() * 96f) + npc.Center;
        }
        for (int num1087 = 0; num1087 < 40; num1087++)
        {
            int num1088 = Dust.NewDust(npc.position, npc.width, npc.height, 226, 0f, 0f, 100, default, 0.5f);
            Main.dust[num1088].noGravity = true;
            Dust dust = Main.dust[num1088];
            dust.velocity *= 2f;
            Main.dust[num1088].position = ((float)Main.rand.NextDouble() * ((float)Math.PI * 2f)).ToRotationVector2() * ((float)Main.rand.NextDouble() * 96f) + npc.Center;
            Main.dust[num1088].velocity = Main.dust[num1088].velocity / 2f + Vector2.Normalize(Main.dust[num1088].position - npc.Center);
            if (Main.rand.Next(2) == 0)
            {
                num1088 = Dust.NewDust(npc.position, npc.width, npc.height, 226, 0f, 0f, 100, default, 0.9f);
                Main.dust[num1088].noGravity = true;
                dust = Main.dust[num1088];
                dust.velocity *= 1.2f;
                Main.dust[num1088].position = ((float)Main.rand.NextDouble() * ((float)Math.PI * 2f)).ToRotationVector2() * ((float)Main.rand.NextDouble() * 96f) + npc.Center;
                Main.dust[num1088].velocity = Main.dust[num1088].velocity / 2f + Vector2.Normalize(Main.dust[num1088].position - npc.Center);
            }
            if (Main.rand.Next(4) == 0)
            {
                num1088 = Dust.NewDust(npc.position, npc.width, npc.height, 226, 0f, 0f, 100, default, 0.7f);
                dust = Main.dust[num1088];
                dust.velocity *= 1.2f;
                Main.dust[num1088].position = ((float)Main.rand.NextDouble() * ((float)Math.PI * 2f)).ToRotationVector2() * ((float)Main.rand.NextDouble() * 96f) + npc.Center;
                Main.dust[num1088].velocity = Main.dust[num1088].velocity / 2f + Vector2.Normalize(Main.dust[num1088].position - npc.Center);
            }
        }
        npc.ai[1]++;
        if (npc.ai[1] >= 3f)
        {
            SoundEngine.PlaySound(SoundID.Item14, npc.position);
            npc.life = 0;
            npc.HitEffect();
            npc.active = false;
        }
    }
}
