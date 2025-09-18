using System.Diagnostics;

namespace IDAnalyzer;

[AttributeUsage(AttributeTargets.Parameter)]
[Conditional("ADD_IDTYPEATTRIBUTE")]
internal sealed class IDTypeAttribute : Attribute
{
    public string Type {  get; }
    public IDTypeAttribute(string type)
    {
        Type = type;
    } 
}
