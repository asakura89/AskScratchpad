param([System.String]$mode, [System.String]$profile)

Clear-Host

function ListAction($profile) {
    Write-Host "nantokanantoka"
    Write-Host $profile
}

function DeleteAction($profile) {
    Write-Host "nan"
    Write-Host $profile
}

$modeActions = @{
    "l" = "ListAction"
    "d" = "DeleteAction"
}

If ([System.Linq.Enumerable]::Contains([System.String[]]$modeActions.Keys, $mode.ToLowerInvariant())) {
    & $modeActions[$mode.ToLowerInvariant()] $profile
}