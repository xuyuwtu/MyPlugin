using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;

namespace VBY.VirtualPlayer;

public class VPlayer : Player
{
    public int type, oldTarget, target;
    public bool ControlUpdate, LifeUpdate;
    public float[] ai = new float[4];
    public VPlayer()
    {
        type = 22;
    }
    public void Update()
    {
        if (!active)
        {
            return;
        }
        bool flag = false;
        int tileX = (int)(position.X + width / 2) / 16;
        int tileY = (int)(position.Y + height / 2) / 16;
        try
        {
            if (tileX >= 4 && tileX <= Main.maxTilesX - 4 && tileY >= 4 && tileY <= Main.maxTilesY - 4)
            {
                if (Main.tile[tileX, tileY] == null)
                {
                    flag = true;
                }
                else if (Main.tile[tileX - 3, tileY] == null)
                {
                    flag = true;
                }
                else if (Main.tile[tileX + 3, tileY] == null)
                {
                    flag = true;
                }
                else if (Main.tile[tileX, tileY - 3] == null)
                {
                    flag = true;
                }
                else if (Main.tile[tileX, tileY + 3] == null)
                {
                    flag = true;
                }
            }
        }
        catch
        {
            flag = true;
        }
        if (flag)
        {
            return;
        }
        oldTarget = target;
        oldDirection = direction;
        AI();

        if (velocity.X < 0.005 && velocity.X > -0.005)
        {
            velocity.X = 0f;
        }
        oldPosition = position;
        oldDirection = direction;
        position += velocity;
    }
    public void AI()
    {
        if (ai[2] == 0)
        {
            for (int i = 0; i < 200; i++)
            {
                var npc = Main.npc[i];
                if (!npc.active || npc.friendly || npc.damage <= 0 || npc.Distance(Center) > 320f)
                {
                    continue;
                }
                if (Collision.CanHit(Center, 1, 1, npc.Center, 1, 1))
                {
                    ControlUpdate = true;
                    controlUseItem = true;
                    gravDir = 1;
                    Projectile.NewProjectile(null, Center, (npc.Center - Center).Normalize(10), 357, 80, 10f, 255);
                }
            }
        }
        ai[2]++;
        if (ai[2] > 60)
        {
            ai[2] = 0;
        }
    }
}