using TShockAPI;

using VBY.Common.Command;

namespace VBY.Common.Config;

public class CommandGroup
{
    public CommandInfo Use = new();
    public CommandInfo Admin = new();
    public TShockAPI.Command[] GetCommands(CommandDelegate use, CommandDelegate admin) => new TShockAPI.Command[] { Use.GetCommand(use), Admin.GetCommand(admin) };
    public TShockAPI.Command[] GetCommands(SubCmdRoot use, SubCmdRoot admin) => new TShockAPI.Command[] { use.GetCommand(Use.Permissions, Use.Names), admin.GetCommand(Admin.Permissions, Admin.Names) };
}