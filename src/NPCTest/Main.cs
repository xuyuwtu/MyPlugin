using System.Reflection;

using TerrariaApi.Server;

using VBY.NPCAI;

namespace VBY.NPCTest;
[ApiVersion(2, 1)]
public class MainPlugin : TerrariaPlugin
{
    public override string Name => "VBY.NPCTest";
    public override string Author => "yu";
    public override Version Version => new(1, 0, 0, 1);
    private readonly MethodInfo[] methods = typeof(NPCAIs).GetMethods(BindingFlags.Static | BindingFlags.Public);
    private readonly MonoMod.RuntimeDetour.Detour ScaleStats_ApplyMultiplayerStatsDetour = new(typeof(NPC).GetMethod(nameof(NPC.ScaleStats_ApplyMultiplayerStats)), typeof(MainPlugin).GetMethod(nameof(ScaleStats_ApplyMultiplayerStats)), new() { ManualApply = true });
    public MainPlugin(Main game) : base(game)
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
        if (npc.type == 5)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossAdjustment);
        }
        if (npc.type == 4)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.65 * (double)balance * (double)bossAdjustment);
        }
        if (npc.type >= 13 && npc.type <= 15)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.7 * (double)balance * (double)bossAdjustment);
            if (npc.type == 13)
            {
                npc.damage = (int)(npc.damage * 1.1);
            }
            if (npc.type == 14)
            {
                npc.damage = (int)(npc.damage * 0.8);
            }
            if (npc.type == 15)
            {
                npc.damage = (int)(npc.damage * 0.8);
            }
            npc.scale *= 1.2f;
            npc.defense += 2;
        }
        if (npc.type == 266 || npc.type == 267)
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
        if (npc.type == 50)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.7 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 0.8);
        }
        if (npc.type == 471)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.85 * (double)(balance * 2f + 1f) / 3.0);
        }
        if (npc.type == 472)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.85 * (double)(balance + 1f) / 2.0);
            npc.damage = (int)(npc.damage * 0.8);
        }
        if (npc.type == 222)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.7 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 0.9);
        }
        if (npc.type == 210 || npc.type == 211)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75);
        }
        if (npc.type == 35)
        {
            npc.lifeMax = (int)(npc.lifeMax * balance * bossAdjustment);
            npc.damage = (int)(npc.damage * 1.1);
        }
        else if (npc.type == 36)
        {
            npc.lifeMax = (int)(npc.lifeMax * 1.3 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 1.1);
        }
        if (npc.type == 668)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.85 * (double)balance * (double)bossAdjustment);
            //npc.damage = npc.damage;
        }
        if (npc.type == 113 || npc.type == 114)
        {
            npc.defense += 6;
            npc.lifeMax = (int)(npc.lifeMax * 0.7 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 1.5);
        }
        else if (npc.type == 115)
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
        else if (npc.type == 116)
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
        else if (npc.type == 117 || npc.type == 118 || npc.type == 119)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.8);
        }
        if (npc.type == 657)
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
            if (npc.type == 134)
            {
                npc.damage *= 2;
            }
            if (npc.type == 135)
            {
                npc.damage = (int)(npc.damage * 0.85);
            }
            if (npc.type == 136)
            {
                npc.damage = (int)(npc.damage * 0.85);
            }
            npc.scale *= 1.05f;
        }
        else if (npc.type == 139)
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
        if (npc.type == 262)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.7 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 1.15);
        }
        else if (npc.type == 264)
        {
            npc.lifeMax = (int)(npc.lifeMax * balance * bossAdjustment);
            npc.damage = (int)(npc.damage * 1.15);
        }
        if (npc.type == 636)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.7 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 1.15);
        }
        if (npc.type >= 245 && npc.type <= 249)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 0.8);
        }
        if (npc.type == 370)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.65 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 0.7);
        }
        else if (npc.type == 371 || npc.type == 372 || npc.type == 373)
        {
            if (npc.type != 371)
            {
                npc.lifeMax = (int)(npc.lifeMax * 0.75);
            }
            npc.damage = (int)(npc.damage * 0.75);
        }
        if (npc.type == 439 || npc.type == 440 || (npc.type >= 454 && npc.type <= 459) || npc.type == 522 || npc.type == 523)
        {
            if (npc.type != 522)
            {
                npc.lifeMax = (int)(npc.lifeMax * 0.75f * balance * bossAdjustment);
            }
            npc.damage = (int)(npc.damage * 0.75);
        }
        if (npc.type == 397 || npc.type == 396 || npc.type == 398)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75 * (double)balance * (double)bossAdjustment);
            npc.damage = (int)(npc.damage * 0.75);
        }
        if (npc.type == 551)
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
            case 305:
            case 306:
            case 307:
            case 308:
            case 309:
            case 310:
            case 311:
            case 312:
            case 313:
            case 314:
            case 326:
            case 329:
            case 330:
                npc.lifeMax = (int)(npc.lifeMax * 0.75 * (double)num5);
                npc.damage = (int)(npc.damage * 0.75);
                break;
            case 315:
            case 325:
            case 327:
                npc.lifeMax = (int)(npc.lifeMax * 0.65 * (double)bossAdjustment);
                npc.damage = (int)(npc.damage * 0.75);
                break;
        }
        switch (npc.type)
        {
            case 338:
            case 339:
            case 340:
            case 341:
            case 342:
            case 343:
            case 347:
            case 348:
            case 349:
            case 350:
            case 351:
            case 352:
                npc.lifeMax = (int)(npc.lifeMax * 0.75 * (double)num5);
                npc.damage = (int)(npc.damage * 0.75);
                break;
            case 344:
            case 345:
            case 346:
                npc.lifeMax = (int)(npc.lifeMax * 0.65 * (double)bossAdjustment);
                npc.damage = (int)(npc.damage * 0.75);
                break;
        }
        if (Main.getGoodWorld)
        {
            if (npc.type == 6 && NPC.AnyNPCs(13))
            {
                npc.lifeMax = (int)(npc.lifeMax * 1.5 * (double)bossAdjustment);
                npc.defense += 2;
            }
            if (npc.type == 32 && NPC.AnyNPCs(35))
            {
                npc.lifeMax = (int)(npc.lifeMax * 1.5 * (double)bossAdjustment);
                npc.defense += 6;
            }
            if (npc.type == 24 && NPC.AnyNPCs(113))
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
