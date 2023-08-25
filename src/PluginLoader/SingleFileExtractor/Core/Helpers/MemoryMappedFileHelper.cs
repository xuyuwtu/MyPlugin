using System.IO.MemoryMappedFiles;

namespace SingleFileExtractor.Core.Helpers;
internal static class MemoryMappedFileHelper
{
    public static MemoryMappedViewAccessor CreateViewAccessor(string fileName)
    {
        using MemoryMappedFile memoryMappedFile = MemoryMappedFile.CreateFromFile(fileName, FileMode.Open, null, 0L, MemoryMappedFileAccess.Read);
        return memoryMappedFile.CreateViewAccessor(0L, 0L, MemoryMappedFileAccess.Read);
    }
}