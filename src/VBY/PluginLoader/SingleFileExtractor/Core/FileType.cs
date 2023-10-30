namespace SingleFileExtractor.Core
{
    public enum FileType : byte
    {
        Unknown,
        Assembly,
        NativeBinary,
        DepsJson,
        RuntimeConfigJson,
        Symbols
    }
}