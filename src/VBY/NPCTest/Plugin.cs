using System.Reflection;

using TerrariaApi.Server;

using VBY.NPCAI;

namespace VBY.NPCTest;
[ApiVersion(2, 1)]
public class Plugin : TerrariaPlugin
{
    public override string Name => "VBY.NPCTest";
    public override string Author => "yu";
    public override Version Version => new(1, 0, 0, 1);
    private readonly MethodInfo[] methods = typeof(NPCAIs).GetMethods(BindingFlags.Static | BindingFlags.Public);
    private readonly MonoMod.RuntimeDetour.Detour ScaleStats_ApplyMultiplayerStatsDetour = new(typeof(NPC).GetMethod(nameof(NPC.ScaleStats_ApplyMultiplayerStats)), typeof(MainPlugin).GetMethod(nameof(ScaleStats_ApplyMultiplayerStats)), new() { ManualApply = true });
    public Plugin(Main game) : base(game)
    {
    }

    public override void Initialize()
    {
        ScaleStats_ApplyMultiplayerStatsDetour.Apply();
        foreach (MethodInfo method in methods)
        {
            AIs.SetMethod(method);
        }
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ScaleStats_ApplyMultiplayerStatsDetour.Dispose();
            foreach (MethodInfo method in methods)
            {
                AIs.RemoveMethod(method);
            }
        }
    }
    public static void ScaleStats_ApplyMultiplayerStats(NPC npc, int numPlayers, float balance, float boost, float bossAdjustment)
    {
        int num = numPlayers - 1;
        if (npc.type == NPCID.ServantofCthulhu)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossAdjustment);
        }
        if (npc.type == NPCID.EyeofCthulhu)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.65 * (double)balance * (double)bossAdjustment);
        }
        if (npc.type >= 13 && npc.type <= 15)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.7 * (double)balance * (double)bossAdjustment);
            if (npc.type == NPCID.EaterofWorldsHead)
            {
                npc.damage = (int)(npc.damage * 1.1);
            }
            if (npc.type == NPCID.EaterofWorldsBody)
            {
                npc.damage = (int)(npc.damage * 0.8);
            }
            if (npc.type == NPCID.EaterofWorldsTail)
            {
                npc.damage = (int)(npc.damage * 0.8);
            }
            npc.scale *= 1.2f;
            npc.defense += 2;
        }
        if (npc.type == NPCID.BrainofCthulhu || npc.type == NPCID.Creeper)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.85 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 0.9);
            npc.scale *= 1.05f;
            for (float num2 = 1f; num2 < balance; num2 += 0.34f)
            {
                if (npc.knockBackResist < 0.1)
                {
                    npc.knockBackResist = 0f;
                    break;
                }
                npc.knockBackResist *= 0.8f;
            }
        }
        if (npc.type == NPCID.KingSlime)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.7 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 0.8);
        }
        if (npc.type == NPCID.GoblinSummoner)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.85 * (double)(balance * 2f + 1f) / 3.0);
        }
        if (npc.type == NPCID.ShadowFlameApparition)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.85 * (double)(balance + 1f) / 2.0);
            npc.damage = (int)(npc.damage * 0.8);
        }
        if (npc.type == NPCID.QueenBee)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.7 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 0.9);
        }
        if (npc.type == NPCID.Bee || npc.type == NPCID.BeeSmall)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75);
        }
        if (npc.type == NPCID.SkeletronHead)
        {
            npc.lifeMax = (int)(npc.lifeMax * balance * bossAdjustment);
            npc.damage = (int)(npc.damage * 1.1);
        }
        else if (npc.type == NPCID.SkeletronHand)
        {
            npc.lifeMax = (int)(npc.lifeMax * 1.3 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 1.1);
        }
        if (npc.type == NPCID.Deerclops)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.85 * (double)balance * (double)bossAdjustment);
            //npc.damage = npc.damage;
        }
        if (npc.type == NPCID.WallofFlesh || npc.type == NPCID.WallofFleshEye)
        {
            npc.defense += 6;
            npc.lifeMax = (int)(npc.lifeMax * 0.7 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 1.5);
        }
        else if (npc.type == NPCID.TheHungry)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.7 * (double)balance);
            //if (numPlayers > 4)
            //{
            //    npc.knockBackResist = 0f;
            //}
            /*else*/ if (numPlayers > 1)
            {
                npc.knockBackResist *= 1f - boost;
            }
            npc.defense += 6;
        }
        else if (npc.type == NPCID.TheHungryII)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.7 * (double)balance);
            //if (numPlayers > 4)
            //{
            //    npc.knockBackResist = 0f;
            //}
            /*else*/ if (numPlayers > 1)
            {
                npc.knockBackResist *= 1f - boost;
            }
        }
        else if (npc.type == NPCID.LeechHead || npc.type == NPCID.LeechBody || npc.type == NPCID.LeechTail)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.8);
        }
        if (npc.type == NPCID.QueenSlimeBoss)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.8f * balance * bossAdjustment);
        }
        if (npc.type >= 658 && npc.type <= 660)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75f * balance * bossAdjustment);
        }
        if (npc.type >= 134 && npc.type <= 136)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75 * (double)balance * (double)bossAdjustment);
            if (npc.type == NPCID.TheDestroyer)
            {
                npc.damage *= 2;
            }
            if (npc.type == NPCID.TheDestroyerBody)
            {
                npc.damage = (int)(npc.damage * 0.85);
            }
            if (npc.type == NPCID.TheDestroyerTail)
            {
                npc.damage = (int)(npc.damage * 0.85);
            }
            npc.scale *= 1.05f;
        }
        else if (npc.type == NPCID.Probe)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75 * (double)(balance * 2f + 1f) / 3.0);
            npc.damage = (int)(npc.damage * 0.8);
            npc.scale *= 1.05f;
        }
        if (npc.type >= 127 && npc.type <= 131)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 0.85);
        }
        if (npc.type >= 125 && npc.type <= 126)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 0.85);
        }
        if (npc.type == NPCID.Plantera)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.7 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 1.15);
        }
        else if (npc.type == NPCID.PlanterasTentacle)
        {
            npc.lifeMax = (int)(npc.lifeMax * balance * bossAdjustment);
            npc.damage = (int)(npc.damage * 1.15);
        }
        if (npc.type == NPCID.HallowBoss)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.7 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 1.15);
        }
        if (npc.type >= 245 && npc.type <= 249)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 0.8);
        }
        if (npc.type == NPCID.DukeFishron)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.65 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 0.7);
        }
        else if (npc.type == NPCID.DetonatingBubble || npc.type == NPCID.Sharkron || npc.type == NPCID.Sharkron2)
        {
            if (npc.type != NPCID.DetonatingBubble)
            {
                npc.lifeMax = (int)(npc.lifeMax * 0.75);
            }
            npc.damage = (int)(npc.damage * 0.75);
        }
        if (npc.type == NPCID.CultistBoss || npc.type == NPCID.CultistBossClone || (npc.type >= 454 && npc.type <= 459) || npc.type == NPCID.AncientLight || npc.type == NPCID.AncientDoom)
        {
            if (npc.type != NPCID.AncientLight)
            {
                npc.lifeMax = (int)(npc.lifeMax * 0.75f * balance * bossAdjustment);
            }
            npc.damage = (int)(npc.damage * 0.75);
        }
        if (npc.type == NPCID.MoonLordHand || npc.type == NPCID.MoonLordHead || npc.type == NPCID.MoonLordCore)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 0.75);
        }
        if (npc.type == NPCID.DD2Betsy)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 0.65);
        }
        else if (NPCID.Sets.BelongsToInvasionOldOnesArmy[npc.type])
        {
            int num3 = 7;
            float num4 = (balance * (num3 - 1) + 1f) / num3;
            npc.lifeMax = (int)(npc.lifeMax * num4 * bossAdjustment);
        }
        float num5 = 1f + num * 0.2f;
        switch (npc.type)
        {
            case NPCID.Scarecrow1:
            case NPCID.Scarecrow2:
            case NPCID.Scarecrow3:
            case NPCID.Scarecrow4:
            case NPCID.Scarecrow5:
            case NPCID.Scarecrow6:
            case NPCID.Scarecrow7:
            case NPCID.Scarecrow8:
            case NPCID.Scarecrow9:
            case NPCID.Scarecrow10:
            case NPCID.Splinterling:
            case NPCID.Hellhound:
            case NPCID.Poltergeist:
                npc.lifeMax = (int)(npc.lifeMax * 0.75 * (double)num5);
                npc.damage = (int)(npc.damage * 0.75);
                break;
            case NPCID.HeadlessHorseman:
            case NPCID.MourningWood:
            case NPCID.Pumpking:
                npc.lifeMax = (int)(npc.lifeMax * 0.65 * (double)bossAdjustment);
                npc.damage = (int)(npc.damage * 0.75);
                break;
        }
        switch (npc.type)
        {
            case NPCID.ZombieElf:
            case NPCID.ZombieElfBeard:
            case NPCID.ZombieElfGirl:
            case NPCID.PresentMimic:
            case NPCID.GingerbreadMan:
            case NPCID.Yeti:
            case NPCID.ElfCopter:
            case NPCID.Nutcracker:
            case NPCID.NutcrackerSpinning:
            case NPCID.ElfArcher:
            case NPCID.Krampus:
            case NPCID.Flocko:
                npc.lifeMax = (int)(npc.lifeMax * 0.75 * (double)num5);
                npc.damage = (int)(npc.damage * 0.75);
                break;
            case NPCID.Everscream:
            case NPCID.IceQueen:
            case NPCID.SantaNK1:
                npc.lifeMax = (int)(npc.lifeMax * 0.65 * (double)bossAdjustment);
                npc.damage = (int)(npc.damage * 0.75);
                break;
        }
        if (Main.getGoodWorld)
        {
            if (npc.type == NPCID.EaterofSouls && NPC.AnyNPCs(NPCID.EaterofWorldsHead))
            {
                npc.lifeMax = (int)(npc.lifeMax * 1.5 * (double)bossAdjustment);
                npc.defense += 2;
            }
            if (npc.type == NPCID.DarkCaster && NPC.AnyNPCs(NPCID.SkeletronHead))
            {
                npc.lifeMax = (int)(npc.lifeMax * 1.5 * (double)bossAdjustment);
                npc.defense += 6;
            }
            if (npc.type == NPCID.FireImp && NPC.AnyNPCs(NPCID.WallofFlesh))
            {
                npc.lifeMax = (int)(npc.lifeMax * 1.5 * (double)bossAdjustment);
                npc.defense += 10;
            }
        }
        npc.defDefense = npc.defense;
        npc.defDamage = npc.damage;
        npc.life = npc.lifeMax;
    }

}
