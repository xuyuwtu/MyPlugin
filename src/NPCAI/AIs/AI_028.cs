namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_028(this NPC npc)
    {
        if (Main.wofNPCIndex < 0)
        {
            npc.active = false;
            return;
        }
        npc.realLife = Main.wofNPCIndex;
        if (Main.npc[Main.wofNPCIndex].life > 0)
        {
            npc.life = Main.npc[Main.wofNPCIndex].life;
        }
        npc.TargetClosest();
        npc.position.X = Main.npc[Main.wofNPCIndex].position.X;
        npc.direction = Main.npc[Main.wofNPCIndex].direction;
        npc.spriteDirection = npc.direction;
        float num380 = (Main.wofDrawAreaBottom + Main.wofDrawAreaTop) / 2;
        num380 = ((!(npc.ai[0] > 0f)) ? ((num380 + Main.wofDrawAreaBottom) / 2f) : ((num380 + Main.wofDrawAreaTop) / 2f));
        num380 -= npc.height / 2;
        if (npc.position.Y > num380 + 1f)
        {
            npc.velocity.Y = -1f;
        }
        else if (npc.position.Y < num380 - 1f)
        {
            npc.velocity.Y = 1f;
        }
        else
        {
            npc.velocity.Y = 0f;
            npc.position.Y = num380;
        }
        if (npc.velocity.Y > 5f)
        {
            npc.velocity.Y = 5f;
        }
        if (npc.velocity.Y < -5f)
        {
            npc.velocity.Y = -5f;
        }
        Vector2 vector39 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
        float num381 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector39.X;
        float num382 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector39.Y;
        float num383 = (float)Math.Sqrt(num381 * num381 + num382 * num382);
        num381 *= num383;
        num382 *= num383;
        bool flag27 = true;
        if (npc.direction > 0)
        {
            if (Main.player[npc.target].position.X + Main.player[npc.target].width / 2 > npc.position.X + npc.width / 2)
            {
                npc.rotation = (float)Math.Atan2(0f - num382, 0f - num381) + 3.14f;
            }
            else
            {
                npc.rotation = 0f;
                flag27 = false;
            }
        }
        else if (Main.player[npc.target].position.X + Main.player[npc.target].width / 2 < npc.position.X + npc.width / 2)
        {
            npc.rotation = (float)Math.Atan2(num382, num381) + 3.14f;
        }
        else
        {
            npc.rotation = 0f;
            flag27 = false;
        }
        if (Main.netMode == 1)
        {
            return;
        }
        int num385 = 4;
        npc.localAI[1] += 1f;
        if (Main.npc[Main.wofNPCIndex].life < Main.npc[Main.wofNPCIndex].lifeMax * 0.75)
        {
            npc.localAI[1] += 1f;
            num385++;
        }
        if (Main.npc[Main.wofNPCIndex].life < Main.npc[Main.wofNPCIndex].lifeMax * 0.5)
        {
            npc.localAI[1] += 1f;
            num385++;
        }
        if (Main.npc[Main.wofNPCIndex].life < Main.npc[Main.wofNPCIndex].lifeMax * 0.25)
        {
            npc.localAI[1] += 1f;
            num385 += 2;
        }
        if (Main.npc[Main.wofNPCIndex].life < Main.npc[Main.wofNPCIndex].lifeMax * 0.1)
        {
            npc.localAI[1] += 2f;
            num385 += 3;
        }
        if (Main.expertMode)
        {
            npc.localAI[1] += 0.5f;
            num385++;
            if (Main.npc[Main.wofNPCIndex].life < Main.npc[Main.wofNPCIndex].lifeMax * 0.1)
            {
                npc.localAI[1] += 2f;
                num385 += 3;
            }
        }
        if (npc.localAI[2] == 0f)
        {
            if (npc.localAI[1] > 600f)
            {
                npc.localAI[2] = 1f;
                npc.localAI[1] = 0f;
            }
        }
        else
        {
            if (!(npc.localAI[1] > 45f) || !Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
            {
                return;
            }
            npc.localAI[1] = 0f;
            npc.localAI[2] += 1f;
            if (npc.localAI[2] >= num385)
            {
                npc.localAI[2] = 0f;
            }
            if (flag27)
            {
                float num386 = 9f;
                int num387 = 11;
                int num388 = 83;
                if (Main.npc[Main.wofNPCIndex].life < Main.npc[Main.wofNPCIndex].lifeMax * 0.5)
                {
                    num387++;
                    num386 += 1f;
                }
                if (Main.npc[Main.wofNPCIndex].life < Main.npc[Main.wofNPCIndex].lifeMax * 0.25)
                {
                    num387++;
                    num386 += 1f;
                }
                if (Main.npc[Main.wofNPCIndex].life < Main.npc[Main.wofNPCIndex].lifeMax * 0.1)
                {
                    num387 += 2;
                    num386 += 2f;
                }
                vector39 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                num381 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector39.X;
                num382 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector39.Y;
                num383 = (float)Math.Sqrt(num381 * num381 + num382 * num382);
                num383 = num386 / num383;
                num381 *= num383;
                num382 *= num383;
                vector39.X += num381;
                vector39.Y += num382;
                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector39.X, vector39.Y, num381, num382, num388, num387, 0f, Main.myPlayer);
            }
        }
    }
}
