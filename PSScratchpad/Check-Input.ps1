Clear-Host
[System.String[]]$quitCommand = @("q", "e", "c")
$inp = Read-Host "Please input one of these [$quitCommand]"
[System.Linq.Enumerable]::Contains($quitCommand, $inp.ToLowerInvariant())
