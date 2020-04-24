Clear-Host

$dir = "D:\Logs\"
$ext = ".log"

$destDir = "D:\Logs\"

$files = @()
Get-ChildItem -Path $dir -Include "*$($ext)" -Recurse |
    Select-Object -ExpandProperty VersionInfo  |
    Select-Object -ExpandProperty FileName |
    ForEach-Object { $files += $_ }

ForEach ($file In $files) {
    Get-Content $file |
        Out-File "$($destDir)\Combined.log"
}
