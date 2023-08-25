using System.Collections;

using TShockAPI;

using Newtonsoft.Json.Linq;

namespace VBY.Common.Command;

public class SubCmdList : SubCmdBase, IEnumerable<SubCmdBase>
{
    public List<SubCmdBase> SubCmds = new();
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
    public override void Run(CommandArgs args)
    {
        if (args.Parameters.Count == CmdIndex)
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
            if (!have) args.Player.SendInfoMessage($"好像没有可用子命令,问问服主是不是配错了");
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
                    if (cmd.NotCanRun(args.Player))
                        break;
                    cmd.Run(args);
                    break;
                }
            }
            if (!found) args.Player.SendInfoMessage($"未知参数 {findText}");
        }
    }
    public void Add(SubCmdBase addNode)
    {
        addNode.Parent = this;
        SubCmds.Add(addNode);
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
    public void SetAllowInfo(SetAllowInfo info, bool toList = true, bool toRun = true, bool toSubNode = true)
    {
        if (toList && toRun)
        {
            foreach(var node in SubCmds)
            {
                node.AllowInfo.SetInfo(info);
            }
        }
        else
        {
            if (toList)
            {
                foreach (var nodeList in SubCmds.OfType<SubCmdList>())
                {
                    nodeList.AllowInfo.SetInfo(info);
                }
            }
            if (toRun)
            {
                foreach (var node in SubCmds.OfType<SubCmdRun>())
                {
                    node.AllowInfo.SetInfo(info);
                }
            }
        }
        if (toSubNode)
        {
            SubCmds.OfType<SubCmdList>().ForEach(x => x.SetAllowInfo(info, toList, toRun, toSubNode));
        }
    }
    public void SetAllNode(SetAllowInfo info, Func<SubCmdBase, bool> func)
    {
        var nodes = SubCmds.Where(func);
        nodes.ForEach(x => x.AllowInfo.SetInfo(info));
        nodes.OfType<SubCmdList>().ForEach(x => x.SetAllNode(info, func));
    }
    internal SubCmdList SelectCmdList(string[] names) => (SubCmdList)this[names, 1, 1];
    internal void UpdateCmdIndex()
    {
        SubCmds.ForEach(x =>
        {
            x.CmdIndex = CmdIndex + 1;
            x.FullCmdName = FullCmdName + x.CmdName;
            if (x is SubCmdRun cmdRun)
            {
                cmdRun.MinArgsCount += x.CmdIndex;
            }
        });
        SubCmds.OfType<SubCmdList>().ForEach(x => x.UpdateCmdIndex());
    }
    public IEnumerator<SubCmdBase> GetEnumerator() => SubCmds.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}