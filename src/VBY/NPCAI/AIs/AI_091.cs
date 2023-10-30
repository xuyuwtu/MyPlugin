namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_091(this NPC npc)
    {
        npc.noGravity = true;
        npc.noTileCollide = false;
        npc.dontTakeDamage = false;
        if (npc.justHit && Main.netMode != 1 && Main.expertMode && Main.rand.Next(6) == 0)
        {
            npc.netUpdate = true;
            npc.ai[0] = -1f;
            npc.ai[1] = 0f;
        }
        if (npc.ai[0] == -1f)
        {
            npc.dontTakeDamage = true;
            npc.noGravity = false;
            npc.velocity.X *= 0.98f;
            npc.ai[1] += 1f;
            if (npc.ai[1] >= 120f)
            {
                npc.ai[0] = (npc.ai[1] = (npc.ai[2] = (npc.ai[3] = 0f)));
            }
        }
        else if (npc.ai[0] == 0f)
        {
            npc.TargetClosest();
            if (Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
            {
                npc.ai[0] = 1f;
                return;
            }
            Vector2 vector274 = Main.player[npc.target].Center - npc.Center;
            vector274.Y -= Main.player[npc.target].height / 4;
            _ = vector274.Length();
            Vector2 center36 = npc.Center;
            center36.X = Main.player[npc.target].Center.X;
            Vector2 vector275 = center36 - npc.Center;
            if (vector275.Length() > 8f && Collision.CanHit(npc.Center, 1, 1, center36, 1, 1))
            {
                npc.ai[0] = 3f;
                npc.ai[1] = center36.X;
                npc.ai[2] = center36.Y;
                Vector2 center37 = npc.Center;
                center37.Y = Main.player[npc.target].Center.Y;
                if (vector275.Length() > 8f && Collision.CanHit(npc.Center, 1, 1, center37, 1, 1) && Collision.CanHit(center37, 1, 1, Main.player[npc.target].position, 1, 1))
                {
                    npc.ai[0] = 3f;
                    npc.ai[1] = center37.X;
                    npc.ai[2] = center37.Y;
                }
            }
            else
            {
                center36 = npc.Center;
                center36.Y = Main.player[npc.target].Center.Y;
                if ((center36 - npc.Center).Length() > 8f && Collision.CanHit(npc.Center, 1, 1, center36, 1, 1))
                {
                    npc.ai[0] = 3f;
                    npc.ai[1] = center36.X;
                    npc.ai[2] = center36.Y;
                }
            }
            if (npc.ai[0] == 0f)
            {
                npc.localAI[0] = 0f;
                vector274.Normalize();
                vector274 *= 0.5f;
                npc.velocity += vector274;
                npc.ai[0] = 4f;
                npc.ai[1] = 0f;
            }
        }
        else if (npc.ai[0] == 1f)
        {
            Vector2 vector276 = Main.player[npc.target].Center - npc.Center;
            float num1412 = vector276.Length();
            float num1413 = 2f;
            num1413 += num1412 / 200f;
            int num1414 = 50;
            vector276.Normalize();
            vector276 *= num1413;
            npc.velocity = (npc.velocity * (num1414 - 1) + vector276) / num1414;
            if (!Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
            }
        }
        else if (npc.ai[0] == 2f)
        {
            npc.noTileCollide = true;
            Vector2 vector277 = Main.player[npc.target].Center - npc.Center;
            float num1415 = vector277.Length();
            float num1416 = 2f;
            int num1417 = 4;
            vector277.Normalize();
            vector277 *= num1416;
            npc.velocity = (npc.velocity * (num1417 - 1) + vector277) / num1417;
            if (num1415 < 600f && !Collision.SolidCollision(npc.position, npc.width, npc.height))
            {
                npc.ai[0] = 0f;
            }
        }
        else if (npc.ai[0] == 3f)
        {
            Vector2 vector278 = new(npc.ai[1], npc.ai[2]);
            Vector2 vector279 = vector278 - npc.Center;
            float num1418 = vector279.Length();
            float num1419 = 1f;
            float num1420 = 3f;
            vector279.Normalize();
            vector279 *= num1419;
            npc.velocity = (npc.velocity * (num1420 - 1f) + vector279) / num1420;
            if (npc.collideX || npc.collideY)
            {
                npc.ai[0] = 4f;
                npc.ai[1] = 0f;
            }
            if (num1418 < num1419 || num1418 > 800f || Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
            {
                npc.ai[0] = 0f;
            }
        }
        else
        {
            if (npc.ai[0] != 4f)
            {
                return;
            }
            if (npc.collideX)
            {
                npc.velocity.X *= -0.8f;
            }
            if (npc.collideY)
            {
                npc.velocity.Y *= -0.8f;
            }
            Vector2 vector280;
            if (npc.velocity.X == 0f && npc.velocity.Y == 0f)
            {
                vector280 = Main.player[npc.target].Center - npc.Center;
                vector280.Y -= Main.player[npc.target].height / 4;
                vector280.Normalize();
                npc.velocity = vector280 * 0.1f;
            }
            float num1421 = 1.5f;
            float num1422 = 20f;
            vector280 = npc.velocity;
            vector280.Normalize();
            vector280 *= num1421;
            npc.velocity = (npc.velocity * (num1422 - 1f) + vector280) / num1422;
            npc.ai[1] += 1f;
            if (npc.ai[1] > 180f)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
            }
            if (Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
            {
                npc.ai[0] = 0f;
            }
            npc.localAI[0] += 1f;
            if (!(npc.localAI[0] >= 5f) || Collision.SolidCollision(npc.position - new Vector2(10f, 10f), npc.width + 20, npc.height + 20))
            {
                return;
            }
            npc.localAI[0] = 0f;
            Vector2 center38 = npc.Center;
            center38.X = Main.player[npc.target].Center.X;
            if (Collision.CanHit(npc.Center, 1, 1, center38, 1, 1) && Collision.CanHit(npc.Center, 1, 1, center38, 1, 1) && Collision.CanHit(Main.player[npc.target].Center, 1, 1, center38, 1, 1))
            {
                npc.ai[0] = 3f;
                npc.ai[1] = center38.X;
                npc.ai[2] = center38.Y;
                return;
            }
            center38 = npc.Center;
            center38.Y = Main.player[npc.target].Center.Y;
            if (Collision.CanHit(npc.Center, 1, 1, center38, 1, 1) && Collision.CanHit(Main.player[npc.target].Center, 1, 1, center38, 1, 1))
            {
                npc.ai[0] = 3f;
                npc.ai[1] = center38.X;
                npc.ai[2] = center38.Y;
            }
        }
    }
}
