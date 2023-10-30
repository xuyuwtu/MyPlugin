using System.IO.MemoryMappedFiles;
using SingleFileExtractor.Core.Exceptions;
using SingleFileExtractor.Core.Helpers;

namespace SingleFileExtractor.Core;

public class ExecutableReader : IDisposable
{
    private StartupInfo? _startupInfo;

    private Bundle? _bundle;

    private bool _isSupported;

    internal MemoryMappedViewAccessor ViewAccessor { get; }

    public string FileName { get; }

    public bool IsSupported
    {
        get
        {
            ReadStartupInfo();
            return _isSupported;
        }
    }

    public StartupInfo StartupInfo => ReadStartupInfo() ?? throw new UnsupportedExecutableException("Is not a .NET Core 3.x, 5.0 or 6.0 executable.");

    public bool IsSingleFile => StartupInfo.IsSingleFile;

    public Bundle Bundle => _bundle ??= Bundle.FromExecutableReader(this);

    public ExecutableReader(string fileName)
    {
        if (!File.Exists(fileName))
        {
            throw new FileNotFoundException("Path to single file executable does not exist.", fileName);
        }

        FileName = fileName;
        ViewAccessor = MemoryMappedFileHelper.CreateViewAccessor(fileName);
    }

    public void ExtractToDirectory(string outputDirectory)
    {
        GuardAgainstNotSingleFile();
        List<string> list = new();
        try
        {
            foreach (FileEntry file in Bundle.Files)
            {
                string text = Path.Combine(outputDirectory, file.RelativePath);
                list.Add(text);
                file.ExtractToFile(text);
            }
        }
        catch
        {
            CleanupFiles(list);
            throw;
        }
    }

    public async Task ExtractToDirectoryAsync(string outputDirectory, CancellationToken cancellationToken = default)
    {
        GuardAgainstNotSingleFile();
        List<string> extractedFileNames = new();
        try
        {
            foreach (FileEntry file in Bundle.Files)
            {
                string text = Path.Combine(outputDirectory, file.RelativePath);
                extractedFileNames.Add(text);
                await file.ExtractToFileAsync(text, cancellationToken);
            }
        }
        catch
        {
            CleanupFiles(extractedFileNames);
            throw;
        }
    }

    private StartupInfo? ReadStartupInfo()
    {
        if (_startupInfo != null)
        {
            return _startupInfo;
        }

        if (BundleReader.TryReadStartupInfo(this, out var startupInfo))
        {
            _isSupported = true;
            return _startupInfo = startupInfo;
        }

        _isSupported = false;
        return null;
    }

    private void GuardAgainstNotSingleFile()
    {
        if (!IsSingleFile)
        {
            throw new InvalidOperationException("Only single file executables can be extracted.");
        }
    }

    private static void CleanupFiles(IEnumerable<string> fileNames)
    {
        foreach (string fileName in fileNames)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    File.Delete(fileName);
                }
                catch
                {
                }
            }
        }
    }

    public void Dispose() => ViewAccessor.Dispose();
}