Clear-Host

[System.Int32[]]$ports = @(80, 443)

$portInfo = Get-NetTCPConnection |
    Select `
        local*, `
        remote*, `
        state, `
        *process, `
        @{Name="ProcessName";Expression={(Get-Process -Id $_.OwningProcess).ProcessName}}, `
        @{Name="UserName";Expression={(Get-Process -Id $_.OwningProcess -IncludeUserName).UserName}} |
    Where { [System.Linq.Enumerable]::Contains($ports, $_.LocalPort) -Or [System.Linq.Enumerable]::Contains($ports, $_.RemotePort) }

$portInfo | Format-Table
$portInfo | Format-Table -AutoSize -Force
$portInfo | Out-GridView
