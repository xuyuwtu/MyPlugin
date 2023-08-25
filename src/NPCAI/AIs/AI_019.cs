using OTAPI;

namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_019(this NPC npc)
    {
        npc.TargetClosest();
        float num276 = 12f;
        Vector2 vector32 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
        float num277 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector32.X;
        float num278 = Main.player[npc.target].position.Y - vector32.Y;
        float num279 = (float)Math.Sqrt(num277 * num277 + num278 * num278);
        num279 = num276 / num279;
        num277 *= num279;
        num278 *= num279;
        bool flag14 = false;
        if (npc.directionY < 0)
        {
            npc.rotation = (float)(Math.Atan2(num278, num277) + 1.57);
            flag14 = ((!(npc.rotation < -1.2) && !(npc.rotation > 1.2)));
            if (npc.rotation < -0.8)
            {
                npc.rotation = -0.8f;
            }
            else if (npc.rotation > 0.8)
            {
                npc.rotation = 0.8f;
            }
            if (npc.velocity.X != 0f)
            {
                npc.velocity.X *= 0.9f;
                if (npc.velocity.X > -0.1 || npc.velocity.X < 0.1)
                {
                    npc.netUpdate = true;
                    npc.velocity.X = 0f;
                }
            }
        }
        if (npc.ai[0] > 0f)
        {
            if (npc.ai[0] == 200f)
            {
                SoundEngine.PlaySound(SoundID.Item5, npc.position);
            }
            npc.ai[0] -= 1f;
        }
        if (Main.netMode != 1 && flag14 && npc.ai[0] == 0f && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
        {
            npc.ai[0] = 200f;
            int num280 = 10;
            int num281 = 31;
            int num282 = Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), vector32.X, vector32.Y, num277, num278, num281, num280, 0f, Main.myPlayer);
            Main.projectile[num282].ai[0] = 2f;
            Main.projectile[num282].timeLeft = 300;
            Main.projectile[num282].friendly = false;
            NetMessage.SendData(27, -1, -1, null, num282);
            npc.netUpdate = true;
        }
        try
        {
            int x6 = (int)npc.position.X / 16;
            int x7 = (int)(npc.position.X + npc.width / 2) / 16;
            int x8 = (int)(npc.position.X + npc.width) / 16;
            int y4 = (int)(npc.position.Y + npc.height) / 16;
            bool flag15 = false;
            if (Main.tile[x6, y4] == null)
            {
                Main.tile[x6, y4] = Hooks.Tile.InvokeCreate();
            }
            if (Main.tile[x7, y4] == null)
            {
                Main.tile[x6, y4] = Hooks.Tile.InvokeCreate();
            }
            if (Main.tile[x8, y4] == null)
            {
                Main.tile[x6, y4] = Hooks.Tile.InvokeCreate();
            }
            if ((Main.tile[x6, y4].nactive() && Main.tileSolid[Main.tile[x6, y4].type]) || (Main.tile[x7, y4].nactive() && Main.tileSolid[Main.tile[x7, y4].type]) || (Main.tile[x8, y4].nactive() && Main.tileSolid[Main.tile[x8, y4].type]))
            {
                flag15 = true;
            }
            if (flag15)
            {
                npc.noGravity = true;
                npc.noTileCollide = true;
                npc.velocity.Y = -0.2f;
                return;
            }
            npc.noGravity = false;
            npc.noTileCollide = false;
            if (Main.rand.Next(2) == 0)
            {
                npc.position += npc.netOffset;
                int num283 = Dust.NewDust(new Vector2(npc.position.X - 4f, npc.position.Y + npc.height - 8f), npc.width + 8, 24, 32, 0f, npc.velocity.Y / 2f);
                Main.dust[num283].velocity.X *= 0.4f;
                Main.dust[num283].velocity.Y *= -1f;
                if (Main.rand.Next(2) == 0)
                {
                    Main.dust[num283].noGravity = true;
                    Dust dust = Main.dust[num283];
                    dust.scale += 0.2f;
                }
                npc.position -= npc.netOffset;
            }
            return;
        }
        catch
        {
            return;
        }
    }
}
