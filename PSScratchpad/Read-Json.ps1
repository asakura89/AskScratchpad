Clear-Host

$json = Get-Content "$(Split-Path -parent $PSCommandPath)\pull-code.json" | ConvertFrom-Json
$json.cards.header.title
