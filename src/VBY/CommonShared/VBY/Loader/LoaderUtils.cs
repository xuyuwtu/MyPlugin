using VBY.Common.Hook;

namespace VBY.Common.Loader;

public static class LoaderUtils
{
    public static ILoader GetLoader<T>(this IEnumerable<T> values, Action<T> initAction, Action<T> clearAction, Func<bool>? loadPrecident = null, bool onPostInit = false, bool manual = false) => new EnumerableLoader<T>(values, initAction, clearAction, loadPrecident, onPostInit, manual);
    public static void AddTo(this ILoader loader, CommonPlugin plugin) => plugin.Loaders.Add(loader);
    public static void AddTo(this TShockAPI.Command loader, CommonPlugin plugin) => plugin.AddCommands.Add(loader);
    public static void AddTo(this HookBase loader, CommonPlugin plugin) => plugin.AttachHooks.Add(loader);
}
