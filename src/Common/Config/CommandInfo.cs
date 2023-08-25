using TShockAPI;

namespace VBY.Common.Config;
public class CommandInfo
{
    public string Permissions = "";
    public string[] Names = Array.Empty<string>();
    public CommandInfo() { }
    public CommandInfo(string permissions, params string[] names)
    {
        Permissions = permissions;
        Names = names;
    }
    public TShockAPI.Command GetCommand(CommandDelegate cmd) => new(Permissions, cmd, Names);
}
