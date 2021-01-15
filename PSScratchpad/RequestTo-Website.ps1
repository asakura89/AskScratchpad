Clear-Host

function Log($message) {
    $logged = "[$([System.DateTime]::Now.ToString("yyyy.MM.dd.HH:mm:ss"))] $($message)"
    Write-Host $logged
}

function Request([System.String]$url, [System.String]$httpMethod) {
    $status = 0
    $statusDesc = [System.String]::Empty
    Try {
        $req = [System.Net.WebRequest]::Create($url) -As [System.Net.HttpWebRequest]
        $req.Method = $httpMethod
        If ($url.Contains("https")) {
            # .Net issue. Need to force it to TLS1.2.
            [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12
        }

        [System.Net.HttpWebRequest]::DefaultCachePolicy = New-Object System.Net.Cache.HttpRequestCachePolicy([System.Net.Cache.HttpRequestCacheLevel]::NoCacheNoStore)
        $res = [System.Net.HttpWebResponse]$req.GetResponse()
        $status = [System.Int32]$res.StatusCode
        $statusDesc = $res.StatusDescription
        $res.Dispose()
        If ($res -Ne $null) {
            $es.Close()
        }
    }
    Catch [System.Net.WebException] {
        $res = [System.Net.HttpWebResponse]$_.Exception.Response
        $status = [System.Int32]$res.StatusCode
        $statusDesc = $res.StatusDescription
        Log $_.Exception.Message
    }
    Catch {
        Throw $_.Exception
    }

    Return "Status Code: $($status), Status: $($statusDesc)"
}

function Run() {
    $httpMethods = @(
        "GET",
        "HEAD",
        "POST",
        "PUT",
        "DELETE",
        "TRACE",
        "CONNECT",
        "OPTIONS",
        "MERGE",
        "PATCH"
    )

    $urls = @(
        "https://localsite.net"
        "https://google.com"
        "https://bing.com"
    )

    ForEach ($url In $urls) {
        ForEach ($httpMethod In $httpMethods) {
            Log "$($httpMethod) - $($url)"
            Try {
                Log "$(Request $url $httpMethod)"
            }
            Catch {
                Log "$(GetExceptionMessage $_.Exception)"
            }
        }
    }
}

Log ":: Start Script ::"
Run
Log ":: Finish Script ::"
