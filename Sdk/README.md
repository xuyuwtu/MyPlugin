## How to use it?
### Using in Visual Studio
copy to \<Visual Studio Install Path\>/MSBuild/Sdks/
example `C:/Program Files/Microsoft Visual Studio/2022/Preview/MSBuild/Sdks/My.Plugins.Sdk`
You may need to create an `MSBuild` folder youself.
path source: `https://github.com/dotnet/msbuild/blob/1205246b0fe49f0d00316c103cf0c381d96063d4/src/Shared/BuildEnvironmentHelper.cs#L668-L687`
### Using in dotnet build
#### create a local nuget feeds
```powershell
$nugetPath = <custom path>
$nugetName = <custom name>

dotnet nuget add source --name $nugetName $nugetPath
# or
# nuget sources add -Name $nugetName -Source $nugetPath
```
#### pack
##### use nuget application
```powershell
nuget pack my.plugins.sdk.nuspec
nuget add My.Plugins.Sdk.1.0.0.nupkg -Source $nugetPath
```
##### use dotnet cli
```powershell
dotnet pack [My.Plugins.Sdk.csproj] --output ./ 
dotnet nuget push My.Plugins.Sdk.1.0.0.nupkg --source $nugetPath
```