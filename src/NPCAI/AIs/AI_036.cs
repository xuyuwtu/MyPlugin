namespace VBY.NPCAI;

partial class AIs
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
            Vector2 vector68 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num548 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector68.X;
            float num549 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector68.Y;
            float num550 = (float)Math.Sqrt(num548 * num548 + num549 * num549);
            npc.rotation = (float)Math.Atan2(num549, num548) - 1.57f;
            if (Main.netMode != 1)
            {
                npc.localAI[0] += 1f;
                if (npc.localAI[0] > 200f)
                {
                    npc.localAI[0] = 0f;
                    float num551 = 8f;
                    int num552 = 25;
                    int num553 = 100;
                    num550 = num551 / num550;
                    num548 *= num550;
                    num549 *= num550;
                    num548 += Main.rand.Next(-40, 41) * 0.05f;
                    num549 += Main.rand.Next(-40, 41) * 0.05f;
                    vector68.X += num548 * 8f;
                    vector68.Y += num549 * 8f;
                    _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector68.X, vector68.Y, num548, num549, num553, num552, 0f, Main.myPlayer);
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
            Vector2 vector69 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num555 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - 350f - vector69.X;
            float num556 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - 20f - vector69.Y;
            float num557 = (float)Math.Sqrt(num555 * num555 + num556 * num556);
            num557 = 7f / num557;
            num555 *= num557;
            num556 *= num557;
            if (npc.velocity.X > num555)
            {
                if (npc.velocity.X > 0f)
                {
                    npc.velocity.X *= 0.9f;
                }
                npc.velocity.X -= 0.1f;
            }
            if (npc.velocity.X < num555)
            {
                if (npc.velocity.X < 0f)
                {
                    npc.velocity.X *= 0.9f;
                }
                npc.velocity.X += 0.1f;
            }
            if (npc.velocity.Y > num556)
            {
                if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y *= 0.9f;
                }
                npc.velocity.Y -= 0.03f;
            }
            if (npc.velocity.Y < num556)
            {
                if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y *= 0.9f;
                }
                npc.velocity.Y += 0.03f;
            }
            npc.TargetClosest();
            vector69 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            num555 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector69.X;
            num556 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector69.Y;
            num557 = (float)Math.Sqrt(num555 * num555 + num556 * num556);
            npc.rotation = (float)Math.Atan2(num556, num555) - 1.57f;
            if (Main.netMode == 1)
            {
                npc.localAI[0] += 1f;
                if (npc.localAI[0] > 80f)
                {
                    npc.localAI[0] = 0f;
                    float num558 = 10f;
                    int num559 = 25;
                    int num560 = 100;
                    num557 = num558 / num557;
                    num555 *= num557;
                    num556 *= num557;
                    num555 += Main.rand.Next(-40, 41) * 0.05f;
                    num556 += Main.rand.Next(-40, 41) * 0.05f;
                    vector69.X += num555 * 8f;
                    vector69.Y += num556 * 8f;
                    _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector69.X, vector69.Y, num555, num556, num560, num559, 0f, Main.myPlayer);
                }
            }
        }
    }
}
