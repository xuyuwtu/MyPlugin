namespace IDAnalyzer;

[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class IDTypeAttribute : Attribute
{
    public string Type { get; }
    public IDTypeAttribute(string type)
    {
        Type = type;
    }
}
