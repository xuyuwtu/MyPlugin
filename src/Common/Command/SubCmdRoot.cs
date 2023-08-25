using System.ComponentModel;
using System.Reflection;

namespace VBY.Common.Command;

public class SubCmdRoot : SubCmdList
{
    public SubCmdRoot(string cmdName) : base(cmdName, "", cmdName.ToLower()) { }
    public override TShockAPI.Command GetCommand() => new(AllowInfo.Permission, Run, Names) { HelpText = Description };
    public override TShockAPI.Command GetCommand(params string[] names) => new(AllowInfo.Permission, Run, names) { HelpText = Description };
    public override TShockAPI.Command GetCommand(string permission, string[] names) => new(AllowInfo.Permission, Run, names) { HelpText = Description };
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
                SubCmdList list = names.Length == 2 ? rootCmd.root : rootCmd.root.SelectCmdList(names);
                var helpTextAttr = method.GetCustomAttribute<HelpTextAttribute>();
                var noderun = new SubCmdRun((SubCmdD)method.GetDelegate(method.IsStatic ? null : target), names[^1], method.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "", new string[] { names[^1].ToLower() }, helpTextAttr?.ArgsHelpText, helpTextAttr?.HelpText, helpTextAttr?.MinArgsCount ?? 0);
                if (string.IsNullOrEmpty(noderun.Description))
                {
                    if (names.Length == 2)
                        noderun.Description = wordChinese[names[^1]];
                    else
                        noderun.Description = $"{wordChinese[names[^1]]}{wordChinese[names[^2]]}";
                }
                if (noderun.ArgsHelpText is not null && noderun.MinArgsCount == 0)
                    noderun.MinArgsCount = noderun.ArgsHelpText.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
                noderun.AllowInfo.SetInfo(method.GetCustomAttribute<AllowInfoAttribute>()?.GetAllowInfo() ?? new());
                list.AddNode(noderun);
            }
        }
    }
}