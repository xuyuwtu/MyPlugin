using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;

using Terraria;
using TerrariaApi.Server;

using TShockAPI;

using VBY.Common;
using VBY.Common.CommandV2;
using VBY.Common.Extension;

namespace VBY.ProgressQuery;

[ApiVersion(2, 1)]
[Description("进度查询")]
public class ProgressQuery : CommonPlugin
{
    public override string Author => "yu";
    public static Func<bool>[] BossFuncs, EventFuncs;
    public static Action<bool>[] BossActions, EventActions;
    private static readonly string[] DefaultBossNames = new[] {"未知", "史莱姆王", "克苏鲁之眼", "邪恶Boss", "蜂王", "骷髅王", "独眼巨鹿", "血肉墙", "史莱姆皇后",
                                     "任意机械BOSS", "毁灭者", "双子魔眼", "机械骷髅王", "世纪之花", "石巨人", "猪龙鱼公爵", "光之女皇", "拜月教邪教徒",
                                     "日耀柱", "星云柱", "星旋柱", "星尘柱", "月亮领主", "哀木", "南瓜王", "常绿尖叫怪", "圣诞坦克", "冰雪女王"};
    private static readonly string[] DefaultEventNames = new[] { "未知", "哥布林军队", "雪人军团", "海盗入侵", "南瓜月", "霜月", "火星暴乱" };
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
            () => NPC.downedFrost,
            () => NPC.downedPirates,
            () => DownedChristmas,
            () => DownedHelloween,
            () => NPC.downedMartians
        };
        BossActions = new Action<bool>[BossFuncs.Length];
        EventActions = new Action<bool>[EventFuncs.Length];
        BossActions[0] = EventActions[0] = x => { };
        var type = typeof(ProgressQuery);
        var module = type.Module;
        foreach (var fa in new (string type, Func<bool>[] func, Action<bool>[] action)[] { ("Boss", BossFuncs, BossActions), ("Event", EventFuncs, EventActions) })
        {
            for (int i = 1; i < fa.func.Length; i++)
            {
                byte[] ilarray = fa.func[i].Method.GetMethodBody()!.GetILAsByteArray()!;
                var dm = new DynamicMethod($"{fa.type}Action{i}", null, new Type[] { typeof(bool) });
                var il = new Common.Emit.ILGeneratorClass(dm);
                il.Ldarg(0);
                if (ilarray[0] == OpCodes.Call.Value)
                {
                    il.Call(type.GetMethod(string.Concat("set_", module.ResolveMethod(ilarray, 1)!.Name.AsSpan(4)))!);
                }
                else if (ilarray[0] == OpCodes.Ldsfld.Value)
                {
                    il.Stsfld(module.ResolveField(ilarray, 1)!);
                }

                il.Ret();
                fa.action[i] = dm.CreateDelegate<Action<bool>>(null);
            }
        }
    }
    public ProgressQuery(Main game) : base(game)
    {
        CmdCommand = new("Progress")
        {
            new SubCmdRun(CmdAll, 2),
            new SubCmdRun(CmdBoss, 2),
            new SubCmdRun(CmdEvent, 2)
        };
        CmdCommand.DefaultCmd = "all";
        CtlCommand = new("Progressctl")
        {
            new SubCmdRun(CtlBoss, 2),
            new SubCmdRun(CtlEvent, 2),
            new SubCmdRun(CtlReset)
        };

        var typeName = GetType().Name;
        ReadConfig = new("Config", Name + ".json")
        {
            Root = new Root()
            {
                Commands = new()
                {
                    Use = new(typeName.ToLower() + "." + "use", "pg"),
                    Admin = new(typeName.ToLower() + "." + "admin", "pgc")
                },
                Texts = new()
                {
                    BossNames = DefaultBossNames.Skip(1).ToArray(),
                    EventNames = DefaultEventNames.Skip(1).ToArray()
                }
            }
        };
        ReadConfig.Read(true);
        AddCommands.AddRange(ReadConfig.Root.Commands.GetCommands(CmdCommand.Run, CtlCommand.Run));
        CmdCommand.SetAllNode(new SetAllowInfo(null, true, null));
        CtlCommand.SetAllNode(new SetAllowInfo(null, true, null));
        Array.Copy(ReadConfig.Root.Texts.BossNames, 0, BossNames, 1, Math.Min(ReadConfig.Root.Texts.BossNames.Length, BossNames.Length));
        Array.Copy(ReadConfig.Root.Texts.EventNames, 0, EventNames, 1, Math.Min(ReadConfig.Root.Texts.EventNames.Length, EventNames.Length));
    }
    [Description("全部")]
    public static void CmdAll(SubCmdArgs args)
    {
        args.Player.SendInfoMessage("Boss:");
        var getInfo = Enumerable.Range(1, BossFuncs.Length - 1).Select(x => (name: BossNames[x], downed: BossFuncs[x]())).ToArray();
        var downedInfo = getInfo.Where(x => x.downed).ToArray();
        if(downedInfo.Length == 0)
        {
            args.Player.SendErrorMessage("没有击败任何Boss");
        }
        else
        {
            if(downedInfo.Length == getInfo.Length)
            {
                args.Player.SendSuccessMessage("全部Boss均已击败");
            }
            else
            {
                args.Player.SendSuccessMessage($"已击败Boss:{string.Join(", ", downedInfo.Select(x => x.name))}");
                args.Player.SendErrorMessage($"未击败Boss:{string.Join(", ", getInfo.Where(x => !x.downed).Select(x => x.name))}");
            }
        }
        args.Player.SendInfoMessage("事件:");
        getInfo = Enumerable.Range(1, EventFuncs.Length - 1).Select(x => (name: EventNames[x], downed: EventFuncs[x]())).ToArray();
        downedInfo = getInfo.Where(x => x.downed).ToArray();
        if (downedInfo.Length == 0)
        {
            args.Player.SendErrorMessage("没有击败任何事件");
        }
        else
        {
            if (downedInfo.Length == getInfo.Length)
            {
                args.Player.SendSuccessMessage("全部事件均已击败");
            }
            else
            {
                args.Player.SendSuccessMessage($"已击败事件:{string.Join(", ", downedInfo.Select(x => x.name))}");
                args.Player.SendErrorMessage($"未击败事件:{string.Join(", ", getInfo.Where(x => !x.downed).Select(x => x.name))}");
            }
        }
    }
    [Description("Boss")]
    public static void CmdBoss(SubCmdArgs args) => Enumerable.Range(1, BossFuncs.Length - 1).ForEach(x => SendDownedInfo(args.Player, x, BossNames[x], BossFuncs[x](), ListExt.ElementAtOrDefault(args.Parameters, 0, "y") == "n"));
    [Description("事件")]
    public static void CmdEvent(SubCmdArgs args) => Enumerable.Range(1, EventFuncs.Length - 1).ForEach(x => SendDownedInfo(args.Player, x, EventNames[x], EventFuncs[x](), ListExt.ElementAtOrDefault(args.Parameters, 0, "y") == "n"));
    [Description("切换Boss击败")]
    public static void CtlBoss(SubCmdArgs args)
        => args.Parameters.Where(x => byte.TryParse(x, out var index) && index > 0 && index < BossFuncs.Length).ForEach(x => ChangeDownedInfo(args.Player, byte.Parse(x), "Boss", BossActions, BossFuncs, BossNames));
    [Description("切换事件击败")]
    public static void CtlEvent(SubCmdArgs args)
        => args.Parameters.Where(x => byte.TryParse(x, out var index) && index > 0 && index < EventFuncs.Length).ForEach(x => ChangeDownedInfo(args.Player, byte.Parse(x), "事件", EventActions, EventFuncs, EventNames));
    [Description("清除全部击败")]
    public static void CtlReset(SubCmdArgs args)
    {
        Main.hardMode = false;
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
                {
                    player.SendSuccessMessage($"{$"[{index}]",4} {name} 已击败");
                }
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
        {
            player.SendSuccessMessage($"{type}: {names[index]} 的击败信息已切换为 {(funcs[index]() ? "已" : "未")}击败");
        }
        else
        {
            player.SendSuccessMessage($"{type}: {$"{names[index],-8}"} 的击败信息已切换为 {(funcs[index]() ? "已" : "未")}击败");
        }
    }
    internal static bool DownedCheck(string str, Func<bool>[] checkedFuncs)
    {
        if (string.IsNullOrEmpty(str))
        {
            return true;
        }

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
                {
                    result = !result;
                }
            }
            if (!result)
            {
                break;
            }
        }
        return result;
    }
    public static bool BossCheck(string str) => DownedCheck(str, BossFuncs);
    public static bool EventCheck(string str) => DownedCheck(str, EventFuncs);
    internal static string DownedCheckToString(string str, string[] checkedNames)
    {
        if (string.IsNullOrEmpty(str))
        {
            return "无";
        }

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
            {
                checklist[i] = '-' + checklist[i];
            }
        }
        return string.Join(" 和 ", checklist);
    }
    public static string BossCheckToString(string str) => DownedCheckToString(str, BossNames);
    public static string EventCheckToString(string str) => DownedCheckToString(str, EventNames);
}