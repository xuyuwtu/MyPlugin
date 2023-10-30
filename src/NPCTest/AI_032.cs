namespace VBY.NPCTest;

public static partial class NPCAIs
{
    public static void AI_032(this NPC npc)
    {
        npc.damage = npc.defDamage;
        npc.defense = npc.defDefense;
        if (npc.ai[3] != 0f)
        {
            NPC.mechQueen = npc.whoAmI;
        }
        npc.reflectsProjectiles = false;
        if (npc.ai[0] == 0f && Main.netMode != 1)
        {
            npc.TargetClosest();
            npc.ai[0] = 1f;
            int num484 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2), (int)npc.position.Y + npc.height / 2, 128, npc.whoAmI);
            Main.npc[num484].ai[0] = -1f;
            Main.npc[num484].ai[1] = npc.whoAmI;
            Main.npc[num484].target = npc.target;
            Main.npc[num484].netUpdate = true;
            num484 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2), (int)npc.position.Y + npc.height / 2, 129, npc.whoAmI);
            Main.npc[num484].ai[0] = 1f;
            Main.npc[num484].ai[1] = npc.whoAmI;
            Main.npc[num484].target = npc.target;
            Main.npc[num484].netUpdate = true;
            num484 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2), (int)npc.position.Y + npc.height / 2, 130, npc.whoAmI);
            Main.npc[num484].ai[0] = -1f;
            Main.npc[num484].ai[1] = npc.whoAmI;
            Main.npc[num484].target = npc.target;
            Main.npc[num484].ai[3] = 150f;
            Main.npc[num484].netUpdate = true;
            num484 = NPC.NewNPC(npc.GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + npc.width / 2), (int)npc.position.Y + npc.height / 2, 131, npc.whoAmI);
            Main.npc[num484].ai[0] = 1f;
            Main.npc[num484].ai[1] = npc.whoAmI;
            Main.npc[num484].target = npc.target;
            Main.npc[num484].netUpdate = true;
            Main.npc[num484].ai[3] = 150f;
        }
        if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 6000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 6000f)
        {
            npc.TargetClosest();
            if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 6000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 6000f)
            {
                npc.ai[1] = 3f;
            }
        }
        if (Main.IsItDay() && npc.ai[1] != 3f && npc.ai[1] != 2f)
        {
            npc.ai[1] = 2f;
            SoundEngine.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
        }
        if (npc.ai[1] == 0f)
        {
            npc.ai[2] += 1f;
            if (npc.ai[2] >= 600f)
            {
                npc.ai[2] = 0f;
                npc.ai[1] = 1f;
                npc.TargetClosest();
                npc.netUpdate = true;
            }
            if (NPC.IsMechQueenUp)
            {
                npc.rotation = npc.rotation.AngleLerp(npc.velocity.X / 15f * 0.5f, 0.75f);
            }
            else
            {
                npc.rotation = npc.velocity.X / 15f;
            }
            float num485 = 0.1f;
            float num486 = 2f;
            float num487 = 0.1f;
            float num488 = 8f;
            int num489 = 200;
            int num490 = 500;
            float num491 = 0f;
            int num492 = ((!(Main.player[npc.target].Center.X < npc.Center.X)) ? 1 : (-1));
            if (NPC.IsMechQueenUp)
            {
                num491 = -450f * num492;
                num489 = 300;
                num490 = 350;
            }
            if (Main.expertMode)
            {
                num485 = 0.03f;
                num486 = 4f;
                num487 = 0.07f;
                num488 = 9.5f;
            }
            if (npc.position.Y > Main.player[npc.target].position.Y - num489)
            {
                if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y *= 0.98f;
                }
                npc.velocity.Y -= num485;
                if (npc.velocity.Y > num486)
                {
                    npc.velocity.Y = num486;
                }
            }
            else if (npc.position.Y < Main.player[npc.target].position.Y - num490)
            {
                if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y *= 0.98f;
                }
                npc.velocity.Y += num485;
                if (npc.velocity.Y < 0f - num486)
                {
                    npc.velocity.Y = 0f - num486;
                }
            }
            if (npc.position.X + npc.width / 2 > Main.player[npc.target].position.X + Main.player[npc.target].width / 2 + 100f + num491)
            {
                if (npc.velocity.X > 0f)
                {
                    npc.velocity.X *= 0.98f;
                }
                npc.velocity.X -= num487;
                if (npc.velocity.X > num488)
                {
                    npc.velocity.X = num488;
                }
            }
            if (npc.position.X + npc.width / 2 < Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - 100f + num491)
            {
                if (npc.velocity.X < 0f)
                {
                    npc.velocity.X *= 0.98f;
                }
                npc.velocity.X += num487;
                if (npc.velocity.X < 0f - num488)
                {
                    npc.velocity.X = 0f - num488;
                }
            }
        }
        else if (npc.ai[1] == 1f)
        {
            if (npc.ai[2] == 0f)
            {
                npc.localAI[3] = 5 + (int)(((float)(npc.lifeMax - npc.life) / npc.lifeMax) / (1f / 12f));
            }
            npc.defense *= 2;
            npc.damage *= 2;
            npc.ai[2] += 1f;
            if (npc.ai[2] == 2f)
            {
                SoundEngine.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
            }
            if(npc.ai[2] % (int)(400 / npc.localAI[3]) == 0)
            {
                var count = npc.ai[2] / (int)(400 / npc.localAI[3]);
                var degress = count * 360 / npc.localAI[3];
                Console.WriteLine(count);
                //var target = npc.Center + (Vector2.UnitY * 16 * 50).RotatedByDegress(degress);
                //var proj = Main.projectile[npc.NewProjectile(npc.Center, (Vector2.UnitY * 16).RotatedByDegress(degress), 526, 50, target.X, target.Y, npc.target)];
                var target = npc.Center + (Vector2.UnitY * 16 * 50).RotatedBy(npc.rotation);
                var proj = Main.projectile[npc.NewProjectile(npc.Center, (Vector2.UnitY * 16).RotatedBy(npc.rotation), 526, 25, target.X, target.Y, npc.target)];
                proj.localAI[0] = npc.Center.X - target.X;
                proj.localAI[1] = npc.Center.Y - target.Y;
                proj.localAI[2] = 299;
            }
            if (npc.ai[2] >= 400f)
            {
                npc.ai[2] = 0f;
                npc.ai[1] = 0f;
            }
            if (NPC.IsMechQueenUp)
            {
                npc.rotation = npc.rotation.AngleLerp(npc.velocity.X / 15f * 0.5f, 0.75f);
            }
            else
            {
                npc.rotation += npc.direction * 0.3f;
            }
            Vector2 vector54 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num493 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector54.X;
            float num494 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector54.Y;
            float num495 = (float)Math.Sqrt(num493 * num493 + num494 * num494);
            float num496 = 2f;
            if (Main.expertMode)
            {
                num496 = 6f;
                if (num495 > 150f)
                {
                    num496 *= 1.05f;
                }
                if (num495 > 200f)
                {
                    num496 *= 1.1f;
                }
                if (num495 > 250f)
                {
                    num496 *= 1.1f;
                }
                if (num495 > 300f)
                {
                    num496 *= 1.1f;
                }
                if (num495 > 350f)
                {
                    num496 *= 1.1f;
                }
                if (num495 > 400f)
                {
                    num496 *= 1.1f;
                }
                if (num495 > 450f)
                {
                    num496 *= 1.1f;
                }
                if (num495 > 500f)
                {
                    num496 *= 1.1f;
                }
                if (num495 > 550f)
                {
                    num496 *= 1.1f;
                }
                if (num495 > 600f)
                {
                    num496 *= 1.1f;
                }
            }
            if (NPC.IsMechQueenUp)
            {
                float num497 = (NPC.npcsFoundForCheckActive[135] ? 0.6f : 0.75f);
                num496 *= num497;
            }
            num495 = num496 / num495;
            npc.velocity.X = num493 * num495;
            npc.velocity.Y = num494 * num495;
            if (NPC.IsMechQueenUp)
            {
                float num498 = Vector2.Distance(npc.Center, Main.player[npc.target].Center);
                if (num498 < 0.1f)
                {
                    num498 = 0f;
                }
                if (num498 < num496)
                {
                    npc.velocity = npc.velocity.SafeNormalize(Vector2.Zero) * num498;
                }
            }
        }
        else if (npc.ai[1] == 2f)
        {
            npc.damage = 1000;
            npc.defense = 9999;
            if (NPC.IsMechQueenUp)
            {
                npc.rotation = npc.rotation.AngleLerp(npc.velocity.X / 15f * 0.5f, 0.75f);
            }
            else
            {
                npc.rotation += npc.direction * 0.3f;
            }
            Vector2 vector55 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num499 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector55.X;
            float num500 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector55.Y;
            float num501 = (float)Math.Sqrt(num499 * num499 + num500 * num500);
            float num502 = 10f;
            num502 += num501 / 100f;
            if (num502 < 8f)
            {
                num502 = 8f;
            }
            if (num502 > 32f)
            {
                num502 = 32f;
            }
            num501 = num502 / num501;
            npc.velocity.X = num499 * num501;
            npc.velocity.Y = num500 * num501;
        }
        else
        {
            if (npc.ai[1] != 3f)
            {
                return;
            }
            if (NPC.IsMechQueenUp)
            {
                int num503 = NPC.FindFirstNPC(125);
                if (num503 >= 0)
                {
                    Main.npc[num503].EncourageDespawn(5);
                }
                num503 = NPC.FindFirstNPC(126);
                if (num503 >= 0)
                {
                    Main.npc[num503].EncourageDespawn(5);
                }
                if (!NPC.AnyNPCs(125) && !NPC.AnyNPCs(126))
                {
                    num503 = NPC.FindFirstNPC(134);
                    if (num503 >= 0)
                    {
                        Main.npc[num503].Transform(136);
                    }
                    npc.EncourageDespawn(5);
                }
                npc.velocity.Y += 0.1f;
                if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y *= 0.95f;
                }
                npc.velocity.X *= 0.95f;
                if (npc.velocity.Y > 13f)
                {
                    npc.velocity.Y = 13f;
                }
            }
            else
            {
                npc.EncourageDespawn(500);
                npc.velocity.Y += 0.1f;
                if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y *= 0.95f;
                }
                npc.velocity.X *= 0.95f;
            }
        }
    }
}
