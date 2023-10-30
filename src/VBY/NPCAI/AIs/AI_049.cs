namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_049(this NPC npc)
    {
        npc.noGravity = true;
        npc.TargetClosest();
        float num755 = 4f;
        float num756 = 0.25f;
        Vector2 vector96 = new(npc.Center.X, npc.Center.Y);
        float num757 = Main.player[npc.target].Center.X - vector96.X;
        float num758 = Main.player[npc.target].Center.Y - vector96.Y - 200f;
        float num759 = (float)Math.Sqrt(num757 * num757 + num758 * num758);
        if (num759 < 20f)
        {
            num757 = npc.velocity.X;
            num758 = npc.velocity.Y;
        }
        else
        {
            num759 = num755 / num759;
            num757 *= num759;
            num758 *= num759;
        }
        if (npc.velocity.X < num757)
        {
            npc.velocity.X += num756;
            if (npc.velocity.X < 0f && num757 > 0f)
            {
                npc.velocity.X += num756 * 2f;
            }
        }
        else if (npc.velocity.X > num757)
        {
            npc.velocity.X -= num756;
            if (npc.velocity.X > 0f && num757 < 0f)
            {
                npc.velocity.X -= num756 * 2f;
            }
        }
        if (npc.velocity.Y < num758)
        {
            npc.velocity.Y += num756;
            if (npc.velocity.Y < 0f && num758 > 0f)
            {
                npc.velocity.Y += num756 * 2f;
            }
        }
        else if (npc.velocity.Y > num758)
        {
            npc.velocity.Y -= num756;
            if (npc.velocity.Y > 0f && num758 < 0f)
            {
                npc.velocity.Y -= num756 * 2f;
            }
        }
        if (npc.position.X + npc.width > Main.player[npc.target].position.X && npc.position.X < Main.player[npc.target].position.X + Main.player[npc.target].width && npc.position.Y + npc.height < Main.player[npc.target].position.Y && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height) && Main.netMode != 1)
        {
            npc.ai[0] += 1f;
            if (npc.ai[0] > 8f)
            {
                npc.ai[0] = 0f;
                int num760 = (int)(npc.position.X + 10f + Main.rand.Next(npc.width - 20));
                int num761 = (int)(npc.position.Y + npc.height + 4f);
                Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), num760, num761, 0f, 5f, 264, 20, 0f, Main.myPlayer);
            }
        }
    }
}
