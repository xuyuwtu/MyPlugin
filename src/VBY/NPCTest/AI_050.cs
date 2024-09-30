namespace VBY.NPCTest;

partial class NPCAIs
{
    public static void AI_050(NPC self)
    {
        self.EncourageDespawn(5);
        if (self.type == NPCID.FungiSpore)
        {
            self.noTileCollide = false;
            if (self.collideX || self.collideY)
            {
                self.life = 0;
                self.HitEffect(0, 100.0);
                self.checkDead();
                return;
            }
        }
        else
        {
            self.noTileCollide = true;
        }
        self.velocity.Y += 0.02f;
        self.TargetClosest();
        if(self.type == NPCID.Spore)
        {
            ref var localAI0 = ref self.localAI[0];
            localAI0++;
            var interval = 60f;
            if(NPC.plantBoss != -1 && Main.npc[NPC.plantBoss].active && (Main.npc[NPC.plantBoss].Center - self.Center).Length() > 16 * 10)
            {
                interval = 30f;
            }
            if (localAI0 >= interval)
            {
                localAI0 = 0;
                Projectile.NewProjectile(self.GetSpawnSourceForNPCFromNPCAI(), self.Center, self.GetToTargetVector2().Normalize(17), ProjectileID.PoisonSeedPlantera, 27, 0);
            }
        }
        if (self.velocity.Y < 0f && Main.player[self.target].position.Y > self.position.Y + 100f)
        {
            self.velocity.Y *= 0.95f;
        }
        if (self.velocity.Y > 1f)
        {
            self.velocity.Y = 1f;
        }
        if (self.position.X + self.width < Main.player[self.target].position.X)
        {
            if (self.velocity.X < 0f)
            {
                self.velocity.X *= 0.98f;
            }
            if (Main.expertMode && self.velocity.X < 0f)
            {
                self.velocity.X *= 0.98f;
            }
            self.velocity.X += 0.1f;
            if (Main.expertMode)
            {
                self.velocity.X += 0.1f;
            }
        }
        else if (self.position.X > Main.player[self.target].position.X + Main.player[self.target].width)
        {
            if (self.velocity.X > 0f)
            {
                self.velocity.X *= 0.98f;
            }
            if (Main.expertMode && self.velocity.X > 0f)
            {
                self.velocity.X *= 0.98f;
            }
            self.velocity.X -= 0.1f;
            if (Main.expertMode)
            {
                self.velocity.X -= 0.1f;
            }
        }
        if (self.velocity.X > 5f || self.velocity.X < -5f)
        {
            self.velocity.X *= 0.97f;
        }
        self.rotation = self.velocity.X * 0.2f;
    }
}
