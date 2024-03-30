namespace System.Reflection;

public static class ReflectionUtils
{
    public static Delegate GetDelegate(MethodInfo methodInfo, object? target)
    {
        var parameters = methodInfo.GetParameters().Select(x => x.ParameterType).ToList();
        Type delegateType;
        if (methodInfo.ReturnType == typeof(void))
        {
            if (parameters.Count > 0)
            {
                delegateType = Type.GetType($"System.Action`{parameters.Count}")!.MakeGenericType(parameters.ToArray());
            }
            else
            {
                delegateType = typeof(Action);
            }
        }
        else
        {
            parameters.Add(methodInfo.ReturnType);
            delegateType = Type.GetType($"System.Func`{parameters.Count}")!.MakeGenericType(parameters.ToArray());
        }
        return Delegate.CreateDelegate(delegateType, methodInfo.IsStatic ? null : target, methodInfo);
    }
}
