Clear-Host

$dir = "C:\Users\<Username>\.nuget\packages"
$ext = ".nupkg"

$destDir = "D:\nuget"

$files = @()
Get-ChildItem -Path $dir -Include "*$($ext)" -Recurse |
    Select-Object -ExpandProperty VersionInfo  |
    Select-Object -ExpandProperty FileName |
    ForEach-Object { $files += $_ }

ForEach ($file In $files) {
    Copy-Item -Path $file -Destination $destDir
}
