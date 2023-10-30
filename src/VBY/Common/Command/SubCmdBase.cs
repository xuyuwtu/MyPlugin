using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TShockAPI;

namespace VBY.Common.Command;
public abstract class SubCmdBase
{
    public int CmdIndex { get; internal set; } = 0;
    public AllowInfo AllowInfo { get; internal set; } = new();
    public string CmdName, FullCmdName;
    public bool Enabled = true;
    public string[] Names;
    public string Description;
    public SubCmdList? Parent { get; internal set; } = null;
    internal SubCmdBase(string cmdName, string description, params string[] names)
    { CmdName = cmdName; FullCmdName = cmdName; Names = names; Description = description; }
    internal virtual bool NotCanRun(TSPlayer player)
    {
        return
            AllowCheck(player, AllowInfo.NeedLoggedIn && !player.IsLoggedIn, $"[{FullCmdName}]请登陆使用此命令") ||
            AllowCheck(player, !AllowInfo.AllowServer && !player.RealPlayer, $"[{FullCmdName}]服务器不允许执行此命令") ||
            AllowCheck(player, !(string.IsNullOrEmpty(AllowInfo.Permission) || player.HasPermission(AllowInfo.Permission)), $"[{FullCmdName}]权限不足");
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
            var setInfo = JsonConvert.DeserializeObject<SetAllowInfo>(token.ToString());
            if (setInfo is null) return;
            AllowInfo.SetInfo(setInfo);
        }
        catch (Exception ex)
        {
            Utils.WriteColorLine(ex.ToString());
        }
    }
    public virtual TShockAPI.Command GetCommand() => new(AllowInfo.Permission, OutCmdRun, Names);
    public virtual TShockAPI.Command GetCommand(params string[] names) => new(AllowInfo.Permission, OutCmdRun, names);
    public virtual TShockAPI.Command GetCommand(string permission, string[] names) => new(permission, OutCmdRun, names);
}