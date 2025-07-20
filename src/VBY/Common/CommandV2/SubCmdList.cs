using System.Collections;

using TShockAPI;

using Newtonsoft.Json.Linq;

namespace VBY.Common.CommandV2;

public class SubCmdList : SubCmdBase, IEnumerable<SubCmdBase>
{
    public List<SubCmdBase> SubCmds = new();
    public string? DefaultCmd;
    public SubCmdList(string cmdName, string description, params string[] names) : base(cmdName, description, names) { }
    public SubCmdBase this[string name]
    {
        get
        {
            SubCmdBase? result;
            if (name.Contains('.'))
            {
                return this[name.Split('.', StringSplitOptions.RemoveEmptyEntries)];
            }
            else
            {
                result = SubCmds.Find(x => x.CmdName == name);
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
                if (!cmd.Enabled)
                {
                    continue;
                }
                args.Player.SendInfoMessage($"{cmd.Names[0]} {cmd.Description}");
                have = true;
            }
            if (!have)
            {
                args.Player.SendInfoMessage($"好像没有可用子命令,问问服主是不是配错了");
            }
        }
        else
        {
            string findText = args.Parameters[CmdIndex];
            bool found = false;
            foreach (var cmd in SubCmds)
            {
                if (!cmd.Enabled)
                {
                    continue;
                }
                if (cmd.Names.Contains(findText, StringComparer.OrdinalIgnoreCase))
                {
                    found = true;
                    if (!cmd.CanRun(args.Player))
                    {
                        break;
                    }
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
    public void Add(SubCmdBase addNode)
    {
        addNode.CmdIndex = CmdIndex + 1;
        addNode.FullCmdName = FullCmdName + "." + addNode.CmdName;
        addNode.Parent = this;
        if (addNode is SubCmdRun runcmd)
            runcmd.MinArgsCount += addNode.CmdIndex;
        SubCmds.Add(addNode);
    }
    public void Add(SubCmdD subCmd) => Add(new SubCmdRun(subCmd));
    public void Adds(int nameCount, params SubCmdD[] subCmds)
    {
        foreach (var subcmd in subCmds)
        {
            Add(new SubCmdRun(subcmd, nameCount));
        }
    }
    public override void SetAllow(JToken? token)
    {
        try
        {
            if (token is null) return;
            base.SetAllow(token);
            var subCmdToken = token.SelectToken("SubCmds");
            if (subCmdToken is null) return;
            foreach (var cmd in SubCmds)
            {
                cmd.SetAllow(subCmdToken?.SelectToken(cmd.CmdName));
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
    public IEnumerator<SubCmdBase> GetEnumerator() => SubCmds.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}