namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_021(this NPC npc)
    {
        if (npc.ai[0] == 0f)
        {
            npc.TargetClosest();
            npc.directionY = 1;
            npc.ai[0] = 1f;
        }
        int num287 = 6;
        if (npc.ai[1] == 0f)
        {
            npc.rotation += npc.direction * npc.directionY * 0.13f;
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
            npc.rotation -= npc.direction * npc.directionY * 0.13f;
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
        npc.velocity.X = num287 * npc.direction;
        npc.velocity.Y = num287 * npc.directionY;
        float num288 = (270 - Main.mouseTextColor) / 400f;
        Lighting.AddLight((int)(npc.position.X + npc.width / 2) / 16, (int)(npc.position.Y + npc.height / 2) / 16, 0.9f, 0.3f + num288, 0.2f);
    }
}
