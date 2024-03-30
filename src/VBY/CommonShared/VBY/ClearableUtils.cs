namespace VBY.Common;

public static class LoaderUtils
{
    public static ILoader GetLoader<T>(this IEnumerable<T> values, Action<T> initAction, Action<T> clearAction) => new EnumerableClearable<T>(values, initAction, clearAction);
}
