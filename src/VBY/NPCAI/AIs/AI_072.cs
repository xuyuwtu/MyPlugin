namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_072(this NPC npc)
    {
        if (npc.type == 384)
        {
            int num1049 = (int)npc.ai[0];
            if (Main.npc[num1049].active && Main.npc[num1049].type == 383)
            {
                npc.velocity = Vector2.Zero;
                npc.position = Main.npc[num1049].Center;
                npc.position.X -= npc.width / 2;
                npc.position.Y -= npc.height / 2;
                npc.gfxOffY = Main.npc[num1049].gfxOffY;
                Lighting.AddLight((int)npc.Center.X / 16, (int)npc.Center.Y / 16, 0.1f, 0.5f, 0.7f);
            }
            else
            {
                npc.life = 0;
                npc.HitEffect();
                npc.active = false;
            }
        }
    }
}
