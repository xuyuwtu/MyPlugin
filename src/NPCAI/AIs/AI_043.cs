namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_043(this NPC npc)
    {
        if (Main.expertMode)
        {
            int num632 = (int)(20f * (1f - npc.life / (float)npc.lifeMax));
            npc.defense = npc.defDefense + num632;
        }
        if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
        {
            npc.TargetClosest();
        }
        bool dead4 = Main.player[npc.target].dead;
        float num633 = 0f;
        if ((double)(npc.position.Y / 16f) < Main.worldSurface)
        {
            num633 += 1f;
        }
        if (!Main.player[npc.target].ZoneJungle)
        {
            num633 += 1f;
        }
        if (Main.getGoodWorld)
        {
            num633 += 0.5f;
        }
        float num634 = Vector2.Distance(npc.Center, Main.player[npc.target].Center);
        if (npc.ai[0] != 5f)
        {
            if (npc.timeLeft < 60)
            {
                npc.timeLeft = 60;
            }
            if (num634 > 3000f)
            {
                npc.ai[0] = 4f;
                npc.netUpdate = true;
            }
        }
        if (dead4)
        {
            npc.ai[0] = 5f;
            npc.netUpdate = true;
        }
        if (npc.ai[0] == 5f)
        {
            npc.velocity.Y *= 0.98f;
            if (npc.velocity.X < 0f)
            {
                npc.direction = -1;
            }
            else
            {
                npc.direction = 1;
            }
            npc.spriteDirection = npc.direction;
            if (npc.position.X < Main.maxTilesX * 8)
            {
                if (npc.velocity.X > 0f)
                {
                    npc.velocity.X *= 0.98f;
                }
                else
                {
                    npc.localAI[0] = 1f;
                }
                npc.velocity.X -= 0.08f;
            }
            else
            {
                if (npc.velocity.X < 0f)
                {
                    npc.velocity.X *= 0.98f;
                }
                else
                {
                    npc.localAI[0] = 1f;
                }
                npc.velocity.X += 0.08f;
            }
            npc.EncourageDespawn(10);
        }
        else if (npc.ai[0] == -1f)
        {
            if (Main.netMode == 1)
            {
                return;
            }
            float num635 = npc.ai[1];
            int num636;
            do
            {
                num636 = Main.rand.Next(3);
                switch (num636)
                {
                    case 1:
                        num636 = 2;
                        break;
                    case 2:
                        num636 = 3;
                        break;
                }
            }
            while (num636 == num635);
            npc.ai[0] = num636;
            npc.ai[1] = 0f;
            npc.ai[2] = 0f;
            npc.netUpdate = true;
        }
        else if (npc.ai[0] == 0f)
        {
            int num637 = 2;
            if (Main.expertMode)
            {
                if (npc.life < npc.lifeMax / 2)
                {
                    num637++;
                }
                if (npc.life < npc.lifeMax / 3)
                {
                    num637++;
                }
                if (npc.life < npc.lifeMax / 5)
                {
                    num637++;
                }
            }
            num637 += (int)(1f * num633);
            if (npc.ai[1] > 2 * num637 && npc.ai[1] % 2f == 0f)
            {
                npc.ai[0] = -1f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.netUpdate = true;
                return;
            }
            if (npc.ai[1] % 2f == 0f)
            {
                npc.TargetClosest();
                float num638 = 20f;
                num638 += 20f * num633;
                if (Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y) < num638)
                {
                    npc.localAI[0] = 1f;
                    npc.ai[1] += 1f;
                    npc.ai[2] = 0f;
                    npc.netUpdate = true;
                    float num639 = 12f;
                    if (Main.expertMode)
                    {
                        num639 = 16f;
                        if (npc.life < npc.lifeMax * 0.75)
                        {
                            num639 += 2f;
                        }
                        if (npc.life < npc.lifeMax * 0.5)
                        {
                            num639 += 2f;
                        }
                        if (npc.life < npc.lifeMax * 0.25)
                        {
                            num639 += 2f;
                        }
                        if (npc.life < npc.lifeMax * 0.1)
                        {
                            num639 += 2f;
                        }
                    }
                    num639 += 7f * num633;
                    Vector2 vector82 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                    float num640 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector82.X;
                    float num641 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector82.Y;
                    float num642 = (float)Math.Sqrt(num640 * num640 + num641 * num641);
                    num642 = num639 / num642;
                    npc.velocity.X = num640 * num642;
                    npc.velocity.Y = num641 * num642;
                    npc.spriteDirection = npc.direction;
                    SoundEngine.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 125);
                    return;
                }
                npc.localAI[0] = 0f;
                float num643 = 12f;
                float num644 = 0.15f;
                if (Main.expertMode)
                {
                    if (npc.life < npc.lifeMax * 0.75)
                    {
                        num643 += 1f;
                        num644 += 0.05f;
                    }
                    if (npc.life < npc.lifeMax * 0.5)
                    {
                        num643 += 1f;
                        num644 += 0.05f;
                    }
                    if (npc.life < npc.lifeMax * 0.25)
                    {
                        num643 += 2f;
                        num644 += 0.05f;
                    }
                    if (npc.life < npc.lifeMax * 0.1)
                    {
                        num643 += 2f;
                        num644 += 0.1f;
                    }
                }
                num643 += 3f * num633;
                num644 += 0.5f * num633;
                if (npc.position.Y + npc.height / 2 < Main.player[npc.target].position.Y + Main.player[npc.target].height / 2)
                {
                    npc.velocity.Y += num644;
                }
                else
                {
                    npc.velocity.Y -= num644;
                }
                if (npc.velocity.Y < 0f - num643)
                {
                    npc.velocity.Y = 0f - num643;
                }
                if (npc.velocity.Y > num643)
                {
                    npc.velocity.Y = num643;
                }
                if (Math.Abs(npc.position.X + npc.width / 2 - (Main.player[npc.target].position.X + Main.player[npc.target].width / 2)) > 600f)
                {
                    npc.velocity.X += 0.15f * npc.direction;
                }
                else if (Math.Abs(npc.position.X + npc.width / 2 - (Main.player[npc.target].position.X + Main.player[npc.target].width / 2)) < 300f)
                {
                    npc.velocity.X -= 0.15f * npc.direction;
                }
                else
                {
                    npc.velocity.X *= 0.8f;
                }
                if (npc.velocity.X < -16f)
                {
                    npc.velocity.X = -16f;
                }
                if (npc.velocity.X > 16f)
                {
                    npc.velocity.X = 16f;
                }
                npc.spriteDirection = npc.direction;
                return;
            }
            if (npc.velocity.X < 0f)
            {
                npc.direction = -1;
            }
            else
            {
                npc.direction = 1;
            }
            npc.spriteDirection = npc.direction;
            int num645 = 600;
            if (Main.expertMode)
            {
                if (npc.life < npc.lifeMax * 0.1)
                {
                    num645 = 300;
                }
                else if (npc.life < npc.lifeMax * 0.25)
                {
                    num645 = 450;
                }
                else if (npc.life < npc.lifeMax * 0.5)
                {
                    num645 = 500;
                }
                else if (npc.life < npc.lifeMax * 0.75)
                {
                    num645 = 550;
                }
            }
            int num646 = 1;
            if (npc.position.X + npc.width / 2 < Main.player[npc.target].position.X + Main.player[npc.target].width / 2)
            {
                num646 = -1;
            }
            num645 -= (int)(100f * num633);
            bool flag34 = false;
            if (npc.direction == num646 && Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) > num645)
            {
                npc.ai[2] = 1f;
                flag34 = true;
            }
            if (Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y) > num645 * 1.5f)
            {
                npc.ai[2] = 1f;
                flag34 = true;
            }
            if (num633 > 0f && flag34)
            {
                npc.velocity *= 0.5f;
            }
            if (npc.ai[2] == 1f)
            {
                npc.TargetClosest();
                npc.spriteDirection = npc.direction;
                npc.localAI[0] = 0f;
                npc.velocity *= 0.9f;
                float num647 = 0.1f;
                if (Main.expertMode)
                {
                    if (npc.life < npc.lifeMax / 2)
                    {
                        npc.velocity *= 0.9f;
                        num647 += 0.05f;
                    }
                    if (npc.life < npc.lifeMax / 3)
                    {
                        npc.velocity *= 0.9f;
                        num647 += 0.05f;
                    }
                    if (npc.life < npc.lifeMax / 5)
                    {
                        npc.velocity *= 0.9f;
                        num647 += 0.05f;
                    }
                }
                if (num633 > 0f)
                {
                    npc.velocity *= 0.7f;
                }
                if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num647)
                {
                    npc.ai[2] = 0f;
                    npc.ai[1] += 1f;
                    npc.netUpdate = true;
                }
            }
            else
            {
                npc.localAI[0] = 1f;
            }
        }
        else if (npc.ai[0] == 2f)
        {
            npc.TargetClosest();
            npc.spriteDirection = npc.direction;
            float num648 = 12f;
            float num649 = 0.07f;
            if (Main.expertMode)
            {
                num649 = 0.1f;
            }
            Vector2 vector83 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num650 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector83.X;
            float num651 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - 200f - vector83.Y;
            float num652 = (float)Math.Sqrt(num650 * num650 + num651 * num651);
            if (num652 < 200f)
            {
                npc.ai[0] = 1f;
                npc.ai[1] = 0f;
                npc.netUpdate = true;
                return;
            }
            _ = num648 / num652;
            if (npc.velocity.X < num650)
            {
                npc.velocity.X += num649;
                if (npc.velocity.X < 0f && num650 > 0f)
                {
                    npc.velocity.X += num649;
                }
            }
            else if (npc.velocity.X > num650)
            {
                npc.velocity.X -= num649;
                if (npc.velocity.X > 0f && num650 < 0f)
                {
                    npc.velocity.X -= num649;
                }
            }
            if (npc.velocity.Y < num651)
            {
                npc.velocity.Y += num649;
                if (npc.velocity.Y < 0f && num651 > 0f)
                {
                    npc.velocity.Y += num649;
                }
            }
            else if (npc.velocity.Y > num651)
            {
                npc.velocity.Y -= num649;
                if (npc.velocity.Y > 0f && num651 < 0f)
                {
                    npc.velocity.Y -= num649;
                }
            }
        }
        else if (npc.ai[0] == 1f)
        {
            npc.localAI[0] = 0f;
            npc.TargetClosest();
            Vector2 vector84 = new(npc.position.X + npc.width / 2 + Main.rand.Next(20) * npc.direction, npc.position.Y + npc.height * 0.8f);
            Vector2 vector85 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num653 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector85.X;
            float num654 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector85.Y;
            float num655 = (float)Math.Sqrt(num653 * num653 + num654 * num654);
            npc.ai[1] += 1f;
            if (Main.expertMode)
            {
                int num656 = 0;
                for (int num657 = 0; num657 < 255; num657++)
                {
                    if (Main.player[num657].active && !Main.player[num657].dead && (npc.Center - Main.player[num657].Center).Length() < 1000f)
                    {
                        num656++;
                    }
                }
                npc.ai[1] += num656 / 2;
                if (npc.life < npc.lifeMax * 0.75)
                {
                    npc.ai[1] += 0.25f;
                }
                if (npc.life < npc.lifeMax * 0.5)
                {
                    npc.ai[1] += 0.25f;
                }
                if (npc.life < npc.lifeMax * 0.25)
                {
                    npc.ai[1] += 0.25f;
                }
                if (npc.life < npc.lifeMax * 0.1)
                {
                    npc.ai[1] += 0.25f;
                }
            }
            bool flag35 = false;
            int num658 = (int)(40f - 18f * num633);
            if (npc.ai[1] > num658)
            {
                npc.ai[1] = 0f;
                npc.ai[2]++;
                flag35 = true;
            }
            if (Collision.CanHit(vector84, 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height) && flag35)
            {
                SoundEngine.PlaySound(3, (int)npc.position.X, (int)npc.position.Y);
                if (Main.netMode != 1)
                {
                    int num659 = Main.rand.Next(210, 212);
                    int num660 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)vector84.X, (int)vector84.Y, num659);
                    Main.npc[num660].velocity = Main.player[npc.target].Center - npc.Center;
                    Main.npc[num660].velocity.Normalize();
                    NPC nPC3 = Main.npc[num660];
                    nPC3.velocity *= 5f;
                    Main.npc[num660].CanBeReplacedByOtherNPCs = true;
                    Main.npc[num660].localAI[0] = 60f;
                    Main.npc[num660].netUpdate = true;
                }
            }
            if (num655 > 400f || !Collision.CanHit(new Vector2(vector84.X, vector84.Y - 30f), 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
            {
                float num661 = 14f;
                float num662 = 0.1f;
                vector85 = vector84;
                num653 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector85.X;
                num654 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector85.Y;
                num655 = (float)Math.Sqrt(num653 * num653 + num654 * num654);
                _ = num661 / num655;
                if (npc.velocity.X < num653)
                {
                    npc.velocity.X += num662;
                    if (npc.velocity.X < 0f && num653 > 0f)
                    {
                        npc.velocity.X += num662;
                    }
                }
                else if (npc.velocity.X > num653)
                {
                    npc.velocity.X -= num662;
                    if (npc.velocity.X > 0f && num653 < 0f)
                    {
                        npc.velocity.X -= num662;
                    }
                }
                if (npc.velocity.Y < num654)
                {
                    npc.velocity.Y += num662;
                    if (npc.velocity.Y < 0f && num654 > 0f)
                    {
                        npc.velocity.Y += num662;
                    }
                }
                else if (npc.velocity.Y > num654)
                {
                    npc.velocity.Y -= num662;
                    if (npc.velocity.Y > 0f && num654 < 0f)
                    {
                        npc.velocity.Y -= num662;
                    }
                }
            }
            else
            {
                npc.velocity *= 0.9f;
            }
            npc.spriteDirection = npc.direction;
            if (npc.ai[2] > 5f)
            {
                npc.ai[0] = -1f;
                npc.ai[1] = 1f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 3f)
        {
            float num663 = 4f;
            float num664 = 0.05f;
            if (Main.expertMode)
            {
                num664 = 0.075f;
                num663 = 6f;
            }
            num664 += 0.2f * num633;
            num663 += 6f * num633;
            Vector2 vector86 = new(npc.position.X + npc.width / 2 + Main.rand.Next(20) * npc.direction, npc.position.Y + npc.height * 0.8f);
            Vector2 vector87 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num665 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector87.X;
            float num666 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - 300f - vector87.Y;
            float num667 = (float)Math.Sqrt(num665 * num665 + num666 * num666);
            npc.ai[1] += 1f;
            int num668 = 40;
            if (Main.expertMode)
            {
                num668 = ((npc.life < npc.lifeMax * 0.1) ? 15 : ((npc.life < npc.lifeMax / 3) ? 25 : ((npc.life >= npc.lifeMax / 2) ? 35 : 30)));
            }
            num668 -= (int)(5f * num633);
            if (npc.ai[1] % num668 == num668 - 1 && npc.position.Y + npc.height < Main.player[npc.target].position.Y && Collision.CanHit(vector86, 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
            {
                SoundEngine.PlaySound(SoundID.Item17, npc.position);
                if (Main.netMode != 1)
                {
                    float num669 = 8f;
                    if (Main.expertMode)
                    {
                        num669 += 2f;
                    }
                    if (Main.expertMode && npc.life < npc.lifeMax * 0.1)
                    {
                        num669 += 3f;
                    }
                    num669 += 7f * num633;
                    int num670 = (int)(80f - 39f * num633);
                    int num671 = (int)(40f - 19f * num633);
                    if (num670 < 1)
                    {
                        num670 = 1;
                    }
                    if (num671 < 1)
                    {
                        num671 = 1;
                    }
                    float num672 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector86.X + Main.rand.Next(-num670, num670 + 1);
                    float num673 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector86.Y + Main.rand.Next(-num671, num671 + 1);
                    float num674 = (float)Math.Sqrt(num672 * num672 + num673 * num673);
                    num674 = num669 / num674;
                    num672 *= num674;
                    num673 *= num674;
                    int num675 = 11;
                    int num676 = 719;
                    int num677 = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector86.X, vector86.Y, num672, num673, num676, num675, 0f, Main.myPlayer);
                    Main.projectile[num677].timeLeft = 300;
                }
            }
            if (!Collision.CanHit(new Vector2(vector86.X, vector86.Y - 30f), 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
            {
                num663 = 14f;
                num664 = 0.1f;
                if (num633 > 0f)
                {
                    num664 = 0.5f;
                }
                vector87 = vector86;
                num665 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector87.X;
                num666 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector87.Y;
                num667 = (float)Math.Sqrt(num665 * num665 + num666 * num666);
                _ = num663 / num667;
                if (npc.velocity.X < num665)
                {
                    npc.velocity.X += num664;
                    if (npc.velocity.X < 0f && num665 > 0f)
                    {
                        npc.velocity.X += num664;
                    }
                }
                else if (npc.velocity.X > num665)
                {
                    npc.velocity.X -= num664;
                    if (npc.velocity.X > 0f && num665 < 0f)
                    {
                        npc.velocity.X -= num664;
                    }
                }
                if (npc.velocity.Y < num666)
                {
                    npc.velocity.Y += num664;
                    if (npc.velocity.Y < 0f && num666 > 0f)
                    {
                        npc.velocity.Y += num664;
                    }
                }
                else if (npc.velocity.Y > num666)
                {
                    npc.velocity.Y -= num664;
                    if (npc.velocity.Y > 0f && num666 < 0f)
                    {
                        npc.velocity.Y -= num664;
                    }
                }
            }
            else if (num667 > 100f)
            {
                npc.TargetClosest();
                npc.spriteDirection = npc.direction;
                _ = num663 / num667;
                if (npc.velocity.X < num665)
                {
                    npc.velocity.X += num664;
                    if (npc.velocity.X < 0f && num665 > 0f)
                    {
                        npc.velocity.X += num664 * 2f;
                    }
                }
                else if (npc.velocity.X > num665)
                {
                    npc.velocity.X -= num664;
                    if (npc.velocity.X > 0f && num665 < 0f)
                    {
                        npc.velocity.X -= num664 * 2f;
                    }
                }
                if (npc.velocity.Y < num666)
                {
                    npc.velocity.Y += num664;
                    if (npc.velocity.Y < 0f && num666 > 0f)
                    {
                        npc.velocity.Y += num664 * 2f;
                    }
                }
                else if (npc.velocity.Y > num666)
                {
                    npc.velocity.Y -= num664;
                    if (npc.velocity.Y > 0f && num666 < 0f)
                    {
                        npc.velocity.Y -= num664 * 2f;
                    }
                }
            }
            float num678 = 20f;
            num678 -= 5f * num633;
            if (npc.ai[1] > num668 * num678)
            {
                npc.ai[0] = -1f;
                npc.ai[1] = 3f;
                npc.netUpdate = true;
            }
        }
        else if (npc.ai[0] == 4f)
        {
            npc.localAI[0] = 1f;
            float num679 = 14f;
            float num680 = 14f;
            Vector2 vector88 = Main.player[npc.target].Center - npc.Center;
            vector88.Normalize();
            vector88 *= num679;
            npc.velocity = (npc.velocity * num680 + vector88) / (num680 + 1f);
            if (npc.velocity.X < 0f)
            {
                npc.direction = -1;
            }
            else
            {
                npc.direction = 1;
            }
            npc.spriteDirection = npc.direction;
            if (num634 < 2000f)
            {
                npc.ai[0] = -1f;
                npc.localAI[0] = 0f;
            }
        }
    }
}
