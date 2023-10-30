namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_087(this NPC npc)
    {
        npc.knockBackResist = 0.2f * Main.GameModeInfo.KnockbackToEnemiesMultiplier;
        npc.dontTakeDamage = false;
        npc.noTileCollide = false;
        npc.noGravity = false;
        npc.reflectsProjectiles = false;
        if (npc.ai[0] != 7f && Main.player[npc.target].dead)
        {
            npc.TargetClosest();
            if (Main.player[npc.target].dead)
            {
                npc.ai[0] = 7f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
        }
        if (npc.ai[0] == 0f)
        {
            npc.TargetClosest();
            Vector2 vector254 = Main.player[npc.target].Center - npc.Center;
            if (Main.netMode != 1 && (npc.velocity.X != 0f || npc.velocity.Y > 100f || npc.justHit || vector254.Length() < 80f))
            {
                npc.ai[0] = 1f;
                npc.ai[1] = 0f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 1f)
        {
            npc.ai[1] += 1f;
            if (Main.netMode != 1 && npc.ai[1] > 36f)
            {
                npc.ai[0] = 2f;
                npc.ai[1] = 0f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 2f)
        {
            Vector2 vector255 = Main.player[npc.target].Center - npc.Center;
            if (Main.netMode != 1 && vector255.Length() > 600f)
            {
                npc.ai[0] = 5f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
            if (npc.velocity.Y == 0f)
            {
                npc.TargetClosest();
                npc.velocity.X *= 0.85f;
                npc.ai[1] += 1f;
                float num1370 = 15f + 30f * (npc.life / (float)npc.lifeMax);
                float num1371 = 3f + 4f * (1f - npc.life / (float)npc.lifeMax);
                float num1372 = 4f;
                if (!Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                {
                    num1372 += 2f;
                }
                if (Main.netMode != 1 && npc.ai[1] > num1370)
                {
                    npc.ai[3] += 1f;
                    if (npc.ai[3] >= 3f)
                    {
                        npc.ai[3] = 0f;
                        num1372 *= 2f;
                        num1371 /= 2f;
                    }
                    npc.ai[1] = 0f;
                    npc.velocity.Y -= num1372;
                    npc.velocity.X = num1371 * npc.direction;
                    npc.netUpdate = true;
                }
            }
            else
            {
                npc.knockBackResist = 0f;
                npc.velocity.X *= 0.99f;
                if (npc.direction < 0 && npc.velocity.X > -1f)
                {
                    npc.velocity.X = -1f;
                }
                if (npc.direction > 0 && npc.velocity.X < 1f)
                {
                    npc.velocity.X = 1f;
                }
            }
            npc.ai[2] += 1f;
            if (npc.ai[2] > 210.0 && npc.velocity.Y == 0f && Main.netMode != 1)
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        npc.ai[0] = 3f;
                        break;
                    case 1:
                        npc.ai[0] = 4f;
                        npc.noTileCollide = true;
                        npc.velocity.Y = -8f;
                        break;
                    case 2:
                        npc.ai[0] = 6f;
                        break;
                    default:
                        npc.ai[0] = 2f;
                        break;
                }
                if (Main.tenthAnniversaryWorld && npc.type == 476 && npc.ai[0] == 3f && Main.rand.Next(2) == 0)
                {
                    npc.ai[0] = 8f;
                }
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 3f)
        {
            npc.velocity.X *= 0.85f;
            npc.dontTakeDamage = true;
            npc.ai[1] += 1f;
            if (Main.netMode != 1 && npc.ai[1] >= 180f)
            {
                npc.ai[0] = 2f;
                npc.ai[1] = 0f;
                npc.netUpdate = true;
            }
            if (Main.expertMode)
            {
                npc.ReflectProjectiles(npc.Hitbox);
                npc.reflectsProjectiles = true;
            }
        }
        else if (npc.ai[0] == 4f)
        {
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.knockBackResist = 0f;
            if (npc.velocity.X < 0f)
            {
                npc.direction = -1;
            }
            else
            {
                npc.direction = 1;
            }
            npc.spriteDirection = npc.direction;
            npc.TargetClosest();
            Vector2 center35 = Main.player[npc.target].Center;
            center35.Y -= 350f;
            Vector2 vector256 = center35 - npc.Center;
            if (npc.ai[2] == 1f)
            {
                npc.ai[1] += 1f;
                vector256 = Main.player[npc.target].Center - npc.Center;
                vector256.Normalize();
                vector256 *= 8f;
                npc.velocity = (npc.velocity * 4f + vector256) / 5f;
                if (Main.netMode != 1 && npc.ai[1] > 6f)
                {
                    npc.ai[1] = 0f;
                    npc.ai[0] = 4.1f;
                    npc.ai[2] = 0f;
                    npc.velocity = vector256;
                    npc.netUpdate = true;
                }
            }
            else if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) < 40f && npc.Center.Y < Main.player[npc.target].Center.Y - 300f)
            {
                if (Main.netMode != 1)
                {
                    npc.ai[1] = 0f;
                    npc.ai[2] = 1f;
                    npc.netUpdate = true;
                }
            }
            else
            {
                vector256.Normalize();
                vector256 *= 12f;
                npc.velocity = (npc.velocity * 5f + vector256) / 6f;
            }
        }
        else if (npc.ai[0] == 4.1f)
        {
            npc.knockBackResist = 0f;
            if (npc.ai[2] == 0f && Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1) && !Collision.SolidCollision(npc.position, npc.width, npc.height))
            {
                npc.ai[2] = 1f;
            }
            if (npc.position.Y + npc.height >= Main.player[npc.target].position.Y || npc.velocity.Y <= 0f)
            {
                npc.ai[1] += 1f;
                if (Main.netMode != 1 && npc.ai[1] > 10f)
                {
                    npc.ai[0] = 2f;
                    npc.ai[1] = 0f;
                    npc.ai[2] = 0f;
                    npc.ai[3] = 0f;
                    npc.netUpdate = true;
                    if (Collision.SolidCollision(npc.position, npc.width, npc.height))
                    {
                        npc.ai[0] = 5f;
                    }
                }
            }
            else if (npc.ai[2] == 0f)
            {
                npc.noTileCollide = true;
                npc.noGravity = true;
                npc.knockBackResist = 0f;
            }
            npc.velocity.Y += 0.2f;
            if (npc.velocity.Y > 16f)
            {
                npc.velocity.Y = 16f;
            }
        }
        else if (npc.ai[0] == 5f)
        {
            if (npc.velocity.X > 0f)
            {
                npc.direction = 1;
            }
            else
            {
                npc.direction = -1;
            }
            npc.spriteDirection = npc.direction;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.knockBackResist = 0f;
            Vector2 vector257 = Main.player[npc.target].Center - npc.Center;
            vector257.Y -= 4f;
            if (Main.netMode != 1 && vector257.Length() < 200f && !Collision.SolidCollision(npc.position, npc.width, npc.height))
            {
                npc.ai[0] = 2f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
            if (vector257.Length() > 10f)
            {
                vector257.Normalize();
                vector257 *= 10f;
            }
            npc.velocity = (npc.velocity * 4f + vector257) / 5f;
        }
        else if (npc.ai[0] == 6f)
        {
            npc.knockBackResist = 0f;
            if (npc.velocity.Y == 0f)
            {
                npc.TargetClosest();
                npc.velocity.X *= 0.8f;
                npc.ai[1] += 1f;
                if (npc.ai[1] > 5f)
                {
                    npc.ai[1] = 0f;
                    npc.velocity.Y -= 4f;
                    if (Main.player[npc.target].position.Y + Main.player[npc.target].height < npc.Center.Y)
                    {
                        npc.velocity.Y -= 1.25f;
                    }
                    if (Main.player[npc.target].position.Y + Main.player[npc.target].height < npc.Center.Y - 40f)
                    {
                        npc.velocity.Y -= 1.5f;
                    }
                    if (Main.player[npc.target].position.Y + Main.player[npc.target].height < npc.Center.Y - 80f)
                    {
                        npc.velocity.Y -= 1.75f;
                    }
                    if (Main.player[npc.target].position.Y + Main.player[npc.target].height < npc.Center.Y - 120f)
                    {
                        npc.velocity.Y -= 2f;
                    }
                    if (Main.player[npc.target].position.Y + Main.player[npc.target].height < npc.Center.Y - 160f)
                    {
                        npc.velocity.Y -= 2.25f;
                    }
                    if (Main.player[npc.target].position.Y + Main.player[npc.target].height < npc.Center.Y - 200f)
                    {
                        npc.velocity.Y -= 2.5f;
                    }
                    if (!Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                    {
                        npc.velocity.Y -= 2f;
                    }
                    npc.velocity.X = 12 * npc.direction;
                    npc.ai[2] += 1f;
                    npc.netUpdate = true;
                }
            }
            else
            {
                npc.velocity.X *= 0.98f;
                if (npc.direction < 0 && npc.velocity.X > -8f)
                {
                    npc.velocity.X = -8f;
                }
                if (npc.direction > 0 && npc.velocity.X < 8f)
                {
                    npc.velocity.X = 8f;
                }
            }
            if (Main.netMode != 1 && npc.ai[2] >= 3f && npc.velocity.Y == 0f)
            {
                npc.ai[0] = 2f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 7f)
        {
            npc.damage = 0;
            npc.life = npc.lifeMax;
            npc.defense = 9999;
            npc.noTileCollide = true;
            npc.alpha += 7;
            if (npc.alpha > 255)
            {
                npc.alpha = 255;
            }
            npc.velocity.X *= 0.98f;
        }
        else
        {
            if (npc.ai[0] != 8f)
            {
                return;
            }
            npc.velocity.X *= 0.85f;
            npc.ai[1] += 1f;
            if (Main.netMode != 1)
            {
                if (!Main.tenthAnniversaryWorld || npc.ai[1] >= 180f)
                {
                    npc.ai[0] = 2f;
                    npc.ai[1] = 0f;
                    npc.netUpdate = true;
                }
                else if (npc.ai[1] % 20f == 0f)
                {
                    npc.AI_87_BigMimic_FireStuffCannonBurst();
                }
            }
        }
    }
}
