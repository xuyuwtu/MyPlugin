using TShockAPI;

namespace VBY.Common.Config;

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
public class CommandInfo
{
    public string Permissions;
    public string[] Names;
    public CommandInfo() { }
    public CommandInfo(string permissions, params string[] names)
    {
        Permissions = permissions;
        Names = names;
    }
    public TShockAPI.Command GetCommand(CommandDelegate cmd) => new(Permissions, cmd, Names);
}
