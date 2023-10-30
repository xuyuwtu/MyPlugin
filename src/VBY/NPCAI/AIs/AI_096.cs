namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_096(this NPC npc)
    {
        float num1465 = 5f;
        float moveSpeed = 0.15f;
        npc.TargetClosest();
        Vector2 desiredVelocity3 = Main.player[npc.target].Center - npc.Center + new Vector2(0f, -250f);
        float num1466 = desiredVelocity3.Length();
        if (num1466 < 20f)
        {
            desiredVelocity3 = npc.velocity;
        }
        else if (num1466 < 40f)
        {
            desiredVelocity3.Normalize();
            desiredVelocity3 *= num1465 * 0.35f;
        }
        else if (num1466 < 80f)
        {
            desiredVelocity3.Normalize();
            desiredVelocity3 *= num1465 * 0.65f;
        }
        else
        {
            desiredVelocity3.Normalize();
            desiredVelocity3 *= num1465;
        }
        npc.SimpleFlyMovement(desiredVelocity3, moveSpeed);
        npc.rotation = npc.velocity.X * 0.1f;
        if (!((npc.ai[0] += 1f) >= 70f))
        {
            return;
        }
        npc.ai[0] = 0f;
        if (Main.netMode != 1)
        {
            Vector2 vector288 = Vector2.Zero;
            while (Math.Abs(vector288.X) < 1.5f)
            {
                vector288 = Vector2.UnitY.RotatedByRandom(1.5707963705062866) * new Vector2(5f, 3f);
            }
            Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center.X, npc.Center.Y, vector288.X, vector288.Y, 539, 60, 0f, Main.myPlayer, 0f, npc.whoAmI);
        }
    }
}
