Clear-Host

$scriptdir = $(Split-Path -parent $PSCommandPath)
$configPath = [System.IO.Path]::Combine($scriptdir, "notification.config.xml")

$hash256 = Get-FileHash $configPath -Algorithm SHA256
$hash512 = Get-FileHash $configPath -Algorithm SHA512

Write-Host $hash256.Hash.ToLowerInvariant()
Write-Host $($hash512.Hash.ToLowerInvariant())
