Clear-Host
$dumppath = ".\file-list.txt"

Get-ChildItem -Path . -Recurse |
    Select-Object @{L="Type"; E={
        If ((Get-Item $_) -Is [System.IO.DirectoryInfo]) { "D" }
        ElseIf ((Get-Item $_) -Is [System.IO.FileInfo]) { "F" } }
    }, @{L="Name"; E={ $_.FullName }} | 
    Where-Object { $_.Type -Eq "F" } |
    Select-Object -ExpandProperty Name |
    Out-File -Encoding "UTF8" -FilePath $dumppath
    