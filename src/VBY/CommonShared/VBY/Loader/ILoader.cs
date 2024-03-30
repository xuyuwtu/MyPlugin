namespace VBY.Common.Loader;

public interface ILoader
{
    bool OnPostInit { get; }
    bool Manual { get; }
    bool Loaded { get; }
    void Load();
    void Clear();
}
