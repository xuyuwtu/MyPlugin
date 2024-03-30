namespace System.Reflection;

public static class ReflectionExt
{
    public static MethodBase? ResolveMethod(this Module module, byte[] bytes, int startIndex) => BitConverter.IsLittleEndian
            ? module.ResolveMethod(BitConverter.ToInt32(bytes, startIndex))
            : module.ResolveMethod(Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan().Slice(startIndex)));
    public static FieldInfo? ResolveField(this Module module, byte[] bytes, int startIndex) => BitConverter.IsLittleEndian
            ? module.ResolveField(BitConverter.ToInt32(bytes, startIndex))
            : module.ResolveField(Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan().Slice(startIndex)));
    public static string ResolveString(this Module module, byte[] bytes, int startIndex) => BitConverter.IsLittleEndian
            ? module.ResolveString(BitConverter.ToInt32(bytes, startIndex))
            : module.ResolveString(Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan().Slice(startIndex)));
}
