using System.Diagnostics;

using TShockAPI;

using Newtonsoft.Json;

using VBY.Basic.Command;

namespace VBY.Basic.Config;
public class MainRoot
{
    public Command Commands = new();
}
public class Command
{
    public CommandInfo Use = new();
    public CommandInfo Admin = new();
    public TShockAPI.Command[] GetCommands(CommandDelegate use, CommandDelegate admin) => new TShockAPI.Command[] { Use.GetCommand(use), Admin.GetCommand(admin) };
    public TShockAPI.Command[] GetCommands(SubCmdRoot use, SubCmdRoot admin) => new TShockAPI.Command[] { use.GetCommand(Use.Permissions, Use.Names), admin.GetCommand(Admin.Permissions, Admin.Names) };
}
public class CommandInfo
{
    public string Permissions;
    public string[] Names;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    public CommandInfo() { }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    public CommandInfo(string permissions, params string[] names)
    {
        Permissions = permissions;
        Names = names;
    }
    public TShockAPI.Command GetCommand(CommandDelegate cmd) => new(Permissions, cmd, Names);
}
public class MainConfig<T> where T : MainRoot, new()
{
    public string ConfigDirectory;
    public string FileName;
    public virtual string ConfigPath
    {
        get => Path.Combine(ConfigDirectory, FileName);
    }
    public virtual bool ConfigExists
    {
        get => File.Exists(ConfigPath);
    }
    public T Root = new();
    public bool Normal;
    public string StateString = "正常";
    public string ErrorString = "无";
    public MainConfig(string configDirectory, string? fileName = null)
    {
        ConfigDirectory = configDirectory;
        FileName = fileName ?? GetType().Namespace!.Split('.', StringSplitOptions.RemoveEmptyEntries)[^1];
    }
    public virtual bool Write(string? value = null, TSPlayer? player = null, bool corver = false, Formatting formatting = Formatting.Indented)
    {
        Normal = true;
        try
        {
            if(ConfigExists)
            {
                if (!corver)
                    return true;
            }
            if (value is null)
            {
                if (Root is null)
                {
                    Type type = GetType();
                    var defaultStream = type.Assembly.GetManifestResourceStream(type.Namespace + "." + FileName) ?? throw new Exception($"{type.Namespace}.{FileName} resource no find");
                    using var filestram = File.Create(ConfigPath);
                    defaultStream.CopyTo(filestram);
                    defaultStream.Dispose();
                }
                else
                {
                    File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Root, Formatting.Indented));
                }
            }
            else
            {
                File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(value, formatting));
            }
        }
        catch (Exception e)
        {
            ErrorString = e.ToString();
            LogAndOut(ErrorString, player);
            Normal = false;
            return false;
        }
        return true;
    }
    public virtual void Write(T root, TSPlayer? player, Formatting formatting = Formatting.Indented) => Write(JsonConvert.SerializeObject(root, formatting), player);
    public virtual bool Read(bool write = false)
    {
        bool result = true;
        if (write)
            result = Write();
        if (!ConfigExists || !result)
            return false;
        try
        {
            Root = JsonConvert.DeserializeObject<T>(File.ReadAllText(ConfigPath))!;
        }
        catch(Exception e)
        {
            ErrorString = e.ToString();
            LogAndOut(ErrorString);
            return false;
        }
        return true;
    }
    public virtual void LogAndOut(string message, TSPlayer? player = null, bool log = false, TraceLevel level = TraceLevel.Info)
    {
        if(player == null)
        {
            switch (level) 
            {
                case TraceLevel.Error:
                    Utils.WriteColorLine(message, ConsoleColor.Red);
                    break;
                case TraceLevel.Warning:
                    Utils.WriteColorLine(message, ConsoleColor.DarkYellow);
                    break;
                case TraceLevel.Info:
                    Utils.WriteInfoLine(message);
                    break;
                default:
                    Console.WriteLine(message);
                    break;
            }

        }
        else
        {
            switch (level)
            {
                case TraceLevel.Error:
                    player.SendErrorMessage(message);
                    break;
                case TraceLevel.Warning:
                    player.SendWarningMessage(message);
                    break;
                case TraceLevel.Info:
                    player.SendInfoMessage(message);
                    break;
                default:
                    player.SendMessage(message, Microsoft.Xna.Framework.Color.White);
                    break;
            }
        }
        if (log)
            TShock.Log.Write(message, level);
    }
}
