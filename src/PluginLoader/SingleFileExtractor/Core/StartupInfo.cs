using System.Runtime.CompilerServices;
using System.Text;

namespace SingleFileExtractor.Core;


public record StartupInfo(string? EntryPoint, long ManifestOffset)
{
    public bool IsSingleFile => ManifestOffset != 0;
}