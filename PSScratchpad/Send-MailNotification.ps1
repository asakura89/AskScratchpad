Clear-Host

Import-Module WebAdministration

function Log($message) {
    $logged = "[$([System.DateTime]::Now.ToString("yyyy.MM.dd.HH:mm:ss"))] $($message)"
    Write-Host $logged
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

function ExtractUrl([System.String]$binding) {
    $splittedBinding = $binding -Split ":"
    $url = $splittedBinding[0] + "://" + $splittedBinding[3] + ":" + $splittedBinding[2]

    Return $url
}

function GetIISSiteInfo() {
    $siteInfos =
        Get-ChildItem IIS:\Sites |
            Where-Object { $_.Name -Match ".+sgpc.+" } |
            Select-Object @{L="Urls";E={ `
                ($_.Bindings.Collection | `
                    Select-Object @{L="InnerItem";E={ (ExtractUrl "$($_.protocol):$($_.bindingInformation)") }} | `
                    Select-Object -ExpandProperty InnerItem
                ) `
            }},
            @{L="Name";E={ $_.Name.ToString() }},
            @{L="State";E={ $_.State.ToString() }}

    Return $siteInfos
}

#:< global config >:#
$scriptdir = $(Split-Path -parent $PSCommandPath)

#:< Script config >:#
$configPath = [System.IO.Path]::Combine($scriptdir, "govgpc-monsc.config")

Try {
    Log ":: Start Script ::"

    $config = ParseConfig $Script:configPath
    #$config = ParseConfig $($Script:configPath + "\\a")
}
Catch {
    $exMessage = "Error caught: `n" + $_.Exception.ItemName + " `n" + $_.Exception.Message + " `n" + $_.Exception.StackTrace
    Log ($exMessage)
}
Finally {
    Log ":: Finish Script ::"
}

Log ":: Start Script ::"
$result = Ping "https://google.com"
Log $result
Log ":: Finish Script ::"


GetIISSiteInfo |
    Format-Table