namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_040(this NPC npc)
    {
        if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
        {
            npc.TargetClosest();
        }
        float num610 = 2f;
        float num611 = 0.08f;
        if (npc.type == 237)
        {
            num610 = 3f;
            num611 = 0.12f;
        }
        if (npc.type == 531)
        {
            num610 = 4f;
            num611 = 0.16f;
        }
        Vector2 vector77 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
        float num612 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2;
        float num613 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2;
        num612 = (int)(num612 / 8f) * 8;
        num613 = (int)(num613 / 8f) * 8;
        vector77.X = (int)(vector77.X / 8f) * 8;
        vector77.Y = (int)(vector77.Y / 8f) * 8;
        num612 -= vector77.X;
        num613 -= vector77.Y;
        if (npc.confused)
        {
            num612 *= -2f;
            num613 *= -2f;
        }
        float num614 = (float)Math.Sqrt(num612 * num612 + num613 * num613);
        if (num614 == 0f)
        {
            num612 = npc.velocity.X;
            num613 = npc.velocity.Y;
        }
        else
        {
            num614 = num610 / num614;
            num612 *= num614;
            num613 *= num614;
        }
        if (Main.player[npc.target].dead)
        {
            num612 = npc.direction * num610 / 2f;
            num613 = (0f - num610) / 2f;
        }
        npc.spriteDirection = -1;
        if (!Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
        {
            npc.ai[0] += 1f;
            if (npc.ai[0] > 0f)
            {
                npc.velocity.Y += 0.023f;
            }
            else
            {
                npc.velocity.Y -= 0.023f;
            }
            if (npc.ai[0] < -100f || npc.ai[0] > 100f)
            {
                npc.velocity.X += 0.023f;
            }
            else
            {
                npc.velocity.X -= 0.023f;
            }
            if (npc.ai[0] > 200f)
            {
                npc.ai[0] = -200f;
            }
            npc.velocity.X += num612 * 0.007f;
            npc.velocity.Y += num613 * 0.007f;
            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X);
            if (npc.velocity.X > 1.5)
            {
                npc.velocity.X *= 0.9f;
            }
            if (npc.velocity.X < -1.5)
            {
                npc.velocity.X *= 0.9f;
            }
            if (npc.velocity.Y > 1.5)
            {
                npc.velocity.Y *= 0.9f;
            }
            if (npc.velocity.Y < -1.5)
            {
                npc.velocity.Y *= 0.9f;
            }
            if (npc.velocity.X > 3f)
            {
                npc.velocity.X = 3f;
            }
            if (npc.velocity.X < -3f)
            {
                npc.velocity.X = -3f;
            }
            if (npc.velocity.Y > 3f)
            {
                npc.velocity.Y = 3f;
            }
            if (npc.velocity.Y < -3f)
            {
                npc.velocity.Y = -3f;
            }
        }
        else
        {
            if (npc.velocity.X < num612)
            {
                npc.velocity.X += num611;
                if (npc.velocity.X < 0f && num612 > 0f)
                {
                    npc.velocity.X += num611;
                }
            }
            else if (npc.velocity.X > num612)
            {
                npc.velocity.X -= num611;
                if (npc.velocity.X > 0f && num612 < 0f)
                {
                    npc.velocity.X -= num611;
                }
            }
            if (npc.velocity.Y < num613)
            {
                npc.velocity.Y += num611;
                if (npc.velocity.Y < 0f && num613 > 0f)
                {
                    npc.velocity.Y += num611;
                }
            }
            else if (npc.velocity.Y > num613)
            {
                npc.velocity.Y -= num611;
                if (npc.velocity.Y > 0f && num613 < 0f)
                {
                    npc.velocity.Y -= num611;
                }
            }
            npc.rotation = (float)Math.Atan2(num613, num612);
        }
        if (npc.type == 531)
        {
            npc.rotation += (float)Math.PI / 2f;
        }
        float num616 = 0.5f;
        if (npc.collideX)
        {
            npc.netUpdate = true;
            npc.velocity.X = npc.oldVelocity.X * (0f - num616);
            if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 2f)
            {
                npc.velocity.X = 2f;
            }
            if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -2f)
            {
                npc.velocity.X = -2f;
            }
        }
        if (npc.collideY)
        {
            npc.netUpdate = true;
            npc.velocity.Y = npc.oldVelocity.Y * (0f - num616);
            if (npc.velocity.Y > 0f && npc.velocity.Y < 1.5)
            {
                npc.velocity.Y = 2f;
            }
            if (npc.velocity.Y < 0f && npc.velocity.Y > -1.5)
            {
                npc.velocity.Y = -2f;
            }
        }
        if (((npc.velocity.X > 0f && npc.oldVelocity.X < 0f) || (npc.velocity.X < 0f && npc.oldVelocity.X > 0f) || (npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f) || (npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f)) && !npc.justHit)
        {
            npc.netUpdate = true;
        }
        if (Main.netMode == 1)
        {
            return;
        }
        if (Main.netMode != 1 && Main.expertMode && npc.target >= 0 && (npc.type == 163 || npc.type == 238 || npc.type == 236 || npc.type == 237) && Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
        {
            npc.localAI[0] += 1f;
            if (npc.justHit)
            {
                npc.localAI[0] -= Main.rand.Next(20, 60);
                if (npc.localAI[0] < 0f)
                {
                    npc.localAI[0] = 0f;
                }
            }
            if (npc.localAI[0] > Main.rand.Next(180, 900))
            {
                npc.localAI[0] = 0f;
                Vector2 vector78 = Main.player[npc.target].Center - npc.Center;
                vector78.Normalize();
                vector78 *= 8f;
                int attackDamage_ForProjectiles8 = npc.GetAttackDamage_ForProjectiles(18f, 18f);
                _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X, npc.Center.Y, vector78.X, vector78.Y, 472, attackDamage_ForProjectiles8, 0f, Main.myPlayer);
            }
        }
        if (!npc.NPCCanStickToWalls())
        {
            int newType = npc.type;
            switch (npc.type)
            {
                case 165:
                    newType = 164;
                    break;
                case 237:
                    newType = 236;
                    break;
                case 238:
                    newType = 163;
                    break;
                case 240:
                    newType = 239;
                    break;
                case 531:
                    newType = 530;
                    break;
            }
            npc.Transform(newType);
        }
    }
}
