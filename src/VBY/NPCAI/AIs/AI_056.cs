namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_056(this NPC npc)
    {
        npc.TargetClosest();
        Vector2 vector109 = new(npc.Center.X, npc.Center.Y);
        float num861 = Main.player[npc.target].Center.X - vector109.X;
        float num862 = Main.player[npc.target].Center.Y - vector109.Y;
        float num863 = (float)Math.Sqrt(num861 * num861 + num862 * num862);
        float num864 = 12f;
        num863 = num864 / num863;
        num861 *= num863;
        num862 *= num863;
        npc.velocity.X = (npc.velocity.X * 100f + num861) / 101f;
        npc.velocity.Y = (npc.velocity.Y * 100f + num862) / 101f;
        npc.rotation = (float)Math.Atan2(num862, num861) - 1.57f;
        npc.position += npc.netOffset;
        int num865 = Dust.NewDust(npc.position, npc.width, npc.height, 180);
        Dust dust = Main.dust[num865];
        dust.velocity *= 0.1f;
        Main.dust[num865].scale = 1.3f;
        Main.dust[num865].noGravity = true;
        npc.position -= npc.netOffset;
    }
}
