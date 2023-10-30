using ReLogic.Utilities;
using Terraria.GameContent.Events;

namespace VBY.NPCAI;

partial class AIs
{
    public static void AI_106(this NPC npc)
    {
        if (npc.alpha == 0)
        {
            Lighting.AddLight(npc.Center, 0.5f, 0.1f, 0.3f);
        }
        if (npc.ai[1] == 0f)
        {
            if (npc.localAI[0] == 0f)
            {
                SoundEngine.PlayTrackedSound(SoundID.DD2_EtherianPortalOpen, npc.Center);
                npc.localAI[3] = SlotId.Invalid.ToFloat();
            }
            if (npc.localAI[0] > 150f)
            {
                ActiveSound activeSound = SoundEngine.GetActiveSound(SlotId.FromFloat(npc.localAI[3]));
                if (activeSound == null)
                {
                    npc.localAI[3] = SoundEngine.PlayTrackedSound(SoundID.DD2_EtherianPortalIdleLoop, npc.Center).ToFloat();
                }
            }
            if (!DD2Event.EnemySpawningIsOnHold)
            {
                npc.ai[0]++;
            }
            if (npc.ai[0] >= DD2Event.LaneSpawnRate)
            {
                if (npc.ai[0] >= DD2Event.LaneSpawnRate * 3)
                {
                    npc.ai[0] = 0f;
                }
                npc.netUpdate = true;
                if (Main.netMode != 1 && (int)npc.ai[0] % DD2Event.LaneSpawnRate == 0)
                {
                    DD2Event.SpawnMonsterFromGate(npc.Bottom);
                    if (DD2Event.EnemySpawningIsOnHold)
                    {
                        npc.ai[0] += 1f;
                    }
                }
            }
            npc.localAI[0]++;
            if (npc.localAI[0] > 180f)
            {
                npc.localAI[0] = 180f;
            }
            if (Main.netMode != 1 && npc.localAI[0] >= 180f)
            {
                if (NPC.AnyNPCs(548))
                {
                    npc.dontTakeDamage = true;
                    return;
                }
                npc.ai[1] = 1f;
                npc.ai[0] = 0f;
                npc.dontTakeDamage = true;
            }
        }
        else if (npc.ai[1] == 1f)
        {
            npc.ai[0]++;
            npc.scale = MathHelper.Lerp(1f, 0.05f, Utils.GetLerpValue(500f, 600f, npc.ai[0], clamped: true));
            ActiveSound activeSound2 = SoundEngine.GetActiveSound(SlotId.FromFloat(npc.localAI[3]));
            if (activeSound2 == null)
            {
                npc.localAI[3] = SoundEngine.PlayTrackedSound(SoundID.DD2_EtherianPortalIdleLoop, npc.Center).ToFloat();
            }
            activeSound2 = SoundEngine.GetActiveSound(SlotId.FromFloat(npc.localAI[3]));
            if (activeSound2 != null)
            {
                activeSound2.Volume = npc.scale;
            }
            if (npc.ai[0] >= 550f)
            {
                npc.dontTakeDamage = false;
                npc.life = 0;
                npc.checkDead();
                npc.netUpdate = true;
                activeSound2?.Stop();
            }
        }
    }
}
