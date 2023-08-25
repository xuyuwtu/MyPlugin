namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_017(this NPC npc)
    {
        npc.noGravity = true;
        if (npc.ai[0] == 0f)
        {
            npc.noGravity = false;
            npc.TargetClosest();
            if (Main.netMode != 1)
            {
                if (npc.velocity.X != 0f || npc.velocity.Y < 0f || npc.velocity.Y > 0.3)
                {
                    npc.ai[0] = 1f;
                    npc.netUpdate = true;
                }
                else
                {
                    Rectangle rectangle = new((int)Main.player[npc.target].position.X, (int)Main.player[npc.target].position.Y, Main.player[npc.target].width, Main.player[npc.target].height);
                    if (new Rectangle((int)npc.position.X - 100, (int)npc.position.Y - 100, npc.width + 200, npc.height + 200).Intersects(rectangle) || npc.life < npc.lifeMax)
                    {
                        npc.ai[0] = 1f;
                        npc.velocity.Y -= 6f;
                        npc.netUpdate = true;
                    }
                }
            }
        }
        else if (!Main.player[npc.target].dead)
        {
            if (npc.collideX)
            {
                npc.velocity.X = npc.oldVelocity.X * -0.5f;
                if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 2f)
                {
                    npc.velocity.X = 2f;
                }
                if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -2f)
                {
                    npc.velocity.X = -2f;
                }
            }
            if (npc.collideY)
            {
                npc.velocity.Y = npc.oldVelocity.Y * -0.5f;
                if (npc.velocity.Y > 0f && npc.velocity.Y < 1f)
                {
                    npc.velocity.Y = 1f;
                }
                if (npc.velocity.Y < 0f && npc.velocity.Y > -1f)
                {
                    npc.velocity.Y = -1f;
                }
            }
            npc.TargetClosest();
            if (npc.direction == -1 && npc.velocity.X > -3f)
            {
                npc.velocity.X -= 0.1f;
                if (npc.velocity.X > 3f)
                {
                    npc.velocity.X -= 0.1f;
                }
                else if (npc.velocity.X > 0f)
                {
                    npc.velocity.X -= 0.05f;
                }
                if (npc.velocity.X < -3f)
                {
                    npc.velocity.X = -3f;
                }
            }
            else if (npc.direction == 1 && npc.velocity.X < 3f)
            {
                npc.velocity.X += 0.1f;
                if (npc.velocity.X < -3f)
                {
                    npc.velocity.X += 0.1f;
                }
                else if (npc.velocity.X < 0f)
                {
                    npc.velocity.X += 0.05f;
                }
                if (npc.velocity.X > 3f)
                {
                    npc.velocity.X = 3f;
                }
            }
            float num266 = Math.Abs(npc.position.X + npc.width / 2 - (Main.player[npc.target].position.X + Main.player[npc.target].width / 2));
            float num267 = Main.player[npc.target].position.Y - npc.height / 2;
            if (num266 > 50f)
            {
                num267 -= 100f;
            }
            if (npc.position.Y < num267)
            {
                npc.velocity.Y += 0.05f;
                if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y += 0.01f;
                }
            }
            else
            {
                npc.velocity.Y -= 0.05f;
                if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y -= 0.01f;
                }
            }
            if (npc.velocity.Y < -3f)
            {
                npc.velocity.Y = -3f;
            }
            if (npc.velocity.Y > 3f)
            {
                npc.velocity.Y = 3f;
            }
        }
        if (npc.wet)
        {
            if (npc.velocity.Y > 0f)
            {
                npc.velocity.Y *= 0.95f;
            }
            npc.velocity.Y -= 0.5f;
            if (npc.velocity.Y < -4f)
            {
                npc.velocity.Y = -4f;
            }
            npc.TargetClosest();
        }

    }
}
