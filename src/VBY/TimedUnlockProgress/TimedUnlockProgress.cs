using System.ComponentModel;

using Terraria;
using Terraria.Utilities;
using TerrariaApi.Server;
using TShockAPI;

using VBY.Common;
using VBY.Common.Config;
using VBY.Common.Hook;
using VBY.Common.Loader;

namespace VBY.TimedUnlockProgress;

[ApiVersion(2, 1)]
[Description("时间解锁NPC")]
public class TimedUnlockProgress : CommonPlugin
{
    public override string Author => "yu";
    public static ConfigManager<Config> MainConfig = new(() => new()) { PostLoadAction = PostLoad };
    public static Dictionary<int, TimeSpan> UnlockTimeInfo = new();
    public static DateTime StartedTime = DateTime.Now;
    public static HashSet<int> NoSendInfoIDs = new();
    public TimedUnlockProgress(Main game) : base(game)
    {
        new Command("tup.use", Cmd, "locktime").AddTo(this);
        new Command("tup.admin", Ctl, "tup").AddTo(this);
        ServerApi.Hooks.NpcSpawn.GetHook(this, OnNpcSpawn).AddTo(this);
        MainConfig.AddTo(this);
    }

    [AutoHook]
    public static void OnCommonCode_DropItemLocalPerClientAndSetNPCMoneyTo0(On.Terraria.GameContent.ItemDropRules.CommonCode.orig_DropItemLocalPerClientAndSetNPCMoneyTo0 orig, NPC npc, int itemId, int stack, bool interactionRequired)
    {
        if (MainConfig.Instance.AddCion && UnlockTimeInfo.TryGetValue(npc.netID, out var time) && !IsUnlocked(time))
        {
            float value = npc.value;
            orig(npc, itemId, stack, interactionRequired);
            npc.value = value;
            return;
        }
        orig(npc, itemId, stack, interactionRequired);
    }

    [AutoHook]
    public static void OnCommonCode_DropItemForEachInteractingPlayerOnThePlayer(On.Terraria.GameContent.ItemDropRules.CommonCode.orig_DropItemForEachInteractingPlayerOnThePlayer orig, NPC npc, int itemId, UnifiedRandom rng, int chanceNumerator, int chanceDenominator, int stack, bool interactionRequired)
    {
        if (MainConfig.Instance.AddCion && UnlockTimeInfo.TryGetValue(npc.netID, out var time) && !IsUnlocked(time))
        {
            float value = npc.value;
            orig(npc, itemId, rng, chanceNumerator, chanceDenominator, stack, interactionRequired);
            npc.value = value;
            return;
        }
        orig(npc, itemId, rng, chanceNumerator, chanceDenominator, stack, interactionRequired);
    }

    private void OnNpcSpawn(NpcSpawnEventArgs args)
    {
        var npc = Main.npc[args.NpcId];
        if (UnlockTimeInfo.ContainsKey(npc.netID))
        {
            var interval = DateTime.Now - StartedTime;
            if(interval < UnlockTimeInfo[npc.netID])
            {
                interval = UnlockTimeInfo[npc.netID] - interval;
                switch (MainConfig.Instance.LockType)
                {
                    case LockType.Strengthen:
                        npc.SetDefaults(npc.netID, new NPCSpawnParams { strengthMultiplierOverride = npc.strengthMultiplier * MainConfig.Instance.StrenghtValue });
                        if (MainConfig.Instance.AddCion)
                        {
                            npc.value = (int)(npc.value * MainConfig.Instance.StrenghtValue);
                        }
                        if (!NoSendInfoIDs.Contains(npc.netID))
                        {
                            TSPlayer.All.SendInfoMessage($"{Lang.GetNPCNameValue(npc.netID)} 还未到解锁时间, 已被强化, 解锁剩余时间:{interval.ToString(Utils.GetShortFormat(interval))}");
                        }
                        break;
                    default:
                        npc.active = false; 
                        if (!NoSendInfoIDs.Contains(npc.netID))
                        {
                            TSPlayer.All.SendErrorMessage($"{Lang.GetNPCNameValue(npc.netID)} 还未到解锁时间, 被禁止生成, 解锁剩余时间:{interval.ToString(Utils.GetShortFormat(interval))}");
                        }
                        args.Handled = true;
                        break;
                }
            }
        }
    }

    private static void PostLoad(Config config, TSPlayer? player, bool _)
    {
        if (string.IsNullOrEmpty(config.StartedTime))
        {
            config.StartedTime = DateTime.Now.ToString();
            MainConfig.Save();
        }
        else
        {
            if(DateTime.TryParse(config.StartedTime, out var result))
            {
                StartedTime = result;
            }
            else
            {
                player?.SendErrorMessage("时间格式错误");
            }
        }
        UnlockTimeInfo.Clear();
        foreach(var info in config.UnlockInfos)
        {
            foreach (var id in ConfigUtlis.GetIntsAsHashSet(info.IDs))
            {
                UnlockTimeInfo.Add(id, info.Time);
            }
        }
        ConfigUtlis.GetIntsAddToCollection(NoSendInfoIDs, config.NoSendInfoIDs, true);
    }
    private void Cmd(CommandArgs args)
    {
        var empty = true;
        var interval = DateTime.Now - StartedTime;
        var infos = UnlockTimeInfo.OrderBy(x => x.Value);
        var timespan = new TimeSpan();
        foreach (var info in infos)
        {
            if (!IsUnlocked(info.Value))
            {
                if (timespan == TimeSpan.Zero)
                {
                    timespan = info.Value;
                }
                if (timespan != info.Value)
                {
                    break;
                }
                var interval2 = info.Value - interval;
                args.Player.SendInfoMessage($"{Lang.GetNPCNameValue(info.Key)}({info.Key}) 剩余时间:{interval2.ToString(Utils.GetShortFormat(interval2))}");
                empty = false;
            }
        }
        if (empty)
        {
            args.Player.SendInfoMessage("全部NPC均已解锁");
        }
    }
    private void Ctl(CommandArgs args)
    {
        var enumerator = args.Parameters.GetEnumerator();
        if(!enumerator.MoveNext())
        {
            args.Player.SendInfoMessage("/tup show");
            args.Player.SendInfoMessage("/tup time [time]");
            args.Player.SendInfoMessage("/tup test <id> [id2] [..]");
            args.Player.SendInfoMessage("/tup reload");
            return;
        }
        switch (enumerator.Current) 
        {
            case "time":
                if (!enumerator.MoveNext())
                {
                    args.Player.SendInfoMessage($"当前设置开服时间:{StartedTime}");
                    break;
                }
                var time = enumerator.Current;
                DateTime setTime;
                if(time == "now")
                {
                    setTime = DateTime.Now;
                }
                else
                {
                    if(!DateTime.TryParse(time, out setTime))
                    {
                        args.Player.SendInfoMessage("时间格式错误");
                        break;
                    }
                }
                MainConfig.Instance.StartedTime = setTime.ToString();
                MainConfig.Save();
                StartedTime = setTime;
                args.Player.SendSuccessMessage($"开服时间已设置为 {setTime}");
                break;
            case "show":
                if (MainConfig.Instance.LockType == LockType.Strengthen)
                {
                    args.Player.SendInfoMessage($"锁定类型: 强化 倍数(x{MainConfig.Instance.StrenghtValue})");
                }
                else
                {
                    args.Player.SendInfoMessage($"锁定类型: 不生成");
                }
                if (!MainConfig.Instance.UnlockInfos.Any())
                {
                    args.Player.SendInfoMessage("没有锁定的NPC");
                    break;
                }
                if (enumerator.MoveNext() && enumerator.Current == "full")
                {
                    foreach (var info in MainConfig.Instance.UnlockInfos)
                    {
                        args.Player.SendInfoMessage($"间隔:{info.Time.ToString(Utils.GetMinFormat(info.Time))} 实际时间:{StartedTime.Add(info.Time)} {(IsUnlocked(info.Time) ? "已解锁" : "未解锁")}");
                        foreach (var id in ConfigUtlis.GetIntsAsHashSet(info.IDs))
                        {
                            args.Player.SendInfoMessage($"ID:{id} {Lang.GetNPCNameValue(id)}");
                        }
                    }
                }
                else
                {
                    int count = 0;
                    foreach (var info in MainConfig.Instance.UnlockInfos)
                    {
                        if (!IsUnlocked(info.Time)) 
                        {
                            count++;
                            args.Player.SendInfoMessage($"间隔:{info.Time.ToString(Utils.GetMinFormat(info.Time))} 实际时间:{StartedTime.Add(info.Time)} 未解锁");
                            args.Player.SendInfoMessage($"ID: {string.Join(", ", info.IDs)}");
                        }
                    }
                    if(count == 0)
                    {
                        args.Player.SendInfoMessage("全部均已解锁");
                    }
                }
                break;
            case "test":
                while (enumerator.MoveNext())
                {
                    if (int.TryParse(enumerator.Current, out var id))
                    {
                        if (UnlockTimeInfo.ContainsKey(id))
                        {
                            if (IsUnlocked(UnlockTimeInfo[id]))
                            {
                                args.Player.SendSuccessMessage($"ID:{id} 可生成");
                            }
                            else
                            {
                                args.Player.SendErrorMessage($"ID:{id} 不可生成 解锁时间:{StartedTime.Add(UnlockTimeInfo[id])}");
                            }
                        }
                        else
                        {
                            args.Player.SendInfoMessage($"ID:{id} 无限制");
                        }
                    }
                }
                break;
            case "reload":
                if (MainConfig.Load(args.Player))
                {
                    args.Player.SendSuccessMessage("重读成功");
                }
                break;
            default:
                args.Player.SendInfoMessage($"未知参数 '{enumerator.Current}' ");
                break;
        }
    }
    private static bool IsUnlocked(TimeSpan interval) => DateTime.Now - StartedTime >= interval;
}