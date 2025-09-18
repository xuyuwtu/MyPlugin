namespace IDAnalyzer;

internal sealed class IdFilterInfo(string memberName, FrozenDictionary<int, string> idToNameDict, ImmutableDictionary<string, string?> properties, string idName)
{
    public string MemberName = memberName;
    public FrozenDictionary<int, string> IdToNameDict = idToNameDict;
    public ImmutableDictionary<string, string?> Properties = properties;
    public string IdName = idName;
}
