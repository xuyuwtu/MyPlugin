using System.Runtime.CompilerServices;

namespace System
{
    internal static class StringExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OrdinalEquals(this string a, string? value) => a.Equals(value, StringComparison.Ordinal);
    }
}