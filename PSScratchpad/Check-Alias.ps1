Clear-Host

@(
    "gci",
    "ni",
    "echo"
) |
Select-Object @{L="Alias"; E={ $(Get-Alias $_).DisplayName }}
