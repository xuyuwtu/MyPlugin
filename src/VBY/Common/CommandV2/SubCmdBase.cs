using TShockAPI;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VBY.Common.CommandV2;
public abstract class SubCmdBase
{
    public int CmdIndex { get; internal set; } = 0;
    public string CmdName { get; internal set; } 
    public string FullCmdName { get; internal set; }
    public bool Enabled = true;
    public string[] Names;
    public string Description;
    public AllowInfo AllowInfo { get; set; } = new();
    public virtual SubCmdList? Parent { get; internal set; } = null; 
    public SubCmdBase(string cmdName, string description, params string[] names)
    {
        CmdName = cmdName;
        FullCmdName = cmdName;
        Names = names.Length == 0 ? new[] { cmdName.ToLower() } : names;
        Description = description;
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
    internal virtual bool NotCanRun(TSPlayer player)
    {
        return
            AllowCheck(player, AllowInfo.NeedLoggedIn && !player.IsLoggedIn, $"[{FullCmdName}]请登陆使用此命令") ||
            AllowCheck(player, !AllowInfo.AllowServer && !player.RealPlayer, $"[{FullCmdName}]服务器不允许执行此命令") ||
            AllowCheck(player, !(string.IsNullOrEmpty(AllowInfo.Permission) || player.HasPermission(AllowInfo.Permission)), $"[{FullCmdName}]权限不足");
    }
    public abstract void Run(CommandArgs args);
    public virtual void SetAllow(JToken? token)
    {
        try
        {
            if (token is null)
            {
                return;
            }
            var setInfo = JsonConvert.DeserializeObject<SetAllowInfo>(token.ToString());
            if (setInfo is null)
            {
                return;
            }
            AllowInfo.SetInfo(setInfo);
        }
        catch (Exception ex)
        {
            Utils.WriteColorLine(ex.ToString());
        }
    }
    public virtual TShockAPI.Command GetCommand() => new(AllowInfo.Permission, Run, Names);
    public virtual TShockAPI.Command GetCommand(string permission) => new(permission, Run, Names);
    public virtual TShockAPI.Command GetCommand(string[] names) => new(AllowInfo.Permission, Run, names);
    public virtual TShockAPI.Command GetCommand(string permission, string[] names) => new(permission, Run, names);
}