namespace System.Collections.Generic
{
    internal static class IListExtensions
    {
        public static int BinarySearch<T, TValue>(this IList<T> values, Func<T, TValue, int> comparison, TValue value)
        {
            int lo = 0;
            int hi = values.Count - 1;
            while (lo <= hi)
            {
                int i = lo + ((hi - lo) >> 1);
                int order = comparison(values[i], value);
                if (order == 0)
                {
                    return i;
                }
                if (order < 0)
                {
                    lo = i + 1;
                }
                else
                {
                    hi = i - 1;
                }
            }
            return ~lo;
        }
    }
}