using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;

namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_092(this NPC npc)
    {
        if (Main.rand.Next(20) == 0)
        {
            switch (Main.rand.Next(15, 18))
            {
                case 15:
                    npc.HitSound = SoundID.NPCHit15;
                    break;
                case 16:
                    npc.HitSound = SoundID.NPCHit16;
                    break;
                case 17:
                    npc.HitSound = SoundID.NPCHit17;
                    break;
            }
        }
        if (Main.netMode == 1)
        {
            return;
        }
        bool flag86 = false;
        int x23 = (int)npc.ai[0];
        int y15 = (int)npc.ai[1];
        if (!flag86 && (!Main.tile[x23, y15].active() || Main.tile[x23, y15].type != 378))
        {
            flag86 = true;
        }
        if (!flag86 && (npc.target == 255 || Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > 160000f))
        {
            npc.TargetClosest(faceTarget: false);
            if (npc.target == 255 || Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > 160000f)
            {
                flag86 = true;
            }
        }
        if (flag86)
        {
            npc.life = 0;
            npc.HitEffect();
            npc.active = false;
            int num1423 = TETrainingDummy.Find((int)npc.ai[0], (int)npc.ai[1]);
            if (num1423 != -1)
            {
                ((TETrainingDummy)TileEntity.ByID[num1423]).Deactivate();
            }
        }
    }
}
