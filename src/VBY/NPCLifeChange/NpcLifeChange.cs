using System.ComponentModel;

using Terraria;
using Terraria.ID;
using TerrariaApi.Server;

using TShockAPI;

using VBY.Common;
using VBY.Common.Config;
using VBY.Common.Hook;
using VBY.Common.Loader;

namespace VBY.NpcLifeChange;

[ApiVersion(2, 1)]
[Description("NPC血量和强化倍数修改")]
public class NpcLifeChange : CommonPlugin
{
    public override string Author => "yu";
    public ConfigManager<MainConfig> MainConfig = new("Config", $"{typeof(NpcLifeChange).Namespace}.json", () => new()) { PostLoadAction = MainPostLoadAction };

    public static ChangeInfo LifeChangeInfo = new();
    public static ChangeInfo StrengthChangeInfo = new();
    private static bool[] NotChangeNPC = new bool[NPCID.Count];
    internal static Func<NPC, bool> NotChangeFunc = Utils.DefaultNotChange;
    public NpcLifeChange(Main game) : base(game)
    {
        new Command("vby.npclife", Cmd, "npclife").AddTo(this);
        MainConfig.AddTo(this);
    }
    #region On
    [AutoHook]
    private static int OnNPC_NewNPC(On.Terraria.NPC.orig_NewNPC orig, Terraria.DataStructures.IEntitySource source, int X, int Y, int Type, int Start, float ai0, float ai1, float ai2, float ai3, int Target)
    {
        var index = orig(source, X, Y, Type, Start, ai0, ai1, ai2, ai3, Target);
        var self = Main.npc[index];
        if (!NotChangeNPC[self.type] && !NotChangeFunc(self))
        {
            self.SetDefaults(Type, new NPCSpawnParams() { strengthMultiplierOverride = StrengthChangeInfo.GetValue(self.strengthMultiplier) });
            self.lifeMax = (int)LifeChangeInfo.GetValue(self.lifeMax);
            self.life = self.lifeMax;
        }
        return index;
    }
    private static void MainPostLoadAction(MainConfig config, TSPlayer? player, bool first)
    {
        if (ChangeInfo.TryParse(config.Life, out var life))
        {
            LifeChangeInfo = life;
        }
        else
        {
            player?.SendInfoMessage("Life 格式转换错误");
        }
        if (ChangeInfo.TryParse(config.Strength, out var strenght))
        {
            StrengthChangeInfo = strenght;
        }
        else
        {
            player?.SendInfoMessage("Strength 格式转换错误");
        }
        Utils.SetBools(NotChangeNPC, config.SkipNPCIDs);
        Utils.SetNotChangeFunc(config.SkipNPCProperty);
    }
    #endregion
    private static void Cmd(CommandArgs args)
    {
        if (!args.Parameters.Any())
        {
            args.Player.SendInfoMessage("/npclife show");
            args.Player.SendInfoMessage("/npclife set <type(str,life)> <value>");
            return;
        }
        switch (args.Parameters[0])
        {
            case "show":
                args.Player.SendInfoMessage($"life:{LifeChangeInfo.Type} {LifeChangeInfo.Value}");
                args.Player.SendInfoMessage($"Strenght:{StrengthChangeInfo.Type} {StrengthChangeInfo.Value}");
                break;
            case "set":
                if(args.Parameters.Count < 3)
                {
                    args.Player.SendInfoMessage("/npclife set <type> <value>");
                    return;
                }
                switch (args.Parameters[1])
                {
                    case "life":
                        {
                            if (ChangeInfo.TryParse(args.Parameters[2], out var changeInfo))
                            {
                                LifeChangeInfo = changeInfo;
                                args.Player.SendInfoMessage($"life:{LifeChangeInfo.Type} {LifeChangeInfo.Value}");
                            }
                            else
                            {
                                args.Player.SendErrorMessage("错误格式");
                            }
                        }
                        break;
                    case "str":
                        {
                            if (ChangeInfo.TryParse(args.Parameters[2], out var changeInfo))
                            {
                                StrengthChangeInfo = changeInfo;
                                args.Player.SendInfoMessage($"Strenght:{StrengthChangeInfo.Type} {StrengthChangeInfo.Value}");
                            }
                            else
                            {
                                args.Player.SendErrorMessage("错误格式");
                            }
                        }
                        break;
                    default:
                        args.Player.SendInfoMessage("未知type");
                        break;
                }
                break;
            default:
                args.Player.SendInfoMessage($"未知命令 {args.Parameters[0]}");
                break;
        }
    }
}
