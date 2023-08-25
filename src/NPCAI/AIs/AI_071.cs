namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_071(this NPC npc)
    {
        npc.noTileCollide = true;
        int num1045 = 90;
        if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
        {
            npc.TargetClosest(faceTarget: false);
            npc.direction = 1;
            npc.netUpdate = true;
        }
        if (npc.ai[0] == 0f)
        {
            npc.ai[1]++;
            _ = npc.type;
            _ = 372;
            npc.noGravity = true;
            npc.dontTakeDamage = true;
            npc.velocity.Y = npc.ai[3];
            if (npc.type == 373)
            {
                float num1046 = (float)Math.PI / 30f;
                float num1047 = npc.ai[2];
                float num1048 = (float)(Math.Cos(num1046 * npc.localAI[1]) - 0.5) * num1047;
                npc.position.X -= num1048 * -npc.direction;
                npc.localAI[1]++;
                num1048 = (float)(Math.Cos(num1046 * npc.localAI[1]) - 0.5) * num1047;
                npc.position.X += num1048 * -npc.direction;
                if (Math.Abs(Math.Cos(num1046 * npc.localAI[1]) - 0.5) > 0.25)
                {
                    npc.spriteDirection = ((!(Math.Cos(num1046 * npc.localAI[1]) - 0.5 >= 0.0)) ? 1 : (-1));
                }
                npc.rotation = npc.velocity.Y * npc.spriteDirection * 0.1f;
                if (npc.rotation < -0.2)
                {
                    npc.rotation = -0.2f;
                }
                if (npc.rotation > 0.2)
                {
                    npc.rotation = 0.2f;
                }
                npc.alpha -= 6;
                if (npc.alpha < 0)
                {
                    npc.alpha = 0;
                }
            }
            if (npc.ai[1] >= num1045)
            {
                npc.ai[0] = 1f;
                npc.ai[1] = 0f;
                if (!Collision.SolidCollision(npc.position, npc.width, npc.height))
                {
                    npc.ai[1] = 1f;
                }
                SoundEngine.PlaySound(4, (int)npc.Center.X, (int)npc.Center.Y, 19);
                npc.TargetClosest();
                npc.spriteDirection = npc.direction;
                Vector2 vector136 = Main.player[npc.target].Center - npc.Center;
                vector136.Normalize();
                npc.velocity = vector136 * 16f;
                npc.rotation = npc.velocity.ToRotation();
                if (npc.direction == -1)
                {
                    npc.rotation += (float)Math.PI;
                }
                npc.netUpdate = true;
            }
        }
        else
        {
            if (npc.ai[0] != 1f)
            {
                return;
            }
            npc.noGravity = true;
            if (!Collision.SolidCollision(npc.position, npc.width, npc.height))
            {
                if (npc.ai[1] < 1f)
                {
                    npc.ai[1] = 1f;
                }
            }
            else
            {
                npc.alpha -= 15;
                if (npc.alpha < 150)
                {
                    npc.alpha = 150;
                }
            }
            if (npc.ai[1] >= 1f)
            {
                npc.alpha -= 60;
                if (npc.alpha < 0)
                {
                    npc.alpha = 0;
                }
                npc.dontTakeDamage = false;
                npc.ai[1]++;
                if (Collision.SolidCollision(npc.position, npc.width, npc.height))
                {
                    if (npc.DeathSound != null)
                    {
                        SoundEngine.PlaySound(npc.DeathSound, npc.position);
                    }
                    npc.life = 0;
                    npc.HitEffect();
                    npc.active = false;
                    return;
                }
            }
            if (npc.ai[1] >= 60f)
            {
                npc.noGravity = false;
            }
            npc.rotation = npc.velocity.ToRotation();
            if (npc.direction == -1)
            {
                npc.rotation += (float)Math.PI;
            }
        }
    }
}
