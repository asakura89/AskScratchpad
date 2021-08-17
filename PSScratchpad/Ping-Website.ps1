Clear-Host

function Log($message) {
    $logmsg = "[$([System.DateTime]::Now.ToString("yyyy.MM.dd.HH:mm:ss"))] $($message)"
    Write-Host $logmsg
}

function Ping([System.String]$url) {
    Log "Clearing `$Error variable."
    $Error.Clear()

    Log "Ping-ing $($url)."
    $req = [System.Net.WebRequest]::Create($url)
    $res = $req.GetResponse()
    $status = [int]$res.StatusCode
    $res.Dispose()

    Log "Done."

    Return $status
}

Log ":: Start Script ::"
$result = Ping "https://google.com"
Log $result
Log ":: Finish Script ::"
