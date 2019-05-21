Clear-Host

Get-IISAppPool |
    Select-Object @{E={"IIS APPPOOL\\$($_.Name)"};L="Name"} |
    Select-Object -ExpandProperty Name |
    Sort-Object
