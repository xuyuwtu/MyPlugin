namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_080(this NPC npc)
    {
        if (npc.ai[0] == 0f)
        {
            if (npc.direction == 0)
            {
                npc.TargetClosest();
                npc.netUpdate = true;
            }
            if (npc.collideX)
            {
                npc.direction = -npc.direction;
                npc.netUpdate = true;
            }
            npc.velocity.X = 3f * npc.direction;
            Vector2 center24 = npc.Center;
            Point point10 = center24.ToTileCoordinates();
            int num1251 = 30;
            if (WorldGen.InWorld(point10.X, point10.Y, 30))
            {
                for (int num1252 = 0; num1252 < 30; num1252++)
                {
                    if (WorldGen.SolidTile(point10.X, point10.Y + num1252))
                    {
                        num1251 = num1252;
                        break;
                    }
                }
            }
            if (num1251 < 15)
            {
                npc.velocity.Y = Math.Max(npc.velocity.Y - 0.05f, -3.5f);
            }
            else if (num1251 < 20)
            {
                npc.velocity.Y *= 0.95f;
            }
            else
            {
                npc.velocity.Y = Math.Min(npc.velocity.Y + 0.05f, 1.5f);
            }
            int num1253 = npc.FindClosestPlayer(out float distanceToPlayer);
            if (num1253 == -1 || Main.player[num1253].dead)
            {
                return;
            }
            if (distanceToPlayer < 352f && Main.player[num1253].Center.Y > npc.Center.Y)
            {
                npc.ai[0] = 1f;
                npc.ai[1] = 0f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 1f)
        {
            npc.ai[1]++;
            npc.velocity *= 0.95f;
            if (npc.ai[1] >= 60f)
            {
                npc.ai[1] = 0f;
                npc.ai[0] = 2f;
                int num1254 = npc.FindClosestPlayer();
                if (num1254 != -1)
                {
                    npc.ai[3] = ((Main.player[num1254].Center.X > npc.Center.X) ? (-1f) : 1f);
                }
                else
                {
                    npc.ai[3] = 1f;
                }
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 2f)
        {
            npc.noTileCollide = true;
            npc.ai[1]++;
            npc.velocity.Y = Math.Max(npc.velocity.Y - 0.1f, -10f);
            npc.velocity.X = Math.Min(npc.velocity.X + npc.ai[3] * 0.05f, 4f);
            if ((npc.position.Y < -npc.height || npc.ai[1] >= 180f) && Main.netMode != 1)
            {
                Main.StartInvasion(4);
                npc.active = false;
                npc.netUpdate = true;
            }
        }
        Vector3 rgb = Color.SkyBlue.ToVector3();
        if (npc.ai[0] == 2f)
        {
            rgb = Color.Red.ToVector3();
        }
        rgb *= 0.65f;
        Lighting.AddLight(npc.Center, rgb);
    }
}
