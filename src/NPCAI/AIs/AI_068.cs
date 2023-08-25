using OTAPI;

namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_068(this NPC npc)
    {
        npc.noGravity = true;
        if (npc.ai[0] == 0f)
        {
            npc.noGravity = false;
            int num1031 = npc.direction;
            int num1032 = npc.target;
            npc.TargetClosest();
            if (num1032 >= 0 && num1031 != 0)
            {
                npc.direction = num1031;
            }
            if (npc.wet && WorldGen.InWorld((int)(npc.Center.X + (npc.width / 2 + 8) * npc.direction) / 16, (int)(npc.Center.Y / 16f), 5))
            {
                float num1033 = 2f;
                npc.velocity.X = (npc.velocity.X * 19f + num1033 * npc.direction) / 20f;
                int num1034 = (int)(npc.Center.X + (npc.width / 2 + 8) * npc.direction) / 16;
                int num1035 = (int)(npc.Center.Y / 16f);
                int j4 = (int)(npc.position.Y / 16f);
                int num1036 = (int)((npc.position.Y + npc.height) / 16f);
                if (Main.tile[num1034, num1035] == null)
                {
                    Main.tile[num1034, num1035] = Hooks.Tile.InvokeCreate();
                }
                if (Main.tile[num1034, num1036] == null)
                {
                    Main.tile[num1034, num1036] = Hooks.Tile.InvokeCreate();
                }
                if (num1034 < 5 || num1034 > Main.maxTilesX - 5 || WorldGen.SolidTile(num1034, num1035) || WorldGen.SolidTile(num1034, j4) || WorldGen.SolidTile(num1034, num1036) || Main.tile[num1034, num1036].liquid == 0)
                {
                    npc.direction *= -1;
                }
                npc.spriteDirection = npc.direction;
                if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y *= 0.5f;
                }
                npc.noGravity = true;
                num1034 = (int)(npc.Center.X / 16f);
                num1035 = (int)(npc.Center.Y / 16f);
                float num1037 = npc.position.Y + npc.height;
                if (Main.tile[num1034, num1035 - 1] == null)
                {
                    Main.tile[num1034, num1035 - 1] = Hooks.Tile.InvokeCreate();
                }
                if (Main.tile[num1034, num1035] == null)
                {
                    Main.tile[num1034, num1035] = Hooks.Tile.InvokeCreate();
                }
                if (Main.tile[num1034, num1035 + 1] == null)
                {
                    Main.tile[num1034, num1035 + 1] = Hooks.Tile.InvokeCreate();
                }
                if (Main.tile[num1034, num1035 - 1].liquid > 0)
                {
                    num1037 = num1035 * 16;
                    num1037 -= Main.tile[num1034, num1035 - 1].liquid / 16;
                }
                else if (Main.tile[num1034, num1035].liquid > 0)
                {
                    num1037 = (num1035 + 1) * 16;
                    num1037 -= Main.tile[num1034, num1035].liquid / 16;
                }
                else if (Main.tile[num1034, num1035 + 1].liquid > 0)
                {
                    num1037 = (num1035 + 2) * 16;
                    num1037 -= Main.tile[num1034, num1035 + 1].liquid / 16;
                }
                num1037 -= 6f;
                if (npc.Center.Y > num1037)
                {
                    npc.velocity.Y -= 0.1f;
                    if (npc.velocity.Y < -8f)
                    {
                        npc.velocity.Y = -8f;
                    }
                    if (npc.Center.Y + npc.velocity.Y < num1037)
                    {
                        npc.velocity.Y = num1037 - npc.Center.Y;
                    }
                }
                else
                {
                    npc.velocity.Y = num1037 - npc.Center.Y;
                }
            }
            if (Main.netMode == 1)
            {
                return;
            }
            if (!npc.wet)
            {
                npc.ai[0] = 1f;
                npc.netUpdate = true;
                npc.direction = -npc.direction;
                return;
            }
            Rectangle rectangle4 = new((int)Main.player[npc.target].position.X, (int)Main.player[npc.target].position.Y, Main.player[npc.target].width, Main.player[npc.target].height);
            if (new Rectangle((int)npc.position.X - 100, (int)npc.position.Y - 100, npc.width + 200, npc.height + 200).Intersects(rectangle4) || npc.life < npc.lifeMax)
            {
                npc.ai[0] = 1f;
                npc.velocity.Y -= 6f;
                npc.netUpdate = true;
                npc.direction = -npc.direction;
            }
        }
        else
        {
            if (Main.player[npc.target].dead)
            {
                return;
            }
            bool flag54 = false;
            npc.ai[1] += 1f;
            if (npc.ai[1] >= 300f)
            {
                flag54 = true;
            }
            if (flag54)
            {
                if (npc.velocity.Y == 0f || npc.collideY || npc.wet)
                {
                    npc.velocity.X = 0f;
                    npc.velocity.Y = 0f;
                    npc.ai[0] = 0f;
                    npc.ai[1] = 0f;
                    if (Main.netMode != 1)
                    {
                        if ((npc.type == 363 || npc.type == 365 || npc.type == 603 || npc.type == 609) && !npc.wet)
                        {
                            int num1038 = npc.direction;
                            npc.Transform(npc.type - 1);
                            npc.TargetClosest();
                            npc.direction = num1038;
                            npc.ai[0] = 0f;
                            npc.ai[1] = 200 + Main.rand.Next(200);
                        }
                        npc.netUpdate = true;
                    }
                }
                else
                {
                    npc.velocity.X *= 0.98f;
                    npc.velocity.Y += 0.1f;
                    if (npc.velocity.Y > 2f)
                    {
                        npc.velocity.Y = 2f;
                    }
                }
                return;
            }
            if (npc.collideX)
            {
                npc.direction *= -1;
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
            int x16 = (int)((npc.position.X + npc.width / 2) / 16f) + npc.direction;
            int num1039 = (int)((npc.position.Y + npc.height) / 16f);
            bool flag55 = true;
            int num1040 = 15;
            bool flag56 = false;
            for (int num1041 = num1039; num1041 < num1039 + num1040; num1041++)
            {
                if (Main.tile[x16, num1041] == null)
                {
                    Main.tile[x16, num1041] = Hooks.Tile.InvokeCreate();
                }
                if ((Main.tile[x16, num1041].nactive() && Main.tileSolid[Main.tile[x16, num1041].type]) || Main.tile[x16, num1041].liquid > 0)
                {
                    if (num1041 < num1039 + 5)
                    {
                        flag56 = true;
                    }
                    flag55 = false;
                    break;
                }
            }
            if (flag55)
            {
                npc.velocity.Y += 0.1f;
            }
            else
            {
                npc.velocity.Y -= 0.1f;
            }
            if (flag56)
            {
                npc.velocity.Y -= 0.2f;
            }
            if (npc.velocity.Y > 3f)
            {
                npc.velocity.Y = 3f;
            }
            if (npc.velocity.Y < -4f)
            {
                npc.velocity.Y = -4f;
            }
        }
    }
}
