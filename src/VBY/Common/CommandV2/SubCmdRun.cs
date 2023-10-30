using System.ComponentModel;
using System.Reflection;
using TShockAPI;

namespace VBY.Common.CommandV2;

public class SubCmdRun : SubCmdBase
{
    internal SubCmdD RunCmd;
    internal bool DireRun { get => string.IsNullOrEmpty(ArgsHelpText) && string.IsNullOrEmpty(HelpText); }
    internal int MinArgsCount = 0;
    public string? ArgsHelpText, HelpText;
    public SubCmdRun(string cmdName, string description, SubCmdD runCmd, params string[] names) : base(cmdName, description, names)
    {
        RunCmd = runCmd;
    }
    public SubCmdRun(SubCmdD runCmd, int nameCount = 1) : this(runCmd.Method.Name.LastWord(), runCmd.Method.GetCustomAttribute<DescriptionAttribute>()?.Description ?? throw new Exception($"method:{runCmd.Method.Name}'s DescriptionAttribute is null"), runCmd,GetNames(runCmd.Method.Name.LastWord(), nameCount))
    {
    }
    internal static string[] GetNames(string name, int count)
    {
        return count switch
        {
            1 => new[] { name.ToLower() },
            2 => new[] { name.ToLower(), char.ToLower(name[0]).ToString() },
            _ => throw new NotSupportedException(),
        };
    }
    public override void Run(CommandArgs args)
    {
        if (DireRun || args.Parameters.Count >= MinArgsCount)
        {
            RunCmd.Invoke(new SubCmdArgs(CmdName, args, args.Parameters.Skip(CmdIndex).ToList()));
        }
        else
        {
            args.Player.SendInfoMessage($"参数不足,最少需要{MinArgsCount - CmdIndex}个参数");
            bool flag1 = !string.IsNullOrEmpty(ArgsHelpText);
            if (flag1) args.Player.SendInfoMessage($"/{string.Join(' ', args.Message[..args.Message.IndexOf(' ')], string.Join(' ', args.Parameters.GetRange(0, CmdIndex)))} {ArgsHelpText}");
            bool flag2 = !string.IsNullOrEmpty(HelpText);
            if (flag2) args.Player.SendInfoMessage(HelpText);
            if (!(flag1 || flag2)) args.Player.SendErrorMessage("此命令没有帮助文本!");
        }
    }
}
