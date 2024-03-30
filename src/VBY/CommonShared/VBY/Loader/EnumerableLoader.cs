using TShockAPI;

using VBY.Common.Loader;

namespace VBY.Common;

public class EnumerableLoader<T> : ILoader
{
    public bool OnPostInit { get; private set; }
    public bool Manual { get; private set; }
    public bool Loaded { get; private set; }
    public IEnumerable<T> Values { get; private set; }
    public Func<bool> LoadPrecident { get; private set; }
    public Action<T> LoadAction { get; private set; }
    public Action<T> ClearAction { get; private set; }
    public EnumerableLoader(IEnumerable<T> values, Action<T> loadAction, Action<T> clearAction, Func<bool>? loadPrecident = null, bool onPostInit = false, bool manual = false)
    {
        Values = values;
        LoadAction = loadAction;
        ClearAction = clearAction;
        LoadPrecident = loadPrecident ?? TrueFunc;
        OnPostInit = onPostInit;
        Manual = manual;
    }
    public void Load()
    {
        if (!Loaded && LoadPrecident())
        {
            Values.ForEach(LoadAction);
            Loaded = true;
        }
    }

    public void Clear()
    {
        //if (Loaded)
        //{
        Values.ForEach(ClearAction);
        Loaded = false;
        //}
    }
    private static bool TrueFunc() => true;
}
