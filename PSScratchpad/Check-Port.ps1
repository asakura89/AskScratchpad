Clear-Host

[System.Int32[]]$ports = @(
    80,
    443
)

<#:<
$portInfo = Get-NetTCPConnection |
    Select `
        local*, `
        remote*, `
        state, `
        *process, `
        @{Name="ProcessName";Expression={(Get-Process -Id $_.OwningProcess).ProcessName}}, `
        @{Name="UserName";Expression={(Get-Process -Id $_.OwningProcess -IncludeUserName).UserName}} |
    Where { [System.Linq.Enumerable]::Contains(([System.Int32[]]$ports), [System.Int32]$_.LocalPort) -Or [System.Linq.Enumerable]::Contains(([System.Int32[]]$ports), [System.Int32]$_.RemotePort) }
>:#>

$portInfo = Get-NetTCPConnection |
    Select-Object `
        LocalAddress, `
        @{Name="LocalPort";Expression={$_.LocalPort.ToString()}}, `
        RemoteAddress, `
        @{Name="RemotePort";Expression={$_.RemotePort.ToString()}}, `
        State, `
        @{Name="OwningProcess";Expression={$_.OwningProcess.ToString()}}, `
        @{Name="ProcessName";Expression={(Get-Process -Id $_.OwningProcess).ProcessName}}, `
        @{Name="UserName";Expression={(Get-Process -Id $_.OwningProcess -IncludeUserName).UserName}} |
    Where-Object {
        [System.Linq.Enumerable]::Contains(([System.Int32[]]$ports), [System.Int32]$_.LocalPort) -Or `
        [System.Linq.Enumerable]::Contains(([System.Int32[]]$ports), [System.Int32]$_.RemotePort)
    } |
    Sort-Object -Property `
        @{Expression={[System.Int32]$_.LocalPort};Descending=$false}, `
        @{Expression={[System.Int32]$_.RemotePort};Descending=$false}, `
        @{Expression="State";Descending=$false}

If ($portInfo -Eq $null) {
    Write-Host "None"
}

$portInfo | Out-GridView
