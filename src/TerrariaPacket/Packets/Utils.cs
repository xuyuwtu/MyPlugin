using System.Runtime.CompilerServices;

namespace Terraria.Net.Packets;

internal static class Utils
{
    public unsafe static void Write(this MemoryStream ms, Color color)
    {
        ms.Write(new ReadOnlySpan<byte>(Unsafe.AsPointer(ref color.packedValue), 3));
    }
    public unsafe static void Write(this MemoryStream ms, RGBColor color)
    {
        ms.Write(new ReadOnlySpan<byte>(Unsafe.AsPointer(ref color), 3));
    }
}
