
Clear-Host

#:< global config >:#
$scriptdir = $(Split-Path -parent $PSCommandPath)

#:< Solr Cloud config >:#
$root = "D:\SolrAndZookeperRootDir"
$zkPrefix = "zk_"
$solrPrefix = "solr_"

#:< Log config >:#
$logname = "cleanup-solrandzookeeperdata"
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

Log ".:: Cleaning up Solr and Zookeeper Data Dir script ::." $true

Try {
    Log "Script start."
    Log "- Listing files ..."
    
    $zkDataDir = Get-ChildItem -Path $root |
        Select-Object -ExpandProperty "Name" |
        Select-String $zkPrefix |
        Select-Object @{E={"$($root)\$($_)\data\version-2"}; L="Path"} |
        Select-Object -ExpandProperty "Path"

    $solrDataDir = @(
        Get-ChildItem -Path $root |
            Select-Object -ExpandProperty "Name" |
            Select-String $solrPrefix |
            Select-Object @{E={"$($root)\$($_)\server\solr"}; L="Path"} |
            Select-Object @{E={
                Get-ChildItem -Path $_.Path |
                    Select-Object `
                        @{E={
                            If ((Get-Item $_.FullName) -Is [System.IO.DirectoryInfo]) { "D" }
                            ElseIf ((Get-Item $_.FullName) -Is [System.IO.FileInfo]) { "F" }
                        }; L="Type"}, `
                        @{E={ $_.FullName }; L="Path"} |
                    Where-Object { $_.Type -Eq "D" -And -Not($_.Path.ToLower().EndsWith("configsets")) } |
                    Select-Object -ExpandProperty "Path"
            }; L="Path"} |
            Select-Object -ExpandProperty "Path"
    )

    $solrLogsDir = Get-ChildItem -Path $root |
        Select-Object -ExpandProperty "Name" |
        Select-String $solrPrefix |
        Select-Object @{E={"$($root)\$($_)\server\logs"}; L="Path"} |
        Select-Object -ExpandProperty "Path"

    $needsToBeDeleted = $zkDataDir + $solrDataDir + $solrLogsDir

    Log $([System.String]::Join([System.Environment]::NewLine, $needsToBeDeleted))

    Log "- Deleting ..."
    ForEach ($path In $needsToBeDeleted) {
        if ((Get-Item $path) -Is [System.IO.DirectoryInfo]) {
            Remove-Item -Path $path -Recurse -Force
            Log "Directory deleted. $($path)"
        }
        else {
            Remove-Item -Path $path -Force
            Log "File deleted. $($path)"
        }
    }

    Log "Done."
}
Catch {
    Log "Error caught."
    Log "- Write to log ..."
    Log $("Error caught: `n" + $_.Exception.ItemName + " `n" + $_.Exception.Message)
    Log "Done."
}
Finally {
    Log "Script finish."
}
