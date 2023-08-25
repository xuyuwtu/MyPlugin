using System.Diagnostics.CodeAnalysis;
using System.Text;
using SingleFileExtractor.Core.Helpers;

namespace SingleFileExtractor.Core;

internal class BundleReader
{
    private static readonly int[] KnownEntryPointPathOffsets = new int[4] { 56, 48, 64, 200 };

    private static readonly byte[] BundleSignature = new byte[32]
    {
        139, 18, 2, 185, 106, 97, 32, 56, 114, 123,
        147, 2, 20, 215, 160, 50, 19, 245, 185, 230,
        239, 174, 51, 24, 238, 59, 45, 206, 36, 179,
        106, 174
    };

    public static bool TryReadStartupInfo(ExecutableReader executableReader, [NotNullWhen(true)] out StartupInfo? startupInfo)
    {
        int num = BinaryKmpSearch.SearchInFile(executableReader.ViewAccessor, BundleSignature);
        if (num == -1)
        {
            startupInfo = null;
            return false;
        }

        string entryPoint = ReadEntryPoint(executableReader.ViewAccessor, num + BundleSignature.Length)!;
        long manifestOffset = executableReader.ViewAccessor.ReadInt64(num - 8);
        startupInfo = new StartupInfo(entryPoint, manifestOffset);
        return true;
    }

    private static string? ReadEntryPoint(UnmanagedMemoryAccessor memoryAccessor, long startOffset)
    {
        Span<byte> span = stackalloc byte[1024];
        int[] knownEntryPointPathOffsets = KnownEntryPointPathOffsets;
        foreach (int num in knownEntryPointPathOffsets)
        {
            byte b = memoryAccessor.ReadByte(startOffset + num - 1);
            byte b2 = memoryAccessor.ReadByte(startOffset + num);
            if (b2 < 32 || b2 > 126 || b != 0)
            {
                continue;
            }

            startOffset += num;
            int num2 = 0;
            while (true)
            {
                byte b3 = memoryAccessor.ReadByte(startOffset + num2);
                if (b3 == 0)
                {
                    break;
                }

                span[num2] = b3;
                num2++;
            }

            Encoding aSCII = Encoding.ASCII;
            Span<byte> span2 = span;
            return aSCII.GetString(span2[..num2]);
        }

        return null;
    }
}