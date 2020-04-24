Clear-Host

$dumppath = "D:\IIS-SiteName.json"
Get-IISSite |
    ConvertTo-Json |
    Out-File -Encoding "UTF8" -FilePath $dumppath
