## How to use it?
### Using in Visual Studio
copy to \<Visual Studio Install Path\>/MSBuild/Sdks/
example `C:/Program Files/Microsoft Visual Studio/2022/Preview/MSBuild/Sdks/My.Plugins.Sdk`
You may need to create an `MSBuild` folder youself.
path source: `https://github.com/dotnet/msbuild/blob/1205246b0fe49f0d00316c103cf0c381d96063d4/src/Shared/BuildEnvironmentHelper.cs#L668-L687`
### Using in dotnet build
```powershell / bash
# create a local nuget feeds
dotnet nuget add source --name <name> <path>

nuget pack my.plugins.sdk.nuspec
nuget add My.Plugins.Sdk.1.0.0.nupkg -Source <path>
```