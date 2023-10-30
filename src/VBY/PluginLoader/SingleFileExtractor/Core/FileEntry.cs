using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;

namespace SingleFileExtractor.Core;

public record FileEntry(ExecutableReader ExecutableReader, long Offset, long Size, long CompressedSize, FileType Type, string RelativePath)
{
    public bool IsCompressed => CompressedSize > 0;

    internal static FileEntry FromBinaryReader(ExecutableReader executableReader, BinaryReader reader, uint majorVersion)
    {
        long offset = reader.ReadInt64();
        long size = reader.ReadInt64();
        long compressedSize = 0L;
        if (majorVersion >= 6)
        {
            compressedSize = reader.ReadInt64();
        }

        FileType type = (FileType)reader.ReadByte();
        string relativePath = reader.ReadString();
        return new FileEntry(executableReader, offset, size, compressedSize, type, relativePath);
    }

    public void ExtractToFile(string targetFileName)
    {
        using Stream destination = OpenDestinationStream(targetFileName);
        using Stream stream = AsStream();
        stream.CopyTo(destination);
    }

    public async Task ExtractToFileAsync(string targetFileName, CancellationToken cancellationToken = default)
    {
        await using Stream destination = OpenDestinationStream(targetFileName);
        await using Stream entryStream = await AsStreamAsync(cancellationToken);
        await entryStream.CopyToAsync(destination, cancellationToken);
    }

    public Stream AsStream()
    {
        if (!IsCompressed)
        {
            return new UnmanagedMemoryStream(ExecutableReader.ViewAccessor.SafeMemoryMappedViewHandle, Offset, Size);
        }

        using UnmanagedMemoryStream stream = new(ExecutableReader.ViewAccessor.SafeMemoryMappedViewHandle, Offset, CompressedSize);
        using DeflateStream deflateStream = new(stream, CompressionMode.Decompress);
        MemoryStream memoryStream = new((int)Size);
        deflateStream.CopyTo(memoryStream);
        if (memoryStream.Length != Size)
        {
            throw new InvalidDataException($"Single file entry {RelativePath} with compressed size {CompressedSize}, was decompressed to size {memoryStream.Length} but expected {Size}.");
        }

        memoryStream.Seek(0L, SeekOrigin.Begin);
        return memoryStream;
    }

    public async Task<Stream> AsStreamAsync(CancellationToken cancellationToken = default)
    {
        if (!IsCompressed)
        {
            return new UnmanagedMemoryStream(ExecutableReader.ViewAccessor.SafeMemoryMappedViewHandle, Offset, Size);
        }

        await using UnmanagedMemoryStream compressedStream = new(ExecutableReader.ViewAccessor.SafeMemoryMappedViewHandle, Offset, CompressedSize);
        await using DeflateStream deflateStream = new(compressedStream, CompressionMode.Decompress);
        MemoryStream decompressedStream = new((int)Size);
        await deflateStream.CopyToAsync(decompressedStream, cancellationToken);
        decompressedStream.Seek(0L, SeekOrigin.Begin);
        return decompressedStream;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Stream OpenDestinationStream(string fileName)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(fileName)!);
        return File.OpenWrite(fileName);
    }
}