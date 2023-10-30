namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_090(this NPC npc)
    {
        npc.noTileCollide = false;
        npc.knockBackResist = 0.4f * Main.GameModeInfo.KnockbackToEnemiesMultiplier;
        npc.noGravity = true;
        npc.rotation = (npc.rotation * 9f + npc.velocity.X * 0.1f) / 10f;
        if (!Main.eclipse)
        {
            npc.EncourageDespawn(5);
            npc.velocity.Y -= 0.2f;
            if (npc.velocity.Y < -8f)
            {
                npc.velocity.Y = -8f;
            }
            npc.noTileCollide = true;
            return;
        }
        if (npc.ai[0] == 0f || npc.ai[0] == 1f)
        {
            for (int num1403 = 0; num1403 < 200; num1403++)
            {
                if (num1403 != npc.whoAmI && Main.npc[num1403].active && Main.npc[num1403].type == npc.type)
                {
                    Vector2 vector267 = Main.npc[num1403].Center - npc.Center;
                    if (vector267.Length() < npc.width + npc.height)
                    {
                        vector267.Normalize();
                        vector267 *= -0.1f;
                        npc.velocity += vector267;
                        NPC nPC3 = Main.npc[num1403];
                        nPC3.velocity -= vector267;
                    }
                }
            }
        }
        if (npc.target < 0 || Main.player[npc.target].dead || !Main.player[npc.target].active)
        {
            npc.TargetClosest();
            Vector2 vector268 = Main.player[npc.target].Center - npc.Center;
            if (Main.player[npc.target].dead || vector268.Length() > 3000f)
            {
                npc.ai[0] = -1f;
            }
        }
        else
        {
            Vector2 vector269 = Main.player[npc.target].Center - npc.Center;
            if (npc.ai[0] > 1f && vector269.Length() > 1000f)
            {
                npc.ai[0] = 1f;
            }
        }
        if (npc.ai[0] == -1f)
        {
            Vector2 vector270 = new(0f, -8f);
            npc.velocity = (npc.velocity * 9f + vector270) / 10f;
            npc.noTileCollide = true;
            npc.dontTakeDamage = true;
        }
        else if (npc.ai[0] == 0f)
        {
            npc.TargetClosest();
            npc.spriteDirection = npc.direction;
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
            Vector2 vector271 = Main.player[npc.target].Center - npc.Center;
            if (vector271.Length() > 800f)
            {
                npc.ai[0] = 1f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
            }
            else if (vector271.Length() > 200f)
            {
                float num1404 = 5.5f + vector271.Length() / 100f + npc.ai[1] / 15f;
                float num1405 = 40f;
                vector271.Normalize();
                vector271 *= num1404;
                npc.velocity = (npc.velocity * (num1405 - 1f) + vector271) / num1405;
            }
            else if (npc.velocity.Length() > 2f)
            {
                npc.velocity *= 0.95f;
            }
            else if (npc.velocity.Length() < 1f)
            {
                npc.velocity *= 1.05f;
            }
            npc.ai[1] += 1f;
            if (npc.ai[1] >= 90f)
            {
                npc.ai[1] = 0f;
                npc.ai[0] = 2f;
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
            Vector2 vector272 = Main.player[npc.target].Center - npc.Center;
            if (vector272.Length() < 300f && !Collision.SolidCollision(npc.position, npc.width, npc.height))
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
            }
            npc.ai[2] += 1f / 60f;
            float num1406 = 5.5f + npc.ai[2] + vector272.Length() / 150f;
            float num1407 = 35f;
            vector272.Normalize();
            vector272 *= num1406;
            npc.velocity = (npc.velocity * (num1407 - 1f) + vector272) / num1407;
        }
        else if (npc.ai[0] == 2f)
        {
            if (npc.velocity.X < 0f)
            {
                npc.direction = -1;
            }
            else if (npc.velocity.X > 0f)
            {
                npc.direction = 1;
            }
            npc.spriteDirection = npc.direction;
            npc.rotation = (npc.rotation * 7f + npc.velocity.X * 0.1f) / 8f;
            npc.knockBackResist = 0f;
            npc.noTileCollide = true;
            Vector2 vector273 = Main.player[npc.target].Center - npc.Center;
            vector273.Y -= 8f;
            float num1408 = 9f;
            float num1409 = 8f;
            vector273.Normalize();
            vector273 *= num1408;
            npc.velocity = (npc.velocity * (num1409 - 1f) + vector273) / num1409;
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
            if (npc.ai[1] > 10f)
            {
                npc.velocity = vector273;
                if (npc.velocity.X < 0f)
                {
                    npc.direction = -1;
                }
                else
                {
                    npc.direction = 1;
                }
                npc.ai[0] = 2.1f;
                npc.ai[1] = 0f;
            }
        }
        else
        {
            if (npc.ai[0] != 2.1f)
            {
                return;
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
            npc.velocity *= 1.01f;
            npc.knockBackResist = 0f;
            npc.noTileCollide = true;
            npc.ai[1] += 1f;
            int num1410 = 45;
            if (npc.ai[1] > num1410)
            {
                if (!Collision.SolidCollision(npc.position, npc.width, npc.height))
                {
                    npc.ai[0] = 0f;
                    npc.ai[1] = 0f;
                    npc.ai[2] = 0f;
                }
                else if (npc.ai[1] > num1410 * 2)
                {
                    npc.ai[0] = 1f;
                    npc.ai[1] = 0f;
                    npc.ai[2] = 0f;
                }
            }
        }
    }
}
