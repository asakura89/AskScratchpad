
Clear-Host

$root = "D:\Workspace\sc91\"

@(
    "zk_1\bin\zkServer.cmd"
    "zk_2\bin\zkServer.cmd"
    "zk_3\bin\zkServer.cmd"
) |
    Select-Object @{ Name = "Command"; Expression = { $($root) + $_ } } |
    ForEach-Object {
        Start-Process cmd -ArgumentList "/c `"$($_.Command)`""
    }

Start-Sleep -Seconds 10

@(
    "solr_1\bin\solr.cmd"
    "solr_2\bin\solr.cmd"
) |
    Select-Object @{ Name = "Command"; Expression = { "`"$($root + $_)`" restart -cloud -f" } } |
    ForEach-Object {
        Start-Process cmd -ArgumentList "/c `"$($_.Command)`""
    }

Start-Sleep -Seconds 30

@(
    "xconnect_collectionsearch\App_Data\jobs\continuous\IndexWorker\XConnectSearchIndexer.exe"
    "cortex_processing\App_Data\jobs\continuous\ProcessingEngine\Sitecore.ProcessingEngine.exe"
    "xconnect_marketingautomation\App_Data\jobs\continuous\AutomationEngine\maengine.exe"
) |
    Select-Object @{ Name = "Command"; Expression = { $($root) + $_ } } |
    ForEach-Object {
        Start-Process Powershell -ArgumentList "-NoExit -Command `"$($_.Command)`""
    }

Start-Sleep -Seconds 30

@(
    "xconnect_collectionsearch\App_Data\jobs\continuous\IndexWorker\XConnectSearchIndexer.exe -rr"
) |
    Select-Object @{ Name = "Command"; Expression = { $($root) + $_ } } |
    ForEach-Object {
        Start-Process Powershell -ArgumentList "-NoExit -Command `"$($_.Command)`""
    }
