namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_000(this NPC npc)
    {
        if (Main.netMode != 1)
        {
            for (int i = 0; i < 255; i++)
            {
                if (Main.player[i].active && Main.player[i].talkNPC == npc.whoAmI)
                {
                    if (npc.type == 589)
                    {
                        npc.AI_000_TransformBoundNPC(i, 588);
                    }
                    if (npc.type == 105)
                    {
                        npc.AI_000_TransformBoundNPC(i, 107);
                    }
                    if (npc.type == 106)
                    {
                        npc.AI_000_TransformBoundNPC(i, 108);
                    }
                    if (npc.type == 123)
                    {
                        npc.AI_000_TransformBoundNPC(i, 124);
                    }
                    if (npc.type == 354)
                    {
                        npc.AI_000_TransformBoundNPC(i, 353);
                    }
                    if (npc.type == 376)
                    {
                        npc.AI_000_TransformBoundNPC(i, 369);
                    }
                    if (npc.type == 579)
                    {
                        npc.AI_000_TransformBoundNPC(i, 550);
                    }
                }
            }
        }
        if (npc.type != 376 && npc.type != 579)
        {
            npc.TargetClosest();
            npc.spriteDirection = npc.direction;
        }
        if (npc.type == 376 || npc.type == 579)
        {
            if (npc.wet || Main.tile[(int)(npc.Center.X / 16f), (int)(npc.position.Y - 4f) / 16].liquid > 0)
            {
                npc.velocity.Y = -0.4f;
                int num = 1;
                if (npc.Center.X / 16f > Main.maxTilesX / 2)
                {
                    num = -1;
                }
                int num2 = 12;
                int num3 = (int)npc.Center.X / 16;
                int j = (int)npc.Center.Y / 16;
                bool flag = false;
                if (num > 0)
                {
                    for (int k = num3; k < num3 + num2; k++)
                    {
                        if (WorldGen.SolidTile(k, j))
                        {
                            flag = true;
                        }
                    }
                }
                else
                {
                    for (int l = num3; l > num3 - num2; l--)
                    {
                        if (WorldGen.SolidTile(l, j))
                        {
                            flag = true;
                        }
                    }
                }
                if (npc.type == 579)
                {
                    flag = true;
                }
                if (flag)
                {
                    npc.velocity.X *= 0.99f;
                    if (npc.velocity.X > -0.01 && npc.velocity.X < 0.01)
                    {
                        npc.velocity.X = 0f;
                    }
                    return;
                }
                npc.velocity.X += num * 0.01f;
                if (npc.velocity.X > 0.2f)
                {
                    npc.velocity.X *= 0.95f;
                }
                if (npc.velocity.X < -0.2f)
                {
                    npc.velocity.X *= 0.95f;
                }
            }
            else
            {
                npc.velocity.X *= 0.93f;
                if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
                {
                    npc.velocity.X = 0f;
                }
            }
        }
        else
        {
            npc.velocity.X *= 0.93f;
            if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
            {
                npc.velocity.X = 0f;
            }
        }
    }
}
