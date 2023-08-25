using OTAPI;

namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_022(this NPC npc)
    {
        bool flag16 = false;
        bool flag17 = npc.type == 330 && !Main.pumpkinMoon;
        if (npc.type == 253 && !Main.eclipse)
        {
            flag17 = true;
        }
        if (npc.type == 490 && Main.dayTime)
        {
            flag17 = true;
        }
        if (npc.justHit)
        {
            npc.ai[2] = 0f;
        }
        if (npc.type == 316 && (Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > 3000f))
        {
            npc.TargetClosest();
            if (Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > 3000f)
            {
                npc.EncourageDespawn(10);
                flag16 = true;
                flag17 = true;
            }
        }
        if (flag17)
        {
            if (npc.velocity.X == 0f)
            {
                npc.velocity.X = Main.rand.Next(-1, 2) * 1.5f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[2] >= 0f)
        {
            int num289 = 16;
            bool flag18 = false;
            bool flag19 = false;
            if (npc.position.X > npc.ai[0] - num289 && npc.position.X < npc.ai[0] + num289)
            {
                flag18 = true;
            }
            else if ((npc.velocity.X < 0f && npc.direction > 0) || (npc.velocity.X > 0f && npc.direction < 0))
            {
                flag18 = true;
            }
            num289 += 24;
            if (npc.position.Y > npc.ai[1] - num289 && npc.position.Y < npc.ai[1] + num289)
            {
                flag19 = true;
            }
            if (flag18 && flag19)
            {
                npc.ai[2] += 1f;
                if (npc.ai[2] >= 30f && num289 == 16)
                {
                    flag16 = true;
                }
                if (npc.ai[2] >= 60f)
                {
                    npc.ai[2] = -200f;
                    npc.direction *= -1;
                    npc.velocity.X *= -1f;
                    npc.collideX = false;
                }
            }
            else
            {
                npc.ai[0] = npc.position.X;
                npc.ai[1] = npc.position.Y;
                npc.ai[2] = 0f;
            }
            npc.TargetClosest();
        }
        else if (npc.type == 253)
        {
            npc.TargetClosest();
            npc.ai[2] += 2f;
        }
        else
        {
            if (npc.type == 330)
            {
                npc.ai[2] += 0.1f;
            }
            else
            {
                npc.ai[2] += 1f;
            }
            if (Main.player[npc.target].position.X + Main.player[npc.target].width / 2 > npc.position.X + npc.width / 2)
            {
                npc.direction = -1;
            }
            else
            {
                npc.direction = 1;
            }
        }
        int x9 = (int)((npc.position.X + npc.width / 2) / 16f) + npc.direction * 2;
        int num290 = (int)((npc.position.Y + npc.height) / 16f);
        bool flag20 = true;
        bool flag21 = false;
        int num291 = 3;
        if (npc.type == 122)
        {
            if (npc.justHit)
            {
                npc.ai[3] = 0f;
                npc.localAI[1] = 0f;
            }
            if (Main.netMode != 1 && npc.ai[3] == 32f && !Main.player[npc.target].npcTypeNoAggro[npc.type])
            {
                float num292 = 7f;
                Vector2 vector33 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float num293 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector33.X;
                float num294 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector33.Y;
                float num295 = (float)Math.Sqrt(num293 * num293 + num294 * num294);
                num295 = num292 / num295;
                num293 *= num295;
                num294 *= num295;
                float num297 = 0.0125f;
                Vector2 vector34 = new Vector2(num293, num294).RotatedByRandom(num297 * ((float)Math.PI * 2f));
                num293 = vector34.X;
                num294 = vector34.Y;
                int num298 = 25;
                int num299 = 84;
                _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector33.X, vector33.Y, num293, num294, num299, num298, 0f, Main.myPlayer);
            }
            num291 = 8;
            if (npc.ai[3] > 0f)
            {
                npc.ai[3] += 1f;
                if (npc.ai[3] >= 64f)
                {
                    npc.ai[3] = 0f;
                }
            }
            if (Main.netMode != 1 && npc.ai[3] == 0f)
            {
                npc.localAI[1] += 1f;
                if (npc.localAI[1] > 120f && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height) && !Main.player[npc.target].npcTypeNoAggro[npc.type])
                {
                    npc.localAI[1] = 0f;
                    npc.ai[3] = 1f;
                    npc.netUpdate = true;
                }
            }
        }
        else if (npc.type == 75)
        {
            num291 = 4;
            npc.position += npc.netOffset;
            if (Main.rand.Next(6) == 0)
            {
                int num301 = Dust.NewDust(npc.position, npc.width, npc.height, 55, 0f, 0f, 200, npc.color);
                Dust dust = Main.dust[num301];
                dust.velocity *= 0.3f;
            }
            if (Main.rand.Next(40) == 0)
            {
                SoundEngine.PlaySound(27, (int)npc.position.X, (int)npc.position.Y);
            }
            npc.position -= npc.netOffset;
        }
        else if (npc.type == 169)
        {
            npc.position += npc.netOffset;
            Lighting.AddLight((int)((npc.position.X + npc.width / 2) / 16f), (int)((npc.position.Y + npc.height / 2) / 16f), 0f, 0.6f, 0.75f);
            npc.alpha = 30;
            if (Main.rand.Next(3) == 0)
            {
                int num302 = Dust.NewDust(npc.position, npc.width, npc.height, 92, 0f, 0f, 200);
                Dust dust = Main.dust[num302];
                dust.velocity *= 0.3f;
                Main.dust[num302].noGravity = true;
            }
            npc.position -= npc.netOffset;
            if (npc.justHit)
            {
                npc.ai[3] = 0f;
                npc.localAI[1] = 0f;
            }
            float num303 = 5f;
            Vector2 vector35 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num304 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector35.X;
            float num305 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector35.Y;
            float num306 = (float)Math.Sqrt(num304 * num304 + num305 * num305);
            num306 = num303 / num306;
            num304 *= num306;
            num305 *= num306;
            if (num304 > 0f)
            {
                npc.direction = 1;
            }
            else
            {
                npc.direction = -1;
            }
            npc.spriteDirection = npc.direction;
            if (npc.direction < 0)
            {
                npc.rotation = (float)Math.Atan2(0f - num305, 0f - num304);
            }
            else
            {
                npc.rotation = (float)Math.Atan2(num305, num304);
            }
            if (Main.netMode != 1 && npc.ai[3] == 16f)
            {
                int num308 = 45;
                int num309 = 128;
                _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector35.X, vector35.Y, num304, num305, num309, num308, 0f, Main.myPlayer);
            }
            num291 = 10;
            if (npc.ai[3] > 0f)
            {
                npc.ai[3] += 1f;
                if (npc.ai[3] >= 64f)
                {
                    npc.ai[3] = 0f;
                }
            }
            if (Main.netMode != 1 && npc.ai[3] == 0f)
            {
                npc.localAI[1] += 1f;
                if (npc.localAI[1] > 120f && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    npc.localAI[1] = 0f;
                    npc.ai[3] = 1f;
                    npc.netUpdate = true;
                }
            }
        }
        else if (npc.type == 268)
        {
            npc.rotation = npc.velocity.X * 0.1f;
            num291 = ((!(Main.player[npc.target].Center.Y < npc.Center.Y)) ? 6 : 12);
            if (Main.netMode != 1 && !npc.confused)
            {
                npc.ai[3] += 1f;
                if (npc.justHit)
                {
                    npc.ai[3] = -45f;
                    npc.localAI[1] = 0f;
                }
                if (Main.netMode != 1 && npc.ai[3] >= 60 + Main.rand.Next(60))
                {
                    npc.ai[3] = 0f;
                    if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                    {
                        float num311 = 10f;
                        Vector2 vector36 = new(npc.position.X + npc.width * 0.5f - 4f, npc.position.Y + npc.height * 0.7f);
                        float num312 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector36.X;
                        float num313 = Math.Abs(num312) * 0.1f;
                        float num314 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector36.Y - num313;
                        num312 += Main.rand.Next(-10, 11);
                        num314 += Main.rand.Next(-30, 21);
                        float num315 = (float)Math.Sqrt(num312 * num312 + num314 * num314);
                        num315 = num311 / num315;
                        num312 *= num315;
                        num314 *= num315;
                        int num317 = 40;
                        int num318 = 288;
                        _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector36.X, vector36.Y, num312, num314, num318, num317, 0f, Main.myPlayer);
                    }
                }
            }
        }
        if (npc.type == 490)
        {
            num291 = 4;
            if (npc.target >= 0)
            {
                float num320 = (Main.player[npc.target].Center - npc.Center).Length();
                num320 /= 70f;
                if (num320 > 8f)
                {
                    num320 = 8f;
                }
                num291 += (int)num320;
            }
        }
        if (npc.position.Y + npc.height > Main.player[npc.target].position.Y)
        {
            if (npc.type == 330)
            {
                flag20 = false;
            }
            else
            {
                for (int num321 = num290; num321 < num290 + num291; num321++)
                {
                    if (Main.tile[x9, num321] == null)
                    {
                        Main.tile[x9, num321] = Hooks.Tile.InvokeCreate();
                    }
                    if ((Main.tile[x9, num321].nactive() && Main.tileSolid[Main.tile[x9, num321].type]) || Main.tile[x9, num321].liquid > 0)
                    {
                        if (num321 <= num290 + 1)
                        {
                            flag21 = true;
                        }
                        flag20 = false;
                        break;
                    }
                }
            }
        }
        if (Main.player[npc.target].npcTypeNoAggro[npc.type])
        {
            bool flag22 = false;
            for (int num322 = num290; num322 < num290 + num291 - 2; num322++)
            {
                if (Main.tile[x9, num322] == null)
                {
                    Main.tile[x9, num322] = Hooks.Tile.InvokeCreate();
                }
                if ((Main.tile[x9, num322].nactive() && Main.tileSolid[Main.tile[x9, num322].type]) || Main.tile[x9, num322].liquid > 0)
                {
                    flag22 = true;
                    break;
                }
            }
            npc.directionY = (!flag22).ToDirectionInt();
        }
        if (npc.type == 169 || npc.type == 268)
        {
            for (int num323 = num290 - 3; num323 < num290; num323++)
            {
                if (Main.tile[x9, num323] == null)
                {
                    Main.tile[x9, num323] = Hooks.Tile.InvokeCreate();
                }
                if ((Main.tile[x9, num323].nactive() && Main.tileSolid[Main.tile[x9, num323].type] && !TileID.Sets.Platforms[Main.tile[x9, num323].type]) || Main.tile[x9, num323].liquid > 0)
                {
                    flag21 = false;
                    flag16 = true;
                    break;
                }
            }
        }
        if (flag16)
        {
            flag21 = false;
            flag20 = true;
            if (npc.type == 268)
            {
                npc.velocity.Y += 2f;
            }
        }
        if (flag20)
        {
            if (npc.type == 75 || npc.type == 169)
            {
                npc.velocity.Y += 0.2f;
                if (npc.velocity.Y > 2f)
                {
                    npc.velocity.Y = 2f;
                }
            }
            else if (npc.type == 490)
            {
                npc.velocity.Y += 0.03f;
                if (npc.velocity.Y > 0.75)
                {
                    npc.velocity.Y = 0.75f;
                }
            }
            else
            {
                npc.velocity.Y += 0.1f;
                if (npc.type == 316 && flag17)
                {
                    npc.velocity.Y -= 0.05f;
                    if (npc.velocity.Y > 6f)
                    {
                        npc.velocity.Y = 6f;
                    }
                }
                else if (npc.velocity.Y > 3f)
                {
                    npc.velocity.Y = 3f;
                }
            }
        }
        else
        {
            if (npc.type == 75 || npc.type == 169)
            {
                if ((npc.directionY < 0 && npc.velocity.Y > 0f) || flag21)
                {
                    npc.velocity.Y -= 0.2f;
                }
            }
            else if (npc.type == 490)
            {
                if ((npc.directionY < 0 && npc.velocity.Y > 0f) || flag21)
                {
                    npc.velocity.Y -= 0.075f;
                }
                if (npc.velocity.Y < -0.75f)
                {
                    npc.velocity.Y = -0.75f;
                }
            }
            else if (npc.directionY < 0 && npc.velocity.Y > 0f)
            {
                npc.velocity.Y -= 0.1f;
            }
            if (npc.velocity.Y < -4f)
            {
                npc.velocity.Y = -4f;
            }
        }
        if (npc.type == 75 && npc.wet)
        {
            npc.velocity.Y -= 0.2f;
            if (npc.velocity.Y < -2f)
            {
                npc.velocity.Y = -2f;
            }
        }
        if (npc.collideX)
        {
            npc.velocity.X = npc.oldVelocity.X * -0.4f;
            if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 1f)
            {
                npc.velocity.X = 1f;
            }
            if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -1f)
            {
                npc.velocity.X = -1f;
            }
        }
        if (npc.collideY)
        {
            npc.velocity.Y = npc.oldVelocity.Y * -0.25f;
            if (npc.velocity.Y > 0f && npc.velocity.Y < 1f)
            {
                npc.velocity.Y = 1f;
            }
            if (npc.velocity.Y < 0f && npc.velocity.Y > -1f)
            {
                npc.velocity.Y = -1f;
            }
        }
        float num324 = 2f;
        if (npc.type == 75)
        {
            num324 = 3f;
        }
        if (npc.type == 253)
        {
            num324 = 4f;
        }
        if (npc.type == 490)
        {
            num324 = 1.5f;
        }
        if (npc.type == 330)
        {
            npc.alpha = 0;
            num324 = 4f;
            if (!flag17)
            {
                npc.TargetClosest();
            }
            else
            {
                npc.EncourageDespawn(10);
            }
            if (npc.direction < 0 && npc.velocity.X > 0f)
            {
                npc.velocity.X *= 0.9f;
            }
            if (npc.direction > 0 && npc.velocity.X < 0f)
            {
                npc.velocity.X *= 0.9f;
            }
        }
        if (npc.direction == -1 && npc.velocity.X > 0f - num324)
        {
            npc.velocity.X -= 0.1f;
            if (npc.velocity.X > num324)
            {
                npc.velocity.X -= 0.1f;
            }
            else if (npc.velocity.X > 0f)
            {
                npc.velocity.X += 0.05f;
            }
            if (npc.velocity.X < 0f - num324)
            {
                npc.velocity.X = 0f - num324;
            }
        }
        else if (npc.direction == 1 && npc.velocity.X < num324)
        {
            npc.velocity.X += 0.1f;
            if (npc.velocity.X < 0f - num324)
            {
                npc.velocity.X += 0.1f;
            }
            else if (npc.velocity.X < 0f)
            {
                npc.velocity.X -= 0.05f;
            }
            if (npc.velocity.X > num324)
            {
                npc.velocity.X = num324;
            }
        }
        num324 = ((npc.type != 490) ? 1.5f : 1f);
        if (npc.directionY == -1 && npc.velocity.Y > 0f - num324)
        {
            npc.velocity.Y -= 0.04f;
            if (npc.velocity.Y > num324)
            {
                npc.velocity.Y -= 0.05f;
            }
            else if (npc.velocity.Y > 0f)
            {
                npc.velocity.Y += 0.03f;
            }
            if (npc.velocity.Y < 0f - num324)
            {
                npc.velocity.Y = 0f - num324;
            }
        }
        else if (npc.directionY == 1 && npc.velocity.Y < num324)
        {
            npc.velocity.Y += 0.04f;
            if (npc.velocity.Y < 0f - num324)
            {
                npc.velocity.Y += 0.05f;
            }
            else if (npc.velocity.Y < 0f)
            {
                npc.velocity.Y -= 0.03f;
            }
            if (npc.velocity.Y > num324)
            {
                npc.velocity.Y = num324;
            }
        }
        if (npc.type == 122)
        {
            Lighting.AddLight((int)npc.position.X / 16, (int)npc.position.Y / 16, 0.4f, 0f, 0.25f);
        }
    }
}
