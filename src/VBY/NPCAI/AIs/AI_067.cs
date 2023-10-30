namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_067(this NPC npc)
    {
        if (Main.netMode != 1)
        {
            int x14 = (int)MathHelper.Clamp((int)(npc.Center.X / 16f), 0f, Main.maxTilesX);
            int y7 = (int)MathHelper.Clamp((int)(npc.Center.Y / 16f), 0f, Main.maxTilesY);
            ITile tile = Main.tile[x14, y7];
            if (tile.shimmer() && tile.liquid > 30)
            {
                npc.GetShimmered();
                return;
            }
        }
        if (npc.type == 359)
        {
            if (npc.ai[3] != 0f)
            {
                npc.scale = npc.ai[3];
                int num1024 = (int)(12f * npc.scale);
                int num1025 = (int)(12f * npc.scale);
                if (num1024 != npc.width)
                {
                    npc.position.X = npc.position.X + npc.width / 2 - num1024 - 2f;
                    npc.width = num1024;
                }
                if (num1025 != npc.height)
                {
                    npc.position.Y = npc.position.Y + npc.height - num1025;
                    npc.height = num1025;
                }
            }
            if (npc.ai[3] == 0f && Main.netMode != 1)
            {
                npc.ai[3] = Main.rand.Next(80, 111) * 0.01f;
                npc.netUpdate = true;
            }
        }
        if (npc.type == 360)
        {
            Lighting.AddLight((int)npc.Center.X / 16, (int)npc.Center.Y / 16, 0.1f, 0.2f, 0.7f);
        }
        if (npc.type == 655)
        {
            Lighting.AddLight((int)npc.Center.X / 16, (int)npc.Center.Y / 16, 0.6f, 0.3f, 0.1f);
        }
        float num1026 = 0.3f;
        if (npc.type == 360 || npc.type == 655)
        {
            num1026 = 0.6f;
        }
        if (npc.ai[0] == 0f)
        {
            npc.TargetClosest();
            npc.directionY = 1;
            npc.ai[0] = 1f;
            if (npc.direction > 0)
            {
                npc.spriteDirection = 1;
            }
        }
        bool flag53 = false;
        if (Main.netMode != 1)
        {
            if (npc.ai[2] == 0f && Main.rand.Next(7200) == 0)
            {
                npc.ai[2] = 2f;
                npc.netUpdate = true;
            }
            if (!npc.collideX && !npc.collideY)
            {
                npc.localAI[3] += 1f;
                if (npc.localAI[3] > 5f)
                {
                    npc.ai[2] = 2f;
                    npc.netUpdate = true;
                }
            }
            else
            {
                npc.localAI[3] = 0f;
            }
        }
        if (npc.ai[2] > 0f)
        {
            npc.ai[1] = 0f;
            npc.ai[0] = 1f;
            npc.directionY = 1;
            if (npc.velocity.Y > num1026)
            {
                npc.rotation += npc.direction * 0.1f;
            }
            else
            {
                npc.rotation = 0f;
            }
            npc.spriteDirection = npc.direction;
            npc.velocity.X = num1026 * npc.direction;
            npc.noGravity = false;
            int x15 = (int)(npc.Center.X + npc.width / 2 * -npc.direction) / 16;
            int y8 = (int)(npc.position.Y + npc.height + 8f) / 16;
            if (Main.tile[x15, y8] != null && !Main.tile[x15, y8].topSlope() && npc.collideY)
            {
                npc.ai[2] -= 1f;
            }
            y8 = (int)(npc.position.Y + npc.height - 4f) / 16;
            x15 = (int)(npc.Center.X + npc.width / 2 * npc.direction) / 16;
            if (Main.tile[x15, y8] != null && Main.tile[x15, y8].bottomSlope())
            {
                npc.direction *= -1;
            }
            if (npc.collideX && npc.velocity.Y == 0f)
            {
                flag53 = true;
                npc.ai[2] = 0f;
                npc.directionY = -1;
                npc.ai[1] = 1f;
            }
            if (npc.velocity.Y == 0f)
            {
                if (npc.localAI[1] == npc.position.X)
                {
                    npc.localAI[2] += 1f;
                    if (npc.localAI[2] > 10f)
                    {
                        npc.direction = 1;
                        npc.velocity.X = npc.direction * num1026;
                        npc.localAI[2] = 0f;
                    }
                }
                else
                {
                    npc.localAI[2] = 0f;
                    npc.localAI[1] = npc.position.X;
                }
            }
        }
        if (npc.ai[2] != 0f)
        {
            return;
        }
        npc.noGravity = true;
        if (npc.ai[1] == 0f)
        {
            if (npc.collideY)
            {
                npc.ai[0] = 2f;
            }
            if (!npc.collideY && npc.ai[0] == 2f)
            {
                npc.direction = -npc.direction;
                npc.ai[1] = 1f;
                npc.ai[0] = 1f;
            }
            if (npc.collideX)
            {
                npc.directionY = -npc.directionY;
                npc.ai[1] = 1f;
            }
        }
        else
        {
            if (npc.collideX)
            {
                npc.ai[0] = 2f;
            }
            if (!npc.collideX && npc.ai[0] == 2f)
            {
                npc.directionY = -npc.directionY;
                npc.ai[1] = 0f;
                npc.ai[0] = 1f;
            }
            if (npc.collideY)
            {
                npc.direction = -npc.direction;
                npc.ai[1] = 0f;
            }
        }
        if (!flag53)
        {
            float num1027 = npc.rotation;
            if (npc.directionY < 0)
            {
                if (npc.direction < 0)
                {
                    if (npc.collideX)
                    {
                        npc.rotation = 1.57f;
                        npc.spriteDirection = -1;
                    }
                    else if (npc.collideY)
                    {
                        npc.rotation = 3.14f;
                        npc.spriteDirection = 1;
                    }
                }
                else if (npc.collideY)
                {
                    npc.rotation = 3.14f;
                    npc.spriteDirection = -1;
                }
                else if (npc.collideX)
                {
                    npc.rotation = 4.71f;
                    npc.spriteDirection = 1;
                }
            }
            else if (npc.direction < 0)
            {
                if (npc.collideY)
                {
                    npc.rotation = 0f;
                    npc.spriteDirection = -1;
                }
                else if (npc.collideX)
                {
                    npc.rotation = 1.57f;
                    npc.spriteDirection = 1;
                }
            }
            else if (npc.collideX)
            {
                npc.rotation = 4.71f;
                npc.spriteDirection = -1;
            }
            else if (npc.collideY)
            {
                npc.rotation = 0f;
                npc.spriteDirection = 1;
            }
            float num1028 = npc.rotation;
            npc.rotation = num1027;
            if (npc.rotation > 6.28)
            {
                npc.rotation -= 6.28f;
            }
            if (npc.rotation < 0f)
            {
                npc.rotation += 6.28f;
            }
            float num1029 = Math.Abs(npc.rotation - num1028);
            float num1030 = 0.1f;
            if (npc.rotation > num1028)
            {
                if ((double)num1029 > 3.14)
                {
                    npc.rotation += num1030;
                }
                else
                {
                    npc.rotation -= num1030;
                    if (npc.rotation < num1028)
                    {
                        npc.rotation = num1028;
                    }
                }
            }
            if (npc.rotation < num1028)
            {
                if ((double)num1029 > 3.14)
                {
                    npc.rotation -= num1030;
                }
                else
                {
                    npc.rotation += num1030;
                    if (npc.rotation > num1028)
                    {
                        npc.rotation = num1028;
                    }
                }
            }
        }
        npc.velocity.X = num1026 * npc.direction;
        npc.velocity.Y = num1026 * npc.directionY;
    }
}
