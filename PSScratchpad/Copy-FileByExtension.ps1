Clear-Host

$dir = "C:\Users\<username>\.nuget\packages"
$ext = ".nupkg"

$destDir = "D:\nuget"

$files = @()
Get-ChildItem -Path $dir -Include "*$($ext)" -Recurse |
    Select-Object -ExpandProperty VersionInfo  |
    Select-Object -ExpandProperty FileName |
    ForEach-Object { $files += $_ }

If ((Test-Path -Path $destDir) -Eq $False) {
    [System.IO.Directory]::CreateDirectory($destDir)
}

ForEach ($file In $files) {
    Copy-Item -Path $file -Destination $destDir
}

