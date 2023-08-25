namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_088(this NPC npc)
    {
        int num1373 = 7;
        npc.noTileCollide = false;
        npc.noGravity = true;
        npc.knockBackResist = 0.2f * Main.GameModeInfo.KnockbackToEnemiesMultiplier;
        npc.damage = npc.defDamage;
        if (!Main.eclipse && Main.netMode != 1)
        {
            if (npc.ai[0] != -1f)
            {
                npc.netUpdate = true;
            }
            npc.ai[0] = -1f;
        }
        else if (npc.target < 0 || Main.player[npc.target].dead || !Main.player[npc.target].active)
        {
            npc.TargetClosest();
            Vector2 vector258 = Main.player[npc.target].Center - npc.Center;
            if (Main.netMode != 1 && (Main.player[npc.target].dead || vector258.Length() > 3000f))
            {
                if (npc.ai[0] != -1f)
                {
                    npc.netUpdate = true;
                }
                npc.ai[0] = -1f;
            }
        }
        else
        {
            Vector2 vector259 = Main.player[npc.target].Center - npc.Center;
            if (Main.netMode != 1 && npc.ai[0] > 1f && vector259.Length() > 1000f)
            {
                if (npc.ai[0] != 1f)
                {
                    npc.netUpdate = true;
                }
                npc.ai[0] = 1f;
            }
        }
        if (npc.ai[0] == -1f)
        {
            Vector2 vector260 = new(0f, -8f);
            npc.velocity = (npc.velocity * 9f + vector260) / 10f;
            npc.noTileCollide = true;
            npc.dontTakeDamage = true;
        }
        else if (npc.ai[0] == 0f)
        {
            npc.TargetClosest();
            if (npc.Center.X < Main.player[npc.target].Center.X - 2f)
            {
                npc.direction = 1;
            }
            if (npc.Center.X > Main.player[npc.target].Center.X + 2f)
            {
                npc.direction = -1;
            }
            npc.spriteDirection = npc.direction;
            npc.rotation = (npc.rotation * 9f + npc.velocity.X * 0.1f) / 10f;
            if (npc.collideX)
            {
                npc.velocity.X *= (0f - npc.oldVelocity.X) * 0.5f;
                if (npc.velocity.X > 4f)
                {
                    npc.velocity.X = 4f;
                }
                if (npc.velocity.X < -4f)
                {
                    npc.velocity.X = -4f;
                }
            }
            if (npc.collideY)
            {
                npc.velocity.Y *= (0f - npc.oldVelocity.Y) * 0.5f;
                if (npc.velocity.Y > 4f)
                {
                    npc.velocity.Y = 4f;
                }
                if (npc.velocity.Y < -4f)
                {
                    npc.velocity.Y = -4f;
                }
            }
            Vector2 vector261 = Main.player[npc.target].Center - npc.Center;
            vector261.Y -= 200f;
            if (vector261.Length() > 800f)
            {
                npc.ai[0] = 1f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
            else if (vector261.Length() > 80f)
            {
                float num1374 = 6f;
                float num1375 = 30f;
                vector261.Normalize();
                vector261 *= num1374;
                npc.velocity = (npc.velocity * (num1375 - 1f) + vector261) / num1375;
            }
            else if (npc.velocity.Length() > 2f)
            {
                npc.velocity *= 0.95f;
            }
            else if (npc.velocity.Length() < 1f)
            {
                npc.velocity *= 1.05f;
            }
            if (Main.netMode == 1)
            {
                return;
            }
            npc.ai[1] += 1f;
            if (npc.justHit)
            {
                npc.ai[1] += Main.rand.Next(10, 30);
            }
            if (!(npc.ai[1] >= 180f))
            {
                return;
            }
            npc.ai[1] = 0f;
            npc.ai[2] = 0f;
            npc.ai[3] = 0f;
            npc.netUpdate = true;
            while (npc.ai[0] == 0f)
            {
                int num1376 = Main.rand.Next(3);
                if (num1376 == 0 && Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                {
                    npc.ai[0] = 2f;
                    continue;
                }
                switch (num1376)
                {
                    case 1:
                        npc.ai[0] = 3f;
                        break;
                    case 2:
                        if (NPC.CountNPCS(478) + NPC.CountNPCS(479) < num1373)
                        {
                            npc.ai[0] = 4f;
                        }
                        break;
                }
            }
        }
        else if (npc.ai[0] == 1f)
        {
            npc.collideX = false;
            npc.collideY = false;
            npc.noTileCollide = true;
            npc.knockBackResist = 0f;
            if (npc.target < 0 || !Main.player[npc.target].active || Main.player[npc.target].dead)
            {
                npc.TargetClosest();
            }
            if (npc.velocity.X < 0f)
            {
                npc.direction = -1;
            }
            else if (npc.velocity.X > 0f)
            {
                npc.direction = 1;
            }
            npc.spriteDirection = npc.direction;
            npc.rotation = (npc.rotation * 9f + npc.velocity.X * 0.08f) / 10f;
            Vector2 vector262 = Main.player[npc.target].Center - npc.Center;
            if (Main.netMode != 1 && vector262.Length() < 300f && !Collision.SolidCollision(npc.position, npc.width, npc.height))
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
            float num1377 = 7f + vector262.Length() / 100f;
            float num1378 = 25f;
            vector262.Normalize();
            vector262 *= num1377;
            npc.velocity = (npc.velocity * (num1378 - 1f) + vector262) / num1378;
        }
        else if (npc.ai[0] == 2f)
        {
            npc.damage = (int)(npc.defDamage * 0.5);
            npc.knockBackResist = 0f;
            if (npc.target < 0 || !Main.player[npc.target].active || Main.player[npc.target].dead)
            {
                npc.TargetClosest();
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
            if (Main.player[npc.target].Center.X - 10f < npc.Center.X)
            {
                npc.direction = -1;
            }
            else if (Main.player[npc.target].Center.X + 10f > npc.Center.X)
            {
                npc.direction = 1;
            }
            npc.spriteDirection = npc.direction;
            npc.rotation = (npc.rotation * 4f + npc.velocity.X * 0.1f) / 5f;
            if (npc.collideX)
            {
                npc.velocity.X *= (0f - npc.oldVelocity.X) * 0.5f;
                if (npc.velocity.X > 4f)
                {
                    npc.velocity.X = 4f;
                }
                if (npc.velocity.X < -4f)
                {
                    npc.velocity.X = -4f;
                }
            }
            if (npc.collideY)
            {
                npc.velocity.Y *= (0f - npc.oldVelocity.Y) * 0.5f;
                if (npc.velocity.Y > 4f)
                {
                    npc.velocity.Y = 4f;
                }
                if (npc.velocity.Y < -4f)
                {
                    npc.velocity.Y = -4f;
                }
            }
            Vector2 vector263 = Main.player[npc.target].Center - npc.Center;
            vector263.Y -= 20f;
            npc.ai[2] += 1f / 45f;
            if (Main.expertMode)
            {
                npc.ai[2] += 1f / 60f;
            }
            float num1379 = 4f + npc.ai[2] + vector263.Length() / 120f;
            float num1380 = 20f;
            vector263.Normalize();
            vector263 *= num1379;
            npc.velocity = (npc.velocity * (num1380 - 1f) + vector263) / num1380;
            if (Main.netMode != 1)
            {
                npc.ai[1] += 1f;
                if (npc.ai[1] > 240f || !Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                {
                    npc.ai[0] = 0f;
                    npc.ai[1] = 0f;
                    npc.ai[2] = 0f;
                    npc.ai[3] = 0f;
                    npc.netUpdate = true;
                }
            }
        }
        else if (npc.ai[0] == 3f)
        {
            npc.knockBackResist = 0f;
            npc.noTileCollide = true;
            if (npc.velocity.X < 0f)
            {
                npc.direction = -1;
            }
            else
            {
                npc.direction = 1;
            }
            npc.spriteDirection = npc.direction;
            npc.rotation = (npc.rotation * 4f + npc.velocity.X * 0.07f) / 5f;
            Vector2 vector264 = Main.player[npc.target].Center - npc.Center;
            vector264.Y -= 12f;
            if (npc.Center.X > Main.player[npc.target].Center.X)
            {
                vector264.X += 400f;
            }
            else
            {
                vector264.X -= 400f;
            }
            if (Main.netMode != 1 && Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) > 350f && Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y) < 20f)
            {
                npc.ai[0] = 3.1f;
                npc.ai[1] = 0f;
                npc.netUpdate = true;
            }
            npc.ai[1] += 1f / 30f;
            float num1381 = 8f + npc.ai[1];
            float num1382 = 4f;
            vector264.Normalize();
            vector264 *= num1381;
            npc.velocity = (npc.velocity * (num1382 - 1f) + vector264) / num1382;
        }
        else if (npc.ai[0] == 3.1f)
        {
            npc.knockBackResist = 0f;
            npc.noTileCollide = true;
            npc.rotation = (npc.rotation * 4f + npc.velocity.X * 0.07f) / 5f;
            Vector2 vector265 = Main.player[npc.target].Center - npc.Center;
            vector265.Y -= 12f;
            float num1383 = 16f;
            float num1384 = 8f;
            vector265.Normalize();
            vector265 *= num1383;
            npc.velocity = (npc.velocity * (num1384 - 1f) + vector265) / num1384;
            if (npc.velocity.X < 0f)
            {
                npc.direction = -1;
            }
            else
            {
                npc.direction = 1;
            }
            npc.spriteDirection = npc.direction;
            npc.ai[1] += 1f;
            if (Main.netMode != 1 && npc.ai[1] > 10f)
            {
                npc.velocity = vector265;
                if (npc.velocity.X < 0f)
                {
                    npc.direction = -1;
                }
                else
                {
                    npc.direction = 1;
                }
                npc.ai[0] = 3.2f;
                npc.ai[1] = 0f;
                npc.ai[1] = npc.direction;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 3.2f)
        {
            npc.damage = (int)(npc.defDamage * 1.3);
            npc.collideX = false;
            npc.collideY = false;
            npc.knockBackResist = 0f;
            npc.noTileCollide = true;
            npc.ai[2] += 1f / 30f;
            npc.velocity.X = (16f + npc.ai[2]) * npc.ai[1];
            if ((npc.ai[1] > 0f && npc.Center.X > Main.player[npc.target].Center.X + 260f) || (npc.ai[1] < 0f && npc.Center.X < Main.player[npc.target].Center.X - 260f))
            {
                if (Main.netMode != 1 && !Collision.SolidCollision(npc.position, npc.width, npc.height))
                {
                    npc.ai[0] = 0f;
                    npc.ai[1] = 0f;
                    npc.ai[2] = 0f;
                    npc.ai[3] = 0f;
                    npc.netUpdate = true;
                }
                else if (Main.netMode != 1 && Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) > 800f)
                {
                    npc.ai[0] = 1f;
                    npc.ai[1] = 0f;
                    npc.ai[2] = 0f;
                    npc.ai[3] = 0f;
                    npc.netUpdate = true;
                }
            }
            npc.rotation = (npc.rotation * 4f + npc.velocity.X * 0.07f) / 5f;
        }
        else if (npc.ai[0] == 4f)
        {
            bool flag84 = (double)(npc.Center.Y / 16f) < Main.worldSurface;
            npc.TargetClosest();
            if (Main.netMode != 1)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                for (int num1385 = 0; num1385 < 1000; num1385++)
                {
                    int num1386 = (int)Main.player[npc.target].Center.X / 16;
                    int num1387 = (int)Main.player[npc.target].Center.Y / 16;
                    int num1388 = 30 + num1385 / 50;
                    int num1389 = 20 + num1385 / 75;
                    num1386 += Main.rand.Next(-num1388, num1388 + 1);
                    num1387 += Main.rand.Next(-num1389, num1389 + 1);
                    if (WorldGen.SolidTile(num1386, num1387))
                    {
                        continue;
                    }
                    bool flag85 = false;
                    int num1390 = 50;
                    while (num1390 > 0)
                    {
                        num1390--;
                        if (!WorldGen.InWorld(num1386, num1387, 5))
                        {
                            flag85 = true;
                            break;
                        }
                        ITile tile2 = Main.tile[num1386, num1387];
                        if (tile2 == null)
                        {
                            flag85 = true;
                            break;
                        }
                        if (tile2.liquid > 0 && tile2.lava())
                        {
                            flag85 = true;
                            break;
                        }
                        ITile tile3 = Main.tile[num1386, num1387 - 1];
                        if (tile3 == null)
                        {
                            flag85 = true;
                            break;
                        }
                        if (tile3.liquid > 0 && tile3.lava())
                        {
                            flag85 = true;
                            break;
                        }
                        if (WorldGen.SolidTile(num1386, num1387) || (flag84 && num1387 > Main.worldSurface))
                        {
                            break;
                        }
                        num1387++;
                    }
                    if (!(num1390 <= 0 || flag85) && (new Vector2(num1386 * 16 + 8, num1387 * 16 + 8) - Main.player[npc.target].Center).Length() < 600f)
                    {
                        npc.ai[0] = 4.1f;
                        npc.ai[1] = num1386;
                        npc.ai[2] = num1387;
                        break;
                    }
                }
            }
            npc.netUpdate = true;
        }
        else if (npc.ai[0] == 4.1f)
        {
            if (npc.velocity.X < -2f)
            {
                npc.direction = -1;
            }
            else if (npc.velocity.X > 2f)
            {
                npc.direction = 1;
            }
            npc.spriteDirection = npc.direction;
            npc.rotation = (npc.rotation * 9f + npc.velocity.X * 0.1f) / 10f;
            npc.noTileCollide = true;
            int num1391 = (int)npc.ai[1];
            int num1392 = (int)npc.ai[2];
            float x21 = num1391 * 16 + 8;
            float y13 = num1392 * 16 - 20;
            Vector2 v12 = new(x21, y13);
            v12 -= npc.Center;
            float num1393 = 6f + v12.Length() / 150f;
            if (num1393 > 10f)
            {
                num1393 = 10f;
            }
            float num1394 = 10f;
            if (v12.Length() < 10f)
            {
                npc.ai[0] = 4.2f;
                npc.netUpdate = true;
            }
            v12 = v12.SafeNormalize(Vector2.Zero);
            v12 *= num1393;
            npc.velocity = (npc.velocity * (num1394 - 1f) + v12) / num1394;
            if (npc.velocity.Length() > num1393)
            {
                npc.velocity.Normalize();
                npc.velocity *= num1393;
            }
        }
        else
        {
            if (npc.ai[0] != 4.2f)
            {
                return;
            }
            npc.rotation = (npc.rotation * 9f + npc.velocity.X * 0.1f) / 10f;
            npc.knockBackResist = 0f;
            npc.noTileCollide = true;
            int num1395 = (int)npc.ai[1];
            int num1396 = (int)npc.ai[2];
            float x22 = num1395 * 16 + 8;
            float y14 = num1396 * 16 - 20;
            Vector2 vector266 = new(x22, y14);
            vector266 -= npc.Center;
            float num1397 = 4f;
            float num1398 = 2f;
            if (Main.netMode != 1 && vector266.Length() < 4f)
            {
                int num1399 = 70;
                if (Main.expertMode)
                {
                    num1399 = (int)(num1399 * 0.75);
                }
                npc.ai[3] += 1f;
                if (npc.ai[3] == num1399)
                {
                    int num1400 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), num1395 * 16 + 8, num1396 * 16, 478, npc.whoAmI);
                    Main.npc[num1400].netUpdate = true;
                }
                else if (npc.ai[3] == num1399 * 2)
                {
                    npc.ai[0] = 0f;
                    npc.ai[1] = 0f;
                    npc.ai[2] = 0f;
                    npc.ai[3] = 0f;
                    npc.netUpdate = true;
                    if (NPC.CountNPCS(478) + NPC.CountNPCS(479) < num1373 && Main.rand.Next(3) != 0)
                    {
                        npc.ai[0] = 4f;
                    }
                    else if (Collision.SolidCollision(npc.position, npc.width, npc.height))
                    {
                        npc.ai[0] = 1f;
                    }
                }
            }
            if (vector266.Length() > num1397)
            {
                vector266.Normalize();
                vector266 *= num1397;
            }
            npc.velocity = (npc.velocity * (num1398 - 1f) + vector266) / num1398;
            if (npc.velocity.Length() > num1397)
            {
                npc.velocity.Normalize();
                npc.velocity *= num1397;
            }
        }
    }
}
