[System.String[]]$quitCommand = @("q", "e", "c")
$inp = Read-Host "Enter to continue or [$quitCommand] to quit"
$quit = [System.Linq.Enumerable]::Contains($quitCommand, $inp.ToLowerInvariant())
While(-Not $quit) {
    Write-Host "Hello, your input was [$inp]"

    $inp = Read-Host "Enter to continue or [$quitCommand] to quit"
    $quit = [System.Linq.Enumerable]::Contains($quitCommand, $inp.ToLowerInvariant())
}