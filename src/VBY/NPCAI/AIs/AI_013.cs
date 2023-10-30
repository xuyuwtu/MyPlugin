using OTAPI;
using Terraria.GameContent;

namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_013(this NPC npc)
    {
        if (npc.ai[0] < 0f || npc.ai[0] >= Main.maxTilesX || npc.ai[1] < 0f || npc.ai[1] >= Main.maxTilesX)
        {
            return;
        }
        if (Main.tile[(int)npc.ai[0], (int)npc.ai[1]] == null)
        {
            Main.tile[(int)npc.ai[0], (int)npc.ai[1]] = Hooks.Tile.InvokeCreate();
        }
        if (!Main.tile[(int)npc.ai[0], (int)npc.ai[1]].active())
        {
            npc.life = -1;
            npc.HitEffect();
            npc.active = false;
            return;
        }
        FixExploitManEaters.ProtectSpot((int)npc.ai[0], (int)npc.ai[1]);
        npc.TargetClosest();
        float num190 = 0.035f;
        float num191 = 150f;
        if (npc.type == 43)
        {
            num191 = ((!Main.getGoodWorld) ? 250f : 350f);
        }
        if (npc.type == 101)
        {
            num191 = 175f;
        }
        if (npc.type == 259)
        {
            num191 = 100f;
        }
        if (npc.type == 175)
        {
            num191 = 500f;
            num190 = 0.05f;
        }
        if (npc.type == 260)
        {
            num191 = 350f;
            num190 = 0.15f;
        }
        npc.ai[2] += 1f;
        if (npc.ai[2] > 300f)
        {
            num191 = (int)((double)num191 * 1.3);
            if (npc.ai[2] > 450f)
            {
                npc.ai[2] = 0f;
            }
        }
        Vector2 vector25 = new(npc.ai[0] * 16f + 8f, npc.ai[1] * 16f + 8f);
        float num192 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - npc.width / 2 - vector25.X;
        float num193 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - npc.height / 2 - vector25.Y;
        float num194 = (float)Math.Sqrt(num192 * num192 + num193 * num193);
        if (num194 > num191)
        {
            num194 = num191 / num194;
            num192 *= num194;
            num193 *= num194;
        }
        if (npc.position.X < npc.ai[0] * 16f + 8f + num192)
        {
            npc.velocity.X += num190;
            if (npc.velocity.X < 0f && num192 > 0f)
            {
                npc.velocity.X += num190 * 1.5f;
            }
        }
        else if (npc.position.X > npc.ai[0] * 16f + 8f + num192)
        {
            npc.velocity.X -= num190;
            if (npc.velocity.X > 0f && num192 < 0f)
            {
                npc.velocity.X -= num190 * 1.5f;
            }
        }
        if (npc.position.Y < npc.ai[1] * 16f + 8f + num193)
        {
            npc.velocity.Y += num190;
            if (npc.velocity.Y < 0f && num193 > 0f)
            {
                npc.velocity.Y += num190 * 1.5f;
            }
        }
        else if (npc.position.Y > npc.ai[1] * 16f + 8f + num193)
        {
            npc.velocity.Y -= num190;
            if (npc.velocity.Y > 0f && num193 < 0f)
            {
                npc.velocity.Y -= num190 * 1.5f;
            }
        }
        if (npc.type == 43)
        {
            if (Main.getGoodWorld)
            {
                if (npc.velocity.X > 3.5)
                {
                    npc.velocity.X = 3.5f;
                }
                if (npc.velocity.X < -3.5)
                {
                    npc.velocity.X = -3.5f;
                }
                if (npc.velocity.Y > 3.5)
                {
                    npc.velocity.Y = 3.5f;
                }
                if (npc.velocity.Y < -3.5)
                {
                    npc.velocity.Y = -3.5f;
                }
            }
            else
            {
                if (npc.velocity.X > 3f)
                {
                    npc.velocity.X = 3f;
                }
                if (npc.velocity.X < -3f)
                {
                    npc.velocity.X = -3f;
                }
                if (npc.velocity.Y > 3f)
                {
                    npc.velocity.Y = 3f;
                }
                if (npc.velocity.Y < -3f)
                {
                    npc.velocity.Y = -3f;
                }
            }
        }
        else if (npc.type == 175)
        {
            if (npc.velocity.X > 4f)
            {
                npc.velocity.X = 4f;
            }
            if (npc.velocity.X < -4f)
            {
                npc.velocity.X = -4f;
            }
            if (npc.velocity.Y > 4f)
            {
                npc.velocity.Y = 4f;
            }
            if (npc.velocity.Y < -4f)
            {
                npc.velocity.Y = -4f;
            }
        }
        else
        {
            if (npc.velocity.X > 2f)
            {
                npc.velocity.X = 2f;
            }
            if (npc.velocity.X < -2f)
            {
                npc.velocity.X = -2f;
            }
            if (npc.velocity.Y > 2f)
            {
                npc.velocity.Y = 2f;
            }
            if (npc.velocity.Y < -2f)
            {
                npc.velocity.Y = -2f;
            }
        }
        if (npc.type == 259 || npc.type == 260)
        {
            npc.rotation = (float)Math.Atan2(num193, num192) + 1.57f;
        }
        else
        {
            if (num192 > 0f)
            {
                npc.spriteDirection = 1;
                npc.rotation = (float)Math.Atan2(num193, num192);
            }
            if (num192 < 0f)
            {
                npc.spriteDirection = -1;
                npc.rotation = (float)Math.Atan2(num193, num192) + 3.14f;
            }
        }
        if (npc.collideX)
        {
            npc.netUpdate = true;
            npc.velocity.X = npc.oldVelocity.X * -0.7f;
            if (npc.velocity.X > 0f && npc.velocity.X < 2f)
            {
                npc.velocity.X = 2f;
            }
            if (npc.velocity.X < 0f && npc.velocity.X > -2f)
            {
                npc.velocity.X = -2f;
            }
        }
        if (npc.collideY)
        {
            npc.netUpdate = true;
            npc.velocity.Y = npc.oldVelocity.Y * -0.7f;
            if (npc.velocity.Y > 0f && npc.velocity.Y < 2f)
            {
                npc.velocity.Y = 2f;
            }
            if (npc.velocity.Y < 0f && npc.velocity.Y > -2f)
            {
                npc.velocity.Y = -2f;
            }
        }
        if (Main.netMode == 1)
        {
            return;
        }
        if (npc.type == 101 && !Main.player[npc.target].DeadOrGhost)
        {
            if (npc.justHit)
            {
                npc.localAI[0] = 0f;
            }
            npc.localAI[0] += 1f;
            if (npc.localAI[0] >= 120f)
            {
                if (!Collision.SolidCollision(npc.position, npc.width, npc.height) && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    float num195 = 10f;
                    vector25 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                    num192 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector25.X + Main.rand.Next(-10, 11);
                    num193 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector25.Y + Main.rand.Next(-10, 11);
                    num194 = (float)Math.Sqrt(num192 * num192 + num193 * num193);
                    num194 = num195 / num194;
                    num192 *= num194;
                    num193 *= num194;
                    int attackDamage_ForProjectiles2 = npc.GetAttackDamage_ForProjectiles(22f, 17.6f);
                    int num196 = 96;
                    int num197 = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector25.X, vector25.Y, num192, num193, num196, attackDamage_ForProjectiles2, 0f, Main.myPlayer);
                    Main.projectile[num197].timeLeft = 300;
                    npc.localAI[0] = 0f;
                }
                else
                {
                    npc.localAI[0] = 100f;
                }
            }
        }
        if (npc.type != 260 || Main.player[npc.target].DeadOrGhost)
        {
            return;
        }
        if (npc.justHit)
        {
            npc.localAI[0] = 0f;
        }
        npc.localAI[0] += 1f;
        if (!(npc.localAI[0] >= 150f))
        {
            return;
        }
        if (!Collision.SolidCollision(npc.position, npc.width, npc.height) && Collision.CanHit(npc, Main.player[npc.target]))
        {
            float num198 = 14f;
            vector25 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            num192 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector25.X + Main.rand.Next(-10, 11);
            float num199 = Math.Abs(num192 * 0.1f);
            if (num193 > 0f)
            {
                num199 = 0f;
            }
            num193 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector25.Y + Main.rand.Next(-10, 11) - num199;
            num194 = (float)Math.Sqrt(num192 * num192 + num193 * num193);
            num194 = num198 / num194;
            num192 *= num194;
            num193 *= num194;
            int num200 = NPC.NewNPC(npc.GetSpawnSourceForProjectileNPC(), (int)npc.Center.X, (int)npc.Center.Y, 261);
            Main.npc[num200].velocity.X = num192;
            Main.npc[num200].velocity.Y = num193;
            Main.npc[num200].netUpdate = true;
            npc.localAI[0] = 0f;
        }
        else
        {
            npc.localAI[0] = 250f;
        }

    }
}
