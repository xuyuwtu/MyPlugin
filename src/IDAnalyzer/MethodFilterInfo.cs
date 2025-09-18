namespace IDAnalyzer;

public sealed class MethodFilterInfo
{
    public string MethodName;
    public int ArgumentCount;
    public int CheckIndex;
    public FrozenDictionary<int, string> IdToNameDict;
    public ImmutableDictionary<string, string?> Properties;
    public string IdName;

    public MethodFilterInfo(string methodName, int checkIndex, int argumentCount, FrozenDictionary<int, string> idToNameDict, ImmutableDictionary<string, string?> properties, string idName)
    {
        MethodName = methodName;
        if (argumentCount < 0)
        {
            argumentCount = -1;
        }
        if (argumentCount != -1 && checkIndex >= argumentCount)
        {
            throw new ArgumentException("checkIndex >= argumentCount");
        }
        ArgumentCount = argumentCount;
        CheckIndex = checkIndex;
        IdToNameDict = idToNameDict;
        Properties = properties;
        IdName = idName;
    }
}
