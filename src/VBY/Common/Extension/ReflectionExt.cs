namespace System.Reflection;

public static class ReflectionExt
{
    public static MethodBase? ResolveMethod(this Module module, byte[] bytes, int startIndex) => module.ResolveMethod(BitConverter.ToInt32(bytes, startIndex));
    public static FieldInfo? ResolveField(this Module module, byte[] bytes, int startIndex) => module.ResolveField(BitConverter.ToInt32(bytes, startIndex));
}
