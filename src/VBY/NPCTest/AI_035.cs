namespace VBY.NPCTest;

partial class NPCAIs
{
    public static void AI_035(this NPC npc)
    {
        npc.spriteDirection = -(int)npc.ai[0];
        if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].aiStyle != 32)
        {
            npc.ai[2] += 10f;
            if (npc.ai[2] > 50f || Main.netMode != 2)
            {
                npc.life = -1;
                npc.HitEffect();
                npc.active = false;
            }
        }
        if (npc.ai[2] == 0f)
        {
            if (Main.npc[(int)npc.ai[1]].ai[1] == 3f)
            {
                npc.EncourageDespawn(10);
            }
            if (Main.npc[(int)npc.ai[1]].ai[1] != 0f)
            {
                npc.localAI[0] += 2f;
                if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y - 100f)
                {
                    if (npc.velocity.Y > 0f)
                    {
                        npc.velocity.Y *= 0.96f;
                    }
                    npc.velocity.Y -= 0.07f;
                    if (npc.velocity.Y > 6f)
                    {
                        npc.velocity.Y = 6f;
                    }
                }
                else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 100f)
                {
                    if (npc.velocity.Y < 0f)
                    {
                        npc.velocity.Y *= 0.96f;
                    }
                    npc.velocity.Y += 0.07f;
                    if (npc.velocity.Y < -6f)
                    {
                        npc.velocity.Y = -6f;
                    }
                }
                if (npc.position.X + npc.width / 2 > Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 120f * npc.ai[0])
                {
                    if (npc.velocity.X > 0f)
                    {
                        npc.velocity.X *= 0.96f;
                    }
                    npc.velocity.X -= 0.1f;
                    if (npc.velocity.X > 8f)
                    {
                        npc.velocity.X = 8f;
                    }
                }
                if (npc.position.X + npc.width / 2 < Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 120f * npc.ai[0])
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X *= 0.96f;
                    }
                    npc.velocity.X += 0.1f;
                    if (npc.velocity.X < -8f)
                    {
                        npc.velocity.X = -8f;
                    }
                }
            }
            else
            {
                npc.ai[3] += 1f;
                if (npc.ai[3] >= 1100f)
                {
                    npc.localAI[0] = 0f;
                    npc.ai[2] = 1f;
                    npc.ai[3] = 0f;
                    npc.netUpdate = true;
                }
                if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y - 150f)
                {
                    if (npc.velocity.Y > 0f)
                    {
                        npc.velocity.Y *= 0.96f;
                    }
                    npc.velocity.Y -= 0.04f;
                    if (npc.velocity.Y > 3f)
                    {
                        npc.velocity.Y = 3f;
                    }
                }
                else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 150f)
                {
                    if (npc.velocity.Y < 0f)
                    {
                        npc.velocity.Y *= 0.96f;
                    }
                    npc.velocity.Y += 0.04f;
                    if (npc.velocity.Y < -3f)
                    {
                        npc.velocity.Y = -3f;
                    }
                }
                if (npc.position.X + npc.width / 2 > Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 + 200f)
                {
                    if (npc.velocity.X > 0f)
                    {
                        npc.velocity.X *= 0.96f;
                    }
                    npc.velocity.X -= 0.2f;
                    if (npc.velocity.X > 8f)
                    {
                        npc.velocity.X = 8f;
                    }
                }
                if (npc.position.X + npc.width / 2 < Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 + 160f)
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X *= 0.96f;
                    }
                    npc.velocity.X += 0.2f;
                    if (npc.velocity.X < -8f)
                    {
                        npc.velocity.X = -8f;
                    }
                }
            }
            Vector2 npcCenter = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float velocityX = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 200f * npc.ai[0] - npcCenter.X;
            float velocityY = Main.npc[(int)npc.ai[1]].position.Y + 230f - npcCenter.Y;
            float num536 = (float)Math.Sqrt(velocityX * velocityX + velocityY * velocityY);
            npc.rotation = (float)Math.Atan2(velocityY, velocityX) + 1.57f;
            if (Main.netMode != 1)
            {
                npc.localAI[0] += 1f;
                if (npc.localAI[0] > 140f)
                {
                    npc.localAI[0] = 0f;
                    float num537 = 12f;
                    int projDamage = 0;
                    int projType = 102;
                    num536 = num537 / num536;
                    velocityX = (0f - velocityX) * num536;
                    velocityY = (0f - velocityY) * num536;
                    velocityX += Main.rand.Next(-40, 41) * 0.01f;
                    velocityY += Main.rand.Next(-40, 41) * 0.01f;
                    npcCenter.X += velocityX * 4f;
                    npcCenter.Y += velocityY * 4f;
                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npcCenter.X, npcCenter.Y, velocityX, velocityY, projType, projDamage, 0f, Main.myPlayer);
                    
                    var velocity = new Vector2(velocityX, velocityY);
                    npc.NewProjectile(npcCenter, velocity.RotatedByDegress(15), projType, projDamage);
                    npc.NewProjectile(npcCenter, velocity.RotatedByDegress(-15), projType, projDamage);
                }
            }
        }
        else
        {
            if (npc.ai[2] != 1f)
            {
                return;
            }
            npc.ai[3] += 1f;
            if (npc.ai[3] >= 300f)
            {
                npc.localAI[0] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
            Vector2 npcCenter = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float velocityX = Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - npcCenter.X;
            _ = Main.npc[(int)npc.ai[1]].position.Y - npcCenter.Y;
            float velocityY = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - 80f - npcCenter.Y;
            float num543 = (float)Math.Sqrt(velocityX * velocityX + velocityY * velocityY);
            num543 = 6f / num543;
            velocityX *= num543;
            velocityY *= num543;
            if (npc.velocity.X > velocityX)
            {
                if (npc.velocity.X > 0f)
                {
                    npc.velocity.X *= 0.9f;
                }
                npc.velocity.X -= 0.04f;
            }
            if (npc.velocity.X < velocityX)
            {
                if (npc.velocity.X < 0f)
                {
                    npc.velocity.X *= 0.9f;
                }
                npc.velocity.X += 0.04f;
            }
            if (npc.velocity.Y > velocityY)
            {
                if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y *= 0.9f;
                }
                npc.velocity.Y -= 0.08f;
            }
            if (npc.velocity.Y < velocityY)
            {
                if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y *= 0.9f;
                }
                npc.velocity.Y += 0.08f;
            }
            npc.TargetClosest();
            npcCenter = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            velocityX = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - npcCenter.X;
            velocityY = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - npcCenter.Y;
            num543 = (float)Math.Sqrt(velocityX * velocityX + velocityY * velocityY);
            npc.rotation = (float)Math.Atan2(velocityY, velocityX) - 1.57f;
            if (Main.netMode != 1)
            {
                npc.localAI[0] += 1f;
                if (npc.localAI[0] > 40f)
                {
                    npc.localAI[0] = 0f;
                    float num544 = 10f;
                    int projDamage = 0;
                    int projType = 102;
                    num543 = num544 / num543;
                    velocityX *= num543;
                    velocityY *= num543;
                    velocityX += Main.rand.Next(-40, 41) * 0.01f;
                    velocityY += Main.rand.Next(-40, 41) * 0.01f;
                    npcCenter.X += velocityX * 4f;
                    npcCenter.Y += velocityY * 4f;
                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npcCenter.X, npcCenter.Y, velocityX, velocityY, projType, projDamage, 0f, Main.myPlayer);

                    var velocity = new Vector2(velocityX, velocityY);
                    npc.NewProjectile(npcCenter, velocity.RotatedByDegress(15), projType, projDamage);
                    npc.NewProjectile(npcCenter, velocity.RotatedByDegress(-15), projType, projDamage);
                }
            }
        }
    }
}
