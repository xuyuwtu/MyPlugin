using System.Text;

namespace SingleFileExtractor.Core;

public record Bundle(int MajorVersion, int MinorVersion, string Hash, IReadOnlyList<FileEntry> Files, ulong? Flags)
{

    internal static Bundle FromExecutableReader(ExecutableReader executableReader)
    {
        if (executableReader.StartupInfo.ManifestOffset == 0L)
        {
            throw new InvalidOperationException("Only single file executables can be extracted.");
        }

        using BinaryReader binaryReader = new(new UnmanagedMemoryStream(executableReader.ViewAccessor.SafeMemoryMappedViewHandle, executableReader.StartupInfo.ManifestOffset, executableReader.ViewAccessor.Capacity - executableReader.StartupInfo.ManifestOffset), Encoding.ASCII);
        uint num = binaryReader.ReadUInt32();
        uint minorVersion = binaryReader.ReadUInt32();
        int num2 = binaryReader.ReadInt32();
        string hash = binaryReader.ReadString();
        ulong? flags = null;
        if (num >= 2)
        {
            binaryReader.ReadInt64();
            binaryReader.ReadInt64();
            binaryReader.ReadInt64();
            binaryReader.ReadInt64();
            flags = binaryReader.ReadUInt64();
        }

        FileEntry[] array = new FileEntry[num2];
        for (int i = 0; i < num2; i++)
        {
            array[i] = FileEntry.FromBinaryReader(executableReader, binaryReader, num);
        }

        return new Bundle((int)num, (int)minorVersion, hash, array, flags);
    }
}
