using VBY.NPCAI;

namespace VBY.NPCTest;

public static partial class NPCAIs
{
    public static void AI_015(this NPC self)
    {
        const int netClientMode = 1;
        const int netServerMode = 2;

        const int countAI = 0;
        const int mainAI = 1;
        const int tooFarCountAI = 2;
        const int lifeAI = 3;

        const int countLocalAI = 0;
        const int teleportXLocalAI = 1;
        const int teleportYLocalAI = 2;
        const int initLocalAI = 3;

        const int isHighJumpLocalAI = 4;
        const int highJumpCountLocalAI = 5;

#pragma warning disable CS0219 // 变量已被赋值，但从未使用过它的值
        const int aiNormalJump1 = 0;
        const int aiNormalJump2 = 1;
        const int aiFastJump = 2;
        const int aiHighJump = 3;
        const int aiUnknown = 4;
        const int aiTeleporting = 5;
        const int aiFindTarget = 6;
#pragma warning restore CS0219 // 变量已被赋值，但从未使用过它的值

        float aiScaleMultiple = 1f;
        float lifeScaleMultiple = 1f;
        bool isInit = false;
        bool hasTarget = false;
        bool isTeleported = false;
        float goodWorldScaleMultipleBase = 2f;
        if (Main.getGoodWorld)
        {
            lifeScaleMultiple *= goodWorldScaleMultipleBase - (1f - self.life / (float)self.lifeMax);
        }
        self.aiAction = 0;
        if (self.ai[lifeAI] == 0f && self.life > 0)
        {
            self.ai[lifeAI] = self.lifeMax;
        }
        if (self.localAI[initLocalAI] == 0f)
        {
            self.localAI = new float[6];
            self.localAI[initLocalAI] = 1f;
            isInit = true;
            if (Main.netMode != netClientMode)
            {
                self.ai[countAI] = -100f;
                self.TargetClosest();
                self.netUpdate = true;
            }
        }
        int searchDistance = 3000;
        if (Main.player[self.target].dead || Vector2.Distance(self.Center, Main.player[self.target].Center) > searchDistance)
        {
            self.TargetClosest();
            if (Main.player[self.target].dead || Vector2.Distance(self.Center, Main.player[self.target].Center) > searchDistance)
            {
                self.EncourageDespawn(10);
                if (Main.player[self.target].Center.X < self.Center.X)
                {
                    self.direction = 1;
                }
                else
                {
                    self.direction = -1;
                }
                if (Main.netMode != netClientMode && self.ai[mainAI] == aiTeleporting)
                {
                    self.netUpdate = true;
                    self.ai[tooFarCountAI] = 0f;
                    self.ai[countAI] = 0f;
                    self.ai[mainAI] = aiTeleporting;
                    self.localAI[teleportXLocalAI] = Main.maxTilesX * 16;
                    self.localAI[teleportYLocalAI] = Main.maxTilesY * 16;
                }
            }
        }
        if (!Main.player[self.target].dead && self.timeLeft > 10 && self.ai[tooFarCountAI] >= 300f && self.ai[mainAI] < aiTeleporting && self.velocity.Y == 0f)
        {
            self.ai[tooFarCountAI] = 0f;
            self.ai[countAI] = 0f;
            self.ai[mainAI] = aiTeleporting;
            if (Main.netMode != netClientMode)
            {
                self.TargetClosest(faceTarget: false);
                Point centerPoint = self.Center.ToTileCoordinates();
                Point targetPoint = Main.player[self.target].Center.ToTileCoordinates();
                Vector2 toTargetDistance = Main.player[self.target].Center - self.Center;
                int teleportMaxDistance = 10;
                int teleportMinDistanceOfSelf = 0;
                int teleportMinDistance = 3;
                int searchCount = 0;
                bool searchCompleted = false;
                if (self.localAI[countLocalAI] >= 360f || toTargetDistance.Length() > 2000f)
                {
                    if (self.localAI[countLocalAI] >= 360f)
                    {
                        self.localAI[countLocalAI] = 360f;
                    }
                    searchCompleted = true;
                    searchCount = 100;
                }
                while (!searchCompleted && searchCount < 100)
                {
                    searchCount++;
                    int teleportTargetTileX = Main.rand.Next(targetPoint.X - teleportMaxDistance, targetPoint.X + teleportMaxDistance + 1);
                    int teleportTargetTileY = Main.rand.Next(targetPoint.Y - teleportMaxDistance, targetPoint.Y + 1);
                    if ((teleportTargetTileY >= targetPoint.Y - teleportMinDistance && teleportTargetTileY <= targetPoint.Y + teleportMinDistance && teleportTargetTileX >= targetPoint.X - teleportMinDistance && teleportTargetTileX <= targetPoint.X + teleportMinDistance) || (teleportTargetTileY >= centerPoint.Y - teleportMinDistanceOfSelf && teleportTargetTileY <= centerPoint.Y + teleportMinDistanceOfSelf && teleportTargetTileX >= centerPoint.X - teleportMinDistanceOfSelf && teleportTargetTileX <= centerPoint.X + teleportMinDistanceOfSelf) || Main.tile[teleportTargetTileX, teleportTargetTileY].nactive())
                    {
                        continue;
                    }
                    int searchTlieY = teleportTargetTileY;
                    int searchTileYOffset = 0;
                    if (Main.tile[teleportTargetTileX, searchTlieY].nactive() && Main.tileSolid[Main.tile[teleportTargetTileX, searchTlieY].type] && !Main.tileSolidTop[Main.tile[teleportTargetTileX, searchTlieY].type])
                    {
                        searchTileYOffset = 1;
                    }
                    else
                    {
                        for (; searchTileYOffset < 150 && searchTlieY + searchTileYOffset < Main.maxTilesY; searchTileYOffset++)
                        {
                            int checkTileY = searchTlieY + searchTileYOffset;
                            if (Main.tile[teleportTargetTileX, checkTileY].nactive() && Main.tileSolid[Main.tile[teleportTargetTileX, checkTileY].type] && !Main.tileSolidTop[Main.tile[teleportTargetTileX, checkTileY].type])
                            {
                                searchTileYOffset--;
                                break;
                            }
                        }
                    }
                    teleportTargetTileY += searchTileYOffset;
                    bool canTeleport = true;
                    if (canTeleport && Main.tile[teleportTargetTileX, teleportTargetTileY].lava())
                    {
                        canTeleport = false;
                    }
                    if (canTeleport && !Collision.CanHitLine(self.Center, 0, 0, Main.player[self.target].Center, 0, 0))
                    {
                        canTeleport = false;
                    }
                    if (canTeleport)
                    {
                        self.localAI[teleportXLocalAI] = teleportTargetTileX * 16 + 8;
                        self.localAI[teleportYLocalAI] = teleportTargetTileY * 16 + 16;
                        searchCompleted = true;
                        break;
                    }
                }
                if (searchCount >= 100)
                {
                    Vector2 bottom = Main.player[Player.FindClosest(self.position, self.width, self.height)].Bottom;
                    self.localAI[teleportXLocalAI] = bottom.X;
                    self.localAI[teleportYLocalAI] = bottom.Y;
                }
            }
        }
        if (!Collision.CanHitLine(self.Center, 0, 0, Main.player[self.target].Center, 0, 0) || Math.Abs(self.Top.Y - Main.player[self.target].Bottom.Y) > 160f)
        {
            self.ai[tooFarCountAI] += 1f;
            if (Main.netMode != netClientMode)
            {
                self.localAI[countLocalAI] += 1f;
            }
        }
        else if (Main.netMode != netClientMode)
        {
            self.localAI[countLocalAI] -= 1f;
            if (self.localAI[countLocalAI] < 0f)
            {
                self.localAI[countLocalAI] = 0f;
            }
        }
        if (self.timeLeft < 10 && (self.ai[countAI] != 0f || self.ai[mainAI] != 0f))
        {
            self.ai[countAI] = 0f;
            self.ai[mainAI] = 0f;
            self.netUpdate = true;
            hasTarget = false;
        }
        if (self.ai[mainAI] == aiTeleporting)
        {
            hasTarget = true;
            self.aiAction = 1;
            self.ai[countAI] += 1f;
            aiScaleMultiple = 0.5f + MathHelper.Clamp((60f - self.ai[countAI]) / 60f, 0f, 1f) * 0.5f;
            if (self.ai[countAI] % 10 == 0)
            {
                Projectile.NewProjectile(self.GetSpawnSourceForNPCFromNPCAI(), self.TopLeft, new(-1, 0), ProjectileID.DemonSickle, self.damage / 4, 0);
                Projectile.NewProjectile(self.GetSpawnSourceForNPCFromNPCAI(), self.TopRight, new(1, 0), ProjectileID.DemonSickle, self.damage / 4, 0);
            }
            if (self.ai[countAI] >= 60f)
            {
                isTeleported = true;
            }
            if (self.ai[countAI] >= 60f && Main.netMode != netClientMode)
            {
                self.Bottom = new Vector2(self.localAI[teleportXLocalAI], self.localAI[teleportYLocalAI]);
                self.ai[mainAI] = aiFindTarget;
                self.ai[countAI] = 0f;
                self.netUpdate = true;
            }
            if (Main.netMode == netClientMode && self.ai[countAI] >= 120f)
            {
                self.ai[mainAI] = aiFindTarget;
                self.ai[countAI] = 0f;
            }
        }
        else if (self.ai[mainAI] == aiFindTarget)
        {
            hasTarget = true;
            self.aiAction = 0;
            self.ai[countAI] += 1f;
            aiScaleMultiple = 0.5f + MathHelper.Clamp(self.ai[countAI] / 30f, 0f, 1f) * 0.5f;
            if (self.ai[countAI] >= 30f && Main.netMode != netClientMode)
            {
                self.ai[mainAI] = 0f;
                self.ai[countAI] = 0f;
                self.netUpdate = true;
                self.TargetClosest();
            }
            if (Main.netMode == netClientMode && self.ai[countAI] >= 60f)
            {
                self.ai[mainAI] = 0f;
                self.ai[countAI] = 0f;
                self.TargetClosest();
            }
        }
        self.dontTakeDamage = (self.hide = isTeleported);
        if (self.velocity.Y == 0f)
        {
            self.velocity.X *= 0.8f;
            if (self.velocity.X > -0.1 && self.velocity.X < 0.1)
            {
                self.velocity.X = 0f;
            }
            if (!hasTarget)
            {
                self.ai[countAI] += 2f;
                if (self.life < self.lifeMax * 0.8)
                {
                    self.ai[countAI] += 1f;
                }
                if (self.life < self.lifeMax * 0.6)
                {
                    self.ai[countAI] += 1f;
                }
                if (self.life < self.lifeMax * 0.4)
                {
                    self.ai[countAI] += 2f;
                }
                if (self.life < self.lifeMax * 0.2)
                {
                    self.ai[countAI] += 3f;
                }
                if (self.life < self.lifeMax * 0.1)
                {
                    self.ai[countAI] += 4f;
                }
                if (self.ai[countAI] >= 0f)
                {
                    self.netUpdate = true;
                    self.TargetClosest();
                    if (self.ai[mainAI] == aiHighJump)
                    {
                        self.velocity.Y = -16f;
                        self.velocity.X += 4.5f * self.direction;
                        self.ai[countAI] = -200f;
                        self.ai[mainAI] = 0f;
                        self.localAI[isHighJumpLocalAI] = 1;
                        self.localAI[highJumpCountLocalAI] = 0;
                    }
                    else if (self.ai[mainAI] == aiFastJump)
                    {
                        self.velocity.Y = -6f;
                        self.velocity.X += 5.5f * self.direction;
                        self.ai[countAI] = -120f;
                        self.ai[mainAI] += 1f;
                        self.localAI[isHighJumpLocalAI] = 0;
                    }
                    else
                    {
                        self.velocity.Y = -8f;
                        self.velocity.X += 5f * self.direction;
                        self.ai[countAI] = -120f;
                        self.ai[mainAI] += 1f;
                        self.localAI[isHighJumpLocalAI] = 0;
                    }
                }
                else if (self.ai[countAI] >= -30f)
                {
                    self.aiAction = 1;
                }
            }
        }
        else if (self.target < 255)
        {
            float velocityBase = 3f;
            if (Main.getGoodWorld)
            {
                velocityBase = 6f;
            }
            if ((self.direction == 1 && self.velocity.X < velocityBase) || (self.direction == -1 && self.velocity.X > -velocityBase))
            {
                if ((self.direction == -1 && self.velocity.X < 0.1) || (self.direction == 1 && self.velocity.X > -0.1))
                {
                    self.velocity.X += 0.2f * self.direction;
                }
                else
                {
                    self.velocity.X *= 0.93f;
                }
            }
            if (self.ai[mainAI] == 0 && self.localAI[isHighJumpLocalAI] == 1)
            {
                self.localAI[highJumpCountLocalAI]++;
                if (self.localAI[highJumpCountLocalAI] % 15 == 0)
                {
                    Projectile.NewProjectile(self.GetSpawnSourceForNPCFromNPCAI(), self.Center, Vector2.Zero, ProjectileID.IceSpike, self.damage / 5, 0);
                }
            }
        }
        if (self.life <= 0)
        {
            return;
        }
        float newScale = (self.life / (float)self.lifeMax * 0.5f + 0.75f) * aiScaleMultiple * lifeScaleMultiple;
        if (newScale != self.scale || isInit)
        {
            self.position.X += self.width / 2;
            self.position.Y += self.height;
            self.scale = newScale;
            self.width = (int)(98f * self.scale);
            self.height = (int)(92f * self.scale);
            self.position.X -= self.width / 2;
            self.position.Y -= self.height;
        }
        if (Main.netMode == netClientMode)
        {
            return;
        }
        int spawnSlimeNeedLostLife = (int)(self.lifeMax * 0.01);
        if (!(self.life + spawnSlimeNeedLostLife < self.ai[lifeAI]))
        {
            return;
        }
        self.ai[lifeAI] = self.life;
        int spawnCount = Main.rand.Next(1, 4);
        for (int i = 0; i < spawnCount; i++)
        {
            int spawnX = (int)(self.position.X + Main.rand.Next(self.width - 32));
            int spawnY = (int)(self.position.Y + Main.rand.Next(self.height - 32));
            int newNpcType = NPCID.BlueSlime;
            if (Main.expertMode && Main.rand.Next(4) == 0)
            {
                newNpcType = NPCID.SlimeSpiked;
            }
            if (Main.masterMode && Main.rand.Next(2) == 0)
            {
                newNpcType = Utils.SelectRandom(Main.rand, NPCID.SpikedJungleSlime, NPCID.SpikedIceSlime);
            }
            int npcIndex = NPC.NewNPC(self.GetSpawnSourceForProjectileNPC(), spawnX, spawnY, newNpcType);
            Main.npc[npcIndex].SetDefaults(newNpcType);
            Main.npc[npcIndex].velocity.X = Main.rand.Next(-15, 16) * 0.1f;
            Main.npc[npcIndex].velocity.Y = Main.rand.Next(-30, 1) * 0.1f;
            Main.npc[npcIndex].ai[0] = -1000 * Main.rand.Next(3);
            Main.npc[npcIndex].ai[1] = 0f;
            if (Main.netMode == netServerMode && npcIndex < Main.maxNPCs)
            {
                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcIndex);
            }
        }
    }
}
