Clear-Host
Get-Command |
    Select-Object @{L="SimpleName"; E={ $(If ($_.CommandType -Eq "Function"){ "Func" } ElseIf ($_.CommandType -Eq "Cmdlet") { "CmdL" } Else { "Othr" }) + ": " + $_.Name }} |
    Select-Object -ExpandProperty SimpleName |
    Sort-Object @{E={ $_ }} |
    Select-String "Item"