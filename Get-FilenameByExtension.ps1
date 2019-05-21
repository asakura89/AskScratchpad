Clear-Host

$dir = "C:\Users\<Username>\.nuget\packages"
$ext = ".nupkg"

Get-ChildItem -Path $dir -Include "*$($ext)" -Recurse |
    Select-Object -ExpandProperty VersionInfo  |
    Select-Object -ExpandProperty FileName |
    Sort-Object
