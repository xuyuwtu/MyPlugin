using TShockAPI;

namespace VBY.Common.CommandV2;

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
public class SetAllowInfo
{
    public bool? NeedLoggedIn { get; set; }
    public bool? AllowServer { get; set; }
    public string? Permission { get; set; }
    public SetAllowInfo() { }
    public SetAllowInfo(bool? needLoggedIn, bool? allowServer, string? permission)
    { NeedLoggedIn = needLoggedIn; AllowServer = allowServer; Permission = permission; }
}
public class AllowInfo
{
    public bool NeedLoggedIn = false;
    public bool AllowServer = true;
    public string Permission = "";
    public AllowInfo() { }
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