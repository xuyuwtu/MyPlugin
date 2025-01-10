namespace VBY.ItemStrengthen;

internal static class Utils
{
    public static void Apply<T>(this T source, ref T target, ref byte flag, byte addFlag) where T : struct
    {
        target = source;
        flag += addFlag;
    }
    public static string GetApplyText(this ModifyInfo? info, int value)
    {
        if (info is null)
        {
            return value.ToString();
        }
        var origin = value;
        info.Apply(ref value);
        if (info.Type == ModifyTypes.Change)
        {
            return $"{value}[{origin}=>{value}]";
        }
        return $"{value}[{origin}{info}=>{value}]";
    }
    public static string GetApplyText(this ModifyInfo? info, float value)
    {
        if (info is null)
        {
            return value.ToString();
        }
        var origin = value;
        info.Apply(ref value);
        if (info.Type == ModifyTypes.Change)
        {
            return $"{value}[{origin}=>{value}]";
        }
        return $"{value}[{origin}{info}=>{value}]";
    }
    public static string GetApplyText<T>(this T? target, T origin) where T : struct
    {
        if (target.HasValue)
        {
            return $"{target.Value}[{origin}=>{target}]"; 
        }
        return origin.ToString()!;
    }
}
