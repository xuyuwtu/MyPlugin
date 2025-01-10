namespace VBY.NPCTest;

public static partial class NPCAIs
{
    public static void AI_004(NPC self)
    {
        const int netClientMode = 1;
        const int netServerMode = 2;

        const int phaseAI = 0;
        const int phase1or2CountAI = 1;
        const int mainAI = 1;
        const int phase1or2RotationValueAI = 2;
        const int countAI = 2;
        const int rushCountAI = 3;

        const int aiFloat = 0;
        const int aiRush = 1;
        const int aiRushing = 2;
        const int aiFastRush = 3;
        const int aiFastRushing = 4;
        const int aiHighFloat = 5;

        bool isLifePhase2 = false;
        if (Main.expertMode && self.life < self.lifeMax * 0.12)
        {
            isLifePhase2 = true;
        }
        bool isLifePhase3 = false;
        if (Main.expertMode && self.life < self.lifeMax * 0.04)
        {
            isLifePhase3 = true;
        }
        float fastRushDurationTime = 20f;
        if (isLifePhase3)
        {
            fastRushDurationTime = 10f;
        }
        if (self.target < 0 || self.target == 255 || Main.player[self.target].dead || !Main.player[self.target].active)
        {
            self.TargetClosest();
        }
        bool targetIsDead = Main.player[self.target].dead;
        float toTargetRotation = (float)Math.Atan2(x: self.position.X + self.width / 2 - Main.player[self.target].position.X - Main.player[self.target].width / 2, y: self.position.Y + self.height - 59f - Main.player[self.target].position.Y - Main.player[self.target].height / 2) + 1.57f;
        if (toTargetRotation < 0f)
        {
            toTargetRotation += 6.283f;
        }
        else if ((double)toTargetRotation > 6.283)
        {
            toTargetRotation -= 6.283f;
        }
        float rotationSpeed = 0f;
        if (self.ai[phaseAI] == 0f && self.ai[mainAI] == aiFloat)
        {
            rotationSpeed = 0.02f;
        }
        if (self.ai[phaseAI] == 0f && self.ai[mainAI] == aiRushing && self.ai[countAI] > 40f)
        {
            rotationSpeed = 0.05f;
        }
        if (self.ai[phaseAI] == 3f && self.ai[mainAI] == aiFloat)
        {
            rotationSpeed = 0.05f;
        }
        if (self.ai[phaseAI] == 3f && self.ai[mainAI] == aiRushing && self.ai[countAI] > 40f)
        {
            rotationSpeed = 0.08f;
        }
        if (self.ai[phaseAI] == 3f && self.ai[mainAI] == aiFastRushing && self.ai[countAI] > fastRushDurationTime)
        {
            rotationSpeed = 0.15f;
        }
        if (self.ai[phaseAI] == 3f && self.ai[mainAI] == aiHighFloat)
        {
            rotationSpeed = 0.05f;
        }
        if (Main.expertMode)
        {
            rotationSpeed *= 1.5f;
        }
        if (isLifePhase3 && Main.expertMode)
        {
            rotationSpeed = 0f;
        }
        if (self.rotation < toTargetRotation)
        {
            if ((double)(toTargetRotation - self.rotation) > 3.1415)
            {
                self.rotation -= rotationSpeed;
            }
            else
            {
                self.rotation += rotationSpeed;
            }
        }
        else if (self.rotation > toTargetRotation)
        {
            if ((double)(self.rotation - toTargetRotation) > 3.1415)
            {
                self.rotation += rotationSpeed;
            }
            else
            {
                self.rotation -= rotationSpeed;
            }
        }
        if (self.rotation > toTargetRotation - rotationSpeed && self.rotation < toTargetRotation + rotationSpeed)
        {
            self.rotation = toTargetRotation;
        }
        if (self.rotation < 0f)
        {
            self.rotation += 6.283f;
        }
        else if (self.rotation > 6.283)
        {
            self.rotation -= 6.283f;
        }
        if (self.rotation > toTargetRotation - rotationSpeed && self.rotation < toTargetRotation + rotationSpeed)
        {
            self.rotation = toTargetRotation;
        }
        if (Main.rand.Next(5) == 0)
        {
            int dustIndex = Dust.NewDust(new Vector2(self.position.X, self.position.Y + self.height * 0.25f), self.width, (int)(self.height * 0.5f), 5, self.velocity.X, 2f);
            Main.dust[dustIndex].velocity.X *= 0.5f;
            Main.dust[dustIndex].velocity.Y *= 0.1f;
        }
        self.reflectsProjectiles = false;
        if (Main.IsItDay() || targetIsDead)
        {
            self.velocity.Y -= 0.04f;
            self.EncourageDespawn(10);
            return;
        }
        if (self.ai[phaseAI] == 0f)
        {
            if (self.ai[mainAI] == aiFloat)
            {
                float speedCoefficient = 5f;
                float velocityAdjustmentThreshold = 0.04f;
                if (Main.expertMode)
                {
                    velocityAdjustmentThreshold = 0.15f;
                    speedCoefficient = 7f;
                }
                if (Main.getGoodWorld)
                {
                    velocityAdjustmentThreshold += 0.05f;
                    speedCoefficient += 1f;
                }
                Vector2 selfCenter = new Vector2(self.position.X + self.width * 0.5f, self.position.Y + self.height * 0.5f);
                float toTargetX = Main.player[self.target].position.X + Main.player[self.target].width / 2 - selfCenter.X;
                float toTargetY = Main.player[self.target].position.Y + Main.player[self.target].height / 2 - 200f - selfCenter.Y;
                float distanceToTarget = (float)Math.Sqrt(toTargetX * toTargetX + toTargetY * toTargetY);
                float distanceToTargetOrigin = distanceToTarget;
                distanceToTarget = speedCoefficient / distanceToTarget;
                toTargetX *= distanceToTarget;
                toTargetY *= distanceToTarget;
                if (self.velocity.X < toTargetX)
                {
                    self.velocity.X += velocityAdjustmentThreshold;
                    if (self.velocity.X < 0f && toTargetX > 0f)
                    {
                        self.velocity.X += velocityAdjustmentThreshold;
                    }
                }
                else if (self.velocity.X > toTargetX)
                {
                    self.velocity.X -= velocityAdjustmentThreshold;
                    if (self.velocity.X > 0f && toTargetX < 0f)
                    {
                        self.velocity.X -= velocityAdjustmentThreshold;
                    }
                }
                if (self.velocity.Y < toTargetY)
                {
                    self.velocity.Y += velocityAdjustmentThreshold;
                    if (self.velocity.Y < 0f && toTargetY > 0f)
                    {
                        self.velocity.Y += velocityAdjustmentThreshold;
                    }
                }
                else if (self.velocity.Y > toTargetY)
                {
                    self.velocity.Y -= velocityAdjustmentThreshold;
                    if (self.velocity.Y > 0f && toTargetY < 0f)
                    {
                        self.velocity.Y -= velocityAdjustmentThreshold;
                    }
                }
                self.ai[countAI] += 1f;
                float checkDistance = 600f;
                if (Main.expertMode)
                {
                    checkDistance *= 0.35f;
                }
                if (self.ai[countAI] >= checkDistance)
                {
                    self.ai[mainAI] = aiRush;
                    self.ai[countAI] = 0f;
                    self.ai[rushCountAI] = 0f;
                    self.target = 255;
                    self.netUpdate = true;
                }
                else if ((self.position.Y + self.height < Main.player[self.target].position.Y && distanceToTargetOrigin < 500f) || (Main.expertMode && distanceToTargetOrigin < 500f))
                {
                    if (!Main.player[self.target].dead)
                    {
                        self.ai[rushCountAI] += 1f;
                    }
                    float spawnThreshold = 110f;
                    if (Main.expertMode)
                    {
                        spawnThreshold *= 0.4f;
                    }
                    if (Main.getGoodWorld)
                    {
                        spawnThreshold *= 0.8f;
                    }
                    if (self.ai[rushCountAI] >= spawnThreshold)
                    {
                        self.ai[rushCountAI] = 0f;
                        self.rotation = toTargetRotation;
                        speedCoefficient = 5f;
                        if (Main.expertMode)
                        {
                            speedCoefficient = 6f;
                        }
                        toTargetX = Main.player[self.target].position.X + Main.player[self.target].width / 2 - selfCenter.X;
                        toTargetY = Main.player[self.target].position.Y + Main.player[self.target].height / 2 - selfCenter.Y;
                        float speedTowardsTarget = speedCoefficient / (float)Math.Sqrt(toTargetX * toTargetX + toTargetY * toTargetY);
                        Vector2 spawnPosition = selfCenter;
                        Vector2 toTargetSpeed = new Vector2(toTargetX * speedTowardsTarget, toTargetY * speedTowardsTarget);
                        spawnPosition.X += toTargetSpeed.X * 10f;
                        spawnPosition.Y += toTargetSpeed.Y * 10f;
                        if (Main.netMode != netClientMode)
                        {
                            int npcIndex = NPC.NewNPC(self.GetSpawnSourceForNPCFromNPCAI(), (int)spawnPosition.X, (int)spawnPosition.Y, NPCID.ServantofCthulhu);
                            Main.npc[npcIndex].velocity.X = toTargetSpeed.X;
                            Main.npc[npcIndex].velocity.Y = toTargetSpeed.Y;
                            if (Main.netMode == netServerMode && npcIndex < Main.maxNPCs)
                            {
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcIndex);
                            }
                        }
                        for (int i = 0; i < 10; i++)
                        {
                        }
                    }
                }
            }
            else if (self.ai[mainAI] == aiRush)
            {
                self.rotation = toTargetRotation;
                float speedCoefficient = 6f;
                if (Main.expertMode)
                {
                    speedCoefficient = 7f;
                }
                if (Main.getGoodWorld)
                {
                    speedCoefficient += 1f;
                }
                Vector2 selfCenter = new Vector2(self.position.X + self.width * 0.5f, self.position.Y + self.height * 0.5f);
                float toTargetX = Main.player[self.target].position.X + Main.player[self.target].width / 2 - selfCenter.X;
                float toTargetY = Main.player[self.target].position.Y + Main.player[self.target].height / 2 - selfCenter.Y;
                float speedTowardsTarget = speedCoefficient / (float)Math.Sqrt(toTargetX * toTargetX + toTargetY * toTargetY);
                self.velocity.X = toTargetX * speedTowardsTarget;
                self.velocity.Y = toTargetY * speedTowardsTarget;
                self.ai[mainAI] = aiRushing;
                self.netUpdate = true;
                if (self.netSpam > 10)
                {
                    self.netSpam = 10;
                }
            }
            else if (self.ai[mainAI] == aiRushing)
            {
                self.ai[countAI] += 1f;
                if (self.ai[countAI] >= 40f)
                {
                    self.velocity *= 0.98f;
                    if (Main.expertMode)
                    {
                        self.velocity *= 0.985f;
                    }
                    if (Main.getGoodWorld)
                    {
                        self.velocity *= 0.99f;
                    }
                    if (self.velocity.X > -0.1 && self.velocity.X < 0.1)
                    {
                        self.velocity.X = 0f;
                    }
                    if (self.velocity.Y > -0.1 && self.velocity.Y < 0.1)
                    {
                        self.velocity.Y = 0f;
                    }
                }
                else
                {
                    self.rotation = (float)Math.Atan2(self.velocity.Y, self.velocity.X) - 1.57f;
                }
                int rushTime = 150;
                if (Main.expertMode)
                {
                    rushTime = 100;
                }
                if (Main.getGoodWorld)
                {
                    rushTime -= 15;
                }
                if (self.ai[countAI] >= rushTime)
                {
                    self.ai[rushCountAI] += 1f;
                    self.ai[countAI] = 0f;
                    self.target = 255;
                    self.rotation = toTargetRotation;
                    if (self.ai[rushCountAI] >= 3f)
                    {
                        self.ai[mainAI] = aiFloat;
                        self.ai[rushCountAI] = 0f;
                    }
                    else
                    {
                        self.ai[mainAI] = aiRush;
                    }
                }
            }
            float lifeThreshold = 0.5f;
            if (Main.expertMode)
            {
                lifeThreshold = 0.65f;
            }
            if (self.life < self.lifeMax * lifeThreshold)
            {
                self.ai[phaseAI] = 1f;
                self.ai[mainAI] = aiFloat;
                self.ai[countAI] = 0f;
                self.ai[rushCountAI] = 0f;
                self.netUpdate = true;
                if (self.netSpam > 10)
                {
                    self.netSpam = 10;
                }
            }
            return;
        }
        if (self.ai[phaseAI] == 1f || self.ai[phaseAI] == 2f)
        {
            if (self.ai[phaseAI] == 1f || self.ai[rushCountAI] == 1f)
            {
                self.ai[phase1or2RotationValueAI] += 0.005f;
                if (self.ai[phase1or2RotationValueAI] > 0.5)
                {
                    self.ai[phase1or2RotationValueAI] = 0.5f;
                }
            }
            else
            {
                self.ai[phase1or2RotationValueAI] -= 0.005f;
                if (self.ai[phase1or2RotationValueAI] < 0f)
                {
                    self.ai[phase1or2RotationValueAI] = 0f;
                }
            }
            self.rotation += self.ai[phase1or2RotationValueAI];
            self.ai[phase1or2CountAI] += 1f;
            if (Main.getGoodWorld)
            {
                self.reflectsProjectiles = true;
            }
            int spawnInterval = 20;
            if (Main.getGoodWorld && self.life < self.lifeMax / 3)
            {
                spawnInterval = 10;
            }
            if (Main.expertMode && self.ai[phase1or2CountAI] % spawnInterval == 0f)
            {
                float speedCoefficient = 5f;
                Vector2 selfCenter = new Vector2(self.position.X + self.width * 0.5f, self.position.Y + self.height * 0.5f);
                float toTargetXOffset = Main.rand.Next(-200, 200);
                float toTargetYOffset = Main.rand.Next(-200, 200);
                if (Main.getGoodWorld)
                {
                    toTargetXOffset *= 3f;
                    toTargetYOffset *= 3f;
                }
                float speedTowardsTarget = speedCoefficient / (float)Math.Sqrt(toTargetXOffset * toTargetXOffset + toTargetYOffset * toTargetYOffset);
                Vector2 spawnPosition = selfCenter;
                Vector2 spawnVelocity = new Vector2(toTargetXOffset * speedTowardsTarget, toTargetYOffset * speedTowardsTarget);
                spawnPosition.X += spawnVelocity.X * 10f;
                spawnPosition.Y += spawnVelocity.Y * 10f;
                if (Main.netMode != netClientMode)
                {
                    int npcIndex = NPC.NewNPC(self.GetSpawnSourceForNPCFromNPCAI(), (int)spawnPosition.X, (int)spawnPosition.Y, NPCID.ServantofCthulhu);
                    Main.npc[npcIndex].velocity = spawnVelocity;
                    if (Main.netMode == netServerMode && npcIndex < Main.maxNPCs)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcIndex);
                    }

                    Projectile.NewProjectile(self.GetSpawnSourceForNPCFromNPCAI(), self.Center, new Vector2(1, 0).RotatedBy(self.rotation), ProjectileID.FlamingScythe, self.damage, 0);
                }
                for (int i = 0; i < 10; i++)
                {
                }
            }
            if (self.ai[phase1or2CountAI] >= 100f)
            {
                if (self.ai[rushCountAI] == 1f)
                {
                    self.ai[rushCountAI] = 0f;
                    self.ai[mainAI] = aiFloat;
                }
                else
                {
                    self.ai[phaseAI] += 1f;
                    self.ai[mainAI] = aiFloat;
                    if (self.ai[phaseAI] == 3f)
                    {
                        self.ai[countAI] = 0f;
                    }
                    else
                    {
                        for (int i = 0; i < 2; i++)
                        {
                        }
                        for (int i = 0; i < 20; i++)
                        {
                        }
                    }
                }
            }
            self.velocity.X *= 0.98f;
            self.velocity.Y *= 0.98f;
            if (self.velocity.X > -0.1 && self.velocity.X < 0.1)
            {
                self.velocity.X = 0f;
            }
            if (self.velocity.Y > -0.1 && self.velocity.Y < 0.1)
            {
                self.velocity.Y = 0f;
            }
            return;
        }
        self.defense = 0;
        int normalDamage = 23;
        int expertDamage = 18;
        if (Main.expertMode)
        {
            if (isLifePhase2)
            {
                self.defense = -15;
            }
            if (isLifePhase3)
            {
                expertDamage = 20;
                self.defense = -30;
            }
        }
        self.damage = self.GetAttackDamage_LerpBetweenFinalValues(normalDamage, expertDamage);
        self.damage = self.GetAttackDamage_ScaledByStrength(self.damage);
        if (self.ai[mainAI] == aiFloat && isLifePhase2)
        {
            self.ai[mainAI] = aiHighFloat;
        }
        if (self.ai[mainAI] == aiFloat)
        {
            float speedCoefficient = 6f;
            float velocityAdjustmentThreshold = 0.07f;
            Vector2 selfCenter = new Vector2(self.position.X + self.width * 0.5f, self.position.Y + self.height * 0.5f);
            float toTargetX = Main.player[self.target].position.X + Main.player[self.target].width / 2 - selfCenter.X;
            float toTargetY = Main.player[self.target].position.Y + Main.player[self.target].height / 2 - 120f - selfCenter.Y;
            float distanceToTarget = (float)Math.Sqrt(toTargetX * toTargetX + toTargetY * toTargetY);
            if (distanceToTarget > 400f && Main.expertMode)
            {
                speedCoefficient += 1f;
                velocityAdjustmentThreshold += 0.05f;
                if (distanceToTarget > 600f)
                {
                    speedCoefficient += 1f;
                    velocityAdjustmentThreshold += 0.05f;
                    if (distanceToTarget > 800f)
                    {
                        speedCoefficient += 1f;
                        velocityAdjustmentThreshold += 0.05f;
                    }
                }
            }
            if (Main.getGoodWorld)
            {
                speedCoefficient += 1f;
                velocityAdjustmentThreshold += 0.1f;
            }
            distanceToTarget = speedCoefficient / distanceToTarget;
            toTargetX *= distanceToTarget;
            toTargetY *= distanceToTarget;
            if (self.velocity.X < toTargetX)
            {
                self.velocity.X += velocityAdjustmentThreshold;
                if (self.velocity.X < 0f && toTargetX > 0f)
                {
                    self.velocity.X += velocityAdjustmentThreshold;
                }
            }
            else if (self.velocity.X > toTargetX)
            {
                self.velocity.X -= velocityAdjustmentThreshold;
                if (self.velocity.X > 0f && toTargetX < 0f)
                {
                    self.velocity.X -= velocityAdjustmentThreshold;
                }
            }
            if (self.velocity.Y < toTargetY)
            {
                self.velocity.Y += velocityAdjustmentThreshold;
                if (self.velocity.Y < 0f && toTargetY > 0f)
                {
                    self.velocity.Y += velocityAdjustmentThreshold;
                }
            }
            else if (self.velocity.Y > toTargetY)
            {
                self.velocity.Y -= velocityAdjustmentThreshold;
                if (self.velocity.Y > 0f && toTargetY < 0f)
                {
                    self.velocity.Y -= velocityAdjustmentThreshold;
                }
            }
            self.ai[countAI] += 1f;
            if (self.ai[countAI] >= 200f)
            {
                self.ai[mainAI] = aiRush;
                self.ai[countAI] = 0f;
                self.ai[rushCountAI] = 0f;
                if (Main.expertMode && self.life < self.lifeMax * 0.35)
                {
                    self.ai[mainAI] = aiFastRush;
                }
                self.target = 255;
                self.netUpdate = true;
            }
            if (Main.expertMode && isLifePhase3)
            {
                self.TargetClosest();
                self.netUpdate = true;
                self.ai[mainAI] = aiFastRush;
                self.ai[countAI] = 0f;
                self.ai[rushCountAI] -= 1000f;
            }
        }
        else if (self.ai[mainAI] == aiRush)
        {
            self.rotation = toTargetRotation;
            float speedCoefficient = 6.8f;
            if (Main.expertMode && self.ai[rushCountAI] == 1f)
            {
                speedCoefficient *= 1.15f;
            }
            if (Main.expertMode && self.ai[rushCountAI] == 2f)
            {
                speedCoefficient *= 1.3f;
            }
            if (Main.getGoodWorld)
            {
                speedCoefficient *= 1.2f;
            }
            Vector2 selfCenter = new Vector2(self.position.X + self.width * 0.5f, self.position.Y + self.height * 0.5f);
            float toTargetX = Main.player[self.target].position.X + Main.player[self.target].width / 2 - selfCenter.X;
            float toTargetY = Main.player[self.target].position.Y + Main.player[self.target].height / 2 - selfCenter.Y;
            float speedTowardsTarget = speedCoefficient / (float)Math.Sqrt(toTargetX * toTargetX + toTargetY * toTargetY);
            self.velocity.X = toTargetX * speedTowardsTarget;
            self.velocity.Y = toTargetY * speedTowardsTarget;
            self.ai[mainAI] = aiRushing;
            self.netUpdate = true;
            if (self.netSpam > 10)
            {
                self.netSpam = 10;
            }
        }
        else if (self.ai[mainAI] == aiRushing)
        {
            float num46 = 40f;
            self.ai[countAI] += 1f;
            if (Main.expertMode)
            {
                num46 = 50f;
            }
            if (self.ai[countAI] >= num46)
            {
                self.velocity *= 0.97f;
                if (Main.expertMode)
                {
                    self.velocity *= 0.98f;
                }
                if (self.velocity.X > -0.1 && self.velocity.X < 0.1)
                {
                    self.velocity.X = 0f;
                }
                if (self.velocity.Y > -0.1 && self.velocity.Y < 0.1)
                {
                    self.velocity.Y = 0f;
                }
            }
            else
            {
                self.rotation = (float)Math.Atan2(self.velocity.Y, self.velocity.X) - 1.57f;
            }
            int rushTime = 130;
            if (Main.expertMode)
            {
                rushTime = 90;
            }
            if (self.ai[countAI] >= rushTime)
            {
                self.ai[rushCountAI] += 1f;
                self.ai[countAI] = 0f;
                self.target = 255;
                self.rotation = toTargetRotation;
                if (self.ai[rushCountAI] >= 3f)
                {
                    self.ai[mainAI] = aiFloat;
                    self.ai[rushCountAI] = 0f;
                    if (Main.expertMode && Main.netMode != 1 && self.life < self.lifeMax * 0.5)
                    {
                        self.ai[mainAI] = aiFastRush;
                        self.ai[rushCountAI] += Main.rand.Next(1, 4);
                    }
                    self.netUpdate = true;
                    if (self.netSpam > 10)
                    {
                        self.netSpam = 10;
                    }
                }
                else
                {
                    self.ai[mainAI] = aiRush;
                }
            }
        }
        else if (self.ai[mainAI] == aiFastRush)
        {
            if (self.ai[rushCountAI] == 4f && isLifePhase2 && self.Center.Y > Main.player[self.target].Center.Y)
            {
                self.TargetClosest();
                self.ai[mainAI] = aiFloat;
                self.ai[countAI] = 0f;
                self.ai[rushCountAI] = 0f;
                self.netUpdate = true;
                if (self.netSpam > 10)
                {
                    self.netSpam = 10;
                }
            }
            else if (Main.netMode != netClientMode)
            {
                self.TargetClosest();
                float speedCoefficient = 20f;
                Vector2 selfCenter = new Vector2(self.position.X + self.width * 0.5f, self.position.Y + self.height * 0.5f);
                float toTargetX = Main.player[self.target].position.X + Main.player[self.target].width / 2 - selfCenter.X;
                float toTargetY = Main.player[self.target].position.Y + Main.player[self.target].height / 2 - selfCenter.Y;
                float speedMultiplier = Math.Abs(Main.player[self.target].velocity.X) + Math.Abs(Main.player[self.target].velocity.Y) / 4f;
                speedMultiplier += 10f - speedMultiplier;
                if (speedMultiplier < 5f)
                {
                    speedMultiplier = 5f;
                }
                if (speedMultiplier > 15f)
                {
                    speedMultiplier = 15f;
                }
                if (self.ai[countAI] == -1f && !isLifePhase3)
                {
                    speedMultiplier *= 4f;
                    speedCoefficient *= 1.3f;
                }
                if (isLifePhase3)
                {
                    speedMultiplier *= 2f;
                }
                toTargetX -= Main.player[self.target].velocity.X * speedMultiplier;
                toTargetY -= Main.player[self.target].velocity.Y * speedMultiplier / 4f;
                toTargetX *= 1f + Main.rand.Next(-10, 11) * 0.01f;
                toTargetY *= 1f + Main.rand.Next(-10, 11) * 0.01f;
                if (isLifePhase3)
                {
                    toTargetX *= 1f + Main.rand.Next(-10, 11) * 0.01f;
                    toTargetY *= 1f + Main.rand.Next(-10, 11) * 0.01f;
                }
                float distanceToTarget = (float)Math.Sqrt(toTargetX * toTargetX + toTargetY * toTargetY);
                float distanceToTargetOrigin = distanceToTarget;
                distanceToTarget = speedCoefficient / distanceToTarget;
                self.velocity.X = toTargetX * distanceToTarget;
                self.velocity.Y = toTargetY * distanceToTarget;
                self.velocity.X += Main.rand.Next(-20, 21) * 0.1f;
                self.velocity.Y += Main.rand.Next(-20, 21) * 0.1f;
                if (isLifePhase3)
                {
                    self.velocity.X += Main.rand.Next(-50, 51) * 0.1f;
                    self.velocity.Y += Main.rand.Next(-50, 51) * 0.1f;
                    float velocityX = Math.Abs(self.velocity.X);
                    float velocityY = Math.Abs(self.velocity.Y);
                    if (self.Center.X > Main.player[self.target].Center.X)
                    {
                        velocityY *= -1f;
                    }
                    if (self.Center.Y > Main.player[self.target].Center.Y)
                    {
                        velocityX *= -1f;
                    }
                    self.velocity.X = velocityY + self.velocity.X;
                    self.velocity.Y = velocityX + self.velocity.Y;
                    self.velocity.Normalize();
                    self.velocity *= speedCoefficient;
                    self.velocity.X += Main.rand.Next(-20, 21) * 0.1f;
                    self.velocity.Y += Main.rand.Next(-20, 21) * 0.1f;
                }
                else if (distanceToTargetOrigin < 100f)
                {
                    if (Math.Abs(self.velocity.X) > Math.Abs(self.velocity.Y))
                    {
                        float velocityX = Math.Abs(self.velocity.X);
                        float velocityY = Math.Abs(self.velocity.Y);
                        if (self.Center.X > Main.player[self.target].Center.X)
                        {
                            velocityY *= -1f;
                        }
                        if (self.Center.Y > Main.player[self.target].Center.Y)
                        {
                            velocityX *= -1f;
                        }
                        self.velocity.X = velocityY;
                        self.velocity.Y = velocityX;
                    }
                }
                else if (Math.Abs(self.velocity.X) > Math.Abs(self.velocity.Y))
                {
                    float num58 = (Math.Abs(self.velocity.X) + Math.Abs(self.velocity.Y)) / 2f;
                    float num59 = num58;
                    if (self.Center.X > Main.player[self.target].Center.X)
                    {
                        num59 *= -1f;
                    }
                    if (self.Center.Y > Main.player[self.target].Center.Y)
                    {
                        num58 *= -1f;
                    }
                    self.velocity.X = num59;
                    self.velocity.Y = num58;
                }
                self.ai[mainAI] = aiFastRushing;
                self.netUpdate = true;
                if (self.netSpam > 10)
                {
                    self.netSpam = 10;
                }
            }
        }
        else if (self.ai[mainAI] == aiFastRushing)
        {
            if (self.ai[countAI] == 0f)
            {
            }
            float rushTime = fastRushDurationTime;
            self.ai[countAI] += 1f;
            if (self.ai[countAI] == rushTime && Vector2.Distance(self.position, Main.player[self.target].position) < 200f)
            {
                self.ai[countAI] -= 1f;
            }
            if (self.ai[countAI] >= rushTime)
            {
                self.velocity *= 0.95f;
                if (self.velocity.X > -0.1 && self.velocity.X < 0.1)
                {
                    self.velocity.X = 0f;
                }
                if (self.velocity.Y > -0.1 && self.velocity.Y < 0.1)
                {
                    self.velocity.Y = 0f;
                }
            }
            else
            {
                self.rotation = (float)Math.Atan2(self.velocity.Y, self.velocity.X) - 1.57f;
            }
            if (self.ai[countAI] >= rushTime + 13f)
            {
                self.netUpdate = true;
                if (self.netSpam > 10)
                {
                    self.netSpam = 10;
                }
                self.ai[rushCountAI] += 1f;
                self.ai[countAI] = 0f;
                if (self.ai[rushCountAI] >= 5f)
                {
                    self.ai[mainAI] = aiFloat;
                    self.ai[rushCountAI] = 0f;
                    if (self.target >= 0 && Main.getGoodWorld && Collision.CanHit(self.position, self.width, self.height, Main.player[self.target].position, self.width, self.height))
                    {
                        self.ai[phaseAI] = 2f;
                        self.ai[mainAI] = aiFloat;
                        self.ai[countAI] = 0f;
                        self.ai[rushCountAI] = 1f;
                        self.netUpdate = true;
                    }
                }
                else
                {
                    self.ai[mainAI] = aiFastRush;
                }

                if (Vector2.Distance(self.Center, Main.player[self.target].Center) < 2000 && Collision.CanHit(self.Center, 1, 1, Main.player[self.target].Center, 1, 1))
                {
                    Projectile.NewProjectile(self.GetSpawnSourceForNPCFromNPCAI(), self.Center, Vector2.Normalize((Main.player[self.target].Center - self.Center).RotatedByRandom(0.7853981852531433)) * 7f, ProjectileID.CultistBossLightningOrbArc, self.damage / 4, 0f, Main.myPlayer, (Main.player[self.target].Center - self.Center).ToRotation(), Main.rand.Next(100));
                }
            }
        }
        else if (self.ai[mainAI] == aiHighFloat)
        {
            float floatDistance = 600f;
            float speedCoefficient = 9f;
            float velocityAdjustmentThreshold = 0.3f;
            Vector2 selfCenter = new Vector2(self.position.X + self.width * 0.5f, self.position.Y + self.height * 0.5f);
            float toTargetX = Main.player[self.target].position.X + Main.player[self.target].width / 2 - selfCenter.X;
            float toTargetY = Main.player[self.target].position.Y + Main.player[self.target].height / 2 + floatDistance - selfCenter.Y;
            float speedTowardsTarget = speedCoefficient / (float)Math.Sqrt(toTargetX * toTargetX + toTargetY * toTargetY);
            toTargetX *= speedTowardsTarget;
            toTargetY *= speedTowardsTarget;
            if (self.velocity.X < toTargetX)
            {
                self.velocity.X += velocityAdjustmentThreshold;
                if (self.velocity.X < 0f && toTargetX > 0f)
                {
                    self.velocity.X += velocityAdjustmentThreshold;
                }
            }
            else if (self.velocity.X > toTargetX)
            {
                self.velocity.X -= velocityAdjustmentThreshold;
                if (self.velocity.X > 0f && toTargetX < 0f)
                {
                    self.velocity.X -= velocityAdjustmentThreshold;
                }
            }
            if (self.velocity.Y < toTargetY)
            {
                self.velocity.Y += velocityAdjustmentThreshold;
                if (self.velocity.Y < 0f && toTargetY > 0f)
                {
                    self.velocity.Y += velocityAdjustmentThreshold;
                }
            }
            else if (self.velocity.Y > toTargetY)
            {
                self.velocity.Y -= velocityAdjustmentThreshold;
                if (self.velocity.Y > 0f && toTargetY < 0f)
                {
                    self.velocity.Y -= velocityAdjustmentThreshold;
                }
            }
            self.ai[countAI] += 1f;
            if (self.ai[countAI] >= 70f)
            {
                self.TargetClosest();
                self.ai[mainAI] = aiFastRush;
                self.ai[countAI] = -1f;
                self.ai[rushCountAI] = Main.rand.Next(-3, 1);
                self.netUpdate = true;
            }
        }
        if (isLifePhase3 && self.ai[mainAI] == aiHighFloat)
        {
            self.ai[mainAI] = aiFastRush;
        }
    }
}
