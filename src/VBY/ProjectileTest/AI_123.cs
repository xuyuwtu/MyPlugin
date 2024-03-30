namespace VBY.ProjectileTest;

partial class ProjectileAIs
{
    public static void AI_123(Projectile proj)
    {
        bool isMoonlordTurret = proj.type == 641;
        bool isRainbowCrystal = proj.type == 643;
        float attachDistance = 1000f;
        proj.velocity = Vector2.Zero;
        if (isMoonlordTurret)
        {
            proj.alpha -= 5;
            if (proj.alpha < 0)
            {
                proj.alpha = 0;
            }
            if (proj.direction == 0)
            {
                proj.direction = Main.player[proj.owner].direction;
            }
            proj.rotation -= proj.direction * ((float)Math.PI * 2f) / 120f;
            proj.scale = proj.Opacity;
            Lighting.AddLight(proj.Center, new Vector3(0.3f, 0.9f, 0.7f) * proj.Opacity);
        }
        if (isRainbowCrystal)
        {
            proj.alpha -= 5;
            if (proj.alpha < 0)
            {
                proj.alpha = 0;
            }
            if (proj.direction == 0)
            {
                proj.direction = Main.player[proj.owner].direction;
            }
            if (++proj.frameCounter >= 3)
            {
                proj.frameCounter = 0;
                if (++proj.frame >= Main.projFrames[proj.type])
                {
                    proj.frame = 0;
                }
            }
            proj.localAI[0]++;
            if (proj.localAI[0] >= 60f)
            {
                proj.localAI[0] = 0f;
            }
        }
        if (proj.ai[0] < 0f)
        {
            proj.ai[0]++;
            if (isMoonlordTurret)
            {
                proj.ai[1] -= proj.direction * ((float)Math.PI / 8f) / 50f;
            }
        }
        if (proj.ai[0] == 0f)
        {
            int attackTargetWhoAmi = -1;
            float distance = attachDistance;
            NPC attackTargetNpc = proj.OwnerMinionAttackTargetNPC;
            if (attackTargetNpc != null && attackTargetNpc.CanBeChasedBy(proj))
            {
                float distanceToTarget = proj.Distance(attackTargetNpc.Center);
                if (distanceToTarget < distance && Collision.CanHitLine(proj.Center, 0, 0, attackTargetNpc.Center, 0, 0))
                {
                    distance = distanceToTarget;
                    attackTargetWhoAmi = attackTargetNpc.whoAmI;
                }
            }
            if (attackTargetWhoAmi < 0)
            {
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(proj))
                    {
                        float distanceToNpc = proj.Distance(npc.Center);
                        if (distanceToNpc < distance && Collision.CanHitLine(proj.Center, 0, 0, npc.Center, 0, 0))
                        {
                            distance = distanceToNpc;
                            attackTargetWhoAmi = i;
                        }
                    }
                }
            }
            if (attackTargetWhoAmi != -1)
            {
                proj.ai[0] = 1f;
                proj.ai[1] = attackTargetWhoAmi;
                proj.netUpdate = true;
                return;
            }
        }
        if (!(proj.ai[0] > 0f))
        {
            return;
        }
        int attachTargetIndex = (int)proj.ai[1];
        if (!Main.npc[attachTargetIndex].CanBeChasedBy(proj))
        {
            proj.ai[0] = 0f;
            proj.ai[1] = 0f;
            proj.netUpdate = true;
            return;
        }
        proj.ai[0]++;
        float attachCount = 30f;
        if (isMoonlordTurret)
        {
            attachCount = 10f;
        }
        if (isRainbowCrystal)
        {
            attachCount = 5f;
        }
        if (!(proj.ai[0] >= attachCount))
        {
            return;
        }
        Vector2 toTargetVector2 = proj.DirectionTo(Main.npc[attachTargetIndex].Center);
        if (toTargetVector2.HasNaNs())
        {
            toTargetVector2 = Vector2.UnitY;
        }
        float toTargetRotation = toTargetVector2.ToRotation();
        int direction = ((toTargetVector2.X > 0f) ? 1 : (-1));
        if (isMoonlordTurret)
        {
            proj.direction = direction;
            proj.ai[0] = -20f;
            proj.ai[1] = toTargetRotation + direction * (float)Math.PI / 6f;
            proj.netUpdate = true;
            if (proj.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(proj.GetProjectileSource_FromThis(), proj.Center.X, proj.Center.Y, toTargetVector2.X, toTargetVector2.Y, 642, proj.damage, proj.knockBack, proj.owner, proj.ai[1], proj.whoAmI);
            }
        }
        if (!isRainbowCrystal)
        {
            return;
        }
        proj.direction = direction;
        proj.ai[0] = -20f;
        proj.netUpdate = true;
        if (proj.owner != Main.myPlayer)
        {
            return;
        }
        NPC targetNpc = Main.npc[attachTargetIndex];
        Vector2 simpleOffsetToTarget = targetNpc.position + targetNpc.Size * Terraria.Utils.RandomVector2(Main.rand, 0f, 1f) - proj.Center;
        for (int i = 0; i < 3; i++)
        {
            Vector2 attackTargetPosition = proj.Center + simpleOffsetToTarget;
            Vector2 targetVelocityOffset = targetNpc.velocity * 30f;
            attackTargetPosition += targetVelocityOffset;
            float num954 = MathHelper.Lerp(0.1f, 0.75f, Terraria.Utils.GetLerpValue(800f, 200f, proj.Distance(attackTargetPosition)));
            if (i > 0)
            {
                attackTargetPosition = proj.Center + simpleOffsetToTarget.RotatedByRandom(0.7853981852531433) * (Main.rand.NextFloat() * num954 + 0.5f);
            }
            float color = Main.rgbToHsl(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB)).X;
            Projectile.NewProjectile(proj.GetProjectileSource_FromThis(), attackTargetPosition.X, attackTargetPosition.Y, 0f, 0f, 644, proj.damage, proj.knockBack, proj.owner, color, proj.whoAmI);
        }
    }
}
