Clear-Host

#:< global config >:#
$scriptdir = $(Split-Path -parent $PSCommandPath)

#:< Log config >:#
$logname = "count-line"
$timestamp = $null

function GetTimeStamp() {
    return [System.DateTime]::Now.ToString("yyyyMMddHHmm")
}

function Log($message, $starting = $false, $writeToScreen = $true) {
    $timest = $Script:timestamp
    If ($timest -Eq $null) {
        $Script:timestamp = GetTimeStamp
        $timest = $Script:timestamp
    }

    $logdir = $Script:scriptdir
    If ([System.String]::IsNullOrEmpty($Script:scriptdir)) {
        $logdir = $(Split-Path -parent $PSCommandPath)
    }
    $logfile = "$($Script:scriptdir)\$($logname)_$($timest).log"
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

#:< --------------------------------------------- >:#

Log "``:: Start Script ::'" $true

$dir = "D:\\New folder"
$exts = @(".cs", ".eqx")

$files = @()
ForEach ($ext In $exts) {
    Get-ChildItem -Path $dir -Include "*$($ext)" -Recurse |
        Select-Object -ExpandProperty VersionInfo  |
        Select-Object -ExpandProperty FileName |
        ForEach-Object { $files += $_ }
}

$total = 0
ForEach ($file In $files) {
    $line = $(Get-Content $file |
        Measure-Object -Line |
        Select-Object -ExpandProperty Lines)
    $total = $total + $line
    Log "$($file) -- $($line)"
}

Log "Total -- $($total)"

Log ".:: Finish Script ::."
