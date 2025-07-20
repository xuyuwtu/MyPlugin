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
    internal virtual bool CanRun(TSPlayer player)
    {
        do
        {
            if (AllowInfo.NeedLoggedIn && !player.IsLoggedIn)
            {
                player.SendErrorMessage($"[{FullCmdName}]请登陆使用此命令");
                break;
            }
            if (!AllowInfo.AllowServer && !player.RealPlayer)
            {
                player.SendErrorMessage($"[{FullCmdName}]服务器不允许执行此命令");
                break;
            }
            if (!(string.IsNullOrEmpty(AllowInfo.Permission) || player.HasPermission(AllowInfo.Permission)))
            {
                player.SendErrorMessage($"[{FullCmdName}]权限不足");
                break;
            }
            return true;
        } while (false);
        return false;
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