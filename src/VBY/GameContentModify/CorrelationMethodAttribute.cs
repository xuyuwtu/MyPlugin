namespace VBY.GameContentModify;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class CorrelationMethodAttribute : Attribute
{
    public Type DeclaringType;
    public string MethodName;

    public CorrelationMethodAttribute(Type declaringType, string methodName)
    {
        DeclaringType = declaringType;
        MethodName = methodName;
    }
}
