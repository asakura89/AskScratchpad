Clear-Host

$expectedDates = @("13", "14")

$dates = Get-ChildItem "E:\\temp\\Sitecore_Logs\\" |
    Select-Object @{L="Stripped"; E={ [System.Text.RegularExpressions.Regex]::Match($_, "(?!.+\.log\.)(?<date>\d{8})(?!\d{6})?") }} |
    Select-Object @{L="Date"; E={ $_.Stripped.Groups["date"].Value.Substring(6) }} |
    Select-Object -ExpandProperty Date -Unique

$containsAllExpectedDates = $expectedDates |
    ForEach-Object `
        -Begin { $start = $True } `
        -Process { $start = $start -And $dates -Contains $_ } `
        -End { $start }

Write-Host $containsAllExpectedDates
