Clear-Host

$range = 1..10

[System.Collections.Generic.List[System.Int32]]$firstSeeds = $range |
    Select-Object @{L="50001"; E={((New-Guid).GetHashCode() % 50001)}} |
    Select-Object -ExpandProperty "50001"

[System.Collections.Generic.List[System.Int32]]$secondSeeds = $range |
    Select-Object @{L="46692"; E={((New-Guid).GetHashCode() % 46692)}} |
    Select-Object -ExpandProperty "46692"

$firstSeeds |
    Sort-Object { $_ }

Write-Host ""
Write-Host "---"
Write-Host ""

$secondSeeds |
    Sort-Object { $_ }

Write-Host ""
Write-Host "---"
Write-Host ""

[System.Collections.Generic.List[System.Int32]]$firstRands = @()
ForEach ($seed In $firstSeeds) {
    $rand = New-Object System.Random($seed)
    $firstRands += $rand.Next(0, 10)
}

$firstRands

Write-Host ""
Write-Host "---"
Write-Host ""

[System.Collections.Generic.List[System.Int32]]$secondRands = @()
ForEach ($seed In $secondSeeds) {
    $rand = New-Object System.Random($seed)
    $secondRands += $rand.Next(0, 10)
}

$secondRands
