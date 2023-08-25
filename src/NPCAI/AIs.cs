using System.Reflection;

namespace VBY.NPCAI;
public static partial class AIs
{
    public static void AI(NPC npc) => _NPCAIs[npc.aiStyle].Invoke(npc);
    private static readonly Type dType = typeof(Action<NPC>);
    public static void SetMethod(MethodInfo method)
    {
        int index = int.Parse(method.Name.Substring(3, 3));
        if (_tempNPCAIs[index] is null)
        {
            _tempNPCAIs[index] = _NPCAIs[index];
            _NPCAIs[index] = (Action<NPC>)Delegate.CreateDelegate(dType, method);
            //Console.WriteLine($"aiStyle:{index} add success. method:{method.Name}");
        }
    }
    public static void SetMethod(Action<NPC> action)
    {
        int index = int.Parse(action.Method.Name.Substring(3, 3));
        if (_tempNPCAIs[index] is null)
        {
            _tempNPCAIs[index] = _NPCAIs[index];
            _NPCAIs[index] = action;
            //Console.WriteLine($"aiStyle:{index} add success. method:{action.Method.Name}");
        }
    }
    public static void RemoveMethod(MethodInfo method)
    {
        int index = int.Parse(method.Name.Substring(3, 3));
        if (_NPCAIs[index].Method == method)
        {
            _NPCAIs[index] = _tempNPCAIs[index]!;
            _tempNPCAIs[index] = null; 
            //Console.WriteLine($"aiStyle:{index} remove success. method:{method.Name}");
        }
    }
    public static void RemoveMethod(Action<NPC> action)
    {
        int index = int.Parse(action.Method.Name.Substring(3, 3));
        if (_NPCAIs[index].Method == action.Method)
        {
            _NPCAIs[index] = _tempNPCAIs[index]!;
            _tempNPCAIs[index] = null;
            //Console.WriteLine($"aiStyle:{index} remove success. method:{action.Method.Name}");
        }
    }
    internal static Action<NPC>[] _NPCAIs = new Action<NPC>[126];
    internal static Action<NPC>?[] _tempNPCAIs = new Action<NPC>[126];
    static AIs()
    {
        foreach(var method in typeof(AIs).GetMethods(BindingFlags.Static | BindingFlags.Public).Where(x => x.Name.StartsWith("AI_")))
        {
            //Console.WriteLine(method.Name);
            _NPCAIs[int.Parse(method.Name["AI_".Length..])] = (Action<NPC>)Delegate.CreateDelegate(dType, method);
        }
        foreach(var method in typeof(NPC).GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(x => x.Name.StartsWith("AI_") && !x.Name["AI_000_".Length..].Contains('_') && x.GetParameters().Length == 0))
        {
            //Console.WriteLine(method.Name);
            _NPCAIs[int.Parse(method.Name.Substring("AI_".Length, 3))] ??= (Action<NPC>)Delegate.CreateDelegate(dType, method);
        }
    }
    public static void TAI(NPC npc)
    {
        if (npc.aiStyle == 0)
            npc.AI_000();
        else if (npc.aiStyle == 1)
        {
            npc.AI_001_Slimes();
        }
        else if (npc.aiStyle == 2)
        {
            npc.AI_002_FloatingEye();
        }
        else if (npc.aiStyle == 3)
        {
            npc.AI_003_Fighters();
        }
        else if (npc.aiStyle == 4)
            npc.AI_004();
        else if (npc.aiStyle == 5)
        {
            npc.AI_005_EaterOfSouls();
        }
        else if (npc.aiStyle == 6)
        {
            npc.AI_006_Worms();
        }
        else if (npc.aiStyle == 7)
        {
            npc.AI_007_TownEntities();
        }
        else if (npc.aiStyle == 8)
            npc.AI_008();
        else if (npc.aiStyle == 9)
            npc.AI_009();
        else if (npc.aiStyle == 10)
            npc.AI_010();
        else if (npc.aiStyle == 11)
            npc.AI_011();
        else if (npc.aiStyle == 12)
            npc.AI_012();
        else if (npc.aiStyle == 13)
            npc.AI_013();
        else if (npc.aiStyle == 14)
            npc.AI_014();
        else if (npc.aiStyle == 15)
            npc.AI_015();
        else if (npc.aiStyle == 16)
            npc.AI_016();
        else if (npc.aiStyle == 17)
            npc.AI_017();
        else if (npc.aiStyle == 18)
            npc.AI_018();
        else if (npc.aiStyle == 19)
            npc.AI_019();
        else if (npc.aiStyle == 20)
            npc.AI_020();
        else if (npc.aiStyle == 21)
            npc.AI_021();
        else if (npc.aiStyle == 22)
            npc.AI_022();
        else if (npc.aiStyle == 23)
            npc.AI_023();
        else if (npc.aiStyle == 24)
            npc.AI_024();
        else if (npc.aiStyle == 25)
            npc.AI_025();
        else if (npc.aiStyle == 26)
        {
            npc.AI_026_Unicorns();
        }
        else if (npc.aiStyle == 27)
            npc.AI_027();
        else if (npc.aiStyle == 28)
            npc.AI_028();
        else if (npc.aiStyle == 29)
            npc.AI_029();
        else if (npc.aiStyle == 30)
            npc.AI_030();
        else if (npc.aiStyle == 31)
            npc.AI_031();
        else if (npc.aiStyle == 32)
            npc.AI_032();
        else if (npc.aiStyle == 33)
            npc.AI_033();
        else if (npc.aiStyle == 34)
            npc.AI_034();
        else if (npc.aiStyle == 35)
            npc.AI_035();
        else if (npc.aiStyle == 36)
            npc.AI_036();
        else if (npc.aiStyle == 37)
        {
            npc.AI_037_Destroyer();
        }
        else if (npc.aiStyle == 38)
            npc.AI_038();
        else if (npc.aiStyle == 39)
            npc.AI_039();
        else if (npc.aiStyle == 40)
            npc.AI_040();
        else if (npc.aiStyle == 41)
            npc.AI_041();
        else if (npc.aiStyle == 42)
            npc.AI_042();
        else if (npc.aiStyle == 43)
            npc.AI_043();
        else if (npc.aiStyle == 44)
            npc.AI_044();
        else if (npc.aiStyle == 45)
        {
            npc.AI_045_Golem();
        }
        else if (npc.aiStyle == 46)
            npc.AI_046();
        else if (npc.aiStyle == 47)
        {
            npc.AI_047_GolemFist();
        }
        else if (npc.aiStyle == 48)
            npc.AI_048();
        else if (npc.aiStyle == 49)
            npc.AI_049();
        else if (npc.aiStyle == 50)
            npc.AI_050();
        else if (npc.aiStyle == 51)
            npc.AI_051();
        else if (npc.aiStyle == 52)
            npc.AI_052();
        else if (npc.aiStyle == 53)
            npc.AI_053();
        else if (npc.aiStyle == 54)
            npc.AI_054();
        else if (npc.aiStyle == 55)
            npc.AI_055();
        else if (npc.aiStyle == 56)
            npc.AI_056();
        else if (npc.aiStyle == 57)
            npc.AI_057();
        else if (npc.aiStyle == 58)
            npc.AI_058();
        else if (npc.aiStyle == 59)
            npc.AI_059();
        else if (npc.aiStyle == 60)
            npc.AI_060();
        else if (npc.aiStyle == 61)
            npc.AI_061();
        else if (npc.aiStyle == 62)
            npc.AI_062();
        else if (npc.aiStyle == 63)
            npc.AI_063();
        else if (npc.aiStyle == 64)
            npc.AI_064();
        else if (npc.aiStyle == 65)
        {
            npc.AI_065_Butterflies();
        }
        else if (npc.aiStyle == 66)
            npc.AI_066();
        else if (npc.aiStyle == 67)
            npc.AI_067();
        else if (npc.aiStyle == 68)
            npc.AI_068();
        else if (npc.aiStyle == 69)
        {
            npc.AI_069_DukeFishron();
        }
        else if (npc.aiStyle == 70)
            npc.AI_070();
        else if (npc.aiStyle == 71)
            npc.AI_071();
        else if (npc.aiStyle == 72)
            npc.AI_072();
        else if (npc.aiStyle == 73)
            npc.AI_073();
        else if (npc.aiStyle == 74)
            npc.AI_074();
        else if (npc.aiStyle == 75)
            npc.AI_075();
        else if (npc.aiStyle == 76)
            npc.AI_076();
        else if (npc.aiStyle == 77)
            npc.AI_077();
        else if (npc.aiStyle == 78)
            npc.AI_078();
        else if (npc.aiStyle == 79)
            npc.AI_079();
        else if (npc.aiStyle == 80)
            npc.AI_080();
        else if (npc.aiStyle == 81)
            npc.AI_081();
        else if (npc.aiStyle == 82)
            npc.AI_082();
        else if (npc.aiStyle == 83)
            npc.AI_083();
        else if (npc.aiStyle == 84)
        {
            npc.AI_084_LunaticCultist();
        }
        else if (npc.aiStyle == 85)
            npc.AI_085();
        else if (npc.aiStyle == 86)
            npc.AI_086();
        else if (npc.aiStyle == 87)
            npc.AI_087();
        else if (npc.aiStyle == 88)
            npc.AI_088();
        else if (npc.aiStyle == 89)
            npc.AI_089();
        else if (npc.aiStyle == 90)
            npc.AI_090();
        else if (npc.aiStyle == 91)
            npc.AI_091();
        else if (npc.aiStyle == 92)
            npc.AI_092();
        else if (npc.aiStyle == 93)
            npc.AI_093();
        else if (npc.aiStyle == 94)
            npc.AI_094();
        else if (npc.aiStyle == 95)
            npc.AI_095();
        else if (npc.aiStyle == 96)
            npc.AI_096();
        else if (npc.aiStyle == 97)
            npc.AI_097();
        else if (npc.aiStyle == 98)
            npc.AI_098();
        else if (npc.aiStyle == 99)
            npc.AI_099();
        else if (npc.aiStyle == 100)
            npc.AI_100();
        else if (npc.aiStyle == 101)
            npc.AI_101();
        else if (npc.aiStyle == 102)
            npc.AI_102();
        else if (npc.aiStyle == 103)
            npc.AI_103();
        else if (npc.aiStyle == 104)
        {
            npc.AI_104();
        }
        else if (npc.aiStyle == 105)
            npc.AI_105();
        else if (npc.aiStyle == 106)
            npc.AI_106();
        else if (npc.aiStyle == 107)
        {
            npc.AI_107_ImprovedWalkers();
        }
        else if (npc.aiStyle == 108)
        {
            npc.AI_108_DivingFlyer();
        }
        else if (npc.aiStyle == 109)
        {
            npc.AI_109_DarkMage();
        }
        else if (npc.aiStyle == 110)
        {
            npc.AI_110_Betsy();
        }
        else if (npc.aiStyle == 111)
        {
            npc.AI_111_DD2LightningBug();
        }
        else if (npc.aiStyle == 112)
        {
            npc.AI_112_FairyCritter();
        }
        else if (npc.aiStyle == 113)
        {
            npc.AI_113_WindyBalloon();
        }
        else if (npc.aiStyle == 114)
        {
            npc.AI_114_Dragonflies();
        }
        else if (npc.aiStyle == 115)
        {
            npc.AI_115_LadyBugs();
        }
        else if (npc.aiStyle == 116)
        {
            npc.AI_116_WaterStriders();
        }
        else if (npc.aiStyle == 117)
        {
            npc.AI_117_BloodNautilus();
        }
        else if (npc.aiStyle == 118)
        {
            npc.AI_118_Seahorses();
        }
        else if (npc.aiStyle == 119)
        {
            npc.AI_119_Dandelion();
        }
        else if (npc.aiStyle == 120)
        {
            npc.AI_120_HallowBoss();
        }
        else if (npc.aiStyle == 121)
        {
            npc.AI_121_QueenSlime();
        }
        else if (npc.aiStyle == 122)
        {
            npc.AI_122_PirateGhost();
        }
        else if (npc.aiStyle == 123)
        {
            npc.AI_123_Deerclops();
        }
        else if (npc.aiStyle == 124)
        {
            npc.AI_124_ElderSlimeChest();
        }
        else if (npc.aiStyle == 125)
        {
            npc.AI_125_ClumsySlimeBalloon();
        }
    }
}