using TShockAPI;

namespace VBY.Common.Command;


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
public class SetAllowInfo
{
    public bool? NeedLoggedIn { get; set; }
    public bool? AllowServer { get; set; }
    public string? Permission { get; set; }
    public SetAllowInfo() { }
    public SetAllowInfo(bool? needLoggedIn, bool? allowServer, string? permission)
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
    public SetAllowInfo GetAllowInfo() => new(NeedLoggedIn, AllowServer, Permission);
}
public class AllowInfo
{
    public bool NeedLoggedIn, AllowServer;
    public string Permission;
    public AllowInfo()
    {
        NeedLoggedIn = false;
        AllowServer = true;
        Permission = "";
    }
    public AllowInfo(bool needLoggedIn = false, bool allowServer = true, string permission = "")
    {
        NeedLoggedIn = needLoggedIn;
        AllowServer = allowServer;
        Permission = permission;
    }
    public void SetInfo(bool? needLoggedIn, bool? allowServer, string? permission)
    {
        if (needLoggedIn is not null) NeedLoggedIn = needLoggedIn.Value;
        if (allowServer is not null) AllowServer = allowServer.Value;
        if (permission is not null) Permission = permission;
    }
    public void SetInfo(SetAllowInfo info) => SetInfo(info.NeedLoggedIn, info.AllowServer, info.Permission);
}