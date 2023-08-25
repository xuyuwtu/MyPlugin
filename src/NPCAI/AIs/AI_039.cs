namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_039(this NPC npc)
    {
        if (npc.target < 0 || Main.player[npc.target].dead || npc.direction == 0)
        {
            npc.TargetClosest();
        }
        bool flag28 = true;
        int num574 = 0;
        if (npc.velocity.X < 0f)
        {
            num574 = -1;
        }
        if (npc.velocity.X > 0f)
        {
            num574 = 1;
        }
        Vector2 vector72 = npc.position;
        vector72.X += npc.velocity.X;
        int num575 = (int)((vector72.X + npc.width / 2 + (npc.width / 2 + 1) * num574) / 16f);
        int num576 = (int)((vector72.Y + npc.height - 1f) / 16f);
        if (num575 * 16 < vector72.X + npc.width && num575 * 16 + 16 > vector72.X)
        {
            ITile tileSafely = Framing.GetTileSafely(num575, num576 - 4);
            ITile tileSafely2 = Framing.GetTileSafely(num575 - num574, num576 - 3);
            ITile tileSafely3 = Framing.GetTileSafely(num575, num576 - 3);
            ITile tileSafely4 = Framing.GetTileSafely(num575, num576 - 2);
            ITile tileSafely5 = Framing.GetTileSafely(num575, num576 - 1);
            ITile tileSafely6 = Framing.GetTileSafely(num575, num576);
            if (((tileSafely6.nactive() && !tileSafely6.topSlope() && !tileSafely5.topSlope() && ((Main.tileSolid[tileSafely6.type] && !Main.tileSolidTop[tileSafely6.type]) || (flag28 && Main.tileSolidTop[tileSafely6.type] && (!Main.tileSolid[tileSafely5.type] || !tileSafely5.nactive()) && tileSafely6.type != 16 && tileSafely6.type != 18 && tileSafely6.type != 134))) || (tileSafely5.halfBrick() && tileSafely5.nactive())) && (!tileSafely5.nactive() || !Main.tileSolid[tileSafely5.type] || Main.tileSolidTop[tileSafely5.type] || (tileSafely5.halfBrick() && (!tileSafely.nactive() || !Main.tileSolid[tileSafely.type] || Main.tileSolidTop[tileSafely.type]))) && (!tileSafely4.nactive() || !Main.tileSolid[tileSafely4.type] || Main.tileSolidTop[tileSafely4.type]) && (!tileSafely3.nactive() || !Main.tileSolid[tileSafely3.type] || Main.tileSolidTop[tileSafely3.type]) && (!tileSafely2.nactive() || !Main.tileSolid[tileSafely2.type] || Main.tileSolidTop[tileSafely2.type]))
            {
                float num577 = num576 * 16;
                if (tileSafely6.halfBrick())
                {
                    num577 += 8f;
                }
                if (tileSafely5.halfBrick())
                {
                    num577 -= 8f;
                }
                if (num577 < vector72.Y + npc.height)
                {
                    float num578 = vector72.Y + npc.height - num577;
                    if ((double)num578 <= 16.1)
                    {
                        npc.gfxOffY += npc.position.Y + npc.height - num577;
                        npc.position.Y = num577 - npc.height;
                        if (num578 < 9f)
                        {
                            npc.stepSpeed = 0.75f;
                        }
                        else
                        {
                            npc.stepSpeed = 1.5f;
                        }
                    }
                }
            }
        }
        if (npc.justHit && npc.type != 417)
        {
            npc.ai[0] = 0f;
            npc.ai[1] = 0f;
            npc.TargetClosest();
        }
        if (npc.type == 154)
        {
            npc.position += npc.netOffset;
            if (Main.rand.Next(10) == 0)
            {
                int num579 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 67, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 90, default, 1.5f);
                Main.dust[num579].noGravity = true;
                Dust dust = Main.dust[num579];
                dust.velocity *= 0.2f;
            }
            npc.position -= npc.netOffset;
        }
        if (npc.ai[0] == 0f)
        {
            if (npc.velocity.X < 0f)
            {
                npc.direction = -1;
            }
            else if (npc.velocity.X > 0f)
            {
                npc.direction = 1;
            }
            npc.spriteDirection = npc.direction;
            Vector2 vector73 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num580 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector73.X;
            float num581 = Main.player[npc.target].position.Y - vector73.Y;
            float num582 = (float)Math.Sqrt(num580 * num580 + num581 * num581);
            bool flag29 = Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height);
            if (npc.type >= 496 && npc.type <= 497)
            {
                if (num582 > 200f && flag29)
                {
                    npc.ai[1] += 2f;
                }
                if (num582 > 600f && (flag29 || npc.position.Y + npc.height > Main.player[npc.target].position.Y - 200f))
                {
                    npc.ai[1] += 4f;
                }
            }
            else
            {
                if (num582 > 200f && flag29)
                {
                    npc.ai[1] += 4f;
                }
                if (num582 > 600f && (flag29 || npc.position.Y + npc.height > Main.player[npc.target].position.Y - 200f))
                {
                    npc.ai[1] += 10f;
                }
                if (npc.wet)
                {
                    npc.ai[1] = 1000f;
                }
            }
            npc.defense = npc.defDefense;
            npc.damage = npc.defDamage;
            if (npc.type >= 496 && npc.type <= 497)
            {
                npc.knockBackResist = 0.75f * Main.GameModeInfo.KnockbackToEnemiesMultiplier;
            }
            else
            {
                npc.knockBackResist = 0.3f * Main.GameModeInfo.KnockbackToEnemiesMultiplier;
            }
            npc.ai[1] += 1f;
            if (npc.ai[1] >= 400f)
            {
                npc.ai[1] = 0f;
                npc.ai[0] = 1f;
            }
            if (!npc.justHit && npc.velocity.X != npc.oldVelocity.X)
            {
                npc.direction *= -1;
            }
            if (npc.velocity.Y == 0f && Main.player[npc.target].position.Y < npc.position.Y + npc.height)
            {
                int num583;
                int num584;
                if (npc.direction > 0)
                {
                    num583 = (int)((npc.position.X + npc.width * 0.5) / 16.0);
                    num584 = num583 + 3;
                }
                else
                {
                    num584 = (int)((npc.position.X + npc.width * 0.5) / 16.0);
                    num583 = num584 - 3;
                }
                int num585 = (int)((npc.position.Y + npc.height + 2f) / 16f) - 1;
                int num586 = num585 + 4;
                bool flag30 = false;
                for (int num587 = num583; num587 <= num584; num587++)
                {
                    for (int num588 = num585; num588 <= num586; num588++)
                    {
                        if (Main.tile[num587, num588] != null && Main.tile[num587, num588].nactive() && Main.tileSolid[Main.tile[num587, num588].type])
                        {
                            flag30 = true;
                        }
                    }
                }
                if (!flag30)
                {
                    npc.direction *= -1;
                    npc.velocity.X = 0.1f * npc.direction;
                }
            }
            if (npc.type >= 496 && npc.type <= 497)
            {
                float num589 = 0.5f;
                if (npc.velocity.X < 0f - num589 || npc.velocity.X > num589)
                {
                    if (npc.velocity.Y == 0f)
                    {
                        npc.velocity *= 0.8f;
                    }
                }
                else if (npc.velocity.X < num589 && npc.direction == 1)
                {
                    npc.velocity.X += 0.07f;
                    if (npc.velocity.X > num589)
                    {
                        npc.velocity.X = num589;
                    }
                }
                else if (npc.velocity.X > 0f - num589 && npc.direction == -1)
                {
                    npc.velocity.X -= 0.07f;
                    if (npc.velocity.X < 0f - num589)
                    {
                        npc.velocity.X = 0f - num589;
                    }
                }
                return;
            }
            float num590 = 1f;
            if (num582 < 400f)
            {
                if (npc.velocity.X < 0f - num590 || npc.velocity.X > num590)
                {
                    if (npc.velocity.Y == 0f)
                    {
                        npc.velocity *= 0.8f;
                    }
                }
                else if (npc.velocity.X < num590 && npc.direction == 1)
                {
                    npc.velocity.X += 0.07f;
                    if (npc.velocity.X > num590)
                    {
                        npc.velocity.X = num590;
                    }
                }
                else if (npc.velocity.X > 0f - num590 && npc.direction == -1)
                {
                    npc.velocity.X -= 0.07f;
                    if (npc.velocity.X < 0f - num590)
                    {
                        npc.velocity.X = 0f - num590;
                    }
                }
            }
            else if (npc.velocity.X < -1.5f || npc.velocity.X > 1.5f)
            {
                if (npc.velocity.Y == 0f)
                {
                    npc.velocity *= 0.8f;
                }
            }
            else if (npc.velocity.X < 1.5f && npc.direction == 1)
            {
                npc.velocity.X += 0.07f;
                if (npc.velocity.X > 1.5f)
                {
                    npc.velocity.X = 1.5f;
                }
            }
            else if (npc.velocity.X > -1.5f && npc.direction == -1)
            {
                npc.velocity.X -= 0.07f;
                if (npc.velocity.X < -1.5f)
                {
                    npc.velocity.X = -1.5f;
                }
            }
        }
        else if (npc.ai[0] == 1f)
        {
            npc.velocity.X *= 0.5f;
            if (npc.type >= 496 && npc.type <= 497)
            {
                npc.ai[1] += 0.5f;
            }
            else
            {
                npc.ai[1] += 1f;
            }
            if (npc.ai[1] >= 30f)
            {
                npc.netUpdate = true;
                npc.TargetClosest();
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[0] = 3f;
                if (npc.type == 417)
                {
                    npc.position.Y += npc.height;
                    npc.height = 32;
                    npc.position.Y -= npc.height;
                    npc.ai[0] = 6f;
                    npc.ai[2] = Main.rand.Next(2, 5);
                }
            }
        }
        else if (npc.ai[0] == 3f)
        {
            if (npc.type == 154 && Main.rand.Next(3) < 2)
            {
                npc.position += npc.netOffset;
                int num591 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 67, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 90, default, 1.5f);
                Main.dust[num591].noGravity = true;
                Dust dust = Main.dust[num591];
                dust.velocity *= 0.2f;
                npc.position -= npc.netOffset;
            }
            float num592 = 2f;
            if (npc.type >= 496 && npc.type <= 497)
            {
                num592 = 1.5f;
            }
            npc.damage = npc.GetAttackDamage_LerpBetweenFinalValues(npc.defDamage * num592, npc.defDamage * num592 * 0.9f);
            npc.defense = npc.defDefense * 2;
            npc.ai[1] += 1f;
            if (npc.ai[1] == 1f)
            {
                npc.netUpdate = true;
                npc.TargetClosest();
                npc.ai[2] += 0.3f;
                npc.rotation += npc.ai[2] * npc.direction;
                npc.ai[1] += 1f;
                bool flag31 = Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height);
                float num593 = 10f;
                if (!flag31)
                {
                    num593 = 6f;
                }
                if (npc.type >= 496 && npc.type <= 497)
                {
                    num593 *= 0.75f;
                }
                Vector2 vector74 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float num594 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector74.X;
                float num595 = Math.Abs(num594) * 0.2f;
                if (npc.directionY > 0)
                {
                    num595 = 0f;
                }
                float num596 = Main.player[npc.target].position.Y - vector74.Y - num595;
                float num597 = (float)Math.Sqrt(num594 * num594 + num596 * num596);
                npc.netUpdate = true;
                num597 = num593 / num597;
                num594 *= num597;
                num596 *= num597;
                if (!flag31)
                {
                    num596 = -10f;
                }
                npc.velocity.X = num594;
                npc.velocity.Y = num596;
                npc.ai[3] = npc.velocity.X;
            }
            else
            {
                if (npc.position.X + npc.width > Main.player[npc.target].position.X && npc.position.X < Main.player[npc.target].position.X + Main.player[npc.target].width && npc.position.Y < Main.player[npc.target].position.Y + Main.player[npc.target].height)
                {
                    npc.velocity.X *= 0.8f;
                    npc.ai[3] = 0f;
                    if (npc.velocity.Y < 0f)
                    {
                        npc.velocity.Y += 0.2f;
                    }
                }
                if (npc.ai[3] != 0f)
                {
                    npc.velocity.X = npc.ai[3];
                    npc.velocity.Y -= 0.22f;
                }
                if (npc.ai[1] >= 90f)
                {
                    npc.noGravity = false;
                    npc.ai[1] = 0f;
                    npc.ai[0] = 4f;
                }
            }
            if (npc.wet && npc.directionY < 0)
            {
                npc.velocity.Y -= 0.3f;
            }
            npc.rotation += npc.ai[2] * npc.direction;
        }
        else if (npc.ai[0] == 4f)
        {
            if (npc.wet && npc.directionY < 0)
            {
                npc.velocity.Y -= 0.3f;
            }
            npc.velocity.X *= 0.96f;
            if (npc.ai[2] > 0f)
            {
                npc.ai[2] -= 0.01f;
                npc.rotation += npc.ai[2] * npc.direction;
            }
            else if (npc.velocity.Y >= 0f)
            {
                npc.rotation = 0f;
            }
            if (npc.ai[2] <= 0f && (npc.velocity.Y == 0f || npc.wet))
            {
                npc.netUpdate = true;
                npc.rotation = 0f;
                npc.ai[2] = 0f;
                npc.ai[1] = 0f;
                npc.ai[0] = 5f;
            }
        }
        else if (npc.ai[0] == 6f)
        {
            npc.damage = npc.GetAttackDamage_LerpBetweenFinalValues(npc.defDamage * 1.8f, npc.defDamage * 1.4f);
            npc.defense = npc.defDefense * 2;
            npc.knockBackResist = 0f;
            if (Main.rand.Next(3) < 2)
            {
                npc.position += npc.netOffset;
                int num598 = Dust.NewDust(npc.Center - new Vector2(30f), 60, 60, 6, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 90, default, 1.5f);
                Main.dust[num598].noGravity = true;
                Dust dust = Main.dust[num598];
                dust.velocity *= 0.2f;
                Main.dust[num598].fadeIn = 1f;
                npc.position -= npc.netOffset;
            }
            npc.ai[1] += 1f;
            if (npc.ai[3] > 0f)
            {
                npc.position += npc.netOffset;
                if (npc.ai[3] == 1f)
                {
                    Vector2 vector75 = npc.Center - new Vector2(50f);
                    for (int num599 = 0; num599 < 32; num599++)
                    {
                        int num600 = Dust.NewDust(vector75, 100, 100, 6, 0f, 0f, 100, default, 2.5f);
                        Main.dust[num600].noGravity = true;
                        Dust dust = Main.dust[num600];
                        dust.velocity *= 3f;
                        num600 = Dust.NewDust(vector75, 100, 100, 6, 0f, 0f, 100, default, 1.5f);
                        dust = Main.dust[num600];
                        dust.velocity *= 2f;
                        Main.dust[num600].noGravity = true;
                    }
                    for (int num601 = 0; num601 < 4; num601++)
                    {
                        int num602 = Gore.NewGore(vector75 + new Vector2(50 * Main.rand.Next(100) / 100f, 50 * Main.rand.Next(100) / 100f) - Vector2.One * 10f, default, Main.rand.Next(61, 64));
                        Gore gore = Main.gore[num602];
                        gore.velocity *= 0.3f;
                        Main.gore[num602].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                        Main.gore[num602].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
                    }
                }
                for (int num603 = 0; num603 < 5; num603++)
                {
                    int num604 = Dust.NewDust(npc.position, npc.width, npc.height, 31, 0f, 0f, 100, default, 1.5f);
                    Main.dust[num604].velocity = Main.dust[num604].velocity * Main.rand.NextFloat();
                }
                npc.ai[3]++;
                if (npc.ai[3] >= 10f)
                {
                    npc.ai[3] = 0f;
                }
                npc.position -= npc.netOffset;
            }
            if (npc.ai[1] == 1f)
            {
                npc.netUpdate = true;
                npc.TargetClosest();
                bool flag32 = Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height);
                float num605 = 16f;
                if (!flag32)
                {
                    num605 = 10f;
                }
                Vector2 vector76 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float num606 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector76.X;
                float num607 = Math.Abs(num606) * 0.2f;
                if (npc.directionY > 0)
                {
                    num607 = 0f;
                }
                float num608 = Main.player[npc.target].position.Y - vector76.Y - num607;
                float num609 = (float)Math.Sqrt(num606 * num606 + num608 * num608);
                npc.netUpdate = true;
                num609 = num605 / num609;
                num606 *= num609;
                num608 *= num609;
                if (!flag32)
                {
                    num608 = -12f;
                }
                npc.velocity.X = num606;
                npc.velocity.Y = num608;
            }
            else
            {
                if (npc.position.X + npc.width > Main.player[npc.target].position.X && npc.position.X < Main.player[npc.target].position.X + Main.player[npc.target].width && npc.position.Y < Main.player[npc.target].position.Y + Main.player[npc.target].height)
                {
                    npc.velocity.X *= 0.9f;
                    if (npc.velocity.Y < 0f)
                    {
                        npc.velocity.Y += 0.2f;
                    }
                }
                if (npc.ai[2] == 0f || npc.ai[1] >= 1200f)
                {
                    npc.ai[1] = 0f;
                    npc.ai[0] = 5f;
                }
            }
            if (npc.wet && npc.directionY < 0)
            {
                npc.velocity.Y -= 0.3f;
            }
            npc.rotation += MathHelper.Clamp(npc.velocity.X / 10f * npc.direction, -(float)Math.PI / 10f, (float)Math.PI / 10f);
        }
        else if (npc.ai[0] == 5f)
        {
            if (npc.type == 417)
            {
                npc.position.Y += npc.height;
                npc.height = 52;
                npc.position.Y -= npc.height;
            }
            npc.rotation = 0f;
            npc.velocity.X = 0f;
            if (npc.type >= 496 && npc.type <= 497)
            {
                npc.ai[1] += 0.5f;
            }
            else
            {
                npc.ai[1] += 1f;
            }
            if (npc.ai[1] >= 30f)
            {
                npc.TargetClosest();
                npc.netUpdate = true;
                npc.ai[1] = 0f;
                npc.ai[0] = 0f;
            }
            if (npc.wet)
            {
                npc.ai[0] = 3f;
                npc.ai[1] = 0f;
            }
        }
    }
}
