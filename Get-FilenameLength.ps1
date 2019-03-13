Clear-Host
$searcheddir = "C:\Windows\Web\Wallpaper"
Get-ChildItem -Path $searcheddir -Recurse |
    Select-Object @{L="Type"; E={
        If ((Get-Item $_) -Is [System.IO.DirectoryInfo]) { "D" }
        ElseIf ((Get-Item $_) -Is [System.IO.FileInfo]) { "F" } }
    }, @{L="Name"; E={ $_.Name }}, @{L="NameLen"; E={ $_.Name.Length }} |
    Sort-Object @{E={ [System.Convert]::ToInt32($_.NameLen) }; Ascending = $false; Descending = $true}
