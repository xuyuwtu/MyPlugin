using System.Reflection;
using System.Reflection.Emit;

using TShockAPI;

namespace VBY.Common.Command;

public class SubCmdRun : SubCmdBase
{
    internal SubCmdD RunCmd;
    internal Delegate RunCmdInstance;
    internal bool DireRun = false;
    internal int MinArgsCount;
    public string? ArgsHelpText, HelpText;
    internal SubCmdRun(SubCmdD runCmd, string cmdName, string description, string[] names, string? argsHelpText = null, string? helpText = null, int minArgsCount = 0) : base(cmdName, description, names)
    {
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
    internal SubCmdRun(Delegate runCmd, string cmdName, string description, string[] names, string? helpText = null, int? minArgsCount = null) : base(cmdName, description, names)
    {
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
        var il = new Emit.ILGeneratorClass(dynamic.GetILGenerator());
        var actionType = Type.GetType($"System.Action`{paramTypes.Length}").MakeGenericType(paramTypes);

        var invokeValue = new List<string>(paramTypes.Length) { };
        var endLabel = il.il.DefineLabel();

        #region localvar

        //var player = args.commandArgs.Player
        var player = il.il.DeclareLocal(TypeOf.TSPlayer);
        il.Ldarg(1);
        il.Ldfld(TypeOf.SubCmdArgs.GetField(nameof(SubCmdArgs.commandArgs)));
        il.Callvirt(TypeOf.CommandArgs.GetProperty(nameof(CommandArgs.Player)).GetGetMethod());
        il.Stloc(player);

        //var parameters = args.Parameters;
        var parameters = il.il.DeclareLocal(TypeOf.ListOfString);
        il.Ldarg(1);
        il.Ldfld(TypeOf.SubCmdArgs.GetField(nameof(SubCmdArgs.Parameters)));
        il.Stloc(parameters);

        //string text
        var text = il.il.DeclareLocal(TypeOf.String);
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
                    il.Ldloc(parameters);
                    il.Ldc_I4(stringIndex);
                    il.Callvirt(parameters.LocalType.GetProperty("Item").GetGetMethod());
                    il.Stloc(text);

                    //Utils.FindByNameOrID(text, player)
                    il.Ldloc(text);
                    il.Ldloc(player);
                    il.Call(typeof(Utils).GetMethod(nameof(Utils.FindByNameOrID)));
                    il.Dup();
                    il.Ldfld(typeof((bool, TSPlayer)).GetField(nameof(ValueTuple<bool, TSPlayer>.Item1)));
                    //if (!Utils.FindByNameOrID(text,player)) return; 
                    var endifLabel = il.il.DefineLabel();
                    il.Brtrue(endifLabel);
                    //return
                    il.Pop();
                    il.Br(endLabel);
                    il.il.MarkLabel(endifLabel);
                    //} inPly = Item2
                    var inPly = il.il.DeclareLocal(TypeOf.TSPlayer);
                    il.Ldfld(typeof((bool, TSPlayer)).GetField(nameof(ValueTuple<bool, TSPlayer>.Item2)));
                    il.Stloc(inPly);
                    invokeValue.Add($"ply:{inPly.LocalIndex}");
                }
            }
            else if (TypeOf.ValueType.Contains(type))
            {
                stringIndex++;
                //text = args.Parameters[i]
                il.Ldloc(parameters);
                il.Ldc_I4(stringIndex);
                il.Callvirt(parameters.LocalType.GetProperty("Item").GetGetMethod());
                il.Stloc(text);

                //bool int.TryParse(string, int32&)
                var endifLabel1 = il.il.DefineLabel();
                il.Ldloc(text);
                var result = il.il.DeclareLocal(type);
                il.Ldloca((short)result.LocalIndex);
                il.Call(type.GetMethod("TryParse", new Type[] { TypeOf.String, type.MakeByRefType() }));

                //if !(int.TryParse)
                il.Brtrue(endifLabel1);

                //{ player.SendErrorMessage("无法把{0}转换成int", text);
                //{ player.SendErrorMessage("无法把{0}转换成int", new object[1]{text});
                il.Ldloc(player);
                il.Ldstr($"无法把 {{0}} 转换成 {type.Name} 范围为:{type.GetField("MinValue").GetValue(null)} - {type.GetField("MaxValue").GetValue(null)} 的整数");
                //var arg = new object[1];
                il.Ldc_I4(1);
                il.Newarr(TypeOf.Object);
                //arg[0] = text;
                il.Dup();
                il.Ldc_I4(0);
                il.Ldloc(text);
                il.Stelem_Ref();

                il.Callvirt(TypeOf.TSPlayer.GetMethod(nameof(TSPlayer.SendErrorMessage), new Type[] { TypeOf.String, TypeOf.Object.MakeArrayType() }));
                //return
                il.Br(endLabel);

                //}
                il.il.MarkLabel(endifLabel1);
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
        il.Ldarg(0);
        il.Ldfld(GetType().GetField(nameof(RunCmdInstance), BindingFlags.Instance | BindingFlags.NonPublic));
        il.Castclass(actionType);
        #endregion

        #region Invoke
        //ld args
        for (int i = 0; i < invokeValue.Count; i++)
        {
            switch (invokeValue[i][..3])
            {
                //paramaters[i]
                case "arg":
                    il.Ldloc(parameters);
                    il.Ldc_I4(int.Parse(invokeValue[i][4..]));
                    il.Callvirt(parameters.LocalType.GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public, new Type[] { TypeOf.Int32 }));
                    break;
                //result
                case "loc":
                    il.Ldloc(short.Parse(invokeValue[i][4..]));
                    break;
                case "ply":
                    if (invokeValue[i].Length == 3)
                    {
                        il.Ldloc(player);
                    }
                    else
                    {
                        il.Ldloc(short.Parse(invokeValue[i][4..]));
                    }
                    break;
                case "par":
                    il.Ldarg(1);
                    il.Ldfld(TypeOf.SubCmdArgs.GetField(nameof(SubCmdArgs.Parameters)));
                    break;
                default:
                    Console.WriteLine("switch error '{0}'", invokeValue[i][..3]);
                    break;
            }
        }
        il.Callvirt(actionType.GetMethod("Invoke", paramTypes));
        #endregion

        il.il.MarkLabel(endLabel);
        il.Ret();
        RunCmd = dynamic.CreateDelegate<SubCmdD>(this);
    }
#pragma warning restore CS8604 // 引用类型参数可能为 null。
#pragma warning restore CS8602 // 解引用可能出现空引用。
    internal static void EmptyInvoke(SubCmdRun nodeRun, SubCmdArgs args) => ((Action)nodeRun.RunCmdInstance).Invoke();
    internal static MethodInfo EmptyInvokeMethod = new Action<SubCmdRun, SubCmdArgs>(EmptyInvoke).Method;
    internal static SubCmdD EmptyInvoke(SubCmdRun nodeRun) => EmptyInvokeMethod.CreateDelegate<SubCmdD>(nodeRun);
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
        if (NotCanRun(args.Player)) return;
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
