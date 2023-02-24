using System.Reflection;
using System.Reflection.Emit;

using Terraria;
using TerrariaApi.Server;

using TShockAPI;

using VBY.Basic.Command;
using VBY.Basic.Extension;

namespace VBY.ProgressQuery;

[ApiVersion(2, 1)]
public class ProgressQuery : TerrariaPlugin
{
    public override string Name => GetType().Name!;
    public override string Description => "进度查询";
    public override string Author => "yu";
    public override Version Version => GetType().Assembly.GetName().Version!;
    public static Func<bool>[] BossFuncs, EventFuncs;
    public static Action<bool>[] BossActions, EventActions;
    private static readonly string[] DefaultBossNames = new[] {"未知", "史莱姆王", "克苏鲁之眼", "世吞或脑子", "蜂王", "骷髅王", "独眼巨鹿", "血肉墙", "史莱姆皇后",
                                     "任意机械BOSS", "毁灭者", "双子魔眼", "机械骷髅王", "世纪之花", "石巨人", "猪龙鱼公爵", "光之女皇", "拜月教邪教徒",
                                     "日耀柱", "星云柱", "星旋柱", "星尘柱", "月亮领主", "哀木", "南瓜王", "常绿尖叫怪", "圣诞坦克", "冰雪女王"};
    private static readonly string[] DefaultEventNames = new[] { "未知", "哥布林军队", "海盗入侵", "南瓜月", "霜月", "火星暴乱" };
    public static readonly string[] BossNames = (string[])DefaultBossNames.Clone();
    public static readonly string[] EventNames = (string[])DefaultEventNames.Clone();
    public static bool DownedChristmas
    {
        get => NPC.downedChristmasTree && NPC.downedChristmasSantank && NPC.downedChristmasIceQueen;
        set
        {
            NPC.downedChristmasTree = value;
            NPC.downedChristmasSantank = value;
            NPC.downedChristmasIceQueen = value;
        }
    }
    public static bool DownedHelloween
    {
        get => NPC.downedHalloweenTree && NPC.downedHalloweenKing;
        set
        {
            NPC.downedHalloweenTree = value;
            NPC.downedHalloweenKing = value;
        }
    }
    public Config ReadConfig;
    public SubCmdRoot CmdCommand, CtlCommand;
    public Command[] AddCommands;
    static ProgressQuery()
    {
        BossFuncs = new Func<bool>[]
        {
            () => true,
            () => NPC.downedSlimeKing,
            () => NPC.downedBoss1,
            () => NPC.downedBoss2,
            () => NPC.downedQueenBee,
            () => NPC.downedBoss3,
            () => NPC.downedDeerclops,
            () => Main.hardMode,
            () => NPC.downedQueenSlime,
            () => NPC.downedMechBossAny,
            () => NPC.downedMechBoss1,
            () => NPC.downedMechBoss2,
            () => NPC.downedMechBoss3,
            () => NPC.downedPlantBoss,
            () => NPC.downedGolemBoss,
            () => NPC.downedFishron,
            () => NPC.downedEmpressOfLight,
            () => NPC.downedAncientCultist,
            () => NPC.downedTowerSolar,
            () => NPC.downedTowerNebula,
            () => NPC.downedTowerVortex,
            () => NPC.downedTowerStardust,
            () => NPC.downedMoonlord,
            () => NPC.downedHalloweenTree,
            () => NPC.downedHalloweenKing,
            () => NPC.downedChristmasTree,
            () => NPC.downedChristmasSantank,
            () => NPC.downedChristmasIceQueen
        };
        EventFuncs = new Func<bool>[]
        {
            () => true,
            () => NPC.downedGoblins,
            () => NPC.downedPirates,
            () => DownedChristmas,
            () => DownedHelloween,
            () => NPC.downedMartians
        };
        BossActions = new Action<bool>[BossFuncs.Length];
        EventActions = new Action<bool>[EventFuncs.Length];
        BossActions[0] = EventActions[0] = x => { return; };
        var type = typeof(ProgressQuery);
        var module = type.Module;
        var propertys = new PropertyInfo[] { type.GetProperty(nameof(DownedChristmas))!, type.GetProperty(nameof(DownedHelloween))! };
        foreach (var fa in new (string type, Func<bool>[] func, Action<bool>[] action)[] { ("Boss", BossFuncs, BossActions), ("Event", EventFuncs, EventActions) })
        {
            for (int i = 1; i < fa.func.Length; i++)
            {
                byte[] ilar = fa.func[i].Method.GetMethodBody()!.GetILAsByteArray()!;
                var dm = new DynamicMethod($"{fa.type}Action{i}", null, new Type[] { typeof(bool) });
                var il = dm.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                if (ilar[0] == OpCodes.Call.Value)
                    il.Emit(OpCodes.Call, type.GetMethod(string.Concat("set_", module.ResolveMethod(BitConverter.ToInt32(ilar, 1))!.Name.AsSpan(4)))!);
                else if (ilar[0] == OpCodes.Ldsfld.Value)
                    il.Emit(OpCodes.Stsfld, module.ResolveField(BitConverter.ToInt32(ilar, 1))!);
                il.Emit(OpCodes.Ret);
                fa.action[i] = dm.CreateDelegate<Action<bool>>(null);
            }
        }
    }
    public ProgressQuery(Main game) : base(game)
    {
        CmdCommand = new("Progress");
        CtlCommand = new("Progressctl");
        CmdCommand.AddCmd(CmdBoss, "Boss");
        CmdCommand.AddCmd(CmdEvent, "事件");
        CtlCommand.AddCmd(CtlBoss, "切换Boss击败");
        CtlCommand.AddCmd(CtlEvent, "切换事件击败");
        CtlCommand.AddCmd(CtlReset, "清除全部击败");
        var typeName = GetType().Name;
        ReadConfig = new(TShock.SavePath, GetType().Name + ".json")
        {
            Root = new Root()
            {
                Commands = new()
                {
                    Use = new(typeName.ToLower() + "." + "use", "pg"),
                    Admin = new(typeName.ToLower() + "." + "admin", "pgc")
                },
                文本 = new()
                {
                    Boss名称 = DefaultBossNames.Skip(1).ToArray(),
                    事件名称 = DefaultEventNames.Skip(1).ToArray()
                }
            }
        };
        ReadConfig.Read(true);
        AddCommands = ReadConfig.Root.Commands.GetCommands(CmdCommand, CtlCommand);
        Array.Copy(ReadConfig.Root.文本.Boss名称, 0, BossNames, 1, Math.Min(ReadConfig.Root.文本.Boss名称.Length, BossNames.Length));
        Array.Copy(ReadConfig.Root.文本.事件名称, 0, EventNames, 1, Math.Min(ReadConfig.Root.文本.事件名称.Length, EventNames.Length));
    }
    public override void Initialize()
    {
        Commands.ChatCommands.AddRange(AddCommands);
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Commands.ChatCommands.RemoveRange(AddCommands);
        }
        base.Dispose(disposing);
    }
    public static void CmdBoss(SubCmdArgs args) => Enumerable.Range(1, BossFuncs.Length - 1).ForEach(x => SendDownedInfo(args.Player, x, BossNames[x], BossFuncs[x](), args.Parameters.GetIndexOrValue(0, "y") == "n"));
    public static void CmdEvent(SubCmdArgs args) => Enumerable.Range(1, EventFuncs.Length - 1).ForEach(x => SendDownedInfo(args.Player, x, EventNames[x], EventFuncs[x](), args.Parameters.GetIndexOrValue(0, "y") == "n"));
    public static void CtlBoss(SubCmdArgs args)
        => args.Parameters.Where(x => byte.TryParse(x, out var index) && index > 0 && index < BossFuncs.Length).ForEach(x => ChangeDownedInfo(args.Player, byte.Parse(x), "Boss", BossActions, BossFuncs, BossNames));
    public static void CtlEvent(SubCmdArgs args)
        => args.Parameters.Where(x => byte.TryParse(x, out var index) && index > 0 && index < EventFuncs.Length).ForEach(x => ChangeDownedInfo(args.Player, byte.Parse(x), "事件", EventActions, EventFuncs, EventNames));
    public static void CtlReset(SubCmdArgs args)
    {
        typeof(NPC).GetFields(BindingFlags.Static | BindingFlags.Public)
            .Where(x => x.Name.StartsWith("downed"))
            .ForEach(x => x.SetValue(null, false));
        TSPlayer.All.SendData(PacketTypes.WorldInfo);
        args.Player.SendSuccessMessage("[Progressctl.Reset]全部击败信息已切换为未击败");
    }
    public static void SendDownedInfo(TSPlayer player, int index, string name, bool downed, bool noSendDowned)
    {
        if (player.RealPlayer)
        {
            player.SendMessage($"[C/FFFF00:[{index}][C/FFFF00:]] [C/FFFF00:{name}] [C/{(downed ? "00FF" : "FF00")}00:{(downed ? "已" : "未")}击败]", Microsoft.Xna.Framework.Color.White);
        }
        else
        {
            if (downed)
            {
                if (!noSendDowned)
                    player.SendSuccessMessage($"{$"[{index}]",4} {name} 已击败");
            }
            else
            {
                player.SendErrorMessage($"{$"[{index}]",4} {name} 未击败");
            }
        }
    }
    public static void ChangeDownedInfo(TSPlayer player, byte index, string type, Action<bool>[] actions, Func<bool>[] funcs, string[] names)
    {
        actions[index](!funcs[index]());
        if (player.RealPlayer)
            player.SendSuccessMessage($"{type}: {names[index]} 的击败信息已切换为 {(funcs[index]() ? "已" : "未")}击败");
        else
            player.SendSuccessMessage($"{type}: {$"{names[index],-8}"} 的击败信息已切换为 {(funcs[index]() ? "已" : "未")}击败");
    }
    internal static bool DownedCheck(string str, Func<bool>[] checkedFuncs)
    {
        if (string.IsNullOrEmpty(str))
            return true;
        var result = false;
        string[] checklist = str.Contains(',') ? str.Split(',', StringSplitOptions.RemoveEmptyEntries) : new string[] { str };
        foreach (var item in checklist)
        {
            if (item.Contains('|'))
            {
                foreach (var item2 in item.Split('|', StringSplitOptions.RemoveEmptyEntries))
                {
                    if (DownedCheck(item2, checkedFuncs))
                    {
                        result = true;
                        break;
                    }
                }
            }
            else
            {
                var flag = item[0] == '-';
                result = int.TryParse(flag ? item.Remove(0, 1) : item, out int index) && index >= 0 && index < checkedFuncs.Length && checkedFuncs[index]();
                if (flag)
                    result = !result;
            }
            if (!result)
                break;
        }
        return result;
    }
    public static bool BossCheck(string str) => DownedCheck(str, BossFuncs);
    public static bool EvnetCheck(string str) => DownedCheck(str, EventFuncs);
    internal static string DownedCheckToString(string str, string[] checkedNames)
    {
        if (string.IsNullOrEmpty(str))
            return "无";
        string[] checklist = str.Contains(',') ? str.Split(',', StringSplitOptions.RemoveEmptyEntries) : new string[] { str };
        for (int i = 0; i < checklist.Length; i++)
        {
            var item = checklist[i];
            var flag = item[0] == '-';
            if (item.Contains('|'))
            {
                var checklist2 = item.Split('|', StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < checklist2.Length; j++)
                {
                    checklist2[j] = DownedCheckToString(checklist2[j], checkedNames);
                }
                checklist[i] = $"({string.Join(" 或 ", checklist2)})";
            }
            else
            {
                checklist[i] = int.TryParse(item, out int index) && index >= 0 && index < checkedNames.Length ? checkedNames[index] : "未知";
            }
            if (flag)
                checklist[i] = '-' + checklist[i];
        }
        return string.Join(" 和 ", checklist);
    }
    public static string BossCheckToString(string str) => DownedCheckToString(str, BossNames);
    public static string EventCheckToString(string str) => DownedCheckToString(str, EventNames);
}