namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_103(this NPC npc)
    {
        if (npc.direction == 0)
        {
            npc.TargetClosest();
        }

        Point pt = npc.Center.ToTileCoordinates();
        ITile tileSafely7 = Framing.GetTileSafely(pt);
        bool flag108 = tileSafely7.nactive() && (TileID.Sets.Conversion.Sand[tileSafely7.type] || TileID.Sets.Conversion.Sandstone[tileSafely7.type] || TileID.Sets.Conversion.HardenedSand[tileSafely7.type]);
        flag108 |= npc.wet;
        bool flag109 = false;
        npc.TargetClosest(faceTarget: false);
        Vector2 vector308 = npc.targetRect.Center.ToVector2();
        if (Main.player[npc.target].velocity.Y > -0.1f && !Main.player[npc.target].dead && npc.Distance(vector308) > 150f)
        {
            flag109 = true;
        }
        if (npc.localAI[0] == -1f && !flag108)
        {
            npc.localAI[0] = 20f;
        }
        if (npc.localAI[0] > 0f)
        {
            npc.localAI[0]--;
        }
        if (flag108)
        {
            if (npc.soundDelay == 0)
            {
                float num1549 = npc.Distance(vector308) / 40f;
                if (num1549 < 10f)
                {
                    num1549 = 10f;
                }
                if (num1549 > 20f)
                {
                    num1549 = 20f;
                }
                npc.soundDelay = (int)num1549;
                SoundEngine.PlaySound(15, npc.Center, 4);
            }

            _ = npc.ai[1];
            bool flag110 = false;
            pt = (npc.Center + new Vector2(0f, 24f)).ToTileCoordinates();
            tileSafely7 = Framing.GetTileSafely(pt.X, pt.Y - 2);
            if (tileSafely7.nactive() && (TileID.Sets.Conversion.Sand[tileSafely7.type] || TileID.Sets.Conversion.Sandstone[tileSafely7.type] || TileID.Sets.Conversion.HardenedSand[tileSafely7.type]))
            {
                flag110 = true;
            }
            npc.ai[1] = flag110.ToInt();
            if (npc.ai[2] < 30f)
            {
                npc.ai[2]++;
            }
            if (flag109)
            {
                npc.TargetClosest();
                npc.velocity.X += npc.direction * 0.15f;
                npc.velocity.Y += npc.directionY * 0.15f;
                if (npc.velocity.X > 5f)
                {
                    npc.velocity.X = 5f;
                }
                if (npc.velocity.X < -5f)
                {
                    npc.velocity.X = -5f;
                }
                if (npc.velocity.Y > 3f)
                {
                    npc.velocity.Y = 3f;
                }
                if (npc.velocity.Y < -3f)
                {
                    npc.velocity.Y = -3f;
                }
                Vector2 vec4 = npc.Center + npc.velocity.SafeNormalize(Vector2.Zero) * npc.Size.Length() / 2f + npc.velocity;
                pt = vec4.ToTileCoordinates();
                tileSafely7 = Framing.GetTileSafely(pt);
                bool flag111 = tileSafely7.nactive() && (TileID.Sets.Conversion.Sand[tileSafely7.type] || TileID.Sets.Conversion.Sandstone[tileSafely7.type] || TileID.Sets.Conversion.HardenedSand[tileSafely7.type]);
                if (!flag111 && npc.wet)
                {
                    flag111 = tileSafely7.liquid > 0;
                }
                int num1551 = 400;
                if (Main.remixWorld)
                {
                    num1551 = 700;
                }
                if (!flag111 && Math.Sign(npc.velocity.X) == npc.direction && npc.Distance(vector308) < num1551 && (npc.ai[2] >= 30f || npc.ai[2] < 0f))
                {
                    if (npc.localAI[0] == 0f)
                    {
                        SoundEngine.PlaySound(14, npc.Center, 542);
                        npc.localAI[0] = -1f;
                    }
                    npc.ai[2] = -30f;
                    Vector2 vector309 = npc.DirectionTo(vector308 + new Vector2(0f, -80f));
                    npc.velocity = vector309 * 12f;
                }
            }
            else
            {
                if (npc.collideX)
                {
                    npc.velocity.X *= -1f;
                    npc.direction *= -1;
                    npc.netUpdate = true;
                }
                if (npc.collideY)
                {
                    npc.netUpdate = true;
                    npc.velocity.Y *= -1f;
                    npc.directionY = Math.Sign(npc.velocity.Y);
                    npc.ai[0] = npc.directionY;
                }
                float num1552 = 6f;
                npc.velocity.X += npc.direction * 0.1f;
                if (npc.velocity.X < 0f - num1552 || npc.velocity.X > num1552)
                {
                    npc.velocity.X *= 0.95f;
                }
                if (flag110)
                {
                    npc.ai[0] = -1f;
                }
                else
                {
                    npc.ai[0] = 1f;
                }
                float num1553 = 0.06f;
                float num1554 = 0.01f;
                if (npc.ai[0] == -1f)
                {
                    npc.velocity.Y -= num1554;
                    if (npc.velocity.Y < 0f - num1553)
                    {
                        npc.ai[0] = 1f;
                    }
                }
                else
                {
                    npc.velocity.Y += num1554;
                    if (npc.velocity.Y > num1553)
                    {
                        npc.ai[0] = -1f;
                    }
                }
                if (npc.velocity.Y > 0.4f || npc.velocity.Y < -0.4f)
                {
                    npc.velocity.Y *= 0.95f;
                }
            }
        }
        else
        {
            if (npc.velocity.Y == 0f)
            {
                if (flag109)
                {
                    npc.TargetClosest();
                }
                float num1555 = 1f;
                npc.velocity.X += npc.direction * 0.1f;
                if (npc.velocity.X < 0f - num1555 || npc.velocity.X > num1555)
                {
                    npc.velocity.X *= 0.95f;
                }
            }
            npc.velocity.Y += 0.3f;
            if (npc.velocity.Y > 10f)
            {
                npc.velocity.Y = 10f;
            }
            npc.ai[0] = 1f;
        }
        npc.rotation = npc.velocity.Y * npc.direction * 0.1f;
        if (npc.rotation < -0.2f)
        {
            npc.rotation = -0.2f;
        }
        if (npc.rotation > 0.2f)
        {
            npc.rotation = 0.2f;
        }
    }
}
