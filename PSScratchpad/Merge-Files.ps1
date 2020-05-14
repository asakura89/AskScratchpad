Clear-Host

$dir = "D:\Logs\"
$ext = ".log"

$outputFile = "$([System.IO.Path]::Combine($($dir), 'Combined.log'))"

$files = @()
Get-ChildItem -Path $dir -Include "*$($ext)" -Recurse |
    Select-Object -ExpandProperty VersionInfo  |
    Select-Object -ExpandProperty FileName |
    ForEach-Object { $files += $_ }

ForEach ($file In $files) {
    Get-Content $file |
        Out-File -Encoding "UTF8" -FilePath "$($outputFile)" -Append
}
