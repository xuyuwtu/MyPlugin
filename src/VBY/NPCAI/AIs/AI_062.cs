namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_062(this NPC npc)
    {
        npc.TargetClosest();
        npc.rotation = Math.Abs(npc.velocity.X) * npc.direction * 0.1f;
        npc.spriteDirection = npc.direction;
        float num984 = 7f;
        Vector2 vector132 = new(npc.Center.X + npc.direction * 20, npc.Center.Y + 6f);
        float num985 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector132.X;
        float num986 = Main.player[npc.target].position.Y - vector132.Y;
        float num987 = (float)Math.Sqrt(num985 * num985 + num986 * num986);
        float num988 = num984 / num987;
        num985 *= num988;
        num986 *= num988;
        bool flag49 = Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1);
        if (Main.dayTime)
        {
            int num989 = 60;
            npc.velocity.X = (npc.velocity.X * (num989 - 1) - num985) / num989;
            npc.velocity.Y = (npc.velocity.Y * (num989 - 1) - num986) / num989;
            npc.EncourageDespawn(10);
            return;
        }
        if (num987 > 600f || !flag49)
        {
            int num990 = 60;
            npc.velocity.X = (npc.velocity.X * (num990 - 1) + num985) / num990;
            npc.velocity.Y = (npc.velocity.Y * (num990 - 1) + num986) / num990;
            return;
        }
        npc.velocity *= 0.98f;
        if (Math.Abs(npc.velocity.X) < 1f && Math.Abs(npc.velocity.Y) < 1f && Main.netMode != 1)
        {
            npc.localAI[0] += 1f;
            if (npc.localAI[0] >= 15f)
            {
                npc.localAI[0] = 0f;
                num985 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector132.X;
                num986 = Main.player[npc.target].Center.Y - vector132.Y;
                num985 += Main.rand.Next(-35, 36);
                num986 += Main.rand.Next(-35, 36);
                num985 *= 1f + Main.rand.Next(-20, 21) * 0.015f;
                num986 *= 1f + Main.rand.Next(-20, 21) * 0.015f;
                num987 = (float)Math.Sqrt(num985 * num985 + num986 * num986);
                num984 = 10f;
                num988 = num984 / num987;
                num985 *= num988;
                num986 *= num988;
                num985 *= 1f + Main.rand.Next(-20, 21) * 0.0125f;
                num986 *= 1f + Main.rand.Next(-20, 21) * 0.0125f;
                _ = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector132.X, vector132.Y, num985, num986, 180, 32, 0f, Main.myPlayer);
            }
        }
    }
}
