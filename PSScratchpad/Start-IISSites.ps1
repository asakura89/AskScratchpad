Clear-Host

Function GetTimeStamp() {
    Return [System.DateTime]::Now.ToString("yyyyMMddHHmm")
}

Function Log($message) {
    $timest = $script:timestamp
    If ($timest -Eq $null) {
        $script:timestamp = GetTimeStamp
        $timest = $script:timestamp
    }

    $logged = "[$([System.DateTime]::Now.ToString("yyyy.MM.dd.HH:mm:ss"))] $($message)"
    Write-Host $logged
}

[System.String[]]$quitCommand = @("q", "e", "c")
Do {
    Write-Host ""
    Write-Host ".:: Start / Stop IIS Website ::."

    $default = "example-"
    If (!($prefix = Read-Host "Prefix (for multiple sites) [$($default)]")) {
        $prefix = $default
    }

    $default = "Start"
    If (!($action = Read-Host "Action (Start/Stop) [$($default)]")) {
        $action = $default
    }

    Get-IISSite |
        Where-Object { $_.Name.ToLowerInvariant().StartsWith($prefix) } |
        ForEach-Object {
            $name = $_.Name
            $status = $_.State.ToString().ToLowerInvariant()
            $logPrfx = "[$($action)] Site: $($name)"

            Log "$($logPrfx)."

            $appPoolName = $_.Applications[0].ApplicationPoolName
            $appPool = $(Get-IISAppPool -Name $appPoolName)
            $appPoolStatus = $appPool.State.ToString().ToLowerInvariant()

            If ($appPoolStatus.Contains($action.ToLowerInvariant())) {
                Log "$($logPrfx), AppPool: $($appPoolName) is already $($appPoolStatus)."
            }
            Else {
                Log "$($logPrfx), AppPool: $($appPoolName)."
                If ($action.ToLowerInvariant() -Eq "start") {
                    $appPool.Start()
                }
                Else {
                    $appPool.Stop()
                }
                Log "$($logPrfx), AppPool: $($appPoolName). Done."
            }

            If ($status.Contains($action.ToLowerInvariant())) {
                Log "$($logPrfx) is already $($status)."
            }
            Else {
                Log "$($logPrfx)."
                If ($action.ToLowerInvariant() -Eq "start") {
                    Start-IISSite -Name $name
                }
                Else {
                    Stop-IISSite -Name $name -Confirm:$False
                }
                Log "$($logPrfx). Done."
            }
        }

        $inp = Read-Host "Enter to continue, exit with Q, E, C"
}
While ([System.Linq.Enumerable]::Contains($quitCommand, $inp.ToLowerInvariant()) -Eq $False)

Write-Host "Quitting."
