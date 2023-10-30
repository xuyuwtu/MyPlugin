using System.Text;

using TShockAPI;

namespace VBY.Common;
public static class Utils
{
    public static void FormatReplace(ref string format, string replace)
    {
        FormatReplace(ref format, replace.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => $"{{{x}}}").ToArray());
    }
    public static void FormatReplace(ref string format, params string[] args)
    {
        StringBuilder sb = new(format);
        for (int i = 0; i < args.Length; i++)
        {
            sb.Replace(args[i], $"{{{i}}}");
        }
        format = sb.ToString();
    }
    public static void WriteColor(string value, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(value);
        Console.ResetColor();
    }
    public static void WriteColorLine(string value, ConsoleColor color = ConsoleColor.Red)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(value);
        Console.ResetColor();
    }
    public static void WriteColorLine((string value, ConsoleColor color)[] values)
    {
        for (int i = 0; i < values.Length - 1; i++)
        {
            WriteColor(values[i].value, values[i].color);
        }
        WriteColorLine(values[^1].value, values[^1].color);
    }
    public static void WriteInfoLine(string value) => WriteColorLine(value, ConsoleColor.Yellow);
    public static void WriteSuccessLine(string value) => WriteColorLine(value, ConsoleColor.Green);
    public static (bool success, TSPlayer? findPlayer) FindByNameOrID(string search, TSPlayer player)
    {
        List<TSPlayer> plys = TSPlayer.FindByNameOrID(search);
        switch (plys.Count)
        {
            case 0:
                player.SendInfoMessage("没有找到玩家:" + search);
                return (false, null);
            case 1:
                return (true, plys[0]);
            default:
                player.SendMultipleMatchError(plys.Select(x => x.Name));
                return (false, null);
        }
    }
}
