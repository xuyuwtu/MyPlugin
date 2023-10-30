namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_014(this NPC npc)
    {
        npc.noGravity = true;
        if (npc.collideX)
        {
            npc.velocity.X = npc.oldVelocity.X * -0.5f;
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
            npc.velocity.Y = npc.oldVelocity.Y * -0.5f;
            if (npc.velocity.Y > 0f && npc.velocity.Y < 1f)
            {
                npc.velocity.Y = 1f;
            }
            if (npc.velocity.Y < 0f && npc.velocity.Y > -1f)
            {
                npc.velocity.Y = -1f;
            }
        }
        if (npc.type == 226)
        {
            int num201 = 1;
            int num202 = 1;
            if (npc.velocity.X < 0f)
            {
                num201 = -1;
            }
            if (npc.velocity.Y < 0f)
            {
                num202 = -1;
            }
            npc.TargetClosest();
            if (!Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
            {
                npc.direction = num201;
                npc.directionY = num202;
            }
        }
        else
        {
            npc.TargetClosest();
        }
        if (npc.type == 158)
        {
            if (npc.position.Y < Main.worldSurface * 16.0 && Main.IsItDay() && !Main.eclipse)
            {
                npc.directionY = -1;
                npc.direction *= -1;
            }
            if (npc.direction == -1 && npc.velocity.X > -7f)
            {
                npc.velocity.X -= 0.2f;
                if (npc.velocity.X > 4f)
                {
                    npc.velocity.X -= 0.1f;
                }
                else if (npc.velocity.X > 0f)
                {
                    npc.velocity.X += 0.05f;
                }
                if (npc.velocity.X < -7f)
                {
                    npc.velocity.X = -7f;
                }
            }
            else if (npc.direction == 1 && npc.velocity.X < 7f)
            {
                npc.velocity.X += 0.2f;
                if (npc.velocity.X < -4f)
                {
                    npc.velocity.X += 0.1f;
                }
                else if (npc.velocity.X < 0f)
                {
                    npc.velocity.X -= 0.05f;
                }
                if (npc.velocity.X > 7f)
                {
                    npc.velocity.X = 7f;
                }
            }
            if (npc.directionY == -1 && npc.velocity.Y > -7f)
            {
                npc.velocity.Y -= 0.2f;
                if (npc.velocity.Y > 4f)
                {
                    npc.velocity.Y -= 0.1f;
                }
                else if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y += 0.05f;
                }
                if (npc.velocity.Y < -7f)
                {
                    npc.velocity.Y = -7f;
                }
            }
            else if (npc.directionY == 1 && npc.velocity.Y < 7f)
            {
                npc.velocity.Y += 0.2f;
                if (npc.velocity.Y < -4f)
                {
                    npc.velocity.Y += 0.1f;
                }
                else if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y -= 0.05f;
                }
                if (npc.velocity.Y > 7f)
                {
                    npc.velocity.Y = 7f;
                }
            }
        }
        else if (npc.type == 226)
        {
            if (npc.direction == -1 && npc.velocity.X > -4f)
            {
                npc.velocity.X -= 0.2f;
                if (npc.velocity.X > 4f)
                {
                    npc.velocity.X -= 0.1f;
                }
                else if (npc.velocity.X > 0f)
                {
                    npc.velocity.X += 0.05f;
                }
                if (npc.velocity.X < -4f)
                {
                    npc.velocity.X = -4f;
                }
            }
            else if (npc.direction == 1 && npc.velocity.X < 4f)
            {
                npc.velocity.X += 0.2f;
                if (npc.velocity.X < -4f)
                {
                    npc.velocity.X += 0.1f;
                }
                else if (npc.velocity.X < 0f)
                {
                    npc.velocity.X -= 0.05f;
                }
                if (npc.velocity.X > 4f)
                {
                    npc.velocity.X = 4f;
                }
            }
            if (npc.directionY == -1 && npc.velocity.Y > -2.5)
            {
                npc.velocity.Y -= 0.1f;
                if (npc.velocity.Y > 2.5)
                {
                    npc.velocity.Y -= 0.05f;
                }
                else if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y += 0.03f;
                }
                if (npc.velocity.Y < -2.5)
                {
                    npc.velocity.Y = -2.5f;
                }
            }
            else if (npc.directionY == 1 && npc.velocity.Y < 2.5)
            {
                npc.velocity.Y += 0.1f;
                if (npc.velocity.Y < -2.5)
                {
                    npc.velocity.Y += 0.05f;
                }
                else if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y -= 0.03f;
                }
                if (npc.velocity.Y > 2.5)
                {
                    npc.velocity.Y = 2.5f;
                }
            }
        }
        else if (npc.type == 660)
        {
            float num203 = 0.1f;
            float num204 = 0.04f;
            float num205 = 4f;
            float num206 = 1.5f;
            int num207 = npc.type;
            if (num207 == 660)
            {
                num203 = 0.35f;
                num204 = 0.3f;
                num205 = 6f;
                num206 = 5f;
            }
            if (npc.direction == -1 && npc.velocity.X > 0f - num205)
            {
                npc.velocity.X -= num203;
                if (npc.velocity.X > num205)
                {
                    npc.velocity.X -= num203;
                }
                else if (npc.velocity.X > 0f)
                {
                    npc.velocity.X += num203 * 0.5f;
                }
                if (npc.velocity.X < 0f - num205)
                {
                    npc.velocity.X = 0f - num205;
                }
            }
            else if (npc.direction == 1 && npc.velocity.X < num205)
            {
                npc.velocity.X += num203;
                if (npc.velocity.X < 0f - num205)
                {
                    npc.velocity.X += num203;
                }
                else if (npc.velocity.X < 0f)
                {
                    npc.velocity.X -= num203 * 0.5f;
                }
                if (npc.velocity.X > num205)
                {
                    npc.velocity.X = num205;
                }
            }
            if (npc.directionY == -1 && npc.velocity.Y > 0f - num206)
            {
                npc.velocity.Y -= num204;
                if (npc.velocity.Y > num206)
                {
                    npc.velocity.Y -= num204;
                }
                else if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y += num204 * 0.75f;
                }
                if (npc.velocity.Y < 0f - num206)
                {
                    npc.velocity.Y = 0f - num206;
                }
            }
            else if (npc.directionY == 1 && npc.velocity.Y < num206)
            {
                npc.velocity.Y += num204;
                if (npc.velocity.Y < 0f - num206)
                {
                    npc.velocity.Y += num204;
                }
                else if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y -= num204 * 0.75f;
                }
                if (npc.velocity.Y > num206)
                {
                    npc.velocity.Y = num206;
                }
            }
        }
        else
        {
            if (npc.direction == -1 && npc.velocity.X > -4f)
            {
                npc.velocity.X -= 0.1f;
                if (npc.velocity.X > 4f)
                {
                    npc.velocity.X -= 0.1f;
                }
                else if (npc.velocity.X > 0f)
                {
                    npc.velocity.X += 0.05f;
                }
                if (npc.velocity.X < -4f)
                {
                    npc.velocity.X = -4f;
                }
            }
            else if (npc.direction == 1 && npc.velocity.X < 4f)
            {
                npc.velocity.X += 0.1f;
                if (npc.velocity.X < -4f)
                {
                    npc.velocity.X += 0.1f;
                }
                else if (npc.velocity.X < 0f)
                {
                    npc.velocity.X -= 0.05f;
                }
                if (npc.velocity.X > 4f)
                {
                    npc.velocity.X = 4f;
                }
            }
            if (npc.directionY == -1 && npc.velocity.Y > -1.5)
            {
                npc.velocity.Y -= 0.04f;
                if (npc.velocity.Y > 1.5)
                {
                    npc.velocity.Y -= 0.05f;
                }
                else if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y += 0.03f;
                }
                if (npc.velocity.Y < -1.5)
                {
                    npc.velocity.Y = -1.5f;
                }
            }
            else if (npc.directionY == 1 && npc.velocity.Y < 1.5)
            {
                npc.velocity.Y += 0.04f;
                if (npc.velocity.Y < -1.5)
                {
                    npc.velocity.Y += 0.05f;
                }
                else if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y -= 0.03f;
                }
                if (npc.velocity.Y > 1.5)
                {
                    npc.velocity.Y = 1.5f;
                }
            }
        }
        if (npc.type == 49 || npc.type == 51 || npc.type == 60 || npc.type == 62 || npc.type == 66 || npc.type == 93 || npc.type == 137 || npc.type == 150 || npc.type == 151 || npc.type == 152 || npc.type == 634)
        {
            if (npc.wet)
            {
                if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y *= 0.95f;
                }
                npc.velocity.Y -= 0.5f;
                if (npc.velocity.Y < -4f)
                {
                    npc.velocity.Y = -4f;
                }
                npc.TargetClosest();
            }
            if (npc.type == 60)
            {
                if (npc.direction == -1 && npc.velocity.X > -4f)
                {
                    npc.velocity.X -= 0.1f;
                    if (npc.velocity.X > 4f)
                    {
                        npc.velocity.X -= 0.07f;
                    }
                    else if (npc.velocity.X > 0f)
                    {
                        npc.velocity.X += 0.03f;
                    }
                    if (npc.velocity.X < -4f)
                    {
                        npc.velocity.X = -4f;
                    }
                }
                else if (npc.direction == 1 && npc.velocity.X < 4f)
                {
                    npc.velocity.X += 0.1f;
                    if (npc.velocity.X < -4f)
                    {
                        npc.velocity.X += 0.07f;
                    }
                    else if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X -= 0.03f;
                    }
                    if (npc.velocity.X > 4f)
                    {
                        npc.velocity.X = 4f;
                    }
                }
                if (npc.directionY == -1 && npc.velocity.Y > -1.5)
                {
                    npc.velocity.Y -= 0.04f;
                    if (npc.velocity.Y > 1.5)
                    {
                        npc.velocity.Y -= 0.03f;
                    }
                    else if (npc.velocity.Y > 0f)
                    {
                        npc.velocity.Y += 0.02f;
                    }
                    if (npc.velocity.Y < -1.5)
                    {
                        npc.velocity.Y = -1.5f;
                    }
                }
                else if (npc.directionY == 1 && npc.velocity.Y < 1.5)
                {
                    npc.velocity.Y += 0.04f;
                    if (npc.velocity.Y < -1.5)
                    {
                        npc.velocity.Y += 0.03f;
                    }
                    else if (npc.velocity.Y < 0f)
                    {
                        npc.velocity.Y -= 0.02f;
                    }
                    if (npc.velocity.Y > 1.5)
                    {
                        npc.velocity.Y = 1.5f;
                    }
                }
            }
            else
            {
                if (npc.direction == -1 && npc.velocity.X > -4f)
                {
                    npc.velocity.X -= 0.1f;
                    if (npc.velocity.X > 4f)
                    {
                        npc.velocity.X -= 0.1f;
                    }
                    else if (npc.velocity.X > 0f)
                    {
                        npc.velocity.X += 0.05f;
                    }
                    if (npc.velocity.X < -4f)
                    {
                        npc.velocity.X = -4f;
                    }
                }
                else if (npc.direction == 1 && npc.velocity.X < 4f)
                {
                    npc.velocity.X += 0.1f;
                    if (npc.velocity.X < -4f)
                    {
                        npc.velocity.X += 0.1f;
                    }
                    else if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X -= 0.05f;
                    }
                    if (npc.velocity.X > 4f)
                    {
                        npc.velocity.X = 4f;
                    }
                }
                if (npc.directionY == -1 && npc.velocity.Y > -1.5)
                {
                    npc.velocity.Y -= 0.04f;
                    if (npc.velocity.Y > 1.5)
                    {
                        npc.velocity.Y -= 0.05f;
                    }
                    else if (npc.velocity.Y > 0f)
                    {
                        npc.velocity.Y += 0.03f;
                    }
                    if (npc.velocity.Y < -1.5)
                    {
                        npc.velocity.Y = -1.5f;
                    }
                }
                else if (npc.directionY == 1 && npc.velocity.Y < 1.5)
                {
                    npc.velocity.Y += 0.04f;
                    if (npc.velocity.Y < -1.5)
                    {
                        npc.velocity.Y += 0.05f;
                    }
                    else if (npc.velocity.Y < 0f)
                    {
                        npc.velocity.Y -= 0.03f;
                    }
                    if (npc.velocity.Y > 1.5)
                    {
                        npc.velocity.Y = 1.5f;
                    }
                }
            }
        }
        if (npc.type == 48 && npc.wet)
        {
            if (npc.velocity.Y > 0f)
            {
                npc.velocity.Y *= 0.95f;
            }
            npc.velocity.Y -= 0.5f;
            if (npc.velocity.Y < -4f)
            {
                npc.velocity.Y = -4f;
            }
            npc.TargetClosest();
        }
        if (npc.type == 158 && Main.netMode != 1)
        {
            Vector2 vector26 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num208 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector26.X;
            float num209 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector26.Y;
            float num210 = (float)Math.Sqrt(num208 * num208 + num209 * num209);
            if (num210 < 200f && npc.position.Y + npc.height < Main.player[npc.target].position.Y + Main.player[npc.target].height && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
            {
                npc.Transform(159);
            }
        }
        npc.ai[1] += 1f;
        if (npc.type == 158)
        {
            npc.ai[1] += 1f;
        }
        if (npc.ai[1] > 200f)
        {
            if (!Main.player[npc.target].wet && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
            {
                npc.ai[1] = 0f;
            }
            float num211 = 0.2f;
            float num212 = 0.1f;
            float num213 = 4f;
            float num214 = 1.5f;
            if (npc.type == 48 || npc.type == 62 || npc.type == 66)
            {
                num211 = 0.12f;
                num212 = 0.07f;
                num213 = 3f;
                num214 = 1.25f;
            }
            if (npc.ai[1] > 1000f)
            {
                npc.ai[1] = 0f;
            }
            npc.ai[2] += 1f;
            if (npc.ai[2] > 0f)
            {
                if (npc.velocity.Y < num214)
                {
                    npc.velocity.Y += num212;
                }
            }
            else if (npc.velocity.Y > 0f - num214)
            {
                npc.velocity.Y -= num212;
            }
            if (npc.ai[2] < -150f || npc.ai[2] > 150f)
            {
                if (npc.velocity.X < num213)
                {
                    npc.velocity.X += num211;
                }
            }
            else if (npc.velocity.X > 0f - num213)
            {
                npc.velocity.X -= num211;
            }
            if (npc.ai[2] > 300f)
            {
                npc.ai[2] = -300f;
            }
        }
        if (Main.netMode == 1)
        {
            return;
        }
        if (npc.type == 48)
        {
            npc.ai[0] += 1f;
            if (npc.ai[0] == 30f || npc.ai[0] == 60f || npc.ai[0] == 90f)
            {
                if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    float num215 = 6f;
                    Vector2 vector27 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                    float num216 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector27.X + Main.rand.Next(-100, 101);
                    float num217 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector27.Y + Main.rand.Next(-100, 101);
                    float num218 = (float)Math.Sqrt(num216 * num216 + num217 * num217);
                    num218 = num215 / num218;
                    num216 *= num218;
                    num217 *= num218;
                    int num219 = 15;
                    int num220 = 38;
                    int num221 = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector27.X, vector27.Y, num216, num217, num220, num219, 0f, Main.myPlayer);
                    Main.projectile[num221].timeLeft = 300;
                }
            }
            else if (npc.ai[0] >= 400 + Main.rand.Next(400))
            {
                npc.ai[0] = 0f;
            }
        }
        if (npc.type == 62 || npc.type == 66)
        {
            npc.ai[0] += 1f;
            if (npc.ai[0] == 20f || npc.ai[0] == 40f || npc.ai[0] == 60f || npc.ai[0] == 80f)
            {
                if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    float num222 = 0.2f;
                    Vector2 vector28 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                    float num223 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector28.X + Main.rand.Next(-100, 101);
                    float num224 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector28.Y + Main.rand.Next(-100, 101);
                    float num225 = (float)Math.Sqrt(num223 * num223 + num224 * num224);
                    num225 = num222 / num225;
                    num223 *= num225;
                    num224 *= num225;
                    int num226 = 21;
                    int num227 = 44;
                    int num228 = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector28.X, vector28.Y, num223, num224, num227, num226, 0f, Main.myPlayer);
                    Main.projectile[num228].timeLeft = 300;
                }
            }
            else if (npc.ai[0] >= 300 + Main.rand.Next(300))
            {
                npc.ai[0] = 0f;
            }
        }
        if (npc.type != 156)
        {
            return;
        }
        npc.ai[0] += 1f;
        if (npc.ai[0] == 20f || npc.ai[0] == 40f || npc.ai[0] == 60f || npc.ai[0] == 80f || npc.ai[0] == 100f)
        {
            if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
            {
                float num229 = 0.2f;
                Vector2 vector29 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float num230 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector29.X + Main.rand.Next(-50, 51);
                float num231 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector29.Y + Main.rand.Next(-50, 51);
                float num232 = (float)Math.Sqrt(num230 * num230 + num231 * num231);
                num232 = num229 / num232;
                num230 *= num232;
                num231 *= num232;
                int num233 = 80;
                int num234 = 115;
                vector29 += npc.velocity * 5f;
                int num235 = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector29.X + num230 * 100f, vector29.Y + num231 * 100f, num230, num231, num234, num233, 0f, Main.myPlayer);
                Main.projectile[num235].timeLeft = 300;
            }
        }
        else if (npc.ai[0] >= 250 + Main.rand.Next(250))
        {
            npc.ai[0] = 0f;
        }

    }
}
