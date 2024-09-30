namespace VBY.NPCTest;

partial class NPCAIs
{
    public static void AI_011(this NPC self)
    {
        self.reflectsProjectiles = false;
        self.defense = self.defDefense;
        if (self.ai[0] == 0f)
        {
            self.TargetClosest();
            self.ai[0] = 1f;
            if (self.type != 68)
            {
                int num148 = NPC.NewNPC(self.GetSpawnSourceForNPCFromNPCAI(), (int)(self.position.X + self.width / 2), (int)self.position.Y + self.height / 2, 36, self.whoAmI);
                Main.npc[num148].ai[0] = -1f;
                Main.npc[num148].ai[1] = self.whoAmI;
                Main.npc[num148].target = self.target;
                Main.npc[num148].netUpdate = true;
                num148 = NPC.NewNPC(self.GetSpawnSourceForNPCFromNPCAI(), (int)(self.position.X + self.width / 2), (int)self.position.Y + self.height / 2, 36, self.whoAmI);
                Main.npc[num148].ai[0] = 1f;
                Main.npc[num148].ai[1] = self.whoAmI;
                Main.npc[num148].ai[3] = 150f;
                Main.npc[num148].target = self.target;
                Main.npc[num148].netUpdate = true;
            }
        }
        if ((self.type == 68 || Main.netMode == 1) && self.localAI[0] == 0f)
        {
            self.localAI[0] = 1f;
            SoundEngine.PlaySound(15, (int)self.position.X, (int)self.position.Y, 0);
        }
        if (Main.player[self.target].dead || Math.Abs(self.position.X - Main.player[self.target].position.X) > 2000f || Math.Abs(self.position.Y - Main.player[self.target].position.Y) > 2000f)
        {
            self.TargetClosest();
            if (Main.player[self.target].dead || Math.Abs(self.position.X - Main.player[self.target].position.X) > 2000f || Math.Abs(self.position.Y - Main.player[self.target].position.Y) > 2000f)
            {
                self.ai[1] = 3f;
            }
        }
        if ((self.type == 68 || Main.IsItDay()) && self.ai[1] != 3f && self.ai[1] != 2f)
        {
            self.ai[1] = 2f;
            SoundEngine.PlaySound(15, (int)self.position.X, (int)self.position.Y, 0);
        }
        int num149 = 0;
        if (Main.expertMode)
        {
            for (int num150 = 0; num150 < 200; num150++)
            {
                if (Main.npc[num150].active && Main.npc[num150].type == 36)
                {
                    num149++;
                }
            }
            self.defense += num149 * 25;
            if ((num149 < 2 || self.life < self.lifeMax * 0.75) && self.ai[1] == 0f)
            {
                float num151 = 80f;
                if (num149 == 0)
                {
                    num151 /= 2f;
                }
                if (Main.getGoodWorld)
                {
                    num151 *= 0.8f;
                }
                if (self.ai[2] % num151 == 0f)
                {
                    Vector2 center3 = self.Center;
                    float num152 = Main.player[self.target].position.X + Main.player[self.target].width / 2 - center3.X;
                    float num153 = Main.player[self.target].position.Y + Main.player[self.target].height / 2 - center3.Y;
                    _ = (float)Math.Sqrt(num152 * num152 + num153 * num153);
                    if (Collision.CanHit(center3, 1, 1, Main.player[self.target].position, Main.player[self.target].width, Main.player[self.target].height))
                    {
                        float num155 = 3f;
                        if (num149 == 0)
                        {
                            num155 += 2f;
                        }
                        float num156 = Main.player[self.target].position.X + Main.player[self.target].width * 0.5f - center3.X + Main.rand.Next(-20, 21);
                        float num157 = Main.player[self.target].position.Y + Main.player[self.target].height * 0.5f - center3.Y + Main.rand.Next(-20, 21);
                        float num158 = (float)Math.Sqrt(num156 * num156 + num157 * num157);
                        num158 = num155 / num158;
                        num156 *= num158;
                        num157 *= num158;
                        Vector2 vector19 = new(num156 * 1f + Main.rand.Next(-50, 51) * 0.01f, num157 * 1f + Main.rand.Next(-50, 51) * 0.01f);
                        vector19.Normalize();
                        vector19 *= num155;
                        vector19 += self.velocity;
                        num156 = vector19.X;
                        num157 = vector19.Y;
                        int attackDamage_ForProjectiles = self.GetAttackDamage_ForProjectiles(17f, 17f);
                        int num159 = 270;
                        center3 += vector19 * 5f;
                        int num160 = Projectile.NewProjectile(self.GetSpawnSource_ForProjectile(), center3.X, center3.Y, num156, num157, num159, attackDamage_ForProjectiles, 0f, Main.myPlayer, -1f);
                        Main.projectile[num160].timeLeft = 300;
                    }
                }
            }
        }
        if (self.ai[1] == 0f)
        {
            self.damage = self.defDamage;
            self.ai[2] += 1f;
            if (self.ai[2] >= 800f)
            {
                self.ai[2] = 0f;
                self.ai[1] = 1f;
                self.TargetClosest();
                self.netUpdate = true;
            }
            self.rotation = self.velocity.X / 15f;
            float num161 = 0.02f;
            float num162 = 2f;
            float num163 = 0.05f;
            float num164 = 8f;
            if (Main.expertMode)
            {
                num161 = 0.03f;
                num162 = 4f;
                num163 = 0.07f;
                num164 = 9.5f;
            }
            if (Main.getGoodWorld)
            {
                num161 += 0.01f;
                num162 += 1f;
                num163 += 0.05f;
                num164 += 2f;
            }
            if (self.position.Y > Main.player[self.target].position.Y - 250f)
            {
                if (self.velocity.Y > 0f)
                {
                    self.velocity.Y *= 0.98f;
                }
                self.velocity.Y -= num161;
                if (self.velocity.Y > num162)
                {
                    self.velocity.Y = num162;
                }
            }
            else if (self.position.Y < Main.player[self.target].position.Y - 250f)
            {
                if (self.velocity.Y < 0f)
                {
                    self.velocity.Y *= 0.98f;
                }
                self.velocity.Y += num161;
                if (self.velocity.Y < 0f - num162)
                {
                    self.velocity.Y = 0f - num162;
                }
            }
            if (self.position.X + self.width / 2 > Main.player[self.target].position.X + Main.player[self.target].width / 2)
            {
                if (self.velocity.X > 0f)
                {
                    self.velocity.X *= 0.98f;
                }
                self.velocity.X -= num163;
                if (self.velocity.X > num164)
                {
                    self.velocity.X = num164;
                }
            }
            if (self.position.X + self.width / 2 < Main.player[self.target].position.X + Main.player[self.target].width / 2)
            {
                if (self.velocity.X < 0f)
                {
                    self.velocity.X *= 0.98f;
                }
                self.velocity.X += num163;
                if (self.velocity.X < 0f - num164)
                {
                    self.velocity.X = 0f - num164;
                }
            }
        }
        else if (self.ai[1] == 1f)
        {
            if (Main.getGoodWorld)
            {
                if (num149 > 0)
                {
                    self.reflectsProjectiles = true;
                }
                else if (self.ai[2] % 200f == 0f && NPC.CountNPCS(32) < 6)
                {
                    int num165 = 1;
                    for (int num166 = 0; num166 < num165; num166++)
                    {
                        int num167 = 1000;
                        for (int num168 = 0; num168 < num167; num168++)
                        {
                            int num169 = (int)(self.Center.X / 16f) + Main.rand.Next(-50, 51);
                            int num170;
                            for (num170 = (int)(self.Center.Y / 16f) + Main.rand.Next(-50, 51); num170 < Main.maxTilesY - 10 && !WorldGen.SolidTile(num169, num170); num170++)
                            {
                            }
                            num170--;
                            if (!WorldGen.SolidTile(num169, num170))
                            {
                                int num171 = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), num169 * 16 + 8, num170 * 16, 32);
                                if (Main.netMode == 2 && num171 < 200)
                                {
                                    NetMessage.SendData(23, -1, -1, null, num171);
                                }
                                break;
                            }
                        }
                    }
                }
            }
            self.defense -= 10;
            self.ai[2] += 1f;
            if (self.ai[2] == 2f)
            {
                SoundEngine.PlaySound(15, (int)self.position.X, (int)self.position.Y, 0);
            }
            if (self.ai[2] % 40 == 0)
            {
                self.NewProjectile(self.GetToTargetVector2().Normalize(4), 299, self.damage / 8);
            }
            if (self.ai[2] >= 400f)
            {
                self.ai[2] = 0f;
                self.ai[1] = 0f;
            }
            self.rotation += self.direction * 0.3f;
            Vector2 vector20 = new(self.position.X + self.width * 0.5f, self.position.Y + self.height * 0.5f);
            float num172 = Main.player[self.target].position.X + Main.player[self.target].width / 2 - vector20.X;
            float num173 = Main.player[self.target].position.Y + Main.player[self.target].height / 2 - vector20.Y;
            float num174 = (float)Math.Sqrt(num172 * num172 + num173 * num173);
            float num175 = 1.5f;
            self.damage = self.GetAttackDamage_LerpBetweenFinalValues(self.defDamage, self.defDamage * 1.3f);
            if (Main.expertMode)
            {
                num175 = 3.5f;
                if (num174 > 150f)
                {
                    num175 *= 1.05f;
                }
                if (num174 > 200f)
                {
                    num175 *= 1.1f;
                }
                if (num174 > 250f)
                {
                    num175 *= 1.1f;
                }
                if (num174 > 300f)
                {
                    num175 *= 1.1f;
                }
                if (num174 > 350f)
                {
                    num175 *= 1.1f;
                }
                if (num174 > 400f)
                {
                    num175 *= 1.1f;
                }
                if (num174 > 450f)
                {
                    num175 *= 1.1f;
                }
                if (num174 > 500f)
                {
                    num175 *= 1.1f;
                }
                if (num174 > 550f)
                {
                    num175 *= 1.1f;
                }
                if (num174 > 600f)
                {
                    num175 *= 1.1f;
                }
                switch (num149)
                {
                    case 0:
                        num175 *= 1.1f;
                        break;
                    case 1:
                        num175 *= 1.05f;
                        break;
                }
            }
            if (Main.getGoodWorld)
            {
                num175 *= 1.3f;
            }
            num174 = num175 / num174;
            self.velocity.X = num172 * num174;
            self.velocity.Y = num173 * num174;
        }
        else if (self.ai[1] == 2f)
        {
            self.damage = 1000;
            self.defense = 9999;
            self.rotation += self.direction * 0.3f;
            Vector2 vector21 = new(self.position.X + self.width * 0.5f, self.position.Y + self.height * 0.5f);
            float num176 = Main.player[self.target].position.X + Main.player[self.target].width / 2 - vector21.X;
            float num177 = Main.player[self.target].position.Y + Main.player[self.target].height / 2 - vector21.Y;
            float num178 = (float)Math.Sqrt(num176 * num176 + num177 * num177);
            num178 = 8f / num178;
            self.velocity.X = num176 * num178;
            self.velocity.Y = num177 * num178;
        }
        else if (self.ai[1] == 3f)
        {
            self.velocity.Y += 0.1f;
            if (self.velocity.Y < 0f)
            {
                self.velocity.Y *= 0.95f;
            }
            self.velocity.X *= 0.95f;
            self.EncourageDespawn(50);
        }
        if (self.ai[1] != 2f && self.ai[1] != 3f && self.type != 68 && (num149 != 0 || !Main.expertMode))
        {
            int num179 = Dust.NewDust(new Vector2(self.position.X + self.width / 2 - 15f - self.velocity.X * 5f, self.position.Y + self.height - 2f), 30, 10, 5, (0f - self.velocity.X) * 0.2f, 3f, 0, default, 2f);
            Main.dust[num179].noGravity = true;
            Main.dust[num179].velocity.X *= 1.3f;
            Main.dust[num179].velocity.X += self.velocity.X * 0.4f;
            Main.dust[num179].velocity.Y += 2f + self.velocity.Y;
            for (int num180 = 0; num180 < 2; num180++)
            {
                num179 = Dust.NewDust(new Vector2(self.position.X, self.position.Y + 120f), self.width, 60, 5, self.velocity.X, self.velocity.Y, 0, default, 2f);
                Main.dust[num179].noGravity = true;
                Dust dust = Main.dust[num179];
                dust.velocity -= self.velocity;
                Main.dust[num179].velocity.Y += 5f;
            }
        }
    }
}
