namespace VBY.NPCTest;

partial class NPCAIs
{
    public static void AI_036(this NPC npc)
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
        if (npc.ai[2] == 0f || npc.ai[2] == 3f)
        {
            if (Main.npc[(int)npc.ai[1]].ai[1] == 3f)
            {
                npc.EncourageDespawn(10);
            }
            if (Main.npc[(int)npc.ai[1]].ai[1] != 0f)
            {
                npc.localAI[0] += 3f;
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
                if (npc.ai[3] >= 800f)
                {
                    npc.ai[2] += 1f;
                    npc.ai[3] = 0f;
                    npc.netUpdate = true;
                }
                if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y - 100f)
                {
                    if (npc.velocity.Y > 0f)
                    {
                        npc.velocity.Y *= 0.96f;
                    }
                    npc.velocity.Y -= 0.1f;
                    if (npc.velocity.Y > 3f)
                    {
                        npc.velocity.Y = 3f;
                    }
                }
                else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 100f)
                {
                    if (npc.velocity.Y < 0f)
                    {
                        npc.velocity.Y *= 0.96f;
                    }
                    npc.velocity.Y += 0.1f;
                    if (npc.velocity.Y < -3f)
                    {
                        npc.velocity.Y = -3f;
                    }
                }
                if (npc.position.X + npc.width / 2 > Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 180f * npc.ai[0])
                {
                    if (npc.velocity.X > 0f)
                    {
                        npc.velocity.X *= 0.96f;
                    }
                    npc.velocity.X -= 0.14f;
                    if (npc.velocity.X > 8f)
                    {
                        npc.velocity.X = 8f;
                    }
                }
                if (npc.position.X + npc.width / 2 < Main.npc[(int)npc.ai[1]].position.X + Main.npc[(int)npc.ai[1]].width / 2 - 180f * npc.ai[0])
                {
                    if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X *= 0.96f;
                    }
                    npc.velocity.X += 0.14f;
                    if (npc.velocity.X < -8f)
                    {
                        npc.velocity.X = -8f;
                    }
                }
            }
            npc.TargetClosest();
            Vector2 npcCenter = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float velocityX = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - npcCenter.X;
            float velocityY = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - npcCenter.Y;
            float num550 = (float)Math.Sqrt(velocityX * velocityX + velocityY * velocityY);
            npc.rotation = (float)Math.Atan2(velocityY, velocityX) - 1.57f;
            if (Main.netMode != 1)
            {
                npc.localAI[0] += 1f;
                if (npc.localAI[0] > 200f)
                {
                    npc.localAI[0] = 0f;
                    float num551 = 8f;
                    int projDamage = 25;
                    int projType = 100;
                    num550 = num551 / num550;
                    velocityX *= num550;
                    velocityY *= num550;
                    velocityX += Main.rand.Next(-40, 41) * 0.05f;
                    velocityY += Main.rand.Next(-40, 41) * 0.05f;
                    npcCenter.X += velocityX * 8f;
                    npcCenter.Y += velocityY * 8f;
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
            if (npc.ai[3] >= 200f)
            {
                npc.localAI[0] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
            Vector2 npcCenter = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float velocityX = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - 350f - npcCenter.X;
            float velocityY = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - 20f - npcCenter.Y;
            float num557 = (float)Math.Sqrt(velocityX * velocityX + velocityY * velocityY);
            num557 = 7f / num557;
            velocityX *= num557;
            velocityY *= num557;
            if (npc.velocity.X > velocityX)
            {
                if (npc.velocity.X > 0f)
                {
                    npc.velocity.X *= 0.9f;
                }
                npc.velocity.X -= 0.1f;
            }
            if (npc.velocity.X < velocityX)
            {
                if (npc.velocity.X < 0f)
                {
                    npc.velocity.X *= 0.9f;
                }
                npc.velocity.X += 0.1f;
            }
            if (npc.velocity.Y > velocityY)
            {
                if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y *= 0.9f;
                }
                npc.velocity.Y -= 0.03f;
            }
            if (npc.velocity.Y < velocityY)
            {
                if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y *= 0.9f;
                }
                npc.velocity.Y += 0.03f;
            }
            npc.TargetClosest();
            npcCenter = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            velocityX = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - npcCenter.X;
            velocityY = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - npcCenter.Y;
            num557 = (float)Math.Sqrt(velocityX * velocityX + velocityY * velocityY);
            npc.rotation = (float)Math.Atan2(velocityY, velocityX) - 1.57f;
            if (Main.netMode == 1)
            {
                npc.localAI[0] += 1f;
                if (npc.localAI[0] > 80f)
                {
                    npc.localAI[0] = 0f;
                    float num558 = 10f;
                    int projDamage = 25;
                    int projType = 100;
                    num557 = num558 / num557;
                    velocityX *= num557;
                    velocityY *= num557;
                    velocityX += Main.rand.Next(-40, 41) * 0.05f;
                    velocityY += Main.rand.Next(-40, 41) * 0.05f;
                    npcCenter.X += velocityX * 8f;
                    npcCenter.Y += velocityY * 8f;
                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npcCenter.X, npcCenter.Y, velocityX, velocityY, projType, projDamage, 0f, Main.myPlayer);

                    var velocity = new Vector2(velocityX, velocityY);
                    npc.NewProjectile(npcCenter, velocity.RotatedByDegress(15), projType, projDamage);
                    npc.NewProjectile(npcCenter, velocity.RotatedByDegress(-15), projType, projDamage);
                }
            }
        }
    }
}
