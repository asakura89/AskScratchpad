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

function GetInfo($path) {
    Return Get-ChildItem -Path $path |
        Select-Object Subject, Issuer, FriendlyName, NotAfter, Thumbprint, EnhancedKeyUsageList |
        Format-List |
        Out-String
}

Log ":: Personal Store ::" $true
$log = GetInfo Cert:\LocalMachine\My
Log $log

Log ":: Intermediate Store ::"
$log = GetInfo Cert:\LocalMachine\CA
Log $log

Log ":: TrustedRoot Store ::"
$log = GetInfo Cert:\LocalMachine\Root
Log $log
