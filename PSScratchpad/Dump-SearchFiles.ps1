Clear-Host
$searcheddir = "D:\Solr"
$dumppath = "D:\temp\log4j.txt"

Get-ChildItem -Path $searcheddir -Include "log4j-*.jar" -Recurse |
    Select-Object @{L="Type"; E={
        If ((Get-Item $_) -Is [System.IO.DirectoryInfo]) { "D" }
        ElseIf ((Get-Item $_) -Is [System.IO.FileInfo]) { "F" } }
    }, @{L="Name"; E={ $_.Name }}, @{L="Path"; E={ [System.IO.Path]::GetDirectoryName($_.FullName) }} | 
    Select-Object @{L="Desc"; E={ "{0}: {1,-50} : {2}" -F $_.Type,$_.Name,$_.Path }} |
    Select-Object -ExpandProperty Desc |
    Out-File -Encoding "UTF8" -FilePath $dumppath