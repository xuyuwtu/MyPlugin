namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_023(this NPC npc)
    {
        npc.noGravity = true;
        npc.noTileCollide = true;
        if (npc.type == 83)
        {
            Lighting.AddLight((int)((npc.position.X + npc.width / 2) / 16f), (int)((npc.position.Y + npc.height / 2) / 16f), 0.2f, 0.05f, 0.3f);
        }
        else if (npc.type == 179)
        {
            Lighting.AddLight((int)((npc.position.X + npc.width / 2) / 16f), (int)((npc.position.Y + npc.height / 2) / 16f), 0.3f, 0.15f, 0.05f);
        }
        else
        {
            Lighting.AddLight((int)((npc.position.X + npc.width / 2) / 16f), (int)((npc.position.Y + npc.height / 2) / 16f), 0.05f, 0.2f, 0.3f);
        }
        if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
        {
            npc.TargetClosest();
        }
        if (npc.ai[0] == 0f)
        {
            float num325 = 9f;
            Vector2 vector37 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num326 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector37.X;
            float num327 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector37.Y;
            float num328 = (float)Math.Sqrt(num326 * num326 + num327 * num327);
            num328 = num325 / num328;
            num326 *= num328;
            num327 *= num328;
            npc.velocity.X = num326;
            npc.velocity.Y = num327;
            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 0.785f;
            npc.ai[0] = 1f;
            npc.ai[1] = 0f;
            npc.netUpdate = true;
        }
        else if (npc.ai[0] == 1f)
        {
            if (npc.justHit)
            {
                npc.ai[0] = 2f;
                npc.ai[1] = 0f;
            }
            npc.velocity *= 0.99f;
            npc.ai[1] += 1f;
            if (npc.ai[1] >= 100f)
            {
                npc.netUpdate = true;
                npc.ai[0] = 2f;
                npc.ai[1] = 0f;
                npc.velocity.X = 0f;
                npc.velocity.Y = 0f;
            }
            else
            {
                npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 0.785f;
            }
        }
        else
        {
            if (npc.justHit)
            {
                npc.ai[0] = 2f;
                npc.ai[1] = 0f;
            }
            npc.velocity *= 0.96f;
            npc.ai[1] += 1f;
            float num330 = npc.ai[1] / 120f;
            num330 = 0.1f + num330 * 0.4f;
            npc.rotation += num330 * npc.direction;
            if (npc.ai[1] >= 120f)
            {
                npc.netUpdate = true;
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
            }
        }
    }
}
