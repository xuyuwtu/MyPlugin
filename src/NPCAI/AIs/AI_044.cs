namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_044(this NPC npc)
    {
        npc.noGravity = true;
        if (npc.collideX)
        {
            if (npc.oldVelocity.X > 0f)
            {
                npc.direction = -1;
            }
            else
            {
                npc.direction = 1;
            }
            npc.velocity.X = npc.direction;
        }
        if (npc.collideY)
        {
            if (npc.oldVelocity.Y > 0f)
            {
                npc.directionY = -1;
            }
            else
            {
                npc.directionY = 1;
            }
            npc.velocity.Y = npc.directionY;
        }
        if (npc.type == 587)
        {
            npc.position += npc.netOffset;
            if (npc.alpha == 255)
            {
                npc.velocity.Y = -6f;
                npc.netUpdate = true;
                for (int num681 = 0; num681 < 15; num681++)
                {
                    Dust dust2 = Dust.NewDustDirect(npc.position, npc.width, npc.height, 5);
                    Dust dust = dust2;
                    dust.velocity *= 0.5f;
                    dust2.scale = 1f + Main.rand.NextFloat() * 0.5f;
                    dust2.fadeIn = 1.5f + Main.rand.NextFloat() * 0.5f;
                    dust = dust2;
                    dust.velocity += npc.velocity * 0.5f;
                }
            }
            npc.alpha -= 15;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }
            if (npc.alpha != 0)
            {
                for (int num682 = 0; num682 < 2; num682++)
                {
                    Dust dust3 = Dust.NewDustDirect(npc.position, npc.width, npc.height, 5);
                    Dust dust = dust3;
                    dust.velocity *= 1f;
                    dust3.scale = 1f + Main.rand.NextFloat() * 0.5f;
                    dust3.fadeIn = 1.5f + Main.rand.NextFloat() * 0.5f;
                    dust = dust3;
                    dust.velocity += npc.velocity * 0.3f;
                }
            }
            if (Main.rand.Next(3) == 0)
            {
                Dust dust4 = Dust.NewDustDirect(npc.position, npc.width, npc.height, 5);
                Dust dust = dust4;
                dust.velocity *= 0f;
                dust4.alpha = 120;
                dust4.scale = 0.7f + Main.rand.NextFloat() * 0.5f;
                dust = dust4;
                dust.velocity += npc.velocity * 0.3f;
            }
            npc.position -= npc.netOffset;
        }
        int num683 = npc.target;
        int num684 = npc.direction;
        if (npc.target == 255 || (npc.type != 587 && Main.player[npc.target].wet) || Main.player[npc.target].dead || Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
        {
            npc.ai[0] = 90f;
            npc.TargetClosest();
        }
        else if (npc.ai[0] > 0f)
        {
            npc.ai[0] -= 1f;
            npc.TargetClosest();
        }
        if (npc.netUpdate && num683 == npc.target && num684 == npc.direction)
        {
            npc.netUpdate = false;
        }
        float num685 = 0.05f;
        float num686 = 0.01f;
        float num687 = 3f;
        float num688 = 1f;
        float num689 = 30f;
        float num690 = 100f;
        float num691 = Math.Abs(npc.position.X + npc.width / 2 - (Main.player[npc.target].position.X + Main.player[npc.target].width / 2));
        float num692 = Main.player[npc.target].position.Y - npc.height / 2;
        if (npc.type == 509)
        {
            num685 = 0.08f;
            num686 = 0.03f;
            num687 = 4.5f;
            num688 = 2f;
            num689 = 40f;
            num690 = 150f;
            num692 = Main.player[npc.target].Center.Y - npc.height / 2;
            npc.rotation = npc.velocity.X * 0.1f;
            for (int num693 = 0; num693 < 200; num693++)
            {
                if (num693 != npc.whoAmI && Main.npc[num693].active && Main.npc[num693].type == npc.type && Math.Abs(npc.position.X - Main.npc[num693].position.X) + Math.Abs(npc.position.Y - Main.npc[num693].position.Y) < npc.width)
                {
                    if (npc.position.X < Main.npc[num693].position.X)
                    {
                        npc.velocity.X -= 0.05f;
                    }
                    else
                    {
                        npc.velocity.X += 0.05f;
                    }
                    if (npc.position.Y < Main.npc[num693].position.Y)
                    {
                        npc.velocity.Y -= 0.05f;
                    }
                    else
                    {
                        npc.velocity.Y += 0.05f;
                    }
                }
            }
        }
        else if (npc.type == 581)
        {
            num685 = 0.06f;
            num686 = 0.02f;
            num687 = 4f;
            num688 = 2f;
            num689 = 40f;
            num690 = 150f;
            num692 = Main.player[npc.target].Center.Y - npc.height / 2;
            npc.rotation = npc.velocity.X * 0.1f;
            for (int num694 = 0; num694 < 200; num694++)
            {
                if (num694 != npc.whoAmI && Main.npc[num694].active && Main.npc[num694].type == npc.type && Math.Abs(npc.position.X - Main.npc[num694].position.X) + Math.Abs(npc.position.Y - Main.npc[num694].position.Y) < npc.width)
                {
                    if (npc.position.X < Main.npc[num694].position.X)
                    {
                        npc.velocity.X -= 0.05f;
                    }
                    else
                    {
                        npc.velocity.X += 0.05f;
                    }
                    if (npc.position.Y < Main.npc[num694].position.Y)
                    {
                        npc.velocity.Y -= 0.05f;
                    }
                    else
                    {
                        npc.velocity.Y += 0.05f;
                    }
                }
            }
        }
        else if (npc.type == 587)
        {
            num685 = 0.13f;
            num686 = 0.09f;
            num687 = 6.5f;
            num688 = 3.5f;
            num689 = 0f;
            num690 = 250f;
            num692 = Main.player[npc.target].position.Y;
            if (Main.dayTime)
            {
                num692 = 0f;
                npc.direction *= -1;
            }
        }
        if (npc.ai[0] <= 0f)
        {
            num687 *= 0.8f;
            num685 *= 0.7f;
            num692 = npc.Center.Y + npc.directionY * 1000;
            if (npc.velocity.X < 0f)
            {
                npc.direction = -1;
            }
            else if (npc.velocity.X > 0f || npc.direction == 0)
            {
                npc.direction = 1;
            }
        }
        if (num691 > num689)
        {
            if (npc.direction == -1 && npc.velocity.X > 0f - num687)
            {
                npc.velocity.X -= num685;
                if (npc.velocity.X > num687)
                {
                    npc.velocity.X -= num685;
                }
                else if (npc.velocity.X > 0f)
                {
                    npc.velocity.X -= num685 / 2f;
                }
                if (npc.velocity.X < 0f - num687)
                {
                    npc.velocity.X = 0f - num687;
                }
            }
            else if (npc.direction == 1 && npc.velocity.X < num687)
            {
                npc.velocity.X += num685;
                if (npc.velocity.X < 0f - num687)
                {
                    npc.velocity.X += num685;
                }
                else if (npc.velocity.X < 0f)
                {
                    npc.velocity.X += num685 / 2f;
                }
                if (npc.velocity.X > num687)
                {
                    npc.velocity.X = num687;
                }
            }
        }
        if (num691 > num690)
        {
            num692 -= num690 / 2f;
        }
        if (npc.position.Y < num692)
        {
            npc.velocity.Y += num686;
            if (npc.velocity.Y < 0f)
            {
                npc.velocity.Y += num686;
            }
        }
        else
        {
            npc.velocity.Y -= num686;
            if (npc.velocity.Y > 0f)
            {
                npc.velocity.Y -= num686;
            }
        }
        if (npc.velocity.Y < 0f - num688)
        {
            npc.velocity.Y = 0f - num688;
        }
        if (npc.velocity.Y > num688)
        {
            npc.velocity.Y = num688;
        }
        if (npc.type != 587 && npc.wet)
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
        }
    }
}
