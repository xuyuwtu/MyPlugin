using System.ComponentModel;
using System.Reflection;

using TShockAPI;

using Newtonsoft.Json.Linq;

namespace VBY.Common.Command;

public class SubCmdList : SubCmdBase
{
    public List<SubCmdBase> SubCmds = new();
    public string DefaultCmd = "";
    internal SubCmdList(string cmdName, string description, params string[] names) : base(cmdName, description, names) { }
    public SubCmdBase this[string name]
    {
        get
        {
            SubCmdBase result = this;
            if (name.Contains('.'))
            {
                return ((SubCmdList)result)[name.Split('.', StringSplitOptions.RemoveEmptyEntries)];
            }
            else
            {
                result = SubCmds.Find(x => x.CmdName == name)!;
                if (result is null)
                    throw new Exception($"{FullCmdName}'s SubCmd '{name}' not find");
            }
            return result;
        }
    }
    public SubCmdBase this[string[] names, int start = 0, int reduce = 0]
    {
        get
        {
            SubCmdBase result = this;
            for (int i = start; i < names.Length - reduce; i++)
                result = ((SubCmdList)result)[names[i]];
            return result;
        }
    }
    private bool DefaultCmdIsNullOrEmpty(CommandArgs args)
    {
        var isNull = string.IsNullOrEmpty(DefaultCmd);
        if (!isNull)
        {
            args.Parameters.Add(DefaultCmd);
        }
        return isNull;
    }
    public override void Run(CommandArgs args)
    {
        if (args.Parameters.Count == CmdIndex && DefaultCmdIsNullOrEmpty(args))
        {
            args.Player.SendInfoMessage($"/{args.Message.Trim()} 的子命令");
            bool have = false;
            foreach (var cmd in SubCmds)
            {
                if (cmd.Enabled)
                {
                    args.Player.SendInfoMessage($"{cmd.Names[0]} {cmd.Description}");
                    have = true;
                }
            }
            if (!have)
            {
                args.Player.SendInfoMessage($"好像没有可用子命令,问问腐竹是不是配错了");
            }
        }
        else
        {
            string findText = args.Parameters[CmdIndex];
            bool found = false;
            foreach (var cmd in SubCmds)
            {
                if (cmd.Enabled && cmd.Names.Contains(findText))
                {
                    found = true;
                    if (cmd.NotCanRun(args.Player)) break;
                    cmd.Run(args);
                    break;
                }
            }
            if (!found)
            { 
                args.Player.SendInfoMessage($"未知参数 {findText}");
            }
        }
    }
    internal override void OutCmdRun(CommandArgs args, int outCount)
    {
        if (NotCanRun(args.Player)) return;
        if (args.Parameters.Count == CmdIndex - outCount)
        {
            args.Player.SendInfoMessage($"/{args.Message.Trim()} 的子命令");
            bool have = false;
            foreach (var cmd in SubCmds)
            {
                if (cmd.Enabled)
                {
                    args.Player.SendInfoMessage($"/{cmd.Names[0]} {cmd.Description}");
                    have = true;
                }
            }
            if (!have) args.Player.SendInfoMessage($"好像没有可用子命令,问问腐竹是不是配错了");
        }
        else
        {
            string findText = args.Parameters[CmdIndex - outCount];
            bool found = false;
            foreach (var cmd in SubCmds)
            {
                if (cmd.Enabled && cmd.Names.Contains(findText))
                {
                    cmd.OutCmdRun(args, outCount);
                    found = true;
                }
            }
            if (!found) args.Player.SendInfoMessage($"未知参数 {findText}");
        }
    }
    internal void AddNode(SubCmdBase addNode)
    {
        addNode.CmdIndex = CmdIndex + 1;
        addNode.FullCmdName = FullCmdName + "." + addNode.CmdName;
        addNode.Parent = this;
        if (addNode is SubCmdRun runcmd)
            runcmd.MinArgsCount += addNode.CmdIndex;
        SubCmds.Add(addNode);
    }
    public SubCmdList AddList(string cmdName, string description, params string[] names)
    {
        var addNode = new SubCmdList(cmdName, description, names);
        AddNode(addNode);
        return addNode;
    }
    public SubCmdList AddList(string cmdName, string description, int nameCount = 1) =>
        AddList(cmdName, description, GetNames(cmdName, nameCount));
    public void AddLists(params (string cmdName, string description)[] values)
    {
        foreach (var value in values)
            AddList(value.cmdName, value.description);
    }
    public SubCmdRun AddCmd(SubCmdD runCmd, string cmdName, string description, string[] names, string? argsHelpText, string? helpText, int minArgsCount = 0)
    {
        var addNode = new SubCmdRun(runCmd, cmdName, description, names, argsHelpText, helpText, minArgsCount);
        AddNode(addNode);
        return addNode;
    }
    public SubCmdRun AddCmd(SubCmdD runCmd, string cmdName, string description, string? argsHelpText, string? helpText, int minArgsCount, int nameCount = 1)
    {
        return AddCmd(runCmd, cmdName, description, GetNames(cmdName, nameCount), argsHelpText, helpText, minArgsCount);
    }
    public SubCmdRun AddCmd(SubCmdD runCmd, string description, string? argsHelpText, string? helpText, int minArgsCount, int nameCount = 1)
    {
        return AddCmd(runCmd, runCmd.Method.Name.LastWord(), description, argsHelpText, helpText, minArgsCount, nameCount);
    }
    public SubCmdRun AddCmd(SubCmdD runCmd, string description, string argsHelpText, string? helpText, int nameCount = 1)
    {
        return AddCmd(runCmd, description, argsHelpText, helpText, argsHelpText.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length, nameCount);
    }
    public SubCmdRun AddCmd(SubCmdD runCmd, string description, string argsHelpText, int nameCount = 1)
    {
        return AddCmd(runCmd, description, argsHelpText, null, nameCount);
    }
    public SubCmdRun AddCmd(SubCmdD runCmd, string description, int nameCount = 1)
    {
        return AddCmd(runCmd, description, null, null, 0, nameCount);
    }
    public SubCmdRun AddCmd(SubCmdD runCmd, int nameCount = 1)
    {
        return AddCmd(runCmd, runCmd.Method.GetCustomAttribute<DescriptionAttribute>()?.Description ?? throw new Exception($"method:{runCmd.Method.Name}'s DescriptionAttribute is null"), nameCount);
    }
    public void AddCmds(int nameCount, params SubCmdD[] cmds)
    {
        foreach (var cmd in cmds)
            AddCmd(cmd, nameCount);
    }
    public SubCmdRun AddCmd(Delegate runCmd, string cmdName, string description, string[] names, string? helpText)
    {
        var addNode = new SubCmdRun(runCmd, cmdName, description, names, helpText);
        AddNode(addNode);
        return addNode;
    }
    public SubCmdRun AddCmdA(SubCmdD runCmd, string cmdName, string description, string[] names, string argsHelpText, string? helpText = null)
    {
        var addNode = new SubCmdRun(runCmd, cmdName, description, names, argsHelpText, helpText, argsHelpText.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length);
        AddNode(addNode);
        return addNode;
    }
    public SubCmdRun AddCmdA(SubCmdD runCmd, string cmdName, string description, string argsHelpText, string? helpText = null) =>
        AddCmdA(runCmd, cmdName, description, new string[] { cmdName.ToLower() }, argsHelpText, helpText);
    public SubCmdRun AddCmdAA(SubCmdD runCmd, string description, string argsHelpText, string? helpText = null) =>
        AddCmdA(runCmd, runCmd.Method.Name.LastWord(), description, new string[] { runCmd.Method.Name.LastWord().ToLower() }, argsHelpText, helpText);
    public override void SetCheck(JToken? token)
    {
        try
        {
            if (token is null) return;
            base.SetCheck(token);
            var subCmdToken = token.SelectToken("SubCmds");
            if (subCmdToken is null) return;
            foreach (var cmd in SubCmds)
            {
                cmd.SetCheck(subCmdToken?.SelectToken(cmd.CmdName));
            }
        }
        catch (Exception ex)
        {
            Utils.WriteColorLine(ex.ToString());
        }
    }
    public void SetAllNode(SetAllowInfo info) => SubCmds.ForEach(node => node.AllowInfo.SetInfo(info));
    public void SetAllNodeList(SetAllowInfo info)
    {
        foreach (var nodeList in SubCmds.OfType<SubCmdList>())
        {
            nodeList.AllowInfo.SetInfo(info);
            nodeList.SetAllNodeList(info);
        }
    }
    public void SetAllNodeRun(SetAllowInfo info)
    {
        foreach (var node in SubCmds)
        {
            if (node is SubCmdList list)
                list.SetAllNodeRun(info);
            else
                node.AllowInfo.SetInfo(info);
        }
    }
    public void SetAllNode(SetAllowInfo info, Func<SubCmdBase, bool> func)
    {
        var nodes = SubCmds.Where(func);
        nodes.ForEach(x => x.AllowInfo.SetInfo(info));
        nodes.OfType<SubCmdList>().ForEach(x => x.SetAllNode(info, func));
    }
    internal SubCmdList SelectCmdList(string[] names) => (SubCmdList)this[names, 1, 1];
    internal static string[] GetNames(string name, int count)
    {
        return count switch
        {
            1 => new[] { name.ToLower() },
            2 => new[] { name.ToLower(), char.ToLower(name[0]).ToString() },
            _ => throw new NotSupportedException(),
        };
    }
}