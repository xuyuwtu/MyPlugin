namespace VBY.GameContentModify;

[AttributeUsage(AttributeTargets.Class)]
internal class ReplaceTypeAttribute : Attribute
{
    public Type Type;
    public ReplaceTypeAttribute(Type type)
    {
         Type = type;
    }
}
