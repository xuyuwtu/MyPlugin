using Microsoft.Win32.SafeHandles;
using System.IO.MemoryMappedFiles;

namespace SingleFileExtractor.Core.Helpers;

internal static class BinaryKmpSearch
{
    public unsafe static int SearchInFile(MemoryMappedViewAccessor accessor, byte[] searchPattern)
    {
        SafeMemoryMappedViewHandle safeMemoryMappedViewHandle = accessor.SafeMemoryMappedViewHandle;
        return KmpSearch(searchPattern, (byte*)(void*)safeMemoryMappedViewHandle.DangerousGetHandle(), (int)safeMemoryMappedViewHandle.ByteLength);
    }

    private static int[] ComputeKmpFailureFunction(byte[] pattern)
    {
        int[] array = new int[pattern.Length];
        if (pattern.Length >= 1)
        {
            array[0] = -1;
        }

        if (pattern.Length >= 2)
        {
            array[1] = 0;
        }

        int num = 2;
        int num2 = 0;
        while (num < pattern.Length)
        {
            if (pattern[num - 1] == pattern[num2])
            {
                array[num] = num2 + 1;
                num2++;
                num++;
            }
            else if (num2 > 0)
            {
                num2 = array[num2];
            }
            else
            {
                array[num] = 0;
                num++;
            }
        }

        return array;
    }

    private unsafe static int KmpSearch(byte[] pattern, byte* bytes, long bytesLength)
    {
        int num = 0;
        int num2 = 0;
        int[] array = ComputeKmpFailureFunction(pattern);
        while (num + num2 < bytesLength)
        {
            if (pattern[num2] == bytes[num + num2])
            {
                if (num2 == pattern.Length - 1)
                {
                    return num;
                }

                num2++;
            }
            else if (array[num2] > -1)
            {
                num = num + num2 - array[num2];
                num2 = array[num2];
            }
            else
            {
                num++;
                num2 = 0;
            }
        }

        return -1;
    }
}