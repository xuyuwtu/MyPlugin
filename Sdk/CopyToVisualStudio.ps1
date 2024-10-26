$deventPath = (Get-ItemPropertyValue -Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\devenv.exe' -Name '(default)').Trim('"')
if($deventPath -ne $null) {
    $sdkFilesPath = [System.IO.Path]::Combine([System.IO.Directory]::GetParent($deventPath).Parent.Parent.FullName, "MSBuild", "Sdks", "My.Plugins.Sdk", "Sdk")
    if(![System.IO.Directory]::Exists($sdkFilesPath)) {
        [System.IO.Directory]::CreateDirectory($sdkFilesPath)
    }
    Copy-Item -Path .\My.Plugins.Sdk\Sdk\* -Destination $sdkFilesPath
}