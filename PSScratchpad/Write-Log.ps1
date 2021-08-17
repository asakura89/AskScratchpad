Clear-Host

function Log($message, $starting = $false, $writeToScreen = $true) {
    $scriptfile = (Get-Item $PSCommandPath)
    $logdir = $scriptfile.Directory
    $logname = "$($scriptfile.BaseName)_$([System.DateTime]::Now.ToString("yyyyMMddHHmm")).log"
    $logfile = [System.IO.Path]::Combine($logdir, $logname)

    $logmsg = "[$([System.DateTime]::Now.ToString("yyyy.MM.dd.HH:mm:ss"))] $($message)"
    If ($writeToScreen) {
        Write-Host $logmsg
    }

    If ($starting) {
        $logmsg | Out-File -Encoding "UTF8" -FilePath $logfile
    }
    Else {
        $logmsg | Add-Content -Encoding "UTF8" -Path $logfile
    }
}


Log "``:: Start Script ::'" $true

Log "Doing something ..."

Log "Doing another thing ..."

Log ".:: Finish Script ::."
