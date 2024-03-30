using GetText;

namespace TShockAPI;

public static class I18n
{
    private static Func<FormattableString, string> GetStringFunc = typeof(TShock).Assembly.GetType("TShockAPI.I18n")!.GetMethod(nameof(Catalog.GetString), new Type[] { typeof(FormattableString) })!.CreateDelegate<Func<FormattableString, string>>();
    private static Func<FormattableStringAdapter, object[], string> GetStringParamFunc = typeof(TShock).Assembly.GetType("TShockAPI.I18n")!.GetMethod(nameof(Catalog.GetString), new Type[] { typeof(FormattableStringAdapter), typeof(object[]) })!.CreateDelegate<Func<FormattableStringAdapter, object[], string>>();
    public static string GetString(FormattableString text) => GetStringFunc(text);
    public static string GetString(FormattableStringAdapter text, params object[] args) => GetStringParamFunc(text, args);
}
