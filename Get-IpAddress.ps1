Clear-Host
Get-NetIpAddress |
    Where-Object { $_.InterfaceAlias.ToLowerInvariant() -Eq "ethernet" -And $_.AddressFamily.ToString().ToLowerInvariant() -Eq "ipv4" } |
    Select-Object -ExpandProperty IpAddress
