namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_050(this NPC npc)
    {
        npc.EncourageDespawn(5);
        if (npc.type == 261)
        {
            npc.noTileCollide = false;
            if (npc.collideX || npc.collideY)
            {
                npc.life = 0;
                npc.HitEffect(0, 100.0);
                npc.checkDead();
                return;
            }
        }
        else
        {
            npc.noTileCollide = true;
        }
        npc.velocity.Y += 0.02f;
        npc.TargetClosest();
        if (npc.velocity.Y < 0f && Main.player[npc.target].position.Y > npc.position.Y + 100f)
        {
            npc.velocity.Y *= 0.95f;
        }
        if (npc.velocity.Y > 1f)
        {
            npc.velocity.Y = 1f;
        }
        if (npc.position.X + npc.width < Main.player[npc.target].position.X)
        {
            if (npc.velocity.X < 0f)
            {
                npc.velocity.X *= 0.98f;
            }
            if (Main.expertMode && npc.velocity.X < 0f)
            {
                npc.velocity.X *= 0.98f;
            }
            npc.velocity.X += 0.1f;
            if (Main.expertMode)
            {
                npc.velocity.X += 0.1f;
            }
        }
        else if (npc.position.X > Main.player[npc.target].position.X + Main.player[npc.target].width)
        {
            if (npc.velocity.X > 0f)
            {
                npc.velocity.X *= 0.98f;
            }
            if (Main.expertMode && npc.velocity.X > 0f)
            {
                npc.velocity.X *= 0.98f;
            }
            npc.velocity.X -= 0.1f;
            if (Main.expertMode)
            {
                npc.velocity.X -= 0.1f;
            }
        }
        if (npc.velocity.X > 5f || npc.velocity.X < -5f)
        {
            npc.velocity.X *= 0.97f;
        }
        npc.rotation = npc.velocity.X * 0.2f;
    }
}
