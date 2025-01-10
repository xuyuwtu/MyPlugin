using System.Reflection;
using System.Runtime.CompilerServices;

namespace System;

internal readonly struct Range(Index start, Index end) : IEquatable<Range>
{
    public Index Start { get; } = start;

    public Index End { get; } = end;
    public static Range All => Index.Start..Index.End;

    public override bool Equals(object? value)
    {
        if (value is Range r && r.Start.Equals(Start))
        {
            return r.End.Equals(End);
        }
        return false;
    }

    public bool Equals(Range other)
    {
        if (other.Start.Equals(Start))
        {
            return other.End.Equals(End);
        }
        return false;
    }

    public override int GetHashCode() => Start.GetHashCode() * 31 + End.GetHashCode();

    public override string ToString() => Start.ToString() + ".." + End;

    public static Range StartAt(Index start) => start..Index.End;

    public static Range EndAt(Index end) => Index.Start..end;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int Offset, int Length) GetOffsetAndLength(int length)
    {
        int start = Start.GetOffset(length);
        int end = End.GetOffset(length);
        if ((uint)end > (uint)length || (uint)start > (uint)end)
        {
            throw new ArgumentOutOfRangeException("length");
        }
        return (start, end - start);
    }
}
