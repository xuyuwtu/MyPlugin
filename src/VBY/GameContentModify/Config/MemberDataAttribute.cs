using System.Diagnostics;

namespace VBY.GameContentModify.Config;
[Conditional("ADD_MEMBERDATA")]
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class MemberDataAttribute : Attribute
{
    public string Description { get; set; }
    public object? Value { get; set; }
    public bool PrivateField { get; set; }
    public MemberDataAttribute(string description)
    {
        Description = description;
    }
    public MemberDataAttribute(string description, object? value) 
    {
        Description = description;
        Value = value;
    }
    public MemberDataAttribute(string description, bool value)
    {
        Description = description;
        Value = value;
    }
    public MemberDataAttribute(string description, int value)
    {
        Description = description;
        Value = value;
    }
    public MemberDataAttribute(string description, double value)
    {
        Description = description;
        Value = value;
    }
}
