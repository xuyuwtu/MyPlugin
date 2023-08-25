namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_029(this NPC npc)
    {
        if (npc.justHit)
        {
            npc.ai[1] = 10f;
        }
        if (Main.wofNPCIndex < 0)
        {
            npc.active = false;
            return;
        }
        npc.TargetClosest();
        float num390 = 0.1f;
        float num391 = 300f;
        npc.damage = npc.defDamage;
        int num392 = 0;
        if (Main.npc[Main.wofNPCIndex].life < Main.npc[Main.wofNPCIndex].lifeMax * 0.5)
        {
            num392 = 60;
            npc.defense = 30;
            if (!Main.expertMode)
            {
                num391 = 700f;
            }
            else
            {
                num390 += 0.066f;
            }
        }
        else if (Main.npc[Main.wofNPCIndex].life < Main.npc[Main.wofNPCIndex].lifeMax * 0.75)
        {
            num392 = 45;
            npc.defense = 20;
            if (!Main.expertMode)
            {
                num391 = 500f;
            }
            else
            {
                num390 += 0.033f;
            }
        }
        if (num392 > 0)
        {
            npc.damage = npc.GetAttackDamage_ScaledByStrength(num392);
        }
        if (Main.expertMode)
        {
            npc.defense = npc.defDefense;
            if (npc.whoAmI % 4 == 0)
            {
                num391 *= 1.75f;
            }
            if (npc.whoAmI % 4 == 1)
            {
                num391 *= 1.5f;
            }
            if (npc.whoAmI % 4 == 2)
            {
                num391 *= 1.25f;
            }
            if (npc.whoAmI % 3 == 0)
            {
                num391 *= 1.5f;
            }
            if (npc.whoAmI % 3 == 1)
            {
                num391 *= 1.25f;
            }
            num391 *= 0.75f;
        }
        float num393 = Main.npc[Main.wofNPCIndex].position.X + Main.npc[Main.wofNPCIndex].width / 2;
        _ = Main.npc[Main.wofNPCIndex].position.Y;
        float num394 = Main.wofDrawAreaBottom - Main.wofDrawAreaTop;
        float y5 = Main.wofDrawAreaTop + num394 * npc.ai[0];
        npc.ai[2] += 1f;
        if (npc.ai[2] > 100f)
        {
            num391 = (int)(num391 * 1.3f);
            if (npc.ai[2] > 200f)
            {
                npc.ai[2] = 0f;
            }
        }
        Vector2 vector40 = new(num393, y5);
        float num395 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - npc.width / 2 - vector40.X;
        float num396 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - npc.height / 2 - vector40.Y;
        float num397 = (float)Math.Sqrt(num395 * num395 + num396 * num396);
        if (npc.ai[1] == 0f)
        {
            if (num397 > num391)
            {
                num397 = num391 / num397;
                num395 *= num397;
                num396 *= num397;
            }
            if (npc.position.X < num393 + num395)
            {
                npc.velocity.X += num390;
                if (npc.velocity.X < 0f && num395 > 0f)
                {
                    npc.velocity.X += num390 * 2.5f;
                }
            }
            else if (npc.position.X > num393 + num395)
            {
                npc.velocity.X -= num390;
                if (npc.velocity.X > 0f && num395 < 0f)
                {
                    npc.velocity.X -= num390 * 2.5f;
                }
            }
            if (npc.position.Y < y5 + num396)
            {
                npc.velocity.Y += num390;
                if (npc.velocity.Y < 0f && num396 > 0f)
                {
                    npc.velocity.Y += num390 * 2.5f;
                }
            }
            else if (npc.position.Y > y5 + num396)
            {
                npc.velocity.Y -= num390;
                if (npc.velocity.Y > 0f && num396 < 0f)
                {
                    npc.velocity.Y -= num390 * 2.5f;
                }
            }
            float num398 = 4f;
            if (Main.expertMode && Main.wofNPCIndex >= 0)
            {
                float num399 = 1.5f;
                float num400 = Main.npc[Main.wofNPCIndex].life / Main.npc[Main.wofNPCIndex].lifeMax;
                if ((double)num400 < 0.75)
                {
                    num399 += 0.7f;
                }
                if ((double)num400 < 0.5)
                {
                    num399 += 0.7f;
                }
                if ((double)num400 < 0.25)
                {
                    num399 += 0.9f;
                }
                if ((double)num400 < 0.1)
                {
                    num399 += 0.9f;
                }
                num399 *= 1.25f;
                num399 += 0.3f;
                num398 += num399 * 0.35f;
                if (npc.Center.X < Main.npc[Main.wofNPCIndex].Center.X && Main.npc[Main.wofNPCIndex].velocity.X > 0f)
                {
                    num398 += 6f;
                }
                if (npc.Center.X > Main.npc[Main.wofNPCIndex].Center.X && Main.npc[Main.wofNPCIndex].velocity.X < 0f)
                {
                    num398 += 6f;
                }
            }
            if (npc.velocity.X > num398)
            {
                npc.velocity.X = num398;
            }
            if (npc.velocity.X < 0f - num398)
            {
                npc.velocity.X = 0f - num398;
            }
            if (npc.velocity.Y > num398)
            {
                npc.velocity.Y = num398;
            }
            if (npc.velocity.Y < 0f - num398)
            {
                npc.velocity.Y = 0f - num398;
            }
        }
        else if (npc.ai[1] > 0f)
        {
            npc.ai[1] -= 1f;
        }
        else
        {
            npc.ai[1] = 0f;
        }
        if (num395 > 0f)
        {
            npc.spriteDirection = 1;
            npc.rotation = (float)Math.Atan2(num396, num395);
        }
        if (num395 < 0f)
        {
            npc.spriteDirection = -1;
            npc.rotation = (float)Math.Atan2(num396, num395) + 3.14f;
        }
        Lighting.AddLight((int)(npc.position.X + npc.width / 2) / 16, (int)(npc.position.Y + npc.height / 2) / 16, 0.3f, 0.2f, 0.1f);
    }
}
