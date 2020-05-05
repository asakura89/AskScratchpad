Clear-Host

#:< global config >:#
$scriptdir = $(Split-Path -parent $PSCommandPath)

#:< Log config >:#
$logname = "test-writelog"
$timestamp = $null

function GetTimeStamp() {
    return [System.DateTime]::Now.ToString("yyyyMMddHHmm")
}

function Log($message, $starting = $false, $writeToScreen = $true) {
    $timest = $script:timestamp
    If ($timest -Eq $null) {
        $script:timestamp = GetTimeStamp
        $timest = $script:timestamp
    }

    $logdir = $script:scriptdir
    If ([System.String]::IsNullOrEmpty($script:scriptdir)) {
        $logdir = $(Split-Path -parent $PSCommandPath)
    }
    $logfile = "$($script:scriptdir)\$($logname)_$($timest).log"
    $logged = "[$([System.DateTime]::Now.ToString("yyyy.MM.dd.HH:mm:ss"))] $($message)"
    If ($writeToScreen) {
        Write-Host $logged
    }

    If ($starting) {
        $logged | Out-File -Encoding "UTF8" -FilePath $logfile
    }
    Else {
        $logged | Add-Content -Encoding "UTF8" -Path $logfile
    }
}


Log "``:: Start Script ::'" $true

Log "Doing something ..."

Log "Doing another thing ..."

Log ".:: Finish Script ::."
