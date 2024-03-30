using TShockAPI;

namespace VBY.OtherCommand;

#nullable disable
public class ReplaceCommand
{
    private string _replaceCommandName;
    public string ReplaceCommandName => _replaceCommandName;
    private CommandDelegate _replaceDelegate;
    private bool _initizlized = false;
    public CommandDelegate OriginalDelegate { get; private set; }
    public CommandDelegate ReplaceDelegate 
    {
        get => _replaceDelegate;
        set
        {
            if (value == null)
            {
                return;
            }
            if (Replaced)
            {
                Restore();
                _replaceDelegate = value;
                Replace();
            }
            else
            {
                _replaceDelegate = value;
            }
        }
    }
    public Command ModifiedCommand { get; private set; }
    public bool Replaced { get; private set; }
    public ReplaceCommand(string replaceCommandName, CommandDelegate replaceDelegate)
    {
        _replaceDelegate = replaceDelegate;
        _replaceCommandName = replaceCommandName;
    }
    public void Replace()
    {
        if (Replaced)
        {
            return;
        }
        if (!_initizlized)
        {
            var command = Commands.ChatCommands.Find(x => x.Name == _replaceCommandName);
            if (command is null)
            {
                throw new ArgumentException($"{nameof(ReplaceCommandName)} can't find");
            }
            OriginalDelegate = command.CommandDelegate;
            ModifiedCommand = command;
        }
        Replaced = true;
        ModifiedCommand.CommandDelegate = ReplaceDelegate;
    }
    public void Restore()
    {
        if (!Replaced)
        {
            return;
        }
        Replaced = false;
        ModifiedCommand.CommandDelegate = OriginalDelegate;
    }
}
