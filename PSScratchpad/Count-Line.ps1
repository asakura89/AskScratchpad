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
