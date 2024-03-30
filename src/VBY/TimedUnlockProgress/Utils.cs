using System.Text;

namespace VBY.TimedUnlockProgress;

public static class Utils
{
    public static string GetShortFormat(TimeSpan timeSpan)
    {
        if (timeSpan.Days > 0)
        {
            return @"dd\天hh\时mm\分ss\秒";
        }
        if (timeSpan.Hours > 0)
        {
            return @"hh\时mm\分ss\秒";
        }
        if (timeSpan.Minutes > 0)
        {
            return @"mm\分ss\秒";
        }
        return @"ss\秒";
    }
    public static string GetMinFormat(TimeSpan timeSpan)
    {
        var sb = new StringBuilder();
        if(timeSpan.Days > 0)
        {
            sb.Append(@"d\天");
        }
        if(timeSpan.Hours > 0)
        {
            sb.Append(@"h\时");
        }
        if(timeSpan.Minutes > 0)
        {
            sb.Append(@"n\分");
        }
        if (timeSpan.Seconds > 0)
        {
            sb.Append(@"s\秒");
        }
        return sb.ToString();
    }
}
