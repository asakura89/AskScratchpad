Clear-Host

$nugetDir = [System.IO.Path]::Combine([System.Environment]::GetFolderPath("LocalApplicationData"), "NuGet")
$nugetExePath = [System.IO.Path]::Combine($nugetDir, "nuget.latest.exe")
$slnPaths = @(
    "E:\projects\project-a.sln",
    "E:\projects\project-b.sln"
)

function DownloadNuGet() {
    If ((Test-Path -Path $nugetDir) -Eq $False) {
        [System.IO.Directory]::CreateDirectory($nugetDir)
    }

    $url = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
    Start-Process Powershell -ArgumentList "-NoProfile -ExecutionPolicy Unrestricted -Command `"$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest '$($url)' -OutFile '$($nugetDir)'`""
}

function RestoreNuGet() {
    ForEach ($sln In $slnPaths) {
        Invoke-Expression "& `"$($nugetExePath)`" restore `"$($sln)`""
    }
}

If ((Test-Path -Path $nugetExePath) -Eq $False) {
    DownloadNuGet
}

RestoreNuGet