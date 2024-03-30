namespace TerrariaPacket;

[AttributeUsage(AttributeTargets.Field)]
public sealed class ConditionAttribute : Attribute
{
    public string FieldName;
    public sbyte BitIndex;
    public ConditionAttribute(string field, sbyte bit = -1)
    {
        BitIndex = bit;
        FieldName = field;
    }
}