using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;

using TShockAPI;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace VBY.Basic.Command;
public abstract class SubCmdNode
{
    public int CmdIndex { get; internal set; } = 0;
    public bool AllowServer = true;
    public bool DescCmd;
    public string CmdName, FullCmdName;
    public bool NeedLoggedIn;
    public string Permission = string.Empty;
    public bool Enabled = true;
    public string[] Names;
    public string Description;
    public NodeType NodeType { get; protected set; }
    public SubCmdNodeList? Parent { get; internal set; } = null;
    internal SubCmdNode(string cmdName, string description, params string[] names)
    { CmdName = cmdName; FullCmdName = cmdName; Names = names; Description = description; }
    internal virtual bool NoCanRun(TSPlayer player)
    {
        return
            AllowCheck(player, NeedLoggedIn && !player.IsLoggedIn, $"[{FullCmdName}]请登陆使用此命令") ||
            AllowCheck(player, !AllowServer && !player.RealPlayer, $"[{FullCmdName}]服务器不允许执行此命令") ||
            AllowCheck(player, !(string.IsNullOrEmpty(Permission) || player.HasPermission(Permission)), $"[{FullCmdName}]权限不足");
    }
    internal static bool AllowCheck(TSPlayer player, bool noAllow, string error)
    {
        if (noAllow)
        {
            player.SendErrorMessage(error);
            return true;
        }
        return false;
    }
    public abstract void Run(CommandArgs args);
    internal void OutCmdRun(CommandArgs args) => OutCmdRun(args, CmdIndex);
    internal abstract void OutCmdRun(CommandArgs args, int outCount);
    public virtual void SetCheck(JToken? token)
    {
        try
        {
            if (token is null) return;
            var setInfo = JsonConvert.DeserializeObject<AllowInfo>(token.ToString());
            if (setInfo is null) return;
            NeedLoggedIn = setInfo.NeedLoggedIn ?? false; Permission = setInfo.Permission ?? ""; AllowServer = setInfo.AllowServer ?? true;
        }
        catch (Exception ex)
        {
            Utils.WriteColorLine(ex.ToString());
        }
    }
    public void SetAllowInfo(AllowInfo info)
    {
        if (info.Permission is not null) Permission = info.Permission;
        if (info.AllowServer.HasValue) AllowServer = info.AllowServer.Value;
        if (info.NeedLoggedIn.HasValue) NeedLoggedIn = info.NeedLoggedIn.Value;
    }
    public virtual TShockAPI.Command GetCommand() => new(Permission, OutCmdRun, Names);
    public virtual TShockAPI.Command GetCommand(params string[] names) => new(Permission, OutCmdRun, names);
    public virtual TShockAPI.Command GetCommand(string permission, string[] names) => new(permission, OutCmdRun, names);
}
public class SubCmdNodeList : SubCmdNode
{
    public List<SubCmdNode> SubCmds = new();
    internal SubCmdNodeList(string cmdName, string description, params string[] names) : base(cmdName, description, names)
    { DescCmd = true; NodeType = NodeType.List; }
    public SubCmdNode this[string name]
    {
        get
        {
            SubCmdNode result = this;
            if (name.Contains('.'))
            {
                return ((SubCmdNodeList)result)[name.Split('.', StringSplitOptions.RemoveEmptyEntries)];
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
    public SubCmdNode this[string[] names, int start = 0, int reduce = 0]
    {
        get
        {
            SubCmdNode result = this;
            for (int i = start; i < names.Length - reduce; i++)
                result = ((SubCmdNodeList)result)[names[i]];
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
            if (!have) args.Player.SendInfoMessage($"好像没有可用子命令,问问腐竹是不是配错了");
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
                    if (cmd.NoCanRun(args.Player)) break;
                    cmd.Run(args);
                    break;
                }
            }
            if (!found) args.Player.SendInfoMessage($"未知参数 {findText}");
        }
    }
    internal override void OutCmdRun(CommandArgs args, int outCount)
    {
        if (NoCanRun(args.Player)) return;
        if (args.Parameters.Count == CmdIndex - outCount)
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
    internal void AddNode(SubCmdNode addNode)
    {
        addNode.CmdIndex = CmdIndex + 1;
        addNode.FullCmdName = FullCmdName + "." + addNode.CmdName;
        addNode.Parent = this;
        if (addNode.NodeType == NodeType.Run)
            ((SubCmdNodeRun)addNode).MinArgsCount += addNode.CmdIndex;
        SubCmds.Add(addNode);
    }
    public SubCmdNodeList AddList(string cmdName, string description, params string[] names)
    {
        var addNode = new SubCmdNodeList(cmdName, description, names);
        AddNode(addNode);
        return addNode;
    }
    public SubCmdNodeList AddList(string cmdName, string description, int nameCount = 1) =>
        AddList(cmdName, description, GetNames(cmdName, nameCount));
    public void AddLists(params (string cmdName,string description)[] values)
    {
        foreach (var value in values)
            AddList(value.cmdName, value.description);
    }
    public SubCmdNodeRun AddCmd(SubCmdD runCmd, string cmdName, string description, string[] names, string? argsHelpText, string? helpText, int minArgsCount = 0)
    {
        var addNode = new SubCmdNodeRun(runCmd, cmdName, description, names, argsHelpText, helpText, minArgsCount);
        AddNode(addNode);
        return addNode;
    }
    public SubCmdNodeRun AddCmd(SubCmdD runCmd, string cmdName, string description, string? argsHelpText, string? helpText, int minArgsCount, int nameCount = 1)
    {
        return AddCmd(runCmd, cmdName, description, GetNames(cmdName, nameCount), argsHelpText, helpText, minArgsCount);
    }
    public SubCmdNodeRun AddCmd(SubCmdD runCmd, string description, string? argsHelpText, string? helpText, int minArgsCount, int nameCount = 1)
    {
        return AddCmd(runCmd, runCmd.Method.Name.LastWord(), description, argsHelpText, helpText, minArgsCount, nameCount);
    }
    public SubCmdNodeRun AddCmd(SubCmdD runCmd, string description, string argsHelpText, string? helpText, int nameCount = 1)
    {
        return AddCmd(runCmd, description, argsHelpText, helpText, argsHelpText.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length, nameCount);
    }
    public SubCmdNodeRun AddCmd(SubCmdD runCmd, string description, string argsHelpText, int nameCount = 1)
    {
        return AddCmd(runCmd, description, argsHelpText, null, nameCount);
    }
    public SubCmdNodeRun AddCmd(SubCmdD runCmd, string description, int nameCount = 1)
    {
        return AddCmd(runCmd, description, null, null, 0, nameCount);
    }
    public SubCmdNodeRun AddCmd(SubCmdD runCmd, int nameCount = 1)
    {
        return AddCmd(runCmd, runCmd.Method.GetCustomAttribute<DescriptionAttribute>()?.Description ?? throw new Exception($"method:{runCmd.Method.Name}'s DescriptionAttribute is null"), nameCount);
    }
    public void AddCmds(int nameCount, params SubCmdD[] cmds)
    {
        foreach (var cmd in cmds)
            AddCmd(cmd, nameCount);
    }
    public SubCmdNodeRun AddCmd(Delegate runCmd, string cmdName, string description, string[] names, string? helpText)
    {
        var addNode = new SubCmdNodeRun(runCmd, cmdName, description, names, helpText);
        AddNode(addNode);
        return addNode;
    }
    public SubCmdNodeRun AddCmdA(SubCmdD runCmd, string cmdName, string description, string[] names, string argsHelpText, string? helpText = null)
    {
        var addNode = new SubCmdNodeRun(runCmd, cmdName, description, names, argsHelpText, helpText, argsHelpText.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length);
        AddNode(addNode);
        return addNode;
    }
    public SubCmdNodeRun AddCmdA(SubCmdD runCmd, string cmdName, string description, string argsHelpText, string? helpText = null) =>
        AddCmdA(runCmd, cmdName, description, new string[] { cmdName.ToLower() }, argsHelpText, helpText);
    public SubCmdNodeRun AddCmdAA(SubCmdD runCmd, string description, string argsHelpText, string? helpText = null) =>
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
    public void SetAllNode(AllowInfo info) => SubCmds.ForEach(node => node.SetAllowInfo(info));
    public void SetAllNodeList(AllowInfo info)
    {
        foreach (var nodeList in SubCmds.Where(node => node.NodeType == NodeType.List).Select(node => (SubCmdNodeList)node))
        {
            nodeList.SetAllowInfo(info);
            nodeList.SetAllNodeList(info);
        }
    }
    public void SetAllNodeRun(AllowInfo info)
    {
        foreach (var node in SubCmds)
        {
            if (node.NodeType == NodeType.List)
                ((SubCmdNodeList)node).SetAllNodeRun(info);
            else
                node.SetAllowInfo(info);
        }
    }
    public void SetAllNode(AllowInfo info, Func<SubCmdNode, bool> func)
    {
        var nodes = SubCmds.Where(func);
        nodes.ForEach(x => x.SetAllowInfo(info));
        nodes.Where(x => x.NodeType == NodeType.List).ForEach(x => ((SubCmdNodeList)x).SetAllNode(info, func));
    }
    internal SubCmdNodeList SelectCmdList(string[] names) => (SubCmdNodeList)this[names, 1, 1];
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
public class SubCmdNodeRun : SubCmdNode
{
    internal SubCmdD RunCmd;
    internal Delegate RunCmdInstance;
    internal bool DireRun = false;
    internal int MinArgsCount;
    public string? ArgsHelpText, HelpText;
    internal SubCmdNodeRun(SubCmdD runCmd, string cmdName, string description, string[] names, string? argsHelpText = null, string? helpText = null, int minArgsCount = 0) : base(cmdName, description, names)
    {
        NodeType = NodeType.Run;
        RunCmd = runCmd;
        RunCmdInstance = runCmd;
        if (argsHelpText is null && helpText is null)
            DireRun = true;
        else
        {
            ArgsHelpText = argsHelpText;
            HelpText = helpText;
        }
        MinArgsCount = minArgsCount;
    }

#pragma warning disable CS8604 // 引用类型参数可能为 null。
#pragma warning disable CS8602 // 解引用可能出现空引用。
    internal SubCmdNodeRun(Delegate runCmd, string cmdName, string description, string[] names, string? helpText = null, int? minArgsCount = null) : base(cmdName, description, names)
    {
        NodeType = NodeType.Run;
        HelpText = helpText;
        if (runCmd.Method.ReturnType != TypeOf.Void)
            throw new Exception("need void method");
        ParameterInfo[] paramInfos = runCmd.Method.GetParameters();
        Type[] paramTypes = paramInfos.Select(x => x.ParameterType).ToArray();
        RunCmdInstance = runCmd;
        if (paramInfos.Length == 0)
        {
            DireRun = true;
            RunCmd = EmptyInvoke(this);
            return;
        }
        if (paramTypes.Length == 1 && paramTypes[0] == TypeOf.SubCmdArgs)
        {
            RunCmd = (SubCmdD)runCmd;
            return;
        }
        var argsHelpTypes = new List<ParameterInfo>(paramInfos);
        for (int i = 0; i < paramInfos.Length; i++)
        {
            var type = paramTypes[i];
            if ((type == TypeOf.TSPlayer || type == TypeOf.ListOfString) && paramInfos[i].GetCustomAttribute<NotFromArgsAttribute>() is null)
                argsHelpTypes.Remove(paramInfos[i]);
        }
        MinArgsCount = minArgsCount ?? argsHelpTypes.Count;
        ArgsHelpText = string.Join(' ', argsHelpTypes.Select(x => $"<{x.Name}: {Info.TypeNames[x.ParameterType.Name]}>"));
        DynamicMethod dynamic = new(cmdName, TypeOf.Void, new Type[] { TypeOf.SubCmdNodeRun, TypeOf.SubCmdArgs });
        var il = dynamic.GetILGenerator();
        var actionType = Type.GetType($"System.Action`{paramTypes.Length}").MakeGenericType(paramTypes);

        var invokeValue = new List<string>(paramTypes.Length) { };
        var endLabel = il.DefineLabel();

        #region localvar

        //var player = args.commandArgs.Player
        var player = il.DeclareLocal(TypeOf.TSPlayer);
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Ldfld, TypeOf.SubCmdArgs.GetField(nameof(SubCmdArgs.commandArgs)));
        il.Emit(OpCodes.Callvirt, TypeOf.CommandArgs.GetProperty(nameof(CommandArgs.Player)).GetGetMethod());
        il.Emit(OpCodes.Stloc, player);

        //var parameters = args.Parameters;
        var parameters = il.DeclareLocal(TypeOf.ListOfString);
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Ldfld, TypeOf.SubCmdArgs.GetField(nameof(SubCmdArgs.Parameters)));
        il.Emit(OpCodes.Stloc, parameters);

        //string text
        var text = il.DeclareLocal(TypeOf.String);
        #endregion

        int stringIndex = -1;
        #region TryParse
        for (int i = 0; i < paramTypes.Length; i++)
        {
            var type = paramTypes[i];
            if (type == TypeOf.String)
            {
                stringIndex++;
                invokeValue.Add($"arg:{stringIndex}");
            }
            else if (type == TypeOf.TSPlayer)
            {
                if (i == 0 && MinArgsCount == paramTypes.Length - 1)
                {
                    invokeValue.Add($"ply");
                }
                else
                {
                    stringIndex++;
                    //text = args.Parameters[i];
                    il.Emit(OpCodes.Ldloc, parameters);
                    il.Emit(OpCodes.Ldc_I4, stringIndex);
                    il.Emit(OpCodes.Callvirt, parameters.LocalType.GetProperty("Item").GetGetMethod());
                    il.Emit(OpCodes.Stloc, text);

                    //Utils.FindByNameOrID(text, player)
                    il.Emit(OpCodes.Ldloc, text);
                    il.Emit(OpCodes.Ldloc, player);
                    il.Emit(OpCodes.Call, typeof(Utils).GetMethod(nameof(Utils.FindByNameOrID)));
                    il.Emit(OpCodes.Dup);
                    il.Emit(OpCodes.Ldfld, typeof((bool, TSPlayer)).GetField(nameof(ValueTuple<bool, TSPlayer>.Item1)));
                    //if (!Utils.FindByNameOrID(text,player)) return; 
                    var endifLabel = il.DefineLabel();
                    il.Emit(OpCodes.Brtrue, endifLabel);
                    //return
                    il.Emit(OpCodes.Pop);
                    il.Emit(OpCodes.Br, endLabel);
                    il.MarkLabel(endifLabel);
                    //} inPly = Item2
                    var inPly = il.DeclareLocal(TypeOf.TSPlayer);
                    il.Emit(OpCodes.Ldfld, typeof((bool, TSPlayer)).GetField(nameof(ValueTuple<bool, TSPlayer>.Item2)));
                    il.Emit(OpCodes.Stloc, inPly);
                    invokeValue.Add($"ply:{inPly.LocalIndex}");
                }
            }
            else if (TypeOf.ValueType.Contains(type))
            {
                stringIndex++;
                //text = args.Parameters[i]
                il.Emit(OpCodes.Ldloc, parameters);
                il.Emit(OpCodes.Ldc_I4, stringIndex);
                il.Emit(OpCodes.Callvirt, parameters.LocalType.GetProperty("Item").GetGetMethod());
                il.Emit(OpCodes.Stloc, text);

                //bool int.TryParse(string, int32&)
                var endifLabel1 = il.DefineLabel();
                il.Emit(OpCodes.Ldloc, text);
                var result = il.DeclareLocal(type);
                il.Emit(OpCodes.Ldloca, (short)result.LocalIndex);
                il.Emit(OpCodes.Call, type.GetMethod("TryParse", new Type[] { TypeOf.String, type.MakeByRefType() }));

                //if !(int.TryParse)
                il.Emit(OpCodes.Brtrue, endifLabel1);

                //{ player.SendErrorMessage("无法把{0}转换成int", text);
                //{ player.SendErrorMessage("无法把{0}转换成int", new object[1]{text});
                il.Emit(OpCodes.Ldloc, player);
                il.Emit(OpCodes.Ldstr, $"无法把 {{0}} 转换成 {type.Name} 范围为:{type.GetField("MinValue").GetValue(null)} - {type.GetField("MaxValue").GetValue(null)} 的整数");
                il.Emit(OpCodes.Ldc_I4_1);
                //var arg = new object[1];
                il.Emit(OpCodes.Newarr, TypeOf.Object);
                il.Emit(OpCodes.Dup);
                //arg[0] = text;
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Ldloc, text);
                il.Emit(OpCodes.Stelem_Ref);

                il.Emit(OpCodes.Callvirt, TypeOf.TSPlayer.GetMethod(nameof(TSPlayer.SendErrorMessage), new Type[] { TypeOf.String, TypeOf.Object.MakeArrayType() }));
                //return
                il.Emit(OpCodes.Br, endLabel);

                //}
                il.MarkLabel(endifLabel1);
                invokeValue.Add($"loc:{result.LocalIndex}");
            }
            else if (type == TypeOf.ListOfString)
            {
                invokeValue.Add("par");
            }
            else
                throw new Exception("now only support string,byte,short,int,long,TSPlayer,List<string>");
        }
        #endregion

        #region Castclass
        //((Action`...)this.RunCmdInstance).Invoke(args...)
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, GetType().GetField(nameof(RunCmdInstance), BindingFlags.Instance | BindingFlags.NonPublic));
        il.Emit(OpCodes.Castclass, actionType);
        #endregion

        #region Invoke
        //ld args
        for (int i = 0; i < invokeValue.Count; i++)
        {
            switch (invokeValue[i][..3])
            {
                //paramaters[i]
                case "arg":
                    il.Emit(OpCodes.Ldloc, parameters);
                    il.Emit(OpCodes.Ldc_I4, int.Parse(invokeValue[i][4..]));
                    il.Emit(OpCodes.Callvirt, parameters.LocalType.GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public, new Type[] { TypeOf.Int32 }));
                    break;
                //result
                case "loc":
                    il.Emit(OpCodes.Ldloc, short.Parse(invokeValue[i][4..]));
                    break;
                case "ply":
                    if (invokeValue[i].Length == 3)
                    {
                        il.Emit(OpCodes.Ldloc, player);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldloc, short.Parse(invokeValue[i][4..]));
                    }
                    break;
                case "par":
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldfld, TypeOf.SubCmdArgs.GetField(nameof(SubCmdArgs.Parameters)));
                    break;
                default:
                    Console.WriteLine("switch error '{0}'", invokeValue[i][..3]);
                    break;
            }
        }
        il.Emit(OpCodes.Callvirt, actionType.GetMethod("Invoke", paramTypes));
        #endregion

        il.MarkLabel(endLabel);
        il.Emit(OpCodes.Ret);
        RunCmd = dynamic.CreateDelegate<SubCmdD>(this);
    }
#pragma warning restore CS8604 // 引用类型参数可能为 null。
#pragma warning restore CS8602 // 解引用可能出现空引用。
    internal static void EmptyInvoke(SubCmdNodeRun nodeRun, SubCmdArgs args) => ((Action)nodeRun.RunCmdInstance).Invoke();
    internal static MethodInfo EmptyInvokeMethod = new Action<SubCmdNodeRun, SubCmdArgs>(EmptyInvoke).Method;
    internal static SubCmdD EmptyInvoke(SubCmdNodeRun nodeRun) => EmptyInvokeMethod.CreateDelegate<SubCmdD>(nodeRun);
    public override void Run(CommandArgs args)
    {
        if (DireRun || args.Parameters.Count >= MinArgsCount)
            RunCmd.Invoke(new SubCmdArgs(CmdName, args, args.Parameters.Skip(CmdIndex).ToList()));
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
    internal override void OutCmdRun(CommandArgs args, int outCount)
    {
        if (NoCanRun(args.Player)) return;
        if (DireRun || args.Parameters.Count >= (MinArgsCount - outCount))
            RunCmd.Invoke(new SubCmdArgs(CmdName, args, args.Parameters.Skip(CmdIndex - outCount).ToList()));
        else
        {
            args.Player.SendInfoMessage($"参数不足,最少需要{MinArgsCount - CmdIndex}个参数");
            bool flag1 = !string.IsNullOrEmpty(ArgsHelpText);
            if (flag1) args.Player.SendInfoMessage(CmdIndex == outCount ? $"/{args.Message.Trim()} {ArgsHelpText}" : $"/{string.Join(' ', args.Message[..args.Message.IndexOf(' ')], string.Join(' ', args.Parameters.GetRange(0, CmdIndex - outCount)))} {ArgsHelpText}");
            bool flag2 = !string.IsNullOrEmpty(HelpText);
            if (flag2) args.Player.SendInfoMessage(HelpText);
            if (!(flag1 || flag2)) args.Player.SendErrorMessage("此命令没有帮助文本!");
        }
    }
}
public class SubCmdRoot : SubCmdNodeList
{
    public SubCmdRoot(string cmdName) : base(cmdName, "", cmdName.ToLower()) { }
    public override TShockAPI.Command GetCommand() => new(Permission, Run, Names) { HelpText = Description };
    public override TShockAPI.Command GetCommand(params string[] names) => new(Permission, Run, names) { HelpText = Description };
    public override TShockAPI.Command GetCommand(string permission, string[] names) => new(Permission, Run, names) { HelpText = Description };
    public static void AutoAdd(object target, SubCmdRoot use, SubCmdRoot admin, Dictionary<string, string> wordChinese)
    {
        var type = target.GetType();
        var methods = type.GetMethods();
        var roots = new (SubCmdRoot root, IEnumerable<MethodInfo> methods)[] { (use, methods.Where(x => x.Name.StartsWith("Cmd"))), (admin, methods.Where(x => x.Name.StartsWith("Ctl"))) };
        foreach (var rootCmd in roots)
        {
            foreach (var method in rootCmd.methods)
            {
                var names = System.Text.RegularExpressions.Regex.Split(method.Name, "(?<!^)(?=[A-Z])");
                SubCmdNodeList list = names.Length == 2 ? rootCmd.root : rootCmd.root.SelectCmdList(names);
                var helpTextAttr = method.GetCustomAttribute<HelpTextAttribute>();
                var noderun = new SubCmdNodeRun((SubCmdD)method.GetDelegate(method.IsStatic ? null : target), names[^1], method.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "", new string[] { names[^1].ToLower() }, helpTextAttr?.ArgsHelpText, helpTextAttr?.HelpText, helpTextAttr?.MinArgsCount ?? 0);
                if (string.IsNullOrEmpty(noderun.Description))
                {
                    if (names.Length == 2)
                        noderun.Description = wordChinese[names[^1]];
                    else
                        noderun.Description = $"{wordChinese[names[^1]]}{wordChinese[names[^2]]}";
                }
                if (noderun.ArgsHelpText is not null && noderun.MinArgsCount == 0)
                    noderun.MinArgsCount = noderun.ArgsHelpText.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
                noderun.SetAllowInfo(method.GetCustomAttribute<AllowInfoAttribute>()?.GetAllowInfo() ?? new());
                list.AddNode(noderun);
            }
        }
    }
}
[AttributeUsage(AttributeTargets.Parameter)]
public class NotFromArgsAttribute : Attribute { }
public readonly struct SubCmdArgs
{
    public readonly string CmdName;
    public readonly List<string> Parameters;
    public readonly CommandArgs commandArgs;
    public TSPlayer Player => commandArgs.Player;
    public SubCmdArgs(string cmdName, CommandArgs args, List<string> parameters)
    { CmdName = cmdName; commandArgs = args; Parameters = parameters; }

}
public delegate void SubCmdD(SubCmdArgs args);
[AttributeUsage(AttributeTargets.Method)]
public class SubCmdInfoAttribute : Attribute
{
    public string CmdName;
    public string[] Names;
    public string? ArgsHelpText, HelpText;
    public int MinArgsCount;
    public SubCmdInfoAttribute(string cmdName, string[] names, string? argsHelpText = null, string? helpText = null, int minArgsCount = 0)
    {
        CmdName = cmdName; Names = names; ArgsHelpText = argsHelpText; HelpText = helpText; MinArgsCount = minArgsCount;
    }
}
[AttributeUsage(AttributeTargets.Method)]
public class HelpTextAttribute : Attribute
{
    public string? ArgsHelpText, HelpText;
    public int MinArgsCount;

    public HelpTextAttribute(string? argsHelpText, string? helpText, int minArgsCount = 0)
    {
        ArgsHelpText = argsHelpText; HelpText = helpText; MinArgsCount = minArgsCount;
    }
    public HelpTextAttribute(string argsHelpText) : this(argsHelpText, null) { }
    public HelpTextAttribute(string argsHelpText, int minArgsCount) : this(argsHelpText, null, minArgsCount) { }
}
public class AllowInfo
{
    public bool? NeedLoggedIn { get; set; }
    public bool? AllowServer { get; set; }
    public string? Permission { get; set; }
    public AllowInfo() { }
    public AllowInfo(bool? needLoggedIn, bool? allowServer, string? permission)
    { NeedLoggedIn = needLoggedIn; AllowServer = allowServer; Permission = permission; }
}

[AttributeUsage(AttributeTargets.Method)]
public class AllowInfoAttribute : Attribute
{
    public bool NeedLoggedIn = false;
    public bool AllowServer = true;
    public string? Permission = null;
    public AllowInfoAttribute() { }
    public AllowInfoAttribute(bool needLoggedIn = false, bool allowServer = true, string? permission = null)
    { NeedLoggedIn = needLoggedIn; AllowServer = allowServer; Permission = permission; }
    public AllowInfo GetAllowInfo() => new(NeedLoggedIn, AllowServer, Permission);
}
public enum NodeType
{
    List, Run
}